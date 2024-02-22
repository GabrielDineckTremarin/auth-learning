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
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        IConfiguration _configuration;
        private readonly BlUser _blUser;
        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _blUser = new BlUser(userService);
            _configuration = configuration;
        }

        [HttpPost]
        [Route("create-user")]
        public async Task<object> CreateUser([FromBody] NewUser user)
        {
            return await _blUser.CreateUser(user);
        }

        [HttpGet]
        [Route("test-unauthorized")]
        [AllowAnonymous]
        public async Task<object> TestUnauthorized() 
        {
            return new { Message = "Hello World Unauthorized", Success = true };
        }

        [HttpGet]
        [Route("test-authorized")]
        [Authorize]
        public async Task<object> TestAuthorized()
        {
            return new { Message = "Hello World Authorized", Success = true };
        }

        [HttpPost]
        [Route("check-login")]
        [AllowAnonymous]
        public async Task<object> CheckLogin([FromBody]DtoUser user)
        {
            try
            {
                if (user.Email == "gabriel@gmail.com" && user.Password == "gab123")
                {

                    var claims = new[]
                    {
                    //new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Email", user.Email),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                         claims: claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                    return new { Message = "Authorized", Success = true, Token = tokenString };
                }
            }
            catch(Exception ex)
            {
                return new { Message = ex.Message, Success = false };
            }


            return new { Message = "Unauthorized", Success = false };
        }

    }
}
