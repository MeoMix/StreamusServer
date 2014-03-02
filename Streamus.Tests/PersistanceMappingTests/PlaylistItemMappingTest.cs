using System;
using NUnit.Framework;
using Streamus.Dao;
using Streamus.Domain;

namespace Streamus.Tests.PersistanceMappingTests
{
    [TestFixture]
    public class PlaylistItemMappingTest
    {
        [Test]
        public void ShouldMap()
        {
            var sessionFactory = NHibernateSessionManager.Instance.SessionFactory;
            using (var session = sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var createdUser = new User { GooglePlusId = "some id?", Name = "user name" };
                session.Save(createdUser);

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
                session.Save(video);
                var playlistItem = new PlaylistItem
                {
                    Cid = "cid",
                    Playlist = playlist2,
                    Video = video,
                    Sequence = 300,
                    Title = "My playlist item",
                };

                playlist2.AddItem(playlistItem);

                session.Save(playlist2);

                session.Flush();
                session.Clear();

                var savedPlaylistItem = session.Get<PlaylistItem>(playlistItem.Id);

                Assert.That(savedPlaylistItem.Title, Is.EqualTo("My playlist item"));
                Assert.That(savedPlaylistItem.Id, Is.Not.EqualTo(Guid.Empty));
                Assert.That(savedPlaylistItem.Sequence, Is.EqualTo(300));

                Assert.That(savedPlaylistItem.Video, Is.EqualTo(playlistItem.Video));

                transaction.Rollback();
            }
        }
    }
}