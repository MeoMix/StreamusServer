using NHibernate;
using Streamus.Domain;
using Streamus.Domain.Interfaces;

namespace Streamus.Dao
{
    public class ErrorDao : AbstractNHibernateDao<Error>, IErrorDao
    {
        public ErrorDao(ISession session)
            : base(session)
        {
            
        }
    }
}