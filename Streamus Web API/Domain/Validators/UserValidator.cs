using FluentValidation;

namespace Streamus_Web_API.Domain.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Name).Length(0, 255);
            RuleFor(user => user.GooglePlusId).NotNull();
        }
    }
}