using System.Text;
using System.Web.Mvc;

namespace WeatherApp.Filters
{
    /// <summary>
    /// Action filter to ensure UTF-8 encoding for all responses
    /// </summary>
    public class EncodingFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Set request encoding to UTF-8
            filterContext.HttpContext.Request.ContentEncoding = Encoding.UTF8;
            
            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            
            // Set response encoding to UTF-8
            response.ContentEncoding = Encoding.UTF8;
            
            // Ensure charset is set in Content-Type header
            if (response.ContentType != null && !response.ContentType.Contains("charset"))
            {
                response.ContentType += "; charset=utf-8";
            }
            
            base.OnResultExecuting(filterContext);
        }
    }
}
