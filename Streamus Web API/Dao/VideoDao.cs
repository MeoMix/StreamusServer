using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;

namespace Streamus_Web_API.Dao
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
                video = Session.Load<Video>(id);
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