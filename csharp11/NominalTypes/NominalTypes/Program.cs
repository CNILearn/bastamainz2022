using System.Diagnostics.CodeAnalysis;

Person p1 = new() { FirstName = "John", LastName = "Doe" };
Person p2 = new("Tom", "Turbo");

//public record class Person(string FirstName, string LastName, string? Middlename = default);

//public record class Student(string FirstName, string LastName, string Course, string? Middlename = default)
//    : Person(FirstName, LastName, Middlename);


public class Person
{
    public Person()
    {        
    }

    [SetsRequiredMembers]
    public Person(string first, string last)
    {
        (FirstName, LastName) = (first, last); 
    }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string? MiddleName { get; init; }
}
