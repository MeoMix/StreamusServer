using FluentValidation;

namespace Streamus_Web_API.Domain.Validators
{
    public class PlaylistItemValidator : AbstractValidator<PlaylistItem>
    {
        public PlaylistItemValidator()
        {
            RuleFor(playlistItem => playlistItem.Playlist).NotNull();
            RuleFor(playlistItem => playlistItem.Sequence).GreaterThanOrEqualTo(0);
        }
    }
}