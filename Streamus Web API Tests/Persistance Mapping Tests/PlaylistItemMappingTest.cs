using NUnit.Framework;
using Streamus_Web_API.Domain;
using System;

namespace Streamus_Web_API_Tests.Tests.PersistanceMappingTests
{
    [TestFixture]
    public class PlaylistItemMappingTest : StreamusTest
    {
        [Test]
        public void ShouldMap()
        {
            using (var transaction = Session.BeginTransaction())
            {
                var createdUser = new User {GooglePlusId = "some id?", Name = "user name"};
                Session.Save(createdUser);

                var playlist2 = new Playlist("users second playlist")
                    {
                        User = createdUser,
                        Sequence = 200,
                    };

                var video = new Video
                    {
                        Id = "some id",
                        Author = "video author",
                        Duration = 90,
                        HighDefinition = true,
                        Title = "my video",
                    };

                Session.Save(video);
                var playlistItem = new PlaylistItem
                    {
                        Cid = "cid",
                        Playlist = playlist2,
                        Video = video,
                        Sequence = 300,
                        Title = "My playlist item",
                    };

                playlist2.AddItem(playlistItem);

                Session.Save(playlist2);

                Session.Flush();
                Session.Clear();

                var savedPlaylistItem = Session.Get<PlaylistItem>(playlistItem.Id);

                Assert.That(savedPlaylistItem.Title, Is.EqualTo("My playlist item"));
                Assert.That(savedPlaylistItem.Id, Is.Not.EqualTo(Guid.Empty));
                Assert.That(savedPlaylistItem.Sequence, Is.EqualTo(300));

                Assert.That(savedPlaylistItem.Video, Is.EqualTo(playlistItem.Video));

                transaction.Rollback();
            }
        }
    }
}