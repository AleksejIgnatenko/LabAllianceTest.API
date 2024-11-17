using LabAllianceTest.API.Abstractions;
using LabAllianceTest.API.Exceptions;
using LabAllianceTest.API.Models;
namespace LabAllianceTest.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Guid> CreateUserAsync(Guid id, string login, string password)
        {
            var (errors, user) = UserModel.Create(id, login, password);
            if (errors.Count > 0)
            {
                throw new UserValidationException(errors);
            }

            var userId = await _userRepository.CreateUserAsync(user);

            return userId;
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }
    }
}
