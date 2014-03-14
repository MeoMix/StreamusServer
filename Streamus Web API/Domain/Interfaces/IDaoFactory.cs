namespace Streamus_Web_API.Domain.Interfaces
{
    /// <summary>
    /// Provides an interface for retrieving DAO objects
    /// </summary>
    public interface IDaoFactory
    {
        IClientErrorDao GetErrorDao();
        IPlaylistDao GetPlaylistDao();
        IPlaylistItemDao GetPlaylistItemDao();
        IShareCodeDao GetShareCodeDao();
        IUserDao GetUserDao();
        IVideoDao GetVideoDao();
    }
}
