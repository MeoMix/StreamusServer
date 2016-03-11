using System;
using FluentNHibernate.Testing;
using NUnit.Framework;
using Streamus_Web_API.Domain;

namespace Streamus_Web_API_Tests.Persistance_Mapping_Tests
{
  [TestFixture]
  public class ClientErrorMappingTest : StreamusTest
  {
    [Test]
    public void ShouldMap()
    {
      using (Session.BeginTransaction())
      {
        new PersistenceSpecification<ClientError>(Session)
            .CheckProperty(e => e.LineNumber, 1)
            .CheckProperty(e => e.Message, "testmessage")
            .CheckProperty(e => e.OperatingSystem, "os")
            .CheckProperty(e => e.TimeOccurred, DateTime.Today)
            .CheckProperty(e => e.Url, "url")
            .CheckProperty(e => e.Architecture, "arch")
            .CheckProperty(e => e.ClientVersion, "0.164")
            .CheckProperty(e => e.UserId, Guid.NewGuid())
            .CheckProperty(e => e.InstanceId, "instance_1")
            .CheckProperty(e => e.Stack, "stacktrace")
            .CheckProperty(e => e.BrowserVersion, "chrome v39")
            .VerifyTheMappings();
      }
    }
  }
}
