using NHibernate;

namespace Streamus_Web_API.Domain.Interfaces
{
    /// <summary>
    /// Provides an interface for retrieving DAO objects
    /// </summary>
    public interface IDaoFactory
    {
        IClientErrorDao GetErrorDao(ISession session);
        IPlaylistDao GetPlaylistDao(ISession session);
        IPlaylistItemDao GetPlaylistItemDao(ISession session);
        IShareCodeDao GetShareCodeDao(ISession session);
        IUserDao GetUserDao(ISession session);
    }
}
