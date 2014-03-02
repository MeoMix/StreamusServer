using AutoMapper;
using FluentValidation;
using Streamus.Domain.Interfaces;
using Streamus.Domain.Validators;
using System;
using Streamus.Dto;

namespace Streamus.Domain
{
    public enum ShareableEntityType
    {
        None = -1,
        Playlist = 0
    }

    public class ShareCode : AbstractDomainEntity<Guid>
    {
        public virtual ShareableEntityType EntityType { get; set; }
        public virtual Guid EntityId { get; set; }
        public virtual string ShortId { get; set; }
        public virtual string UrlFriendlyEntityTitle { get; set; }

        public ShareCode()
        {
            Id = Guid.Empty;
            EntityId = Guid.Empty;
            EntityType = ShareableEntityType.None;
            ShortId = string.Empty;
            UrlFriendlyEntityTitle = string.Empty;
        }

        public ShareCode(IShareableEntity shareableEntity)
            : this()
        {
            if (shareableEntity is Playlist)
            {
                EntityType = ShareableEntityType.Playlist;
            }

            EntityId = shareableEntity.Id;
            UrlFriendlyEntityTitle = shareableEntity.GetUrlFriendlyTitle();
            ShortId = shareableEntity.GetShortId();
        }

        public static ShareCode Create(ShareCodeDto shareCodeDto)
        {
            ShareCode shareCode = Mapper.Map<ShareCodeDto, ShareCode>(shareCodeDto);
            return shareCode;
        }

        public virtual void ValidateAndThrow()
        {
            var validator = new ShareCodeValidator();
            validator.ValidateAndThrow(this);
        }

    }
}