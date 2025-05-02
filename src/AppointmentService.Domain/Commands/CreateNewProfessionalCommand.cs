using AppointmentService.Shared.ViewModels;
using NetDevPack.SimpleMediator;
using OperationResult;

namespace AppointmentService.Domain.Commands
{
    public record CreateNewProfessionalCommand : IRequest<Result<ProfessionalViewModel>>
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string ProfilePicture { get; set; }

        public bool IsEnabled { get; set; }
    }
}
