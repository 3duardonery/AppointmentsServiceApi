using AppointmentService.Domain.Models;
using OperationResult;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentService.Domain.Repository
{
    public interface FactoryProfessionalImp
    {
        Task<Result<IEnumerable<Professional>>> Professionals();
        Task<Result<Professional>> Save(Professional professional);
        Task<Result<Professional>> GetProfessionalById(string professionalId);
        Task<Result<Professional>> Enable(string professionalId);
        Task<Result<bool>> Disable(string professionalId);
        Task<Result> SetServices(string professionalId, IEnumerable<Service> services);
    }
}
