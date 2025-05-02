using System.Collections.Generic;
using AppointmentService.Shared.ViewModels;
using NetDevPack.SimpleMediator;
using OperationResult;

namespace AppointmentService.Domain.Queries
{
    public record GetAllProfessionalsQuery : IRequest<Result<IEnumerable<ProfessionalViewModel>>>
    {
    }
}
