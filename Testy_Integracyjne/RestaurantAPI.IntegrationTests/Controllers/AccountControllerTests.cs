using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using RestaurantAPI.Entities;
using RestaurantAPI.IntegrationTests.Helpers;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.IntegrationTests;

public class AccountControllerTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;
    private readonly Mock<IAccountService> _accountServiceMock = new();

    public AccountControllerTests(WebApplicationFactory<Startup> factory)
    {
        _client = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    //Nadpisywanie dbContextu
                    var dbContextOptions = services.SingleOrDefault(service =>
                        service.ServiceType == typeof(DbContextOptions<RestaurantDbContext>));
                    if (dbContextOptions != null) services.Remove(dbContextOptions);
                    services.AddDbContext<RestaurantDbContext>(options => options.UseInMemoryDatabase("RestaurantDb"));
                    services.AddSingleton<IAccountService>(_accountServiceMock.Object);
                });
            }).CreateClient();
    }

    [Fact]
    public async Task RegisterUser_ForValidModel_ReturnsOk()
    {
        //arrange
        var registerUser = new RegisterUserDto
        {
            Email = "test@test.com",
            Password = "Asdfasdf1!",
            ConfirmPassword = "Asdfasdf1!"
        };

        var httpContent = registerUser.ToJsonHttpContent();
        //act
        var response = await _client.PostAsync("/api/account/register", httpContent);
        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task RegisterUser_ForInvalidModel_ReturnsBadRequest()
    {
        //arrange
        var registerUser = new RegisterUserDto
        {
            Password = "Asdfasdf21!",
            ConfirmPassword = "Asdfasdf1!"
        };

        var httpContent = registerUser.ToJsonHttpContent();
        //act
        var response = await _client.PostAsync("/api/account/register", httpContent);
        //assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_ForRegisteredUser_ReturnsOk()
    {
        //arrange
        _accountServiceMock
            .Setup(e => e.GenerateJwt(It.IsAny<LoginDto>()))
            .Returns("jwt");
        
        var loginDto = new LoginDto()
        {
            Email = "test@test.com",
            Password = "password123"
        };

        var httpContent = loginDto.ToJsonHttpContent();
        
        // act
        var response = await _client.PostAsync("/api/account/login", httpContent);
        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

    }
}