using AppointmentService.Data.DataContext;
using AppointmentService.Domain.Models;
using AppointmentService.Domain.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using OperationResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentService.Data.Repository
{
    public sealed class FactoryProfessional : FactoryProfessionalImp
    {

        private readonly IMongoCollection<Professional> _professionals;

        public FactoryProfessional(MongoContext mongoContext)
            => _professionals = mongoContext.GetCollection<Professional>("professionals");

        public Task<Result<bool>> Disable(string professionalId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Professional>> Enable(string professionalId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<Professional>> GetProfessionalById(string professionalId)
        {
            try
            {
                var filter = Builders<Professional>.Filter.Eq("_id", ObjectId.Parse(professionalId));

                var professional = await _professionals.FindAsync(filter).ConfigureAwait(false);

                return professional.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public async Task<Result<IEnumerable<Professional>>> Professionals()
        {
            try
            {
                var professionals = await _professionals.FindAsync(_ => true);

                return professionals.ToList();
            }
            catch (Exception ex)
            {
                return Result.Error<IEnumerable<Professional>>(ex);
            }
        }

        public async Task<Result<Professional>> Save(Professional professional)
        {
            try
            {
                await _professionals.InsertOneAsync(professional).ConfigureAwait(false);

                return Result.Success(professional);
            }
            catch (Exception ex)
            {
                return Result.Error<Professional>(ex);
            }
        }

        public async Task<Result> SetServices(string professionalId, IEnumerable<Service> services)
        {
            try
            {
                var filter = Builders<Professional>.Filter.Eq("_id", ObjectId.Parse(professionalId));

                var professional = await _professionals.UpdateOneAsync(filter,
                    Builders<Professional>.Update.Set(rec => rec.Services, services));

                return Result.Success();
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
