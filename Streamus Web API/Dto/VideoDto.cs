using System.Collections.Generic;
using AutoMapper;
using Streamus_Web_API.Domain;

namespace Streamus_Web_API.Dto
{
    public class VideoDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public string Author { get; set; }
        public bool HighDefinition { get; set; }

        public VideoDto()
        {
            Id = string.Empty;
            Title = string.Empty;
            Author = string.Empty;
        }

        /// <summary>
        /// Converts a Domain object to a DTO
        /// </summary>
        public static VideoDto Create(Video video)
        {
            VideoDto videoDto = Mapper.Map<Video, VideoDto>(video);
            return videoDto;
        }

        public static List<VideoDto> Create(IEnumerable<Video> videos)
        {
            List<VideoDto> videoDtos = Mapper.Map<IEnumerable<Video>, List<VideoDto>>(videos);
            return videoDtos;
        }
    }
}