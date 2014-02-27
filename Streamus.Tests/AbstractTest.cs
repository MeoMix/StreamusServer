using Autofac;
using log4net;
using NUnit.Framework;
using Streamus.Dao;
using Streamus.Domain.Interfaces;
using System.Reflection;

namespace Streamus.Tests
{
    public abstract class AbstractTest
    {
        protected ILog Logger;
        protected IDaoFactory DaoFactory;
        protected IManagerFactory ManagerFactory;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            //  Initialize Autofac for dependency injection.
            AutofacRegistrations.RegisterDaoFactory();

            using (ILifetimeScope scope = AutofacRegistrations.Container.BeginLifetimeScope())
            {
                DaoFactory = scope.Resolve<IDaoFactory>();
                ManagerFactory = scope.Resolve<IManagerFactory>(new NamedParameter("logger", Logger));
            }

            Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            //  Initialize AutoMapper with Streamus' server mappings.
            Streamus.InitializeApplication();
        }
    }
}
