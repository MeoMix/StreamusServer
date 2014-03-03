using System;
using NUnit.Framework;
using Streamus.Dao;
using Streamus.Domain;

namespace Streamus.Tests.PersistanceMappingTests
{
    [TestFixture]
    public class PlaylistMappingTest
    {
        [Test]
        public void ShouldMap()
        {
            var sessionFactory = new NHibernateConfiguration().Configure().BuildSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var createdUser = new User {GooglePlusId = "some id?", Name = "user name"};

                    session.Save(createdUser);

                    var playlist2 = new Playlist("users second playlist")
                        {
                            User = createdUser,
                            Sequence = 200,
                        };

                    var playlistItem = new PlaylistItem
                        {
                            Cid = "cid",
                            Playlist = playlist2,
                            Video = new Video(),
                            Sequence = 200,
                        };

                    playlist2.AddItem(playlistItem);

                    var playlistId = session.Save(playlist2);

                    session.Flush();
                    session.Clear();

                    var savedPlaylist = session.Get<Playlist>(playlistId);

                    Assert.That(savedPlaylist.Title, Is.EqualTo("users second playlist"));
                    Assert.That(savedPlaylist.Id, Is.Not.EqualTo(Guid.Empty));
                    Assert.That(savedPlaylist.Sequence, Is.EqualTo(200));

                    Assert.That(savedPlaylist.Items, Has.Exactly(1).EqualTo(playlistItem));

                    transaction.Rollback();
                }
            }
        }
    }
}