using log4net;
using Streamus.Domain;
using Streamus.Domain.Interfaces;
using Streamus.Dto;
using System;
using System.Web.Mvc;

namespace Streamus.Controllers
{
    [SessionManagement]
    public class ShareCodeController : AbstractController
    {
        private readonly IShareCodeManager ShareCodeManager;
        private readonly IPlaylistManager PlaylistManager;

        public ShareCodeController(ILog logger, IManagerFactory managerFactory)
            : base(logger)
        {
            try
            {
                ShareCodeManager = managerFactory.GetShareCodeManager();
                PlaylistManager = managerFactory.GetPlaylistManager();
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
            if (entityType != ShareableEntityType.Playlist)
                throw new NotSupportedException("Only Playlist entityType can be shared currently.");

            Playlist playlist = PlaylistManager.CopyAndSave(entityId);
            ShareCode shareCode = ShareCodeManager.GetShareCode(playlist);
            ShareCodeDto shareCodeDto = ShareCodeDto.Create(shareCode);

            return Json(shareCodeDto);
        }
    }
}
