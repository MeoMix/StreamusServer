using System;
using FluentNHibernate.Mapping;
using Streamus.Domain;

namespace Streamus.Dao.Mappings
{
    public class UserMapping : ClassMap<User>
    {
        public UserMapping()
        {
            Table("[Users]");
            Not.LazyLoad();
            Id(e => e.Id).GeneratedBy.GuidComb().UnsavedValue(Guid.Empty);

            Map(e => e.Name).Not.Nullable();
            Map(e => e.GooglePlusId).Not.Nullable();

            HasMany(u => u.Playlists)
                .Inverse()
                .Not.LazyLoad()
                .Fetch.Select()
                .Cascade.AllDeleteOrphan()
                .KeyColumn("UserId");
        }
    }
}