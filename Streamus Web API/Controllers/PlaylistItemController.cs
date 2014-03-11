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
    public class PlaylistItemController : StreamusController
    {
        private readonly IPlaylistManager PlaylistManager;
        private readonly IPlaylistItemManager PlaylistItemManager;

        public PlaylistItemController(ILog logger, ISession session, IManagerFactory managerFactory)
            : base(logger, session)
        {
            PlaylistManager = managerFactory.GetPlaylistManager();
            PlaylistItemManager = managerFactory.GetPlaylistItemManager();
        }

        [HttpPost]
        public PlaylistItemDto Create(PlaylistItemDto playlistItemDto)
        {
            PlaylistItemDto savedPlaylistItemDto;

            using(ITransaction transaction = Session.BeginTransaction())
            {
                PlaylistItem playlistItem = PlaylistItem.Create(playlistItemDto, PlaylistManager);

                playlistItem.Playlist.AddItem(playlistItem);

                PlaylistItemManager.Save(playlistItem);

                savedPlaylistItemDto = PlaylistItemDto.Create(playlistItem);
                
                transaction.Commit();
            }

            return savedPlaylistItemDto;
        }

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
                List<PlaylistItem> playlistItems = PlaylistItem.Create(playlistItemDtos, PlaylistManager);

                //  Split items into their respective playlists and then save on each.
                foreach (var playlistGrouping in playlistItems.GroupBy(i => i.Playlist))
                {
                    List<PlaylistItem> groupingItems = playlistGrouping.ToList();
                    playlistGrouping.Key.AddItems(groupingItems);
                    PlaylistItemManager.Save(groupingItems);
                }

                savedPlaylistItemDtos = PlaylistItemDto.Create(playlistItems);

                transaction.Commit();
            }

            return savedPlaylistItemDtos;
        }

        [HttpPut]
        public PlaylistItemDto Update(PlaylistItemDto playlistItemDto)
        {
            PlaylistItemDto updatedPlaylistItemDto;

            using (ITransaction transaction = Session.BeginTransaction())
            {

                PlaylistItem playlistItem = PlaylistItem.Create(playlistItemDto, PlaylistManager);
                PlaylistItemManager.Update(playlistItem);

                updatedPlaylistItemDto = PlaylistItemDto.Create(playlistItem);
                transaction.Commit();
            }

            return updatedPlaylistItemDto;
        }

        [HttpPut]
        public IEnumerable<PlaylistItemDto> UpdateMultiple(List<PlaylistItemDto> playlistItemDtos)
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
                List<PlaylistItem> playlistItems = PlaylistItem.Create(playlistItemDtos, PlaylistManager);

                PlaylistItemManager.Update(playlistItems);

                savedPlaylistItemDtos = PlaylistItemDto.Create(playlistItems);

                transaction.Commit();
            }

            return savedPlaylistItemDtos;
        }

        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            using (ITransaction transaction = Session.BeginTransaction())
            {
                PlaylistItemManager.Delete(id);
                transaction.Commit();
            }

            return Ok();
        }
    }
}
