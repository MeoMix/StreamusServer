using log4net;
using Streamus.Domain;
using Streamus.Domain.Interfaces;
using Streamus.Dto;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Streamus.Controllers
{
    [SessionManagement]
    public class VideoController : AbstractController
    {
        private readonly IVideoManager VideoManager;

        public VideoController(ILog logger, IManagerFactory managerFactory)
            : base(logger)
        {
            try
            {
                VideoManager = managerFactory.GetVideoManager();
            }
            catch (TypeInitializationException exception)
            {
                Logger.Error(exception.InnerException);
                throw exception.InnerException;
            }
        }

        /// <summary>
        ///     Save's a Video. It's a PUT because the video's ID will already
        ///     exist when coming from the client. Still need to decide whether
        ///     the item should be saved or updated, though.
        /// </summary>
        [HttpPut]
        public ActionResult Update(VideoDto videoDto)
        {
            Video video = Video.Create(videoDto);

            VideoManager.Save(video);

            VideoDto savedVideoDto = VideoDto.Create(video);

            return new JsonServiceStackResult(savedVideoDto);
        }

        [HttpGet]
        public ActionResult Get(string id)
        {
            Video video = VideoManager.Get(id);
            VideoDto videoDto = VideoDto.Create(video);

            return new JsonServiceStackResult(videoDto);
        }

        [HttpPost]
        public ActionResult SaveVideos(List<VideoDto> videoDtos)
        {
            List<Video> videos = Video.Create(videoDtos);

            VideoManager.Save(videos);
            return new JsonServiceStackResult(videos);
        }

        [HttpGet]
        public ActionResult GetByIds(List<string> ids)
        {
            var videoDtos = new List<VideoDto>();

            //  The default model binder doesn't support passing an empty array as JSON to MVC controller, so check null.
            if (ids != null)
            {
                IList<Video> videos = VideoManager.Get(ids);
                videoDtos = VideoDto.Create(videos);
            }

            return new JsonServiceStackResult(videoDtos);
        }
    }
}
