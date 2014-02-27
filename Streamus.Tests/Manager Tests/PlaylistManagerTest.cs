using System;
using NHibernate;
using NUnit.Framework;
using Streamus.Dao;
using Streamus.Domain;
using Streamus.Domain.Interfaces;

namespace Streamus.Tests.Manager_Tests
{
    [TestFixture]
    public class PlaylistManagerTest : AbstractTest
    {
        private IPlaylistDao PlaylistDao { get; set; }
        private IPlaylistManager PlaylistManager;

        private User User { get; set; }
        private Video Video { get; set; }
        private Helpers Helpers;

        /// <summary>
        ///     This code is only ran once for the given TestFixture.
        /// </summary>
        [TestFixtureSetUp]
        public new void TestFixtureSetUp()
        {
            IVideoManager videoManager;

            try
            {
                PlaylistDao = DaoFactory.GetPlaylistDao();

                IVideoDao videoDao = DaoFactory.GetVideoDao();
                PlaylistManager = ManagerFactory.GetPlaylistManager(PlaylistDao, videoDao);
                videoManager = ManagerFactory.GetVideoManager(videoDao);

                Helpers = new Helpers(DaoFactory, ManagerFactory);
            }
            catch (TypeInitializationException exception)
            {
                throw exception.InnerException;
            }

            User = Helpers.CreateUser();
            Video = Helpers.CreateUnsavedVideoWithId();

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            videoManager.Save(Video);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();
        }

        [Test]
        public void Updates()
        {
            Playlist playlist = User.CreateAndAddPlaylist();

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            PlaylistManager.Save(playlist);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            PlaylistManager.UpdateTitle(playlist.Id, "Existing Playlist 001");
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            Playlist playlistFromDatabase = PlaylistDao.Get(playlist.Id);
            //  Test that the product was successfully inserted
            Assert.IsNotNull(playlistFromDatabase);
            //  Sessions should be isolated -- before and after should be different here.
            Assert.AreNotEqual(playlist.Title, playlistFromDatabase.Title);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();
        }

        /// <summary>
        ///     Verifies that a Playlist can be deleted properly.
        /// </summary>
        [Test]
        public void DeletePlaylist()
        {
            //  Create a new Playlist and write it to the database.
            string title = string.Format("New Playlist {0:D4}", User.Playlists.Count);
            var playlist = new Playlist(title);

            User.AddPlaylist(playlist);

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            PlaylistManager.Save(playlist);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            //  Now delete the created Playlist and ensure it is removed.
            PlaylistManager.Delete(playlist.Id);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            Playlist deletedPlaylist = PlaylistDao.Get(playlist.Id);

            bool objectNotFoundExceptionEncountered = false;
            try
            {
                //  Evaluating a lazyily-loaded entity which isn't in the database will throw an ONF exception.
                Assert.IsNull(deletedPlaylist);
            }
            catch (ObjectNotFoundException)
            {
                objectNotFoundExceptionEncountered = true;
            }

            Assert.IsTrue(objectNotFoundExceptionEncountered);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();
        }
    }
}
