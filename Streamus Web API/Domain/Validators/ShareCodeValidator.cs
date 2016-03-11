using System;
using FluentValidation;

namespace Streamus_Web_API.Domain.Validators
{
  public class ShareCodeValidator : AbstractValidator<ShareCode>
  {
    public ShareCodeValidator()
    {
      RuleFor(shareCode => shareCode.EntityType).NotEqual(EntityType.None);
      RuleFor(shareCode => shareCode.EntityId).NotEqual(Guid.Empty);
      RuleFor(shareCode => shareCode.ShortId).Length(12);
      RuleFor(shareCode => shareCode.UrlFriendlyEntityTitle.Length).GreaterThan(0);
      RuleFor(shareCode => shareCode.UrlFriendlyEntityTitle.IndexOf(" "))
          .Equal(-1)
          .WithName("UrlFriendlyEntityTitle");
    }
  }
}