using AppointmentService.Shared.ViewModels;
using NetDevPack.SimpleMediator;
using OperationResult;

namespace AppointmentService.Domain.Queries
{
    public record GetProfessionalByEmailQuery : IRequest<Result<ProfessionalViewModel>>
    {
        public required string Email { get; set; }
    }
}
