using NHibernate;
using NHibernate.Criterion;
using Streamus.Domain;
using Streamus.Domain.Interfaces;

namespace Streamus.Dao
{
    public class ShareCodeDao : AbstractNHibernateDao<ShareCode>, IShareCodeDao
    {
        public ShareCodeDao(ISession session)
            : base(session)
        {
            
        }

        public ShareCode GetByShortIdAndEntityTitle(string shareCodeShortId, string urlFriendlyEntityTitle)
        {
            ICriteria criteria = Session
                .CreateCriteria(typeof (ShareCode), "ShareCode")
                .Add(Restrictions.Eq("ShareCode.ShortId", shareCodeShortId))
                .Add(Restrictions.Eq("ShareCode.UrlFriendlyEntityTitle", urlFriendlyEntityTitle));

            var shareCode = criteria.UniqueResult<ShareCode>();

            return shareCode;
        }
    }
}