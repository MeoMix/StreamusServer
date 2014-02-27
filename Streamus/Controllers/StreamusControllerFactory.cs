using Autofac;
using Streamus.Dao;
using Streamus.Domain.Interfaces;
using log4net;
using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace Streamus.Controllers
{
    public class StreamusControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            using (ILifetimeScope scope = AutofacRegistrations.Container.BeginLifetimeScope())
            {
                IDaoFactory daoFactory = scope.Resolve<IDaoFactory>();
                IManagerFactory managerFactory = scope.Resolve<IManagerFactory>();
                IController controller = Activator.CreateInstance(controllerType, new object[] {logger, daoFactory, managerFactory}) as Controller;

                return controller;
            }
        }
    }
}
