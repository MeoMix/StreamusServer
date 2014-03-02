using NHibernate;
using NHibernate.Criterion;
using Streamus.Domain;
using Streamus.Domain.Interfaces;
using System.Collections.Generic;

namespace Streamus.Dao
{
    public class VideoDao : AbstractNHibernateDao<Video>, IVideoDao
    {
        public VideoDao(ISession session)
            : base(session)
        {
            
        }

        public Video Get(string id)
        {
            Video video = null;

            if (id != default(string))
            {
                video = Session.Get<Video>(id);
            }

            return video;
        }

        public IList<Video> Get(List<string> ids)
        {
            IQueryOver<Video, Video> criteria = Session
                .QueryOver<Video>()
                .Where(video => video.Id.IsIn(ids));

            IList<Video> videos = criteria.List<Video>();

            return videos;
        }
    }
}