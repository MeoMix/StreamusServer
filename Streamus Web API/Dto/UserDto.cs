using System;
using System.Collections.Generic;
using AutoMapper;
using Streamus_Web_API.Domain;

namespace Streamus_Web_API.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string GooglePlusId { get; set; }
        public List<PlaylistDto> Playlists { get; set; }
        public string Language { get; set; }

        public UserDto()
        {
            Playlists = new List<PlaylistDto>();
        }

        public static UserDto Create(User user)
        {
            UserDto userDto = Mapper.Map<User, UserDto>(user);
            return userDto;
        }

        public void SetPatchableProperties(User user)
        {
            if (GooglePlusId != null)
                user.GooglePlusId = GooglePlusId;

            if (Language != null)
                user.Language = Language;
        }
    }
}