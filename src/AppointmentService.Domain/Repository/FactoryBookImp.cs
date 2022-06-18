using AppointmentService.Domain.Models;
using MongoDB.Bson;
using OperationResult;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentService.Domain.Repository
{
    public interface FactoryBookImp
    {
        Task<Result<IEnumerable<Book>>> GetBookByService(ObjectId serviceId);
        Task<Result<IEnumerable<Book>>> GetBookByProfesionalEmail(string email);
        Task<Result<Book>> GetBookById(string bookId);
        Task<Result<Book>> GetBookByServiceAndDate(ObjectId serviceId, DateTime sheduleDate);
        Task<Result<IEnumerable<Book>>> Save(IEnumerable<Book> book);
        Task<Result<Book>> UpdateAvailableHours(Book book);
        Task<Result> CancelBook(string bookId);
    }
}
