using System.Collections.Generic;

namespace WeatherApp.Models
{
    // Полная модель погоды для представления
    public class WeatherViewModel
    {
        public CurrentWeather Current { get; set; }
        public List<HourlyForecast> Hourly { get; set; }
        public List<DailyForecast> Daily { get; set; }
        public bool IsLoading { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public string CityName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
