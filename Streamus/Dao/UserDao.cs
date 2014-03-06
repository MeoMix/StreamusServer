using NHibernate;
using NHibernate.Criterion;
using Streamus.Domain;
using Streamus.Domain.Interfaces;
using System;

namespace Streamus.Dao
{
    public class UserDao : AbstractNHibernateDao<User>, IUserDao
    {
        public UserDao(ISession session)
            : base(session)
        {
            
        }

        public User Get(Guid id)
        {
            User user = null;

            if (id != default(Guid))
            {
                user = Session.Get<User>(id);
            }

            return user;
        }

        public User GetByGooglePlusId(string googlePlusId)
        {
            User user = null;

            if (googlePlusId != string.Empty)
            {
                ICriteria criteria = Session
                    .CreateCriteria(typeof (User), "User")
                    .Add(Restrictions.Eq("User.GooglePlusId", googlePlusId));

                user = criteria.UniqueResult<User>();
            }

            return user;
        }

        //  http://stackoverflow.com/questions/3390561/nhibernate-update-single-field-without-loading-entity
        public void UpdateGooglePlusId(Guid id, string googlePlusId)
        {
            Session.CreateQuery("update User set GooglePlusId = :googlePlusId where id = :id")
               .SetParameter("googlePlusId", googlePlusId)
               .SetParameter("id", id)
               .ExecuteUpdate();
        }
    }
}
