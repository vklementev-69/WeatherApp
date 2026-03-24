using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public static class Mapper
    {

        public static WeatherViewModel MapToViewModel(WeatherApiResponse current, WeatherApiResponse forecast)
        {
            var viewModel = new WeatherViewModel
            {
                CityName = current.location.name,
                Latitude = current.location.lat,
                Longitude = current.location.lon,
                Current = MapCurrentWeather(current),
                Hourly = ExtractHourlyForecast(forecast),
                Daily = ExtractDailyForecast(forecast),
                IsLoading = false,
                HasError = false
            };

            return viewModel;
        }

        private static CurrentWeather MapCurrentWeather(WeatherApiResponse data)
        {
            return new CurrentWeather
            {
                Location = data.location.name,
                Country = data.location.country,
                Temperature = data.current.temp_c,
                FeelsLike = data.current.feelslike_c,
                Condition = data.current.condition.text,
                ConditionIcon = "https:" + data.current.condition.icon,
                Humidity = data.current.humidity,
                WindSpeed = data.current.wind_kph,
                WindDirection = data.current.wind_dir,
                Pressure = data.current.pressure_mb,
                Visibility = data.current.vis_km,
                LastUpdated = data.current.last_updated
            };
        }

        private static List<HourlyForecast> ExtractHourlyForecast(WeatherApiResponse forecast)
        {
            var hourlyList = new List<HourlyForecast>();
            var currentTime = DateTime.Now;
            var endOfTomorrow = currentTime.Date.AddDays(2).AddHours(-1);

            foreach (var day in forecast.forecast.forecastday)
            {
                foreach (var hour in day.hour)
                {
                    var hourTime = DateTime.Parse(hour.time);

                    // Берем только часы от текущего момента до конца следующего дня
                    if (hourTime >= currentTime && hourTime <= endOfTomorrow)
                    {
                        hourlyList.Add(new HourlyForecast
                        {
                            Time = hourTime,
                            Temperature = hour.temp_c,
                            Condition = hour.condition.text,
                            ConditionIcon = "https:" + hour.condition.icon,
                            ChanceOfRain = hour.chance_of_rain,
                            WindSpeed = hour.wind_kph,
                            IsDay = hour.is_day == 1
                        });
                    }
                }
            }

            return hourlyList;
        }

        private static List<DailyForecast> ExtractDailyForecast(WeatherApiResponse forecast)
        {
            var dailyList = new List<DailyForecast>();

            foreach (var day in forecast.forecast.forecastday)
            {
                var date = DateTime.Parse(day.date);
                dailyList.Add(new DailyForecast
                {
                    Date = date,
                    DayOfWeek = GetRussianDayOfWeek(date.DayOfWeek),
                    MaxTemp = day.day.maxtemp_c,
                    MinTemp = day.day.mintemp_c,
                    AvgTemp = day.day.avgtemp_c,
                    Condition = day.day.condition.text,
                    ConditionIcon = "https:" + day.day.condition.icon,
                    ChanceOfRain = day.day.daily_chance_of_rain,
                    MaxWindSpeed = day.day.maxwind_kph,
                    TotalPrecipitation = day.day.totalprecip_mm,
                    AvgHumidity = day.day.avghumidity,
                    Sunrise = day.astro.sunrise,
                    Sunset = day.astro.sunset
                });
            }

            return dailyList;
        }

        private static string GetRussianDayOfWeek(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday: return "Понедельник";
                case DayOfWeek.Tuesday: return "Вторник";
                case DayOfWeek.Wednesday: return "Среда";
                case DayOfWeek.Thursday: return "Четверг";
                case DayOfWeek.Friday: return "Пятница";
                case DayOfWeek.Saturday: return "Суббота";
                case DayOfWeek.Sunday: return "Воскресенье";
                default: return dayOfWeek.ToString();
            }
        }
    }
}