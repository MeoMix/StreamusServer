using AutoMapper;
using FluentValidation;
using Streamus.Domain.Validators;
using Streamus.Dto;
using System;
using System.Collections.Generic;

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

        public static PlaylistItem Create(PlaylistItemDto playlistItemDto)
        {
            PlaylistItem playlistItem = Mapper.Map<PlaylistItemDto, PlaylistItem>(playlistItemDto);
            return playlistItem;
        }

        public static List<PlaylistItem> Create(IEnumerable<PlaylistItemDto> playlistItemDtos)
        {
            List<PlaylistItem> playlistItems = Mapper.Map<IEnumerable<PlaylistItemDto>, List<PlaylistItem>>(playlistItemDtos);
            return playlistItems;
        }

        public virtual void ValidateAndThrow()
        {
            var validator = new PlaylistItemValidator();
            validator.ValidateAndThrow(this);
        }
    }
}