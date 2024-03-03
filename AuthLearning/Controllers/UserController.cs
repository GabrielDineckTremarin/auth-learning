using AuthLearning.Business;
using AuthLearning.Models;
using AuthLearning.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthLearning.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    //[Authorize(AuthenticationSchemes = APIUserTokenScheme.Tracker)]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly BlUser _blUser;
        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _blUser = new BlUser(userService, configuration);
            _configuration = configuration;
        }

        [HttpPost]
        [Route("create-user")]
        [AllowAnonymous]
        public async Task<object> CreateUser([FromBody] NewUser user)
        {
            return await _blUser.CreateUser(user);
        }



        [HttpDelete]
        [Route("delete-user")]
        [Authorize]
        public async Task<object> DeleteUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _blUser.DeleteUser(userId);
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<object> CheckLogin([FromBody]DtoUser user)
        {
            try
            {
                var result = await _blUser.Login(user);
                return result;            
            }
            catch(Exception ex)
            {
                return new { Message = ex.Message, Success = false };
            }

        }




    }
}
