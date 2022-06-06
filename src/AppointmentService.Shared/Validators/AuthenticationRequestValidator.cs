using AppointmentService.Shared.Dto;
using FluentValidation;

namespace AppointmentService.Shared.Validators
{
    public sealed class AuthenticationRequestValidator : AbstractValidator<AuthenticationRequestDto>
    {
        public AuthenticationRequestValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .NotEmpty()
                .NotNull()
                .WithMessage("Email field does not be null/empty/invalid");

            RuleFor(x => x.Password)
                .NotEmpty()
                .NotNull()
                .WithMessage("Password field does not be null/empty");
        }
    }
}
