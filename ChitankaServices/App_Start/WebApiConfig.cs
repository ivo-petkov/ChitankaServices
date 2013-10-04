using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ChitankaServices
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                  name: "BooksApi",
                  routeTemplate: "api/books/{action}/{letter}",
                  defaults: new { controller = "books", letter = RouteParameter.Optional }
              );

            config.Routes.MapHttpRoute(
                  name: "AuthorsApi",
                  routeTemplate: "api/authors/{action}/{letter}",
                  defaults: new { controller = "authors", letter = RouteParameter.Optional }
              );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );            
        }
    }
}
