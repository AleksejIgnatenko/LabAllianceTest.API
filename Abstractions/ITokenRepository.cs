using OpenIddict.Abstractions;
using OpenIddict.EntityFrameworkCore.Models;

namespace LabAllianceTest.API.Abstractions
{
    public interface ITokenRepository
    {
        Task<OpenIddictEntityFrameworkCoreToken?> FindTokenAsync(string refreshToken);
        Task<object?> CreateRefreshTokenAsync(OpenIddictTokenDescriptor refreshTokenDescriptor);
        Task<string?> GetTokenIdAsync(object token);
    }
}