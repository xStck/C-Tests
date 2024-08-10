using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.IntegrationTests.Controllers.Data;
using RestaurantAPI.IntegrationTests.Helpers;
using RestaurantAPI.Models;

namespace RestaurantAPI.IntegrationTests.Controllers;

public class DishControllerTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;
    private WebApplicationFactory<Startup> _factory;

    public DishControllerTests()
    {
        _client = CreateHttpClient("RestaurantDb_1");
        SeedData();
    }

    private void SeedData()
    {
        var restaurants = new List<Restaurant>
        {
            new()
            {
                Id = 1,
                Name = "Restaurant1"
            },
            new()
            {
                Id = 2,
                Name = "Restaurant2"
            },
            new()
            {
                Id = 3,
                Name = "Restaurant3"
            }
        };

        var dishes = new List<Dish>
        {
            new()
            {
                Name = "D1",
                RestaurantId = 1
            },
            new()
            {
                Name = "D2",
                RestaurantId = 1
            },
            new()
            {
                Name = "D3",
                RestaurantId = 2
            },
            new()
            {
                Name = "D4",
                RestaurantId = 2
            },
            new()
            {
                Name = "D5",
                RestaurantId = 3
            },
            new()
            {
                Name = "D6",
                RestaurantId = 3
            }
        };
        
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var _dbContext = scope.ServiceProvider.GetService<RestaurantDbContext>();
        
        restaurants.ForEach(restaurant =>
        {
            if (_dbContext.Restaurants.SingleOrDefault(x => x.Id == restaurant.Id) == null)
                _dbContext.Add(restaurant);
        });
        
        dishes.ForEach(dish =>
        {
            if (_dbContext.Dishes.SingleOrDefault(x => x.Id == dish.Id) == null)
                _dbContext.Add(dish);
        });
        _dbContext.SaveChanges();
    }

    private HttpClient CreateHttpClient(string dbName)
    {
        var factory = new WebApplicationFactory<Startup>();
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbContext =
                    services
                        .SingleOrDefault(dbc =>
                            dbc.ServiceType == typeof(DbContextOptions<RestaurantDbContext>)
                        );
                if (dbContext != null) services.Remove(dbContext);
                services.AddDbContext<RestaurantDbContext>(opt => opt.UseInMemoryDatabase(dbName));
            });
        });
        return _factory.CreateClient();
    }
    
    [Theory]
    [InlineData(1, 22)]
    [InlineData(1, 333)]
    [InlineData(1, 34)]
    [InlineData(222, 5)]
    [InlineData(333, 7)]
    public async Task GetSingleDish_ForValidData_ReturnsNotFound(int restaurantId, int dishId)
    {
        //act
        var result = await _client.GetAsync($"api/restaurant/{restaurantId}/dish/{dishId}");
        //assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 2)]
    [InlineData(1, 3)]
    [InlineData(2, 5)]
    [InlineData(3, 7)]
    public async Task GetSingleDish_ForValidData_ReturnsOk(int restaurantId, int dishId)
    {
        //act
        var result = await _client.GetAsync($"api/restaurant/{restaurantId}/dish/{dishId}");
        //assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task GetAllDishes_ForValidData_ReturnsOk(int restaurantId)
    {
        //act
        var result = await _client.GetAsync($"api/restaurant/{restaurantId}/dish");
        //assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Theory]
    [InlineData(11)]
    [InlineData(22)]
    [InlineData(33)]
    public async Task GetAllDishes_ForInvlidData_ReturnsNotFound(int restaurantId)
    {
        //act
        var result = await _client.GetAsync($"api/restaurant/{restaurantId}/dish");
        //assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task Delete_ForValidRestaurantId_ReturnsNoContent(int restaurantId)
    {
        //act
        var response = await _client.DeleteAsync($"api/restaurant/{restaurantId}/dish");
        //assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(30)]
    public async Task Delete_ForInvalidRestaurantId_ThrowsNotFoundException(int restaurantId)
    {
        //act
        var result = await _client.DeleteAsync($"api/restaurant/{restaurantId}/dish");
        //assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [ClassData(typeof(CreateDishWithValidDataTestData))]
    public async Task Post_ForValidData_ReturnsCreatedStatus(int restaurantId, CreateDishDto dishDto)
    {
        //act
        var content = dishDto.ToJsonHttpContent();
        var result = await _client.PostAsync($"api/restaurant/{restaurantId}/dish", content);
        //assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Theory]
    [ClassData(typeof(CreateDishWithInvalidDataTest))]
    public async Task Post_ForInvalidData_ReturnsNotFound(int restaurantId, CreateDishDto dishDto)
    {
        //act
        var content = dishDto.ToJsonHttpContent();
        var result = await _client.PostAsync($"api/restaurant/{restaurantId}/dish", content);
        //assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
}