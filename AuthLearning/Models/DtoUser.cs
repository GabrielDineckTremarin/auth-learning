
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthLearning.Models
{
    public class DtoUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }

    public class NewUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
    }
}
