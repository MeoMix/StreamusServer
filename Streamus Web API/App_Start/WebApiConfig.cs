using System.Web.Http;

namespace Streamus_Web_API.App_Start
{
    public static class WebApiConfig
    {
        //  TODO: Create test cases for custom routing.
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Routes.MapHttpRoute("UpdateTitle", "Playlist/UpdateTitle/{playlistId}/{title}");

            //  TODO: What does this do?
            // Web API routes
            //config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }
    }
}
