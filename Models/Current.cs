namespace WeatherApp.Models
{
    public class Current
    {
        public double temp_c { get; set; }
        public double feelslike_c { get; set; }
        public Condition condition { get; set; }
        public int humidity { get; set; }
        public double wind_kph { get; set; }
        public string wind_dir { get; set; }
        public double pressure_mb { get; set; }
        public double vis_km { get; set; }
        public string last_updated { get; set; }
    }
}
