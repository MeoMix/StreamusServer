using System;
using FluentNHibernate.Mapping;
using Streamus_Web_API.Domain;

namespace Streamus_Web_API.Dao.Mappings
{
  public class PlaylistMapping : ClassMap<Playlist>
  {
    public PlaylistMapping()
    {
      Table("[Playlists]");
      Id(e => e.Id).GeneratedBy.GuidComb().UnsavedValue(Guid.Empty);

      Map(e => e.Title).Not.Nullable().Length(Playlist.MaxTitleLength);
      Map(e => e.Sequence).Not.Nullable();

      //  Only update properties which have changed.
      DynamicUpdate();

      HasMany(p => p.Items)
          .Inverse()
          .Cascade.AllDeleteOrphan().KeyColumn("PlaylistId");

      References(p => p.User).Column("UserId");
    }
  }
}