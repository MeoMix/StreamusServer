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
        public static void RegisterAndSetResolver()
        {
            // Create the container builder.
            var containerBuilder = new ContainerBuilder();

            // Register the Web API controllers.
            containerBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //  Only generate one SessionFactory ever because it is expensive.
            containerBuilder.Register(x => new NHibernateConfiguration().Configure().BuildSessionFactory()).SingleInstance();

            //  Everything else wants an instance of Session per HTTP request, so indicate that:
            containerBuilder.Register(x => x.Resolve<ISessionFactory>().OpenSession()).InstancePerApiRequest();
            containerBuilder.Register(x => LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)).InstancePerApiRequest();

            containerBuilder.RegisterType<NHibernateDaoFactory>().As<IDaoFactory>().InstancePerApiRequest();
            containerBuilder.RegisterType<StreamusManagerFactory>().As<IManagerFactory>().InstancePerApiRequest();

            // Build the container.
            ILifetimeScope container = containerBuilder.Build();

            // Create the depenedency resolver.
            var dependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Configure Web API with the dependency resolver.
            GlobalConfiguration.Configuration.DependencyResolver = dependencyResolver;
        }
    }
}