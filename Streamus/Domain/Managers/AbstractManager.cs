using log4net;

namespace Streamus.Domain.Managers
{
    public abstract class AbstractManager
    {
        protected readonly ILog Logger;

        protected AbstractManager(ILog logger)
        {
            Logger = logger;
        }
    }
}