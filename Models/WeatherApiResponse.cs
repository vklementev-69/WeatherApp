namespace WeatherApp.Models
{
    // Модели для десериализации JSON от API
    public class WeatherApiResponse
    {
        public Location location { get; set; }
        public Current current { get; set; }
        public Forecast forecast { get; set; }
    }
}
