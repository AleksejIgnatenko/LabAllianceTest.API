using LabAllianceTest.API.Abstractions;
using OpenIddict.Abstractions;
using OpenIddict.EntityFrameworkCore.Models;

namespace LabAllianceTest.API.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly LabAllianceTestAPIDbContext _context;
        private readonly IOpenIddictTokenManager _tokenManager;

        public TokenRepository(LabAllianceTestAPIDbContext context, IOpenIddictTokenManager tokenManager)
        {
            _context = context;
            _tokenManager = tokenManager;
        }

        public async Task<OpenIddictEntityFrameworkCoreToken?> FindTokenAsync(string refreshToken)
        {
            return await _context.Tokens.FindAsync(refreshToken);
        }

        public async Task<object?> CreateRefreshTokenAsync(OpenIddictTokenDescriptor refreshTokenDescriptor)
        {
            return await _tokenManager.CreateAsync(refreshTokenDescriptor);
        }

        public async Task<string?> GetTokenIdAsync(object token)
        {
            return await _tokenManager.GetIdAsync(token);
        }
    }
}
