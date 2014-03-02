using System.Text;
using NHibernate;
using log4net;
using System.Web.Mvc;

namespace Streamus.Controllers
{
    public abstract class StreamusController : Controller
    {
        protected readonly ILog Logger;
        protected new readonly ISession Session;
        private ITransaction Transaction;

        protected StreamusController(ILog logger, ISession session)
        {
            Logger = logger;
            Session = session;
        }

        protected override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            Transaction = Session.BeginTransaction();
        }

        protected override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            Transaction.Commit();
            Transaction.Dispose();
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
