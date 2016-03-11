using System;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Caching;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Streamus_Web_API.Controllers
{
  public class ThrottleAttribute : ActionFilterAttribute
  {
    /// <summary>
    ///     A unique name for this Throttle.
    /// </summary>
    /// <remarks>
    ///     We'll be inserting a Cache record based on this name and client IP, e.g. "Name-192.168.0.1"
    /// </remarks>
    public string Name { get; set; }

    /// <summary>
    ///     The number of seconds clients must wait before executing this decorated route again.
    /// </summary>
    public int Seconds { get; set; }

    /// <summary>
    ///     A text message that will be sent to the client upon throttling.  You can include the token {n} to
    ///     show this.Seconds in the message, e.g. "Wait {n} seconds before trying again".
    /// </summary>
    public string Message { get; set; }

    public override void OnActionExecuting(HttpActionContext actionContext)
    {
      string key = string.Concat(Name, "-", GetClientIp(actionContext.Request));
      bool allowExecute = false;

      if (HttpRuntime.Cache[key] == null)
      {
        HttpRuntime.Cache.Add(key,
                              true, // is this the smallest data we can have?
                              null, // no dependencies
                              DateTime.Now.AddSeconds(Seconds), // absolute expiration
                              Cache.NoSlidingExpiration,
                              CacheItemPriority.Low,
                              null); // no callback

        allowExecute = true;
      }

      if (!allowExecute)
      {
        if (string.IsNullOrEmpty(Message))
        {
          Message = "You may only perform this action every {n} seconds.";
        }

        actionContext.Response = actionContext.Request.CreateResponse(
            HttpStatusCode.Conflict,
            Message.Replace("{n}", Seconds.ToString())
            );
      }
    }

    private static string GetClientIp(HttpRequestMessage request)
    {
      if (request.Properties.ContainsKey("MS_HttpContext"))
      {
        return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
      }

      if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
      {
        var prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
        return prop.Address;
      }

      return null;
    }
  }
}
