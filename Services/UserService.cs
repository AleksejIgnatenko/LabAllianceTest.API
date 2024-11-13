using LabAllianceTest.API.Abstractions;
using LabAllianceTest.API.Exceptions;
using LabAllianceTest.API.Models;

namespace LabAllianceTest.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;

        public UserService(IUserRepository userRepository, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task<Guid> CreateUserAsync(Guid id, string login, string password)
        {
            var (errors, user) = UserModel.Create(id, login, password);
            if (errors.Count > 0)
            {
                throw new UserValidationException(errors);
            }

            await _userRepository.CreateUserAsync(user);

            return user.Id;
        }

        public async Task<string> LoginUserAsync(string login, string password)
        {
            var user = await _userRepository.LoginUserAsync(login, password);

            var token = _jwtProvider.GenerateToken(user);

            return token;
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }
    }
}
