using log4net;
using NHibernate;
using NUnit.Framework;
using Streamus.Domain.Interfaces;
using Subtext.TestLibrary;
using System.Web.Mvc;

namespace Streamus.Tests
{
    public abstract class StreamusTest
    {
        protected ILog Logger;
        protected IDaoFactory DaoFactory;
        protected IManagerFactory ManagerFactory;
        protected Helpers Helpers;
        protected ISession Session;

        private HttpSimulator HttpSimulator;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            using (var httpSimulator = new HttpSimulator().SimulateRequest())
            {
                //  Initialize AutoMapper and AutoFac.
                Streamus.InitializeApplication();

                Logger = DependencyResolver.Current.GetService<ILog>();
                DaoFactory = DependencyResolver.Current.GetService<IDaoFactory>();
                Session = DependencyResolver.Current.GetService<ISession>();
                ManagerFactory = DependencyResolver.Current.GetService<IManagerFactory>();
            }

            Helpers = new Helpers(ManagerFactory);
        }

        [SetUp]
        public void SetUp()
        {
            HttpSimulator = new HttpSimulator().SimulateRequest();

            Logger = DependencyResolver.Current.GetService<ILog>();
            DaoFactory = DependencyResolver.Current.GetService<IDaoFactory>();
            Session = DependencyResolver.Current.GetService<ISession>();
            ManagerFactory = DependencyResolver.Current.GetService<IManagerFactory>();
        }

        [TearDown]
        public void TearDown()
        {
            HttpSimulator.Dispose();
        }
    }
}
