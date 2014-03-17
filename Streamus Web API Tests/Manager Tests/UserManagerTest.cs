using System.Linq;
using NUnit.Framework;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;

namespace Streamus_Web_API_Tests.Tests.Manager_Tests
{
    [TestFixture]
    public class UserManagerTest : StreamusTest
    {
        private IUserDao UserDao { get; set; }

        /// <summary>
        ///     This code is only ran once for the given TestFixture.
        /// </summary>
        [SetUp]
        public new void TestFixtureSetUp()
        {
            UserDao = DaoFactory.GetUserDao();
        }

        [Test]
        public void CreateUser_UserDoesntExist_UserCreated()
        {
            User user = Helpers.CreateUser();

            Assert.IsNotNull(user);
            Assert.IsNotEmpty(user.Playlists);
        }

        [Test]
        public void CreateUser_UserDoesntExist_PlaylistSequenceSetCorrectly()
        {
            User user = Helpers.CreateUser();

            Assert.IsNotNull(user);
            Assert.IsNotEmpty(user.Playlists);
            Assert.AreEqual(user.Playlists.Count, 1);
            Assert.AreEqual(user.Playlists.First().Sequence, 10000);
        }
    }
}