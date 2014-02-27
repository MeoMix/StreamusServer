using Autofac;
using Streamus.Domain.Interfaces;
using Streamus.Domain.Managers;

namespace Streamus.Dao
{
    public class AutofacRegistrations
    {
        public static IContainer Container { get; private set; }

        public static void RegisterDaoFactory()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<NHibernateDaoFactory>().As<IDaoFactory>();
            containerBuilder.RegisterType<StreamusManagerFactory>().As<IManagerFactory>();
            Container = containerBuilder.Build();
        }
    }
}