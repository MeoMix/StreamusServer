using FluentValidation;

namespace Streamus_Web_API.Domain.Validators
{
    public class ClientErrorValidator : AbstractValidator<ClientError>
    {
        public ClientErrorValidator()
        {
            RuleFor(clientError => clientError.Message).Length(0, 255);
            RuleFor(clientError => clientError.LineNumber).GreaterThan(-1);
            RuleFor(clientError => clientError.ClientVersion).Length(0, 255);
            RuleFor(clientError => clientError.Url).Length(0, 255);
            RuleFor(clientError => clientError.TimeOccurred).NotNull();
        }
    }
}