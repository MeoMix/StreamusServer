using System;
using Streamus.Domain.Interfaces;

namespace Streamus.Domain.Managers
{
    public class ShareCodeManager : AbstractManager
    {
        private static readonly PlaylistManager PlaylistManager = new PlaylistManager();

        private IPlaylistDao PlaylistDao { get; set; }
        private IShareCodeDao ShareCodeDao { get; set; }

        public ShareCodeManager()
        {
            PlaylistDao = DaoFactory.GetPlaylistDao();
            ShareCodeDao = DaoFactory.GetShareCodeDao();
        }

        public ShareCode GetShareCode(ShareableEntityType entityType, Guid entityId)
        {
            //  TODO: Support sharing other entities.
            if (entityType != ShareableEntityType.Playlist)
                throw new NotSupportedException("Only Playlist entityType can be shared currently.");

            ShareCode shareCode;

            try
            {
                Playlist playlistToCopy = PlaylistDao.Get(entityId);

                if (playlistToCopy == null)
                {
                    string errorMessage = string.Format("No playlist found with id: {0}", entityId);
                    throw new ApplicationException(errorMessage);
                }

                var shareablePlaylistCopy = new Playlist(playlistToCopy);
                PlaylistManager.Save(shareablePlaylistCopy);

                shareCode = new ShareCode(shareablePlaylistCopy);
                Save(shareCode);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }

            return shareCode;
        }

        public void Save(ShareCode shareCode)
        {
            try
            {
                DoSave(shareCode);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

        /// <summary>
        ///     This is the work for saving a ShareCode without the Transaction wrapper.
        /// </summary>
        private void DoSave(ShareCode shareCode)
        {
            shareCode.ValidateAndThrow();
            ShareCodeDao.Save(shareCode);
        }
    }
}