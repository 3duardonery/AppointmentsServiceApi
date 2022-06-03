using AppointmentService.Data.DataContext;
using AppointmentService.Domain.Models;
using AppointmentService.Domain.Repository;
using MongoDB.Driver;
using OperationResult;
using System;
using System.Threading.Tasks;

namespace AppointmentService.Data.Repository
{
    public sealed class FactoryLog : FactoryLogImp
    {
        private readonly IMongoCollection<Log> _logs;

        public FactoryLog(MongoContext mongoContext)
            => _logs = mongoContext.GetCollection<Log>("logs");

        public async Task<Result<Log>> Save(Log log)
        {
            try
            {
                await _logs.InsertOneAsync(log).ConfigureAwait(false);

                return Result.Success(log);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
