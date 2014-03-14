using FluentNHibernate.Testing;
using NUnit.Framework;
using Streamus_Web_API.Domain;
using System;

namespace Streamus_Web_API_Tests.Tests.PersistanceMappingTests
{
    [TestFixture]
    public class ClientErrorMappingTest : StreamusTest
    {
        [Test]
        public void ShouldMap()
        {
            using (Session.BeginTransaction())
            {
                var sampleOccurenceTime = DateTime.Today;

                new PersistenceSpecification<ClientError>(Session)
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
