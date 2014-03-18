using NHibernate;
using NUnit.Framework;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;

namespace Streamus_Web_API_Tests.Tests.Manager_Tests
{
    [TestFixture]
    public class PlaylistManagerTest : StreamusTest
    {
        private IPlaylistManager PlaylistManager;

        private User User { get; set; }

        /// <summary>
        ///     This code is only ran once for the given TestFixture.
        /// </summary>
        [SetUp]
        public new void TestFixtureSetUp()
        {
            PlaylistManager = ManagerFactory.GetPlaylistManager(Session);

            User = Helpers.CreateUser();
        }

        [Test]
        public void Updates()
        {
            Playlist playlist = User.CreateAndAddPlaylist();
            PlaylistManager.Save(playlist);

            const string newTitle = "Existing Playlist 001";
            PlaylistManager.UpdateTitle(playlist.Id, newTitle);
            Assert.AreEqual(playlist.Title, newTitle);
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

            PlaylistManager.Save(playlist);

            //  Now delete the created Playlist and ensure it is removed.
            PlaylistManager.Delete(playlist.Id);


            bool exceptionEncountered = false;
            try
            {
                Playlist deletedPlaylist = PlaylistManager.Get(playlist.Id);
            }
            catch (ObjectNotFoundException)
            {
                exceptionEncountered = true;
            }

            Assert.IsTrue(exceptionEncountered);
        }
    }
}