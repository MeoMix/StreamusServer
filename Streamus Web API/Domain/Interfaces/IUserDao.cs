using System;
using System.Collections.Generic;

namespace Streamus_Web_API.Domain.Interfaces
{
  public interface IUserDao : IDao<User>
  {
    User Get(Guid id);
    User GetByGooglePlusId(string googlePlusId);
    IList<User> GetAllByGooglePlusId(string googlePlusId);
    void UpdateGooglePlusIds(IList<Guid> ids, string googlePlusId);
  }
}
