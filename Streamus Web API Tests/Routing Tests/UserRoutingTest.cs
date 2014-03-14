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
    public class UserRoutingTest : StreamusTest
    {
        private const string RoutePrefix = "http://localhost/User/";

        [Test]
        public void POST_Should_route_to_UserController_Create_method()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, RoutePrefix);
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);
            var route = Helpers.RouteRequest(config, request);

            route.Controller.Should().Be<UserController>();
            route.Action.Should().Be("Create");
        }

        [Test]
        public void HttpGet_Should_route_to_UserController_Get_method()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, RoutePrefix + Guid.NewGuid());
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);
            var route = Helpers.RouteRequest(config, request);

            route.Controller.Should().Be<UserController>();
            route.Action.Should().Be("Get");
        }

        [Test]
        public void HttpGet_GetByGooglePlusId_Should_route_to_UserController_GetByGooglePlusId_method()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, RoutePrefix + "GetByGooglePlusId/" + "12345");
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);
            var route = Helpers.RouteRequest(config, request);

            route.Controller.Should().Be<UserController>();
            route.Action.Should().Be("GetByGooglePlusId");
        }

        [Test]
        public void HttpPatch_UpdateGooglePlusId_Should_route_to_UserController_UpdateGooglePlusId_method()
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), RoutePrefix + "UpdateGooglePlusId/");
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);
            var route = Helpers.RouteRequest(config, request);

            route.Controller.Should().Be<UserController>();
            route.Action.Should().Be("UpdateGooglePlusId");
        }
    }
}
