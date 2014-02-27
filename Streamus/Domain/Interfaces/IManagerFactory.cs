namespace Streamus.Domain.Interfaces
{
    public interface IManagerFactory
    {
        IErrorManager GetErrorManager(IErrorDao errorDao);
        IPlaylistItemManager GetPlaylistItemManager(IPlaylistItemDao playlistItemDao, IVideoDao videoDao);
        IPlaylistManager GetPlaylistManager(IPlaylistDao playlistDao, IVideoDao videoDao);
        //  TODO: It's kind of weird this one depends on another manager.
        IShareCodeManager GetShareCodeManager(IPlaylistDao playlistDao, IShareCodeDao shareCodeDao, IPlaylistManager playlistManager);
        IUserManager GetUserManager(IUserDao userDao);
        IVideoManager GetVideoManager(IVideoDao videoDao);
    }
}