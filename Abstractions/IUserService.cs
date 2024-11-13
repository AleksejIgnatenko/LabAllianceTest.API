using LabAllianceTest.API.Models;

namespace LabAllianceTest.API.Abstractions
{
    public interface IUserService
    {
        Task<Guid> CreateUserAsync(Guid id, string login, string password);
        Task<List<UserModel>> GetAllUsersAsync();
        Task<string> LoginUserAsync(string login, string password);
    }
}