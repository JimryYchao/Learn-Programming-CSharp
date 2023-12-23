# CSharp Expressions

表达式是运算符和操作数的序列，该子句定义了语法、操作数和运算符的求值顺序以及表达式的含义。

---
## 1. 静态绑定和动态绑定

*绑定（Binding）* 是根据表达式的类型或值（参数、操作数、接收方）来确定一个操作含义的过程。例如，方法调用的绑定是根据接收方和参数的类型确定的。运算符的绑定是根据其操作数的类型确定的。

在 C# 中，操作的绑定通常是在编译时根据其子表达式的编译时类型确定的。如果表达式包含错误，编译器会检测并报告错误。这种方法被称为 *静态绑定*。

如果表达式是动态表达式（即操作中包含有 `dynamic` 类型），这表明它参与的任何绑定都应该基于其运行时类型，而不是编译时的类型。这种操作的绑定被延迟到程序运行期间执行该操作的时间。这被称为 *动态绑定*。

当一个操作被动态绑定时，编译器通常不会执行类型检查。如果运行时绑定失败，则在运行时抛出异常。

C# 中服从绑定的操作有成员访问、方法调用、委托调用、元素访问、对象创建、重载运算符操作、赋值运算、隐式和显式转换。当不涉及动态表达式时，C# 默认为静态绑定。若其中一个构成的表达式为动态表达式时，将该操作视为动态绑定。

如果方法调用是动态绑定的，并且任何参数（包括接收方）都有 `in` 修饰符，则会导致编译时错误，`in` 参数不能用于动态绑定的表达式。

```csharp
object  o = 5;
dynamic d = 5;

Console.WriteLine(5);  // static  binding to Console.WriteLine(int)  编译时选择重载
Console.WriteLine(o);  // static  binding to Console.WriteLine(object)  编译时选择重载
Console.WriteLine(d);  // dynamic binding to Console.WriteLine(int)  运行时选择重载
```

>---

### 1.1. 动态绑定

动态绑定允许 C# 程序与 *动态对象* 交互，包括 C# 类型对象和不遵循 C# 类型系统常规规则的对象。也就是说，动态对象可以是具有不同类型系统的其他编程语言中的对象，也可以是以编程方式实现不同操作的绑定语义的对象。

动态对象实现其自身语义的机制是实现定义的。给定的接口由动态对象实现，以向 C# 运行时发出信号，表明它们具有特殊的语义。每当动态对象上的操作被动态绑定时，它们将接管自己的绑定语义而不是使用 C# 规范中指定的操作。

>---

### 1.2. 子表达式的类型

当一个操作被静态绑定时，子表达式的类型（例如，接收者、参数、索引或操作数）总是被认为是该表达式的编译时类型。

当动态绑定操作时，根据子表达式的编译时类型确定子表达式的类型：
- 编译时类型为 `dynamic` 的子表达式被认为具有该表达式在运行时计算得到的实际值的类型。
- 编译时类型是类型参数的子表达式，被认为具有该类型参数在运行时绑定到的类型。
- 否则，子表达式被认为具有其编译时类型。

---
## 2. 运算符

表达式有操作数和运算符构造，表达式的运算符指示对操作数应用哪些操作。包含三种类型的运算符：
- 一元运算符：接受一个操作数，并使用前缀（`-x`）或后缀表示法（`x++`）。
- 二元运算符：接受两个操作数，并且都是用中辍表示法（`x + y`）。
- 三元运算符：仅有一个三元运算符 `?:`，使用中辍表示法（`c?x:y`）。

表达式中运算符的求值顺序由运算符的优先级和结合性决定。

某些运算符可以被重载。运算符重载允许为其中一个或两个操作数为用户定义的类或结构类型指定用户定义的运算符实现。

>---

### 2.1. 运算符优先级和结合性

优先级从高到底的顺序列举 C# 支持的运算符：

| **Category**                     | **Operators**                                          |
| -------------------------------  | -------------------------------------------------------|
| 基本表达式                          | `x.y` `x?.y` `f(x)` `a[x]` `a?[x]` `x++` `x--` `x!` `new` `typeof` `default` `checked` `unchecked` `delegate` `stackalloc` `nameof` `sizeof` `x->y` |
| 一元                            | `+x` `-x` `!x` `~x` `++x` `--x` `(T)x` `await x` `&x` `*x` `true` `false`| 
|索引与范围 | `^n` `a..b` |
|`switch` 和 `with` 表达式|  `switch {...}` `with {...}` |
| 乘法                   | `x * y` `x / y` `x % y` |
| 加法                         | `x + y` `x - y` |
| 移位                            | `x << y` `x >> y` `x >>> y` |
| 关系和类型测试      | `x < y` `x > y` `x <= y` `x >= y` `is` `as` |
| 相等                      | `x == y` `x != y` |
| 布尔逻辑或按位逻辑 AND                      | `x & y`  |
| 布尔逻辑或按位逻辑 XOR              | `x ^ y`  |
| 布尔逻辑或按位逻辑 OR                       | `x \| y`  |
| 条件 AND                  | `x && y`  |
| 条件 OR                   | `x \|\| y`  |
| 空合并运算或 `throw` 表达式                  | `x ?? y`  `throw x`  |
| 条件运算符                      | `?:`   |
| 赋值与 Lambda 声明 | `x = y` `x = ref y` `x *= y` `x /= y ` `x %= y` `x += y` `x -= y` `x <<= y` `x >>= y` `x >>>= y` `x &= y` `x ^= y` `x \|= y` `=>`   |

当操作数出现在两个具有相同优先级的运算符之间时，运算符的结合性控制运算符的执行顺序：
- 除了赋值运算和空合并运算外，所有的二元运算符都是左结合性的，即操作从左到右执行。如 `a + b - c` 将计算 `(a + b) - c`。
- 右结合运算符从右往左的顺序计算。赋值运算符、null 合并运算符、lambda `=>` 和条件运算符 `?:` 是右结合运算符。 例如，`x = y = z` 将计算为 `x = (y = z)`。

使用括号更改运算符的结合性和优先级：

```csharp
int a = 13 / 5 / 2;
int b = 13 / (5 / 2);
Console.WriteLine($"a = {a}, b = {b}");  // output: a = 1, b = 6
```

>---

### 2.2. 运算符重载

用户定义类型可以重载一元和二元运算符。相应的复合运算符也会隐式重载。

从 C#11 开始，重载 **算术运算符** 时，可以使用 `checked` 关键字定义该运算符的已检查版本。定义已检查的运算符时，还必须定义不带 `checked` 修饰符的相应运算符的未检查版本。

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

#### 2.2.1. 可重载的运算符

- 算数运算符：一元 `++`、`--`、`+`、`-` 和二元 `*`、`/`、`%`、`+`、`-` 算术运算符。
- 逻辑运算符：一元 `!` 和二元 `&`、`|`、`^`。
- 比较运算符：二元 `<` 和 `>`、`<=` 和 `>=`、`==` 和 `!=`，成对的运算符需要同时重载。
- 位运算：一元 `~` 和二元 `&`、`|`、`^`。
- 移位运算符：二元 `<<`、`>>`、`>>>`，C#11 之前右操作数必须为 `int`，C#11 开始重载移位运算符的右侧操作数的类型可以是任意类型。
- 一元 `true` 和 `false` 运算符，只能返回 `bool` 类型。用户类型定义了 `&` 或 `|` 运算符重载时，可以使用相应的条件逻辑运算符 `&&` 或 `||`。

---
## 3. 基本表达式

基本表达式包含一些简单形式的表达式。

```ANTLR
primary_expression
    : primary_no_array_creation_expression
    | array_creation_expression
    ;

primary_no_array_creation_expression
    : literal
    | interpolated_string_expression
    | simple_name
    | parenthesized_expression
    | tuple_expression      
    | member_access
    | null_conditional_member_access
    | invocation_expression
    | element_access
    | null_conditional_element_access
    | this_access
    | base_access
    | post_increment_expression
    | post_decrement_expression
    | object_creation_expression
    | delegate_creation_expression
    | anonymous_object_creation_expression
    | typeof_expression
    | sizeof_expression
    | checked_expression
    | unchecked_expression
    | default_value_expression
    | nameof_expression    
    | anonymous_method_expression
    | pointer_member_access     // unsafe code support
    | pointer_element_access    // unsafe code support
    | stackalloc_expression
    ;
```

>---

### 3.1. 文本

由文本组成的表达式归类为值，是值的源代码表示形式。文本字面值包含布尔值 `true` 和 `false`、整数和实数字面值、字符、字符串、`null`。

```csharp
bool b = true;
int num = 10;
float f = 1.0f;
char c = 'a';
string str = "Hello";
object obj = null;
```

>---

### 3.2. 内插字符串

内插字符串由 `$`、`@$` 或 `$@` 作为前缀，后接字符串字面值。在文本中，包含零个到多个内插表达式 `{ expr }`，每个插入包含一个表达式和一个可选的格式规范。

将内插字符串解析为结果字符串时，带有内插表达式的项会替换为表达式结果的字符串表示形式。

内插字符串初始化常量时，所有的内插表达式也必须是常量字符串。C#11 起内插表达式支持使用换行符，以使表达式更具有可读性。

```csharp
$"{<interpolationExpression>[,<alignment>][:<formatString>]}"
// - interpolationExpression  生成需要设置格式的结果的表达式
// - alignment                int 常数表达式，定义对齐方式和最小字符宽度，负值表示左对齐，正值表示右对齐
// - formatString             受表达式结果类型支持的格式字符串，例如 DateTime 格式化输出

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

>---

### 3.3. 成员访问

- `.`（成员访问）：用于访问命名空间、类型、对象的成员。
- `[]`（数组元素或索引器访问）：用于访问数组元素或类型索引器。
- `?.` 和 `?[]`（`null` 条件访问）：仅当操作数为非 `null` 时才用于执行成员或元素访问运算，否则返回 `null`。
- `()`（方法调用）：用于调用方法成员、扩展方法或调用委托对象。

```csharp
// . 成员访问
System.Console.WriteLine("Hello, World");

// [] 数组或索引器元素访问
int[] arr = [1, 2, 3, 4, 5, 6, 7];
arr[0] = 1000;

// ?. ?[] Null 条件访问 
double SumNumbers(List<double[]> setsOfNumbers, int indexOfSetToSum)
    => setsOfNumbers?[indexOfSetToSum]?.Sum() ?? double.NaN;
var sum = SumNumbers(null, 0);
Console.WriteLine(sum);  // output: NaN

// () 方法或委托调用
var Print = (string mess) => Console.WriteLine("PRINT : " + mess);
Print("Hello, World");
```

>---

### 3.4. this 访问

只能在类或结构的实例构造函数、实例方法、实例访问器或终结器的块中使用 `this`。接口提供实现的实例成员块中也可以使用 `this`。`this` 有以下含义之一：

- 当 `this` 在类的实例构造函数中的表达式中使用时，它被归类为一个值。值的类型是发生使用的类的实例类型，且是对正在构造的对象的引用。

- 当 `this` 在类的实例方法或实例访问器中的表达式中使用时，它被归类为一个值。该值的类型是发生使用的类的实例类型，且是对调用该方法或访问器的对象的引用。
  
- 当 `this` 在结构的实例构造函数中的表达式中使用时，它被归类为变量。变量的类型是发生使用的结构的实例类型，且表示正在构造的结构。如果构造函数声明没有构造函数初始化式，则 `this` 变量的行为与结构类型的 `out` 形参完全相同。这意味着变量必须在实例构造函数的每个执行路径中被明确地赋值。

- 当 `this` 用于结构的实例方法或实例访问器中的表达式中使用时，它会归类为变量。变量的类型是发生使用的结构的实例类型。如果方法或访问器不是迭代器，则该 `this` 变量表示调用了方法或访问器的结构，其行为与 `ref` 结构类型的参数完全相同。否则认为该 `this` 变量表示为其调用了方法或访问器的结构的副本，其行为与结构类型的值参数完全相同。

- 类的 `this` 可以在匿名函数中使用，匿名函数将捕获对类实例对象的引用；结构的 `this` 无法在匿名函数或局部函数中使用，可以在外部范围内先声明一个局部变量并分配 `this`。

- `this` 不能在静态方法、静态属性访问器或字段声明的变量初始化值中引用。

```csharp
class Sample
{
    int Value;
    public Sample(int Value) => this.Value = Value;
}
```

>---

### 3.5. base 访问

`base` 通常用于访问当前类或结构中类似命名的被隐藏的基类成员。只能在实例构造函数、实例方法、实例访问器、终结器的块中使用 `base`。在类中充当 `base` 基类对象的引用，对于结构类型则是对其直接基类 `System.ValueType` 的引用。 

当 `base` 引用虚函数成员时，在运行时通过查找函数成员的最派生实现来确定的（非 `this`）。在虚函数成员的重写方法中，可以使用 `base` 调用该函数成员的继承实现。如果 `base` 引用的函数成员是抽象的，则会发生绑定时错误。

```csharp
class Base
{
    public virtual void Fun(string mess) => Console.WriteLine(mess);
}
class Sample : Base
{
    public override void Fun(string mess)
    {
        base.Fun("Output: " + mess);
    }
}
```

>---

### 3.6. new 运算符

`new` 运算符用于创建类型的新实例，主要是：
- 对象创建表达式用于创建类或值类型的新实例。
- 数组创建表达式用于创建数组类型的新实例。
- 委托创建表达式用于创建委托类型的新实例。
- 匿名对象创建表达式用于构造匿名类型的新实例。

#### 3.6.1. 对象创建

```ANTLR
object_creation_expression
    : new <TypeName> (<argument_list>?) <object_or_collection_initializer>?
    | new <TypeName> object_or_collection_initializer
    ;

object_or_collection_initializer
    : { member_initializer_list? }    
```

对象创建表达式的类型必须是类类型、结构类型、类或结构的构造类型、`using` 声明类型，不能是元组表达式、抽象或静态类。

参数列表仅在类型是类或结构类型时可用（调用对应参数签名的构造函数）。对象创建表达式可以省略构造函数参数列表和括号，前提是它包含对象初始值设定项。

```csharp
using Point = (int x, int y);
class Sample
{
    public int x { get; set; }
    public int y { get; set; }
    public Sample() { }
    public Sample(int x, int y) { }

    static void Main(string[] args)
    {
        // 对象创建表达式
        Sample s1 = new Sample();
        Sample s2 = new();
        Point p1 = new Point();
        Point p2 = new();

        // 使用参数列表的对象创建
        Sample s3 = new Sample(x: 1, y: 2);
        Sample s4 = new(1, 2);
        Point p3 = new(1, 2);


    }
}
class Rectangle
{
    public Point P1 { get; } = new Point();
    public Point P2 { get; } = new Point();
}
```

#### 3.6.2. 对象初始值设定项

对象初始值设定项为对象的零个或多个可访问的字段、属性、索引元素指定值。

```csharp
using Point = (int x, int y);
class Sample
{
    public int x { get; set; }
    public int y { get; set; }
    static void Main(string[] args)
    {
        // 使用对象初始化设定项的对象创建
        Sample s5 = new Sample { x = 1, y = 2 };
        Sample s6 = new() { x = 1, y = 2 };
        Point p4 = new() { x = 1, y = 2 };
        Point p5 = new Point { x = 1, y = 2 };

        // 使用索引器
        Dictionary<string, int> Persons = new Dictionary<string, int>
        {
            ["Tom"] = 10,
            ["Mary"] = 13,
            ["Hello"] = 5,
            ["World"] = 12
        };
    }
}
```

`required` 修饰的字段或属性，强制调用方在创建对象时使用对象初始值设定项设置这些 `required` 的值。

```csharp
var person = new Person { FirstName = "Joe", LastName = "Doe" };
public class Person
{
    public required string FirstName { get; set; }
    public required string LastName;
}
```

具有 `init` 访问器的属性，可以在构造函数或对象初始值设定项中设置它们的值。

```csharp
var person = new Person { FirstName = "Joe", LastName = "Doe" };
public class Person
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
}
```


#### 3.6.3. 集合初始值设定项

集合初始化项指定集合的元素，集合对象必须是实现 `IEnumerable` 的类型，且包含一个可访问的 `Add` 实例方法或扩展方法。

集合初始值设定项允许指定一个或多个元素初始值设定项。元素初始值设定项可以是简单的值、表达式或对象初始值设定项。

```csharp
List<int> digits = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };  
Dictionary<string, int> person = new Dictionary<string, int>
{
    { "Tom",10 },
    {"Mary", 13 },
    {"Hello", 5 },
    {"World", 12 }
};

// 用户定义集合类型
SampleList<int> arr = new SampleList<int> { 1, 2, 3, 4, 5 };
class SampleList<T> : IEnumerable<T>
{
    List<T> list = new List<T>(80);
    public void Add(T val) => list.Add(val);
    public IEnumerator<T> GetEnumerator() => list.AsEnumerable().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();
}
```

具有集合只读属性初始化的对象初始值设定项，由于无法为属性分配新列表，但是可以使用省略列表创建。

```csharp
Persons owner = new Persons
{
    Names = // new List<string> 省略创建
    {
        "Tom", "Jerry", "Hello", "World"
    }
};
public class Persons
{
    public IList<string> Names { get; } = new List<string>();
}
```

#### 3.6.4. 委托创建

```ANTLR
delegate_creation_expression
    : new delegate_type ( <expression> )
    ;
```

委托创建表达式的参数应该是一个方法组、一个匿名函数或编译时类型 `dynamic` 或委托类型的值。

委托的调用列表在实例化委托时确定，然后在委托的整个生命周期内保持不变。一旦创建了委托，就不可能更改委托的可调用实体 `Target`。当两个委托合并或从另一个委托中删除一个委托时，将产生一个新的委托，现有委托的内容没有更改。

不能创建引用属性、索引器、用户定义运算符、实例构造函数、终结器或静态构造函数的委托。 

```csharp
class Sample
{
    delegate double DoubleFunc(double x);
    DoubleFunc f = new DoubleFunc(Square);
    static double Square(double x) => x * x;
}
```

#### 3.6.5. 匿名类型创建

```ANTLR
anonymous_object_creation_expression
    : new { member_declarator_list? } 
    ;
```

匿名对象创建表达式声明匿名类型并返回该类型的实例。匿名类型是直接从对象继承的无名类类型。匿名类型的成员是从用于创建该类型实例的匿名对象初始化设定项中推断出的只读属性序列 `new {p1 = e1, p2 = e2, ... , pv = ev }`。

无法将 `null` 或匿名函数分配给匿名类型的只读属性。在同一个程序中，两个匿名对象初始化设定项以相同的顺序指定具有相同名称和编译时类型的属性序列，将产生相同匿名类型的实例。

```csharp
var p1 = new { Name = "Lawnmower", Price = 495.00 };
var p2 = new { Name = "Shovel", Price = 26.95 };
p1 = p2;
```

#### 3.6.6. 数组的创建

```csharp
// 单维数组
int[] arrA = new int[] { 1, 2, 3, 4, 5, 6 };
var arrB = new int[3] { 1, 2, 3 };
var arrC = new[]{ 1, 2, 3 };
int[] arrD = { 1, 2, 3 };

// 二维数组
int[,] arr2A = new int[,] { { 1, 2 }, { 3, 4 } };
var arr2B = new int[2, 2] { { 1, 2 }, { 3, 4 } };
var arr3C = new[,]{ { 1, 2 }, { 3, 4 } };
int[,] arr3D = { { 1, 2 }, { 3, 4 } };

// 交错数组
var arr3A = new int[2][] { new int[] { 1, 2 }, new int[] { 3, 4, 5, 6 } };
int[][] arr3B = { new[] { 1, 2 }, new int[] { 3, 4, 5, 6 } };

// 数组元素为数组的二维数组
int[,][] arr4 = new int[2, 2][] 
{
    { new int[] {1,2 }, new int[] {0 } } ,
    { new int[] {1,2,3 }, new int[] {0,0,0,0 } }
};

// 数组元素为二维数组的数组
int[][,] arr5 = new int[2][,]
{
    new int[,]{ {1,2 },{3,4 },{5,6 } },
    new int[,]{ {1,1,1 },{2,2,2 } }
};
```

> 匿名类型的数组

```csharp
var arr = new[] {
    new{ name = "Hello", Id = 1 },
    new{ name = "World", Id = 2 },
    new{ name = "Empty", Id = 3 },
};
```

>---

### 3.7. typeof 运算符

`typeof` 操作符用于获取 `System.Type` 的类型对象。运算符的参数可以是类型名称、未绑定的类型名称、`void`，不能是 `dynamic` 类型。`System.Type` 与 `System.Reflection` 中的类能够用来获取有关加载的程序集、`Type` 对象关联类型等的相关信息。

`typeof` 运算符可用于类型参数，结果是绑定到类型参数的运行时类型的 `System.Type`的类型对象。`typeof` 也可用于构造类型会未绑定的泛型类型，其中未绑定泛型对象的 `Type` 类型对象和实例类型的 `Type` 对象不同，实例类型在运行时始终是封闭式构造类型，它的 `Type` 对象依赖于正在使用的运行时类型参数。未绑定的泛型类型没有类型参数。

`object.GetType()` 方法也可以用来获取对象运行时类型的 `Type` 对象。

```csharp
class Sample<T>
{
    public static void PrintTypes()
    {
        Type[] t = {
            typeof(int),                     // System.Int32
            typeof(System.Int32),            // System.Int32
            typeof(string),                  // System.String
            typeof(double[]),                // System.Double[]
            typeof(void),                    // System.Void
            typeof(T),                       // System.Int32
            typeof(Sample<T>),               // Sample`1[System.Int32]
            typeof(Sample<Sample<T>>),       // Sample`1[Sample`1[System.Int32]]
            typeof(Sample<>)                 // Sample`1[T] 
        };
        for (int i = 0; i < t.Length; i++)
            Console.WriteLine(t[i]);
    }
}
```

>---

### 3.8. sizeof 运算符

`sizeof` 运算符用于返回给定类型的变量所占用的 8 位字节数。`sizeof` 运算符的参数通常是一个非托管 `unmanaged` 类型的名称，或是一个限定为非托管类型的类型参数。

对于预定义大小内置类型 `sbyte`、`byte`、`short`、`ushort`、`int`、`uint`、`long`、`ulong`、`char`、`float`、`double`、`decimal`、`bool`，`sizeof` 运算符产生一个常量值。

对于枚举类型 `E`，表达式 `sizeof(E)` 的结果是一个常数值，等于枚举基础类型的大小。

对于所有其他类型，`sizeof` 运算符的结果是实现定义的，并且被归为变量。由于本机整数类型 `nint` 和 `nuint`、任意指针类型、任意托管类型（相当于是指针）、用户定义结构、可空值类型没有预定义的大小，因此 `sizeof` 只能在不安全的上下文中使用。

结构类型的成员的打包顺序未指定，出于对齐的目的，可以在结构体的开头、内部、末尾进行未命名的填充，用作填充的位的内容是不确定的。当 `sizeof` 应用到 `struct` 类型的操作数时，其结果是该类型变量的总字节数，并包括填充在内。


```csharp
Sample<int>.PrintSizes();
struct Sample<T>()
{
    int value = 0;
    int value1 = 0;
    int value2 = 0;
    public unsafe static void PrintSizes()
    {
        int[] sizes = {
            sizeof(sbyte),    sizeof(sbyte?),   // 1,2
            sizeof(byte),     sizeof(byte?),    // 1,2
            sizeof(short),    sizeof(short?),   // 2,4
            sizeof(ushort),   sizeof(ushort?),  // 2,4
            sizeof(int),      sizeof(int?),     // 4,8
            sizeof(uint),     sizeof(uint?),    // 4,8
            sizeof(long),     sizeof(long?),    // 8,16
            sizeof(ulong),    sizeof(ulong?),   // 8,16
            sizeof(float),    sizeof(float?),   // 4,8
            sizeof(double),   sizeof(double?),  // 8,16
            sizeof(decimal),  sizeof(decimal?), // 16,24
            sizeof(bool),     sizeof(bool?),    // 1,2
            sizeof(char),     sizeof(char?),    // 2,4
            sizeof(AttributeTargets),    sizeof(AttributeTargets?),  // 4,8
            sizeof(nint),            sizeof(nint?),       // 8,16
            sizeof(nuint),           sizeof(nuint?),      // 8,16
            sizeof(dynamic),         sizeof(dynamic?),    // 8,8
            sizeof(object),          sizeof(object),      // 8,8
            sizeof(string),          sizeof(string),      // 8,8
            sizeof(int[]),           sizeof(int[]),       // 8,8
            sizeof(int[][]),         sizeof(int[][]),     // 8,8
            sizeof(double[]),        sizeof(double[]),    // 8,8
            sizeof(double[,]),       sizeof(double[,]),   // 8,8
            sizeof(Sample<T>),       sizeof(Sample<T>?),  // 12,16
            sizeof(Sample<T?>),      sizeof(Sample<T?>?), // 12,16
            sizeof(Sample<Sample<T>>),      // 12
            sizeof(void*),                  // 8
            sizeof(int*),              sizeof(int?*),     // 8,8
            sizeof(double*),           sizeof(double?*),  // 8,8
            sizeof(decimal*),          sizeof(decimal?*), // 8,8
            sizeof(string*),           sizeof(object*),   // 8,8
            sizeof(delegate* <void>),                   // 8
            sizeof(delegate* unmanaged<void>),          // 8
            sizeof(delegate* managed<int, void>),       // 8
            sizeof(delegate* unmanaged<int, void>)      // 8
        };
        for (int i = 0; i < sizes.Length; i++)
            Console.Write(sizes[i] + ",");
    }
}
```

>---

### 3.9. nameof 表达式

`nameof` 表达式用于以常量字符串的形式获取程序实体（可以是命名空间、类型名称、成员名称、变量名、类型参数名称、标识符等程序实体）的名称。`nameof` 表达式在运行时没有作用，它的值是命名实体的最后一个标识符，因此它的程序实体参数不能求值，不能是表达式。

`nameof` 表达式在编译时对实体执行简单的名称和成员访问查找。若实体是方法组，则不需要也不能指定类型参数；命名实体也不能是动态类型的表达式，可以是动态类型的变量标识符；不能是 C# 关键字。

若标识符中包含 `@` 前缀，将被删除；每个 Unicode 序列也被转换成对应的 Unicode 字符；删除任何格式化字符串。

```csharp
using TestAlias = System.String;
class Program
{
    static void Main()
    {
        var point = (x: 3, y: 4);

        string n1 = nameof(System);                      // "System"
        string n2 = nameof(System.Collections.Generic);  // "Generic"
        string n3 = nameof(point);                       // "point"
        string n4 = nameof(point.x);                     // "x"
        string n5 = nameof(Program);                     // "Program"
        string n6 = nameof(System.Int32);                // "Int32"
        string n7 = nameof(TestAlias);                   // "TestAlias"
        string n8 = nameof(List<int>);                   // "List"
        string n9 = nameof(Program.InstanceMethod);      // "InstanceMethod"
        string n10 = nameof(Program.GenericMethod);      // "GenericMethod"
        string n11 = nameof(Program.NestedClass);        // "NestedClass"

        // Invalid
        // string x1 = nameof(List<>);            // Empty type argument list
        // string x2 = nameof(List<T>);           // T is not in scope
        // string x3 = nameof(GenericMethod<>);   // Empty type argument list
        // string x4 = nameof(GenericMethod<T>);  // T is not in scope
        // string x5 = nameof(int);               // Keywords not permitted
        // Type arguments not permitted for method group
        // string x6 = nameof(GenericMethod<Program>);
    }

    void InstanceMethod() { }
    void GenericMethod<T>()
    {
        string n1 = nameof(List<T>); // "List"
        string n2 = nameof(T);       // "T"
    }
    class NestedClass : Program;
}
```

从 C#11 起，`nameof` 表达式允许在方法或参数的特性中使用 `nameof(parameter)`：

```csharp
[MyAttribute(nameof(parameter))] void M(int parameter) { }  // 方法参数

[MyAttribute(nameof(TParameter))] void M<TParameter>() { }  // 方法类型参数

void M(int parameter, [MyAttribute(nameof(parameter))] int other) { }  // 参数特性
```

>---

### 3.10. 空合并运算符

`??` 和 `??=` 空合并运算符在左操作数为 `null` 时，返回右操作数。若左操作数计算结果非 `null` 时，运算符不会计算其右操作数。空合并运算符是右结合运算符。

`??=` 用于左操作数为 `null` 时将右操作数赋值给左操作数，它的左操作数必须是变量、属性或索引器元素，类型必须是可以为 `null` 的值类型或引用类型。 

```csharp
int? num = null;
int val = num ?? default;
num ??= default;
```

可在包含 `?.` 和 `?[]` 的表达式中，当表达式结果为 `null` 时，可以使用 `??` 运算符来提供替代表达式用于求值。

```csharp
double SumNumbers(List<double[]> setsOfNumbers, int indexOfSetToSum)
    => setsOfNumbers?[indexOfSetToSum]?.Sum() ?? double.NaN;
```

可为 null 值类型转换为其基础类型时，`??` 可用于空检查并为基础类型提供默认值。

```csharp
int? num = null;
int _num = num ?? -1;

// 等价于
int _num = num.HasValue ? num.Value : -1;
```

可以使用 `throw` 作为 `??` 运算符的右操作符。

```csharp
public string Name
{
    get => name;
    set => name = value ?? throw new ArgumentNullException(nameof(value), "Name cannot be null");
}
```

>---

### 3.11. 弃元

弃元（`_`）是一种在应用程序代码中人为取消使用的占位符变量，相当于未赋值的变量，但是它们没有值。弃元将意图传达给编译器和其他读取代码的文件：用户打算忽略表达式的结果。可以使用弃元用来忽略表达式的结果、元组表达式的一个或多个成员、方法的 `out` 参数或模式匹配表达式的目标。

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

- 从 C#9 开始，可以使用弃元指定 Lambda 表达式或匿名函数中不使用的两个或更多输入参数。

```csharp
Func<int, int, int, int> Func = (_, _, val) => val * val;
var Func2 = delegate (int _, int _, int val) { return val * val; };
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

>---

### 3.12. 空包容运算符

一元后缀 `!` 运算符是 `null` 包容运算符或 `null` 抑制运算符。在已启用的可为空的注释上下文中，使用 `null` 包容运算符来取消上述表达式的所有可为 `null` 警告。`null` 包容运算符在运行时不起作用，它仅通过更改表达式的 `null` 状态来影响编译器的静态流分析。

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

>---

### 3.13. checked 和 unchecked 运算符

`checked` 和 `unchecked` 运算符用于控制整型算数和转换的溢出检查上下文，可以是表达式或语句两种形式。运算符这会对计算结果进行是否溢出检查，而不会对函数调用有影响。

```ANTLR
checked
    : checked ( expr )  
    | checked { <block> }
    ;
unchecked
    : unchecked ( expr )  
    | unchecked { <block> }
    ;
```

在整型算数和转换的过程中，当产生的结果太大而无法在目标整数类型中表示时：
- 在 `checked` 检查的上下文中，如果操作是常量表达式，则会发生编译时错误；否则，在运行时执行该操作时，会引发 `System.OverflowException` 异常。
- 在未检查的上下文中，通过丢弃不适合目标类型的任何高阶位来截断结果。

对于非常量表达式的运算，没有任何 `checked` 或 `unchecked` 的运算符或语句包围时，默认是不检查溢出的，除非是外部因素（例如编译器开关或执行环境配置等）调用检查。

对于常量表达式的运算，默认始终检查溢出，除非在 `unchecked` 上下文中。

对于以提升或本身表达式是浮点类型，无论是 `checked` 还是 `unchecked` 的上下文，都不会对浮点类型的结果进行溢出检查。

```csharp
class Sample
{
    static readonly int x = 1000000;
    static readonly int y = 1000000;

    const int cx = 1000000;
    const int cy = 1000000;
    static void Main(string[] args)
    {
        var rt1 = checked(x * y);     // throw Overflow
        var rt2 = unchecked(x * y);   // rt = -727379968
        var rt3 = x * y;              // 取决于是否检查的默认值

        var rt4 = checked(cx * cy);   // 编译时错误，overflow
        var rt5 = unchecked(cx * cy); // rt = -727379968 
        var rt6 = cx * cy;            // 编译时错误，overflow
    }
}
```

>---

### 3.14. default 表达式

`default` 表达式用于获取类型的默认值。表达式的结果是显式类型的默认值，或者是指定目标类型的默认值。当类型是简单值类型、任何枚举类型、已知引用类型的类型参数或引用类型时，`default` 表达式是一个常量表达时。

```csharp
Sample<String> S1 = new();
Console.WriteLine(S1.Default is null);  // True
Console.WriteLine(S1.Value);     // 0

Sample<double> S2 = new();  
Console.WriteLine(S2.Default);   // 0

Sample<int?> S3 = new();
Console.WriteLine(S3.Default.HasValue);  // False

class Sample<T>
{
    public T Default => default(T);
    public int Value { get; set; } = default;
}
```

>---

### 3.15. stackalloc 表达式

堆栈分配表达式从执行堆栈中分配一块内存。执行堆栈是存储局部变量的内存区域，不是托管堆的一部分。当当前函数返回时，用于本地变量存储的内存将自动恢复。

```ANTLR
stackalloc_expression
    : stackalloc unmanaged_type [ expression ] <{ stackalloc_initializer }>?
    | stackalloc [] { stackalloc_initializer }
```

`stackalloc` 表达式在堆栈上分配内存块，在表达式 `stackalloc T[E]` 中，`T` 必须是非托管类型。如果没有指定数组的初始化项，新分配的 `stackalloc` 内存块上的内容是未定义的。使用 `stackalloc` 会自动启用公共语言运行时 CLR 中的缓冲区溢出检测功能。

`stackalloc` 表达式只可以用于局部变量的初始化表达式或赋值操作。不能在 `catch` 或 `finally` 子句中使用堆栈分配内存。

在方法返回时，将自动丢弃在方法执行期间创建的堆栈中分配的内存块。不能显式释放使用 `stackalloc` 分配的内存。堆栈中分配的内存块不受垃圾回收的影响，也不必通过 `fixed` 语句固定。

```csharp
// Memory uninitialized
Span<int> span1 = stackalloc int[3];

// Memory initialized
Span<int> span2 = stackalloc int[3] { -10, -15, -30 };

// Type int is inferred
Span<int> span3 = stackalloc[] { 11, 12, 13 };

// Error; result is int*, not allowed in a safe context
var span4 = stackalloc[] { 11, 12, 13 };

// Error; no conversion from Span<int> to Span<long>
Span<long> span5 = stackalloc[] { 11, 12, 13 };

// Converts 11 and 13, and returns Span<long> 
Span<long> span6 = stackalloc[] { 11, 12L, 13 };

// Converts all and returns Span<long>
Span<long> span7 = stackalloc long[] { 11, 12, 13 };

// Implicit conversion of Span<T>
ReadOnlySpan<int> span8 = stackalloc int[] { 10, 22, 30 };

// Implicit conversion of Span<T>
Widget<double> span9 = stackalloc double[] { 1.2, 5.6 };

public class Widget<T>
{
    public static implicit operator Widget<T>(Span<double> sp) { return null; }
}
```

将 `stackalloc` 表达式的结果分配给 `System.Span<T>` 或 `System.ReadOnlySpan<T>` 类型时，可以不使用 `unsafe` 上下文。建议尽可能使用 `Span<T>` 或 `ReadOnlySpan<T>` 类型来处理堆栈中分配的内存。

```csharp
int length = 1000;
Span<byte> buffer = length <= 1024 ? stackalloc byte[length] : new byte[length];

ReadOnlySpan<int> numbers = stackalloc[] { 1, 2, 3, 4, 5, 6 };
var ind = numbers.IndexOfAny(stackalloc[] { 2, 4, 6, 8 });
Console.WriteLine(ind);  // output: 1
```

可以将 `stackalloc` 表达式的结果分配给指针类型，必须使用 `unsafe` 上下文。

```csharp
unsafe
{
    int length = 3;
    int* numbers = stackalloc int[length];
    for (var i = 0; i < length; i++)
        numbers[i] = i;
}
```

堆栈上可用的内存量存在限制，如果在堆栈上分配过多的内存，会引发 `StackOverflowException`。限制使用 `stackalloc` 分配的内存量，如果预期的缓冲区大小低于特定限制，则在堆栈上分配内存；否则，使用所需长度的数组。避免在循环内使用 `stackalloc`。在循环外分配内存块，然后在循环内重用它。

```csharp
const int MaxStackLimit = 1024;
Span<byte> buffer = inputLength <= MaxStackLimit ? 
        stackalloc byte[MaxStackLimit] : new byte[inputLength];
```

>---

### 3.16. with 表达式

使用 `with` 表达式创建左侧操作数的副本，附加需要修改的特性属性和字段。在 C#9 中，`with` 表达式的左侧操作数必须为记录类型。

从 C#10 开始，`with` 表达式的左侧操作数可以为结构类型或匿名类型。对于引用类型成员，在复制操作数时仅复制对成员实例的引用，副本和原始操作数都具有对同一引用类型实例的访问权限。
 
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

>---

### 3.17. await 表达式

`await` 运算符用于暂停封闭异步函数的求值，直到操作数表示的异步操作完成为止。`await` 只允许在 `async` 声明的异步函数体中使用，在非异步的嵌套方法或匿名方法、`lock` 语句块中、不安全的上下文中不能使用 `await` 表达式，异步的匿名方法无法转换为表达式树类型。

`await` 运算符的操作数是任务类型（`Task`、`Task<TResult>`、`ValueTask`、`ValueTask<TResult>` 等），它表示在计算 `await` 表达式时可能完成或未完成的异步操作。`await` 运算符的目的是暂停封闭异步函数的执行，直到等待的任务完成，然后获得其结果。

```csharp
public class AwaitOperator
{
    public static async Task Main()
    {
        Task<int> downloading = DownloadDocsMainPageAsync();
        Console.WriteLine($"{nameof(Main)}: Launched downloading.");

        int bytesLoaded = await downloading;
        Console.WriteLine($"{nameof(Main)}: Downloaded {bytesLoaded} bytes.");
    }

    private static async Task<int> DownloadDocsMainPageAsync()
    {
        Console.WriteLine($"{nameof(DownloadDocsMainPageAsync)}: About to start downloading.");

        var client = new HttpClient();
        byte[] content = await client.GetByteArrayAsync("https://learn.microsoft.com/en-us/");

        Console.WriteLine($"{nameof(DownloadDocsMainPageAsync)}: Finished downloading.");
        return content.Length;
    }
}
// Output similar to:
// DownloadDocsMainPageAsync: About to start downloading.
// Main: Launched downloading.
// DownloadDocsMainPageAsync: Finished downloading.
// Main: Downloaded 27700 bytes.
```

#### 3.17.1. awaitable expression

`await` 表达式的结果必须是可等待的，可等待的表达式 `E` 的结果类型具有一个 `GetAwaiter` 的可访问的无参非泛型的实例或扩展方法，并且返回类型 `A` 必须满足：
- `A` 实现接口 `System.Runtime.CompilerServices.INotifyCompletion`。
- `A` 有一个可访问、可读的 `bool` 类型的实例属性 `IsCompleted`。
- `A` 有一个可访问的无参非泛型实例方法 `GetResult`。 

`GetAwaiter` 方法的目的是为当前等待任务获取一个 `Awaiter` 等待器，对于 `await` 表达式而言，类型 `A` 是一个 `Awaiter` 类型。

`IsCompleted` 属性的目的是确定任务是否完成，如果完成，则不需要暂停求值。

`INotifyCompletion.OnCompleted` 方法用于为 `Task` 任务注册一个 `continuation` “延续” 的委托类型，并在 `Task` 任务完成后调用。

`GetResult` 方法的目的是在任务完成后获得任务的结果。该结果可能是成功完成，可能带有结果值，也可能是由 `GetResult` 方法抛出的异常。

#### 3.17.2. await 表达式的运行时求值顺序

对于表达式 `await E`：
- 计算 `(E).GetAwaiter()` 求值获取一个 `Awaiter` 类型的等待器 `A`。
- 通过计算 `(A).IsCompleted` 求值的得到一个描述当前任务完成状态的 `bool` 值 `b`。
- 如果 `b` 为 `false`，则求值取决于 `A` 是否实现 `System.Runtime.CompilerServices.ICriticalNotifyCompletion` 接口，定义 `r` 为 `resumption` 恢复委托：
  - 当 `a` 未实现 `ICriticalNotifyCompletion` 接口，则求值 `((a) as INotifyCompletion).OnCompleted(r)`。
  - 当 `a` 实现了 `ICriticalNotifyCompletion` 接口，则求值 `((a) as ICriticalNotifyCompletion).UnsafeOnCompleted(r)`。
  - 然后暂停执行，并将控制权返回给当前异步方法的调用者。
- 立即调用（若 `b` 为 `true`）或稍后调用（`b` 为 `false`）恢复委托，以调用 `(a).GetResult()` 求值，若返回一个值，则它就是 `await` 表达式的结果，否则没有返回值。

方法 `OnCompleted` 和 `UnsafeOnCompleted` 的实现应该使恢复委托 `r` 最多被调用一次。否则，封闭异步函数的行为是未定义的。

>---

### 3.18. 匿名函数

匿名函数是一个表示 “内联” 方法定义的表达式。匿名函数本身没有值或类型，但可以转换为兼容的委托或表达式树类型。匿名函数转换的求值取决于转换的目标类型：
- 如果是委托类型，则转换的求值为引用匿名函数定义的方法的委托值。
- 如果是表达式树类型，则转换的计算结果为表达式树，该表达式树将方法的结构表示为对象结构。

有两种形式的匿名方法表示方法，一种使用 `delegate` 声明匿名方法表达式（块主体），另一种使用简洁的 Lambda 表达式或语句（块或表达式主体）。

#### 3.18.1. delegate 匿名方法表达式

使用 `delegate` 声明匿名函数表达式，函数体仅支持语句块主体。

```ANTLR
anonymous_method_expression
    : delegate ()? { <sequence-of-statements> }
    | delegate ( parameters? ) { <sequence-of-statements> }
```

匿名函数表达式的参数列表是可选的，如果给出了参数，则必须显式声明参数类型。匿名函数不能是动态绑定操作的接收方、参数或是操作数。

```csharp
var fun = delegate (int x) { return x + 1; };      // Anonymous method expression
Func<int, int> fun1 = delegate { return 1 + 1; };  // Parameter list omitted
Func<int, int> fun2 = static delegate { return 1 + 1; };        // static
var fun3 = async delegate (int sec){ await Task.Delay(sec); };  // async
```

#### 3.18.2. Lambda 表达式

使用 Lambda 表达式来创建匿名函数：

```ANTLR
lambda_expression
    : async? <return_type>? ( input_parameters? ) => expression ;

lambda_statement
    : async? <return_type>? ( input_parameters? ) => { <sequence-of-statements> } ;
```

Lambda 表达式的参数可以声明隐式参数，若无法推断参数的类型时，则需要显式指定类型。隐式变量只有一个时，括号可以省略。

```csharp
Func<int,int> Square = x => x * x;
var Sum = (int x, int y) => x + y;
var Iterator = (params IEnumerable[] arr) =>
{
    foreach (var item in arr)
        Console.WriteLine(item);
};
```

通过使用 `async` 和 `await` 关键字，可以创建包含异步处理的 Lambda 表达式和语句。

```csharp
var asyncAction = async (Action ac) => await Task.Run(ac);
await asyncAction(() => Console.WriteLine("Hello, World"));
```

从 C#9 开始，可以使用弃元指定 Lambda 表达式中不使用的两个或更多输入参数。如果只有一个输入参数命名为 `_`，则在 Lambda 表达式中，`_` 将被视为该参数的名称。

```csharp
Func<int, int, int> Constant = (_, _) => 99;
```

从 C# 9.0 开始，可以将 `static` 修饰符应用于 Lambda 表达式，以防止由 Lambda 无意中捕获本地变量或实例状态，但可以引用静态成员和常量定义。

```csharp
Func<double, double> square = static x => x * x;
```

从 C#10 开始，可以在输入参数前面指定 Lambda 表达式的返回类型。

```csharp
var Choose = object (bool b) =>  b? 1:"two";
```

从 C#10 开始，可以将特性添加到 Lambda 表达式及其参数。

```csharp
var concat = ([DisallowNull] string a, [DisallowNull] string b) => a + b;
```

从 C#12 开始，可以为 Lambda 表达式上的参数提供默认值。

```csharp
var fun = (string? mess = "") => Console.WriteLine(mess);
```

#### 3.18.3. 匿名函数签名

匿名函数的签名定义了匿名函数的形参的名称和可选的类型。如果匿名函数有一个显式类型声明的方法签名，那么兼容的委托类型和表达式树类型也必须具有相同参数类型、参数修饰符和相同的参数顺序的方法签名。与方法组转换委托不同，不支持匿名函数参数类型的协变转换。

若匿名函数没有签名，那么兼容的委托类型和表达式树类型的参数列表中为没有任何 `out` 参数修饰的形参。

```csharp
class Sample
{
    delegate void Fun1(int a);
    delegate void Fun2(float b);
    delegate void Fun3(out int b);
    delegate void Fun4(ref int b, int c, string s);

    static void Main()
    {
        Fun1 f1A = delegate { };
        Fun2 f1B = delegate { };
        Fun3 f1C = delegate { };  // out paramster
        Fun4 f1D = delegate { };

        Fun1 f2A = x => x *= x;    // 自动推断
        Fun2 f2B = x => x *= x;   // 自动推断
        Fun3 f2C = (out int x) => x = 0;  // 需要显式声明类型
        Fun4 f2D = delegate (ref int a, int b, string s) { };
    }
}
```

#### 3.18.4. 匿名函数主体

匿名函数主体只能使用自身签名中声明的 `ref`、`in`、`out` 参数，无法使用外部范围的其他应用变量。

```csharp
class Sample
{
    static void Test(ref int val, string s)
    {
        var f = delegate (out int s_val)
        {
            Console.WriteLine(s);
            s_val = val; // err
            return ref s_val;
        };
    }
}
```

匿名函数主体只能使用自身签名中声明的 `ref struct` 类型的参数，不能使用外部范围的 `ref struct` 变量。

```csharp
class Sample
{
    static void Test(Span<int> val, string s)
    {
        var f = delegate(Span<int> s_buff)
        {
            Console.WriteLine(s);
            var buff = val;  // err

            for (int i = 0; i < s_buff.Length; i++)
                s_buff[i] = i;
        };
    }
}
```

如果 `this` 是结构类型，则匿名函数的函数主体无法使用 `this`，可以在外部范围声明一个 `this` 赋值的局部变量。

```csharp
struct Sample
{
    int Value;
    void Test()
    {
        var @this = this;

        Action f = delegate
        {
            int val = this.Value;  // err
            int s_val = @this.Value;
        };
    }
}
```

函数体可以访问匿名函数范围外的外部变量。对外部变量的访问将在调用引用匿名函数的委托时处于活动状态。

```csharp
Action ac = null;
Sample S= new Sample();
S.Fun(ref ac);
if(ac is not null)
{
    for (int i = 0; i < 10; i++)
        ac.Invoke();
}
class Sample
{
    int Counter;
    public void Fun(ref Action ac)
    {
        ac = delegate
        {
            Counter++;
            Console.WriteLine($"Counter : {Counter}");
        };
    }
}
```

#### 3.18.5. 外部变量

任何局部变量、值参数、参数数组等的作用域可以覆盖到匿名方法的函数体时，这些变量都被称为匿名函数的外部变量。在类的实例函数成员中，`this` 值被认为是值参数。

当一个外部变量被匿名函数引用时，外部变量就被匿名函数捕获了。通常局部变量的生命周期仅限于与它关联的块或语句的执行，但是作为匿名函数内被捕获的外部变量，局部变量的生命周期至少会延长，直到从匿名函数创建的委托或表达式树符合垃圾回收的条件。

```csharp
class Test
{
    delegate int D();
    static D F()
    {
        int x = 0;
        D result = () => ++x;
        return result;
    }
    static void Main()
    {
        D d = F();
        Console.WriteLine(d());   // 1
        Console.WriteLine(d());   // 2
        Console.WriteLine(d());   // 3
    }
}
```

当局部变量或值参数被匿名函数捕获时，局部变量或形参不再被认为是固定变量，而是可移动变量。但是由于不能获取捕获外部变量的地址，所以这些外部变量不能在 `fixed` 语句中使用。

在将匿名函数转换为表达式树时，对编译器生成的对象引用可以存储在表达式树中，对局部变量的访问可以表示为对这些对象的字段访问。这种方法的优点是，它允许在委托和表达式树之间共享 “提升” 的局部变量。

---
## 4. 算数运算符

一元 `++`（增量）、`--`（减量）、`+`（加）和 `-`（减）运算符。
二元 `*`（乘法）、`/`（除法）、`%`（余数）、`+`（加法）和 `-`（减法）运算符。

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
Console.WriteLine(-13 / 5);      // output: -2
Console.WriteLine(13 / 5.0);     // output: 2.6
Console.WriteLine(-5 % 4);       // output: -1
Console.WriteLine(5 % -4);       // output: 1
Console.WriteLine(-5.2f % 2.0f); // output: -1.2
Console.WriteLine(5 + 4.3);      // output: 9.3
Console.WriteLine(47 - 3);       // output: 44
Console.WriteLine(7.5m - 2.3m);  // output: 5.2

// 复合运算
int c = 5;
Console.WriteLine(c += 9);  // c=c+9, output: 14
Console.WriteLine(c -= 4);  // c=c-4, output: 10 
Console.WriteLine(c *= 2);  // c=2*c, output: 20
Console.WriteLine(c /= 4);  // c=c/4, output: 5
Console.WriteLine(c %= 3);  // c=c%3, output: 2
```

---
## 5. 关系运算符

二元 `<`（小于）、`>`（大于）、`<=`（小于等于）、`>=`（大于等于）、`==`（等于）、`!=`（不等于）运算符。

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

### 5.1. 相等性比较

有时需要比较两个值是否相等。相等性可以测试 “值相等性” 或 “引用相等性”：
  - 值相等性也称为 “等效性”，指两个对象包含相同的一个或多个值。
  - 引用相等性指两个对象引用均引用同一基础对象。

使用 `Object.ReferenceEquals` 方法确定两个引用是否引用同一对象。引用相等性的概念仅适用于引用类型，传递值类型只会传递值的副本给 `ReferenceEquals` 的参数，因此使用该方法比较两个值类型，结果始终返回 `false`。

```csharp
using System;
class Test
{
    public int Num { get; set; }
    public string Str { get; set; }
    static void Main()
    {
        Test a = new Test() { Num = 1, Str = "Hi" };
        Test b = new Test() { Num = 1, Str = "Hi" };

        bool areEqual = System.Object.ReferenceEquals(a, b);
        // False:
        System.Console.WriteLine("ReferenceEquals(a, b) = {0}", areEqual);

        // Assign b to a.
        b = a;

        // Repeat calls with different results.
        areEqual = System.Object.ReferenceEquals(a, b);
        // True:
        System.Console.WriteLine("ReferenceEquals(a, b) = {0}", areEqual);

        // Keep the console open in debug mode.
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
}
```

#### 5.1.1. 值相等性

针对值相等性的测试，可以使用 `==` 运算符。对记录来说，值相等性是指如果记录类型的两个变量类型相匹配，且所有属性和字段值（或引用对象）都一致，那么记录类型的两个变量是相等的。字符串按值相等性比较。

可以使用 `Object.Equals` 比较两个结构对象的值相等性，对于引用类型则比较引用相等性。 

```csharp
int num1 = 0;
Console.WriteLine(num1 == 0);  // true

Sample s = new() { name = "Hello", num = 1 };
Sample s2 = new() { name = "Hello", num = 1 };
Console.WriteLine(s.Equals(s2));  // true

rSample rs = new("Hi", 1001);
rSample rs1 = new("Hi", 1001);
Console.WriteLine(rs == rs1); // true

string str1 = "Hello";
string str2 = "Hell";
Console.WriteLine(str1 == str2);  // false
str2 += "o";
Console.WriteLine(str1 == str2);  // true

struct Sample
{
    public required int num;
    public required string name;
}
record rSample(string name, int num);
```

#### 5.1.2. 用户定义类型相等性

定义类或结构时，需确定为类型创建值相等性（或等效性）的自定义定义是否有意义。通常，预期将类型的对象添加到集合时，或者这些对象主要用于存储一组字段或属性时，需实现值相等性。可以基于类型中所有字段和属性的比较结果来定义值相等性，也可以基于子集进行定义。

类和结构中的等效性实现均应遵循：
  - 自反属性：`x.Equals(x)` 将返回 `true`。
  - 对称属性：`x.Equals(y)` 返回与 `y.Equals(x)` 相同的值。
  - 可传递属性：如果 `(x.Equals(y) && y.Equals(z))` 返回 `true`，则 `x.Equals(z)` 将返回 `true`。
  - 只要未修改 `x` 和 `y` 引用的对象，`x.Equals(y)` 的连续调用就将返回相同的值。
  - 任何非 `null` 值均不等于 `null`。 然而，当 `x` 为 `null` 时，`x.Equals(y)` 将引发异常。

定义的任何结构都已具有其从 `Object.Equals(Object)` 方法的 `System.ValueType` 替代中继承的值相等性的默认实现。此实现使用反射来检查类型中的所有字段和属性。尽管此实现可生成正确的结果，但与专门为类型编写的自定义实现相比，它的速度相对较慢。

> 如何设计相等性

- 重写替代 `object.Equals(object)` 方法，大多数情况下，该方法应只调入使用 `System.IEquatable<T>` 接口实现类型的特定 `Equals` 方法。实际的等效性比较将在此接口中执行。
- 对于方法 `System.IEquatable<T>.Equals(T? other)` 应仅检查类中声明的字段。仅当要比较的变量的运行时类型相同时，才应将两个变量视为相等。
- 可选的重载 `==` 和 `!=` 运算符。
- 替代 `Object.GetHashCode`，以便具有值相等性的两个对象生成相同的哈希代码。
- 若要支持 “大于” 或 “小于” 定义，请为类型实现 `IComparable<T>` 接口，并同时重载 `<` 和 `>` 运算符

> 类用户定义相等性

```csharp
Point2D p1 = new Point2D(0, 100);
Point2D p2 = new Point2D(0, 200);
Console.WriteLine(p1 == p2);  // false
p2.Y = 100;
Console.WriteLine(p1 == p2);  // true

class Point2D(int x, int y) : IEquatable<Point2D>
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
    public override bool Equals(object? obj) => this.Equals(obj as Point2D);

    public bool Equals(Point2D? other)
    {
        return
            object.ReferenceEquals(other, this)  // 比较引用相等性
            // 比较值相等性
            || other is not null   // 不和 null 比较
            && this.GetType() == other.GetType()  // 运行时类型相等
            && this.X == other.X     // 属性相等性 
            && this.Y == other.Y;
    }
    public override int GetHashCode() => (X, Y).GetHashCode();
    public static bool operator ==(Point2D lhs, Point2D rhs) => lhs?.Equals(rhs) ?? rhs is null;
    public static bool operator !=(Point2D lhs, Point2D rhs) => !(lhs == rhs);
}
```

> 结构用户定义相等性

```csharp
struct Point2D(int x, int y) : IEquatable<Point2D>
{
    public int X { get; private set; } = x;
    public int Y { get; private set; } = y;
    public override bool Equals(object? obj) => obj is Point2D other && this.Equals(other);
    public bool Equals(Point2D p) => X == p.X && Y == p.Y;
    public override int GetHashCode() => (X, Y).GetHashCode();
    public static bool operator ==(Point2D lhs, Point2D rhs) => lhs.Equals(rhs);
    public static bool operator !=(Point2D lhs, Point2D rhs) => !(lhs == rhs);
}
```

#### 5.1.3. 浮点值的相等性

由于二进制计算机上的浮点算法不精确，因此浮点值（`double` 和 `float`）的相等比较会出现问题。若要被视为相等，两个 `double` 值必须表示相同的值。

由于值之间的精度差异，或者由于一个或两个值丢失精度，预期相同的浮点值通常不相等，因为它们的最小有效位数存在差异。调用 `Equals` 方法以确定两个值是否相等，或调用 `CompareTo` 方法以确定两个 `double` 值之间的关系，通常会产生意外的结果。

```csharp
double value1 = .333333333333333;
double value2 = 1.0/3;
Console.WriteLine("{0:R} = {1:R}: {2}", value1, value2, value1.Equals(value2));
// The example displays the following output:
//        0.333333333333333 = 0.33333333333333331: False
```

比较浮点值相等性，可以使用 `Math.Round` 方法是两个浮点值具有相同的精度。

```csharp
double value1 = .333333333333333;
double value2 = 1.0 / 3;
int precision = 7;
value1 = Math.Round(value1, precision);
value2 = Math.Round(value2, precision);
Console.WriteLine("{0:R} = {1:R}: {2}", value1, value2, value1.Equals(value2));

// The example displays the following output:
//        0.3333333 = 0.3333333: True
```

另一种方式是测试近似相等性而不是相等性。这要求定义两个值可以相差但仍相等的绝对量，或者定义较小的值与较大值相差的相对量。

```csharp
using System;
public class Example
{
    public static void Main()
    {
        double one1 = .1 * 10;
        double one2 = 0;
        for (int ctr = 1; ctr <= 10; ctr++)
            one2 += .1;

        Console.WriteLine("{0:R} = {1:R}: {2}", one1, one2, one1.Equals(one2));
        Console.WriteLine("{0:R} is approximately equal to {1:R}: {2}",
                          one1, one2,
                          IsApproximatelyEqual(one1, one2, .000000001));
    }
    static bool IsApproximatelyEqual(double value1, double value2, double epsilon)
    {
        if (value1.Equals(value2))
            return true;
        if (Double.IsInfinity(value1) | Double.IsNaN(value1))
            return value1.Equals(value2);
        else if (Double.IsInfinity(value2) | Double.IsNaN(value2))
            return value1.Equals(value2);
        
        // Handle zero to avoid division by zero
        double divisor = Math.Max(value1, value2);
        if (divisor.Equals(0))
            divisor = Math.Min(value1, value2);

        return Math.Abs((value1 - value2) / divisor) <= epsilon;
    }
}
// The example displays the following output:
//       1 = 0.99999999999999989: False
//       1 is approximately equal to 0.99999999999999989: True
```

可以利用浮点格式的设计功能：两个浮点值的整数表示形式之间的差异指示分隔它们的可能浮点值的数量。例如，`0.0` 和 `double.Epsilon` 的二进制格式表示的整数之间的差值为 1。

```csharp
public class Example
{
    public static void Main()
    {
        Console.WriteLine($"{Convert.ToString(BitConverter.DoubleToInt64Bits(double.Epsilon), 2)}".PadLeft(64, '0'));
        // '0'*63 + '1' = 1L, Epsilon 与 0.0 的二进制表示的整数仅相差 1

        double value1 = .1 * 10;
        double value2 = 0;
        for (int ctr = 0; ctr < 10; ctr++)
            value2 += .1;

        Console.WriteLine("{0:R} = {1:R}: {2}", value1, value2,
                          HasMinimalDifference(value1, value2, 1));
    }
    public static bool HasMinimalDifference(double value1, double value2, int units = 1)
    {
        long lValue1 = BitConverter.DoubleToInt64Bits(value1);
        long lValue2 = BitConverter.DoubleToInt64Bits(value2);

        // 非零时符号位不同则返回 false, 此时要比较 +0 and -0.
        if ((lValue1 >> 63) != (lValue2 >> 63))
            return value1 == value2;  // 比较 +0.0 与 -0.0, true

        long diff = Math.Abs(lValue1 - lValue2);
        // 比较浮点二进制代表的整数的差值
        return diff <= (long)units;
    }
}
// The example displays the following output:
//     1 = 0.9999999999999999: True   
```

> Double.Epsilon

- `Double.Epsilon` 在测试相等性时，有时用作两 `Double` 个值之间距离的绝对度量值。但是，`Double.Epsilon` 测量的是值为零的 `Double` 对象可以加或减的最小可能值。对于大多数正值和负 `Double` 值，`Double.Epsilon` 的值太小，无法被检测到。因此，除零值外，不建议在相等性测试中使用它。

```csharp
static bool ZeroEquals(double val)
{
    return Math.Abs(val - 0.0) < double.Epsilon;
}
```

#### 5.1.4. 为 ref 变量创建引用相等性比较 

对两个 `ref` 变量使用 `==` 或 `!=`，对于值类型则比较值相等性，对于引用类型则比较引用相等性。若要检查两个 `ref` 值类型是否引用同一个对象，则比较它们的地址值。

```csharp
int number = 0;
int number2 = number;

ref int n1 = ref number;
ref int n2 = ref number2;

Console.WriteLine(n1 == n2);  // true
Console.WriteLine(n1.refsEquals(ref n2));  // false
n2 = ref number;
Console.WriteLine(n1.refsEquals(ref n2));  // true
n2 = ref n1;
Console.WriteLine(n1.refsEquals(ref n2));  // true

public static class ObjectExt
{
    public unsafe static bool refsEquals<T>(this scoped ref T obj, scoped ref T other) where T : struct
    {
        fixed (T* reft1 = &obj, reft2 = &other)
            return reft1 == reft2;
    }
    public unsafe static bool refsEquals<T>(this scoped ref T? obj, scoped ref T? other) where T : struct
    {
        fixed (T?* reft1 = &obj, reft2 = &other)
            return reft1 == reft2;
    }
}
```

---
## 6. 类型测试

### 6.1. is 运算符

`is` 运算有两种形式，一种是类型测试（右侧是一个类型），一种模式匹配（右侧是一个模式）。

`is` 类型测试运算用于检查对象的运行时类型是否与给定的类型兼容。`E is T` 表达式中，`E` 表示一个表达式，`T` 是一个非动态类型，结果为一个布尔值。`is` 测试 `E` 是否为空，是否可以通过引用转换、装箱转换、拆箱转换、包装转换、拆包转换为类型 `T`。

测试的表达式 `E` 不能是方法组、匿名方法、lambda 表达式。`is` 运算符不会考虑用户定义的转换。

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

#### 6.1.1. 检查 null 值

```csharp
using System.Diagnostics;
object obj = null;

// 相等性测试
Trace.Assert(obj == null);
// ReferenceEquals
Trace.Assert(object.ReferenceEquals(obj, null));
// is 表达式
Trace.Assert(obj is null);
Trace.Assert(obj is not object);
Trace.Assert(obj is not { });
```

>---

### 6.2. as 运算符

`as` 运算符用于显式地将值转换为给定的引用类型或可空值类型。与强制转换表达式不同，`as` 运算符永远不会抛出异常。若指定的转换不可能，则结果值为 `null`。

`as` 运算符将 `E as T` 表达式中，`T` 不能是不可为 null 的值类型。若 `E` 不是动态类型，`E as T` 等效于 `E is T ? (T)(E) : (T)null`。如果 `E` 在编译时是动态的，`as` 运算符不是动态绑定的，因此 `E as T` 等效于 `E is T ? (T)(object)(E) : (T)null`

```csharp
IEnumerable<int> numbers = new[] { 10, 20, 30 };
IList<int> indexable = numbers as IList<int>;
if (indexable != null)
    Console.WriteLine(indexable[0] + indexable[indexable.Count - 1]);  // output: 40
```

`as` 运算符的操作数是类型参数时，它不能是无约束的，应使用强制转换表达式执行。

```csharp
class Sample
{
    public string F(object o)
        => o as string;  // OK, string is a reference type

    public T G<T>(object o) where T : Attribute
        => o as T;       // Ok, T has a class constraint

    public U H<U>(object o)
        => o as U;       // Error, U is unconstrained
}
```

>---

### 6.3. typeof 类型测试

`typeof` 运算符用于获取某个类型的 `System.Type` 实例。`typeof` 运算符的实参必须是类型或类型形参的名称，不能是 `dynamic` 或任何可为 null 的引用类型。
`Object.GetType()` 用以获取某个实例对象的相应类型的 `System.Type` 实例。使用 `typeof` 运算符和 `object.GetType()` 来检查表达式结果的运行时类型是否与给定的类型完全匹配。

```csharp
object b = new Giraffe();
Console.WriteLine(b is Animal);  // output: True
Console.WriteLine(b.GetType() == typeof(Animal));  // output: False

Console.WriteLine(b is Giraffe);  // output: True
Console.WriteLine(b.GetType() == typeof(Giraffe));  // output: True

class Animal;
class Giraffe : Animal;
```

---
## 7. 逻辑运算与条件逻辑运算

一元 `!`（逻辑非）运算符。

二元 `&`（逻辑与）、`|`（逻辑或）和 `^`（逻辑异或）运算符，这些运算符始终计算两个操作数。

二元 `&&`（条件逻辑与）和 `||`（条件逻辑或）运算符，这些运算符仅在必要时才计算右侧操作数。

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

>---

### 7.1. 用户定义条件逻辑运算符

若用户定义类型已包含 `|`（或 `&`）运算符重载，可以定义 `true` 和 `false` 的运算符重载以支持该类型执行条件逻辑运算 `||`（或 `&&`），唯一的要求是 `true` 和 `false` 运算符的操作数和返回类型都是其包含类型 `T`。

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
    /* output:
        Getting fuel launch status...
        Wait!
    */

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
## 8. 条件运算符

条件运算符 `? :` 也称为三元条件运算符，用于计算布尔表达式 `condition ? consequent : alternative`，并根据布尔表达式的计算结果为 `true` 还是 `false` 来返回两个表达式中的一个结果。

`consequent` 和 `alternative` 的类型必须可隐式转换为目标类型。

```csharp
var rand = new Random();
var condition = rand.NextDouble() > 0.5;

var x = condition ? 12 : (int?)null;
```

>---

### 8.1. ref 条件表达式

条件 `ref` 表达式可有条件地返回变量引用：`condition ? ref consequent : ref alternative`。在 `ref` 条件表达式中，`consequent` 和 `alternative` 的类型必须相同。

可以使用 `ref` 分配条件 `ref` 表达式的结果，将其用作引用返回，或将其作为 `ref`、`out` 或 `in` 方法参数传递。

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
## 9. 位运算和移位运算

一元 `~`（按位求补）运算符。

二进制 `&`（按位与）、`|`（按位或）和 `^`（按位异或）运算符。

二进制 `<<`（左移）、`>>`（右移）和 `>>>`（无符号右移）运算符。

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
## 10. 类型转换

转换使表达式被转换为或被视为属于特定类型。转换可能涉及表示的变化，转换可以是隐式转换或显式强制转换。一些转换是语言定义的，用户可以自定义类型转换。

```csharp
int a = 123;
long b = a;      // implicit
int c = (int)b;  // explicit
```

>---

### 10.1. 隐式转换

隐式转换可能会在多种情况下发生，包括函数成员调用、强制转换表达式、赋值运算等。预定义的隐式转换始终会成功，并且不会引发异常。

#### 10.1.1. 隐式恒等转换

恒等转换将任意类型转换为相同类型，即类型 `T` 或类型 `T` 的表达式可转换为 `T` 本身：
- `dynamic` 和 `object` 之间存在恒等转换。
- 在相同泛型构造的类型之间，如果每个对应类型参数之间存在恒等转换，构造实例之间存在恒等转换。
- 具有相同密度的元组类型之间，如果每对对应元素类型之间存在恒等转换时，元组之间存在恒等转换。


#### 10.1.2. 隐式数值转换

当某一数值类型 `T` 的值域在目标类型 `U` 的可表示值域范围内，则 `T` 可以隐式转换为 `U`，例如：
- 从 `byte`、`sbyte` 到 `short`、`int`、`nint`、`long`、`float`、`double`、`decimal`。
- 从 `byte` 到 `ushort`、`uint`、`nuint`、`ulong`。
- 从 `short`、`ushort` 到 `int`、`nint`、`long`、`float`、`double`、`decimal`。
- 从 `ushort` 到 `uint`、`nuint`、`ulong`。
- 从 `int`、`uint` 到 `nint`、`long`、`float`、`double`、`decimal`。
- 从 `uint` 到 `nuint`、`ulong`。
- 从 `nint` 到 `long`、`float`、`double`、`decimal`。
- 从 `nuint` 到 `ulong`、`float`、`double`、`decimal`。
- 从 `long`、`ulong` 到 `float`、`double`、`decimal`。
- 从 `char` 到 `ushort`、`short`、`uint`、`int`、`nint`、`nuint`、`ulong`、`long`、`float`、`double`、`decimal`。
- 从 `float` 到 `double`。

从整型到浮点数的转换可能会导致精度损失，但是不会导致范围损失。

#### 10.1.3. 隐式枚举转换

隐式枚举转换允许将任意整数类型的 `0` 值常量转换为任意枚举类型和任何可空枚举类型。其他的整数类型数值需要强制转换运算。

```csharp
TaskStatus status0 = 0;
TaskStatus status = (TaskStatus)5;
```

#### 10.1.4. 隐式内插字符串转换

隐式内插字符串转换允许将内插字符串转换为 `System.IFormattable` 或 `System.FormattableString`。

#### 10.1.5. 隐式可空转换

对于非空值类型的预定义隐式转换，也可以用于这些类型的可空值类型。非空值类型 `T` 到其可空类型 `T?` 之间存在隐式转换。

基于从 `S` 到 `T` 的底层转换的可空转换求值过程：
- 对于 `S?` 到 `T?` 的转换，若 `S?` 的值为空，则结果为 `T?` 的空值；否则转换将作为从 `S?` 展开为 `S` 后，再从 `S` 到 `T`，再从 `T` 包装为 `T?`。
- 对于 `S` 到 `T?` 的转换，先从 `S` 到 `T`，再从 `T` 包装到 `T?`。

```csharp
int num = 10;
long lnum = num;    // S to T
long? nlnum = num;  // S to T?

int? n_num = num;
long? n_lnum = n_num;  // S? to T?
```

#### 10.1.6. null 的转换

存在从 `null` 字面值到任何引用类型或可空值类型的隐式转换。对于引用类型转换为空引用，对于可空值类型则生成空值。

#### 10.1.7. 隐式引用转换

隐式引用转换是指那些可以证明总是成功的引用类型之间的转换，在运行时不需要检查。隐式引用转换有：
- 从任意引用类型到 `object` 和 `dynamic` 的转换。
- 从任意类类型 `S` 到其基类 `T`、基接口 `I` 的转换。
- 从任意接口类型 `I` 到其基接口的 `Ibase` 的转换。
- 元素类型为 `Se` 的数组 `S` 到元素类型为 `Te` 的数组 `T` 存在隐式转换需要满足：
  - 数组 `S` 和 `T` 具有相同的维度。
  - `Se` 和 `Te` 都是引用类型，且 `Se` 到 `Te` 存在隐式转换。  
- 从任何数组类型到 `System.Array` 以及它实现的接口类型的转换。
- 从任意一维数组类型 `S[]` 到 `System.Collections.Generic.IList<T>` 及其基接口的隐式转换，需满足 `S` 到 `T` 存在隐式恒等转换或隐式引用转换。 
- 从任意委托类型到 `Delegate` 和它实现的接口的转换。
- 从 null 值到任意引用类型。
- 从任意引用类型 `S` 到引用类型 `T` 的隐式转换，需满足从 `S` 到 `T` 存在隐式恒等转换或隐式引用转换。
- 从任意引用类型 `S` 到接口或委托类型 `T` 的隐式转换，需满足 `S` 到 `T` 存在隐式恒等转换或隐式引用转换，且 `S` 可以协变转换到 `T`。
- 涉及任何引用类型的类型参数的隐式转换。
 
引用转换永远不会改变被转换对象的引用标识。虽然引用转换可以更改引用的类型，但它不会更改引用对象的类型或值。

#### 10.1.8. 装箱转换

装箱转换允许将值类型隐式转换为引用类型，装箱转换有：
- 从任意值类型到 `object`、`System.ValueType` 的转换。
- 从任意枚举类型到 `System.Enum` 的转换。
- 从任意非空值类型到其实现的接口的转换。
- 从任意非空值类型到任意接口 `I` 的隐式转换，需满足非空值类型可装箱转换的另一个接口 `I0` 到 `I` 之间存在恒等转换或协变转换。
- 从任意可空类型到任意引用类型的转换，其中存在可空类型的基础类型到该引用类型的装箱转换。

将非空值类型的值装箱包括分配一个对象实例并将值复制到该实例中。若可空值类型的值是空值，则其装箱为空引用，否则将展开底层值并生成该值装箱的引用。

#### 10.1.9. 隐式动态转换

存在从动态类型表达式到任意类型 `T` 的隐式动态转换，该转换是动态绑定的，这意味着将在运行时寻求从表达式的运行时类型到 `T` 的隐式转换，若无法成功转换，则抛出运行时异常。

```csharp
object o  = "object"
dynamic d = "dynamic";

string s1 = o;  // Fails at compile-time -- no conversion exists
string s2 = d;  // Compiles and succeeds at run-time
int i     = d;  // Compiles but fails at run-time -- no conversion exists
```

#### 10.1.10. 隐式常量表达式转换

隐式常量表达式的转换允许：
- `int` 类型的常量表达式可以转换为 `sbyte`、`byte`、`short`、`ushort`、`uint`、`nint`、`nuint`、`ulong` 类型，前提是该常量值在目标类型的范围内。
- `long` 类型的常量表达式可以转换为 `ulong` 类型，前提是非负值。

#### 10.1.11. 涉及类型参数的隐式转换

给定的类型参数 `T` 存在以下隐式转换： 

- 对于已知为引用类型的类型参数 `T`，允许从 `T` 到其任意基类 `C`、从由 `T` 到 `C` 实现的任何接口 `I` 和 `I` 的任意基接口的隐式引用转换。
  
- 若 `T` 不知是否为引用类型时，涉及 `T` 到任意基类或基接口的转换在编译时被认为是装箱转换。在运行时，如果 `T` 是值类型，则转换为装箱转换执行，否则转换作为隐式转换或恒等转换执行。

- 从 `T` 到类型参数 `U` 的转换，具体取决于 `U` 的类型参数约束。若 `U` 是值类型，则 `T` 到 `U` 的类型必须相同，且不执行任何转换；若 `T` 是值类型，则转换将作为装箱转换；否则将作为隐式引用转换或恒等转换。

- 从 `null` 到引用类型的类型参数 `T`。 

#### 10.1.12. 隐式元组转换

如果元组表达式 `E` 与元组类型 `T` 具有相同的密度，且存在从 `E` 中的每个元素到 `T` 中相应元素类型的隐式转换，则存在 `E` 到 `T` 的隐式转换。转换通过创建 `System.ValueTuple<...>` 类型，并从左到右的顺序初始化它的每个字段。

如果元组表达式中的元素名与元组类型中相应的元素名不匹配，则发出警告，表达式的元素名称将被忽略。

```csharp
(int, string) t1 = (1, "One");
(byte, string) t2 = (2, null);
(int, string) t3 = (null, null);        // Error: No conversion
(int i, string s) t4 = (i: 4, "Four");
(int i, string) t5 = (x: 5, s: "Five"); // Warning: Names are ignored
```

#### 10.1.13. 用户定义的隐式转换

用户定义的隐式转换包括由从一个可选的标准隐式转换，到执行用户定义的隐式转换运算符，再到执行另一个可选的标准隐式转换。

#### 10.1.14. 匿名函数转换和方法组转换

匿名函数和方法组本身没有类型，它们可以隐式地转换为委托类型。一些 Lambda 表达式可以隐式转换为表达式树类型。

#### 10.1.15. 默认值转换

存在从 `default` 到任何类型的隐式转换，此转换将生成推断类型的默认值。

#### 10.1.16. 隐式抛出转换

`throw` 表达式没有类型，但是它们可以隐式转换为任何类型。

>---

### 10.2. 显式转换

显式转换可在强制转换表达式（`(type)value`）中发生。显式转换集包含所有的隐式转换，即隐式转换可以显式使用强制转换表达式。

```csharp
int num = 123;
object obj = (object)num;  // 隐式转换

int num2 = (int)obj;       // 显式强制转换
```

不是隐式转换的显式转换是指不能证明总是成功的转换、已知可能丢失信息的转换以及跨类型域的转换，这些转换差异很大，必须显式标记。显式转换可能会存在无效的强制转换。

#### 10.2.1. 显式数字转换

- 从 `sbyte` 到 `byte`、`ushort`、`uint`、`ulong`、`char`。
- 从 `byte` 到 `sbyte`、`char`.
- 从 `short` 到 `sbyte`、`byte`、`ushort`、`uint`、`ulong`、`char`
- 从 `ushort` 到 `sbyte`、`byte`、`short`、`char`
- 从 `int` 到 `sbyte`、`byte`、`short`、`ushort`、`uint`、`ulong`、`char`。
- 从 `uint` 到 `sbyte`、`byte`、`short`、`ushort`、`int`、`char`。
- 从 `nint` 到 `sbyte`、`byte`、`short`、`ushort`、`int`、`uint`、`nuint`、`ulong`、`char`。
- 从 `nuint` 到 `sbyte`、`byte`、`short`、`ushort`、`int`、`uint`、`nint`、`long`、`char`。
- 从 `long` 到 `sbyte`、`byte`、`short`、`ushort`、`int`、`uint`、`ulong`、`char`。
- 从 `ulong` 到 `sbyte`、`byte`、`short`、`ushort`、`int`、`uint`、`long`、`char`。
- 从 `char` 到 `sbyte`、`byte`、`short`。
- 从 `float` 到 `sbyte`、`byte`、`short`、`ushort`、`int`、`uint`、`long`、`ulong`、`char`、`decimal`。
- 从 `double` 到 `sbyte`、`byte`、`short`、`ushort`、`int`、`uint`、`long`、`ulong`、`char`、`float`、`decimal`。
- 从 `decimal` 到 `sbyte`、`byte`、`short`、`ushort`、`int`、`uint`、`long`、`ulong`、`char`、`float`、`double`。

由于显式转换可能会丢失信息，或可能导致引发异常。对于显式数值转换：
- 在 `checked` 的上下文，源操作数在目标类型范围内则转换成功，否则将引发 `System.OverflowException` 异常。
- 在 `unchecked` 的上下文，始终转换成功。对于溢出的源类型将放弃额外的高位来截断源值，而小的源值则是符号扩展（有符号数）或零扩展（无符号数）高位。
- 对于从 `decimal` 到整数类型的转换，源值向零舍入到最接近的整数值。超出范围时引发 `Overflow` 异常。
- 对于 `float`、`double` 到整数类型的转换，在检查的上下文中，超出范围会引发溢出异常；未检查的上下文中向零舍入到最接近的整数值，不在范围的浮点值将是目标类型的未指定值。
- 对于 `double` 转换为 `float`，将值舍入到最接近的 `float` 值。若太小则结果是正零或负零，太大则是正无穷或负无穷。
- 对于 `float`、`double` 到 `decimal` 的转换，将源值转换为 `decimal` 格式，并将第 28 位小数后舍入到最接近的数。若太小时结果为零，若是 `nan`、无穷大或值太大而无法表示为 `decimal` 时，将引发溢出异常。
- 对于从 `decimal` 到浮点类型的转换，将源值舍入到最接近的目标类型值。这种转换可能会丢失精度，但不会引发异常。

#### 10.2.2. 显式枚举转换

显式枚举转换包括从任意数值类型到枚举类型的显式转换，从任意枚举类型到任意数值类型的转换，或任意枚举类型之间的转换。

整数类型的 0 值可以隐式转换为枚举类型。

#### 10.2.3. 显式可空转换

对于非空值类型的预定义显式转换，也可以用于这些类型的可空值类型。非空值类型 `T?` 到其可空类型 `T` 之间存在式转换。

基于从 `S` 到 `T` 的底层显式转换的可空显式转换求值过程：
- 对于 `S?` 到 `T?` 的转换，若 `S?` 的值为空，则结果为 `T?` 的空值；否则转换将作为从 `S?` 展开为 `S` 后，再从 `S` 到 `T`，再从 `T` 包装为 `T?`。
- 对于 `S` 到 `T?` 的转换，先从 `S` 到 `T`，再从 `T` 包装到 `T?`。
- 对于 `S?` 到 `T` 的转换，先是 `S?` 展开到 `S`，再从 `S` 到 `T` 的基本转换。

```csharp
long lnum = 10;    
int num = (int)lnum;  // (T)S to T
int? nnum = (int?)lnum;  // (T?)S to T?

long? l_num = lnum;   // T to T?
int? n_num = (int?)l_num;  // (T?)S? to T?
```

#### 10.2.4. 显式引用转换

显式引用转换是需要运行时检查以确保它们正确的引用类型之间的转换。如果显式引用转换失败，将引发 `System.InvalidCastException` 异常。

若要在运行时成功进行显式引用转换，源操作数的值必须为 `null`，或者源操作数引用的对象的实际类型必须是可通过隐式引用转换转换为目标类型的类型或装箱转换。

```csharp
object obj = "Hello";
string str = (string)obj;

obj = 123;
str = (string)obj;  // err : 源类型的运行时类型 int 无法隐式转换为 string
```

#### 10.2.5. 显式元组转换

如果元组表达式 `E` 与元组类型 `T` 具有相同的密度，且存在从 `E` 中的每个元素到 `T` 中相应元素类型的显式转换，则存在 `E` 到 `T` 的显式转换。转换通过创建 `System.ValueTuple<...>` 类型，并从左到右的顺序初始化它的每个字段，并对每一个元素应用显式转换。

如果元组表达式中的元素名与元组类型中相应的元素名不匹配，则发出警告，表达式的元素名称将被忽略。

```csharp
(int, string) t1 = (ValueTuple<int, string>)(1L, "One");
(byte, string) t2 = (ValueTuple<byte, string>)(2L, null);
(int, string) t3 = (ValueTuple<int, string>)(null, null);        // Error: No conversion
(int i, string s) t4 = (ValueTuple<int, string>)(d: 3.1415, "Four"); // Warning: Names are ignored
```

#### 10.2.6. 拆箱转换

拆箱转换允许将引用类型显式转换为值类型。拆箱转换操作包括：首先检查对象实例是否是给定值类型的装箱值，然后将该值从实例中复制出来。拆箱到可空值类型时，空引用生成为可空值类型的 `null` 值。拆箱空引用将引发 `System.NullReferenceException`

前提是引用类型是包含目标类型的装箱类型或兼容类型，否则将引发 `System.InvalidCastException` 异常。

#### 10.2.7. 显式动态转换

存在从 `dynamic` 到任何类型的 `T` 的显式动态转换，转换是动态绑定的，这意味着将在运行时检查表达式的运行时类型是否与目标类型存在显式转换。不存在任何转换时将产生异常。

#### 10.2.8. 涉及类型参数的显式转换

给定的类型参数 `T` 存在以下显式转换： 

- 对于已知为引用类型的类型参数 `T`，允许从 `T` 的任意有效基类 `C` 到 `T`、或任意接口到 `T` 的显式引用转换。
  
- 若 `T` 不知是否为引用类型时，涉及任意基类或基接口到 `T` 的转换在编译时被认为是拆箱转换。在运行时，如果 `T` 是值类型，则转换为拆箱转换执行，否则转换作为显式引用转换或恒等转换执行。

- 从 `U` 到类型参数 `T` 的转换，前提是 `T` 依赖于 `U`。在运行时，若 `T`、`U` 都是值类型，则 `T` 和 `U` 的类型必须相同，且不执行任何转换；若 `T` 是值类型，`U` 是引用类型，则转换将作为拆箱转换；否则 `T`、`U` 都是引用类型且转换将作为显式引用转换或恒等转换。

- 不允许从无约束的类型参数直接显式转换为非接口类型。此目的是为了防止语义混淆。

```csharp
//var a = Sample<int>.Fun(15);  // err
var a2 = Sample<long>.Fun(15);  // ok

class Sample<T>
{
    public static long Fun(T t)
    {
        // return (long)t;  // err
        return (long)(object)t;  // t 必须是 long
    }
}
```

#### 10.2.9. 用户定义的显式转换

用户定义的显式转换包括先可选的标准显式转换，然后执行用户定义的隐式或显式转换操作符，最后是另一个可选的标准显式转换。

>---

### 10.3. 用户定义转换

用户定义的转换将源表达式转换为另一种类型。用户定义转换的求值以查找源表达式或目标类型的最特定的用户定义转换运算符为核心。当确定了最特定的用户定义转换运算符，则用户定义的转换最多执行：
- （可选）首先，执行从源表达式到用户定义或提升的转换运算的操作数类型的标准转换。
- 接下来，调用用户定义或提升的转换运算符来执行转换。
- （可选）最后，执行从用户定义转换的结果类型到目标类型的标准转换。

标准转换指的是非用户定义的其他隐式或显式转换。用户定义的转换求值从不涉及多个用户定义或提升的转换运算符。也就是说，从类型 `S` 到 `T`，不会涉及先从 `S` 到 `X`，然后从 `X` 到 `T` 的转换。

使用关键字 `implicit`（隐式）和 `explicit`（显式）声明用户定义转换。定义转换的类型必须是该转换的源类型或目标类型。其中一个操作数必须是源类型。

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

>---

### 10.4. 匿名方法的转换

匿名方法表达式（`delegate`）和 Lambda 表达式或语句被归类为匿名方法。匿名方法没有类型，但可以隐式地转换为兼容的委托类型。一些 Lambda 表达式也可以转换为兼容的表达式树类型。

匿名方法 `F` 和委托类型 `D` 兼容，则：
- 若 `F` 包含一个匿名方法签名，则 `D` 和 `F` 具有相同数目的参数和返回类型。

```csharp
Action<int, string> dele = delegate (int x, string y) { };
Action<int, string> dele2 = delegate (long x, string y) { }; // err 签名不一致
```

- 若 `F` 不包含方法签名，则 `D` 可以有零到多个任意类型的参数，只要没有 `out` 修饰符。

```csharp
Action<int, string> dele = delegate { };
Action dele2 = delegate { };    // err : 委托参数中包含 out 参数

delegate void Action(out int val);
```

- 若 `F` 具有显式类型化的参数列表，则 `D` 的每个参数与 `F` 中相应的参数具有相同的类型和修饰符。

```csharp
Action dele2 = delegate (out int x, string str, ref float y) { x = 10; };   
delegate void Action(out int val, string str, ref float f);
```

- 若 `F` 具有隐式类型化的参数列表，则 `D` 中没有 `ref`、`in`、`out` 参数。

```csharp
Action dele = (x, str) => { x = str.Length; };
delegate void Action(int val, string str);
```

#### 10.4.1. 匿名方法转换到委托类型的求值

将匿名函数转换为委托类型会生成一个委托实例，该实例引用匿名函数并可能在转换时捕获处于活动状态的外部变量集（可能为空）。当调用委托时，将执行匿名函数体。主体中的代码使用委托引用捕获的外部变量集执行。

从匿名函数生成的委托的调用列表包含单个条目，其中委托的确切目标对象和目标方法未指定。特别是，委托的目标对象是空、封闭函数成员的 `this` 值还是其他对象都没有指定。

将语义相同的匿名函数与捕获的相同（可能为空的）外部变量实例集转换为相同的委托类型，允许（但不是必需）返回相同的委托实例。相同是指在所有情况下，在给定相同参数的情况下，匿名函数的执行将产生相同的效果。

```csharp
delegate double Function(double x);

class Test
{
    static double[] Apply(double[] a, Function f)
    {
        double[] result = new double[a.Length];
        for (int i = 0; i < a.Length; i++)
            result[i] = f(a[i]);
        return result;
    }

    static void F(double[] a, double[] b)
    {
        a = Apply(a, (double x) => Math.Sin(x));
        b = Apply(b, (double y) => Math.Sin(y));
        // 由于两个匿名函数委托具有捕获的外部变量的相同集，并且由于匿名函数在语
        // 义上是相同的，因此编译器允许委托引用相同的目标方法。实际上，允许编译
        // 器从两个匿名函数表达式返回完全相同的委托实例。
    }
}
```
 
#### 10.4.2. Lambda 表达式转换到表达式树类型的求值

将 Lambda 表达式转换为表达式树类型会生成表达式树类型，即 Lambda 表达式转换的结果产生一个表示 Lambda 表达式本身结构的对象结构。

并非每个 Lambda 表达式都可以转换为表达式树类型。始终存在到兼容委托类型的转换，但由于特定于实现的原因，它可能在编译时失败。常见的原因包括：
- Lambda 包含一个语句块。
- Lambda 是异步的。
- Lambda 包含 `in`、`out`、`ref` 参数。
- Lambda 包含赋值操作。
- Lambda 包含一个动态操作。

>---

### 10.5. 方法组的转换

存在从方法组 `E` 到兼容委托类型 `D` 的隐式转换。

```csharp
delegate string D1(object o);
delegate object D2(string s);
delegate object D3();
delegate string D4(object o, params object[] a);
delegate string D5(int i);

class Test
{
    static string Fun(object o) => "";
    static void Test()
    {
        D1 d1 = Fun;            // Ok
        D2 d2 = Fun;            // Ok
        D3 d3 = Fun;            // Error -- 不适用
        D4 d4 = Fun;            // Error -- 适用但不兼容
        D5 d5 = Fun;            // Error -- 不兼容
    }
}
```

>---

### 10.6. 帮助程序类转换

若要在非兼容类型（如整数和 `System.DateTime` 对象，或十六进制字符串和字节数组）之间转换，可使用 `System.BitConverter` 类、`System.Convert` 类和内置数值类型的 `Parse` 方法（如 `Int32.Parse`）。

> 字节数组和内置数据类型的互相转换（`BitConverter`）

```csharp
// ----- ToInt32 -----
byte[] bytes = { 0, 0, 0, 25 };
// If the system architecture is little-endian (that is, little end first),
// reverse the byte array.
if (BitConverter.IsLittleEndian)
    Array.Reverse(bytes);
int i = BitConverter.ToInt32(bytes, 0);
Console.WriteLine("int: {0}", i);  // 25

// ----- GetBytes -----
byte[] bytes = BitConverter.GetBytes(201805978);
Console.WriteLine("byte array: " + BitConverter.ToString(bytes)); // 9A-50-07-0C
```

---
## 11. 模式匹配

模式是一种语法形式，可以使用 `is` 表达式、`switch` 语句和 `switch` 表达式将输入表达式与任意数量的特征匹配。C# 支持多种模式，包括声明、类型、常量、关系、属性、列表、var 和弃元。可以使用布尔逻辑关键字 `and`、`or` 和 `not` 组合模式。

模式匹配类型：
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

>---

### 11.1. 模式匹配表达式

#### 11.1.1. is 模式

```csharp
bool rt_2 = E is <pattern>;

// exam
int i = 34;
object iBoxed = i;
int? jNullable = 42;
if (jNullable is not null)  // 检查 null
    if (iBoxed is int a && jNullable is int b)
        Console.WriteLine(a + b);  // output 76
```

#### 11.1.2. switch 语句

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

#### 11.1.3. switch 表达式

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

>---

### 11.2. 声明模式

使用声明模式检查表达式的运行时类型是否与给定类型兼容。声明模式的表达式结果 `E` 为非 null 且在运行时类型是 `T` 类型、或可隐式转换类型、或 `T` 的派生类型、或具有基础类型 `T` 的可为 null 的值类型、或存在从 `E` 的运行时类型到类型 `T` 的装箱或取消装箱转换，则模式匹配成功。  

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

>---

### 11.3. 类型模式

使用类型模式检查表达式 `E` 的运行时类型是否与给定类型 `T` 兼容。类型模式的表达式结果 `E` 为非 null 且在运行时类型是 `T` 类型、或可隐式转换类型、或 `T` 的派生类型、或具有基础类型 `T` 的可为 null 的值类型、或存在从 `E` 的运行时类型到类型 `T` 的装箱或取消装箱转换，则模式匹配成功。

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

>---

### 11.4. 常量模式

可使用常量模式来测试表达式结果是否等于指定的常量。

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

>---

### 11.5. 关系模式

可使用关系模式将表达式结果与常量进行比较。在关系模式中，可使用关系运算符 `<`、`>`、`<=` 或 `>=` 中的任何一个。关系模式的右侧部分必须是常数表达式。

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

>---

### 11.6. 逻辑模式

可使用 `not`、`and` 和 `or` 模式连结符来创建逻辑模式。其中优先级为 `not` > `and` > `or`。

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

>---

### 11.7. 属性模式

可以使用属性模式将表达式的属性或字段与嵌套模式进行匹配。

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

>---

### 11.8. 位置模式

可使用位置模式来解构表达式结果并将结果值与相应的嵌套模式匹配。

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

>---

### 11.9. var 模式

可使用 `var` 模式来匹配任何表达式（包括 `null`），并将其结果分配给新的局部变量。

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

>---

### 11.10. 弃元模式

可使用弃元模式 `_` 来匹配任何表达式，包括 `null`。

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

>---

### 11.11. 列表模式

从 C#11 开始，可以将数组或列表与模式的序列进行匹配。当每个嵌套模式与输入序列的相应元素匹配时，列表模式就会匹配。若要匹配任何元素，可使用弃元模式；若想要捕获元素，可使用 `var` 模式；若要仅匹配输入序列开头或 / 和结尾的元素，可使用切片模式 `..`，切片模式匹配零个或多个元素，最多可在列表模式中使用一个切片模式。

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
## 12. LINQ 和查询表达式

语言集成查询 LINQ 是一系列直接将查询功能集成到 C# 语言的技术统称。LINQ 最明显的 “语言集成” 部分就是查询表达式。

查询表达式采用声明性查询语法编写而成。使用查询语法，可以用最少的代码对数据源执行筛选、排序和分组操作。

```csharp
// Specify the data source.
int[] scores = { 97, 92, 81, 60 };

// Define the query expression.
IEnumerable<int> scoreQuery =
    from score in scores
    where score > 80
    select score;

// Execute the query.
foreach (int i in scoreQuery)
    Console.Write(i + " ");
// Output: 97 92 81
```

>---

### 12.1. 查询表达式概述

查询表达式可用于查询并转换所有启用了 LINQ 的数据源中的数据。例如，通过一个查询即可检索 SQL 数据库中的数据，并生成 XML 流作为输出。

查询表达式中的变量全都是强类型，可以使用匿名隐式变量声明。在编译时，查询表达式根据 C# 规范规则转换成标准查询运算符方法调用。可使用查询语法表示的任何查询都可以使用方法语法进行表示。

通常，在编写 LINQ 查询时尽量使用查询语法，并在必要时尽可能使用方法语法。查询表达式通常比使用方法语法编写的等同表达式更具可读性。一些查询操作（如 `Count` 或 `Max`）没有等效的查询表达式子句，因此必须表示为方法调用。

查询表达式可被编译成表达式树或委托，`IEnumerable<T>` 查询编译为委托。`IQueryable` 和 `IQueryable<T>` 查询编译为表达式树。

>---

### 12.2. 查询操作的三个部分

查询是一种从数据源检索数据的表达式，使用专门的查询语句来表示。LINQ 通过提供处理各种数据源和数据格式的数据的一致模型，简化了各种各样的查询语言（例如用于关系数据库的 SQL 和用于 XML 的 XQuery 等）。

在 LINQ 查询中，可以使用相同的基本编码模式来查询和转换 XML 文档、SQL 数据库、ADO\.NET 数据集、.NET 集合中的数据以及 LINQ 提供程序可用的任何其他格式的数据。

所有的 LINQ 查询操作都是由获取数据源、创建查询、执行查询三个部分的操作组成。

```csharp
// The Three Parts of a LINQ Query:
// 1. Data source.
int[] numbers = new int[7] { 0, 1, 2, 3, 4, 5, 6 };

// 2. Query creation.
// numQuery is an IEnumerable<int>
var numQuery =
    from num in numbers
    where (num % 2) == 0
    select num;

// 3. Query execution.
foreach (int num in numQuery)
    Console.Write("{0,1} ", num);
```

#### 12.2.1. 获取数据源

查询在 `foreach` 语句中执行，因此支持 `IEnumerable`、`IEnumerable<T>` 或派生接口（如泛型 `IQueryable<T>`）的类型称为可查询类型。可查询类型不需要进行修改或特殊处理就可以用作 LINQ 数据源。

```csharp
// Create a data source from an XML document.
// using System.Xml.Linq;
XElement contacts = XElement.Load(@"c:\myContactList.xml");
```

#### 12.2.2. 创建查询

查询指定要从数据源中检索的信息，可以指定在返回这些信息之前如何对其进行排序、分组和结构化。查询存储在查询变量中，并用查询表达式进行初始化。可以使用查询语法或方法语法来构造查询表达式。

```csharp
int[] numbers = { 5, 10, 8, 3, 6, 12 };
// 查询语法
var numQuery1 =
           from num in numbers
           where num % 2 == 0
           orderby num
           select num;

Console.WriteLine(string.Join(", ", numQuery1)); // 6, 8, 10, 12

// 方法语法
IEnumerable<int> numQuery2 = numbers
    .Where(num => num % 2 == 0)
    .OrderBy(n => n);

Console.WriteLine(string.Join(", ", numQuery2)); // 6, 8, 10, 12
```

#### 12.2.3. 执行查询

**查询延迟执行**：查询的实际执行将推迟到在 `foreach` 语句中循环访问查询变量之后进行。

```csharp
int[] numbers = { 5, 10, 8, 3, 6, 12 };

var numQuery =
     from num in numbers
     where (num % 2) == 0
     select num;

//  Query execution.
foreach (int num in numQuery)
    Console.Write("{0,1} ", num);
```

**强制立即执行**：对一系列源元素执行聚合函数的查询必须首先循环访问这些元素。`Count`、`Max`、`Average` 和 `First` 就属于此类查询。这些方法在执行时不使用显式 `foreach` 语句，它们常返回单个值。

要强制立即执行任何查询并缓存其结果，可调用 `ToList` 或 `ToArray` 方法。

```csharp
int[] numbers = { 5, 10, 8, 3, 6, 12 };

List<int> numQuery2 =
    (from num in numbers
     where (num % 2) == 0
     select num).ToList();

int[] numQuery3 =
    (from num in numbers
     where (num % 2) == 0
     select num).ToArray();
```

>---

### 12.3. 查询表达式

查询表达式是以查询语法表示的查询：
  - 以 `from` 子句开头，必须以 `select` 或 `group` 子句结尾。
  - 在第一个 `from` 子句与最后一个 `select` 或 `group` 子句之间，可以包含以下可选子句中的一个或多个：`where`、`orderby`、`join`、`let`，或者是其他 `from` 子句。
  - 可以使用 `into` 关键字，使 `join` 或 `group` 子句的结果充当相同查询表达式中的其他查询子句的数据源。

> 查询表达式示意图

![](./.img/LINQ%20查询表达式.png)


#### 12.3.1. from-in 子句：获取数据源

查询表达式以 `from` 子句开头，用以指定将在其上运行查询或子查询的数据源序列（source sequence），并表示源序列中每个元素的本地范围变量（local range variable）。

```csharp
from [type-name] <identifier> in <enumerable-expr>
```

`from` 数据源必须是 `IEnumerable` 或 `IEnumerable<T>`、`IQueryable<T>` 类型之一。范围变量和数据源已强类型化。

数据源实现 `IEnumerable<T>` 时，编译器推断范围变量的类型。若源是 `IEnumerable` 则需要显式指定范围变量的类型。

```csharp
// A simple data source.
int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

// Create the query.
// lowNums is an IEnumerable<int>
var lowNums = from num in numbers // 依据数据源的类型推断范围变量 num 为 int
              where num < 5
              select num;

// Execute the query.
foreach (int i in lowNums)
    Console.Write(i + " ");
```

若源序列的每个元素本身是一个序列或包含一个序列时，可以使用复合 `from` 子句。可以向任一的 `from` 子句添加 `where` 或 `orderby` 子句筛选结果。

```csharp
// A simple data source.
List<Student> students = new List<Student>
{
    new Student("Omelchenko", new List<int> {97, 72, 81, 60}),
    new Student ("O'Donnell", new List<int> {75, 84, 91, 39}),
    new Student ("Mortensen", new List<int> {88, 94, 65, 85}),
    new Student ("Garcia", new List<int> {97, 89, 85, 82}),
    new Student ("Beebe", new List<int> {35, 72, 91, 70})
};

// Create the query.
var scoreQuery = from student in students
                 from score in student.Scores   // from 子句
                 where score > 90
                 orderby score ascending   // 正序排序
                 select new { student = student.Name, score };

// Execute the query.
foreach (var s in scoreQuery)
    Console.WriteLine("{0} Score: {1}", s.student, s.score);

public record Student(string Name, List<int> Scores);
```

> 两个 from 子句的完全交叉联接

```csharp
// A simple data source.
char[] upperCase = { 'A', 'B', 'C' };
char[] lowerCase = { 'x', 'y', 'z' };

// Create the query.
var joinQuery =
           from lower in lowerCase
           where lower != 'x'
           from upper in upperCase
           select new { lower, upper };

// Execute the query.
foreach (var pair in joinQuery)
    Console.WriteLine("{0} is matched to {1}", pair.upper, pair.lower);

// Output
/*
    A is matched to y
    B is matched to y
    C is matched to y
    A is matched to z
    B is matched to z
    C is matched to z
 */
```

#### 12.3.2. where 子句：筛选

`where` 子句用在查询表达式中，用于指定将在查询表达式中返回数据源中的哪些元素。它使用一个布尔条件（谓词，predicate）应用于每个源元素并返回满足条件的元素。

```csharp
where <boolean-expr> 
```

`where` 子句是一种筛选极值，可以在查询表达式中的任何位置，除了不能是第一个或最后一个子句。

```csharp
var queryLowNums =
        from num in new int[] { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 }
        where num < 5
        where num % 2 == 0
        // 等效于
        // where num < 5 && num % 2 == 0
        select num;
```

#### 12.3.3. select 子句：选择与投影

在查询表达式中，`select` 子句指定在执行查询时产生的值的类型。根据计算所有以前的子句以及根据 `select` 子句本身的所有表达式得出结果。查询表达式必须以 `select` 子句或 `group` 子句结尾。

```csharp
select <expr>
```

`select` 子句常用于直接返回源数据，也可以用于将源数据转换（或投影）为新类型。

```csharp
var Numbers =
    from i in new int[] { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 }
    select new { x = i, sin = Math.Sin(i) };  // select 创建新类型

foreach (var i in Numbers)
    Console.WriteLine($"Sin({i.x}) = {i.sin:F6} ");
```

#### 12.3.4. group 子句：分组

`group` 子句返回一个 `IGrouping<TKey,TElement>` 对象序列，这些对象包含零个或更多与该组的键值匹配的项。`by` 用于指定应返回项的分组方式。

```csharp
group <TElem> by <TKey> [into <identifier>]
```

以 `group` 结尾的查询。

```csharp
// Query variable is an IEnumerable<IGrouping<char, Student>>
var studentQuery1 =
    from student in students
    group student by student.Last[0];

record Student(string First, string Last);
```

> `IGrouping<TKey, TElement>`

- 由于 `group` 查询产生的 `IGrouping<TKey,TElement>` 对象实质上是一个由列表组成的列表，因此必须使用嵌套的 `foreach` 循环来访问每一组中的各个项。
- 外部循环用于循环访问组键，内部循环用于循环访问组本身包含的每个项。组可能具有键，但没有元素。

```csharp
foreach (IGrouping<char, Student> studentGroup in studentQuery2)
{
     Console.WriteLine(studentGroup.Key);
     // Explicit type for student could also be used here.
     foreach (var student in studentGroup)
         Console.WriteLine("   {0}, {1}", student.Last, student.First);
 }
```

> 键 Key 类型和分组

- 组键可以是任何类型，如字符串、内置数值类型、用户定义的命名类型或匿名类型。

```csharp
// --------------- 源数据定义 -----------------
List<Student> students = new List<Student>
{
    new Student {First="Svetlana", Last="Omelchenko", ID=111, Scores= new List<int> {97, 72, 81, 60}},
    new Student {First="Claire", Last="O'Donnell", ID=112, Scores= new List<int> {75, 84, 91, 39}},
    new Student {First="Sven", Last="Mortensen", ID=113, Scores= new List<int> {99, 89, 91, 95}},
    new Student {First="Cesar", Last="Garcia", ID=114, Scores= new List<int> {72, 81, 65, 84}},
    new Student {First="Debra", Last="Garcia", ID=115, Scores= new List<int> {97, 89, 85, 82}}
};

// 字符串分组
var StringQuery =
    from student in students
    group student by student.Last;

// 布尔分组
var boolQuery = 
    from student in students
    group student by student.Scores.Average() > 80;  // 分为 true / false 两组

// 数值范围分组
var RangeQuery =
            from student in students
            let avg = (int)student.Scores.Average()
            group student by (avg / 10);  // 以 10 分为范围

record Student
{
    public required string First { get; init; }
    public required string Last { get; init; }
    public required int ID { get; init; }
    public required List<int> Scores { get; init; }
}  
```

#### 12.3.5. into 子句：附加查询

可使用 `into` 创建一个临时的标识符，并将 `group`、`join`、`select` 子句的结果存储到新的组中，该标识符成为附加查询命令的生成器。

```csharp
{select|join|group} into <identifier> {...}
```

> `into` 附加 `group`

```csharp
Random r = new Random();
int[] arr = new int[100];
for (int i = 0; i < 100; i++)
    arr[i] = r.Next(-100, 100);

var query =
    from num in arr
    group num by num / 10 into numGroup
    where numGroup.Count() > 0
    orderby numGroup.Key
    select new { Range = numGroup.Key * 10, Numbers = numGroup.ToArray() };

foreach (var item in query)
    Console.WriteLine($"Numbers in " +
        $"[{(item.Range < 0 ? item.Range - 10 : item.Range)}," +
        $"{(item.Range < 0 ? item.Range : item.Range + 10)})] " +
        $"are {string.Join(",", item.Numbers)}");
```

> `into` 附加 `select`

```csharp
Random r = new Random();
int[] arr = new int[100];
for (int i = 0; i < 100; i++)
    arr[i] = r.Next(-100, 100);

var query =
    from num in arr
    where num > 0
    orderby num
    select num into G
    group G by G / 10;

foreach (var item in query)
{
    Console.Write($"Numbers in [{item.Key * 10}, {(item.Key + 1) * 10}] are ");
    Console.WriteLine(string.Join(",", item));
}
```

#### 12.3.6. orderby 子句：中间件排序

在查询表达式中，`orderby` 子句可导致返回的序列或子序列（组）以升序或降序排序。排序操作基于一个或多个属性对序列的元素进行排序。第一个排序条件对元素执行主要排序。通过指定第二个排序条件，可以对每个主要排序组内的元素进行次要排序。

元素类型的默认比较器执行排序，默认排序顺序为升序（`ascending`），降序为 `descending`。还可以指定自定义比较器，但只适用于方法语法。

```csharp
orderby <Primary>[,<secondary>...] [ascending/descending: default= ascending]
```

> 对数据排序

- 主要升序排序。

```csharp
var query = from word in new string[] { "the", "quick", "brown", "fox", "jumps" }
            orderby word.Length
            select word;

Console.WriteLine(string.Join(", ", query));
// the, fox, quick, brown, jumps
```

- 主要降序排序。

```csharp
var query = from word in new string[] { "the", "quick", "brown", "fox", "jumps" }
            orderby word.Substring(0,1) descending
            select word;

Console.WriteLine(string.Join(", ", query));
// the, quick, jumps, fox, brown
```

- 次要升序排序。

```csharp
var query = from word in new string[] { "the", "quick", "brown", "fox", "jumps" }
            orderby word.Length, word.Substring(0, 1)
            select word;

Console.WriteLine(string.Join(", ", query));
// fox, the, brown, jumps, quick
```

- 次要降序排序。

```csharp
var query = from word in new string[] { "the", "quick", "brown", "fox", "jumps" }
            orderby word.Length ascending, word.Substring(0, 1) descending
            select word;

Console.WriteLine(string.Join(", ", query));
// the, fox, quick, jumps, brown
```

#### 12.3.7. join-in-on-equals 子句：联接

`join` 子句可用于将两个没有直接关系元素的源序列相关联（同等联接），要求每个序列中的元素具有能够与其他序列的相应属性进行比较的属性，或者包含一个这样的属性。`join` 子句使用 `equals` 关键字比较指定的键是否相等（值相等性）。

```csharp
join <inner-identifier> in <inner-sequence> on <outer-key> equals <inner-key> [into <identifier>]
```

`join` 子句的输出形式取决于执行的联接的具体类型：内部联接、分组联接、左外部联接。

```csharp
var categories = new[]
{
    new { Name = "A", ID = 101 },
    new { Name = "B", ID = 102 },
    new { Name = "C", ID = 103 },
    new { Name = "D", ID = 104 },
};
var products = new[]
{
    new { Name = "Apple", CategoryID = 101 },
    new { Name = "Football", CategoryID = 102 },
    new { Name = "Train", CategoryID = 103 },
    new { Name = "Banana", CategoryID = 101 },
    new { Name = "Car", CategoryID = 103 },
    new { Name = "Basketball", CategoryID = 102 },
};
// 内联联接
Console.WriteLine("----- inner Join Query -----");
var innerJoinQuery =
    from category in categories
    join prod in products on category.ID equals prod.CategoryID
    select new { ProductName = prod.Name, Category = category.Name }; //produces flat sequence
foreach (var product in innerJoinQuery)
    Console.WriteLine("{0} : {1}", product.Category, product.ProductName);

// 分组联接
Console.WriteLine("----- inner Group Join Query -----");
var innerGroupJoinQuery =
    from category in categories
    join prod in products on category.ID equals prod.CategoryID into prodGroup
    select new { CategoryName = category.Name, Products = prodGroup };
foreach (var item in innerGroupJoinQuery)
{
    Console.WriteLine($"{item.CategoryName} : ");
    foreach (var productName in item.Products.Select(p => p.Name))
        Console.WriteLine("    " + productName);
}

// 左外部联接
Console.WriteLine("----- left Outer Join Query -----");
var leftOuterJoinQuery =
    from category in categories
    join prod in products on category.ID equals prod.CategoryID into prodGroup
    from item in prodGroup.DefaultIfEmpty(new { Name = "Empty", CategoryID = 0 })
    select new { CatName = category.Name, ProdName = item.Name };
foreach (var product in leftOuterJoinQuery)
    Console.WriteLine("{0} : {1}", product.CatName, product.ProdName);
```

#### 12.3.8. let 子句：引入范围变量

在查询表达式中，可以通过 `let` 子句创建一个新的范围变量并通过提供的表达式结果初始化该变量。
  
```csharp
let <identifier> = <expr>
```
  
使用值进行初始化后，范围变量不能用于存储另一个值。但是，如果范围变量持有可查询类型（`IEnumerable`），则可以查询该变量。

```csharp
string[] strings =
{
    "A penny saved is a penny earned.",
    "The early bird catches the worm.",
    "The pen is mightier than the sword."
 };

// Split the sentence into an array of words
// and select those whose first letter is a vowel.
var earlyBirdQuery =
    from sentence in strings
    let words = sentence.Split(' ', '.', ',')  // words 是可查询类型
    from word in words
    where !string.IsNullOrEmpty(word)
    let w = word.ToLower()[0]  // word 首字母小写化
    where w == 'a' || w == 'e' || w == 'i' || w == 'o' || w == 'u'
    select word; // 若以韵母开头

// Execute the query.
Console.WriteLine("Words start with a vowel : " + string.Join(",", earlyBirdQuery));
```

>---

### 12.4. 标准查询运算符

标准查询运算符是组成 LINQ 模式的方法，这些方法中的大多数都作用于序列。其中序列指其类型实现 `IEnumerable<T>` 接口或 `IQueryable<T>` 接口的对象，`System.Linq.Enumerable` 的扩展方法作用于类型 `IEnumerable<T>` 的对象，`System.Linq.Queryable` 的扩展方法作用于类型 `IQueryable<T>` 的对象。

标准查询运算符提供包括筛选、投影、聚合、排序等在内的查询功能。各个标准查询运算符在执行时间上有所不同，返回单一实例值的方法（例如 `Average` 和 `Sum` 等）立即执行，返回序列的方法会延迟查询执行，并返回一个可枚举的对象。

对于在集合上运行的 `IEnumerable<T>` 的查询方法，返回可枚举对象，在枚举该对象时，将使用查询运算符的逻辑，并返回查询结果。而扩展 `IQueryable<T>` 的方法不会实现任何查询行为，它们生成一个表示要执行的查询的表达式树，源 `IQueryable<T>` 对象执行查询处理。

```csharp
string sentence = "the quick brown fox jumps over the lazy dog";
string[] words = sentence.Split(' ');

// Using query expression syntax.  
var query = from word in words
            group word.ToUpper() by word.Length into gr
            orderby gr.Key
            select new { Length = gr.Key, Words = gr };

// Using method-based query syntax.  
var query2 = words.
    GroupBy(word => word.Length, w => w.ToUpper()).
    OrderBy(gr => gr.Key).
    Select(gr => new { Length = gr.Key, Words = gr });

foreach (var obj in query)
{
    Console.WriteLine("Words of length {0}:", obj.Length);
    foreach (string word in obj.Words)
        Console.WriteLine(word);
}
```

> 查询表达式语法表

- `Cast`：使用显式类型化范围变量，相当于 `from <type> <variable> in <source-sequence>`。
- `GroupBy`：对应 `group ... by ... [into ...]`。
- `GroupJoin`：对应 `join ... in ... on ... equals ... into ...`。
- `Join`：对应 `join ... in ... on ... equals ...`。
- `OrderBy`：对应 `orderBy <... [ascending]>`。
- `OrderByDescending`：对应 `orderBy <... descending>`。
- `Select`：对应 `select <...>`。
- `SelectMany`：对应多个 `from` 子句。
- `ThenBy`：对应 `orderBy ..., <... [ascending]>`。
- `ThenByDescending`：对应 `orderBy ..., <... descending>`。
- `Where` 方法：对应 `where <...>`。

#### 12.4.1. 筛选数据：OfType、Where

筛选是指将结果集限制为仅包含满足指定条件的元素的操作。
  - `OfType<T>`：根据指定类型筛选 `IEnumerable` 或 `IQueryable` 的元素。
  - `Where`：基于谓词筛选值序列。对应于查询表达式的 `where` 子句。

> `ofType`

```csharp
// OfType
object?[] datas = { 1, "Mary", "Hello", 5.2f, "World", 15, "Ychao", null, 'A' };
IEnumerable<string> strs = datas.OfType<string>();
// Mary,Hello,World,Ychao
```

> `Where`

```csharp
string[] words = { "the", "quick", "brown", "fox", "jumps" };
var query = from w in words
            where w.Length > 2 select w;
// where 子句等效于
var queryFun = words.Where(str => str.Length > 3);
// quick,brown,jumps
```

#### 12.4.2. 投影运算：Select、SelectMany、Zip

投影是指将序列中的每个元素投影到新表单。可以构造从每个元素生成的新类型，或对其执行数学函数等：
  - `Select`：投影基于转换函数的值。对应于查询表达式的 `Select` 子句。
  - `SelectMany`：投影基于转换函数的值序列，然后将它们展平为一个序列。对应于查询表达式的多个 `from` 子句。
  - `Zip`：使用 2-3 个指定序列中的元素生成元组序列或用户定义序列，组合的序列长度不超过最短序列。

`Select` 与 `SelectMany` 的区别在于：`Select` 返回一个与源集合具有相同元素数目的集合；`SelectMany` 将中间数组序列串联为一个最终结果值，其中包含每个中间数组中的每个值。

> `Select`

```csharp
List<string> words = new() { "an", "apple", "a", "day" };

var query = from word in words
            select word.Substring(0, 1);
// 等效于
var queryFun = words.Select(word => word.Substring(0, 1));
// a,a,a,d
```

> `SelectMany`

```csharp
List<string> phrases = new() { "an apple a day", "the quick brown fox" };

IEnumerable<string> query = from phrase in phrases
            from word in phrase.Split(' ')
            select word;
// 等效于
IEnumerable<string> queryFun = phrases.SelectMany(selector: source => source.Split(' '));
// an,apple,a,day,the,quick,brown,fox
```

> Zip

- `Zip` 投影运算符有多个重载，可以投影为元组类型，或是用户定义类型。所有 `Zip` 方法都处理两个或更多可能是异构类型的序列。

```csharp
// An int array with 7 elements.
IEnumerable<int> numbers = new[] { 1, 2, 3, 4, 5, 6, 7 };
// A char array with 6 elements.
IEnumerable<char> letters = new[] { 'A', 'B', 'C', 'D', 'E', 'F' };

// 投影用户定义序列
var query = numbers.Zip(letters, resultSelector: (first, second) => new { number = first, letter = second });
// 投影元组
IEnumerable<(int First, char Second)> query2 = numbers.Zip(letters);
```

#### 12.4.3. 集合操作：Distinct、Except、Intersect、Union

LINQ 中的集合操作指的是生成结果集的查询操作，该结果集基于相同或不同集合（或集）中是否存在等效元素：
  - `Distinct`、`DistinctBy`：返回序列中的非重复元素。可以指定 `IEqualityComparer<T>` 对值进行比较；`DistinctBy` 可以指定键选择器函数。
  - `Except`、`ExceptBy`：返回差集，差集指位于一个集合但不位于另一个集合的元素。
  - `Intersect`、`IntersectBy`：返回交集，交集指同时出现在两个集合中的元素。元素不重复出现。
  - `Union`、`UnionBy`：返回并集，并集指位于两个集合中任一集合的唯一的元素。

```csharp
int[] numbers1 = { 1, 3, 4, 5, 5, 7, 8, 10, 20, 23, 45 };
int[] numbers2 = { 1, 1, 3, 5, 6, 9, 10, 12, 20, 25, 45 };

var queryDistinct = numbers1.Distinct();
// 非重复序列 : 1,3,4,5,7,8,10,20,23,45 
var queryExcept = numbers1.Except(numbers2);
// 差集 : 4,7,8,23
var queryIntersect = numbers1.Intersect(numbers2);  
// 交集 : 1,3,5,10,20,45
var queryUnion = numbers1.Union(numbers2);
// 并集 : 1,3,4,5,7,8,10,20,23,45,6,9,12,25
```

#### 12.4.4. 排序操作：OrderBy、ThenBy、Reverse

排序操作基于一个或多个属性对序列的元素进行排序。第一个排序条件对元素执行主要排序，可以通过指定第二个排序条件，对每个主要排序组内的元素进行次要排序：
  - `Order`：按升序对序列的元素进行排序，此方法使用默认比较器。可以显式定义比较器接口。
  - `OrderBy`：按升序对值主要排序。对应于查询表达式的 `orderby <...>`。
  - `OrderByDescending`：按降序对值主要排序。对应于查询表达式的	`orderby <... descending>`。
  - `ThenBy`：按升序对主要排序组内元素执行次要排序。对应于查询表达式的 `orderby ..., <...>`。
  - `ThenByDescending`：按降序对主要排序组内元素执行次要排序。对应于查询表达式的 `orderby ..., <... descending>`。
  - `Reverse`：反转集合中元素的顺序。

```csharp
string[] words = { "the", "quick", "brown", "fox", "jumps" };

var Order = words.Order();
// 默认排序 : brown, fox, jumps, quick, the

var OrderBy = from word in words
              orderby word.Length
              select word;
var OrderByFun = words.OrderBy(word => word.Length);
// 主要升序排序 : the,fox,quick,brown,jumps

var OrderByDescending = from word in words
                        orderby word.Length descending
                        select word;
var OrderByDescendingFun = words.OrderByDescending(word => word.Length);
// 主要降序排序 : quick,brown,jumps,the,fox

var ThenBy = from word in words
             orderby word.Length, word.Substring(0, 1)
             select word;
var ThenByFun = words.OrderBy(word => word.Length).ThenBy(word => word.Substring(0, 1));
// 主要升序次要升序 : fox,the,brown,jumps,quick

var ThenByDescending = from word in words
                       orderby word.Length, word.Substring(0,1) descending
                       select word;
var ThenByDescendingFun = words.OrderBy(word => word.Length).ThenByDescending(word => word.Substring(0, 1));
// 主要升序次要降序 : the,fox,quick,jumps,brown

var Reverse = words.Reverse();
// 反转集合 : jumps,fox,brown,quick,the
```

#### 12.4.5. 限定符运算：All、Any、Contains

限定符运算返回一个 `bool` 值，该值指示序列中是否有一些元素满足条件或是否所有元素都满足条件：
  - `All`：确定是否序列中的所有元素都满足条件。
  - `Any`：确定序列中是否存在元素，或有元素满足条件。
  - `Contains`：确定序列是否包含指定的元素。

```csharp
List<Market> markets = new List<Market>
{
    new Market { Name = "Emily's", Items = new string[] { "kiwi", "cheery", "banana" } },
    new Market { Name = "Kim's", Items = new string[] { "melon", "mango", "olive" } },
    new Market { Name = "Adam's", Items = new string[] { "kiwi", "apple", "orange" } },
};

var All = markets.Where(market => market.Items.All(item => item.Length == 5))
        .Select(market => market.Name + " market");   // Kim's market

var Any = markets.Where(market => market.Items.Any(item => item.StartsWith('o')))
        .Select(market => market.Name + " market");   // Kim's market, Adam's market

var Contains = markets.Where(market => market.Items.Contains("kiwi"))
        .Select(market => market.Name + " market");   // Emily's market, Adam's market
```

#### 12.4.6. 数据分区：Skip、Take、Chunk

LINQ 中的分区是指将输入序列划分为两个部分的操作，无需重新排列元素，然后返回其中一个部分：
  - `Skip`：省略序列从开头起 `Count` 个元素。
  - `SkipLast`：省略序列从末尾起 `Count` 个元素。
  - `SkipWhile`：基于谓词函数跳过元素，直到元素不符合条件时，返回后续不满足条件的元素。
  - `Take`：获取序列从开头起前 `Count` 个元素。
  - `TakeLast`：获取序列从末尾起 `Count` 个元素。
  - `TakeWhile`：基于谓词函数获取元素，直到元素不符合条件时，返回前面满足条件的元素。
  - `Chunk`：将序列的元素拆分为指定大小的区块。

```csharp
int[] numbers = { 1, 3, 5, 7, 9, 2, 4, 6, 8, 0 };

IEnumerable<int> Skip = numbers.Skip(3);
// 跳过前三个 : 7,9,2,4,6,8,0

IEnumerable<int> SkipLast = numbers.SkipLast(3);
// 省略后三个 : 1,3,5,7,9,2,4

IEnumerable<int> SkipWhile = numbers.SkipWhile(num => num <= 7);
// 直至不满足 <= 7 : 9,2,4,6,8,0

IEnumerable<int> Take = numbers.Take(3);
// 获取前 3 个 : 1,3,5

IEnumerable<int> TakeLast = numbers.TakeLast(3);
// 获取后 3 个 : 6,8,0

IEnumerable<int> TakeWhile = numbers.TakeWhile(num => num <= 7);
// 一直满足 <= 7 的前序列 : 1,3,5,7

IEnumerable<int[]> Chunk = numbers.Chunk(3);
// 三三分组 : (1,3,5), (7,9,2), (4,6,8), (0)
```

#### 12.4.7. 生成运算：DefaultIfEmpty、Empty、Range、Repeat

生成是指创建新的值序列：
  - `DefaultIfEmpty`：用默认值单一实例集合替换空集合。
  - `Enumerable.Empty`：返回一个空集合。
  - `Enumerable.Range`：生成包含数字序列的集合。
  - `Enumerable.Repeat`：生成包含一个重复值的集合。

```csharp
var Query = from length in Enumerable.Range(0, 40)
            let str = Enumerable.Repeat('口', length).ToArray()
            select (Len: length, str: str) into gr
            let str = gr.Len % 2 == 0 ? Enumerable.Empty<char>() : gr.str
            select str.DefaultIfEmpty('\n');

foreach (var chars in Query)
    foreach (var c in chars)
        Console.Write(c);
```

#### 12.4.8. 相等运算：SequenceEqual

两个序列，其相应元素相等且具有被视为相等的相同数量的元素：
  - `SequenceEqual`：通过以成对方式比较元素确定两个序列是否相等。长度不等或某一对元素不等，返回 `false`。

```csharp
int[] arr = { 1, 2, 3, 4, 5 };
List<int> list = new List<int>{ 1, 2, 3, 4, 5 };
var equals = arr.SequenceEqual(list);  // true
```

#### 12.4.9. 元素运算：ElementAt、First、Last、Single

元素运算从序列中返回唯一、特定的元素：
  - `ElementAt`：返回集合中指定索引处的元素。
  - `ElementAtOrDefault`：返回集合中指定索引处的元素；如果索引超出范围，则返回默认值。
  - `First`：返回集合的第一个元素或满足条件的第一个元素。
  - `FirstOrDefault`：返回集合的第一个元素或满足条件的第一个元素，不存在则返回默认值。
  - `Last`：返回集合的最后一个元素或满足条件的最后一个元素。
  - `LastOrDefault`：返回集合的最后一个元素或满足条件的最后一个元素，不存在时返回默认值。
  - `Single`：返回集合的唯一一个元素或满足条件的唯一一个元素。
  - `SingleOrDefault`：返回集合的唯一一个元素或满足条件的唯一一个元素，没有要返回的元素则返回默认值。

```csharp
int?[] numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9];

int? ElementAt = numbers.ElementAt(5);  // 6
int? ElememtAtOrDefault = numbers.ElementAtOrDefault(numbers.Count());  // null

int? First = numbers.First();   // 1
int? FirstPredicate = numbers.First(predicate: num => num > 3);  // 4
int FirstDefault = new int[0].FirstOrDefault();  // 0 

int? Last = numbers.Last();  // 9 
int? LastPredicate = numbers.Last(predicate: num => num < 5);  // 4
int? LastDefault = numbers.FirstOrDefault(predicate: num => num < 10);  // 1

// var Single = numbers.Single();  // error
int? Single = numbers.Single(predicate: num => num == 5);  // 5
int SingleDefault = new int[0].SingleOrDefault();  // 0
int? SingleDefault2 = numbers.SingleOrDefault(predicate: num => num > 100);  // null
```

#### 12.4.10. 转换数据类型：AsEnumerable、AsQueryable、OfType、ToArray、ToList、ToHashSet、ToDictionary、ToLookUp

转换方法可更改输入对象的类型：
  - `AsEnumerable`：返回类型化为 `IEnumerable<T>` 的输入。
  - `AsQueryable`：将 `IEnumerable` 转换为 `IQueryable` 或 `IEnumerable<T>` 转换为 `IQueryable<T>`。
  - `Cast`：将集合中的元素转换为指定类型。
  - `OfType`：根据其转换为指定类型的能力筛选值。
  - `ToArray`：将集合转换为数组。此方法将强制执行查询。
  - `ToList`：将集合转换为 `List<T>`。此方法强制执行查询。
  - `ToHashSet`：从 `IEnumerable<T>` 或 `IQueryable` 创建一个 `HashSet<T>`。
  - `ToDictionary`：根据键选择器函数将元素放入 `Dictionary<TKey,TValue>`。此方法强制执行查询。
  - `ToLookup`：根据键选择器函数将元素放入 `Lookup<TKey,TElement>`（一对多字典）。此方法强制执行查询。

```csharp
List<object> phrases = new List<object> { "an apple a day", "the quick brown fox" };
List<object> objs = new List<object> { 1, 2, 3, "World", 4, 5, 6, "Hello" };

IEnumerable<string> Cast = phrases.Cast<string>();

var OfTypeInt = objs.OfType<int>();  // 1,2,3,4,5,6
var OfTypeString = objs.OfType<string>();  // World,Hello

var SplitQuery = phrases.OfType<string>().SelectMany(phrase => phrase.Split(' '));
string[] ToArray = SplitQuery.ToArray();
List<string> ToList = SplitQuery.ToList();
HashSet<string> ToHashSet = SplitQuery.ToHashSet();
// an,apple,a,day,the,quick,brown,fox

Dictionary<char, string> ToDictionary = (from string phrase in phrases
                                         group phrase by phrase[0])
                                        .ToDictionary(keySelector: group => group.Key, elementSelector: group => group.ToArray()[0]);

ILookup<char, List<string>> ToLookup = (from string phrase in phrases
                                        from word in phrase.Split(' ')
                                        group word by word[0])
                                                  .ToLookup(keySelector: group => group.Key, elementSelector: g => g.ToList());
```

#### 12.4.11. 附加运算：Concat、Append、Prepend

串联是指将一个序列或元素附加到另一个序列的操作：
  - `Concat`：连接两个序列以组成一个序列。
  - `Append`：在序列尾端附加一个同类型元素。
  - `Prepend`：在序列的开头添加值。

```csharp
int[] arr = { 1, 2, 3, 4, 5 };

var Concat = arr.Concat(new int[] { 6, 7, 8, 9, 0 });
// 1,2,3,4,5,6,7,8,9,0

var Append = arr.Append(100);
// 1,2,3,4,5,100

var Prepend = arr.Prepend(-100);
// -100,1,2,3,4,5
```

#### 12.4.12. 聚合运算：Aggregate、Average、Count、LongCount、Max、Min、Sum

聚合运算从值的集合中计算出单个值：
  - `Aggregate`：对集合的值执行自定义聚合运算。
  - `Average`：计算值集合的平均值。
  - `Count`：对集合中元素计数，可选择仅对满足谓词函数的元素计数。
  - `LongCount`：对大型集合中元素计数，可选择仅对满足谓词函数的元素计数。
  - `Max`、`MaxBy`：确定集合中的最大值。
  - `Min`、`MinBy`：确定集合中的最小值。
  - `Sum`：对集合中的值求和。

```csharp
int[] numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 30, 40, 50];

var Aggregate = numbers.Aggregate(seed: 0, func: (Seed, num2) => Seed + num2);  // 累计相加：195
var Average = numbers[9..].Average(); // 第 9 索引起的后续元素的平均值：30

int Count = numbers.Count(predicate: num => num > 9);  // 大于 9 的元素数：5
long LongCount = numbers.LongCount();   // 14

var Max = numbers.Max();  // 50
var MinBy = numbers.MinBy(num => 1f / num);  // 50

var Sum = numbers.Sum(selector: num => num < 10 ? num : 0); // 小于 10 的元素和：45
```

#### 12.4.13. 联接运算：Join、GroupJoin

联接两个数据源就是将一个数据源中的对象与另一个数据源中具有相同公共属性的对象相关联：
  - `Join`：根据键选择器函数 `Join` 两个序列并提取值对。对应于查询表达式的 `join-in-on-equals` 子句。
  - `GroupJoin`：根据键选择器函数 `Join` 两个序列，并对每个元素的结果匹配项进行分组。对应于查询表达式的 `join-in-on-equals into ...` 子句。

```csharp
List<Product> products = new List<Product>
{
    new Product { Name = "Cola", CategoryId = 0 },
    new Product { Name = "Tea", CategoryId = 0 },
    new Product { Name = "Apple", CategoryId = 1 },
    new Product { Name = "Kiwi", CategoryId = 1 },
    new Product { Name = "Carrot", CategoryId = 2 },
};
List<Category> categories = new List<Category>
{
    new Category { Id = 0, CategoryName = "Beverage" },
    new Category { Id = 1, CategoryName = "Fruit" },
    new Category { Id = 2, CategoryName = "Vegetable" }
};

var Join = from product in products
           join category in categories on product.CategoryId equals category.Id
           select (product: product.Name, category: category.CategoryName);
var JoinFun = products.Join(categories,
            outer => outer.CategoryId, inner => inner.Id,
            (outer, inner) => (product: outer.Name, category: inner.CategoryName));

if (Join.SequenceEqual(JoinFun))
    Console.WriteLine("Join is equal to JoinFun ");  // True

var GroupJoin = from category in categories
                join product in products on category.Id equals product.CategoryId into Group
                select new { Category = category.CategoryName, Group = Group.ToList() };
var GroupJoinFun = categories.GroupJoin(products,
            outer => outer.Id, inner => inner.CategoryId,
            (category, inners) => new { Category = category.CategoryName, Group = inners.ToList() });
// -------------------------------------------------------
record struct Product(string Name, int CategoryId);
record struct Category(int Id, string CategoryName);
```

> 执行内部联接

- 在关系数据库术语中，内部联接会生成一个结果集，在该结果集中，第一个集合的每个元素对于第二个集合中的每个匹配元素都会出现一次。如果第一个集合中的元素没有匹配元素，则它不会出现在结果集中：
  - 简单键联接：基于简单键使两个数据源中的元素相关联的简单内部联接。
  - 复合键联接：基于复合键使两个数据源中的元素相关联的内部联接。复合键是由多个值组成的键，可以基于多个属性使元素相关联。
  - 多联接：可以将任意数量的联接操作相互追加，以执行多联接。
  - 使用分组联接实现的内部联接。

```csharp
// ===================== 简单键联接 =======================
using System;
using System.Drawing;

Person magnus = new("Magnus", "Hedlund");
Person terry = new("Terry", "Adams");
Person charlotte = new("Charlotte", "Weiss");
Person arlene = new("Arlene", "Huff");
Person rui = new("Rui", "Raposo");
List<Person> people = new() { magnus, terry, charlotte, arlene, rui };
List<Pet> pets = new List<Cat>()
{
    new("Barley", terry),
    new("Boots", terry),
    new("Whiskers", charlotte),
    new("Blue Moon", rui),
    new("Daisy", magnus),
}.Cast<Pet>().ToList();
var SampleKeyQuery = from person in people
                     join pet in pets on person equals pet.Owner
                     select new { Owner = person.FirstName, PetName = pet.Name };
var SampleKeyQueryFun = people.Join(pets,
     outer => outer, inner => inner.Owner,
     (outer, inner) => new { Owner = outer.FirstName, PetName = inner.Name });
if (SampleKeyQuery.SequenceEqual(SampleKeyQueryFun))
    foreach (var ownerAndPet in SampleKeyQuery)
        Console.WriteLine($"\"{ownerAndPet.PetName}\" is owned by {ownerAndPet.Owner}");
/** Output
    "Daisy" is owned by Magnus
    "Barley" is owned by Terry
    "Boots" is owned by Terry
    "Whiskers" is owned by Charlotte
    "Blue Moon" is owned by Rui
*/

// ===================== 复合键联接 =======================
List<Employee> employees = new()
{
    new(FirstName: "Terry", LastName: "Adams", EmployeeID: 522459),
    new("Charlotte", "Weiss", 204467),
    new("Magnus", "Hedland", 866200),
    new("Vernette", "Price", 437139)
};
List<Student> students = new()
{
    new(FirstName: "Vernette", LastName: "Price", StudentID: 9562),
    new("Terry", "Earls", 9870),
    new("Terry", "Adams", 9913)
};
var CompositeKeyQuery = from employee in employees
                        join student in students on new { employee.FirstName, employee.LastName } 
                        equals new { student.FirstName, student.LastName }
                        select employee.FirstName + " " + employee.LastName;
var CompositeKeyQueryFun = employees.Join(students,
    employee => new { employee.FirstName, employee.LastName },
    student => new { student.FirstName, student.LastName },
     (e, s) => e.FirstName + " " + e.LastName);
if (CompositeKeyQuery.SequenceEqual(CompositeKeyQueryFun))
    foreach (var person in CompositeKeyQuery)
        Console.WriteLine(person);
/** Output
    Terry Adams
    Vernette Price
 */

// ===================== 多联接 =======================
Person phyllis = new("Phyllis", "Harris");
people.Add(phyllis);
List<Cat> cats = pets.Cast<Cat>().ToList();
List<Dog> dogs = new()
{
    new(Name: "Four Wheel Drive", Owner: phyllis),
    new("Duke", magnus),
    new("Denim", terry),
    new("Wiley", charlotte),
    new("Snoopy", rui),
    new("Snickers", arlene),
};
var MultipleJoinQuery = from person in people
                        join cat in cats on person equals cat.Owner
                        join dog in dogs on new { Owner = person, Letter = cat.Name.Substring(0, 1) }
                        equals new { dog.Owner, Letter = dog.Name.Substring(0, 1) }
                        select new { CatName = cat.Name, DogName = dog.Name };
var MultipleJoinQueryFun = people
            .Join(cats, per => per, cat => cat.Owner, (person, cat) => new { person, cat })
            .Join(dogs, onwer => new { Owner = onwer.person, Letter = onwer.cat.Name.Substring(0, 1) },
                        dog => new { dog.Owner, Letter = dog.Name.Substring(0, 1) },
                        (onwer, dog) => new { CatName = onwer.cat.Name, DogName = dog.Name });
if (MultipleJoinQuery.SequenceEqual(MultipleJoinQueryFun))
    foreach (var Pet in MultipleJoinQuery)
        Console.WriteLine($"Cat: {Pet.CatName} & Dog: {Pet.DogName} have the same owners");
/** Output
    Cat: Daisy & Dog: Duke have the same owners
    Cat: Whiskers & Dog: Wiley  have the same owners
 */

// =====================  使用分组联接的内联 =======================
var GroupJoinQuery =
    from person in people
    join pet in pets on person equals pet.Owner into gj
    from subpet in gj
    select new
    {
        OwnerName = person.FirstName,
        PetName = subpet.Name
    };
var GroupJoinQueryFun = people.GroupJoin(pets,
        person => person, pet => pet.Owner, (person, pets) => new { person, gj=pets })
        .SelectMany(pet => pet.gj,
       (groupJoinPet, subpet) => new { OwnerName = groupJoinPet.person.FirstName, PetName = subpet.Name });
if (GroupJoinQuery.SequenceEqual(GroupJoinQueryFun))
    foreach (var v in GroupJoinQuery)
        Console.WriteLine($"{v.OwnerName} - {v.PetName}");
/** Output
    Magnus - Daisy
    Terry - Barley
    Terry - Boots
    Charlotte - Whiskers
    Rui - Blue Moon
 */

// -----------------------------------------------------------
; record Person(string FirstName, string LastName);
record Pet(string Name, Person Owner);
record Employee(string FirstName, string LastName, int EmployeeID);
record Student(string FirstName, string LastName, int StudentID);
record Cat(string Name, Person Owner) : Pet(Name, Owner);
record Dog(string Name, Person Owner) : Pet(Name, Owner);
```

> 执行分组联接

- 分组联接对于生成分层数据结构十分有用，它将第一个集合中的每个元素与第二个集合中的一组相关元素进行配对。
- 第一个集合的每个元素都会出现在分组联接的结果集中（无论是否在第二个集合中找到关联元素）。 在未找到任何相关元素的情况下，该元素的相关元素序列为空。 
  
```csharp
var categories = new[]
{
    new { Name = "A", ID = 101 },
    new { Name = "B", ID = 102 },
    new { Name = "C", ID = 103 },
    new { Name = "D", ID = 104 },
};
var products = new[]
{
    new { Name = "Apple", CategoryID = 101 },
    new { Name = "Football", CategoryID = 102 },
    new { Name = "Train", CategoryID = 103 },
    new { Name = "Banana", CategoryID = 101 },
    new { Name = "Car", CategoryID = 103 },
    new { Name = "Basketball", CategoryID = 102 },
};
var innerGroupJoinQuery =
    from category in categories
    join prod in products on category.ID equals prod.CategoryID into prodGroup
    select new { CategoryName = category.Name, Products = prodGroup.DefaultIfEmpty(new { Name = "Empty", CategoryID = 0 }) };
var innerGroupJoinQueryFun = categories.GroupJoin(products, cate => cate.ID, prod => prod.CategoryID,
            (cate, gr) => new { CategoryName = cate.Name, Products = gr.DefaultIfEmpty(new { Name = "Empty", CategoryID = 0 }) });

foreach (var item in innerGroupJoinQueryFun)
{
    Console.Write($"\n{item.CategoryName} : ");
    foreach (var productName in item.Products.Select(p => p.Name))
        Console.Write(productName + "  ");
}
/*
    A : Apple  Banana
    B : Football  Basketball
    C : Train  Car
    D : Empty
 */
```

> 执行左外部联接

- 左外部联接：返回第一个集合的每个元素，无论该元素在第二个集合中是否有任何相关元素。

```csharp
// 改写分组联接的查询变量
var leftOuterJoins =
    from category in categories
    join prod in products on category.ID equals prod.CategoryID into prodGroup
    from p in prodGroup.DefaultIfEmpty(new { Name = "", CategoryID = 0 })
    select new { CategoryName = category.Name, Product = p };

var leftOuterJoinsFun = categories
            .GroupJoin(products,
                       cate => cate.ID,
                       prod => prod.CategoryID,
                       (cate, gr) => new { cate, gr })
            .SelectMany(prods => prods.gr.DefaultIfEmpty(new { Name = "", CategoryID = 0 }),
                       (cate, prod) => new { CategoryName = cate.cate.Name, Product = prod });
foreach (var v in leftOuterJoinsFun)
    Console.WriteLine($"{v.CategoryName + ":",-15}{v.Product.Name}");
/*
    A:             Apple
    A:             Banana
    B:             Football
    B:             Basketball
    C:             Train
    C:             Car
    D:
 */
```

> 执行交叉联接

- 谨慎使用交叉联接，因为它们可能会生成非常大的结果集。

```csharp
int[] numbers = { 1, 2, 3, 4 };
string[] strings = { "World", "Hello" };

var CrossJoinQuery = from number in numbers
                     from str in strings
                     select (number, str);
Console.WriteLine(string.Join(", ", CrossJoinQuery));
// (1, World), (1, Hello), (2, World), (2, Hello), (3, World), (3, Hello), (4, World), (4, Hello)
```

> 执行非同等联接

```csharp
var categories = new[]
{
    new { Name = "A", ID = 101 },
    new { Name = "B", ID = 102 },
};
var products = new[]
{
    new { Name = "Apple", CategoryID = 101 },
    new { Name = "Football", CategoryID = 102 },
    new { Name = "Train", CategoryID = 103 },
    new { Name = "Banana", CategoryID = 101 },
    new { Name = "Car", CategoryID = 103 },
    new { Name = "Basketball", CategoryID = 102 },
};

var nonEquiJoinQuery = from p in products
                       let cates = from c in categories
                                   select c.ID
                       where cates.Contains(p.CategoryID) == true
                       orderby p.CategoryID
                       select p;
foreach (var v in nonEquiJoinQuery)
    Console.WriteLine($"{v.CategoryID}  {v.Name}");
/** Output
    101  Apple
    101  Banana
    102  Football
    102  Basketball
 */
```

#### 12.4.14. 数据分组：GroupBy、ToLookUp

分组是指将数据分到不同的组，使每组中的元素拥有公共的属性：
  - `GroupBy`：对共享通用属性的元素进行分组。每组由一个 `IGrouping<TKey,TElement>` 对象表示。对应于查询表达式的 `group-by` 或 `group-by-into` 子句。
  - `ToLookUp`：将元素插入基于键选择器函数的 `Lookup<TKey,TElement>`（一种一对多字典）。

```csharp
List<int> numbers = new List<int>() { 35, 44, 200, 84, 3987, 4, 199, 329, 446, 208 };
// ========= GroupBy
var query = from number in numbers
            orderby number
            group number by number % 2 into gr
            select new { Key = gr.Key == 0 ? "Even Numbers" : "Odd NUmbers", Numbers = gr.ToList() };
var queryFun = numbers
            .OrderBy(num => num)
            .GroupBy(num => num % 2)
            .Select(gr => new { Key = gr.Key == 0 ? "Even Numbers" : "Odd NUmbers", Numbers = gr.ToList() });
foreach (var group in query)
    Console.WriteLine($"{group.Key} : {string.Join(",", group.Numbers)}");
// Even Numbers : 4,44,84,200,208,446
// Odd NUmbers : 35,199,329,398

// ========= LookUp
Lookup<string, List<int>> queryToLookUp = numbers
            .OrderBy(num => num)
            .GroupBy(num => num % 2)
            .ToLookup(gr => gr.Key == 0 ? "Even Numbers" : "Odd NUmbers", gr => gr.ToList())
            as Lookup<string, List<int>>;
foreach (var group in queryToLookUp)
    foreach (var list in queryToLookUp[group.Key])
        Console.WriteLine($"{group.Key} : {string.Join(",", list)}");
// Even Numbers : 4,44,84,200,208,446
// Odd NUmbers : 35,199,329,3987
```

---
## 13. 集合表达式

可以使用集合表达式来创建常见的集合值。集合表达式在 `[` 和 `]` 括号之间包含元素的序列。可以为一维数组类型、`System.Span<T>` 和 `System.ReadOnlySpan<T>`、支持集合初始化设定项的类型（例如 `System.Collections.Generic.List<T>` 等）使用集合表达式语法。

```csharp
Span<string> weekDays = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
foreach (var day in weekDays)
    Console.WriteLine(day);

int[] arr = [10, 20, 30];
List<int> list = [1, 2, 3, 4, 5];
```

>---

### 13.1. 内联集合值

可以使用展开运算符 `..` 在集合表达式中使用内联集合值。

```csharp
int[] left = [1, 2, 3, 4];
int[] right = [5, 6, 7, 8, 9];
int[] all = [.. left, .. right, 0];
Console.WriteLine(string.Join(",", all)); 
// output: 1,2,3,4,5,6,7,8,9,0
```

>--- 

### 13.2. 集合表达式转换

集合表达式可转换为单维数组类型 `T[]`。

```csharp
int[] arr = [1, 2, 3, 4, 5];
int[][] arr2 = [[10, 20, 30], [0], arr];      // 交错数组
```

集合表达式可转换为 `Span<T>` 或 `ReadOnlySpan<T>` 类型。

```csharp
Span<int> arr = [1, 2, 3, 4, 5];
ReadOnlySpan<int> arr2 = [.. arr, 6, 7, 8, 9];
Span<int> arr3 = [.. arr2, 0];
Console.WriteLine(string.Join(",", arr3.ToArray()));
// output: 1,2,3,4,5,6,7,8,9,0
```

集合表达式可转换为实现 `IEnumerable<T>` 接口并拥有一个公共或扩展定义的 `Add` 方法。

```csharp
ReadOnlyArray<int> arr = [1, 2, 3, 4, 5, 6];
foreach (int i in arr)
    Console.WriteLine(i);

struct ReadOnlyArray<T>() : IEnumerable<T>
{
    private readonly T[] values = new T[100];
    private int Length = 0;
    public void Add(T value)
    {
        if (Length > values.Length - 1)
            throw new ArgumentOutOfRangeException("Capacity");
        values[Length++] = value;
    }
    public IEnumerator<T> GetEnumerator() => values[..^(values.Length - Length)].AsEnumerable().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => values[..^(values.Length - Length)].GetEnumerator();
}
```

集合表达式可转换为实现 `IEnumerable` 接口并拥有一个公共或扩展定义的 `Add` 方法。

```csharp
InlineBuffer arr = [1, 2, "Hello", 4, 3.1415, 6];
foreach (int i in arr)
    Console.WriteLine(i);

class InlineBuffer : IEnumerable
{
    public readonly List<object> buffer = new List<object>(80);
    public void Add(object value) => buffer.Add(value);
    public IEnumerator GetEnumerator() => buffer.GetEnumerator();
}
```

集合表达式可转换为实现任何继承 `System.Collections.IEnumerable` 或 `System.Collections.Generic.IEnumerable<T>` 接口的接口的类型。

```csharp
MyList<int> arr = [1, 2, 3, 4, 5];

class MyList<T> : IReadOnlyList<T>  // IReadOnlyList<T> : IEnumerable<T>
{
    public T this[int index] => throw new NotImplementedException();
    public int Count => throw new NotImplementedException();
    public IEnumerator<T> GetEnumerator() => throw new NotImplementedException();
    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    public void Add(T t) { }
}
```

>---

### 13.3. 集合表达式实例的构造过程

当目标类型是实现 `System.Collections.IEnumerable` 接口的结构或类类型，且目标类型没有类似 `Create` 集合生成器的方法，则集合实例的构造如下：
- 元素按顺序求值，部分或所有元素可能在下面的步骤中求值。编译器可以通过调用每个扩展元素表达式上的 `Length` 或 `Count` 属性来确定集合表达式的已知长度。
- 调用不带参数的构造函数。
- 对于每个元素，按顺序：
  - 如果元素是表达式元素，则使用元素表达式作为参数调用适用的 `Add` 方法或扩展方法。
  - 如果元素是展开元素（`.. Expr`），则使用以下方法之一：
    - 在扩展元素表达式上调用适用的 `GetEnenumerator` 实例或扩展方法，对于来自枚举器的每个枚举项，在集合实例上调用适用的 `Add` 实例或扩展方法，并将该项作为参数。如果枚举实现了 `IDisposable`，那么在枚举之后将调用 `Dispose`。
    - 在使用扩展元素表达式作为参数的集合实例上调用适用的 `AddRange` 实例或扩展方法。
    - 使用集合实例和 `int` 索引作为参数，在扩展元素表达式上调用适用的 `CopyTo` 实例或扩展方法。
- 在上面的构造步骤中，可以在带有 `int Capacity` 参数的集合实例上调用一次或多次适用的 `EnsureCapacity` 实例或扩展方法。

当目标类型是数组、`Span` 类型、具有 `Create` 集合生成器的类型或接口，则集合实例的构造如下：
- 元素按顺序求值。部分或所有元素可能在下面的步骤中求值。编译器可以通过调用每个扩展元素表达式上的可计数属性来确定集合表达式的已知长度。
- 创建初始化实例的方法如下：
  - 如果目标类型是一个数组，并且集合表达式的长度已知，则分配一个具有预期长度的数组。
  - 如果目标类型是 `Span` 或具有 `Create` 方法的类型，并且集合具有已知的长度，则将引用连续存储创建具有预期长度的 `Span`。
  - 否则分配中间存储。
- 对于每个元素按顺序：
  - 如果元素是表达式元素，则调用初始化实例索引器将计算的表达式添加到当前索引处。
  - 如果该元素是展开元素，则使用以下方法之一：
    - 调用已知接口或类型的成员将项从扩展元素表达式复制到初始化实例。
    - 在扩展元素表达式上调用适用的 `GetEnenumerator` 实例或扩展方法，对于枚举器中的每个项，调用初始化实例索引器以在当前索引处添加项。如果枚举数实现了 `IDisposable`，那么在枚举之后将调用 `Dispose`。
    - 使用初始化实例和 `int` 索引作为参数，在扩展元素表达式上调用适用的 `CopyTo` 实例或扩展方法。
- 如果为集合分配了中间存储，则为集合实例分配实际集合长度，并将初始化实例中的值复制到集合实例中，或者如果需要一个 `Span`，编译器可能会使用中间存储中实际集合长度的 `Span`。否则初始化实例就是集合实例。
- 如果目标类型具有创建方法，则使用 `Span` 实例调用该创建方法。

>---

### 13.4. 为 IDictionary 类型扩展集合表达式构造语法

```csharp
Dictionary<int, int> ArrDic = [(1, 1), (2, 2), (3, 3)];
public static class KYExt
{
    public static void Add<TK, TV>(this IDictionary<TK, TV> Dic, (TK key, TV value) KV)
    {
        if (Dic.ContainsKey(KV.key))
            return;
        Dic.Add(KV.key, KV.value);
    }
}
```

>---

### 13.5. 集合生成器
 
集合表达式还可转换具有集合生成器的类型。类型通过编写绑定名称的生成器方法（例如 `Create`）和对集合类型应用 `System.Runtime.CompilerServices.CollectionBuilderAttribute` 来指示生成器方法来选择加入集合表达式支持。集合类型必须具有迭代器 `GetEnumerator`。

> 设计一个可使用集合表达式的集合类型

- 首先必须将 `CollectionBuilderAttribute` 属性添加到需要使用集合表达式构造的集合类型上，并指定集合生成器类和构造器方法的名称。生成器必须是非泛型类或结构，生成器方法必须是 `static` 并使用 `ReadOnlySpan<T>` 为唯一参数，以集合元素类型为 `Span` 类型。
- `ReadOnlySpan` 参数可以显式的声明为 `scoped` 或 `[UnscopedRef]`，如果参数隐式或显式地限定了作用域，则编译器可能会在堆栈而不是堆上为 `Span` 分配存储空间。

```csharp
[CollectionBuilder(typeof(MyCollectionBuilder), "Build")]
public class MyCollection
{
    public readonly int[] Values;
    public MyCollection(int[] arr) => Values = arr;

    internal class MyCollectionBuilder
    {
        internal static MyCollection Build(ReadOnlySpan<int> arr) => new MyCollection(arr.ToArray());
    }
}
```

- 集合类型需要一个 `IEnumerator<T> GetEnumerator()` 方法为其集合元素提供迭代功能。也可以继承 `IEnumerable` 或 `IEnumerable<T>`。

```csharp
public class MyCollection
{
    //....
    public IEnumerator<int> GetEnumerator() => Values.AsEnumerable().GetEnumerator();
    //....
}
```

- 使用集合表达式初始化该集合类型。

```csharp
MyCollection arr = [1,2,3,4];
```

#### 13.5.1. 泛型集合生成器

`CollectionBuilderAttribute` 特性指定的集合生成器必须是非泛型类或结构，生成器方法是可以使用类型参数的。声明集合生成器的类不能嵌套在泛型类型中。

```csharp
using System.Runtime.CompilerServices;

MyCollection<int> arr = [1, 2, 3, 4, 5];

[CollectionBuilder(typeof(MyCollectionBuilder), "Build")]
public record MyCollection<T>(params T[] Values)
{
    public IEnumerator<T> GetEnumerator() => Values.AsEnumerable().GetEnumerator();
}
internal class MyCollectionBuilder
{
    internal static MyCollection<T> Build<T>(ReadOnlySpan<T> arr) => new MyCollection<T>(arr.ToArray());
}
```

---
## 14. 索引与范围运算符

索引和范围运算符可以在序列的访问器中使用：
- `^`（从末尾开始索引）：指示元素位置来自序列的末尾，`[^n] = [Length - n]`。
- `..`（范围）：指定可用于获取一系列序列元素的索引范围。`..` 运算符指定某一索引范围的开头和末尾作为其操作数，左侧操作数是范围的包含性开头，右侧操作数是范围的不包含性末尾。`[a..]` 等效于 `[a..^0]`、`[..b]` 等效于 `[0..b]`、`[..]` 等效于 `[0..^0]`。

```csharp
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

>---

### 14.1. Index 与 Range 

索引 `System.Index` 和范围 `System.Range` 为访问序列中的单个元素或范围提供了简洁的语法。表达式 `^0` 属于 `System.Index` 类型，表达式 `a..b` 属于 `System.Range` 类型。若任何类型提供带 `Index` 或 `Range` 参数的索引器，则该类型可分别显式支持索引 `^n` 或范围 `a..b`。

单维度数组、交错数组、`String`、`Span<T>` 和 `ReadOnlySpan<T>` 同时支持索引和范围。`List<T>` 仅支持索引。

```csharp
System.Range r = 0..5;
System.Index i = ^1;

string str = "Hello, World";
string hello = str[0..5];  // str[r]

List<int> arr = [0, 1, 2, 3, 4, 5];
var last = arr[^1];    // arr[i]
```

>---

### 14.2. 索引和范围的类型支持

若任何类型提供带 `Index` 或 `Range` 参数的索引器，则该类型可分别显式支持索引或范围。

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

>---

### 14.3. 索引和范围的隐式支持

若某个类型具有一个名为 `int Length{ get;}` 或 `int Count{ get;}` 实例属性和一个 `T this[int index]` 实例索引器，并且没有仅以 `System.Index` 类型为索引的索引器时，该类型隐式支持索引运算。首选使用 `Length`。

若某个类型具有一个名为 `int Length{ get;}` 实例属性和一个 `public T[] Slice(int start, int length)` 实例方法，并且没有仅以 `System.Range` 为索引的索引器时，该类型隐式支持范围运算。

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

>---

---
## 15. 指针相关的运算符

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