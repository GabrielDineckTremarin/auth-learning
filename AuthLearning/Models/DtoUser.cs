﻿
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthLearning.Models
{
    public class DtoUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}