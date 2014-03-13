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
    //[RoutePrefix("PlaylistItem")]
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

        //  TODO: This has a bug if I attempt to save just 1 playlistItemDto it doesn't know which route to go to.
        //[ActionName("Create")]
        //[Route("{playlistItemDto}")]
        [HttpPost]
        public PlaylistItemDto Create(PlaylistItemDto playlistItemDto)
        {
            PlaylistItemDto savedPlaylistItemDto;

            using(ITransaction transaction = Session.BeginTransaction())
            {
                VideoDto videoDto = playlistItemDto.Video;
                Video video = new Video(videoDto.Id, videoDto.Title, videoDto.Duration, videoDto.Author);
                Playlist playlist = PlaylistManager.Get(playlistItemDto.PlaylistId);
                PlaylistItem playlistItem = new PlaylistItem(playlistItemDto.Id, playlistItemDto.Sequence, playlistItemDto.Title, playlist, video);

                playlistItem.Playlist.AddItem(playlistItem);

                PlaylistItemManager.Save(playlistItem);

                savedPlaylistItemDto = PlaylistItemDto.Create(playlistItem);
                
                transaction.Commit();
            }

            return savedPlaylistItemDto;
        }

        //[ActionName("CreateMultiple")]
        //[Route("{playlistItemDtos}")]
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
                List<PlaylistItem> playlistItems = new List<PlaylistItem>();

                foreach (PlaylistItemDto playlistItemDto in playlistItemDtos)
                {
                    Video video = new Video(playlistItemDto.Video.Id, playlistItemDto.Video.Title, playlistItemDto.Video.Duration, playlistItemDto.Video.Author);
                    Playlist playlist = PlaylistManager.Get(playlistItemDto.PlaylistId);

                    playlistItems.Add(new PlaylistItem(playlistItemDto.Id, playlistItemDto.Sequence, playlistItemDto.Title, playlist, video));
                }

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
                VideoDto videoDto = playlistItemDto.Video;
                Video video = new Video(videoDto.Id, videoDto.Title, videoDto.Duration, videoDto.Author);
                Playlist playlist = PlaylistManager.Get(playlistItemDto.PlaylistId);
                PlaylistItem playlistItem = new PlaylistItem(playlistItemDto.Id, playlistItemDto.Sequence, playlistItemDto.Title, playlist, video);
                PlaylistItemManager.Update(playlistItem);

                updatedPlaylistItemDto = PlaylistItemDto.Create(playlistItem);
                transaction.Commit();
            }

            return updatedPlaylistItemDto;
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
