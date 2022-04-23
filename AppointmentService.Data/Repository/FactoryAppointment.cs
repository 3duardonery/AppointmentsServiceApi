using AppointmentService.Data.DataContext;
using AppointmentService.Domain.Models;
using AppointmentService.Domain.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using OperationResult;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentService.Data.Repository
{
    public sealed class FactoryAppointment : FactoryAppointmentImp
    {
        private readonly IMongoCollection<Appointment> _appointments;

        public FactoryAppointment(MongoContext mongoContext)
            => _appointments = mongoContext.GetCollection<Appointment>("appointments");

        public async Task<Result> Cancel(string appointmentId)
        {
            try
            {
                var filter = Builders<Appointment>.Filter.Eq("_id", ObjectId.Parse(appointmentId));

                var professional = await _appointments.UpdateOneAsync(filter,
                    Builders<Appointment>.Update.Set(rec => rec.IsCancelled, true));

                return Result.Success();
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public async Task<Result<Appointment>> GetAppointmentbyId(string appointmentId)
        {
            try
            {
                var filter = Builders<Appointment>.Filter.Eq("_id", ObjectId.Parse(appointmentId));

                var appointments = await _appointments.FindAsync(filter).ConfigureAwait(false);

                return Result.Success(appointments.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public async Task<Result<IEnumerable<Appointment>>> GetAppointmentsByCustomerId(string customerId)
        {
            try
            {
                var filter = Builders<Appointment>.Filter.Eq("customerId", customerId);

                var appointments = await _appointments.FindAsync(filter).ConfigureAwait(false);

                return Result.Success(appointments.ToEnumerable());
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public async Task<Result<IEnumerable<Appointment>>> GetAppointmentsByProfessionalId(string professionalId)
        {
            try
            {
                var filter = Builders<Appointment>.Filter.Eq("professionalReference._id", ObjectId.Parse(professionalId));

                var appointments = await _appointments.FindAsync(filter).ConfigureAwait(false);

                return Result.Success(appointments.ToEnumerable());
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public async Task<Result<Appointment>> Save(Appointment appointment)
        {
            try
            {
                await _appointments.InsertOneAsync(appointment).ConfigureAwait(false);

                return appointment;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
