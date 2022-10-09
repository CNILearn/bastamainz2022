using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string email = "christian@christiannagel.com";
bool isEmail = MyRegEx.CheckEmail(email);
Console.WriteLine(isEmail);

isEmail = MyRegEx.CheckEmail("test.com");
Console.WriteLine(isEmail);

public partial class MyRegEx
{
    [GeneratedRegex(@"^[a-z|A-Z]+@[a-z|A-Z]+.[a-z|A-Z]+$")]
    public static partial Regex Email();

    public static bool CheckEmail(string input)
    {
        bool isMatch = Email().IsMatch(input);
        return isMatch;
    }

}