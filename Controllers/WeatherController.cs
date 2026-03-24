using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using WeatherApp.Services;

namespace WeatherApp.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherService _weatherService;

        // Координаты Москвы
        private const double MOSCOW_LAT = 55.7558;
        private const double MOSCOW_LON = 37.6173;
        private const int TIMEOUT = 10;

        public WeatherController()
        {
            _weatherService = new WeatherService();
        }

        // GET: Weather
        public async Task<ActionResult> Index()
        {
            // Set UTF-8 encoding for the response
            Response.ContentEncoding = Encoding.UTF8;
            using (var cts = new CancellationTokenSource())
            {
                cts.CancelAfter(TimeSpan.FromSeconds(TIMEOUT)); // Timeout

                var weatherData = await _weatherService.GetWeatherDataAsync(MOSCOW_LAT, MOSCOW_LON, cts.Token);
                return View(weatherData);
            }
        }

        // AJAX: Обновление данных
        [HttpPost]
        public async Task<JsonResult> RefreshWeather()
        {
            // Set UTF-8 encoding for JSON response
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/json; charset=utf-8";

            using (var cts = new CancellationTokenSource())
            {
                cts.CancelAfter(TimeSpan.FromSeconds(TIMEOUT)); // Timeout

                var weatherData = await _weatherService.GetWeatherDataAsync(MOSCOW_LAT, MOSCOW_LON, cts.Token);
                return Json(weatherData, JsonRequestBehavior.AllowGet);
            }
        }
        }
}
