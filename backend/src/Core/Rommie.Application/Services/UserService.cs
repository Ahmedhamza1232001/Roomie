using Microsoft.Extensions.Logging;
using Rommie.Application.Abstractions;
using Rommie.Application.Abstractions.Identity;
using Rommie.Application.Dtos.Requests;
using Rommie.Application.Dtos.Responses;
using Rommie.Application.Interfaces;
using Rommie.Application.Repositories;
using Rommie.Domain.Entities;
using Rommie.Domain.Exceptions;

namespace Rommie.Application.Services;

public class UserService(
    IUserRepository userRepository,
    IGenericRepository<User, Guid> userGenericRepository,
    IIdentityProviderService identityProviderService,
    ILogger<UserService> logger,
    IUnitOfWork unitOfWork) : IUserService
{
    public async Task<Guid> CreateUserAsync(CreateUserRequestDto createUserRequestDto, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating user with email: {Email}", createUserRequestDto.Email);
        User? user = await userRepository.GetByEmail(createUserRequestDto.Email);
        if (user != null)
        {
            logger.LogWarning("User creation failed. Email already exists: {Email}", createUserRequestDto.Email);
            throw new ConflictException("User.Conflict.Email", createUserRequestDto.Email);
        }
        string userIdentitfier = await identityProviderService.RegisterUserAsync(new UserModel(createUserRequestDto.Email, createUserRequestDto.Password, createUserRequestDto.FirstName, createUserRequestDto.LastName), cancellationToken);
        user = User.Create(createUserRequestDto.Email, createUserRequestDto.FirstName, createUserRequestDto.LastName, userIdentitfier);
        userGenericRepository.Add(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return user.Id;
    }

    public async Task<LoginUserResponse> LoginUserAsync(string email, string Password, CancellationToken cancellationToken = default)
    {
        return await identityProviderService.LoginUserAsync(email, Password, cancellationToken);
    }

    public async Task<LoginUserResponse> RefreshUserAsnc(RefreshTokenRequestDto refreshTokenRequestDto, CancellationToken cancellationToken = default)
    {
        return await identityProviderService.RefreshUserAsync(refreshTokenRequestDto.Token, cancellationToken);
    }



}
