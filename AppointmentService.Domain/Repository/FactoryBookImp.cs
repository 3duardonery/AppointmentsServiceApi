using AppointmentService.Domain.Models;
using OperationResult;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentService.Domain.Repository
{
    public interface FactoryBookImp
    {
        Task<Result<IEnumerable<Book>>> GetBookByService(string serviceId);
        Task<Result<IEnumerable<Book>>> Save(IEnumerable<Book> book);
    }
}
