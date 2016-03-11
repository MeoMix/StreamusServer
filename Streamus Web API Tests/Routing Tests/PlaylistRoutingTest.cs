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
  public class PlaylistRoutingTest : StreamusTest
  {
    private const string RoutePrefix = "http://localhost/Playlist/";

    [Test]
    public void POST_Should_route_to_PlaylistController_Create_method()
    {
      var request = new HttpRequestMessage(HttpMethod.Post, RoutePrefix);
      var config = new HttpConfiguration();

      WebApiConfig.Register(config);
      var route = Helpers.RouteRequest(config, request);

      route.Controller.Should().Be<PlaylistController>();
      route.Action.Should().Be("Create");
    }

    [Test]
    public void GET_Should_route_to_PlaylistController_Get_method()
    {
      var request = new HttpRequestMessage(HttpMethod.Get, RoutePrefix + Guid.NewGuid());
      var config = new HttpConfiguration();

      WebApiConfig.Register(config);
      var route = Helpers.RouteRequest(config, request);

      route.Controller.Should().Be<PlaylistController>();
      route.Action.Should().Be("Get");
    }

    [Test]
    public void DELETE_Should_route_to_PlaylistController_Delete_method()
    {
      var request = new HttpRequestMessage(HttpMethod.Delete, RoutePrefix + Guid.NewGuid());
      var config = new HttpConfiguration();

      WebApiConfig.Register(config);
      var route = Helpers.RouteRequest(config, request);

      route.Controller.Should().Be<PlaylistController>();
      route.Action.Should().Be("Delete");
    }

    [Test]
    public void PATCH_Should_route_to_PlaylistController_Patch_method()
    {
      var request = new HttpRequestMessage(new HttpMethod("PATCH"), RoutePrefix + Guid.NewGuid());
      var config = new HttpConfiguration();

      WebApiConfig.Register(config);
      var route = Helpers.RouteRequest(config, request);

      route.Controller.Should().Be<PlaylistController>();
      route.Action.Should().Be("Patch");
    }

    [Test]
    public void POST_CreateCopyByShareCode_Should_route_to_PlaylistController_CreateCopyByShareCode_method()
    {
      var request = new HttpRequestMessage(HttpMethod.Post, RoutePrefix + "Copy/");
      var config = new HttpConfiguration();

      WebApiConfig.Register(config);
      var route = Helpers.RouteRequest(config, request);

      route.Controller.Should().Be<PlaylistController>();
      route.Action.Should().Be("Copy");
    }
  }
}
