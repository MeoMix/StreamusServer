using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Configuration;
using Streamus_Web_API.Dao.Mappings;
using Configuration = NHibernate.Cfg.Configuration;
using Environment = NHibernate.Cfg.Environment;

namespace Streamus_Web_API.Dao
{
    public class NHibernateConfiguration
    {
        public FluentConfiguration Configure()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

            FluentConfiguration fluentConfiguration = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(connectionString).ShowSql().FormatSql())
                .Mappings(cfg => cfg.FluentMappings.AddFromAssemblyOf<UserMapping>())
                .ExposeConfiguration(ConfigureStreamusDataAccess);

            return fluentConfiguration;
        }

        private static void ConfigureStreamusDataAccess(Configuration configuration)
        {
            //  NHibernate.Context.WebSessionContext - analogous to ManagedWebSessionContext above, stores the current session in HttpContext. 
            //  You are responsible to bind and unbind an ISession instance with static methods of class CurrentSessionContext.
            configuration.SetProperty("current_session_context_class", "web");
            configuration.SetProperty("connection.isolation", "ReadUncommitted");
            configuration.SetProperty(Environment.CommandTimeout, TimeSpan.FromSeconds(15).TotalSeconds.ToString());
#if DEBUG
            configuration.SetProperty("default_schema", "[Streamus].[dbo]");
            configuration.SetProperty("generate_statistics", "true");
#else
            configuration.SetProperty("default_schema", "[dbo]");
#endif
        }
    }
}