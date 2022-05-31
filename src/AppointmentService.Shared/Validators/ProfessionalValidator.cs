using AppointmentService.Shared.Dto;
using FluentValidation;

namespace AppointmentService.Shared.Validators
{
    public sealed class ProfessionalValidator : AbstractValidator<ProfessionalDto> 
    {
        public ProfessionalValidator() => RuleFor(x => x.Name)
            .NotEmpty().NotNull().WithMessage("This field do not be null or empty");
    }
}
