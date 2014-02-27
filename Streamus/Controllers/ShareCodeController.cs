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

        public ShareCodeController(ILog logger, IManagerFactory managerFactory)
            : base(logger)
        {
            try
            {
                IPlaylistManager playlistManager = managerFactory.GetPlaylistManager();
                ShareCodeManager = managerFactory.GetShareCodeManager(playlistManager);
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
