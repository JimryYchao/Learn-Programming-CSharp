# CSharp 基本概念

---
## 程序结构

C# 中的组织结构概念包括程序、命名空间、类型、成员和程序集：程序声明类型，类型包含成员并被整理到命名空间中，成员可包含常量、字段、方法、属性、索引器、事件、运算符、构造函数、终结器、嵌套类型等。编译完的程序打包到程序集中，程序集的文件扩展名一般为 `*.exe`（应用程序）或 `*.dll`（库）。
 
程序集包含中间语言（IL）指令形式的可执行代码和元数据形式的符号信息。执行前，.NET 公共语言运行时的实时（JIT）编译器会将程序集中的 IL 代码转换为特定于处理器的代码。

由于程序集是包含代码和元数据的自描述功能单元，只需在编译程序时引用特定的程序集，即可在程序中使用此程序集中包含的公共类型和成员。

> C# 程序的通用结构

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

>---

### 应用启动与终止

一个程序可以编译为一个类库，作为其他应用程序的一部分使用，也可以是一个可以直接启动的应用程序。当应用程序运行时，将创建一个新的应用程序域 "*Application Domain*"。同一台计算机上可以同时存在多个不同的应用程序实例，每个实例都有自己的应用程序域。

应用程序域通过充当应用程序状态的容器来实现应用程序隔离，用以隔离应用程序及其使用的类库中定义的类型。加载到一个应用程序域中的类型不同于加载到另一个应用程序域中的相同类型，并且对象的实例不能在应用程序域中直接共享。每个应用程序域都有自己类型的静态变量副本，并且每个应用程序域最多运行一次类型的静态构造函数。

作为应用程序编译的程序应包含至少一个符合规范要求的方法作为入口点：该方法以 `Main` 命名的非泛型类型的静态声明，不能是泛型方法，也不能在泛型类型中声明。`Main` 方法是可执行程序的入口点，也是程序控制开始和结束的位置。

>---

### Main 方法

`Main` 方法是 C# 应用程序的入口点，库和服务不要求定义程序入口点。`Main` 方法是应用程序启动后调用的第一个方法。程序中只能有一个入口点，如果多个类包含 `Main` 方法，必须使用 `StartupObject` 编译器选项来告知程序，将指定哪个 `Main` 方法用作应用程序入口点。

```csharp
class Program
{
    static void Main(string[] args)
    {
        // Display the number of command line arguments.
        Console.WriteLine(args.Length);
    }
}
```

`Main` 方法可定义为常规方法并返回类型 `void`、`int`，或定义为 `async` 异步方法并返回 `Task` 或 `Task<int>`。若返回 `int` 或 `Task<int>`，可使程序将状态信息传递给调用可执行文件的其他程序或脚本。
`Main` 方法的声明中，可选包含命令行自变量 `string[] args` 参数。可使用 `System.Environment.GetCommandLineArgs();` 方法获取命令行参数。与 C 和 C++ 不同，程序的名称不被视为 `args` 数组中的第一个命令行实参，但它是 `GetCommandLineArgs()` 方法中的第一个元素。 

```csharp
class Program
{
    static void Main(string[] args)
    {
        var E_args = Environment.GetCommandLineArgs();
        if (args.Length > 0)
            Console.WriteLine("args[0] = " + args[0]);
        Console.WriteLine("E_args[0] = " + E_args[0]);
    }
}
```

> 有效的 Main 签名

```csharp
public static void Main() { }
public static int Main() { }
public static void Main(string[] args) { }
public static int Main(string[] args) { }

public static async Task Main() { }
public static async Task<int> Main() { }
public static async Task Main(string[] args) { }
public static async Task<int> Main(string[] args) { }
```

> Async Main

声明 `async Main` 返回值时，编译器会生成样本代码，用于调用 `Main` 中的异步方法。如果未指定 `async` 关键字，则需要自行编写该代码，如以下示例所示。示例中的代码可确保程序一直运行，直到异步操作完成：

```csharp
public static void Main()
{
    AsyncConsoleWork().GetAwaiter().GetResult();
}
private static async Task<int> AsyncConsoleWork()
{
    // Main body here
    return 0;
}
```

该示例代码可替换为：

```csharp
static async Task<int> Main(string[] args)
{
    return await AsyncConsoleWork();
}
private static async Task<int> AsyncConsoleWork()
{
    // main body here 
    return 0;
}
```

将 `Main` 声明为 `async` 的优点是，编译器始终生成正确的代码。当应用程序入口点返回 `Task` 或 `Task<int>` 时，编译器生成一个新的入口点，该入口点调用应用程序代码中声明的入口点方法。假设此入口点名为 `$GeneratedMain`，编译器将为这些入口点生成以下代码：

```csharp
static Task Main(){}  // 生成等效项 => 
private static void $GeneratedMain() 
    => Main().GetAwaiter().GetResult();

static Task Main(string[] args){} // 生成等效项 =>
private static void $GeneratedMain(string[] args) 
    => Main(args).GetAwaiter().GetResult();

static Task<int> Main(){} // 生成等效项 =>
private static int $GeneratedMain() 
    => Main().GetAwaiter().GetResult();

static Task<int> Main(string[] args){} // 生成等效项 =>
private static int $GeneratedMain(string[] args) 
    => Main(args).GetAwaiter().GetResult();
```

>---

### 命令行自变量

`Main` 方法的参数是一个表示命令行参数的 `String` 数组，`args` 始终不为 `null`。通常，通过测试 `args.Length > 0` 来确定参数是否存在。

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

>---

### 顶级语句

从 C# 9 开始，无需在控制台应用程序项目中显式包含 `Main` 方法，可以使用顶级语句功能最大程度地减少必须编写的代码。在这种情况下，编译器将自动为应用程序生成类和 `Main` 方法入口点。

一个应用程序只能有一个入口点，一个项目只能有一个包含顶级语句的文件。

可以显式编写 `Main` 方法，但它不能作为入口点。在具有顶级语句的项目中，不能使用 `-main` 编译器选项来选择入口点，即使该项目具有一个或多个 `Main` 方法。

`using` 指令必须出现顶级语句之前，而顶级语句隐式位于全局命名空间中。具有顶级语句的文件还可以包含命名空间和类型定义，但它们必须位于顶级语句之后。

```csharp
using System.Text;
// 顶级语句
StringBuilder builder = new();
builder.AppendLine("Hello");
builder.AppendLine("World!");
Console.WriteLine(builder.ToString());
Console.WriteLine(new Point(0, 0));
// 类型声明
record Point(int x, int y);
```

> `args`

顶级语句可以引用 `args` 变量来访问输入的任何命令行参数。`args` 变量永远不会为 `null`，但如果未提供任何命令行参数，则其 `Length` 将为零。

```csharp
if (args.Length > 0)
    Console.WriteLine($"Argument={string.Join(", ", args)}");
else Console.WriteLine("No arguments");
Console.WriteLine(Environment.GetCommandLineArgs()[0]);
```

> `await`

顶级语句中可以通过使用 `await` 来调用异步方法。

```csharp
Console.Write("Hello ");
await Task.Delay(5000);
Console.WriteLine("World!");
```

> 进程的退出代码

可以在顶级语句中使用 `return` 语句。

```csharp
Console.WriteLine("Hello World!");
return 0;
```

> 隐式入口点方法

编译器会生成一个方法，作为具有顶级语句的项目的程序入口点。方法的签名取决于顶级语句是包含 `await` 关键字还是 `return` 语句。

```csharp
// 包含 await 和 return
static async Task<int> GeneratedMain(string[] args){}
// 包含 await
static async Task GeneratedMain(string[] args){}
// 包含 return
static int GeneratedMain(string[] args){}
// 非异步无返回
static void GeneratedMain(string[] args){}
```

>---

### 终止应用

应用程序终止时将控制权返回到执行环境。当 `Main` 方法返回 `int` 或 `Task<int>` 时，若程序执行完没有导致异常，则返回 `int` 的值用作应用程序的终止状态码，用以表示向执行环境传递应用程序成功或失败的通信。

如果有效入口点方法的返回类型为 `void` 或 `Task`，并且执行完成后没有导致异常，则终止状态码为 `0`。

如果有效入口点方法由于异常而终止，则退出代码是特定于实现的。终结器 `finalizer` 是否作为应用程序终止的一部分运行是具体实现的。.NET 框架实现尽一切合理的努力为所有尚未被垃圾收集的对象调用终结器，除非这种清理已经被抑制（通过调用库方法 `GC.SuppressFinalize`）。

>---
### 托管执行和 CIL

处理器不能直接解释程序集，程序集使用公共中间语言（*Common intermediate Language*，CIL）或中间语言 IL。C# 编译器将 C# 源代码文件转换成中间语言。将 CIL 代码转换成处理器能理解的机器码，还需要在 VES（*Virtual Execution System*，虚拟执行系统），运行时（*runtime*）中按需即时编译（*just-in-time compilation*，JIT 编译）。代码在 “运行时” 的上下文中执行，就称为托管代码 （*managed code*），在 “运行时” 的控制下执行的过程则称为托管执行 （*managed execution*）。

“托管” 是指 “运行时” 管理程序的内存分配、安全性和 JIT 编译等方面，从而控制了主要的程序行为。而执行时不需要 “运行时” 的代码称为本机代码（*native code*）或非托管代码 （*unmanaged code*）。

“运行时” 规范包含在一个包容面更广的规范中，即 CLI（*Common Language Infrastructure*，公共语言基础结构 ）规范：
- CIL（*Common intermediate Language*）公共中间语言，又称 MSIL，简写为 IL。
- BCL（*Base Class Library*）基类库，属于 FCL 的一个子集。
- FCL（*Framework Class Library*）框架类库，内层由大部分 BCL 组成，主要对 .NET 框架、运行时和 CIL 语言提供支持（预定义类型、集合类型、线程处理、应用程序域、运行时、安全性、互操作等）；中间层是包含了对操作系统功能的封装（文件系统、网络连接、图形图像、XML 操作等）；最外层是包含各种类型的应用程序（Windows Forms、ASP.NET、WPF、WCF、WF 等）。
- CTS（*Common Type System*）公共类型系统，
- CLS（*Common Language Specification*）公共语言规范，
- CLR（*Common Language Runtime*）公共语言运行时，也叫 VES（*Virtual Execution System*，虚拟执行系统），CLR 是 .NET 程序集的运行环境。
- 元数据包括程序集的布局或文件格式规范。
  
CIL 在 CLR 执行引擎的上下文中运行，其中运行的主要服务和功能包括：
- 语言互操作性：不同源语言间的互操作性。语言编译器将每种源语言转换成相同中间语言 CIL 来实现这种互操作性。
- 类型安全：检查类型间转换，确保兼容的类型才能相互转换。这有助于防范缓冲区溢出。
- 代码访问安全性：程序集开发者的代码有权在计算机上执行的证明。
- 垃圾回收：一种内存管理机制，自动释放 CLR 为数据分配的空间。
- 平台可移植性：同一程序集可在多种操作系统上运行。
- BCL（基类库）：提供开发者能（在所有 .NET 框架中）依赖的大型代码库。


---
## 声明

C# 程序中的声明定义程序的构成元素。C# 使用命名空间对相关程序元素进行逻辑分组。命名空间可以包含类型声明和嵌套命名空间声明。

类型声明用于定义类、结构、接口、枚举，以及委托。类型声明中允许的成员种类取决于类型声明的形式：类声明可以包含常量、字段、方法、属性、事件、索引器、运算符、实例构造函数、静态构造函数、终结器和嵌套类型。

> 声明限制

声明在其所属的名称空间中定义一个名称。在名称空间中引入同名成员的两个或多个声明会造成编译时错误，但以下情况除外：
- 允许在同一名称空间中使用两个或多个具有相同名称的命名空间。这些相同名称的命名空间被聚合成一个逻辑命名空间，并共享一个名称空间。
- 在不同的程序中，在相同的命名空间中的声明可以共享相同的名称。但是，如果将这些声明包含在同一个应用程序中，可能会引入歧义。
- 允许在同一名称空间中使用两个或多个具有相同名称但不同签名的方法（方法重载）。允许在同一名称空间中使用两个或多个具有相同名称但类型形参数量不同的类型声明（泛型类型）。
- 在同一名称空间中带有 `partial` 修饰符的两个或多个类型声明可以共享相同的名称、相同数量的类型参数和相同的类型定义（类、结构或接口）。在这种情况下，类型声明构成了一个类型，并且它们自己被聚合成一个单独的名称空间。
- 同一名称空间中的命名空间声明和类型声明可以共享相同的名称，只要类型声明至少有一个类型形参。

> 声明空间分类

- 在程序的所有编译单元中，不包含在某个命名空间下的声明包含在全局名称空间（global declaration space）内，全局名称空间下的成员被聚合在全局命名空间内，可以通过别名 `global` 和 `::` 访问。
- 在程序的所有编译单元中，所有相同完全限定名称的命名空间中的成员声明隶属于该名称的单个逻辑命名空间。
- 每个编译单元和命名空间内都有一个别名（`using`、`extern alias`）名称空间。
- 每个非分部类、非分部结构和非分部接口声明都将创建一个新的名称空间。除了构造函数以外，类或结构不能包含与类或结构同名的成员声明，但允许声明具有相同名称不同签名的方法或运算符重载。基类不会占用派生类的名称空间，基接口不会占用派生接口的名称空间，因此允许派生类或接口声明与继承成员同名的成员，这类声明的成员将隐藏具有相同名称的继承成员。
- 每个委托、枚举声明都将创建一个新的名称空间。
- 每个方法声明、属性声明、访问器声明、索引器声明、运算符声明、构造函数声明、匿名函数和局部函数声明都会创建一个新的局部变量名称空间。局部变量名称空间可以嵌套，但是无法在局部和嵌套的名称空间内包含同名的局部声明。
- 每个块 `{ }` 或 `switch-case` 区间、`for`、`foreach`、`using` 等语句都将为局部变量和本地常量创建局部变量名称空间。 
- 声明名称的文本顺序通常没有意义。例如对于命名空间、常量、方法、属性、事件、索引器、操作符、实例构造函数、终结器、静态构造函数和类型的声明和使用，文本顺序并不重要。声明顺序的重要性体现在：
  - 字段名称的声明顺序决定了它们的初始化式执行的顺序；
  - 局部变量应该在使用之前定义；
  - 当省略关联常数值时，枚举成员的声明顺序很重要。

```csharp
class ClassA
{
    void F()
    {
        int i = 0;
        if (true)
        {
            int i = 1;  // err: i 在外部块声明，不能在嵌套块中重新声明
        }
    }
    void G()
    {
        if (true)
        {
            int i = 0;  // err: i 在外部块声明，不能在嵌套块中重新声明
        }
        int i = 1;
    }
    void H()
    {
        if (true)
        {
            int i = 0;
        }
        if (true)
        {
            int i = 1;
        }
    }
    void I()
    {
        for (int i = 0; i < 10; i++)
            H();
        for (int i = 0; i < 10; i++) // i 在单独的非嵌套块中声明
            H();
    }
}
```

> 完全限定名称

每个命名空间和类型都有一个完全限定的名称，该名称唯一地标识命名空间或类型。成员 N 的完全限定名是从全局空间开始的标识符的完整层次结构路径，命名空间或类型的完全限定名称始终是唯一的。

```csharp
class A { }                // A
namespace X                // X
{
    class B                // X.B
    {
        class C { }         // X.B.C
    }
    namespace Y            // X.Y
    {
        class D { }         // X.Y.D
    }
}
namespace X.Y              // X.Y
{
    class E { }             // X.Y.E
    class G<T>             // X.Y.G<>
    {
        class H { }         // X.Y.G<>.H
    }
    class G<S, T>           // X.Y.G<,>
    {
        class H<U> { }      // X.Y.G<,>.H<>
    }
}
```

---
## 类型与变量

类型定义 C# 中的任何数据的组织结构和行为。类型的声明可以包含其成员、基类型、它实现的接口和该类型的行为。变量是用于引用特定类型的实例的标签。标识符是分配给类型（类、接口、结构、记录、委托或枚举）、成员、变量或命名空间的名称，是不包含任何空格的 Unicode 字符序列。

C# 是一种强类型语言，每个变量和常量、求值的表达式、方法形参与返回值等都有一个类型。.NET 类库定义了内置数值类型和表示各种构造的复杂类型，其中包括文件系统、网络连接、对象的集合和数组以及日期。

类型中可存储的信息有：类型变量所需的存储空间、可以表示的最大值和最小值、包含的成员（方法、字段、事件等）、继承的基类型、它实现 的接口、允许执行的运算种类等。编译器使用类型信息来确保在代码中执行的所有操作都是类型安全的。编译器将类型信息作为元数据嵌入可执行文件中，公共语言运行时（CLR）在运行时使用元数据，以在分配和回收内存时进一步保证类型安全性。

C# 有两种类型：值类型和引用类型。值类型的变量直接包含它们的数据，每个变量都有自己的数据副本，对一个变量执行的运算不会影响到另一个变量。引用类型的变量存储对数据（称为 “对象”）的引用，两个变量可以引用同一个对象，对一个变量执行的运算可能会影响另一个变量引用的对象。值类型在堆栈上存储数据，而引用类型在堆上分配内存。

C# 的值类型进一步分为：简单类型、枚举类型、结构类型、可以为 null 的值类型和元组值类型。C# 引用类型又细分为类类型、接口类型、数组类型和委托类型。

> C# 的值类型可分为：

- 简单类型：
  - 有符号整数类型：`sbyte`、`short`、`int`、`long`、`nint`。
  - 无符号整数类型：`byte`、`ushort`、`uint`、`ulong`、`nuint`。
  - Unicode 字符类型：`char`，表示 UTF-16 代码单元。
  - IEEE 二进制浮点数类型：`float`、`double`。
  - 高精度十进制浮点数类型：`decimal`。
  - 布尔类型：`bool`。
- 枚举类型：格式为 `enum E {...}` 的用户定义类型。`enum` 类型是一种包含已命名常量的独特类型。
- 结构类型：格式为 `struct S {...}` 的用户定义类型。`record struct` 为记录结构类型。
- 可以为 null 的值类型：值为 `null` 的其他所有值类型的扩展。
- 元组类型：格式为 `(T1,T2,...)` 的用户定义类型。

> C# 引用类型可分为：

- 类类型：
  - 其他所有类型的最终基类：`object`。
  - Unicode 字符串：`string`，表示 UTF-16 代码单元序列。
  - 格式为：`class C {...}` 的用户定义类型。`record` 为记录类型。
- 接口类型：格式为 `interface I {...}` 的用户定义类型。
- 数组类型：一维、多维和交错数组。例如 `int[]`、`int[,]`、`int[][]`。
- 委托类型：格式为 `delegate TResult D(...)` 的用户定义类型。

> 用户定义类型

C# 程序使用类型声明创建新类型。用户可定义以下 C# 基本类型：类类型、结构类型、接口类型、枚举类型和委托类型。还可以声明 `record` 类型（`record struct` 或 `record class`）。

- `class` ：类类型定义包含数据成员（字段）和函数成员（方法、属性、构造函数等）的数据结构。类支持单一继承和多态。类对象在堆上分配内存，并受垃圾回收器的管理。
- `struct`：结构类型定义包含数据成员和函数成员的结构，与类相似。结构属于堆栈分配对象，不受垃圾回收器的管理。结构不支持用户指定的继承，可以实现接口。
- `interface`：接口类型将协定定义为一组已命名的公共成员。实现 `interface` 的 `class` 和 `struct` 必须提供接口成员的实现代码。接口和类、结构都可以实现多个接口。。
- `delegate`：委托类型表示引用包含特定参数列表和返回类型的方法。通过委托，可以将方法组视为变量并可作为参数传递的实体。委托类同于函数式语言提供的函数类型或函数指针概念，但是委托是面向对象且类型安全。
- `record`：记录类型用类提供封装数据的内置功能。可以声明为 `record` 引用类型或 `record struct` 值类型。记录类型具有编译器合成成员，主要用于存储值，关联行为最少。
- `enum`：枚举类型是由基础整型数值类型的一组命名常量定义的值类型。可以显式指定关联的常数值。

```csharp
class MyClass{}
struct MyStruct{}
interface IMyInterface{}
enum MyEnum{}
record class MyClass_Record{}
record struct MyStruct_Record{}
```

> 变量

C# 有多种变量，其中包括字段、数组元素、局部变量和参数。变量表示存储位置，每个变量都具有一种类型，用于确定可以在变量中存储哪些值：

- 不可为 null 的值类型：具有精确类型的值。
- 可以为 null 的值类型：`null` 值或具有精确类型的值。
- `object`：`null` 引用、对任意引用类型的对象的引用，或对任意值类型的装箱值的引用。
- 类类型：`null` 引用、对类类型实例的引用，或对派生自类类型的类实例的引用。
- 接口类型：`null` 引用、对实现接口类型的类类型实例或值类型的装箱值的引用。
- 数组类型：`null` 引用、对数组类型实例的引用，或对兼容的数组类型实例的引用。
- 委托类型：`null` 引用、对兼容的委托类型实例的引用。
- 记录类型：引用记录包含 `null` 引用或记录类型实例的引用，结构记录包含结构记录实例的值。

>---

### 在变量声明中指定类型

当在程序中声明变量或常量时，必须指定其类型或使用 `var` 关键字让编译器推断类型。声明变量后，不能使用新类型重新声明该变量，并且不能分配与其声明的类型不兼容的值，但是可以将值转换为其他类型。

```csharp
// Declaration only:
float temperature;
string name;
MyClass myClass;

// Declaration with initializers (four examples):
char firstLetter = 'C';
var limit = 3;
int[] source = { 0, 1, 2, 3, 4, 5 };
var query = from item in source
            where item <= limit
            select item;
```

>---

### .NET 内置类型

C# 提供了一组标准的内置类型。这些类型表示整数、浮点值、布尔表达式、文本字符、十进制值和其他数据类型。还有内置的 `string` 和 `object` 类型。

- 内置值类型：`bool`、`byte`、`sbyte`、`char`、`decimal`、`double`、`float`、`int`、`uint`、`nint`、`nuint`、`long`、`ulong`、`short`、`ushort`。
- 内置引用类型：`object`、`string`、`dynamic`。
- 无类型：`void`。

```csharp
// 整数
byte b = 1;
int i = 0xffff;  // 十六进制
// 浮点值
double d = 1.0;
float f = 1.0f;
// 布尔
bool state = true;
// 文本字符
char ch = 'C';
// 十进制值
decimal digit = 1980;
// 字符串
string str = "Hello world";
// object
object obj = null;
```

>---

### 非托管类型

- 如果某个类型是以下类型之一，则它是非托管类型：
  - `sbyte`、`byte`、`short`、`ushort`、`int`、`uint`、`long`、`ulong`、`nint`、`nuint`、`char`、`float`、`double`、`decimal` 或 `bool`。
  - 任何枚举类型，任何指针类型，任何仅包含非托管类型字段的用户定义的结构类型。

* 非托管类型使用 `unmanaged` 约束指定：类型参数为 “非指针、不可为 null 的非托管类型”。

```csharp
public struct Coords<T> where T : unmanaged
{
    public T X;
    public T Y;
}
```

>---

### .NET 通用类型系统

对于 .NET 中的类型系统，它支持继承原则。类型可以派生自其他类型（称为基类型）。派生类型继承（有一些限制）基类型的方法、属性和其他成员。所有类型派生自单个基类型，即 `System.Object (C# keyword: object)`。这样的统一类型层次结构称为通用类型系统（CTS）。

CTS 中的每种类型被定义为值类型或引用类型。这些类型包括 .NET 类库中定义的所有类型以及用户定义类型。使用 `struct` 关键字定义的类型是值类型；所有内置数值类型都是 `struct`。使用 `class` 或 `record` 关键字定义的类型是引用类型。

> 类、结构或记录声明类似于一张蓝图，用于在运行时创建实例或对象：

- 类是引用类型，创建类型的对象后，向其分配对象的变量仅保留对相应内存的引用（将对象引用分配给新变量后，新变量会引用原始对象）。
- 结构是值类型，创建结构时，向其分配结构的变量保留结构的实际数据（将结构分配给新变量时，会复制结构）。
- 记录类型可以是引用类型（`record` 或 `record class`）或值类型（`record struct`）

> 值类型

值类型派生自 `System.ValueType`（派生自 `System.Object`）。值类型变量直接包含其值，结构的内存在声明变量的任何上下文中进行内联分配。对于值类型变量，没有单独的堆分配或垃圾回收开销。

值类型分为 `struct` 和 `enum` 两类。内置的数值类型是结构，使用 `struct` 关键字可以创建用户自定义值类型，无法从任何值类型派生类型。`enum` 枚举定义的是一组已命名的整型常量，所有枚举从 `System.Enum`（继承自 `System.ValueType`）继承。


> 引用类型

定义为 `class`、`record class`、`delegate`、数组或 `interface` 的类型是引用类型。引用类型在声明时包含 null 值，直到用户为其分配该类型的实例或使用 `new()` 创建一个。无法使用 `new` 运算符直接实例化 `interface`，只能分配实现接口的派生实例。所有数组都是引用类型，即使元素是值类型，也不例外。数组隐式派生自 `System.Array` 类。

创建对象时，会在托管堆上分配内存。变量只保留对对象位置的引用。对于托管堆上的类型，在分配内存和回收内存时都会产生开销。“垃圾回收” 是 CLR 的自动内存管理功能，用于执行资源管理和内存回收。

引用类型完全支持继承。创建类时，可以从其他任何接口或非密封类继承。派生类可以从基类中继承并替代虚拟方法。

>---

### 泛型类型

可使用一个或多个类型参数声明的类型，用作实际类型（具体类型）的占位符。客户端代码在创建类型实例时提供具体类型，这种类型称为泛型类型。例如，.NET 类型 `System.Collections.Generic.List<T>` 具有一个类型参数 `T`，当创建类型的实例时，必须指定列表将包含的对象的类型 `T`，例如 `string`：

```csharp
List<string> stringList = new List<string>();
stringList.Add("String example");
// compile time error adding a type other than a string:
stringList.Add(4);
```

可以声明泛型类、泛型结构、泛型记录、泛型接口、泛型方法和泛型委托。通过使用类型参数，可重用同一类型以保存任意类型的元素，且无需将每个元素转换为对象。泛型集合类称为强类型集合，因为编译器知道集合元素的具体类型，并能在编译时检查或引发错误。

```csharp
public class Pair<TFirst, TSecond> // <,> 类型参数列表
{
    public TFirst First { get; }
    public TSecond Second { get; }
    public Pair(TFirst first, TSecond second) => 
        (First, Second) = (first, second);
}
```

使用泛型类时，必须为每个类型参数提供类型自变量。

```csharp
{
    var pair = new Pair<int, string>(1, "two");
    int i = pair.First;     //TFirst int
    string s = pair.Second; //TSecond string
}
```


>---

### 隐式类型、匿名类型和可以为 null 的值类型

> 隐式类型

可以使用 `var` 关键字隐式键入一个局部变量（但不是类成员）。隐式类型仍可在编译时获取类型，但类型是由编译器提供。

```csharp
var list = new List<int> { 1, 2, 3, 4, 5, 6 };   // List<int>
var d_num = 3.1415;     // double
var func = mess => Console.WriteLine(mess);     // Action<string>
```

> 匿名类型

可结合使用 `new` 运算符和对象初始值设定项创建匿名类型。匿名类型提供了一种便捷的方法，可用来将一组只读属性封装到单个对象中，而无需预定义一个类型。

```csharp
var Person = new { FirstName = "John", LastName = "Ychao" };
var anonArray = new[] {
    new { name = "apple", diam = 4 },
    new { name = "grape", diam = 1 }
};
```

> 可以为 null 的值类型

普通值类型不能具有 `null` 值，可以在类型后面追加 `?`，创建可为空的值类型。例如，`int?` 是还可以包含值 `null` 的 `int` 类型。可以为 `null` 的值类型是泛型结构类型 `System.Nullable<T>` 的实例。 

```csharp
int? val = null, val2 = 10;
if (val.HasValue)  // false
    Console.WriteLine(val.Value);   
Console.WriteLine(val2 ?? 0);  // 10   
```

>---

### 编译时类型和运行时类型

变量可以具有不同的编译时和运行时类型。编译时类型是源代码中变量的声明或推断类型；运行时类型是该变量所引用的实例的类型。

```csharp
// 运行时和编译时类型相同
string message = "This is a string of characters";   // 编译时和运行时都是 string

// 运行时和编译时类型不同
object anotherMessage = "This is another string of characters";     // 编译时为 object，运行时为 string
IEnumerable<char> someCharacters = "abcdefghijklmnopqrstuvwxyz";    // 编译时为 IEnumerable<char>，运行时为 string
```

---
## 成员与可访问性

命名空间和类型拥有成员，这些成员通常通过使用限定名和成员访问符 `.` 来获得。类型的成员可以在类型定义中声明，也可以从类型的基类继承。基类的所有成员（除构造函数、终结器外）将成为派生类型的成员。成员的可访问性不会控制是否继承成员，只会限制派生类型对该成员的可访问性。也可以通过声明同名成员隐藏继承的成员。

>---
### 类型构建成员

#### 命名空间成员

没有在封闭命名空间的命名空间声明或类型声明是全局命名空间的成员，可以显式 `global::` 进行访问成员。命名空间中声明的命名空间和类型是该命名空间的成员。命名空间没有任何访问限制，其他成员类型可以添加可访问性。 

#### 结构成员

结构体的成员由在结构中声明的成员和从结构体的直接基类 `System.ValueType` 和间接基类 `System.Object` 继承的成员构成。

简单类型的成员直接对应于由简单类型别名的结构类型的成员：
- `sbyte` 对应于 `System.Sbyte`。
- `byte` 对应于 `System.Byte`。
- `short` 对应于 `System.Int16`。
- `ushort` 对应于 `System.UInt16`。
- `int` 对应于 `System.Int32`。
- `uint` 对应于 `System.UInt`。
- `long` 对应于 `System.Int64`。
- `ulong` 对应于 `System.UInt64`。
- `char` 对应于 `System.Char`。
- `float` 对应于 `System.Single`。
- `double` 对应于 `System.Double`。
- `decimal` 对应于 `System.Decimal`。
- `bool` 对应于 `System.Boolean`。
- `nint` 对应于 `System.IntPtr`。
- `nuint` 对应于 `System.UIntPtr`。

#### 枚举成员

枚举的成员是枚举中声明的常量，以及从枚举的直接基类 `System.Enum` 和间接基类 `System.ValueType` 和 `object` 继承的成员。

#### 类成员

类的成员是在类中声明的成员，并且从基类中继承的成员（除了 `object` 没有基类）的类外。从基类继承的成员包括常量、字段、方法、属性、事件、索引器、运算符、嵌套类型和基类的类型，但不包括基类的实例构造函数、终结器和静态构造函数。

类中可以包含常量、字段、方法、属性、事件、索引器、运算符、实例构造函数、析构函数、静态构造函数和嵌套类型的声明。

`string` 和 `object` 直接对应于其别名的类类型的成员：

- `object` 对应于 `System.Object`。
- `string` 对应于 `System.String`。

#### 接口成员

接口的成员是在接口和接口的所有基接口中声明的成员。严格来讲，类对象中的成员不是任何接口的成员，但是类对象的接口实现成员可以在任何接口中通过成员查找获得。

```csharp
interface ISample
{
    void Func();
}
class Sample : ISample
{
    public void Func() => Console.WriteLine("Func Invoked ...");
    static void Main(string[] args)
    {
        Sample sample = new Sample();
        sample.Func();

        ISample iSample = sample;
        iSample.Func();
    }
}
```

#### 数组成员

数组成员是从类 `System.Array` 继承的成员。

#### 委托成员

委托的成员是从类 `System.Delegate` 或 `System.MulticastDelegate` 继承的成员。它还包含一些方法成员如 `Invoke`、`EndInvoke`、`BeginInvoke`。

>---

### 成员访问

成员声明允许对成员的可访问性进行控制。成员的可访问性由成员声明的可访问性和其直接包含类型的可访问性结合确定。

#### 可访问性声明

成员的可访问性可以是：

  - `public`：访问不受限制。
  - `protected`：访问限于包容类型、或派生自包含类的类型。
  - `internal`：访问仅限于当前程序集（`.exe` 或 `.dll`）。
  - `protected internal`：访问限于当前程序集、包容类型或派生自包含类的类型。
  - `private`：访问仅限于包容类型。
  - `private protected`：访问限于包含类型、或同一程序集中派生自包含类的类型。
  - `file`：（C#11）已声明的类型仅在当前源文件中可见。文件范围的类型通常用于源生成器。

当成员声明不包括任何访问修饰符时，声明位置的上下文决定声明的可访问性：

  - 命名空间没有任何访问限制，隐式声明为 `public` 且无法添加访问修饰符。
  - 位于编译单元或命名空间中声明的顶级类型只能具有 `internal` 或 `public` 可访问性，默认可访问性为 `internal`。
  - `class` 的成员默认为 `private`，其成员可以声明 `public`、`internal`、`protected`、`protected internal`、`private`、`private protected`。
  - `struct` 的成员默认为 `private`，其成员可以声明为 `public`、`internal`、`private`。
  - `interface` 的成员默认为 `public`，其成员可以声明为 `public`、`internal`、`protected`、`protected internal`、`private`、`private protected`，其中声明为 `private` 的接口成员必须具有默认的实现。
  - `enum` 的成员默认为 `public`，其成员不允许添加访问修饰符。

成员的可访问域绝不会超出包含类型的可访问域。嵌套类型的可访问性依赖于它的可访问域，该域是由已声明的成员可访问性和直接包含类型的可访问域这二者共同确定的。嵌套类型的可访问域不能超出包含类型的可访问域。可访问域表示类型或成员可以引用哪些程序分区。

> 文件本地类型（C#11）

- `file` 修饰符将顶级类型的范围和可见性限制为其所包含的文件范围。`file` 修饰符通常应用于源生成器编写的类型。**文件本地类型** 为源生成器提供了一种方便的方法，能够避免在生成的类型之间发生名称冲突。
- `file` 可用于修饰 `class`、`struct`、`enum`、`interface`、`record`、`delegate`、`record struct`、`Attribute class`。

#### 可访问性约束

C# 语言中的一些结构要求类型至少具有与成员或其他类型一样的可访问性。存在以下可访问性限制：
- 类类型的直接基类至少具有应与类类型本身相同的可访问性。
- 接口类型的显式基接口至少具有与接口类型本身相同的可访问性。
- 委托类型的返回类型和参数类型至少具有与委托类型本身相同的可访问性。
- 常量的类型至少具有与常量本身相同的可访问性。
- 字段的类型至少具有与字段本身相同的可访问性。
- 方法的返回类型和参数类型至具有与方法本身相同的可访问。
- 属性的类型至少具有与属性本身相同的可访问性。
- 事件的类型至少具有与事件本身同样的可访问性。
- 索引器的类型和参数类型至少具有与索引器本身相同的可访问性。
- 运算符的返回类型和参数类型至少具有与运算符本身具有相同的可访问性。
- 实例构造函数的参数类型至少具有与实例构造函数本身相同的可访问性。
- 类型参数上的接口或类类型约束至少具有与声明该约束的成员相同的可访问性。

```csharp
class SampleA { }
public class SampleB
{
    SampleA F() => new SampleA();
    internal SampleA G() => F();
    public SampleA H() => F();  // err: SampleA 比方法 H 的可访问性低
}
```

---
## 签名与重载

方法、实例构造函数、索引器和运算符由它们的签名来描述：
- 方法的签名由方法的名称、类型参数的数目以及其每个形参的类型和参数传递模式组成，按从左到右的顺序排列。方法的签名不包括返回类型、参数名称、类型参数名称、参数类型约束、`params` 或 `this` 参数修饰，也不包括参数是否为可选。
- 实例构造函数的签名由其参数类型和传递模式组成，并考虑排列顺序。
- 索引器的签名由形参的类型组成，并考虑排列顺序。
- 运算符的签名由运算符和形参的类型组成，并考虑排列顺序。
- 转换运算符的签名由源类型和目标类型组成，不包含 `implicit` 和 `explicit`。

签名是用于在类、结构和接口中重载成员的启用机制：重载的成员在其包含类型内的签名必须是唯一的。因此可以声明多个重载不同签名的同名方法、实例构造函数、索引器和运算符。对于签名，类型 `object` 和 `dynamic` 被认为是相同的。

尽管参数修饰符 `in`、`out`、`ref` 被认为是签名的一部分，但是在单个类型中声明的成员不能仅通过参数修饰符来区分签名，它们之间无法形成重载。

```csharp
interface ITest
{
    void F();                        // F()
    void F(int x);                   // F(int)
    void F(ref int x);               // F(ref int)
    void F(out int x);               // F(out int)      error
    void F(int x, int y);            // F(int, int)
    int F(string s);                 // F(string)
    int F(int x);                    // F(int)          error
    void F(string[] a);              // F(string[])
    void F(params string[] a);       // F(string[])     error
}
```

---
## 自动内存管理

C# 采用自动内存管理，使开发人员无手动分配和释放由对象占用的内存。自动内存管理策略是由 *垃圾回收器* 实现的。对象的内存管理生命周期如下所示：
  1. 创建对象时，将为其分配内存，运行构造函数，并将对象视为实时对象。
  2. 如果对象及其任何部分不能通过任何可能的执行继续访问（除了运行终结器），则该对象被视为不再使用，并可用于终结回收。C# 编译器和垃圾回收器可以选择对代码进行分析，以确定将来可能会使用哪些对对象的引用。如果作用域中的局部变量是唯一的对象引用，并且从当前执行点开始的任何可能的执行中都不会引用该本地变量，则垃圾回收器可能（但不必需）将该对象视为不再使用。
  3. 一旦对象符合销毁条件，在稍后某个未指定的时间运行对象的终结器（如果有）。正常情况下，对象的析构函数只运行一次，尽管特定于实现的 Api 可能会允许重写此行为。
  4. 一旦对象的终结器运行，如果该对象及其任何部分不能通过任何可能的执行继续进行访问（包括运行终结器），则该对象将被视为不可访问且认为该对象符合回收条件。
  5. 最后，在对象变为符合收集条件后的某个时间，垃圾回收器将释放与该对象关联的内存。

垃圾回收器维护有关对象使用情况的信息，并使用这些信息做出内存管理决策，如在内存中定位新创建的对象的位置，何时重新定位对象以及何时不再使用或不可访问对象。

可以通过 `System.GC` 类上的静态方法控制垃圾回收器的行为，此类可用于请求进行收集，运行（或不运行）终结器。由于垃圾收集器在决定何时收集对象和运行终结器方面有很大的自由度，因此符合标准的实现可能产生与以下代码显示的输出不同的输出：

```csharp
class Sample
{
    unsafe static void Main(string[] args)
    {
        int i = 0;
        do
        {
            Sample s = new Sample();
            s.CreateGC(100000);
            Task.Delay(200).GetAwaiter().GetResult();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        } while (i++ < 10);
    }
    static nint Ps;
    static nint LastPs;

   async void CreateGC(int size)
    {
        await Task.Delay(1);
        int[] arr = new int[size];
        for (int i = 0; i < arr.Length; i++)
            arr[i] = i;
    }
    ~Sample() => Console.WriteLine("Finalizer Invoked");
}
```

通常情况下，终结器只对存储在其自身字段中的数据执行清理，而不对引用的对象或静态字段执行任何操作。使用终结器的替代方法是让类实现 `System.IDisposable` 接口，这使得对象的客户端可以确定何时释放对象的资源，通常是通过将对象作为 `using` 语句中的资源来访问。

---
## 执行顺序

C# 程序的执行过程中，每个执行线程的副作用在关键的执行点被保留。*副作用* 定义为对易失性字段的读写、对非易失性变量的写入、对外部资源的写入以及抛出异常。这些副作用的顺序应该被保留的关键执行点包含有对 `volatile` 字段的引用，`lock` 语句，以及线程的创建和终止。

执行环境可以自由地改变 C# 程序的执行顺序，但要受以下约束：
- 数据依赖关系保存在执行线程内。计算每个变量的值时，就好像线程中的所有语句都按照原始程序顺序执行一样。
- 初始化顺序规则保留（字段初始化和变量初始值设定项）。
- 对于 `volatile` 字段的读写，会保留副作用的顺序。如果执行环境可以推断出表达式的值没有被使用，并且没有产生必要的副作用（包括调用方法或访问 `volatile` 字段所引起的任何副作用），则不需要对该表达式的部分进行求值。当程序执行被异步事件中断时（例如另一个线程抛出的异常），不能保证可观察到的副作用在原始程序顺序中可见。

---