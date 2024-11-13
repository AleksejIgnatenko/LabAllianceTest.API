using LabAllianceTest.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace LabAllianceTest.API
{
    public class LabAllianceTestAPIDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public LabAllianceTestAPIDbContext(DbContextOptions<LabAllianceTestAPIDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
