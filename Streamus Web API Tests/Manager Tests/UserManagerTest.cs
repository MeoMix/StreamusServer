using System.Linq;
using NHibernate;
using NUnit.Framework;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;

namespace Streamus_Web_API_Tests.Tests.Manager_Tests
{
    [TestFixture]
    public class UserManagerTest : StreamusTest
    {
        private IUserManager UserManager;

        /// <summary>
        ///     This code runs before every test.
        /// </summary>
        [SetUp]
        public void SetupContext()
        {
            UserManager = ManagerFactory.GetUserManager(Session);
        }

        [Test]
        public void CreateUser_UserDoesntExist_UserCreated()
        {
            User createdUser;
            using (ITransaction transaction = Session.BeginTransaction())
            {
                createdUser = UserManager.CreateUser();
                transaction.Commit();
            }

            Session.Clear();
            User user = UserManager.Get(createdUser.Id);

            Assert.IsNotNull(user);
            Assert.IsNotEmpty(user.Playlists);
        }

        [Test]
        public void CreateUser_UserDoesntExist_PlaylistSequenceSetCorrectly()
        {
            User createdUser;
            using (ITransaction transaction = Session.BeginTransaction())
            {
                createdUser = UserManager.CreateUser();
                transaction.Commit();
            }

            Session.Clear();
            User user = UserManager.Get(createdUser.Id);

            Assert.IsNotNull(user);
            Assert.IsNotEmpty(user.Playlists);
            Assert.AreEqual(user.Playlists.Count, 1);
            Assert.AreEqual(user.Playlists.First().Sequence, 10000);
        }

        [Test]
        public void CreateUser_GetByGooglePlusId_UserReturned()
        {
            string googlePlusId = Helpers.GetRandomGooglePlusId();

            using (ITransaction transaction = Session.BeginTransaction())
            {
                UserManager.CreateUser(googlePlusId);
                transaction.Commit();
            }

            Session.Clear();

            User user = UserManager.GetByGooglePlusId(googlePlusId);

            Assert.NotNull(user);
        }

        [Test]
        public void MergeAllByGooglePlusId_OneOtherAccountExists_PlaylistsMergedCorrectly()
        {
            string googlePlusId = Helpers.GetRandomGooglePlusId();

            using (ITransaction transaction = Session.BeginTransaction())
            {
                User firstUser = UserManager.CreateUser(googlePlusId);
                Helpers.CreateItemInPlaylist(firstUser.CreateAndAddPlaylist());

                User secondUser = UserManager.CreateUser(googlePlusId);
                Helpers.CreateItemInPlaylist(secondUser.CreateAndAddPlaylist());

                transaction.Commit();
            }

            Session.Clear();

            using (ITransaction transaction = Session.BeginTransaction())
            {
                UserManager.MergeAllByGooglePlusId(googlePlusId);
                transaction.Commit();
            }

            Session.Clear();

            User mergedUser = UserManager.GetByGooglePlusId(googlePlusId);
            Assert.AreEqual(mergedUser.Playlists.Count, 3);
        }
    }
}