using System;
using AutoMapper;
using Streamus_Web_API.Domain;

namespace Streamus_Web_API.Dto
{
    public class ErrorDto
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public int LineNumber { get; set; }
        public string Url { get; set; }
        public string ClientVersion { get; set; }
        public DateTime TimeOccurred { get; set; }
        public string OperatingSystem { get; set; }
        public string Architecture { get; set; }

        public ErrorDto()
        {
            Message = string.Empty;
            Architecture = string.Empty;
            OperatingSystem = string.Empty;
            LineNumber = -1;
            Url = string.Empty;
            ClientVersion = string.Empty;
            TimeOccurred = DateTime.Now;
        }

        public static ErrorDto Create(Error error)
        {
            ErrorDto errorDto = Mapper.Map<Error, ErrorDto>(error);
            return errorDto;
        }
    }
}