﻿using System;

namespace Streamus_Web_API.Domain.Interfaces
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
        void Update(User user);
        User MergeByGooglePlusId(Guid id, string googlePlusId);
        User MergeAllByGooglePlusId(string googlePlusId);

        User Get(Guid userId);
        User GetByGooglePlusId(string googlePlusId);
    }
}
