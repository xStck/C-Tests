namespace Exercises._2;

public static class ListHelper
{
    public static List<int> FilterOddNumber(List<int> listOfNumbers)
    {
        var evenNumbers = new List<int>();
        for (var i = 0; i < listOfNumbers.Count; i++)
            if (listOfNumbers[i] % 2 != 0)
                evenNumbers.Add(listOfNumbers[i]);

        return evenNumbers;
    }
}