using AppointmentService.Shared.Dto;
using AppointmentService.Shared.ViewModels;
using OperationResult;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentService.Domain.Services
{
    public interface ProfessionalServicesServiceImp
    {
        Task<Result<IEnumerable<ServiceViewModel>>> GetAllServices();
        Task<Result<ServiceViewModel>> CreateNewService(ProfessionalServiceDto service);
    }
}
