using AutoMapper;
using Streamus_Web_API.Domain;
using System;
using System.Collections.Generic;

namespace Streamus_Web_API.Dto
{
    public class PlaylistDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid UserId { get; set; }
        public List<PlaylistItemDto> Items { get; set; }
        public double? Sequence { get; set; }

        public PlaylistDto()
        {
            Id = Guid.Empty;
            Items = new List<PlaylistItemDto>();
        }

        public static PlaylistDto Create(Playlist playlist)
        {
            PlaylistDto playlistDto = Mapper.Map<Playlist, PlaylistDto>(playlist);
            return playlistDto;
        }

        public void SetPatchableProperties(Playlist playlist)
        {
            if (Title != null)
                playlist.Title = Title;

            if (Sequence.HasValue)
                playlist.Sequence = (double)Sequence;
        }
    }
}