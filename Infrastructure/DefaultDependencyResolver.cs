using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WeatherApp.Infrastructure
{
    public class DefaultDependencyResolver : IDependencyResolver
    {
        private readonly IServiceProvider _provider;

        public DefaultDependencyResolver(IServiceProvider provider)
        {
            _provider = provider;
        }

        public object GetService(Type serviceType)
        {
            return _provider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _provider.GetServices(serviceType) ?? new List<object>();
        }
    }
}