using FluentValidation;

namespace Streamus_Web_API.Domain.Validators
{
    public class ClientErrorValidator : AbstractValidator<ClientError>
    {
        public ClientErrorValidator()
        {
            RuleFor(clientError => clientError.Message).NotNull().Length(0, ClientError.MaxMessageLength);
            RuleFor(clientError => clientError.LineNumber).GreaterThan(ClientError.LineNumberDefault);
            RuleFor(clientError => clientError.ClientVersion).NotNull().Length(0, ClientError.MaxClientVersionLength);
            RuleFor(clientError => clientError.BrowserVersion).NotNull();
            RuleFor(clientError => clientError.Url).NotNull().Length(0, ClientError.MaxUrlLength);
            RuleFor(clientError => clientError.Stack).NotNull().Length(0, ClientError.MaxStackLength);
            RuleFor(clientError => clientError.TimeOccurred).NotNull();
        }
    }
}