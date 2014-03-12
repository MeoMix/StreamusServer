using NUnit.Framework;
using Streamus_Web_API.Controllers;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;
using Streamus_Web_API.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Streamus_Web_API_Tests.Controller
{
    [TestFixture]
    public class UserControllerTest : StreamusTest
    {
        private PlaylistItemController PlaylistItemController;
        private UserController UserController;
        private IUserManager UserManager;

        /// <summary>
        ///     This code is only ran once for the given TestFixture.
        /// </summary>
        [SetUp]
        public new void TestFixtureSetUp()
        {
            PlaylistItemController = new PlaylistItemController(Logger, Session, ManagerFactory);
            UserController = new UserController(Logger, Session, ManagerFactory);
            UserManager = ManagerFactory.GetUserManager();
        }

        [Test]
        public void CreateUser_UserDoesNotExist_UserCreated()
        {
            var createdUserDto = UserController.Create();

            User userFromDatabase = UserManager.Get(createdUserDto.Id);
            Assert.That(userFromDatabase != null);
        }

        /// <summary>
        /// Ensure that graceful fallback occurs if the database glitches out and doesn't have a user.
        /// </summary>
        [Test]
        public void GeteUser_UserDoesNotExist_UserCreated()
        {
            Guid guid = Guid.NewGuid();

            var createdUserDto = UserController.Get(guid);

            User userFromDatabase = UserManager.Get(createdUserDto.Id);
            Assert.That(userFromDatabase != null);
            Assert.AreNotEqual(guid, userFromDatabase.Id);
        }

        //  TODO: Test the response time of this and make sure it isn't a long running query for big playlists.
        [Test]
        public void GetUserWithBulkPlaylistItems_UserCreatedWithLotsOfItems_UserHasOnePlaylist()
        {
            var createdUserDto = UserController.Create();

            const int numItemsToCreate = 2000;

            Guid playlistId = createdUserDto.Playlists.First().Id;
            List<PlaylistItemDto> playlistItemDtos = Helpers.CreatePlaylistItemsDto(numItemsToCreate, playlistId);

            PlaylistItemController.CreateMultiple(playlistItemDtos);

            User userFromDatabase = UserManager.Get(createdUserDto.Id);

            Assert.That(userFromDatabase.Playlists.Count == createdUserDto.Playlists.Count);
            Assert.That(userFromDatabase.Playlists.First().Items.Count() == numItemsToCreate);
        }

        //  TODO: GooglePlusID should be immutable.
        [Test]
        public void UpdateUserGooglePlusId_NoGooglePlusIdSet_GooglePlusIdSetSuccessfully()
        {
            const string googlePlusId = "109695597859594825120";

            var createdUserDto = UserController.Create();
            createdUserDto.GooglePlusId = googlePlusId;

            UserController.UpdateGooglePlusId(createdUserDto);

            User userFromDatabase = UserManager.Get(createdUserDto.Id);

            Assert.That(userFromDatabase.Playlists.Count == createdUserDto.Playlists.Count);
        }
    }
}