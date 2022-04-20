using AppointmentService.Shared.Dto;
using AppointmentService.Shared.ViewModels;
using OperationResult;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentService.Domain.Services
{
    public interface ProfessionalServiceImp
    {
        Task<Result<IEnumerable<ProfessionalViewModel>>> GetAllProfessionals();
        Task<Result<ProfessionalViewModel>> CreateNewProfessional(ProfessionalDto professional);
    }
}
