using log4net;
using NHibernate;
using NUnit.Framework;
using Streamus_Web_API;
using Streamus_Web_API.Domain.Interfaces;
using System.Web.Http;

namespace Streamus_Web_API_Tests
{
    public abstract class StreamusTest
    {
        protected ILog Logger;
        protected IDaoFactory DaoFactory;
        protected IManagerFactory ManagerFactory;
        protected Helpers Helpers;
        protected ISession Session;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            //  Initialize AutoMapper and AutoFac.
            StreamusWebApi.InitializeApplication();

            using (var scope = GlobalConfiguration.Configuration.DependencyResolver.BeginScope())
            {
                //  TODO: Consider initializing Helpers during setup to keep this DRY.
                Logger = (ILog)scope.GetService(typeof(ILog));
                DaoFactory = (IDaoFactory)scope.GetService(typeof(IDaoFactory));
                Session = (ISession)scope.GetService(typeof(ISession));
                ManagerFactory = (IManagerFactory)scope.GetService(typeof(IManagerFactory));
            }

            Helpers = new Helpers(ManagerFactory);
        }

        [SetUp]
        public void SetUp()
        {
            using (var scope = GlobalConfiguration.Configuration.DependencyResolver.BeginScope())
            {
                //  TODO: Consider initializing Helpers during setup to keep this DRY.
                Logger = (ILog)scope.GetService(typeof(ILog));
                DaoFactory = (IDaoFactory)scope.GetService(typeof(IDaoFactory));
                Session = (ISession)scope.GetService(typeof(ISession));
                ManagerFactory = (IManagerFactory)scope.GetService(typeof(IManagerFactory));
            }
        }

        [TearDown]
        public void TearDown()
        {
        }
    }
}
