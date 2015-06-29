using System.Collections.Generic;
using System.Linq;
using NHibernate;
using log4net;
using Streamus_Web_API.Domain.Interfaces;
using System;

namespace Streamus_Web_API.Domain.Managers
{
    /// <summary>
    ///     Provides a common spot for methods against Users which require transactions (Creating, Updating, Deleting)
    /// </summary>
    public class UserManager : StreamusManager, IUserManager
    {
        private IUserDao UserDao { get; set; }

        public UserManager(ILog logger, IUserDao userDao) 
            : base(logger) 
        {
            UserDao = userDao;
        }

        public User Get(Guid id)
        {
            User user;

            try
            {
                user = UserDao.Get(id);

                if (user == null)
                {
                    Logger.DebugFormat("Failed to find user with ID {0}. Creating new user with that ID.", id);

                    //  If failed to find by ID -- assume an error on my DB's part and gracefully fall back to a new user account since it is missing.
                    user = CreateUser();
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }

            return user;
        }

        public User GetByGooglePlusId(string googlePlusId)
        {
            User user;

            try
            {
                user = UserDao.GetByGooglePlusId(googlePlusId);
            }
            catch (NonUniqueResultException nonUniqueResultException)
            {
                Logger.Error(nonUniqueResultException);
                user = MergeAllByGooglePlusId(googlePlusId);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }

            return user;
        }

        public IList<User> GetAllByGooglePlusId(string googlePlusId)
        {
            IList<User> users;

            try
            {
                users = UserDao.GetAllByGooglePlusId(googlePlusId);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }

            return users;
        }

        public User MergeByGooglePlusId(Guid id, string googlePlusId)
        {
            User googlePlusUser = GetByGooglePlusId(googlePlusId);
            User user = Get(id);
            googlePlusUser.MergeUser(user);

            Update(googlePlusUser);
            UserDao.UpdateGooglePlusIds(new List<Guid> {id}, string.Empty);

            return googlePlusUser;
        }

        public User MergeAllByGooglePlusId(string googlePlusId)
        {
            IList<User> googlePlusUsers = GetAllByGooglePlusId(googlePlusId);

            //  Since I don't have an ID to merge into I will just merge into the first user and treat that as the qualified one.
            User user = googlePlusUsers[0];
            googlePlusUsers.RemoveAt(0);

            foreach (User googlePlusUser in googlePlusUsers)
            {
                user.MergeUser(googlePlusUser);
            }

            Update(user);
            ClearGooglePlusId(googlePlusUsers.Select(u => u.Id).ToList());

            return user;
        }

        /// <summary>
        ///     Creates a new User and saves it to the DB. As a side effect, also creates a new, empty
        ///     Playlist and also saves it to the DB.
        /// </summary>
        /// <returns>The created user with a generated GUID</returns>
        public User CreateUser(string googlePlusId = "")
        {
            User user;

            try
            {
                user = new User
                    {
                        GooglePlusId = googlePlusId
                    };

                user.ValidateAndThrow();
                UserDao.Save(user);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }

            return user;
        }

        public void Save(User user)
        {
            try
            {
                user.ValidateAndThrow();
                UserDao.Save(user);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

        public void Update(User user)
        {
            try
            {
                user.ValidateAndThrow();
                UserDao.Update(user);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

        private void ClearGooglePlusId(IList<Guid> ids)
        {
            try
            {
                UserDao.UpdateGooglePlusIds(ids, string.Empty);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }
    }
}
