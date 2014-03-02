using System;
using FluentNHibernate.Testing;
using NUnit.Framework;
using Streamus.Dao;
using Streamus.Domain;

namespace Streamus.Tests.PersistanceMappingTests
{
    [TestFixture]
    public class ErrorMappingTest
    {
        [Test]
        public void ShouldMap()
        {
            var sessionFactory = NHibernateSessionManager.Instance.SessionFactory;
            using (var session = sessionFactory.OpenSession())
            using (session.BeginTransaction())
            {
                var sampleOccurenceTime = DateTime.Today;

                new PersistenceSpecification<Error>(session)
                    .CheckProperty(e => e.LineNumber, 1)
                    .CheckProperty(e => e.Message, "testmessage")
                    .CheckProperty(e => e.OperatingSystem, "operating system")
                    .CheckProperty(e => e.TimeOccurred, sampleOccurenceTime)
                    .CheckProperty(e => e.Url, "url")
                    .CheckProperty(e => e.Architecture, "astrological")
                    .VerifyTheMappings();
            }
        }
    }
}
