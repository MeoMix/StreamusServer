using NHibernate;
using log4net;
using Streamus_Web_API.Domain.Interfaces;
using System;

namespace Streamus_Web_API.Domain.Managers
{
  public class StreamusManagerFactory : IManagerFactory
  {
    private readonly ILog _logger;
    private readonly IDaoFactory _daoFactory;
    private IClientErrorManager _clientErrorManager;
    private IPlaylistItemManager _playlistItemManager;
    private IPlaylistManager _playlistManager;
    private IShareCodeManager _shareCodeManager;
    private IUserManager _userManager;
    private IEmailManager _emailManager;

    public StreamusManagerFactory(ILog logger, IDaoFactory daoFactory)
    {
      if (logger == null) throw new NullReferenceException("logger");
      if (daoFactory == null) throw new NullReferenceException("daoFactory");

      _logger = logger;
      _daoFactory = daoFactory;
    }

    public IClientErrorManager GetErrorManager(ISession session)
    {
      return _clientErrorManager ?? (_clientErrorManager = new ClientErrorManager(_logger, _daoFactory.GetErrorDao(session)));
    }

    public IPlaylistItemManager GetPlaylistItemManager(ISession session)
    {
      return _playlistItemManager ?? (_playlistItemManager = new PlaylistItemManager(_logger, _daoFactory.GetPlaylistItemDao(session)));
    }

    public IPlaylistManager GetPlaylistManager(ISession session)
    {
      return _playlistManager ?? (_playlistManager = new PlaylistManager(_logger, _daoFactory.GetPlaylistDao(session)));
    }

    public IShareCodeManager GetShareCodeManager(ISession session)
    {
      return _shareCodeManager ?? (_shareCodeManager = new ShareCodeManager(_logger, _daoFactory.GetShareCodeDao(session)));
    }

    public IUserManager GetUserManager(ISession session)
    {
      return _userManager ?? (_userManager = new UserManager(_logger, _daoFactory.GetUserDao(session)));
    }

    public IEmailManager GetEmailManager()
    {
      return _emailManager ?? (_emailManager = new EmailManager(_logger));
    }
  }
}