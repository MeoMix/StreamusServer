using NHibernate;
using NHibernate.Criterion;
using Streamus.Domain;
using Streamus.Domain.Interfaces;
using System;

namespace Streamus.Dao
{
    public class UserDao : AbstractNHibernateDao<User>, IUserDao
    {
        public User Get(Guid id)
        {
            User user = null;

            if (id != default(Guid))
            {
                user = NHibernateSession.Get<User>(id);
            }

            return user;
        }

        public User GetByGooglePlusId(string googlePlusId)
        {
            User user = null;

            if (googlePlusId != string.Empty)
            {
                ICriteria criteria = NHibernateSession
                    .CreateCriteria(typeof (User), "User")
                    .Add(Restrictions.Eq("User.GooglePlusId", googlePlusId));

                user = criteria.UniqueResult<User>();
            }

            return user;
        }
    }
}