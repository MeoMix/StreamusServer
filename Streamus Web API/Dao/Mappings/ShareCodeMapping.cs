using System;
using FluentNHibernate.Mapping;
using Streamus_Web_API.Domain;

namespace Streamus_Web_API.Dao.Mappings
{
    public class ShareCodeMapping : ClassMap<ShareCode>
    {
        public ShareCodeMapping()
        {
            Table("[ShareCodes]");

            Id(e => e.Id).GeneratedBy.GuidComb().UnsavedValue(Guid.Empty);

            //  Only update properties which have changed.
            DynamicUpdate();

            Map(e => e.EntityType).Not.Nullable().Length(AbstractShareableDomainEntity.MaxEntityTypeLength);
            Map(e => e.EntityId).Not.Nullable();
            Map(e => e.ShortId).Not.Nullable().Length(AbstractShareableDomainEntity.MaxShortIdLength);
            Map(e => e.UrlFriendlyEntityTitle).Not.Nullable().Length(AbstractShareableDomainEntity.MaxUrlFriendlyTitleLength);
        }
    }
}