using log4net;
using NHibernate;
using Streamus.Domain.Interfaces;
using System;

namespace Streamus.Domain.Managers
{
    /// <summary>
    ///     Provides a common spot for methods against Errors which require transactions (Creating, Updating, Deleting)
    /// </summary>
    public class ErrorManager : StreamusManager, IErrorManager
    {
        private IErrorDao ErrorDao { get; set; }

        public ErrorManager(ILog logger, ISession session, IErrorDao errorDao)
            : base(logger, session)
        {
            ErrorDao = errorDao;
        }

        public void Save(Error error)
        {
            try
            {
                using (ITransaction transaction = Session.BeginTransaction())
                {
                    error.ValidateAndThrow();
                    ErrorDao.Save(error);

                    transaction.Commit();
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

    }
}