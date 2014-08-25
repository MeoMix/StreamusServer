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
        public double Sequence { get; set; }
        public string Title { get; set; }
        //  Client ID is used to associate a DTO with a client-side entity which wasn't saved before sending to the server.
        public string Cid { get; set; }
        public SongDto Song { get; set; }

        public PlaylistItemDto()
        {
            Id = Guid.Empty;
            Title = string.Empty;
            Sequence = -1;
            Title = string.Empty;
            Cid = string.Empty;
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