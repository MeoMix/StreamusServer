using System.Net.Http;
using Streamus_Web_API.Dao;
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

        private HttpRequestMessage HttpRequestMessage;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            StreamusWebApi.InitializeApplication();
        }

        [SetUp]
        public void SetUp()
        {
            HttpConfiguration httpConfiguration = new HttpConfiguration();
            AutofacRegistrations.RegisterAndSetResolver(httpConfiguration);

            HttpRequestMessage = new HttpRequestMessage();
            HttpRequestMessage.SetConfiguration(httpConfiguration);

            IDependencyScope dependencyScope = HttpRequestMessage.GetDependencyScope();

            Logger = (ILog)dependencyScope.GetService(typeof(ILog));
            DaoFactory = (IDaoFactory)dependencyScope.GetService(typeof(IDaoFactory));
            Session = (ISession)dependencyScope.GetService(typeof(ISession));
            ManagerFactory = (IManagerFactory)dependencyScope.GetService(typeof(IManagerFactory));

            Helpers = new Helpers(Session, ManagerFactory);
        }

        [TearDown]
        public void TearDown()
        {
            HttpRequestMessage.Dispose();
        }
    }
}
