using System;
using FluentNHibernate.Mapping;
using Streamus.Domain;

namespace Streamus.Dao.Mappings
{
    public class ShareCodeMapping : ClassMap<ShareCode>
    {
        public ShareCodeMapping()
        {
            Table("[ShareCodes]");

            Id(e => e.Id).GeneratedBy.GuidComb().UnsavedValue(Guid.Empty);

            //  Only update properties which have changed.
            DynamicUpdate();

            Map(e => e.EntityType).CustomType<ShareableEntityType>().Not.Nullable();
            Map(e => e.EntityId).Not.Nullable();
            Map(e => e.ShortId).Not.Nullable();
            Map(e => e.UrlFriendlyEntityTitle).Not.Nullable();
        }
    }
}