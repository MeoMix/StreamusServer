using NHibernate;
using log4net;
using System.Web.Mvc;

namespace Streamus.Controllers
{
    public class HomeController : StreamusController
    {
        public HomeController(ILog logger, ISession session)
            : base(logger, session)
        {
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
