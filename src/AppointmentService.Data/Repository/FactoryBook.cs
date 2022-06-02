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

        public async Task<Result<IEnumerable<Book>>> GetBookByService(ObjectId serviceId)
        {
            try
            {
                var filter = Builders<Book>.Filter.Eq("serviceReference._id", serviceId) & Builders<Book>.Filter.Gt("date", DateTime.Today);

                var books = await _books.FindAsync(filter).ConfigureAwait(false);

                return Result.Success(books.ToEnumerable());
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public async Task<Result<Book>> GetBookByServiceAndDate(ObjectId serviceId, DateTime sheduleDate)
        {
            try
            {
                var filterReferenceService = Builders<Book>.Filter
                    .Eq("serviceReference._id", serviceId);

                var filterDate = Builders<Book>.Filter.Eq("date", sheduleDate);

                var finalFilter = Builders<Book>.Filter.And(filterReferenceService, filterDate);

                var books = await _books.FindAsync(finalFilter).ConfigureAwait(false);

                return Result.Success(books.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public async Task<Result<Book>> UpdateAvailableHours(Book book)
        {
            try
            {
                var filter = Builders<Book>.Filter.Eq("_id", book.Id);

                var professional = await _books.UpdateOneAsync(filter,
                    Builders<Book>.Update.Set(rec => rec.AvailableHours, book.AvailableHours));

                return Result.Success(book);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public async Task<Result<Book>> GetBookById(string bookId)
        {
            try
            {
                var filter = Builders<Book>.Filter.Eq("_id", bookId);

                var books = await _books.FindAsync(filter).ConfigureAwait(false);

                return Result.Success(books.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public async Task<Result> CancelBook(string bookId)
        {
            try
            {
                var filter = Builders<Book>.Filter.Eq("_id", bookId);

                var professional = await _books.UpdateOneAsync(filter,
                    Builders<Book>.Update.Set(rec => rec.IsEnabled, false));

                return Result.Success();
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
