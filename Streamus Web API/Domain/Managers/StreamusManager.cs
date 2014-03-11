using log4net;

namespace Streamus_Web_API.Domain.Managers
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