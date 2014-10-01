using FluentValidation;

namespace Streamus_Web_API.Domain.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            //  TODO: Remove the Name property since it is unused.
            //  TODO: It would be nice to ensure that ID can't change once it has been set.
            RuleFor(user => user.Name).Length(0, 255);
            RuleFor(user => user.GooglePlusId).NotNull();
            RuleFor(user => user.Playlists).NotNull();
        }
    }
}