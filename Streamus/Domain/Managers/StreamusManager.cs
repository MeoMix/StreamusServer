using log4net;

namespace Streamus.Domain.Managers
{
    public abstract class StreamusManager
    {
        protected readonly ILog Logger;

        protected StreamusManager(ILog logger)
        {
            Logger = logger;
        }
    }
}