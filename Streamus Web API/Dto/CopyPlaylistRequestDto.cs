using System;

namespace Streamus_Web_API.Dto
{
  public class CopyPlaylistRequestDto
  {
    public Guid UserId { get; set; }
    public Guid PlaylistId { get; set; }

    public CopyPlaylistRequestDto()
    {

    }

    public CopyPlaylistRequestDto(Guid userId, Guid playlistId)
    {
      UserId = userId;
      PlaylistId = playlistId;
    }
  }
}