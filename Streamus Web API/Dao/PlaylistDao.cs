using System;
using NHibernate;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;

namespace Streamus_Web_API.Dao
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