using System.Web.Http;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;
using Streamus_Web_API.Dto;
using log4net;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Streamus_Web_API.Controllers
{
    [RoutePrefix("PlaylistItem")]
    public class PlaylistItemController : StreamusController
    {
        private readonly IPlaylistManager PlaylistManager;
        private readonly IPlaylistItemManager PlaylistItemManager;

        public PlaylistItemController(ILog logger, ISession session, IManagerFactory managerFactory)
            : base(logger, session)
        {
            PlaylistManager = managerFactory.GetPlaylistManager(session);
            PlaylistItemManager = managerFactory.GetPlaylistItemManager(session);
        }

        [Route("")]
        [HttpPost]
        public PlaylistItemDto Create(PlaylistItemDto playlistItemDto)
        {
            PlaylistItemDto savedPlaylistItemDto;

            using(ITransaction transaction = Session.BeginTransaction())
            {
                Playlist playlist = PlaylistManager.Get(playlistItemDto.PlaylistId);

                SongDto songDto = playlistItemDto.Song;

                PlaylistItem playlistItem = new PlaylistItem(playlistItemDto.Id, playlistItemDto.Title,  playlistItemDto.Cid, songDto.Id, songDto.Type, songDto.Title, songDto.Duration, songDto.Author);
                playlistItemDto.SetPatchableProperties(playlistItem);

                playlist.AddItem(playlistItem);

                PlaylistItemManager.Save(playlistItem);

                savedPlaylistItemDto = PlaylistItemDto.Create(playlistItem);
                
                transaction.Commit();
            }

            return savedPlaylistItemDto;
        }

        [Route("CreateMultiple")]
        [HttpPost]
        public IEnumerable<PlaylistItemDto> CreateMultiple(List<PlaylistItemDto> playlistItemDtos)
        {
            List<PlaylistItemDto> savedPlaylistItemDtos;

            int count = playlistItemDtos.Count;

            if (count > 1000)
                Session.SetBatchSize(count / 10);
            else if (count > 500)
                Session.SetBatchSize(count / 5);
            else if (count > 2)
                Session.SetBatchSize(count / 2);

            using (ITransaction transaction = Session.BeginTransaction())
            {
                List<PlaylistItem> savedPlaylistItems = new List<PlaylistItem>();

                //  Split items into their respective playlists and then save on each.
                foreach (var playlistGrouping in playlistItemDtos.GroupBy(pid => pid.PlaylistId))
                {
                    List<PlaylistItemDto> groupedPlaylistItemDtos = playlistGrouping.ToList();
                    Playlist playlist = PlaylistManager.Get(playlistGrouping.Key);

                    foreach (var playlistItemDto in groupedPlaylistItemDtos)
                    {
                        SongDto songDto = playlistItemDto.Song;

                        PlaylistItem playlistItem = new PlaylistItem(playlistItemDto.Id, playlistItemDto.Title, playlistItemDto.Cid, songDto.Id, songDto.Type, songDto.Title, songDto.Duration, songDto.Author);
                        playlistItemDto.SetPatchableProperties(playlistItem);
                        
                        playlist.AddItem(playlistItem);

                        savedPlaylistItems.Add(playlistItem);
                    }
                }

                PlaylistItemManager.Save(savedPlaylistItems);

                savedPlaylistItemDtos = PlaylistItemDto.Create(savedPlaylistItems);

                transaction.Commit();
            }

            return savedPlaylistItemDtos;
        }

        [Route("{id:guid}")]
        [HttpPatch]
        public void Patch(Guid id, PlaylistItemDto playlistItemDto)
        {
            using (ITransaction transaction = Session.BeginTransaction())
            {
                PlaylistItem playlistItem = PlaylistItemManager.Get(id);
                playlistItemDto.SetPatchableProperties(playlistItem);
                PlaylistItemManager.Update(playlistItem);

                transaction.Commit();
            }
        }

        [Route("{id:guid}")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            using (ITransaction transaction = Session.BeginTransaction())
            {
                PlaylistItemManager.Delete(id);
                transaction.Commit();
            }
        }
    }
}
