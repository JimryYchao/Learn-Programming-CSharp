## CSharp 类型

值类型和引用类型是 C# 类型的两个主要类别：
值类型的变量包含类型的实例。对于值类型变量，会复制相应的类型实例。
引用类型的变量存储对其数据（对象）的引用。对于引用类型，两种变量可引用同一对象，对一个变量执行的操作会影响另一个变量所引用的对象。

---

### 值类型

#### 整型数值类型

```csharp
sbyte   _int8 = -8;         // -2^7 ~ 2^7-1
byte    _uint8 = 8;         // 0 ~ 2^8-1
short   _int16 = -16;       // -2^15 ~ 2^15-1
ushort  _uint16 = 16;       // 0 ~ 2^16
int     _int32 = -32;       // -2^31 ~ 2^31-1
uint    _uint32 = 32;       // 0 ~ 2^32
long    _int64 = -64L;      // -2^63 ~ 2^63-1
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
long _long_64 = 0xffffL             // long, ulong 长整数后缀 L, l
uint _uint_32 = 32u                 // 无符号整数后缀 U, u
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
- 

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

- 可以使用 `readonly` 修饰符来声明结构类型为不可变，其所有数据成员都必须是只读的。
- 还可以使用 `readonly` 修饰符来声明结构的实例成员不会被修改，因此只限定当前成员而不用将整个结构类型声明为 `readonly`。
- 通常，将 `readonly` 修饰符应用于以下类型的实例成员：
  - 方法：该方法不能修改结构中的其他实例成员，因为它们在函数域内是只读的。
  - 属性和索引器：`get` 访问器是默认的 `readonly get`；若修饰整个属性，则 `set` 无法声明，可以声明为 `init` 访问器。
  - 只读字段或属性可以在构造函数中重新初始化。

```csharp
public struct Sample
{
    public readonly int x;
    public int y;
    public readonly int Sum() => x + y;
    public int Data { readonly get; set; }
    public readonly int ReadonlyData { get; init; }
}
```

> with 表达式（C#10）

- 可以使用 `with` 表达式生成修改了指定属性或字段的结构类型实例的副本

```csharp
Point p1 = new(1, 1);
Point p2 = p1 with { X = 0 };
Console.WriteLine("Point(p1) = ({0},{1})", p1.X, p1.Y);
Console.WriteLine("Point(p2) = ({0},{1})", p2.X, p2.Y);

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

- 如果值类型必须被频繁装箱，那么在这些情况下最好避免使用值类型。可通过使用泛型集合（例如 `System.Collections.Generic.List<T>`）来避免装箱值类型。
- 装箱和取消装箱过程需要进行大量的计算。对值类型进行装箱时，必须创建一个全新的对象，这可能比简单的引用赋值用时最多长 20 倍。取消装箱的过程所需时间可达赋值操作的四倍。
- 装箱用于在垃圾回收堆中存储值类型。装箱是值类型到 `object` 类型或到此值类型所实现的任何接口类型的隐式转换。对值类型装箱会在堆中分配一个对象实例，并将该值复制到新的对象中。
- 取消装箱（拆箱）是从 `object` 类型到值类型或从接口类型到实现该接口的值类型的显式转换。首先要先检查对象实例，以确保它是给定值类型的装箱值，然后再将该值从实例复制到值类型变量中。被取消装箱的项必须是对一个对象的引用，该对象是先前通过装箱该值类型的实例创建的。尝试取消装箱 `null` 会导致 `NullReferenceException`， 尝试取消装箱对不兼容值类型的引用会导致 `InvalidCastException`。

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
    }
    }";
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

#### delegate 委托类型

- 委托类型的声明与方法签名相似，它有一个返回值和任意数目任意类型的参数。`delegate` 是一种可用于封装命名方法或匿名方法的引用类型，安全且可靠。可以使用 `event` 修饰委托类型将其转换为事件。必须使用具有兼容返回类型和输入参数的方法或 lambda 表达式实例化委托。

```csharp
public delegate void MessageDelegate(string message);
public delegate int AnotherDelegate(MyType m, long num);

MessageDelegate MessagePrint = message => Console.WriteLine(message);
MessageDelegate Debug = delegate (string message) 

MessagePrint.Invoke("Hello World!");

// delegate 声明匿名方法
var Debug = delegate (string message)
{
    MessagePrint?.Invoke(message);
};
```

- 在 .NET 中，`System.Action` 和 `System.Func` 类型为许多常见委托提供泛型定义。

```csharp
Action<string> Debug = delegate (string mess) { Console.WriteLine(mess); };
Func<int,int,int> Sum = delegate (int a, int b) { return a + b; };
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