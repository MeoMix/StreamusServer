using log4net;
using NHibernate;
using NUnit.Framework;
using Streamus_Web_API;
using Streamus_Web_API.Domain.Interfaces;
using System.Web.Http;
using System.Web.Http.Dependencies;

namespace Streamus_Web_API_Tests
{
    public abstract class StreamusTest
    {
        protected ILog Logger;
        protected IDaoFactory DaoFactory;
        protected IManagerFactory ManagerFactory;
        protected Helpers Helpers;
        protected ISession Session;

        private IDependencyScope Scope;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            StreamusWebApi.InitializeApplication();
        }

        [SetUp]
        public void SetUp()
        {
            Scope = GlobalConfiguration.Configuration.DependencyResolver.BeginScope();

            Logger = (ILog)Scope.GetService(typeof(ILog));
            DaoFactory = (IDaoFactory)Scope.GetService(typeof(IDaoFactory));
            Session = (ISession)Scope.GetService(typeof(ISession));
            ManagerFactory = (IManagerFactory)Scope.GetService(typeof(IManagerFactory));

            Helpers = new Helpers(ManagerFactory);
        }

        [TearDown]
        public void TearDown()
        {
            Scope.Dispose(); 
        }
    }
}
