using System.Collections.Generic;

namespace Streamus.Domain.Interfaces
{
    public interface IVideoManager
    {
        void Save(Video video);
        void Save(IEnumerable<Video> videos);
    }
}
