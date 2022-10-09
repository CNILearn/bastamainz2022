
using System.Runtime.CompilerServices;

Test<FooTest> t1 = new();
t1.TestMethod();

public class FooTest : IFoo
{
    public static void Foo()
    {
        Console.WriteLine("FooTest.Foo");
    }
}

public class Test<T>
    where T : IFoo
{
    public void TestMethod()
    {
        T.Foo();
    }
}

public interface IFoo
{
    abstract static void Foo();
}

public interface IFoo2 : IFoo
{
    static virtual void Foo2()
    {
        Console.WriteLine("IFoo2.Foo");
    }
}

public class FooTest2 : IFoo2
{
    public static void Foo()
    {
        Console.WriteLine("FooTest2.Foo");
    }
}