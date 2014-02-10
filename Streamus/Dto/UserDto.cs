using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using AutoMapper;
using Streamus.Domain;

namespace Streamus.Dto
{
    [DataContract]
    public class UserDto
    {
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [DataMember(Name = "googlePlusId")]
        public string GooglePlusId { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "playlists")]
        public List<PlaylistDto> Playlists { get; set; }

        public UserDto()
        {
            Name = string.Empty;
            GooglePlusId = string.Empty;
            Playlists = new List<PlaylistDto>();
        }

        public static UserDto Create(User user)
        {
            UserDto userDto = Mapper.Map<User, UserDto>(user);
            return userDto;
        }
    }
}