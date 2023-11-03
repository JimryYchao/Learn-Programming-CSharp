## CSharp 程序构建基块

C# 中的类型主要由成员、表达式、语句等构建基块生成的。构成成员的类型一般为：常量、字段、方法、属性、索引器、事件、运算符、构造函数、终结器、嵌套类型等。表达式是在操作数和运算符的基础之上构造而成。程序操作使用语句进行表示。.NET 声明命名空间来整理类型。

---
### 命名空间

- C# 程序使用命名空间进行组织。命名空间既用作组织 “内部” 系统为一个程序，同时又向 “外部” 其他程序公开程序元素。使用 `namespace` 关键字声明命名空间，命名空间是隐式的 `public`，并且命名空间的声明不能包含任何访问修饰符。

```csharp
namespace SampleNameSpace
{
    namespace NestedNameSpace{}
}

// 等价于
namespace SampleNameSpace.NestedNameSpace { }
{
    // 顶级类型声明
}
```

- 声明的命名空间定义了包含一组相关对象的作用域，可以使用命名空间来组织代码元素并创建全局唯一类型。在命名空间内，可以声明 `class`、`interface`、`record`、`struct`、`enum`、`delegate`、嵌套命名空间。

```csharp
SampleNamespace.Nested.SampleClass2 sc2 = new();

namespace SampleNamespace
{
    class SampleClass { }
    interface ISampleInterface { }
    struct SampleStruct { }
    enum SampleEnum { a, b }
    delegate void SampleDelegate(int i);

    namespace Nested
    {
        class SampleClass2 { }
    }
}
```

<br>

#### 全局命名空间

- 编辑器会添加一个默认未命名的命名空间，被称为全局命名空间，并存在于每个文件中。全局命名空间中的任何标识符都属于该空间，`global` 被定义为全局命名空间的别名，因此可以使用 `global::` 用于显式调用全局命名空间。

```csharp
namespace SampleSpace
{
    public static class Console
    {
        public static void Debug(object? message)
        {
            global::System.Console.WriteLine(message);
        }
    }
}
```

<br>

#### 文件范围的命名空间

- 文件范围的命名空间声明能够作出以下声明：一个文件中的所有类型都在一个命名空间中。文件范围的命名空间不能包含其他命名空间声明。从 C#10 开始可使用文件范围的命名空间声明。

```csharp
namespace SampleNamespace;

class AnotherSampleClass
{
    public void AnotherSampleMethod()
    {
        System.Console.WriteLine(
            "SampleMethod inside SampleNamespace");
    }
}

namespace AnotherNamespace; // Not allowed!

namespace ANestedNamespace // Not allowed!
{
   // declarations...
}
```

<br>

#### using 指令

- `using` 指令允许使用在命名空间中定义的类型，而无需指定该类型的完全限定命名空间。声明 `using` 引用命名空间时将导入隶属于该空间中的所有类型。

```csharp
using System;
using System.Collections.Generic;

List<int> arrList = [0, 2, 4, 6, 8];
Console.WriteLine(string.Join(",", arrList));
```

- 也可以使用 `using` 创建命名空间的别名或类型的别名。可以使用 `.` 或 `::` 使用别名空间的类型。
- 命名空间别名限定符 `::` 可以保证类型名称查找不受新类型和成员引入的影响，例如名称歧义。命名空间别名限定符始终出现在两个标识符（称为左侧和右侧标识符）之间。与常规 `.` 限定符不同，限定符的左侧标识符 `::` 仅作为 `extern` 或 `using` 别名进行查找。


```csharp
using Generic = System.Collections.Generic;
using Debug = System.Console;

Generic.List<int> arr = [0, 2, 4, 6, 8];
Debug.WriteLine(string.Join(",", arr));
// 命名空间别名限定符
Generic::List<int> arr2 = [0, 2, 4, 6, 8];
```

- 使用 `using static` 指定一个类型，可以在当前文件范围内无需指定该类型名称即可访问其静态成员和嵌套类型。`using static` 将直接包含在类型声明中的嵌套类型和静态成员导入到立即封闭的编译单元或命名空间体中，从而在无限制的情况下使用每个成员和类型的标识符。扩展方法不能直接导入为静态方法，但是可用于扩展方法调用。

```csharp
using System;
using System.Collections.Generic;
using static System.Math;
using static System.Linq.Enumerable;

(int Quotient, int Remainder) ar = DivRem(58, 10);   // Math.DivRem
Console.WriteLine(ar);   // (5,8)

List<char> chars = "Hello, World".ToList();   // Linq.Enumerable 扩展方法
```

<br>

#### 全局 using 指令

- 从 C#10 开始使用 `global using` 创建全局引用，`global` 这意味着 `using` 指令的作用范围从当前声明作用域扩大到所有文件，这包括全局命名空间引用、全局 `using` 别名、全局 `using static` 类型。

```csharp
global using System;
global using System.Collections.Generic;
global using Sample = Sample.Sample.Sample.Sample.SampleSpace;
global using static System.Linq.Enumerable;

Sample::SampleClass Sc = new("Hello World".ToList());
Console.WriteLine(Sc.Greeting);  // HELLO WORLD

namespace Sample.Sample.Sample.Sample.SampleSpace
{
    public class SampleClass(List<char> chars)
    {
        public string Greeting { get; } = new string(chars.Select(char.ToUpper).ToArray());
    }
}
```

<br>

#### 程序集外部别名

- 有时可能不得不引用具有相同的完全限定类型名称的程序集的两个版本或多个版本。通过使用外部程序集别名，可在别名命名的根级别命名空间内包装每个程序集的命名空间，使其能够在同一文件中使用。

* 例如在 Visual Studio 中向项目添加 `grid.dll` 和 `grid20.dll` 的引用。在依赖项程序集的 “属性” 选项卡，并将别名从 “全局” 分别更改为 “GridV1” 和 “GridV2”。使用 `extern` 导入这些别名：

```csharp
extern alias GridV1;  
extern alias GridV2;
```

- 然后就可以通过使用 `using` 别名指令为命名空间或类型创建别名。

```csharp
using Class1V1 = GridV1::Namespace.Class1;
using Class1V2 = GridV2::Namespace.Class1;
```

---
### 类型和成员的可访问性

- 类型和构成类型的成员都有关联的可访问性，用于控制类型或成员能够访问的程序文本区域。可访问性可分为：
  - `public`：访问不受限制。
  - `protected`：访问限于包容类型、或派生自包含类的类型。
  - `internal`：访问仅限于当前程序集（`.exe` 或 `.dll`）。
  - `protected internal`：访问限于当前程序集、包容类型或派生自包含类的类型。
  - `private`：访问仅限于包容类型。
  - `private protected`：访问限于包含类型、或同一程序集中派生自包含类的类型。
  - `file`：（C#11）已声明的类型仅在当前源文件中可见。文件范围的类型通常用于源生成器。

> 可访问性级别

- 使用访问修饰符 `public`、`protected`、`internal`、`private` 为成员类型指定可访问性级别。如果未在成员声明中指定访问修饰符，则将使用默认可访问性。

* 命名空间没有任何访问限制。位于命名空间的顶级类型只能具有 `internal` 或 `public` 可访问性，默认可访问性为 `internal`。它们的成员则默认具有：
  - `enum` 的成员默认为 `public`，其成员不允许添加访问修饰符。
  - `class` 的成员默认为 `private`，其成员可以声明 `public`、`internal`、`protected`、`protected internal`、`private`、`private protected`。
  - `interface` 的成员默认为 `public`，其成员可以声明为 `public`、`internal`、`protected`、`protected internal`、`private`、`private protected`，其中声明为 `private` 的接口成员必须具有默认的实现。
  - `struct` 的成员默认为 `private`，其成员可以声明为 `public`、`internal`、`private`。

- 嵌套类型的可访问性依赖于它的可访问域，该域是由已声明的成员可访问性和直接包含类型的可访问域这二者共同确定的。嵌套类型的可访问域不能超出包含类型的可访问域。可访问域表示类型或成员可以引用哪些程序分区。

> 文件本地类型（C#11）

- `file` 修饰符将顶级类型的范围和可见性限制为其所包含的文件范围。`file` 修饰符通常应用于源生成器编写的类型。**文件本地类型** 为源生成器提供了一种方便的方法，能够避免在生成的类型之间发生名称冲突。
- `file` 可用于修饰 `class`、`struct`、`enum`、`interface`、`record`、`delegate`、`record struct`、`Attribute class`。

---
### 字段

- 字段是在类、结构、记录中直接声明的任意类型的变量，字段是其包含类型的成员：
  - 使用静态修饰符 `static` 声明的字段定义的是静态字段。静态字段只指明一个存储位置，无论创建多少个类实例，永远只有一个静态字段副本。
  - 不使用静态修饰符声明的字段定义的是实例字段。每个类实例均包含相应类的所有实例字段的单独副本。

* 字段会在对象实例的构造函数被调用之前即刻初始化。如果构造函数分配了字段的值，则它将覆盖在字段声明期间给定的任何值。 

```csharp
public class Color{
    public static readonly Color Black = new(0,0,0);  // 静态只读字段
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

#### readonly 字段

- 可以将字段声明为 `readonly`。只读字段只能在初始化期间或在构造函数中赋值。只读字段类型于常量，在退出构造函数时不能重新分配只读字段。只读值类型字段包含数据，数据不可变；只读引用类型包含对其数据的引用，无法重新分配但是可以修改引用的状态。

```csharp
ReadonlySample rs1 = new ReadonlySample();
Console.WriteLine(rs1.GUI);  // -1
rs1 = new ReadonlySample(99);
Console.WriteLine(rs1.GUI);  // 99

class ReadonlySample
{
    public readonly int GUI = -1;  // 初始化时赋值
    public ReadonlySample(int gUI) => GUI = gUI;  // 构造函数中赋值
    public ReadonlySample() { }
}
```

<br>

#### required 字段

- 可以将字段或属性声明为 `required`。必填的成员必须由构造函数初始化，或者在创建对象时由对象初始值设定项初始化。`required` 成员可以在类、结构、记录中声明。必须初始化 `require` 的成员，可以将其初始化为 `null`，若必填成员没有初始化，编译器会发出错误。
- 必填成员必须至少与其包含类型一样的可访问性，如 `public` 类型不能包含 `protected` 的 `required` 成员。派生类不能隐藏（`new` 隐藏）在基类中声明的 `required` 成员。
- 不能声明 `required readonly` 字段，因为必填字段必须可设置。

```csharp
RequiredSample rs = new() { FirstName = "John", LastName = "Ychao", ID = 10010 }; // 初始化设定项

abstract class Base
{
    internal required abstract int ID { get; init; }
}
class RequiredSample : Base
{
    // 必填字段
    public required string FirstName;
    public required string LastName;
    // 必填属性, 无法隐藏
    internal override required int ID { get; init; } = -1;

```

- 当类型参数包含 `new()` 约束时，不能将具有任何 `required` 成员的类型用作类型参数。编译器无法强制在泛型代码中初始化所有必需的成员。

```csharp
RequiredSample rs = new() { FirstName = "John", LastName = "Ychao" }; // 初始化设定项
Sample<RequiredSample> S = new();  // ERROR : CS9040

class Sample<T> where T : new();
class RequiredSample
{
    public required string FirstName;
    public required string LastName;
}
```

> `SetsRequiredMembersAttribute` 构造函数

- `SetsRequiredMembers` 属性通知编译器构造函数设置了该类或结构中的所有 `required` 成员。编译器假定任何具有 `System.Diagnostics.CodeAnalysis.SetsRequiredMembersAttribute` 属性的构造函数都会初始化所有 `required` 成员。调用此类构造函数的任何代码都不需要对象初始值设定项来设置所需的成员。这主要用于位置记录和主构造函数。
- `SetsRequiredMembers` 禁用编译器检查所有 `required` 成员在创建对象时是否已初始化。

```csharp
RequiredSample rs = new RequiredSample("Jimry", "Ychao"); // 无需在初始化设定项中分配必填成员
RequiredSample rs2 = new() { FirstName = "Jimry", LastName = "Ychao" }; // 初始化设定项

class RequiredSample
{
    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public RequiredSample(string firstName, string lastName) => (FirstName, LastName) = (firstName, lastName);
    public RequiredSample() { }
    public required string FirstName;
    public required string LastName;
}
```

> 主构造函数中的参数

- 从 C#12 开始，主构造函数参数是声明字段的替代方法。在结构或类中，可以捕获这些参数并将其用于类型中声明的字段或在其成员中调用。对于记录类型，这些参数被编译器实现为公共属性。

```csharp
Person p = new Person("John", "Ychao");
Console.WriteLine(p); // John·Ychao

struct Person(string FirstName, string LastName)
{
    public override string ToString() => $"{FirstName}·{LastName}";
}
```

<br>

#### volatile 字段

- `volatile` 字段表示变量的易变的，可以由多个同时执行的线程修改。只能在类、结构、记录中声明易变字段，不能将局部变量声明为 `volatile`。
  
* 可定义以下类型的易变字段：
  - 引用类型、指针类型、简单类型（`sbyte`、`byte`、`short`、`ushort`、`int`、`uint`、`char`、`float` 和 `bool`）、枚举类型、已知为引用类型的泛型类型参数、`nint` 和 `nuint`（即 `IntPtr` 和 `UIntPtr`）。
  - 其他类型（包括 `double` 和 `long`）无法标记为 `volatile`，因为对这些类型的字段的读取和写入不能保证是原子的，若要保护对这些类型字段的多线程访问，请使用 `Interlocked` 类成员或使用 `lock` 语句保护访问权限。

- 出于性能原因，编译器，运行时系统甚至硬件都可能重新排列对存储器位置的读取和写入。声明为 `volatile` 的字段将从某些类型的优化中排除。在多处理器系统上，易失性读取操作不保证获取由任何处理器写入该内存位置的最新值；同样，易失性写入操作不保证写入的值会立即对其他处理器可见。

```csharp
class VolatileTest<T> where T : class
{
    // volatile 字段
    public volatile int sharedStorage;
    // volatile 引用类型的泛型类型参数
    volatile T shared_data;
}
```

---
### 常量

- 常量是在编译时设置其值并且不能更改其值的字段。使用 `const` 关键字声明的常量是不可变的值，在声明时初始化，在编译时是已知的，在程序的生命周期内不会改变。仅 C# 内置类型中除了 `object` 均可被声明为常量，不包括用户定义类型，但是可以使用 `readonly` 声明只读字段。
- 程序运行时没有与常量相关联的变量地址，编译器在编译时直接将常量的文本值替换到它生成的中间语言 IL 代码中，因此 `const` 字段不能通过引用传递。

```csharp
Console.WriteLine("PI : " + Sample.MATH_PI);

class Sample
{
    public const double MATH_PI = 3.1415926535;
}
```

---
### 方法

- 方法是包含一系列语句的代码块，程序通过调用该方法并指定任何所需的方法参数使语句得以执行。在 C# 中，每个执行的指令均在方法的上下文中执行。方法可以分为静态方法（通过类进行访问）和实例方法（通过类实例进行访问）。
- 方法可能包含一个参数列表，这些参数表示传递给方法的值或变量引用。方法具有返回类型，它用于指定方法计算和返回的值的类型。如果方法无返回值，则它的返回类型设置为 `void`。
- 方法可能也包含一组类型参数，必须在调用方法时指定类型自变量（泛型方法）。

```csharp
// 方法主体是由若干条语句组成的块
public void Print(string message)
{
    Console.WriteLine("DEBUG: " + message);
}
// 方法主体是单个表达式语句的表达式主体形式
public void Print(string message) => Console.WriteLine("DEBUG: " + message);
```

<br>

#### 静态和实例方法

- 使用 `static` 修饰符声明的方法是静态方法。静态方法不对特定的实例起作用，只能直接访问静态成员。未使用 `static` 修饰符声明的方法是实例方法。实例方法对特定的实例起作用，能够访问静态和实例成员。

```csharp
class Entity{
    static int static_elem;
    int s_elem;
    // 静态方法调用静态成员
    public static int GetStaticElem() => static_elem;
    // 实例方法调用实例成员，静态成员
    public int GetElem() => this.s_elem + Entity.static_elem;
}
```

<br>

#### 方法参数

- 在 C# 中，实参可以按值或按引用传递给形参。按值传递是将变量副本传递给方法，按引用传递（使用 `in`、`ref`、`out` 修饰方法参数）是将对变量的访问传递给方法。

> 参数传递方式

- 按值传递值类型时，方法对参数的修改不会影响到源数据。

```csharp
int n = 5;
System.Console.WriteLine("The value before calling the method: {0}", n);
SquareIt(n);  // Passing the variable by value.
System.Console.WriteLine("The value after calling the method: {0}", n);

static void SquareIt(int x)
// The parameter x is passed by value.
// Changes to x will not affect the original value of x.
{
    x *= x;
    System.Console.WriteLine("The value inside the method: {0}", x);
}
/* Output:
    The value before calling the method: 5
    The value inside the method: 25
    The value after calling the method: 5
*/
```

- 按引用传递值类型时，方法修改参数所引用对象的状态，会同时修改源数据的状态。重新分配参数以引用其他对象时，则不会更改传递的源数据。

```csharp
int n = 5;
System.Console.WriteLine("The value before calling the method: {0}", n);
SquareIt(ref n);  // Passing the variable by reference.
System.Console.WriteLine("The value after calling the method: {0}", n);

static void SquareIt(ref int x) 
// The parameter x is passed by reference.
// Changes to x will affect the original value of x.
{
    x *= x;
    System.Console.WriteLine("The value inside the method: {0}", x);
}
/* Output:
    The value before calling the method: 5
    The value inside the method: 25
    The value after calling the method: 25
*/
```

- 按值传递引用类型，方法修改参数所引用对象的状态，会同时修改源数据的状态。重新分配参数以引用其他对象时，则不会更改传递的源数据。

```csharp
int[] arr = { 1, 4, 5 };
System.Console.WriteLine("Inside Main, before calling the method, the first element is: {0}", arr[0]);

Change(arr);
System.Console.WriteLine("Inside Main, after calling the method, the first element is: {0}", arr[0]);

static void Change(int[] pArray)
{
    pArray[0] = 888;  // This change affects the original element.
    pArray = new int[5] { -3, -1, -2, -3, -4 };   // This change is local.
    System.Console.WriteLine("Inside the method, the first element is: {0}", pArray[0]);
}
/* Output:
    Inside Main, before calling the method, the first element is: 1
    Inside the method, the first element is: -3
    Inside Main, after calling the method, the first element is: 888
*/
```

- 按引用传递引用类型，方法修改参数所引用对象的状态，会同时修改源数据的状态。若重新分配参数以引用其他对象时，也会更改源数据的引用。

```csharp
int[] arr = { 1, 4, 5 };
System.Console.WriteLine("Inside Main, before calling the method, the first element is: {0}", arr[0]);

Change(ref arr);
System.Console.WriteLine("Inside Main, after calling the method, the first element is: {0}", arr[0]);

static void Change(ref int[] pArray)
{
    // Both of the following changes will affect the original variables:
    pArray[0] = 888;
    pArray = new int[5] { -3, -1, -2, -3, -4 };
    System.Console.WriteLine("Inside the method, the first element is: {0}", pArray[0]);
}
/* Output:
    Inside Main, before calling the method, the first element is: 1
    Inside the method, the first element is: -3
    Inside Main, after calling the method, the first element is: -3
*/
```

<br>

#### 方法参数修饰符

- `ref` 指定此参数由引用传递：必须明确分配 `ref` 参数的自变量。被调用的方法可以重新分配该参数。
- `in` 指定此参数由引用传递：必须明确分配 `in` 参数的自变量。被调用的方法无法重新分配该参数。
- `out` 指定此参数由引用传递：无需明确分配 `out` 参数的自变量。被调用的方法必须分配该参数。
- `params` 指定此参数采用可变数量的参数：可变参数只能是参数列表中的最后一位。

```csharp
int number = 45;
int[] arr = [1, 2, 3, 4];

Ref_Func(ref number);
Console.WriteLine(number); // 144
Out_Func(out int s_num);
Console.WriteLine(s_num);  // 0
In_Func(in arr);
Console.WriteLine(arr[0]); // 100
Params_Func(1, 20, 300, 4000); // 1,20,300,4000

void Ref_Func(ref int num)      // ref 引用的实参可能被修改或重新分配
{
    num += 99;
}
void Out_Func(out int num)      // out 引用的实参必须被重新分配
{
    num = default;
}
void In_Func(in int[] num)      // in 引用的实参无法重新被分配，若是传递的引用类型，则可能被修改
{
    // num = [1, 2, 3, 4]; // ERROR: 无法重新分配只读变量
    num[0] = 100;
}
void Params_Func(params int[] arr)
{
    if (arr is not null)
        Console.WriteLine(string.Join(",", arr));
}
```

- `ref`、`in`、`out` 不能用于异步方法和迭代器方法。对于扩展方法，第一个参数不能使用 `out` 修饰；当参数不是结构或是非 `struct` 约束的泛型类型时，不能对首元使用 `ref` 修饰；`in` 只能修饰结构首元，不能是泛型方法值类型约束的类型参数。

```csharp
public static class Ext
{
    public static int Ref_ExtFun(ref this int x) => x * x;
    public static T Ref_ExtFun<T>(ref this T x) where T : struct => default(T);
    public static int In_ExtFun(in this int x) => x * x;
    public static T In_ExtFun<T>(in this int x, T t) where T : struct => t;
 // public static T In_ExtFun<T>(in this T t) where T : struct => t; // ERROR : CS8338
}
```

<br>

#### 命名参数与可选参数

> 命名参数

- 通过命名实参，可以为形参指定实参，方法是将实参与该形参的名称匹配，而不是与形参在形参列表中的位置匹配。使用命名参数传递实参时，可以不再需要将实参的顺序与所调用方法的形参列表顺序相匹配，而位置参数必须与对应形参位置相匹配。每个形参的实参都可按形参名称显式进行指定。
- 当命名参数与位置参数一起使用时，必须要它们用在正确的位置。

```csharp
class NamedExample
{
    static void Main(string[] args)
    {
        // The method can be called in the normal way, by using positional arguments.
        PrintOrderDetails("Gift Shop", 31, "Red Mug");

        // Named arguments can be supplied for the parameters in any order.
        PrintOrderDetails(orderNum: 31, productName: "Red Mug", sellerName: "Gift Shop");
        PrintOrderDetails(productName: "Red Mug", sellerName: "Gift Shop", orderNum: 31);

        // Named arguments mixed with positional arguments are valid
        // as long as they are used in their correct position.
        PrintOrderDetails("Gift Shop", 31, productName: "Red Mug");
        PrintOrderDetails(sellerName: "Gift Shop", 31, productName: "Red Mug");
        PrintOrderDetails("Gift Shop", orderNum: 31, "Red Mug");

        // However, mixed arguments are invalid if used out-of-order.
        // The following statements will cause a compiler error.
        // PrintOrderDetails(productName: "Red Mug", 31, "Gift Shop");
        // PrintOrderDetails(31, sellerName: "Gift Shop", "Red Mug");
        // PrintOrderDetails(31, "Red Mug", sellerName: "Gift Shop");
    }
    static void PrintOrderDetails(string sellerName, int orderNum, string productName)
    {
        if (string.IsNullOrWhiteSpace(sellerName))
            throw new ArgumentException("Seller name cannot be null or empty.", nameof(sellerName));
        Console.WriteLine($"Seller: {sellerName}, Order #: {orderNum}, Product: {productName}");
    }
}
```

> 可选参数

- 方法、构造函数、索引器或委托的定义可以指定其形参为必需还是可选。任何调用都必须为所有必需的形参提供实参，但可以为可选的形参省略实参。每个可选参数都有一个默认值，可以是常量表达式、`new ValType()` 值类型或 `default`。
- 可选参数列表定义必须定义参数列表的末尾和必须参数之后。被提供实参的可选参数之前的全部可选参数都必须提供相应的实参。

```csharp
ExampleMethod(required);

void ExampleMethod(int required, string optionalstr = "default string", int optionalint = new int());
```

> 可选参数特性和默认参数值特性

- 可通过使用 `.NET OptionalAttribute` 类声明可选参数，但是 `OptionalAttribute` 形参不需要默认值。如果需要默认值，可以为形参指定 `DefaultParameterValueAttribute` 类。

```csharp
using System.Runtime.InteropServices;

ExampleMethod(0);

void ExampleMethod(int required, [Optional] string optionalstr, [Optional, DefaultParameterValue("DEFAULT_PARAM_VALUE")] string defaultOptional)
{
    Console.WriteLine($"optional str is null: {optionalstr == null}");
    // Output: optional str is null: True
    Console.WriteLine($"default optional str is : {defaultOptional}");
    // Output: default optional str is : DEFAULT_PARAM_VALUE
}
```

- 可选参数与 `[Optional, DefaultParameter(VALUE)]` 之间存在一些行为差异。特性 `DefaultParameterValueAttribute` 允许用户以其他方式为不支持默认参数的语言指定默认参数值。将此属性应用于代码后，支持默认参数的语言可以使用指定的值作为默认参数。对于 `DefaultParameterValueAttribute` 为 `COM` 互操作接口的方法指定默认参数，特别有用。

```csharp
using System.Runtime.InteropServices;

public class Program
{
    public static void MethodWithObjectDefaultAttr1([Optional, DefaultParameterValue(123)] object obj) {} // OK
    public static void MethodWithObjectDefaultAttr2([Optional, DefaultParameterValue("abc")] object obj) {} // OK
    public static void MethodWithObjectDefaultAttr3([Optional, DefaultParameterValue(null)] object? obj) {} // OK

    public static void MethodWithObjectDefaultParam1(object obj = 123) {} // CS1763
    public static void MethodWithObjectDefaultParam2(object obj = "abc") {} // CS1763
    public static void MethodWithObjectDefaultParam3(object obj? = null) {} // OK
}
```

<br>

#### 方法主体和局部变量

- 方法主体指定了在调用方法时执行的语句。方法主体可以声明特定于方法调用的局部变量，局部变量在使用前必须先明确赋值，可以稍后的语句中延迟赋值。

- 方法使用 `return` 语句将控制权返回给调用方。对于无返回类型的 `void` 方法，`return` 语句可省略。

```csharp
public static void WriteSquares()
{
    int i = 0;
    int j;
    while (i < 10)
    {
        j = i * i;
        Console.WriteLine($"{i} x {i} = {j}");
        i++;
    }
}
```

<br>

#### 虚方法、重写方法和抽象方法

- 可使用虚方法、重写方法和抽象方法来定义类类型层次结构的行为：
  - 虚方法是在基类中声明和实现的方法，一般定义缺省行为，其中任何派生类都可提供更具体的实现。
  - 重写方法是在派生类中实现的方法，可修改基类实现的行为。
  - 抽象方法是在抽象基类中声明的方法，必须在所有派生类中重写。抽象方法不在抽象基类中定义实现。

* 虚方法和抽象方法不能声明在密封 `sealed` 类中。

```csharp
abstract class Base
{
    // 抽象方法
    public abstract void BasePrint(string message);     
    // 基类中的虚方法
    public virtual void Print(string message) => Console.WriteLine("DEBUG : ");
}
class Derived : Base
{
    // 重写抽象方法
    public override void BasePrint(string message) => base.Print(message);
    // 重写父类的虚方法
    public override void Print(string message) => Console.WriteLine("SON DEBUG : " + message);
}
```

<br>

#### 方法重载

- 借助方法重载，同一类中可以有多个同名的方法，只要这些方法具有唯一签名即可。编译器使用重载决策（查找与自变量匹配度最高的）来确定要调用的特定方法，并在找不到任何最佳匹配项时报告错误。

```csharp
class OverloadingExample
{
    static void F() => Console.WriteLine("F()");
    static void F(object x) => Console.WriteLine("F(object)");
    static void F(int x) => Console.WriteLine("F(int)");
    static void F(double x) => Console.WriteLine("F(double)");
    static void F<T>(T x) => Console.WriteLine($"F<T>(T), T is {typeof(T)}");            
    static void F(double x, double y) => Console.WriteLine("F(double, double)");
    
    public static void UsageExample()
    {
        F();            // Invokes F()
        F(1);           // Invokes F(int)
        F(1.0);         // Invokes F(double)
        F("abc");       // Invokes F<T>(T), T is System.String
        F((double)1);   // Invokes F(double)
        F((object)1);   // Invokes F(object)
        F<int>(1);      // Invokes F<T>(T), T is System.Int32
        F(1, 1);        // Invokes F(double, double)
    }
}
```

> 使用方法参数的方法重载决策

- 当一个方法具有 `ref`、`in` 或 `out` 参数，另一个方法具有值传递的参数时，则可以重载方法。成员方法不能具有仅在 `ref`、`in` 或 `out` 方面不同的签名，若类型的两个成员之间的唯一区别在于相同位置的参数具有不同的 `ref`、`in`、`out` 修饰时，则会发生编译器错误。

```csharp
class Sample
{
    public int Func(int num) => num;
    public int Func(in int num) => num;
 // public int Func(ref int num) => num;  // 无法构成重载
    public int Func(out int num, int num2) => num = num2;
    public int Func(ref int num, short num2) => num + num2;
    public int Func(in int num, in int num2) => num + num2;
}
```

- `params` 可变参数方法无法和具有值传递和相同签名的方法构成方法重载。

```csharp
class Sample
{
    public int Func(params int[] arr) => arr.Length;
 // public int Func(int[] arr) => arr.Length; // : ERROR
    public int Func(ref int[] arr) => arr.Length; // 可以构成重载
}
```

<br>

#### 本地函数

- 本地函数是一种嵌套在另一个成员中的方法，且仅能从其包含成员中调用它，因此本地函数不包含任何访问修饰符。可以在方法、构造函数、属性访问器、事件访问器、匿名方法、Lambda 表达式、终结器、其他本地函数中声明和调用本地函数。在相同作用域下声明的本地函数无法声明同名的重载函数。
- 可以声明 `async`、`unsafe`、`static`、`extern static` 修饰的本地函数。从 C#9 开始，可以将特性应用于本地函数、其参数和类型参数。

```csharp
private static string GetText(string path, string filename)
{
    var reader = File.OpenText($"{AppendPathSeparator(path)}{filename}");
    var text = reader.ReadToEnd();
    return text;

    string AppendPathSeparator(string filepath)
    {
        return filepath.EndsWith(@"\") ? filepath : filepath + @"\";
    }
}
```

* 本地函数与 Lambda 表达式的区别在于：
  - Lambda 表达式是在运行时声明和分配的对象，使用前必须对其进行明确赋值，并在声明时转换为委托；
  - 本地函数在编译时定义，若只是通过调用方法一样调用本地函数而不捕获封闭范围中的变量时，本地函数不会转换为委托，只有在本地函数中明确分配了封闭范围的变量时，本地函数将作为委托类型实现。
- 本地函数可以避免 Lambda 表达式始终需要的堆分配。若本地函数不会转化为委托时，并且本地函数捕获的变量不会被其他转换为委托的 Lambda 或本地函数捕获，则编译器可以避免堆分配。

```csharp
public static int LocalFunctionFactorial(int n)
{
    return nthFactorial(n);
    // 本地函数编译时定义
    int nthFactorial(int number) => 
        number < 2 ? 1 : number * nthFactorial(number - 1);
}
public static int LambdaFactorial(int n)
{
    Func<int, int> nthFactorial = default(Func<int, int>);
    // Lambda 运行时分配，并转换为委托
    nthFactorial = number => 
        number < 2 ? 1 : number * nthFactorial(number - 1);

    return nthFactorial(n);
}

int M()
{
    int y;
    LocalFunction();
    return y;
    // 本地函数捕获封闭范围中的 y 并明确分配，此时本地函数作为本地函数实现。编译器将为其堆分配。 
    void LocalFunction() => y = 0;
}
```

<br>

#### 扩展方法

- 扩展方法使你能够向现有类型 “添加” 方法，而无需创建新的派生类型、重新编译或以其他方式修改原始类型。扩展方法是一种静态方法，只能在顶级静态类中声明，但可以像扩展类型上的实例方法一样进行调用。扩展方法的第一个参数指定方法操作的类型。
- 在代码中，可以使用实例方法语法调用该扩展方法。编译器生成的中间语言 IL 会将代码转换为对静态方法的调用。扩展方法并未真正违反封装原则，它无法访问方法所扩展的类型中的专用变量。
- 可以使用扩展方法来扩展类或接口，但不能重写扩展方法。与接口或类方法具有相同名称和签名的扩展方法永远不会被调用。编译时，扩展方法的优先级总是比类型本身中定义的实例方法低。当编译器遇到方法调用时，它首先在该类型的实例方法中寻找匹配的方法；如果未找到任何匹配方法，编译器将搜索为该类型定义的任何扩展方法，并且绑定到它找到的第一个扩展方法。

```csharp
int number = 0;
Console.WriteLine(number.TypeName()); // Int32
Console.WriteLine(number.NameOf());   // number

public static class Ext
{
    // 返回变量的类型名称
    public static string TypeName<T>(this T type) => type.GetType().Name;
    // 返回实参变量的标识符
    public static string NameOf<T>(this T parameter,
        [System.Runtime.CompilerServices.CallerArgumentExpression("parameter")] string parameterName = null)
        => parameterName.ToString();
}
```

- 可以为结构类型设计 `ref` 扩展方法，用来更改要引用结构的状态。仅允许值类型或受结构约束的泛型类型作为 `ref` 扩展方法的第一个参数。

```csharp
int num = 1;
num.Increment();
Console.WriteLine(num);  // 2

public static class Ext
{
    public static void Increment(ref this int number) => number++;
}
```

---

### 属性

- 属性是类、结构或记录中可以像字段一样访问的方法。属性可以为字段提供保护，以避免字段在对象不知道的情况下被更改。在属性中可以组合实现 `get` 读访问器、`set` 写访问器或 `init` 构造访问器，这些访问器可以具有不同的访问级别。`value` 用于定义由 `set` 或 `init` 访问器分配的值。
- 属性可以被声明为 `virtual` 或 `abstract`，以在派生类型中进行重写。

```csharp
public class TimePeriod
{
    private double _seconds;
    public double Hours
    {
        get => _seconds / 3600;
        set
        {
            // 在分配值之前进行验证
            if (value < 0 || value > 24)
                throw new ArgumentOutOfRangeException(nameof(value), "The valid range is between 0 and 24.");
            _seconds = value * 3600;
        }
    }
    // 表达式主体定义
    public string Greeting => "Hello, World"; 
}
```

<br>

#### 自动实现的属性

- 当属性访问器中不需要任何其他逻辑时，自动实现的属性会使属性声明更加简洁。编译器会为其自动创建仅可以通过该属性的 `get` 和 `set` 访问器访问的专用、匿名支持字段。自动实现的属性必须具有 `get` 访问器。

```csharp
Sample s = new() { GUI = 10010 };
s.FullName = "Hello World";

class Sample
{
    public required int GUI { get; init; } = -1; // 自动实现，可以设置初始化值而不是使用类型默认值
    public string FullName { get; set; }
}
```

> 如何实现不可变属性

- 仅声明 `get` 访问器的属性只能在构造函数中可变，无法在对象的初始化构造器中重新分配。
- 声明 `get` 和 `init` 访问器的属性在构造函数中可变，也可以在对象的初始化构造器中重新分配。
- 表达式主体形式的属性声明仅由一个 `get` 访问器构成，且无法在任何地方修改。
- 声明 `get` 和 `private set` 访问器的属性只能在该类型中设置，对于外部则不可变。

```csharp
Sample s = new();
Console.WriteLine(s);
// Sample { OnlyGet = 10010, Get_Init = 10086, Private_Set = 10000, ReadOnly = 30096 }
Console.WriteLine(s.Private_Set);  // 10001

Sample s2 = s with { Get_Init = 20010 };
Console.WriteLine(s2);
// Sample { OnlyGet = 10010, Get_Init = 20010, Private_Set = 10001, ReadOnly = 40021 }

struct Sample
{
    public Sample()
    {
        OnlyGet = 10010;
        Get_Init = 10086;
        Private_Set = 10000;
    }
    public int OnlyGet { get; }
    public int Get_Init { get; init; }
    public int Private_Set { get; private set; }
    public int ReadOnly => Get_Init + OnlyGet + Private_Set++;
}
```

<br>

#### 接口属性

- 接口中声明的实例属性不能具有初始化设定项。接口中声明的实例属性，编译器不会为其创建自动实现，需要在派生类型中实现或自动属性实现。除非在接口中为声明的属性提供默认实现，此时该属性只能通过接口实例访问，除非在派生类型中重新实现。添加非 `public` 访问修饰符的实例属性只能在派生类型中显式实现，除非在派生类型中重新声明为 `public`。
- 接口中的静态属性可以设定初始化值，且是可以自动实现的。

```csharp
ISampleA s = new Sample() { InstanceId = 100 };
// s.GUI 无法访问
ISampleA.Name = "World";
Console.WriteLine(ISampleA.Name);

Sample s1 = new Sample() { InstanceId = 10086 };
Console.WriteLine(s1.InstanceId); // 10086

ISampleB sb = s1;
Console.WriteLine(sb.GUI); // 10010

interface ISampleA
{
    static string Name { get; set; } = "Hello"; // 自动实现
    protected int _GUI { get; } // 不会自动实现
    int InstanceId { get; } 
}
interface ISampleB : ISampleA
{
   int GUI => _GUI; // 不会自动实现
}
class Sample : ISampleB
{
    public int InstanceId { get; init; }
    int ISampleA._GUI => 10010;
}
```

- 派生类型默认实现接口属性时（非显式接口实现），可以为其额外添加的访问器，非公共的属性可以重新声明为 `public`，此时将覆盖原有的默认实现定义。

```csharp
Sample s = new Sample() { GUI = 10010, Name = "Hello" };

interface ISample
{
    string Name { get; }
    protected int GUI { get; }
}
class Sample : ISample
{
    public required string Name { get; init; }
    public required int GUI { get; init; }
}
```

<br>

#### readonly 属性

- 可以将 `readonly` 应用于结构的属性访问器，以指示实例成员不会在该访问器中改变。也可以将 `readonly` 修饰整个属性，以表示所有的访问器都是 `readonly`，要求该属性不能是自动实现。
- `readonly` 无法修饰 `class` 非字段成员。

```csharp
struct NumberArray(params int[] numbers)
{
    public readonly int First
    {
        get => numbers[0];
        set => numbers[0] = value;
    }
}
```

- `readonly` 可以应用于某些自动实现的属性，但它不会产生有意义的效果。无论是否存在关键字，编译器都将所有自动实现的 `get` 访问器视为只读 `readonly`。

```csharp
// Allowed
public readonly int Prop1 { get; }
public int Prop2 { readonly get; init;}
public int Prop3 { readonly get; set; }

// Not allowed
public readonly int Prop4 { get; set; }
public int Prop5 { get; readonly set; }
```

<br>

#### required 属性

- 类型除了可以声明必填字段，也可以声明必填属性，该属性必须具有具有 `set` 或 `init` 访问器。显式接口实现的属性不能标记为 `required`。派生类型无法删除重写 `required` 的属性的状态，但可以在重写属性中添加 `required` 修饰。

```csharp
RequiredSample rs = new RequiredSample("Jimry", "Ychao", 10010); // 无需在初始化设定项中分配必填成员
RequiredSample rs2 = new() { FirstName = "Jimry", LastName = "Ychao", ID = 10086 }; // 初始化设定项

interface IBase { int ID { get; } }
class RequiredSample : IBase
{
    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public RequiredSample(string firstName, string lastName, int id) 
        => (FirstName, LastName, ID) = (firstName, lastName, id);
    public RequiredSample() { }
    // required 属性
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    // 继承的属性中添加 required 修饰
    public required int ID { get; init; }
}
```

- `readonly required` 属性必须具有 `set` 或 `init` 访问器。

```csharp
public readonly required int Prop1 { get; init; }
public readonly required int Prop2 { get { .. } set { .. } }
```

<br>

#### 记录中的主构造函数

- 拥有主构造函数的记录，编译器会将其参数自动实现公共属性。对于 `record` 实现的公共属性是只读的（`{ get; init; }`），对于 `record struct` 则是读写的（`{ get; set; }`）。

```csharp
C_Person cp = new("Jimry", "Ychao");
S_Person sp = new();
sp.FirstName = "Hello";
sp.LastName = "World";

Console.WriteLine(cp);
// C_Person { FirstName = Jimry, LastName = Ychao }
Console.WriteLine(sp);
// S_Person { FirstName = Hello, LastName = World }

record struct S_Person(string FirstName, string LastName);
record C_Person(string FirstName, string LastName);
```

---
### 索引器

- 索引器允许类或结构的实例就像数组一样进行索引。索引器类似于属性，但是需要使用参数进行索引访问。索引器可以不必使用整数值进行索引，可以使用任何类型的值作为索引。索引器可以重载，因此可以由多个形参作为检索条件。
- 索引器可以是 `virtual` 或 `abstract`，无法声明为 `static`。

```csharp
var arr = new NumberArray(1, 2, 3, 4, 5, 6);
Console.WriteLine("Last elem is " + arr[arr.Length - 1]);

class NumberArray(params int[] Numbers)
{
    public int this[int index]
    {
        get => Numbers[index];
        set => Numbers[index] = value;
    }
    public int Length => Numbers.Length;
}
```

- 无法对作为检索的索引器参数使用引用传递修饰，但可以通过检索将索引值按引用返回（`return ref`），此时可以将索引器值按引用进行参数传递。由于索引器不是变量，因此非引用传递返回的索引器值不能用作是 `ref` 或 `out` 参数。`return ref` 返回的索引器不能设置 `set` 访问器。

```csharp
var arr = new NumberArray(1, 2, 3, 4, 5, 6);
ref int last = ref arr[arr.Length - 1];
last = 99;  // 引用，数组末尾元素的别称

unsafe
{
    fixed (int* plast1 = &last, ptr = arr)
    {
        int* plast2 = ptr + arr.Length - 1; // 获取数组的末尾元素
        if (plast1 == plast2)  // 指针地址相同
            Console.WriteLine(arr[arr.Length - 1]);  // 99
    }
}

file class NumberArray(params int[] Numbers)
{
    public ref int this[int index] => ref Numbers[index];
    public int Length => Numbers.Length;
    public ref int GetPinnableReference() => ref Numbers[0];
}
```

- 通过声明索引器，编译器会自动在对象上生成一个名为 `Item` 的属性，无法从实例成员访问表达式直接访问 `Item` 属性。如果在包含索引器的对象中添加自己的 `Item` 属性，则将收到 CS0102 编译器错误。要避免此错误，可以使用 `IndexerNameAttribute` 来重命名索引器。

```csharp
struct SampleCollection<T>( T[] arr)
{
    [System.Runtime.CompilerServices.IndexerName("__Item")]
    public ref T this[int i] =>  ref arr[i];
    T Item { get; set; }
}
```

<br>

#### readonly 索引器

- 和 `readonly` 属性类似，可以应用于整个索引器或单个访问器，指示该访问器不会修改 `this` 的状态。

```csharp
public readonly int this[int index]
{
    get { /*..*/ }
    set { /*..*/ }
}
```

<br>

#### 为索引器提供索引和范围运算 

- 可以为类型声明仅以 `System.Index` 为检索参数的索引器，该索引器将支持索引运算 `^`；可以为类型声明仅以 `System.Index` 为检索参数的索引器，该索引器将支持范围运算 `..`。

```csharp
var arr = new NumberArray(1, 2, 3, 4, 5, 6);
Console.WriteLine($"Last Elem is {arr[^1]}");
// Last Elem is 6
Console.WriteLine($"The arr Range(1,3) is [{string.Join(",", arr[1..4])}]");
// The arr Range(1,3) is [2,3,4]

class NumberArray(params int[] Numbers)
{
    public int this[Index index] => Numbers[index];
    public int[] this[Range range] => Numbers[range];
}
```

> 索引和范围的隐式支持

- 若定义类型中声明了一个 `int Length {get;}` 实例属性和 `this[int]` 的索引器，且没有 `this[System.Index]` 和 `this[System.Range]` 的索引器时，该类型将隐式支持索引运算。继续定义一个实例方法 `public TResult[] Slice(int start, int length)`，该类型将隐式支持范围运算。

```csharp
var arr = new NumberArray(1, 2, 3, 4, 5, 6);
Console.WriteLine($"Last Elem is {arr[^1]}");
// Last Elem is 6
Console.WriteLine($"The arr Range(1,3) is [{string.Join(",", arr[1..4])}]");
// The arr Range(1,3) is [2,3,4]

class NumberArray(params int[] Numbers)
{
    public int Length => Numbers.Length;
    public int this[int index] => Numbers[index];
    // 不要显式调用
    public int[] Slice(int start, int length) 
        => Numbers[start..(start + length)];
}
```

<br>

#### 接口中的索引器

- 可以在接口上声明索引器，接口访问器不使用修饰符，通常没有正文，仅以指示该索引器为读写、只读还是只写。可以为接口中定义的索引器提供实现，但这种情况非常少。

```csharp
interface ISampleArray<T>
{
    T this[int index] { get; }
    int Length { get; }
}
```

---
### 事件

- 事件是对象用于（向系统中的所有相关组件）广播已发生事情的一种方式。任何其他组件都可以订阅事件，并在事件引发时得到通知。发送或引发事件的类型称为 “发布者”，接收或处理事件的类型称为 “订阅者”。发行者确定如何引发事件，而订阅者确定对事件发生后做出何种响应。
- 使用关键字 `event` 声明事件。实际上，事件是建立在对委托的语言支持之上的。一个事件可以有多个订阅者，订阅者也可以处理来自多个发行者的多个事件。
- 事件的调用只能在其包容对象中引发。包含定义类型之外的代码无法引发事件，也不能执行任何其他操作，只能进行订阅或取消订阅。

```csharp
class EventSample
{
    static event EventHandler<string> MessageUpdating = 
        (object? sender, string mess) => Console.WriteLine($"{sender} >>> {mess}");
    static void Main(string[] args)
    {
        if (MessageUpdating is Delegate or MulticastDelegate)
            MessageUpdating?.Invoke(nameof(Sample) ,"Event is a kind of Delegate.");   // 引发事件
    }
}
```

<br>

#### 事件访问器

- 最简单的方式是将该事件声明为公共字段形式，编译器生成的代码会为其创建包装器，以便事件对象只能通过安全的方式进行访问。
- 可以显式为事件添加访问器 `add` 和 `remove` 并关联一个内部的事件对象，可以在访问器中添加额外的验证代码或用户定义代码。具有访问器的事件只能出现在 `+=`、`-=` 的左侧。

```csharp
Sample s = new Sample();
s.MyEvent += (o, e) => Console.WriteLine($"{o} MyEvent Invoke >>> {e}");

s.Greeting("Hello, World");
// Sample MyEvent Invoke >>> Hello, World

class Sample
{
    private event EventHandler<string> myEvent;  // 字段形式
    public event EventHandler<string> MyEvent    // 添加访问器
    {
        add => myEvent += value;
        remove => myEvent -= value;
    }
    public void Greeting(string mess) => RaiseMyEvent(mess);
    private void RaiseMyEvent(string mess) => myEvent?.Invoke(this, mess);
}
```

<br>

#### 事件的订阅与取消

- 类似与委托，通过使用 `+=` 运算符订阅事件，使用 `-=` 运算符取消订阅。事件的初始化和事件的引发只能在其包含类型中实现定义。

```csharp
class EventHandlerSample
{
    public static event Action<int> WhileNumberGreaterThanZero;
    // 事件触发方法
    static void On_WhileNumberGreaterThanZero(int arg) => WhileNumberGreaterThanZero?.Invoke(arg);
    static void Main(string[] args)
    {
        // 首次调用 SubscriberA，SubscriberB
        SubscriberA sa = new SubscriberA();
        SubscriberB sb = new SubscriberB();

        Random r = new Random(DateTime.Now.Microsecond);
        for (int i = 0; i < 10; i++)
        {
            int rand = r.Next(-999, 999);
            if (rand > 0)  // 在随机数大于 0 时引发事件
                On_WhileNumberGreaterThanZero(rand);
        }
        // 取消订阅事件
        WhileNumberGreaterThanZero -= SubscriberA.Square;

        for (int i = 0; i < 10; i++)
        {
            int rand = r.Next(-999, 999);
            if (rand > 0)  // 在随机数大于 0 时引发事件
                On_WhileNumberGreaterThanZero(rand);
        }
    }
}
class SubscriberA
{
    // 在首次调用时订阅事件
    static SubscriberA() => EventHandlerSample.WhileNumberGreaterThanZero += Square;
    public static void Square(int number) => Console.WriteLine($"The square of {number} is {number * number}");

}
class SubscriberB
{
    // 在首次调用时订阅事件
    static SubscriberB() => EventHandlerSample.WhileNumberGreaterThanZero += Sqrt;
    public static void Sqrt(int number) => Console.WriteLine($"The square root of {number} is {Math.Sqrt(number):F6}");
}
```

> 匿名方法的订阅和取消

- 取消定于 `-=` 的右侧是匿名方法或 Lambda 表达式时，它会成为不同的委托实例，并静默地不执行任何操作。

```csharp
class EventHandlerSample
{
    public static event Action<string> Debug;
    static void Main(string[] args)
    {
        // 订阅 lambda 表达式
        Debug += mess => Console.WriteLine(mess);
        // 订阅匿名类型
        var log = (string mess) => Console.WriteLine("LOG : " + mess);
        Debug += log;  // 同时保存了 log 的信息
        // 引发事件
        Debug?.Invoke("Hello");

        // 取消订阅
        Debug -= mess => Console.WriteLine(mess);  // 此操作无效
        Debug -= log;

        // 引发事件
        Debug?.Invoke("World");
    }
}
```

<br>

#### readonly 事件

- `readonly` 可应用于提供实现的事件，但不能应用于类似于字段的事件。`readonly` 不能应用于单个事件访问器（`add` / `remove`）

```csharp
// Allowed
public readonly event Action<EventArgs> Event1
{
    add { }
    remove { }
}

// Not allowed
public event Action<EventArgs> Event2;
public event Action<EventArgs> Event3
{
    readonly add { }
    readonly remove { }
}
public static readonly event Action<EventArgs> Event4
{
    add { }
    remove { }
}
```

<br>

#### 标准 .NET 事件模式

- .NET 事件通常遵循几种已知模式。标准化这些模式意味着开发人员可利用这些标准事件模式，将其应用于任何 .NET 事件程序。在 .NET 类库中，事件是基于 `System.EventHandler` 委托和 `System.EventArgs` 基类：

```csharp
public delegate void EventHandler<TEventArgs>(object? sender, TEventArgs e);
public class EventArgs
{
    public static readonly EventArgs Empty;
    public EventArgs() { }
}
```

- .NET 事件委托的标准签名：参数列表中包含 `sender` 发件人和 `args` 事件参数，按照惯例发件人使用 `object`。事件参数一般是派生自 `System.EventArgs`（也可以是用户定义），若事件类型不需要任何参数，仍需要提供这两个参数，事件参数使用 `EventArgs.Empty` 来表示事件不包含任何附加信息。

```csharp
void EventRaised(object? sender, EventArgs args);
```

- 设计一个符合标准 .NET 事件模式的组件：该组件中定义一个功能，它将搜索并列出目录或子目录中任何遵循模式的文件，并且在找到的每个与模式匹配的文件时引发一个事件。

> 定义 FileSearcher 和找到文件时的初始事件参数 FileFoundArgs

```csharp
// 初始事件参数
public class FileFoundArgs : EventArgs
{
    public string FoundFile { get; }
    public FileFoundArgs(string fileName) => FoundFile = fileName;
}
// 事件发布者
public class FileSearcher
{
    // 发布者定义事件处理程序
    public event EventHandler<FileFoundArgs>? FileFound;
    public void Search(string directory, string searchPattern)
    {
        // 找到任何符合条件的文件都将引发事件
        foreach (var file in Directory.EnumerateFiles(directory, searchPattern))
            RaiseFileFound(file);
    }
    // 符合标准的事件委托
    private void RaiseFileFound(string file) =>
        FileFound?.Invoke(this, new FileFoundArgs(file));
}
```

- 向发布者的事件处理程序添加订阅者，并调用 `Search`。

```csharp
class EventSample
{
    static FileSearcher AddEventHandler()
    {
        var fileLister = new FileSearcher();
        EventHandler<FileFoundArgs> onFileFound = (sender, eventArgs) =>
        {
            Console.WriteLine(eventArgs.FoundFile);
        };
        fileLister.FileFound += onFileFound;
        return fileLister;
    }
    static void Main(string[] args)
    {
        // 向发布者的事件系统添加订阅者
        var fileLister = AddEventHandler();
        // 创建组件功能，寻找所有后缀名为 .dll 的文件
        fileLister.Search(System.AppDomain.CurrentDomain.BaseDirectory, "*.dll");
    }
}
```

> 为 FileSearcher 事件处理程序添加取消功能

- 为组件设计取消程序，即在满足设定条件的情况下，事件处理系统能够停止进一步的处理。可以使用 `EventArgs` 对象来包含订阅者可以用来通信取消的字段。可以在事件参数中添加一个 `bool` 字段以指示取消状态：
  - 一种模式是任何一个订阅者都可以取消或中止事件处理程序：新字段初始化为 `false`，任何订阅者都可以将其更改为 `true`，在所有的订阅者得到事件引发的通知后，发布者检查该事件参数的布尔状态值并采取相应的操作，
  - 另一种是只有在所有订阅者都希望取消操作时才会取消操作：初始化新字段以指示取消状态，任何订阅者都可以更改它。在所有的订阅者都确认引发的事件后，发布者检查布尔值并采取操作，额外的是，发布者需要知道是否有订阅者收到该事件引发的通知。

* 实现第一种模式：

```csharp
public class FileFoundArgs : EventArgs
{
    public string FoundFile { get; }
    public FileFoundArgs(string fileName) => FoundFile = fileName;
    // 订阅者添加用来通信取消或中止事件系统处理程序的字段
    public bool CancelRequested { get; set; }
}
```

- 在发布者中添加引发事件后的取消标志检查：

```csharp
public class FileSearcher
{
    // 发布者定义事件
    public event EventHandler<FileFoundArgs>? FileFound;
    public void Search(string directory, string searchPattern)
    {
        SearchDirectory(directory, searchPattern);
    }
    private void SearchDirectory(string directory, string searchPattern)
    {
        foreach (var file in Directory.EnumerateFiles(directory, searchPattern))
        {
            FileFoundArgs args = RaiseFileFound(file);
            if (args.CancelRequested)  // 检查取消标志
                break;
        }
    }
    // 符合标准的事件委托
    private FileFoundArgs RaiseFileFound(string file)
    {
        var args = new FileFoundArgs(file);
        FileFound?.Invoke(this, args);
        return args;
    }
}
```

- 向发布者的事件处理程序添加订阅者，并调用 `Search`。

```csharp
class EventSample
{
    static FileSearcher AddEventHandler()
    {
        var fileLister = new FileSearcher();
        EventHandler<FileFoundArgs> onFileFound = (sender, eventArgs) =>
        {
            Console.WriteLine(eventArgs.FoundFile);
            // 在找到 test.dll 文件时取消事件处理系统
            if (Path.GetFileNameWithoutExtension(eventArgs.FoundFile) == "test")
                eventArgs.CancelRequested = true;
        };
        fileLister.FileFound += onFileFound;
        return fileLister;
    }
    static void Main(string[] args)
    {
        // 向发布者的事件系统添加处理程序
        var fileLister = AddEventHandler();
        // 创建组件功能，寻找所有后缀名为 .dll 的文件
        fileLister.Search(System.AppDomain.CurrentDomain.BaseDirectory, "*.dll");
    }
}
```

> 向组件添加一个新功能，该功能可以遍历所有子目录以搜索文件

- 在 `FileSearcher` 中添加一个新事件，在每次新目录开始搜索开始时引发该事件，订阅者能够跟踪进度并根据进度更新用户。开始设计之前定义一个新的事件参数，用于报告新的目录和进度。

```csharp
internal class SearchDirectoryArgs : EventArgs
{
    internal string CurrentSearchDirectory { get; }
    internal int TotalDirs { get; }
    internal int CompletedDirs { get; }
    // 状态更新函数
    internal SearchDirectoryArgs(string dir, int totalDirs, int completedDirs)
    {
        CurrentSearchDirectory = dir;
        TotalDirs = totalDirs;
        CompletedDirs = completedDirs;
    }
}
```

- 在 `FileSearcher` 中定义一个可以添加和删除事件处理程序的显式事件属性，和事件响应方法

```csharp
public class FileSearcher
{
    // ....
    private EventHandler<SearchDirectoryArgs>? _directoryChanged;
    internal event EventHandler<SearchDirectoryArgs> DirectoryChanged
    {
        add { _directoryChanged += value; }
        remove { _directoryChanged -= value; }
    }
    private void RaiseSearchDirectoryChanged(string directory, int totalDirs, int completedDirs)
    {
        _directoryChanged?.Invoke(this,new SearchDirectoryArgs(directory, totalDirs, completedDirs));
    }
    // ....
}
```

- 重定义或添加重载 `FileSearcher.Search` 方法以引发这两个事件。

```csharp
public class FileSearcher
{
    // ....
    public void Search(string directory, string searchPattern, bool searchSubDirs = false)
    {
        if (searchSubDirs)
        {
            var allDirectories = Directory.GetDirectories(directory, "*.*", SearchOption.AllDirectories);
            var completedDirs = 0;
            var totalDirs = allDirectories.Length + 1;
            foreach (var dir in allDirectories)
            {
                RaiseSearchDirectoryChanged(dir, totalDirs, completedDirs++);
                // Search 'dir' and its subdirectories for files that match the search pattern:
                SearchDirectory(dir, searchPattern);
            }
            // Include the Current Directory:
            RaiseSearchDirectoryChanged(directory, totalDirs, completedDirs++);
            SearchDirectory(directory, searchPattern);
        }
        else
            SearchDirectory(directory, searchPattern);
    }
    // ....
}
```

- 向发布者的事件处理程序添加订阅者，并调用 `Search`。

```csharp
class EventSample
{
    static FileSearcher AddEventHandler()
    {
        // ....
        fileLister.DirectoryChanged += (sender, eventArgs) =>
        {
            Console.Write($"Entering '{eventArgs.CurrentSearchDirectory}'.");
            Console.WriteLine($" {eventArgs.CompletedDirs} of {eventArgs.TotalDirs} completed...");
        };
        return fileLister;
    }
    static void Main(string[] args)
    {
        // 向发布者的事件系统添加处理程序
        var fileLister = AddEventHandler();
        // 创建组件功能，寻找所有后缀名为 .dll 的文件
        fileLister.Search(System.AppDomain.CurrentDomain.BaseDirectory, "*.dll", true);
    }
}
```

> 完整代码

```csharp
// 事件参数
public class FileFoundArgs : EventArgs
{
    public string FoundFile { get; }
    public FileFoundArgs(string fileName) => FoundFile = fileName;
    // 订阅者添加用来通信取消或中止事件系统处理程序的字段
    public bool CancelRequested { get; set; }
}
internal struct SearchDirectoryArgs  // 事件参数的设计不要求必须派生自 EventArgs
{
    internal string CurrentSearchDirectory { get; }
    internal int TotalDirs { get; }
    internal int CompletedDirs { get; }
    internal SearchDirectoryArgs(string dir, int totalDirs, int completedDirs)
    {
        CurrentSearchDirectory = dir;
        TotalDirs = totalDirs;
        CompletedDirs = completedDirs;
    }
}

// 事件发布者
public class FileSearcher
{
    // 发布者定义事件
    public event EventHandler<FileFoundArgs>? FileFound;
    private EventHandler<SearchDirectoryArgs>? _directoryChanged;
    internal event EventHandler<SearchDirectoryArgs> DirectoryChanged
    {
        add { _directoryChanged += value; }
        remove { _directoryChanged -= value; }
    }
    // 事件处理系统
    public void Search(string directory, string searchPattern, bool searchSubDirs = false)
    {
        if (searchSubDirs)
        {
            var allDirectories = Directory.GetDirectories(directory, "*.*", SearchOption.AllDirectories);
            var completedDirs = 0;
            var totalDirs = allDirectories.Length + 1;
            foreach (var dir in allDirectories)
            {
                RaiseSearchDirectoryChanged(dir, totalDirs, completedDirs++);
                // Search 'dir' and its subdirectories for files that match the search pattern:
                SearchDirectory(dir, searchPattern);
            }
            // Include the Current Directory:
            RaiseSearchDirectoryChanged(directory, totalDirs, completedDirs++);
            SearchDirectory(directory, searchPattern);
        }
        else
            SearchDirectory(directory, searchPattern);
    }
    private void SearchDirectory(string directory, string searchPattern)
    {
        foreach (var file in Directory.EnumerateFiles(directory, searchPattern))
        {
            FileFoundArgs args = RaiseFileFound(file);
            if (args.CancelRequested)  // 检查取消标志
                break;
        }
    }
    // 符合标准的事件委托
    private FileFoundArgs RaiseFileFound(string file)
    {
        var args = new FileFoundArgs(file);
        FileFound?.Invoke(this, args);
        return args;
    }
    private void RaiseSearchDirectoryChanged(string directory, int totalDirs, int completedDirs)
    {
        _directoryChanged?.Invoke(this, new SearchDirectoryArgs(directory, totalDirs, completedDirs));
    }
}
class EventSample
{
    // 添加订阅者
    static FileSearcher AddEventHandler()
    {
        var fileLister = new FileSearcher();
        EventHandler<FileFoundArgs> onFileFound = (sender, eventArgs) =>
        {
            Console.WriteLine(eventArgs.FoundFile);
            // 取消标志：在找到 test.dll 文件时取消事件处理系统
            if (Path.GetFileNameWithoutExtension(eventArgs.FoundFile) == "test")
                eventArgs.CancelRequested = true;
        };
        fileLister.FileFound += onFileFound;

        fileLister.DirectoryChanged += (sender, eventArgs) =>
        {
            Console.Write($"Entering '{eventArgs.CurrentSearchDirectory}'.");
            Console.WriteLine($" {eventArgs.CompletedDirs} of {eventArgs.TotalDirs} completed...");
        };
        return fileLister;
    }
    static void Main(string[] args)
    {
        // 向发布者的事件系统添加处理程序
        var fileLister = AddEventHandler();
        // 创建组件功能，寻找所有后缀名为 .dll 的文件
        fileLister.Search(System.AppDomain.CurrentDomain.BaseDirectory, "*.dll", true);
    }
}
```

<br>

#### 异步事件订阅

- 可以将事件订阅异步方法的事件处理程序，事件订阅者代码调用异步方法时，只能创建安全的 `async void` 方法。

```csharp
worker.StartWorking += async (sender, eventArgs) =>
{
    try
    {
        await DoWorkAsync();
    }
    catch (Exception e)
    {
        // Some form of logging.
        Console.WriteLine($"Async task failure: {e.ToString()}");
        // Consider gracefully, and quickly exiting.
    }
};
```

<br>

#### 委托与事件的区别

- 编译器为委托和事件都提供了一个后期绑定方案：它们通过调用仅在运行时绑定的方法组来进行组件之间的通信，所以委托和事件都支持单播和多播方式。它们都支持添加和删除处理程序的类似语法，拥有相同的方法调用语法 `T()` 或 `T?.Invoke`。
- 在确定要使用的语言功能时，在需要实现方法回调时，应使用基于委托的设计；若代码在不调用任何订阅服务器的情况下可完成其所有工作，应使用基于事件的设计。一般而言，当需要返回值时选择委托，委托的返回值可能会以某种方式影响算法；用于事件的委托一般是无返回类型。
- 只有包含事件的类型才能调用事件，以外的类型只能订阅或取消事件监听器。委托通常作为参数传递。
- 事件监听器通常具有较长的生存期，事件源可能会在程序的整个生存期内引发事件；而许多基于委托的设计，用作方法的参数进行传递，在返回该方法后不再使用此委托。

---
### 运算符

- 用户定义的类型可重载预定义的 C# 运算符，当一个或两个操作数都是某类型时，此类型可提供操作的自定义实现。
- 使用 `operator` 声明用户运算符定义，运算符必须是 `public static`。一元运算符有一个输入参数，二元运算符有两个输入参数。重载运算符时，参数列表至少有一个参数是类型 `T` 或 `T?`，`T` 是包含运算符声明的类型。

<br>

#### 可重载的运算符

- 算数运算符：一元 `++`、`--`、`+`、`-` 和二元 `*`、`/`、`%`、`+`、`-` 算术运算符。
- 逻辑运算符：一元 `!` 和二元 `&`、`|`、`^`。
- 比较运算符：二元 `<` 和 `>`、`<=` 和 `>=`、`==` 和 `!=`，成对的运算符需要同时重载。
- 位运算：一元 `~` 和二元 `&`、`|`、`^`。
- 移位运算符：二元 `<<`、`>>`、`>>>`，C#11 之前右操作数必须为 `int`，C#11 开始重载移位运算符的右侧操作数的类型可以是任意类型。
- 一元 `true` 和 `false` 运算符，只能返回 `bool` 类型。用户类型定义了 `&` 或 `|` 运算符重载时，可以使用相应的条件逻辑运算符 `&&` 或 `||`。

```csharp
readonly record struct Point(int X, int Y)
{
    // 一元运算符重载
    public static Point operator +(Point p) => new(+p.X, +p.Y);
    public static Point operator -(Point p) => new(-p.X, -p.Y);
    // 二元运算符重载
    public static Point operator +(Point l, Point r) => new(l.X + r.X, l.Y + r.Y);
    public static Point operator -(Point l, Point r) => new(l.X - r.X, l.Y - r.Y);
}
```

<br>

#### checked 用户定义算数运算符

- 重载算数运算符时，可以使用 `checked` 关键字定义该运算符的已检查版本。定义已检查的运算符时，还必须定义不带 `checked` 修饰符的相应运算符。
- `checked` 运算符在已检查的上下文中调用；不带 `checked` 修饰符的运算符在未检查的上下文中调用。

```csharp
readonly record struct Point(int X, int Y)
{
    public static Point operator checked +(Point l, Point r) => checked(new Point(l.X + l.X, r.Y + r.Y));
    public static Point operator +(Point l, Point r) => new(l.X + r.X, l.Y + r.Y);
}
```

<br>

#### 用户定义类型转换

- 可以在目标类型中使用 `operator` 和 `implicit` 或 `explicit` 关键字分别用于定义隐式转换或显式转换。

```csharp
Point p = (Point)(1, 2);
Console.WriteLine(p);
// Point { X = 1, Y = 2 }

(int, int) val = p;
Console.WriteLine(val);
// (1, 2)

readonly record struct Point(int X, int Y)
{
    // 可以隐式转换为元组
    public static implicit operator (int X, int Y)(Point p) => (p.X, p.Y);
    // 定义强制转换元组为 Point
    public static explicit operator Point((int X, int Y) p) => new(p.X, p.Y);
}
```

---
### 构造函数

- 每当创建类、结构或记录的实例时，将会调用其构造函数。构造函数之间可以形成重载。在创建一个新对象时，有多个操作在初始化新实例时进行：
  1. 实例字段设置为 0 位模式。通常由运行时来完成。
  2. 字段初始化设定项运行。派生程序最高类型的字段初始值设定项运行。
  3. 基类型字段初始值设定项运行，以直接基开始到 `System.Object` 的字段初始值设定项。
  4. 基实例构造函数运行。以 `System.Object.Object` 开始到直接基的任何实例构造函数。
  5. 实例构造函数运行。该类型的实例构造函数运行。
  6. 对象初始值设定项运行。如果表达式包含任何对象初始值设定项，后者会在实例构造函数运行后运行。对象初始值设定项按文本顺序运行。
  7. 若存在静态构造函数且尚未运行，静态构造函数会在任何实例构造函数操作执行之前运行，执行顺序从直接基开始到 `System.Object` 方向的任何已声明的静态构造函数。静态函数只会在首次使用类型时调用，仅调用一次。

```csharp
SubDerived d = new SubDerived();
/*
 Static SubDerived()...
 Static Derived()...
 Static Base()...
 Base()...
 Derived()...
 SubDerived()...
*/

class Base
{
    static Base() => Console.WriteLine("Static Base()...");
    public Base() => Console.WriteLine("Base()...");
}
class Derived:Base
{
    static Derived() => Console.WriteLine("Static Derived()...");
    public Derived() => Console.WriteLine("Derived()...");
}
class SubDerived : Derived
{
    static SubDerived() => Console.WriteLine("Static SubDerived()...");
    public SubDerived() => Console.WriteLine("SubDerived()...");
}
```

* 除非类是静态的，否则 C# 编译器将为无构造函数的类提供一个公共的无参数构造函数，以便该类可以实例化。可以将构造函数设置为受保护或私有构造函数，可以阻止类在外部被实例化。

```csharp
Sample s = new();  // ERROR: CS0122

class Sample{ private Sample(){} }
```

* 在 C#10 之前，结构类型不能包含显式无参数构造函数，因为编译器会自动提供一个。实例化结构时，结构中的所有内存初始化为默认值，值类型设置为 0，可为 null 类型和引用类型设置为 null。

```csharp
SampleStruct ss = new SampleStruct();

Console.WriteLine(ss.Value);                    // 0
Console.WriteLine(ss.ValueNullable is null);    // True
Console.WriteLine(ss.Name is null);             // True

struct SampleStruct
{
    public int Value;
    public int? ValueNullable;
    public string Name;
}
```

<br>

#### 实例构造函数

- 在执行构造函数之前总会先调用其直接基类的构造函数，可以使用 `base` 显式指定要调用的直接基类构造函数。在派生类中，如果不使用 `base` 关键字来显式调用基类构造函数，则将隐式调用无参数构造函数（若基类没有构造函数，编译器会自动为其提供公共无参构造）。
- 如果在某个类中声明至少一个实例构造函数，则 C# 不提供无参数构造函数。
- 若基类中存在非无参构造函数且没有提供无参构造函数时，派生类必须使用 `base` 显式调用基类构造函数。

```csharp
Derived d = new Derived(99);
// Base(99)...
// Derived(99)...
Derived d2 = new Derived();
// Base()...
// Derived()...

class Base
{
    protected Base() => Console.WriteLine($"Base()...");
    protected Base(int val) => Console.WriteLine($"Base({val})...");
}
class Derived:Base
{
    public Derived(int val):base(val) => Console.WriteLine($"Derived({val})...");
    public Derived() => Console.WriteLine($"Derived()...");
}

```

- 构造函数可以使用 `this` 调用同一对象中的另一个构造函数。

```csharp
Sample s = new();
// Sample(-1)...
// Sample()...

class Sample
{
    public Sample() : this(-1) => Console.WriteLine("Sample()...");
    public Sample(int val) => Console.WriteLine($"Sample({val})...");
}
```

> this 和 base

- `base` 关键字用于从派生类中访问基类的成员，常用于调用基类上已被其他方法重写的方法，或用于指定创建派生类实例时应调用的基类构造函数。`base` 仅允许基类访问构造函数、实例方法和实例属性访问器中进行。
- `this` 关键字指代类的当前实例，常用于限定类似名称隐藏的成员（`this.name = name`），或将对象作为参数传递给方法，或在类型中声明索引器，或引用构造函数。

```csharp
Derived d = new();
d.Func();
// Base.Func()...
// Derived.Func()...

d.SetName("Hello");
Console.WriteLine(d.Name);  // Hello

class Base
{
    public virtual void Func() => Console.WriteLine("Base.Func()...");
}
class Derived : Base
{
    public string Name { get; private set; }
    public override void Func()
    {
        base.Func();
        Console.WriteLine("Derived.Func()...");
    }
    public void SetName(string Name) => this.Name = Name;   
}
```

<br>

#### 主构造函数

- 从 C#12 开始，可以在类、结构和记录中声明主构造函数，将任何参数放在类型名称后面的括号中。主构造函数的位置参数位于声明类型的整个主体中，可以用于初始化属性或字段，或用作方法或局部函数中的变量，编译器将这些参数自动实现为私有字段（对于记录类型，编译器将合成一个与主构造函数参数同名的公共属性，若显式声明同名属性，位置参数则实现为私有字段），可以定义同名属性并使用位置参数作为初始化设定项。

```csharp
var sr = new SampleRecord(10, "Hello");
Console.WriteLine(sr.Name);

var sc = new SampleClass(99);
Console.WriteLine(sc.val);

record SampleRecord(int val, string Name)
{
    public int val { get; set; } = val;
}
class SampleClass(int val)
{
    public int val { get; } = val;
}
```

- 主构造函数指示这些参数对于类型的任何实例是必需的，任何显式编写的构造函数都必须使用 `this(...)` 初始化表达式语法来调用主构造函数。对于 `class` 和 `record class`，主构造函数存在时，编译器不会为其生成隐式的无参构造函数；对于 `struct` 和 `record struct`，在未显式指定时始终生成隐式的无参构造函数，主构造函数参数和所有的字段都初始化为 0 位模式。

```csharp
Sample s = new Sample();
Console.WriteLine(s.Name);  // Empty

class Base(int val);
class Sample(int val, string Name) : Base(val)
{
    public Sample() : this(0, "Empty")
    {
        Console.WriteLine("Sample()...");
    }
    public string Name { get; } = Name;
    public int Value { get => val; set => val = value; }
}
```

<br>

#### 静态构造函数

- 静态构造函数用于初始化任何静态数据，或执行仅需执行一次的特定操作。将在创建第一个实例或引用任何静态成员之前自动调用静态构造函数，静态构造函数最多调用一次。静态构造函数可以在 `class`、`struct`、`record` 类型中定义，最多只能有一个静态构造函数，为该声明类型独有，不能被继承或重载。
* 在创建类型实例之前，有多个操作在静态初始化时执行（若是首次使用类型时是引用任何静态成员而非创建实例时，则不会调用基类的静态构造函数）：
  1. 静态字段设置为 0 位模式。通常由运行时来完成。
  2. 静态字段初始化设定项运行。派生程序最高类型的静态字段初始值设定项运行。
  3. 基类型静态字段初始值设定项运行，以直接基开始到 `System.Object` 的静态字段初始值设定项。
  4. 基静态构造函数运行。以 `System.Object.Object` 开始到直接基的任何静态构造函数。
  5. 静态构造函数运行。该类型的静态构造函数运行。
  6. PS：


- 静态构造函数不能直接调用，并且仅应由公共语言运行时 CLR 调用。如果静态构造函数引发异常，运行时也不会再次调用该函数，并且类型在应用程序域的生存期内将保持未初始化，该异常将包装在异常中 `TypeInitializationException` ，并且无法实例化该类型，也无法调用该类型的任何静态成员。
* 声明为 `static readonly` 的字段可能仅被分配为初始化设定项或在静态构造函数中初始化。若不提供静态函数，则应该为 `static readonly` 字段提供初始化设定项。

```csharp
SubDerived d = new SubDerived();
/*
 Static SubDerived()...
 Static Derived()...
 Static Base()...
*/

class Base
{
    static Base() => Console.WriteLine("Static Base()...");
}
class Derived : Base
{
    static Derived() => Console.WriteLine("Static Derived()...");
}
class SubDerived : Derived
{
    static SubDerived() => Console.WriteLine("Static SubDerived()...");
}
```

<br>

#### 接口中的静态构造函数

- 接口中可以定义静态成员和静态构造函数（非静态抽象成员和虚拟成员），为接口类型所有。实现接口的类型无法访问这些成员。静态抽象成员和虚拟成员定义在泛型接口中。
- 接口静态构造函数在首次使用接口的任何静态成员时被调用，仅调用一次，且不会调用基接口的静态构造。使用派生类型时也无法调用接口的静态构造函数。

```csharp
SampleDerived s_class = new();
// static SampleDerived()...
// static Sample()...
ISampleDerived sd = s_class;
// 不会触发接口静态构造
Console.WriteLine(ISampleDerived.Name);
// static ISampleDerived()...
// ISampleDerived    // 不会触发基接口的静态构造
Console.WriteLine(ISample.Name);
// static ISample()...
// ISample

interface ISample
{
    static ISample() => Console.WriteLine("static ISample()...");
    public static string Name = "ISample";
}
interface ISampleDerived : ISample
{
    static ISampleDerived() => Console.WriteLine("static ISampleDerived()...");
    public new static string Name = "ISampleDerived";
}
class Sample : ISampleDerived
{
    static Sample() => Console.WriteLine("static Sample()...");
}
class SampleDerived : Sample
{
    static SampleDerived() => Console.WriteLine("static SampleDerived()...");
}
```

<br> 

#### 复制构造函数

- C# 记录类型为对象隐式提供复制构造函数。对于非密封记录，复制构造函数是 `protected` 的，密封记录和结构记录始终是 `private` 的。创建记录时使用 `with` 运算符时，自动调用它的复制构造函数。可以显式创建复制构造函数定义复制规则。

```csharp
// 调用记录的默认复制构造函数
SampleA sa = new SampleA([1, 2, 3, 4, 5]);
SampleA sa_copy = sa with { };

Console.WriteLine(string.Join(",",sa_copy.Arr));
sa.Arr[0] = 999;
Console.WriteLine(sa_copy.Arr[0] == sa.Arr[0]);  // True
// ---------------------------------------------------
// 调用记录的给定复制构造函数
SampleB sb = new SampleB([1, 2, 3, 4, 5]);
SampleB sb_copy = sb with { };
Console.WriteLine(string.Join(",", sb_copy.Arr));
sb.Arr[0] = 999;
Console.WriteLine(sb_copy.Arr[0] == sb.Arr[0]);  // False
Console.WriteLine(sb_copy.Arr[0]);  // 1

record SampleA(int[] Arr);
record SampleB(int[] Arr)
{
    // 显式定义复制构造函数
    protected SampleB(SampleB s)
    {
        Console.WriteLine("Copying SampleB...");
        Arr = new int[s.Arr.Length];
        Array.Copy(s.Arr, Arr, Arr.Length);
    }
}
```

> class 中的复制构造函数

- `with` 表达式无法用于非记录类型，非结构类型。对于类类型，可以自行编写复制构造函数。

```csharp
Person p1 = new("George", 40);
Person p2 = new(p1);
p2.Age = 39;

Console.WriteLine(p1);  // George is 40.
Console.WriteLine(p2);  // George is 39.

class Person(string name, int age)
{
    public string Name { get; set; } = name;
    public int Age { get; set; } = age;
    public Person(Person p) : this(p.Name, p.Age) { }
    public override string ToString() => $"{Name} is {Age}.";
}
```

---
### 终结器

- 终结器用于在垃圾回收器收集类实例时执行任何必要的最终清理操作。在大多数情况下，通过使用 `System.Runtime.InteropServices.SafeHandle` 或派生类包装任何非托管句柄，可以免去编写终结器的过程。
- 无法在结构中定义终结器，它们仅用于在堆中分配的托管类型的类，一个类只能有一个终结器。终结器无法被继承和重载，也不能被手动调用，由垃圾回收器调用。
- 程序员无法控制何时调用终结器，因为这由垃圾回收器决定。垃圾回收器检查应用程序不再使用的对象：如果它认为某个对象符合终止条件，则调用终结器（如果有），并回收用来存储此对象的内存。可以通过调用 `GC.Collect` 强制进行垃圾回收，但多数情况下应避免此调用，因为它可能会造成性能问题。

```csharp
public class Destroyer
{
    public override string ToString() => GetType().Name;
    ~Destroyer() => Console.WriteLine($"The {ToString()} finalizer is executing.");
}
```

- 拥有终结器的类型，编译器会为其构造一个 `Finalize()` 方法，并隐式调用 `object` 基类上的 `Finalize()`，此时无法创建或重写 `Finalize()` 方法。

```csharp
public class Destroyer
{
    public override string ToString() => GetType().Name;
    ~Destroyer() => Console.WriteLine($"The {ToString()} finalizer is executing.");

    // 对终结器的调用会隐式转换为
    protected override void Finalize(){
        try{
            // cleanup statements
            Console.WriteLine($"The {ToString()} finalizer is executing.");
        }finally{
            base.Finalize();
        }
    }
}
```

---
### 表达式

- 表达式是在操作数和运算符的基础之上构造而成。表达式的运算符指明了向操作数应用的运算，运算符的示例包括 `+`、`-`、`*`、`/` 和 `new`，操作数的示例包括文本、字段、局部变量和表达式。如果某个表达式包含多个运算符，则运算符的优先顺序控制各个运算符的计算顺序。如果操作数两边的两个运算符的优先级相同，那么运算符的结合性决定了运算的执行顺序：

- C# 提供了运算符，用于执行算术、逻辑、按位、移位运算、比较、类型转换、类型测试和模式匹配等。除了赋值运算符和 `null` 合并运算符之外，所有二元运算符均为左结合运算符，即从左向右执行运算。赋值运算符、null 合并 `??` 和 `??=` 运算符和条件运算符 `?:` 为右结合运算符，即从右向左执行运算。可以使用括号控制优先级和结合性。 
  
- 大部分运算符可重载。借助运算符重载，可以为一个或两个操作数为用户定义类或结构类型的运算指定用户定义运算符实现代码。

---
### 语句

- 程序操作使用语句进行表示。C# 支持几种不同的语句，其中许多语句是从嵌入语句的角度来定义的：
  - 声明语句用于声明局部变量和常量。
  - 表达式语句用于计算表达式。可用作语句的表达式包括方法调用、使用 `new` 运算符的对象分配、使用 `=` 和复合赋值运算符的赋值、使用 `++` 和 `--` 的递增和递减运算和 `await` 表达式。
  - 选择语句用于根据一些表达式的值从多个可能的语句中选择一个以供执行，例如 `if` 和 `switch` 语句。
  - 迭代语句用于重复执行嵌入语句，例如 `while`、`do`、`for` 和 `foreach` 语句。
  - 跳转语句用于转移控制权，例如 `break`、`continue`、`goto`、`throw`、`return` 和 `yield` 语句。
  - 异常处理语句 `try...catch` 用于捕获在代码块执行期间发生的异常，`try...finally` 语句用于指定始终执行的最终代码，无论异常发生与否。
  - `checked` 和 `unchecked` 语句用于控制整型类型算术运算和转换的溢出检查上下文。
  - `fixed` 语句可防止垃圾回收器重新定位可移动变量，并声明指向该变量的指针
  - `lock` 语句用于获取给定对象的相互排斥锁定，执行语句，然后解除锁定。
  - `using` 语句用于获取资源，执行语句，然后释放资源。

---
### 分部声明

- 分布声明用于拆分一个类、一个接口、一个结构或一个方法的定义到两个或更多的文件中。每个源文件包含类型或方法定义的一部分，编译应用程序时将所有的部分组合起来。若要拆分定义，则使用 `partial` 关键字修饰类型或方法。所有部分都必须使用 `partial` 关键字，各个部分都具有相同的访问修饰符。

```csharp
[Attr1, Attr2("hello")]
partial class A {}

[Attr3, Attr2("goodbye")]
partial class A {}

// 等效于
[Attr1, Attr2("hello"), Attr3, Attr2("goodbye")]
class A {}
```

  
<br>  

#### 分布类

- 常在以下几种情况下需要拆分类定义：
  - 处理大型项目时，使一个类分布于多个独立文件中可以让多位程序员同时对该类进行处理。
  - 当使用自动生成的源文件时，用户可以添加代码而不需要重新创建源文件。Visual Studio 在创建 Windows 窗体、Web 服务包装器代码等时会使用这种方法。用户可以创建使用这些类的代码，这样就不需要修改由 Visual Studio 生成的文件。
  - 使用源生成器在类中生成附加功能时。

* 可以为声明的分布类型的各个部分指定不同的基接口，声明不同的成员，应用不同的特性。在编译时，各个部分都合并起来形成最终的类型。在某一分部定义中声明的任何类、结构、接口和成员可供所有其他部分使用。最终类型是所有部分在编译时的组合。
* 要成为同一类型的各个部分的所有分部类型定义都必须在同一程序集和同一模块（`.exe` 或 `.dll` 文件）中进行定义。分部定义不能跨越多个模块。
* 泛型类型可以是分部的，每个分部声明都必须以相同的顺序使用相同的参数名。

```csharp
public partial class PartClass
{
    void FuncInline() { }
}

public partial class PartClass
{
    public void Func() => FuncInline();
}
```

<br>

#### 分部方法

- 分部类或结构可以包含分部方法。类的一个部分包含方法的签名，可以在同一部分或另一部分中定义实现。根据方法的签名，可能需要实现。对于无访问修饰符、无返回类型、不使用 `out` 参数的分部方法（可以包含 `static` 和 `unsafe` 修饰），不需要提供实现即可定义。如果未提供该实现，则会在编译时删除分部方法以及对该方法的所有调用。声明分部方法的实现前需要先声明其定义。

```csharp
public partial class PartClass
{
   partial void PartFunc1();
   partial void PartFunc2();
}
```

- 任何分部方法使用访问修饰符、有返回值、使用 `out` 参数或是 `sealed`、`abstract`、`virtual`、`new` 修饰的方法，都必须提供实现。分部方法允许类型的某个部分的实现者定义方法，类型另一部分的实现者提供这些方法的具体实现。此方法很有用：生成样板代码的模板和源生成器。
  - 模板代码：模板保留方法名称和签名，以便生成的代码可以调用方法。调用但不实现该方法不会导致编译时错误或运行时错误。
  - 源生成器：源生成器提供方法的实现。开发人员可以添加方法声明（通常由源生成器读取属性）和编写调用这些方法的代码。源生成器在编译过程中运行并提供实现。

* 分部类型的两个部分中的分部方法签名必须匹配。实现分部方法声明可以与相应的定义分部方法声明出现在同一部分中。
* 分部方法可以是泛型的。约束将放在定义分部方法声明上，但也可以选择重复放在实现声明上，不能声明不同的约束组合。参数和类型参数名称在实现声明和定义声明中不必相同。

```csharp
public partial class PartClass
{
    private partial void PartFunc1(out int a);
    static partial void PartFunc2<T>() where T: notnull;
}

public partial class PartClass
{
    private partial void PartFunc1(out int b) => b = default;
    static partial void PartFunc2<U>() { }
}
```

- 可以为已定义并实现的分部方法生成委托，但不能为已经定义但未实现的分部方法生成委托。

```csharp
public partial class PartClass
{
    static partial void PartFunc1();
    static partial void PartFunc2();
}

public partial class PartClass
{
    static partial void PartFunc1() { }

    Action Ac1 = PartFunc1;
 //   Action Ac2 = PartFunc2;  // ERROR : CS0762
}
```

---
### 异常和错误

