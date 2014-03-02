using NHibernate;
using log4net;
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
            :base(logger, session)
        {
            PlaylistManager = managerFactory.GetPlaylistManager();
            UserManager = managerFactory.GetUserManager();
            ShareCodeManager = managerFactory.GetShareCodeManager();
        }

        [HttpPost]
        public JsonResult Create(PlaylistDto playlistDto)
        {
            Playlist playlist = Playlist.Create(playlistDto);
            playlist.User.AddPlaylist(playlist);

            //  Make sure the playlist has been setup properly before it is cascade-saved through the User.
            playlist.ValidateAndThrow();

            PlaylistManager.Save(playlist);

            PlaylistDto savedPlaylistDto = PlaylistDto.Create(playlist);

            return Json(savedPlaylistDto);
        }

        [HttpPut]
        public JsonResult Update(PlaylistDto playlistDto)
        {
            Playlist playlist = Playlist.Create(playlistDto);
            PlaylistManager.Update(playlist);

            PlaylistDto updatedPlaylistDto = PlaylistDto.Create(playlist);
            return Json(updatedPlaylistDto);
        }

        [HttpGet]
        public JsonResult Get(Guid id)
        {
            Playlist playlist = PlaylistManager.Get(id);
            PlaylistDto playlistDto = PlaylistDto.Create(playlist);

            return Json(playlistDto);
        }

        [HttpDelete]
        public JsonResult Delete(Guid id)
        {
            PlaylistManager.Delete(id);

            return Json(new
                {
                    success = true
                });
        }

        [HttpPost]
        public JsonResult UpdateTitle(Guid playlistId, string title)
        {
            PlaylistManager.UpdateTitle(playlistId, title);

            return Json(new
                {
                    success = true
                });
        }

        /// <summary>
        ///     Retrieves a ShareCode relating to a Playlist, create a copy of the Playlist referenced by the ShareCode,
        ///     and return the copied Playlist.
        /// </summary>
        [HttpGet]
        public JsonResult CreateCopyByShareCode(string shareCodeShortId, string urlFriendlyEntityTitle, Guid userId)
        {
            ShareCode shareCode = ShareCodeManager.GetByShortIdAndEntityTitle(shareCodeShortId, urlFriendlyEntityTitle);

            //  Never return the sharecode's playlist reference. Make a copy of it to give out so people can't modify the original.
            Playlist playlistToCopy = PlaylistManager.Get(shareCode.EntityId);

            User user = UserManager.Get(userId);

            var playlistCopy = new Playlist(playlistToCopy);
            user.AddPlaylist(playlistCopy);

            PlaylistManager.Save(playlistCopy);

            PlaylistDto playlistDto = PlaylistDto.Create(playlistCopy);
            return Json(playlistDto);
        }
    }
}
