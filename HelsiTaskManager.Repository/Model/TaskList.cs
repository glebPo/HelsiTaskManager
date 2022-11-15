using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace HelsiTaskManager.Repository
{
    public class TaskList : BaseEntity
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        [BsonRepresentation(BsonType.String)]
        public string Name { get; set; }
        public List<Goal> Goals { get; set; }
        public List<ObjectId> LinkedUsers { get; set; }
    }
}
