using System;

namespace Streamus.Domain.Interfaces
{
    public interface IPlaylistDao : IDao<Playlist>
    {
        Playlist Get(Guid id);
        void DeleteById(Guid id);
        void UpdateTitleById(Guid id, string title);
    }
}
