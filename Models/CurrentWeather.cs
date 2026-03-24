namespace WeatherApp.Models
{
    // Модель для текущей погоды
    public class CurrentWeather
    {
        public string Location { get; set; }
        public string Country { get; set; }
        public double Temperature { get; set; }
        public double FeelsLike { get; set; }
        public string Condition { get; set; }
        public string ConditionIcon { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public string WindDirection { get; set; }
        public double Pressure { get; set; }
        public double Visibility { get; set; }
        public string LastUpdated { get; set; }
    }
}
