using FluentValidation.Results;
using LabAllianceTest.API.Validations;

namespace LabAllianceTest.API.Models
{
    public class UserModel
    {
        public Guid Id { get; }
        public string Login { get; } = string.Empty;
        public string Password { get; } = string.Empty;

        public UserModel(Guid id, string login)
        {
            Id = id;
            Login = login;
        }

        public UserModel(Guid id, string login, string password)
        {
            Id = id;
            Login = login;
            Password = password;
        }

        public static (Dictionary<string, string> errors, UserModel user) Create(Guid id, string login, bool useValidation = true)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            UserModel user = new UserModel(id, login);
            if (!useValidation) { return (errors, user); }

            UserValidation userValidation = new UserValidation();
            ValidationResult result = userValidation.Validate(user);
            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    errors[failure.PropertyName] = failure.ErrorMessage;
                }
            }

            return (errors, user);
        }

        public static (Dictionary<string, string> errors, UserModel user) Create(Guid id, string login, string password, bool useValidation = true)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            UserModel user = new UserModel(id, login, password);
            if (!useValidation) { return (errors, user); }

            UserValidation userValidation = new UserValidation();
            ValidationResult result = userValidation.Validate(user);
            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    errors[failure.PropertyName] = failure.ErrorMessage;
                }
            }

            return (errors, user);
        }
    }
}
