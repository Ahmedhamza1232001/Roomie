using System.Text.Json;
using Rommie.Application.Dtos.Requests;
using Rommie.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Rommie.Application.Dtos.Responses;
using Rommie.Domain.ValueObjects;
using Rommie.Api.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Rommie.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController(IUserService userService, IDocumentService documentService, ILogger<UserController> logger) : ControllerBase
    {
        [HttpPost("register")]
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
            logger.LogInformation("LoginUser called with {RequestDto}", JsonSerializer.Serialize(requestDto));
            var loginUserResponse = await userService.LoginUserAsync(requestDto.Email, requestDto.Password);
            logger.LogInformation("LoginUser succeeded for Email: {Email}", requestDto.Email);
            return Ok(loginUserResponse);
        }

        [HttpPost("upload-document")]
        [Authorize]
        public async Task<ActionResult<VerificationRequestStatus>> UploadDocument([FromForm] CreateDocumentRequestDto requestDto)
        {
            var userId = User.GetUserId();
            logger.LogInformation("UploadDocument called with {RequestDto}", JsonSerializer.Serialize(requestDto));
            var status = await documentService.CreateDocumentVerificationRequestAsync(userId, requestDto);
            logger.LogInformation("UploadDocument succeeded with status: {Status}", status);
            return Ok(status);
        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<LoginUserResponse>> RefreshToken([FromBody] RefreshTokenRequestDto requestDto)
        {
            logger.LogInformation("LoginUser called with {token}", JsonSerializer.Serialize(requestDto));
            var loginUserResponse = await userService.RefreshUserAsnc(requestDto);
            logger.LogInformation("LoginUser succeeded for token: {token}", requestDto.Token);
            return Ok(loginUserResponse);
        }
    }


}