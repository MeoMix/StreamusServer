using AutoMapper;
using FluentValidation;
using Streamus.Domain.Validators;
using Streamus.Dto;
using System;

namespace Streamus.Domain
{
    public class Error : AbstractDomainEntity<Guid>
    {
        public string Message { get; set; }
        public int LineNumber { get; set; }
        public string Url { get; set; }
        public string ClientVersion { get; set; }
        public DateTime TimeOccurred { get; set; }

        public Error()
        {
            Message = string.Empty;
            LineNumber = -1;
            Url = string.Empty;
            ClientVersion = string.Empty;
            TimeOccurred = DateTime.Now;
        }

        public static Error Create(ErrorDto errorDto)
        {
            Error error = Mapper.Map<ErrorDto, Error>(errorDto);

            if (error.Message.Length > 255)
            {
                //  When receiving an error message from the client -- ensure it is a maximum of 255 characters before saving.
                error.Message = string.Format("{0}...", error.Message.Substring(0, 252));
            }

            //  When receiving an error message from the client -- set the TimeOccurred upon receiving the DTO from the client.
            error.TimeOccurred = DateTime.Now;

            return error;
        }

        public void ValidateAndThrow()
        {
            var validator = new ErrorValidator();
            validator.ValidateAndThrow(this);
        }
    }
}