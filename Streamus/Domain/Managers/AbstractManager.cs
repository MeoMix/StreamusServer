using Autofac;
using log4net;
using Streamus.Dao;

namespace Streamus.Domain.Managers
{
    public abstract class AbstractManager
    {
        protected readonly ILog Logger;
        protected readonly ILifetimeScope Scope;

        protected AbstractManager(ILog logger)
        {
            AutofacRegistrations.RegisterDaoFactory();
            Scope = AutofacRegistrations.Container.BeginLifetimeScope();

            Logger = logger;
        }

    }
}