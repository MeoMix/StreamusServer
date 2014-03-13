using System.Net.Http;
using System.Web.Http;
using FluentAssertions;
using NUnit.Framework;
using Streamus_Web_API.App_Start;
using Streamus_Web_API.Controllers;
using Streamus_Web_API.Dto;

namespace Streamus_Web_API_Tests.Controller
{
    [TestFixture]
    public class ErrorControllerTest : StreamusTest
    {
        private ErrorController ErrorController;

        [SetUp]
        public new void TestFixtureSetUp()
        {
            ErrorController = new ErrorController(Logger, Session, ManagerFactory);
        }

        [Test]
        public void POST_error_Should_route_to_ErrorController_Create_method()
        {
            // setups
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/Error/");
            var config = new HttpConfiguration();

            // act
            WebApiConfig.Register(config);
            var route = Helpers.RouteRequest(config, request);

            // asserts
            route.Controller.Should().Be<ErrorController>();
            route.Action.Should().Be("Create");
        }

        [Test]
        public void CreateError_ShortMessage_ErrorCreated()
        {
            var errorDto = new ErrorDto
                {
                    Message = "Hello World",
                    ClientVersion = "0.99",
                    LineNumber = 2
                };

            ErrorDto createdErrorDto = ErrorController.Create(errorDto);
            Assert.NotNull(createdErrorDto);
        }

        [Test]
        public void CreateError_LongMessage_MessageTruncatedErrorCreated()
        {
            var errorDto = new ErrorDto
                {
                    Message =
                        "Hello World This is a Really Long Message Which is going to be over 255 characters in length when finished which will cause the end result message to be truncated with three periods as to not overflow the database. Can I confirm that this is happening? Thanks",
                    ClientVersion = "0.99",
                    LineNumber = 2
                };

            ErrorDto createdErrorDto = ErrorController.Create(errorDto);

            Assert.NotNull(createdErrorDto);
            Assert.That(createdErrorDto.Message.Length == 255);
        }
    }
}