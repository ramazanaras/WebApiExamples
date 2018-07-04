using Programming.API.Attributes;
using Programming.API.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Programming.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            //artık her controllerda bu attribüte eklenmiş olur.
            config.Filters.Add(new ApiExceptionAttribute());

            //geçerli bir kullanıcı bir istekte bulunursa bu classa düş
            config.MessageHandlers.Add(new APIKeyHandler()); //KENDİ HANDLERIMIZ


            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
