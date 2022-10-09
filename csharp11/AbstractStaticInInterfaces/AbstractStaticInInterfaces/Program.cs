
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
