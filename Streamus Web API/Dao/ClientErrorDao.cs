using NHibernate;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;

namespace Streamus_Web_API.Dao
{
  public class ClientErrorDao : AbstractNHibernateDao<ClientError>, IClientErrorDao
  {
    public ClientErrorDao(ISession session)
        : base(session)
    {

    }
  }
}