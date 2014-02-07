using System;
using System.Linq;
using NHibernate;
using NUnit.Framework;
using Streamus.Dao;
using Streamus.Domain;
using Streamus.Domain.Interfaces;
using Streamus.Domain.Managers;

namespace Streamus.Tests.Manager_Tests
{
    [TestFixture]
    public class PlaylistManagerTest : AbstractTest
    {
        private IPlaylistDao PlaylistDao { get; set; }
        private User User { get; set; }
        private Folder Folder { get; set; }
        private Video Video { get; set; }
        private static readonly PlaylistManager PlaylistManager = new PlaylistManager();

        /// <summary>
        ///     This code is only ran once for the given TestFixture.
        /// </summary>
        [TestFixtureSetUp]
        public new void TestFixtureSetUp()
        {
            try
            {
                PlaylistDao = DaoFactory.GetPlaylistDao();
            }
            catch (TypeInitializationException exception)
            {
                throw exception.InnerException;
            }

            User = Helpers.CreateUser();
            Folder = User.Folders.First();

            Video = Helpers.CreateUnsavedVideoWithId();

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            new VideoManager().Save(Video);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();
        }

        /// <summary>
        ///     Make sure that when the first PlaylistItem is added to a Playlist that the
        ///     Playlist's FirstItem field is appropriately set in the database.
        /// </summary>
        //[Test]
        //public void AddItem_NoItemsInPlaylist_FirstItemIdSet()
        //{
        //    Playlist playlist = Folder.CreateAndAddPlaylist();
        //    PlaylistManager.Save(playlist);

        //    PlaylistItem playlistItem = Helpers.CreateItemInPlaylist(playlist);

        //    //  Remove entity from NHibernate cache to force DB query to ensure actually created.
        //    NHibernateSessionManager.Instance.SessionFactory.GetCurrentSession().Clear();

        //    Playlist playlistFromDatabase = PlaylistDao.Get(playlist.Id);
        //    Assert.AreEqual(playlistFromDatabase.FirstItem, playlistItem);
        //}

        [Test]
        public void Updates()
        {
            Playlist playlist = Folder.CreateAndAddPlaylist();

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
        ///     Verifies that a Playlist can be deleted properly. The Playlist
        ///     has no items underneath it and the Folder is assumed to not have any additional Playlists.
        /// </summary>
        [Test]
        public void DeletePlaylist()
        {
            //  Create a new Playlist and write it to the database.
            string title = string.Format("New Playlist {0:D4}", Folder.Playlists.Count);
            var playlist = new Playlist(title);

            Folder.AddPlaylist(playlist);

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
