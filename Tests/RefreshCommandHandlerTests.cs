using Application.Common.Interfaces;
using Application.Features.Auth.Commands.Refresh;
using Contracts.Auth;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Security;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Tests;

public class RefreshCommandHandlerTests
{
    private const string Ip = "127.0.0.1";

    [Fact]
    public async Task Should_rotate_active_refresh_and_return_new_tokens()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var incomingRaw = "incoming-refresh";
        var incomingHash = "incoming-hash";
        var newRaw = "new-refresh";
        var newHash = "new-hash";

        var user = User.Create("John", "Doe", Email.Create("john@doe.com"), Phone.Create("+79998887766"),
            "jdoe", "hash");
        typeof(User).GetProperty("Id")!.SetValue(user, userId);

        var activeRefresh = RefreshToken.Create(userId, incomingHash, TimeSpan.FromDays(1), Ip);

        var jwt = new Mock<IJwtTokenGenerator>();
        jwt.Setup(j => j.HashToken(incomingRaw)).Returns(incomingHash);
        jwt.Setup(j => j.HashToken(newRaw)).Returns(newHash);
        jwt.Setup(j => j.GenerateTokens(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns(("access-token", newRaw, TimeSpan.FromDays(7)));

        var refreshRepo = new Mock<IRefreshTokenRepository>();
        refreshRepo.Setup(r => r.GetTokenByHashAsync(incomingHash, It.IsAny<CancellationToken>()))
            .ReturnsAsync(activeRefresh);

        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(r => r.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        
        var uowRepo = new Mock<IUnitOfWork>();

        var ctx = new Mock<IRequestContext>();
        ctx.SetupGet(c => c.IpAddress).Returns(Ip);

        var handler = new RefreshCommandHandler(jwt.Object, refreshRepo.Object, ctx.Object, userRepo.Object, uowRepo.Object);

        // Act
        var result = await handler.Handle(new RefreshCommand { Token = incomingRaw }, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(new TokenDto
        {
            AccessToken = "access-token",
            RefreshToken = newRaw
        });

        refreshRepo.Verify(r => r.AddTokenAsync(
            It.Is<RefreshToken>(t => t.TokenHash == newHash && t.UserId == userId),
            It.IsAny<CancellationToken>()), Times.Once);
        refreshRepo.Verify(r => uowRepo.Object.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Expired_refresh_revokes_and_throws_unauthorized()
    {
        var userId = Guid.NewGuid();
        var incomingRaw = "incoming-refresh";
        var incomingHash = "incoming-hash";

        var expired = RefreshToken.Create(userId, incomingHash, TimeSpan.FromHours(-1), Ip);

        var jwt = new Mock<IJwtTokenGenerator>();
        jwt.Setup(j => j.HashToken(incomingRaw)).Returns(incomingHash);

        var refreshRepo = new Mock<IRefreshTokenRepository>();
        refreshRepo.Setup(r => r.GetTokenByHashAsync(incomingHash, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expired);

        var userRepo = new Mock<IUserRepository>();
        var ctx = new Mock<IRequestContext>();
        ctx.SetupGet(c => c.IpAddress).Returns(Ip);
        var uowRepo = new Mock<IUnitOfWork>();

        var handler = new RefreshCommandHandler(jwt.Object, refreshRepo.Object, ctx.Object, userRepo.Object, uowRepo.Object);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            handler.Handle(new RefreshCommand { Token = incomingRaw }, CancellationToken.None));

        refreshRepo.Verify(r => uowRepo.Object.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
