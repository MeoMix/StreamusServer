using System;
using NUnit.Framework;
using Streamus.Dao;
using Streamus.Domain;
using Streamus.Domain.Interfaces;

namespace Streamus.Tests.Manager_Tests
{
    [TestFixture]
    public class UserManagerTest : StreamusTest
    {
        private IUserDao UserDao { get; set; }

        /// <summary>
        ///     This code is only ran once for the given TestFixture.
        /// </summary>
        [TestFixtureSetUp]
        public new void TestFixtureSetUp()
        {
            try
            {
                UserDao = DaoFactory.GetUserDao();
            }
            catch (TypeInitializationException exception)
            {
                throw exception.InnerException;
            }
        }

        [Test]
        public void CreateUser_UserDoesntExist_UserCreated()
        {
            User user = Helpers.CreateUser();

            Assert.IsNotNull(user);
            Assert.IsNotEmpty(user.Playlists);
        }
    }
}