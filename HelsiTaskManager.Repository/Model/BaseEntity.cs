using MongoDB.Bson.Serialization.Attributes;

namespace HelsiTaskManager.Repository
{
    public class BaseEntity
    {
        public ObjectId Id { get; set; }
        public ObjectId OwnerId { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Created { get; set; }
    }
}
