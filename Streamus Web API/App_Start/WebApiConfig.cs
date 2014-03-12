using System.Web.Http;

namespace Streamus_Web_API.App_Start
{
    public static class WebApiConfig
    {
        //  TODO: Create test cases for custom routing.
        public static void Register(HttpConfiguration config)
        {
            //  TODO: These methods break RESTful practices. How can I fix?
            // Web API configuration and services
            config.Routes.MapHttpRoute("UpdateTitle", "Playlist/UpdateTitle/{playlistDto}");
            config.Routes.MapHttpRoute("CreateCopyByShareCode", "Playlist/CreateCopyByShareCode/{shareCodeRequestDto}");
            config.Routes.MapHttpRoute("UpdateGooglePlusId", "User/UpdateGooglePlusId/{userDto}");
            config.Routes.MapHttpRoute("GetShareCode", "ShareCode/GetShareCode/{playlistId}");
   
            //  TODO: Kinda odd this one says no params when it actually takes a List. If I specify a param it conflicts with the other POST though.
            config.Routes.MapHttpRoute(
                name: "CreateMultiple",
                routeTemplate: "PlaylistItem/CreateMultiple",
                defaults: new
                    {
                        controller = "PlaylistItem",
                        action = "CreateMultiple"
                    }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }
    }
}
