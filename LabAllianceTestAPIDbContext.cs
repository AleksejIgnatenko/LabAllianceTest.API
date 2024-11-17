using LabAllianceTest.API.Entities;
using LabAllianceTest.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenIddict.EntityFrameworkCore.Models;

namespace LabAllianceTest.API
{
    public class LabAllianceTestAPIDbContext : IdentityDbContext<UserEntity>
    {
        // DbSet для хранения токенов OpenIddict
        public DbSet<OpenIddictEntityFrameworkCoreToken> Tokens { get; set; }
        public DbSet<OpenIddictEntityFrameworkCoreApplication> Applications { get; set; }
        public DbSet<OpenIddictEntityFrameworkCoreAuthorization> Authorizations { get; set; }
        //public DbSet<UserEntity> UserEntities { get; set; }

        public LabAllianceTestAPIDbContext(DbContextOptions<LabAllianceTestAPIDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        // Метод для настройки модели
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Если необходимы еще дополнительные настройки для вашей модели
        }
    }
}
