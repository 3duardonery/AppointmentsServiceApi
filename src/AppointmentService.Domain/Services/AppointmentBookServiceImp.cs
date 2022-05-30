using AppointmentService.Shared.Dto;
using AppointmentService.Shared.ViewModels;
using OperationResult;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentService.Domain.Services
{
    public interface AppointmentBookServiceImp
    {
        Task<Result<AppointmentViewModel>> Save(AppointmentRequestDto appointment);
        Task<Result<IEnumerable<AppointmentViewModel>>> GetAppointmentsByCustomerId(string customerId);
        Task<Result<IEnumerable<AppointmentViewModel>>> GetAppointmentsByProfessionalId(string professionalId);
        Task<Result> CancelAppointment(string appointmentId);
    }
}
