using System.Numerics;

var result = AddAll(new[] { 1, 2, .3, 4, 5, 6, 7, 8, 9 });
Console.WriteLine(result);

//int AddAll(int[] values)
//{
//    int result = 0;
//    foreach (var value in values)
//    {
//        result += value;
//    }
//    return result;
//}

T AddAll<T>(T[] values) where T : INumber<T>
{
    T result = T.Zero;
    foreach (var value in values)
    {
        result += value;
    }
    return result;
}

T AddAll2<T>(T[] values) where T : INumber<T> =>
    values switch
    {
        [] => T.Zero,
        [var first, .. var rest] => first + AddAll2(rest),
    };

T AddAll3<T>(Span<T> values) where T : INumber<T> =>
    values switch
    {
        [] => T.Zero,
        [var first, .. var rest] => first + AddAll3(rest),
    };

// INumber, IParsable

Student s = new Student { FirstName = "Tom", LastName = "Turbo" };

public class Person
{
    public required string FirstName { get; init; }
    public string? MiddleName { get; init; }
    public required string LastName { get; set; }

}

public class Student : Person
{
    public int Id { get; init; }
}