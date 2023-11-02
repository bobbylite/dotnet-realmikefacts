using System.Net;
using System.Text;
using System.Text.Json;
using bobbylite.realmikefacts.web.Models.Authorization;
using bobbylite.realmikefacts.web.Services.Cookie;
using bobbylite.realmikefacts.web.Services.Graph;
using Microsoft.AspNetCore.Http;
using Moq;

namespace bobbylite.realmikefacts.tests.Services;

public class AuthorizationCookieServiceTests
{
    [Fact]
    public async Task DetermineGroupMembership_CookieDoesNotExist_UserIsNotMember()
    {
        // Arrange
        var graphService = new Mock<IGraphService>();
        var httpContextAccessor = CreateHttpContextAccessorWithRequestCookies();
        var cookieService = new AuthorizationCookieService(graphService.Object, httpContextAccessor.Object);
        var groupId = "not-deadbeef";
        var userId = "not-foobar";
        byte[] bytes = Encoding.ASCII.GetBytes("{\"groups\":[{\"group_id\":\"deadbeef\"}],\"user_id\":\"foobar\"}");
        var cookie = Convert.ToBase64String(bytes);

        // Act
        var result = await cookieService.DetermineGroupMembership(cookie, groupId, userId);

        // Assert
        Assert.False(result);
        graphService.Verify(s => s.DoesUserBelongToGroup(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        httpContextAccessor.Verify(s => s.HttpContext!.Response.Cookies.Delete(It.IsAny<string>()), Times.Never);
        httpContextAccessor.Verify(s => s.HttpContext!.Response.Cookies.Append(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CookieOptions>()), Times.Never);
    }

    [Fact]
    public async Task DetermineGroupMembership_CookieExists_UserIsMember()
    {
        // Arrange
        var graphService = new Mock<IGraphService>();
        graphService.Setup(s => s.DoesUserBelongToGroup(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
        var httpContextAccessor = CreateHttpContextAccessorWithRequestCookies();
        var cookieService = new AuthorizationCookieService(graphService.Object, httpContextAccessor.Object);
        var groupId = "deadbeef";
        var userId = "foobar";
        string cookieString = $"{{\"groups\":[{{\"group_id\":\"{groupId}\"}}],\"user_id\":\"{userId}\"}}";
        byte[] bytes = Encoding.ASCII.GetBytes(cookieString);
        var cookie = Convert.ToBase64String(bytes);
        httpContextAccessor
            .Setup(m => m.HttpContext!.Request.Cookies[It.IsAny<string>()])
            .Returns(".AspNetCore.Custom.Auth.Cookies");
        
        // Act
        var result = await cookieService.DetermineGroupMembership(cookie, groupId, userId);

        // Assert
        Assert.True(result);
        graphService.Verify(s => s.DoesUserBelongToGroup(userId, groupId), Times.Never);
        httpContextAccessor.Verify(s => s.HttpContext!.Response.Cookies.Delete(It.IsAny<string>()), Times.Never);
        httpContextAccessor.Verify(s => s.HttpContext!.Response.Cookies.Append(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CookieOptions>()), Times.Never);
    }

    [Fact]
    public async Task DetermineGroupMembership_CookieExists_UserIsNotMember()
    {
        // Arrange
        var graphService = new Mock<IGraphService>();
        graphService.Setup(s => s.DoesUserBelongToGroup(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);
        var httpContextAccessor = CreateHttpContextAccessorWithRequestCookies();
        var cookieService = new AuthorizationCookieService(graphService.Object, httpContextAccessor.Object);
        var groupId = "deadbeef";
        var userId = "foobar";
        string cookieString = $"{{\"groups\":[{{\"group_id\":\"not-{groupId}\"}}],\"user_id\":\"not-{userId}\"}}";
        byte[] bytes = Encoding.ASCII.GetBytes(cookieString);
        var cookie = Convert.ToBase64String(bytes);

        // Act
        var result = await cookieService.DetermineGroupMembership(cookie, groupId, userId);

        // Assert
        Assert.False(result);
        graphService.Verify(s => s.DoesUserBelongToGroup(userId, groupId), Times.Once);
        httpContextAccessor.Verify(s => s.HttpContext!.Response.Cookies.Delete(It.IsAny<string>()), Times.Never);
        httpContextAccessor.Verify(s => s.HttpContext!.Response.Cookies.Append(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CookieOptions>()), Times.Never);
    }

    [Fact]
    public void GetGroupsFromCookie_ValidCookie_ReturnsGroupAuthorizationModel()
    {
        // Arrange
        var graphService = new Mock<IGraphService>();
        graphService.Setup(s => s.DoesUserBelongToGroup(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);
        var httpContextAccessor = CreateHttpContextAccessorWithRequestCookies();
        var cookieService = new AuthorizationCookieService(graphService.Object, httpContextAccessor.Object);
        var groupAuthorizationModel = new GroupAuthorizationModel
        {
            Groups = new List<GroupInformation>
            {
                new GroupInformation
                {
                    GroupId = "SomeGroupId"
                }
            },
            UserId = "UserId"
        };
        var encodedCookie = Convert.ToBase64String(Encoding.ASCII.GetBytes(JsonSerializer.Serialize(groupAuthorizationModel)));

        // Act
        var result = cookieService.GetGroupsFromCookie(encodedCookie);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(groupAuthorizationModel.Groups.Count, result.Groups!.Count);
        Assert.Equal(groupAuthorizationModel.UserId, result.UserId);
    }

    [Fact]
    public void GetGroupsFromCookie_InvalidCookie_ThrowsException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AuthorizationCookieService(null!, null!));
    }

    // Add more tests for other methods, including CreateCookie, DeleteCookie, GetCookie, DoesCookieExist, etc.

    // Helper method to create an HttpContextAccessor with RequestCookies
    private Mock<IHttpContextAccessor> CreateHttpContextAccessorWithRequestCookies()
    {
        var httpContext = new DefaultHttpContext();
        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);
        return httpContextAccessor;
    }
}
