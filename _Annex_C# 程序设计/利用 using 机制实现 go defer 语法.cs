using var defer = Defer.New(() => Console.WriteLine(1));
defer.Do(() => Console.WriteLine(2));
Console.WriteLine("Hello");
defer.Do(() => Console.WriteLine(3));
using (defer.Do(() => Console.WriteLine(4)))
{
    // ...
} // >> 提前 defer
defer.Do(() => Console.WriteLine(5));
Console.WriteLine("World");
// Hello, 4,3,2,1, World ,5

/// <summary>
/// defer.Do(action) like a go defer.
/// <code>
/// {
///     using var defer = Defer.New(ac);
///     ...
///     defer.Do(ac2)
///     ...
/// } // >>> defer do ac2, ac1 ... ac
/// </code>
/// </summary>
public sealed class Defer : IDisposable
{
    static Stack<Defer> deferPool = new Stack<Defer>();

    private Defer()
    {
        deferStack = new Stack<Action>();
    }
    private Stack<Action> deferStack;


    public static Defer New(Action action)
    {
        Defer defer;
        lock (deferPool)
        {
            if (deferPool.TryPop(out Defer d))
                defer = d;
            else defer = new Defer();
        }
        defer.Do(action);
        return defer;
    }
    public Defer Do(Action action)
    {
        if (action is not null)
            this.deferStack.Push(action);
        return this;
    }
    void IDisposable.Dispose()
    {
        for (int i = 0; i < deferStack.Count;)
        {
            try
            {
                this.deferStack.Pop().Invoke();
            }
            catch { }
        }
        lock (deferPool)
            Defer.deferPool.Push(this);
    }
}