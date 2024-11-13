using LabAllianceTest.API.Models;

namespace LabAllianceTest.API.Abstractions
{
    public interface IJwtProvider
    {
        string GenerateToken(UserModel user);
        string RefreshToken(string jwtToken);
    }
}