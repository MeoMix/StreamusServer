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
        private IClientErrorDao ClientErrorDao;
        private IPlaylistDao PlaylistDao;
        private IPlaylistItemDao PlaylistItemDao;
        private IShareCodeDao ShareCodeDao;
        private IUserDao UserDao;

        public IClientErrorDao GetErrorDao(ISession session)
        {
            return ClientErrorDao ?? (ClientErrorDao = new ClientErrorDao(session));
        }

        public IPlaylistDao GetPlaylistDao(ISession session)
        {
            return PlaylistDao ?? (PlaylistDao = new PlaylistDao(session));
        }

        public IPlaylistItemDao GetPlaylistItemDao(ISession session)
        {
            return PlaylistItemDao ?? (PlaylistItemDao = new PlaylistItemDao(session));
        }

        public IShareCodeDao GetShareCodeDao(ISession session)
        {
            return ShareCodeDao ?? (ShareCodeDao = new ShareCodeDao(session));
        }

        public IUserDao GetUserDao(ISession session)
        {
            return UserDao ?? (UserDao = new UserDao(session));
        }
    }
}
