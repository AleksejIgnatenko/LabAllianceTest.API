using Microsoft.AspNetCore.Identity;

namespace LabAllianceTest.API.Exceptions
{
    public class UserCreationException : Exception
    {
        public IEnumerable<IdentityError> Errors { get; }

        public UserCreationException(IEnumerable<IdentityError> errors) : base("User creation failed.")
        {
            Errors = errors;
        }
    }
}
