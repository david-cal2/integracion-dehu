using PSOENotificaciones.Controllers;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;

namespace PSOENotificaciones
{
    public class RouteConfig
    {


        
        public static void RegisterRoutes(RouteCollection routes)
        {
            
            
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Home",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Login",
                id = UrlParameter.Optional }
            );
         

        }
      
    }
}
