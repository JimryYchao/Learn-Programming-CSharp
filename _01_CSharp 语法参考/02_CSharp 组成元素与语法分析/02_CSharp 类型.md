## CSharp 类型

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

