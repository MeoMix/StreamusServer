using FluentNHibernate.Testing;
using NUnit.Framework;
using Streamus_Web_API.Domain;

namespace Streamus_Web_API_Tests.Tests.PersistanceMappingTests
{
    [TestFixture]
    public class VideoMappingTest : StreamusTest
    {
        [Test]
        public void ShouldMap()
        {
            using (var transaction = Session.BeginTransaction())
            {
                new PersistenceSpecification<Video>(Session)
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