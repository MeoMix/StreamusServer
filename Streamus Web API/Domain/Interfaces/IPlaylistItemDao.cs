using System;

namespace Streamus_Web_API.Domain.Interfaces
{
    public interface IPlaylistItemDao : IDao<PlaylistItem>
    {
        PlaylistItem Get(Guid id);
    }
}
