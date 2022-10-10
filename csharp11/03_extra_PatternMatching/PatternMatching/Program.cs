using static TrafficLight;

TypePatternSample();
PatternWithLogicalOperator();

await TuplesPatternSampleAsync();

PropertyPatternSample();

ListPatternSample();

void TypePatternSample()
{
    static string TypePattern(object? o) =>
        o switch
        {
            null => "just null",
            Book b => $"a book with this title: {b.Title}",
            42 => "a constant with value 42",
            string[] { Length: > 3 } => "a string array with more than 3 elements",
            string[] => "any other string array",
            _ => "anything else"
        };

    Console.WriteLine($"{nameof(TypePatternSample)}");
    Console.WriteLine(TypePattern(null));
    Console.WriteLine(TypePattern(new Book("Professional C#", "Wiley")));
    Console.WriteLine(TypePattern(42));
    Console.WriteLine(TypePattern(new [] {"one", "two", "three", "four"}));
    Console.WriteLine(TypePattern(new[] {"one", "two"}));
    Console.WriteLine(TypePattern("more"));
    Console.WriteLine();
}

void PatternWithLogicalOperator()
{
    string LogicalOperators(Person p) =>
        p switch
        {
            { PhoneNumber: not null } and { PhoneNumber: not ""} => $"{p.FirstName} {p.LastName}, ({p.PhoneNumber})",
            _ => $"{p.FirstName} {p.LastName}"
        };

    Console.WriteLine(nameof(PatternWithLogicalOperator));
    Person p1 = new("Tom", "Turbo");
    Person p2 = new("Bruce", "Wayne", "+108154711");
    Console.WriteLine(LogicalOperators(p1));
    Console.WriteLine(LogicalOperators(p2));
    Console.WriteLine();
}

async Task TuplesPatternSampleAsync()
{
    static (TrafficLight Current, TrafficLight Previous) NextLight(TrafficLight current, TrafficLight previous) =>
        (current, previous) switch
        {
            (Red, _) => (Amber, current),
            (Amber, Red) => (Green, current),
            (Green, _) => (Amber, current),
            (Amber, Green) => (Red, current),
            _ => (Amber, current)
        };

    Console.WriteLine(nameof(TuplesPatternSampleAsync));
    var currentLight = Red;
    var previousLight = Red;
    for (int i = 0; i < 10; i++)
    {
        (currentLight, previousLight) = NextLight(currentLight, previousLight);
        Console.WriteLine($"current light: {currentLight}");
        await Task.Delay(1000);
    }
    Console.WriteLine();
}

void PropertyPatternSample()
{
    string Check(Person person) =>
        person switch
        {
            { FirstName: "Clark" } => $"{person} is a Clark",
            { Address: { City: "Smallville" } } => $"{person} is from Smallville",
            { Address.City: "Gotham City" } => $"{person} is from Gotham City",  // new with C# 10
            _ => $"{person} is not listed"
        };

    foreach (var p in GetPeople())
    {
        Console.WriteLine(Check(p));
    }
}

void ListPatternSample()
{
    string ListPattern(int[] list)
    {
        return list switch
        {
            [1, 2, 3] => "1, 2, and 3 in the list",
            [1, 2, .. var x, 5] => $"slice pattern {x}",
            { Length: > 3 } => "array with more than three elements",            
            _ => "not listed"
        };
    }

    int[] one = { 1, 2, 3 };
    int[] two = { 1, 2, 4, 5, 6, 9, 11, 5 };
    Console.WriteLine(ListPattern(one));
    Console.WriteLine(ListPattern(two));
}


IEnumerable<Person> GetPeople() =>
    new[] 
    {
        new Person("Clark", "Kent", Address: new Address("Smallville", "USA")),
        new Person("Lois", "Lane", Address: new Address("Smallville", "USA")),
        new Person("Bruce", "Wayne", Address: new Address("Gotham City", "USA")),
        new Person("Alfred", "Pennyworth", Address: new Address("Gotham City", "USA")),
        new Person("Dick", "Grayson", Address: new Address("Gotham City", "USA")),
        new Person("Barry", "Allen", Address: new Address("Central City", "USA")),
    };

public enum TrafficLight
{
    Red,
    Amber,
    Green
}

public record Book(string Title, string Publisher);

public record Address(string City, string Country);

public record Person(string FirstName, string LastName, string? PhoneNumber = null, Address? Address = null)
{
    public override string ToString() => $"{FirstName} {LastName}";
}
