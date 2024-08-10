using System.Collections;
using Exercises._3;

namespace Exercises.Tests;

public class ValidatorTestsData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
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

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}