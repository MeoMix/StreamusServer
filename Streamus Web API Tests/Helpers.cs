using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using NHibernate;
using NSubstitute;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;
using Streamus_Web_API.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Streamus_Web_API_Tests
{
    /// <summary>
    ///     Stores common methods used by tests. Just useful for keeping things DRY between test cases.
    /// </summary>
    public class Helpers
    {
        private readonly IUserManager UserManager;
        private readonly IPlaylistItemManager PlaylistItemManager;

        public Helpers(ISession session, IManagerFactory managerFactory)
        {
            UserManager = managerFactory.GetUserManager(session);
            PlaylistItemManager = managerFactory.GetPlaylistItemManager(session);
        }

        /// <summary>
        ///     Creates a new Video and PlaylistItem, puts item in the database and then returns
        ///     the item. Just a nice utility method to keep things DRY.
        /// </summary>
        public PlaylistItem CreateItemInPlaylist(Playlist playlist)
        {
            Video videoNotInDatabase = CreateUnsavedVideoWithId();

            //  Create a new PlaylistItem and write it to the database.
            string title = videoNotInDatabase.Title;
            var playlistItem = new PlaylistItem(title, videoNotInDatabase);

            playlist.AddItem(playlistItem);

            PlaylistItemManager.Save(playlistItem);

            return playlistItem;
        }

        /// <summary>
        ///     Creates a new Video with a random Id, or a given Id if specified, saves it to the database and returns it.
        /// </summary>
        public Video CreateUnsavedVideoWithId(string idOverride = "", string titleOverride = "")
        {
            //  Create a random video ID to ensure the Video doesn't exist in the database currently.
            string randomVideoId = idOverride == string.Empty ? Guid.NewGuid().ToString().Substring(0, 11) : idOverride;
            string title = titleOverride == string.Empty ? string.Format("Video {0}", randomVideoId) : titleOverride;
            var video = new Video(randomVideoId, title, 999, "Author");

            return video;
        }

        public User CreateUser()
        {
            User user = UserManager.CreateUser();
            return user;
        }

        /// <summary>
        ///     Generate a PlaylistDto which has the User as its parent.
        /// </summary>
        /// <returns></returns>
        public PlaylistDto CreatePlaylistDto(Guid userIdOverride)
        {
            var playlistDto = new PlaylistDto
            {
                UserId = userIdOverride
            };

            return playlistDto;
        }

        /// <summary>
        ///     Create a new Playlist, save it to the DB, then generate a PlaylistItemDto
        ///     which has the Playlist as its parent.
        /// </summary>
        public PlaylistItemDto CreatePlaylistItemDto()
        {
            User user = CreateUser();

            Guid playlistId = user.Playlists.First().Id;

            Video video = CreateUnsavedVideoWithId();
            VideoDto videoDto = VideoDto.Create(video);

            var playlistItemDto = new PlaylistItemDto
            {
                PlaylistId = playlistId,
                Video = videoDto
            };

            return playlistItemDto;
        }

        /// <summary>
        ///     Create a new Playlist, save it to the DB, then generate N PlaylistItemDtos
        ///     which have the Playlist as their parent.
        /// </summary>
        public List<PlaylistItemDto> CreatePlaylistItemsDto(int itemsToCreate, Guid playlistId = default(Guid))
        {
            if (playlistId == default(Guid))
            {
                User user = CreateUser();
                playlistId = user.Playlists.First().Id;
            }

            Video video = CreateUnsavedVideoWithId();
            VideoDto videoDto = VideoDto.Create(video);

            List<PlaylistItemDto> playlistItemDtos = new List<PlaylistItemDto>();

            for (int i = 0; i < itemsToCreate; i++)
            {
                var playlistItemDto = new PlaylistItemDto
                {
                    PlaylistId = playlistId,
                    Video = videoDto
                };

                playlistItemDtos.Add(playlistItemDto);
            }

            return playlistItemDtos;
        }

        public static RouteInfo RouteRequest(HttpConfiguration config, HttpRequestMessage request)
        {
            // create context
            var controllerContext = new HttpControllerContext(config, Substitute.For<IHttpRouteData>(), request);

            // get route data
            var routeData = config.Routes.GetRouteData(request);
            RemoveOptionalRoutingParameters(routeData.Values);

            request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
            controllerContext.RouteData = routeData;

            // get controller type
            var controllerDescriptor = new DefaultHttpControllerSelector(config).SelectController(request);
            controllerContext.ControllerDescriptor = controllerDescriptor;

            // get action name
            var actionMapping = new ApiControllerActionSelector().SelectAction(controllerContext);

            return new RouteInfo
            {
                Controller = controllerDescriptor.ControllerType,
                Action = actionMapping.ActionName
            };
        }

        private static void RemoveOptionalRoutingParameters(IDictionary<string, object> routeValues)
        {
            var optionalParams = routeValues
                .Where(x => x.Value == RouteParameter.Optional)
                .Select(x => x.Key)
                .ToList();

            foreach (var key in optionalParams)
            {
                routeValues.Remove(key);
            }
        }
    }

    public class RouteInfo
    {
        public Type Controller { get; set; }

        public string Action { get; set; }
    }
}