using System.Numerics;

var result = AddAll1(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
Console.WriteLine(result);

var result2 = AddAll2(new double[] { 1, 2, 3, 4.8, 5, 6, 7, 8, 9 });
Console.WriteLine(result2);

int AddAll1(int[] values)
{
    int result = 0;
    foreach (var value in values)
    {
        result += value;
    }
    return result;
}

// with INumber<T> constraint!
T AddAll2<T>(T[] values) where T : INumber<T>
{
    T result = T.Zero;
    foreach (var value in values)
    {
        result += value;
    }
    return result;
}

// with list pattern matching - and INumber<T> constraint
T AddAll3<T>(T[] values) where T : INumber<T> =>
    values switch
    {
        [] => T.Zero,
        [var first, .. var rest] => first + AddAll3(rest),
    };

T AddAll4<T>(Span<T> values) where T : INumber<T> =>
    values switch
    {
        [] => T.Zero,
        [var first, .. var rest] => first + AddAll4(rest),
    };
