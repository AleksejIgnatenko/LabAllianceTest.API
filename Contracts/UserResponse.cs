namespace LabAllianceTest.API.Contracts
{
    public record UserResponse(
        Guid Id,
        string Login,
        string Password
        );
}
