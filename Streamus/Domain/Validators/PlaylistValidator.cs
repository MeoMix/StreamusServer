using FluentValidation;

namespace Streamus.Domain.Validators
{
    public class PlaylistValidator : AbstractValidator<Playlist>
    {
        public PlaylistValidator()
        {
            RuleFor(playlist => playlist.Title).Length(0, 255);
        }
    }
}