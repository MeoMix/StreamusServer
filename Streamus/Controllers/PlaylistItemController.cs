using log4net;
using NHibernate;
using Streamus.Domain;
using Streamus.Domain.Interfaces;
using Streamus.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Streamus.Controllers
{
    public class PlaylistItemController : StreamusController
    {
        private readonly IPlaylistItemManager PlaylistItemManager;

        public PlaylistItemController(ILog logger, IManagerFactory managerFactory)
            : base(logger)
        {
            PlaylistItemManager = managerFactory.GetPlaylistItemManager();
        }

        [HttpPost]
        public JsonResult Create(PlaylistItemDto playlistItemDto)
        {
            PlaylistItemDto savedPlaylistItemDto;

            using(ITransaction transaction = Session.BeginTransaction())
            {
                PlaylistItem playlistItem = PlaylistItem.Create(playlistItemDto);

                playlistItem.Playlist.AddItem(playlistItem);

                PlaylistItemManager.Save(playlistItem);

                savedPlaylistItemDto = PlaylistItemDto.Create(playlistItem);
                
                transaction.Commit();
            }

            return Json(savedPlaylistItemDto);
        }

        [HttpPost]
        public JsonResult CreateMultiple(List<PlaylistItemDto> playlistItemDtos)
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
                List<PlaylistItem> playlistItems = PlaylistItem.Create(playlistItemDtos);

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

            return Json(savedPlaylistItemDtos);
        }

        [HttpPut]
        public ActionResult Update(PlaylistItemDto playlistItemDto)
        {
            PlaylistItemDto updatedPlaylistItemDto;

            using (ITransaction transaction = Session.BeginTransaction())
            {

                PlaylistItem playlistItem = PlaylistItem.Create(playlistItemDto);
                PlaylistItemManager.Update(playlistItem);

                updatedPlaylistItemDto = PlaylistItemDto.Create(playlistItem);
                transaction.Commit();
            }

            return Json(updatedPlaylistItemDto);
        }

        [HttpPut]
        public ActionResult UpdateMultiple(List<PlaylistItemDto> playlistItemDtos)
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
                List<PlaylistItem> playlistItems = PlaylistItem.Create(playlistItemDtos);

                PlaylistItemManager.Update(playlistItems);

                savedPlaylistItemDtos = PlaylistItemDto.Create(playlistItems);

                transaction.Commit();
            }

            return Json(savedPlaylistItemDtos);
        }

        [HttpDelete]
        public JsonResult Delete(Guid id)
        {
            using (ITransaction transaction = Session.BeginTransaction())
            {
                PlaylistItemManager.Delete(id);
                transaction.Commit();
            }

            return Json(new
                {
                    success = true
                });
        }
    }
}
