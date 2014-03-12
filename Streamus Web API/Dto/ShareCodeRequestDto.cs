using System;

namespace Streamus_Web_API.Dto
{
    public class ShareCodeRequestDto
    {
        public Guid UserId { get; set; }
        public string ShortId { get; set; }
        public string UrlFriendlyEntityTitle { get; set; }
    }
}