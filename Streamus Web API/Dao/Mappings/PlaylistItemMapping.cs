using System;
using FluentNHibernate.Mapping;
using Streamus_Web_API.Domain;

namespace Streamus_Web_API.Dao.Mappings
{
    public class PlaylistItemMapping : ClassMap<PlaylistItem>
    {
        public PlaylistItemMapping()
        {
            Table("[PlaylistItems]");

            Id(e => e.Id).GeneratedBy.GuidComb().UnsavedValue(Guid.Empty);

            //  Only update properties which have changed.
            DynamicUpdate();

            Map(e => e.Title).Not.Nullable();
            Map(e => e.Sequence).Not.Nullable();
            Map(e => e.Author).Not.Nullable();
            Map(e => e.Duration).Not.Nullable();
            Map(e => e.SourceId).Not.Nullable();
            Map(e => e.HighDefinition).Not.Nullable();
            Map(e => e.SourceType).Not.Nullable();
            Map(e => e.SourceTitle).Not.Nullable();

            References(p => p.Playlist).Column("PlaylistId");
        }
    }
}