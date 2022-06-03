using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace AppointmentService.Domain.Models
{
    public sealed class Book
    {
        public Book() => Id = Guid.NewGuid().ToString();
        public string Id { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("availableTimes")]
        public IEnumerable<Time> AvailableHours { get; set; }

        [BsonElement("serviceReference")]
        public Service ServiceReference { get; set; }

        [BsonElement("professionalReference")]
        public Professional ProfessionalReference { get; set; }

        [BsonElement("isEnabled")]
        public bool IsEnabled { get; set; }       
    }
}
