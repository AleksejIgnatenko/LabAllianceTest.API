﻿using LabAllianceTest.API.Abstractions;
using LabAllianceTest.API.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LabAllianceTest.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtProvider _jwtProvider;

        public UserController(IUserService userService, IJwtProvider jwtProvider)
        {
            _userService = userService;
            _jwtProvider = jwtProvider;
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

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> UserLoginAsync([FromBody] UserRequest userRequest)
        {
            var token = await _userService.LoginUserAsync(userRequest.Login, userRequest.Password);

            return Ok(new { token });
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();

            var response = users.Select(u => new UserResponse(u.Id, u.Login, u.Password)).ToList();

            return Ok(users);
        }

        [HttpGet]
        [Route("refreshToken")]
        public ActionResult RefreshToken()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var newToken = _jwtProvider.RefreshToken(token);

            return Ok(new { token });
        }
    }
}