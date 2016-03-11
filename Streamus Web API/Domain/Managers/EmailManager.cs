using log4net;
using Streamus_Web_API.Domain.Interfaces;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;
using NHibernate;

namespace Streamus_Web_API.Domain.Managers
{
  public class EmailManager : StreamusManager, IEmailManager
  {
    public EmailManager(ILog logger)
        : base(logger)
    {
    }

    public void SendEmail(Email email)
    {
      try
      {
        MailAddress fromAddress = new MailAddress("admin@streamus.com", "Sean Anderson");
        MailAddress toAddress = new MailAddress("admin@streamus.com", "Sean Anderson");
        string password = ConfigurationManager.AppSettings["emailPassword"];

        SmtpClient smtpClient = new SmtpClient
        {
          Host = "smtp.gmail.com",
          Port = 587,
          EnableSsl = true,
          DeliveryMethod = SmtpDeliveryMethod.Network,
          UseDefaultCredentials = false,
          Credentials = new NetworkCredential(fromAddress.Address, password)
        };

        using (MailMessage mailMessage = new MailMessage(fromAddress, toAddress)
        {
          Subject = email.Subject,
          Body = email.Body
        })
        {
          smtpClient.Send(mailMessage);
        }
      }
      catch (Exception exception)
      {
        Logger.Error(exception);
        throw;
      }
    }
  }
}
