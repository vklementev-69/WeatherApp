namespace WeatherApp.Models
{
    public class Day
    {
        public double maxtemp_c { get; set; }
        public double mintemp_c { get; set; }
        public double avgtemp_c { get; set; }
        public Condition condition { get; set; }
        public double maxwind_kph { get; set; }
        public double totalprecip_mm { get; set; }
        public int avghumidity { get; set; }
        public int daily_chance_of_rain { get; set; }
    }
}
