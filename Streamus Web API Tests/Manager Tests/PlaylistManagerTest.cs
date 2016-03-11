using NHibernate;
using NUnit.Framework;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;

namespace Streamus_Web_API_Tests.Manager_Tests
{
  [TestFixture]
  public class PlaylistManagerTest : StreamusTest
  {
    private IPlaylistManager _playlistManager;

    private User User { get; set; }

    /// <summary>
    ///     This code is only ran once for the given TestFixture.
    /// </summary>
    [SetUp]
    public new void TestFixtureSetUp()
    {
      _playlistManager = ManagerFactory.GetPlaylistManager(Session);

      User = Helpers.CreateUser();
    }

    /// <summary>
    ///     Verifies that a Playlist can be deleted properly.
    /// </summary>
    [Test]
    public void DeletePlaylist()
    {
      //  Create a new Playlist and write it to the database.
      string title = $"New Playlist {User.Playlists.Count:D4}";
      var playlist = new Playlist(title);

      User.AddPlaylist(playlist);

      _playlistManager.Save(playlist);

      //  Now delete the created Playlist and ensure it is removed.
      _playlistManager.Delete(playlist.Id);


      bool exceptionEncountered = false;
      try
      {
        Playlist deletedPlaylist = _playlistManager.Get(playlist.Id);
      }
      catch (ObjectNotFoundException)
      {
        exceptionEncountered = true;
      }

      Assert.IsTrue(exceptionEncountered);
    }
  }
}