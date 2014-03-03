using System;
using System.Web.Mvc;
using NHibernate;
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
        private readonly ISession Session;

        public NHibernateDaoFactory(ISession session)
        {
            if (session == null) throw new NullReferenceException("session");

            Session = session;
        }

        public IErrorDao GetErrorDao()
        {
            return ErrorDao ?? (ErrorDao = new ErrorDao(Session));
        }

        public IPlaylistDao GetPlaylistDao()
        {
            return PlaylistDao ?? (PlaylistDao = new PlaylistDao(Session));
        }

        public IPlaylistItemDao GetPlaylistItemDao()
        {
            return PlaylistItemDao ?? (PlaylistItemDao = new PlaylistItemDao(Session));
        }

        public IShareCodeDao GetShareCodeDao()
        {
            return ShareCodeDao ?? (ShareCodeDao = new ShareCodeDao(Session));
        }

        public IUserDao GetUserDao()
        {
            return UserDao ?? (UserDao = new UserDao(Session));
        }

        public IVideoDao GetVideoDao()
        {
            return VideoDao ?? (VideoDao = new VideoDao(Session));
        }
    }
}