using LabAllianceTest.API.Abstractions;
using LabAllianceTest.API.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LabAllianceTest.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly string _jwtSecret = "YourStrongBase64EncodedKeyHereShouldBe32Bytes";

        public TokenService(IUserRepository userRepository, ITokenRepository tokenRepository)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
        }

        public async Task<(string? accessToken, string? refreshTokenId)> ExchangeAsync(string login, string password)
        {
            var user = await _userRepository.AuthenticateUserAsync(login, password);

            // Генерация JWT токена
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            // Создание refresh токена
            var refreshTokenDescriptor = new OpenIddictTokenDescriptor
            {
                Subject = user.Id.ToString(),
                ExpirationDate = DateTimeOffset.UtcNow.AddDays(30),
                CreationDate = DateTime.UtcNow,
                Type = "refresh_token"
            };

            var refreshToken = await _tokenRepository.CreateRefreshTokenAsync(refreshTokenDescriptor);
            var refreshTokenId = await _tokenRepository.GetTokenIdAsync(refreshToken);

            return (accessToken, refreshTokenId);
        }

        public async Task<string> RefreshAsync(string refreshTokenId)
        {
            // Поиск RefreshToken по Id
            var refreshToken = await _tokenRepository.FindTokenAsync(refreshTokenId)
                ?? throw new TokenNotFoundException("Refresh token not found.");

            // Валидация refresh токена
            var tokenInfo = refreshToken;
            if (tokenInfo == null || tokenInfo.ExpirationDate < DateTimeOffset.UtcNow)
            {
                throw new InvalidTokenException("Invalid refresh token.");
            }

            var userId = tokenInfo.Subject;

            var claims = new[]
{
                new Claim(JwtRegisteredClaimNames.Sub, userId),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
            issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            var newAccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return newAccessToken;
        }
    }
}
