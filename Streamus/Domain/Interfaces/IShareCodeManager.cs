using System;

namespace Streamus.Domain.Interfaces
{
    public interface IShareCodeManager
    {
        ShareCode GetShareCode(ShareableEntityType entityType, Guid entityId);
        void Save(ShareCode shareCode);
    }
}
