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

        public virtual string SourceId { get; set; }
        public virtual SourceType SourceType { get; set; }
        public virtual int Duration { get; set; }
        public virtual string Author { get; set; }
        public virtual bool HighDefinition { get; set; }
        public virtual string SourceTitle { get; set; }

        public PlaylistItem()
        {
            Id = Guid.Empty;
            Title = string.Empty;
            Sequence = -1;
            SourceId = string.Empty;
            Title = string.Empty;
            Author = string.Empty;
            SourceType = SourceType.None;
            SourceTitle = string.Empty;
        }

        public PlaylistItem(string title, string sourceId, SourceType sourceType, string sourceTitle, int duration, string author)
            : this()
        {
            Title = title;
            SourceId = sourceId;
            SourceType = sourceType;
            SourceTitle = sourceTitle;
            Duration = duration;
            Author = author;
        }

        public PlaylistItem(PlaylistItem playlistItem)
            : this()
        {
            Title = playlistItem.Title;
            SourceId = playlistItem.SourceId;
            Author = playlistItem.Author;
            SourceType = playlistItem.SourceType;
            SourceTitle = playlistItem.SourceTitle;
        }

        public PlaylistItem(Guid id, int sequence, string title, string sourceId, SourceType sourceType, string sourceTitle, int duration, string author)
            : this(title, sourceId, sourceType, sourceTitle, duration, author)
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