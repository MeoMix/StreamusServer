using System.Web.Http;
using NHibernate;
using log4net;
using System;

namespace Streamus_Web_API.Controllers
{
    public abstract class StreamusController : ApiController
    {
        protected readonly ILog Logger;
        protected readonly ISession Session;

        protected StreamusController(ILog logger, ISession session)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            if (session == null) throw new ArgumentNullException("session");

            Logger = logger;

            Session = session;
        }
    }
}
