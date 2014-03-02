using System;
using FluentNHibernate.Mapping;
using Streamus.Domain;

namespace Streamus.Dao.Mappings
{
    public class PlaylistItemMapping : ClassMap<PlaylistItem>
    {
        public PlaylistItemMapping()
        {
            Table("[PlaylistItems]");
            Not.LazyLoad();
            Id(e => e.Id).GeneratedBy.GuidComb().UnsavedValue(Guid.Empty);

            Map(e => e.Title).Not.Nullable();
            Map(e => e.Sequence).Not.Nullable();

            References(p => p.Playlist).Column("PlaylistId");
            References(p => p.Video).Column("VideoId").Cascade.SaveUpdate();
        }
    }
}