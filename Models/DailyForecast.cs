using System;

namespace WeatherApp.Models
{
    // Модель для дневного прогноза
    public class DailyForecast
    {
        public DateTime Date { get; set; }
        public string DayOfWeek { get; set; }
        public double MaxTemp { get; set; }
        public double MinTemp { get; set; }
        public double AvgTemp { get; set; }
        public string Condition { get; set; }
        public string ConditionIcon { get; set; }
        public int ChanceOfRain { get; set; }
        public double MaxWindSpeed { get; set; }
        public double TotalPrecipitation { get; set; }
        public double AvgHumidity { get; set; }
        public string Sunrise { get; set; }
        public string Sunset { get; set; }
    }
}
