using AuthLearning.Models;
using AuthLearning.Repository;

namespace AuthLearning.Service
{
    public interface IUserService
    {
        DtoUser GetUser(string email);
        DtoUser GetUserById(string id);
        void CreateUser(DtoUser user);
        void UpdateUser(DtoUser user);
        void DeleteUser(string email);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public DtoUser GetUser(string email)
        {
            return _userRepository.GetUser(email);
        }
        
        public DtoUser GetUserById(string id)
        {
            return _userRepository.GetUserById(id);
        }

        public void CreateUser(DtoUser user)
        {
            _userRepository.CreateUser(user);
        }


        public void UpdateUser(DtoUser user)
        {
            _userRepository.UpdateUser(user);
        }

        public void DeleteUser(string email)
        {
            _userRepository.DeleteUser(email);
        }
    }
}
