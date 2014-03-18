using NUnit.Framework;
using Streamus_Web_API.Controllers;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;
using Streamus_Web_API.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Streamus_Web_API_Tests.Controller
{
    [TestFixture]
    public class PlaylistControllerTest : StreamusTest
    {
        private PlaylistController PlaylistController;
        private PlaylistItemController PlaylistItemController;
        private IShareCodeManager ShareCodeManager;
        private IPlaylistManager PlaylistManager;
        private IUserManager UserManager;

        [SetUp]
        public new void TestFixtureSetUp()
        {
            PlaylistController = new PlaylistController(Logger, Session, ManagerFactory);
            PlaylistItemController = new PlaylistItemController(Logger, Session, ManagerFactory);

            ShareCodeManager = ManagerFactory.GetShareCodeManager(Session);
            UserManager = ManagerFactory.GetUserManager(Session);
            PlaylistManager = ManagerFactory.GetPlaylistManager(Session);
        }

        [Test]
        public void DeletePlaylist_PlaylistEmpty_PlaylistDeletedSuccessfully()
        {
            User user = Helpers.CreateUser();

            PlaylistController.Delete(user.Playlists.First().Id);
        }

        [Test]
        public void DeletePlaylist_PlaylistHasItemsInIt_PlaylistDeletedSuccessfully()
        {
            User user = Helpers.CreateUser();
            Playlist playlist = user.Playlists.First();

            Helpers.CreateItemInPlaylist(playlist);
            Helpers.CreateItemInPlaylist(playlist);

            PlaylistController.Delete(playlist.Id);
        }

        [Test]
        public void DeletePlaylist_NextToBigPlaylist_NoStackOverflowException()
        {
            User user = Helpers.CreateUser();

            Guid firstPlaylistId = user.Playlists.First().Id;

            PlaylistDto playlistDto = Helpers.CreatePlaylistDto(user.Id);

            var createdPlaylistDto = PlaylistController.Create(playlistDto);

            const int numItemsToCreate = 150;
            List<PlaylistItemDto> playlistItemDtos = Helpers.CreatePlaylistItemsDto(numItemsToCreate, createdPlaylistDto.Id);

            foreach (var splitPlaylistItemDtos in Split(playlistItemDtos, 50))
            {
                PlaylistItemController.CreateMultiple(splitPlaylistItemDtos);
            }

            //  Now delete the first playlist.
            PlaylistController.Delete(firstPlaylistId);
        }

        public static List<List<PlaylistItemDto>> Split(List<PlaylistItemDto> source, int splitSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / splitSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        [Test]
        public void CreatePlaylist_PlaylistDoesntExist_PlaylistCreated()
        {
            User user = Helpers.CreateUser();
            PlaylistDto playlistDto = Helpers.CreatePlaylistDto(user.Id);

            var createdPlaylistDto = PlaylistController.Create(playlistDto);

            //  Make sure we actually get a Playlist DTO back from the Controller.
            Assert.NotNull(createdPlaylistDto);

            User userFromDatabase = UserManager.Get(createdPlaylistDto.UserId);

            //  Make sure that the created playlist was cascade added to the User
            Assert.That(userFromDatabase.Playlists.Count(p => p.Id == createdPlaylistDto.Id) == 1);
        }

        [Test]
        public void GetSharedPlaylist_PlaylistShareCodeExists_CopyOfPlaylistCreated()
        {
            User user = Helpers.CreateUser();

            Playlist playlist = PlaylistManager.CopyAndSave(user.Playlists.First().Id);
            ShareCode shareCode = ShareCodeManager.GetShareCode(playlist);

            ShareCodeRequestDto shareCodeRequestDto = new ShareCodeRequestDto(user.Id, shareCode.ShortId, shareCode.UrlFriendlyEntityTitle);

            //  Create a new playlist for the given user by loading up the playlist via sharecode.
            var playlistDto = PlaylistController.CreateCopyByShareCode(shareCodeRequestDto);

            //  Make sure we actually get a Playlist DTO back from the Controller.
            Assert.NotNull(playlistDto);

            User userFromDatabase = UserManager.Get(playlistDto.UserId);

            //  Make sure that the created playlist was cascade added to the User
            Assert.That(userFromDatabase.Playlists.Count(p => p.Id == playlistDto.Id) == 1);
        }

        //[Test]
        //public void CreatePlaylist_PlaylistHasItemsInIt_UserDoesntHaveExtraPlaylistReferences()
        //{
        //    User user = Helpers.CreateUser();
        //    PlaylistDto playlistDto = Helpers.CreatePlaylistDto(user.Id);

        //    playlistDto.Items.Add(Helpers.CreatePlaylistItemDto());
        //    playlistDto.Items.Add(Helpers.CreatePlaylistItemDto());
        //    playlistDto.Items.Add(Helpers.CreatePlaylistItemDto());
        //    playlistDto.Items.Add(Helpers.CreatePlaylistItemDto());

        //    var createdPlaylistDto = PlaylistController.Create(playlistDto);

        //    //  Make sure we actually get a Playlist DTO back from the Controller.
        //    Assert.NotNull(createdPlaylistDto);

        //    User userFromDatabase = UserManager.Get(createdPlaylistDto.UserId);

        //    //  Make sure that the created playlist was cascade added to the User
        //    Assert.That(userFromDatabase.Playlists.Count(p => p.Id == createdPlaylistDto.Id) == 1);
        //}
    }
}