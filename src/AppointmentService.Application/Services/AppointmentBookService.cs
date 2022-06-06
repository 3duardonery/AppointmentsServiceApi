using AppointmentService.Domain.Models;
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

            UpdateBook(book.Value, appointment.Value.SlotId);

            var updateSlotsInBook = await _factoryBook.UpdateAvailableHours(book.Value)
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

        public async Task<Result<AppointmentViewModel>> Reschedule(ResheduleAppointmentDto resheduleRequest)
        {
            var appointmentObjectId = ObjectId.Empty;
            var serviceObjectId = ObjectId.Empty;

            ObjectId.TryParse(resheduleRequest.AppointmentId, out appointmentObjectId);

            var appointment = await _factoryAppointment
                .GetAppointmentbyId(appointmentObjectId).ConfigureAwait(false);

            if (!appointment.IsSuccess || appointment.Value == null)
                return appointment.Exception;

            var book = await _factoryBook.GetBookById(appointment.Value.BookId)
                .ConfigureAwait(false);

            if (!book.IsSuccess)
                return book.Exception;

            UpdateBook(book.Value, appointment.Value.SlotId);

            ObjectId.TryParse(resheduleRequest.ServiceId, out serviceObjectId);

            var (resheduleIsSuccess, newBook, resheduleException) = await 
                _factoryBook.GetBookByServiceAndDate(serviceObjectId, resheduleRequest.Date)
                .ConfigureAwait(false);

            if (!resheduleIsSuccess)
                return resheduleException;

            var slot = book.Value.AvailableHours.FirstOrDefault(x => x.AvailableHour
            .Equals(resheduleRequest.Time) && x.CustomerId is null);

            if (slot is null)
                return new Exception("The slot already occuppied");

            var reshedule = new Appointment
            {
                CustomerId = resheduleRequest.CustomerId,
                CustomerName = resheduleRequest.CustomerName,
                ProfessionalReference = newBook.ProfessionalReference,
                BookId = newBook.Id,
                SlotId = slot.Id,
                Date = newBook.Date,
                Executed = false,
                ServiceReference = newBook.ServiceReference,
                Time = TimeSpan.Parse(slot.AvailableHour),

            };

            var (updateBookIsSuccess, updateBookResult, updateBookexception) = await _factoryBook.UpdateAvailableHours(book.Value).ConfigureAwait(false);

            if (!updateBookIsSuccess)
                return updateBookexception;

            var updateAppointment = await _factoryAppointment.Cancel(appointmentObjectId)
                .ConfigureAwait(false);

            if (!updateAppointment.IsSuccess)
                return updateAppointment.Exception;

            var appointmentResult = await _factoryAppointment.Save(reshedule)
               .ConfigureAwait(false);

            if (!appointmentResult.IsSuccess)
                return appointmentResult.Exception;

            return Result.Success(_mapper.Map<AppointmentViewModel>(appointmentResult.Value));
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
                CustomerName = appointmentRequest.CustomerName,
                ProfessionalReference = book.Value.ProfessionalReference,
                BookId = book.Value.Id,
                SlotId = slot.Id,
                Date = book.Value.Date,
                Executed = false,
                ServiceReference = book.Value.ServiceReference,
                Time = TimeSpan.Parse(slot.AvailableHour),
                
            };

            var (isSuccess, updateBookResult, exception) = await _factoryBook.UpdateAvailableHours(book.Value).ConfigureAwait(false);

            if (!isSuccess)
                return exception;

            var appointmentResult = await _factoryAppointment.Save(appointment)
                .ConfigureAwait(false);

            if (!appointmentResult.IsSuccess)
                return appointmentResult.Exception;

            return Result.Success(_mapper.Map<AppointmentViewModel>(appointmentResult.Value));
        }


        private static Result<Book> UpdateBook(Book update, string slotId)
        {
            var slotToBeCancelled = update.AvailableHours
                .FirstOrDefault(x => x.Id == slotId);

            if (slotToBeCancelled is null)
                return new Exception("Slot dos not exists");

            var newSlot = new Time
            {
                AvailableHour = slotToBeCancelled.AvailableHour,
                ProfessionalId = slotToBeCancelled.ProfessionalId,
            };

            slotToBeCancelled.IsCancelled = true;

            var slots = update.AvailableHours.ToList();

            slots.Add(newSlot);

            update.AvailableHours = slots.OrderBy(x => x.AvailableHour);

            return update;
        }
    }
}
