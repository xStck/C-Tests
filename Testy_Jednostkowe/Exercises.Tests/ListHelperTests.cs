using Exercises._2;
using FluentAssertions;

namespace Exercises.Tests;

public class ListHelperTests
{
    public static IEnumerable<object[]> GetSampleData()
    {
        yield return new object[] { new List<int> { 1, 2, 3, 4, 5 }, new List<int>  { 1, 3, 5 } };
        yield return new object[] { new List<int>  { 4, 4, 4, 4 }, new List<int>() };
        yield return new object[] { new List<int>  { 5, 5, 5, 5, 5 }, new List<int>  { 5, 5, 5, 5, 5 } };
    }
    
    [Theory]
    [MemberData(nameof(GetSampleData))]
    public void FilterOddNumber_ForListOfNumbers_ReturnsEvenNumbersOnly
        (List<int> numbers, List<int> expectResult)
    {
        //arrange
        //act
        var result = ListHelper.FilterOddNumber(numbers);
        //assert
        result.Should().Equal(expectResult);
    }
}