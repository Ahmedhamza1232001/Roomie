using System.Text.Json;
using Rommie.Application.Dtos.Requests;
using Rommie.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Rommie.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpGet("auth")]
        public ActionResult<string> TestAuth()
        {
            var claims = User.Claims.ToDictionary(c => c.Type, c => c.Value);
            var json = JsonSerializer.Serialize(claims, new JsonSerializerOptions { WriteIndented = true });
            return Ok(json);
        }
        [HttpPost()]
        public async Task<ActionResult<Guid>> CreateUser([FromBody] CreateUserRequestDto requestDto)
        {
            var userid = await userService.CreateUser(requestDto);
            return Ok(userid);
        }
        [HttpPatch("toggle-two-factor-authentication/{userId}")]
        public async Task<ActionResult> ToggleTFA([FromRoute] Guid userId)
        {
            bool TfaStatus = await userService.ToggleTwoFactorAuthenticationAsync(userId);
            return Ok(TfaStatus);
        }
    }
}