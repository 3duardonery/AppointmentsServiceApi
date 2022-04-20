using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace AppointmentService.Domain.Models
{
    public sealed class ProfessionalService
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("duration")]
        public int Duration { get; set; }

        [BsonElement("isEnabled")]
        public bool IsEnabled { get; set; }
    }
}
