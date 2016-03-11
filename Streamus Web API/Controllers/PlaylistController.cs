using log4net;
using NHibernate;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;
using Streamus_Web_API.Dto;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Streamus_Web_API.Controllers
{
  [RoutePrefix("Playlist")]
  public class PlaylistController : StreamusController
  {
    private readonly IPlaylistManager _playlistManager;
    private readonly IUserManager _userManager;

    public PlaylistController(ILog logger, ISession session, IManagerFactory managerFactory)
        : base(logger, session)
    {
      _playlistManager = managerFactory.GetPlaylistManager(session);
      _userManager = managerFactory.GetUserManager(session);
    }

    [Route("")]
    [HttpPost]
    public PlaylistDto Create(PlaylistDto playlistDto)
    {
      PlaylistDto savedPlaylistDto;

      using (ITransaction transaction = Session.BeginTransaction())
      {
        User user = _userManager.Get(playlistDto.UserId);

        Playlist playlist = new Playlist(playlistDto.Id);
        playlistDto.SetPatchableProperties(playlist);

        user.AddPlaylist(playlist);

        List<PlaylistItem> playlistItems = new List<PlaylistItem>();
        foreach (PlaylistItemDto dto in playlistDto.Items)
        {
          // TODO: Backwards compatibility for old type.
          VideoDto videoDto = dto.Video ?? dto.Song;
          PlaylistItem playlistItem = new PlaylistItem(dto.Id, dto.Cid, videoDto.Id, videoDto.Type, videoDto.Title, videoDto.Duration, videoDto.Author);
          dto.SetPatchableProperties(playlistItem);
          playlistItems.Add(playlistItem);
        }
        playlist.AddItems(playlistItems);

        _playlistManager.Save(playlist);
        savedPlaylistDto = PlaylistDto.Create(playlist);

        transaction.Commit();
      }

      return savedPlaylistDto;
    }

    [Route("{id:guid}")]
    [HttpGet]
    public PlaylistDto Get(Guid id)
    {
      PlaylistDto playlistDto;
      using (ITransaction transaction = Session.BeginTransaction())
      {
        Playlist playlist = _playlistManager.Get(id);
        playlistDto = PlaylistDto.Create(playlist);

        transaction.Commit();
      }

      return playlistDto;
    }

    [Route("{id:guid}")]
    [HttpDelete]
    public void Delete(Guid id)
    {
      using (ITransaction transaction = Session.BeginTransaction())
      {
        _playlistManager.Delete(id);
        transaction.Commit();
      }
    }

    [Route("{id:guid}")]
    [HttpPatch]
    public void Patch(Guid id, PlaylistDto playlistDto)
    {
      using (ITransaction transaction = Session.BeginTransaction())
      {
        Playlist playlist = _playlistManager.Get(id);
        playlistDto.SetPatchableProperties(playlist);
        _playlistManager.Update(playlist);

        transaction.Commit();
      }
    }

    /// <summary>
    ///     Retrieves a Playlist, create a copy of the Playlist, and returns the copied Playlist
    /// </summary>
    [Route("Copy")]
    [HttpPost]
    public PlaylistDto Copy(CopyPlaylistRequestDto copyPlaylistRequestDto)
    {
      PlaylistDto playlistDto;

      using (ITransaction transaction = Session.BeginTransaction())
      {
        Playlist playlistToCopy = _playlistManager.Get(copyPlaylistRequestDto.PlaylistId);
        User user = _userManager.Get(copyPlaylistRequestDto.UserId);

        var playlistCopy = new Playlist(playlistToCopy);
        user.AddPlaylist(playlistCopy);

        _playlistManager.Save(playlistCopy);

        playlistDto = PlaylistDto.Create(playlistCopy);

        transaction.Commit();
      }

      return playlistDto;
    }
  }
}
