using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using AutoMapper;
using Streamus.Domain;

namespace Streamus.Dto
{
    [DataContract]
    public class PlaylistDto
    {
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "folderId")]
        public Guid FolderId { get; set; }

        [DataMember(Name = "items")]
        public List<PlaylistItemDto> Items { get; set; }

        [DataMember(Name = "sequence")]
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