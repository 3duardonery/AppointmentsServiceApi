using AppointmentService.Data.DataContext;
using AppointmentService.Domain.Models;
using AppointmentService.Domain.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using OperationResult;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentService.Data.Repository
{
    public sealed class FactoryBook : FactoryBookImp
    {
        private readonly IMongoCollection<Book> _books;

        public FactoryBook(MongoContext mongoContext)
            => _books = mongoContext.GetCollection<Book>("books");

        public async Task<Result<IEnumerable<Book>>> Save(IEnumerable<Book> books)
        {
            try
            {
                await _books.InsertManyAsync(books).ConfigureAwait(false);

                return Result.Success(books);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public async Task<Result<IEnumerable<Book>>> GetBookByService(string serviceId)
        {
            try
            {
                var filter = Builders<Book>.Filter.Eq("serviceReference._id", ObjectId.Parse(serviceId));

                var books = await _books.FindAsync(filter).ConfigureAwait(false);

                return Result.Success(books.ToEnumerable());
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
