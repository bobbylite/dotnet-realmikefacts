using bobbylite.realmikefacts.web.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Identity.Web.Resource;

namespace bobbylite.realmikefacts.tests.Extensions;

public class WebApplicationBuilderExtensionsTests
{
    [Fact]
    public void AddAuthenticationWithNullWebApplicationBuilderThrowsException()
    {
        // arrange
        WebApplicationBuilder? webApplicationBuilder = null;

        // act & assert
        var ex = Assert.Throws<ArgumentNullException>(() =>  
            webApplicationBuilder!.AddAuthentication());
        Assert.Contains("webApplicationBuilder", ex.Message);
    }
    
    [Fact]
    public void AddAuthenticationSucceeds()
    {
        // arrange
        WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder();

        // act
        webApplicationBuilder.AddAuthentication();
        
        // assert
        Assert.Contains(webApplicationBuilder.Services, s =>
            s.ServiceType == typeof(IAuthenticationService));
        Assert.Contains(webApplicationBuilder.Services, s =>
            s.ServiceType == typeof(MicrosoftIdentityIssuerValidatorFactory));
        Assert.Contains(webApplicationBuilder.Services, s =>
            s.ServiceType == typeof(ApplicationPartManager));
    }
    
    [Fact]
    public void AddAuthorizationWithNullWebApplicationBuilderThrowsException()
    {
        // arrange
        WebApplicationBuilder? webApplicationBuilder = null;

        // act & assert
        var ex = Assert.Throws<ArgumentNullException>(() =>  
            webApplicationBuilder!.AddAuthorization());
        Assert.Contains("webApplicationBuilder", ex.Message);
    }
    
    [Fact]
    public void AddAuthorizationSucceeds()
    {
        // arrange
        WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder();

        // act
        webApplicationBuilder.AddAuthorization();
        
        // assert
        Assert.Contains(webApplicationBuilder.Services, s =>
            s.ServiceType == typeof(IAuthorizationService));
        Assert.Contains(webApplicationBuilder.Services, s =>
            s.ServiceType == typeof(IAuthorizationPolicyProvider));
    }
}