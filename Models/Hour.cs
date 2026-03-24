namespace WeatherApp.Models
{

    public class Hour
    {
        public string time { get; set; }
        public double temp_c { get; set; }
        public Condition condition { get; set; }
        public int chance_of_rain { get; set; }
        public double wind_kph { get; set; }
        public int is_day { get; set; }
    }
}
