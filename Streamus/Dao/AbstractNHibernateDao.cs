using System;
using System.Collections.Generic;
using NHibernate;
using Streamus.Domain.Interfaces;

namespace Streamus.Dao
{
    public class AbstractNHibernateDao<T> : MarshalByRefObject, IDao<T> where T : class
    {
        protected ISession Session { get; set; }
        private readonly Type PersistentType = typeof (T);

        public AbstractNHibernateDao(ISession session)
        {
            Session = session;
        } 

        /// <summary>
        ///     Loads every instance of the requested type with no filtering.
        /// </summary>
        public List<T> GetAll()
        {
            ICriteria criteria = Session.CreateCriteria(PersistentType);
            return criteria.List<T>() as List<T>;
        }

        /// <summary>
        ///     For entities with automatatically generated IDs, such as identity, SaveOrUpdate may
        ///     be called when saving a new entity.  SaveOrUpdate can also be called to update any
        ///     entity, even if its ID is assigned. This method modifies the entity passed in.
        /// </summary>
        public void SaveOrUpdate(T entity)
        {
            Session.SaveOrUpdate(entity);
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
