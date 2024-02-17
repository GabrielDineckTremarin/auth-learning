using AuthLearning.Business;
using AuthLearning.Models;
using AuthLearning.Service;
using Microsoft.AspNetCore.Mvc;

namespace AuthLearning.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly BlUser _blUser;
        public UserController(IUserService userService)
        {
            _userService = userService;
            _blUser = new BlUser(userService);

        }

        [HttpPost]
        [Route("create-user")]
        public async Task<object> CreateUser([FromBody] NewUser user)
        {
            return await _blUser.CreateUser(user);
        }

    }
}
