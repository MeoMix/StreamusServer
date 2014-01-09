using Streamus.Dao;
using Streamus.Domain.Interfaces;
using System;

namespace Streamus.Domain.Managers
{
    /// <summary>
    ///     Provides a common spot for methods against Users which require transactions (Creating, Updating, Deleting)
    /// </summary>
    public class UserManager : AbstractManager
    {
        private IUserDao UserDao { get; set; }

        public UserManager()
        {
            UserDao = DaoFactory.GetUserDao();
        }

        /// <summary>
        ///     Creates a new User and saves it to the DB. As a side effect, also creates a new, empty
        ///     Folder (which has a new, empty Playlist) for the created User and saves it to the DB.
        /// </summary>
        /// <returns>The created user with a generated GUID</returns>
        public User CreateUser(string googlePlusId = "")
        {
            User user;

            try
            {
                NHibernateSessionManager.Instance.BeginTransaction();

                user = new User
                    {
                        GooglePlusId = googlePlusId
                    };

                user.ValidateAndThrow();
                UserDao.Save(user);

                NHibernateSessionManager.Instance.CommitTransaction();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                NHibernateSessionManager.Instance.RollbackTransaction();
                throw;
            }

            return user;
        }

        public void Save(User user)
        {
            try
            {
                NHibernateSessionManager.Instance.BeginTransaction();

                user.ValidateAndThrow();
                UserDao.Save(user);

                NHibernateSessionManager.Instance.CommitTransaction();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                NHibernateSessionManager.Instance.RollbackTransaction();
                throw;
            }
        }

        //  TODO: Should be able to just Save a user rather than have to update one property explicitly.
        public void UpdateGooglePlusId(Guid userId, string googlePlusId)
        {
            try
            {
                NHibernateSessionManager.Instance.BeginTransaction();
                User user = UserDao.Get(userId);
                user.GooglePlusId = googlePlusId;
                UserDao.Update(user);
                NHibernateSessionManager.Instance.CommitTransaction();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                NHibernateSessionManager.Instance.RollbackTransaction();
                throw;
            }
        }
    }
}