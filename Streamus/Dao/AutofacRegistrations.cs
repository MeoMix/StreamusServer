using Autofac;
using Autofac.Integration.Mvc;
using log4net;
using NHibernate;
using Streamus.Domain.Interfaces;
using Streamus.Domain.Managers;
using System.Reflection;
using System.Web.Mvc;

namespace Streamus.Dao
{
    public class AutofacRegistrations
    {
        public static void RegisterDaoFactory()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterControllers(Assembly.GetAssembly(typeof(Streamus)));
            containerBuilder.Register(x => NHibernateSessionManager.Instance.SessionFactory).SingleInstance();
            containerBuilder.Register(x => x.Resolve<ISessionFactory>().OpenSession()).InstancePerHttpRequest();
            containerBuilder.Register(x => x.Resolve<IManagerFactory>()).InstancePerHttpRequest();
            containerBuilder.Register(x => LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)).As<ILog>().InstancePerHttpRequest();

            containerBuilder.RegisterType<NHibernateDaoFactory>().As<IDaoFactory>();
            containerBuilder.RegisterType<StreamusManagerFactory>().As<IManagerFactory>();

            IContainer container = containerBuilder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}