using NLog;
using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherService _weatherService;

        // Координаты Москвы
        private readonly double _moscowLat;
        private readonly double _moscowLon;
        private readonly Logger _logger;
        private const int TIMEOUT = 10;

        public WeatherController()
        {
            _weatherService = new WeatherService();
            _logger = LogManager.GetCurrentClassLogger();
            if (!Double.TryParse(ConfigurationManager.AppSettings["MoscowLat"], out _moscowLat))
                throw new ArgumentException("Web.coinfig. MoscowLat is not valid.");
            if (!Double.TryParse(ConfigurationManager.AppSettings["MoscowLon"], out _moscowLon))
                throw new ArgumentException("Web.coinfig. MoscowLon is not valid.");
        }

        // GET: Weather
        public async Task<ActionResult> Index()
        {
            // Set UTF-8 encoding for the response
            Response.ContentEncoding = Encoding.UTF8;
            try
            {
                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(TIMEOUT)))
                {
                    var weatherData = await _weatherService.GetWeatherDataAsync(
                        _moscowLat,
                        _moscowLon,
                        cts.Token);

                    return View(weatherData);
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.Error(ex);
                return View(new WeatherViewModel
                {
                    HasError = true,
                    ErrorMessage = "Ошибка подключения к сервису погоды. Проверьте интернет-соединение."
                });
            }
            catch (TaskCanceledException ex)
            {
                _logger.Error(ex);
                return View(new WeatherViewModel
                {
                    HasError = true,
                    ErrorMessage = "Превышено время ожидания ответа от сервера."
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View(new WeatherViewModel
                {
                    HasError = true,
                    ErrorMessage = $"Произошла ошибка при получении данных: {ex.Message}"
                });
            }
        }

        // AJAX: Обновление данных
        [HttpGet]
        public async Task<JsonResult> RefreshWeather()
        {
            // Set UTF-8 encoding for JSON response
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/json; charset=utf-8";

            using (var cts = new CancellationTokenSource())
            {
                cts.CancelAfter(TimeSpan.FromSeconds(TIMEOUT)); // Timeout

                var weatherData = await _weatherService.GetWeatherDataAsync(_moscowLat, _moscowLon, cts.Token);
                return Json(weatherData, JsonRequestBehavior.AllowGet);
            }
        }
        }
}
