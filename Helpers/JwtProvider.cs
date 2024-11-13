using LabAllianceTest.API.Abstractions;
using LabAllianceTest.API.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LabAllianceTest.API.Helpers
{
    public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
    {
        private readonly JwtOptions _options = options.Value;

        public string GenerateToken(UserModel user)
        {
            Claim[] claims = [new("userId", user.Id.ToString()),
                             new("login", user.Login.ToString()!)];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_options.ExpiresHours));

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }

        //Обновление токена
        public string RefreshToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(jwtToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var userId = principal.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
                var login = principal.Claims.FirstOrDefault(c => c.Type == "login")?.Value;

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(login))
                {
                    throw new Exception("Invalid token");
                }

                var newUser = UserModel.Create(Guid.Parse(userId), login, false).user;

                return GenerateToken(newUser);
            }
            catch (Exception)
            {
                throw new Exception("Token is invalid or expired");
            }
        }
    }
}
