using System.Net.Http;
using System.Web.Http;
using FluentAssertions;
using NUnit.Framework;
using Streamus_Web_API;
using Streamus_Web_API.Controllers;

namespace Streamus_Web_API_Tests.Routing_Tests
{
  [TestFixture]
  public class EmailRoutingTest : StreamusTest
  {
    private const string RoutePrefix = "http://localhost/Email/";

    [Test]
    public void Post_ContactFormDto_RouteToEmailControllerCreate()
    {
      var request = new HttpRequestMessage(HttpMethod.Post, RoutePrefix);
      var config = new HttpConfiguration();

      WebApiConfig.Register(config);
      var route = Helpers.RouteRequest(config, request);

      route.Controller.Should().Be<EmailController>();
      route.Action.Should().Be("Create");
    }
  }
}
