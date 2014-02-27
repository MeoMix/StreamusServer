using log4net;
using System.Web.Mvc;

namespace Streamus.Controllers
{
    public abstract class AbstractController : Controller
    {
        protected readonly ILog Logger;

        protected AbstractController(ILog logger)
        {
            Logger = logger;
        }
    }
}
