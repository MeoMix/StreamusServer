using Streamus_Web_API.Domain;

namespace Streamus_Web_API.Dto
{
    public class SongDto
    {
        public string Id { get; set; }
        public SongType Type { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public string Author { get; set; }

        public SongDto()
        {
            Id = string.Empty;
            Title = string.Empty;
            Author = string.Empty;
            Type = SongType.None;
        }
    }
}