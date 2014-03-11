using System;
using System.Collections.Generic;
using AutoMapper;
using Streamus_Web_API.Domain;

namespace Streamus_Web_API.Dto
{
    public class PlaylistItemDto
    {
        public Guid PlaylistId { get; set; }
        public Guid Id { get; set; }
        public int Sequence { get; set; }

        //  Store Title on PlaylistItem as well as on Video because user might want to rename PlaylistItem.
        public string Title { get; set; }
        public VideoDto Video { get; set; }

        //  TODO: Still necessary? I thought Backbone wouldn't drop a property if it wasn't found in response request?
        public string Cid { get; set; }

        public PlaylistItemDto()
        {
            Id = Guid.Empty;
            Title = string.Empty;
            Sequence = -1;
        }

        public static PlaylistItemDto Create(PlaylistItem playlistItem)
        {
            PlaylistItemDto playlistItemDto = Mapper.Map<PlaylistItem, PlaylistItemDto>(playlistItem);
            return playlistItemDto;
        }

        public static List<PlaylistItemDto> Create(IEnumerable<PlaylistItem> playlistItems)
        {
            List<PlaylistItemDto> playlistItemDtos = Mapper.Map<IEnumerable<PlaylistItem>, List<PlaylistItemDto>>(playlistItems);
            return playlistItemDtos;
        }
    }
}