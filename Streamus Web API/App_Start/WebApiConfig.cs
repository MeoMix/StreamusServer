using System.Web.Http.Cors;
using Streamus_Web_API.Dao;
using System.Web.Http;

namespace Streamus_Web_API.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("http://dev.streamus.com,https://streamus.com,chrome-extension://jbnkffmindojffecdhbbmekbmkkfpmjd,chrome-extension://nnmcpagedcgekmljdamaeahfbmmjloho", "*", "*");
            config.EnableCors(cors);

            AutofacRegistrations.RegisterAndSetResolver(config);
        }
    }
}