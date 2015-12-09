using FluentValidation;
using Streamus_Web_API.Domain.Validators;
using System;

namespace Streamus_Web_API.Domain
{
    public class PlaylistItem : AbstractDomainEntity<Guid>
    {
        public virtual Playlist Playlist { get; set; }
        public virtual double Sequence { get; set; }

        public virtual string VideoId { get; set; }
        public virtual VideoType VideoType { get; set; }
        public virtual string VideoTitle { get; set; }
        public virtual int Duration { get; set; }
        public virtual string Author { get; set; }

        //  Client ID is used to associate a DTO with a client-side entity which wasn't saved before sending to the server.
        public virtual string Cid { get; set; }

        public const int MaxAuthorLength = 100;

        //  The longest YouTube ID currently is 11, but I'm doing 25 just in case that grows in the future.
        public const int MaxVideoIdLength = 25;
        //  The longest VideoType is currently 10, SoundCloud, but I'm doing 25 just in case that grows in the future.
        public const int MaxVideoTypeLength = 25;
        public const int MaxVideoTitleLength = 255;

        public PlaylistItem()
        {
            Id = Guid.Empty;
            Sequence = -1;
            VideoId = string.Empty;
            Author = string.Empty;
            VideoType = VideoType.None;
            VideoTitle = string.Empty;
            Cid = string.Empty;
        }

        public PlaylistItem(string cid, string videoId, VideoType videoType, string videoTitle, int duration, string author)
            : this()
        {
            VideoId = videoId;
            VideoType = videoType;
            VideoTitle = videoTitle;
            Duration = duration;
            Author = author;
            Cid = cid;
        }

        public PlaylistItem(PlaylistItem playlistItem)
            : this()
        {
            VideoId = playlistItem.VideoId;
            VideoType = playlistItem.VideoType;
            VideoTitle = playlistItem.VideoTitle;
            Author = playlistItem.Author;
            Cid = playlistItem.Cid;
            Duration = playlistItem.Duration;
            Sequence = playlistItem.Sequence;
        }

        public PlaylistItem(Guid id, string cid, string videoId, VideoType videoType, string videoTitle, int duration, string author)
            : this(cid, videoId, videoType, videoTitle, duration, author)
        {
            Id = id;
        }

        public virtual void ValidateAndThrow()
        {
            var validator = new PlaylistItemValidator();
            validator.ValidateAndThrow(this);
        }
    }
}