using System;

namespace Streamus_Web_API.Domain.Interfaces
{
  public interface IPlaylistManager
  {
    Playlist Get(Guid id);
    Playlist CopyAndSave(Guid id);
    void Save(Playlist playlist);
    void Update(Playlist playlist);
    void Delete(Guid id);
  }
}