using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;
using Streamus.Controllers;
using Streamus.Dao;
using Streamus.Domain;
using Streamus.Domain.Interfaces;
using Streamus.Dto;

namespace Streamus.Tests.Controller_Tests
{
    [TestFixture]
    public class UserControllerTest : AbstractTest
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
                PlaylistItemController = new PlaylistItemController(Logger, ManagerFactory);
                UserController = new UserController(Logger, ManagerFactory);
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
            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            JsonResult result = UserController.Create();
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();

            var createdUserDto = (UserDto) result.Data;

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            User userFromDatabase = UserManager.Get(createdUserDto.Id);
            Assert.That(userFromDatabase != null);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();
        }

        //  TODO: Test the response time of this and make sure it isn't a long running query for big playlists.
        [Test]
        public void GetUserWithBulkPlaylistItems_UserCreatedWithLotsOfItems_UserHasOnePlaylist()
        {
            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            JsonResult result = UserController.Create();
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();

            var createdUserDto = (UserDto) result.Data;

            const int numItemsToCreate = 2000;

            Guid playlistId = createdUserDto.Playlists.First().Id;
            List<PlaylistItemDto> playlistItemDtos = Helpers.CreatePlaylistItemsDto(numItemsToCreate, playlistId);

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            PlaylistItemController.CreateMultiple(playlistItemDtos);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();

            User userFromDatabase = UserManager.Get(createdUserDto.Id);

            Assert.That(userFromDatabase.Playlists.Count == createdUserDto.Playlists.Count);
            Assert.That(userFromDatabase.Playlists.First().Items.Count() == numItemsToCreate);

            //  Different sessions -- first should be de-synced from the second.
            Assert.That(userFromDatabase.Playlists.First().Items.Count() !=
                        createdUserDto.Playlists.First().Items.Count());
        }

        //  TODO: GooglePlusID should be immutable.
        [Test]
        public void UpdateUserGooglePlusId_NoGooglePlusIdSet_GooglePlusIdSetSuccessfully()
        {
            const string googlePlusId = "109695597859594825120";

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            JsonResult result = UserController.Create();
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();

            var createdUserDto = (UserDto) result.Data;

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            UserController.UpdateGooglePlusId(createdUserDto.Id, googlePlusId);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            User userFromDatabase = UserManager.Get(createdUserDto.Id);

            Assert.That(userFromDatabase.Playlists.Count == createdUserDto.Playlists.Count);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();
        }
    }
}
