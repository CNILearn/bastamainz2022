using System.Numerics;

Point<int> pt1 = new(4, 5);
Translation<int> tr1 = new(2, 3);
Point<int> pt2 = pt1 + tr1;
Console.WriteLine(pt2);

Point<double> pt3 = new(4.5, 3.3);
Translation<double> tr2 = new(2.2, 3.3);
Point<double> pt4 = pt3 + tr2;
Console.WriteLine(pt4);

public record struct Translation<T>(T XOffset, T YOffset)
    : IAdditiveIdentity<Translation<T>, Translation<T>>
    where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
{
    public static Translation<T> AdditiveIdentity =>
        new(XOffset: T.AdditiveIdentity, YOffset: T.AdditiveIdentity);
}

public record struct Point<T>(T X, T Y)
    : IAdditionOperators<Point<T>, Translation<T>, Point<T>>
    where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
{
    public static Point<T> operator +(Point<T> left, Translation<T> right) =>
        left with { X = left.X + right.XOffset, Y = left.Y + right.YOffset };
}
