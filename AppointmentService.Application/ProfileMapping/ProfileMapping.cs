using AppointmentService.Domain.Models;
using AppointmentService.Shared.Dto;
using AppointmentService.Shared.ViewModels;
using AutoMapper;

namespace AppointmentService.Application.ProfileMapping
{
    public sealed class ProfileMapping : Profile
    {
        public ProfileMapping()
        {
            CreateMap<Professional, ProfessionalViewModel>().ReverseMap();
            CreateMap<Professional, ProfessionalBookViewModel>().ReverseMap();
            CreateMap<Professional, ProfessionalDto>().ReverseMap();
            CreateMap<Service, ServiceViewModel>().ReverseMap();
            CreateMap<Service, ServiceBookViewModel>().ReverseMap();
            CreateMap<Service, ProfessionalServiceDto>().ReverseMap();
            CreateMap<Time, TimeViewModel>().ReverseMap();
            CreateMap<Book, BookViewModel>().ForPath(d => d.BookDateStringValue, 
                opt => opt.MapFrom(b => b.Date.ToString("dd/MM/yyyy"))).ReverseMap();
        }
    }
}
