using Autofac;
using Autofac.Integration.Mvc;
using Streamus.Domain.Interfaces;
using log4net;
using NHibernate;
using Streamus.Domain.Managers;
using System.Reflection;
using System.Web.Mvc;

namespace Streamus.Dao
{
    public class AutofacRegistrations
    {
        public static void RegisterAndSetResolver()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterControllers(Assembly.GetExecutingAssembly());

            //  Only generate one SessionFactory ever because it is expensive.
            containerBuilder.Register(x => new NHibernateConfiguration().Configure().BuildSessionFactory()).SingleInstance();

            //  Everything else wants an instance of Session per HTTP request, so indicate that:
            containerBuilder.Register(x => x.Resolve<ISessionFactory>().OpenSession()).InstancePerHttpRequest();
            containerBuilder.Register(x => LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)).InstancePerHttpRequest();

            containerBuilder.RegisterType<NHibernateDaoFactory>().As<IDaoFactory>().InstancePerHttpRequest();
            containerBuilder.RegisterType<StreamusManagerFactory>().As<IManagerFactory>().InstancePerHttpRequest();

            //  containerBuilder.RegisterModule adds all the required http modules to support per web request lifestyle and change default controller factory to the one that uses Autofac.
            containerBuilder.RegisterModule(new AutofacWebTypesModule());

            IContainer container = containerBuilder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}