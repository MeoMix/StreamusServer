namespace Streamus_Web_API.Domain.Interfaces
{
    public interface IManagerFactory
    {
        IClientErrorManager GetErrorManager();
        IPlaylistItemManager GetPlaylistItemManager();
        IPlaylistManager GetPlaylistManager();
        IShareCodeManager GetShareCodeManager();
        IUserManager GetUserManager();
    }
}