using System;
using FluentValidation;
using Streamus_Web_API.Domain.Validators;
using Streamus_Web_API.Dto;

namespace Streamus_Web_API.Domain
{
    public class Error : AbstractDomainEntity<Guid>
    {
        public virtual string Message { get; set; }
        public virtual int LineNumber { get; set; }
        public virtual string Url { get; set; }
        public virtual string ClientVersion { get; set; }
        public virtual DateTime TimeOccurred { get; set; }
        public virtual string OperatingSystem { get; set; }
        public virtual string Architecture { get; set; }

        public Error()
        {
            Message = string.Empty;
            LineNumber = -1;
            Url = string.Empty;
            ClientVersion = string.Empty;
            TimeOccurred = DateTime.Now;
            OperatingSystem = string.Empty;
            Architecture = string.Empty;
        }

        //  TODO: Consider not coupling to Dto here and just pass in params or use constructor.
        public static Error Create(ErrorDto errorDto)
        {
            Error error = new Error
                {
                    Architecture = errorDto.Architecture,
                    ClientVersion = errorDto.ClientVersion,
                    Id = errorDto.Id,
                    LineNumber = errorDto.LineNumber,
                    Message = errorDto.Message,
                    OperatingSystem = errorDto.OperatingSystem,
                    TimeOccurred = errorDto.TimeOccurred,
                    Url = errorDto.Url  
                };

            if (error.Message.Length > 255)
            {
                //  When receiving an error message from the client -- ensure it is a maximum of 255 characters before saving.
                error.Message = string.Format("{0}...", error.Message.Substring(0, 252));
            }

            //  When receiving an error message from the client -- set the TimeOccurred upon receiving the DTO from the client.
            error.TimeOccurred = DateTime.Now;

            return error;
        }

        public virtual void ValidateAndThrow()
        {
            var validator = new ErrorValidator();
            validator.ValidateAndThrow(this);
        }
    }
}