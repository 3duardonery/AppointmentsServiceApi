﻿using AppointmentService.Data.DataContext;
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

        public async Task<Result<IEnumerable<Appointment>>> GetAppointmentsByCustomerId(string customerId)
        {
            try
            {
                var filter = Builders<Appointment>.Filter.Eq("customerId", ObjectId.Parse(customerId));

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
                var filter = Builders<Appointment>.Filter.Eq("professionalId", ObjectId.Parse(professionalId));

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
