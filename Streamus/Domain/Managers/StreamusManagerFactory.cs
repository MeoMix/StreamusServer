using System;
using log4net;
using Streamus.Domain.Interfaces;

namespace Streamus.Domain.Managers
{
    public class StreamusManagerFactory : IManagerFactory
    {
        private readonly ILog Logger;
        private readonly IDaoFactory DaoFactory;

        private IErrorManager ErrorManager;
        private IPlaylistItemManager PlaylistItemManager;
        private IPlaylistManager PlaylistManager;
        private IShareCodeManager ShareCodeManager;
        private IUserManager UserManager;
        private IVideoManager VideoManager;

        public StreamusManagerFactory(ILog logger, IDaoFactory daoFactory)
        {
            if (logger == null) throw new NullReferenceException("logger");
            if (daoFactory == null) throw new NullReferenceException("daoFactory");

            Logger = logger;
            DaoFactory = daoFactory;
        }

        public IErrorManager GetErrorManager()
        {
            return ErrorManager ?? (ErrorManager = new ErrorManager(Logger, DaoFactory.GetErrorDao()));
        }

        public IPlaylistItemManager GetPlaylistItemManager()
        {
            return PlaylistItemManager ?? (PlaylistItemManager = new PlaylistItemManager(Logger, DaoFactory.GetPlaylistItemDao(), DaoFactory.GetVideoDao()));
        }

        public IPlaylistManager GetPlaylistManager()
        {
            return PlaylistManager ?? (PlaylistManager = new PlaylistManager(Logger, DaoFactory.GetPlaylistDao(), DaoFactory.GetVideoDao()));
        }

        public IShareCodeManager GetShareCodeManager()
        {
            return ShareCodeManager ?? (ShareCodeManager = new ShareCodeManager(Logger, DaoFactory.GetShareCodeDao()));
        }

        public IUserManager GetUserManager()
        {
            return UserManager ?? (UserManager = new UserManager(Logger, DaoFactory.GetUserDao()));
        }

        public IVideoManager GetVideoManager()
        {
            return VideoManager ?? (VideoManager = new VideoManager(Logger, DaoFactory.GetVideoDao()));
        }
    }
}