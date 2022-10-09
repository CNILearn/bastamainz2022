using System.Numerics;

var result = AddAll4<int>(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
Console.WriteLine(result);

var r = Average(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
Console.WriteLine(r);

double Average(int[] values)
{
    int counter = 0;
    int result = 0;
    foreach (var val in values)
    {
        result += val;
        counter++;
    }
    return (double) result / counter;
}

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

// INumber, IParsable

public interface IGame<T>
    where T : class
{
    void Start();
    void Move(T move);
}

public class GameServer<T>
    where T : class, IParsable<T>, IGame<T>
{
    private T _item;
    public GameServer(T item)
    {
        _item = item;
    }

    T GetGame(string s)
    {
        return T.Parse(s, null);
    }
}