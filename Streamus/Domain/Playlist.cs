using FluentValidation;
using Streamus.Domain.Interfaces;
using Streamus.Domain.Validators;
using Streamus.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Streamus.Domain
{
    public class Playlist : AbstractShareableDomainEntity
    {
        public virtual User User { get; set; }
        //  Use interfaces so NHibernate can inject with its own collection implementation.
        public virtual ICollection<PlaylistItem> Items { get; set; }
        public virtual int Sequence { get; set; }

        public Playlist()
        {
            Id = Guid.Empty;
            Title = string.Empty;
            Items = new List<PlaylistItem>();
            Sequence = -1;
        }

        public Playlist(string title)
            : this()
        {
            Title = title;
        }

        public Playlist(Playlist playlist)
            : this()
        {
            Copy(playlist);
        }

        public static Playlist Create(PlaylistDto playlistDto, IUserManager userManager, IPlaylistManager playlistManager)
        {
            Playlist playlist = new Playlist
                {
                    Id = playlistDto.Id,
                    Items = PlaylistItem.Create(playlistDto.Items, playlistManager),
                    Sequence = playlistDto.Sequence,
                    Title = playlistDto.Title,
                    User = userManager.Get(playlistDto.UserId)
                };

            //  TODO: This seems unnecessary...
            //  TODO: I could probably leverage backbone's CID property to have the items know of their playlist. Or maybe I should just enforce adding client-side before saving?
            //  If an unsaved playlist comes from the client with items already in it, the items will not know their playlist's ID.
            //  So, re-map to the playlist as appropriate.
            List<PlaylistItem> improperlyAddedItems = playlist.Items.Where(i => i.Playlist == null).ToList();
            improperlyAddedItems.ForEach(i => playlist.Items.Remove(i));
            playlist.AddItems(improperlyAddedItems);

            return playlist;
        }

        public virtual void Copy(Playlist playlist)
        {
            Title = playlist.Title;

            foreach (PlaylistItem playlistItem in playlist.Items)
            {
                PlaylistItem shareableItemCopy = new PlaylistItem(playlistItem);
                AddItem(shareableItemCopy);
            }
        }

        public virtual void AddItem(PlaylistItem playlistItem)
        {
            //  Item must be removed from other Playlist before AddItem affects it.
            if (playlistItem.Playlist != null && playlistItem.Playlist.Id != Id)
            {
                string message = string.Format("Item {0} is already a child of Playlist {1}", playlistItem.Title, playlistItem.Playlist.Title);
                throw new Exception(message);
            }

            //  Client might set the sequence number.
            if (playlistItem.Sequence < 0)
            {
                if (Items.Any())
                {
                    playlistItem.Sequence = Items.OrderBy(i => i.Sequence).Last().Sequence + 10000;
                }
                else
                {
                    playlistItem.Sequence = 10000;
                }
            }
            
            playlistItem.Playlist = this;
            Items.Add(playlistItem);
        }

        public virtual void AddItems(IEnumerable<PlaylistItem> playlistItems)
        {
            playlistItems.ToList().ForEach(AddItem);
        }

        public virtual void RemoveItem(PlaylistItem playlistItem)
        {
            Items.Remove(playlistItem);
        }

        public virtual void RemoveItems(IEnumerable<PlaylistItem> playlistItems)
        {
            playlistItems.ToList().ForEach(RemoveItem);
        }

        public virtual void ValidateAndThrow()
        {
            var validator = new PlaylistValidator();
            validator.ValidateAndThrow(this);
        }

    }
}