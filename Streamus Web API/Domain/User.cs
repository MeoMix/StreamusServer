using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Streamus_Web_API.Domain.Validators;

namespace Streamus_Web_API.Domain
{
  public class User : AbstractDomainEntity<Guid>
  {
    public virtual string GooglePlusId { get; set; }
    //  Use interfaces so NHibernate can inject with its own collection implementation.
    public virtual ICollection<Playlist> Playlists { get; set; }
    public virtual string Language { get; set; }

    //  GooglePlusID is usually a number, but, in some instances, can be a gmail address which is maximum of 75 characters, but doing 100 to be safe.
    public const int MaxGooglePlusIdLength = 100;
    public const int MaxLanguageLength = 10;

    public User()
    {
      GooglePlusId = string.Empty;
      Playlists = new List<Playlist>();
      Language = string.Empty;

      //  A user should always have at least one Playlist.
      CreateAndAddPlaylist();
    }

    public virtual Playlist CreateAndAddPlaylist()
    {
      var playlist = new Playlist("New Playlist")
      {
        User = this
      };

      AddPlaylist(playlist);

      return playlist;
    }

    public virtual void RemovePlaylist(Playlist playlist)
    {
      Playlists.Remove(playlist);
    }

    public virtual void ValidateAndThrow()
    {
      var validator = new UserValidator();
      validator.ValidateAndThrow(this);
    }

    public virtual void AddPlaylist(Playlist playlist)
    {
      //  Client might not set the sequence number.
      if (playlist.Sequence < 0)
      {
        if (Playlists.Any())
        {
          playlist.Sequence = Playlists.OrderBy(i => i.Sequence).Last().Sequence + 10000;
        }
        else
        {
          playlist.Sequence = 10000;
        }
      }

      playlist.User = this;
      Playlists.Add(playlist);
    }

    public virtual void MergeUser(User user)
    {
      foreach (Playlist playlist in user.Playlists.Where(p => p.Items.Count > 0))
      {
        AddPlaylist(new Playlist(playlist));
      }
    }
  }
}
