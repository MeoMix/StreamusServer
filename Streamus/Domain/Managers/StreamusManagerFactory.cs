using log4net;
using Streamus.Domain.Interfaces;

namespace Streamus.Domain.Managers
{
    public class StreamusManagerFactory : IManagerFactory
    {
        private readonly ILog Logger;
        private IErrorManager ErrorManager;
        private IPlaylistItemManager PlaylistItemManager;
        private IPlaylistManager PlaylistManager;
        private IShareCodeManager ShareCodeManager;
        private IUserManager UserManager;
        private IVideoManager VideoManager;

        public StreamusManagerFactory(ILog logger)
        {
            Logger = logger;
        }

        public IErrorManager GetErrorManager(IErrorDao errorDao)
        {
            return ErrorManager ?? (ErrorManager = new ErrorManager(Logger, errorDao));
        }

        public IPlaylistItemManager GetPlaylistItemManager(IPlaylistItemDao playlistItemDao, IVideoDao videoDao)
        {
            return PlaylistItemManager ?? (PlaylistItemManager = new PlaylistItemManager(Logger, playlistItemDao, videoDao));
        }

        public IPlaylistManager GetPlaylistManager(IPlaylistDao playlistDao, IVideoDao videoDao)
        {
            return PlaylistManager ?? (PlaylistManager = new PlaylistManager(Logger, playlistDao, videoDao));
        }

        public IShareCodeManager GetShareCodeManager(IPlaylistDao playlistDao, IShareCodeDao shareCodeDao, IPlaylistManager playlistManager)
        {
            return ShareCodeManager ?? (ShareCodeManager = new ShareCodeManager(Logger, playlistDao, shareCodeDao, playlistManager));
        }

        public IUserManager GetUserManager(IUserDao userDao)
        {
            return UserManager ?? (UserManager = new UserManager(Logger, userDao));
        }

        public IVideoManager GetVideoManager(IVideoDao videoDao)
        {
            return VideoManager ?? (VideoManager = new VideoManager(Logger, videoDao));
        }
    }
}