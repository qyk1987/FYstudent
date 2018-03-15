using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.Http.Cors;
using System.Net.Http.Headers;
using FYstudentMgr.Common;
using Newtonsoft.Json;
using JsonPatch.Formatting;

namespace FYstudentMgr
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            //config.Formatters.Remove(config.Formatters.XmlFormatter);
            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Insert(0, new JsonDateTimeConverter());
            //设置Datetime 到 時間戳 的互轉 
            //JsonSerializerSettings jSettings = new Newtonsoft.Json.JsonSerializerSettings()
            //{
            //    Formatting = Formatting.Indented,
            //    DateTimeZoneHandling = DateTimeZoneHandling.Local
            //};
            //jSettings.Converters.Add(new JsonDateTimeConverter());
            //config.Formatters.JsonFormatter.SerializerSettings = jSettings;
            // Web API routes
            config.MapHttpAttributeRoutes();
            //var cors = new EnableCorsAttribute("*", "*", "*");
            //config.EnableCors(cors);
            config.Formatters.Add(new JsonPatchFormatter());
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
