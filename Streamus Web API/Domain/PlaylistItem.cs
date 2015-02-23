using FluentValidation;
using Streamus_Web_API.Domain.Validators;
using System;

namespace Streamus_Web_API.Domain
{
    public class PlaylistItem : AbstractDomainEntity<Guid>
    {
        public virtual Playlist Playlist { get; set; }
        public virtual double Sequence { get; set; }
        public virtual string Title { get; set; }

        public virtual string SongId { get; set; }
        public virtual SongType SongType { get; set; }
        public virtual string SongTitle { get; set; }
        public virtual int Duration { get; set; }
        public virtual string Author { get; set; }
        
        //  TODO: I don't think the Domain layer should know about this. It should be dumb to a client existing.
        //  Client ID is used to associate a DTO with a client-side entity which wasn't saved before sending to the server.
        public virtual string Cid { get; set; }

        public const int MaxTitleLength = 255;
        public const int MaxAuthorLength = 100;

        //  The longest YouTube ID currently is 11, but I'm doing 25 just in case that grows in the future.
        public const int MaxSongIdLength = 25;
        //  The longest SongType is currently 10, SoundCloud, but I'm doing 25 just in case that grows in the future.
        public const int MaxSongTypeLength = 25;
        public const int MaxSongTitleLength = 255;

        public PlaylistItem()
        {
            Id = Guid.Empty;
            Title = string.Empty;
            Sequence = -1;
            SongId = string.Empty;
            Title = string.Empty;
            Author = string.Empty;
            SongType = SongType.None;
            SongTitle = string.Empty;
            Cid = string.Empty;
        }

        public PlaylistItem(string title, string cid, string songId, SongType songType, string songTitle, int duration, string author)
            : this()
        {
            Title = title;
            SongId = songId;
            SongType = songType;
            SongTitle = songTitle;
            Duration = duration;
            Author = author;
            Cid = cid;
        }

        public PlaylistItem(PlaylistItem playlistItem)
            : this()
        {
            Title = playlistItem.Title;
            SongId = playlistItem.SongId;
            SongType = playlistItem.SongType;
            SongTitle = playlistItem.SongTitle;
            Author = playlistItem.Author;
            Cid = playlistItem.Cid;
            Duration = playlistItem.Duration;
            Sequence = playlistItem.Sequence;
        }

        public PlaylistItem(Guid id, string title, string cid, string songId, SongType songType, string songTitle, int duration, string author)
            : this(title, cid, songId, songType, songTitle, duration, author)
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