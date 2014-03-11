using System.Collections.Generic;

namespace Streamus_Web_API.Domain.Interfaces
{
    public interface IVideoManager
    {
        Video Get(string id);
        IList<Video> Get(List<string> ids); 
        void Save(Video video);
        void Save(IEnumerable<Video> videos);
    }
}
