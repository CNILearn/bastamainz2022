using System.Diagnostics.CodeAnalysis;
using System.Text;

// UTF8 string literal

ReadOnlySpan<byte> mystring = "Bücher"u8;
byte[] array = "my string"u8.ToArray();


Console.WriteLine(Encoding.UTF8.GetString(mystring));

// raw string literal

ProcessXml("""
    <books>
        <book isbn="3443874">
            <title>Professional C#</title>
        </book>
    </books>
    """);

// keep indentation
string s2 = """
    <books>
        <book isbn="3443874">
            <title>Professional C#</title>
        </book>
    </books>
""";

Console.WriteLine(s2);
Console.WriteLine();

// string interpolation
string isbn = "344343";
string title = "Professional C# and .NET";

string s3 = $""" 
    <books>
        <book isbn="{isbn}">
            <title>{title}</title>
        </book>
    </books>
    """;

Console.WriteLine(s3);
Console.WriteLine();

// replace { with {{ using $$
string val = "waboom";
string code1 = $$"""
public void Main()
{
    string hello = "{{val}}";
    Console.WriteLine($"This is so cool {hello}");
}
""";



ProcessCSharp("""
    void Foo()
    {
        Console.WriteLine(""Hello World"");
    }
    """);

Console.WriteLine(code1);
Console.WriteLine();

// generate JSON
string json1 = $$"""
    {
      "book": {
        "title": "{{title}}",
        "publisher": "Wiley"
      }
    };
    """;

ProcessJson(json1);


ProcessJson($$"""
{
    "book": {
        "title": "{{title}}",
        "publisher": "Wiley"
    }
};
""");



void ProcessXml([StringSyntax(StringSyntaxAttribute.Xml)] string xml)
{
    Console.WriteLine(xml);
    Console.WriteLine();
}

void ProcessJson([StringSyntax(StringSyntaxAttribute.Json)] string json)
{
    Console.WriteLine(json);
    Console.WriteLine();
}

void ProcessCSharp([StringSyntax("csharp")] string csharp)
{
    Console.WriteLine(csharp);
    Console.WriteLine();
}