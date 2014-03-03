using log4net;
using NHibernate;
using Streamus.Domain;
using Streamus.Domain.Interfaces;
using Streamus.Dto;
using System;
using System.Web.Mvc;

namespace Streamus.Controllers
{
    public class PlaylistController : StreamusController
    {
        private readonly IPlaylistManager PlaylistManager;
        private readonly IUserManager UserManager;
        private readonly IShareCodeManager ShareCodeManager;

        public PlaylistController(ILog logger, ISession session, IManagerFactory managerFactory)
            : base(logger, session)
        {
            PlaylistManager = managerFactory.GetPlaylistManager();
            UserManager = managerFactory.GetUserManager();
            ShareCodeManager = managerFactory.GetShareCodeManager();
        }

        [HttpPost]
        public JsonResult Create(PlaylistDto playlistDto)
        {
            PlaylistDto savedPlaylistDto;

            using (ITransaction transaction = Session.BeginTransaction())
            {
                Playlist playlist = Playlist.Create(playlistDto);
                playlist.User.AddPlaylist(playlist);

                //  Make sure the playlist has been setup properly before it is cascade-saved through the User.
                playlist.ValidateAndThrow();

                PlaylistManager.Save(playlist);

                savedPlaylistDto = PlaylistDto.Create(playlist);

                transaction.Commit();
            }

            return Json(savedPlaylistDto);
        }

        [HttpPut]
        public JsonResult Update(PlaylistDto playlistDto)
        {
            PlaylistDto updatedPlaylistDto;

            using (ITransaction transaction = Session.BeginTransaction())
            {
                Playlist playlist = Playlist.Create(playlistDto);
                PlaylistManager.Update(playlist);

                updatedPlaylistDto = PlaylistDto.Create(playlist);

                transaction.Commit();
            }

            return Json(updatedPlaylistDto);
        }

        [HttpGet]
        public JsonResult Get(Guid id)
        {
            PlaylistDto playlistDto;   
            using (ITransaction transaction = Session.BeginTransaction())
            {
                Playlist playlist = PlaylistManager.Get(id);
                playlistDto = PlaylistDto.Create(playlist);

                transaction.Commit();
            }

            return Json(playlistDto);
        }

        [HttpDelete]
        public JsonResult Delete(Guid id)
        {            
            using (ITransaction transaction = Session.BeginTransaction())
            {
                PlaylistManager.Delete(id);
                transaction.Commit();
            }

            return Json(new
                {
                    success = true
                });
        }

        [HttpPost]
        public JsonResult UpdateTitle(Guid playlistId, string title)
        {
            using (ITransaction transaction = Session.BeginTransaction())
            {
                PlaylistManager.UpdateTitle(playlistId, title);

                transaction.Commit();
            }

            return Json(new
                {
                    success = true
                });
        }

        //  TODO: Maybe this should be ShareCodeController's deal and not PlaylistController?
        /// <summary>
        ///     Retrieves a ShareCode relating to a Playlist, create a copy of the Playlist referenced by the ShareCode,
        ///     and return the copied Playlist.
        /// </summary>
        [HttpGet]
        public JsonResult CreateCopyByShareCode(string shareCodeShortId, string urlFriendlyEntityTitle, Guid userId)
        {
            PlaylistDto playlistDto;

            using (ITransaction transaction = Session.BeginTransaction())
            {
                ShareCode shareCode = ShareCodeManager.GetByShortIdAndEntityTitle(shareCodeShortId, urlFriendlyEntityTitle);

                //  Never return the sharecode's playlist reference. Make a copy of it to give out so people can't modify the original.
                Playlist playlistToCopy = PlaylistManager.Get(shareCode.EntityId);

                User user = UserManager.Get(userId);

                var playlistCopy = new Playlist(playlistToCopy);
                user.AddPlaylist(playlistCopy);

                PlaylistManager.Save(playlistCopy);

                playlistDto = PlaylistDto.Create(playlistCopy);

                transaction.Commit();
            }

            return Json(playlistDto);
        }
    }
}
