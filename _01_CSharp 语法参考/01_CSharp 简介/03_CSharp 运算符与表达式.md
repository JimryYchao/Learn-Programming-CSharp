## CSharp 运算符和表达式

---
### 算数运算

- 一元 `++`（增量）、`--`（减量）、`+`（加）和 `-`（减）运算符
- 二元 `*`（乘法）、`/`（除法）、`%`（余数）、`+`（加法）和 `-`（减法）运算符

> 算数运算中的程序设定

- `ulong` 类型不支持一元 `-` 运算符。
- 整数除法的结果为整数，并等于两数之商向零舍入后的结果。
- 整数余数 `a % b` 的结果是 `a - (a / b) * b` 得出的值，非零余数的符号与左侧操作数的符号相同。
- 优先级：`x++, x--` > `++x, --x, +x, -x` > `*, /, %` > `+, -`。
- 复合运算：二元 `*=`、`/=`、`%=`、`+=`、`-=`。

> 算数溢出与被零除

- 整数被零除总是引发 `DivideByZeroException`。
- 发生整数算术溢出，溢出检查上下文将控制引发的行为，默认算数环境为 `unchecked` 上下文。
- 使用 `float` 和 `double` 类型的算术运算永远不会引发异常。对于 `decimal` 类型的操作数，算术溢出始终会引发 `OverflowException`，被零除总是引发 `DivideByZeroException`。
- 由于实数和浮点运算的浮点表达形式的常规限制，在使用浮点类型的计算中可能会发生舍入误差。

```csharp
// 增量或减量运算
int i = 3;
Console.WriteLine(i);   // output: 3
Console.WriteLine(i++); // output: 3    // use 在前，增量在后
Console.WriteLine(i);   // output: 4
Console.WriteLine(--i); // output: 3    // 增量在前，use 在后
Console.WriteLine(i);   // output: 3

// 一元运算
uint a = 5;
var b = -a;
Console.WriteLine(b);            // output: -5
Console.WriteLine(b.GetType());  // output: System.Int64

// 二元算数
Console.WriteLine(-13 / 5);     // output: -2
Console.WriteLine(13 / 5.0);    // output: 2.6
Console.WriteLine(-5 % 4);      // output: -1
Console.WriteLine(5 % -4);      // output: 1
Console.WriteLine(-5.2f % 2.0f); // output: -1.2
Console.WriteLine(5 + 4.3);     // output: 9.3
Console.WriteLine(47 - 3);      // output: 44
Console.WriteLine(7.5m - 2.3m); // output: 5.2

// 复合运算
int c = 5;
Console.WriteLine(c += 9);  // c=c+9, output: 14
Console.WriteLine(c -= 4);  // c=c-4, output: 10 
Console.WriteLine(c *= 2);  // c=2*c, output: 20
Console.WriteLine(c /= 4);  // c=c/4, output: 5
Console.WriteLine(c %= 3);  // c=c%3, output: 2
```

---
### 逻辑运算

- 一元 `!`（逻辑非）运算符。
- 二元 `&`（逻辑与）、`|`（逻辑或）和 `^`（逻辑异或）运算符，这些运算符始终计算两个操作数。
- 二元 `&&`（条件逻辑与）和 `||`（条件逻辑或）运算符，这些运算符仅在必要时才计算右侧操作数。

> 逻辑运算中的程序设定

- 对于 `bool?` 操作数：
  - `true & true = true`，`false & false/null/true = false`，`null & null/true = null`。
  - `false | false = false`，`true | false/null/true = true`，`null | null/false = null`。
  - `!null = null`，`null ^ null/false/true = null`。 
  - 条件逻辑运算符 `&&` 和 `||` 不支持 `bool?` 操作数。

- 优先级：`!` > `&` > `^` > `|` > `&&` > `||`。
- 复合运算：二元 `&=`、`|=`、`^=`。


```csharp
class Sample
{
    static bool SecondOperand(string @operator, bool? first = null)
    {
        Console.Write($"{(first.HasValue ? first.Value.ToString() : ""), 5} {@operator,2} Second operand is evaluated >> result = ");
        return true;
    }
    static void Main()
    {
        // 逻辑运算
        Console.WriteLine(!SecondOperand("!"));
        Console.WriteLine(true & SecondOperand("&", true));
        Console.WriteLine(false & SecondOperand("&", false));
        Console.WriteLine(true ^ SecondOperand("^",true));
        Console.WriteLine(false ^ SecondOperand("^",false));
        Console.WriteLine(true | SecondOperand("|", true));
        Console.WriteLine(false | SecondOperand("|",false));

        // 条件逻辑运算
        Console.WriteLine(true && SecondOperand("&&", true));
        Console.WriteLine(false && SecondOperand("&&",false));
        Console.WriteLine(true || SecondOperand("||", true));
        Console.WriteLine(false || SecondOperand("||", false));

        // 复合运算
        bool boo = true;
        Console.WriteLine(boo &= SecondOperand("&=", boo));
        Console.WriteLine(boo ^= SecondOperand("^=", boo));
        Console.WriteLine(boo |= SecondOperand("|=", boo));
    }
}
/*
       ! Second operand is evaluated >> result = False
 True  & Second operand is evaluated >> result = True
False  & Second operand is evaluated >> result = False
 True  ^ Second operand is evaluated >> result = False
False  ^ Second operand is evaluated >> result = True
 True  | Second operand is evaluated >> result = True
False  | Second operand is evaluated >> result = True
 True && Second operand is evaluated >> result = True
False
True
False || Second operand is evaluated >> result = True
 True &= Second operand is evaluated >> result = True
 True ^= Second operand is evaluated >> result = False
False |= Second operand is evaluated >> result = True
 */
```

> 用户定义的条件逻辑运算符

- 若用户定义类型已包含 `|`（或 `&`）运算符重载，可以定义 `true` 和 `false` 的运算符重载以支持该类型执行条件逻辑运算 `||`（或 `&&`）。

```csharp
public struct LaunchStatus(int status)
{
    public static readonly LaunchStatus Green = new LaunchStatus(0);
    public static readonly LaunchStatus Yellow = new LaunchStatus(1);
    public static readonly LaunchStatus Red = new LaunchStatus(2);
    private int Status = status;

    public static bool operator false(LaunchStatus x) => x == Red;
    public static bool operator true(LaunchStatus x) => x == Green || x == Yellow;

    public static LaunchStatus operator &(LaunchStatus x, LaunchStatus y)
    {
        if (x == Red || y == Red || (x == Yellow && y == Yellow))
            return Red;
        if (x == Yellow || y == Yellow)
            return Yellow;
        return Green;
    }
    public static bool operator ==(LaunchStatus x, LaunchStatus y) => x.Status == y.Status;
    public static bool operator !=(LaunchStatus x, LaunchStatus y) => !(x == y);
    public override bool Equals(object obj) => obj is LaunchStatus other && this == other;
    public override int GetHashCode() => status;
}
public class LaunchStatusTest
{
    public static void Main()
    {
        LaunchStatus okToLaunch = GetFuelLaunchStatus() && GetNavigationLaunchStatus();
        Console.WriteLine(okToLaunch ? "Ready to go!" : "Wait!");
    }
    static LaunchStatus GetFuelLaunchStatus()
    {
        Console.WriteLine("Getting fuel launch status...");
        return LaunchStatus.Red;
    }
    static LaunchStatus GetNavigationLaunchStatus()
    {
        Console.WriteLine("Getting navigation launch status...");
        return LaunchStatus.Yellow;
    }
}
```

---
### 比较运算

- 二元 `<`（小于）、`>`（大于）、`<=`（小于等于）、`>=`（大于等于）、`==`（等于）、`!=`（不等于）运算符。

> 比较运算中的程序设定

- 所有的数值类型都支持这些运算符。任一操作数是 `NaN` 非数字，比较运算的结果都是 `false`。枚举类型比较仅限于相同枚举类型的操作数。`char` 类型比较对应的字符代码。
- 这些运算符不支持复合运算。
- 类型相等性比较：
  - 引用非记录类型只有在引用同一对象时相等。
  - 记录类型当所有字段对应值和自动实现的属性相等时判定相等性。
  - 字符串具有相等长度且每个字符位置有相同字符时判定相等性。
  - 两个委托类型在其调用列表长度和每个位置具有相同的条目时，二者相等。

```csharp
Console.WriteLine(0xffff < '你');  // False
Console.WriteLine('我' > '你');    // True
Console.WriteLine(float.NegativeInfinity <= 0); // True
Console.WriteLine(float.NaN >= 0); // False
Console.WriteLine('A' == 65);      // True
Console.WriteLine('我' != '你');   // True
```

---
### new 运算符

- `new` 运算符创建类型的新实例。可用于构造函数调用、目标类型化、创建数组、匿名类型实例化。

```c
// 构造函数调用
List<int> list1 = new List<int>(capacity: 10);

// 目标类型化
List<int> list2 = new(capacity: 10);
List<int> list3 = new() { Capacity = 20 };
Dictionary<int, List<int>> lookup = new()
{
    [1] = new() { 1, 2, 3 },
    [2] = new() { 5, 8, 3 },
    [5] = new() { 1, 0, 4 }
};

// 数组创建
int[] arr1 = new int[100];
int[] arr2 = new int[] { 10, 20, 30 };
int[] arr3 = new[] { 10, 20, 30 };

// 匿名类型实例化
var exam = new { Greet = "Hello", Name = "World" };
```

---
### with 表达式

- 使用 `with` 表达式创建左侧操作数的副本，附加需要修改的特性属性和字段。在 C#9 中，`with` 表达式的左侧操作数必须为记录类型。从 C#10 开始，`with` 表达式的左侧操作数可以为结构类型或匿名类型。对于引用类型成员，在复制操作数时仅复制对成员实例的引用，副本和原始操作数都具有对同一引用类型实例的访问权限。
 
```csharp
var Point = new { x = 1, y = 1 };
var Point_copy = Point with { x = 10 };

Sample s = new Sample("Ychao") { ID = 1 };
Sample s_copy = s with { Name = "Hello", ID = 2 };

record Sample(string name)
{
    public string Name { get; init; } = name;
    public int ID { get; set; }
}
``` 

---
### 集合表达式

- 可以使用集合表达式来创建常见的集合值。集合表达式在 `[` 和 `]` 括号之间包含元素的序列。

```csharp
Span<string> weekDays = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
foreach (var day in weekDays)
    Console.WriteLine(day);
```

> .. 分布元素

- 使用分布元素 `..` 在集合表达式中使用内联集合值。

```csharp
int[] left = [1, 2, 3, 4];
int[] right = [5, 6, 7, 8, 9];
int[] all = [.. left, .. right, 0];
Console.WriteLine(string.Join(",", all)); 
// output: 1,2,3,4,5,6,7,8,9,0
```

<!-- > 集合生成器

- 从 Net8 起，类型通过编写 `Create()` 方法和对集合类型应用 `System.Runtime.CompilerServices.CollectionBuilderAttribute` 来指示生成器方法来选择加入集合表达式支持。 
- url=https://learn.microsoft.com/zh-cn/dotnet/csharp/language-reference/operators/collection-expressions
-->
 

---
### 成员访问与 null 条件运算

- `.`（成员访问）：用于访问命名空间或类型的成员。
- `[]`（数组元素或索引器访问）：用于访问数组元素或类型索引器。
- `?.` 和 `?[]`（`null` 条件运算符）：仅当操作数为非 `null` 时才用于执行成员或元素访问运算，否则返回 `null`。
- `()`（调用）：用于调用被访问的方法或调用委托。
- `^`（从末尾开始索引）：指示元素位置来自序列的末尾，`[^n] = [Length - n]`。
- `..`（范围）：指定可用于获取一系列序列元素的索引范围。`..` 运算符指定某一索引范围的开头和末尾作为其操作数，左侧操作数是范围的包含性开头，右侧操作数是范围的不包含性末尾。`[a..]` 等效于 `[a..^0]`、`[..b]` 等效于 `[0..b]`、`[..]` 等效于 `[0..^0]`。

```csharp
// . 成员访问
System.Console.WriteLine("Hello, World");

// [] 索引器元素访问
int[] arr = [1, 2, 3, 4, 5, 6, 7];
arr[0] = 1000;

// ?. ?[] Null 条件运算符 
double SumNumbers(List<double[]> setsOfNumbers, int indexOfSetToSum)
    => setsOfNumbers?[indexOfSetToSum]?.Sum() ?? double.NaN;
var sum = SumNumbers(null, 0);
Console.WriteLine(sum);  // output: NaN

// () 调用表达式
var Print = (string mess) => Console.WriteLine("PRINT : " + mess);
Print("Hello, World");

// ^ 末尾运算符
var xs = new int[] { 0, 10, 20, 30, 40 };
int last = xs[^1];  // [^0] 将超出索引范围

// .. 范围运算符
var rx = new int[] { 0, 100, 200, 300, 400 };
int halfIndex = rx.Length / 2;
var rx_LeftHalf = rx[..halfIndex];
var rx_RightHalf = rx[^(rx.Length - halfIndex)..];
Console.WriteLine("Left Half : " + string.Join(",", rx_LeftHalf));
Console.WriteLine("Right Half : " + string.Join(",", rx_RightHalf));
```

<br>

#### 索引与范围 

- 索引 `System.Index` 和范围 `System.Range` 为访问序列中的单个元素或范围提供了简洁的语法。表达式 `^0` 属于 `System.Index` 类型，表达式 `a..b` 属于 `System.Range` 类型。若任何类型提供带 `Index` 或 `Range` 参数的索引器，则该类型可分别显式支持索引 `^n` 或范围 `a..b`。
- 单维度数组、交错数组、`String`、`Span<T>` 和 `ReadOnlySpan<T>` 同时支持索引和范围。`List<T>` 仅支持索引。

```csharp
System.Range r = 0..5;
System.Index i = ^1;

string str = "Hello, World";
string hello = str[0..5];  // str[r]

List<int> arr = [0, 1, 2, 3, 4, 5];
var last = arr[^1];    // arr[i]
```

> 索引和范围的类型支持

- 若任何类型提供带 `Index` 或 `Range` 参数的索引器，则该类型可分别显式支持索引或范围。

```csharp
PointArray ps = new PointArray(
    new(0, 1), new(1, 2), new(2, 3), 
    new(3, 4), new(4, 5), new(5, 6));
Console.WriteLine("Last Point is " + ps[^1]);
// Last Point is (5, 6)
Console.WriteLine("The Points of range 1~4 are " + string.Join(",", ps[1..5]));
// The Points of range 1~4 are (1, 2),(2, 3),(3, 4),(4, 5)

readonly record struct Point(int x, int y)
{
    public override string ToString() => (x, y).ToString();
}
record struct PointArray(params Point[] ps)
{
    public Point this[Index i] => ps[i];
    public Point[] this[Range r] => ps[r];
}
```

> 索引和范围的隐式支持

- 若某个类型具有一个名为 `int Length{ get;}` 或 `int Count{ get;}` 实例属性和一个 `T this[int index]` 实例索引器，并且没有仅以 `System.Index` 类型为索引的索引器时，该类型隐式支持索引运算。首选使用 `Length`。
- 若某个类型具有一个名为 `int Length{ get;}` 实例属性和一个 `public T[] Slice(int start, int length)` 实例方法，并且没有仅以 `System.Range` 为索引的索引器时，该类型隐式支持范围运算。

```csharp
PointArray ps = new PointArray(
    new(0, 1), new(1, 2), new(2, 3), 
    new(3, 4), new(4, 5), new(5, 6));
Console.WriteLine("Last Point is " + ps[^1]);
// Last Point is (5, 6)
Console.WriteLine("The Points of range 1~4 are " + string.Join(",", ps[1..5]));
// The Points of range 1~4 are (1, 2),(2, 3),(3, 4),(4, 5)

readonly record struct Point(int x, int y)
{
    public override string ToString() => (x, y).ToString();
}
record struct PointArray(params Point[] ps)
{
    public Point this[int i] => ps[i];
    public int Length => ps.Length;
    public Point[] Slice(int start, int length)
    {
        var _ps = new Point[length];
        Array.Copy(ps, start, _ps, 0, length);
        return _ps;
    }
}
```


---
### 位运算符与移位运算符

- 一元 `~`（按位求补）运算符。
- 二进制 `&`（按位与）、`|`（按位或）和 `^`（按位异或）运算符。
- 二进制 `<<`（左移）、`>>`（右移）和 `>>>`（无符号右移）运算符。

> 位运算和移位运算中的程序设定

- 这些运算符是针对 `int`、`uint`、`long` 和 `ulong` 类型定义的。位运算和移位运算永远不会导致溢出。
- 左移运算会放弃超出结果类型范围的高阶位，并将低阶空位位置设置为零；右移位运算会放弃低阶位，其中有符号数高位用 1 补齐，无符号数用 0 补齐；`>>>` 运算符始终执行逻辑移位，高位始终用 0 补齐。
- `~1010 = 0101`，`1010 & 1100 = 1000`，`1010 | 1100 = 1110`，`1010 ^ 1100 = 0110`。 
- 优先级：`~` > `<<`、`>>`、`>>>` > `&` > `^` > `|`。
- 复合运算：二元 `&=`、`^=`、`|=`、`<<=`、`>>=`、`>>>=`。

> 移位运算中的移位计数

- 对于 `<<`、`>>`、`>>>` 运算符的右操作数必须是 `int` 类型，对于 `x << count`、`x >> count` 和 `x >>> count` 表达式，实际移位计数取决于 `x` 的类型：
  - 若 `x` 为 `int` 或 `uint` 则移位计数由右侧操作数的低阶五位定义，即 `count & 0x1F` 或 `count & 0b_1_1111`。
  - 若 `x` 为 `long` 或 `ulong` 则移位计数由右侧操作数的低阶六位定义，即 `count & 0x3F` 或 `count & 0b_11_1111`。

```csharp
uint a = 0b_0000_1111_0000_1111_0000_1111_0000_1100u;
uint b = 0b_1010_0101_1100_0011_0000_1111_1010_0101u;
int c = -0b_1011_0101_1010;

// 位运算
ToString(~a, "~", a);
ToString(a & b, "&", b, a);
ToString(a | b, "|", b, a);
ToString(a ^ b, "^", b, a);

// 按位运算
ToString(a << 6, "<<", 6, a, false);
ToString(a >> 6, ">>", 6, a, false);
ToString(c << 6, "<<", 6, c, false);
ToString(c >> 6, ">>", 6, c, false);
ToString(b >>> 10, ">>>", 10, b, false);
ToString(c >>> 10, ">>>", 10, c, false);

static void ToString(long value, string @operator, long rhs, long? lhs = null, bool rbase2 = true)
    => Console.WriteLine(
        $"\n{(lhs.HasValue ? lhs.Value.toString() : ""),+36}" +
              $"\n{@operator,-4}{rhs.toString(rbase2),32}\n  " +
              $"= {value.toString(),32}");
static class Ext
{
    public static string toString(this long val, bool base2 = true) => Convert.ToString((int)val, toBase: base2 ? 2 : 10);
}
```

---
### 类型测试与类型转换

#### is、as、typeof 类型测试

- `is` 运算符检查 `Expression is T` 表达式结果的运行时类型是否与给定类型 `T` 兼容。其测试的表达式 `Expression` 不能是方法组、匿名方法、lambda 表达式。`is` 运算符不会考虑用户定义的转换。
- 类型测试 `E is T` 的表达式结果 `E` 为非 null 且在运行时类型是 `T` 类型、或可隐式转换类型、或 `T` 的派生类型、或具有基础类型 `T` 的可为 null 的值类型、或存在从 `E` 的运行时类型到类型 `T` 的装箱或取消装箱转换，则模式匹配成功。

```csharp
object b = new Base();
Console.WriteLine(b is Base);  // output: True
Console.WriteLine(b is Derived);  // output: False

object d = new Derived();
Console.WriteLine(d is Base);  // output: True
Console.WriteLine(d is Derived); // output: True

class Base;
class Derived : Base;
```

- `as` 运算符将 `Expression as T` 表达式结果显式转换为给定的引用类型或可为 null 的类型。`T` 不能是不可为 null 的值类型。如果无法进行转换，则 `as` 运算符返回 `null`，因此 `as` 运算符永远不会引发异常。`E as T` 等价于 `E is T ? (T)(E) : (T)null`。

```csharp
IEnumerable<int> numbers = new[] { 10, 20, 30 };
IList<int> indexable = numbers as IList<int>;
if (indexable != null)
    Console.WriteLine(indexable[0] + indexable[indexable.Count - 1]);  // output: 40
```

- `typeof` 运算符用于获取某个类型的 `System.Type` 实例。`typeof` 运算符的实参必须是类型或类型形参的名称，不能是 `dynamic` 或任何可为 null 的引用类型。
- `Object.GetType()` 用以获取某个实例对象的相应类型的 `System.Type` 实例。使用 `typeof` 运算符和 `object.GetType()` 来检查表达式结果的运行时类型是否与给定的类型完全匹配。

```csharp
object b = new Giraffe();
Console.WriteLine(b is Animal);  // output: True
Console.WriteLine(b.GetType() == typeof(Animal));  // output: False

Console.WriteLine(b is Giraffe);  // output: True
Console.WriteLine(b.GetType() == typeof(Giraffe));  // output: True

class Animal;
class Giraffe : Animal;
```

<br>

#### 强制转换

- 形式为 `(T)E` 的强制转换表达式将表达式 `E` 的结果显式转换为类型 `T`。如果不存在从类型 `E` 到类型 `T` 的显式转换，则发生编译时错误。在运行时，显式转换可能不会成功，强制转换表达式可能会引发异常。

```csharp
double x = 1234.7;
int a = (int)x;
Console.WriteLine(a);   // output: 1234

IEnumerable<int> numbers = new int[] { 10, 20, 30 };
IList<int> list = (IList<int>)numbers;
Console.WriteLine(list.Count);  // output: 3
Console.WriteLine(list[1]);  // output: 20
```

<br>

#### 用户定义转换

- 可以使用 `operator` 和 `implicit` 或 `explicit` 关键字分别用于定义隐式转换或显式转换。定义转换的类型必须是该转换的源类型或目标类型。

```csharp
var p = (Point)(10, 1);
(double x, double y) P = new Point(1, 10);

class Point(double x, double y)
{
    public double X => x;
    public double Y => y;

    // 从 Point 到元组的隐式转换
    public static implicit operator (double x, double y)(Point p) => (p.X, p.Y);
    // 从 元组 到 Point 的显式转换
    public static explicit operator Point((double x, double y) p) => new Point(p.x, p.y);
}
```

---
### 指针相关的运算符

- 一元 `&` 地址运算符，用于获取变量的地址。
- 一元 `*` 间接运算符，用于获取指针指向的变量。
- `->` 指针成员访问和 `[]` 元素访问运算符。
- 指针算术运算：`+`、`-`、`++` 和 `--`。
- 指针比较运算：`==`、`!=`、`<`、`>`、`<=` 和 `>=`。

> 指针相关运算符的程序设定

- `&` 的操作数必须是固定变量（固定变量是驻留在不受垃圾回收器操作影响的存储位置的变量）。驻留在可能受垃圾回收器影响的存储位置的变量（如重定位）称为可移动变量，对象字段和数组元素是可移动变量的示例，使用 `fixed` 语句 “固定”，则可以获取可移动变量的地址。无法获取常量或值的地址。
- `*` 获取其操作数指向的变量，它也称为取消引用运算符。`*` 运算符的操作数必须是指针类型，不能将 `*` 运算符应用于类型 `void*` 的表达式。
- `->` 指针成员访问合并间接 `*` 和成员访问 `.`，即 `X->y` 等价于 `(*X).y`。
- 对于指针类型 `T*` 的表达式 `ptr`，`ptr[n]` 形式的指针元素访问计算方式为 `*(ptr + n)`，等价于 `*(T*)((IntPtr)ptr + n * sizeof(T))`。参考指针算数。

> 指针算数

- 向指针 `p` 增加或从指针中减少整数值 `n`，表示将 `p` 的地址添加或减去 `n*sizeof(T)`。增量减量运算表示指针自增或自减 1 个 `sizeof(T)`。
- 比较运算用于比较两个指针操作数给出的地址。

```csharp
unsafe
{
    int[] arr = { 0, 1, 2, 3, 4, 5, 6 };
    fixed (int* ptr = arr)
    {
        nint I_ptr = (nint)ptr;
        nint I_ptr_2 = I_ptr + 2 * sizeof(int);****
        if (I_ptr_2 == (nint)(&ptr[2]))
            Console.WriteLine(I_ptr_2);     // could be: 2775296852288

        int* _Ptr = ptr;
        Console.WriteLine(*_Ptr);               // 0
        Console.WriteLine(*++_Ptr);             // 1

        Console.WriteLine(*(ptr + 2));          // 2
        Console.WriteLine(ptr[2]);              // 2

        Console.WriteLine(_Ptr >= ptr);         // True
        Console.WriteLine(--_Ptr == ptr);       // True
    }
}
```

---
### 三元条件运算

- 条件运算符 `?:` 也称为三元条件运算符，用于计算布尔表达式 `condition ? consequent : alternative`，并根据布尔表达式的计算结果为 `true` 还是 `false` 来返回两个表达式中的一个结果。
- `consequent` 和 `alternative` 的类型必须可隐式转换为目标类型。


```csharp
var rand = new Random();
var condition = rand.NextDouble() > 0.5;

var x = condition ? 12 : (int?)null;
```

> ref 条件表达式

- 条件 `ref` 表达式可有条件地返回变量引用：`condition ? ref consequent : ref alternative`。在 `ref` 条件表达式中，`consequent` 和 `alternative` 的类型必须相同。
- 可以使用 `ref` 分配条件 `ref` 表达式的结果，将其用作引用返回，或将其作为 `ref`、`out` 或 `in` 方法参数传递。

```csharp
var smallArray = new int[] { 1, 2, 3, 4, 5 };
var largeArray = new int[] { 10, 20, 30, 40, 50 };

int index = 7;
ref int refValue = ref ((index < 5) ? ref smallArray[index] : ref largeArray[index - 5]);
refValue = 0;

index = 2;
((index < 5) ? ref smallArray[index] : ref largeArray[index - 5]) = 100;

Console.WriteLine(string.Join(" ", smallArray));
Console.WriteLine(string.Join(" ", largeArray));
// Output:
// 1 2 100 4 5
// 10 20 0 40 50
```




---
### Lambda 表达式

- 使用 Lambda 表达式来创建匿名函数：
  - 表达式 Lambda：`(input-parameters) => expression`。
  - 语句 Lambda：`(input-parameters) => { <sequence-of-statements> }`。

- 任何 Lambda 表达式都可以转换为委托类型，Lambda 表达式可以转换的委托类型由其参数和返回值的类型定义。
- 从 C#9 开始，可以使用弃元指定 Lambda 表达式中不使用的两个或更多输入参数。如果只有一个输入参数命名为 `_`，则在 Lambda 表达式中，`_` 将被视为该参数的名称。
- 从 C#10 开始，可以在输入参数前面指定 Lambda 表达式的返回类型。
- 从 C#10 开始，可以将特性添加到 Lambda 表达式及其参数。
- 从 C#12 开始，可以为 Lambda 表达式上的参数提供默认值。

```csharp
Func<int,int> Square = x => x * x;
var Sum = (int x, int y) => x + y;
// 显式指定返回类型
var Choose = object (bool b) =>  b? 1:"two";
// 参数列表中使用弃元
Func<int, int, int> Constant = (_, _) => 99;
// 添加特性到参数中
var concat = ([DisallowNull] string a, [DisallowNull] string b) => a + b;
// 使用 params
var Iterator = (params IEnumerable[] arr) =>
{
    foreach (var item in arr)
        Console.WriteLine(item);
};
```

> 异步 Lambda

- 通过使用 `async` 和 `await` 关键字，可以创建包含异步处理的 Lambda 表达式和语句。

```csharp
var asyncAction = async (Action ac) => await Task.Run(ac);
await asyncAction(() => Console.WriteLine("Hello, World"));
```

> 静态 Lambda

- 从 C# 9.0 开始，可以将 `static` 修饰符应用于 Lambda 表达式，以防止由 Lambda 无意中捕获本地变量或实例状态，但可以引用静态成员和常量定义。

```csharp
Func<double, double> square = static x => x * x;
```

> 捕获 Lambda 表达式中的外部变量和变量范围

- Lambda 可以引用外部变量。外部变量是在定义 Lambda 表达式的方法中或包含 Lambda 表达式的类型中的范围内变量。以这种方式捕获的变量将进行存储以备在 lambda 表达式中使用。必须明确地分配外部变量，然后才能在 Lambda 表达式中使用该变量。
- 用于 Lambda 表达式中的变量范围：
  - 捕获的变量将不会被作为垃圾回收，直至引用变量的委托符合垃圾回收的条件。
  - 在封闭方法中看不到 Lambda 表达式内引入的变量。
  - Lambda 表达式无法从封闭方法中直接捕获 `in`、`ref` 或 `out` 参数。
  - Lambda 表达式中的 `return` 语句不会导致封闭方法返回。
  - 如果相应跳转语句的目标位于 Lambda 表达式块之外，Lambda 表达式不得包含 `goto`、`break` 或 `continue` 语句。如果目标在块内部，在 Lambda 表达式块外部使用跳转语句也是错误的。

```csharp
public static class VariableScopeWithLambdas
{
    public class VariableCaptureGame
    {
        internal Action<int>? updateCapturedLocalVariable;
        internal Func<int, bool>? isEqualToCapturedLocalVariable;

        public void Run(int input)
        {
            int j = 0;
            updateCapturedLocalVariable = x =>
            {
                j = x;
                bool result = j > input;
                Console.WriteLine($"{j} is greater than {input}: {result}");
            };

            isEqualToCapturedLocalVariable = x => x == j;
            Console.WriteLine($"Local variable before lambda invocation: {j}");
            updateCapturedLocalVariable(10);
            Console.WriteLine($"Local variable after lambda invocation: {j}");
        }
    }
    public static void Main()
    {
        var game = new VariableCaptureGame();

        int gameInput = 5;
        game.Run(gameInput);

        int jTry = 10;
        bool result = game.isEqualToCapturedLocalVariable!(jTry);
        Console.WriteLine($"Captured local variable is equal to {jTry}: {result}");

        int anotherJ = 3;
        game.updateCapturedLocalVariable!(anotherJ);

        bool equalToAnother = game.isEqualToCapturedLocalVariable(anotherJ);
        Console.WriteLine($"Another lambda observes a new value of captured variable: {equalToAnother}");
    }
    // Output:
    // Local variable before lambda invocation: 0
    // 10 is greater than 5: True
    // Local variable after lambda invocation: 10
    // Captured local variable is equal to 10: True
    // 3 is greater than 5: False
    // Another lambda observes a new value of captured variable: True
}
```

---
### 弃元

- 弃元（`_`）是一种在应用程序代码中人为取消使用的占位符变量，相当于未赋值的变量，但是它们没有值。弃元将意图传达给编译器和其他读取代码的文件：用户打算忽略表达式的结果。可以使用弃元用来忽略表达式的结果、元组表达式的一个或多个成员、方法的 `out` 参数或模式匹配表达式的目标。

> 弃元的常见应用

- 析构元组和用户类型对象解构：析构元组或解构用户类型对象时，可以使用弃元用以忽略不需要的位置返回值。

```csharp
var (_, _, _, pop1, _, pop2) = QueryCityDataForYears("New York City", 1960, 2010);
Console.WriteLine($"Population change, 1960 to 2010: {pop2 - pop1:N0}");
// The example displays the following output:
//      Population change, 1960 to 2010: 393,149

static (string, double, int, int, int, int) QueryCityDataForYears(string name, int year1, int year2)
{
    int population1 = 0, population2 = 0;
    double area = 0;
    if (name == "New York City")
    {
        area = 468.48;
        if (year1 == 1960)
            population1 = 7781984;
        if (year2 == 2010)
            population2 = 8175133;
        return (name, area, year1, population1, year2, population2);
    }
    return ("", 0, 0, 0, 0, 0);
}
```

- 从 C#9 开始，可以使用弃元指定 Lambda 表达式中不使用的两个或更多输入参数。

```csharp
Func<int, int, int, int> Func = (_, _, val) => val * val;
```

- 对具有 `out` 参数的方法的调用。

```csharp
string[] dateStrings = {"05/01/2018 14:57:32.8", "2018-05-01 14:57:32.8",
                      "2018-05-01T14:57:32.8375298-04:00", "5/01/2018",
                      "5/01/2018 14:57:32.80 -07:00",
                      "1 May 2018 2:57:32.8 PM", "16-05-2018 1:00:32 PM",
                      "Fri, 15 May 2018 20:10:57 GMT" };

foreach (string dateString in dateStrings)
{
    if (DateTime.TryParse(dateString, out _))
        Console.WriteLine($"'{dateString}': valid");
    else
        Console.WriteLine($"'{dateString}': invalid");
}
```

- `switch` 表达式中使用弃元匹配任意的表达式，包括 `null` 在内。

```csharp
static Point Transform(Point point) => point switch
{
    { X: 0, Y: 0 } => new Point(0, 0),
    { X: var x, Y: var y } when x < y => new Point(x + y, y),
    { X: var x, Y: var y } when x > y => new Point(x - y, y),
    _ => new Point(2 * point.X, 2 * point.Y),  // 弃元模式
};
public readonly record struct Point(int X, int Y);
```

- 可使用独立弃元来指示要忽略的任何变量。例如使用弃元来忽略异步操作返回的 `Task` 对象，并忽略该异步操作生成的任何错误。

```csharp
static async Task ExecuteAsyncMethods()
{
    Console.WriteLine("About to launch a task...");
    _ = Task.Run(() =>
    {
        var iterations = 0;
        for (int ctr = 0; ctr < int.MaxValue; ctr++)
            iterations++;
        Console.WriteLine("Completed looping operation...");
        throw new InvalidOperationException();
    });
    await Task.Delay(5000);
    Console.WriteLine("Exiting after 5 second delay");
}
// The example displays output like the following:
//       About to launch a task...
//       Completed looping operation...
//       Exiting after 5 second delay
```

- `_` 也是有效的标识符，因此在支持弃元的上下文之外，若已有名为 `_` 的标识符已在范围内，则无法使用弃元。

```csharp
private static bool RoundTrips(int _)
{
    string value = _.ToString();
    int newValue = 0;
    _ = Int32.TryParse(value, out newValue);
  // error CS0029: Cannot implicitly convert type 'bool' to 'int'
    return _ == newValue;
}
```

---
### 模式匹配

- 可以使用 `is` 表达式、`switch` 语句和 `switch` 表达式将输入表达式与任意数量的特征匹配。C# 支持多种模式，包括声明、类型、常量、关系、属性、列表、var 和弃元。可以使用布尔逻辑关键字 `and`、`or` 和 `not` 组合模式。

<br>

#### is 表达式

```csharp
bool rt_1 = E is T;
bool rt_2 = E is T t;

// exam
int i = 34;
object iBoxed = i;
int? jNullable = 42;
if (jNullable is not null)  // 检查 null
    if (iBoxed is int a && jNullable is int b)
        Console.WriteLine(a + b);  // output 76
```

<br>

#### switch 语句

```csharp
switch (<switch_on>)
{
    case <exp_1>:
    case <exp_2>: 
        // do...
        break; // return, goto
    default:
        // default do
        break;
}

// exam
DisplayMeasurement(-4);     // Output: Measured value is -4; too low.
DisplayMeasurement(5);      // Output: Measured value is 5.
DisplayMeasurement(30);     // Output: Measured value is 30; too high.
DisplayMeasurement(double.NaN);  // Output: Failed measurement.

void DisplayMeasurement(double measurement)
{
    switch (measurement)
    {
        case < 0.0: Console.WriteLine($"Measured value is {measurement}; too low."); break;
        case > 15.0: Console.WriteLine($"Measured value is {measurement}; too high."); break;
        case double.NaN: Console.WriteLine("Failed measurement."); break;
        default: Console.WriteLine($"Measured value is {measurement}."); break;
    }
}
```

<br>

#### switch 表达式

```csharp
var rt = <switch_on> switch
{
    case_1 => case_1_return,
    case_2 => case_2_return,
    //....cases
    _ => default_return
};

// exam
public static class SwitchExample
{
    public enum Direction { Up, Down, Right, Left }
    public enum Orientation { North, South, East, West }
    public static Orientation ToOrientation(Direction direction) => direction switch
    {
        Direction.Up => Orientation.North,
        Direction.Right => Orientation.East,
        Direction.Down => Orientation.South,
        Direction.Left => Orientation.West,
        _ => throw new ArgumentOutOfRangeException(nameof(direction), $"Not expected direction value: {direction}"),
    };
    static void Main()
    {
        var direction = Direction.Right;
        Console.WriteLine($"Map view direction is {direction}." + $" Cardinal orientation is {ToOrientation(direction)}.");
        // Output: Map view direction is Right. Cardinal orientation is East.
    }
}
```

> 模式匹配类型
  - 声明模式：用于检查表达式的运行时类型，如果匹配成功，则将表达式结果分配给声明的变量。
  - 类型模式：用于检查表达式的运行时类型。 在 C# 9.0 中引入。
  - 常量模式：用于测试表达式结果是否等于指定常量。
  - 关系模式：用于将表达式结果与指定常量进行比较。 在 C# 9.0 中引入。
  - 逻辑模式：用于测试表达式是否与模式的逻辑组合匹配。 在 C# 9.0 中引入。
  - 属性模式：用于测试表达式的属性或字段是否与嵌套模式匹配。
  - 位置模式：用于解构表达式结果并测试结果值是否与嵌套模式匹配。
  - var 模式：用于匹配任何表达式并将其结果分配给声明的变量。
  - 弃元模式：用于匹配任何表达式。
  - 列表模式：测试序列元素是否与相应的嵌套模式匹配。 在 C# 11 中引入。

<br>

#### 声明模式

- 使用声明模式检查表达式的运行时类型是否与给定类型兼容。声明模式的表达式结果 `E` 为非 null 且在运行时类型是 `T` 类型、或可隐式转换类型、或 `T` 的派生类型、或具有基础类型 `T` 的可为 null 的值类型、或存在从 `E` 的运行时类型到类型 `T` 的装箱或取消装箱转换，则模式匹配成功。  

> is 表达式

```csharp
object greeting = "Hello, World!";
if (greeting is string message)
    Console.WriteLine(message.ToLower());  // output: hello, world!
```

> switch 表达式和语句

```csharp
var numbers = new int[] { 10, 20, 30 };
Console.WriteLine(GetSourceLabel(numbers));  // output: 1

var letters = new List<char> { 'a', 'b', 'c', 'd' };
Console.WriteLine(GetSourceLabel(letters));  // output: 2
// switch 表达式
static int GetSourceLabel<T>(IEnumerable<T> source) => source switch
{
    Array array => 1,
    ICollection<T> collection => 2,
    _ => 3,
};
// switch 语句
static int GetSourceLabel<T>(IEnumerable<T> source)
{
    switch (source)
    {
        case Array array: /*do...*/ return 1;
        case ICollection<T> collection: return 2;
        default: return 3;
    }
}
```

- 可以使用弃元代替变量名。

```csharp
static int GetSourceLabel<T>(IEnumerable<T> source) => source switch
{
    Array _ => 1,     // 弃元替代
    ICollection<T> _ => 2,  
    _ => 3,
};
```

<br>

#### 类型模式

- 使用类型模式检查表达式 `E` 的运行时类型是否与给定类型 `T` 兼容。类型模式的表达式结果 `E` 为非 null 且在运行时类型是 `T` 类型、或可隐式转换类型、或 `T` 的派生类型、或具有基础类型 `T` 的可为 null 的值类型、或存在从 `E` 的运行时类型到类型 `T` 的装箱或取消装箱转换，则模式匹配成功。

> is 表达式

```csharp
var input = Console.ReadLine();
if (input is string)
    Console.WriteLine("INPUT: " + input);
```

> switch 表达式和语句

```csharp
int[] d = null;
Console.WriteLine(GetSourceLabel(d));  // output: -1

var numbers = new int[] { 10, 20, 30 };
Console.WriteLine(GetSourceLabel(numbers));  // output: 1

var letters = new List<char> { 'a', 'b', 'c', 'd' };
Console.WriteLine(GetSourceLabel(letters));  // output: 2
// switch 表达式
static int GetSourceLabel<T>(IEnumerable<T> source) => source switch
{
    Array => 1, // 类型模式
    ICollection<T> => 2,
    null => -1,
    _ => 3,
};
// switch 语句
static int GetSourceLabel<T>(IEnumerable<T> source)
{
    switch (source)
    {
        case Array: return 1;
        case ICollection<T>: return 2;
        case null: return -1;
        default: return 3;
    }
};
```

<br>

#### 常量模式

- 可使用常量模式来测试表达式结果是否等于指定的常量。

```csharp
// is 表达式
var input = Console.ReadLine();
if (input is not (null or "" or "\n"))
    Console.WriteLine("INPUT: "+input);

// switch 表达式
var rt = Console.ReadLine() switch
{
    "\n" => "\\n",
    "" => "Empty",  // 直接回车输入 ""
    string input => input,
    null => "null",     // ctrl+letter 输出 null
};
Console.WriteLine("INPUT: " + rt);

// switch 语句
switch (Console.ReadLine())
{
    case "\n": rt = "\\n"; break;
    case "": rt = "Empty"; break;
    case string input: rt = input; break;
    case null: rt = "null"; break;
}
Console.WriteLine("INPUT: " + rt);
```

<br>

#### 关系模式

- 可使用关系模式将表达式结果与常量进行比较。在关系模式中，可使用关系运算符 `<`、`>`、`<=` 或 `>=` 中的任何一个。关系模式的右侧部分必须是常数表达式。

```csharp
Console.WriteLine(Classify(13));   // output: Too high
Console.WriteLine(Classify(double.NaN));  // output: Unknown
Console.WriteLine(Classify(2.4));  // output: Acceptable

static string Classify(double measurement) => measurement switch
{
    < -4.0 => "Too low",
    > 10.0 => "Too high",
    double.NaN => "Unknown",
    _ => "Acceptable",
};
```

<br>

#### 逻辑模式

- 可使用 `not`、`and` 和 `or` 模式连结符来创建逻辑模式。其中优先级为 `not` > `and` > `or`。

```csharp
Console.WriteLine(Classify(13));    // output: High
Console.WriteLine(Classify(-100));  // output: Too low
Console.WriteLine(Classify(5.7));   // output: Acceptable
static string Classify(double measurement) => measurement switch
{
    < -40.0 => "Too low",
    >= -40.0 and < 0 => "Low",
    >= 0 and < 10.0 => "Acceptable",
    >= 10.0 and < 20.0 => "High",
    >= 20.0 => "Too high",
    double.NaN => "Unknown",
};
```

<br>

#### 属性模式

- 可以使用属性模式将表达式的属性或字段与嵌套模式进行匹配。

```csharp
public record Order(int Items, decimal Cost);

public decimal CalculateDiscount(Order order) => order switch
{
    { Items: > 10, Cost: > 1000.00m } => 0.10m,
    { Items: > 5, Cost: > 500.00m } => 0.05m,
    { Cost: > 250.00m } => 0.02m,
    null => throw new ArgumentNullException(nameof(order), "Can't calculate discount on null order"),
    var someObject => 0m,
};
```

> 嵌套

```csharp
public record Point(int X, int Y);
public record Segment(Point Start, Point End);

static bool IsAnyEndOnXAxis(Segment segment) =>
    segment is { Start: { Y: 0 } } or { End: { Y: 0 } };
// 扩展属性模式：等价行为
static bool IsAnyEndOnXAxis(Segment segment) =>
    segment is { Start.Y: 0 } or { End.Y: 0 };
```

<br>

#### 位置模式

- 可使用位置模式来解构表达式结果并将结果值与相应的嵌套模式匹配。

```csharp
static string Classify(Point point) => point switch
{
    (0, 0) => "Origin",
    ( > 0, 0) => "positive X",
    ( < 0, 0) => "negative X",
    (0, > 0) => "positive Y",
    (0, < 0) => "negative Y",
    _ => "Just a point",
};
public readonly struct Point(int x, int y)
{
    public int X { get; } = x;
    public int Y { get; } = y;
    // Deconstruct 方法用于解构表达式结果
    public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);
}
```

> 在位置模式中使用属性模式

```csharp
var random = new Random(DateTime.Now.Microsecond);
int index = 0;
while (index < 100)
{
    index++;
    var input = new Vector2D(random.NextDouble() * 20 - 10, random.NextDouble() * 20 - 10);
    if (input is Vector2D(> 0, > 0) p)
    {
        if (IsInDomain(p))
            Console.WriteLine(p + " is in Domain"); // 第一象限与半径为 10 的圆的相交区域
    }
}
static bool IsInDomain(Vector2D point) => point is ( > 0, > 0) { magnitude: < 10 };
public record Vector2D(double X, double Y)
{
    public double magnitude => Math.Abs(unchecked(Math.Sqrt(X * X + Y * Y)));
    public override string ToString() => ($"{X:F2}", $"{Y:F2}").ToString();
}
```

<br>

#### var 模式

- 可使用 `var` 模式来匹配任何表达式（包括 `null`），并将其结果分配给新的局部变量。

```csharp
Console.WriteLine(Transform(new Point(1, 2)));  // output: Point { X = -1, Y = 2 }
Console.WriteLine(Transform(new Point(5, 2)));  // output: Point { X = 5, Y = -2 }

static Point Transform(Point point) => point switch
{
    var (x, y) when x < y => new Point(-x, y),
    var (x, y) when x > y => new Point(x, -y),
    (int x, int y) => new Point(x, y),
};
public record Point(int X, int Y);
```

<br>

#### 弃元模式

- 可使用弃元模式 `_` 来匹配任何表达式，包括 `null`。

```csharp
static decimal GetDiscountInPercent(DayOfWeek? dayOfWeek) => dayOfWeek switch
{
    DayOfWeek.Monday => 0.5m,
    DayOfWeek.Tuesday => 12.5m,
    DayOfWeek.Wednesday => 7.5m,
    DayOfWeek.Thursday => 12.5m,
    DayOfWeek.Friday => 5.0m,
    DayOfWeek.Saturday => 2.5m,
    DayOfWeek.Sunday => 2.0m,
    _ => 0.0m,  // 弃元
};
```

<br>

#### 列表模式

- 从 C#11 开始，可以将数组或列表与模式的序列进行匹配。当每个嵌套模式与输入序列的相应元素匹配时，列表模式就会匹配。若要匹配任何元素，可使用弃元模式；若想要捕获元素，可使用 `var` 模式；若要仅匹配输入序列开头或 / 和结尾的元素，可使用切片模式 `..`，切片模式匹配零个或多个元素，最多可在列表模式中使用一个切片模式。

```csharp
Console.WriteLine(new[] { 1, 2, 3, 4, 5 } is [> 0, > 0, ..]);  // True
Console.WriteLine(new[] { 1, 1 } is [_, _, ..]);               // True
Console.WriteLine(new[] { 0, 1, 2, 3, 4 } is [> 0, > 0, ..]);  // False
Console.WriteLine(new[] { 1 } is [1, 2, ..]);                  // False

Console.WriteLine(new[] { 1, 2, 3, 4 } is [.., > 0, > 0]);     // True
Console.WriteLine(new[] { 2, 4 } is [.., > 0, 2, 4]);          // False
Console.WriteLine(new[] { 2, 4 } is [.., 2, 4]);               // True

Console.WriteLine(new[] { 1, 2, 3, 4 } is [>= 0, .., 2 or 4]); // True
Console.WriteLine(new[] { 1, 0, 0, 1 } is [1, 0, .., 0, 1]);   // True
Console.WriteLine(new[] { 1, 0, 1 } is [1, 0, .., 0, 1]);      // False
```

- 可以在切片模式中嵌套子模式。

```csharp
void MatchMessage(string message)
{
    var result = message is ['a' or 'A', .. var s, 'a' or 'A']
        ? $"Message {message} matches; inner part is {s}."
        : $"Message {message} doesn't match.";
    Console.WriteLine(result);
}

MatchMessage("aBBA");  // output: Message aBBA matches; inner part is BB.
MatchMessage("apron");  // output: Message apron doesn't match.

void Validate(int[] numbers)
{
    var result = numbers is [< 0, .. { Length: >= 2 and <= 4 }, > 0] ? "valid" : "not valid";
    Console.WriteLine(result);
}

Validate(new[] { -1, 0, 1 });  // output: not valid
Validate(new[] { -1, 0, 0, 1 });  // output: valid
Validate(new[] { -1, 0, 2, 0, 1 });  // output: valid
```


---
### 运算符重载

- 用户定义类型可以重载一元和二元运算符。相应的复合运算符也会隐式重载。
- 从 C#11 开始，重载 **算术运算符** 时，可以使用 `checked` 关键字定义该运算符的已检查版本。定义已检查的运算符时，还必须定义不带 `checked` 修饰符的相应运算符的未检查版本。

```csharp
record struct Point(double x, double y)
{
    // 二元算数重载
    public static Point operator +(Point lhs, Point rhs) => new Point(lhs.x + rhs.x, lhs.y + rhs.y);
    public static Point operator -(Point lhs, Point rhs) => new Point(lhs.x - rhs.x, lhs.y - rhs.y);
    // 二元 checked - 重载
    public static Point operator checked -(Point lhs, Point rhs) => new Point(lhs.x - rhs.x, lhs.y - rhs.y);

    // 一元重载
    public static Point operator ++(Point lhs) => new Point(++lhs.x, ++lhs.y);
    public static Point operator checked -(Point lhs) => new Point(-lhs.x, -lhs.y);
}
```

<br>

#### 可重载的运算符

- 算数运算符：一元 `++`、`--`、`+`、`-` 和二元 `*`、`/`、`%`、`+`、`-` 算术运算符。
- 逻辑运算符：一元 `!` 和二元 `&`、`|`、`^`。
- 比较运算符：二元 `<` 和 `>`、`<=` 和 `>=`、`==` 和 `!=`，成对的运算符需要同时重载。
- 位运算：一元 `~` 和二元 `&`、`|`、`^`。
- 移位运算符：二元 `<<`、`>>`、`>>>`，C#11 之前右操作数必须为 `int`，C#11 开始重载移位运算符的右侧操作数的类型可以是任意类型。
- 一元 `true` 和 `false` 运算符，只能返回 `bool` 类型。用户类型定义了 `&` 或 `|` 运算符重载时，可以使用相应的条件逻辑运算符 `&&` 或 `||`。

---
### 空合并操作符

- `??` 和 `??=` 空合并运算符在左操作数为 `null` 时，返回右操作数。若左操作数计算结果非 `null` 时，运算符不会计算其右操作数。空合并运算符是右结合运算符。
- `??=` 用于左操作数为 `null` 时将右操作数赋值给左操作数，它的左操作数必须是变量、属性或索引器元素。
- `??` 和 `??=` 运算符的左操作数的类型必须是可以为 `null` 的值类型或引用类型。 

```csharp
int? num = null;
int val = num ?? default;
num ??= default;
```

- 可在包含 `?.` 和 `?[]` 的表达式中，当表达式结果为 `null` 时，可以使用 `??` 运算符来提供替代表达式用于求值。

```csharp
double SumNumbers(List<double[]> setsOfNumbers, int indexOfSetToSum)
    => setsOfNumbers?[indexOfSetToSum]?.Sum() ?? double.NaN;
```

- 可为 null 值类型转换为其基础类型时，`??` 可用于空检查并为基础类型提供默认值。

```csharp
int? num = null;
int _num = num ?? -1;

// 等价于
int _num = num.HasValue ? num.Value : -1;
```

- 可以使用 `throw` 作为 `??` 运算符的右操作符。

```csharp
public string Name
{
    get => name;
    set => name = value ?? throw new ArgumentNullException(nameof(value), "Name cannot be null");
}
```

---
### 空包容运算符

- 一元后缀 `!` 运算符是 `null` 包容运算符或 `null` 抑制运算符。在已启用的可为空的注释上下文中，使用 `null` 包容运算符来取消上述表达式的所有可为 `null` 警告。`null` 包容运算符在运行时不起作用，它仅通过更改表达式的 `null` 状态来影响编译器的静态流分析。

```csharp
#nullable enable
var person_warning = new Person(null);
// Warning CS8625: Cannot convert null literal to non-nullable reference type
var person1_no_warning = new Person(null!);
public class Person(string name)
{
    public string Name  => name ?? throw new ArgumentNullException(nameof(name));
}
```

> NotNullWhen

- 使用 `NotNullWhen` 属性告知编译器，当方法返回 `true` 时，`IsValid` 方法的参数不能是 `null`。

```csharp
#nullable enable
using System.Diagnostics.CodeAnalysis;
class Sample
{
    public static void Main()
    {
        Person? p = Find("John"); ;
        if (IsValid(p))
            Console.WriteLine($"Found {p.Name}");
    }
    public static bool IsValid([NotNullWhen(true)] Person? person)
    //public static bool IsValid(Person? person)  // p.Name: Dereference of a possibly null reference
        => person is not null && person.Name is not null;
    public class Person(string name)
    {
        public string Name => name ?? throw new ArgumentNullException(nameof(name));
    }
    static Person? Find(string name)
        => name is not null ? new Person(name) : null;
}
```

---
### sizeof 和 nameof 运算符

- `sizeof` 运算符返回给定类型的变量所占用的字节数。`sizeof` 运算符的参数必须是一个非托管类型的名称，或是一个限定为非托管类型的类型参数。`sizeof` 运算符需要不安全上下文，除非类型是已知预定义大小内置类型 `sbyte`、`byte`、`short`、`ushort`、`int`、`uint`、`long`、`ulong`、`char`、`float`、`double`、`decimal`、`bool`、枚举类型的名称。

```csharp
Console.WriteLine(Point.TypeSize);
record struct Point(int x, int y)
{
    public static unsafe int TypeSize => sizeof(Point);
}
```

- `nameof` 表达式可生成变量、类型或成员的名称作为字符串常量。`nameof` 表达式在编译时进行求值，在运行时无效。 

```csharp
Sample TestClass = null;
Console.WriteLine(TestClass.NameOf());  // TestClass

class Sample;
public static class Ext
{
    // 获取变量的名称
    public static string NameOf(this object obj, 
    [System.Runtime.CompilerServices.CallerArgumentExpression("obj")] string? name  = null) => name;
    // 获取变量类型的大小
    public unsafe static int Sizeof<T>(this T obj) => sizeof(T);
}
```

---
### stackalloc 表达式

- `stackalloc` 表达式在堆栈上分配内存块，在表达式 `stackalloc T[E]` 中，`T` 必须是非托管类型。使用 `stackalloc` 会自动启用公共语言运行时 CLR 中的缓冲区溢出检测功能。
- 在方法返回时，将自动丢弃在方法执行期间创建的堆栈中分配的内存块。不能显式释放使用 `stackalloc` 分配的内存。堆栈中分配的内存块不受垃圾回收的影响，也不必通过 `fixed` 语句固定。

* 将 `stackalloc` 表达式的结果分配给 `System.Span<T>` 或 `System.ReadOnlySpan<T>` 类型时，可以不使用 `unsafe` 上下文。建议尽可能使用 `Span<T>` 或 `ReadOnlySpan<T>` 类型来处理堆栈中分配的内存

```csharp
int length = 1000;
Span<byte> buffer = length <= 1024 ? stackalloc byte[length] : new byte[length];

ReadOnlySpan<int> numbers = stackalloc[] { 1, 2, 3, 4, 5, 6 };
var ind = numbers.IndexOfAny(stackalloc[] { 2, 4, 6, 8 });
Console.WriteLine(ind);  // output: 1
```

- 可以将 `stackalloc` 表达式的结果分配给指针类型，必须使用 `unsafe` 上下文。

```csharp
unsafe
{
    int length = 3;
    int* numbers = stackalloc int[length];
    for (var i = 0; i < length; i++)
        numbers[i] = i;
}
```

- 堆栈上可用的内存量存在限制，如果在堆栈上分配过多的内存，会引发 `StackOverflowException`。限制使用 `stackalloc` 分配的内存量，如果预期的缓冲区大小低于特定限制，则在堆栈上分配内存；否则，使用所需长度的数组。避免在循环内使用 `stackalloc`。在循环外分配内存块，然后在循环内重用它。

```csharp
const int MaxStackLimit = 1024;
Span<byte> buffer = inputLength <= MaxStackLimit ? stackalloc byte[MaxStackLimit] : new byte[inputLength];
```

---