## CSharp 语句

---
### 声明语句

- 声明语句声明新的局部变量、局部常量或 `ref` 局部变量。 

```csharp
int a, b;        // 仅声明
int c = 1;       // 声明并初始化
var arr = new[] { 1, 2, 3, 4, 5 };    // 隐式类型声明
const string greeting = "Hello";      // 常量声明
ref int r_num = ref c;
```

---
### 选择语句

#### if 语句

- `if`、`if-else`、`if-else if` 语句根据布尔表达式的结果选择要遵循的若干代码路径的哪一个。

```csharp
string scan = Console.ReadLine();

if (scan is null)
    Console.WriteLine("Input NULL");
else if (scan is "")
    Console.WriteLine("Input Empty");
else Console.WriteLine(scan);
```

<br>

#### switch 语句

- `switch` 语句根据与匹配表达式匹配的模式来选择要执行的语句列表。可以为 `switch` 语句的一部分指定多个 `case` 模式；`default` 模式始终匹配成功。在 `switch` 语句中，控制不能从一个 `case` 部分贯穿到下一个 `case` 部分。可以使用跳转语句将控制从 `switch` 传递出去。可以在 `case` 模式中使用 `when` 筛选。

```csharp
DisplayMeasurements(3, 4);  // Output: First measurement is 3, second measurement is 4.
DisplayMeasurements(5, 5);  // Output: Both measurements are valid and equal to 5.
DisplayMeasurements(-1, 1); // Output: One or both measurements are not valid.

void DisplayMeasurements(int a, int b)
{
    switch ((a, b))
    {
        case ( > 0, > 0) when a == b:
            Console.WriteLine($"Both measurements are valid and equal to {a}.");
            break;
        case ( > 0, > 0):
            Console.WriteLine($"First measurement is {a}, second measurement is {b}.");
            break;
        default:
            Console.WriteLine("One or both measurements are not valid.");
            break;
    }
}
```

---
### 迭代语句

#### for 语句

- `for( 初始化表达式; 条件; 迭代器){ 循环体 }` 在指定条件的布尔表达式的计算结果为 `true` 时，`for` 语句会执行一条语句或一个语句块。“初始化表达式” 部分仅在进入循环前执行一次，并根据条件的值确定是否进入循环体。“条件” 部分在返回 `true` 或不存在时执行循环中的下一个迭代。“迭代器” 部分定义循环主体的每次执行后将执行的操作。`for` 语句的每部分都是可选的：`for(;;);`。

```csharp
for (int i = 0; i < 10; i++)
    Console.WriteLine(i);
```

<br>

#### foreach 语句

- `foreach(var t in ts)` 语句为类型实例中实现 `System.Collections.IEnumerable` 或 `System.Collections.Generic.IEnumerable<T>` 接口的每个元素执行语句或语句块。

```csharp
var fibNumbers = new List<int> { 0, 1, 1, 2, 3, 5, 8, 13 };
foreach (int element in fibNumbers)
{
    Console.Write($"{element} ");
}
// Output:
// 0 1 1 2 3 5 8 13
```

- 除了这些类型，迭代对象可以是其类型具有公共无参数 `GetEnumerator` 方法（从 C# 9 开始，`GetEnumerator` 方法可以是类型的扩展方法），`GetEnumerator` 方法的返回类型具有公共 `Current` 属性和公共无参数 `bool MoveNext` 方法。

```csharp
NumArray arr = new NumArray(1, 2, 3, 4, 5, 6, 7, 8, 9);
foreach (int i in arr)
    Console.WriteLine(i);

record struct NumArray(params int[] nums)
{
    public IEnumerator<int> GetEnumerator() => nums.ToList().GetEnumerator();
}
```

- 若枚举器的 `Current` 属性返回 `ref Current` 则可以使用 `ref` 或 `ref readonly` 修饰声明迭代变量。

```csharp
Span<int> storage = stackalloc int[10];
int num = 0;
foreach (ref int item in storage)
    item = num++;
foreach (ref readonly var item in storage)
    Console.Write($"{item} ");
// Output:
// 0 1 2 3 4 5 6 7 8 9
```

> await foreach

- 可以使用 `await foreach` 语句来使用异步数据流，即对实现 `IAsyncEnumerable<T>` 接口的集合类型进行迭代。异步检索下一个元素时，可能会挂起循环的每次迭代。可以将 `await foreach` 语句与类型具有公共无参 `GetAsyncEnumerator` 方法且该方法的返回类型具有公共 `Current` 属性和公共无参数 `ValueTask<bool> MoveNextAsync` 方法的实例一起使用。

```csharp
await foreach (int n in GenerateNumbersAsync(5))
{
    Console.Write(n);
    Console.Write(" ");
}
// Output: 0 2 4 6 8
async IAsyncEnumerable<int> GenerateNumbersAsync(int count)
{
    for (int i = 0; i < count; i++)
        yield return await ProduceNumberAsync(i);
}
async Task<int> ProduceNumberAsync(int seed)
{
    await Task.Delay(1000);
    return 2 * seed;
}
```

<br>

#### do 语句

- `do{ .. }while(e)` 在指定的布尔表达式 `e` 的计算结果为 `true` 时，`do` 语句会执行一条语句或一个语句块。在每次执行循环之后都会计算此表达式，因此 `do` 循环会执行一次或多次。

```csharp
int n = 0;
do
{
    Console.Write(n);
    n++;
} while (n < 5);
// Output:
// 01234
```

<br>

#### while 语句

- `while(e){ .. }` 在指定的布尔表达式 `e` 的计算结果为 `true` 时，`while` 语句会执行一条语句或一个语句块。由于在每次执行循环之前都会计算此表达式，所以 `while` 循环会执行零次或多次。

```csharp
int n = 0;
while (n < 5)
{
    Console.Write(n);
    n++;
}
// Output:
// 01234
```

---
### 跳转语句

#### break 语句

- `break` 语句将终止最接近的封闭迭代语句（即 `for`、`foreach`、`while` 或 `do` 循环）或 `switch` 语句。`break` 语句将控制权转交给已终止语句后面的语句。在嵌套循环中，`break` 语句仅终止包含它的最内部循环。在循环内使用 `switch` 语句时，`switch` 节末尾的 `break` 语句仅从 `switch` 语句中转移控制权。

```csharp
double[] measurements = { -4, 5, 30, double.NaN };
foreach (double measurement in measurements)
{
    switch (measurement)
    {
        case < 0.0:
            Console.WriteLine($"Measured value is {measurement}; too low.");
            break;
        case > 15.0:
            Console.WriteLine($"Measured value is {measurement}; too high.");
            break;
        case double.NaN:
            Console.WriteLine("Failed measurement.");
            break;
        default:
            Console.WriteLine($"Measured value is {measurement}.");
            break;
    }
}
// Output:
// Measured value is -4; too low.
// Measured value is 5.
// Measured value is 30; too high.
// Failed measurement.
```

<br>

#### continue 语句

- `continue` 语句启动最接近的封闭迭代语句（即 `for`、`foreach`、`while` 或 `do` 循环）的新迭代。

```csharp
for (int i = 0; i < 5; i++)
{
    Console.Write($"Iteration {i}: ");
    if (i < 3)
    {
        Console.WriteLine("skip");
        continue;
    }
    Console.WriteLine("done");
}
// Output:
// Iteration 0: skip
// Iteration 1: skip
// Iteration 2: skip
// Iteration 3: done
// Iteration 4: done
```

<br>

#### return 语句

- `return` 语句终止它所在的函数的执行，并将控制权和函数结果（若有）返回给调用方。

```csharp
double surfaceArea = CalculateCylinderSurfaceArea(1, 1);
Console.WriteLine($"{surfaceArea:F2}"); // output: 12.57

double CalculateCylinderSurfaceArea(double baseRadius, double height)
{
    double baseArea = Math.PI * baseRadius * baseRadius;
    double sideArea = 2 * Math.PI * baseRadius * height;
    return 2 * baseArea + sideArea;     // return
}
```

<br>

#### goto 语句

- `goto` 语句将控制权转交给带有标签的语句

```csharp
var matrices = new Dictionary<string, int[][]>
{
    ["A"] = new[]
    {
        new[] { 1, 2, 3, 4 },
        new[] { 4, 3, 2, 1 }
    },
    ["B"] = new[]
    {
        new[] { 5, 6, 7, 8 },
        new[] { 8, 7, 6, 5 }
    },
};

CheckMatrices(matrices, 4);

void CheckMatrices(Dictionary<string, int[][]> matrixLookup, int target)
{
    foreach (var (key, matrix) in matrixLookup)
    {
        for (int row = 0; row < matrix.Length; row++)
            for (int col = 0; col < matrix[row].Length; col++)
                if (matrix[row][col] == target)
                    goto Found;
        Console.WriteLine($"Not found {target} in matrix {key}.");
        continue;
    Found:
        Console.WriteLine($"Found {target} in matrix {key}.");
    }
}
// Output:
// Found 4 in matrix A.
// Not found 4 in matrix B.
```

- 在 `switch` 语句中 使用 `goto` 语句将控制权移交到具有常量大小写的 `case` 或 `goto default`。

```csharp
public enum CoffeeChoice
{
    Plain,
    WithMilk,
    WithIceCream,
}
public class GotoInSwitchExample
{
    public static void Main()
    {
        Console.WriteLine(CalculatePrice(CoffeeChoice.Plain));  // output: 10.0
        Console.WriteLine(CalculatePrice(CoffeeChoice.WithMilk));  // output: 15.0
        Console.WriteLine(CalculatePrice(CoffeeChoice.WithIceCream));  // output: 17.0
    }
    private static decimal CalculatePrice(CoffeeChoice choice)
    {
        decimal price = 0;
        switch (choice)
        {
            case CoffeeChoice.Plain:
                price += 10.0m;
                break;
            case CoffeeChoice.WithMilk:
                price += 5.0m;
                goto case CoffeeChoice.Plain;
            case CoffeeChoice.WithIceCream:
                price += 7.0m;
                goto case CoffeeChoice.Plain;
        }
        return price;
    }
}
```

<br>

#### yield 语句

- 在迭代器中使用 `yield` 语句提供下一个值的 `yield return` 或表示迭代结束的 `yield break`。迭代器的返回类型可以是 `IEnumerable<T>`、`IEnumerable`、`IAsyncEnumerable<T>` 异步迭代。
- 当开始对迭代器的结果进行迭代时，迭代器会一直执行，直到到达第一个 `yield return` 语句为止。 然后，迭代器的执行会暂停，调用方会获得第一个迭代值并处理该值。在后续的每次迭代中，迭代器的执行都会在导致上一次挂起的 `yield return` 语句之后恢复，并继续执行，直到到达下一个 `yield return` 语句为止。当控件到达迭代器或 `yield break` 语句的末尾时，迭代完成。
- Lambda 表达式中不允许使用 `yield return` 语句。

```csharp
foreach (var item in Square([1, 2, 3, 4, 5, 6, 99999/* 溢出位 */, 7, 8, 9]))
    Console.Write(item + " ");  // output: 1 4 9 16 25 36
IEnumerable<int> Square(int[] items)
{
    int rt;
    for (int i = 0; i < items.Length; i++)
    {
        try { rt = checked(items[i] * items[i]); }
        catch { yield break; }  // 在溢出时跳出迭代
        yield return rt;
    }
}
```

> 异步迭代器

```csharp
await foreach (var item in SquareAsync([1, 2, 3, 4, 5, 99999, 6, 7, 8]))
    Console.WriteLine(item);

async IAsyncEnumerable<int> SquareAsync(int[] nums)
{
    int rt = 0;
    for (int i = 0; i < nums.Length; i++)
    {
        try { rt = await Square(nums[i]); }
        catch
        {
            Console.WriteLine("Overflow");
            yield break;
        }
        yield return rt;
    }
}
async Task<int> Square(int num)
{
    await Task.Run(() => Console.Write($"Input {num}*{num} = "));
    return checked(num * num);
};
```

---
### 异常处理语句

- 使用 `throw` 和 `try` 语句来处理异常，使用 `throw` 语句引发异常，使用 `try` 语句捕获和处理在执行代码块期间可能发生的异常。
- 引发异常时，公共语言运行时 CLR 将查找可以处理此异常的 `catch` 块。如果当前执行的方法不包含此类 `catch` 块，则 CLR 查看调用了当前方法的方法，并以此类推遍历调用堆栈。如果未找到 `catch` 块，CLR 将终止正在执行的线程。

<br>

#### throw 语句

- 在 `throw e;` 语句中，表达式 `e` 的结果必须隐式转换为 `System.Exception`。可以在 `catch` 块中使用 `throw;` 语句重新引发由 `catch` 处理的异常。`throw;` 保留异常的原始堆栈跟踪，该跟踪存储在 `Exception.StackTrace` 属性中；`throw e;` 更新 `e` 的 `StackTrace` 属性。

```csharp
try
{
    Action ac = null;
    ac();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw e;  // line 信息更新
}
```

> throw 表达式

```csharp
class Person(string name)
{
    public string Name => name ?? throw new ArgumentNullException(nameof(name));
}
```

<br>

#### try-catch

- 用 `try-catch` 语句处理在执行代码块期间可能发生的异常。将代码置于 `try` 块中可能发生异常的位置，使用 `catch` 子句指定要在相应的 `catch` 块中处理的异常的基类型。
- 可以提供多个 `catch` 子句，也可以为 `catch` 指定异常筛选器 `when`。可以在 `catch` 中使用 `throw` 语句重新引发异常。

```csharp
try
{
    // try do....
}
catch (ArgumentException e) when (e is ArgumentNullException || e is ArgumentOutOfRangeException)
{
    Console.WriteLine($"Processing failed: {e.Message}");
}
catch (OperationCanceledException)
{
    Console.WriteLine("Processing is cancelled.");
}
catch // 无筛选条件
{
    throw;  // 重新引发异常
}
```

- 如果异步函数中发生异常，则等待函数的结果时，它会传播到函数的调用方。如果迭代器方法中发生异常，则仅当迭代器前进到下一个元素时，它才会传播到调用方。

```csharp
await Run();

static async Task Run()
{
    try
    {
        Task<int> processing = ProcessAsync(-1);
        Console.WriteLine("Launched processing.");

        int result = await processing;
        Console.WriteLine($"Result: {result}.");
    }
    catch (ArgumentException e)
    {
        Console.WriteLine($"Processing failed: {e.Message}");
    }
    // Output:
    // Launched processing.
    // Processing failed: Input must be non-negative. (Parameter 'input')
}
static async Task<int> ProcessAsync(int input)
{
    if (input < 0)
    {
        throw new ArgumentOutOfRangeException(nameof(input), "Input must be non-negative.");
    }

    await Task.Delay(500);
    return input;
}
```

<br>

#### try-finally

- 在 `try-finally` 语句中，当控件离开 `try` 块时，将执行 `finally` 块。控件可能会离开 `try` 块，由于 `try` 正常执行、或执行跳转语句、或发生异常。可以使用 `finally` 块来清理 `try` 块中使用的已分配资源。可以在 `finally` 块之前可选的使用 `catch` 块进行捕获异常处理。

```csharp
try
{
    int[] arr = [1, 2, 3, 4, 5, 6, 7, 8, 9];
    for (int i = 0; i <= arr.Length; i++)
        arr[i]++;
}
catch (ArgumentException e) when (e is ArgumentNullException || e is ArgumentOutOfRangeException)
{
    Console.WriteLine($"Processing failed: {e.Message}");
}
finally
{
    Console.WriteLine("Finally ...");
}
```

---
### checked 和 unchecked 语句

- `checked` 和 `unchecked` 语句指定整型类型算术运算和转换的溢出检查上下文。当发生整数算术溢出时，溢出检查上下文将定义发生的情况。在已检查的上下文中，引发 `System.OverflowException`；如果在常数表达式中发生溢出，则会发生编译时错误。在未检查的上下文中，会通过丢弃任何不适应目标类型的高序位来将操作结果截断。 

- 默认情况下，整型算术运算和转换在未检查的上下文中执行。常数表达式在已检查的上下文中计算，如果发生溢出，则会发生编译时错误。可以使用 `unchecked` 为常数表达式显式指定未检查的上下文。

- 从 C#11 开始，用户可以定义的 `checked` 运算符重载和转换。

```csharp
int Sum(int x, int y) => unchecked(x + y);
int Mul(int x, int y)
{
    checked
    {
        int mul = x * y;
        Console.WriteLine(mul);
        return mul;
    }
}

```

---
### fixed 语句

- `fixed` 语句可防止垃圾回收器重新定位可移动变量，并声明指向该变量的指针。固定变量的地址在语句的持续时间内不会更改。只能在相应的 `fixed` 语句中使用声明的指针，且声明的指针是只读的，无法修改。
- `fixed` 而可初始化声明使用数组的指针、使用变量的地址、使用实现名为 `GetPinnableReference` 的方法的类型实例（方法返回非托管类型的 `ref` 变量，例如 .NET 类型 `System.Span<T>` 和 `System.ReadOnlySpan<T>`）、使用字符串、使用固定大小的缓冲区（堆栈上声明的 `stackalloc` 内存不需要固定）。

> 使用数组

```csharp
unsafe
{
    int[] arr = [10, 20, 30, 40, 50];
    fixed (int* p = arr)
    {
        int index = 0;
        foreach (int i in arr)
        {
            p[index] = i * i; 
            index++;
        }
        Console.WriteLine(string.Join(", ", arr));
        // Output: 100, 400, 900, 1600, 2500
    }
}
```

> 使用变量的地址

```csharp
unsafe
{
    int[] numbers = { 10, 20, 30 };
    fixed (int* toFirst = &numbers[0], toLast = &numbers[^1])
        Console.WriteLine(toLast - toFirst);  // output: 2
}
```

> 使用实现名为 `GetPinnableReference` 的方法的类型实例

```csharp
NumberArray arr = new(1, 2, 3, 4, 5, 6);
unsafe
{
    fixed(int* p = arr)
        for(int i = 0;i< 6; i++)
            Console.WriteLine(p[i]);
}
record NumberArray(params int[] arr)
{
    public ref int GetPinnableReference() => ref arr[0];
}
```

> 使用字符串

```csharp
ToUpper("Hello, World"); // Output: HELLO, WORLD

unsafe static void ToUpper(string str)
{
    fixed(char* f = str)
    {
        int index = 0;
        foreach (char c in str)
            f[index] = char.ToUpper(f[index++]);
    }
    Console.WriteLine(str);
}
```

> 使用固定大小的缓冲区

```csharp
internal unsafe struct Buffer
{
    public fixed byte fixedBuffer[1024];
}
internal unsafe class Example
{
    public Buffer buffer = default;
}
class Sample
{
    unsafe void Handle(byte[] data)
    {
        var ex = new Example();
        fixed (byte* buff = ex.buffer.fixedBuffer)
        {
            // handle data in fixedBuffer
        }
    }
}
```

---
### lock 语句

- `lock(x){ .. }` 语句获取给定对象的互斥锁，执行语句块，然后释放锁，其中 `x` 是引用类型。当锁被持有时，持有该锁的线程可以再次获取并释放该锁。任何其他线程都被阻止获取锁并等待，直到锁被释放。`lock` 语句确保在任何时刻最多只有一个线程执行它的线程体。
- 在 `lock` 语句的正文中不能使用 `await` 表达式。

```csharp
public class Account
{
    private readonly object balanceLock = new object();
    private decimal balance;
    public Account(decimal initialBalance) => balance = initialBalance;
    public void UpdateAccount(decimal amount)
    {
        lock (balanceLock)
            balance += amount;
    }
    public decimal GetBalance()
    {
        lock (balanceLock)
            return balance;
    }
}
class AccountTest
{
    static async Task Main()
    {
        var account = new Account(1000);
        var tasks = new Task[100];
        for (int i = 0; i < tasks.Length; i++)
            tasks[i] = Task.Run(() => Update(account));
        await Task.WhenAll(tasks);
        Console.WriteLine($"Account's balance is {account.GetBalance()}");
        // Output:
        // Account's balance is 2000
    }
    static void Update(Account account)
    {
        decimal[] amounts = { 0, 2, -3, 6, -2, -1, 8, -5, 11, -6 };
        foreach (var amount in amounts)
            account.UpdateAccount(amount);
    }
}
```

  - 当同步对共享资源的线程访问时，一般锁定专用对象实例（例如，`private readonly object balanceLock = new object();`）或另一个不太可能被代码无关部分用作 `lock` 对象的实例。避免对不同的共享资源使用相同的 `lock` 对象实例，因为这可能导致死锁或锁争用。避免使用 `this`、`Type` 实例、字符串字面量作为 `lock` 对象。

---
### using 语句

- `using(<IDisposable> disposable){ .. }` 语句或 `using <IDisposable> disposable;` 声明可确保正确使用 `IDisposable` 实例 `disposable`：`disposable` 局部变量在它的作用域末尾调用它的 `Dispose` 方法并释放该对象。`using` 语句可确保在发生异常的情况下也会释放 `IDisposable` 实例。在一个 `using` 语句中声明多个实例时，它们将按声明的相反顺序释放。
- 由 `using` 语句或声明进行声明的变量是只读的，无法重新分配该变量或将其作为 `ref` 或 `out` 参数传递。

```csharp
// using 语句
static IEnumerable<int> LoadNumbers_1(string filePath)
{
    var numbers = new List<int>();
    using (StreamReader reader = File.OpenText("numbers.txt"))  
    {
        string line;
        while ((line = reader.ReadLine()) is not null)
            if (int.TryParse(line, out int number))
                numbers.Add(number);
    }
    return numbers; 
}
// using 声明
static IEnumerable<int> LoadNumbers_2(string filePath)
{
    using StreamReader reader = File.OpenText(filePath); 
    var numbers = new List<int>();
    string line;
    while ((line = reader.ReadLine()) is not null)
        if (int.TryParse(line, out int number))
            numbers.Add(number);
    return numbers;
}
```

- 使用 `await using` 语句来正确使用 `IAsyncDisposable` 实例：在声明的局部变量离开被声明的作用域语句块时，将自动调用 `DisposeAsync` 方法释放该实例。`await using` 也可以使用拥有公共无参 `public async ValueTask DisposeAsync()` 方法的对象（不必是 `IAsyncDisposable` 接口实例）。

```csharp
await using (var resource = new AsyncDisposableExample())
{
    // Use the resource
}
class AsyncDisposableExample{
    public async ValueTask DisposeAsync() => Console.WriteLine("DisposeAsync...");
}

```

- 可以将 `using` 语句和声明与适用于可释放模式的 `ref` 结构的实例一起使用，该结构有一个实例 `public void Dispose()` 方法。

```csharp
int num = 1;
using (Sample s = new Sample(ref num))
{
    // ref 字段可以在 using 中修改，但其引用不可修改
    s.Value = 10;
    Console.WriteLine(num);
}
ref struct Sample
{
    public Sample(ref int val)
    {
        Value = ref val;
    }
    public ref int Value;
    public void Dispose() => Console.WriteLine("ref struct Disposed");
}
```

---