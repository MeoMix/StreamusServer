using AutoMapper;
using Streamus_Web_API.Domain;

namespace Streamus_Web_API.Dto
{
    public class ClientErrorDto
    {
        public string UserId { get; set; }
        public string Message { get; set; }
        public int LineNumber { get; set; }
        public string Url { get; set; }
        public string ClientVersion { get; set; }
        public string OperatingSystem { get; set; }
        public string Architecture { get; set; }
        public string Stack { get; set; }
        public string BrowserVersion { get; set; }

        public ClientErrorDto()
        {
            UserId = string.Empty;
            Message = string.Empty;
            Architecture = string.Empty;
            OperatingSystem = string.Empty;
            LineNumber = -1;
            Url = string.Empty;
            ClientVersion = string.Empty;
            Stack = string.Empty;
            BrowserVersion = string.Empty;
        }

        public static ClientErrorDto Create(ClientError clientError)
        {
            ClientErrorDto clientErrorDto = Mapper.Map<ClientError, ClientErrorDto>(clientError);
            return clientErrorDto;
        }
    }
}