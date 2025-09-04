using System.Text.Json;
using Rommie.Application.Dtos.Requests;
using Rommie.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Rommie.Application.Dtos.Responses;

namespace Rommie.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController(IUserService userService, ILogger<UserController> logger) : ControllerBase
    {
        [HttpPost()]
        public async Task<ActionResult<Guid>> CreateUser([FromBody] CreateUserRequestDto requestDto)
        {
            logger.LogInformation("CreateUser called with {RequestDto}", JsonSerializer.Serialize(requestDto));
            var userid = await userService.CreateUserAsync(requestDto);
            logger.LogInformation("CreateUser succeeded with UserId: {UserId}", userid);
            return Ok(userid);
        }
        [HttpPost("login")]
        public async Task<ActionResult<LoginUserResponse>> LoginUser([FromBody] LoginUserRequestDto requestDto)
        {
            return Ok(await userService.LoginUserAsync(requestDto.Email, requestDto.Password));
        }
    }
}