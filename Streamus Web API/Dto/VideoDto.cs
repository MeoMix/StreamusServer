using Streamus_Web_API.Domain;

namespace Streamus_Web_API.Dto
{
    public class VideoDto
    {
        public string Id { get; set; }
        public VideoType Type { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public string Author { get; set; }

        public VideoDto()
        {
            Id = string.Empty;
            Title = string.Empty;
            Author = string.Empty;
            Type = VideoType.None;
        }
    }
}