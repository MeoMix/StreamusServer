using System.Configuration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System;
using System.Web;
using Streamus.Dao.Mappings;
using Configuration = NHibernate.Cfg.Configuration;

namespace Streamus.Dao
{
    /// <summary>
    ///     Handles creation and management of sessions and transactions.  It is a singleton because
    ///     building the initial session factory is very expensive. Inspiration for this class came
    ///     from Chapter 8 of Hibernate in Action by Bauer and King.  Although it is a sealed singleton
    ///     you can use TypeMock (http://www.typemock.com) for more flexible testing.
    /// </summary>
    public class NHibernateSessionManager
    {
        private readonly string ConnectionString;

        public ISessionFactory SessionFactory
        {
            get;
            private set;
        }

        //  http://csharpindepth.com/Articles/General/Singleton.aspx
        private static readonly Lazy<NHibernateSessionManager> Lazy = new Lazy<NHibernateSessionManager>(() => new NHibernateSessionManager(ConfigurationManager.ConnectionStrings["default"].ConnectionString));

        public static NHibernateSessionManager Instance { get { return Lazy.Value; } }

        /// <summary>
        ///     Initializes the NHibernate session factory upon instantiation.
        /// </summary>
        private NHibernateSessionManager(string connectionString)
        {
            ConnectionString = connectionString;
            InitializeSessionFactory();
        }

        //  http://www.piotrwalat.net/nhibernate-session-management-in-asp-net-web-api/
        private void InitializeSessionFactory()
        {
            FluentConfiguration fluentConfiguration = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(ConnectionString).ShowSql().FormatSql())
                .Mappings(cfg => cfg.FluentMappings.AddFromAssemblyOf<UserMapping>())
                .ExposeConfiguration(ConfigureStreamusDataAccess);

            SessionFactory = fluentConfiguration.BuildSessionFactory();
        }

        private static void ConfigureStreamusDataAccess(Configuration configuration)
        {
            if (HttpContext.Current != null)
            {
                //  NHibernate.Context.WebSessionContext - analogous to ManagedWebSessionContext above, stores the current session in HttpContext. 
                //  You are responsible to bind and unbind an ISession instance with static methods of class CurrentSessionContext.
                configuration.SetProperty("current_session_context_class", "web");
            }
            else
            {
                configuration.SetProperty("current_session_context_class", "call");
            }

            configuration.SetProperty("connection.isolation", "ReadUncommitted");
            //configuration.SetProperty("max_fetch_depth", "3");

#if DEBUG
            configuration.SetProperty("default_schema", "[Streamus].[dbo]");
            configuration.SetProperty("generate_statistics", "true");
#else
            configuration.SetProperty("default_schema", "[db896d0fe754cd4f46b3d0a2c301552bd6].[dbo]");
#endif
        }
    }
}