using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.IntegrationTests.Helpers;
using RestaurantAPI.Models;

namespace RestaurantAPI.IntegrationTests.Controllers;

//IClassFixture pozwala na współdzielenie contextu w tym przypadku factory
public class RestaurantControllerTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Startup> _factory;

    public RestaurantControllerTests(WebApplicationFactory<Startup> factory)
    {
        // By podmienić na EntityFramework.InMemory potrzebny jest nuget Microsoft.EntityFrameworkCore.InMemory
        // oraz  Microsoft.Extensions.DependencyInjection który umożliwia nadpisywanie rejestracji dowolnych serwisów
        // w testach
        //InMemory nie pozwala na raw queries
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    //Nadpisywanie dbContextu
                    var dbContextOptions = services.SingleOrDefault(service =>
                        service.ServiceType == typeof(DbContextOptions<RestaurantDbContext>));
                    if (dbContextOptions != null) services.Remove(dbContextOptions);
                    services.AddDbContext<RestaurantDbContext>(options => options.UseInMemoryDatabase("RestaurantDb"));

                    //Uwierzytelnianie, autoryzacja i dodawanie claimów 
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                });
            });
        _client = _factory.CreateClient();
    }

    [Theory]
    [InlineData("pageSize=5&pageNumber=1")]
    [InlineData("pageSize=15&pageNumber=2")]
    [InlineData("pageSize=10&pageNumber=3")]
    public async Task GetAll_WithQueryParameters_ReturnsOkResult(string queryParams)
    {
        //act
        var response = await _client.GetAsync("/api/restaurant?" + queryParams);
        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("pageSize=100&pageNumber=3")]
    [InlineData("pageSize=11&pageNumber=3")]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetAll_WithInvalidQueryParameters_ReturnsBadRequest(string queryParams)
    {
        //act
        var response = await _client.GetAsync("/api/restaurant?" + queryParams);
        //assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // Test wymagający autoryzacji i uwierzytelniania
    [Fact]
    public async Task CreateRestaurant_WithValidModel_ReturnsCreatedStatus()
    {
        //arrange
        var model = new CreateRestaurantDto
        {
            Name = "TestRestaurant",
            City = "Lublin",
            Street = "Długa 5"
        };
        var httpContent = model.ToJsonHttpContent();
        //act
        var response = await _client.PostAsync("/api/restaurant", httpContent);
        //assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateRestaurant_WithInvalidModel_RestaurantBadRequest()
    {
        //arrange
        var model = new CreateRestaurantDto
        {
            ContactEmail = "a@a.pl",
            Description = "aaa",
            ContactNumber = "999999999"
        };
        var httpContent = model.ToJsonHttpContent();
        //act
        var response = await _client.PostAsync("/api/restaurant", httpContent);
        //assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Delete_ForNonExistingRestaurant_ReturnsNotFound()
    {
        //act
        var response = await _client.DeleteAsync("/api/restaurant/987");
        //assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private void SeedRestaurant(Restaurant restaurant)
    {
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var _dbContext = scope.ServiceProvider.GetService<RestaurantDbContext>();
        _dbContext.Restaurants.Add(restaurant);
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task Delete_ForRestaurantOwner_ReturnsNoContent()
    {
        //arrange
        var restaurant = new Restaurant
        {
            CreatedById = 1,
            Name = "Test"
        };
        //seed
        SeedRestaurant(restaurant);
        //act
        var response = await _client.DeleteAsync("/api/restaurant/" + restaurant.Id);
        //assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_ForNonRestaurantOwner_ReturnsForbidden()
    {
        //arrange
        var restaurant = new Restaurant
        {
            CreatedById = 99999,
            Name = "Test"
        };
        //seed
        SeedRestaurant(restaurant);
        //act
        var response = await _client.DeleteAsync("/api/restaurant/" + restaurant.Id);
        //assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}