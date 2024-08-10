using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace RestaurantAPI.IntegrationTests;

//testy rejestrowania zależności przez kontrolery
public class StartupTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly WebApplicationFactory<Startup> _factory;
    private readonly List<Type> _controllerTypes;

    public StartupTests(WebApplicationFactory<Startup> factory)
    {
        _controllerTypes = typeof(Startup).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(ControllerBase)))
            .ToList();
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                _controllerTypes.ForEach(c => services.AddScoped(c));
            });
        });
        
    }
    [Fact]
    public void ConfigureServices_ForControllers_RegistersAllDependencies()
    {
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        _controllerTypes.ForEach(x =>
        {
            var controller = scope.ServiceProvider.GetService(x);
            controller.Should().NotBeNull();
        });
    }
}