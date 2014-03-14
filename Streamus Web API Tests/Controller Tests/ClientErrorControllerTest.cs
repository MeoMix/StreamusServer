using NUnit.Framework;
using Streamus_Web_API.Controllers;
using Streamus_Web_API.Dto;

namespace Streamus_Web_API_Tests.Controller
{
    [TestFixture]
    public class ClientErrorControllerTest : StreamusTest
    {
        private ClientErrorController ClientErrorController;

        [SetUp]
        public new void TestFixtureSetUp()
        {
            ClientErrorController = new ClientErrorController(Logger, Session, ManagerFactory);
        }

        [Test]
        public void CreateError_ShortMessage_ErrorCreated()
        {
            var clientErrorDto = new ClientErrorDto
                {
                    Message = "Hello World",
                    ClientVersion = "0.99",
                    LineNumber = 2
                };

            ClientErrorDto createdErrorDto = ClientErrorController.Create(clientErrorDto);
            Assert.NotNull(createdErrorDto);
        }

        [Test]
        public void CreateError_LongMessage_MessageTruncatedErrorCreated()
        {
            var clientErrorDto = new ClientErrorDto
                {
                    Message =
                        "Hello World This is a Really Long Message Which is going to be over 255 characters in length when finished which will cause the end result message to be truncated with three periods as to not overflow the database. Can I confirm that this is happening? Thanks",
                    ClientVersion = "0.99",
                    LineNumber = 2
                };

            ClientErrorDto createdErrorDto = ClientErrorController.Create(clientErrorDto);

            Assert.NotNull(createdErrorDto);
            Assert.That(createdErrorDto.Message.Length == 255);
        }
    }
}