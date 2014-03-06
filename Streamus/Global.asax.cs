using AutoMapper;
using Newtonsoft.Json;
using Streamus.App_Start;
using Streamus.Dao;
using Streamus.Domain;
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
            //NHibernateProfiler.Initialize();

            JsonMediaTypeFormatter json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All;

            //  Don't serialize JSON empty strings into null -- instead turn them into empty strings because this is more intuitive and I don't want to have to do n
            ModelBinders.Binders.DefaultBinder = new JsonEmptyStringNotNullModelBinder();

            AutofacRegistrations.RegisterAndSetResolver();

            CreateAutoMapperMaps();
        }

        /// <summary>
        ///     Initialize the AutoMapper mappings for the solution.
        ///     Do not reverse the mappings between domain and DTO: http://lostechies.com/jimmybogard/2009/09/18/the-case-for-two-way-mapping-in-automapper/
        ///     http://automapper.codeplex.com/
        /// </summary>
        public static void CreateAutoMapperMaps()
        {
            Mapper.CreateMap<Error, ErrorDto>();

            Mapper.CreateMap<Playlist, PlaylistDto>();
            Mapper.CreateMap<PlaylistItem, PlaylistItemDto>();
            Mapper.CreateMap<ShareCode, ShareCodeDto>();

            Mapper.CreateMap<User, UserDto>();
            Mapper.CreateMap<Video, VideoDto>();

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
