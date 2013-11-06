using FluentValidation;

namespace Streamus.Domain.Validators
{
    public class PlaylistItemValidator : AbstractValidator<PlaylistItem>
    {
        public PlaylistItemValidator()
        {
            RuleFor(playlistItem => playlistItem.Playlist).NotNull();
            RuleFor(playlistItem => playlistItem.Video).NotNull();
            RuleFor(playlistItem => playlistItem.Sequence).GreaterThanOrEqualTo(0);
            //RuleFor(playlistItem => playlistItem.NextItem).NotNull();
            //RuleFor(playlistItem => playlistItem.PreviousItem).NotNull();
        }
    }
}