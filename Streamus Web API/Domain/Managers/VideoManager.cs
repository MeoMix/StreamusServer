using log4net;
using Streamus_Web_API.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Streamus_Web_API.Domain.Managers
{
    public class VideoManager : StreamusManager, IVideoManager
    {
        private IVideoDao VideoDao { get; set; }

        public VideoManager(ILog logger, IVideoDao videoDao)
            : base(logger) 
        {
            VideoDao = videoDao;
        }

        public Video Get(string id)
        {
            Video video;

            try
            {
                video = VideoDao.Get(id);
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
                videos = VideoDao.Get(ids);
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
                DoSave(video);
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
                videosList.ForEach(DoSave);
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
