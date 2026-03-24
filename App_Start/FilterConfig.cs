using System.Web.Mvc;
using WeatherApp.Filters;

namespace WeatherApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new EncodingFilter()); // Add UTF-8 encoding filter
        }
    }
}
