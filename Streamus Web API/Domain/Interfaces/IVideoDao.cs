using System.Collections.Generic;

namespace Streamus_Web_API.Domain.Interfaces
{
    public interface IVideoDao : IDao<Video>
    {
        Video Get(string id);
        IList<Video> Get(List<string> ids);
    }
}
