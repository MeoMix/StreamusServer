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
            Not.LazyLoad();
            Id(e => e.Id).GeneratedBy.GuidComb().UnsavedValue(Guid.Empty);

            Map(e => e.EntityType).CustomType<ShareableEntityType>().Not.Nullable();
            Map(e => e.EntityId).Not.Nullable();
            Map(e => e.ShortId).Not.Nullable();
            Map(e => e.UrlFriendlyEntityTitle).Not.Nullable();
        }
    }
}