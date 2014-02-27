using log4net;
using Streamus.Domain;
using Streamus.Domain.Interfaces;
using Streamus.Domain.Managers;
using Streamus.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Streamus.Controllers
{
    [SessionManagement]
    public class PlaylistItemController : Controller
    {
        private readonly ILog Logger;
        private readonly IPlaylistItemManager PlaylistItemManager;

        public PlaylistItemController(ILog logger, IDaoFactory daoFactory, IManagerFactory managerFactory)
        {
            Logger = logger;
            IPlaylistItemDao playlistItemDao = daoFactory.GetPlaylistItemDao();
            IVideoDao videoDao = daoFactory.GetVideoDao();

            PlaylistItemManager = managerFactory.GetPlaylistItemManager(playlistItemDao, videoDao);
        }

        [HttpPost]
        public JsonServiceStackResult Create(PlaylistItemDto playlistItemDto)
        {
            PlaylistItem playlistItem = PlaylistItem.Create(playlistItemDto);

            playlistItem.Playlist.AddItem(playlistItem);

            PlaylistItemManager.Save(playlistItem);
            
            PlaylistItemDto savedPlaylistItemDto = PlaylistItemDto.Create(playlistItem);

            return new JsonServiceStackResult(savedPlaylistItemDto);
        }

        [HttpPost]
        public JsonServiceStackResult CreateMultiple(List<PlaylistItemDto> playlistItemDtos)
        {
            List<PlaylistItem> playlistItems = PlaylistItem.Create(playlistItemDtos);

            //  Split items into their respective playlists and then save on each.
            foreach (var playlistGrouping in playlistItems.GroupBy(i => i.Playlist))
            {
                List<PlaylistItem> groupingItems = playlistGrouping.ToList();

                Playlist playlist = groupingItems.First().Playlist;
                playlist.AddItems(groupingItems);
      
                PlaylistItemManager.Save(groupingItems);
            }

            List<PlaylistItemDto> savedPlaylistItemDtos = PlaylistItemDto.Create(playlistItems);

            return new JsonServiceStackResult(savedPlaylistItemDtos);
        }

        [HttpPut]
        public ActionResult Update(PlaylistItemDto playlistItemDto)
        {
            PlaylistItem playlistItem = PlaylistItem.Create(playlistItemDto);
            PlaylistItemManager.Update(playlistItem);

            PlaylistItemDto updatedPlaylistItemDto = PlaylistItemDto.Create(playlistItem);

            return new JsonServiceStackResult(updatedPlaylistItemDto);
        }

        [HttpPut]
        public ActionResult UpdateMultiple(List<PlaylistItemDto> playlistItemDtos)
        {
            List<PlaylistItem> playlistItems = PlaylistItem.Create(playlistItemDtos);

            PlaylistItemManager.Update(playlistItems);

            List<PlaylistItemDto> savedPlaylistItemDtos = PlaylistItemDto.Create(playlistItems);

            return new JsonServiceStackResult(savedPlaylistItemDtos);
        }

        [HttpDelete]
        public JsonResult Delete(Guid id)
        {
            PlaylistItemManager.Delete(id);

            return Json(new
                {
                    success = true
                });
        }
    }
}
