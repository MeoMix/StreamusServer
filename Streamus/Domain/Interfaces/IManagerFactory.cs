namespace Streamus.Domain.Interfaces
{
    public interface IManagerFactory
    {
        IErrorManager GetErrorManager();
        IPlaylistItemManager GetPlaylistItemManager();
        IPlaylistManager GetPlaylistManager();
        IShareCodeManager GetShareCodeManager();
        IUserManager GetUserManager();
        IVideoManager GetVideoManager();
    }
}