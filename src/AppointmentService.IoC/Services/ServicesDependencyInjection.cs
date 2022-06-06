using AppointmentService.Application.Services;
using AppointmentService.Data.Repository;
using AppointmentService.Domain.Repository;
using AppointmentService.Domain.Services;
using AppointmentService.Shared.Settings;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AppointmentService.IoC.Services
{
    public static class ServicesDependencyInjection
    {
        public static void AddServicesInjection(this IServiceCollection services, AppSettings settings)
        {
            services.AddScoped<ProfessionalServiceImp, ProfessionalService>();
            services.AddScoped<FactoryProfessionalImp, FactoryProfessional>();

            services.AddScoped<FactoryProfessionalServicesImp, FactoryProfessionalServices>();
            services.AddScoped<ProfessionalServicesServiceImp, ProfessionalServicesService>();

            services.AddScoped<FactoryBookImp, FactoryBook>();
            services.AddScoped<BookServiceImp, BookService>();

            services.AddScoped<FactoryAppointmentImp, FactoryAppointment>();
            services.AddScoped<AppointmentBookServiceImp, AppointmentBookService>();

            services.AddScoped<FactoryLogImp, FactoryLog>();

            services.AddHttpClient<AuthenticationServiceImp, AuthenticationService>(builder => {
                builder.BaseAddress = new Uri(settings.AuthEndpoint);                
            });
        }
    }
}
