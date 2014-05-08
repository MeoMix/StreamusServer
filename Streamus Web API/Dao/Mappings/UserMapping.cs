using FluentNHibernate.Mapping;
using Streamus_Web_API.Domain;
using System;

namespace Streamus_Web_API.Dao.Mappings
{
    public class UserMapping : ClassMap<User>
    {
        public UserMapping()
        {
            Table("[Users]");

            Not.LazyLoad();

            Id(e => e.Id).GeneratedBy.GuidComb().UnsavedValue(Guid.Empty);

            //  Only update properties which have changed.
            DynamicUpdate();

            Map(e => e.Name).Not.Nullable();
            Map(e => e.GooglePlusId).Not.Nullable();

            HasMany(u => u.Playlists)
                .Inverse()
                //  100% of the time a user is loaded their playlists are sent back to the server, so it's OK to do this.
                .Not.LazyLoad()
                .Cascade.AllDeleteOrphan()
                .KeyColumn("UserId");
        }
    }
}