using System.Web.Http;
using System.Web.Http.Cors;
using Streamus_Web_API.Dao;

namespace Streamus_Web_API
{
  public static class WebApiConfig
  {
    public static void Register(HttpConfiguration config)
    {
      var cors = new EnableCorsAttribute("http://dev.streamus.com:8080,https://streamus.com,chrome-extension://jbnkffmindojffecdhbbmekbmkkfpmjd,chrome-extension://nnmcpagedcgekmljdamaeahfbmmjloho", "*", "*");
      config.EnableCors(cors);

      AutofacRegistrations.RegisterAndSetResolver(config);
    }
  }
}