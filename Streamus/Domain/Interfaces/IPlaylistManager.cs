
using System;
using System.Collections.Generic;

namespace Streamus.Domain.Interfaces
{
    public interface IPlaylistManager
    {
        void Save(Playlist playlist);
        void Save(IEnumerable<Playlist> playlists);
        void Update(Playlist playlist);
        void Delete(Guid id);
        void UpdateTitle(Guid playlistId, string title);
    }
}
