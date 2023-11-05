## CSharp 类型

<!-- 值类型和引用类型是 C# 类型的两个主要类别：
值类型的变量包含类型的实例。对于值类型变量，会复制相应的类型实例。
引用类型的变量存储对其数据（对象）的引用。对于引用类型，两种变量可引用同一对象，对一个变量执行的操作会影响另一个变量所引用的对象。 -->

---
### 值类型

#### 整型数值类型

```csharp
// 有符号整数
sbyte   _int8 = -8;         // -2^7 ~ 2^7-1
short   _int16 = -16;       // -2^15 ~ 2^15-1
int     _int32 = -32;       // -2^31 ~ 2^31-1
long    _int64 = -64L;      // -2^63 ~ 2^63-1
// 无符号整数
byte    _uint8 = 8;         // 0 ~ 2^8-1
ushort  _uint16 = 16;       // 0 ~ 2^16
uint    _uint32 = 32;       // 0 ~ 2^32
ulong   _uint64 = 64l;      // 0 ~ 2^64
// 本机大小的整数
nint    _IntPtr = new IntPtr();     // 有符号本机 32 位或 64 位整数
nuint   _UIntPtr = new UIntPtr();   // 无符号本机 32 位或 64 位整数
```

> 不同进制数表示

```csharp
var decimalLiteral = 42;            // 十进制
var hexLiteral = 0x2A;              // 十六进制 0x, 0X
var binaryLiteral = 0b_0010_1010;   // 二进制 0b, 0B
```

> 整数文本后缀

```csharp
long _long_64 = 0xffffL;            // long, ulong 长整数后缀 L, l
uint _uint_32 = 32u;                // 无符号整数后缀 U, u
ulong _ulong_64 = 0xffffuL;         // 无符号长整数后缀 ul, lu, Ul, lU, UL, LU 
```

> 本机大小的整数

- 本机大小的整数类型具有特殊行为，因为存储是由目标计算机上的自然整数大小决定的。

```csharp
// sizeof, 获取本机整数的大小需要在不安全的上下文中
unsafe
{
    Console.WriteLine($"size of nint = {sizeof(nint)}");
    Console.WriteLine($"size of nuint = {sizeof(nuint)}");
}
// 本机整数的极值
Console.WriteLine($"nint.MinValue = {nint.MinValue}");
Console.WriteLine($"nint.MaxValue = {nint.MaxValue}");
Console.WriteLine($"nuint.MinValue = {nuint.MinValue}");
Console.WriteLine($"nuint.MaxValue = {nuint.MaxValue}");
// 数值转换：没有适用于本机大小整数文本的直接语法，可以改为使用其他整数值的隐式或显式强制转换
// 本机整数支持内置数值转换规则
nint a = (nint)otherInteger;
```

<br>

#### 浮点型数值类型

```csharp
float F_32 = 3.1415f;          // f 或 F 后缀，6~9 位精度
double D_64 = 3.1415;          // 隐式 d 或 D 后缀，15~17 位精度
decimal M_128 = 3.1415m;       // m 或 M 后缀，28~29 位精度
// 无穷定义
var n_inf = double.NegativeInfinity;    // 负无穷  
var p_inf = double.PositiveInfinity;    // 正无穷
// 非数值
var nan = double.NaN;   // 非数值
```

<br>

#### 内置数值转换

> 隐式数值转换

- 任何整数都可以隐式转换为任何浮点数。
- 相同大小的整型无符号类型和有符号类型之间不存在隐式转换（例如 `int` 和 `uint`）。不存在 `float` 或 `double` 与 `decimal` 之间的隐式转换。
- 整型类型之间，若值在目标类型范围内，则可以隐式转换。

```csharp
float i_f = int.MaxValue;
long ui_L = uint.MaxValue;
double f_d = float.MaxValue;
```

> 显式数值转换

- 整数类型之间的转换，结果取决于溢出检查。若在 `checked` 溢出检查上下文中，若结果在目标范围内则转换成功，否则引发 `OverflowException` 异常。若在 `unchecked` 未检查上下文中，则始终检查成功，大于目标类型时发生多余高位截断，小于目标类型时，有符号数为符号扩展补足高位，无符号数为零扩展补足高位。
- `decimal` 转换为整数类型时，若值位于目标类型的范围之外，则引发 `OverflowException` 异常。
- 浮点数转换为整数时，值先向零舍入到最接近的整数值，然后再按照整数类型之间的转换规则进行数值转换。
- `double` 转换为 `float` 时，将值舍入到最接近的 `float` 值。若过大或过小，则结果将为无穷大或零。
- `float` 和 `double` 转换为 `decimal` 时，源值转换为 `decimal` 表示形式，并四舍五入到 `decimal` 的精度。源值过小时转换为零，源值为非数字、无穷或无法表示为 `decimal` 时将引发 `OverflowException`。相反，`decimal` 转换为其他浮点类型时，分别舍入到最接近的目标类型。

```csharp
int L_i = (int)LongMax;
//int L_i_err = checked((int)LongMax);  // err, overflow
float d_f = (float)15d;
decimal fd_Ld = (decimal)3.1415d;
```

<br>

#### 布尔类型

- `bool` 用于表示一个布尔值，可以为 `true` 或 `false`。可以使用 `Convert` 类进行布尔转换：非零数值转换为 `true`，零转换为 `false`；字符串转换时忽略大小写。
 
```csharp
bool t = true;
bool f = false;
bool rt = Convert.ToBoolean("True");    // true
bool rt2 = Convert.ToBoolean(0);        // false
```

<br>

#### 字符类型

- `char` 类型用来表示 Unicode UTF-16 字符，类型支持比较、相等、增量和减量运算符。算数和逻辑位运算得到 `int` 结果。
- `char` 的值可以用字符文本、Unicode 转义序列（`\uUUUU`，四位）、十六进制转义序列（`\xXX`）表示。`char` 可以隐式转换为其它包含数值类型，或显式转换为未包含数值类型。其他类型则无法隐式转换为 `char`。
- 字符串类型将文本表示为 `char` 值的序列。

```csharp
char Ch = 'H';
char Ch_x = '\x65';
char Ch_u = '\u00ff';
```

<br>

#### 枚举类型

- 枚举类型是由基础整型数值类型的一组命名常量定义的值类型。使用 `enum` 定义枚举类型并指定其枚举成员的名称。默认情况下，枚举项关联的常数类型为 `int`，默认从零开始并按照文本顺序递增 1，可以显式指定关联的常数值。
- 可以使用枚举类型，通过一组互斥值或选项组合来表示选项。若要表示选项组合，请将枚举类型定义为位标志。

```csharp
public enum Days
{
    None      = 0b_0000_0000,  // 0
    Monday    = 0b_0000_0001,  // 1
    Tuesday   = 0b_0000_0010,  // 2
    Wednesday = 0b_0000_0100,  // 4
    Thursday  = 0b_0000_1000,  // 8
    Friday    = 0b_0001_0000,  // 16
    Saturday  = 0b_0010_0000,  // 32
    Sunday    = 0b_0100_0000,  // 64
    Weekend   = Saturday | Sunday
}
```

- `System.Enum` 类型是所有枚举类型的抽象基类。可在基类约束中使用 `System.Enum`（称为枚举约束），以指定类型参数为枚举类型。对于任何枚举类型，都存在分别与 `System.Enum` 类型的装箱和取消装箱相互转换。
- 对于任何枚举类型，枚举类型与其基础整型类型之间存在显式转换。使用 `Enum.IsDefined` 方法来确定枚举类型是否包含具有特定关联值的枚举成员。

```csharp
Console.WriteLine(1.ConvertEnum<Season>());     // Summer
Console.WriteLine((-1).IsDefinedByEnum<Season>());  // False

Season val = (Season)1;     // 强制转换

public enum Season
{
    Spring,
    Summer,
    Autumn,
    Winter
}
public static class EnumExt
{
    public static T ConvertEnum<T>(this int eVal) where T : Enum
        => (T)Enum.ToObject(typeof(T), eVal);
    public static bool IsDefinedByEnum<T>(this int eVal) where T : Enum 
        => Enum.IsDefined(typeof(T), eVal);
}
```

<br>

#### 结构类型

- 结构类型（struct type）是一种可封装数据和相关功能的值类型，结构类型具有值语义，其变量包含类型的实例。使用 `struct` 关键字定义结构类型：
- 从 C#10 开始，可定义记录结构类型。记录类型提供用于封装数据的内置功能。可同时定义 `record struct` 和 `readonly record struct` 类型。

```csharp
public struct Coords(double x, double y)
{
    public double X => x;
    public double Y => y;
    public override string ToString() => $"({X}, {Y})";
}
// 记录结构声明
public record struct Data<T> where T : class
{
    public T Bindings;
    public readonly int GUI { get; init; }
}
```

> readonly 结构类型和 readonly 实例成员

- 可以使用 `readonly` 修饰符来声明结构类型为不可变，编译器会强制所有的实例字段都是只读的。

```csharp
readonly struct Sample
{
    public readonly string FirstName;
    public readonly string LastName;
}
```

- 还可以使用 `readonly` 修饰符来结构的实例成员不会被修改，因此只限定当前成员而不用将整个结构类型声明为 `readonly`。

```csharp
struct Sample
{
    public string FirstName;
    public readonly string LastName;
}
```

- 通常，将 `readonly` 修饰符应用于以下类型的实例成员：
  - 方法：该方法不能修改结构中的其他实例成员，因为它们在函数域内是只读的。
  - 属性和索引器：指定访问器不会修改实例成员，可以修饰整个属性或索引器，或修饰单个访问器。
  - 只读字段或属性可以在构造函数中重新初始化。
  - 事件：具有访问器的事件，不能应用于单个事件访问器和事件字段。

```csharp
public struct Sample
{
    public readonly int x;
    public int y;
    public readonly int Sum() => x + y;
    public int Data { readonly get; set; }
    public readonly int ReadonlyData { get; init; }
    public readonly event Action Func { add { /*..*/ } remove { /*..*/ } }
}
```

> with 表达式（C#10）

- 可以使用 `with` 表达式生成修改了指定属性或字段的结构类型实例的副本

```csharp
Point p1 = new(1, 1);
Point p2 = p1 with { X = 0 };
Console.WriteLine("Point(p1) = ({0},{1})", p1.X, p1.Y);     // (1, 1)
Console.WriteLine("Point(p2) = ({0},{1})", p2.X, p2.Y);     // (0, 1)

public struct Point(double x, double y)
{
    public static Point Origin => default;
    public double X { get => x; set => x = value; }
    public double Y { get => y; set => y = value; }
}
```

> ref 结构类型

- 可以在结构类型的声明中使用 `ref` 修饰符。`ref struct` 类型的实例是在堆栈上分配的，不能转义到托管堆。`ref struct` 有以下限制：
  - `ref struct` 不能是数组的元素类型、不能实现接口、不能是类型参数、不能在迭代器中使用。
  - `ref struct` 不能是类或非 `ref struct` 的字段的声明类型。
  - `ref struct` 不能被装箱为 `System.ValueType` 或 `System.Object`。
  - `ref struct` 变量不能由 Lambda 表达式或本地函数捕获。
  - `ref struct` 变量不能在 `async` 方法中使用，但可以在同步方法中使用 `ref struct` 变量，例如，在返回 `Task` 或 `Task<TResult>` 的同步方法中。

```csharp
interface ISample { }
interface ISample<T> { }
public ref struct Point(double x, double y) //:ISample  // err: 不能实现接口
{
    public static Point Origin => default;
    public double X { get => x; set => x = value; }
    public double Y { get => y; set => y = value; }
}
class Sample
{
    //Point[] points;   // err: 不能构造数组
    //ISample<Point> sample;    // err: 不能是类型参数
    //Point Origin;   // err: 不能是类或非 ref 结构的字段
    //object origin = (object)Point.Origin;  // err: ref 结构无法装箱成 object
    async void AsyncTest()
    {
        // Point Origin = new(0, 0); // err: async 方法中无法使用 ref 结构
    }
}
```

- 从 C#11 开始，可以在 `ref struct` 中声明 `ref` 字段。`ref` 字段可能具有 null 值，使用 `Unsafe.IsNullRef<T>(ref T src)` 方法确定 `ref` 字段是否为 `null`。
- 当 `readonly` 修饰 `ref` 字段时：
  - `ref`：在任何时候，都可以使用 `=` 为此字段关联引用赋值，或使用 `= ref` 重新赋值引用。
  - `readonly ref`：只能在构造函数或 `init` 访问器中使用 `= ref` 重新赋值引用。可以在字段访问修饰符允许的任何时间点使用 `=` 为此字段关联引用赋值。 
  - `ref readonly`：在任何时候，都不能使用 `=` 为此类字段关联引用赋值，但是可以使用 `= ref` 重新赋值引用。
  - `readonly ref readonly`：只能在构造函数或 `init` 访问器中通过 `= ref` 重新赋值引用。

```csharp
class DATA
{
    public static int F_Data = 0;
    public static int RF_Data = 0;
    public static int FR_Data = 0;
    public static int RFR_Data = 0;
}
ref struct Ref_Data
{
    public ref int F_Data;              // 表示引用可修改，值可修改
    public readonly ref int RF_Data;    // 表示引用不可修改，值可修改
    public ref readonly int FR_Data;    // 表示引用可修改，值不可修改
    public readonly ref readonly int RFR_Data;  // 表示引用和值均不可修改
    public Ref_Data()
    {
        // 可以在构造函数或 init 属性访问器中重新赋值
        F_Data = ref DATA.F_Data;
        FR_Data = ref DATA.FR_Data;
        RF_Data = ref DATA.RF_Data;
        RFR_Data = ref DATA.RFR_Data;
    }
}
```

> 内联数组

- 从 C#12 开始，可以将内联数组声明为 `struct` 类型。内联数组是包含相同类型的 N 个元素的连续块的结构，它是一个安全代码，等效于仅在不安全代码中可用的固定缓冲区声明，编译器可以利用有关内联数组的已知信息。内联数组是包含单个字段、且未指定其他任何的显式布局的 `struct`。
- 使用 `System.Runtime.CompilerServices.InlineArrayAttribute` 特性修饰 `struct` 类型，并指定一个大于零的值。

```csharp
[System.Runtime.CompilerServices.InlineArray(10)]
public struct InlineArray<T>
{
    private T Elem;
}
```

- 可以像访问数组一样访问内联数组，以读取和写入值，还可以使用范围和索引运算符。
- 对单个字段的类型有最低限制：它不能是指针类型，但可以是任何引用类型或任何值类型。几乎可以将内联数组与任何 C# 数据结构一起使用。
- 内联数组是一种高级语言功能。它们适用于高性能方案，在这些方案中，内联的连续元素块比其他替代数据结构速度更快。
- 运行时团队和其他库作者使用内联数组来提高应用的性能。内联数组使开发人员能够创建固定大小的 `struct` 类型数组。具有内联缓冲区的结构应提供类似于不安全的固定大小缓冲区的性能特征。

```csharp
var buffer = new Buffer();
for (int i = 0; i < 10; i++)
    buffer[i] = i;
foreach (var i in buffer)
    Console.WriteLine(i);

[System.Runtime.CompilerServices.InlineArray(10)]
public struct Buffer
{
    private int _element0;
}
```

<br>

#### 元组类型

- 元组功能提供了简洁的语法来将多个数据元素分组成一个轻型数据结构。定义元组类型，需要指定其所有数据成员的类型，可以在元组初始化表达式中或元组类型的定义中显式指定元组字段名称（默认为 `type.Item1`、`type.Item2` ...）。
- 元组类型支持相等运算符 `==` 和 `!=`。

```csharp
(double x, double y) Point1 = new(1, 1);
(double, double) Point2 = new();
Point2.Item1 = Point1.x;
if(Point2 != Point1)
    Console.WriteLine(Point2);  // (1,0)
```

- 元组最常见的用例之一是作为方法返回类型，可以将方法结果分组为元组返回类型，而不是定义 `out` 方法参数。可以使用赋值运算符 `=` 在单独的变量中析构元组实例，析构元组时可以使用弃元。

```csharp
var (X, Y, _) = GetRandomPoint();   // 析构元组
Console.WriteLine("The Point2D = ({0},{1})", X, Y);
static (int, int, int) GetRandomPoint()
{
    Random rand = new Random(DateTime.Now.Millisecond);
    return (rand.Next(-128, 128), rand.Next(-128, 128), rand.Next(-128, 128));
}
```

> 解构函数

- 可以在 `struct`、`class`、`record`、`interface` 中声明名为 `Deconstruct` 的方法，该方法返回 `void`，且拥有至少两个 `out` 参数。该方法为这些类型提供解构为元组的功能支持，`Deconstruct` 方法支持重载。
- 在声明主构造函数的记录中，编译器会为其自动生成一个 `Deconstruct` 方法，其参数列表对照位置记录中的位置参数。

```csharp
 // 为 class 定义解构函数
var (fname, lname) = new Person("Hello", "World");
class Person(string firstName, string lastName)
{
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;
    public void Deconstruct(out string firstName, out string lastName)
        => (firstName, lastName) = (FirstName, LastName);
}

// 解构一个位置记录
var (fname, lname) = new Person("Hello", "World");
record Person(string firstName, string lastName);
```

- 解构函数也可以是扩展方法，可以为指定类型提供额外的 `Deconstruct` 扩展方法。

```csharp
public static void Deconstruct(this <Type> destTypeObj, out <Type1> val, out <Type2> val2[, out < Type3 > val3...]0) { }

var (<Type1> rt1, <Type2> rt2[, ...]) = destTypeObj;
```

- 系统类型的扩展方法：为了方便起见，某些系统类型提供 `Deconstruct` 方法。例如 `System.Collections.Generic.KeyValuePair<TKey,TValue>` 类型提供此功能，循环访问 `Dictionary` 时，每个元素都是 `KeyValuePair<TKey,TValue>`。

```csharp
Dictionary<string, int> snapshotCommitMap = new(StringComparer.OrdinalIgnoreCase)
{
    ["https://github.com/dotnet/docs"] = 16_465,
    ["https://github.com/dotnet/runtime"] = 114_223,
    ["https://github.com/dotnet/installer"] = 22_436,
    ["https://github.com/dotnet/roslyn"] = 79_484,
    ["https://github.com/dotnet/aspnetcore"] = 48_386
};
foreach (var (repo, commitCount) in snapshotCommitMap)
    Console.WriteLine($"The {repo} repository had {commitCount:N0} commits as of November 10th, 2021.");
```

<br>

#### 可为 null 的值类型

- 可为 null 值类型 `T?` 表示其基础值类型 `T` 的所有值及额外的 `null` 值，其默认值为 `null`。任何可为空的值类型都是泛型 `System.Nullable<T>` 结构的实例。
- 可以将 `is` 运算符与类型模式结合使用，既检查 null 的可为空值类型的实例，又检索基础类型的值。或使用 `Nullable<T>.HasValue` 指示可为空值类型的实例是否有基础类型的值，如果 `HasValue` 为 `true`，则 `Nullable<T>.Value` 获取基础类型的值。也可以使用空合并操作符将可空类型转换为其基础类型。

```csharp
int? n = Console.ReadLine().Parse<int>();
// 几种空判定s
if(n != null) 
    Console.WriteLine("The input is "+ n.Value);
if(n is int temp)
    Console.WriteLine("The input is " + temp);
if(n.HasValue)
    Console.WriteLine("The input is " + n.Value);
Console.WriteLine("The input is " + (n ?? int.MinValue));

public static partial class Ext
{
    public static T? Parse<T>(this IConvertible? val) where T : IConvertible?
    {
        T s_t = default;
        IConvertible temp = s_t switch
        {
            sbyte => Convert.ToSByte(val),
            byte => Convert.ToByte(val),
            short => Convert.ToInt16(val),
            ushort => Convert.ToUInt16(val),
            int => Convert.ToInt32(val),
            uint => Convert.ToUInt32(val),
            long => Convert.ToInt64(val),
            ulong => Convert.ToUInt64(val),
            float => Convert.ToSingle(val),
            double => Convert.ToDouble(val),
            decimal => Convert.ToDecimal(val),
            _ => null
        };
        return (T?)temp;
    }
}
```

- 可为空值类型拥有预定义的一元或二元运算符时，若至少存在一个 `null` 值时，运算结果也为 `null`。对于比较运算符 `<`、`>`、`<=` 和 `>=`，如果一个或全部两个操作数都为 `null`，则结果为 `false`。`null == null` 返回 `true`。


> 确定可为空的值类型

```csharp
Console.WriteLine($"int? is {(IsNullable(typeof(int?)) ? "nullable" : "non nullable")} value type");
Console.WriteLine($"int is {(IsNullable(typeof(int)) ? "nullable" : "non-nullable")} value type");

bool IsNullable(Type type) => Nullable.GetUnderlyingType(type) != null;

// Output:
// int? is nullable value type
// int is non-nullable value type
```

---
### 引用类型

#### object 对象类型

- `object` 类型是 `System.Object` 在 .NET 中的别名。在 C# 的统一类型系统中，所有类型（预定义类型、用户定义类型、引用类型和值类型）都是直接或间接从 `System.Object` 继承的。可以将任何类型的值赋给 `object` 类型的变量。
- 将值类型的变量转换为对象的过程称为装箱。将 `object` 类型的变量转换为值类型的过程称为拆箱。装箱是隐式的，拆箱是显式的。装箱和拆箱的概念是类型系统 C# 统一视图的基础。

> 装箱与拆箱

- 装箱用于在垃圾回收堆中存储值类型。装箱是值类型到 `object` 类型或到此值类型所实现的任何接口类型的隐式转换。对值类型装箱会在堆中分配一个对象实例，并将该值复制到新的对象中。
- 取消装箱（拆箱）是从 `object` 类型到值类型、或从接口类型到实现该接口的值类型的显式转换。首先要先检查对象实例，以确保它是给定值类型的装箱值，然后再将该值从实例复制到值类型变量中。被取消装箱的项必须是对一个对象的引用，该对象是先前通过装箱该值类型的实例创建的。尝试取消装箱 `null` 会导致 `NullReferenceException`， 尝试取消装箱对不兼容值类型的引用会导致 `InvalidCastException`。
- 如果值类型必须被频繁装箱，那么在这些情况下最好避免使用值类型。可通过使用泛型集合（例如 `System.Collections.Generic.List<T>`）来避免装箱值类型。
- 装箱和取消装箱过程需要进行大量的计算。对值类型进行装箱时，必须创建一个全新的对象，这可能比简单的引用赋值用时最多长 20 倍。取消装箱的过程所需时间可达赋值操作的四倍。

```csharp
int i = 123;
object o = i;  // implicit boxing
try
{
    int j = (short)o;  // attempt to unbox
    System.Console.WriteLine("Unboxing OK.");
}
catch (System.InvalidCastException e)
{
    System.Console.WriteLine("Error: Incorrect unboxing. \n{0}", e.Message);
    int j = (int)o;
    System.Console.WriteLine("Correct unboxing.");
}
```

<br>

#### string 字符串类型

- `string` 类型表示零个或多个 Unicode 字符的序列。使用相等运算符 `==` 和 `!=` 比较 `string` 对象的值，为不是比较对象的引用。

```csharp
string str1 = "hello";
string str2 = "h";
str2 += "ello";
Console.WriteLine(str1 == str2);  // true
Console.WriteLine(object.ReferenceEquals(str1, str2)); // false
```

> 字符串拼接

- `+` 用于拼接两个字符串片段。字符串是不可变的，每次赋值时，编译器实际上会创建一个新的字符串对象来保存新的字符序列，并将新对象赋值给目标，并将之前的内存用于垃圾回收。

```csharp
string str = "Hello " + "World!";
```

> 字符串索引

- `[]` 运算符可用于只读访问字符串的个别字符。

```csharp
string str = "test";
for (int i = 0; i < str.Length; i++)
  Console.Write(str[i] + " ");
// Output: t e s t
```

> 字符串内插

- `$` 字符将字符串字面量标识为内插字符串，内插字符串是可能包含内插表达式的字符串文本。将内插字符串解析为结果字符串时，带有内插表达式的项会替换为表达式结果的字符串表示形式。
- 内插字符串初始化常量时，所有的内插表达式也必须是常量字符串。C#11 起内插表达式支持使用换行符，以使表达式更具有可读性

```csharp
$"{<interpolationExpression>[,<alignment>][:<formatString>]}"
// - interpolationExpression     生成需要设置格式的结果的表达式
// - alignment                   常数表达式，定义对齐方式和最小字符宽度，负值表示左对齐，正值表示右对齐
// - formatString                受表达式结果类型支持的格式字符串，例如 DateTime 格式化输出

Console.WriteLine($"|{"Left",-7}|{"Right",7}|");
// |Left   |  Right|

const int FieldWidthRightAligned = 20;      // $"{{" 打印 {
Console.WriteLine($"{{{Math.PI,FieldWidthRightAligned}}} - default formatting of the pi number");
Console.WriteLine($"{{{Math.PI,FieldWidthRightAligned:F3}}} - display only three decimal digits of the pi number");
//{   3.141592653589793} - default formatting of the pi number
//{               3.142} - display only three decimal digits of the pi number

string message = $"The usage policy for {safetyScore} is {
    safetyScore switch
    {
        > 90 => "Unlimited usage",
        > 80 => "General usage, with daily safety check",
        > 70 => "Issues must be addressed within 1 week",
        > 50 => "Issues must be addressed within 1 day",
        _ => "Issues must be addressed before continued use",
    }}";
```

> 逐字字符串

- `@` 指示将原义解释字符串。简单转义序列（如代表反斜杠的 `"\\"`）、十六进制转义序列（如代表大写字母 A 的 `"\x0041"`）和 Unicode 转义序列（如代表大写字母 A 的 `"\u0041"`）都将按字面解释。引号转义 `""` 不会按字面解释。
- 逐字内插字符串中，大括号转义序列（`{{` 和 `}}`）不按字面解释。

```csharp
string filename1 = @"c:\documents\files\u0066.txt";
string filename2 = "c:\\documents\\files\\u0066.txt";
Console.WriteLine(filename1);
Console.WriteLine(filename2);
// The example displays the following output:
//     c:\documents\files\u0066.txt
//     c:\documents\files\u0066.txt

string str = $@"{{{Math.PI,20}}} >> ""default formatting of the pi number""";
Console.WriteLine(str);
//{   3.141592653589793} >> "default formatting of the pi number"
```

> 原始字符串

- 原始字符串字面量从 C#11 开始可用。字符串字面量可以包含任意文本，而无需转义序列，字符串字面量可以包括空格和新行、嵌入引号以及其他特殊字符。原始字符串字面量用至少三个双引号（`"""`） 括起来。

```csharp
var message = """
This is a multi-line
    string literal with the second line indented.
"""
// 原始字符串的起始、结束引导序列长度要超过字符串中最长的引号序列长度
"""""
This raw string literal has four """", count them: """" four!
embedded quote characters in a sequence. That's why it starts and ends
with five double quotes.

You could extend this example with as many embedded quotes as needed for your text.
"""""
```

> UTF-8 字符串字面量

- .NET 中的字符串是使用 UTF-16 编码存储的。UTF-8 是 Web 协议和其他重要库的标准。从 C#11 开始，可以将 `u8` 后缀添加到字符串字面量以指定 UTF-8 编码。UTF-8 字面量存储为 `ReadOnlySpan<byte>` 对象。 UTF-8 字符串字面量的自然类型是 `ReadOnlySpan<byte>`。UTF-8 字符串字面量不能与字符串内插结合使用。

```csharp
using System.Text;

ReadOnlySpan<byte> strU8 = "Hello world!"u8;
string strU16 = Encoding.UTF8.GetString(strU8);
Console.WriteLine(strU16);

string str = "Hello world!";
ReadOnlySpan<byte> bytes = Encoding.UTF8.GetBytes(str);
```

<br>

#### Array 数组类型

- 数组是一种数据结构，类型 `System.Array` 是所有数组类型的抽象基类型，它包含多个通过计算索引访问的变量。数组中的元素均为同一种类型。使用 `new` 运算创建数组的实例。
- 数组具有确定与每个数组元素关联的索引数的排名，其中数组的秩被称为数组的维数。秩为一的数组成为 “单维数组”，大于 1 的数组称为 “多维数组”。数组的每个维度都有一个关联的长度，维度长度决定了该维度的有效索引范围。数组的元素总数为数组的秩和每个维度长度的乘积。
  
> 数组的声明

```csharp
// 几种声明数组的形式
int[] arr1 = new int[10];        // 元素均初始化为 0
int[] arr2 = { 0, 1, 2, 3 };     // 初始化构造
int[] arr3 = new int[] { 1, 2, 3 };
int[] arr4 = new int[3] { 1, 2, 3 };
int[] arr5 = new[] { 1, 2, 3 };
int[] arr6 = [10, 20, 30];       // 集合表达式

// 多维数组的声明
int[] a1 = new int[10];             // 一维数组
int[,] a2 = new int[10, 5];         // 二维数组
int[,,] a3 = new int[10, 5, 2];     // 三维数组
```

> 数组的访问

```csharp
// 一维数组
int[] arr = [1, 2, 3, 4, 5, 6, 7, 8, 9];
for (int i = 0; i < arr.Length; i++)
    arr[i] *= arr[i];
Console.WriteLine(arr.Rank); // 数组的秩
Console.WriteLine(string.Join(",", arr));

// 多维数组
int[,] Matrix3_4 = new int[3, 4] {
    { 31, 32, 33, 34 },
    { 24, 25, 26, 27 },
    { 19, 18, 17, 16 }
};
Console.WriteLine(Matrix3_4.Rank);
for (int L = 0, r = 0, i = 0; L < Matrix3_4.Length; L++)
{
    (r, i) = Math.DivRem(L, Matrix3_4.GetLength(0) + 1);
    Console.WriteLine($"Matrix3_4[{r},{i}] = {Matrix3_4[r, i]}");
}
```

> 交替数组

- 包含数组类型元素的数组有时称为 “交错数组”，因为元素数组的长度不必全都一样。交错数组是一维数组。

```csharp
int[][] arrs = {
    [1,2,3,4],
    [10,20,30,40,50],
    [100,1000]
};
```

<br>

#### delegate 委托类型

- 委托类型的定义与方法签名相似，它有一个返回值和任意数目任意类型的参数。委托类型是一种可用于封装命名方法或匿名方法的引用类型，是面向对象的、类型安全的和可靠的。
- 使用 `delegate` 关键字声明委托类型，编译器将委托相关的操作代码的调用映射到 `System.Delegate` 和 `System.MulticastDelegate` 类成员的方法调用。必须使用具有兼容返回类型和输入参数的方法或 Lambda 表达式实例化委托。在实例化委托时，可以将委托的实例与任何兼容的方法相关联，并可以通过委托实例调用方法。
- 将方法作为参数进行引用的能力使委托成为定义回调方法的理想选择。委托类似于 C++ 函数指针，但委托面向对象，会同时封装对象实例和方法，因此委托允许将方法作为参数进行传递。

```csharp
MessageDelegate debug = Console.WriteLine;
if (debug is MulticastDelegate or Delegate)
    debug("Hello World >>> " + debug.Method.Name);

public delegate void MessageDelegate(string message);
public delegate int AnotherDelegate(int num1, int num2);
```

- 委托对象可以由方法名称、Lambda 表达式或匿名方法进行构造。作为委托参数传递的方法必须具有与委托声明相同的签名，委托实例可以封装静态方法或实例方法。
- 在 .NET 中，`System.Action`、`System.Func`、`System.Predicate` 类型为许多常见委托提供泛型定义。

```csharp
Action<string> MessagePrint = null;
// 方法
void Print(string mess) => Console.WriteLine("Function : " + mess);
MessagePrint += Print; 
// delegate 匿名方法
MessagePrint += 
delegate (string mess)
{
    Console.WriteLine("Delegate : " + mess);
};
// Lambda 表达式
MessagePrint += message => Console.WriteLine("Lambda : " + message);

// 委托调用
MessagePrint("Hello World!"); 
MessagePrint?.Invoke("Hello World!"); // 等效写法
```

> delegate 匿名方法

- `delegate` 运算符创建一个可以转换为委托类型的匿名方法。匿名方法可以转换为 `System.Action` 和 `System.Func<TResult>` 等类型，用作许多方法的参数。

```csharp
// delegate 匿名方法
Func<int, int, int> sum = delegate (int a, int b) { return a + b; };
// lambda 匿名方法
var _Debug = (string message) => MessagePrint?.Invoke(message);
```

> 多播委托

- 可以通过使用 `+` 组合多个委托对象分配到一个委托实例中。多播委托中包含已分配委托列表，此多播委托被调用时会按照添加的先后顺序依次调用列表中的委托。`-` 用于从多播委托中删除组件委托。`+=` 可以将方法或匿名方法构造为委托对象并分配到多播委托中，`-=` 则表示从多播委托中移除该方法的委托实例。`+`、`-` 运算符支持委托对象和方法组之间的运算。
- 删除委托对象时，若右操作数是 Lambda 表达式或匿名方法（非匿名类型）时，此操作无效。

```csharp
class Sample
{
    delegate void SampleDelegate();
    private static void Function_1() => Console.WriteLine("Function_1");
    private static void Function_2() => Console.WriteLine("Function_2");

    static SampleDelegate Action_1 = delegate { Console.WriteLine("Action_1"); };
    static SampleDelegate Action_2 = delegate { Console.WriteLine("Action_2"); };

    static SampleDelegate Lambda_1 = () => Console.WriteLine("Lambda_1");
    static SampleDelegate Lambda_2 = () => Console.WriteLine("Lambda_2");

    static void Main(string[] args)
    {
        SampleDelegate MultoDel_1 = Function_1;
        MultoDel_1 += Action_1;
        MultoDel_1 += Lambda_1;
        MultoDel_1?.Invoke();
        // Function_1   Action_1    Lambda_1
        var MultoDel_2 = Action_2 + Function_2 + Lambda_2;
        MultoDel_2?.Invoke();
        // Action_2     Function_2  Lambda_2
        MultoDel_1 = MultoDel_1 + MultoDel_2 - Function_1 - Lambda_2;
        MultoDel_1?.Invoke();
        // Action_1     Lambda_1    Action_2    Function_2
    }
}
```

<br>

#### dynamic 动态类型

- `dynamic` 类型表示变量的使用和对其成员的引用绕过编译时类型检查，改为在运行时解析这些操作。在大多数情况下，`dynamic` 类型与 `object` 类型的行为类似，具体而言，任何非 Null 表达式都可以转换为 `dynamic` 类型。
- 编译器不会对包含类型 `dynamic` 的表达式的操作进行解析或类型检查，编译器将有关该操作信息打包在一起，之后这些信息会用于在运行时评估操作，因此 `dynamic` 类型只在编译时存在，在运行时则不存在。

```csharp
using System;
class Program
{
    static void Main(string[] args)
    {
        ExampleClass ec = new ExampleClass();
        Console.WriteLine(ec.ExampleMethod(10));
        Console.WriteLine(ec.ExampleMethod("value"));
        // The following line causes a compiler error because ExampleMethod takes only one argument.
        //Console.WriteLine(ec.ExampleMethod(10, 4));

        dynamic dynamic_ec = new ExampleClass();
        Console.WriteLine(dynamic_ec.ExampleMethod(10));
        // Because dynamic_ec is dynamic, the following call to ExampleMethod
        // with two arguments does not produce an error at compile time.
        // However, it does cause a run-time error.
        //Console.WriteLine(dynamic_ec.ExampleMethod(10, 4));
    }
}
class ExampleClass
{
    static dynamic _field;
    dynamic Prop { get; set; }
    public dynamic ExampleMethod(dynamic d)
    {
        dynamic local = "Local variable";
        int two = 2;
        if (d is int)
            return local;
        else
            return two;
    }
}
// Results:
// Local variable
// 2
// Local variable
```

<br>

#### record 记录类型

- 从 C#9 开始，可以使用 `record` 修饰符定义一个引用类型，用来提供用于封装数据的内置功能。C#10 允许 `record class` 语法作为同义词来阐明引用类型，并允许 `record struct` 使用相同功能定义值类型。
- 位置记录：在记录上声明主构造函数时，编译器会为记录类型自动生成一个位置构造函数，同时根据位置参数自动生成一个解构函数 `Deconstruct` 以支持将位置记录解构为元组，并在该位置记录中为主构造函数的参数生成公共属性。

```csharp
public record Person(string FirstName, string LastName);
// 相当于
public record Person{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    // 结构函数在记录中自动生成，可以声明方法重载或显式声明默认的 Deconstruct
    public void Deconstruct(out string firstName, out string lastName) 
    => (firstName, lastName) = (FirstName, LastName)
}

public record struct Point(int x, int y);
// 相当于
public record struct Point{
    public int x {get; set;}
    public int y {get; set;}
}

public readonly record struct Score(int Math, int English);
// 相当于
public readonly record struct Score{
    public int Math { get; init; }
    public int English { get; init; }
}
```

> 属性定义的位置语法

- 在创建实例时，可以使用位置参数来声明记录的属性，并初始化属性：
- 在为记录的属性定义使用位置语法时，编译器将创建以下内容：
  - 为记录声明中提供的每个位置参数提供一个公共的自动实现的属性：对于 `record` 类型，为 `required` 只读属性；对于 `readonly record struct` 类型，为只读属性；对于 `record struct` 类型，为读写属性。
  - 在主构造函数上，它的参数与记录声明上的位置参数匹配。
  - 对于 `record struct` 类型，则是将每个字段设置为其默认值的无参数构造函数。

* 若要更改自动实现的属性的可访问性或可变性，或为访问器提供实现，可以在源中自行定义同名的属性，并从记录的位置参数初始化该属性。

```csharp
public record Person(string FirstName, string LastName, string Id)
{
    internal string Id { get; init; } = Id;
}
```

> 位置记录中的解构函数

- 为了支持将 `record` 对象能解构成元组，我们给 `record` 添加解构函数 `Deconstruct`。声明主构造函数的记录定义为位置记录，该位置记录会为主构造函数中的位置参数自动生成一个解构函数。

```csharp
record Person
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public Person(string firstName, string lastName) 
        => (FirstName, LastName) = (firstName, lastName);
    public void Deconstruct(out string firstName, out string lastName) 
        => (firstName, lastName) = (FirstName, LastName);
}
// 相当于
record Person(string FirstName, string LastName);
```

- 解构记录为元组。

```csharp
var (first, last) = new Person("Hello", "World");
record Person(string FirstName, string LastName);
```

- 重定义解构函数或重载解构函数 `Deconstruct`

```csharp
using System.Diagnostics;

var (first, last) = new Person("Hello", "World");
var (firstName, _, Number) = new Person("Hello", "World") { PhoneNumber = "5566-6655" };

record Person(string FirstName, string LastName)
{
    public string PhoneNumber { get; set; } = "";
    // 重定义
    public void Deconstruct(out string firstName, out string lastName)
    {
        Console.WriteLine("Use Deconstruct >> " + new StackFrame(0, true));
        (firstName, lastName) = (FirstName, LastName);
    }
    // 重载
    public void Deconstruct(out string firstName, out string lastName, out string PhoneNumber)
    {
        firstName = FirstName;
        lastName = LastName;
        PhoneNumber = this.PhoneNumber;
    }
}
```

> 值相等性

- 对于 `class` 类型，两个对象引用内存中的同一对象，则这两个对象相等。
- 对于 `struct` 类型，两个对象是相同的类型并且存储相同的值，则这两个对象相等。
- 对于 `record` 类型，如果两个对象是相同的类型且存储相同的值，则这两个对象相等。

```csharp
class Sample
{
    public record Person(string FirstName, string LastName, string[] PhoneNumbers);

    static void Main()
    {
        var phoneNumbers = new string[2];
        Person person1 = new("Nancy", "Davolio", phoneNumbers);
        Person person2 = new("Nancy", "Davolio", phoneNumbers);

        Console.WriteLine(person1 == person2); // output: True
        person1.PhoneNumbers[0] = "555-1234";
        Console.WriteLine(person1 == person2); // output: True
        Console.WriteLine(ReferenceEquals(person1, person2)); // output: False
    }
}
```

- 为实现值相等性，编译器合成了几种方法：
  - `Object.Equals(Object)` 的替代，无法显式声明此替代。
  - 运算符 `==` 和 `!=` 的替代，无法显式声明这些运算符。
  - `virtual` 或 `sealed` 的 `Equals(R? other)`，其中 `R` 是记录类型。此方法实现 `IEquatable<T>`，可以显式声明此方法，还应该提供 `GetHashCode` 的实现。
  - `Object.GetHashCode()` 的替代，可以显式声明此方法。
  - 提供返回 `Type` 的 `EqualityContract` 只读属性的实现，可以显式声明此属性。该属性在密封记录中是 `private` 的，在可继承的记录中是 `protected virtual` 的。由于在默认实现的 `GetHashCode` 方法中调用了 `EqualityContract`，因此不建议在此属性中调用 `GetHashCode` 方法。  

```csharp
using System.Diagnostics;

Person p1 = new("Hello", "World");
Person pClone = p1;
pClone.PhoneNumber = "6666-5555";
Console.WriteLine(p1);
Console.WriteLine(Object.ReferenceEquals(p1, pClone));   // true

var p2 = p1 with { PhoneNumber = "5566-6655" };         // with 调用复制构造函数
Console.WriteLine(p2);
Console.WriteLine(Object.ReferenceEquals(p1, p2));      // false

var p3 = p1 with { };               // with 调用复制构造函数
Console.WriteLine(p3 == p1);        // 调用 Person.Equals, true
Console.WriteLine(Object.ReferenceEquals(p1, p3));      // false

record Person(string FirstName, string LastName) : IEquatable<Person>
{
    protected virtual Type EqualityContract
    {
        get
        {
            Console.WriteLine("Use EqualityContract at " + new StackFrame(1).GetMethod().Name);
            return this.GetType();
        }
    }
    public override int GetHashCode()
    {
        Console.WriteLine("Use GetHashCode");
        return unchecked((EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295
               + EqualityComparer<string>.Default.GetHashCode(FirstName)) * -1521134295
               + EqualityComparer<string>.Default.GetHashCode(LastName));
    }
    public virtual bool Equals(Person? other)
    {
        Console.WriteLine("Use Equals");
        return (object)other != null
                && EqualityContract == other.EqualityContract
                && EqualityComparer<string>.Default.Equals(FirstName, other.FirstName)
                && EqualityComparer<string>.Default.Equals(LastName, other.LastName);
    }
    protected Person(Person origin)
    {
        Console.WriteLine("Use Clone");
        (FirstName, LastName) = origin;
        PhoneNumber = origin.PhoneNumber;
    }
    public string PhoneNumber { get; set; } = "";
}
/*
Person { FirstName = Hello, LastName = World, PhoneNumber = 6666-5555 }
True
Use Clone
Person { FirstName = Hello, LastName = World, PhoneNumber = 5566-6655 }
False
Use Clone
Use Equals
Use EqualityContract at Equals
Use EqualityContract at Equals
True
False
*/
```

> 非破坏性变化

- 若需要复制包含一些修改的实例，可以使用 `with` 表达式来实现非破坏性变化。`with` 表达式创建一个新的记录实例，该实例是现有记录实例的一个副本，并修改了指定的属性或字段。

```csharp
class Sample
{
    public record Person(string FirstName, string LastName)
    {
        public string[] PhoneNumbers { get; init; }
    }

    public static void Main()
    {
        Person person1 = new("Nancy", "Davolio") { PhoneNumbers = new string[1] };
        Console.WriteLine(person1);
        // output: Person { FirstName = Nancy, LastName = Davolio, PhoneNumbers = System.String[] }

        Person person2 = person1 with { FirstName = "John" };
        Console.WriteLine(person2);
        // output: Person { FirstName = John, LastName = Davolio, PhoneNumbers = System.String[] }
        Console.WriteLine(person1 == person2);
        // output: False

        person2 = person1 with { PhoneNumbers = new string[1] };
        Console.WriteLine(person2);
        // output: Person { FirstName = Nancy, LastName = Davolio, PhoneNumbers = System.String[] }
        Console.WriteLine(person1 == person2); 
        // output: False

        person2 = person1 with { };
        Console.WriteLine(person1 == person2); 
        // output: True
    }
}
```

- `with` 表达式可以设置位置属性或使用标准属性语法创建的属性。显式声明属性必须有一个 `init` 或 `set` 访问器才能在 `with` 表达式中进行更改。
- `with` 表达式的结果是一个浅的副本，这意味着对于引用属性，只复制对实例的引用。原始记录和副本最终都具有对同一实例的引用。
- 编译器合成了一个克隆方法 `Clone` 和一个复制构造函数 `recordType(recordType origin)`，虚拟克隆方法返回由复制构造函数初始化的新记录。用户不能替代克隆方法，也不能在任意记录类型中创建名为 `Clone` 的成员。克隆方法的实际名称是由编译器生成的，当使用 `with` 表达式时，编译器将创建调用克隆方法的代码，然后设置 `with` 表达式中指定的属性。复制构造函数可以被显式定义，在非密封记录中必须是 `public` 或 `protected`。
- `with` 表达式会调用类型的复制构造函数。

```csharp
Person p1 = new("Hello", "World");
Person pClone = p1;
pClone.PhoneNumber = "6666-5555";
Console.WriteLine(p1);
Console.WriteLine(Object.ReferenceEquals(p1, pClone));   // true

var p2 = p1 with { PhoneNumber = "5566-6655" };
Console.WriteLine(p2);
Console.WriteLine(Object.ReferenceEquals(p1, p2));      // false

var p3 = p1 with { };
Console.WriteLine(Object.ReferenceEquals(p1, p3));      // false, 值相等性

sealed record Person(string FirstName, string LastName)
{
    private Person(Person origin)
    {
        Console.WriteLine("Use Clone");
        (FirstName, LastName) = origin;
        PhoneNumber = origin.PhoneNumber;
    }
    public string PhoneNumber { get; set; } = "";
}
```

> 用于显示的内置格式设置

- 记录类型具有编译器生成的 `ToString` 方法，可显式公共属性和字段的名称和值。 `ToString` 方法返回一个格式如下的字符串：`<record type name> { <property name> = <value>, <property name> = <value>, ...}`，其中每个 `<value>` 打印的字符串是属性或字段对应类型的 `ToString()`。为了实现此功能，编译器在 `record class` 类型中合成了一个虚拟 `PrintMembers` 方法和一个 `ToString` 替代，此成员在 `record struct` 类型中为 `private`。

```csharp
using System.Text;

PointArray X = new((0, 0), (1, 1), (2, 2), (3, 3), (4, 4));
Console.WriteLine(X);   // Output: (0,0),(1,1),(2,2),(3,3),(4,4)

public record struct Point(int x, int y)
{
    public static implicit operator Point((int, int) p) => new Point(p.Item1, p.Item2);
    public override string ToString() => $"({this.x},{this.y})";
}
public readonly record struct PointArray(params Point[] points)
{
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        if (points.Length > 0)
        {
            sb = new StringBuilder(points[0].ToString());
            foreach (Point p in points[1..points.Length])
                sb.Append("," + p.ToString());
        }
        return sb.ToString();
    }
}
```

- 自定义 PrintMembers 方法

```csharp
using System.Text;

PointArray X = new((0, 0), (1, 1), (2, 2), (3, 3), (4, 4));
Console.WriteLine(X);   
// Output: PointArray { points = { (0,0), (1,1), (2,2), (3,3), (4,4) } }

public record struct Point(int x, int y)
{
    public static implicit operator Point((int, int) p) => new Point(p.Item1, p.Item2);
    public override string ToString() => $"({this.x},{this.y})";
}
public readonly record struct PointArray(params Point[] points)
{
    public readonly int Length => points.Length;
    private bool PrintMembers(StringBuilder sb)
    {
        if (points.Length == 0)
            return false;
        else
        {
            sb.Append($"points = {{ {points[0].ToString()}");
            foreach (Point p in points[1..points.Length])
                sb.Append(", " + p.ToString());
            sb.Append(" }");
            return true;
        }
    }
}
```

> 继承

- 一条记录可以从另一条记录继承。派生记录为基本记录主构造函数中的所有参数声明位置参数，基本记录声明并初始化这些属性；派生记录不会隐藏它们，而只会创建和初始化未在其基本记录中声明的参数的属性。
- 要使两个记录变量相等，运行时类型必须相等。包含变量的类型可能不同，但相等性测试依赖于实际对象的运行时类型，而不是声明的变量类型。
- `with` 表达式结果的运行时间类型与表达式操作数相同：运行时类型的所有属性都会被复制，但用户只能设置编译时类型的属性。
- 派生记录类型的合成 `PrintMembers` 方法并调用基实现 `base.PrintMembers()`。结果是派生类型和基类型的所有公共属性和字段都包含在 `ToString` 输出中。派生记录也会重新合成基记录的 `EqualityContract`、`GetHashCode`、`Deconstruct` 方法。 

```csharp
class Sample
{
    public abstract record Person(string FirstName, string LastName);
    public record Teacher(string FirstName, string LastName, int Grade)
        : Person(FirstName, LastName);
    public record Student(string FirstName, string LastName, int Grade)
        : Person(FirstName, LastName);

    public static void Main()
    {
        Person teacher = new Teacher("Nancy", "Davolio", 3);
        Console.WriteLine(teacher);
        // output: Teacher { FirstName = Nancy, LastName = Davolio, Grade = 3 }

        /* 相等性测试 */
        Person student = new Student("Nancy", "Davolio", 3);
        Console.WriteLine(teacher == student); // output: False
        Student student2 = new Student("Nancy", "Davolio", 3);
        Console.WriteLine(student2 == student); // output: True

        /* with 表达式 */
        Person clone_teacher = teacher with { FirstName = "Tom" }; // 无法定义 Grade，虽然在运行时类型包含此属性
        Teacher teacher2 = (Teacher)teacher with { Grade = 6 };
        Console.WriteLine(teacher2);
        // output: Teacher { FirstName = Nancy, LastName = Davolio, Grade = 6 }

        /* 解构函数 */
        var (first, second) = (Teacher)teacher;  // 支持基记录的解构函数
        var (first2, second2, grade) = (Teacher)teacher; // 在 Teacher 重新生成的解构函数
    }
}
```

<br>

#### class 类类型

- 使用 `class` 关键字声明类。在 C# 中仅允许单一继承，一个类仅能从一个基类继承实现，但是一个类可以实现多个接口。可以声明具有类型参数的泛型类。
- 一个类可包含的成员有：构造函数、常量、字段、终结器、方法、属性、索引器、运算符、事件、委托、类、记录、接口、结构类型、枚举类型。

```csharp
class MyClass
{
    // 构造函数
    public MyClass() { S_OnDestroy = static delegate { Console.WriteLine("MyClass Destroyed"); }; }
    // 常量
    public const string Version = "0.0.1";
    // 枚举
    public enum Day { workDay, weekDay }
    // 字段
    private readonly static MyClass s_default = new MyClass();
    // 静态属性
    public static MyClass Default => s_default;
    // 实例方法
    public Data CreateData(byte[] data, int id) => new Data(data, id);
    // 索引器
    public int this[int index] => index;
    // 运算符
    public static bool operator ==(MyClass left, MyClass right) => left.Equals(right);
    public static bool operator !=(MyClass left, MyClass right) => !left.Equals(right);
    // 委托
    delegate void OnDestroy();
    // 事件
    event OnDestroy S_OnDestroy;
    // 终结器
    ~MyClass() => S_OnDestroy?.Invoke();
    // 嵌套接口
    public interface INested { }
    // 嵌套类
    sealed class NestedClass : INested { }
    // 嵌套结构
    struct NestedStruct : INested { }
    // 记录
    public readonly record struct Data(byte[] data, int gui);
}

class Sample
{
    static void Main(string[] args)
    {
        MyClass.Data d1 = MyClass.Default.CreateData("Hello"u8.ToArray(), "Hello".GetHashCode());
    }
}
```

> 抽象类与密封类

- 使用 `abstract` 可以创建不完整且必须在派生类中实现的类和类成员（属性、方法、索引器等）。抽象类不可实例化，一般用于提供一个可供多个派生类共享的通用基类定义。在抽象类中定义抽象方法，目的是将方法的具体实现（`override`）延迟到派生类。抽象方法是没有实现的特殊的虚方法，在基类中提供 `virtual` 虚方法并提供默认的方法实现，派生类可以重写（`override`）虚方法以扩展或重新定义派生类的行为。

```csharp
interface ISample       // 接口
{
    void Debug(string message);
}
abstract class BaseSample : ISample     // 继承接口并定义抽象方法
{
    public abstract void Debug(string mess);
    public virtual void Error(string mess) => Debug(mess);
}
internal class Sample : BaseSample      // 为抽象类和接口提供方法实现
{
    public override void Debug(string mess) => Console.WriteLine(mess);
    public sealed override void Error(string mess) => base.Error("ERROR : " + mess);
}
```

- 使用 `sealed` 定义密封类或密封成员。密封类防止被继承，密封成员防止被派生类重写。

```csharp
using System.Text;

PointArray ps = new((0, 0), (1, 1), (2, 2));
Console.WriteLine(ps);
// Output : (0, 0), (1, 1), (2, 2)

// 定义抽象记录防止被继承
sealed record PointArray(params (int, int)[] points)
{
    // 重写 ToString 方法
    public override string ToString()
    {
        if (points.Length > 0)
        {
            StringBuilder sb = new StringBuilder(points[0].ToString());
            foreach (var p in points[1..])
                sb.Append(", " + p);
            return sb.ToString();
        }
        else return string.Empty;
    }
}
```

- 使用 `new` 修饰符显式隐藏从基类继承的成员。

```csharp
public class BaseC
{
    public int x;
    public void Invoke() { }
}
public class DerivedC : BaseC
{
    new public void Invoke() { }
}
```

> 静态类和静态类成员

- 静态类基本上与非静态类相同，但是静态类无法实例化，因此静态类仅能声明静态成员。加载引用静态类的程序时，.NET 运行时会加载该静态类的类型信息，并在程序中首次引用类之前初始化其字段并调用其静态构造函数。静态构造函数只调用一次，在程序所驻留的应用程序域的生存期内，静态类会保留在内存中。
- 静态类只能从 `object` 继承，无法创建任何实例成员，也不能被任何类继承。静态类中可以声明静态扩展方法。

```csharp
public static class EnumExt
{   
    public static T? ConvertEnum<T>(this int eVal) where T : Enum
        => (T)Enum.ToObject(typeof(T), eVal);  // int 转换为 enum
    public static bool IsDefinedByEnum<T>(this int eVal) where T : Enum
        => Enum.IsDefined(typeof(T), eVal);    // 检查 enum 是否关联整数值
}
```


<br>

#### interface 接口

- `interface` 关键字定义接口类型。接口包含非抽象 `record`、`class` 或 `struct` 必须实现的一组相关功能的定义，类或结构可以实现多个接口。
- 接口可为成员定义默认实现，还可以定义 `static` 成员，以便提供常见功能的单个实现，在类型中声明的同名静态成员不会覆盖接口中声明的静态成员。从 C#11 开始，非字段接口成员可以是 `static abstract`。
- 接口可以包含实例方法、属性、事件、索引器，也可以包含静态构造函数、静态字段、常量或运算符。接口成员默认是公共的，可以显式指定可访问性修饰符，其中 `private` 成员必须有默认实现。
- 类的属性和索引器可以为接口中定义的属性或索引器定义额外的访问器。若接口属性或索引器使用显式接口实现而不是派生类型实现时，访问器必须匹配。

```csharp
interface IMyInterface<T> where T : IMyInterface<T>
{
    // 接口静态构造
    static IMyInterface() => Console.WriteLine("Static IMyInterface()");
    // 静态成员
    static void Func() => Console.WriteLine("Static IMyInterface.Func");
    const int Zero = 0;
    abstract static void AbsFunc();
    // 实例方法
    void Action();
    // 实例方法：默认实现
    virtual void S_Action() => Console.WriteLine("IMyInterface.S_Action");
    // 属性或索引器
    int GUI { get; }
    // 运算符 static abstract/virtual 运算符
    static virtual bool operator ==(T lhs, T rhs) => lhs.Equals(rhs);
    static virtual bool operator !=(T lhs, T rhs) => !lhs.Equals(rhs);
    static abstract T operator ++(T other);
    // 
}
class MyClass : IMyInterface<MyClass>
{
    public int GUI => GetHashCode();
    int counter = 0;
    public static void AbsFunc() { }    // 实现接口静态抽象成员

    public void Action() => Console.WriteLine("MyClass.Action");

    public static MyClass operator ++(MyClass other)
    {
        other.counter++;
        return other;
    }
}
```

- 当接口声明方法的默认实现时，实现该接口的任何类都会继承该实现，首先需要将派生实例转换为接口类型，才可以访问接口成员上的默认实现。派生类型可以重新实现任何虚拟接口成员（显式接口实现或派生类型实现均可），而不使用接口的默认实现，此时转换为接口类型时将调用派生类型重新实现的成员。

```csharp
Sample s = new Sample();
s.Func1();           // Sample.Func1
((IFace)s).Func2();  // IFace.Func2

IFace sf = s;
sf.Func1();          // Sample.Func1
sf.Func2();          // IFace.Func2

interface IFace{
    void Func1() => Console.WriteLine("IFace.Func1");
    void Func2() => Console.WriteLine("IFace.Func2");
}
class Sample : IFace{
    public void Func1() => Console.WriteLine("Sample.Func1");
}
```

> 显式接口实现

- 若多个接口具有相同的方法成员时，若要调用不同的实现，根据所使用的接口，可以显式实现接口成员。显式接口实现没有访问修饰符，因为它不能作为其定义类型的成员进行访问，只能通过对应的接口实例调用。

```csharp
Sample logger = new Sample();
string mess = "Hello World";
logger.Print(mess);
((IDebug)logger).Print(mess);
((IError)logger).Print(mess);
/* Output
 Hello World
 DEBUG : Hello World
 ERROR : Hello World
*/

interface IDebug{
    void Print(string mess);
}
interface IError{
    void Print(string mess);
}
class Sample : IDebug, IError
{
    // 显式接口实现
    void IDebug.Print(string mess) => Console.WriteLine("DEBUG : " + mess);
    void IError.Print(string mess) => Console.WriteLine("ERROR : " + mess);
    // 默认实现
    public void Print(string mess) => Console.WriteLine(mess);
}
```

> 静态抽象和虚拟成员

- 从 C#11 开始，接口可以声明除字段之外的所有成员类型的 `static abstract` 和 `static virtual` 成员。接口指定抽象静态成员，然后要求类和结构为接口抽象静态成员提供显式或隐式实现。接口静态抽象或虚拟成员只能从受接口约束的类型参数或派生实现类型中访问。
- 接口中声明的 `static virtual` 和 `static abstract` 方法没有类似于类中声明的 `virtual` 或 `abstract` 方法的运行时调度机制。相反，编译器使用编译时可用的类型信息，即调用基（编译时）类型的静态方法。`static virtual` 方法几乎完全是在泛型接口中声明的。

```csharp
interface IFace
{
    static abstract void Func();
    static abstract event Action E;
    static abstract object Proper { get; set; }
}
interface IFace<T> where T : IFace<T>
{
    static abstract void Func();
    static abstract event Action E;
    static abstract T P { get; set; }
    static abstract object Proper { get; set; }
    static abstract T operator +(T l, T r);
    static abstract bool operator ==(T l, T r);
    static abstract bool operator !=(T l, T r);
    static abstract implicit operator T(string s);
    static abstract explicit operator string(T t);
}
```

- 访问接口静态抽象成员

```csharp
Console.WriteLine(Sample.Instance);
Sample.Func();

interface IInstance<T> where T : IInstance<T>
{
    static abstract void Func();
    static abstract T Instance { get; }
}
class Sample : IInstance<Sample>
{
    public static Sample Instance { get; } = new Sample();
    public static void Func() => Console.WriteLine("Sample.Func");
}
```

<br>

#### 可为 null 的引用类型

- 由于在可为 null 的感知上下文选择加入了代码，可以使用可为 null 的引用类型.可为 null 的引用类型、null 静态分析警告和 null 包容运算符是可选的语言功能。在可为 null 的感知上下文中：
  - 引用类型 `T` 的变量必须用非 `null` 值进行初始化，并且不能为其分配可能为 `null` 的值。
  - 引用类型 `T?` 的变量可以用 `null` 进行初始化，也可以分配 `null`，但在取消引用之前必须对照 `null` 进行检查。
  - 类型为 `T?` 的变量 `m` 在应用 `null` 包容运算符时被认为是非空的，如 `m!` 中所示。

- 类型为 `T` 的变量和类型为 `T?` 的变量由相同的 .NET 类型表示。可为 null 的引用类型不是新的类类型，而是对现有引用类型的注释。编译器使用这些注释来帮助你查找代码中潜在的 null 引用错误。不可为 null 的引用类型和可为 null 的引用类型在运行时没有区别。
- 可以通过两种方式控制可为 null 的上下文。在项目级别，可以添加 `<Nullable>enable</Nullable>` 项目设置。在单个 C# 源文件中，可以添加 `#nullable enable` 来启用可为 null 的上下文。在 .NET 6 之前，新项目使用默认值 `<Nullable>disable</Nullable>`。从 .NET 6 开始，新项目将在项目文件中包含 `<Nullable>enable</Nullable>` 元素。

---
### 匿名类型

- 匿名类型提供了一种方便的方法，可用来将一组只读属性封装到单个对象中，而无需首先显式定义一个类型，每个属性的类型由编译器推断。类型名由编译器生成，并且不能在源代码级使用，可结合使用 `new` 运算符和对象初始值设定项创建匿名类型。
- 匿名类型包含一个或多个公共只读属性。无法包含其他种类的类成员（如方法或事件）。用来初始化属性的表达式不能为 null、匿名函数或指针类型。

```csharp
var v = new { Amount = 108, Message = "Hello" };
Console.WriteLine(v.Amount + v.Message);
```

- 匿名类型是 `class` 类型，它们直接派生自 `object`，并且无法强制转换为除 `object` 外的任何类型。如果程序集中的两个或多个匿名对象初始值指定了属性序列，这些属性采用相同顺序且具有相同的名称和类型，则编译器将对象视为相同类型的实例，它们共享同一编译器生成的类型信息。
- 无法将字段、属性、时间或方法的返回类型声明为具有匿名类型。同样，也不能将方法、属性、构造函数或索引器的形参声明为具有匿名类型。要将匿名类型或包含匿名类型的集合作为参数传递给某一方法，可将参数作为类型 `object` 进行声明。

> 应用

- 匿名类型通常用在查询表达式的 `select` 子句中，以便返回源序列中每个对象的属性子集。

```csharp
var productQuery =
    from prod in products
    select new { prod.Color, prod.Price };

foreach (var v in productQuery)
    Console.WriteLine("Color={0}, Price={1}", v.Color, v.Price);
```

- 还可以按另一种类型（类、结构或另一个匿名类型）的对象定义字段。它通过使用保存此对象的变量来完成。

```csharp
var product = new Product();
var bonus = new { note = "You won!" };
var shipment = new { address = "Nowhere St.", product };
var shipmentWithBonus = new { address = "Somewhere St.", product, bonus };
```

- 可通过将隐式键入的本地变量与隐式键入的数组相结合创建匿名键入的元素的数组。

```csharp
var anonArray = new[] { new { name = "apple", diam = 4 }, new { name = "grape", diam = 1 }};
```

- 匿名类型支持采用 `with` 表达式形式的非破坏性修改。

```csharp
var apple = new { Item = "apples", Price = 1.35 };
var onSale = apple with { Price = 0.79 };
Console.WriteLine(apple);
Console.WriteLine(onSale);
```

---
### 隐式类型

- 声明局部变量时，可以让编译器从初始化表达式推断出变量的类型。使用 `var` 关键字声明隐式类型，隐式类型只能应用于本地方法范围内的变量。`var` 的常见用途是用于构造函数调用表达式，例如 `var xs = new List<int>();`。

```csharp
var Ps = new PointArray(PointArray.RandomPoints(50));
Ps.AddPoints(PointArray.RandomPoints(50));
var first_Ps = Ps.GetPointsInQuadrant(1);
var Second_Ps = Ps.GetPointsInQuadrant(2);
var Third_Ps = Ps.GetPointsInQuadrant(3);
var Forth_Ps = Ps.GetPointsInQuadrant(4);

Print(first_Ps);
Print(Second_Ps);
Print(Third_Ps);
Print(Forth_Ps);
// -----------------------------------------------
static void Print<T>(in IEnumerable<T> arr)
{
    foreach (var item in arr)
        Console.WriteLine(item.ToString());
}

record struct PointArray(params (int x, int y)[] points)
{
    public readonly int PointsCount => points.Length;
    private bool PrintMembers(System.Text.StringBuilder sb)
    {
        if (points.Length > 0)
        {
            sb.Append(points[0]);
            foreach (var p in points[1..])
                sb.Append(" ," + p);
            return true;
        }
        return false;
    }
    public static (int, int)[] RandomPoints(int count)
    {
        int seed = DateTime.Now.Microsecond;
        Random r = new Random(seed);
        var ps = new (int, int)[count];
        for (int i = 0; i < count; i++)
            ps[i] = (r.Next(-128, 128), r.Next(-128, 128));
        return ps;
    }

    public void AddPoints(params (int x, int y)[] points)
    {
        (int x, int y)[] newPoints = new (int x, int y)[points.Length + this.points.Length];
        Array.Copy(this.points, newPoints, this.points.Length);
        Array.Copy(points, 0, newPoints, this.points.Length, points.Length);
        this.points = newPoints;
    }
    public (int, int)[] GetPointsInQuadrant(uint order)
    {
        if (order < 0 || order > 4)
            return default;
        var state = static delegate (int x, int y, uint order)
        {
            return order switch
            {
                1 => x > 0 && y > 0,
                2 => x > 0 && y < 0,
                3 => x < 0 && y < 0,
                4 => x < 0 && y > 0,
            };
        };
        var ps = from (int x, int y) p in this.points
                 where state(p.x, p.y, order)
                 select p;
        return ps.ToArray();
    }
}
```

---
### 指针类型

#### 不安全上下文

- C# 支持 `unsafe` 上下文，用户可在其中编写不可验证的代码。在 `unsafe` 上下文中，代码可使用指针、分配和释放内存块，以及使用函数指针调用方法。可以将方法、类型和代码块定义为不安全。调用需要指针的本机函数时，需使用不安全代码，因此可能会引发安全风险和稳定性风险。在某些情况下，通过移除数组绑定检查，不安全代码可提高应用程序的性能。
- 指针不能指向引用（`ref`）或包含引用的结构，因为无法对对象引用进行垃圾回收，即使有指针指向它也是如此。垃圾回收器并不跟踪是否有任何类型的指针指向对象。

```csharp
int* p;         // p 是指向整数的指针。
int** p;        // p 是指向整数的指针的指针。
int*[] p;       // p 是指向整数的指针的一维数组。
char* p;        // p 是指向字符的指针。
void* p;        // p 是指向未知类型的指针。

int* p1, p2, p3;    // Ok
int *p1, *p2, *p3;  // Invalid in C#
```

- 无法对 `void*` 类型的指针应用间接寻址运算符，但是可以使用强制转换将 `void` 指针转换为任何其他指针类型，反之亦然。
- 指针可以为 null。将间接寻址运算符应用于 null 指针将导致空引用异常。
- 在方法之间传递指针可能会导致未定义的行为。

```csharp
unsafe
{
    fixed (void* PEmptyString = &string.Empty)
        Console.WriteLine(Convert.ToString((long)(nuint)PEmptyString, 16));  // 输出指针地址值

    int* p = null;
    int a = *p;   // ERROR: System.NullReferenceException: “Object reference not set to an instance of an object.”
}
```

> 获取对象的地址

```csharp
int[] arr = [10, 20, 30, 40, 50];

unsafe
{
    // 必须将对象固定在堆上，这样它在使用时，垃圾回收器不会移动它
    fixed (int* p = arr) // 或 &arr[0]. &arr[index]
    {
        // 固定指针无法移动, 无法赋值
        //  p++;  // CS1656
        // 所以创建另一个指针来显示它的递增。
        int* p2 = p;
        Console.WriteLine(*p2);  // 10
        // 由于指针的类型，增加 p2 会使指针增加其基础类型大小的字节：4
        p2 += 1;
        Console.WriteLine(*p2);  // 20
        p2 += 1;
        Console.WriteLine(*p2);  // 30

        Console.WriteLine("--------");
        // 对 p 解引用并递增会改变 arr[0] 的值
        Console.WriteLine(*p);   // 10
        *p += 1;
        Console.WriteLine(*p);   // 11
        *p += 1;
        Console.WriteLine(*p);   // 12
    }
    Console.WriteLine(arr[0]);  // 12
}
```

<br>

#### 指针相关的运算符和语句

- `*`：执行指针间接寻址。
- `->`：通过指针访问结构或类对象的成员。
- `[]`：为指针建立索引。
- `&`：获取变量的地址。
- `++` 和 `--`：递增和递减指针。
- `+` 和 `-`：执行指针算法。
- `==`、`!=`、`<`、`>`、`<=` 和 `>=`：比较指针。
- `stackalloc`：在堆栈上分配内存。
- `fixed` 语句：临时固定变量以便找到其地址。

<br>

#### 固定大小的缓冲区

- 可以使用 `fixed` 关键字来创建在数据结构中具有固定大小的数组的缓冲区。当编写与其他语言或平台的数据源进行互操作的方法时，固定大小的缓冲区很有用。固定大小的缓冲区可以采用允许用于常规结构成员的任何属性或修饰符。唯一的限制是数组类型必须为 `bool`、`byte`、`char`、`short`、`int`、`long`、`sbyte`、`ushort`、`uint`、`ulong`、`float` 或 `double`。
  
```csharp
internal unsafe struct Buffer
{
    public fixed char fixedBuffer[128];
}
```

- 在安全代码中，包含数组的 C# 结构不包含该数组的元素，而是包含对该数组的引用。当在不安全的代码块中使用数组时，可以在结构中嵌入固定大小的数组。使用 `fixed` 语句获取指向数组第一个元素的指针，通过此指针访问数组的元素。`fixed` 语句将 `fixedBuffer` 实例字段固定到内存中的特定位置。

```csharp
internal unsafe struct Buffer
{
    public fixed char fixedBuffer[128];
}
internal unsafe class Example
{
    public Buffer buffer = default;
}
private static void AccessEmbeddedArray()
{
    var example = new Example();
    unsafe
    {
        // Pin the buffer to a fixed location in memory.
        fixed (char* charPtr = example.buffer.fixedBuffer)
        {
            *charPtr = 'A';
        }
        // Access safely through the index:
        char c = example.buffer.fixedBuffer[0];
        Console.WriteLine(c);

        // Modify through the index:
        example.buffer.fixedBuffer[0] = 'B';
        Console.WriteLine(example.buffer.fixedBuffer[0]);
    }
}
```

- 固定大小的缓冲区使用 `System.Runtime.CompilerServices.UnsafeValueTypeAttribute` 进行编译，它指示公共语言运行时 CLR 某个类型包含可能溢出的非托管数组。

```csharp
internal unsafe struct Buffer
{
    public fixed char fixedBuffer[128];
}
// 为 Buffer 生成 C# 的编译器的特性如下
internal struct Buffer
{
    [StructLayout(LayoutKind.Sequential, Size = 256)]
    [CompilerGenerated]
    [UnsafeValueType]
    public struct <fixedBuffer>e__FixedBuffer
    {
        public char FixedElementField;
    }

    [FixedBuffer(typeof(char), 128)]
    public <fixedBuffer>e__FixedBuffer fixedBuffer;
}
```

- 使用 `stackalloc` 分配的内存还会在 CLR 中自动启用缓冲区溢出检测功能

```csharp
unsafe
{
    int* pSafe = stackalloc int[10];
    for (int i = 0; i < 100; i++)
        *(pSafe + i) = i;
    // 进行缓冲区溢出检查，溢出时引发异常 System.AccessViolationException

    Example ex = new Example();
    fixed (int* pUnsafe = ex.buffer.fixedBuffer)
    {
        for (int i = 0; i < 100; i++)
            *(pUnsafe + i) = i;   // 不进行缓冲区溢出检查
    }
}
internal unsafe struct Buffer
{
    public fixed int fixedBuffer[10];
}
internal unsafe class Example
{
    public Buffer buffer = default;
}
```

<br> 

#### 函数指针

- C# 提供 `delegate` 类型来定义安全函数指针对象。 调用委托时，需要实例化从 `System.Delegate` 派生的类型并对其 `Invoke` 方法进行虚拟方法调用，该虚拟调用使用 IL 指令 `callvirt`
- 可以使用 `delegate*` 语法定义函数指针。编译器将使用 IL 指令 `calli` 指令来调用函数，而不是实例化为委托对象并调用 `Invoke`。在性能关键的代码路径中，使用 IL 指令 `calli` 效率更高。

```csharp
// 委托定义参数
public static T Combine<T>(Func<T, T, T> combinator, T left, T right) => combinator(left, right);
// 函数指针定义参数
public static T UnsafeCombine<T>(delegate*<T, T, T> combinator, T left, T right) => combinator(left, right);
```

- 函数指针只能在 `unsafe` 上下文中声明，只能在静态成员方法或静态本地方法使用地址运算符 `&`。

```csharp
unsafe
{
    // 函数指针声明和调用
    delegate*<int, int> pAbs = &Abs;
    Console.WriteLine(pAbs(-999));  // 999
    // 本地静态方法
    static int Abs(int val) => Math.Abs(val);
}
```

> 函数指针声明语法

```csharp
delegate * calling_convention_specifier? <parameter_list, return_type> 

calling_convention_specifier? : 可选的调用约定说明符, 默认为 managed
    - managed : 默认调用约定
    - unmanaged : 非托管调用约定, 未显式指定调用约定类别, 则使用运行时平台默认语法
    - unmanaged [Calling_convertion|,Calling_convertion...] : 指定特定的非托管调用约定, 一到若干个
        - Calling_convertion : 调用约定
                - Cdecl : 调用方清理堆栈
                - stdcall : 被调用方清理堆栈, 这是从托管代码调用非托管函数的默认约定
                - Thiscall : 指定方法调用的第一个参数是 this 指针, 该指针存储在寄存器 ECX 中
                - Fastcall : 调用约定指定在寄存器中传递函数的参数 (如果可能), NET 可能不支持 
                - MemberFunction : 指示使用的调用约定是成员函数变体
                - SuppressGCTransition : 指示方法应禁止 GC 转换作为调用约定的一部分
```

- 可以对函数指针显式使用调用约定说明符 `unmanaged`、`managed`，默认使用 `managed` 调用约定（使用托管方法）。
- 使用 `unmanaged` 调用约定时，可以显式指定一个或多个 ECMA-335 调用约定（`Cdecl`、`Stdcall`、`Fastcall`、`Thiscall`）或 `MemberFunction`、`SuppressGCTransition`。未显式指定的 `unmanaged` 调用约定，则指示 CLR 选择平台的默认调用约定（在运行时基于平台选择调用约定）。
- 函数调用约定，是指当一个函数被调用时，函数的参数会被传递给被调用的函数，返回值会被返回给调用函数。函数的调用约定就是描述参数是怎么传递和由谁平衡堆栈的，当然还有返回值。

```csharp
unsafe class Sample
{
    // 委托
    public static T Combine<T>(Func<T, T, T> combinator, T left, T right) => combinator?.Invoke(left, right);
    // 函数指针
    public static T UnsafeCombine<T>(delegate*<T, T, T> combinator, T left, T right) => combinator(left, right);

    public static T ManagedCombine<T>(delegate* managed<T, T, T> combinator, T left, T right) => combinator(left, right);

    public static T CDeclCombine<T>(delegate* unmanaged[Cdecl]<T, T, T> combinator, T left, T right) => combinator(left, right);
    
    public static T StdcallCombine<T>(delegate* unmanaged[Stdcall]<T, T, T> combinator, T left, T right) => combinator(left, right);
    
    public static T FastcallCombine<T>(delegate* unmanaged[Fastcall]<T, T, T> combinator, T left, T right) => combinator(left, right);
    
    public static T ThiscallCombine<T>(delegate* unmanaged[Thiscall]<T, T, T> combinator, T left, T right) => combinator(left, right);
    
    public static T UnmanagedCombine<T>(delegate* unmanaged<T, T, T> combinator, T left, T right) => combinator(left, right);
}
```

---
### 类型默认值

- 任何引用类型：`null`。
- 任何内置数值类型：`0`。
- `bool`：`false`。
- `char`：`\0`。
- `enum`：`(E)0`。
- `struct`：成员各类型默认值。
- 可为 null 的值类型：`HasValue` 属性为 `false` 且 `Value` 属性未定义的实例，即 `null`。

> 默认值表达式

- 默认值表达式生成类型的默认值。有两种类型的表达式：`default` 运算符调用和 `default` 文本：
  - `default` 运算符的实参必须是类型或类型形参的名称。
  - `default` 文本用于生成类型的默认值，可用于变量赋值、可选方法参数的默认值、`return` 语句、方法参数传递。

```csharp
int num = default(int);         // default 运算符
string str = default;           // default 文本值
```

---

### 泛型类型

- 借助泛型，可以根据要处理的精确数据类型设计方法、委托、类、结构或接口，以提高代码的可重用性和类型安全性。泛型是为所存储或使用的一个或多个类型具有占位符（类型形参）的类、结构、接口、方法和委托。例如泛型集合类可以将类型形参用作其存储的对象类型的占位符，泛型方法可将其类型形参用作其返回值的类型或用作其形参之一的类型。

```csharp
abstract class GenericSample
{
    // 泛型方法
    public abstract T GetGenericValue<T>();
    // 泛型类
    class GenericClass<T>;
    // 泛型结构
    struct GenericStruct<T>;
    // 泛型接口
    interface IGenericInterface<T>;
    // 泛型委托
    delegate void GenericDelegate<T>();
    // 泛型记录
    record GenericRecordClass<T>;
    record struct GenericRecordStruct<T>;
}
```

- 创建泛型类型的实例时或使用泛型方法时，需要指定用于替代类型形参的实际类型。在类型形参出现的每一处位置用选定的类型进行替代，这会创建一个新的构造泛型类型或方法。

```csharp
Generic<string> g_string = new Generic<string>();
g_string.Field = "A string";
Console.WriteLine("Generic.Field           = \"{0}\"", g_string.Field);
Console.WriteLine("g_string.GetType() = {0}", g_string.GetType().FullName);
/*
Generic.Field           = "A string"
g_string.GetType() = Generic`1[[System.String, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]
*/

record struct Generic<T>(string Field);   // 泛型记录
```

<br>

#### 泛型约束

- 约束告知编译器类型参数必须具备的功能。在没有任何约束的情况下，类型参数可以是任何类型。可以在泛型的定义中使用 `where` 子句指定对类型参数的参数类型约束。
- 约束指定类型参数的功能和预期，声明这些约束意味着可以使用约束类型的操作和方法调用。

```csharp
interface ISample<T> where T : class;
// 指定类型参数必须是引用类型
```

- 可以对类型参数指定的约束有：
  - `struct`、`class`、`unmanaged`、`notnull`、`default` 约束不能组合或重复，且必须先在约束列表中指定。
  - `new()` 不能和 `unmanaged` 和 `struct` 一起使用，且只能是约束列表中的最后一个。

```csharp
interface ISample1<T> where T : struct;     // 不可为 null 的值类型
interface ISample2<T> where T : class;      // 不可为 null 的引用类型
interface ISample3<T> where T : class?;     // 可为 null 的引用类型
interface ISample4<T> where T : notnull;    // 不可为 null 的类型
interface ISample5<T> where T : unmanaged;  // 不可为 null 的非托管类型
interface ISample6<T> where T : new();      // 具有公共无参构造函数的类型
interface ISample7<T> where T : Base;       // 指定的基类或其派生类型
interface ISample8<T> where T : Base?;      // 可为 null 的指定基类或其派生类型
interface ISample9<T> where T : IBase;      // 指定的接口或实现接口的类型
interface ISample10<T> where T : IBase?;    // 可为 null 的指定接口或实现接口的类型
interface ISample11<T, U> where T : U;      // 指定 T 是 U 或 U 的派生类型

class Base;
interface IBase;
```

<br>

#### 不受约束的类型参数批注 `?` 和 default 约束

- 在 C# 8 中，`?` 批注仅适用于显式约束为值类型或引用类型的类型参数。在 C#9 中，`?` 批注可应用于任何类型参数，而不考虑约束。

```csharp
static T? FirstOrDefault<T>(this IEnumerable<T> collection) { ... };   // 不受约束的类型参数批注
```

- 如果类型参数 `T` 替换为引用类型，则 `T?` 表示该引用类型的可以为 null 的实例。

```csharp
var s1 = new string[0].FirstOrDefault();  // string? s1
var s2 = new string?[0].FirstOrDefault(); // string? s2
```

- 如果 `T` 用值类型替换，则 `T?` 表示的实例 `T`。 

```csharp
var i1 = new int[0].FirstOrDefault();   // int i1
var i2 = new int?[0].FirstOrDefault();  // int? i2
```

- 如果 `T` 使用批注类型替换 `U?`，则 `T?` 表示批注的类型 `U?` 而不是 `U??`。如果 `T` 将替换为类型 `U`，则 `T?` 表示 `U?`，即使在上下文中也是如此 `#nullable disable`。 

```csharp
var u1 = new U[0].FirstOrDefault();  // U? u1
var u2 = new U?[0].FirstOrDefault(); // U? u2
#nullable disable
var u3 = new U[0].FirstOrDefault();  // U? u3
```

> default 约束

- 为了与现有代码兼容，重写和显式接口实现的泛型方法不能包含显式约束子句，而 `T?` 在重写或显式接口实现的方法中被视为 `Nullable<T>`，其中 `T` 是值类型。
  
```csharp
class Base
{
    public virtual void Func<T>(T? t) { }
}
interface ISample
{
    void Func<T>(T? t); // T? 被认为是 Nullable<T>
}
class Derived : Base, ISample
{
    // 找不到合适的方法重写
    public override void Func<T>(T? t) { } // CS0453
    void ISample.Func<T>(T? t) { }   // CS0453
}
```  
  
- 为了允许对约束为引用类型的类型参数进行注释 `?`，C#8 允许在泛型方法上显式地约束 `where T: class` 和 `where T: struct`。

```csharp
class Base
{
    public virtual void Func<T>(T? t) where T : struct { }
    public virtual void Func<T>(T? t) where T : class { }
}
interface ISample
{
    void IFunc<T>(T? t) where T : struct; // T? 被认为是 Nullable<T>
}
class Derived : Base, ISample
{
    public override void Func<T>(T? t) /* where T: struct */{ }  // 重写 struct 约束方法
    public override void Func<T>(T? t) where T : class { }
    void ISample.IFunc<T>(T? t) { }
}
```

- 为了允许对不受引用类型或值类型约束的类型参数进行注释，C#9 允许一个新的 `where T: default` 约束。重写或显式接口实现的方法使用 `default` 约束以外的约束是错误的。

```csharp
class Base
{
    public virtual void Func<T>(T? t) where T : struct { }
    public virtual void Func<T>(T? t) { }
}
interface ISample
{
    void IFunc<T>(T? t);
}
class Derived : Base, ISample
{
    public override void Func<T>(T? t) /* where T: struct */{ }
    public override void Func<T>(T? t) where T : default { }
    void ISample.IFunc<T>(T? t) where T : default { }
}
```

- 当重写方法或接口方法中的相应类型参数被约束为引用类型或值类型时，使用 `default` 约束是错误的。

```csharp
class Base
{
    public virtual void Func<T>(T? t) where T : struct { }
    public virtual void Func<T>(T? t) where T : class { }
}
class Derived : Base
{
    public override void Func<T>(T? t) /* where T: struct */{ }
    //public override void Func<T>(T? t) where T : class{ }
    public override void Func<T>(T? t) where T : default { } // CS8822
}
```

<br>

#### 泛型类型中的静态成员

- 使用泛型类型时指定类型参数时，运行时将创建该类型参数的构造泛型类型。从同一泛型类型的构建的不同构造泛型类型之间，各构造泛型类型的静态成员（包括静态构造函数、字段、方法、属性等）独立存在。在首次调用该类型时，会首先调用它的静态构造函数。对于泛型接口类型的不能构造类型之间，静态成员（非抽象）也是相互独立的。

```csharp
var I1 = ISample<int>.Default;
Console.WriteLine("-------------");
var I2 = ISample<string>.Default;
Console.WriteLine("-------------");
var s1 = Sample<float>.Default;
Console.WriteLine("-------------");
var s2 = Sample<object>.Default;

/*
Static ISample() >> Int32
-------------
Static ISample() >> String
-------------
Static Sample() >> Single
-------------
Static Sample() >> Object
*/

interface ISample<T> 
{
    static ISample() =>Console.WriteLine($"Static ISample() >> {typeof(T).Name}");
    public static T? Default { get; set; } = default;
}
class Sample<T>
{
    static Sample() => Console.WriteLine($"Static Sample() >> {typeof(T).Name}");
    public static T? Default { get; set; } = default;
}
```

<br>

#### 协变与逆变

- 借助泛型类型参数的协变和逆变，可以使用类型自变量的派生程度比目标构造类型更高（协变）或更低（逆变）的构造泛型类型。协变和逆变统称为 “变体”，未标记为协变或逆变的泛型类型参数称为 “固定参数” 。协变和逆变类型参数仅限于泛型接口和泛型委托类型，变体仅适用于与引用类型。当类型参数指定为值类型时，该类型参数对于生成的构造类型是不可变的。
- 使用 `in` 关键字指定类型参数是逆变的，逆变的类型参数可以用作泛型接口的方法或泛型委托的参数类型。`out` 关键字指定类型参数是协变的，协变的类型参数可用作接口方法的返回类型。

```csharp
delegate TResult GenericDelegate<in T, out TResult>(T arg);
interface IGeneric<in T, out TResult>
{
    TResult GetResult(T arg);
}
```

- 协变和逆变能够实现委托类型、泛型接口类型和泛型类型参数的隐式引用转换。

```csharp
class VariantSample
{
    delegate A DCovariant<out A>();             // 协变泛型委托
    delegate void DContravariant<in A>(A a);    // 逆变泛型委托

    interface ICovariant<out A> { }         // 协变泛型接口
    interface IContravariant<in A> { }      // 逆变泛型接口

    class Base;
    class Derived : Base;
    class Sample<T> : ICovariant<T>, IContravariant<T>;
    static T SampleFunc<T>() => default;
    static void SampleFunc<T>(T t) { }

    static void Main(string[] args)
    {
        // 泛型接口中的协变
        ICovariant<Base> I_B = new Sample<Base>();
        ICovariant<Derived> I_D = new Sample<Derived>();
        I_B = I_D;  // 协变

        // 泛型接口中的逆变
        IContravariant<Base> I_B2 = new Sample<Base>();
        IContravariant<Derived> I_D2 = new Sample<Derived>();
        I_D2 = I_B2;  // 逆变

        // 泛型委托中的协变
        DCovariant<Base> D_B = SampleFunc<Base>;
        DCovariant<Derived> D_D = SampleFunc<Derived>;
        D_B = D_D;  // 协变

        // 泛型委托中的逆变
        DContravariant<Base> D_B2 = SampleFunc<Base>;
        DContravariant<Derived> D_D2 = SampleFunc<Derived>;
        D_D2 = D_B2;  // 逆变
    }
}
```

> 扩展变体泛型接口

- 扩展变体泛型接口时，必须使用 `in` 和 `out` 关键字来显式指定派生接口是否支持变体。编译器不会根据正在扩展的接口来推断变体。

```csharp
interface ICovariant<out T> { }
interface IInvariant<T> : ICovariant<T> { }
interface IExtCovariant<out T> : ICovariant<T> { }

```

- 如果泛型类型参数 `T` 在一个接口中声明为协变，则无法在扩展接口中将其声明为逆变。

```csharp
interface ICovariant<out T> { }
interface IContravariant<in T> { }
interface IInvariant<T> : ICovariant<T>, IContravariant<T> { }  // 无法声明逆变或协变
```

> 避免多义性 

- 实现变体泛型接口时，变体有时可能会导致多义性。应避免这样的多义性。如果在一个类中使用不同的泛型类型参数来显式实现同一变体泛型接口，便会产生多义性。在这种情况下，编译器不会产生错误，但未指定将在运行时选择哪个接口实现。这种多义性可能导致代码中出现小 bug。

```csharp
// Simple class hierarchy.
class Animal;
class Cat : Animal;
class Dog : Animal;

// This class introduces ambiguity
// because IEnumerable<out T> is covariant.
class Pets : IEnumerable<Cat>, IEnumerable<Dog>
{
    IEnumerator<Cat> IEnumerable<Cat>.GetEnumerator()
    {
        Console.WriteLine("Cat");
        // Some code.
        return null;
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        // Some code.
        return null;
    }
    IEnumerator<Dog> IEnumerable<Dog>.GetEnumerator()
    {
        Console.WriteLine("Dog");
        // Some code.
        return null;
    }
}
class Program
{
    public static void Test()
    {
        IEnumerable<Animal> pets = new Pets();
        pets.GetEnumerator();  // Cat ? Dog ? 
    }
}
```

> 数组的协变

- 数组的协变使派生程度更大的类型的数组能够隐式转换为派生程度更小的类型的数组。

```csharp
IEnumerable<object> e = new List<string>();
IEnumerable<object> e2 = new List<int>();  // CS0266，值类型不支持协变
IEnumerable<object>[] enumerables = new List<string>[] { }; // 数组的协变
```

---