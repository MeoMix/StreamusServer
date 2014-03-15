using log4net;
using NHibernate;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;
using Streamus_Web_API.Dto;
using System;
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
            PlaylistManager = managerFactory.GetPlaylistManager();
            UserManager = managerFactory.GetUserManager();
            ShareCodeManager = managerFactory.GetShareCodeManager();
        }
        
        [Route("")]
        [HttpPost]
        public PlaylistDto Create(PlaylistDto playlistDto)
        {
            PlaylistDto savedPlaylistDto;

            using (ITransaction transaction = Session.BeginTransaction())
            {
                User user = UserManager.Get(playlistDto.UserId);

                Playlist playlist = new Playlist(playlistDto.Id, playlistDto.Sequence, playlistDto.Title, user);

                foreach (PlaylistItemDto playlistItemDto in playlistDto.Items)
                {
                    Video video = new Video(playlistItemDto.Video.Id, playlistItemDto.Video.Title, playlistItemDto.Video.Duration, playlistItemDto.Video.Author);
                    
                    playlist.AddItem(new PlaylistItem(playlistItemDto.Id, playlistItemDto.Sequence, playlistItemDto.Title, playlist, video));
                }

                playlist.User.AddPlaylist(playlist);

                //  Make sure the playlist has been setup properly before it is cascade-saved through the User.
                playlist.ValidateAndThrow();

                PlaylistManager.Save(playlist);

                savedPlaylistDto = PlaylistDto.Create(playlist);

                transaction.Commit();
            }

            return savedPlaylistDto;
        }

        [Route("")]
        [HttpPut]
        public PlaylistDto Update(PlaylistDto playlistDto)
        {
            PlaylistDto updatedPlaylistDto;

            using (ITransaction transaction = Session.BeginTransaction())
            {
                User user = UserManager.Get(playlistDto.UserId);

                Playlist playlist = new Playlist(playlistDto.Id, playlistDto.Sequence, playlistDto.Title, user);

                foreach (PlaylistItemDto playlistItemDto in playlistDto.Items)
                {
                    Video video = new Video(playlistItemDto.Video.Id, playlistItemDto.Video.Title, playlistItemDto.Video.Duration, playlistItemDto.Video.Author);

                    playlist.AddItem(new PlaylistItem(playlistItemDto.Id, playlistItemDto.Sequence, playlistItemDto.Title, playlist, video));
                }

                PlaylistManager.Update(playlist);

                updatedPlaylistDto = PlaylistDto.Create(playlist);

                transaction.Commit();
            }

            return updatedPlaylistDto;
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

        [Route("UpdateTitle")]
        [HttpPatch]
        public void UpdateTitle(PlaylistDto playlistDto)
        {
            using (ITransaction transaction = Session.BeginTransaction())
            {
                PlaylistManager.UpdateTitle(playlistDto.Id, playlistDto.Title);

                transaction.Commit();
            }
        }

        /// <summary>
        ///     Retrieves a ShareCode relating to a Playlist, create a copy of the Playlist referenced by the ShareCode,
        ///     and return the copied Playlist.
        /// </summary>
        [Route("CreateCopyByShareCode")]
        [HttpGet]
        public PlaylistDto CreateCopyByShareCode(ShareCodeRequestDto shareCodeRequestDto)
        {
            PlaylistDto playlistDto;

            using (ITransaction transaction = Session.BeginTransaction())
            {
                ShareCode shareCode = ShareCodeManager.GetByShortIdAndEntityTitle(shareCodeRequestDto.ShortId, shareCodeRequestDto.UrlFriendlyEntityTitle);

                //  Never return the sharecode's playlist reference. Make a copy of it to give out so people can't modify the original.
                Playlist playlistToCopy = PlaylistManager.Get(shareCode.EntityId);

                User user = UserManager.Get(shareCodeRequestDto.UserId);

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
