using System;

namespace WeatherApp.Models
{
    // Модель для почасового прогноза
    public class HourlyForecast
    {
        public DateTime Time { get; set; }
        public double Temperature { get; set; }
        public string Condition { get; set; }
        public string ConditionIcon { get; set; }
        public int ChanceOfRain { get; set; }
        public double WindSpeed { get; set; }
        public bool IsDay { get; set; }
    }
}
