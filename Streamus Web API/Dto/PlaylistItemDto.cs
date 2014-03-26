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
        public string Title { get; set; }
        public string SourceId { get; set; }
        public SourceType SourceType { get; set; }
        public string SourceTitle { get; set; }
        public int Duration { get; set; }
        public string Author { get; set; }
        public bool HighDefinition { get; set; }

        public PlaylistItemDto()
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