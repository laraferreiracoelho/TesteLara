using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TesteDois
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Serviços e configuração da API da Web

            // Rotas da API da Web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
      name: "ActionBased",
      routeTemplate: "api/{controller}/{action}/{id}",
      defaults: new { id = RouteParameter.Optional }
  );
            config.Routes.MapHttpRoute(
              name: "ActionBased2",
              routeTemplate: "api/{controller}/action/{action}/{id}",
              defaults: new { id = RouteParameter.Optional }
          );
            config.Routes.MapHttpRoute(
      name: "ActionBased3",
      routeTemplate: "api/{controller}/GetByEmail/{email}",
      defaults: new { email = RouteParameter.Optional }
  );


            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
        }
    }
}
