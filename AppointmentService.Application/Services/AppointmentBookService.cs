using AppointmentService.Domain.Models;
using AppointmentService.Domain.Repository;
using AppointmentService.Domain.Services;
using AppointmentService.Shared.Dto;
using AppointmentService.Shared.ViewModels;
using AutoMapper;
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

        public async Task<Result<IEnumerable<AppointmentViewModel>>> GetAppointmentsByCustomerId(string customerId)
        {
            var (isSuccess, appointments, exception) = await _factoryAppointment.GetAppointmentsByCustomerId(customerId).ConfigureAwait(false);

            if (!isSuccess)
                return exception;

            return Result.Success(_mapper.Map<IEnumerable<AppointmentViewModel>>(appointments));
        }

        public async Task<Result<IEnumerable<AppointmentViewModel>>> GetAppointmentsByProfessionalId(string professionalId)
        {
            var (isSuccess, appointments, exception) = await _factoryAppointment.GetAppointmentsByProfessionalId(professionalId).ConfigureAwait(false);

            if (!isSuccess)
                return exception;

            return Result.Success(_mapper.Map<IEnumerable<AppointmentViewModel>>(appointments));
        }

        public async Task<Result<AppointmentViewModel>> Save(AppointmentRequestDto appointmentRequest)
        {
            var book = await _factoryBook.GetBookByServiceAndDate(appointmentRequest.ServiceId, appointmentRequest.Date).ConfigureAwait(false);

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
