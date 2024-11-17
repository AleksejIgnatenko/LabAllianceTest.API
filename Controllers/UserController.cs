using LabAllianceTest.API.Abstractions;
using LabAllianceTest.API.Contracts;
using LabAllianceTest.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LabAllianceTest.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("registration")]
        public async Task<ActionResult> UserRegistrationAsync([FromBody] UserRequest userRequest)
        {
            var userId = await _userService.CreateUserAsync(Guid.NewGuid(),
                                                            userRequest.Login,
                                                            userRequest.Password);

            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();

            var response = users.Select(u => new UserResponse(u.Id, u.Login, u.Password)).ToList();

            return Ok(users);
        }
    }
}
