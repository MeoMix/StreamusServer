using System.Collections.Generic;
using NUnit.Framework;
using Streamus.Controllers;
using Streamus.Dao;
using Streamus.Domain;
using Streamus.Domain.Interfaces;
using Streamus.Domain.Managers;
using Streamus.Dto;
using System;
using System.Linq;

namespace Streamus.Tests.Controller_Tests
{
    [TestFixture]
    public class PlaylistControllerTest : AbstractTest
    {
        private static readonly UserManager UserManager = new UserManager();
        private static readonly PlaylistItemController PlaylistItemController = new PlaylistItemController();
        private static readonly ShareCodeManager ShareCodeManager = new ShareCodeManager();
        private static readonly PlaylistController PlaylistController = new PlaylistController();
        private IFolderDao FolderDao { get; set; }

        /// <summary>
        ///     This code is only ran once for the given TestFixture.
        /// </summary>
        [TestFixtureSetUp]
        public new void TestFixtureSetUp()
        {
            try
            {
                FolderDao = DaoFactory.GetFolderDao();
            }
            catch (TypeInitializationException exception)
            {
                throw exception.InnerException;
            }
        }

        [Test]
        public void DeletePlaylist_NextToBigPlaylist_NoStackOverflowException()
        {
            User user = Helpers.CreateUser();

            Folder folder = user.Folders.First();
            Guid firstPlaylistId = folder.Playlists.First().Id;

            PlaylistDto playlistDto = Helpers.CreatePlaylistDto(folder.Id);

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            JsonServiceStackResult result = (JsonServiceStackResult)PlaylistController.Create(playlistDto);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();

            PlaylistDto createdPlaylistDto = (PlaylistDto)result.Data;

            const int numItemsToCreate = 150;
            List<PlaylistItemDto> playlistItemDtos = Helpers.CreatePlaylistItemsDto(numItemsToCreate, createdPlaylistDto.Id);

            foreach (List<PlaylistItemDto> splitPlaylistItemDtos in Split(playlistItemDtos, 50))
            {
                NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
                PlaylistItemController.CreateMultiple(splitPlaylistItemDtos);
                NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();
            }

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            //  Now delete the first playlist.
            PlaylistController.Delete(firstPlaylistId);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();
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
            PlaylistDto playlistDto = Helpers.CreatePlaylistDto();

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            JsonServiceStackResult result = (JsonServiceStackResult)PlaylistController.Create(playlistDto);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();

            PlaylistDto createdPlaylistDto = (PlaylistDto) result.Data;

            //  Make sure we actually get a Playlist DTO back from the Controller.
            Assert.NotNull(createdPlaylistDto);

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            Folder folder = FolderDao.Get(createdPlaylistDto.FolderId);

            //  Make sure that the created playlist was cascade added to the Folder
            Assert.That(folder.Playlists.Count(p => p.Id == createdPlaylistDto.Id) == 1);

            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();
        }

        [Test]
        public void GetSharedPlaylist_PlaylistShareCodeExists_CopyOfPlaylistCreated()
        {
            User user = Helpers.CreateUser();

            Folder folder = user.Folders.First();

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            ShareCode shareCode = ShareCodeManager.GetShareCode(ShareableEntityType.Playlist, folder.Playlists.First().Id);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            JsonServiceStackResult result = (JsonServiceStackResult)PlaylistController.CreateCopyByShareCode(shareCode.ShortId, shareCode.UrlFriendlyEntityTitle, folder.Id);
            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();
            
            PlaylistDto playlistDto = (PlaylistDto) result.Data;

            //  Make sure we actually get a Playlist DTO back from the Controller.
            Assert.NotNull(playlistDto);

            NHibernateSessionManager.Instance.OpenSessionAndBeginTransaction();
            Folder folderFromDatabase = FolderDao.Get(playlistDto.FolderId);

            //  Make sure that the created playlist was cascade added to the Folder
            Assert.That(folderFromDatabase.Playlists.Count(p => p.Id == playlistDto.Id) == 1);

            NHibernateSessionManager.Instance.CommitTransactionAndCloseSession();
        }

    }
}
