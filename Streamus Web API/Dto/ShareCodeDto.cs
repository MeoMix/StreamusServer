using AutoMapper;
using System;
using Streamus_Web_API.Domain;

namespace Streamus_Web_API.Dto
{
    public class ShareCodeDto
    {
        public Guid Id { get; set; }
        //  TODO: Broke my naming convention should be entityType on client and not entity
        public ShareableEntityType EntityType { get; set; }
        public Guid EntityId { get; set; }
        public string ShortId { get; set; }
        public string UrlFriendlyEntityTitle { get; set; }

        public ShareCodeDto()
        {
            Id = Guid.Empty;
            EntityId = Guid.Empty;
            EntityType = ShareableEntityType.None;
            ShortId = string.Empty;
            UrlFriendlyEntityTitle = string.Empty;
        }

        public static ShareCodeDto Create(ShareCode shareCode)
        {
            ShareCodeDto shareCodeDto = Mapper.Map<ShareCode, ShareCodeDto>(shareCode);
            return shareCodeDto;
        }
    }
}