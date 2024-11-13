using LabAllianceTest.API.Models;

namespace LabAllianceTest.API.Abstractions
{
    public interface IUserRepository
    {
        Task<Guid> CreateUserAsync(UserModel user);
        Task<List<UserModel>> GetAllUsersAsync();
        Task<UserModel> LoginUserAsync(string login, string password);
    }
}