using System.Net;
using System.Net.Http;
using System.Web.Http;
using Streamus_Web_API.Dto;
using log4net;
using NHibernate;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;

namespace Streamus_Web_API.Controllers
{
  [RoutePrefix("Email")]
  public class EmailController : StreamusController
  {
    private readonly IEmailManager _emailManager;

    public EmailController(ILog logger, ISession session, IManagerFactory managerFactory)
        : base(logger, session)
    {
      _emailManager = managerFactory.GetEmailManager();
    }

    [Route("")]
    [HttpPost, Throttle(Name = "EmailThrottle", Message = "Wait {n} seconds before sending another message.", Seconds = 60)]
    public HttpResponseMessage Create(ContactDto contactDto)
    {
      Email email = new Email($"Contact from: {contactDto.Name}", $"Email: {contactDto.Email} \n Message: {contactDto.Message}");
      _emailManager.SendEmail(email);

      return Request.CreateResponse(HttpStatusCode.NoContent);
    }
  }
}
