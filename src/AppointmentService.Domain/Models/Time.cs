using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace AppointmentService.Domain.Models
{
    public sealed class Time
    {
        public Time() => Id = Guid.NewGuid().ToString();

        public string Id { get; set; }

        [BsonElement("availableHour")]
        public string AvailableHour { get; set; }

        [BsonElement("professionalId")]
        public string ProfessionalId { get; set; }

        [BsonElement("customerId")]
        public string CustomerId { get; set; }

        [BsonElement("customerName")]
        public string CustomerName { get; set; }

        [BsonElement("isCancelled")]
        public bool IsCancelled { get; set; }

    }
}
