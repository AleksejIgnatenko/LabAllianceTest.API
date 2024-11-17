namespace LabAllianceTest.API.Exceptions
{
    public class UserValidationException : Exception
    {
        public Dictionary<string, string> Errors { get; }

        public UserValidationException(Dictionary<string, string> errors)
        {
            Errors = errors;
        }
    }
}
