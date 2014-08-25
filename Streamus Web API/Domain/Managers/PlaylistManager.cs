using log4net;
using Streamus_Web_API.Domain.Interfaces;
using System;

namespace Streamus_Web_API.Domain.Managers
{
    public class PlaylistManager : StreamusManager, IPlaylistManager
    {
        private IPlaylistDao PlaylistDao { get; set; }

        public PlaylistManager(ILog logger, IPlaylistDao playlistDao)
            : base(logger)
        {
            PlaylistDao = playlistDao;
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

        public void Update(Playlist playlist)
        {
            try
            {
                playlist.ValidateAndThrow();
                PlaylistDao.Update(playlist);
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
                playlist.User.Playlists.Remove(playlist);
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
                Playlist playlist = Get(playlistId);
                playlist.Title = title;

                Update(playlist);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

        public void UpdateSequence(Guid playlistId, double seqeunce)
        {
            try
            {
                Playlist playlist = Get(playlistId);
                playlist.Sequence = seqeunce;

                Update(playlist);
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
                Playlist playlistToCopy = PlaylistDao.Get(id);

                if (playlistToCopy == null)
                {
                    throw new ApplicationException(string.Format("No playlist found with id: {0}", id));
                }

                copiedPlaylist = new Playlist(playlistToCopy);
                DoSave(copiedPlaylist);
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
                playlistItem.ValidateAndThrow();
            }

            playlist.ValidateAndThrow();
            PlaylistDao.Save(playlist);
        }
    }
}
