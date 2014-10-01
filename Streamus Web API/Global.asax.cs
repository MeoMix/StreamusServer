using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Streamus_Web_API.App_Start;
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

            InitializeApplication();
        }

        public static void InitializeApplication()
        {
            //NHibernateProfiler.Initialize();

            //  I don't want $id, no need to support circular references: http://stackoverflow.com/questions/18355312/map-id-in-web-api-to-custom-id
            JsonMediaTypeFormatter jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;

            //  Convert sentence casing to camel casing when serializing to JSON.
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //  TODO: I need this disabled to properly handle PATCH requests.
            //  Serialize JSON null as an empty string. Having a null string is never useful and should be avoided.
            //jsonFormatter.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Populate;

            CreateAutoMapperMaps();
        }

        /// <summary>
        ///     Initialize the AutoMapper mappings for the solution.
        ///     Do not reverse the mappings between domain and DTO: http://lostechies.com/jimmybogard/2009/09/18/the-case-for-two-way-mapping-in-automapper/
        ///     http://automapper.codeplex.com/
        /// </summary>
        public static void CreateAutoMapperMaps()
        {
            Mapper.CreateMap<ClientError, ClientErrorDto>();

            Mapper.CreateMap<Playlist, PlaylistDto>();
            Mapper.CreateMap<PlaylistItem, PlaylistItemDto>()
                  .ForMember(dest => dest.Song, opt => opt.ResolveUsing(src => new SongDto
                      {
                          Author = src.Author,
                          Duration = src.Duration,
                          Id = src.SongId,
                          Title = src.SongTitle,
                          Type = src.SongType
                      }));

            Mapper.CreateMap<ShareCode, ShareCodeDto>();

            Mapper.CreateMap<User, UserDto>();
     
            Mapper.AssertConfigurationIsValid();
        }
    }
}
