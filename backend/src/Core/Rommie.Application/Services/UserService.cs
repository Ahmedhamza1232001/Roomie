using Rommie.Application.Abstractions;
using Rommie.Application.Abstractions.Identity;
using Rommie.Application.Dtos.Requests;
using Rommie.Application.Interfaces;
using Rommie.Application.Repositories;
using Rommie.Domain.Entities;
using Rommie.Domain.Exceptions;

namespace Rommie.Application.Services;

public class UserService(
    IUserRepository userRepository,
    IGenericRepository<User, Guid> userGenericRepository,
    IIdentityProviderService identityProviderService,
    IUnitOfWork unitOfWork) : IUserService
{
    public async Task<Guid> CreateUser(CreateUserRequestDto createUserRequestDto, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByEmail(createUserRequestDto.Email);
        if (user != null)
        {
            throw new ConflictException("User.Conflict.Email", createUserRequestDto.Email);
        }
        string userIdentitfier = await identityProviderService.RegisterUserAsync(new UserModel(createUserRequestDto.Email, createUserRequestDto.Password, createUserRequestDto.FirstName, createUserRequestDto.LastName), cancellationToken);
        user = User.Create(createUserRequestDto.Email, createUserRequestDto.FirstName, createUserRequestDto.LastName, userIdentitfier);
        userGenericRepository.Add(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return user.Id;
    }

    public async Task<bool> ToggleTwoFactorAuthenticationAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await userGenericRepository.GetById(userId) ?? throw new NotFoundException("User.NotFound", userId);
        await identityProviderService.ToggleTwoFactorAuthenticationAsync(user.IdentityProviderId, user.TwoFactorEnabled, cancellationToken);
        user.ToggleTwoFactorAuthentication();
        userGenericRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return user.TwoFactorEnabled;
    }

    public async Task UpdateUserAsync(Guid userId, UpdateUserRequestDto updateUserRequest, CancellationToken cancellationToken = default)
    {
        var user = await userGenericRepository.GetById(userId);
        if (user == null)
        {
            throw new NotFoundException("User.NotFound", userId);
        }
        user.Update(updateUserRequest.FirstName, updateUserRequest.LastName);
        userGenericRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
