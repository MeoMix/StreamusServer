using NHibernate;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;

namespace Streamus_Web_API.Dao
{
    public class ErrorDao : AbstractNHibernateDao<Error>, IErrorDao
    {
        public ErrorDao(ISession session)
            : base(session)
        {
            
        }
    }
}