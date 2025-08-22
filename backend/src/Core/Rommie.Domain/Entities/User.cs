using Rommie.Domain.Abstractions;
using Rommie.Domain.Events;

namespace Rommie.Domain.Entities;

public class User : Entity
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string IdentityProviderId { get; private set; } = string.Empty;
    public bool TwoFactorEnabled { get; private set; } = false;
    public static User Create(string email, string firstName, string lastName, string identityProviderId)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            IdentityProviderId = identityProviderId,
            TwoFactorEnabled = false
        };
        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
        return user;
    }

    public void Update(string? firstName, string? lastName)
    {
        if (!string.IsNullOrEmpty(firstName))
        {
            FirstName = firstName;
        }
        if (!string.IsNullOrEmpty(lastName))
        {
            LastName = lastName;
        }
        RaiseDomainEvent(new UserUpdatedDomainEvent(Id));
    }
    public void ToggleTwoFactorAuthentication()
    {
        TwoFactorEnabled = !TwoFactorEnabled;
    }
}
