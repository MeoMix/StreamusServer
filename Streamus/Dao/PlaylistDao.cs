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

        public void DeleteById(Guid id)
        {
            Session.CreateSQLQuery("delete from Playlists where id = :id")
                .SetParameter("id", id)
                .ExecuteUpdate();
        }

        public void UpdateTitleById(Guid id, string title)
        {
            Session.CreateSQLQuery("update Playlists set title = :title where id = :id")
                .SetParameter("id", id)
                .SetParameter("title", title)
                .ExecuteUpdate();
        }
    }
}