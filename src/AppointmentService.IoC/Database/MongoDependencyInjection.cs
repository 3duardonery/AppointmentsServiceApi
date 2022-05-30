using AppointmentService.Data.DataContext;
using AppointmentService.Shared.Settings;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace AppointmentService.IoC.Database
{
    public static class MongoDependencyInjection
    {
        public static void AddMongoDBConfiguration(this IServiceCollection services, AppSettings settings)
        {
            services.AddSingleton((IMongoClient)new MongoClient(settings.ConnectionString));
            services.AddSingleton<MongoContext>();
        }
    }
}
