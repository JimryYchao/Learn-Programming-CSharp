# CSharp 程序构建基块

C# 中的类型主要由成员、表达式、语句等构建基块生成的。构成成员的类型一般为：常量、字段、方法、属性、索引器、事件、运算符、构造函数、终结器、嵌套类型等。表达式是在操作数和运算符的基础之上构造而成。程序操作使用语句进行表示。.NET 声明命名空间来整理类型。类型和成员可以使用特性进行信息标注。

---
## 1. 命名空间

C# 程序使用命名空间进行组织。命名空间既用作组织 “内部” 系统为一个程序，同时又向 “外部” 其他程序公开程序元素。使用 `namespace` 关键字声明命名空间，命名空间是隐式的 `public`，并且命名空间的声明不能包含任何访问修饰符。

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

声明的命名空间定义了包含一组相关对象的作用域，可以使用命名空间来组织代码元素并创建全局唯一类型。在命名空间内，可以声明 `class`、`interface`、`record`、`struct`、`enum`、`delegate`、嵌套命名空间。

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

>---

### 1.1. 全局命名空间

编辑器会添加一个默认未命名的命名空间，被称为全局命名空间，并存在于每个文件中。全局命名空间中的任何标识符都属于该空间，`global` 被定义为全局命名空间的别名，因此可以使用 `global::` 用于显式调用全局命名空间。

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

>---

### 1.2. 文件范围的命名空间

文件范围的命名空间声明能够作出以下声明：一个文件中的所有类型都在一个命名空间中。文件范围的命名空间不能包含其他命名空间声明。从 C#10 开始可使用文件范围的命名空间声明。

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

>---

### 1.3. using 指令

#### 1.3.1. using namespace 指令

`using` 引用指令将其他命名空间中包含的类型导入到封闭的编译单元或命名空间中，z在该范围内的成员声明中，可以直接引用给定命名空间中包含的类型（不包括嵌入的命名空间类型）。

```csharp
using System;
using System.Collections.Generic;

List<int> arrList = [0, 2, 4, 6, 8];
Console.WriteLine(string.Join(",", arrList));
```

使用 `using namespace_name` 导入的成员名称，会被编译单元或命名空间内相同名称的命名成员隐藏。

```csharp
namespace SampleSpace
{
    public class Sample;
}
namespace OtherSpace
{
    using SampleSpace;
    class Sample
    {
        SampleSpace.Sample SameNameType;
    }
}
```

#### 1.3.2. using 别名指令

使用 `using` 别名指令创建命名空间或类型的别名。命名空间别名限定符 `::` 可以保证类型名称查找不受新类型和成员引入的影响，例如名称歧义。

命名空间别名限定符始终出现在两个标识符（称为左侧和右侧标识符）之间。与常规 `.` 限定符不同，限定符的左侧标识符 `::` 仅作为 `extern` 或 `using` 别名进行查找。


```csharp
using Generic = System.Collections.Generic;
using Debug = System.Console;

Generic.List<int> arr = [0, 2, 4, 6, 8];
Debug.WriteLine(string.Join(",", arr));
// 命名空间别名限定符
Generic::List<int> arr2 = [0, 2, 4, 6, 8];
```

#### 1.3.3. using static 指令

使用 `using static` 指定一个类型，可以在当前文件范围内无需指定该类型名称即可访问其静态成员和嵌套类型。`using static` 将直接包含在类型声明中的嵌套类型和静态成员导入到立即封闭的编译单元或命名空间体中，从而在无限制的情况下使用每个成员和类型的标识符。扩展方法不能直接导入为静态方法，但是可用于扩展方法调用。

```csharp
using System;
using System.Collections.Generic;
using static System.Math;
using static System.Linq.Enumerable;

(int Quotient, int Remainder) ar = DivRem(58, 10);   // Math.DivRem
Console.WriteLine(ar);   // (5,8)

List<char> chars = "Hello, World".ToList();   // Linq.Enumerable 扩展方法
```

>---

### 1.4. 程序集外部别名

有时可能不得不引用具有相同的完全限定类型名称的程序集的两个版本或多个版本。通过使用外部程序集别名，可在别名命名的根级别命名空间内包装每个程序集的命名空间，使其能够在同一文件中使用。

例如在 Visual Studio 中向项目添加 `grid.dll` 和 `grid20.dll` 的引用。在依赖项程序集的 “属性” 选项卡，并将别名从 “全局” 分别更改为 “GridV1” 和 “GridV2”。使用 `extern` 导入这些别名：

```csharp
extern alias GridV1;  
extern alias GridV2;
```

然后就可以通过使用 `using` 别名指令为命名空间或类型创建别名。

```csharp
using Class1V1 = GridV1::Namespace.Class1;
using Class1V2 = GridV2::Namespace.Class1;
```

>---

### 1.5. 全局 using 指令

从 C#10 开始使用 `global using` 创建全局引用，`global` 这意味着 `using` 指令的作用范围从当前声明作用域扩大到所有文件，这包括全局命名空间引用、全局 `using` 别名、全局 `using static` 类型、全局 `extern alias`。

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

>---

### 1.6. 任何类型的别名

从 C#12 开始，可以使用 `using` 别名指令创建任何类型的别名，包括有元组类型、指针类型、数组类型、可为 null 的值类型等。类型不能是可为 null 的引用类型。

```csharp
<global> using <unsafe> Identifier = type;

global using Point = (int, int);
global using unsafe pFunVoid = delegate*<void>;
global using Number = int[];
using StrList = System.Collections.Generic.List<string?>;
using NullableInt = int?;

using str = string?; // err
```

---
## 2. 类型构建基块

用户可以定义类类型、结构类型、接口类型、委托类型、枚举类型：
- 类类型可以包含的成员有字段、常量、方法、属性、索引器、事件、实例构造函数、静态构造函数、运算符、终结器和嵌套类型。
- 结构类型可以包含的成员有字段、常量、方法、属性、索引器、事件、实例构造函数、静态构造函数、运算符和嵌套类型。
- 接口类型可以包含的成员有静态字段、常量、方法、属性、索引器、事件、静态构造函数、运算符、嵌套类型。
- 委托类型是引用一个或多个方法的数据结构，委托的声明定义了一个从 `System.Delegate` 派生的类。
- 枚举类型包含一组命名常量的值类型，每个枚举类型都有一个相应的整型作为其基础类型。
- 嵌套类型可以是任意的用户定义类型。

>---

### 2.1. 静态成员和实例成员

类型的成员要么是静态成员，要么是实例成员。静态成员被视为归属于类型本身，而实例成员视为属于类型实例。

静态成员（字段、方法、属性、事件、运算符或静态构造函数）的声明包含 `static` 修饰符。常量和嵌套类型声明隐式地声明为静态成员，静态成员具有：
  - 静态成员不对特定实例成员进行操作，因此无法在静态成员中示使用 `this`、`base`。
  - 静态成员直接通过类型本身进行访问和调用。
  - 非泛型类中静态成员仅标识一个存储位置，无论创建多少个非泛型类的实例，静态字段都只有一个副本。
  - 每个泛型类型的封闭构造类型都有自己的一组静态成员，与该封闭构造类型的实例数量无关。

```csharp
Sample.Output();  // 10010

class Sample
{
    static Sample()
    {
        Value = 10010;
    }
    static int Value;
    public static void Output()
    {
        Console.WriteLine(Value);
    }
}
```

实例成员（字段、方法、属性、事件、索引器、实例构造函数或终结器）的声明不包含 `static` 修饰符：
  - 类型的每个实例都包含该类型所有实例字段的单独集合。
  - 实例函数成员对类型的给定实例或实例成员进行操作，可以使用 `this`、`base`。
  - 实例成员可以使用类型中声明的静态成员字段、方法、属性、事件、运算符。

```csharp
Sample s1 = new(100);
Sample s2 = new(100000);

s1.Output();  // 100
s2.Output();  // 10010

class Sample
{
    static Sample()
    {
        MaxValue = 10010;
    }
    static int MaxValue;
    public Sample(int value) => _ = value > MaxValue ? S_Value = MaxValue : S_Value = value;
    public int S_Value { get; set; }
    public void Output()
    {
        Console.WriteLine(S_Value);
    }
}
```

>---

### 2.2. 保留的成员名称

为了方便底层 C# 运行时实现，对于每个作为属性、事件或索引器的源成员声明，实现应该根据成员声明的类型、名称和类型保留一些方法签名（某些使用 `get`、`set` 命名的保留成员）。如果程序声明的成员的签名与在同一作用域中声明的成员保留的签名相匹配，即使底层运行时实现没有使用这些保留，也会导致编译时错误。

保留名不应引入声明，因此它们不参与成员查找。声明关联的保留方法签名参与继承，并隐藏这两个签名。

> 保留名称的目的

- 允许底层实现使用普通标识符作为方法名，用于获取或设置对 C# 语言特性的访问。
- 允许其他语言使用普通标识符作为获取或设置访问 C# 语言特性的方法名进行互操作。
- 通过使保留成员名的细节在所有 C# 实现中保持一致，帮助确保一个符合标准的编译器接受的源代码被另一个编译器接受。

#### 2.2.1. 为属性保留的成员名

对于 `T` 类型的属性 `P`，保留成员签名：

```csharp
T get_P();
void set_P(T value);
```

无论属性是只读还是只写，这两个签名都是保留的，属性的访问器（编译器为其构造为保留成员）也无法被显式调用。

```csharp
class Sample
{
    public int P => 123;
}
class Derived : Sample
{
    public int get_P() => 456;
    static void Main(string[] args)
    {
        Derived A = new Derived();
        Sample B = A;

        Console.WriteLine(A.P);     // 123
        Console.WriteLine(B.P);     // 123
        Console.WriteLine(A.get_P()); // 456
        //Console.WriteLine(B.get_P()); // err 无法显式调用属性的访问器
    }
}
```

#### 2.2.2. 为事件保留的成员名

对于委托类型为 `D` 的事件 `E`，保留成员签名：

```csharp
void add_E(D handler);
void remove_E(D handler);
```

#### 2.2.3. 为索引保留的成员名

对于参数列表为 `PList` 的类型为 `T` 的索引器，无论索引器是只读还是只写的，保留成员签名：

```csharp
T get_Item(PList);
void set_Item(PList, T value);
```

编译器也会为索引器生成一个特殊名称的属性成员：

```csharp
T Item[PList];
```

#### 2.2.4. 为终结器保留的成员名

对于包含终结器的类型，保留成员签名：

```csharp
void Finalize();
```

---
### 2.3. 常量

常量是表示常数值的类型成员，可在编译时计算的值。常量声明中指定的类型应为 `sbyte`、`byte`、`short`、`ushort`、`int`、`uint`、`long`、`ulong`、`nint`、`nuint`、`char`、`float`、`double`、`decimal`、`bool`、`string`、枚举类型等。对于其他类型，可以使用 `readonly` 声明只读字段来达到常量的效果。

常量是在编译时设置其值并且不能更改其值的字段。使用 `const` 关键字声明的常量是不可变的值，在声明时初始化，在编译时是已知的，在程序的生命周期内不会改变。

程序运行时没有与常量相关联的变量地址，编译器在编译时直接将常量的文本值替换到它生成的中间语言 IL 代码中，因此 `const` 字段不能通过引用传递。

```csharp
Console.WriteLine("PI : " + Sample.MATH_PI);

class Sample
{
    public const double MATH_PI = 3.1415926535;
}
```

>---

### 2.4. 字段

字段是表示与对象或类型关联的变量的成员。字段是其包含类型的成员：
  - 使用静态修饰符 `static` 声明的字段定义的是静态字段。静态字段只指明一个存储位置，无论创建多少个类实例，永远只有一个静态字段副本。
  - 不使用静态修饰符声明的字段定义的是实例字段。每个类实例均包含相应类的所有实例字段的单独副本。

字段会在对象实例的构造函数被调用之前即刻初始化。如果构造函数分配了字段的值，则它将覆盖在字段声明期间给定的任何值。 

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

#### 2.4.1. volatile 字段

当字段的声明包含 `volatile` 修饰符时，该声明引入的字段是 `volatile` 字段。对于非易失性字段，重新排序指令的优化技术可能导致在没有同步访问字段的多线程程序中出现意想不到的和不可预测的结果。这些优化可以由编译器、运行时系统或硬件执行。对于 `volatile` 字段，这样的重新排序优化是受限的:
  
- 对 `volatile` 字段的读取称为易失性读取。易失读具有 “获取语义”，它保证发生在指令序列中对内存的任何引用之前。
- 对 `volatile` 字段的写入称为易失性写入。易失性写具有 “释放语义”，它保证发生在指令序列中写指令之前的任何内存引用之后。

这些限制确保所有线程都将按照执行顺序观察的任何其他线程执行的易失性写操作。符合标准的实现不需要提供从所有执行线程中看到的易失性写入的单一总顺序。

只能在类、结构、记录中声明易变字段，不能将局部变量声明为 `volatile`。可定义以下类型的易变字段：
  - 引用类型、指针类型、简单类型（`sbyte`、`byte`、`short`、`ushort`、`int`、`uint`、`nint`、`nuint`、`char`、`float` 和 `bool`）、枚举类型（不包含 `long` 和 `ulong`）、已知为引用类型的泛型类型参数、（即 `IntPtr` 和 `UIntPtr`）。
  - 其他类型（例如 `double`、`long` 等）无法标记为 `volatile`，因为对这些类型的字段的读取和写入不能保证是原子的，若要保护对这些类型字段的多线程访问，请使用 `Interlocked` 类成员或使用 `lock` 语句保护访问权限。

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

#### 2.4.2. readonly 字段

可以将字段声明为 `readonly` 只读字段。只读字段只能在初始值设定项、或在构造函数中初始化值，实例只读字段也可以在其包含类型中的所有属性的 `init` 访问器中设置。只读字段类似于常量，在退出构造函数时不能重新分配只读字段。

只读值类型字段包含数据，数据不可变；只读引用类型包含对其数据的引用，无法重新分配但是可以修改引用的状态。

```csharp
public class Color
{
    public static readonly Color Black = new Color(0, 0, 0);
    public static readonly Color White = new Color(255, 255, 255);
    public static readonly Color Red = new Color(255, 0, 0);
    public static readonly Color Green = new Color(0, 255, 0);
    public static readonly Color Blue = new Color(0, 0, 255);

    private byte red, green, blue;

    public Color(byte r, byte g, byte b)
    {
        red = r;
        green = g;
        blue = b;
    }
}
```

#### 2.4.3. required 字段

可以将字段或属性声明为 `required`。必需的成员必须由构造函数初始化，或者在创建对象时由对象初始值设定项初始化。`required` 成员可以在类、结构、记录中声明。必须初始化 `require` 的成员，可以将其初始化为 `null`，若必填成员没有初始化，编译器会发出错误。

必需成员必须至少与其包含类型一样的可访问性，如 `public` 类型不能包含 `protected` 的 `required` 成员。派生类不能隐藏（`new` 隐藏）在基类中声明的 `required` 成员。必需成员不能标记为 `Obsolete`，也不能声明位 `required readonly` 字段，因为必需字段必须可设置。

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
}
```

当类型参数包含 `new()` 约束时，不能将具有任何 `required` 成员的类型用作类型参数。编译器无法强制在泛型代码中初始化所有必需的成员。

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

> 结构的 required 字段

对于使用 `default` 或 `default(StructType)` 创建的结构类型的实例，不强制执行必需的成员。它们对使用 `new StructType()` 创建的结构实例强制执行，即使 `StructType` 没有无参数构造函数并且使用默认的结构构造函数也是如此。

```csharp
Sample s = new() {/*Name = "Hello"*/ }; // err
Sample s2 = default;  // okey

struct Sample
{
    public required string Name { get; set; }
}
```



> `SetsRequiredMembersAttribute` 构造函数

- `SetsRequiredMembers` 属性通知编译器构造函数设置了该类或结构中的所有 `required` 成员。编译器假定任何具有 `System.Diagnostics.CodeAnalysis.SetsRequiredMembersAttribute` 属性的构造函数都会初始化所有 `required` 成员。调用此类构造函数的任何代码都不需要对象初始值设定项来设置所需的成员。这主要用于位置记录和主构造函数。
- 任何构造函数链接到该特性标记的构造函数，都必须也设置 `SetsRequiredMembersAttribute` 特性。
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

- 特性类中 `required` 修饰的必需成员，必需在命名参数列表进行设置。

```csharp
class SampleAttribute : Attribute
{
    public required string Name { get; set; }
}

[Sample(Name = nameof(Example))]
class Example;
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

#### 2.4.4. ref 结构：ref 字段

可以在 `ref struct` 中声明 `ref` 引用字段或 `ref readonly` 只读引用字段。`ref` 字段可能具有 null 值，使用 `Unsafe.IsNullRef<T>(ref T src)` 方法确定 `ref` 字段是否为 `null`。

当 `readonly` 修饰 `ref` 字段时：
  - `ref`：在任何时候，都可以使用 `=` 为此字段关联引用赋值，或使用 `= ref` 重新赋值引用。
  - `ref readonly`：在任何时候，都不能使用 `=` 为此类字段关联引用赋值，但是可以使用 `= ref` 重新赋值引用。
  - `readonly ref`：只能在构造函数或 `init` 访问器中使用 `= ref` 重新赋值引用。可以在字段访问修饰符允许的任何时间点使用 `=` 为此字段关联引用赋值。 
  - `readonly ref readonly`：只能在构造函数或 `init` 访问器中通过 `= ref` 重新赋值引用。

```csharp
class DATA
{
    public static int F_Data = 0;
    public static int FR_Data = 0;
    public static int RF_Data = 0;
    public static int RFR_Data = 0;
}
ref struct Ref_Data
{
    public ref int F_Data;              // 表示引用可修改，值可修改
    public ref readonly int FR_Data;    // 表示引用可修改，值不可修改
    public readonly ref int RF_Data;    // 表示引用不可修改，值可修改
    public readonly ref readonly int RFR_Data;  // 表示引用和值均不可修改
    public Ref_Data()
    {
        // 可以在构造函数或 init 属性访问器中重新赋值
        F_Data = ref DATA.F_Data;
        RF_Data = ref DATA.RF_Data;
        FR_Data = ref DATA.FR_Data;
        RFR_Data = ref DATA.RFR_Data;
    }
}
```

>---

### 2.5. 方法

方法是可由对象或类型执行已实现定义的计算或操作的成员。方法包含一系列语句的代码块，程序通过调用该方法并指定任何所需的方法参数使语句得以执行。

在 C# 中，每个执行的指令均在方法的上下文中执行。方法可以分为静态方法（通过类进行访问）和实例方法（通过类实例进行访问）。

方法可能包含一个参数列表，这些参数表示传递给方法的值或变量引用。方法可能也包含一组类型参数，必须在调用方法时指定类型自变量（泛型方法）。

方法具有返回类型，它用于指定方法计算和返回的值的类型。如果方法无返回值，则它的返回类型设置为 `void`。如果方法是 `async`，它的有效返回类型是 `Task`、`Task<TResult>`、`ValueTask`、`ValueTask<TResult>` 之一，只有在方法作为异步事件处理程序的启动时，`async` 方法可以返回 `void`。


```csharp
// 方法主体是由若干条语句组成的块
public void Print(string message)
{
    Console.WriteLine("DEBUG: " + message);
}
// 方法主体是单个表达式语句的表达式主体形式
public void Print(string message) => Console.WriteLine("DEBUG: " + message);
```

#### 2.5.1. 静态和实例方法

使用 `static` 修饰符声明的方法是静态方法。静态方法不对特定的实例起作用，只能直接访问静态成员。未使用 `static` 修饰符声明的方法是实例方法。实例方法对特定的实例起作用，能够访问静态和实例成员。

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

#### 2.5.2. 方法参数

方法的参数可以是值参数（无参数修饰）、输入参数（`in`）、引用参数（`ref`）、输出参数（`out`）、只读参数（`ref readonly`）、参数数组（`params`）。`scoped` 注释的参数的安全转义范围（`ref struct` 类型的参数）或引用转义范围（`ref`、`in`、`ref readonly` 参数）是当前方法，`[UnscopedRef]` 注释的参数的安全转义范围或引用转义范围比默认的类别高一级。

在 C# 中，实参可以按值或按引用传递给形参。按值传递是将变量副本传递给方法，按引用传递（使用 `in`、`ref`、`out`、`ref readonly` 修饰方法参数）是将对变量的访问传递给方法。

> 参数调用点使用规则

- 参数修饰符

| CallSite annotation | `ref` parameter | `ref readonly` parameter | `in` parameter | `out` parameter |
| ------------------- | --------------- | ------------------------ | -------------- | --------------- |
| `ref`               | Allowed         | **Allowed**              | **Warning**    | Error           |
| `in`                | Error           | **Allowed**              | Allowed        | Error           |
| `out`               | Error           | **Error**                | Error          | Allowed         |
| No annotation       | Error           | **Warning**              | Allowed        | Error           |

```csharp
class Sample
{
    void Fun(in int v, ref int v2, ref readonly int v3, out int v4)
    {
        v4 = default(int);
    }
    void Test(int v)
    {
        // ref-readonly: 'ref v'
        Fun(in v, ref v, ref v, out v);  
        // in/ref-readonly: 'v'
        Fun(v, ref v,  v, out v);    
        // in: 'ref v'; ref-readonly: 'in v'
        Fun(ref v, ref v, in v, out v);  
    }
}
```

- 实参表达式

| Value kind | `ref` parameter | `ref readonly` parameter | `in` parameter | `out` parameter |
| ---------- | --------------- | ------------------------ | -------------- | --------------- |
| *Rvalue*     | Error           | **Warning**              | Allowed        | Error           |
| *Lvalue*     | Allowed         | **Allowed**              | Allowed        | Allowed         |

```csharp
class Sample
{
    void Fun(in int v, ref int v2, ref readonly int v3, out int v4)
    {
        v4 = default(int);
    }
    void Test(int v)
    {
        // All paramters are Lvalue
        Fun(in v, ref v,  ref v, out v); 
        // in, ref-readonly are Rvalue
        Fun(10010, ref v,  10086, out v); 
    }
}
```

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

#### 2.5.3. 方法参数修饰符

- `ref` 指定此实参表达式为引用传递：传递的 *Lvalue* 必须明确分配。方法内可以重新赋值。
- `in` 指定此实参表达式为引用传递：传递的值（可以是左值或右值表示式）必须明确分配。方法内无法重新赋值。
- `out` 指定此实参表达式为引用传递：传递的 *Lvalue* 无需明确分配。方法内必须分配该变量的值。
- `ref readonly` 指定此实参表达式为引用传递：传递的值（可以是左值或右值表示式）必须明确分配。方法内无法重新赋值。
- 引用传递参数都可以通过 `= ref` 重新分配引用。
- `params` 指定此参数采用可变数量的参数，可变参数只能是参数列表中的最后一位。

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

> params 方法的重载决策

方法查找更倾向于先是参数列表一致的方法，未找到时查找参数数组方法或扩展方法。

```csharp
class Sample
{
    static void Fun(params int[] args)
    {
        Console.WriteLine("Fun(params args) invoked");
    }
    static void Fun(int v1, int v2)
    {
        Console.WriteLine("Fun(v1, v2) invoked");
    }
    static void Main(string[] args)
    {
        Fun();              // "Fun(params args) invoked"
        Fun(1);        // "Fun(params args) invoked"
        Fun(1,2);           // "Fun(v1, v2) invoked"
        Fun(1,2,3);    // "Fun(params args) invoked"
    }
}
```

参数数组的值可以是 `null`。

```csharp
class Sample
{
    static void Fun(params string[] args)
    {
        Console.WriteLine(args == null);
    }
    static void Main(string[] args)
    {
        Fun();              // false, args 是零元素数组
        Fun(null);     // true
        Fun((string)null);    // (string)null 相当于 new string(null), 方法相当于 Fun(new string[]{null})
    }
}
```

对于 `object[]`，方法的正常形式可能与单个对象参数的扩展形式之间产生歧义，`object[]` 作为单个参数存在时本身可以隐式转换为 `object`。因此可以使用强制转换类解决歧义。

```csharp
class Test
{
    static void F(params object[] args)
    {
        foreach (object o in args)
        {
            Console.Write(o.GetType().FullName);
            Console.Write(", ");
        }
        Console.WriteLine();
    }
    static void Main()
    {
        object[] a = { 1, "Hello", 123.456 };
        object o = a;
        F(a);             // System.Int32, System.String, System.Double,
        F((object)a);     // System.Object[],
        F(o);             // System.Object[],
        F((object[])o);   // System.Int32, System.String, System.Double,
    }
}
```

#### 2.5.4. 命名参数

带有参数名称的参数被称为命名参数，没有名称的参数是位置参数。通过命名实参，可以为形参指定实参，方法是将实参与该形参的名称匹配。

使用命名实参，就不再需要将实参的顺序与所调用方法的形参列表中的形参顺序相匹配。每个参数都可以使用命名参数。当位置参数位于位置错误的命名参数或命名 `params` 参数之后，将发生编译时错误。

允许在非尾随位置使用命名参数，只要它们在正确的位置，例如 `DoSomething(isEmployed:true, name, age)`。

尾随位置参数后的命名参数可以不与方法形参位置相匹配。

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

        // Named arguments behind of positional arguments can be supplied for
        // the parameters in any order.
        PrintOrderDetails("Gift Shop", productName: "Red Mug", orderNum: 31);

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

#### 2.5.5. 可选参数

方法、构造函数、索引器或委托的定义可以指定其形参为必需还是可选。任何调用都必须为所有必需的形参提供实参，但可以为可选的形参省略实参。每个可选参数都有一个默认值，可以是常量表达式、`new ValType()` 值类型或 `default`。

在分部方法实现、显式接口成员实现、单参数索引器声明、运算符声明中使用可选参数，编译器会生成警告，这些成员永远不能允许以省略实参的方式调用。

可选参数列表定义必须定义参数列表的末尾和必须参数之后。被提供实参的可选参数之前的全部可选参数都必须提供相应的实参。

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

#### 2.5.6. 方法主体和局部变量

方法主体指定了在调用方法时执行的语句。方法主体可以声明特定于方法调用的局部变量，局部变量在使用前必须先明确赋值，可以稍后的语句中延迟赋值。

方法使用 `return` 语句将控制权返回给调用方。对于无返回类型的 `void` 方法，`return` 语句可省略。

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

#### 2.5.7. 方法重载

借助方法重载，同一类中可以有多个同名的方法，只要这些方法具有唯一签名即可。编译器使用重载决策（查找与自变量匹配度最高的）来确定要调用的特定方法，并在找不到任何最佳匹配项时报告错误。

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

#### 2.5.8. 虚方法、重写方法、抽象方法和密封方法

可使用虚方法（`virtual`）、重写方法（`override`）、抽象方法（`abstract`）、密封方法（`sealed`）等实例方法来定义类类型层次的行为。虚方法和抽象方法不能声明在密封 `sealed` 类中：
  - 虚方法是在基类中声明和实现的方法，一般定义缺省行为，其中任何派生类都可提供更具体的实现。
  - 重写方法是在派生类中实现或重写的方法，可重写从基类继承的具有相同签名的虚方法或抽象方法，重写方法可以被密封 `sealed`，可以使用 `base` 访问隐藏的继承方法。
  - 抽象方法是在抽象基类中声明的方法，必须在派生类中提供实现，它是隐式的虚方法。抽象方法不在抽象基类中定义实现。`base` 无法引用基类的 `abstract` 方法。
  - 密封方法重写从基类继承的虚方法、重写方法，并防止此类方法在派生类型中进一步重写。

方法的调用总是在类型对象中查找最派生的方法实现。

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
    // 重写父类的虚方法，并密封
    public sealed override void Print(string message) => Console.WriteLine("SON DEBUG : " + message);
}
```

结构类型可以重写继承 `object` 的虚方法，例如 `ToString`。由于结构类型已被密封，因此不能将方法声明为 `virtual`、`abstract`、`sealed`。结构中的方法是隐式 `sealed` 的。

```csharp
struct Sample
{
    public override string ToString() { ... } 
}
```

#### 2.5.9. 外部方法

`extern` 修饰符用于声明在外部实现的方法，通常使用 C# 以外的语言。常见用法是在使用 `Interop` 服务调入非托管代码时与 `DllImport` 特性一起使用。必须将方法声明为 `static`。

```csharp
//using System.Runtime.InteropServices;
class ExternTest
{
    [DllImport("User32.dll", CharSet=CharSet.Unicode)]
    public static extern int MessageBox(IntPtr h, string m, string c, int type);

    static int Main()
    {
        string myString;
        Console.Write("Enter your message: ");
        myString = Console.ReadLine();
        return MessageBox((IntPtr)0, myString, "My Message Box", 0);
    }
}
```

#### 2.5.10. 局部函数

局部函数是一种嵌套在另一个函数成员中的方法，且仅能从其包含成员中调用它，因此局部函数不包含任何访问修饰符。可以在方法、构造函数、属性访问器、事件访问器、匿名方法、Lambda 表达式、终结器、其他局部函数中声明和调用局部函数。在相同作用域下声明的局部函数无法声明同名的重载函数。

可以声明 `async`、`unsafe`、`static`、`extern static` 修饰的局部函数。从 C#9 开始，可以将特性应用于局部函数、其参数和类型参数。`extern` 表示局部函数是外部的。

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

使用 `Conditional` 特性标记的局部方法也只能是静态的。

```csharp
#undef Conditonal

using System.Diagnostics;
using System.Reflection;

public class Sample
{
    public static unsafe void Fun(out Delegate inline)
    {
        // 调用被忽略
        funInline("Hello World");

        // 反射获取内联方法
        var t = typeof(Sample);
        foreach (var me in t.GetRuntimeMethods())
        {
            if (me.Name.Contains("funInline"))
            {
                Console.WriteLine(me.Name);
                inline = me.CreateDelegate<Action<string>>(target: null);
                return;
            }
        }
        inline = null;

        [Conditional("Conditonal")]
        static void funInline(string mess)
        {
            Console.WriteLine(mess);
        }
    }
    static void Main(string[] args)
    {
        Fun(out Delegate de);
        de?.DynamicInvoke("Reflection funInline");
    }
}
```

> 局部函数与 Lambda 表达式的区别

Lambda 表达式是在运行时声明和分配的对象，使用前必须对其进行明确赋值，并在声明时转换为委托；局部函数在编译时定义，若只是通过调用方法一样调用局部函数而不捕获封闭范围中的变量时，局部函数不会转换为委托，只有在局部函数中明确分配了封闭范围的变量时，局部函数将作为委托类型实现。

局部函数可以避免 Lambda 表达式始终需要的堆分配。若局部函数不会转化为委托时，并且局部函数捕获的变量不会被其他转换为委托的 Lambda 或局部函数捕获，则编译器可以避免堆分配。

除非将局部函数转换为委托，否则捕获是在值类型的帧中完成的，这意味着使用带有捕获功能的局部函数不会产生任何 GC 压力。

```csharp
public static int LocalFunctionFactorial(int n)
{
    return nthFactorial(n);
    // 局部函数编译时定义
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
    // 局部函数捕获封闭范围中的 y 并明确分配，此时局部函数作为局部函数实现。编译器将为其堆分配。 
    void LocalFunction() => y = 0;
}
```

> 外部变量的 CIL 实现

C# 编译器为捕捉外部变量的匿名方法或局部函数创建一个闭包（*closure*），它是一个数据结构，被捕获的局部变量作为实例字段在闭包中实现，从而延长了其生存期，所有使用局部变量的地方都改为使用闭包中的那个字段：
- 对于转换为委托的匿名方法或局部函数，编译器为其构造一个类类型的闭包。
  
```csharp
// CSharp
class Sample
{
    int value;
    delegate void SampleDelegate();
    static SampleDelegate StaticFun(Sample s)
    {
        int v = 0;
        SampleDelegate? result = delegate { Console.WriteLine(++s.value + v); };
        return result;
    }
    static void Main()
    {
        Sample s = new Sample();
        var de = Sample.StaticFun(s);
        de?.Invoke(); // 1
        de?.Invoke(); // 2
        de?.Invoke(); // 3
    }
}
// MSIL to C#
internal class Sample
{
    // result 的闭包类型 
	[CompilerGenerated]
	private sealed class <>c__DisplayClass2_0
	{
		public Sample s;
		public int v;
		internal void <StaticFun>b__0()  // 匿名函数
		{
			Console.WriteLine(++s.value + v);
		}
	}
    [System.Runtime.CompilerServices.NullableContext(1)]
	private static SampleDelegate StaticFun(Sample s)
	{
		<>c__DisplayClass2_0 <>c__DisplayClass2_ = new <>c__DisplayClass2_0();  // 闭包调用
		<>c__DisplayClass2_.s = s;
		<>c__DisplayClass2_.v = 0;
		return new SampleDelegate(<>c__DisplayClass2_.<StaticFun>b__0);
	}
    // ... rest members
}
```
  
- 对于捕获外部变量的局部函数，且未转换为委托类型时，编译器为其构造一个结构类型的闭包，并相应的在包含类型中为其构造一个私有方法，并将闭包类型作为引用参数进行传递。

```csharp
// CSharp
class Sample
{
    int value;
    void Fun()
    {
        int x = 0;
        fun();      // 未转换为委托的局部函数调用
        void fun() =>     // 局部函数声明，捕获 this
            Console.WriteLine(++this.value + x++);
    }

    static void Main()
    {
        Sample s = new Sample();
        s.Fun();  // 1
        s.Fun();  // 2
        s.Fun();  // 3
    }
}
// MSIL to C#
internal class Sample
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct <>c__DisplayClass1_0   // 捕获外部变量且未转换委托的局部函数的闭包结构
	{
		public Sample <>4__this;
		public int x;
	}

	private void Fun()
	{
		<>c__DisplayClass1_0 <>c__DisplayClass1_ = default(<>c__DisplayClass1_0);   // 闭包调用
		<>c__DisplayClass1_.<>4__this = this;
		<>c__DisplayClass1_.x = 0;
		<Fun>g__fun|1_0(ref <>c__DisplayClass1_);
	}

	[CompilerGenerated]
	private void <Fun>g__fun|1_0(ref <>c__DisplayClass1_0 P_0)  // 提升局部函数为包含类型的私有函数成员
	{
		Console.WriteLine(++value + P_0.x++);
	}
    // rest members ...
}
```

- 未捕获外部变量的局部函数，CIL 为在它的外部包含类型中为其构造为一个私有方法，并在声明局部函数的地方调用该构造的方法。

```csharp
// CSharp
class Sample
{
    static void Main()
    {
        funInline();
        void funInline()
        {
            Console.WriteLine("=== End ===");
        }
    }
}
// MSIL to C#
internal class Sample
{
	private static void Main()
	{
		<Main>g__funInline|0_0();   // 不包含闭包的局部函数调用
	}

	[CompilerGenerated]
	internal static void <Main>g__funInline|0_0()  // 提升局部函数为包含类型的私有函数成员
	{
		Console.WriteLine("=== End ===");
	}
}
```

#### 2.5.11. 方法协变返回

允许方法的重写声明比它重写的方法更派生的返回类型。

```csharp
class Base
{
    public virtual Base Create() => new Base();
}
class Derived : Base
{
    public override Derived Create() => new Derived();
}
```

#### 2.5.12. 静态类：扩展方法

扩展方法可以对现有类型 “添加” 方法，而无需创建新的派生类型、重新编译或以其他方式修改原始类型。扩展方法是一种静态方法，只能在非泛型、非嵌套的顶级静态类中声明，但可以像扩展类型上的实例方法一样进行调用。扩展方法的第一个参数包含 `this` 修饰，指定方法扩展的类型对象：
  - 只有当参数具有值类型时，才可以使用 `in` 修饰符。
  - 只有当参数具有值类型、或是 `struct` 约束的泛型类型时，才可以使用 `ref` 修饰。
  - 它不能是指针类型。

在代码中，可以使用实例方法语法调用该扩展方法。编译器生成的中间语言 IL 会将代码转换为对静态方法的调用。扩展方法并未真正违反封装原则，它无法访问方法所扩展的类型中的专用变量。

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
    // ref 修饰
    public static void Increment(ref this int number) => number++;
}
```

可以使用扩展方法来扩展类或接口，但不能重写扩展方法。与接口或类方法具有相同名称和签名的扩展方法永远不会被调用。编译时，扩展方法的优先级总是比类型本身中定义的实例方法低。

当编译器遇到方法调用时，它首先在该类型的实例方法中寻找匹配的方法；如果未找到任何匹配方法，编译器将搜索为该类型定义的任何扩展方法，并且绑定到它找到的最匹配的扩展方法。

#### 2.5.13. ref 方法返回

方法的返回类型可以使用 `ref` 修饰或 `ref readonly`，表示该方法返回一个变量引用：
- `ref` 返回变量的引用，可以重新分配（`= ref`）或赋值修改（`=`）引用变量。
- `red readonly` 返回只读的变量引用，无法赋值修改（`=`）引用变量，可以重新分配（`= ref`）引用。
- 可以将 `ref` 方法返回值分配给 `ref readonly` 变量。
- `ref readonly` 方法返回值可以作为 `in` 参数传递（不能是 `ref`、`out` 参数）。  

```csharp
class Sample
{
    public int value;
    class RefSampleClass(Sample s)
    {
        public int GetValue() => s.value;
        public ref int GetRefValue() => ref s.value;
        public ref readonly int GetRefReadonlyValue() => ref s.value;
    }
    static void Main(string[] args)
    {
        Sample s = new() { value = 10010 };
        RefSampleClass refSample = new(s);
        int local = s.value;

        int v = refSample.GetValue();
        ref int rv = ref refSample.GetRefValue();
        ref readonly int rcv = ref refSample.GetRefReadonlyValue();

        // ref readonly 作为 ref 方法的接收方
        ref readonly int rv_readonly = ref refSample.GetRefValue();

        v = 10;  // 修改值
        Console.WriteLine(s.value);   // 10010
        rv = 100;  // 修改值引用
        Console.WriteLine(s.value);   // 100

        // s.value = local;
        // rv_readonly = 99;  // err，无法赋值修改
        // rcv = 10086;  // err，无法赋值修改

        rv_readonly = ref rcv;  // 允许重新分配
    }
}
```

#### 2.5.14. 结构：readonly 方法

结构类型中可以声明 `readonly` 修饰的实例方法，表明该实例方法无法修改 `this` 的实例字段。无法在类中声明 `readonly` 方法。

```csharp
class Sample
{
    public int value;
    struct RefSampleStruct(Sample s)
    {
        int s_value;
        
        // ref 返回方法
        public int GetValue() => s.value;
        public ref int GetRefValue() => ref s.value;
        public ref readonly int GetRefReadonlyValue() => ref s.value;
        
        // readonly 方法
        public readonly int GetValue_R()
        {
            //this.s_value = 100;  // err
            return s.value;
        }
        public readonly ref int GetRefValue_R() => ref s.value;
        public readonly ref readonly int GetRefReadonlyValue_R() => ref s.value;
    }
}
```

>---

### 2.6. 属性

属性提供对对象或类特征的访问，不表示存储位置。属性是类、结构或记录中可以像字段一样访问的方法。属性可以为字段提供保护，以避免字段在对象不知道的情况下被更改。

在属性中可以组合实现 `get` 读访问器、`set` 写访问器或 `init` 构造访问器，这些访问器可以具有不同的访问级别，访问器的可访问性必须比属性本身具有更强的限制。`value` 用于定义由 `set` 或 `init` 访问器分配的值。

在类中，属性可以被声明为 `virtual` 或 `abstract`，以允许在派生类中进行重写。属性的修饰符组合和方法规则相同。

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

#### 2.6.1. 属性访问器

属性的访问器可以是 `get` 读访问器、`set` 写访问器、`init` 初始化访问器中的一个，也可以是 `get` / `set` 或 `get` / `init` 组合。静态属性不包含 `init` 访问器。

只读 `get` 属性仅在包含类型的实例构造函数中初始化属性值。

```csharp
struct Sample
{
    public Sample(int value)
    {
        this.Value = value;   // 仅在实例构造函数中可设置
    }
    public int Value { get; }
}    
```

`set` 访问器可以在其可访问域中对属性进行设置。

```csharp
Sample s = new Sample { Value1 = 100 };  // 对象初始化项
s.Value1 = 1000;  // 外部访问

class Sample
{
    public Sample()
    {
        Value1 = 1;
        Value2 = 2;
        Value3 = 3;
    }
    public int Value1 { get; set; }
    public int Value2 { get; protected set; } // 包含或派生类型内可设置
    public int Value3 { get; private set; }  // 包含类型内可设置
}
class Derived : Sample
{
    public Derived()
    {
        Value1 = 10;  // 派生内
        Value2 = 20;  // 派生内
        Value3 = 30;  //err
    }
}
```

`init` 访问器可以在对象初始化项中、`with` 表达式初始化项中、包含类型（`this`）或派生类型（`base`）的实例构造函数中、在所有属性的 `init` 初始化访问器中、带有命名参数的内部属性用法（例如特性命名参数列表）被认为是可设置的。

```csharp
Sample sample = new Sample { Value = 10010 };  // 对象初始化项

Sample s2 = sample with { Value = 10086 };  // with 表达式

struct Sample
{
    public Sample(int value)
    {
        this.Value = value;   // 在实例构造函数中可设置
    }
    public int Value { get; init; }
    public int PropInit
    {
        get => 0;
        init
        {
            Value = 10010;  // 其他属性的 init 中
        }
    }
}
```

任何实例只读字段也可以在所有属性的 `init` 访问器中赋值初始化，且仅限于其相同的包含类型。

```csharp
struct Sample
{
    public int Value { get => s_value; init => s_value = value; }

    private readonly int s_value;
}
```

任何自动实现的属性都可以包含一个初始值设定项。具有实例字段初始值设定项的结构类型需要显式声明一个实例构造函数。

```csharp
class Sample
{
    public int Value { get; init; } = 0;
    public int ReadOnlyValue { get; } = 10010;
    public int VariableValue { get; set; } = 10086;
}
```

#### 2.6.2. 如何实现不可变属性

- 仅声明 `get` 访问器的属性只能在构造函数中可变，无法在对象初始化设定项中重新分配。
- 声明 `get` 和 `init` 访问器的属性在实例构造函数中可变，也可以在对象初始化设定项中重新分配。
- 表达式主体形式的属性声明仅由一个 `get` 访问器构成，且无法在任何地方修改。
- 声明 `get` 和 `private set` 访问器的属性只能在该包含类型中设置，对于外部则不可变。

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

#### 2.6.3. 自动实现的属性

当属性访问器中不需要任何其他逻辑时，自动实现的属性会使属性声明更加简洁。编译器会为其自动创建仅可以通过该属性的 `get` 和 `set` 访问器访问的专用、匿名支持字段。自动实现的属性必须具有 `get` 访问器。

```csharp
Sample s = new() { GUI = 10010 };
s.FullName = "Hello World";

class Sample
{
    public required int GUI { get; init; } = -1; // 设置初始化值
    public string FullName { get; set; }
}
```

自动实现的属性，编译器会为其提供一个隐藏的后台字段，该字段不可访问，只能通过与之关联的属性访问。但该隐藏字段可以通过自动实现的属性应用目标特性。

```csharp
[Serializable]
public class SampleData
{
    [field: NonSerialized]
    public string MySecret { get; set; } = "Hello";
}
```

#### 2.6.4. 虚属性、重写属性、抽象属性和密封属性（类专属）

类可以像方法一样声明属性版本的虚属性、重写属性、抽象属性和密封属性，它们和类似方法的行为完全相同。

```csharp
abstract class Base
{
    // 抽象属性
    protected abstract int Value { get; set; }
    // 虚属性
    public virtual int DeCounter => Value--;
}
class Derived : Base
{
    // 抽象属性实现
    protected override int Value { get; set; } = 100;
    // 密封的重写属性
    public sealed override int DeCounter => Value > 0 ? base.DeCounter : 0;
}
```

属性可以只重写基属性已声明的访问器的其中一个。同方法的协变返回，只读属性的重写声明可以是比基属性类型的更派生类型。

```csharp
class Base
{
    public virtual Base Instance { get; set; }
}
class Derived : Base
{
    // 重写基属性的只读访问器，并返回更派生的类型
    public override Derived Instance => new Derived();  
}
```

#### 2.6.5. 接口属性

接口中声明的实例属性不能具有初始化设定项。接口中声明的实例属性，编译器不会为其创建自动实现，需要在派生类型中实现或自动属性实现。除非在接口中为声明的属性提供默认实现，此时该属性只能通过接口实例访问，除非在派生类型中重新实现。

添加非 `public` 访问修饰符的实例属性只能在派生类型中显式实现，除非在派生类型中重新声明为 `public`。

接口中的静态属性可以设定初始化值，且是可以自动实现的。

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

派生类型默认实现接口属性时（非显式接口实现），可以为其额外添加的访问器，非公共的属性可以重新声明为 `public`，此时将覆盖原有的默认实现定义。

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

#### 2.6.6. ref 属性

使用 `ref` 修饰的属性表示按引用返回属性关联的值，`ref` 属性不能包含 `set` 和 `init` 访问器。可以在类或结构中声明 `ref` 属性，但是必须提供实现，按引用返回的属性无法自动实现。

```csharp
class Sample
{
    public int value;
    public class RefSampleClass(Sample s)
    {
        public int Value => s.value;
        public ref int rValue => ref s.value;
        public ref readonly int rcValue => ref s.value;
    }
}
```

#### 2.6.7. readonly 属性（结构专属）

可以将 `readonly` 应用于结构的属性访问器，以指示实例成员不会在该访问器中改变。也可以将 `readonly` 修饰整个属性，以表示所有的访问器都是 `readonly`，该属性不能是自动实现。

`readonly` 无法修饰 `class` 非字段成员。

```csharp
class Sample
{
    public int value;
    public struct RefSampleStruct(Sample s)
    {
        public readonly int R_Value => s.value;
        public readonly ref int R_rValue => ref s.value;
        public readonly ref readonly int R_rcValue => ref s.value;
    }
}
```

`readonly` 可以应用于某些自动实现的属性，但它不会产生有意义的效果。无论是否存在关键字，编译器都将所有自动实现的 `get` 访问器视为只读 `readonly`。`init` 访问器不能标记为 `readonly`。

```csharp
// Allowed
public readonly int Prop1 { get; }
public int Prop2 { readonly get; init;}  
public int Prop3 { readonly get; set; }

// Not allowed
public readonly int Prop4 { get; set; }
public int Prop5 { get; readonly set; }
```

#### 2.6.8. required 属性

类型除了可以声明 `required` 必填字段，也可以声明必填属性，该属性必须具有具有 `set` 或 `init` 访问器。显式接口实现的属性不能标记为 `required`。派生类型无法删除重写 `required` 的属性的状态，但可以在重写属性中添加 `required` 修饰。

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

`readonly required` 属性必须具有 `set` 或 `init` 访问器。

```csharp
public readonly required int Prop1 { get; init; }
public readonly required int Prop2 { get { .. } set { .. } }
```

#### 2.6.9. 记录中的主构造函数

拥有主构造函数的记录，编译器会将其参数自动实现公共属性。对于 `record` 实现的公共属性是只读的（`{ get; init; }`），对于 `record struct` 则是读写的（`{ get; set; }`）。

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

>---

### 2.7. 索引器

索引器允许类或结构的实例就像数组一样进行索引。索引器类似于属性，但是需要使用参数进行索引访问。索引器可以不必使用整数值进行索引，可以使用任何类型的值作为索引。

索引器可以重载，因此可以由多个形参作为检索条件。

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

索引器可以是 `virtual` 或 `abstract`，无法声明为 `static`。重写索引器时，可以仅重写基索引器已声明访问器的其中一个，重写索引器的只读访问器时可以返回比它重写的基索引器更派生的返回类型。

```csharp
class Base
{
    public virtual object this[int index]
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }
}
class Derived : Base
{
    // 重写只读访问器，返回更派生类型
    public override string this[int index]
    {
        get => throw new NotImplementedException();
    }
}
```

通过声明索引器，编译器会自动在对象上生成一个名为 `Item` 的属性，无法从实例成员访问表达式直接访问 `Item` 属性。如果在包含索引器的对象中添加自己的 `Item` 属性，则将收到 CS0102 编译器错误。要避免此错误，可以使用 `IndexerNameAttribute` 来重命名索引器。

```csharp
struct SampleCollection<T>( T[] arr)
{
    // [System.Runtime.CompilerServices.IndexerName("__Item")]
    public ref T this[int i] =>  ref arr[i];
    T Item { get; set; }
}
```

#### 2.7.1. ref 返回

尽管访问索引器元素的语法与访问数组元素的语法相同，但索引器元素不被归类为变量。因此无法对索引器元素作为 `ref`、`out`、`int` 参数传递，除非索引值是按引用返回元素（`return ref`）。`return ref` 返回的索引器不能设置 `set` 访问器。

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

#### 2.7.2. readonly 索引器

- 和 `readonly` 属性类似，可以应用于整个索引器或单个访问器，指示该访问器不会修改 `this` 的状态。

```csharp
public readonly int this[int index]
{
    get { /*..*/ }
    set { /*..*/ }
}
```

#### 2.7.3. 为索引器提供索引和范围运算 

可以为类型声明以 `System.Index` 为检索参数的索引器，该索引器将支持索引运算 `^`。

可以为类型声明以 `System.Range` 为检索参数的索引器，该索引器将支持范围运算 `..`。

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

若定义类型中声明了一个 `int Length {get;}` 实例属性和 `this[int]` 的索引器，且没有 `this[System.Index]` 和 `this[System.Range]` 的索引器时，该类型将隐式支持索引运算。

继续定义一个实例方法 `public TResult[] Slice(int start, int length)`，该类型将隐式支持范围运算。

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

#### 2.7.4. 接口中的索引器

可以在接口上声明索引器，接口访问器不使用修饰符，通常没有正文，仅以指示该索引器为读写、只读还是只写。可以为接口中定义的索引器提供实现，但这种情况非常少。

```csharp
interface ISampleArray<T>
{
    T this[int index] { get; }
    int Length { get; }
}
```

>---

### 2.8. 事件

事件是使对象或类型能够通知或广播的成员。客户端可以通过事件处理程序为事件附加可执行代码。

事件是对象用于（向系统中的所有相关组件）广播已发生事情的一种方式。任何其他组件都可以订阅事件，并在事件引发时得到通知。发送或引发事件的类型称为 “发布者”，接收或处理事件的类型称为 “订阅者”。发行者确定如何引发事件，而订阅者确定对事件发生后做出何种响应。

使用关键字 `event` 声明事件。实际上，事件是建立在对委托的语言支持之上的。一个事件可以有多个订阅者，订阅者也可以处理来自多个发行者的多个事件。

事件的调用只能在其包容对象中引发。包含定义类型之外的代码无法引发事件，也不能执行任何其他操作，只能进行订阅或取消订阅。

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

#### 2.8.1. 事件访问器

最简单的方式是将该事件声明为公共字段形式，编译器生成的代码会为其创建包装器，以便事件对象只能通过安全的方式进行访问。

在编译类似字段的事件时，编译器会自动创建存储来保存委托，并为该事件创建访问器，以便向委托字段添加或删除事件处理程序。添加（`+=`）和删除（`-=`）操作是线程安全的。

可以显式为事件添加访问器 `add` 和 `remove` 并关联一个内部的事件对象，可以在访问器中添加额外的验证代码或用户定义代码。具有访问器的事件只能出现在 `+=`、`-=` 的左侧。

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

#### 2.8.2. 事件的订阅与取消

类似与委托，通过使用 `+=` 运算符订阅事件，使用 `-=` 运算符取消订阅。事件的初始化和事件的引发只能在其包含类型中实现定义。

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

取消定于 `-=` 的右侧是匿名方法或 Lambda 表达式时，它会成为不同的委托实例，并静默地不执行任何操作。请务必为表示事件处理程序的表达式声明局部变量，以确保取消订阅删除该处理程序。

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

#### 2.8.3. readonly 事件

`readonly` 可应用于提供实现的事件，但不能应用于类似于字段的事件。`readonly` 不能应用于事件的访问器。

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

#### 2.8.4. 标准 .NET 事件模式

.NET 事件通常遵循几种已知模式。标准化这些模式意味着开发人员可利用这些标准事件模式，将其应用于任何 .NET 事件程序。在 .NET 类库中，事件是基于 `System.EventHandler` 委托和 `System.EventArgs` 基类：

```csharp
public delegate void EventHandler<TEventArgs>(object? sender, TEventArgs e);
public class EventArgs
{
    public static readonly EventArgs Empty;
    public EventArgs() { }
}
```

.NET 事件委托的标准签名：参数列表中包含 `sender` 发件人和 `args` 事件参数，按照惯例发件人使用 `object`。事件参数一般是派生自 `System.EventArgs`（也可以是用户定义），若事件类型不需要任何参数，仍需要提供这两个参数，事件参数使用 `EventArgs.Empty` 来表示事件不包含任何附加信息。

```csharp
void EventRaised(object? sender, EventArgs args);
```

设计一个符合标准 .NET 事件模式的组件：该组件中定义一个功能，它将搜索并列出目录或子目录中任何遵循模式的文件，并且在找到的每个与模式匹配的文件时引发一个事件。

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

为组件设计取消程序，即在满足设定条件的情况下，事件处理系统能够停止进一步的处理。可以使用 `EventArgs` 对象来包含订阅者可以用来通信取消的字段。可以在事件参数中添加一个 `bool` 字段以指示取消状态：
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

在 `FileSearcher` 中添加一个新事件，在每次新目录开始搜索开始时引发该事件，订阅者能够跟踪进度并根据进度更新用户。开始设计之前定义一个新的事件参数，用于报告新的目录和进度。

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

#### 2.8.5. 异步事件订阅

可以将事件订阅异步方法的事件处理程序，事件订阅者代码调用异步方法时，只能创建安全的 `async void` 方法。

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

#### 2.8.6. 委托与事件的区别

编译器为委托和事件都提供了一个后期绑定方案：它们通过调用仅在运行时绑定的方法组来进行组件之间的通信，所以委托和事件都支持单播和多播方式。它们都支持添加和删除处理程序的类似语法，拥有相同的方法调用语法 `T()` 或 `T?.Invoke`。

在确定要使用的语言功能时，在需要实现方法回调时，应使用基于委托的设计；若代码在不调用任何订阅服务器的情况下可完成其所有工作，应使用基于事件的设计。一般而言，当需要返回值时选择委托，委托的返回值可能会以某种方式影响算法；用于事件的委托一般是无返回类型。

只有包含事件的类型才能调用事件，以外的类型只能订阅或取消事件监听器。委托通常作为参数传递。

事件监听器通常具有较长的生存期，事件源可能会在程序的整个生存期内引发事件；而许多基于委托的设计，用作方法的参数进行传递，在返回该方法后不再使用此委托。

>---

### 2.9. 运算符

运算符是定义可应用于类型实例的表达式运算符含义的成员。使用 `operator` 声明用户运算符定义，运算符必须是 `public static`。

用户定义的类型可重载预定义的 C# 运算符，一元运算符有一个输入参数，二元运算符有两个输入参数。

重载运算符时，参数列表至少有一个参数是类型 `T` 或 `T?`，`T` 是包含运算符声明的类型。

#### 2.9.1. 可重载的运算符

- 算数运算符：一元 `++`、`--`、`+`、`-` 和二元 `*`、`/`、`%`、`+`、`-` 算术运算符。
- 逻辑运算符：一元 `!` 和二元 `&`、`|`、`^`。
- 比较运算符：二元 `<` 和 `>`、`<=` 和 `>=`、`==` 和 `!=`，成对的运算符需要同时重载。
- 位运算：一元 `~` 和二元 `&`、`|`、`^`。
- 移位运算符：二元 `<<`、`>>`、`>>>`，C#11 之前右操作数必须为 `int`，C#11 开始重载移位运算符的右侧操作数的类型可以是任意类型。
- 一元 `true` 和 `false` 运算符，只能返回 `bool` 类型。若用户类型同时定义了 `&` 或 `|` 运算符重载，则可以使用相应的条件逻辑运算符 `&&` 或 `||`。

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

#### 2.9.2. checked 用户定义算数运算符

重载算数运算符时，可以使用 `checked` 关键字定义该运算符的已检查版本。定义已检查的运算符时，还必须同时定义不带 `checked` 修饰符的运算符版本。

`checked` 运算符在已检查的上下文中调用；不带 `checked` 修饰符的运算符在未检查的上下文中调用。

```csharp
readonly record struct Point(int X, int Y)
{
    public static Point operator checked +(Point l, Point r) => checked(new Point(l.X + l.X, r.Y + r.Y));
    public static Point operator +(Point l, Point r) => new(l.X + r.X, l.Y + r.Y);
}
```

#### 2.9.3. 用户定义类型转换

可以在目标类型中使用 `operator` 和 `implicit` 或 `explicit` 关键字分别用于定义隐式转换或显式转换。无法定义从接口类型到另一个接口类型的用户定义转换。

如果两个类型之间存在预定义的转换，则忽略这些类型之间的任何用户定义的转换。

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

>---

### 2.10. 实例构造函数

实例构造函数是实现初始化类型实例所需操作的成员。每当创建类、结构或记录的实例时，将会调用其构造函数。构造函数之间可以形成重载，但无法被继承。

在创建一个新对象时，有多个操作在初始化新实例时进行：
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
class Derived : Base
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

除非类是静态，否则 C# 编译器将为没有声明实例构造函数的类提供一个公共的无参数构造函数，以便该类可以实例化。可以将构造函数设置为受保护或私有构造函数，可以阻止类在外部被实例化。

```csharp
Sample s = new();  // ERROR: CS0122

class Sample{ private Sample(){} }
```

在 C#10 之前，结构类型不能包含显式无参数构造函数，因为编译器会自动提供一个。实例化结构时，结构中的所有内存初始化为默认值，值类型设置为 0，可为 null 类型和引用类型设置为 null。

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

#### 2.10.1. 构造函数初始化器

所有实例构造函数（除了 `object` 的构造函数以外）都隐式地在函数体执行之前包含对另一个实例构造函数的调用，隐式调用的构造函数由构造函数初始化器决定：
- `base(arguments?)` 形式的实例构造函数初始化器，在调用前将调用直接基类的实例构造函数，具体选择哪一个基类的实例构造函数由 `arguments` 和重载决策来选择。
- `this(arguments?)` 形式的构造构造函数初始化器，在调用前将先调用同一类的另一个实例构造函数。
- 没有初始化器的实例构造函数，将隐式提供 `base()` 形式的初始化器。最终所有的实例构造函数都会关联到 `object` 的构造函数。

在执行构造函数之前总会调用其直接基类的某个构造函数，可以使用 `base` 显式指定要调用的直接基类构造函数。在派生类中，如果不使用 `base` 关键字来显式调用基类构造函数，则将隐式调用无参数构造函数（若基类没有构造函数，编译器会自动为其提供公共无参构造）。

如果在某个类中声明至少一个实例构造函数，则 C# 不提供无参数构造函数。结构不同于类，它一定会包含一个公共无参的实例构造函数（无论是用户显式声明还是编译器提供）。

若基类中存在非无参构造函数且没有提供无参构造函数时，派生类必须使用 `base` 显式调用基类构造函数。

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

实例构造函数可以使用 `this` 调用同一对象中的另一个构造函数。

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

#### 2.10.2. 主构造函数

从 C#12 开始，可以在类、结构和记录中声明主构造函数，将任何参数放在类型名称后面的括号中。主构造函数的位置参数位于声明类型的整个主体中，可以用于初始化属性或字段，或用作方法或局部函数中的变量，编译器将这些参数自动实现为私有字段（对于记录类型，编译器将合成一个与主构造函数参数同名的公共属性，若显式声明同名属性，位置参数则实现为私有字段），可以定义同名属性并使用位置参数作为初始化设定项。

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

主构造函数指示这些参数对于类型的任何实例是必需的，任何显式编写的构造函数都必须使用 `this(...)` 初始化表达式语法来调用主构造函数。对于 `class` 和 `record class`，主构造函数存在时，编译器不会为其生成隐式的无参构造函数；对于 `struct` 和 `record struct`，在未显式指定时始终生成隐式的无参构造函数，主构造函数参数和所有的字段都初始化为 0 位模式。

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

#### 2.10.3. 复制构造函数

C# 记录类型为对象隐式提供复制构造函数。对于非密封记录，复制构造函数是 `protected` 的，密封记录和结构记录始终是 `private` 的。创建记录时使用 `with` 运算符时，自动调用它的复制构造函数。可以显式创建复制构造函数定义复制规则。

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

`with` 表达式无法用于非记录类型，非结构类型。对于类类型，可以自行编写复制构造函数。

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

>---

### 2.11. 静态构造函数

静态构造函数是实现初始化封闭类所需操作的成员。

静态构造函数通常用于初始化任何静态数据，或执行仅需执行一次的特定操作。将在创建第一个实例或引用任何静态成员之前自动调用静态构造函数，静态构造函数最多调用一次。

静态构造函数可以在 `class`、`struct`、`record` 类型中定义，最多只能有一个静态构造函数，为该声明类型独有，不能被继承或重载。

在创建类型实例之前，有多个操作在静态初始化时执行（若是首次使用类型时是引用任何静态成员而非创建实例时，则不会调用基类的静态构造函数）：
  1. 静态字段设置为 0 位模式。通常由运行时来完成。
  2. 静态字段初始化设定项运行。派生程序最高类型的静态字段初始值设定项运行。
  3. 基类型静态字段初始值设定项运行，以直接基开始到 `System.Object` 的静态字段初始值设定项。
  4. 基静态构造函数运行。以 `System.Object.Object` 开始到直接基的任何静态构造函数。
  5. 静态构造函数运行。该类型的静态构造函数运行。

如果一个类型中包含 `Main` 入口方法，则该类的静态构造函数在调用 `Main` 方法之前执行。

静态构造函数不能直接调用，并且仅应由公共语言运行时 CLR 调用。如果静态构造函数引发异常，运行时也不会再次调用该函数，并且类型在应用程序域的生存期内将保持未初始化，该异常将包装在异常中 `TypeInitializationException` ，并且无法再创建该类型的实例，也无法调用该类型的任何静态成员。

声明为 `static readonly` 的字段仅能在声明时初始化或在静态构造函数中初始化。若不提供静态函数，则应该为 `static readonly` 字段提供初始化设定项。

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

#### 2.11.1. 结构中的静态构造函数

结构体的静态函数在引用结构的静态成员、调用结构类型的显式声明的构造函数、访问结构变量时，才会被调用。创建未提供初始值的结构类型的数组时，不会调用结构的静态构造函数和任何实例构造函数。

```csharp
SampleStruct[] SstructArr = new SampleStruct[10];
SampleClass[] SclassArr = new SampleClass[10];
Console.WriteLine("---------------");
SampleClass sc = new();
Console.WriteLine("---------------");
Console.WriteLine(SstructArr[0].Value);
Console.WriteLine("---------------");
SampleStruct ss = new SampleStruct();
Console.WriteLine(ss.Value);

/**
---------------
Static SampleClass
---------------
Static SampleStruct
0
---------------
SampleStruct Ctor
1
 */
struct SampleStruct
{
    static SampleStruct() => Console.WriteLine("Static SampleStruct");
    public SampleStruct() => Console.WriteLine("SampleStruct Ctor");
    public int Value { get; set; } = 1;
}
class SampleClass
{
    static SampleClass() => Console.WriteLine("Static SampleClass");
}
```

#### 2.11.2. 接口中的静态构造函数

接口中可以定义静态成员和静态构造函数（非静态抽象成员和虚拟成员），为接口类型所有。实现接口的类型无法访问这些成员。静态抽象成员和虚拟成员定义在泛型接口中。

接口静态构造函数在首次使用接口的任何静态成员时被调用，仅调用一次，且不会调用基接口的静态构造。使用派生类型时也无法调用接口的静态构造函数。

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

>--- 

### 2.12. 终结器

终结器是实现终结类实例所需操作的成员。终结器不能被继承。

终结器是自动调用的，不能显式调用。当任何代码不再可能使用实例时，该实例才有资格进行终结。实例的终结器可以在实例符合终结条件后的任何时间执行。当实例结束时，将按从派生最多到最少的顺序调用该实例继承链中的终结器。终结器可以在任何线程上执行。

```csharp
class Sample
{
    ~Sample(){ ... }  // 终结器声明
}
```

终结器是通过覆盖 `System.Object` 上的 `Finalize` 虚方法来实现的。C# 程序不允许重写这个方法或直接调用它。

```csharp
class Sample
{
    override protected void Finalize() { }  // Error
    public void F()
    {
        this.Finalize();                   // Error
    }
}
```

终结器用于在垃圾回收器收集类实例时执行任何必要的最终清理操作。在大多数情况下，通过使用 `System.Runtime.InteropServices.SafeHandle` 或派生类包装任何非托管句柄，可以免去编写终结器的过程。

无法在结构中定义终结器，它们仅用于在堆中分配的托管类型的类，一个类只能有一个终结器。终结器无法被继承和重载，也不能被手动调用，由垃圾回收器调用。

程序员无法控制何时调用终结器，因为这由垃圾回收器决定。垃圾回收器检查应用程序不再使用的对象：如果它认为某个对象符合终止条件，则调用终结器（如果有），并回收用来存储此对象的内存。可以通过调用 `GC.Collect` 强制进行垃圾回收，但多数情况下应避免此调用，因为它可能会造成性能问题。

```csharp
public class Destroyer
{
    public override string ToString() => GetType().Name;
    ~Destroyer() => Console.WriteLine($"The {ToString()} finalizer is executing.");
}
```

拥有终结器的类型，编译器会为其构造一个 `Finalize()` 方法，并隐式调用 `object` 基类上的 `Finalize()`，此时无法创建或重写 `Finalize()` 方法。

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
## 3. 分部声明

### 3.1. 分部类型

类型声明可以跨越多个编译单元进行分部声明。使用 `partial` 修饰符声明 `class`、`struct`、`interface` 为分部类型。分部类型声明的每一部分都应包含 `partial` 修饰符，并应与其他部分声明在相同的命名空间或包含类型中。

编译器会将分部类型的所有部分一起编译，并将各部分的类型的修饰、特性、声明成员合并。分部类型不允许扩展已编译的类型。

```csharp
// File: Customer1.cs
public partial class Customer
{
    private List<int> orders;
    public Customer()
    {
       // ...
    }
}

// File: Customer2.cs
public partial class Customer
{
    public void SubmitOrder(int orderSubmitted) => orders.Add(orderSubmitted);
    public bool HasOutstandingOrders() => orders.Count > 0;
}
```

可以在分部类或结构上声明主构造函数，且只有一个分部声明上可以拥有参数列表。

```csharp
partial class Sample(int a, int b) { }
partial class Sample(int a) {} // err
```

> 分部类的常见使用场景

常在以下几种情况下需要拆分类定义：
  - 处理大型项目时，使一个类分布于多个独立文件中可以让多位程序员同时对该类进行处理。
  - 当使用自动生成的源文件时，用户可以添加代码而不需要重新创建源文件。Visual Studio 在创建 Windows 窗体、Web 服务包装器代码等时会使用这种方法。用户可以创建使用这些类的代码，这样就不需要修改由 Visual Studio 生成的文件。
  - 使用源生成器在类中生成附加功能时。

可以为声明的分布类型的各个部分指定不同的基接口，声明不同的成员，应用不同的特性。在编译时，各个部分都合并起来形成最终的类型。在某一分部定义中声明的任何类、结构、接口和成员可供所有其他部分使用。最终类型是所有部分在编译时的组合。

要成为同一类型的各个部分的所有分部类型定义都必须在同一程序集和同一模块（`.exe` 或 `.dll` 文件）中进行定义。分部定义不能跨越多个模块。

泛型类型可以是分部的，每个分部声明都必须以相同的顺序使用相同的参数名。

>---

### 3.2. 分部方法

分部类或结构可以包含分部方法。类的一个部分包含方法的签名，可以在同一部分或另一部分中定义实现。根据方法的签名，可能需要实现。对于无访问修饰符、无返回类型、不使用 `out` 参数的分部方法（可以包含 `static` 和 `unsafe` 修饰），不需要提供实现即可定义。

如果未提供该实现，则会在编译时删除分部方法以及对该方法的所有调用。声明分部方法的实现前需要先声明其定义。

```csharp
public partial class PartClass
{
   partial void PartFunc1();
   partial void PartFunc2();
}
```

任何分部方法使用访问修饰符、有返回值、使用 `out` 参数或是 `sealed`、`abstract`、`virtual`、`new` 修饰的方法，都必须提供实现。分部方法允许类型的某个部分的实现者定义方法，类型另一部分的实现者提供这些方法的具体实现。

生成样板代码的模板和源生成器，使用分部方法可能会很有用：
  - 模板代码：模板保留方法名称和签名，以便生成的代码可以调用方法。调用但不实现该方法不会导致编译时错误或运行时错误。
  - 源生成器：源生成器提供方法的实现。开发人员可以添加方法声明（通常由源生成器读取属性）和编写调用这些方法的代码。源生成器在编译过程中运行并提供实现。

分部类型的两个部分中的分部方法签名必须匹配，参数名称不做要求。实现分部方法声明可以与相应的定义分部方法声明出现在同一部分中。

分部方法可以是泛型的。约束将放在定义分部方法声明上，但也可以选择重复放在实现声明上，不能声明不同的约束组合。参数和类型参数名称在实现声明和定义声明中不必相同。

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

可以为已定义并实现的分部方法生成委托，但不能为已经定义但未实现的分部方法生成委托。

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

接口中的无访问修饰的 `void` 分部方法定义声明，被视为具有隐式的 `private` 的私有接口方法。若提供修饰符则需要提供该分部方法的实现声明。接口分部方法的基本规则与类型中的分部方法规则相同。没有实现声明的分部方法，编译时将在调用点删除分部方法调用。

---
## 4. 迭代器

使用迭代器块实现的函数成员称为迭代器。迭代器块可以用作函数体，只要该函数成员的返回类型是 `IEnumerator` 或 `IEnumerable` 类型之一。方法块、运算符函数体块、访问器块都可以作为迭代器实现。事件、构造函数和终结器不能作为迭代器实现。

使用迭代器块实现的函数成员，不能使用任何 `in`、`out`、`ref` 修饰的参数或 `ref struct` 类型的参数。

>---

### 4.1. 枚举器接口和可枚举接口

枚举器接口是 `System.Collections.IEnumerator` 和 `System.Collections.Generic.IEnumerator<T>` 以及它们的派生接口。

可枚举接口是 `System.Collections.IEnumerable` 和 `System.Collections.Generic.IEnumerable<T>` 以及它们的派生接口。

>---

### 4.2. yield 类型

枚举器用于一个生成类型相同的变量序列，这种类型称为枚举器的 `yield` 类型：
- 返回 `IEnumerator` 或 `IEnumerable` 的枚举器的 `yield` 类型是 `object`。
- 返回 `IEnumerator<T>` 或 `IEnumerable<T>` 的枚举器的 `yield` 类型是 `T`。

>---

### 4.3. 迭代器对象

当使用迭代器块实现返回枚举器接口类型的函数成员时，调用函数成员不会立即执行迭代器块中的代码，它将创建并返回一个枚举器对象。枚举器对象封装了迭代器块中指定的代码，并且在调用枚举器对象的 `MoveNext` 方法时执行迭代器块中的代码。

枚举器对象具有的特征：
- 它实现了 `IEnumerator` 或 `IEnumerator<T>` 接口。
- 实现 `IEnumerator<T>` 接口的枚举器同时也需要实现 `IDisposable` 接口。
- 使用传递给函数成员的实参或参数的副本初始化它。
- 它有四种可能的状态：*before*、*running*、*suspended*、*after*。初始化时处于 *before* 状态。

枚举器对象通常是编译器生成的枚举器类的实例，该类封装迭代器块中的代码并实现枚举器接口，但也可以采用其他实现方法。如果枚举器类是由编译器生成的，该类将直接或间接嵌套在包含函数成员的类中，它将具有私有可访问性，并且它将保留一个供编译器使用的名称。

<!-- 枚举器对象不支持 `IEnumerator.Reset` 方法。调用此方法将导致 `System.NotSupportedException`。 -->

#### 4.3.1. MoveNext 方法

枚举器对象的 `MoveNext` 方法封装了迭代器块的代码，调用 `MoveNext` 方法执行迭代器块中的代码，并适当地设置枚举器对象的 `Current` 属性。`MoveNext` 执行的行为取决于调用 `MoveNext` 时枚举器对象的状态：
- 如果枚举对象的状态是 *before*，调用 `MoveNext`：
  - 将状态更改为 *running* 状态。
  - 将迭代器块的形参（包括 `this`）分配给初始化枚举器对象时保存的实参值和实例值。
  - 从头开始执行迭代器块，直到执行被中断。
- 如果枚举器对象的状态是 *running*，则调用 `MoveNext` 的结果是未指定的。
- 如果枚举器对象的状态是 *suspended*，调用 `MoveNext`：
  - 将状态更改为 *running* 状态。
  - 将所有局部变量和参数（包括 `this`）的值恢复为上次暂停迭代器块执行时保存的值。自上次调用 `MoveNext` 之后，这些变量引用的任何对象的内容都可能发生了变化。
  - 在导致暂停执行的 `yield return` 语句之后立即恢复迭代器块的执行，并继续执行直到执行被中断。
- 如果枚举器对象的状态为 *after*，则调用 `MoveNext` 返回 `false`。

当 `MoveNext` 执行迭代器块时，执行可以通过四种方式中断：通过 `yield return` 语句、`yield break` 语句、遇到迭代器块的结束以及抛出并传播出迭代器块的异常：
- 当遇到 `yield return` 语句时：
  - 语句中给出的表达式被求值，隐式转换为 `yield` 类型，并赋值给枚举器对象的    `Current` 属性。
  - 迭代器块的执行被暂停。保存所有局部变量和参数（包括 `this`）的值，以及 `yield return` 语句的位置。如果 `yield return` 语句在一个或多个 `try` 块中，则此时不会执行相关的 `finally` 块。
  - 将枚举器对象的状态改为 *suspended*。
  - `MoveNext` 方法向其调用者返回 `true`，表明迭代成功推进到下一个值。
- 当遇到 `yield break` 语句时：
  - 如果 `yield break` 语句在一个或多个 `try` 块中，则执行相关的 `finally` 块。
  - 将枚举器对象的状态更改为 *after*。
  - `MoveNext` 方法向调用者返回 `false`，表示迭代完成。
- 当遇到迭代器块的末尾时：
  - 将迭代器对象的状态更改为 *after*。
  - `MoveNext` 方法向调用者返回 `false`，表示迭代完成。
- 当抛出异常并将其传播到迭代器块外时：
  - 迭代器块中适当的 `finally` 块将被异常传播执行。
  - 将枚举器对象的状态更改为 *after*。
  - 异常传播继续到 `MoveNext` 方法的调用者。

#### 4.3.2. Current 属性

枚举器对象的 `Current` 属性受迭代器块中的 `yield return` 语句的影响。

当枚举器对象处于 *suspended* 状态时，`Current` 的值是之前调用 `MoveNext` 时设置的值。当枚举数对象处于 *before*、*running* 或 *after* 状态时，访问 `Current` 的结果未指定。

对于 `yield` 类型不是 `object` 的迭代器，访问实现 `IEnumerable` 枚举器对象的 `Current` 的结果，对应于访问实现 `IEnumerator<T>` 的枚举器对象的 `Current` 并将结果强制转换为 `object`。

#### 4.3.3. Dispose 方法

`Dispose` 方法用于通过将枚举器对象的状态设置为 *after* 来清理迭代：
- 如果枚举器对象的状态为 *before*，则调用 `Dispose` 将状态更改为 *after*。
- 如果枚举器对象的状态为 *running*，则调用 `Dispose` 的结果未指定。
- 如果枚举器对象的状态为 *suspended*，则调用 `Dispose` ：
  - 将状态更改为 `running`。
  - 执行任何 `finally` 块，就好像最后执行的 `yield return` 语句是一个 `yield break` 语句一样。如果发生异常则会将其传播到迭代器块外，枚举器对象的状态设置为 *after*，并将异常传播给 `Dispose` 方法的调用者。
  - 更改枚举器的状态为 *after*。
- 如果枚举器对象的状态为 *after*，则调用 *Dispose* 不会有任何影响。

>---

### 4.4. 可枚举对象

当使用迭代器块实现返回 `IEnumerable` 或 `IEnumerable<T>` 可枚举接口类型的函数成员时，调用该函数成员不会立即执行迭代器块中的代码。相反，将创建并返回一个可枚举对象。

可枚举对象的 `GetEnumerator` 方法返回一个枚举器对象，该枚举器对象封装了迭代器块中指定的代码，并且在调用枚举器对象的 `MoveNext` 方法时执行迭代器块中的代码。

可枚举对象具有以下特征:
- 它实现了 `IEnumerable` 或 `IEnumerable<T>`。
- 使用传递给函数成员的实参值（如果有的话）和实例值的副本初始化它。

可枚举对象通常是编译器生成的可枚举类的实例，该类封装迭代器块中的代码并实现可枚举接口，但也可以使用其他实现方法。如果一个可枚举类是由编译器生成的，那么该类将直接或间接地嵌套在包含函数成员的类中，它将具有私有可访问性，并且它将保留一个名称供编译器使用。

#### 4.4.1. GetEnumerator

可枚举对象提供了 `IEnumerable` 或 `IEnumerable<T>` 接口的 `GetEnumerator` 方法的实现。这两个 `GetEnumerator` 方法共享一个公共实现，该实现获取并返回一个可用的枚举器对象。

>---

### 4.5. 实现一个可枚举对象和对应的枚举器

```csharp
class ReadOnlyArray<T>(T[] values) : IEnumerable<T>
{
    T[] arr = values;
    public IEnumerator<T> GetEnumerator() => new SampleEnumerator(this);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    class SampleEnumerator(ReadOnlyArray<T> arr) : IEnumerator<T>
    {
        ReadOnlyArray<T> Binds = arr;
        int index = -1;
        public T Current => Binds.arr[index];
        object IEnumerator.Current => Current;
        public void Dispose() => Binds = null;
        public bool MoveNext() => ++index < Binds.arr.Length;
        public void Reset() => index = -1;
    }
}
```

---
## 5. 异步方法

带有 `async` 修饰符的方法或匿名函数称为异步方法。术语 *async* 用于描述具有 `async` 修饰符的任何类型的函数。

异步方法中无法使用任何 `ref`、`in`、`out` 修饰或 `ref struct` 类型的参数。

异步方法的返回类型可以是 `void`（异步事件处理程序）或者是 *Task Type*。有返回值的异步方法返回泛型类型的 *`TaskType`*，无返回值的返回非泛型的 *`TaskType`*。返回任务类型的方法称为 *Task-returning*。

标准库类型是 `System.Threading.Tasks.Task` 和构造类型 `System.Threading.Tasks.Task<T>`，以及通过 `System.Runtime.CompilerServices.AsyncMethodBuilderAttribute` 属性与任务构建器 *Task Builder Type* 类型相关联的类、结构或接口类型。

*Task Builder Type* 是对应于特定 *Task Type* 的类或结构类型。例如 *`TaskType`* 对应的构建器是 *`TaskBuilderType`*，*`TaskType<T>`* 对应的构建器是 *`TaskBuilderType<T>`*。

任务类型的确切定义，从语言的角度类来看，任务类型处于未完成 *incomplete*、成功 *succeeded* 或失败 *faulted* 的状态之一。*faulted* 的任务记录了相关的异常。*succeeded* 的 *`TaskType<T>`* 记录了一个类型 `T` 的结果。*Task Type* 是可等待的，它可以是 `await` 表达式的操作数。

```csharp
using System.Runtime.CompilerServices; 
[AsyncMethodBuilder(typeof(MyTaskMethodBuilder<>))]
class MyTask<T>
{
    public Awaiter<T> GetAwaiter() { ... }
}

class Awaiter<T> : INotifyCompletion
{
    public void OnCompleted(Action completion) { ... }
    public bool IsCompleted { get; }
    public T GetResult() { ... }
}
// 任务类型 MyTask<T> 与任务构建器类型 MyTaskMethodBuilder<T> 和 Awaiter<T> 类型相关联。
```

异步函数可以通过在函数体中使用 `await` 表达式来暂停求值。之后可以通过调用 *Resumption Delegate* 恢复委托在暂停的 `await` 表达式上恢复计算。恢复委托的类型是 `System.Action`，当调用它时，异步函数调用的计算将从 `await` 表达式停止的地方恢复。如果函数调用从未暂停，则异步函数调用的 *Current Caller* 当前调用者为原始调用者，否则为恢复委托的最近调用者。

>---

### 5.1. *Task Type* 构建器

任务构建器类型最多只能有一个类型参数，并且不能嵌套在泛型类型中。任务构建器类型应具有以下可访问成员（对于非泛型任务构建器类型，`SetResult` 没有参数）：

```csharp
using System.Runtime.CompilerServices;

class TaskBuilderType<T>
{
    public TaskType<T> Task { get; }
    
    public static TaskBuilderType<T> Create();

    public void Start<TStateMachine>(ref TStateMachine stateMachine) 
        where TStateMachine : IAsyncStateMachine;
    
    public void SetStateMachine(IAsyncStateMachine stateMachine);
    
    public void SetException(Exception exception);
    
    public void SetResult(T result);
    
    public void AwaitOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : INotifyCompletion
        where TStateMachine : IAsyncStateMachine;
    
    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : ICriticalNotifyCompletion
        where TStateMachine : IAsyncStateMachine;
}
```

编译器生成的代码使用 ``TaskBuilderType<T>`` 来实现暂停和恢复异步函数求值的语义：
- 调用 `TaskBuilderType.Create()` 方法来创建构建器 `TaskBuilderType<T>` 的实例 `builder`。
- 调用 `builder.Start(ref stateMachine)` 来将构建器与编译器生成的状态机实例 `stateMachine` 关联起来。构建器应该在 `Start()` 中或在 `Start()` 返回后调用 `stateMachine.MoveNext()` 来推进状态机。
- 在 `Start()` 方法返回后，`async` 方法将调用属性 `builder.Task` 获取一个 *`TaskType<T>`* 任务作为异步方法的返回值。
- 对 `stateMachine.MoveNext()` 的每次调用都将推进状态机：
  - 如果状态机成功完成，则调用`builder.SetResult()` 并返回方法返回值（如果有的话）；
  - 如果在状态机抛出异常，则调用 `builder.SetException(e)`。
- 如果状态机到达一个 `await expr` 表达式，则调用 `expr.GetAwaiter()` 方法以获取等待器 `awaiter`：
  - 如果 `awaiter` 实现了 `ICriticalNotifyCompletion` 接口，且 `IsCompleted` 属性为 `false`，则状态机调用 `builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine)` 方法；该 `AwaitUnsafeOnCompleted` 应该调用 `awaiter.UnsafeOnCompleted(action)` 方法，并在 `awaiter` 完成时调用 `stateMachine.MoveNext()`。
  - 否则，状态机将调用 `builder.AwaitOnCompleted(ref awaiter, ref stateMachine)`。该 `AwaitOnCompleted` 应该调用 `awaiter.OnCompleted(action)` 方法，并在 `awaiter` 完成时调用 `stateMachine.MoveNext()`。
- `SetStateMachine(IAsyncStateMachine)` 可以由编译器生成的 `IAsyncStateMachine` 实现调用，以识别与状态机实例关联的构建器实例，特别是在状态机被实现为值类型的情况下。
  - 如果构建器调用 `stateMachine.SetStateMachine(stateMachine)`，则 `stateMachine` 将在与 `stateMachine` 关联的构建器实例上调用 `builder.SetStateMachine (stateMachine)`。

>---

### 5.2. *Task-returning* 异步方法的求值

调用 *Task-returning* 的异步函数会生成返回 *Task Type* 任务类型的实例 `returnTask`。任务最初处于未完成状态。

然后对异步函数体进行求值，直到它被挂起（通过到达 `await` 表达式）或终止，此时将控制权连同 `returnTask` 一起返回给调用者。

当异步函数体终止时，`returnTask` 将从不完整状态移出：
- 如果函数体因到达 `return` 语句或函数体结束而终止，则在 `returnTask` 中记录结果值（如果有返回值的话），并将其置于 *succeeded* 状态。
- 如果函数体由于未捕获的异常而终止，则将异常记录在 `returnTask` 中，并将其置于 *faulted* 状态。

>---

### 5.3. *Void-returning* 异步方法的求值

如果异步方法的返回类型为 `void`，则求值与 *Task-returning* 的不同之处在于：因为不返回任何任务，所以该函数将完成和异常传递给当前线程的同步上下文（*Synchronization context*）。同步上下文的确切定义依赖于实现，但它表示当前线程运行的 “位置”。当 *Void-returning* 的异步函数的求值开始、成功完成或抛出未捕获的异常时，将通知同步上下文。

这允许该线程上下文持续跟踪在其下运行了多少 *Void-returning* 的异步函数，并决定如何传播从中产生的异常。

---
## 6. 异常

C# 中的异常系统提供了一种结构化的、统一的、类型安全的方式来处理系统级和应用级的错误条件。

异常可以通过两种方式抛出：
- `throw` 语句无条件地抛出异常。控件永远不会到达紧随抛出之后的语句。
- C# 语句和表达式在处理过程中出现的某些异常情况，会导致在某些情况下无法正常完成操作。（例如，整数除法中的 0 除）
  
公共语言运行时 CLR、.NET、第三方库和应用程序代码都可产生异常。.NET 中的托管异常在 Win32 结构化异常处理机制的基础之上实现。

```csharp
public class ExceptionTest
{
    static double SafeDivision(double x, double y)
    {
        if (y == 0)
            throw new DivideByZeroException();
        return x / y;
    }
    public static void Main()
    {
        double a = 98, b = 0;
        double result;
        try
        {
            result = SafeDivision(a, b);
            Console.WriteLine("{0} divided by {1} = {2}", a, b, result);
        }
        catch (DivideByZeroException)
        {
            Console.WriteLine("Attempted divide by zero.");  
        }
    }
}
```

>---

### 6.1. System.Exception

异常是最终全都派生自 `System.Exception` 的类型：
  - 调用堆栈状态：每个 `Exception` 对象都包含一个 `StackTrace` 字符串属性，包含当前调用堆栈上的方法的名称和引发异常的位置。
  - 错误文本说明：每个 `Exception` 对象都包含一个名为 `Message` 的属性，此字符串用来设置为解释发生异常的原因。
  - `InnerException`：它是 `Exception` 对象的只读属性。如果它非 `null`，则指向导致当前异常的异常（即，当前异常是处理 `InnerException` 的 `catch` 块中引发的）。否则，它的值为 `null`，表示其异常不是由其他异常引起的。以这种方式链接在一起的异常对象的数目是任意的。

可以在 `catch` 语句中捕捉查看这些异常。

```csharp
try
{
    ExceptionTest();
}
catch (NotImplementedException ex)
{
    Console.WriteLine($"EROOR : {ex.Message}\n" + ex.StackTrace);
}

void ExceptionTest() => throw new NotImplementedException();
```

>---

### 6.2. 异常处理

异常由 `try` 语句处理。

C# 语言的异常处理功能有助于处理在程序运行期间发生的任何意外或异常情况。异常处理功能使用 `try` 语句块来尝试执行可能失败的操作，并在捕获异常时在 `catch` 中处理故障，以及在操作结束后在 `finally` 中清除资源。异常是使用 `throw` 关键字创建而成的。

一般情况下，异常并不是由代码直接调用的方法抛出，而是由调用堆栈中再往下的另一个方法抛出。发生异常时，CLR 会展开堆栈，系统根据异常的运行时类型，搜索最近的可以处理该异常的 `catch` 子句。

首先，在当前方法中搜索包含 `try` 语句的词法，并按顺序考虑 `try` 语句的相关 `catch` 子句。如果失败，则搜索调用当前方法的方法，寻找包含当前方法调用点的词法封闭 `try` 语句。此搜索将继续进行，直到找到可以处理当前异常的 `catch` 子句。如果在调用堆栈中找不到相应的 `catch` 代码块，将会终止进程并向用户显示消息。没有命名异常类的 `catch` 子句可以处理任何异常。

一旦找到匹配的 `catch` 子句，系统就准备将控制转移到 `catch` 子句的第一个语句。在开始执行 `catch` 子句之前，系统首先按顺序执行与 `try` 语句相关的 `finally` 子句，这些语句嵌套程度比捕获异常的语句嵌套程度高：
- 在 `try` 中执行可能抛出异常的语句。程序使用 `throw` 关键字，用以显式生成异常。
- 在 `try` 中出现异常后，控制流会跳转到调用堆栈中任意位置上的首个相关异常处理程序（`catch`）。可以使用 `catch` 代码块末尾的 `throw` 关键字重新抛出异常。
- 若未找到给定异常对应的异常处理程序，那么程序会停止执行，并显示错误消息。
- 无论是否引发异常，`finally` 代码块中的代码仍会执行。使用 `finally` 代码块可释放资源。例如，关闭在 `try` 代码块中打开的任何流或文件。

```csharp
try
{
    // ..... do 
    string str = null;
    Console.WriteLine(str.ToLower());
}
catch (NullReferenceException ex1) 
{
    Console.WriteLine($"Arguement err : {ex1.Message}\n{ex1.StackTrace}");
}
catch
{
    throw;  // 重新抛出时，将异常传递给被调用方而不调用 finally
}
finally
{
    Console.WriteLine("Do Finally work...");
}
/*
    Arguement err : Object reference not set to an instance of an object.
        at Program.<Main>$(String[] args) in Program.cs:line 7
    Do Finally work...
*/
```

如果没有找到匹配的 `catch` 子句，则会发生以下三种情况之一：

- 如果对匹配 `catch` 子句的搜索到达静态构造函数或静态字段初始化式，则使用 `System.TypeInitializationException` 并在触发静态构造函数调用时抛出。`System.TypeInitializationException` 的 `InnerException` 包含最初抛出的异常。
- 如果异常存在于终结器内，将中止终结器，并调用基类终结器（如果有）。
- 如果搜索匹配的 `catch` 子句到达最初启动线程的代码，则终止线程的执行。这种终止的影响是由实现定义的。

>---

### 6.3. 重新引发异常

在 `catch` 块捕获的异常，可以通过一条单独的 `throw;` 语句重新抛出保留原始栈信息的异常。若使用 `throw` 抛出的是 `catch` 块中的具体异常，则会更新所有栈信息来匹配新的抛出位置。这会导致指示异常最初发生位置的栈信息丢失。

```csharp
class Sample
{
    static void Fun() => throw new NotImplementedException();
    static void Main(string[] args)
    {
        try
        {
            Fun();
        }
        catch (Exception e)
        {
            throw;
            //throw e;
        }
    } 
}
```

如何抛出现有异常而不替换栈信息？可以使用 `System.Runtime.ExceptionServices.ExceptionDispatchInfo` 类专门处理这种情况。

```csharp
using System.Runtime.ExceptionServices;
class ExceptionHandler
{
    public static void Capture(Exception ex, bool isThrow = false)
    {
        ExceptionDispatchInfo.Throw(ex);
        // or 
        var exInfo = ExceptionDispatchInfo.Capture(ex);
        if(isThrow)
            exInfo.Throw();
    }
}
class Sample
{
    static void Fun() => throw new NotImplementedException();
    static void Main(string[] args)
    {
        try
        {
            Fun();
        }
        catch (Exception e)
        {
            ExceptionHandler.Capture(e, true);
        }
    }
}
```

>---

### 6.4. 定义异常的类别

- 程序可以引发 `System` 命名空间中的预定义异常类（前面提到的情况除外），或通过从 `Exception` 派生来创建其自己的异常类。
- 派生类应该至少定义三个构造函数：一个无参数构造函数、一个用于设置消息属性，一个用于设置 `Message` 和 `InnerException` 属性。

```csharp
[Serializable]
public class InvalidDepartmentException : Exception
{
    public InvalidDepartmentException() : base() { }
    public InvalidDepartmentException(string message) : base(message) { }
    public InvalidDepartmentException(string message, Exception inner) : base(message, inner) { }
}
```

>---

### 6.5. 常见 .NET 异常类型

```csharp
ArithmeticException         // 算术运算期间出现的异常的基类，例如 DivideByZeroException 和 OverflowException。
ArgumentException           // 传递给方法的参数无效
ArgumentNullException       // 不应为 null 的参数传递了 null 值
ArrayTypeMismatchException	// 存储数组元素时，由于元素类型与数组类型不兼容引发。
ApplicationException        // 作为应用程序定义的异常的基类。
DivideByZeroException	    // 尝试将整数值除以零时引发。
FormatException             // 当参数的格式无效或复合格式字符串格式不正确时引发的异常。
IndexOutOfRangeException	// 索引小于零或超出数组边界时，尝试对数组编制索引时引发。
InvalidCastException	    // 从基类型显式转换为接口或派生类型在运行时失败时引发。
InvalidOperationException   // 当方法调用对于对象的当前状态无效时引发的异常。
NullReferenceException	    // 尝试引用值为 null 的对象时引发。
NotImplementedException     // 当请求的方法或操作未实现时引发的异常。 
OutOfMemoryException	    // 尝试使用新运算符分配内存失败时引发。 此异常表示可用于公共语言运行时的内存已用尽。
OverflowException	        // checked 上下文中的算术运算溢出时引发。
RuntimeBinderException      // 表示在处理 C# 运行时绑定中的动态绑定时发生的错误。
StackOverflowException	    // 执行堆栈由于有过多挂起的方法调用而用尽时引发；通常表示非常深的递归或无限递归。
TypeInitializationException	// 静态构造函数引发异常并且没有兼容的 catch 子句来捕获异常时引发。
```

>---

### 6.6. 捕捉非 CLS 异常

- 包括 C++/CLI 在内的某些 .NET 语言允许对象引发并非派生自 `Exception` 的异常，这类异常被称为非 CLS 异常或非异常。无法在 C# 中引发非 CLS 异常。可以在 `catch (RuntimeWrappedException ex)` 内捕获，通过 `RuntimeWrappedException.WrappedException` 属性访问原始异常。
- 如果为了响应非 CLS 异常需要执行某些操作（如写入日志文件），且无需访问异常信息时，请使用此方法。

```csharp
// Class library written in C++/CLI.
var myClass = new ThrowNonCLS.Class1();

try
{
    // throws gcnew System::String("I do not derive from System.Exception!");  
    myClass.TestThrow();
}
catch (System.Runtime.CompilerServices.RuntimeWrappedException e)
{
    String s = e.WrappedException as String;
    if (s != null)
        Console.WriteLine(s);
}
```

-  默认情况下，公共语言运行时包装所有异常。要禁用此行为，请将此程序集级别属性添加到代码中，通常位于 `AssemblyInfo.cs` 文件：

```csharp
[assembly: RuntimeCompatibilityAttribute(WrapNonExceptionThrows = false)];
```

---