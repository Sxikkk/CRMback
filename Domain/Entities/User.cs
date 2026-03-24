using Domain.Enums;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public Email Email { get; private set; }
    public Phone Phone { get; private set; }
    public string UserName { get; private set; }
    public string PasswordHash { get; private set; }
    public ERole Role { get; private set; }
    public ICollection<RefreshToken> RefreshTokens { get; private set; } = null!;
    public Guid? OrganizationId { get; private set; }
    public Organization? Organization { get; private set; }
    private User() { }

    public static User Create(
        string name,
        string surname,
        Email email,
        Phone phone,
        string userName,
        string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Name required");
        if (string.IsNullOrWhiteSpace(surname)) throw new DomainException("Surname required");
        if (string.IsNullOrWhiteSpace(userName)) throw new DomainException("Username required");
        if (string.IsNullOrWhiteSpace(passwordHash)) throw new DomainException("Password hash required");
        
        return new User
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Surname = surname.Trim(),
            Email = email,
            Phone = phone,
            UserName = userName.Trim(),
            PasswordHash = passwordHash,
            RefreshTokens =  new List<RefreshToken>(),
            Role = ERole.User,
        };
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Name cannot be empty");
        Name = name.Trim();
    }

    public void ChangeRole(ERole role)
    {
        Role = role;
    }
    
    public void ChangeUserName(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName)) throw new DomainException("Username cannot be empty");
        UserName = userName.Trim();
    }
    
    public void ChangePassword(string hashedPassword)
    {
        PasswordHash = hashedPassword ?? throw new ArgumentNullException(nameof(hashedPassword));
    }
    
    public void ChangePhone(Phone phone)
    {
        Phone = phone ?? throw new ArgumentNullException(nameof(phone));
    }
    
    public void ChangeEmail(Email newEmail)
    {
        Email = newEmail ?? throw new ArgumentNullException(nameof(newEmail));
    }
    
    public void AssignToOrganization(Guid organizationId)
    {
        OrganizationId = organizationId;
    }
}