using System;
using System.Collections.Generic;

namespace Streamus.Domain.Interfaces
{
    public interface IPlaylistItemManager
    {
        PlaylistItem Get(Guid id);
        void Delete(Guid itemId);
        void Save(IEnumerable<PlaylistItem> playlistItems);
        void Save(PlaylistItem playlistItem);
        void Update(IEnumerable<PlaylistItem> playlistItems);
        void Update(PlaylistItem playlistItem);
    }
}
