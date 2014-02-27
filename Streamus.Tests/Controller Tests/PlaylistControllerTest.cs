using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Streamus.Controllers;
using Streamus.Dao;
using Streamus.Domain;
using Streamus.Domain.Interfaces;
using Streamus.Dto;

namespace Streamus.Tests.Controller_Tests
{
    [TestFixture]
    public class PlaylistControllerTest : AbstractTest
    {
        private PlaylistController PlaylistController;
        private PlaylistItemController PlaylistItemController;
        private IShareCodeManager ShareCodeManager;
        private IUserDao UserDao;
        private Helpers Helpers;

        /// <summary>
        ///     This code is only ran once for the given TestFixture.
        /// </summary>
        [TestFixtureSetUp]
        public new void TestFixtureSetUp()
        {
            try
            {
                PlaylistController = new PlaylistController(Logger, DaoFactory, ManagerFactory);
                PlaylistItemController = new PlaylistItemController(Logger, DaoFactory, ManagerFactory);

                UserDao = DaoFactory.GetUserDao();

                IPlaylistDao playlistDao = DaoFactory.GetPlaylistDao();
                IShareCodeDao shareCodeDao = DaoFactory.GetShareCodeDao();
                IVideoDao videoDao = DaoFactory.GetVideoDao();
                IPlaylistManager playlistManager = ManagerFactory.GetPlaylistManager(playlistDao, videoDao);
                ShareCodeManager = ManagerFactory.GetShareCodeManager(playlistDao, shareCodeDao, playlistManager);

                Helpers = new Helpers(DaoFactory, ManagerFactory);
            }
            catch (TypeInitializationException exception)
            {
                throw exception.InnerException;
            }
        }

        [Test]
        public void DeletePlaylist_NextToBigPlaylist_NoStackOverflowException()
        {
            User user = Helpers.CreateUser();

            Guid firstPlaylistId = user.Playlists.First().Id;

            PlaylistDto playlistDto = Helpers.CreatePlaylistDto(user.Id);

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            var result = (JsonServiceStackResult) PlaylistController.Create(playlistDto);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();

            var createdPlaylistDto = (PlaylistDto) result.Data;

            const int numItemsToCreate = 150;
            List<PlaylistItemDto> playlistItemDtos = Helpers.CreatePlaylistItemsDto(numItemsToCreate, createdPlaylistDto.Id);

            foreach (var splitPlaylistItemDtos in Split(playlistItemDtos, 50))
            {
                NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
                PlaylistItemController.CreateMultiple(splitPlaylistItemDtos);
                NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();
            }

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            //  Now delete the first playlist.
            PlaylistController.Delete(firstPlaylistId);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();
        }

        public static List<List<PlaylistItemDto>> Split(List<PlaylistItemDto> source, int splitSize)
        {
            return source
                .Select((x, i) => new {Index = i, Value = x})
                .GroupBy(x => x.Index/splitSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        [Test]
        public void CreatePlaylist_PlaylistDoesntExist_PlaylistCreated()
        {
            User user = Helpers.CreateUser();
            PlaylistDto playlistDto = Helpers.CreatePlaylistDto(user.Id);

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            var result = (JsonServiceStackResult) PlaylistController.Create(playlistDto);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();

            var createdPlaylistDto = (PlaylistDto) result.Data;

            //  Make sure we actually get a Playlist DTO back from the Controller.
            Assert.NotNull(createdPlaylistDto);

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            User userFromDatabase = UserDao.Get(createdPlaylistDto.UserId);

            //  Make sure that the created playlist was cascade added to the User
            Assert.That(userFromDatabase.Playlists.Count(p => p.Id == createdPlaylistDto.Id) == 1);

            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();
        }

        [Test]
        public void GetSharedPlaylist_PlaylistShareCodeExists_CopyOfPlaylistCreated()
        {
            User user = Helpers.CreateUser();

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            ShareCode shareCode = ShareCodeManager.GetShareCode(ShareableEntityType.Playlist, user.Playlists.First().Id);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            //  Create a new playlist for the given user by loading up the playlist via sharecode.
            var result = (JsonServiceStackResult)PlaylistController.CreateCopyByShareCode(shareCode.ShortId, shareCode.UrlFriendlyEntityTitle, user.Id);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();

            var playlistDto = (PlaylistDto) result.Data;

            //  Make sure we actually get a Playlist DTO back from the Controller.
            Assert.NotNull(playlistDto);

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            User userFromDatabase = UserDao.Get(playlistDto.UserId);

            //  Make sure that the created playlist was cascade added to the User
            Assert.That(userFromDatabase.Playlists.Count(p => p.Id == playlistDto.Id) == 1);

            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();
        }
    }
}
