using Streamus.Domain.Interfaces;

namespace Streamus.Dao
{
    /// <summary>
    ///     Exposes access to NHibernate DAO classes.  Motivation for this DAO
    ///     framework can be found at http://www.hibernate.org/328.html.
    /// </summary>
    public class NHibernateDaoFactory : IDaoFactory
    {
        private IErrorDao ErrorDao;
        private IPlaylistDao PlaylistDao;
        private IPlaylistItemDao PlaylistItemDao;
        private IShareCodeDao ShareCodeDao;
        private IUserDao UserDao;
        private IVideoDao VideoDao;

        public IErrorDao GetErrorDao()
        {
            return ErrorDao ?? (ErrorDao = new ErrorDao());
        }

        public IPlaylistDao GetPlaylistDao()
        {
            return PlaylistDao ?? (PlaylistDao = new PlaylistDao());
        }

        public IPlaylistItemDao GetPlaylistItemDao()
        {
            return PlaylistItemDao ?? (PlaylistItemDao = new PlaylistItemDao());
        }

        public IShareCodeDao GetShareCodeDao()
        {
            return ShareCodeDao ?? (ShareCodeDao = new ShareCodeDao());
        }

        public IUserDao GetUserDao()
        {
            return UserDao ?? (UserDao = new UserDao());
        }

        public IVideoDao GetVideoDao()
        {
            return VideoDao ?? (VideoDao = new VideoDao());
        }
    }
}