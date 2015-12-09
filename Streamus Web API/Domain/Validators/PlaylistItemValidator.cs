using FluentValidation;

namespace Streamus_Web_API.Domain.Validators
{
    public class PlaylistItemValidator : AbstractValidator<PlaylistItem>
    {
        public PlaylistItemValidator()
        {
            RuleFor(playlistItem => playlistItem.Playlist).NotNull();
            RuleFor(playlistItem => playlistItem.Sequence).GreaterThanOrEqualTo(0);
            RuleFor(playlistItem => playlistItem.VideoId).NotNull().Length(1, 255);
            RuleFor(playlistItem => playlistItem.VideoType).NotEqual(VideoType.None);
            RuleFor(playlistItem => playlistItem.VideoTitle).NotNull().Length(1, 255);
            RuleFor(playlistItem => playlistItem.Duration).GreaterThan(0);
            RuleFor(playlistItem => playlistItem.Author).NotNull().Length(1, 255);
        }
    }
}