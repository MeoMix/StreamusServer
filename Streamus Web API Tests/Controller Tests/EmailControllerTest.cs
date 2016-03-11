using System.Net.Http;
using System.Web.Http;
using Moq;
using NUnit.Framework;
using Streamus_Web_API.Controllers;
using Streamus_Web_API.Domain.Interfaces;
using Streamus_Web_API.Dto;

namespace Streamus_Web_API_Tests.Controller_Tests
{
  [TestFixture]
  public class EmailControllerTest : StreamusTest
  {
    private EmailController _emailController;

    [SetUp]
    public new void TestFixtureSetUp()
    {
      Mock<IManagerFactory> mockManagerFactory = new Mock<IManagerFactory>();
      Mock<IEmailManager> mockEmailManager = new Mock<IEmailManager>();
      mockManagerFactory.Setup(mf => mf.GetEmailManager()).Returns(mockEmailManager.Object);
      _emailController = new EmailController(Logger, Session, mockManagerFactory.Object);
    }

    [Test]
    public void SendEmail_ValidContactForm_EmailSent()
    {
      _emailController.Request = new HttpRequestMessage();
      _emailController.Request.SetConfiguration(new HttpConfiguration());
      var contactFormDto = new ContactDto("John Doe", "John.Doe@gmail.com", "Hello, world!");

      HttpResponseMessage response = _emailController.Create(contactFormDto);
      Assert.IsTrue(response.IsSuccessStatusCode);
    }
  }
}
