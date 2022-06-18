using AppointmentService.Domain.Models;
using AppointmentService.Domain.Repository;
using AppointmentService.Domain.Services;
using AppointmentService.Shared.Dto;
using AppointmentService.Shared.ViewModels;
using AutoMapper;
using MongoDB.Bson;
using OperationResult;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentService.Application.Services
{
    public sealed class BookService : BookServiceImp
    {
        private readonly FactoryBookImp _factoryBook;
        private readonly FactoryProfessionalServicesImp _factoryProfessionalServices;
        private readonly FactoryProfessionalImp _factoryProfessional;
        private readonly FactoryLogImp _factoryLog;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        private readonly int _defaultMinutesForServiceExecution = 60;

        public BookService(
        FactoryBookImp factoryBook, 
        FactoryProfessionalServicesImp factoryProfessionalServices,
        FactoryProfessionalImp factoryProfessional,
        FactoryLogImp factoryLog,
        ILogger logger,
        IMapper mapper)
        {
            _factoryBook = factoryBook;
            _factoryProfessionalServices = factoryProfessionalServices;
            _factoryProfessional = factoryProfessional;
            _factoryLog = factoryLog;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> CancelBookById(CancelBookRequest request)
        {
            var result = await _factoryBook.CancelBook(request.BookId)
                .ConfigureAwait(false);

            if (!result.IsSuccess)
                return result.Exception;

            var log = new LogEvent();

            log.SetData(request.Reason, "Book.Cancel", "book", request.BookId, request.CancelBy);

            var logResult = await _factoryLog.Save(log).ConfigureAwait(false);

            if (!logResult.IsSuccess)
                _logger.Error($"[BookCancel] - {logResult.Exception.Message}");

            return Result.Success();
        }

        public async Task<Result<IEnumerable<BookViewModel>>> GetAvailableBooksByServiceId(string serviceId)
        {
            var serviceObjectId = ObjectId.Empty;

            ObjectId.TryParse(serviceId, out serviceObjectId);

            var (isSuccess, results, exception) = await _factoryBook.GetBookByService(serviceObjectId).ConfigureAwait(false);

            if (!isSuccess)
                return exception;

            return Result.Success(_mapper.Map<IEnumerable<BookViewModel>>(results));
        }

        public async Task<Result<IEnumerable<BookViewModel>>>
            MountANewBookWithLimitInDays(OpenBookRequestDto openBookRequest)
        {
            var avilableBook = new List<Book>();

            var differenceInDates = (openBookRequest.EndDate - openBookRequest.StartDate).TotalDays;

            var professionalReference = await _factoryProfessional
                .GetProfessionalById(openBookRequest.ProfessionalId)
                .ConfigureAwait(false);

            if (!professionalReference.IsSuccess)
                return professionalReference.Exception;

            var serviceDurationInMinutes = _defaultMinutesForServiceExecution;

            var servicesReferences = new List<Service>();

            for(var index = 0; index < openBookRequest.ServiceIds.Count; index++)
            {
                var serviceResult = await _factoryProfessionalServices
                    .GetServiceById(openBookRequest.ServiceIds.ElementAt(index)).ConfigureAwait(false);

                serviceDurationInMinutes = serviceResult.Value.Duration;

                servicesReferences.Add(serviceResult.Value);
            }

            if (openBookRequest.ServiceIds.Count > 1)
                serviceDurationInMinutes = openBookRequest.Duration;                

            for (int start = 0; start <= differenceInDates; start++)
            {
                var book = new Book
                {
                    Date = openBookRequest.StartDate,
                    IsEnabled = true,
                    ProfessionalReference = professionalReference.Value,
                    ServiceReferences = servicesReferences,
                    AvailableHours = GetAvailableHours(openBookRequest.StartDate, openBookRequest.StartTime, openBookRequest.EndTime, serviceDurationInMinutes)
                };

                avilableBook.Add(book);

                openBookRequest.StartDate = openBookRequest.StartDate.AddDays(1);

            }

            var (isSuccess, books, exception) = await _factoryBook.Save(avilableBook)
                .ConfigureAwait(false);

            if (!isSuccess)
                return exception;

            return Result.Success(_mapper.Map<IEnumerable<BookViewModel>>(books));
        }

        private IEnumerable<Time> GetAvailableHours(DateTime startDate, TimeSpan startHour, TimeSpan endHour, int duration)
        {
            List<Time> availableTimes = new List<Time>();
            DateTime start = startDate;
            DateTime end = startDate.Add(endHour);

            start.Add(startHour);

            while (end >= start)
            {
                if (start.Hour < startHour.Hours || start.Hour > endHour.Hours)
                {
                    start = start.AddMinutes(duration);
                    continue;
                }

                availableTimes.Add(new Time { 
                    AvailableHour = start.ToString("HH:mm", CultureInfo.InvariantCulture),
                });
                start = start.AddMinutes(duration);
            }
            return availableTimes;
        }
    }
}
