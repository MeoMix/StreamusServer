using Streamus.Dao;
using Streamus.Domain.Interfaces;
using System;
using log4net;

namespace Streamus.Domain.Managers
{
    public class ShareCodeManager : AbstractManager, IShareCodeManager
    {
        private readonly IPlaylistManager PlaylistManager;

        private IPlaylistDao PlaylistDao { get; set; }
        private IShareCodeDao ShareCodeDao { get; set; }

        public ShareCodeManager(ILog logger, IPlaylistDao playlistDao, IShareCodeDao shareCodeDao, IPlaylistManager playlistManager)
            : base(logger)
        {
            PlaylistDao = playlistDao;
            ShareCodeDao = shareCodeDao;
            PlaylistManager = playlistManager;
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