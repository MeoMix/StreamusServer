using AutoMapper;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using Newtonsoft.Json;
using Streamus.App_Start;
using Streamus.Dao;
using Streamus.Domain;
using Streamus.Domain.Interfaces;
using Streamus.Dto;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Streamus
{
    public class Streamus : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            InitializeApplication();
        }

        public static void InitializeApplication()
        {
            NHibernateProfiler.Initialize();

            JsonMediaTypeFormatter json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All;

            //  Don't serialize JSON empty strings into null -- instead turn them into empty strings because this is more intuitive and I don't want to have to do n
            ModelBinders.Binders.DefaultBinder = new JsonEmptyStringNotNullModelBinder();

            AutofacRegistrations.RegisterAndSetResolver();

            CreateAutoMapperMaps();
        }

        /// <summary>
        ///     Initialize the AutoMapper mappings for the solution.
        ///     http://automapper.codeplex.com/
        /// </summary>
        public static void CreateAutoMapperMaps()
        {
            IDaoFactory daoFactory = DependencyResolver.Current.GetService<IDaoFactory>();

            Mapper.CreateMap<Error, ErrorDto>()
                    .ReverseMap();

            IPlaylistDao playlistDao = daoFactory.GetPlaylistDao();
            IUserDao userDao = daoFactory.GetUserDao();

            Mapper.CreateMap<Playlist, PlaylistDto>();
            Mapper.CreateMap<PlaylistDto, Playlist>()
                    .ForMember(playlist => playlist.User,
                                opt => opt.MapFrom(playlistDto => userDao.Get(playlistDto.UserId)));

            Mapper.CreateMap<PlaylistItem, PlaylistItemDto>();
            Mapper.CreateMap<PlaylistItemDto, PlaylistItem>()
                    .ForMember(playlistItem => playlistItem.Playlist,
                                opt => opt.MapFrom(playlistItemDto => playlistDao.Get(playlistItemDto.PlaylistId)));

            Mapper.CreateMap<ShareCode, ShareCodeDto>().ReverseMap();

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
}