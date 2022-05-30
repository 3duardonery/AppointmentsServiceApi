using AppointmentService.Shared.Dto;
using AppointmentService.Shared.ViewModels;
using OperationResult;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentService.Domain.Services
{
    public interface BookServiceImp
    {
        Task<Result<IEnumerable<BookViewModel>>> GetAvailableBooksByServiceId(string serviceId);
        Task<Result<IEnumerable<BookViewModel>>> MountANewBookWithLimitInDays(OpenBookRequestDto openBookRequest);
    }
}
