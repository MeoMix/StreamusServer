using FluentValidation;
using Streamus.Domain.Interfaces;
using Streamus.Domain.Validators;
using Streamus.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Streamus.Domain
{
    public class PlaylistItem : AbstractDomainEntity<Guid>
    {
        public virtual Playlist Playlist { get; set; }
        public virtual int Sequence { get; set; }
        public virtual string Title { get; set; }
        public virtual Video Video { get; set; }

        //  Not written to the database. Used for client to tell who is who after a save.
        public virtual string Cid { get; set; }

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

        public static PlaylistItem Create(PlaylistItemDto playlistItemDto, IPlaylistManager playlistManager)
        {
            PlaylistItem playlistItem = new PlaylistItem
                {
                    Cid = playlistItemDto.Cid,
                    Id = playlistItemDto.Id,
                    Playlist = playlistManager.Get(playlistItemDto.PlaylistId),
                    Sequence = playlistItemDto.Sequence,
                    Title = playlistItemDto.Title,
                    Video = Video.Create(playlistItemDto.Video)
                };

            return playlistItem;
        }

        public static List<PlaylistItem> Create(IEnumerable<PlaylistItemDto> playlistItemDtos, IPlaylistManager playlistManager)
        {
            List<PlaylistItem> playlistItems = new List<PlaylistItem>(playlistItemDtos.Select(pid => Create(pid, playlistManager)));
            return playlistItems;
        }

        public virtual void ValidateAndThrow()
        {
            var validator = new PlaylistItemValidator();
            validator.ValidateAndThrow(this);
        }
    }
}