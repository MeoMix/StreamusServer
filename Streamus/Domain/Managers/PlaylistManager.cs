using log4net;
using NHibernate;
using Streamus.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Streamus.Domain.Managers
{
    public class PlaylistManager : StreamusManager, IPlaylistManager
    {
        private IPlaylistDao PlaylistDao { get; set; }
        private IVideoDao VideoDao { get; set; }

        public PlaylistManager(ILog logger, ISession session, IPlaylistDao playlistDao, IVideoDao videoDao)
            : base(logger, session)
        {
            PlaylistDao = playlistDao;
            VideoDao = videoDao;
        }

        public Playlist Get(Guid id)
        {
            Playlist playlist;

            try
            {
                using (ITransaction transaction = Session.BeginTransaction())
                {
                    playlist = PlaylistDao.Get(id);

                    transaction.Commit();
                }
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
                using (ITransaction transaction = Session.BeginTransaction())
                {
                    DoSave(playlist);

                    transaction.Commit();
                }
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
                    Session.SetBatchSize(playlistsList.Count / 10);
                }
                else if (playlistsList.Count > 3)
                {
                    Session.SetBatchSize(playlistsList.Count / 3);
                }

                using (ITransaction transaction = Session.BeginTransaction())
                {
                    playlistsList.ForEach(DoSave);
                    transaction.Commit();
                }
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
                using (ITransaction transaction = Session.BeginTransaction())
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

                    transaction.Commit();
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
                using (ITransaction transaction = Session.BeginTransaction())
                {
                    PlaylistDao.DeleteById(id);

                    transaction.Commit();
                }
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
                using (ITransaction transaction = Session.BeginTransaction())
                {
                    PlaylistDao.UpdateTitleById(playlistId, title);
                    transaction.Commit();
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

        /// <summary>
        /// Copy a playlist. Useful for sharing.
        /// </summary>
        /// <param name="id">The playlist ID to copy</param>
        /// <returns>A new playlist with a new ID which has been saved.</returns>
        public Playlist CopyAndSave(Guid id)
        {
            Playlist copiedPlaylist;

            try
            {
                using (ITransaction transaction = Session.BeginTransaction())
                {
                    Playlist playlistToCopy = PlaylistDao.Get(id);

                    if (playlistToCopy == null)
                    {
                        string errorMessage = string.Format("No playlist found with id: {0}", id);
                        throw new ApplicationException(errorMessage);
                    }

                    copiedPlaylist = new Playlist(playlistToCopy);
                    DoSave(copiedPlaylist);

                    transaction.Commit();
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }

            return copiedPlaylist;
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