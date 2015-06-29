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
            UserManager = ManagerFactory.GetUserManager(Session);
        }

        //  TODO: Test case for user w/ GooglePlusID

        [Test]
        public void CreateUser_UserDoesNotExist_UserCreated()
        {
            var createdUserDto = UserController.Create(new UserDto());

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
            var createdUserDto = UserController.Create(new UserDto());
            
            const int numItemsToCreate = 2000;

            Guid playlistId = createdUserDto.Playlists.First().Id;
            List<PlaylistItemDto> playlistItemDtos = Helpers.CreatePlaylistItemsDto(numItemsToCreate, playlistId);

            PlaylistItemController.CreateMultiple(playlistItemDtos);

            Session.Clear();

            User userFromDatabase = UserManager.Get(createdUserDto.Id);

            Assert.That(userFromDatabase.Playlists.Count == createdUserDto.Playlists.Count);
            Assert.That(userFromDatabase.Playlists.First().Items.Count() == numItemsToCreate);
        }

        [Test]
        public void UpdateUserGooglePlusId_NoGooglePlusIdSet_GooglePlusIdSetSuccessfully()
        {
            string googlePlusId = Helpers.GetRandomGooglePlusId();
                                         
            var createdUserDto = UserController.Create(new UserDto
                {
                    GooglePlusId = googlePlusId
                });

            User userFromDatabase = UserManager.Get(createdUserDto.Id);

            Assert.That(userFromDatabase.Playlists.Count == createdUserDto.Playlists.Count);
        }

        [Test]
        public void HasLinkedGoogleAccount_GoogleIdDoesNotExist_ReturnsFalse()
        {
            string googlePlusId = Helpers.GetRandomGooglePlusId();

            bool hasLinkedGooglePlusId = UserController.HasLinkedGoogleAccount(googlePlusId);

            Assert.That(hasLinkedGooglePlusId == false);
        }

        [Test]
        public void PatchUser_GooglePlusIdProvided_GooglePlusIdModified()
        {
            User user = Helpers.CreateUser();
            string newGooglePlusId = Helpers.GetRandomGooglePlusId();

            UserDto userDto = new UserDto {GooglePlusId = newGooglePlusId};
            UserController.Patch(user.Id, userDto);

            Assert.AreEqual(user.GooglePlusId, newGooglePlusId);
        }

        [Test]
        public void MergeUser_TwoAccountsExist_DataMerged()
        {
            User user = Helpers.CreateUser();
            user.CreateAndAddPlaylist();
            user.GooglePlusId = Helpers.GetRandomGooglePlusId();
            UserManager.Update(user);

            User newUser = Helpers.CreateUser();
            Playlist createdPlaylist = newUser.CreateAndAddPlaylist();
            Helpers.CreateItemInPlaylist(createdPlaylist);

            UserDto mergedUserDto = UserController.MergeByGooglePlusId(newUser.Id, user.GooglePlusId);

            Assert.AreEqual(mergedUserDto.Playlists.Count, 3);
        }
    }
}