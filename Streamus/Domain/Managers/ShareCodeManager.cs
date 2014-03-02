using NHibernate;
using log4net;
using Streamus.Domain.Interfaces;
using System;

namespace Streamus.Domain.Managers
{
    public class ShareCodeManager : StreamusManager, IShareCodeManager
    {
        private IShareCodeDao ShareCodeDao { get; set; }

        public ShareCodeManager(ILog logger, ISession session, IShareCodeDao shareCodeDao)
            : base(logger, session)
        {
            ShareCodeDao = shareCodeDao;
        }

        public ShareCode GetByShortIdAndEntityTitle(string shareCodeShortId, string urlFriendlyEntityTitle)
        {
            ShareCode shareCode;

            try
            {
                using (ITransaction transaction = Session.BeginTransaction())
                {
                    shareCode = ShareCodeDao.GetByShortIdAndEntityTitle(shareCodeShortId, urlFriendlyEntityTitle);

                    if (shareCode == null)
                        throw new ApplicationException("Unable to locate shareCode in database.");

                    if (shareCode.EntityType != ShareableEntityType.Playlist)
                        throw new ApplicationException("Expected shareCode to have entityType of Playlist");

                    transaction.Commit();
                }
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
                using (ITransaction transaction = Session.BeginTransaction())
                {
                    shareCode = new ShareCode(shareableEntity);
                    DoSave(shareCode);
                    transaction.Commit();
                }
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
                using (ITransaction transaction = Session.BeginTransaction())
                {
                    DoSave(shareCode);
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
        ///     This is the work for saving a ShareCode without the Transaction wrapper.
        /// </summary>
        private void DoSave(ShareCode shareCode)
        {
            shareCode.ValidateAndThrow();
            ShareCodeDao.Save(shareCode);
        }
    }
}