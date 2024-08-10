using FluentValidation.TestHelper;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Models.Validators;

namespace RestaurantAPI.IntegrationTests.ValidatorTests;

public class RestaurantQueryValidatorTests
{
    public static IEnumerable<object[]> GetSampleValidData()
    {
        var list = new List<RestaurantQuery>
        {
            new()
            {
                PageNumber = 1,
                PageSize = 5
            },
            new()
            {
                PageNumber = 2,
                PageSize = 15
            },
            new()
            {
                PageNumber = 22,
                PageSize = 5
            },
            new()
            {
                PageNumber = 22,
                PageSize = 15,
                SortBy = nameof(Restaurant.Name)
            },
            new()
            {
                PageNumber = 22,
                PageSize = 15,
                SortBy = nameof(Restaurant.Category)
            }
        };
        return list.Select(q => new object[] { q });
    }    
    
    public static IEnumerable<object[]> GetSampleInvalidData()
    {
        var list = new List<RestaurantQuery>
        {
            new()
            {
                PageNumber = 0,
                PageSize = 5
            },
            new()
            {
                PageNumber = 2,
                PageSize = 152
            },
            new()
            {
                PageNumber = 22,
                PageSize = 53
            },
            new()
            {
                PageNumber = 22,
                PageSize = 15,
                SortBy = nameof(Restaurant.ContactEmail)
            },
            new()
            {
                PageNumber = 22,
                PageSize = 15,
                SortBy = nameof(Restaurant.ContactNumber)
            }
        };
        return list.Select(q => new object[] { q });
    }

    [Theory]
    [MemberData(nameof(GetSampleValidData))]
    public void Validate_ForCorrectModel_ReturnsSuccess(RestaurantQuery model)
    {
        //arrange
        var validator = new RestaurantQueryValidator();
        //act
        var result = validator.TestValidate(model);
        //assert
        result.ShouldNotHaveAnyValidationErrors();
    }    
    
    [Theory]
    [MemberData(nameof(GetSampleInvalidData))]
    public void Validate_ForIncorrectCorrectModel_ReturnsFaile(RestaurantQuery model)
    {
        //arrange
        var validator = new RestaurantQueryValidator();
        //act
        var result = validator.TestValidate(model);
        //assert
        result.ShouldHaveAnyValidationError();
    }
}