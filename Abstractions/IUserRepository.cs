using LabAllianceTest.API.Models;

namespace LabAllianceTest.API.Abstractions
{
    public interface IUserRepository
    {
        Task<Guid> CreateUserAsync(UserModel user);
        Task<UserModel> AuthenticateUserAsync(string login, string password);
        Task<List<UserModel>> GetAllUsersAsync();
    }
}