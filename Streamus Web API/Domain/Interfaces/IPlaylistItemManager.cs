using System;
using System.Collections.Generic;

namespace Streamus_Web_API.Domain.Interfaces
{
    public interface IPlaylistItemManager
    {
        PlaylistItem Get(Guid id);
        void Delete(Guid itemId);
        void Save(IEnumerable<PlaylistItem> playlistItems);
        void Save(PlaylistItem playlistItem);
        void Update(PlaylistItem playlistItem);
        void UpdateSequence(Guid playlistId, double sequence);
    }
}
