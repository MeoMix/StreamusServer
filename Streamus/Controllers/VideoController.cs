using NHibernate;
using log4net;
using Streamus.Domain;
using Streamus.Domain.Interfaces;
using Streamus.Dto;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Streamus.Controllers
{
    public class VideoController : StreamusController
    {
        private readonly IVideoManager VideoManager;

        public VideoController(ILog logger, ISession session, IManagerFactory managerFactory)
            : base(logger, session)
        {
            VideoManager = managerFactory.GetVideoManager();
        }

        /// <summary>
        ///     Save's a Video. It's a PUT because the video's ID will already
        ///     exist when coming from the client. Still need to decide whether
        ///     the item should be saved or updated, though.
        /// </summary>
        [HttpPut]
        public JsonResult Update(VideoDto videoDto)
        {
            Video video = Video.Create(videoDto);

            VideoManager.Save(video);

            VideoDto savedVideoDto = VideoDto.Create(video);

            return Json(savedVideoDto);
        }

        [HttpGet]
        public JsonResult Get(string id)
        {
            Video video = VideoManager.Get(id);
            VideoDto videoDto = VideoDto.Create(video);

            return Json(videoDto);
        }

        [HttpPost]
        public JsonResult SaveVideos(List<VideoDto> videoDtos)
        {
            List<Video> videos = Video.Create(videoDtos);

            VideoManager.Save(videos);
            return Json(videos);
        }

        [HttpGet]
        public JsonResult GetByIds(List<string> ids)
        {
            var videoDtos = new List<VideoDto>();

            //  The default model binder doesn't support passing an empty array as JSON to MVC controller, so check null.
            if (ids != null)
            {
                IList<Video> videos = VideoManager.Get(ids);
                videoDtos = VideoDto.Create(videos);
            }

            return Json(videoDtos);
        }
    }
}
