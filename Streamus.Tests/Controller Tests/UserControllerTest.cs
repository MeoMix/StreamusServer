using NUnit.Framework;
using Streamus.Controllers;
using Streamus.Dao;
using Streamus.Domain;
using Streamus.Domain.Interfaces;
using Streamus.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Streamus.Tests.Controller_Tests
{
    [TestFixture]
    public class UserControllerTest : AbstractTest
    {
        private static readonly PlaylistItemController PlaylistItemController = new PlaylistItemController();
        private static readonly UserController UserController = new UserController();
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
        public void CreateUser_UserDoesNotExist_UserCreated()
        {
            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            JsonServiceStackResult result = (JsonServiceStackResult)UserController.Create();
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();

            UserDto createdUserDto = (UserDto)result.Data;

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            User userFromDatabase = UserDao.Get(createdUserDto.Id);
            Assert.That(userFromDatabase != null);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();
        }

        //  TODO: Test the response time of this and make sure it isn't a long running query for big playlists.
        [Test]
        public void GetUserWithBulkPlaylistItemsInFolder_UserCreatedWithLotsOfItems_UserHasOneFolderOnePlaylist()
        {
            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            JsonServiceStackResult result = (JsonServiceStackResult)UserController.Create();
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();

            UserDto createdUserDto = (UserDto)result.Data;

            const int numItemsToCreate = 2000;

            Guid playlistId = createdUserDto.Folders.First().Playlists.First().Id;
            List<PlaylistItemDto> playlistItemDtos = Helpers.CreatePlaylistItemsDto(numItemsToCreate, playlistId);

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            PlaylistItemController.CreateMultiple(playlistItemDtos);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();

            User userFromDatabase = UserDao.Get(createdUserDto.Id);

            Assert.That(userFromDatabase.Folders.Count == createdUserDto.Folders.Count);
            Assert.That(userFromDatabase.Folders.First().Playlists.Count == createdUserDto.Folders.First().Playlists.Count);
            Assert.That(userFromDatabase.Folders.First().Playlists.First().Items.Count() == numItemsToCreate);

            //  Different sessions -- first should be de-synced from the second.
            Assert.That(userFromDatabase.Folders.First().Playlists.First().Items.Count() != createdUserDto.Folders.First().Playlists.First().Items.Count());
        }
        
        //  TODO: GooglePlusID should be immutable.
        [Test]
        public void UpdateUserGooglePlusId_NoGooglePlusIdSet_GooglePlusIdSetSuccessfully()
        {
            const string googlePlusId = "109695597859594825120";

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            JsonServiceStackResult result = (JsonServiceStackResult)UserController.Create();
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();

            UserDto createdUserDto = (UserDto)result.Data;

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            UserController.UpdateGooglePlusId(createdUserDto.Id, googlePlusId);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            User userFromDatabase = UserDao.Get(createdUserDto.Id);

            Assert.That(userFromDatabase.Folders.Count == createdUserDto.Folders.Count);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();
        }
    }
}
