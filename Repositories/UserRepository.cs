using Azure.Core;
using LabAllianceTest.API.Abstractions;
using LabAllianceTest.API.Entities;
using LabAllianceTest.API.Exceptions;
using LabAllianceTest.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LabAllianceTest.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly LabAllianceTestAPIDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public UserRepository(LabAllianceTestAPIDbContext context, IPasswordHasher passwordHasher, UserManager<UserEntity> userManager)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _userManager = userManager;
        }

        public async Task<Guid> CreateUserAsync(UserModel user)
        {
            var existingUser = await _userManager.FindByNameAsync(user.Login);
            if (existingUser != null)
            {
                throw new RepositoryException("Username already exists.");
            }

            var userEntity = new UserEntity
            {
                Id = user.Id.ToString(),
                UserName = user.Login,
                Password = user.Password
            };

            var result = await _userManager.CreateAsync(userEntity, userEntity.Password);
            if (!result.Succeeded)
            {
                throw new UserCreationException(result.Errors);
            }

            return Guid.Parse(userEntity.Id);
        }

        public async Task<UserModel> AuthenticateUserAsync(string login, string password)
        {
            var userEntity = await _userManager.FindByNameAsync(login);
            if (userEntity == null || !await _userManager.CheckPasswordAsync(userEntity, password))
            {
                throw new AuthenticationFailedException("Invalid credentials.");
            }

            var userModel = UserModel.Create(
                Guid.Parse(userEntity.Id),
                userEntity.Login,
                userEntity.Password,
                false).user;

            return userModel;
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            var userEntities = await _context.Users.ToListAsync();

            var users = userEntities.Select(u => UserModel.Create(Guid.Parse(u.Id), u.UserName, u.Password, false).user).ToList();

            return users;
        }
    }
}
