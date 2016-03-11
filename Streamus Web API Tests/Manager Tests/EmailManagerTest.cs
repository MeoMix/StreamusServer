using System;
using System.Web;
using NUnit.Framework;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;

namespace Streamus_Web_API_Tests.Manager_Tests
{
  [TestFixture]
  public class EmailManagerTest : StreamusTest
  {
    private IEmailManager _emailManager;

    /// <summary>
    ///     This code runs before every test.
    /// </summary>
    [SetUp]
    public void SetupContext()
    {
      _emailManager = ManagerFactory.GetEmailManager();
      HttpContext.Current = new HttpContext(new HttpRequest(null, "http://tempuri.org", null), new HttpResponse(null));
    }

    /// <summary>
    ///     Send an email and ensure it was sent without exception.
    /// </summary>
    [Test]
    public void SendEmail_EmailIsValid_EmailSent()
    {
      bool exceptionEncountered = false;

      try
      {
        _emailManager.SendEmail(new Email("Hello", "World"));
      }
      catch (Exception)
      {
        exceptionEncountered = true;
      }

      Assert.IsFalse(exceptionEncountered);
    }
  }
}
