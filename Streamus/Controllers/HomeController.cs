using System.Web.Mvc;

namespace Streamus.Controllers
{
    [SessionManagement]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
