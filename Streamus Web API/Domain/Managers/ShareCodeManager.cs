using log4net;
using Streamus_Web_API.Domain.Interfaces;
using System;

namespace Streamus_Web_API.Domain.Managers
{
    public class ShareCodeManager : StreamusManager, IShareCodeManager
    {
        private IShareCodeDao ShareCodeDao { get; set; }

        public ShareCodeManager(ILog logger, IShareCodeDao shareCodeDao)
            : base(logger)
        {
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

                if (shareCode.EntityType != EntityType.Playlist)
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
                DoSave(shareCode);
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
