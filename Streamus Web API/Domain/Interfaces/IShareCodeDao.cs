namespace Streamus_Web_API.Domain.Interfaces
{
    public interface IShareCodeDao : IDao<ShareCode>
    {
        ShareCode GetByShortIdAndEntityTitle(string shareCodeShortId, string urlFriendlyEntityTitle);
    }
}
