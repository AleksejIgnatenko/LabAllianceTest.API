namespace LabAllianceTest.API.Abstractions
{
    public interface ITokenService
    {
        Task<(string? accessToken, string? refreshTokenId)> ExchangeAsync(string login, string password);
        Task<string> RefreshAsync(string refreshTokenId);
    }
}