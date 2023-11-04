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
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly Mock<IGraphService> _mockGraphService;
    private readonly IAuthorizationCookieService _authorizationCookieService;
    
    public AuthorizationCookieServiceTests()
    {
        _mockGraphService = new Mock<IGraphService>();
        _mockHttpContextAccessor = CreateHttpContextAccessorWithoutRequestCookies();
        var cookieService = new AuthorizationCookieService(_mockGraphService.Object, _mockHttpContextAccessor.Object);

        _authorizationCookieService = cookieService;
    }
    
    [Fact]
    public void DoesCookieExist_Success()
    {
        // Arrange
        var httpContextAccessor = CreateHttpContextAccessorWithRequestCookies();
        var authorizationCookieService= new AuthorizationCookieService(_mockGraphService.Object, httpContextAccessor.Object);
        
        // Act
        var doesCookieExist = authorizationCookieService.DoesCookieExist();
        
        // Assert
        Assert.True(doesCookieExist);
        httpContextAccessor.Verify(s => s.HttpContext, Times.Once);
    }
    
    [Fact]
    public void DeleteCookie_Success()
    {
        // Arrange
        var httpContextAccessor = CreateHttpContextAccessorWithRequestCookies();
        var authorizationCookieService= new AuthorizationCookieService(_mockGraphService.Object, httpContextAccessor.Object);
        
        // Act
        authorizationCookieService.DeleteCookie();
        
        // Assert
        httpContextAccessor.Verify(s => s.HttpContext, Times.Once);
    }
    
    [Fact]
    public void CreateCookie_WithUserId_Success()
    {
        // Arrange
        var userId = "foobar";

        // Act
        _authorizationCookieService.CreateCookie(userId);

        // Assert
        _mockHttpContextAccessor.Verify(s => s.HttpContext!.Response.Cookies.Delete(It.IsAny<string>()), Times.Never);
    }
    
    [Fact]
    public void CreateCookie_WithUserIdAndGroupId_Success()
    {
        // Arrange
        var userId = "foobar";
        var groupId = "deadbeef";

        // Act
        _authorizationCookieService.CreateCookie(userId, groupId);

        // Assert
        _mockHttpContextAccessor.Verify(s => s.HttpContext!.Response.Cookies.Delete(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void GetCookie_Success()
    {
        // Arrange
        var httpContextAccessor = CreateHttpContextAccessorWithRequestCookies();
        var authorizationCookieService= new AuthorizationCookieService(_mockGraphService.Object, httpContextAccessor.Object);
        
        // Act
        var cookie = authorizationCookieService.GetCookie();

        // Assert
        Assert.NotNull(cookie);
        Assert.NotEmpty(cookie);
    }
    
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
        httpContextAccessor.Verify(s => s.HttpContext, Times.Once);
    }

    [Fact]
    public async Task DetermineGroupMembership_CookieExists_UserIsMember()
    {
        var groupId = "deadbeef";
        var userId = "foobar";
        string cookieString = $"{{\"groups\":[{{\"group_id\":\"{groupId}\"}}],\"user_id\":\"{userId}\"}}";
        byte[] bytes = Encoding.ASCII.GetBytes(cookieString);
        var cookie = Convert.ToBase64String(bytes);
        var httpContextAccessor = CreateHttpContextAccessorWithRequestCookies();
        var authorizationCookieService= new AuthorizationCookieService(_mockGraphService.Object, httpContextAccessor.Object);
        
        // Act
        var result = await authorizationCookieService.DetermineGroupMembership(cookie, groupId, userId);
        
        // Assert
        Assert.True(result);
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
    
    private Mock<IHttpContextAccessor> CreateHttpContextAccessorWithoutRequestCookies()
    {
        return new Mock<IHttpContextAccessor>();
    }
    
    private Mock<IHttpContextAccessor> CreateHttpContextAccessorWithRequestCookies()
    {
        string cookieString = $"{{\"groups\":[{{\"group_id\":\"deadbeef\"}}],\"user_id\":\"foobar\"}}";
        byte[] bytes = Encoding.ASCII.GetBytes(cookieString);
        var cookie = Convert.ToBase64String(bytes);
        
        var requestCookieCollection = new Mock<IRequestCookieCollection>();
        requestCookieCollection
            .Setup(m => m[It.IsAny<string>()])
            .Returns(cookie);
        
        var httpContext = new DefaultHttpContext()
        {
            Request = { Cookies = requestCookieCollection.Object }
        };
        
        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        httpContextAccessor
            .Setup(a => a.HttpContext)
            .Returns(httpContext);
            
        return httpContextAccessor;
    }
}
