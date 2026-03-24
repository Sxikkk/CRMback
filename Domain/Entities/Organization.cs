using Domain.Enums;

namespace Domain.Entities;

public class Organization
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? Inn { get; private set; }
    public string? Ogrn { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Website { get; private set; }
    public string? City { get; private set; }
    public DateTime CreateDate { get; private set; } =  DateTime.UtcNow;
    public EOrganizationType Type { get; private set; }
    public EOrganizationStatus Status { get; private set; }
    
    private Organization() { }

    public static Organization Create(string name, EOrganizationType type = EOrganizationType.Company)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name required");

        return new Organization
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Type = type,
            Status = EOrganizationStatus.Active
        };
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name required");
        Name = name.Trim();
    }

    public void SetInn(string? inn) => Inn = inn?.Trim().ToUpperInvariant();
    public void SetOgrn(string? ogrn) => Ogrn = ogrn?.Trim();
    public void SetContact(string? email, string? phone)
    {
        Email = email?.Trim().ToLowerInvariant();
        Phone = phone?.Trim();
    }

    public void Block() => Status = EOrganizationStatus.Blocked;
    public void Activate() => Status = EOrganizationStatus.Active;
}
