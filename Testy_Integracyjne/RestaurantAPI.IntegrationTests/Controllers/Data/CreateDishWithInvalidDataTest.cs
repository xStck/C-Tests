using System.Collections;
using RestaurantAPI.Models;

namespace RestaurantAPI.IntegrationTests.Controllers.Data;

public class CreateDishWithInvalidDataTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        var list = new List<object[]>
        {
            new object[]
            {
                55,
                new CreateDishDto()
                {
                    Name = "Dish1"
                }
            },
            new object[]
            {
                56,
                new CreateDishDto()
                {
                    Name = "Dish2"
                }
            },
        };
        return list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}