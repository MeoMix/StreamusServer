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

            Map(e => e.Sequence).Not.Nullable();
            Map(e => e.Author).Not.Nullable().Length(PlaylistItem.MaxAuthorLength);
            Map(e => e.Duration).Not.Nullable();
            Map(e => e.VideoId).Not.Nullable().Length(PlaylistItem.MaxVideoIdLength);
            Map(e => e.VideoType).Not.Nullable().Length(PlaylistItem.MaxVideoTypeLength);
            Map(e => e.VideoTitle).Not.Nullable().Length(PlaylistItem.MaxVideoTitleLength);

            References(p => p.Playlist).Column("PlaylistId");
        }
    }
}