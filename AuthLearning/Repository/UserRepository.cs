using AuthLearning.Models;
using AuthLearning.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AuthLearning.Repository
{
    public interface IUserRepository
    {
        void CreateUser(DtoUser user);
        void UpdateUser(DtoUser user);
        void DeleteUser(string email);
        DtoUser GetUser(string email);
        DtoUser GetUserById(string id);

    }
    public class UserRepository : IUserRepository
    {
        private readonly MongoContext _mongoContext;
        private readonly IMongoCollection<DtoUser> _userCollection;
        public UserRepository()
        {
            _mongoContext = new MongoContext();
            _userCollection = _mongoContext.GetCollection<DtoUser>("DtoUser");
        }

        public void CreateUser(DtoUser user)
        {
            _userCollection.InsertOne(user);
        }

        public DtoUser GetUser(string email)
        {
            var filter = Builders<DtoUser>.Filter.Eq("Email", email);
            var user = _userCollection.Find(filter).FirstOrDefault();
            return user;
        }

        public DtoUser GetUserById(string id)
        {
            var filter = Builders<DtoUser>.Filter.Eq("_id", new ObjectId(id));
            var user = _userCollection.Find(filter).FirstOrDefault();
            return user;
        }

        public void UpdateUser(DtoUser user)
        {
   
            var filter = Builders<DtoUser>.Filter.Eq("Email", user.Email);
            var update = Builders<DtoUser>.Update
                .Set(x => x.Username, user.Username)
                .Set(x => x.Email, user.Email)
                .Set(x => x.Password, user.Password);

            _userCollection.UpdateOne(filter, update);

        }

        public void DeleteUser(string email)
        {
            var filter = Builders<DtoUser>.Filter.Eq("_id", ObjectId.Parse(email));
            _userCollection.DeleteOne(filter);
        }
    }
}
