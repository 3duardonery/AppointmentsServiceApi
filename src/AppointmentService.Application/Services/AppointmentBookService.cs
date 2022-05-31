﻿using AppointmentService.Domain.Models;
using AppointmentService.Domain.Repository;
using AppointmentService.Domain.Services;
using AppointmentService.Shared.Dto;
using AppointmentService.Shared.ViewModels;
using AutoMapper;
using MongoDB.Bson;
using OperationResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentService.Application.Services
{
    public sealed class AppointmentBookService : AppointmentBookServiceImp
    {
        private readonly FactoryAppointmentImp _factoryAppointment;
        private readonly FactoryBookImp _factoryBook;
        private readonly FactoryProfessionalImp _factoryProfessional;
        private readonly IMapper _mapper;

        public AppointmentBookService(FactoryAppointmentImp factoryAppointment, 
            FactoryBookImp factoryBook,
            FactoryProfessionalImp factoryProfessional,
            IMapper mapper)
        {
            _factoryAppointment = factoryAppointment;
            _factoryBook = factoryBook;
            _factoryProfessional = factoryProfessional;
            _mapper = mapper;
        }

        public async Task<Result> CancelAppointment(string appointmentId)
        {
            var appointmentObjectId = ObjectId.Empty;

            ObjectId.TryParse(appointmentId, out appointmentObjectId);

            var appointment = await _factoryAppointment
                .GetAppointmentbyId(appointmentObjectId).ConfigureAwait(false);

            if (!appointment.IsSuccess)
                return appointment.Exception;

            var book = await _factoryBook.GetBookById(appointment.Value.BookId)
                .ConfigureAwait(false);

            if (!book.IsSuccess)
                return book.Exception;

            var slotToBeCancelled = book.Value.AvailableHours
                .FirstOrDefault(x => x.Id == appointment.Value.SlotId);

            if (slotToBeCancelled is null)
                return new Exception("Slot dos not exists");

            var newSlot = new Time
            {
                AvailableHour = slotToBeCancelled.AvailableHour,
                ProfessionalId = slotToBeCancelled.ProfessionalId,
            };

            slotToBeCancelled.IsCancelled = true;

            var slots = book.Value.AvailableHours.ToList();

            slots.Add(newSlot);

            book.Value.AvailableHours = slots.OrderBy(x => x.AvailableHour);

            var updateSlotsInBook = await _factoryBook.Update(book.Value)
                .ConfigureAwait(false);

            if (!updateSlotsInBook.IsSuccess)
                return updateSlotsInBook.Exception;

            var updateAppointment = await _factoryAppointment.Cancel(appointmentObjectId)
                .ConfigureAwait(false);

            if (!updateAppointment.IsSuccess)
                return updateAppointment.Exception;

            return Result.Success();
        }

        public async Task<Result<IEnumerable<AppointmentViewModel>>> GetAppointmentsByCustomerId(string customerId)
        {
            var (isSuccess, appointments, exception) = await _factoryAppointment.GetAppointmentsByCustomerId(customerId).ConfigureAwait(false);

            if (!isSuccess)
                return exception;

            return Result.Success(_mapper.Map<IEnumerable<AppointmentViewModel>>(appointments));
        }

        public async Task<Result<IEnumerable<AppointmentViewModel>>> GetAppointmentsByProfessionalId(string professionalId)
        {
            var professionalObjectId = ObjectId.Empty;

            ObjectId.TryParse(professionalId, out professionalObjectId);

            var (isSuccess, appointments, exception) = await _factoryAppointment.GetAppointmentsByProfessionalId(professionalObjectId).ConfigureAwait(false);

            if (!isSuccess)
                return exception;

            return Result.Success(_mapper.Map<IEnumerable<AppointmentViewModel>>(appointments));
        }

        public async Task<Result<AppointmentViewModel>> Save(AppointmentRequestDto appointmentRequest)
        {
            var appointmentObjectId = ObjectId.Empty;
                
            ObjectId.TryParse(appointmentRequest.ServiceId, out appointmentObjectId);

            var book = await _factoryBook.GetBookByServiceAndDate(appointmentObjectId, appointmentRequest.Date).ConfigureAwait(false);

            if (!book.IsSuccess)
                return book.Exception;

            if (book.Value is null)
                return new Exception("There is no book");

            var slot = book.Value.AvailableHours.FirstOrDefault(x => x.AvailableHour
            .Equals(appointmentRequest.Time) && x.CustomerId is null);

            if (slot is null)
                return new Exception("The slot already occuppied");
           

            slot.CustomerId = appointmentRequest.CustomerId;

            var appointment = new Appointment 
            {
                CustomerId = appointmentRequest.CustomerId,
                ProfessionalReference = book.Value.ProfessionalReference,
                BookId = book.Value.Id,
                SlotId = slot.Id,
                Date = book.Value.Date,
                Executed = false,
                ServiceReference = book.Value.ServiceReference,
                Time = TimeSpan.Parse(slot.AvailableHour),
                
            };

            var (isSuccess, updateBookResult, exception) = await _factoryBook.Update(book.Value).ConfigureAwait(false);

            if (!isSuccess)
                return exception;

            var appointmentResult = await _factoryAppointment.Save(appointment)
                .ConfigureAwait(false);

            if (!isSuccess)
                return appointmentResult.Exception;

            return Result.Success(_mapper.Map<AppointmentViewModel>(appointmentResult.Value));
        }
    }
}