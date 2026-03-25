using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        private readonly string _apikey;
        private readonly string _baseUrl;

        public WeatherService()
        {
            _httpClient = WeatherHttpClient.Instance;
            _baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            _apikey = ConfigurationManager.AppSettings["WeatherApiKey"];
        }

        public async Task<WeatherViewModel> GetWeatherDataAsync(double lat, double lon, CancellationToken token)
        {
            var location = $"{lat.ToString(System.Globalization.CultureInfo.InvariantCulture)},{lon.ToString(System.Globalization.CultureInfo.InvariantCulture)}";

            WeatherApiResponse currentData = await GetCurrentWeather(location, token);

            WeatherApiResponse forecastData = await GetForecastWeather(location, token);

            return Mapper.MapToViewModel(currentData, forecastData);
        }

        private async Task<WeatherApiResponse> GetForecastWeather(string location, CancellationToken token)
        {
            // Получаем прогноз на 3 дня
            var forecastUrl = $"{_baseUrl}/forecast.json?key={_apikey}&q={location}&days=3&lang=ru";
            var forecastResponse = await _httpClient.GetAsync(forecastUrl, token);
            var forecastContent = await forecastResponse.Content.ReadAsStringAsync();
            var forecastData = JsonConvert.DeserializeObject<WeatherApiResponse>(forecastContent);
            return forecastData;
        }

        private async Task<WeatherApiResponse> GetCurrentWeather(string location, CancellationToken token)
        {
            // Получаем текущую погоду
            var currentUrl = $"{_baseUrl}/current.json?key={_apikey}&q={location}&lang=ru";
            var currentResponse = await _httpClient.GetAsync(currentUrl, token);
            var currentContent = await currentResponse.Content.ReadAsStringAsync();
            var currentData = JsonConvert.DeserializeObject<WeatherApiResponse>(currentContent);
            return currentData;
        }
    }
}
