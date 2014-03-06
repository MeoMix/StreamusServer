using FluentNHibernate.Testing;
using NUnit.Framework;
using Streamus.Dao;
using Streamus.Domain;

namespace Streamus.Tests.PersistanceMappingTests
{
    [TestFixture]
    public class VideoMappingTest
    {
        [Test]
        public void ShouldMap()
        {
            var sessionFactory = new NHibernateConfiguration().Configure().BuildSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    new PersistenceSpecification<Video>(session)
                        .CheckProperty(v => v.Author, "author")
                        .CheckProperty(v => v.Duration, 90)
                        .CheckProperty(v => v.HighDefinition, true)
                        .CheckProperty(v => v.Id, "some id")
                        .CheckProperty(v => v.Title, "title")
                        .VerifyTheMappings();

                    transaction.Rollback();
                }
            }

        }
    }
}