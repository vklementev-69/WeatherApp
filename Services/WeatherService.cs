using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public interface IWeatherService
    {
        Task<WeatherViewModel> GetWeatherDataAsync(double lat, double lon, CancellationToken token);
    }

    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly Logger _logger;
        private const string API_KEY = "fa8b3df74d4042b9aa7135114252304";
        private const string BASE_URL = "http://api.weatherapi.com/v1";

        public WeatherService()
        {
            _httpClient = new HttpClient();
            _logger = LogManager.GetCurrentClassLogger();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<WeatherViewModel> GetWeatherDataAsync(double lat, double lon, CancellationToken token)
        {
            try
            {
                var location = $"{lat.ToString(System.Globalization.CultureInfo.InvariantCulture)},{lon.ToString(System.Globalization.CultureInfo.InvariantCulture)}";
                
                // Получаем текущую погоду
                var currentUrl = $"{BASE_URL}/current.json?key={API_KEY}&q={location}&lang=ru";
                var currentResponse = await _httpClient.GetAsync(currentUrl, token);
                var currentContent = await currentResponse.Content.ReadAsStringAsync();
                var currentData = JsonConvert.DeserializeObject<WeatherApiResponse>(currentContent);

                // Получаем прогноз на 3 дня
                var forecastUrl = $"{BASE_URL}/forecast.json?key={API_KEY}&q={location}&days=3&lang=ru";
                var forecastResponse = await _httpClient.GetAsync(forecastUrl, token);
                var forecastContent = await forecastResponse.Content.ReadAsStringAsync();
                var forecastData = JsonConvert.DeserializeObject<WeatherApiResponse>(forecastContent);

                return Mapper.MapToViewModel(currentData, forecastData);
            }
            catch (HttpRequestException ex)
            {
                _logger.Error(ex);
                return new WeatherViewModel
                {
                    HasError = true,
                    ErrorMessage = "Ошибка подключения к сервису погоды. Проверьте интернет-соединение."
                };
            }
            catch (TaskCanceledException ex)
            {
                _logger.Error(ex);
                return new WeatherViewModel
                {
                    HasError = true,
                    ErrorMessage = "Превышено время ожидания ответа от сервера."
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new WeatherViewModel
                {
                    HasError = true,
                    ErrorMessage = $"Произошла ошибка при получении данных: {ex.Message}"
                };
            }
        }
    }
}
