using System;
using System.Net.Http;

namespace WeatherApp.Services
{
    public class WeatherHttpClient
    {
        private static readonly HttpClient _instance = new HttpClient();

        static WeatherHttpClient()
        {
            _instance.Timeout = TimeSpan.FromSeconds(30);
        }

        public static HttpClient Instance => _instance;
    }
}