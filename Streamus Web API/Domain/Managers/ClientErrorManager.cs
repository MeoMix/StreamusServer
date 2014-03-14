using log4net;
using Streamus_Web_API.Domain.Interfaces;
using System;

namespace Streamus_Web_API.Domain.Managers
{
    /// <summary>
    ///     Provides a common spot for methods against ClientErrors which require transactions (Creating, Updating, Deleting)
    /// </summary>
    public class ClientErrorManager : StreamusManager, IClientErrorManager
    {
        private IClientErrorDao ClientErrorDao { get; set; }

        public ClientErrorManager(ILog logger, IClientErrorDao clientErrorDao)
            : base(logger)
        {
            ClientErrorDao = clientErrorDao;
        }

        public void Save(ClientError clientError)
        {
            try
            {
                clientError.ValidateAndThrow();
                ClientErrorDao.Save(clientError);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

    }
}
