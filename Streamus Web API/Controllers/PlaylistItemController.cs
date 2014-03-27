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

                PlaylistItem playlistItem = new PlaylistItem(playlistItemDto.Id, playlistItemDto.Sequence, playlistItemDto.Title, songDto.Id, songDto.Type, songDto.Title, songDto.Duration, songDto.Author);
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
            {
                Session.SetBatchSize(count / 10);
            }
            else if (count > 3)
            {
                Session.SetBatchSize(count / 3);
            }

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
                        PlaylistItem playlistItem = new PlaylistItem(playlistItemDto.Id, playlistItemDto.Sequence, playlistItemDto.Title, songDto.Id, songDto.Type, songDto.Title, songDto.Duration, songDto.Author);
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
        
        [Route("")]
        [HttpPut]
        public PlaylistItemDto Update(PlaylistItemDto playlistItemDto)
        {
            PlaylistItemDto updatedPlaylistItemDto;

            using (ITransaction transaction = Session.BeginTransaction())
            {
                PlaylistItem playlistItem = PlaylistItemManager.Get(playlistItemDto.Id);

                playlistItem.Title = playlistItemDto.Title;
                playlistItem.Sequence = playlistItemDto.Sequence;

                PlaylistItemManager.Update(playlistItem);

                updatedPlaylistItemDto = PlaylistItemDto.Create(playlistItem);
                transaction.Commit();
            }

            return updatedPlaylistItemDto;
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
