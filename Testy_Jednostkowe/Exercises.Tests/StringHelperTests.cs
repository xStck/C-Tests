using Exercises._1;
using FluentAssertions;

namespace Exercises.Tests;

public class StringHelperTests
{
    [Theory]
    // [ClassData(typeof(StringHelperTestsData))]
    [JsonFileData("Data/StringHelperData.json")]
    public void ReverseWords_ForGivenSentence_ReturnsReversedSentence(string words, string result)
    {
        //act
        var reversedWords = StringHelper.ReverseWords(words);
        //assert
        reversedWords.Should().Be(result);
    }

    [Theory]
    [InlineData("ala")]
    [InlineData("kamilslimak")]
    public void IsPalindrom_ForGivenWord_ReturnsTrueValue(string word)
    {
        //act
        var palindrom = StringHelper.IsPalindrom(word);
        //assert
        palindrom.Should().Be(true);
    }

    [Theory]
    [InlineData("Kamien")]
    [InlineData("Testowanie")]
    public void IsPalindrom_ForGivenWord_ReturnsFalseValue(string word)
    {
        //act
        var palindrom = StringHelper.IsPalindrom(word);
        //assert
        palindrom.Should().Be(false);
    }
}