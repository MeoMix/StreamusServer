namespace Streamus.Domain.Interfaces
{
    public interface IShareCodeManager
    {
        ShareCode GetByShortIdAndEntityTitle(string shareCodeShortId, string urlFriendlyEntityTitle);
        ShareCode GetShareCode(IShareableEntity shareableEntity);
        void Save(ShareCode shareCode);
    }
}
