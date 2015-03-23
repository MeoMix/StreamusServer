using log4net;
using NHibernate;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;
using Streamus_Web_API.Dto;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Streamus_Web_API.Controllers
{
    [RoutePrefix("Playlist")]
    public class PlaylistController : StreamusController
    {
        private readonly IPlaylistManager PlaylistManager;
        private readonly IUserManager UserManager;
        private readonly IShareCodeManager ShareCodeManager;

        public PlaylistController(ILog logger, ISession session, IManagerFactory managerFactory)
            : base(logger, session)
        {
            PlaylistManager = managerFactory.GetPlaylistManager(session);
            UserManager = managerFactory.GetUserManager(session);
            ShareCodeManager = managerFactory.GetShareCodeManager(session);
        }
        
        [Route("")]
        [HttpPost]
        public PlaylistDto Create(PlaylistDto playlistDto)
        {
            PlaylistDto savedPlaylistDto;

            using (ITransaction transaction = Session.BeginTransaction())
            {
                User user = UserManager.Get(playlistDto.UserId);

                Playlist playlist = new Playlist(playlistDto.Id);
                playlistDto.SetPatchableProperties(playlist);

                user.AddPlaylist(playlist);

                List<PlaylistItem> playlistItems = new List<PlaylistItem>();
                foreach (PlaylistItemDto dto in playlistDto.Items)
                {
                    PlaylistItem playlistItem = new PlaylistItem(dto.Id, dto.Title, dto.Cid, dto.Song.Id, dto.Song.Type, dto.Song.Title, dto.Song.Duration, dto.Song.Author);
                    dto.SetPatchableProperties(playlistItem);
                    playlistItems.Add(playlistItem);
                }
                playlist.AddItems(playlistItems);

                PlaylistManager.Save(playlist);
                savedPlaylistDto = PlaylistDto.Create(playlist);

                transaction.Commit();
            }

            return savedPlaylistDto;
        }
        
        [Route("{id:guid}")]
        [HttpGet]
        public PlaylistDto Get(Guid id)
        {
            PlaylistDto playlistDto;   
            using (ITransaction transaction = Session.BeginTransaction())
            {
                Playlist playlist = PlaylistManager.Get(id);
                playlistDto = PlaylistDto.Create(playlist);

                transaction.Commit();
            }

            return playlistDto;
        }

        [Route("{id:guid}")]
        [HttpDelete]
        public void Delete(Guid id)
        {            
            using (ITransaction transaction = Session.BeginTransaction())
            {
                PlaylistManager.Delete(id);
                transaction.Commit();
            }
        }

        [Route("{id:guid}")]
        [HttpPatch]
        public void Patch(Guid id, PlaylistDto playlistDto)
        {
            using (ITransaction transaction = Session.BeginTransaction())
            {
                Playlist playlist = PlaylistManager.Get(id);
                playlistDto.SetPatchableProperties(playlist);
                PlaylistManager.Update(playlist);

                transaction.Commit();
            }
        }

        /// <summary>
        ///     Retrieves a Playlist, create a copy of the Playlist, and returns the copied Playlist
        /// </summary>
        [Route("Copy")]
        [HttpPost]
        public PlaylistDto Copy(CopyPlaylistRequestDto copyPlaylistRequestDto)
        {
            PlaylistDto playlistDto;

            using (ITransaction transaction = Session.BeginTransaction())
            {
                Playlist playlistToCopy = PlaylistManager.Get(copyPlaylistRequestDto.PlaylistId);
                User user = UserManager.Get(copyPlaylistRequestDto.UserId);

                var playlistCopy = new Playlist(playlistToCopy);
                user.AddPlaylist(playlistCopy);

                PlaylistManager.Save(playlistCopy);

                playlistDto = PlaylistDto.Create(playlistCopy);

                transaction.Commit();
            }

            return playlistDto;
        }
    }
}
