using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace AuthLearning.Security
{
    public class JwtAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration _configuration;
        public JwtAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration configuration)
            : base(options, logger, encoder, clock)
        {
            _configuration = configuration;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Missing Authorization Header");
            }

            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var validatedToken = ValidateAndDecodeToken(token);

            if (validatedToken == null)
            {
                return AuthenticateResult.Fail("Invalid Token");
            }

            var claims = new[] { new Claim(ClaimTypes.Name, validatedToken.Email) };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

        private ValidatedToken ValidateAndDecodeToken(string token)
        {
            var key = configuration["Jwt:Key"];

            // vou validar o token aqui usando a chave
            return new ValidatedToken { Email = "123123213" };
        }
    }

    public class ValidatedToken
    {
        public string Email { get; set; }
        // objeto retornado par ao token valido/invalido
    }
}
