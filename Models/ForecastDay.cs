using System.Collections.Generic;

namespace WeatherApp.Models
{
    public class ForecastDay
    {
        public string date { get; set; }
        public Day day { get; set; }
        public Astro astro { get; set; }
        public List<Hour> hour { get; set; }
    }
}
