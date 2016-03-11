using System;
using NUnit.Framework;
using Streamus_Web_API.Domain;

namespace Streamus_Web_API_Tests.Persistance_Mapping_Tests
{
  [TestFixture]
  public class PlaylistMappingTest : StreamusTest
  {
    [Test]
    public void ShouldMap()
    {
      using (var transaction = Session.BeginTransaction())
      {
        var createdUser = new User
        {
          GooglePlusId = "some id?"
        };

        Session.Save(createdUser);

        var playlist2 = new Playlist("users second playlist")
        {
          User = createdUser,
          Sequence = 200,
        };

        var playlistItem = new PlaylistItem
        {
          Playlist = playlist2,
          Sequence = 200,
        };

        playlist2.AddItem(playlistItem);

        var playlistId = Session.Save(playlist2);

        Session.Flush();
        Session.Clear();

        var savedPlaylist = Session.Get<Playlist>(playlistId);

        Assert.That(savedPlaylist.Title, Is.EqualTo("users second playlist"));
        Assert.That(savedPlaylist.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(savedPlaylist.Sequence, Is.EqualTo(200));

        Assert.That(savedPlaylist.Items, Has.Exactly(1).EqualTo(playlistItem));

        transaction.Rollback();
      }
    }
  }
}