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
    class ShareCodeRoutingTest : StreamusTest
    {
        private const string RoutePrefix = "http://localhost/ShareCode/";

        [Test]
        public void POST_error_Should_route_to_ErrorController_Create_method()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, RoutePrefix + "GetShareCode/" + Guid.NewGuid());
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);
            var route = Helpers.RouteRequest(config, request);

            route.Controller.Should().Be<ShareCodeController>();
            route.Action.Should().Be("GetShareCode");
        }
    }
}
