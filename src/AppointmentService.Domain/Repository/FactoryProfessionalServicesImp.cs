using AppointmentService.Domain.Models;
using OperationResult;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentService.Domain.Repository
{
    public interface FactoryProfessionalServicesImp
    {
        Task<Result<Service>> Save(Service service);        
        Task<Result<IEnumerable<Service>>> Services();
        Task<Result<IEnumerable<Service>>> GetServicesByIds(List<string> ids);
        Task<Result<Service>> GetServiceById(string id);
    }
}
