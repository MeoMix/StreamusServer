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

        //  TODO: It feels weird to be retrieving information like this, but I'm not sure of a better way to tackle it just yet.
        [Route("{shortId}/{entityTitle}")]
        [HttpGet]
        public ShareCodeDto GetShareCodeByShortIdAndEntityTitle(string shortId, string entityTitle)
        {
            ShareCodeDto shareCodeDto;

            using (ITransaction transaction = Session.BeginTransaction())
            {
                ShareCode shareCode = ShareCodeManager.GetByShortIdAndEntityTitle(shortId, entityTitle);
                shareCodeDto = ShareCodeDto.Create(shareCode);

                transaction.Commit();
            }

            return shareCodeDto;
        }

        [Route("GetShareCode")]
        [HttpGet]
        public ShareCodeDto GetShareCode(Guid playlistId)
        {
            ShareCodeDto shareCodeDto;

            using (ITransaction transaction = Session.BeginTransaction())
            {
                //  Copy the playlist here to serve a static copy instead of whatever the state is when share code is accessed.
                Playlist playlist = PlaylistManager.CopyAndSave(playlistId);
                ShareCode shareCode = ShareCodeManager.GetShareCode(playlist);
                shareCodeDto = ShareCodeDto.Create(shareCode);

                transaction.Commit();
            }

            return shareCodeDto;
        }
    }
}
