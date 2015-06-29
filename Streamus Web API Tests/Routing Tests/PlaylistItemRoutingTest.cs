using FluentAssertions;
using NUnit.Framework;
using Streamus_Web_API.App_Start;
using Streamus_Web_API.Controllers;
using System;
using System.Net.Http;
using System.Web.Http;

namespace Streamus_Web_API_Tests.Routing_Tests
{
    [TestFixture]
    public class PlaylistItemRoutingTest : StreamusTest
    {
        private const string RoutePrefix = "http://localhost/PlaylistItem/";

        [Test]
        public void POST_Should_route_to_PlaylistItemController_Create_method()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, RoutePrefix);
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);
            var route = Helpers.RouteRequest(config, request);

            route.Controller.Should().Be<PlaylistItemController>();
            route.Action.Should().Be("Create");
        }

        [Test]
        public void POST_CreateMultiple_Should_route_to_PlaylistItemController_CreateMultiple_method()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, RoutePrefix + "CreateMultiple/");
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);
            var route = Helpers.RouteRequest(config, request);

            route.Controller.Should().Be<PlaylistItemController>();
            route.Action.Should().Be("CreateMultiple");
        }

        [Test]
        public void PATCH_Should_route_to_PlaylistItemController_Patch_method()
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), RoutePrefix + Guid.NewGuid());
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);
            var route = Helpers.RouteRequest(config, request);

            route.Controller.Should().Be<PlaylistItemController>();
            route.Action.Should().Be("Patch");
        }

        [Test]
        public void DELETE_Should_route_to_PlaylistItemController_Delete_method()
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, RoutePrefix + Guid.NewGuid());
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);
            var route = Helpers.RouteRequest(config, request);

            route.Controller.Should().Be<PlaylistItemController>();
            route.Action.Should().Be("Delete");
        }
    }
}
