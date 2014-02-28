using log4net;
using Streamus.Domain;
using Streamus.Domain.Interfaces;
using Streamus.Dto;
using System.Web.Mvc;

namespace Streamus.Controllers
{
    [SessionManagement]
    public class ErrorController : AbstractController
    {
        private readonly IErrorManager ErrorManager;

        public ErrorController(ILog logger, IManagerFactory managerFactory)
            : base(logger)
        {
            ErrorManager = managerFactory.GetErrorManager();
        }

        [HttpPost, Throttle(Name = "ClientErrorThrottle", Message = "You must wait {n} seconds before accessing logging another error.", Seconds = 60)]
        public JsonResult Create(ErrorDto errorDto)
        {
            Error error = Error.Create(errorDto);
            ErrorManager.Save(error);

            ErrorDto savedErrorDto = ErrorDto.Create(error);

            return Json(savedErrorDto);
        }
    }
}
