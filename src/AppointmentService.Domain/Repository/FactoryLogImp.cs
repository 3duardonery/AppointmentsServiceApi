using AppointmentService.Domain.Models;
using OperationResult;
using System.Threading.Tasks;

namespace AppointmentService.Domain.Repository
{
    public interface FactoryLogImp
    {
        Task<Result<Log>> Save(Log log);
    }
}
