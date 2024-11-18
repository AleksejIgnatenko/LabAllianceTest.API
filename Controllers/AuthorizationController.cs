using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.Data;
using LabAllianceTest.API.Contracts;
using LabAllianceTest.API.Abstractions;

namespace LabAllianceTest.API.Controllers
{
    [ApiController]
    [Route("connect")]
    public class AuthorizationController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthorizationController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        // Создание токена
        [HttpPost("token")]
        public async Task<ActionResult> ExchangeAsync([FromBody] UserRequest request)
        {
            var (accessToken, refreshTokenId) = await _tokenService.ExchangeAsync(request.Login, request.Password);

            return Ok(new
            {
                access_token = accessToken,
                refresh_token = refreshTokenId,
                token_type = "bearer",
            });
        }

        // Обновления access токена
        [HttpPost("refresh")]
        public async Task<ActionResult> Refresh([FromBody] string refreshTokenRequest)
        {
            Console.WriteLine(refreshTokenRequest);
            var newAccessToken = await _tokenService.RefreshAsync(refreshTokenRequest);

            return Ok(new
            {
                access_token = newAccessToken,
                refresh_token = refreshTokenRequest,
                token_type = "Bearer",
            });
        }
    }
}
