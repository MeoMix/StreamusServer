using System.Web.Http;

namespace Streamus_Web_API.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.EnsureInitialized();
        }
    }
}
