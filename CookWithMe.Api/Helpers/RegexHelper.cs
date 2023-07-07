using System.Text.RegularExpressions;

namespace CookWithMe.Helpers;

public static class RegexHelper
{
    public static readonly Regex LettersAndNumbers = new Regex("^[a-zA-Z0-9]*$");
    public static readonly Regex Letters = new Regex("^[a-zA-Z]*$");
}