using NHibernate;
using Streamus_Web_API.Domain.Interfaces;

namespace Streamus_Web_API.Dao
{
  public class AbstractNHibernateDao<T> : IDao<T> where T : class
  {
    protected ISession Session { get; set; }

    public AbstractNHibernateDao(ISession session)
    {
      Session = session;
    }

    public T Merge(T entity)
    {
      return Session.Merge(entity);
    }

    public void Save(T entity)
    {
      Session.Save(entity);
    }

    public void Update(T entity)
    {
      Session.Update(entity);
    }

    public void Delete(T entity)
    {
      Session.Delete(entity);
    }
  }
}
