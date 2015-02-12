using System;
using FluentNHibernate.Mapping;
using Streamus_Web_API.Domain;

namespace Streamus_Web_API.Dao.Mappings
{
    public class ClientErrorMapping : ClassMap<ClientError>
    {
        public ClientErrorMapping()
        {
            Table("[Errors]");

            Id(e => e.Id).GeneratedBy.GuidComb().UnsavedValue(Guid.Empty);

            //  Only update properties which have changed.
            DynamicUpdate();

            Map(e => e.Message).Length(ClientError.MaxMessageLength).Not.Nullable();
            Map(e => e.LineNumber).Not.Nullable();
            Map(e => e.Url).Length(ClientError.MaxUrlLength).Not.Nullable();
            Map(e => e.ClientVersion).Length(ClientError.MaxClientVersionLength).Not.Nullable();
            Map(e => e.TimeOccurred).Not.Nullable();
            Map(e => e.OperatingSystem).Length(ClientError.MaxOperatingSystemLength).Not.Nullable();
            Map(e => e.Architecture).Length(ClientError.MaxArchitectureLength).Not.Nullable();
            Map(e => e.Stack).Length(ClientError.MaxStackLength).Not.Nullable();
            Map(e => e.BrowserVersion).Length(ClientError.MaxBrowserVersionLength).Not.Nullable();
            Map(e => e.InstanceId).Length(ClientError.MaxInstanceIdLength).Not.Nullable();
            Map(e => e.UserId).Not.Nullable();
        }
    }
}