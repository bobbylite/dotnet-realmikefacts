using bobbylite.realmikefacts.web.Extensions;
using Microsoft.AspNetCore.Builder;

namespace bobbylite.realmikefacts.tests.Extensions;

public class WebApplicationExtensionsTests
{
    [Fact]
    public void AddMiddlewareWithNullWebApplicationThrowsException()
    {
        // arrange
        WebApplication? webApplication = null;

        // act & assert
        var ex = Assert.Throws<ArgumentNullException>(() =>  
            webApplication!.AddMiddleware());
        Assert.Contains("webApplication", ex.Message);
    }
    
    [Fact]
    public void AddEndpointsWithNullWebApplicationThrowsException()
    {
        // arrange
        WebApplication? webApplication = null;

        // act & assert
        var ex = Assert.Throws<ArgumentNullException>(() =>  
            webApplication!.AddEndpoints());
        Assert.Contains("webApplication", ex.Message);
    }
}