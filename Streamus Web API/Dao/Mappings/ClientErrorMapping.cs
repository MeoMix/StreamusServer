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

            Map(e => e.Message).Not.Nullable().Length(ClientError.MaxMessageLength);
            Map(e => e.LineNumber).Not.Nullable();
            Map(e => e.Url).Not.Nullable().Length(ClientError.MaxUrlLength);
            Map(e => e.ClientVersion).Not.Nullable().Length(ClientError.MaxClientVersionLength);
            Map(e => e.TimeOccurred).Not.Nullable();
            Map(e => e.OperatingSystem).Not.Nullable().Length(ClientError.MaxOperatingSystemLength);
            Map(e => e.Architecture).Not.Nullable().Length(ClientError.MaxArchitectureLength);
            Map(e => e.Stack).Not.Nullable().Length(ClientError.MaxStackLength);
            Map(e => e.BrowserVersion).Not.Nullable().Length(ClientError.MaxBrowserVersionLength);
            Map(e => e.InstanceId).Not.Nullable().Length(ClientError.MaxInstanceIdLength);
            Map(e => e.UserId).Not.Nullable();
        }
    }
}