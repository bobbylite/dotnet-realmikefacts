using System.Security.Claims;
using bobbylite.realmikefacts.web.Authorization;
using bobbylite.realmikefacts.web.Services.Cookie;
using bobbylite.realmikefacts.web.Services.Graph;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Graph.Models;
using Moq;

namespace bobbylite.realmikefacts.tests.Authorization;

public class AdministratorsGroupAuthorizationHandlerTests
{
    [Fact]
    public async Task HandleRequirementAsync_UserNotAuthenticated_CookieDeleted()
    {
        // Arrange
        var graphService = new Mock<IGraphService>().Object;
        var mockAuthorizationCookieService = new Mock<IAuthorizationCookieService>();
        var authorizationCookieService = mockAuthorizationCookieService.Object;
        var handler = new AdministratorsGroupAuthorizationHandler(graphService, authorizationCookieService);
        var mockContext = CreateAuthorizationHandlerContext(false);
        var context = mockContext.Object;

        // Act
        await handler.HandleAsync(context);

        // Assert
        Assert.False(context.HasSucceeded);
        mockContext.Verify(s => s.Succeed(It.IsAny<IAuthorizationRequirement>()), Times.Once);
        mockAuthorizationCookieService.Verify(s => s.DeleteCookie(), Times.Never);
    }

    [Fact]
    public async Task HandleRequirementAsync_UserAuthenticated_CookieNotDeleted()
    {
        // Arrange
        var graphService = new Mock<IGraphService>().Object;
        var mockAuthorizationCookieService = new Mock<IAuthorizationCookieService>();
        var authorizationCookieService = mockAuthorizationCookieService.Object;
        var handler = new AdministratorsGroupAuthorizationHandler(graphService, authorizationCookieService);
        var context = CreateAuthorizationHandlerContext(true);

        // Act
        await handler.HandleAsync(context.Object);

        // Assert
        Assert.False(context.Object.HasSucceeded);
        context.Verify(s => s.Succeed(It.IsAny<IAuthorizationRequirement>()), Times.Once);
        mockAuthorizationCookieService.Verify(s => s.DeleteCookie(), Times.Never);
    }

    [Fact]
    public async Task HandleRequirementAsync_UserAuthenticated_CookieExists_UserIsMember()
    {
        // Arrange
        var graphService = new Mock<IGraphService>().Object;
        var mockAuthorizationCookieService = new Mock<IAuthorizationCookieService>();
        var authorizationCookieService = mockAuthorizationCookieService.Object;
        var handler = new AdministratorsGroupAuthorizationHandler(graphService, authorizationCookieService);
        var context = CreateAuthorizationHandlerContext(true);
        mockAuthorizationCookieService
            .Setup(s => s.DoesCookieExist())
            .Returns(true);
        mockAuthorizationCookieService
            .Setup(s => s.DetermineGroupMembership(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new Task<bool>(() => false));

        // Act
        await handler.HandleAsync(context.Object);

        // Assert
        Assert.False(context.Object.HasSucceeded);
        context.Verify(s => s.Succeed(It.IsAny<IAuthorizationRequirement>()), Times.Once);
        mockAuthorizationCookieService.Verify(s => s.CreateCookie(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task HandleRequirementAsync_UserAuthenticated_CookieExists_UserIsNotMember()
    {
        // Arrange
        var graphService = new Mock<IGraphService>().Object;
        var mockAuthorizationCookieService = new Mock<IAuthorizationCookieService>();
        var authorizationCookieService = mockAuthorizationCookieService.Object;
        var handler = new AdministratorsGroupAuthorizationHandler(graphService, authorizationCookieService);
        var context = CreateAuthorizationHandlerContext(true);
        mockAuthorizationCookieService
            .Setup(s => s.DoesCookieExist()).Returns(true);
        mockAuthorizationCookieService
            .Setup(s => s.DetermineGroupMembership(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new Task<bool>(() => false));

        // Act
        await handler.HandleAsync(context.Object);

        // Assert
        Assert.False(context.Object.HasSucceeded);
        context.Verify(s => s.Succeed(It.IsAny<IAuthorizationRequirement>()), Times.Once);
        mockAuthorizationCookieService.Verify(s => s.CreateCookie(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task HandleRequirementAsync_UserAuthenticated_CookieDoesNotExist_UserIsMember()
    {
        // Arrange
        var mockGraphService = new Mock<IGraphService>();
        var graphService = mockGraphService.Object;
        var mockAuthorizationCookieService = new Mock<IAuthorizationCookieService>();
        var authorizationCookieService = mockAuthorizationCookieService.Object;
        var handler = new AdministratorsGroupAuthorizationHandler(graphService, authorizationCookieService);
        var context = CreateAuthorizationHandlerContext(true);
        mockAuthorizationCookieService.Setup(s => s.DoesCookieExist()).Returns(false);
        mockGraphService.Setup(s => s.GetAllGroupMemberships(It.IsAny<string>())).ReturnsAsync(new List<DirectoryObject>());

        // Act
        await handler.HandleAsync(context.Object);

        // Assert
        Assert.False(context.Object.HasSucceeded);
        context.Verify(s => s.Succeed(It.IsAny<IAuthorizationRequirement>()), Times.Once);
        mockAuthorizationCookieService.Verify(s => s.CreateCookie(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task HandleRequirementAsync_UserAuthenticated_CookieDoesNotExist_UserIsNotMember()
    {
        // Arrange
        var mockGraphService = new Mock<IGraphService>();
        var graphService = mockGraphService.Object;
        var mockAuthorizationCookieService = new Mock<IAuthorizationCookieService>();
        var authorizationCookieService = mockAuthorizationCookieService.Object;
        var handler = new AdministratorsGroupAuthorizationHandler(graphService, authorizationCookieService);
        var context = CreateAuthorizationHandlerContext(true);
        mockAuthorizationCookieService.Setup(s => s.DoesCookieExist()).Returns(false);
        mockGraphService.Setup(s => s.GetAllGroupMemberships(It.IsAny<string>())).ReturnsAsync(new List<DirectoryObject>());

        // Act
        await handler.HandleAsync(context.Object);

        // Assert
        Assert.False(context.Object.HasSucceeded);
        context.Verify(s => s.Succeed(It.IsAny<IAuthorizationRequirement>()), Times.Once);
        mockAuthorizationCookieService.Verify(s => s.CreateCookie(It.IsAny<string>()), Times.Never);
    }

    private Mock<AuthorizationHandlerContext> CreateAuthorizationHandlerContext(bool isAuthenticated)
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "UserId"),
        }, "TestAuthentication"));

        var context = new Mock<AuthorizationHandlerContext>(
            new[] { new AdministratorsGroupRequirement() }, user, null!);

        context.Object.Succeed(new AdministratorsGroupRequirement());

        if (!isAuthenticated)
        {
            context.Setup(c => c.User.Identity!.IsAuthenticated).Returns(false);
        }

        return context;
    }
}