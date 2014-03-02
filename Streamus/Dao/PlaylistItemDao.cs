using NHibernate;
using Streamus.Domain;
using Streamus.Domain.Interfaces;
using System;

namespace Streamus.Dao
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
                playlistItem = Session.Get<PlaylistItem>(id);
            }

            return playlistItem;
        }

        public void DeleteById(Guid id)
        {
            Session.CreateSQLQuery("delete from PlaylistItems where id = :id")
                .SetParameter("id", id)
                .ExecuteUpdate();
        }
    }
}