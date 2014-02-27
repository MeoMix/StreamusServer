using System;

namespace Streamus.Domain.Interfaces
{
    public interface IShareCodeManager
    {
        ShareCode GetByShortIdAndEntityTitle(string shareCodeShortId, string urlFriendlyEntityTitle);
        ShareCode GetShareCode(ShareableEntityType entityType, Guid entityId);
        void Save(ShareCode shareCode);
    }
}
