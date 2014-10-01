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

        public const int MaxMessageLength = 255;
        public const int MaxStackLength = 2000;
        public const int MaxClientVersionLength = 255;
        public const int MaxUrlLength = 255;
        public const int LineNumberDefault = -1;
        private const string Ellipses = "...";

        public ClientError()
        {
            Message = string.Empty;
            LineNumber = LineNumberDefault;
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
                Message = string.Format("{0}{1}", Message.Substring(0, MaxMessageLength - Ellipses.Length), Ellipses);

            if (Stack.Length > MaxStackLength)
                Stack = string.Format("{0}{1}", Stack.Substring(0, MaxStackLength - Ellipses.Length), Ellipses);

            if (ClientVersion.Length > MaxClientVersionLength)
                ClientVersion = string.Format("{0}{1}", ClientVersion.Substring(0, MaxClientVersionLength - Ellipses.Length), Ellipses);

            if (Url.Length > MaxUrlLength)
                Url = string.Format("{0}{1}", Url.Substring(0, MaxUrlLength - Ellipses.Length), Ellipses);
        }

        public virtual void ValidateAndThrow()
        {
            var validator = new ClientErrorValidator();
            validator.ValidateAndThrow(this);
        }
    }
}