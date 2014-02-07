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

                //  If failed to find by ID -- assume an error on my DB's part and gracefully fall back to a new user account since it is missing.
                if (user == null)
                {
                    user = new User
                        {
                            Id = id
                        };
                    Save(user);
                }

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

        //  http://stackoverflow.com/questions/3390561/nhibernate-update-single-field-without-loading-entity
        public void UpdateGooglePlusId(Guid id, string googlePlusId)
        {
            NHibernateSession.CreateQuery("update User set GooglePlusId = :googlePlusId where id = :id")
               .SetParameter("googlePlusId", googlePlusId)
               .SetParameter("id", id)
               .ExecuteUpdate();
        }
    }
}