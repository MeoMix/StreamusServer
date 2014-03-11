using NHibernate;
using System;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;

namespace Streamus_Web_API.Dao
{
    public class PlaylistItemDao : AbstractNHibernateDao<PlaylistItem>, IPlaylistItemDao
    {
        public PlaylistItemDao(ISession session)
            : base(session)
        {
            
        }

        public PlaylistItem Get(Guid id)
        {
            PlaylistItem playlistItem = null;

            if (id != default(Guid))
            {
                playlistItem = Session.Load<PlaylistItem>(id);
            }

            return playlistItem;
        }
    }
}