using FluentValidation;

namespace Streamus_Web_API.Domain.Validators
{
  public class UserValidator : AbstractValidator<User>
  {
    public UserValidator()
    {
      RuleFor(user => user.GooglePlusId).NotNull();
      RuleFor(user => user.Playlists).NotNull();
      RuleFor(user => user.Language).NotNull().Length(0, 10);
    }
  }
}