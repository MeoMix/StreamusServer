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

        public NHibernateDaoFactory()
        {
            //  TODO: Is this different than passing ISession into NHibernateDaoFactory with AutoFac?
            Session = DependencyResolver.Current.GetService<ISession>();

            if (Session == null) throw new NullReferenceException("Session");
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