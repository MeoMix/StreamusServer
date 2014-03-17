using log4net;
using Streamus_Web_API.Domain.Interfaces;
using System;

namespace Streamus_Web_API.Domain.Managers
{
    public class StreamusManagerFactory : IManagerFactory
    {
        private readonly ILog Logger;
        private readonly IDaoFactory DaoFactory;

        private IClientErrorManager ClientErrorManager;
        private IPlaylistItemManager PlaylistItemManager;
        private IPlaylistManager PlaylistManager;
        private IShareCodeManager ShareCodeManager;
        private IUserManager UserManager;

        public StreamusManagerFactory(ILog logger, IDaoFactory daoFactory)
        {
            if (logger == null) throw new NullReferenceException("logger");
            if (daoFactory == null) throw new NullReferenceException("daoFactory");

            Logger = logger;
            DaoFactory = daoFactory;
        }

        public IClientErrorManager GetErrorManager()
        {
            return ClientErrorManager ?? (ClientErrorManager = new ClientErrorManager(Logger, DaoFactory.GetErrorDao()));
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
    }
}