using System.Text;
using log4net;
using System.Web.Mvc;

namespace Streamus.Controllers
{
    public abstract class StreamusController : Controller
    {
        protected readonly ILog Logger;

        protected StreamusController(ILog logger)
        {
            Logger = logger;
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }
    }
}
