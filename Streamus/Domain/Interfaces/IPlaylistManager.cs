using System;

namespace Streamus.Domain.Interfaces
{
    public interface IPlaylistManager
    {
        Playlist Get(Guid id);
        Playlist CopyAndSave(Guid id);
        void Save(Playlist playlist);
        void Update(Playlist playlist);
        void Delete(Guid id);
        void UpdateTitle(Guid playlistId, string title);
    }
}
