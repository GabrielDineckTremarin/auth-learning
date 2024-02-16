using AuthLearning.Models;
using AuthLearning.Repository;

namespace AuthLearning.Service
{
    public interface IUserService
    {
        DtoUser GetUser(string email);
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
