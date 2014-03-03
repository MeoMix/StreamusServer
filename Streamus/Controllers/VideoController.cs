using NHibernate;
using log4net;
using Streamus.Domain;
using Streamus.Domain.Interfaces;
using Streamus.Dto;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Streamus.Controllers
{
    public class VideoController : StreamusController
    {
        private readonly IVideoManager VideoManager;

        public VideoController(ILog logger, IManagerFactory managerFactory)
            : base(logger)
        {
            VideoManager = managerFactory.GetVideoManager();
        }

    }
}
