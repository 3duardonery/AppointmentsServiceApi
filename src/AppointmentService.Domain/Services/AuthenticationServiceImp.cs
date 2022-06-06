using AppointmentService.Shared.Dto;
using AppointmentService.Shared.ViewModels;
using OperationResult;
using System.Threading.Tasks;

namespace AppointmentService.Domain.Services
{
    public interface AuthenticationServiceImp
    {
        Task<Result<AuthResponseViewModel>> LogIn(AuthenticationRequestDto user);
    }
}
