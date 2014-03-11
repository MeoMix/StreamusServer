using NHibernate;
using Streamus_Web_API.Domain.Interfaces;
using log4net;

namespace Streamus_Web_API.Controllers
{
    public class VideoController : StreamusController
    {
        private readonly IVideoManager VideoManager;

        public VideoController(ILog logger, ISession session, IManagerFactory managerFactory)
            : base(logger, session)
        {
            VideoManager = managerFactory.GetVideoManager();
        }

    }
}
