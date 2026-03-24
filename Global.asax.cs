using NLog;
using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WeatherApp
{
    public class MvcApplication : HttpApplication
    {
        private Logger _logger;
        protected void Application_Start()
        {
            _logger = LogManager.GetCurrentClassLogger();
            _logger.Info("Application started.");
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            
            // Disable X-AspNetMvc-Version header
            MvcHandler.DisableMvcResponseHeader = true;
            
            // Set default encoding to UTF-8
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        protected void Application_BeginRequest()
        {
            // Set UTF-8 encoding for all responses
            Response.ContentEncoding = Encoding.UTF8;
            Request.ContentEncoding = Encoding.UTF8;
        }

        protected void Application_PreSendRequestHeaders()
        {
            // Remove server headers for security
            Response.Headers.Remove("X-Powered-By");
            Response.Headers.Remove("X-AspNet-Version");
            
            // Ensure UTF-8 charset is set
            if (Response.ContentType != null && !Response.ContentType.Contains("charset"))
            {
                Response.ContentType += "; charset=utf-8";
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Log errors here if needed
            var ex = Server.GetLastError();
            _logger.Error(ex);
        }
    }
}
