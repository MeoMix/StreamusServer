using System;

namespace Streamus_Web_API.Domain.Interfaces
{
    public interface IShareableEntity
    {
        Guid Id { get; set; }
        string GetUrlFriendlyTitle();
        string GetShortId();
    }
}
