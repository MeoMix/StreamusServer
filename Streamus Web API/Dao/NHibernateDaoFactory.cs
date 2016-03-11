using NHibernate;
using Streamus_Web_API.Domain.Interfaces;

namespace Streamus_Web_API.Dao
{
  /// <summary>
  ///     Exposes access to NHibernate DAO classes.  Motivation for this DAO
  ///     framework can be found at http://www.hibernate.org/328.html.
  /// </summary>
  public class NHibernateDaoFactory : IDaoFactory
  {
    private IClientErrorDao _clientErrorDao;
    private IPlaylistDao _playlistDao;
    private IPlaylistItemDao _playlistItemDao;
    private IShareCodeDao _shareCodeDao;
    private IUserDao _userDao;

    public IClientErrorDao GetErrorDao(ISession session)
    {
      return _clientErrorDao ?? (_clientErrorDao = new ClientErrorDao(session));
    }

    public IPlaylistDao GetPlaylistDao(ISession session)
    {
      return _playlistDao ?? (_playlistDao = new PlaylistDao(session));
    }

    public IPlaylistItemDao GetPlaylistItemDao(ISession session)
    {
      return _playlistItemDao ?? (_playlistItemDao = new PlaylistItemDao(session));
    }

    public IShareCodeDao GetShareCodeDao(ISession session)
    {
      return _shareCodeDao ?? (_shareCodeDao = new ShareCodeDao(session));
    }

    public IUserDao GetUserDao(ISession session)
    {
      return _userDao ?? (_userDao = new UserDao(session));
    }
  }
}
