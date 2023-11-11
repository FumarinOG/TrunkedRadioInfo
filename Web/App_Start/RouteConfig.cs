using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "System",
                "System/{action}/{systemID}",
                new { controller = "System" });

            routes.MapRoute(
                "SystemDetail",
                "SystemDetail/{action}/{systemID}",
                new { controller = "SystemDetail" });

            routes.MapRoute(
                "Talkgroup",
                "Talkgroup/{systemID}/{talkgroupID}",
                new { controller = "Talkgroup", action = "Index" }
            );

            routes.MapRoute(
                "Radio",
                "Radio/{systemID}/{radioID}",
                new { controller = "Radio", action = "Index" }
            );

            routes.MapRoute(
                "Tower",
                "Tower/{systemID}/{towerNumber}",
                new { controller = "Tower", action = "Index" }
            );

            routes.MapRoute(
                "Frequency",
                "Frequency/{systemID}/{towerNumber}",
                new { controller = "Frequency", action = "Index" }
            );

            routes.MapRoute(
                "Patch",
                "Patch/{systemID}/{fromTalkgroupID}/{toTalkgroupID}",
                new { controller = "Patch", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
