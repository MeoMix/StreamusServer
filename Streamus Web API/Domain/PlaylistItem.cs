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
        
        //  TODO: I don't think the Domain layer should know about this. It should be dumb to a client existing.
        //  Client ID is used to associate a DTO with a client-side entity which wasn't saved before sending to the server.
        public virtual string Cid { get; set; }

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
            Author = playlistItem.Author;
            SongType = playlistItem.SongType;
            SongTitle = playlistItem.SongTitle;
            Cid = playlistItem.Cid;
        }

        public PlaylistItem(Guid id, int sequence, string title, string cid, string songId, SongType songType, string songTitle, int duration, string author)
            : this(title, cid, songId, songType, songTitle, duration, author)
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