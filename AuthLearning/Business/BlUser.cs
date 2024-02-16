using AuthLearning.Models;
using AuthLearning.Service;

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
    }


}
