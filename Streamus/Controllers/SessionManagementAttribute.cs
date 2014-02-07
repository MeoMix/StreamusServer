using Streamus.Dao;
using System.Web.Mvc;

namespace Streamus.Controllers
{
    public class SessionManagementAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
        }

        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();
        }
    }
}
