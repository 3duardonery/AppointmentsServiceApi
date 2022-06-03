using AppointmentService.Domain.Models;
using OperationResult;
using System.Threading.Tasks;

namespace AppointmentService.Domain.Repository
{
    public interface FactoryLogImp
    {
        Task<Result<LogEvent>> Save(LogEvent log);
    }
}
