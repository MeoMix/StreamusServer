using NHibernate;
using log4net;
using System;
using System.Text;
using System.Web.Mvc;

namespace Streamus.Controllers
{
    public abstract class StreamusController : Controller
    {
        protected readonly ILog Logger;
        protected new readonly ISession Session;

        protected StreamusController(ILog logger, ISession session)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            if (session == null) throw new ArgumentNullException("session");

            Logger = logger;

            //  TODO: I receive a warning, "Multiple sessions in a single request." by NHProf if I work with the ISession parameter and not through DependencyResolver.
            //Session = DependencyResolver.Current.GetService<ISession>();
            Session = session;
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
