using System.Collections.Generic;
using NetDevPack.SimpleMediator;
using OperationResult;

namespace AppointmentService.Domain.Commands
{
    public record UpdateProfessionalServicesDependenciesCommand : IRequest<Result>
    {
        public required string ProfessionalId { get; set; }
        public required List<string> ServiceIds { get; set; }
    }
}
