using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Collections.Generic;

namespace AppointmentService.Domain.Models
{
    public sealed class Book
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("date")]
        public string Date { get; set; }

        [BsonElement("availableTimes")]
        public IEnumerable<Time> AvailableTimes { get; set; }

        [BsonElement("serviceId")]
        public string ServiceId { get; set; }


        [BsonElement("isEnabled")]
        public bool IsEnabled { get; set; }
    }
}
