using System;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Streamus_Web_API.App_Start;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Dto;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json.Converters;

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
            JsonSerializerSettings serializerSettings = GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;
            serializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;

            //  Convert sentence casing to camel casing when serializing to JSON.
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //  Prefer using string names over integer values when dealing with enums for readability
            serializerSettings.Converters.Add(new StringEnumConverter
            {
                CamelCaseText = true
            });

            CreateAutoMapperMaps();
        }

        //  Make it harder for hackers by removing server information.
        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Headers.Remove("Server");
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
            // TODO: Backwards compatibility for old type.
            Mapper.CreateMap<PlaylistItem, PlaylistItemDto>()
                .ForMember(dest => dest.Song, opt => opt.ResolveUsing(src => src == null ? new VideoDto() : new VideoDto
                    {
                        Author = src.Author,
                        Duration = src.Duration,
                        Id = src.VideoId,
                        Title = src.VideoTitle,
                        Type = src.VideoType
                    }))
                    .ForMember(dest => dest.Video, opt => opt.ResolveUsing(src => src == null ? new VideoDto() : new VideoDto
                      {
                          Author = src.Author,
                          Duration = src.Duration,
                          Id = src.VideoId,
                          Title = src.VideoTitle,
                          Type = src.VideoType
                      }));


            Mapper.CreateMap<ShareCode, ShareCodeDto>();

            Mapper.CreateMap<User, UserDto>();
     
            Mapper.AssertConfigurationIsValid();
        }
    }
}
