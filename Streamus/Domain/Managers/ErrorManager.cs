using Streamus.Domain.Interfaces;
using System;
using log4net;

namespace Streamus.Domain.Managers
{
    /// <summary>
    ///     Provides a common spot for methods against Errors which require transactions (Creating, Updating, Deleting)
    /// </summary>
    public class ErrorManager : AbstractManager, IErrorManager
    {
        private IErrorDao ErrorDao { get; set; }

        public ErrorManager(ILog logger, IErrorDao errorDao)
            : base(logger)
        {
            ErrorDao = errorDao;
        }

        public void Save(Error error)
        {
            try
            {
                error.ValidateAndThrow();
                ErrorDao.Save(error);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

    }
}