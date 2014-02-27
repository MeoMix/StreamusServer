using log4net;
using Streamus.Domain;
using Streamus.Domain.Interfaces;
using Streamus.Dto;
using System.Web.Mvc;

namespace Streamus.Controllers
{
    [SessionManagement]
    public class ErrorController : Controller
    {
        private readonly ILog Logger;
        private readonly IErrorManager ErrorManager;

        public ErrorController(ILog logger, IDaoFactory daoFactory, IManagerFactory managerFactory)
        {
            Logger = logger;
            IErrorDao errorDao = daoFactory.GetErrorDao();

            ErrorManager = managerFactory.GetErrorManager(errorDao);
        }

        [HttpPost, Throttle(Name = "ClientErrorThrottle", Message = "You must wait {n} seconds before accessing logging another error.", Seconds = 60)]
        public ActionResult Create(ErrorDto errorDto)
        {
            Error error = Error.Create(errorDto);
            ErrorManager.Save(error);

            ErrorDto savedErrorDto = ErrorDto.Create(error);

            return new JsonServiceStackResult(savedErrorDto);
        }
    }
}
