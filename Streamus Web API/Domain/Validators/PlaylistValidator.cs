using FluentValidation;

namespace Streamus_Web_API.Domain.Validators
{
    public class PlaylistValidator : AbstractValidator<Playlist>
    {
        public PlaylistValidator()
        {
            //  TODO: It would be nice to ensure that ID can't change once it has been set.
            RuleFor(playlist => playlist.Title).NotNull().Length(1, 255);
            //  When sharing a playlist the user can be null and, in that case, it's OK for the sequence to not be set. Otherwise it needs to be.
            RuleFor(playlist => playlist.Sequence).GreaterThanOrEqualTo(0).When(playlist => playlist.User != null);
            RuleFor(playlist => playlist.Items).NotNull();
        }
    }
}