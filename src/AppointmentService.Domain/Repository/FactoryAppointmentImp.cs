using AppointmentService.Domain.Models;
using MongoDB.Bson;
using OperationResult;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentService.Domain.Repository
{
    public interface FactoryAppointmentImp
    {
        Task<Result<Appointment>> Save(Appointment appointment);
        Task<Result> Cancel(ObjectId appointmentId);
        Task<Result<Appointment>> GetAppointmentbyId(ObjectId appointmentId);
        Task<Result<IEnumerable<Appointment>>> GetAppointmentsByCustomerId(string customerId);
        Task<Result<IEnumerable<Appointment>>> GetAppointmentsByProfessionalId(ObjectId professionalId);
    }
}
