using log4net;
using Streamus.Domain;
using Streamus.Domain.Interfaces;
using Streamus.Dto;
using System;
using System.Web.Mvc;

namespace Streamus.Controllers
{
    [SessionManagement]
    public class ShareCodeController : Controller
    {
        private readonly ILog Logger;
        private readonly IShareCodeManager ShareCodeManager;
        private readonly IShareCodeDao ShareCodeDao;

        public ShareCodeController(ILog logger, IDaoFactory daoFactory, IManagerFactory managerFactory)
        {
            Logger = logger;

            try
            {
                ShareCodeDao = daoFactory.GetShareCodeDao();
                IPlaylistDao playlistDao = daoFactory.GetPlaylistDao();
                IVideoDao videoDao = daoFactory.GetVideoDao();
                IPlaylistManager playlistManager = managerFactory.GetPlaylistManager(playlistDao, videoDao);
                ShareCodeManager = managerFactory.GetShareCodeManager(playlistDao, ShareCodeDao, playlistManager);
            }
            catch (TypeInitializationException exception)
            {
                Logger.Error(exception.InnerException);
                throw exception.InnerException;
            }
        }

        [HttpGet]
        public JsonResult GetShareCode(ShareableEntityType entityType, Guid entityId)
        {
            ShareCode shareCode = ShareCodeManager.GetShareCode(entityType, entityId);
            ShareCodeDto shareCodeDto = ShareCodeDto.Create(shareCode);

            return new JsonServiceStackResult(shareCodeDto);
        }
    }
}
