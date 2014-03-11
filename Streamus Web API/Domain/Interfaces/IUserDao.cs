using System;

namespace Streamus_Web_API.Domain.Interfaces
{
    public interface IUserDao : IDao<User>
    {
        User Get(Guid id);
        User GetByGooglePlusId(string googlePlusId);
        void UpdateGooglePlusId(Guid id, string googlePlusId);
    }
}
