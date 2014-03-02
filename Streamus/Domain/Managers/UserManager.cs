    using NHibernate;
using log4net;
using Streamus.Domain.Interfaces;
using System;

namespace Streamus.Domain.Managers
{
    /// <summary>
    ///     Provides a common spot for methods against Users which require transactions (Creating, Updating, Deleting)
    /// </summary>
    public class UserManager : StreamusManager, IUserManager
    {
        private IUserDao UserDao { get; set; }

        public UserManager(ILog logger, ISession session, IUserDao userDao) 
            : base(logger, session) 
        {
            UserDao = userDao;
        }

        public User Get(Guid id)
        {
            User user;

            try
            {
                using (ITransaction transaction = Session.BeginTransaction())
                {
                    user = UserDao.Get(id);

                    transaction.Commit();
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
                using (ITransaction transaction = Session.BeginTransaction())
                {
                    user = UserDao.GetByGooglePlusId(googlePlusId);

                    transaction.Commit();
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }

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
                using (ITransaction transaction = Session.BeginTransaction())
                {
                    user = new User
                        {
                            GooglePlusId = googlePlusId
                        };

                    user.ValidateAndThrow();
                    UserDao.Save(user);

                    transaction.Commit();
                }
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
                using (ITransaction transaction = Session.BeginTransaction())
                {
                    user.ValidateAndThrow();
                    UserDao.Save(user);

                    transaction.Commit();
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

        public void UpdateGooglePlusId(Guid userId, string googlePlusId)
        {
            try
            {
                using (ITransaction transaction = Session.BeginTransaction())
                {
                    UserDao.UpdateGooglePlusId(userId, googlePlusId);

                    transaction.Commit();
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

    }
}