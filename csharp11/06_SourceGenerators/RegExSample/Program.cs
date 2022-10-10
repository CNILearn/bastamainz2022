using System.Text.RegularExpressions;


string email = "Christian@christiannagel.com";
bool isEmail = MyRegEx.CheckEmail(email);
Console.WriteLine(isEmail);

isEmail = MyRegEx.CheckEmail("test.com");
Console.WriteLine(isEmail);

public partial class MyRegEx
{
    [GeneratedRegex("""^[a-z]+@[a-z]+.[a-z]+$""", RegexOptions.IgnoreCase)]
    public static partial Regex Email();

    public static bool CheckEmail(string input)
    {
        bool isMatch = Email().IsMatch(input);
        return isMatch;
    }

}