using NHibernate;
using log4net;
using System.Web.Mvc;

namespace Streamus.Controllers
{
    public class HomeController : StreamusController
    {
        public HomeController(ILog logger)
            : base(logger)
        {
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
