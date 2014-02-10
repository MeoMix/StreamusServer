namespace Streamus.Domain.Interfaces
{
    /// <summary>
    /// Provides an interface for retrieving DAO objects
    /// </summary>
    public interface IDaoFactory
    {
        IErrorDao GetErrorDao();
        IPlaylistDao GetPlaylistDao();
        IPlaylistItemDao GetPlaylistItemDao();
        IShareCodeDao GetShareCodeDao();
        IUserDao GetUserDao();
        IVideoDao GetVideoDao();
    }
}
