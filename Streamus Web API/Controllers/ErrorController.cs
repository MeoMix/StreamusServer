using System.Web.Http;
using Streamus_Web_API.Domain.Interfaces;
using Streamus_Web_API.Dto;
using log4net;
using NHibernate;

namespace Streamus_Web_API.Controllers
{
    [RoutePrefix("Error")]
    public class ErrorController : StreamusController
    {
        private readonly IErrorManager ErrorManager;

        public ErrorController(ILog logger, ISession session, IManagerFactory managerFactory)
            : base(logger, session)
        {
            ErrorManager = managerFactory.GetErrorManager();
        }

        [Route("")]
        [HttpPost, Throttle(Name = "ClientErrorThrottle", Message = "You must wait {n} seconds before accessing logging another error.", Seconds = 60)]
        public ErrorDto Create(ErrorDto errorDto)
        {
            ErrorDto savedErrorDto;

            using (ITransaction transaction = Session.BeginTransaction())
            {
                Domain.Error error = new Domain.Error(errorDto.Architecture, errorDto.ClientVersion, errorDto.LineNumber, errorDto.Message, errorDto.OperatingSystem, errorDto.Url);
                ErrorManager.Save(error);

                savedErrorDto = ErrorDto.Create(error);

                transaction.Commit();
            }

            return savedErrorDto;
        }
    }
}
