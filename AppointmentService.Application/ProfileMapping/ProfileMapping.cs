﻿using AppointmentService.Domain.Models;
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
            CreateMap<Professional, ProfessionalDto>().ReverseMap();
            CreateMap<Service, ProfessionalServiceViewModel>().ReverseMap();
            CreateMap<Service, ProfessionalServiceDto>().ReverseMap();
        }
    }
}
