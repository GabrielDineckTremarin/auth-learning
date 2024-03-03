using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Logging;

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
            var token = "";
            //if (!Request.Headers.ContainsKey("Authorization"))
            //{
            //    return AuthenticateResult.Fail("Missing Authorization Header");
            //}
            if (Request.Headers.ContainsKey(HeaderNames.Authorization) && Request.Headers[HeaderNames.Authorization].ToString().StartsWith("Bearer"))
                token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer", string.Empty).Trim();



            var validatedToken = ValidateAndDecodeToken(token);

            if (validatedToken == null || String.IsNullOrEmpty(validatedToken.Id))
            {
                return AuthenticateResult.Fail("Invalid Token");
            }

            var claims = new[] { new Claim(ClaimTypes.Name, validatedToken.Id) };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

        private ValidatedToken ValidateAndDecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var key = _configuration["Jwt:Key"];

                var validationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(key)),
          
                };
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return new ValidatedToken() { Id = userId };
            }
            catch (Exception ex) 
            {
                return null;
            }
        }
    }

    public class ValidatedToken
    {
        public string Id { get; set; }
        // objeto retornado par ao token valido/invalido
    }
}
