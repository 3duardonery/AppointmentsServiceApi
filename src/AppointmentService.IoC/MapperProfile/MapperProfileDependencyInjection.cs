using AppointmentService.Application.ProfileMapping;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AppointmentService.IoC.MapperProfile
{
    public static class MapperProfileDependencyInjection
    {
        public static void AddMapperProfileConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ProfileMapping).GetTypeInfo().Assembly);
        }
    }
}
