using NUnit.Framework;
using Streamus.Controllers;
using Streamus.Dto;

namespace Streamus.Tests.Controller_Tests
{
    [TestFixture]
    public class ErrorControllerTest : AbstractTest
    {
        private static readonly ErrorController ErrorController = new ErrorController();

        /// <summary>
        ///     This code is only ran once for the given TestFixture.
        /// </summary>
        [TestFixtureSetUp]
        public new void TestFixtureSetUp()
        {
        }

        [Test]
        public void CreateError_ShortMessage_ErrorCreated()
        {
            ErrorDto errorDto = new ErrorDto
                {
                    Message = "Hello World",
                    ClientVersion = "0.99",
                    LineNumber = 2
                };

            JsonServiceStackResult result = (JsonServiceStackResult)ErrorController.Create(errorDto);
            ErrorDto createdErrorDto = (ErrorDto)result.Data;

            Assert.NotNull(createdErrorDto);
        }

        [Test]
        public void CreateError_LongMessage_MessageTruncatedErrorCreated()
        {
            ErrorDto errorDto = new ErrorDto
            {
                Message = "Hello World This is a Really Long Message Which is going to be over 255 characters in length when finished which will cause the end result message to be truncated with three periods as to not overflow the database. Can I confirm that this is happening? Thanks",
                ClientVersion = "0.99",
                LineNumber = 2
            };

            JsonServiceStackResult result = (JsonServiceStackResult)ErrorController.Create(errorDto);
            ErrorDto createdErrorDto = (ErrorDto)result.Data;

            Assert.NotNull(createdErrorDto);
            Assert.That(createdErrorDto.Message.Length == 255);
        }
    }
}
