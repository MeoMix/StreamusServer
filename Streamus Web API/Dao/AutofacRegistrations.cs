using System;
using Autofac;
using Autofac.Integration.WebApi;
using log4net;
using NHibernate;
using Streamus_Web_API.Domain.Interfaces;
using Streamus_Web_API.Domain.Managers;
using System.Reflection;
using System.Web.Http;

namespace Streamus_Web_API.Dao
{
    public class AutofacRegistrations
    {
        public static void RegisterAndSetResolver(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.MapHttpAttributeRoutes();

            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //  Per Design Patterns: Elements of Reusable Object-Oriented Software - an abstract factory is often used as a singleton.
            containerBuilder.Register(x => new NHibernateConfiguration().Configure().BuildSessionFactory()).SingleInstance();
            //containerBuilder.Register(x => new NHibernateDaoFactory()).As<IDaoFactory>().SingleInstance();
            //containerBuilder.RegisterType<StreamusManagerFactory>().As<IManagerFactory>().SingleInstance();

            //  TODO: Still not really sure why I need InstancePerApiRequest here. Figure it out! Maybe if I removed the need to pass params into ManagerFactory it'll be OK
            containerBuilder.RegisterType<NHibernateDaoFactory>().As<IDaoFactory>().InstancePerApiRequest();
            containerBuilder.RegisterType<StreamusManagerFactory>().As<IManagerFactory>().InstancePerApiRequest();

            //  Everything else wants an instance of Session per HTTP request, so indicate that:
            containerBuilder.Register(x => x.Resolve<ISessionFactory>().OpenSession()).InstancePerApiRequest();
            containerBuilder.Register(x => LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)).InstancePerApiRequest();
            
            ILifetimeScope container = containerBuilder.Build();

            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            httpConfiguration.EnsureInitialized();
        }
    }
}