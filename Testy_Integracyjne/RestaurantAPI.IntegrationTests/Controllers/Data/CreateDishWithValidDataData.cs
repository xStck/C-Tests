using System.Collections;
using RestaurantAPI.Models;

namespace RestaurantAPI.IntegrationTests.Controllers.Data;

public class CreateDishWithValidDataTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        var data = new List<object[]>
        {
            new object[]
            {
                1,
                new CreateDishDto()
                {
                    Name = "Dish1"
                }
            },
            new object[]
            {
                2,
                new CreateDishDto()
                {
                    Name = "Dish2"
                }
            },
            new object[]
            {
                2,
                new CreateDishDto()
                {
                    Name = "Dish3"
                }
            },
            new object[]
            {
                3,
                new CreateDishDto()
                {
                    Name = "Dish4"
                }
            }
        };
        return data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}