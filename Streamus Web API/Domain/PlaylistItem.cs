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
        public virtual Video Video { get; set; }

        public PlaylistItem()
        {
            Id = Guid.Empty;
            Title = string.Empty;
            Sequence = -1;
        }

        public PlaylistItem(string title, Video video)
            : this()
        {
            Title = title;
            Video = video;
        }

        public PlaylistItem(PlaylistItem playlistItem)
            : this()
        {
            Title = playlistItem.Title;
            Video = playlistItem.Video;
        }

        public PlaylistItem(Guid id, int sequence, string title, Playlist playlist, Video video)
            : this()
        {
            Id = id;
            Sequence = sequence;
            Title = title;
            Playlist = playlist;
            Video = video;
        }

        public virtual void ValidateAndThrow()
        {
            var validator = new PlaylistItemValidator();
            validator.ValidateAndThrow(this);
        }
    }
}