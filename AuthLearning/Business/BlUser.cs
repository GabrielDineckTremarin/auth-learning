using AuthLearning.Models;
using AuthLearning.Service;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthLearning.Business
{
    public class BlUser
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public BlUser(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        public async Task<object> CreateUser(NewUser newUser)
        {
            try
            {
                var validation = await ValidateUser(newUser);
                if(!string.IsNullOrEmpty(validation))throw new Exception(validation);

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.Password, BCrypt.Net.BCrypt.GenerateSalt(12));

                var user = new DtoUser()
                {
                    Email = newUser.Email,
                    Password = hashedPassword,
                    Username = newUser.Username,
                };
                _userService.CreateUser(user);


                return new { Message = "User created", Success = true };
            }
            catch (Exception ex)
            {
                return new { Message = ex.Message, Success = false };

            }
        }
        public async Task<string> ValidateUser(NewUser user)
        {
            if (user == null)
                return "Please, fill out the form correctly";

            if (String.IsNullOrEmpty(user.Email))
                return "You need to enter a valid email";

            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regexEmail = new Regex(emailPattern);
            if(!regexEmail.IsMatch(user.Email))
                return "You need to enter a valid email";

            var existingUser = _userService.GetUser(user.Email);
            if(existingUser != null)
                return "This email is registered already";

            if(String.IsNullOrEmpty(user.Username))
                return "You need to enter a valid username";

            var count = 0;
            foreach(var ch in user.Username)
            {
                count++;
            }

            if(count < 10)
                return "Your username must have at least 10 characters";

            if (count > 20)
                return "Your username must have at most 20 characters";

            if (String.IsNullOrEmpty(user.Password))
                return "You need to enter a valid password";

            if (String.IsNullOrEmpty(user.ConfirmPassword))
                return "You need to confirm your password";

            string regexPattern = "[@; ]";
            Regex regexUsername = new Regex(regexPattern);
            if(regexUsername.IsMatch(user.Username))
                return "Your username can not contain characters such as ';', '@' and blank spaces";


            count = 0;
            foreach(var ch in user.Password)
            {
                count++;
            }

            if (count < 10)
                return "Your password must have at least 10 characters";

            if (count > 20)
                return "Your password must have at most 20 characters";

            //string passPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@_])[A-Za-z\d@_]+$";
            string passPattern = @"^(?=.*[a-zA-Z])(?=.*\d).+";
            Regex regexPass = new Regex(passPattern);
            if(!regexPass.IsMatch(user.Password))
                return "Your password must have at least one special characters such as '@' or '_', one uppercase letter, one lowercase letter and one number";

            if (user.ConfirmPassword != user.Password)
                return "The passwords do not match";

            return String.Empty;

        }


        public async Task<object> Login(DtoUser user)
        {
            try
            {
                if (user == null) throw new Exception("Invalid user!");

                if(String.IsNullOrEmpty(user.Email) || String.IsNullOrEmpty(user.Password))
                    throw new Exception("You need to fill out the email and password fields!");

                var dbUser = _userService.GetUser(user.Email);

                if (dbUser == null) throw new Exception("User not found");

                if (dbUser == null) throw new Exception("There is not any user with this email!");

                if(dbUser.Email == user.Email && BCrypt.Net.BCrypt.Verify(user.Password, dbUser.Password))
                {
                    var claims = new[]
                    {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", user.Id),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        claims: claims,
                        expires: DateTime.UtcNow.AddMinutes(20),
                        signingCredentials: signIn);

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                    return new { Message = "Authorized", Success = true, Token = tokenString, Username = dbUser.Username };

                } 



                return new { Message = "Invalid Credentials", Success = false };
            }
            catch (Exception ex)
            {
                return new { Message = ex.Message, Success = false };

            }
        }


        public async Task<object> TestAuthentication(string userId)
        {
            var user = _userService.GetUser(userId);
            
            return new { 
                Message = user != null ? $"Hello {user.Username}, what's up." : "User not logged in", 
                Success = true,
                Data = user
            
            };
        }
    }


}
