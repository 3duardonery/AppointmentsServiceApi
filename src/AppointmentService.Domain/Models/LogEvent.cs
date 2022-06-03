using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;

namespace AppointmentService.Domain.Models
{
    public sealed class LogEvent
    {
        public LogEvent() => CreatedAt = DateTime.UtcNow;

        public void SetData(string description, string actionType, 
            string collectionName, string objectId, string username)
        {
            ActionType = actionType;
            Description = description;
            CollectionName = collectionName;
            ObjectId = objectId;
            Username = username;
        }

        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }

        [BsonElement("actionType")]
        public string ActionType { get; private set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; private set; }

        [BsonElement("description")]
        public string Description { get; private set; }

        [BsonElement("collectionName")]
        public string CollectionName { get; private set; }

        [BsonElement("objectId")]
        public string ObjectId { get; private set; }

        [BsonElement("username")]
        public string Username { get; private set; }
    }
}
