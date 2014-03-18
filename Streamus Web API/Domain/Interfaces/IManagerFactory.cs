using NHibernate;

namespace Streamus_Web_API.Domain.Interfaces
{
    public interface IManagerFactory
    {
        IClientErrorManager GetErrorManager(ISession session);
        IPlaylistItemManager GetPlaylistItemManager(ISession session);
        IPlaylistManager GetPlaylistManager(ISession session);
        IShareCodeManager GetShareCodeManager(ISession session);
        IUserManager GetUserManager(ISession session);
    }
}