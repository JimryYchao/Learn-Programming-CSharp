## CSharp 程序结构

C# 中的关键组织结构概念包括程序、命名空间、类型、成员和程序集：
- 程序声明类型，而类型则包含成员，并被整理到命名空间中。
- 类型示例包括类、结构和接口。
- 成员示例包括字段、方法、属性和事件。
- 编译完的 C# 程序实际上会打包到程序集中。程序集的文件扩展名通常为 `.exe` 或 `.dll`，具体视其分别实现的是应用程序还是库而定。

程序集包含中间语言（IL）指令形式的可执行代码和元数据形式的符号信息。执行前，.NET 公共语言运行时的实时（JIT）编译器会将程序集中的 IL 代码转换为特定于处理器的代码。

由于程序集是包含代码和元数据的自描述功能单元，因此无需在 C# 中使用 `#include` 指令和头文件。只需在编译程序时引用特定的程序集，即可在 C# 程序中使用此程序集中包含的公共类型和成员。

---
### C# 程序的通用结构

C# 程序由一个或多个文件组成，每个文件均包含零个或多个命名空间。一个命名空间包含类、结构、接口、枚举、委托等类型或其他命名空间。

```csharp
// A skeleton of a C# program
using System;
namespace YourNamespace{
    class YourClass{}
    struct YourStruct{}
    interface IYourInterface{}
    delegate void YourDelegate();
    enum YourEnum{}
    namespace YourNestedNamespace{
        struct YourStruct{}
    }
    class Program{
        static void Main(string[] args){
            //Your program starts here...
            Console.WriteLine("Hello world!");
        }
    }
} 
```

---
### 程序构建基块
#### 成员

`class` 的成员要么是静态成员，要么是实例成员。静态成员属于类，而实例成员则属于对象（类实例）。类可以包含的成员类型有：常量、字段、方法、属性、索引器、事件、运算符、构造函数、终结器、嵌套类型等。

> 成员访问修饰

每个类成员都有关联的可访问性，用于控制能够访问成员的程序文本区域：
- `public`：访问不受限制。
- `private`：访问仅限于此类。
- `protected`：访问仅限于此类或派生自此类的类。
- `internal`：仅可访问当前程序集（`.exe` 或 `.dll`）。
- `protected internal`：仅可访问此类、从此类中派生的类，或者同一程序集中的类。
- `private protected`：仅可访问此类或同一程序集中从此类中派生的类。

<br>

#### 字段

字段是与类或类实例相关联的变量：
- 使用静态修饰符声明的字段定义的是静态字段。静态字段只指明一个存储位置，无论创建多少个类实例，永远只有一个静态字段副本。
- 不使用静态修饰符声明的字段定义的是实例字段。每个类实例均包含相应类的所有实例字段的单独副本。

```csharp
public class Color{
    public static readonly Color Black = new(0,0,0);  // 静态字段
    // 字段
    public byte R;
    public byte G;
    public byte B;
    public Color(byte r, byte g, byte b)    // 构造函数
    {
        R = r;
        G = g;
        B = b;
    }
}
```

<br>

#### 方法

方法是实现对象或类可执行的计算或操作的成员。静态方法是通过类进行访问。实例方法是通过类实例进行访问。

方法可能包含一个参数列表，这些参数表示传递给方法的值或变量引用。方法具有返回类型，它用于指定方法计算和返回的值的类型。如果方法未返回值，则它的返回类型为 `void`。
方法可能也包含一组类型参数，必须在调用方法时指定类型自变量（泛型方法）。

```csharp
public void Print(string message) => Console.WriteLine("DEBUG: " + message);
```

> 方法参数

参数用于将值或变量引用传递给方法。方法参数从调用方法时指定的自变量中获取其实际值。有四类参数：值参数、引用参数、输出参数和参数数组。

```csharp
void Func1(int i, double d){}           // 值参数
void Func2(ref int i, ref double d){}   // 引用参数
void Func3(out int i, out double d){}   // 输出参数
void Func4(int i, params double[] args){}   // 参数数组
```

> 方法主体和局部变量

方法主体指定了在调用方法时执行的语句。方法主体可以声明特定于方法调用的变量，此类变量称为局部变量。局部变量声明指定了类型名称、变量名称以及可能的初始值。
C# 要求必须先明确赋值局部变量，然后才能获取其值。方法使用 `return` 语句将控制权返回给调用方；对于 `void` 方法返回，`return` 可省略。

```csharp
int sum(int a, int b){
    int sum = a+b;
    return sum;
}
```

> 静态和实例方法

使用 `static` 修饰符声明的方法是静态方法。静态方法不对特定的实例起作用，只能直接访问静态成员。
未使用 `static` 修饰符声明的方法是实例方法。实例方法对特定的实例起作用，并能够访问静态和实例成员。

```csharp
class Entity{
    static int static_elem;
    int s_elem;
    // 静态方法调用静态成员
    public static int GetStaticElem() => static_elem;
    // 实例方法调用实例成员
    public int GetElem() => this.s_elem;
}
```

> 虚方法、重写方法和抽象方法

可使用虚方法、重写方法和抽象方法来定义类类型层次结构的行为：
- 虚方法是在基类中声明和实现的方法，其中任何派生类都可提供更具体的实现。
- 重写方法是在派生类中实现的方法，可修改基类实现的行为。
- 抽象方法是在基类中声明的方法，必须在所有派生类中重写。抽象方法不在基类中定义实现。

```csharp
abstract class Father
{
    // 抽象方法
    public abstract void BasePrint(string message);     
    // 基类中的虚方法
    public virtual void Print(string message) => Console.WriteLine("DEBUG : ");
}
class SonOfFather : Father
{
    // 重写抽象方法
    public override void BasePrint(string message) => base.Print(message);
    // 重写父类的虚方法
    public override void Print(string message) => Console.WriteLine("SON DEBUG : " + message);
}
```

<br>

#### 构造函数

C# 支持实例和静态构造函数。实例构造函数是实现初始化类实例所需执行的操作的成员。静态构造函数是实现在首次加载类时初始化类本身所需执行的操作的成员。

实例构造函数可重载并且可具有可选参数，并且不会被子类继承。如果没有为类提供实例构造函数，则会自动提供无参实例构造函数。

```csharp
class Program{
    static Program(){}   // 静态构造函数
    public Program(){}   // 无参实例构造函数
}
```

<br>

#### 属性

属性是字段的自然扩展，与字段不同的是，属性不指明存储位置。属性包含访问器，用于指定在读取或写入属性值时执行的语句。`get` 访问器读取该值，`set` 访问器写入该值。

C# 支持实例属性和静态属性。属性声明也可以包含 `virtual`、`abstract` 或 `override` 修饰符。

```csharp
class Point{
    public static Point Zero {get => new(0, 0, 0);}     // 静态属性
    public int X {get; set;}    
    public int Y {get; set;}
}
```

<br>

#### 索引器

索引器允许类或结构的实例就像数组一样进行索引。借助索引器成员，可以将对象编入索引。索引器类似于属性，索引器分为读写、只读和只写索引器，且索引器的访问器可以是 `virtual` 的。
索引器可被重载。一个类可声明多个索引器，只要其参数的数量或类型不同即可。

```csharp
class SampleCollection<T>
{
   // Declare an array to store the data elements.
   private T[] arr = new T[100];
   // Define the indexer to allow client code to use [] notation.
   public T this[int i]
   {
      get => arr[i];
      set => arr[i] = value;
   }
}
```

通过声明索引器，编译器会自动在对象上生成一个名为 `Item` 的属性，无法从实例成员访问表达式直接访问 `Item` 属性。如果在包含索引器的对象中添加自己的 `Item` 属性，则将收到 CS0102 编译器错误。要避免此错误，请使用 `IndexerNameAttribute` 来重命名索引器。

```csharp
struct SampleCollection<T>( T[] arr)
{
    [System.Runtime.CompilerServices.IndexerName("__Item")]
    public ref T this[int i] =>  ref arr[i];
    T Item { get; set; }
}
```

> 接口中的索引器

可以在接口上声明索引器。其访问器不使用修饰符，通常没有正文，用以指示索引器为读写、只读还是只写。

```csharp
interface ISampleDictionary
{
    string this[string key] { get ; set; }
}
```


<br>

#### 事件

借助事件成员，类或对象可以提供通知。事件的声明方式与字段类似，事件的声明中包括 `event` 关键字，且类型必须是委托类型。

在声明事件成员的类中，事件的行为与委托类型的字段完全相同（前提是事件不是抽象的，且不声明访问器）。客户端通过事件处理程序响应事件。使用 `+=` 和 `-=` 运算符分别可以附加和删除事件处理程序。

```csharp
public class Program
{
    static unsafe void Main(string[] args)
    {
        var e = EntityCreator.GetEntity();
    }
    class EntityCreator
    {
        static void print(Entity e) => Console.WriteLine(e.ToString() + " Created");
        static EntityCreator() => Entity.PrintEvent += print;  // 注册 create 事件
        public static Entity GetEntity() => new Entity();
    }
    class Entity
    {
        public static event Action<Entity> PrintEvent;
        public Entity() => PrintEvent?.Invoke(this);
    }
}
```

事件声明可以显式提供 `add` 和 `remove` 访问器。

```csharp
class Entity
{
    private static event Action<Entity> printEvent;
    public static event Action<Entity> PrintEvent
    {
        add => printEvent += value;
        remove => printEvent -= value;
    }
    public Entity() => printEvent?.Invoke(this);
}
```

<br>

#### 运算符

运算符是定义向类实例应用特定表达式运算符的含义的成员。可以定义三种类型的运算符：一元运算符、二元运算符和转换运算符（`implicit`、`explicit`）。所有运算符都必须声明为 `public` 和 `static`。

```c
public class Vector2(double x, double y)
{
    public double X { get => x; set => x = value; }
    public double Y { get => y; set => y = value; }
    // 二元运算符
    public static Vector2 operator +(Vector2 lhs, Vector2 rhs) => new(lhs.X + rhs.X, lhs.Y + rhs.Y);
    public static Vector2 operator -(Vector2 lhs, Vector2 rhs) => new(lhs.X - rhs.X, lhs.Y - rhs.Y);
}
public class Vector3(double x, double y, double z)
{
    public double X { get => x; set => x = value; }
    public double Y { get => y; set => y = value; }
    public double Z { get => z; set => z = value; }
    // 转换运算符
    public static implicit operator Vector2(Vector3 v3) => new Vector2(v3.X, v3.Y);
    public static explicit operator Vector3(Vector2 v2) => new Vector3(v2.X, v2.Y, 0);
}
public class Program
{
    static unsafe void Main(string[] args)
    {
        Vector3 p3 = new Vector3(1, 1, 1);
        Vector2 p2 = p3 + new Vector2(-1, -1);
        Console.WriteLine($"The new Point = ({p2.X}, {p2.Y})");  // The new Point = (0, 0)
    }
}
```

<br>

#### 终结器

终结器是实现完成类实例所需的操作的成员。通常，需要使用终结器来释放非托管资源。实例的终结器在垃圾回收期间自动调用。
垃圾回收器在决定何时收集对象和运行终结器时有很大自由度。具体而言，终结器的调用时间具有不确定性，可以在任意线程上执行终结器。

```csharp
class Program{
    ~Program(){
        // ...
    }
}
```

<br>

#### 表达式

表达式是在操作数和运算符的基础之上构造而成。表达式的运算符指明了向操作数应用的运算。运算符存在不同的优先级，可以使用括号控制优先级和结合性。

如果操作数两边的两个运算符的优先级相同，那么运算符的结合性决定了运算的执行顺序：
- 除了赋值运算符和 null 合并运算符之外，所有二元运算符均为左结合运算符，即从左向右执行运算。
- 赋值运算符、null 合并 `??` 和 `??=` 运算符和条件运算符 `?:` 为右结合运算符，即从右向左执行运算。

<br>

#### 语句

程序操作使用语句进行表示。可使用的语句类型有：局部变量声明、局部常量声明、表达式语句、`if` 语句、`switch` 语句、`while` 语句、`do` 语句、`for` 语句、`foreach` 语句、`break` 语句、`continue` 语句、`goto` 语句、`return` 语句、`yield` 语句、`throw` 和 `try` 语句、`checked` 和 `unchecked` 语句、`lock` 语句、`using` 语句。


---
### `Main` 方法

`Main` 方法是 C# 应用程序的入口点，库和服务不要求使用 `Main` 方法作为入口点。`Main` 方法是应用程序启动后调用的第一个方法。C# 程序中只能有一个入口点，如果多个类包含 `Main` 方法，必须使用 `StartupObject` 编译器选项来编译程序，以指定将哪个 `Main` 方法用作入口点。

```c
class Program{
    static void Main(string[] args){
        // Display the number of command line arguments.
        Console.WriteLine(args.Length);
    }
}
```

> `Main` 概述

- `Main` 方法是可执行程序的入口点，也是程序控制开始和结束的位置。
- `Main` 在类或结构中声明，且必须是 `static`，它不需要是 `public`。封闭类或结构不一定要是静态的。
- `Main` 的返回类型可以是 `void`、`int`、`Task` 或 `Task<int>`。当且仅当 `Main` 返回 `Task` 或 `Task<int>` 时，`Main` 的声明可包括 `async` 修饰符。这明确排除了 `async void Main` 方法。
- `Main` 方法的声明可使用或不使用包含命令行自变量的 `string[]` 参数。

```csharp
// 有效的 Main 签名
public static int Main() { }
public static async Task<int> Main() { }
public static int Main(string[] args) { }
public static async Task<int> Main(string[] args) { }
public static void Main() { }
public static async Task Main() { }
public static void Main(string[] args) { }
public static async Task Main(string[] args) { }
```

> `Main` 返回值

`Main` 方法若返回 `int` 或 `Task<int>`，可使程序将状态信息传递给调用可执行文件的其他程序或脚本。
声明 `Main` 的 `async` 返回值时，编译器会生成样本代码，用于调用 `Main` 中的异步方法。如果未指定 `async` 关键字，则需要自行编写该代码，如以下示例所示。示例中的代码可确保程序一直运行，直到异步操作完成：

```csharp
public static void Main(){
    AsyncConsoleWork().GetAwaiter().GetResult();
}
private static async Task<int> AsyncConsoleWork(){
    // Main body here
    return 0;
}
```

该样本代码可替换为：

```csharp
static async Task<int> Main(string[] args){
    return await AsyncConsoleWork();
}
```

将 `Main` 声明为 `async` 的优点是，编译器始终生成正确的代码。当应用程序入口点返回 `Task` 或 `Task<int>` 时，编译器生成一个新的入口点，该入口点调用应用程序代码中声明的入口点方法。假设此入口点名为 `$GeneratedMain`，编译器将为这些入口点生成以下代码：

- `static Task Main()` 导致编译器生成 `private static void $GeneratedMain() => Main().GetAwaiter().GetResult();` 的等效项。
- `static Task Main(string[])` 导致编译器生成 `private static void $GeneratedMain(string[] args) => Main(args).GetAwaiter().GetResult();` 的等效项。
- `static Task<int> Main()` 导致编译器生成 `private static int $GeneratedMain() => Main().GetAwaiter().GetResult();` 的等效项。
- `static Task<int> Main(string[])` 导致编译器生成 `private static int $GeneratedMain(string[] args) => Main(args).GetAwaiter().GetResult();` 的等效项。

---
### 命令行自变量

`Main` 方法的参数是一个表示命令行参数的 `String` 数组。 通常，通过测试 `args.Length > 0` 来确定参数是否存在。

```csharp
public class Functions
{
    // 计算阶乘
    public static long Factorial(int n)
    {
        if ((n < 0) || (n > 20))
            return -1;
        long tempResult = 1;
        for (int i = 1; i <= n; i++)
            tempResult *= i;
        return tempResult;
    }
}
class MainClass
{
    static int Main(string[] args)
    {
        // 测试是否有输入命令行参数
        if (args.Length == 0)
        {
            Console.WriteLine("Please enter a numeric argument.");
            Console.WriteLine("Usage: Factorial <num>");
            return 1;
        }

        int num;      // 尝试转换参数
        bool test = int.TryParse(args[0], out num);
        if (!test)
        {
            Console.WriteLine("Please enter a numeric argument.");
            Console.WriteLine("Usage: Factorial <num>");
            return 1;
        }

        // 计算阶乘
        long result = Functions.Factorial(num);
        if (result == -1) Console.WriteLine("Input must be >= 0 and <= 20.");
        else Console.WriteLine($"The Factorial of {num} is {result}.");
        return 0;
    }
}
/* powershell: MainClass.exe 3 */
// If 3 is entered on command line, the output reads:  The factorial of 3 is 6.
```

---
### 顶级语句

从 C# 9 开始，无需在控制台应用程序项目中显式包含 `Main` 方法，可以使用顶级语句功能最大程度地减少必须编写的代码。在这种情况下，编译器将为应用程序生成类和 `Main` 方法入口点。
可以使用顶级语句功能最大程度地减少必须编写的代码。在这种情况下，编译器将为应用程序生成类和 `Main` 方法入口点。

可以显式编写 `Main` 方法，但它不能作为入口点。在具有顶级语句的项目中，不能使用 `-main` 编译器选项来选择入口点，即使该项目具有一个或多个 `Main` 方法。

> `using` 指令

- `using` 指令必须出现顶级语句之前，而顶级语句隐式位于全局命名空间中。
- 具有顶级语句的文件还可以包含命名空间和类型定义，但它们必须位于顶级语句之后。

```csharp
using System.Text;
// 顶级语句
StringBuilder builder = new();
builder.AppendLine("Hello");
builder.AppendLine("World!");
Console.WriteLine(builder.ToString());
```

> `args`

- 顶级语句可以引用 `args` 变量来访问输入的任何命令行参数。`args` 变量永远不会为 null，但如果未提供任何命令行参数，则其 `Length` 将为零。

```csharp
if(args.Length > 0)
     foreach (var arg in args)
        Console.WriteLine($"Argument={arg}");
else Console.WriteLine("No arguments");
```

> `await`

- 顶级语句中可以通过使用 `await` 来调用异步方法。

```csharp
Console.Write("Hello ");
await Task.Delay(5000);
Console.WriteLine("World!");
```

> 进程的退出代码

- 可以在顶级语句中使用 `return` 语句。

```csharp
Console.WriteLine("Hello World!");
return 0;
```

> 隐式入口点方法

编译器会生成一个方法，作为具有顶级语句的项目的程序入口点。方法的签名取决于顶级语句是包含 `await` 关键字还是 `return` 语句。

```csharp
// await 和 return
static async Task<int> GeneratedMain(string[] args){}
// await
static async Task GeneratedMain(string[] args){}
// return
static int GeneratedMain(string[] args){}
// 非异步无返回
static void GeneratedMain(string[] args){}
```
