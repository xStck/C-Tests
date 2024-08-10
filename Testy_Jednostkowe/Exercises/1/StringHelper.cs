namespace Exercises._1;

public static class StringHelper
{
    public static string ReverseWords(string str)
    {
        var splitWords = str.Split(' ');
        var reversedWords = splitWords.Where(x => x != "").Select(x => x.Trim()).Reverse();
        var result = string.Join(' ', reversedWords);
        return result;
    }

    public static bool IsPalindrom(string str)
    {
        return str.SequenceEqual(str.Reverse());
    }
}