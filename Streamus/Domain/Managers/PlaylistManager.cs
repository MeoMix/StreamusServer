using Streamus.Dao;
using Streamus.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace Streamus.Domain.Managers
{
    public class PlaylistManager : AbstractManager, IPlaylistManager
    {
        private IPlaylistDao PlaylistDao { get; set; }
        private IVideoDao VideoDao { get; set; }

        public PlaylistManager(ILog logger, IPlaylistDao playlistDao, IVideoDao videoDao)
            : base(logger)
        {
            PlaylistDao = playlistDao;
            VideoDao = videoDao;
        }

        public Playlist Get(Guid id)
        {
            Playlist playlist;

            try
            {
                playlist = PlaylistDao.Get(id);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }

            return playlist;
        }

        public void Save(Playlist playlist)
        {
            try
            {
                DoSave(playlist);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

        public void Save(IEnumerable<Playlist> playlists)
        {
            try
            {
                List<Playlist> playlistsList = playlists.ToList();

                if (playlistsList.Count > 1000)
                {
                    NHibernateSessionManager.Instance.SessionFactory.GetCurrentSession().SetBatchSize(playlistsList.Count / 10);
                }
                else
                {
                    NHibernateSessionManager.Instance.SessionFactory.GetCurrentSession().SetBatchSize(playlistsList.Count / 5);
                }

                playlistsList.ForEach(DoSave);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

        public void Update(Playlist playlist)
        {
            try
            {
                playlist.ValidateAndThrow();

                Playlist knownPlaylist = PlaylistDao.Get(playlist.Id);

                if (knownPlaylist == null)
                {
                    PlaylistDao.Update(playlist);
                }
                else
                {
                    PlaylistDao.Merge(playlist);
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

        public void Delete(Guid id)
        {
            try
            {
                Playlist playlist = PlaylistDao.Get(id);
                playlist.User.RemovePlaylist(playlist);
                PlaylistDao.Delete(playlist);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

        public void UpdateTitle(Guid playlistId, string title)
        {
            try
            {
                Playlist playlist = PlaylistDao.Get(playlistId);
                playlist.Title = title;
                PlaylistDao.Update(playlist);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

        /// <summary>
        ///     This is the work for saving a PlaylistItem without the Transaction wrapper.
        /// </summary>
        private void DoSave(Playlist playlist)
        {
            foreach (PlaylistItem playlistItem in playlist.Items)
            {
                //  This is a bit of a hack, but NHibernate pays attention to the "dirtyness" of immutable entities.
                //  As such, if two PlaylistItems reference the same Video object -- NonUniqueObjectException is thrown even though no changes
                //  can be persisted to the database.
                playlistItem.Video = VideoDao.Merge(playlistItem.Video);

                playlistItem.ValidateAndThrow();
                playlistItem.Video.ValidateAndThrow();
            }

            playlist.ValidateAndThrow();
            PlaylistDao.Save(playlist);
        }
    }
}