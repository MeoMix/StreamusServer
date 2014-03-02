using log4net;
using NHibernate;
using Streamus.Dao;
using Streamus.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Streamus.Domain.Managers
{
    public class VideoManager : StreamusManager, IVideoManager
    {
        private IVideoDao VideoDao { get; set; }

        public VideoManager(ILog logger, ISession session, IVideoDao videoDao)
            : base(logger, session) 
        {
            VideoDao = videoDao;
        }

        public Video Get(string id)
        {
            Video video;

            try
            {
                using (ITransaction transaction = Session.BeginTransaction())
                {
                    video = VideoDao.Get(id);
                    transaction.Commit();
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }

            return video;
        }

        public IList<Video> Get(List<string> ids)
        {
            IList<Video> videos;

            try
            {
                using (ITransaction transaction = Session.BeginTransaction())
                {
                    videos = VideoDao.Get(ids);
                    transaction.Commit();
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }

            return videos;
        }

        public void Save(Video video)
        {
            try
            {
                using (ITransaction transaction = Session.BeginTransaction())
                {
                    DoSave(video);
                    transaction.Commit();
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

        public void Save(IEnumerable<Video> videos)
        {
            try
            {
                List<Video> videosList = videos.ToList();

                if (videosList.Count > 1000)
                {
                    Session.SetBatchSize(videosList.Count / 10);
                }
                else if (videosList.Count > 3)
                {
                    Session.SetBatchSize(videosList.Count / 3);
                }

                using (ITransaction transaction = Session.BeginTransaction())
                {
                    videosList.ForEach(DoSave);

                    transaction.Commit();
                }

                Session.SetBatchSize(0);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

        private void DoSave(Video video)
        {
            video.ValidateAndThrow();

            //  Merge instead of SaveOrUpdate because Video's ID is assigned, but the same Video
            //  entity can be referenced by many different Playlists. As such, it is common to have the entity
            //  loaded into the cache.
            VideoDao.Merge(video);
        }
    }
}