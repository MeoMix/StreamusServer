using NUnit.Framework;
using Streamus.Controllers;
using Streamus.Domain;
using Streamus.Domain.Interfaces;
using Streamus.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Streamus.Tests.Controller_Tests
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
        [TestFixtureSetUp]
        public new void TestFixtureSetUp()
        {
            try
            {
                PlaylistItemController = new PlaylistItemController(Logger, Session, ManagerFactory);
                UserController = new UserController(Logger, Session, ManagerFactory);
                UserManager = ManagerFactory.GetUserManager();
            }
            catch (TypeInitializationException exception)
            {
                throw exception.InnerException;
            }
        }

        [Test]
        public void CreateUser_UserDoesNotExist_UserCreated()
        {
            JsonResult result = UserController.Create();

            var createdUserDto = (UserDto) result.Data;

            User userFromDatabase = UserManager.Get(createdUserDto.Id);
            Assert.That(userFromDatabase != null);
        }

        //  TODO: Test the response time of this and make sure it isn't a long running query for big playlists.
        [Test]
        public void GetUserWithBulkPlaylistItems_UserCreatedWithLotsOfItems_UserHasOnePlaylist()
        {
            JsonResult result = UserController.Create();

            var createdUserDto = (UserDto) result.Data;

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

            JsonResult result = UserController.Create();

            var createdUserDto = (UserDto) result.Data;

            UserController.UpdateGooglePlusId(createdUserDto.Id, googlePlusId);

            User userFromDatabase = UserManager.Get(createdUserDto.Id);

            Assert.That(userFromDatabase.Playlists.Count == createdUserDto.Playlists.Count);
        }
    }
}
