using log4net;
using Streamus.Domain.Interfaces;
using System;

namespace Streamus.Domain.Managers
{
    public class ShareCodeManager : AbstractManager, IShareCodeManager
    {
        private IPlaylistDao PlaylistDao { get; set; }
        private IShareCodeDao ShareCodeDao { get; set; }

        public ShareCodeManager(ILog logger, IPlaylistDao playlistDao, IShareCodeDao shareCodeDao)
            : base(logger)
        {
            PlaylistDao = playlistDao;
            ShareCodeDao = shareCodeDao;
        }

        public ShareCode GetByShortIdAndEntityTitle(string shareCodeShortId, string urlFriendlyEntityTitle)
        {
            ShareCode shareCode;

            try
            {
                shareCode = ShareCodeDao.GetByShortIdAndEntityTitle(shareCodeShortId, urlFriendlyEntityTitle);

                if (shareCode == null)
                    throw new ApplicationException("Unable to locate shareCode in database.");

                if (shareCode.EntityType != ShareableEntityType.Playlist)
                    throw new ApplicationException("Expected shareCode to have entityType of Playlist");
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }

            return shareCode;
        }

        public ShareCode GetShareCode(IShareableEntity shareableEntity)
        {
            ShareCode shareCode;

            try
            {
                shareCode = new ShareCode(shareableEntity);
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