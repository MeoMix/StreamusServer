using FluentValidation;

namespace Streamus_Web_API.Domain.Validators
{
    public class PlaylistItemValidator : AbstractValidator<PlaylistItem>
    {
        public PlaylistItemValidator()
        {
            //  TODO: It would be nice to ensure that ID can't change once it has been set.
            RuleFor(playlistItem => playlistItem.Playlist).NotNull();
            RuleFor(playlistItem => playlistItem.Title).NotNull().Length(1, 255);
            RuleFor(playlistItem => playlistItem.Sequence).GreaterThanOrEqualTo(0);
            RuleFor(playlistItem => playlistItem.SongId).NotNull().Length(1, 255);
            RuleFor(playlistItem => playlistItem.SongType).NotEqual(SongType.None);
            RuleFor(playlistItem => playlistItem.SongTitle).NotNull().Length(1, 255);
            RuleFor(playlistItem => playlistItem.Duration).GreaterThan(0);
            RuleFor(playlistItem => playlistItem.Author).NotNull().Length(1, 255);
        }
    }
}