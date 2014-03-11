using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Streamus_Web_API.App_Start;
using Streamus_Web_API.Dao;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Dto;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;

namespace Streamus_Web_API
{
    public class StreamusWebApi : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
        
        public static void InitializeApplication()
        {
            //NHibernateProfiler.Initialize();

            JsonMediaTypeFormatter jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All;
            
            //  Convert sentence casing to camel casing when serializing to JSON.
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //  Serialize JSON null as an empty string. Having a null string is never useful and should be avoided.
            jsonFormatter.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Populate;

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
}
