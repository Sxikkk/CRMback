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
    public DateTime CreateDate { get; private set; } = DateTime.UtcNow;
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
    
    public void SetWebsite(string? website) => Website = website?.Trim();

    public void Block() => Status = EOrganizationStatus.Blocked;
    public void Activate() => Status = EOrganizationStatus.Active;

    public static Organization CreateTaskflow()
    {
        return new Organization
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "Моя Организация",
            Inn = "1234567890",
            Ogrn = "1234567890123",
            Email = "info@myorg.com",
            Phone = "+7 123 456 7890",
            Website = "https://myorg.com",
            City = "Москва",
            Type = EOrganizationType.Company,
            Status = EOrganizationStatus.Active,
            CreateDate = DateTime.SpecifyKind(new DateTime(2026, 3, 26), DateTimeKind.Utc)
        };
    }
    
    public static string CreateDescription(string? inn = null, string? ogrn = null, EOrganizationType? orgType = null, string? orgPhone = null) =>
        $"{inn ?? "неизвестно"} - {orgType ?? EOrganizationType.Other} - {ogrn ?? "неизвестно"} - {orgPhone ?? "неизвестно"}";
}