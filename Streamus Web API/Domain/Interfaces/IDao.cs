using System.Collections.Generic;

namespace Streamus_Web_API.Domain.Interfaces
{
    public interface IDao<T>
    {
        List<T> GetAll();
        void SaveOrUpdate(T entity);
        T Merge(T entity);
        void Save(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
