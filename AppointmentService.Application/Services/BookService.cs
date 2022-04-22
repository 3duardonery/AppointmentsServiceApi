using AppointmentService.Domain.Models;
using AppointmentService.Domain.Repository;
using AppointmentService.Domain.Services;
using AppointmentService.Shared.Dto;
using AppointmentService.Shared.ViewModels;
using AutoMapper;
using OperationResult;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace AppointmentService.Application.Services
{
    public sealed class BookService : BookServiceImp
    {
        private readonly FactoryBookImp _factoryBook;
        private readonly FactoryProfessionalServicesImp _factoryProfessionalServices;
        private readonly IMapper _mapper;

        public BookService(FactoryBookImp factoryBook, FactoryProfessionalServicesImp factoryProfessionalServices, IMapper mapper)
        {
            _factoryBook = factoryBook;
            _factoryProfessionalServices = factoryProfessionalServices;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<BookViewModel>>> GetAvailableBooksByServiceId(string serviceId)
        {
            var (isSuccess, results, exception) = await _factoryBook.GetBookByService(serviceId).ConfigureAwait(false);

            if (!isSuccess)
                return exception;

            return Result.Success(_mapper.Map<IEnumerable<BookViewModel>>(results));
        }

        public async Task<Result<IEnumerable<BookViewModel>>>
            MountANewBookWithLimitInDays(OpenBookRequestDto openBookRequest)
        {
            var avilableBook = new List<Book>();

            var differenceInDates = (openBookRequest.EndDate - openBookRequest.StartDate).TotalDays;

            var serviceDuration = await _factoryProfessionalServices
                .GetServiceById(openBookRequest.ServiceId).ConfigureAwait(false);

            if (!serviceDuration.IsSuccess)
                return serviceDuration.Exception;


            for (int start = 0; start <= differenceInDates; start++)
            {
                var book = new Book();

                book.Date = openBookRequest.StartDate;
                book.IsEnabled = true;
                book.ServiceReference = serviceDuration.Value;
                book.AvailableHours = GetAvailableHours(openBookRequest.StartDate, openBookRequest.StartTime, openBookRequest.EndTime, serviceDuration.Value.Duration);
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
