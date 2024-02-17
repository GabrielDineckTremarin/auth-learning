using AuthLearning.Models;
using AuthLearning.Service;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthLearning.Business
{
    public class BlUser
    {
        private readonly IUserService _userService;
        public BlUser(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<object> CreateUser(DtoUser user)
        {
            try
            {
                _userService.CreateUser(user);
                return new { Message = "User created", Success = true };
            }
            catch (Exception ex)
            {
                return new { Message = ex.Message, Success = false };

            }
        }

        public async Task<object> teste()
        {
            try
            {
                var user = new DtoUser()
                {
                    Email = "teste@gmail.com",
                    Password = "password123",
                    Username = "TesteUser",

                };
                _userService.CreateUser(user);

                var createdUser = _userService.GetUser(email: user.Email.ToString());
                return new { Message = "User created", Success = true };
            }
            catch (Exception ex)
            {
                return new { Message = ex.Message, Success = false };

            }
        }
    }


}
