using System.Web.Http;
using System.Web.Http.Results;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;
using Streamus_Web_API.Dto;
using log4net;
using NHibernate;
using System;

namespace Streamus_Web_API.Controllers
{
    public class ShareCodeController : StreamusController
    {
        private readonly IShareCodeManager ShareCodeManager;
        private readonly IPlaylistManager PlaylistManager;

        public ShareCodeController(ILog logger, ISession session, IManagerFactory managerFactory)
            : base(logger, session)
        {
            ShareCodeManager = managerFactory.GetShareCodeManager();
            PlaylistManager = managerFactory.GetPlaylistManager();
        }

        [HttpGet]
        public ShareCodeDto GetShareCode(ShareableEntityType entityType, Guid entityId)
        {
            if (entityType != ShareableEntityType.Playlist)
                throw new NotSupportedException("Only Playlist entityType can be shared currently.");

            ShareCodeDto shareCodeDto;
            using (ITransaction transaction = Session.BeginTransaction())
            {
                Playlist playlist = PlaylistManager.CopyAndSave(entityId);
                ShareCode shareCode = ShareCodeManager.GetShareCode(playlist);
                shareCodeDto = ShareCodeDto.Create(shareCode);

                transaction.Commit();
            }

            return shareCodeDto;
        }
    }
}
