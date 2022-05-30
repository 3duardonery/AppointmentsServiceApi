using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;

namespace AppointmentService.Domain.Models
{
    public sealed class Appointment
    {
        public Appointment() => SetCreateAtToCurrentDate();

        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("bookId")]
        public string BookId { get; set; }

        [BsonElement("slotId")]
        public string SlotId { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; private set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; private set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("time")]
        public TimeSpan Time { get; set; }

        [BsonElement("executed")]
        public bool Executed { get; set; }

        [BsonElement("isCancelled")]
        public bool IsCancelled { get; set; }

        [BsonElement("customerId")]
        public string CustomerId { get; set; }

        [BsonElement("professionalReference")]
        public Professional ProfessionalReference { get; set; }

        [BsonElement("serviceReference")]
        public Service ServiceReference { get; set; }

        private void SetCreateAtToCurrentDate()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}
