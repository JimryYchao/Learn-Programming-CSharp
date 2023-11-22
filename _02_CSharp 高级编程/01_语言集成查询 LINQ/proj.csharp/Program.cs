class A
{
    ~A() => System.Console.WriteLine("Destruct instance of A");
}

class B
{
    object Ref;

    public B(object o)
    {
        Ref = o;
    }

    ~B() => Console.WriteLine("Destruct instance of B");
}
class Test
{
    static void CollectTest()
    {
        B b = new B(new A());
        b = null;
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
    static void Main()
    {
        CollectTest();
        Console.ReadKey();
    }
}
/** Output maybe
    Destruct instance of A
    Destruct instance of B
*** or    
    Destruct instance of B
    Destruct instance of A
 */