using log4net;
using NHibernate;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;
using Streamus_Web_API.Dto;
using System;
using System.Web.Http;

namespace Streamus_Web_API.Controllers
{
  [RoutePrefix("ShareCode")]
  public class ShareCodeController : StreamusController
  {
    private readonly IShareCodeManager _shareCodeManager;
    private readonly IPlaylistManager _playlistManager;

    public ShareCodeController(ILog logger, ISession session, IManagerFactory managerFactory)
        : base(logger, session)
    {
      _shareCodeManager = managerFactory.GetShareCodeManager(session);
      _playlistManager = managerFactory.GetPlaylistManager(session);
    }

    //  TODO: It feels weird to be retrieving information like this, but I'm not sure of a better way to tackle it just yet.
    [Route("{shortId}/{entityTitle}")]
    [HttpGet]
    public ShareCodeDto GetShareCodeByShortIdAndEntityTitle(string shortId, string entityTitle)
    {
      ShareCodeDto shareCodeDto;

      using (ITransaction transaction = Session.BeginTransaction())
      {
        ShareCode shareCode = _shareCodeManager.GetByShortIdAndEntityTitle(shortId, entityTitle);
        shareCodeDto = ShareCodeDto.Create(shareCode);

        transaction.Commit();
      }

      return shareCodeDto;
    }

    [Route("GetShareCode")]
    [HttpGet]
    public ShareCodeDto GetShareCode(Guid id, EntityType entityType)
    {
      ShareCodeDto shareCodeDto;

      if (entityType != EntityType.Playlist)
      {
        throw new NotSupportedException("Only playlists can be shared currently");
      }

      using (ITransaction transaction = Session.BeginTransaction())
      {
        //  Copy the playlist here to serve a static copy instead of whatever the state is when share code is accessed.
        Playlist playlist = _playlistManager.CopyAndSave(id);
        ShareCode shareCode = _shareCodeManager.GetShareCode(playlist);
        shareCodeDto = ShareCodeDto.Create(shareCode);

        transaction.Commit();
      }

      return shareCodeDto;
    }

    //  TODO: Remove this in future update. Kept for backwards compatibility.
    [Route("GetShareCode")]
    [HttpGet]
    public ShareCodeDto GetShareCode(Guid playlistId)
    {
      ShareCodeDto shareCodeDto;

      using (ITransaction transaction = Session.BeginTransaction())
      {
        //  Copy the playlist here to serve a static copy instead of whatever the state is when share code is accessed.
        Playlist playlist = _playlistManager.CopyAndSave(playlistId);
        ShareCode shareCode = _shareCodeManager.GetShareCode(playlist);
        shareCodeDto = ShareCodeDto.Create(shareCode);

        transaction.Commit();
      }

      return shareCodeDto;
    }
  }
}
