using System.Web.Http;
using Streamus_Web_API.Domain.Interfaces;
using Streamus_Web_API.Dto;
using log4net;
using NHibernate;

namespace Streamus_Web_API.Controllers
{
    [RoutePrefix("ClientError")]
    public class ClientErrorController : StreamusController
    {
        private readonly IClientErrorManager ClientClientErrorManager;

        public ClientErrorController(ILog logger, ISession session, IManagerFactory managerFactory)
            : base(logger, session)
        {
            ClientClientErrorManager = managerFactory.GetErrorManager();
        }

        [Route("")]
        [HttpPost, Throttle(Name = "ClientErrorThrottle", Message = "You must wait {n} seconds before accessing logging another client error.", Seconds = 60)]
        public ClientErrorDto Create(ClientErrorDto clientErrorDto)
        {
            ClientErrorDto savedErrorDto;

            using (ITransaction transaction = Session.BeginTransaction())
            {
                Domain.ClientError clientError = new Domain.ClientError(clientErrorDto.Architecture, clientErrorDto.ClientVersion, clientErrorDto.LineNumber, clientErrorDto.Message, clientErrorDto.OperatingSystem, clientErrorDto.Url);
                ClientClientErrorManager.Save(clientError);

                savedErrorDto = ClientErrorDto.Create(clientError);

                transaction.Commit();
            }

            return savedErrorDto;
        }
    }
}
