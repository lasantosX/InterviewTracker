using InterviewTracker.Api.Auth;
using InterviewTracker.Api.Controllers;
using InterviewTracker.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace InterviewTracker.Tests;

public class AuthControllerTests
{
    private JwtTokenService CreateJwtTokenService()
    {
        var settings = new Dictionary<string, string?>
        {
            { "Jwt:Key", "ThisIsASecretKeyForInterviewTrackerDemo12345" },
            { "Jwt:Issuer", "InterviewTracker" },
            { "Jwt:Audience", "InterviewTrackerUsers" }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();

        return new JwtTokenService(configuration);
    }

    [Fact]
    public void Login_WhenCredentialsAreInvalid_ReturnsUnauthorized()
    {
        var tokenService = CreateJwtTokenService();
        var controller = new AuthController(tokenService);

        var result = controller.Login(new LoginRequest
        {
            Username = "wrong",
            Password = "wrong"
        });

        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public void Login_WhenCredentialsAreValid_ReturnsOkWithToken()
    {
        var tokenService = CreateJwtTokenService();
        var controller = new AuthController(tokenService);

        var result = controller.Login(new LoginRequest
        {
            Username = "admin",
            Password = "password"
        });

        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.NotNull(okResult.Value);
    }
}