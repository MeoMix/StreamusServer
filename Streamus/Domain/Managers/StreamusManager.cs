using NHibernate;
using log4net;

namespace Streamus.Domain.Managers
{
    public abstract class StreamusManager
    {
        protected readonly ILog Logger;
        protected readonly ISession Session;

        protected StreamusManager(ILog logger, ISession session)
        {
            Logger = logger;
            Session = session;
        }
    }
}