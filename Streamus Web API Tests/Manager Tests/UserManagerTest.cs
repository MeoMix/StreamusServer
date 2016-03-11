using System.Linq;
using NHibernate;
using NUnit.Framework;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;

namespace Streamus_Web_API_Tests.Manager_Tests
{
  [TestFixture]
  public class UserManagerTest : StreamusTest
  {
    private IUserManager _userManager;

    /// <summary>
    ///     This code runs before every test.
    /// </summary>
    [SetUp]
    public void SetupContext()
    {
      _userManager = ManagerFactory.GetUserManager(Session);
    }

    [Test]
    public void CreateUser_UserDoesntExist_UserCreated()
    {
      User createdUser;
      using (ITransaction transaction = Session.BeginTransaction())
      {
        createdUser = _userManager.CreateUser();
        transaction.Commit();
      }

      Session.Clear();
      User user = _userManager.Get(createdUser.Id);

      Assert.IsNotNull(user);
      Assert.IsNotEmpty(user.Playlists);
    }

    [Test]
    public void CreateUser_UserDoesntExist_PlaylistSequenceSetCorrectly()
    {
      User createdUser;
      using (ITransaction transaction = Session.BeginTransaction())
      {
        createdUser = _userManager.CreateUser();
        transaction.Commit();
      }

      Session.Clear();
      User user = _userManager.Get(createdUser.Id);

      Assert.IsNotNull(user);
      Assert.IsNotEmpty(user.Playlists);
      Assert.AreEqual(user.Playlists.Count, 1);
      Assert.AreEqual(user.Playlists.First().Sequence, 10000);
    }

    [Test]
    public void CreateUser_GetByGooglePlusId_UserReturned()
    {
      string googlePlusId = Helpers.GetRandomGooglePlusId();

      using (ITransaction transaction = Session.BeginTransaction())
      {
        _userManager.CreateUser(googlePlusId);
        transaction.Commit();
      }

      Session.Clear();

      User user = _userManager.GetByGooglePlusId(googlePlusId);

      Assert.NotNull(user);
    }

    [Test]
    public void MergeAllByGooglePlusId_OneOtherAccountExists_PlaylistsMergedCorrectly()
    {
      string googlePlusId = Helpers.GetRandomGooglePlusId();

      using (ITransaction transaction = Session.BeginTransaction())
      {
        User firstUser = _userManager.CreateUser(googlePlusId);
        Helpers.CreateItemInPlaylist(firstUser.CreateAndAddPlaylist());

        User secondUser = _userManager.CreateUser(googlePlusId);
        Helpers.CreateItemInPlaylist(secondUser.CreateAndAddPlaylist());

        transaction.Commit();
      }

      Session.Clear();

      using (ITransaction transaction = Session.BeginTransaction())
      {
        _userManager.MergeAllByGooglePlusId(googlePlusId);
        transaction.Commit();
      }

      Session.Clear();

      User mergedUser = _userManager.GetByGooglePlusId(googlePlusId);
      Assert.AreEqual(mergedUser.Playlists.Count, 3);
    }
  }
}