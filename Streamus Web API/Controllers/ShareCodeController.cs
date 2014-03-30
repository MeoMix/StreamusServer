using log4net;
using NHibernate;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;
using Streamus_Web_API.Dto;
using System;
using System.Web.Http;

namespace Streamus_Web_API.Controllers
{
    [RoutePrefix("ShareCode")]
    public class ShareCodeController : StreamusController
    {
        private readonly IShareCodeManager ShareCodeManager;
        private readonly IPlaylistManager PlaylistManager;

        public ShareCodeController(ILog logger, ISession session, IManagerFactory managerFactory)
            : base(logger, session)
        {
            ShareCodeManager = managerFactory.GetShareCodeManager(session);
            PlaylistManager = managerFactory.GetPlaylistManager(session);
        }

        [Route("GetShareCode")]
        [HttpGet]
        public ShareCodeDto GetShareCode(Guid playlistId)
        {
            ShareCodeDto shareCodeDto;

            using (ITransaction transaction = Session.BeginTransaction())
            {
                Playlist playlist = PlaylistManager.CopyAndSave(playlistId);
                ShareCode shareCode = ShareCodeManager.GetShareCode(playlist);
                shareCodeDto = ShareCodeDto.Create(shareCode);

                transaction.Commit();
            }

            return shareCodeDto;
        }
    }
}
