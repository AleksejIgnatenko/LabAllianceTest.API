using Microsoft.AspNetCore.Identity;

namespace LabAllianceTest.API.Entities
{
    public class UserEntity : IdentityUser
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
