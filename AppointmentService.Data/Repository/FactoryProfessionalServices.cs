using AppointmentService.Data.DataContext;
using AppointmentService.Domain.Models;
using AppointmentService.Domain.Repository;
using MongoDB.Driver;
using OperationResult;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentService.Data.Repository
{
    public sealed class FactoryProfessionalServices : FactoryProfessionalServicesImp
    {
        private readonly IMongoCollection<Service> _services;

        public FactoryProfessionalServices(MongoContext mongoContext)
            => _services = mongoContext.GetCollection<Service>("services");

        public async Task<Result<IEnumerable<Service>>> Services()
        {
            try
            {
                var services = await _services.FindAsync(_ => true).ConfigureAwait(false);

                return services.ToList();
            }
            catch (Exception ex)
            {
                return Result.Error<IEnumerable<Service>>(ex);
            }
        }

        public async Task<Result<IEnumerable<Service>>> GetServicesByIds(List<string> ids)
        {
            try
            {
                var filter = Builders<Service>.Filter.In(x => x.Id, ids);
                var services = await _services.FindAsync(filter: filter).ConfigureAwait(false);

                return services.ToList();
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public async Task<Result<Service>> Save(Service service)
        {
            try
            {
                await _services.InsertOneAsync(service).ConfigureAwait(false);
                return service;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
