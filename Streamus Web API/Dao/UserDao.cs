using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using System;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;

namespace Streamus_Web_API.Dao
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
            .CreateCriteria(typeof(User), "User")
            .Add(Restrictions.Eq("User.GooglePlusId", googlePlusId));

        user = criteria.UniqueResult<User>();
      }

      return user;
    }

    public IList<User> GetAllByGooglePlusId(string googlePlusId)
    {
      IList<User> users = new List<User>();

      if (googlePlusId != string.Empty)
      {
        ICriteria criteria = Session
            .CreateCriteria(typeof(User), "User")
            .Add(Restrictions.Eq("User.GooglePlusId", googlePlusId));

        users = criteria.List<User>();
      }

      return users;
    }

    //  http://stackoverflow.com/questions/3390561/nhibernate-update-single-field-without-loading-entity
    public void UpdateGooglePlusIds(IList<Guid> ids, string googlePlusId)
    {
      Session.CreateQuery("update User set GooglePlusId = :googlePlusId where id in (:ids)")
         .SetParameter("googlePlusId", googlePlusId)
         .SetParameterList("ids", ids)
         .ExecuteUpdate();
    }
  }
}
