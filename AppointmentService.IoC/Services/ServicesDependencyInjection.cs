using AppointmentService.Application.Services;
using AppointmentService.Data.Repository;
using AppointmentService.Domain.Repository;
using AppointmentService.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AppointmentService.IoC.Services
{
    public static class ServicesDependencyInjection
    {
        public static void AddServicesInjection(this IServiceCollection services)
        {
            services.AddScoped<ProfessionalServiceImp, ProfessionalService>();
            services.AddScoped<FactoryProfessionalImp, FactoryProfessional>();

            services.AddScoped<FactoryProfessionalServicesImp, FactoryProfessionalServices>();
            services.AddScoped<ProfessionalServicesServiceImp, ProfessionalServicesService>();

            services.AddScoped<FactoryBookImp, FactoryBook>();
            services.AddScoped<BookServiceImp, BookService>();
        }
    }
}
