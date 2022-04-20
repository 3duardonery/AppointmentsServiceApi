using MongoDB.Bson.Serialization.Attributes;

namespace AppointmentService.Domain.Models
{
    public sealed class Time
    {
        [BsonElement("availableHour")]
        public string AvailableHour { get; set; }

        [BsonElement("professionalId")]
        public string ProfessionalId { get; set; }

        [BsonElement("customerId")]
        public string CustomerId { get; set; }

        [BsonElement("isCancelled")]
        public bool IsCancelled { get; set; }

    }
}
