using LabAllianceTest.API.Abstractions;
using LabAllianceTest.API.Entities;
using LabAllianceTest.API.Exceptions;
using LabAllianceTest.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LabAllianceTest.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LabAllianceTestAPIDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public UserRepository(LabAllianceTestAPIDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<Guid> CreateUserAsync(UserModel user)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Login.Equals(user.Login));

            if (existingUser != null) { throw new RepositoryException("Пользователь с таким логином уже существует."); }
            else
            {
                UserEntity userEntity = new UserEntity
                {
                    Id = user.Id,
                    Login = user.Login,
                    Password = _passwordHasher.Generate(user.Password)
                };

                await _context.Users.AddAsync(userEntity);
                await _context.SaveChangesAsync();

                return userEntity.Id;
            }
        }

        public async Task<UserModel> LoginUserAsync(string login, string password)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Login.Equals(login))
                ?? throw new AuthenticationFailedException("Неверный логин или пароль."); ;

            var user = UserModel.Create(
                existingUser.Id,
                existingUser.Login,
                existingUser.Password,
                false).user;

            return user;
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            var userEntities = await _context.Users.ToListAsync();

            var users = userEntities.Select(u => UserModel.Create(u.Id, u.Login, u.Password, false).user).ToList();

            return users;
        }
    }
}
