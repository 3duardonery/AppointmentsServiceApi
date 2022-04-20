﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace AppointmentService.Domain.Models
{
    public sealed class Professional
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("profilePicture")]
        public string ProfilePicture { get; set; }

        [BsonElement("isEnabled")]
        public bool IsEnabled { get; set; }

    }
}
