using System;
using System.Collections.Generic;
using AutoMapper;
using Streamus_Web_API.Domain;

namespace Streamus_Web_API.Dto
{
    public class PlaylistDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid UserId { get; set; }
        public List<PlaylistItemDto> Items { get; set; }
        public int Sequence { get; set; }

        public PlaylistDto()
        {
            Id = Guid.Empty;
            Title = string.Empty;
            Items = new List<PlaylistItemDto>();
        }

        public static PlaylistDto Create(Playlist playlist)
        {
            PlaylistDto playlistDto = Mapper.Map<Playlist, PlaylistDto>(playlist);
            return playlistDto;
        }
    }
}