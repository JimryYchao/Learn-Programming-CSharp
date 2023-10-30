## CSharp 语言介绍

```csharp
using System;
class Hello{
    static void Main(){
        Console.WriteLine("Hello World!");
    }
}
```

---
### C# 语言简介

C# 源于 C 语言系列，是一种面向对象的、面向组件的、类型安全的新式编程语言。开发人员利用 C# 能够生成在 .NET 中运行的多种安全可靠的应用程序。

---
### .NET 体系结构

C# 程序在 .NET 上运行，而 .NET 是名为公共语言运行时（Common Language Runtime，CLR）的虚拟执行系统和一组类库。CLR 是 Microsoft 对公共语言基础结构（CLI）国际标准的实现。CLI 是创建执行和开发环境的基础，语言和库可以在其中无缝地协同工作。

C# 源代码被编译成符合 CLI 规范的中间语言（IL）。IL 代码和资源（如位图和字符串）存储在扩展名通常为 `.dll` 的程序集中。程序集包含一个介绍程序集的类型、版本和区域性的清单。

执行 C# 程序时，程序集将加载到 CLR。CLR 会直接执行实时（JIT）编译，将 IL 代码转换成本机指令。CLR 可提供自动垃圾回收、异常处理和资源管理相关的服务。CLR 执行的代码有时称为 “托管代码”，而 “非托管代码” 被编译成面向特定平台的本机语言。

语言互操作性是 .NET 的一项重要功能。C# 编译器生成的 IL 代码符合公共类型规范（CTS）。通过 C# 生成的 IL 代码可以与通过 .NET 版本的 F#、Visual Basic、C++ 生成的代码进行交互。还有 20 多种与 CTS 兼容的语言。单个程序集可包含多个用不同 .NET 语言编写的模块。这些类型可以相互引用，就像它们是用同一种语言编写的一样。

.NET 还包含大量库，这些库的功能包括文件输入输出、字符串控制、XML 分析、Web 应用程序框架和 Windows 窗体控件。典型的 C# 应用程序广泛使用 .NET 类库来处理常见的 “管道” 零碎工作。

---
### 类型与变量

类型定义 C# 中的任何数据的结构和行为。类型的声明可以包含其成员、基类型、它实现的接口和该类型允许的操作。变量是用于引用特定类型的实例的标签。标识符是分配给类型（类、接口、结构、记录、委托或枚举）、成员、变量或命名空间的名称，是不包含任何空格的 Unicode 字符序列。

C# 是一种强类型语言，每个变量和常量、求值的表达式、方法形参与返回值等都有一个类型。.NET 类库定义了内置数值类型和表示各种构造的复杂类型，其中包括文件系统、网络连接、对象的集合和数组以及日期。

类型中可存储的信息有：类型变量所需的存储空间、可以表示的最大值和最小值、包含的成员（方法、字段、事件等）、继承自的基类型、它实现的接口、允许执行的运算种类等。编译器使用类型信息来确保在代码中执行的所有操作都是类型安全的。编译器将类型信息作为元数据嵌入可执行文件中，公共语言运行时（CLR）在运行时使用元数据，以在分配和回收内存时进一步保证类型安全性。

C# 有两种类型：值类型和引用类型。值类型的变量直接包含它们的数据，每个变量都有自己的数据副本，对一个变量执行的运算不会影响到另一个变量。引用类型的变量存储对数据（称为 “对象”）的引用，两个变量可以引用同一个对象，对一个变量执行的运算可能会影响另一个变量引用的对象。值类型在堆栈上存储数据，而引用类型在堆上分配内存。

C# 的值类型进一步分为：简单类型、枚举类型、结构类型、可以为 null 的值类型和元组值类型。C# 引用类型又细分为类类型、接口类型、数组类型和委托类型。

> C# 的值类型可分为：

- 简单类型：
  - 有符号整型：`sbyte`、`short`、`int`、`long`、`nint`。
  - 无符号整型：`byte`、`ushort`、`uint`、`ulong`、`unint`。
  - Unicode 字符：`char`，表示 UTF-16 代码单元。
  - IEEE 二进制浮点：`float`、`double`。
  - 高精度十进制浮点：`decimal`。
  - 布尔值：`bool`。
- 枚举类型：格式为 `enum E {...}` 的用户定义类型。`enum` 类型是一种包含已命名常量的独特类型。
- 结构类型：格式为 `struct S {...}` 的用户定义类型。`record struct` 为记录结构类型。
- 可以为 null 的值类型：值为 `null` 的其他所有值类型的扩展。
- 元组值类型：格式为 `(T1,T2,...)` 的用户定义类型。

> C# 引用类型可分为：

- 类类型：
  - 其他所有类型的最终基类：`object`。
  - Unicode 字符串：`string`，表示 UTF-16 代码单元序列。
  - 格式为：`class C {...}` 的用户定义类型。`record` 为记录类型。
- 接口类型：格式为 `interface I {...}` 的用户定义类型。
- 数组类型：一维、多维和交错数组。例如 `int[]`、`int[,]`、`int[][]`。
- 委托类型：格式为 `delegate TResult D(...)` 的用户定义类型。

> 用户定义类型

C# 程序使用类型声明创建新类型。用户可定义以下六种 C# 基本类型：类类型、结构类型、接口类型、枚举类型、委托类型和元组值类型。还可以声明 `record` 类型（`record struct` 或 `record class`）。

- `class` ：类类型定义包含数据成员（字段）和函数成员（方法、属性、构造函数等）的数据结构。类支持单一继承和多态。类对象在堆上分配内存，并受垃圾回收器的管理。
- `struct`：结构类型定义包含数据成员和函数成员的结构，与类相似。结构属于堆栈分配对象，不受垃圾回收器的管理。结构不支持用户指定的继承，可以实现接口。
- `interface`：接口类型将协定定义为一组已命名的公共成员。实现 `interface` 的 `class` 和 `struct` 必须提供接口成员的实现代码。接口和类、结构都可以实现多个接口。。
- `delegate`：委托类型表示引用包含特定参数列表和返回类型的方法。通过委托，可以将方法组视为变量并可作为参数传递的实体。委托类同于函数式语言提供的函数类型或函数指针概念，但是委托是面向对象且类型安全。
- `record`：记录类型用类提供封装数据的内置功能。可以声明为 `record` 引用类型或 `record struct` 值类型。记录类型具有编译器合成成员，主要用于存储值，关联行为最少。
- `enum`：枚举类型是由基础整型数值类型的一组命名常量定义的值类型。可以显式指定关联的常数值。
- `ValueTuple`：元组功能提供了简洁的语法来将多个数据元素分组成一个轻型数据结构。元组最常见的用例之一是作为方法返回类型。

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

<br>

#### 在变量声明中指定类型

当在程序中声明变量或常量时，必须指定其类型或使用 `var` 关键字让编译器推断类型。声明变量后，不能使用新类型重新声明该变量，并且不能分配与其声明的类型不兼容的值，但是可以将值转换未其他类型。

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

<br>

#### .NET 内置类型

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

<br>

#### 非托管类型

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

<br>

#### .NET 通用类型系统

对于 .NET 中的类型系统，它支持继承原则。类型可以派生自其他类型（称为基类型）。派生类型继承（有一些限制）基类型的方法、属性和其他成员。所有类型派生自单个基类型，即 `System.Object (C# keyword: object)`。这样的统一类型层次结构称为通用类型系统（CTS）。

CTS 中的每种类型被定义为值类型或引用类型。这些类型包括 .NET 类库中的所有自定义类型以及用户定义类型。使用 `struct` 关键字定义的类型是值类型；所有内置数值类型都是 `struct`。使用 `class` 或 `record` 关键字定义的类型是引用类型。

> 类、结构或记录声明类似于一张蓝图，用于在运行时创建实例或对象：

- 类是引用类型，创建类型的对象后，向其分配对象的变量仅保留对相应内存的引用（将对象引用分配给新变量后，新变量会引用原始对象）。
- 结构是值类型，创建结构时，向其分配结构的变量保留结构的实际数据（将结构分配给新变量时，会复制结构）。
- 记录类型可以是引用类型（`record` 或 `record class`）或值类型（`record struct`）

> 值类型

值类型派生自 `System.ValueType`（派生自 `System.Object`）。值类型变量直接包含其值，结构的内存在声明变量的任何上下文中进行内联分配。对于值类型变量，没有单独的堆分配或垃圾回收开销。

值类型分为 `struct` 和 `enum` 两类。内置的数值类型是结构，使用 `struct` 关键字可以创建用户自定义值类型，无法从任何值类型派生类型。`enum` 枚举定义的是一组已命名的整型常量，所有枚举从 `System.Enum`（继承自 `System.ValueType`）继承。


> 引用类型

定义为 `class`、`record class`、`delegate`、数组或 `interface` 的类型是引用类型。引用类型在声明时包含 null 值，直到用户为其分配该类型的实例或使用 `new()` 创建一个。无法使用 `new` 运算符直接实例化 `interface`，只能分配实现接口的派生实例。所有数组都是引用类型，即使元素是值类型，也不例外。数组隐式派生自 `System.Array` 类。

创建对象时，会在托管堆上分配内存。变量只保留对对象位置的引用。对于托管堆上的类型，在分配内存和回收内存时都会产生开销。“垃圾回收” 是 CLR 的自动内存管理功能，用于执行回收。

引用类型完全支持继承。创建类时，可以从其他任何接口或未定义为密封的类继承。派生类可以从基类中继承并替代虚拟方法。

<br>

#### 泛型类型

可使用一个或多个类型参数声明的类型，用作实际类型（具体类型）的占位符。客户端代码在创建类型实例时提供具体类型，这种类型称为泛型类型。例如，.NET 类型 `System.Collections.Generic.List<T>` 具有一个类型参数 `T`，当创建类型的实例时，必须指定列表将包含的对象的类型 `T`，例如 `string`：

```csharp
List<string> stringList = new List<string>();
stringList.Add("String example");
// compile time error adding a type other than a string:
stringList.Add(4);
```

可以声明泛型类、泛型结构、泛型记录、泛型接口、泛型方法和泛型委托。通过使用类型参数，可重用同一类型以保存任意类型的元素，且无需将每个元素转换为对象。泛型集合类称为强类型集合，因为编译器知道集合元素的具体类型，并能在编译时引发错误。

```csharp
public class Pair<TFirst, TSecond> // <,> 类型参数列表
{
    public TFirst First { get; }
    public TSecond Second { get; }
    public Pair(TFirst first, TSecond second) => 
        (First, Second) = (first, second);
}
```

使用泛型类时，必须为每个类型参数提供类型自变量

```csharp
{
    var pair = new Pair<int, string>(1, "two");
    int i = pair.First;     //TFirst int
    string s = pair.Second; //TSecond string
}
```


<br>

#### 隐式类型、匿名类型和可以为 null 的值类型

> 隐式类型

可以使用 `var` 关键字隐式键入一个局部变量（但不是类成员）。隐式类型仍可在编译时获取类型，但类型是由编译器提供。

```csharp
var list = new List<int> { 1, 2, 3, 4, 5, 6 };   // List<int>
var d_num = 3.1415;     // double
var func = mess => Console.WriteLine(mess);     // Action<string>
```

> 匿名类型

可结合使用 `new` 运算符和对象初始值设定项创建匿名类型。匿名类型提供了一种方便的方法，可用来将一组只读属性封装到单个对象中，而无需首先显式定义一个类型。

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

<br>

#### 编译时类型和运行时类型

变量可以具有不同的编译时和运行时类型。编译时类型是源代码中变量的声明或推断类型；运行时类型是该变量所引用的实例的类型。

```csharp
// 运行时和编译时类型相同
string message = "This is a string of characters";   // 编译时和运行时都是 string

// 运行时和编译时类型不同
object anotherMessage = "This is another string of characters";     // 编译时为 object，运行时为 string
IEnumerable<char> someCharacters = "abcdefghijklmnopqrstuvwxyz";    // 编译时为 IEnumerable<char>，运行时为 string
```

---
### 程序结构

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

<br>

#### Main 方法

`Main` 方法是 C# 应用程序的入口点，库和服务不要求定义程序入口点。`Main` 方法是应用程序启动后调用的第一个方法。程序中只能有一个入口点，如果多个类包含 `Main` 方法，必须使用 `StartupObject` 编译器选项来编译程序，以指定将哪个 `Main` 方法用作入口点。

```c
class Program
{
    static void Main(string[] args)
    {
        // Display the number of command line arguments.
        Console.WriteLine(args.Length);
    }
}
```

- `Main` 方法是可执行程序的入口点，也是程序控制开始和结束的位置。只能在类或结构中静态方法声明。
- `Main` 的返回类型可以是 `void`、`int`、`Task` 或 `Task<int>`。当且仅当 `Main` 返回 `Task` 或 `Task<int>` 时，`Main` 的声明可包括 `async` 修饰符。这明确排除了 `async void Main` 方法。
- `Main` 方法的声明中，可选包含命令行自变量 `string[] args` 参数。可使用 `System.Environment.GetCommandLineArgs();` 方法获取命令行参数。与 C 和 C++ 不同，程序的名称不被视为 `args` 数组中的第一个命令行实参，但它是 `GetCommandLineArgs()` 方法中的第一个元素。 

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

`Main` 方法若返回 `int` 或 `Task<int>`，可使程序将状态信息传递给调用可执行文件的其他程序或脚本。

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

<br>

#### 命令行自变量

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

<br>

#### 顶级语句

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

---