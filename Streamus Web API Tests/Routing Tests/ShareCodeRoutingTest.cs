using FluentAssertions;
using NUnit.Framework;
using Streamus_Web_API.Controllers;
using System;
using System.Net.Http;
using System.Web.Http;
using Streamus_Web_API;

namespace Streamus_Web_API_Tests.Routing_Tests
{
  [TestFixture]
  class ShareCodeRoutingTest : StreamusTest
  {
    private const string RoutePrefix = "http://localhost/ShareCode/";

    [Test]
    public void GET_GetShareCode_Should_route_to_ShareCodeController_GetShareCode_method()
    {
      var request = new HttpRequestMessage(HttpMethod.Get, RoutePrefix + "GetShareCode/?playlistId=" + Guid.NewGuid());
      var config = new HttpConfiguration();

      WebApiConfig.Register(config);
      var route = Helpers.RouteRequest(config, request);

      route.Controller.Should().Be<ShareCodeController>();
      route.Action.Should().Be("GetShareCode");
    }

    [Test]
    public void GET_ShareCodeByShortIdAndEntityTitle_Should_route_to_ShareCodeController_GetShareCodeByShortIdAndEntityTitle_method()
    {
      var request = new HttpRequestMessage(HttpMethod.Get, RoutePrefix + "abc123/foo");
      var config = new HttpConfiguration();

      WebApiConfig.Register(config);
      var route = Helpers.RouteRequest(config, request);

      route.Controller.Should().Be<ShareCodeController>();
      route.Action.Should().Be("GetShareCodeByShortIdAndEntityTitle");
    }
  }
}
