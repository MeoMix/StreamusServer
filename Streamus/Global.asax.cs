using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ServiceStack.Text;
using Streamus.App_Start;
using Streamus.Dao;
using Streamus.Domain;
using Streamus.Domain.Interfaces;
using Streamus.Dto;
using Streamus.Extensions;

namespace Streamus
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            JsonMediaTypeFormatter json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All;

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //  Register your new model binder
            ModelBinders.Binders.DefaultBinder = new JsonEmptyStringNotNullModelBinder();

            AutofacRegistrations.RegisterDaoFactory();

            CreateAutoMapperMaps();

            //Remove and JsonValueProviderFactory and add JsonServiceStackValueProviderFactory
            ValueProviderFactories.Factories.Remove(ValueProviderFactories.Factories.OfType<JsonValueProviderFactory>().FirstOrDefault());
            ValueProviderFactories.Factories.Add(new JsonServiceStackValueProviderFactory());

        }

        /// <summary>
        ///     Initialize the AutoMapper mappings for the solution.
        ///     http://automapper.codeplex.com/
        /// </summary>
        public static void CreateAutoMapperMaps()
        {
            AutofacRegistrations.RegisterDaoFactory();
            ILifetimeScope scope = AutofacRegistrations.Container.BeginLifetimeScope();
            var daoFactory = scope.Resolve<IDaoFactory>();

            Mapper.CreateMap<Error, ErrorDto>()
                  .ReverseMap();

            IPlaylistDao playlistDao = daoFactory.GetPlaylistDao();
            IFolderDao folderDao = daoFactory.GetFolderDao();
            IUserDao userDao = daoFactory.GetUserDao();

            Mapper.CreateMap<Playlist, PlaylistDto>()
                  .ReverseMap()
                  .ForMember(playlist => playlist.Folder,
                             opt => opt.MapFrom(playlistDto => folderDao.Get(playlistDto.FolderId)));

            Mapper.CreateMap<PlaylistItem, PlaylistItemDto>()
                  .ReverseMap()
                  .ForMember(playlistItem => playlistItem.Playlist,
                             opt => opt.MapFrom(playlistItemDto => playlistDao.Get(playlistItemDto.PlaylistId)));

            Mapper.CreateMap<ShareCode, ShareCodeDto>().ReverseMap();

            Mapper.CreateMap<Folder, FolderDto>()
                  .ReverseMap()
                  .ForMember(folder => folder.User,
                             opt => opt.MapFrom(folderDto => userDao.Get(folderDto.UserId)));

            Mapper.CreateMap<User, UserDto>().ReverseMap();
            Mapper.CreateMap<Video, VideoDto>().ReverseMap();

            Mapper.AssertConfigurationIsValid();
        }
    }

    public class JsonEmptyStringNotNullModelBinder : DefaultModelBinder
    {
        //  Ensures that when JSON is deserialized null strings become empty.strings before persisting to the database.
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            bindingContext.ModelMetadata.ConvertEmptyStringToNull = false;

            Binders = new ModelBinderDictionary
                {
                    DefaultBinder = this
                };

            return base.BindModel(controllerContext, bindingContext);
        }
    }

    public sealed class JsonServiceStackValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext");

            if (!controllerContext.HttpContext.Request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
                return null;

            var reader = new StreamReader(controllerContext.HttpContext.Request.InputStream).BaseStream;

            return new DictionaryValueProvider<object>(
                    ServiceStack.Text.JsonSerializer.DeserializeFromStream<Dictionary<string, object>>(reader).AsExpandoObject(),
                    CultureInfo.CurrentCulture);
        }
    }
}