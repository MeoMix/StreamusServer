using System;
using NHibernate;
using Streamus.Domain;
using Streamus.Domain.Interfaces;

namespace Streamus.Dao
{
    public class PlaylistDao : AbstractNHibernateDao<Playlist>, IPlaylistDao
    {
        public PlaylistDao(ISession session)
            : base(session)
        {
            
        }

        public Playlist Get(Guid id)
        {
            Playlist playlist = null;

            if (id != default(Guid))
            {
                playlist = Session.Load<Playlist>(id);
            }

            return playlist;
        }
    }
}