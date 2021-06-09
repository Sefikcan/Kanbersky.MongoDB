using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Kanbersky.MongoDB.Models
{
    public abstract class BaseMongoEntity
    {
        public BaseMongoEntity()
        {
            CreatedOn = DateTime.Now;
            ModifiedOn = DateTime.Now;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedOn { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime ModifiedOn { get; set; }
    }
}
