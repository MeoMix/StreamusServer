using System;
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using Streamus.Dao;
using Streamus.Domain;

namespace Streamus.Tests.PersistanceMappingTests
{
    [TestFixture]
    public class UserMappingTest
    {
        [Test]
        public void ShouldMap()
        {
            var sessionFactory = NHibernateSessionManager.Instance.SessionFactory;
            using (var session = sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var createdUser = new User { GooglePlusId = "some id?", Name = "user name" };
                var playlist = new Playlist("boss songs") { User = createdUser };
                createdUser.AddPlaylist(playlist);

                session.Save(createdUser);
                session.Flush();
                session.Clear();

                var savedUser = session.Get<User>(createdUser.Id);
                Assert.That(savedUser.Id, Is.Not.EqualTo(Guid.Empty));

                Assert.That(savedUser.GooglePlusId, Is.EqualTo(createdUser.GooglePlusId));
                Assert.That(savedUser.Name, Is.EqualTo(createdUser.Name));
                Assert.That(savedUser.Playlists, Has.Count.EqualTo(2));

                Assert.That(session.Query<Playlist>().Where(p => p.User == savedUser).ToList(), Has.Count.EqualTo(2));

                transaction.Rollback();
            }
        }
    }
}