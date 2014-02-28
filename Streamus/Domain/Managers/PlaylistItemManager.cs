using log4net;
using Streamus.Dao;
using Streamus.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Streamus.Domain.Managers
{
    public class PlaylistItemManager : AbstractManager, IPlaylistItemManager
    {
        private IPlaylistItemDao PlaylistItemDao { get; set; }
        private IVideoDao VideoDao { get; set; }

        public PlaylistItemManager(ILog logger, IPlaylistItemDao playlistItemDao, IVideoDao videoDao)
            : base(logger)
        {
            PlaylistItemDao = playlistItemDao;
            VideoDao = videoDao;
        }

        public PlaylistItem Get(Guid id)
        {
            PlaylistItem playlistItem;

            try
            {
                playlistItem = PlaylistItemDao.Get(id);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }

            return playlistItem;
        }

        public void Delete(Guid itemId)
        {
            try
            {
                PlaylistItem playlistItem = PlaylistItemDao.Get(itemId);

                //  Be sure to remove from Playlist first so that cascade doesn't re-save.
                playlistItem.Playlist.RemoveItem(playlistItem);
                PlaylistItemDao.Delete(playlistItem);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

        public void Update(IEnumerable<PlaylistItem> playlistItems)
        {
            try
            {
                var playlistItemList = playlistItems.ToList();

                if (playlistItemList.Count > 1000)
                {
                    NHibernateSessionManager.Instance.SessionFactory.GetCurrentSession().SetBatchSize(playlistItemList.Count / 10);
                }
                else
                {
                    NHibernateSessionManager.Instance.SessionFactory.GetCurrentSession().SetBatchSize(playlistItemList.Count / 5);
                }

                playlistItemList.ForEach(DoUpdate);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

        public void Update(PlaylistItem playlistItem)
        {
            try
            {
                DoUpdate(playlistItem);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

        public void Save(PlaylistItem playlistItem)
        {
            try
            {
                DoSave(playlistItem);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

        public void Save(IEnumerable<PlaylistItem> playlistItems)
        {
            try
            {
                var playlistItemList = playlistItems.ToList();

                if (playlistItemList.Count > 1000)
                {
                    NHibernateSessionManager.Instance.SessionFactory.GetCurrentSession().SetBatchSize(playlistItemList.Count / 10);
                }
                else
                {
                    NHibernateSessionManager.Instance.SessionFactory.GetCurrentSession().SetBatchSize(playlistItemList.Count / 5);
                }

                playlistItemList.ForEach(DoSave);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

        /// <summary>
        /// This is the work for saving a PlaylistItem without the Transaction wrapper.
        /// </summary>
        private void DoSave(PlaylistItem playlistItem)
        {            
		    //  This is a bit of a hack, but NHibernate pays attention to the "dirtyness" of immutable entities.
            //  As such, if two PlaylistItems reference the same Video object -- NonUniqueObjectException is thrown even though no changes
            //  can be persisted to the database.
            playlistItem.Video = VideoDao.Merge(playlistItem.Video);

            playlistItem.ValidateAndThrow();
            playlistItem.Video.ValidateAndThrow();

            PlaylistItemDao.Save(playlistItem);
        }

        private void DoUpdate(PlaylistItem playlistItem)
        {
            playlistItem.ValidateAndThrow();
            playlistItem.Video.ValidateAndThrow();

            PlaylistItem knownPlaylistItem = PlaylistItemDao.Get(playlistItem.Id);

            if (knownPlaylistItem == null)
            {
                PlaylistItemDao.Update(playlistItem);
            }
            else
            {
                PlaylistItemDao.Merge(playlistItem);
            }
        }
    }
}