using System.Web.Http;
using Streamus_Web_API.Domain;
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
            ClientClientErrorManager = managerFactory.GetErrorManager(session);
        }

        [Route("")]
        [HttpPost, Throttle(Name = "ClientErrorThrottle", Message = "You must wait {n} seconds before accessing logging another client error.", Seconds = 60)]
        public ClientErrorDto Create(ClientErrorDto clientErrorDto)
        {
            ClientErrorDto savedErrorDto;

            using (ITransaction transaction = Session.BeginTransaction())
            {
                ClientError clientError = new ClientError(clientErrorDto.InstanceId, clientErrorDto.Architecture, clientErrorDto.ClientVersion, clientErrorDto.LineNumber, clientErrorDto.BrowserVersion, clientErrorDto.Message, clientErrorDto.OperatingSystem, clientErrorDto.Url, clientErrorDto.Stack);
                ClientClientErrorManager.Save(clientError);

                savedErrorDto = ClientErrorDto.Create(clientError);

                transaction.Commit();
            }

            return savedErrorDto;
        }
    }
}
