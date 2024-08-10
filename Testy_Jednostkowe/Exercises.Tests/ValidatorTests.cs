using Exercises._3;
using FluentAssertions;

namespace Exercises.Tests;

public class ValidatorTests
{
    public static IEnumerable<object[]> GetRangeList()
    {
        yield return new object[]
        {
            new List<DateRange>
            {
                new(new DateTime(2020, 1, 1), new DateTime(2020, 1, 15)),
                new(new DateTime(2020, 2, 1), new DateTime(2020, 2, 15))
            }
        };

        yield return new object[]
        {
            new List<DateRange>
            {
                new(new DateTime(2020, 1, 15), new DateTime(2020, 1, 25))
            }
        };

        yield return new object[]
        {
            new List<DateRange>
            {
                new(new DateTime(2020, 1, 8), new DateTime(2020, 1, 25))
            }
        };

        yield return new object[]
        {
            new List<DateRange>
            {
                new(new DateTime(2020, 1, 12), new DateTime(2020, 1, 14))
            }
        };

        yield return new object[]
        {
            new List<DateRange>
            {
                new(new DateTime(2020, 1, 20), new DateTime(2020, 1, 23))
            }
        };

        yield return new object[]
        {
            new List<DateRange>
            {
                new(new DateTime(2020, 1, 2), new DateTime(2020, 1, 10))
            }
        };
    }

    [Theory]
    [MemberData(nameof(GetRangeList))]
    public void ValidateOverlapping_ForOverlappingDateRanges_ReturnsFalse(List<DateRange> ranges)
    {
        // arrange
        var input = new DateRange(new DateTime(2020, 1, 10), new DateTime(2020, 1, 20));
        var validator = new Validator();

        //act
        var result = validator.ValidateOverlapping(ranges, input);

        //assert
        result.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(ValidatorTestsData))]
    public void ValidateOverlapping_ForNonOverlappingDateRanges_ReturnsTrue(List<DateRange> ranges)
    {
        // arrange
        var input = new DateRange(new DateTime(2020, 1, 26), new DateTime(2020, 1, 30));
        var validator = new Validator();

        //act
        var result = validator.ValidateOverlapping(ranges, input);

        //assert
        result.Should().BeTrue();
    }
}