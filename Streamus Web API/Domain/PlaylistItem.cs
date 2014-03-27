using FluentValidation;
using Streamus_Web_API.Domain.Validators;
using System;

namespace Streamus_Web_API.Domain
{
    public class PlaylistItem : AbstractDomainEntity<Guid>
    {
        public virtual Playlist Playlist { get; set; }
        public virtual int Sequence { get; set; }
        public virtual string Title { get; set; }

        public virtual string SongId { get; set; }
        public virtual SongType SongType { get; set; }
        public virtual string SongTitle { get; set; }
        public virtual int Duration { get; set; }
        public virtual string Author { get; set; }
        public virtual bool HighDefinition { get; set; }

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
        }

        public PlaylistItem(string title, string songId, SongType songType, string songTitle, int duration, string author)
            : this()
        {
            Title = title;
            SongId = songId;
            SongType = songType;
            SongTitle = songTitle;
            Duration = duration;
            Author = author;
        }

        public PlaylistItem(PlaylistItem playlistItem)
            : this()
        {
            Title = playlistItem.Title;
            SongId = playlistItem.SongId;
            Author = playlistItem.Author;
            SongType = playlistItem.SongType;
            SongTitle = playlistItem.SongTitle;
        }

        public PlaylistItem(Guid id, int sequence, string title, string songId, SongType songType, string songTitle, int duration, string author)
            : this(title, songId, songType, songTitle, duration, author)
        {
            Id = id;
            Sequence = sequence;
        }

        public virtual void ValidateAndThrow()
        {
            var validator = new PlaylistItemValidator();
            validator.ValidateAndThrow(this);
        }
    }
}