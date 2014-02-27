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
        protected Helpers Helpers;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            //  Initialize AutoMapper and AutoFac.
            Streamus.InitializeApplication();
            Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            using (ILifetimeScope scope = AutofacRegistrations.Container.BeginLifetimeScope())
            {
                DaoFactory = scope.Resolve<IDaoFactory>();
                ManagerFactory = scope.Resolve<IManagerFactory>(new NamedParameter("logger", Logger));
            }

            Helpers = new Helpers(ManagerFactory);
        }
    }
}
