using System;
using FluentValidation;
using Streamus_Web_API.Domain.Validators;

namespace Streamus_Web_API.Domain
{
    public class ClientError : AbstractDomainEntity<Guid>
    {
        public virtual string Message { get; set; }
        public virtual int LineNumber { get; set; }
        public virtual string Url { get; set; }
        public virtual string ClientVersion { get; set; }
        public virtual DateTime TimeOccurred { get; set; }
        public virtual string OperatingSystem { get; set; }
        public virtual string Architecture { get; set; }
        public virtual string Stack { get; set; }

        private const int MaxMessageLength = 255;
        private const int MaxStackLength = 2000;
        private const string Ellipses = "...";

        public ClientError()
        {
            Message = string.Empty;
            LineNumber = -1;
            Url = string.Empty;
            ClientVersion = string.Empty;
            TimeOccurred = DateTime.Now;
            OperatingSystem = string.Empty;
            Architecture = string.Empty;
            Stack = string.Empty;
        }

        public ClientError(string architecture, string clientVersion, int lineNumber, string message, string operatingSystem, string url, string stack)
            : this()
        {
            Architecture = architecture;
            ClientVersion = clientVersion;
            LineNumber = lineNumber;
            Message = message;
            OperatingSystem = operatingSystem;
            Url = url;
            Stack = stack;

            if (Message.Length > MaxMessageLength)
            {
                //  Ensure that client error message is a maximum of 255 characters before saving.
                Message = string.Format("{0}{1}", Message.Substring(0, MaxMessageLength - Ellipses.Length), Ellipses);
            }

            if (Stack.Length > MaxStackLength)
            {
                //  Ensure that client error stack trace is a maximum of 2000 characters before saving.
                Stack = string.Format("{0}{1}", Stack.Substring(0, MaxStackLength - Ellipses.Length), Ellipses);
            }
        }

        public virtual void ValidateAndThrow()
        {
            var validator = new ClientErrorValidator();
            validator.ValidateAndThrow(this);
        }
    }
}