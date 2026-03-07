using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Commands.Validators;
using Domain.Entities;
using Domain.Exceptions;
using Domain.ValueObjects;
using Xunit;

namespace Tests;

public class RegisterValidatorTests
{
    private readonly RegisterValidator _validator = new();

    [Fact]
    public void Should_fail_when_email_is_invalid()
    {
        var cmd = new RegisterCommand
        {
            Name = "John",
            Surname = "Doe",
            Username = "jdoe",
            Email = "bad-email",
            Phone = "+79998887766",
            Password = "Passw0rd"
        };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public void Should_fail_when_password_too_short()
    {
        var cmd = new RegisterCommand
        {
            Name = "John",
            Surname = "Doe",
            Username = "jdoe",
            Email = "john@doe.com",
            Phone = "+79998887766",
            Password = "123"
        };

        var result = _validator.Validate(cmd);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password");
    }

    [Fact]
    public void Should_pass_for_valid_command()
    {
        var cmd = new RegisterCommand
        {
            Name = "John",
            Surname = "Doe",
            Username = "jdoe",
            Email = "john@doe.com",
            Phone = "+79998887766",
            Password = "Passw0rd"
        };

        var result = _validator.Validate(cmd);

        Assert.True(result.IsValid);
    }
}

public class UserTests
{
    [Fact]
    public void Create_should_throw_on_empty_name()
    {
        var email = Email.Create("john@doe.com");
        var phone = Phone.Create("+79998887766");

        Assert.Throws<DomainException>(() =>
            User.Create("", "Doe", email, phone, "jdoe", "hash"));
    }

    [Fact]
    public void Create_should_set_properties_on_valid_input()
    {
        var email = Email.Create("john@doe.com");
        var phone = Phone.Create("+79998887766");

        var user = User.Create("John", "Doe", email, phone, "jdoe", "hash");

        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal("John", user.Name);
        Assert.Equal("Doe", user.Surname);
        Assert.Equal(email, user.Email);
        Assert.Equal(phone, user.Phone);
        Assert.Equal("jdoe", user.UserName);
        Assert.Equal("hash", user.PasswordHash);
    }
}
