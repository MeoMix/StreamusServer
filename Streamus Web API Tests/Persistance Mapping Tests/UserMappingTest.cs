using NHibernate.Linq;
using NUnit.Framework;
using Streamus_Web_API.Domain;
using System;
using System.Linq;

namespace Streamus_Web_API_Tests.Tests.PersistanceMappingTests
{
    [TestFixture]
    public class UserMappingTest : StreamusTest
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

                var playlist = new Playlist("boss videos")
                    {
                        User = createdUser
                    };

                createdUser.AddPlaylist(playlist);

                Session.Save(createdUser);
                Session.Flush();
                Session.Clear();

                var savedUser = Session.Get<User>(createdUser.Id);
                Assert.That(savedUser.Id, Is.Not.EqualTo(Guid.Empty));

                Assert.That(savedUser.GooglePlusId, Is.EqualTo(createdUser.GooglePlusId));
                Assert.That(savedUser.Playlists, Has.Count.EqualTo(2));

                Assert.That(Session.Query<Playlist>().Where(p => p.User == savedUser).ToList(), Has.Count.EqualTo(2));

                transaction.Rollback();
            }
        }
    }
}