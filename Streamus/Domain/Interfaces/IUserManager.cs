using System;

namespace Streamus.Domain.Interfaces
{
    public interface IUserManager
    {
        /// <summary>
        ///     Creates a new User and saves it to the DB. As a side effect, also creates a new, empty
        ///     Playlist and also saves it to the DB.
        /// </summary>
        /// <returns>The created user with a generated GUID</returns>
        User CreateUser(string googlePlusId = "");

        void Save(User user);
        void UpdateGooglePlusId(Guid userId, string googlePlusId);

        User Get(Guid userId);
        User GetByGooglePlusId(string googlePlusId);
    }
}
