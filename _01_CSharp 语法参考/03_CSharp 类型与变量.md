# CSharp 类型与变量

<!-- 值类型和引用类型是 C# 类型的两个主要类别：
值类型的变量包含类型的实例。对于值类型变量，会复制相应的类型实例。
引用类型的变量存储对其数据（对象）的引用。对于引用类型，两种变量可引用同一对象，对一个变量执行的操作会影响另一个变量所引用的对象。 -->

C# 的类型主要分为引用类型和值类型两大类，它们可以是泛型类型，并接受一个或多个类型参数。值类型和引用类型的区别在于，值类型的变量直接包含其数据，引用类型的变量存储对其数据（对象）的引用。

对于引用类型，两个变量可能引用同一个对象，因此对一个变量的操作可能影响到另一个变量引用的对象。对于值类型，每个变量都有其自己的数据副本，对一个变量的操作不可能影响另一个变量。

变量表示存储位置，每个变量都表示一个类型，用于确定可以在变量中存储的值，可以通过赋值或运算符操作改变变量的值。在获取变量前必须明确赋值。

当变量是 `ref` 或 `out` 形参时，它没有自己的存储空间，而是引用另一个变量的存储空间。实际上，引用参数的变量实际上是另一个变量的别名，而不是一个独立的变量。

C# 的类型系统是统一的，即任何类型的值都可以被视为对象，每个类型都直接或间接地派生自 `object` 类型，`object` 所有类型的最终基类。引用类型的值可以直接转换为 `object`，而值类型的值需要装箱操作转换为 `object`，反之 `object` 转换为原值类型则需要拆箱操作。

---
## 引用类型

### Class 类类型

类类型定义了一个数据结构，其中包含了数据成员（常量和字段）、函数成员（方法、属性、事件、索引器、运算符、实例构造函数、静态构造函数和终结器）和嵌套成员。类类型支持继承，这是一种派生类可以扩展和专门化基类的机制。

预定义的类类型在 C# 中具有特殊含义：
- `System.Object`：所有其他类型的最终基类。
- `System.String`：C# 字符串类型。
- `System.ValueType`：所有值类型的基类。
- `System.Enum`：所有枚举类型的基类。
- `System.Array`：所有数组类型的基类。
- `System.Delegate`：所有委托类型的基类。
- `System.Exception`：所有异常类型的基类。
- `System.Attribute`：所有特性类型的基类。

#### 类声明

使用 `class` 关键字声明类。类的声明中可以包含一组可选的 `Attribute` 特性、一组可选的类修饰符（访问修饰符，`abstract`、`static`、`sealed` 等）、分部修饰 `partial`、`class` 关键字、一个可选的类型参数列表和附加的约束、类的标识符、类的主体。

```csharp
public sealed class SampleClass<T> where T : notnull
{
    // ... Members
}
```

> 类修饰符

- `unsafe` 修饰符表示允许在类的范围内使用不安全代码。
- `new` 修饰嵌套类用以隐藏同名的继承成员。
- `public`、`protected`、`internal`、`private` 修饰符控制类的可访问性。在顶级类中只能使用 `public` 和 `internal`。
- `partial` 修饰符用以声明分部类。
- `abstract` 声明抽象类，`static` 声明静态类，`sealed` 声明密封类。 

#### Generic 泛型类

类的声明中可以包含若干的类型参数，和一组可选的类型参数约束。具有类型参数的构造类未指定约束时，默认约束为 `object`。声明泛型构造类的实例时，必须指定类型参数的具体类型。

```csharp
Sample<int> S = new();
S.Value = 99;

class Sample<T> where T : struct // T 约束为结构类型
{
    public T Value { get; set; }
}
```

#### Abstract 抽象类

抽象类代表抽象实体。其抽象成员 定义了从抽象实体派生的对象应包含什么，但这种成员不包含实现。抽象类的大多数功能通常都没有实现。一个类要从抽象类成功地派生，必须为抽象基类中的抽象方法提供具体的实现。

`abstract` 修饰符用于指示类是不完整的，并打算作为基类。抽象类与非抽象类的区别在于：
- 抽象类不能直接实例化为对象，但可以包含派生自抽象类的非抽象类实例的引用。
- 抽象类中可以可选的声明抽象函数成员（属性、方法、索引器、运算符等）。
- 抽象类无法被密封。

在抽象类中定义抽象方法，目的是将抽象方法的具体实现（`override`）延迟到派生类。抽象方法是没有实现的特殊的虚方法。在基类中也可以声明 `virtual` 虚方法并提供默认的方法实现，派生类可以重写（`override`）虚方法以扩展或重新定义派生类的行为。

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

#### Sealed 密封类

使用 `sealed` 定义密封类或密封成员，密封类防止被继承，密封成员防止被派生类重写。因此密封类不能是抽象类。这些密封类也支持运行时优化，因此可以将密封类对象上的虚函数成员的调用转换为非虚调用。

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
class Point3D: PointArray;  // err: 无法派生密封类
```

#### Static 静态类

`static` 修饰符用以声明静态类。静态类不能被实例化，它的成员必须显式地包含静态修饰符（常量和嵌套类型除外）。

静态类也不能被用作类型，因此无法用作是基类、成员的组成类型、泛型类型参数或类型约束，也不能用于数组类型、`new()`、强制转换、模式匹配、`sizeof`、`default` 表达式等。

静态类只能隐式继承 `object` 且无法从其他类或接口派生，无法声明任何实例成员，也不能被任何类继承。静态类中可以声明静态扩展方法。

加载引用静态类的程序时，.NET 运行时会加载该静态类的类型信息，并在程序中首次引用类之前初始化其字段并调用其静态构造函数。静态构造函数只调用一次，在程序所驻留的应用程序域的生存期内，静态类会保留在内存中。

```csharp
public static class EnumExt
{   
    public static T? ConvertEnum<T>(this int eVal) where T : Enum
        => (T)Enum.ToObject(typeof(T), eVal);  // int 转换为 enum
    public static bool IsDefinedByEnum<T>(this int eVal) where T : Enum
        => Enum.IsDefined(typeof(T), eVal);    // 检查 enum 是否关联整数值
}
```

#### 基类规范

类声明中可以包含一个 *class_base* 规范，该规范定义了类的直接基类和类直接实现的接口。若规范中只列出了接口类型，则直接基类是 `object`。C# 类仅支持单一线性继承（不能循环依赖），因此除了 `object` 每个类都仅有一个直接基类。

类从其直接基类继承成员。若直接基类是构造类型，则必须指定构造类型的类型参数（不能是直接基类类型），也可是类本身作为构造类型的类型参数。

```csharp
class Sample<T>;
class DerivedSample : Sample<int>;
class DerivedSample<U> : Sample<U>;
```

*class_base* 中可能包含接口类型的列表，类可以直接实现给定的接口类型。

```csharp
interface ISample
{
    void Fun();
}
class Sample : ISample
{
    public void Fun() => Console.WriteLine("Hello World");
}
```

#### 类成员

一个类可包含的成员有：常量、字段、方法、属性、索引器、运算符、事件、终结器、实例构造函数、静态构造函数和嵌套类型（类、接口、结构、枚举、委托）。

```csharp
class MyClass
{
    // 实例构造函数
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

#### 类继承

类继承其直接基类的成员。继承意味着类隐式地包含其直接基类的所有成员，但基类的实例构造函数、终结器和静态构造函数除外。

继承的一些重要方面是:
- 继承是可传递的。如果 c 从 x 派生，x 派生自 A，则 c 继承 B 中声明的成员和 A 中声明的成员。
- 派生类继承其直接基类。派生类可以向其继承的成员添加新成员，但不能删除继承成员的定义。
- 实例构造函数、终结器和静态构造函数不被继承，但所有其他成员都可以继承，不管它们声明的可访问性如何。
- 派生类可以通过声明具有相同名称或签名的新成员来隐藏继承的成员。但是，隐藏继承的成员并不会删除该成员，它只是使该成员无法通过派生类直接访问。
- 类的实例包含在类及其基类中声明的所有实例字段的集合，并且存在从派生类类型到其任何基类类型的隐式转换。因此，对某个派生类实例的引用可以被视为对其任何基类实例的引用。
- 类可以声明虚方法、属性、索引器和事件，派生类可以重写这些函数成员的实现。这使类能够显示多态行为，其中函数成员调用执行的操作取决于调用该函数成员的实例的运行时类型。

>---

### Object 对象类型

`object` 类型是 `System.Object` 在 .NET 中的别名。在 C# 的统一类型系统中，所有类型（预定义类型、用户定义类型、引用类型和值类型）都是直接或间接从 `System.Object` 继承的。可以将任何类型的值赋给 `object` 类型的变量。

引用类型的值可以直接隐式转换成 `object`。值类型的值需要经过装箱操作后隐式转换为 `object`。将 `object` 类型的变量转换为值类型的过程称为拆箱。装箱是隐式的，拆箱是显式的。装箱和拆箱的概念是类型系统 C# 统一视图的基础。

```csharp
string str = "Hello World";
object obj = str;  // implicit
string s_str = (string)str;  

int num = 10010;
obj = num;      // implicit boxing
int s_num = (int)num;  // unboxing
```

>---

### Dynamic 动态类型

`dynamic` 类型可以引用任何对象，当运算符应用于动态类型的表达式时，操作解析被延迟到程序运行时。非法的操作应用到动态类型时，编译期间不会给出任何错误，当操作在运行时失败，将引发异常。

编译器不会对包含类型 `dynamic` 的表达式的操作进行解析或类型检查，编译器将有关该操作信息打包在一起，之后这些信息会用于在运行时评估操作，因此 `dynamic` 类型只在编译时存在，在运行时则不存在。

在大多数情况下，`dynamic` 类型与 `object` 类型的行为类似。由于 `dynamic` 和 `object` 之间存在隐式转换，两者在函数上被认为是具有相同的签名。若两者都可作为类型推断的目标类型时，类型推断的结果更倾向于 `dynamic` 而不是 `object`。

```csharp
using System;
class Program
{
    static void Main(string[] args)
    {
        ExampleClass ec = new ExampleClass();
        Console.WriteLine(ec.ExampleMethod(10));
        Console.WriteLine(ec.ExampleMethod("value"));
        /* The following line causes a compiler error because ExampleMethod takes only one argument. */
        //Console.WriteLine(ec.ExampleMethod(10, 4));

        dynamic dynamic_ec = new ExampleClass();
        Console.WriteLine(dynamic_ec.ExampleMethod(10));
        /* Because dynamic_ec is dynamic, the following call to ExampleMethod
           with two arguments does not produce an error at compile time.
           However, it does cause a run-time error. */
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

#### CIL 中的动态类型

在 CIL 中，`dynamic` 类型实际上是一个 `System.Object`，当没有任何调用时，它的声明和 `object` 没有任何区别。为调用成员，编译器要声明 `System.Runtime.CompilerServices.CallSite<T>` 类型的一个变量，`T` 视成员签名而变化。

如 `ToString()` 这样的调用，也需实例化 `CallSite<Func<CallSite,object,string>>` 类型。另外还会动态定义一个方法，该方法可通过参数 `CallSite site`，`object dynamicTarget` 和 `string result` 进行调用。其中，`site` 是调用点本身。 `dynamicTarget` 是要在上面调用方法的 `object`，而 `result` 是 `ToString()` 方法调用的基础类型的返回值。注意不是直接实例化 `CallSite<Func<CallSite _site, object dynamicTarget, string result>>`，而是通过一个 `Create()` 工厂方法来实例化它。这个方法接受一个 `Microsoft.CSharp.RuntimeBinder.CSharpConvertBinder` 类型的参数。在得到 `CallSite<T>` 的一个实例后，最后一步是调用 `CallSite<T>.Target()` 来调用实际的成员。

在执行时，框架会在底层用反射来查找成员并验证签名是否匹配。然后，CLR 生成一个表达式树，它代表由调用点定义的动态表达式。表达式树编译好后，就得到了和本来应由编译器生成的结果相似的 CIL。这些 CIL 代码在调用点缓存下来，并通过一个委托调用来实际地触发调用。由于 CIL 现已缓存于调用点，所以后续调用不会再产生反射和编译的开销。

> C# 调用

```csharp
class Sample
{
    static void Main(string[] args)
    {
        dynamic dy = 0;
        string str = dy.ToString();
    }
}
```

> CIL To C#

```csharp
internal class Sample
{
	[CompilerGenerated]
	private static class <>o__0   // 生成一个动态绑定关联对象
	{
		public static CallSite<Func<CallSite, object, object>> <>p__0;
		public static CallSite<Func<CallSite, object, string>> <>p__1;
	}

	[System.Runtime.CompilerServices.NullableContext(1)]
	private static void Main(string[] args)
	{
		object dy = 0;   // 被视为 `object` 的 `dynamic dy = 0;`
		if (<>o__0.<>p__1 == null)
		{
			<>o__0.<>p__1 = CallSite<Func<CallSite, object, string>>
                .Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(Sample)));
		}
		Func<CallSite, object, string> target = <>o__0.<>p__1.Target;
		CallSite<Func<CallSite, object, string>> <>p__ = <>o__0.<>p__1;
		if (<>o__0.<>p__0 == null)
		{
			<>o__0.<>p__0 = CallSite<Func<CallSite, object, object>>
            .Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Sample), 
                    new CSharpArgumentInfo[1] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
		}
		string str = target(<>p__, <>o__0.<>p__0.Target(<>o__0.<>p__0, dy));  // 调用 CallSite<T>.Target
	}
}
```

#### 动态绑定的限制

- 无法在动态类型上使用索引和范围运算 `dynamicObj[a..b]` 或 `dynamicObj[^a]`。

>---

### String 字符串类型

`string` 类型是直接从 `object` 继承的密封类类型，它的实例表示 Unicode 字符序列。可以将 `string` 的值写成字符串字面值。关键字 `string` 是预定义类 `System.String` 的别名。相等运算符 `==` 和 `!=` 用以比较 `string` 对象的值，而不是比较 `string` 对象的引用。

```csharp
string str1 = "hello";
string str2 = "h";
str2 += "ello";
Console.WriteLine(str1 == str2);  // true
Console.WriteLine(object.ReferenceEquals(str1, str2)); // false
```

#### 字符串拼接

`+` 用于拼接两个字符串片段。字符串是不可变的，每次赋值时，编译器实际上会创建一个新的字符串对象来保存新的字符序列，并将新对象赋值给目标，并将之前的内存用于垃圾回收。

```csharp
string str = "Hello " + "World!";
```

#### 字符串索引

`[]` 运算符可用于访问字符串字符序列中的指定索引位置的字符。

```csharp
string str = "test";
for (int i = 0; i < str.Length; i++)
  Console.Write(str[i] + " ");
// Output: t e s t
```

#### 字符串内插

`$` 字符将字符串字面量标识为内插字符串，内插字符串是可能包含内插表达式的字符串文本。将内插字符串解析为结果字符串时，带有内插表达式的项会替换为表达式结果的字符串表示形式。大括号转义序列（`{{` 和 `}}`）表示为 `{` 和 `}` 的字符串形式。

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

内插字符串初始化常量时，所有的内插表达式也必须是常量字符串。

```csharp
public class Sample
{
    const string S1 = $"Hello world";
    const string S2 = $"Hello{" "}World";
    const string S3 = $"{S1} Kevin, welcome to the team!";
}
```

C#11 起内插表达式支持使用换行，以使表达式更具有可读性。

```csharp
var v = $"Count is\t: {this.Is.A.Really(long(expr))
                            .That.I.Should(
                                be + able)[
                                    to.Wrap()]}.";
```

#### 逐字字符串

`@` 指示将原义解释字符串。简单转义序列（如代表反斜杠的 `"\\"`）、十六进制转义序列（如代表大写字母 A 的 `"\x0041"`）和 Unicode 转义序列（如代表大写字母 A 的 `"\u0041"`）都将按字面解释。引号转义 `""` 不会按字面解释。

逐字内插字符串中，大括号转义序列（`{{` 和 `}}`）不按字面解释。

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

#### 原始字符串

原始字符串字面量从 C#11 开始可用。字符串字面量可以包含任意文本，而无需转义序列，字符串字面量可以包括空格和新行、嵌入引号以及其他特殊字符。原始字符串字面量用至少三个双引号（`"""`）的分隔符括起来。

```csharp
var message = """
This is a multi-line
    string literal with the second line indented.
""";
// 原始字符串的起始、结束引导序列长度要超过字符串中最长的引号序列长度
"""""
This raw string literal has four """", count them: """" four!
embedded quote characters in a sequence. That's why it starts and ends
with five double quotes.

You could extend this example with as many embedded quotes as needed for your text.
"""""
```

原始字符串支持单行形式，分隔符和字符串内容在同一行。单行形式不参与行首空格缩进。

```csharp
var str = """This is a single line""";
```

多行原始字符串的字面量分隔符必须位于自己的行，末尾分隔符的右侧决定了原始字符串的行缩进。

```csharp
var str = """
    This is a multi-line
        string literal with the second line indented.
""";
Console.WriteLine(str);
/* output
    This is a multi-line
        string literal with the second line indented.
| <--- 行缩进位置
*/

var str2 = """
    This is a multi-line
        string literal with the second line indented.
    """;
/* output
This is a multi-line
    string literal with the second line indented.
| <--- 行缩进位置
*/
```

原始字符串也支持内插表达式，字符串指定开始插值所需的大括号数目（由开头的内插字符 `$` 数目决定），任何少于这个数的大括号序列都被视为字符串内容。

```csharp
string value = "text";
var str =
    $$"""
    {
        "Summary": {{value}},
        "length": {{value.Length}}
    }
    """;
string value2 = $$"""{{
    1
    + 2
    + 3}}""";  // 被视为单行原始字符串
```

#### UTF-8 字符串字面量

.NET 中的字符串是使用 UTF-16 编码存储的。UTF-8 是 Web 协议和其他重要库的标准。从 C#11 开始，可以将 `u8` 后缀添加到字符串字面量以指定 UTF-8 编码。UTF-8 字面量存储为 `ReadOnlySpan<byte>` 对象，两个 `UTF-8` 字符串之间可以拼接。UTF-8 字符串字面量的自然类型也是 `ReadOnlySpan<byte>`。UTF-8 字符串字面量不能与字符串内插结合使用，但可以是 `@` 逐字字符串。

```csharp
using System.Text;

// u8 to u16
ReadOnlySpan<byte> strU8 = @"Hello world!"u8;
string strU16 = Encoding.UTF8.GetString(strU8);
Console.WriteLine(strU16);

// u16 to u8
string str = "Hello world!";
ReadOnlySpan<byte> bytes = Encoding.UTF8.GetBytes(str);

// u8 + u8
ReadOnlySpan<byte> str1 = "Hello"u8 + " World"u8;
```

>---

### Interface 接口类型

接口定义了一个协议，实现接口的类型必须遵循它的协议。一个接口可以继承多个基接口，一个类或结构可以实现多个接口。

使用 `interface` 关键字定义接口类型，可以包含方法、属性、事件、索引器，也可以包含静态构造函数、静态成员、常量、运算符、嵌套类型等，这些成员默认的可访问性是 `public`。

```csharp
interface ISample
{
    // 实例成员
    void FunA();
    int Value { get; set; }
    event Action MEvent;
    int this[int index] { get; set; }

    // 静态成员
    static ISample() { Console.WriteLine("Static ISample"); }
    static int GUI => 10010;
    static Action StaticEvent;
    const string TypeName = nameof(ISample);
    static ISample operator ++(ISample s)
    {
        s.Value++;
        return s;
    }

    // 嵌套类型
    delegate void OnInvoke();
    class Nested;
    interface INestedSample;
}
class Sample : ISample
{
    public int this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public event Action MEvent;
    public void FunA() => throw new NotImplementedException();
}
```

类的属性和索引器可以为接口中定义的属性或索引器定义额外的访问器。若接口属性或索引器使用显式接口实现而不是派生类型隐式实现时，访问器必须匹配。

```csharp
interface ISample
{
    int Value { get; }
    string Name { get; }
}
class Sample : ISample
{
    public int Value { get; set; }  // 额外的 set 访问器
    string ISample.Name { get; /* set; // err */ }
}
```

#### 隐式实现与显式接口实现

类或结构应提供继承接口中未实现成员的实现定义，在实现类或结构中定位接口成员的实现的过程称为接口映射。可以以隐式或显式接口方式实现继承的接口成员。隐式实现的成员只能声明为 `public` 且无法修改访问修饰符，但可以声明为 `abstract` 或 `virtual`。显式接口实现的方法无法添加访问修饰符或 `abstract`、`virtual`、`sealed`。

```csharp
interface ISample
{
    void FunA();
    void FunB();
}
class Sample : ISample
{
    public virtual void FunA() { /* ... */ }  // 隐式实现，添加 virtual
    async void ISample.FunB() { /* ... */ }  // 显式接口实现，声明为异步方法
}
```

若当接口方法映射到类中的虚方法（使用隐式实现）时，派生类则可能重写虚方法并更改接口的实现和映射关系。

```csharp
interface ISample
{
    void Fun();
}
class Sample  : ISample
{
    public virtual void Fun() => Console.WriteLine("Sample Fun");
}
class Derived : Sample
{
    public override void Fun() => Console.WriteLine("Derived Fun");
    static void Main(string[] args)
    {
        Sample s = new Sample();
        Derived d = new Derived();
        ISample Is = s;
        ISample Id = d;
        s.Fun();    // Sample Fun
        d.Fun();    // Derived Fun
        Is.Fun();   // Sample Fun
        Id.Fun();   // Derived Fun
    }
}
```

对于多继承接口，当两个或多个不相关的基接口声明具有相同名称或签名的成员时，可能会出现歧义。

```csharp
interface ICounter
{
    void Count(int c);
    int Value { get; set; }
    void Fun();
}
interface IList
{
    int Count { get; set; }
    int Value { get; set; }
    void Fun();
}
interface IListCounter : ICounter, IList;

class Sample
{
    public void Test(IListCounter x)
    {
        x.Value = 100;  // 歧义
        x.Fun();        // 歧义

        x.Count(1);
        x.Count = 1;   // 被隐藏，
        ((IList)x).Count = 1;
        ((ICounter)x).Count(1);
    }
}
```

为消除接口之间的歧义，类或接口可以声明显式接口成员实现，用以调用限定于接口的成员。类或结构的显式接口实现不包含任何修饰符，它不作为实现类型的成员，只能通过接口实例调用。

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

若接口函数成员具有一个参数数组，末位排序的参数数组在派生类型中实现时可以在类或结构中可选地附加 `params` 修饰。若接口方法的参数是一个数组，隐式实现时可选地附加 `params` 修饰，显式接口实现时不能附加。

```csharp
interface ISample
{
    void Fun(params int[] arr);
    void FunB(int[] arr);
}
class Sample : ISample
{
    public void Fun(params int[] arr) { }  // 默认实现
    public void FunB(params int[] arr) { }

    // or
    void ISample.Fun(params int[] arr) { }     // 显式接口实现
    void ISample.FunB(int[] arr) { }
}
```

> 显式接口实现的目的

- 由于显式接口成员不能通过类或结构实例访问，因此它们允许将接口实现排除在类或结构的公共接口之外。
- 显式接口成员实现允许消除具有相同签名成员的歧义。若没有显式接口成员实现，类和结构就不可能具有相同签名和返回类型的接口成员的不同实现。


#### 接口成员默认实现

一般而言，接口不提供其成员的实现，仅用来指定实现接口的类或结构应提供实现的成员。接口可为成员定义默认实现，以便提供常见功能的默认实现。接口成员中提供的默认实现等效于派生类型中的显式接口实现，只能通过接口实例进行访问。

```csharp
interface ISample
{
    void FunA();
    void FunB() => Console.WriteLine("ISample.FunB");  // 默认实现的接口方法
}
class Sample : ISample
{
    public void FunA() => Console.WriteLine("Sample.FunA");
    static void Main(string[] args)
    {
        Sample s = new Sample();
        s.FunA();       // Sample.FunA

        ISample s2 = s; 
        s2.FunA();      // Sample.FunA
        s2.FunB();      // ISample.FunB
        // 接口实例访问显式实现的成员
    }
}
```

具有默认实现的接口方法不要求其派生类型显式接口重定义，接口实现类型可以重定义具有默认实现（非 `sealed` 或 `private`）的接口成员，以改变派生继承的接口映射关系：
- 派生接口只能通过显式接口方式重写基接口方法，可以将基接口方法重新声明为抽象 `abstract`。
- 派生类型可以通过显式接口方式或隐式方式实现继承的接口方法，并改变派生类型与基接口的映射关系。

```csharp
interface ISampleA
{
    void FunA();
    void FunB() => Console.WriteLine("ISample.FunB");  // 默认实现的接口方法
}
interface ISampleB : ISampleA
{
    abstract void ISampleA.FunB();   // 重新声明为 abstract
}
class Sample : ISampleA, ISampleB
{
    public void FunA() => Console.WriteLine("Sample.FunA From ISampleA");
    public void FunB() => Console.WriteLine("Sample.FunB From ISampleB");  
    // 覆盖继承的接口映射, 可以是隐式或显式实现
    static void Main(string[] args)
    {
        Sample s = new Sample();
        s.FunA();       // Sample.FunA From ISampleA

        ISampleA s2 = s;
        s2.FunA();      // Sample.FunA From ISampleA
        s2.FunB();      // Sample.FunB From ISampleB

        ISampleB s3 = s;
        s3.FunA();      // Sample.FunA From ISampleA
        s3.FunB();      // Sample.FunB From ISampleB
    }
}
```

没有默认实现的接口成员是隐式公共抽象的，可以显式指定可访问性修饰符。其中 `private`、`virtual`、`sealed` 修饰的成员必须有默认实现。具有默认实现的非私有成员，是隐式 `virtual` 的，可以显式指定为 `virtual`。声明为 `sealed` 的接口成员无法被派生接口或派生类型通过显式接口重定义的方式改变从基接口继承的接口映射关系，即使是在派生类或接口中使用隐式方式实现，但是可以在派生接口中使用 `new` 隐藏继承的成员。

```csharp
interface ISampleA
{
    void FunA();
    private void FunInline() => Console.WriteLine("ISampleA.FunInline");
    sealed void FunB() => Console.WriteLine("ISampleA.FunB");  // 默认实现的接口方法
    virtual void FunC() => FunInline();  // virtual,sealed,private 方法需要有方法主体
}
interface ISampleB : ISampleA
{
    //void ISampleA.FunB() { }   // err, 无法通过显式实现改变继承的接口映射
    new void FunB();  // new 隐藏 ISampleA.FunB 并成为 ISampleB 的成员
}
class Sample : ISampleA, ISampleB
{
    public void FunA() => Console.WriteLine("Sample.FunA From ISampleA");
    public void FunB() => Console.WriteLine("Sample.FunB From ISampleB");
    static void Main(string[] args)
    {
        Sample s = new Sample();
        s.FunA();       // Sample.FunA From ISampleA

        ISampleA s2 = s;
        s2.FunA();      // Sample.FunA From ISampleA
        s2.FunB();      // ISampleA.FunB     
        s2.FunC();      // ISampleA.FunInline

        ISampleB s3 = s;
        s3.FunA();      // Sample.FunA From ISampleA
        s3.FunB();      // Sample.FunB From ISampleB
        s3.FunC();      // ISampleA.FunInline
    }
}
```

密封或私有的接口方法无法在派生中通过显式接口实现的方式进行重定义以改变从基接口继承的接口映射。即使在派生类型中声明为 `public` 方法，也无法覆盖继承的接口映射关系。接口的 `private` 方法是隐式密封的。

```csharp
interface ISample
{
    private void Fun(int a) => Console.WriteLine($"ISample.Fun({a})");
    // private >> protected 或其他访问修饰符
    // protected void Fun(int a) => Console.WriteLine($"ISample.Fun({a})");  // Output: Sample.Fun(10)
    sealed void Output(int a)
    {
        Console.Write("ISample.Output : ");
        Fun(a);
    }
}
class Sample : ISample
{
    public void Fun(int a) => Console.WriteLine($"Sample.Fun({a})");
    public void Output(int a)
    {
        Console.Write("Sample.Output : ");
        Fun(a);
    }
    static void Main(string[] args)
    {
        Sample s = new Sample();
        s.Output(10010);  // Sample.Output : Sample.Fun(10010)  // 无法改变接口映射

        ISample sample = s;
        sample.Output(10010);  // ISample.Output : ISample.Fun(10010)
    }
}
```

#### 泛型方法的实现

当泛型方法隐式实现接口方法时，为每个方法类型参数给出的约束在两个声明中应该是等效的（在任何接口类型参数被适当的类型参数替换之后），其中方法类型参数由从左到右的顺序位置标识。隐式实现的方法必须显式指定约束（类型参数是 `object` 的约束不需要显式指定约束），而显式接口实现的方法隐式继承类型参数约束，不能显式声明约束。当隐式实现的约束声明不合法时，只能通过显式方式实现接口成员。

```csharp
interface ISample<X, Y, Z>
{
    void FunA<T>(T t) where T : X?;
    void FunB<T>(T t) where T : Y;
    void FunC<T>(T t) where T : Z;
}

class C : ISample<object, C, string>
{
    public void FunA<T>(T t) { }                  // Ok，`FunA` 不需要指定 `where T:object` 约束，因为 `object` 是所有类型参数的隐式约束。
    public void FunB<T>(T t) where T : C { }      // Ok，`FunB` 指定的约束和接口中的约束匹配
//  public void FunC<T>(T t) where T : string { } // Error，只能显式接口实现
    void ISample<object, C, string>.FunC<T>(T t) { }
}
```

#### 接口重实现

在类中显式方式实现的接口无法在派生类中重写接口映射，除非在子类中添加接口到 *class_base* 以进行接口的重实现。派生类的隐式或显式重实现的接口成员将覆盖从基类继承的接口映射关系。

```csharp
interface ISample
{
    void Fun();
}
class Sample : ISample
{
    void ISample.Fun() => Console.WriteLine("Sample Fun");
}
class Derived : Sample, ISample
{
    public void Fun() => Console.WriteLine("Derived Fun");  // 接口重新映射
    static void Main(string[] args)
    {
        Sample s = new Sample();
        Sample sd = new Derived();
        Derived d = new Derived();
        ISample Is = s;
        ISample Isd = sd;
        ISample Id = d;
        Is.Fun();   // Sample Fun
        Isd.Fun();  // Derived Fun
        Id.Fun();   // Derived Fun
    }
}
```

#### 抽象类与接口

与非抽象类一样，抽象类应提供在类的基类列表中列出的所有接口成员的实现。但是，允许抽象类将接口方法映射到抽象方法上。显式接口实现的成员不能是抽象的。

```csharp
interface ISample
{
    void Fun();
    int Value { get; }
}
abstract class AbSample : ISample
{
    public abstract int Value { get; }
    void ISample.Fun() => Console.WriteLine(Value);
}
```

#### 接口的静态抽象和虚拟成员

从 C#11 开始，接口可以声明除静态字段之外的所有静态成员类型的 `static abstract` 和 `static virtual` 成员。

```csharp
interface ISample
{
    static abstract void Func();
    static abstract event Action E;
    static abstract object Proper { get; set; }
}
interface ISample<T> where T : ISample<T>
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

接口指定静态抽象成员，然后要求类和结构为接口抽象静态成员提供显式或隐式实现，静态虚成员具有默认实现。显式实现的接口静态 `abstract` 或 `virtual` 成员只能从受接口约束的类型参数上访问，隐式实现的接口静态 `abstract` 或 `virtual` 成员可以通过派生类型直接访问。

```csharp
interface ISample
{
    static abstract void FunA();
    static abstract void FunB();
    static virtual void FunC() => Console.WriteLine("Static ISample.FunC");
}
class Sample : ISample
{
    public static void FunA() => Console.WriteLine("Static Sample.FunA");
    static  void ISample.FunB() => Console.WriteLine("Sample: Static ISample.FunB");
    static void Main(string[] args)
    {
        // 派生类直接访问
        Sample.FunA();   // Static Sample.FunA

        Test<Sample>();
    }
    static void Test<T>() where T : ISample
    {
        // 通过类型参数访问接口成员
        T.FunA();    // Static Sample.FunA
        T.FunB();    // Sample: Static ISample.FunB
        T.FunC();    // Static ISample.FunC
    }

    // public static void FunC() => Console.WriteLine("Static Sample.FunC");  // 隐式重定义
    // Test<Sample>: T.FunC();   // Static Sample.FunC
    // Sample.FunC();   // 隐式方式实现的静态虚成员可以直接通过派生类型访问
}
```

派生接口同样可以重新定义从基接口继承静态抽象或静态虚成员。

```csharp
interface ISample
{
    static abstract void FunA();
    static abstract void FunB();
    static virtual void FunC() => Console.WriteLine("Static ISample.FunC");
}
interface IDerivedSample: ISample
{
    static void ISample.FunA() => Console.WriteLine("IDerivedSample : Static ISample.FunA");  // 接口重定义
    static abstract void ISample.FunC();  // 重新定义为 static abstract
}
```

接口中声明的 `static virtual` 和 `static abstract` 方法没有类似于类中声明的 `virtual` 或 `abstract` 方法的运行时调度机制。相反，编译器使用编译时可用的类型信息，即调用基（编译时）类型的静态方法。`static virtual` 和 `static abstract` 方法几乎完全是在泛型接口中声明的。

```csharp
interface ISample<T> where T : ISample<T>, new()
{
    static virtual void Fun() { }
    static virtual T Value { get; } = new T();
    static abstract event Action E;
    static abstract T operator ++(T t);
}

struct Sample : ISample<Sample>
{
    public int Value { get; set; } = 0;
    public Sample(int value)
    {
        this.Value = value;
    }
    static void ISample<Sample>.Fun() { }  // 显式重定义
    public static event Action E;  // 隐式实现

    // 显式实现，无法从 Sample 访问, 只能通过类型参数访问
    static Sample ISample<Sample>.operator ++(Sample s)
    {
        s.Value++;
        return s;
    }
    static void Main(string[] args)
    {
        Sample s = new Sample(99);
        s.Increment(ref s);
        Console.WriteLine(s.Value);  // 100
    }
    void Increment<T> (ref T t) where T : ISample<T>,new()
    {
        t++;  // 类型参数访问
    }
}
```

对于与非虚实例成员的对称，静态非字段成员允许使用可选的 `sealed` 修饰符，即使它们默认是非虚的：

```csharp
interface ISample
{
    static sealed void M() => Console.WriteLine("Default behavior");

    static int f = 0;
    static sealed int P1 { get; set; }
    static sealed int P2 { get => f; set => f = value; }

    static sealed event Action E1;
    static sealed event Action E2 { add => E1 += value; remove => E1 -= value; }
    static sealed ISample operator +(ISample l, ISample r) => l;
}
```

>---

### Array 数组类型

数组是一种包含零个到多个变量的数据结构，可以通过计算索引访问这些变量。数组中的变量亦称为元素，它们具有相同的类型，数组的元素可以是任何类型。类型 `System.Array` 是所有数组类型的抽象基类型。

数组具有确定与每个元素相关联的索引的秩，数组的秩也被称为数组的维数。秩为一的数组称为一维数组，秩大于一的数组称为多维数组，特定尺寸的多维数组通常被称为二维数组、三维数组等。

数组的每个维度都有一个相关联的长度，该长度是一个大于等于零的整数。维度长度不是数组类型的一部分，而是在运行时创建数组类型的实例时建立的。维度的长度决定了该维度索引的有效范围：对于长度为 N 的维度，索引的范围可以从 0 到 N-1。数组中元素的总数是数组中每个维度长度的乘积。如果数组的一个或多个维度的长度为零，则称该数组为空。

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

数组的元素类型本身也可以是数组类型，这类数组不同于多维数组，被称为交错数组。

```csharp
int[][] pascals = 
{
    new int[] {1},
    new int[] {1, 1},
    new int[] {1, 2, 1},
    new int[] {1, 3, 3, 1}
};
```

#### 数组的访问

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

#### 数组和泛型集合接口

一个单维数组 `T[]` 实现了 `System.Collections.Generic.IList<T>` 接口，因此存在从 `T[]` 到 `IList<T>` 及其基接口的隐式转换。同样 `T[]` 也实现了 `System.Collections.Generic.IReadOnlyList<T>` 接口。

```csharp
class Test
{
    static void Main()
    {
        int[] arr = new int[] { 0, 1, 2, 3, 4 };
        IList<int> list = arr;
        IReadOnlyList<int> rolist = arr;
    }
}
```

若 `S` 到 `T` 存在引用隐式转换，则 `S[]` 也可以隐式转换为 `IList<T>` 或 `IReadOnlyList<T>`。

```csharp
class Test
{
    static void Main()
    {
        string[] sa = new string[5];
        object[] oa1 = new object[5];
        object[] oa2 = sa;

        IList<string> lst1 = sa;  // Ok
        IList<string> lst2 = oa1; // Error, cast needed
        IList<object> lst3 = sa;  // Ok
        IList<object> lst4 = oa1; // Ok

        IList<string> lst5 = (IList<string>)oa1; // Exception
        IList<string> lst6 = (IList<string>)oa2; // Ok

        IReadOnlyList<string> lst7 = sa;        // Ok
        IReadOnlyList<string> lst8 = oa1;       // Error, cast needed
        IReadOnlyList<object> lst9 = sa;        // Ok
        IReadOnlyList<object> lst10 = oa1;      // Ok
        IReadOnlyList<string> lst11 = (IReadOnlyList<string>)oa1; // Exception
        IReadOnlyList<string> lst12 = (IReadOnlyList<string>)oa2; // Ok
    }
}
```

>---

### Delegate 委托类型

委托是引用一个或多个方法的数据结构。委托的声明定义了一个从 `System.Delegate` 派生的类。委托实例封装了一个调用列表，该列表是一个或多个方法的列表，每个方法都是一个可调用实体。对于实例方法，可调用实体由实例和该实例上的方法组成。对于静态方法，可调用实体仅由一个方法组成。调用委托时，将导致调用这个列表上的每个可调用实体。

在 C/C++ 中，与委托最接近的等效项是函数指针。C# 的函数指针只能引用静态函数，而委托可以引用静态和实例方法，并且委托还存储了对方法入口点的引用和对调用方法的对象实例引用。

#### 委托的声明

委托类型的声明与方法签名相似，它有一个返回值和任意数目任意类型的参数。委托类型是一种可用于封装命名方法或匿名方法的引用类型，是面向对象的、类型安全的和可靠的。

使用 `delegate` 关键字声明委托类型，编译器将委托相关的操作代码的调用映射到 `System.Delegate` 和 `System.MulticastDelegate` 类成员的方法调用。必须使用具有兼容返回类型和输入参数的方法或 Lambda 表达式实例化委托。在实例化委托时，可以将委托的实例与任何兼容的方法相关联，并可以通过委托实例调用方法。

将方法作为参数进行引用的能力使委托成为定义回调方法的理想选择。委托类似于 C++ 函数指针，但委托面向对象，会同时封装对象实例和方法，因此委托允许将方法作为参数进行传递。

```csharp
MessageDelegate debug = Console.WriteLine;
if (debug is MulticastDelegate or Delegate)
    debug("Hello World >>> " + debug.Method.Name);

public delegate void MessageDelegate(string message);
public delegate int AnotherDelegate(int num1, int num2);
```

在 .NET 中，`System.Action`、`System.Func`、`System.Predicate` 类型为许多常见委托提供泛型定义。

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

#### 多播委托

可以通过使用 `+` 组合多个委托对象分配到一个委托实例中。多播委托中包含已分配委托列表，此多播委托被调用时会按照添加的先后顺序依次调用列表中的委托。`-` 用于从多播委托中删除组件委托。

`+=` 可以将方法或匿名方法构造为委托对象并分配到多播委托中，`-=` 则表示从多播委托中移除该方法的委托实例。`+`、`-` 运算符支持委托对象和方法组之间的运算。

一个委托可以多次出现在调用列表中，这样的委托被删除时，总是删除调用列表的最后一个。删除委托对象时，若右操作数是 Lambda 表达式或匿名方法（非匿名类型）时，此操作无效。

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

>---

### 可为空的引用类型

由于在可为 `null` 的感知上下文选择加入了代码，可以使用可为 `null` 的引用类型、`null` 静态分析警告和空包容运算符（`!`）是可选的语言功能。在可为 `null` 的感知上下文中：
  - 引用类型 `T` 的变量必须用非 `null` 值进行初始化，并且不能为其分配可能为 `null` 的值。
  - 引用类型 `T?` 的变量可以用 `null` 进行初始化，也可以分配 `null`，但在取消引用之前必须对照 `null` 进行检查。
  - 类型为 `T?` 的变量 `m` 在应用空包容运算符时被认为是非空的，如 `m!` 中所示。

类型为 `T` 的变量和类型为 `T?` 的变量由相同的 .NET 类型表示。可为 `null` 的引用类型不是新的类类型，而是对现有引用类型的注释。编译器使用这些注释来帮助你查找代码中潜在的 `null` 引用错误。不可为 `null` 的引用类型和可为 `null` 的引用类型在运行时没有区别。

可以通过两种方式控制可为 null 的上下文。在项目级别，可以添加 `<Nullable>enable</Nullable>` 项目设置。在单个 C# 源文件中，可以添加 `#nullable enable` 来启用可为 null 的上下文。在 .NET 6 之前，新项目使用默认值 `<Nullable>disable</Nullable>`。从 .NET 6 开始，新项目将在项目文件中包含 `<Nullable>enable</Nullable>`。

可空引用类型不能出现在：
- 作为基类或接口。
- 作为对象构造表达式（`new()`）中的类型。
- 作为委托构造表达式（`new delegateType()`）中的 `delegateType` 类型。
- 作为 `is` 表达式、`catch` 子句、类型模式中的类型。

---
## 值类型

值类型包含结构类型和枚举类型。C# 提供了一组预定义的结构类型，被称为简单类型，简单类型可以通过关键字来识别。

引用类型可以包含 `null` 值，但是值类型只有是为可为空的值类型时，才能包含 `null` 值。对于每一个非空值类型，都有一个对应的可空值类型。

>---

### System.ValueType 类型

所有的值类型都隐式继承类 `System.ValueType`，任何类型都不可能从值类型派生，所有值类型都是隐式密封的。

>---

### 默认构造函数

所有值类型都隐式声明了一个公共无参非静态的 *默认构造函数*，该函数返回一个零初始化的实例作为值类型的 *默认值（`default`）* 。用户定义的结构类型可以显式声明一个公共无参的实例构造函数和其他参数化的实例构造函数。

值类型的默认值是由全零位模式产生的值：
  - 对于 `sbyte`、`byte`、`short`、`ushort`、`int`、`uint`、`long`、`ulong` 的默认值为 0.
  - 对于 `char` 的默认值为 `\x0000`。
  - 对于 `float` 的默认值为 `0.0F`。
  - 对于 `double` 的默认值为 `0.0d`。
  - 对于 `decimal` 的默认值为 `0.0m`。
  - 对于 `bool` 的默认值为 `false`。
  - 对于 `enum` 的类型 `E` 默认值为 `0` 并转换为类型 `E`。
  - 对于 `struct`，其默认值为所有的值类型字段设置为默认值和所有的引用类型字段设置为 `null` 值。
  - 对于可空值类型的默认值为其 `HasValue` 属性为 `false` 的实例。

>---

### Simple 简单类型

C# 提供了一组预定义的结构类型（简单类型），它们可以通过关键字来标识，这些关键字是这些预定义类型的别名：
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

简单类型与其他结构类型的不同之处在于：
- 大多数简单类型允许通过文本字面量来创建值。
- 当表达式的操作数都是简单类型常量时，编译器会在编译时对表达式求值。
- 可以通过 `const` 声明简单类型的常量，其他类型只能通过 `static readonly` 起到类似的效果。
- 涉及简单类型的转换可以参与其他结构类型定义的转换运算符的求值，但是用户定义的转换运算符不能参与其他用户定义的转换运算符的求值。


### Integer 整数类型

C# 支持 11 种整数类型：`sbyte`、`byte`、`short`、`ushort`、`int`、`uint`、`nint`、`nuint`、`long`、`ulong`、`char`，所有的有符号整数都使用二进制补码格式表示：
- `sbyte`：有符号 8 位整数，介于 -128 ~ 127 之间。
- `byte`：无符号 8 位整数，介于 0 ~ 255 之间。
- `short`：有符号 16 位整数，介于 -32768 ~ 32767 之间。
- `ushort`：无符号 16 位整数，介于 0 ~ 65535 之间。
- `int`：有符号 32 位整数，介于 -2147483648 ~ 2147483647 之间。
- `uint`：无符号 32 位整数，介于 0 ~ 4294967295 之间。
- `long`：有符号 64 位整数，介于 -9223372036854775808 ~ 9223372036854775807 之间。
- `ulong`：无符号 64 位整数，介于 0 ~ 18446744073709551615 之间。
- `char`：表示值介于 0 ~ 65535 之间的 16 位无符号整数。`char` 类型的可能值集与 Unicode 字符集相对应。
- `nint`：表示为本机大小的有符号整数，32 或 64 位。
- `nuint`：表示为本机大小的无符号整数，32 或 64 位。

整数类型的一元或二元运算符总是使用 `int`、`uint`、`long`、`ulong` 精度的整型进行运算。`char` 归类于整型，但是没有其他整数类型到 `char` 类型的隐式转换。`char` 的常量应写成字符形式或 `(char)integer` 的强制转换形式。

`checked` 和 `unchecked` 运算符和语句用于控制整型算数运算和转换的溢出检查。`checked` 上下文中整型运算溢出会引发 `System.OverflowException` 异常；`unchecked` 上下文中将忽略溢出，并且不会丢弃任何不适合目标类型的高序位。  

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

> 数字分隔符

任何数字序列都可以用下划线分隔，两个连续数字之间可以有一个以上的下划线。在小数中也可以使用。

```csharp
int bin = 0b1001_1010_0001_0100;
int hex = 0x1b_a0_44_fe;
int dec = 33_554_432;
int weird = 1_2__3___4____5_____6______7_______8________9;
double real = 1_000.111_1e-1_000;
```

#### char 字符类型

`char` 类型用来表示 Unicode UTF-16 字符，类型支持比较、相等、增量和减量运算符。算数和逻辑位运算得到 `int` 结果。

`char` 的值可以用字符文本、Unicode 转义序列（`\uUUUU`，四位）、十六进制转义序列（`\xXX`）表示。`char` 可以隐式转换为其它包含数值类型，或显式转换为未包含数值类型。其他类型则无法隐式转换为 `char`。

```csharp
char Ch = 'H';
char Ch_x = '\x65';
char Ch_u = '\u00ff';
```

#### 本机大小的整数

```csharp
// 本机大小的整数
nint    _IntPtr = new IntPtr();     // 有符号本机 32 位或 64 位整数
nuint   _UIntPtr = new UIntPtr();   // 无符号本机 32 位或 64 位整数
```

类型 `nint` 和 `nuint` 由底层类型由 `System.IntPtr` 和 `System.UIntPtr` 表示，编译器为这些类型提供额外的转换和操作。本机大小的整数类型具有特殊行为，因为存储是由目标计算机上的自然整数大小决定的。

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

类型 `nint` 和 `nuint` 转换和操作是由编译器合成的，不是底层 `IntPtr` 和 `UIntPtr` 类型的一部分，因此这些转换和运算符操作无法从动态类型的运行时绑定中获得。

```csharp
nint x = 2;
nint y = x + x; // ok
dynamic d = x;
nint z = d + x; // RuntimeBinderException: '+' cannot be applied 'System.IntPtr' and 'System.IntPtr'
```

>---

### Floating-point 浮点类型

C# 支持两种 IEEE-754 格式表示的 `float` （32 位单精度）和 `double`（64 位双精度）浮点数，它们的值域包含：
- 正零和负零，常规算数中和简单值 0 行为相同，在一些特殊的操作中区分零的正负。
- 正无穷（`1.0/0.0`）与负无穷（`-1.0/0.0`）。
- 非数字值，NaN 是由无效的浮点运算产生的，如 `0.0/0`。
- `float` 可以表示具有大约 1.5 × 10⁻⁴⁵ ~ 3.4 × 10³⁸ 的值，精度为 7 位。
- `double` 可以表示具有大约 5.0 × 10⁻³²⁴ ~ 1.7 × 10³⁰⁸ 的值，精度为 15 ~ 16 位。

```csharp
float F_32 = 3.1415f;          // f 或 F 后缀，6~9 位精度
double D_64 = 3.1415;          // 隐式 d 或 D 后缀，15~17 位精度
// 无穷定义
var n_inf = double.NegativeInfinity;    // 负无穷  
var p_inf = double.PositiveInfinity;    // 正无穷
// 非数值
var nan = 0.0/0;   // 非数值
```

如果二元运算符的任一操作数是浮点类型，则应用数值提升，并以 `float` 或 `double` 执行操作：
- 浮点运算永远不会产生异常，在特殊情况下，浮点运算产生零、无穷大或 NaN。
- 浮点运算的结果四舍五入到目标格式中最接近的可表示值。
- 若浮点运算结果的大小对于目标格式来说太小，则将运算结果变成正零或负零。
- 若浮点运算结果的大小对于目标格式来说太大，则将运算结果变为正无穷大或负无穷大。
- 若浮点操作无效，则该操作的结果为 NaN；若其中一个操作数为 NaN 时，最终运算结果也是 NaN。

>---

### Decimal 十进制数值类型 

`decimal` 类型是一种 128 位的数据类型，适合于金融和货币计算。十进制类型可以表示的范围至少为 -7.9 × 10⁻²⁸ ~ 7.9 × 10²⁸, 至少有 28 位精度。

```csharp
decimal M_128 = 3.1415m;       // m 或 M 后缀，28~29 位精度
```

若二元运算符的任一操作数是 `decimal` 类型，则应用数值提升，并以 `double` 执行该操作：
- 对于 `decimal` 类型的值进行操作的结果是计算精确的结果，并舍入以适应表示。
- 结果四舍五入到最接近的可表示值。舍入可能会从非零值产生零值。
- 若十进制算数运算产生的结果对于 `decimal` 太大，则会产生 `System.OverflowException` 异常。
- 不同于 `float` 和 `double`，`decimal` 可以精确表示 0.1 的值。

#### decimal 和浮点类型 

`decimal` 比 `float` 和 `double` 具有更高的精度，所以可能比浮点类型具有更小的范围。从浮点类型转换为 `decimal` 类型可能产生溢出异常，反过来 `decimal` 转换为浮点类型可能会损失精度或溢出异常。因此它们之间不存在隐式转换，可以显式强制转换。

```csharp
decimal dm = 1.0m;
double d = (double)dm;

float f = 3.14f;
decimal df = (decimal)f;
```

>---

### Bool 布尔类型

`bool` 类型表示布尔逻辑值，值可以为 `true` 或 `false`。布尔类型和其他类型之间不存在标准转换，不能用布尔值代替整型。

在 C/C++ 中，零的整型或浮点型值、空指针可以被转换为布尔值 `false`，非零整型或浮点型、非空指针可以被转换为布尔值 `true`。在 C# 中，这种转换是通过显式地将整数或浮点值与零进行比较，或者显式地将对象引用与 `null` 进行比较来完成的。
 
可以使用 `Convert` 类进行布尔转换：非零数值转换为 `true`，零转换为 `false`；字符串转换时忽略大小写。
 
```csharp
bool t = true;
bool f = false;
bool rt = Convert.ToBoolean("True");    // true
bool rt2 = Convert.ToBoolean(0);        // false
```

>---

### Enumeration 枚举类型

枚举类型是声明一组命名常量的值类型。每个枚举类型都有一个相应的整型作为其基础类型，该整数类型可以是 `byte`、`sbyte`、`short`、`ushort`、`int`、`uint`、`long`、`ulong`。默认的基础类型是 `int`。

使用 `enum` 定义枚举类型，并声明其枚举成员的命名常量和关联常数值，多个枚举成员可以共享相同的关联值。

关联值可以隐式或显式赋值。若枚举成员是枚举类型中的第一个枚举成员，在为显式赋值时，其关联值为 0。其他未显式赋值的枚举成员的关联值是前一个成员的值加 1。

```csharp
enum Color : uint
{
    Red = 1,
    Green = 2,
   // Blue = -3, // err 
    Blue = 3,
    Max = Blue
}
```

可以使用枚举类型，通过一组互斥值或选项组合来表示选项。若要表示选项组合，请将枚举类型定义为位标志。

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

#### System.Enum

`System.Enum` 类型是所有枚举类型的抽象基类。可在基类约束中使用 `System.Enum`（称为枚举约束），以指定类型参数为枚举类型。对于任何枚举类型，都存在分别与 `System.Enum` 类型的装箱和取消装箱的相互转换。

对于任何枚举类型，枚举类型与其基础整型类型之间存在显式转换。使用 `Enum.IsDefined` 方法来确定枚举类型是否包含具有特定关联值的枚举成员。

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

>---

### ValueTuple 元组类型

元组类型表示具有可选名称和单独类型的有序、固定长度的值序列。元组类型写为 `(T1 I1, ... , Tn In)`，n ≥ 2，标识符 `I1, ..., In` 是可选的元组元素名。

`(T1, ..., Tn)` 这种构造语法是 `System.ValueTuple<...>` 的简写，它是一组泛型结构类型。能够直接表示 2 ~ 7 数目之间的任意元组类型。数量大于 7 的元组用泛型 `ValueTuple<T1, ..., T7, TRest>` 表示，其中 `TRest` 包含一个嵌套其余元素的值，由另一个 `ValueTuple<...>` 表示。

元组的值可以通过元组表达式创建，也可以是 `new ValueTuple<...>`。

```csharp
using Point = (int, int);

Point p1 = (1, 2);  // 元组表达式
Point p2 = new ValueTuple<int, int>(1, 2);
Point p3 = new(1, 2);
Point p4 = ValueTuple.Create(1, 2);
```

元组类型中的元素名称必须不同，未提供显式名称的元素根据其元素顺序命名为 `ItemX`，`X` 是从 1 开始的序列。可选元素名不在 `ValueTuple<...>` 类型中，并且不会存在元组值得运行时表示形式中。`ItemX` 的显式定义名称只能用在相应的 `X` 位置。

```csharp
using Point = (int x, int y);

Point p1 = (1, 2);
Console.WriteLine(p1.Item1);  // 使用默认的元素名称
Console.WriteLine(p1.y);    // 使用显式提供的元素名称

(int Item1, string Item2) pair1 = (5, "Five");  
(int Item2, string) pair2 = (5, "Five");  // err, Item2 只能用于 2 位置
```

元组类型支持相等运算符 `==` 和 `!=`。

```csharp
(double x, double y) Point1 = new(1, 1);
(double, double) Point2 = new();
Point2.Item1 = Point1.x;
if(Point2 != Point1)
    Console.WriteLine(Point2);  // (1,0)
```

元组最常见的用例之一是作为方法返回类型，可以将方法结果分组为元组返回类型，而不是定义 `out` 方法参数。可以使用赋值运算符 `=` 在单独的变量中析构元组实例，析构元组时可以使用弃元。

```csharp
var (X, Y, _) = GetRandomPoint();   // 析构元组
Console.WriteLine("The Point2D = ({0},{1})", X, Y);
static (int, int, int) GetRandomPoint()
{
    Random rand = new Random(DateTime.Now.Millisecond);
    return (rand.Next(-128, 128), rand.Next(-128, 128), rand.Next(-128, 128));
}
```

#### 解构函数

可以在 `struct`、`class`、`record`、`interface` 中声明名为 `Deconstruct` 的方法，该方法返回 `void`，且拥有至少两个 `out` 参数。该方法为这些类型提供解构为元组的功能支持，`Deconstruct` 方法支持重载。

在声明主构造函数的记录中，编译器会为其自动生成一个 `Deconstruct` 方法，其参数列表对照位置记录中的位置参数。

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

解构函数也可以是扩展方法，可以为指定类型提供额外的 `Deconstruct` 扩展方法。

```csharp
public static void Deconstruct(this <Type> destTypeObj, out <Type1> val, out <Type2> val2[, out < Type3 > val3...]0) { }

var (<Type1> rt1, <Type2> rt2[, ...]) = destTypeObj;
```

> 系统类型的扩展方法

为了方便起见，某些系统类型提供 `Deconstruct` 方法。例如 `System.Collections.Generic.KeyValuePair<TKey,TValue>` 类型提供此功能，循环访问 `Dictionary` 时，每个元素都是 `KeyValuePair<TKey,TValue>`。

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

>---

### Nullable-value 可为空的值类型

可空值类型 `T?` 表示其基础值类型 `T` 的所有值及额外的 `null` 值，其默认值为 `null`。任何可为空的值类型都是泛型 `System.Nullable<T>` 结构的实例。每个可空值类型包含两个只读属性 `HasValue` 和 `Value`，当 `HasValue` 为 `false` 时，`Value` 为 `null` 值。

访问可空值类型变量的 `Value` 属性的过程称为展开（*unwrapping*）；为给定值类型创建可空值类型的过程称为包装（*wrapping*） 

```csharp
int num = 0;
int? n_num = num;  // wrapping

int? nullNumble = 10010;
// num = (int)nullNumble; // 可能出现 null 引用异常
num = nullNumble ?? default;  // unwrapping
Console.WriteLine(num);  // 10010
```

#### 可空值类型的 null 检查

可以将 `is` 运算符与类型模式结合使用，既检查 null 的可空值类型的实例，又检索基础类型的值。或使用 `Nullable<T>.HasValue` 指示可为空值类型的实例是否有基础类型的值，如果 `HasValue` 为 `true`，则 `Nullable<T>.Value` 获取基础类型的值。也可以使用空合并操作符将可空类型转换为其基础类型。

```csharp
void NullCheck<T>(T? n) where T : struct
{
    // 几种空判定s
    if (n != null)
        Console.WriteLine("The input is " + n.Value);
    if (n is int temp)
        Console.WriteLine("The input is " + temp);
    if (n.HasValue)
        Console.WriteLine("The input is " + n.Value);
    Console.WriteLine("The input is " + (n ?? default(T)));
}
```

> 确定可为空的值类型

```csharp
Console.WriteLine($"int? is {(IsNullable(typeof(int?)) ? "nullable" : "non nullable")} value type");
Console.WriteLine($"int is {(IsNullable(typeof(int)) ? "nullable" : "non-nullable")} value type");

bool IsNullable(Type type) => Nullable.GetUnderlyingType(type) != null;

// Output:
// int? is nullable value type
// int is non-nullable value type
```

#### 可空值类型的运算

可为空值类型拥有预定义的一元或二元运算符时，若至少存在一个 `null` 值时，运算结果也为 `null`。对于比较运算符 `<`、`>`、`<=` 和 `>=`，如果一个或全部两个操作数都为 `null`，则结果为 `false`。`null == null` 返回 `true`。

>---

### Struct 结构类型

结构体类似于类，因为它们表示可以包含数据成员和函数成员的数据结构。与类不同的是，结构是值类型，不需要堆分配。结构类型的变量直接包含该结构的数据，而类类型的变量包含对数据的引用，被称为对象。从 C#11 起，对于任何在结构构造函数返回或使用之前没有显式赋值的字段，都会将这些字段隐式初始化为默认值（在 C# 的早期版本编译器会给出这些未显式赋值字段的明确赋值错误）。

结构类型可以声明常量、字段、方法、属性、事件、索引器、运算符、实例构造函数、静态构造函数和嵌套类型。使用 `struct` 关键字定义结构类型。

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

#### readonly 只读结构

可以使用 `readonly` 修饰符来声明结构类型为不可变，它所有的实例字段都应声明为 `readonly` 的。它的实例属性不能包含 `set` 访问器，也不能声明字段形式的事件。

当一个只读结构的实例被传递给一个方法时，它的 `this` 被视为一个 `in` 参数，它禁止对任何实例字段进行写访问（构造函数除外）。

```csharp
readonly struct Sample
{
    public readonly string FirstName;
    public readonly string LastName;
    public string Name => FirstName + " " + LastName;
}
```

#### ref 结构

`ref` 修饰声明的结构类型被称为引用结构（*ref struct*），它的实例在执行堆栈上分配，不能转义到托管堆。

`ref struct` 有以下限制：
  - `ref struct` 不能是数组的元素类型、元组的元素类型、不能实现接口、不能是类型参数、不能在迭代器中使用。
  - `ref struct` 不能是类或非 `ref struct` 的字段的声明类型。
  - `ref struct` 不能被装箱为 `System.ValueType` 或 `System.Object`。
  - `ref struct` 变量不能由 Lambda 表达式或局部函数捕获。
  - `ref struct` 变量不能在 `async` 方法中使用，但可以在同步方法中使用 `ref struct` 变量。例如，在返回 `Task` 或 `Task<TResult>` 的同步方法中。
  - `ref struct` 中不能声明异步实例方法、迭代器实例方法。

`ref` 字段有以下限制：
- `ref` 字段只能在 `ref struct` 内部声明。
- `ref` 字段不能声明为 `static`、`volatile`、`const`。
- `ref` 字段的类型不能是 `ref struct`。
- 引用程序集生成过程必须在 `ref struct` 中保留 `ref` 字段的存在。
- `readonly ref struct` 必须将其 `ref` 字段声明为 `readonly ref`。

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

`ref struct` 可以使用 `default` 表达式进行初始化，但是它的所有 `ref` 字段被初始化为 `null`，因此直接访问这些字段将引发 `NullReferenceException`。可以在调用 `ref` 字段或变量前，使用 `Unsafe.IsNullRef<T>(ref T t)` 检查变量值的引用是否为空。

```csharp
ref struct Sample
{
    private ref int Value;
    public int GetValue()
    {
        if (System.Runtime.CompilerServices.Unsafe.IsNullRef(ref Value))
            throw new InvalidOperationException(message);
        return Value;
    }
}
```

`ref struct` 结构的 `ref` 字段可以与 `readonly` 组合：
- `readonly ref` 字段不能在构造函数或 `init` 属性访问器之外重新 `= ref` 赋值，但是可以在此之外的任何上下文中作为一个值进行 `=` 赋值（注意查 `null`）。
- `ref readonly` 字段可以被 `= ref` 赋值，但是不能 `=` 赋值。
- `readonly ref readonly` 是 `readonly ref` 和 `ref readonly` 的组合，即仅能在构造函数或 `init` 属性访问器中初始化，在其他任何上下文均不能通过 `= ref` 或 `=` 改变字段的状态。 

```csharp
ref struct ReadOnlyExample
{
    ref readonly int Field1;
    readonly ref int Field2;
    readonly ref readonly int Field3;

    void Uses(int[] array)
    {
        Field1 = ref array[0];  // Okay
        Field1 = array[0];      // Error: can't assign ref readonly value (value is readonly)

        Field2 = ref array[0];  // Error: can't repoint readonly ref
        Field2 = array[0];      // Okay
        
        Field3 = ref array[0];  // Error: can't repoint readonly ref
        Field3 = array[0];      // Error: can't assign ref readonly value (value is readonly)
    }
}
```

#### 类和结构的区别

> 结构体的值类型，类是引用类型。

- 结构体是值类型，具有值语义。类是引用类型，具有引用语义。类实例之间可以相互依赖。但结构体之间相互依赖会导致编译时错误。结构体所依赖的结构体的完整集合是直接依赖关系的传递闭包（值包含值）。

```csharp
// 自我依赖
struct Node
{
    int data;
    Node next;  // 导致闭包循环
}
// 相互依赖
struct A { B b; }
struct B { A a; }
// ===================================
class NodeClass
{
    int data;
    NodeClass next; 
}
class AClass { BClass b;  }
class BClass { AClass b; }
```

> 继承

所有结构类型都隐式继承类 `System.ValueType`。结构声明可以指定接口，但不能指定基类。结构总是隐式密封的，因此结构中不允许使用 `abstract` 和 `sealed`。由于结构体无法被继承，因此它的成员不能是 `protected`、`internal protected`、`private protected`。

结构体中的函数不能是 `virtual` 或 `abstract`，但是可以重写从 `System.ValueType` 继承的虚方法。

```csharp
struct Person(string name) : ICloneable
{
    public object Clone() => new Person(name);
    public override string ToString() => name ?? "";
}
```

> 赋值

对结构类型变量的赋值将创建被赋值变量的数据副本。这与对类类型变量的赋值不同，后者复制引用，但不复制引用所标识的对象。

与赋值操作类似，当将结构体作为值形参传递或作为函数成员的结果返回时，将创建该结构体的副本。结构体可以使用 `in`、`out` 或 `ref` 形参通过引用传递给函数成员。

当结构体的属性或索引器是赋值的目标时，与该属性或索引器访问相关联的实例表达式应归类为变量。如果实例表达式被归类为值，则会发生编译时错误。

> 默认值

对于类类型和其他引用类型的变量，默认值为空。但是，由于结构体是不能为空的值类型，因此结构体的默认值是将所有值类型字段设置为默认值并将所有引用类型字段设置为空所产生的值。

```csharp
class Sample(string name)
{
    public readonly string Name = name;
    struct Person(string name)
    {
        public readonly string Name = name;
    }
    static void Main(string[] args)
    {
        Person[] people = new Person[10];
        Sample[] samples = new Sample[10];

        Console.WriteLine(people[0]);   // Sample+Person
        Console.WriteLine(samples[0]);  // null
    }
}
```

> 装箱与拆箱

只需在编译时将引用视为另一种类型，就可以将类类型的值转换为类型对象或由类实现的接口类型。同样，对象类型的值或接口类型的值可以在不更改引用的情况下转换回类类型（在这种情况下需要进行运行时类型检查）。

```csharp
interface ISample
{
    string Name { get; }
}
class Sample(string name) : ISample
{
    public string Name { get; set; } = name;
    static void Main(string[] args)
    {
        Sample S = new Sample("Hello");
        ISample id = S;             // Sample -> ISample
        Console.WriteLine(id.Name); // Hello
        S.Name = "World";
        Console.WriteLine(id.Name); // World

        Sample S2 = id as Sample;   // ISample -> Sample
    }
}
```

由于结构体不是引用类型，当结构类型的值转换为某些引用类型时，将发生装箱操作。同样，当某个引用类型的值被转换回结构类型时，将发生拆箱操作。与类类型上的相同操作的一个关键区别是，装箱和拆箱将结构体值复制到或复制到已装箱的实例之外。

在装箱或拆箱操作之后，对未装箱结构体所做的更改不会反映在装箱结构体中。

```csharp
interface ISample
{
    string Name { get; set; }
}
struct Sample(string name) : ISample
{
    public string Name { get; set; } = name;
    static void Main(string[] args)
    {
        Sample S = new Sample("Hello");
        ISample id = S;             // Sample -> ISample
        Console.WriteLine(id.Name); // Hello
        S.Name = "World";
        Console.WriteLine(id.Name); // Hello
        S.Name = id.Name;
        Console.WriteLine(((Sample)id).Equals(S));  // True, 值相等性

        id.Name = "World";
        if (id is Sample S2)    // ISample -> Sample
            Console.WriteLine(S2.Name);  // World
    }
}
```

> this 的含义

`this` 在结构体中的含义不同于 `this` 在类中的含义。当结构类型覆盖从 `System.ValueType` 继承的虚方法时（如 `Equals`、`GetHashCode` 或 `ToString`），通过结构类型的实例调用虚拟方法不会导致装箱。即使将结构用作类型参数并且通过类型参数类型的实例调用也是如此。

```csharp
class Sample
{
    struct Counter
    {
        int value;
        public override string ToString() => value++.ToString();
    }
    static void Test<T>() where T : new()
    {
        T x = new T();
        Console.WriteLine(x.ToString());    // 0
        Console.WriteLine(x.ToString());    // 1
        Console.WriteLine(x.ToString());    // 2
    }
    static void Main() => Test<Counter>();
}
```

> 字段初始化项

结构体中的实例字段和默认实现的属性具有初始值设定项时，必须同时包含显式声明的构造函数。

```csharp
struct Sample(string first, string last)
{
    public string Name { get; set; } = first + " " + last;
    public string FirstName = first;
    public string LastName = last;
}
```

> 构造函数

没有声明构造函数的非静态类或结构，编译器为提供一个默认的公共无参实例构造函数。类中显式声明实例构造函数，则无需提供默认构造函数。结构体中始终包含一个公共无参实例构造函数，无论是否显式声明。

```csharp
//Sample sc = new Sample(); // err
Sample sc = new Sample(10010);
SampleStruct ss = new SampleStruct();  // 隐式提供

class Sample(int value);
struct SampleStruct(int value);
```

#### 内联数组

从 C#12 开始，可以声明结构类型的内联数组。内联数组是包含相同类型的 N 个元素的连续块的结构，它是一个安全代码，等效于仅在不安全代码中可用的固定缓冲区声明，编译器可以利用有关内联数组的已知信息。内联数组是仅包含单个字段、且未指定其他任何的显式布局的结构类型。

使用 `System.Runtime.CompilerServices.InlineArrayAttribute` 特性修饰 `struct` 类型，并指定一个大于零的值。

```csharp
[System.Runtime.CompilerServices.InlineArray(10)]
public struct InlineArray<T>
{
    private T Elem;
}
// 固定缓冲区
public struct Buffer
{
    public unsafe fixed int buffer[1024]; 
}
```

内联数组是一种高级语言功能。它们适用于高性能方案，在这些方案中，内联的连续元素块比其他替代数据结构速度更快。可以像访问数组一样访问内联数组，以读取和写入值，还可以使用范围和索引运算符。

内联数组对单个字段的类型有最低限制：它不能是指针类型，但可以是任何引用类型或任何值类型。几乎可以将内联数组与任何 C# 数据结构一起使用。

运行时团队和其他库作者使用内联数组来提高应用的性能。内联数组使开发人员能够创建固定大小的 `struct` 类型数组。具有内联缓冲区的结构应提供类似于不安全的固定大小缓冲区的性能特征。

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

>---

### Boxing & unboxing 装箱和拆箱

装箱和拆箱的概念提供了值类型和引用类型之间的桥梁。允许值类型的任何值和 `object` 之间进行转换。拆箱和装箱支持对类型系统的统一视图，任何类型的值最终都可以被视为对象。

#### 装箱

装箱用于在垃圾回收堆中存储值类型。装箱是值类型到引用类型的隐式转换。对值类型装箱会在堆中分配一个对象实例，并将该值复制到新的对象中。对于可空值类型，若 `HasValue` 为 `false`，装箱时将产生空引用，否则将展开并装箱其基础类型的值。

装箱转换意味着对被装箱的值做一个副本。

> 存在以下装箱转换

- 从任意值类型到 `object`。
- 从任意值类型到 `System.ValueType`。
- 从任意枚举类型到 `System.Enum`。
- 从任意非空值类型到其实现的接口类型 `I`，包含协变接口。
- 从任意可空值类型到引用类型，其中存在 `T?` 的底层类型 `T` 到引用类型的装箱转换。

> 装箱模拟

- 假设每个值类型存在一个对应的装箱类来模拟装箱过程。

```csharp
interface ISample
{
    void Fun();
}
struct S : ISample
{
    public void Fun() { }
}
sealed class S_Boxing<T>(T value) : ISample where T : ISample
{
    T Value = value;
    public void Fun() => Value.Fun();
}
```

- 装箱一个值类型。

```csharp
S s = new S();
object box = s;

// 可以想象为
S s = new S();
object box = new S_Boxing<S>(s);
```

- 实际上假象的 `S_Boxing` 并不存在，类型 `S` 的装箱值具有运行时类型，可以使用类型测试左操作数是否为右操作数的装箱版本。

```csharp
int i = 123;
object box = i;
if(box is int)
    Console.WriteLine("Box contains an int");
```

#### 拆箱

取消装箱（拆箱）是从引用类型到值类型的显式转换。拆箱操作首先要检查对象实例的运行时类型，以确保它是给定目标值类型的装箱值，然后再将该值从实例复制到值类型变量中。

被取消装箱的项必须是对一个对象的引用，该对象是先前通过装箱该值类型的实例创建的。目标类型是非空值类型时，尝试取消装箱 `null` 会导致 `NullReferenceException`。若是可空值类型，包含 `null` 的引用类型可以拆箱到该目标类型，否则拆箱到其基础值类型。尝试取消装箱对不兼容值类型的引用会导致 `InvalidCastException`。

如果值类型必须被频繁装箱，那么在这些情况下最好避免使用值类型。可通过使用泛型集合（例如 `System.Collections.Generic.List<T>`）来避免装箱值类型。

装箱和取消装箱过程需要进行大量的计算。对值类型进行装箱时，必须创建一个全新的对象，这可能比简单的引用赋值用时最多长 20 倍。取消装箱的过程所需时间可达赋值操作的四倍。

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
/**
    Error: Incorrect unboxing.
    Unable to cast object of type 'System.Int32' to type 'System.Int16'.
    Correct unboxing.
 */
```

---
## 匿名类型

匿名类型提供了一种方便的方法，可用来将一组只读属性封装到单个对象中，而无需首先显式定义一个类型，每个属性的类型由编译器推断。类型名由编译器生成，并且不能在源代码级使用，可结合使用 `new` 运算符和对象初始值设定项创建匿名类型。

匿名类型包含一个或多个公共只读属性。无法包含其他种类的类成员（如方法或事件）。用来初始化属性的表达式不能为 null、匿名函数或指针类型。

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
## 泛型类型

泛型类型声明本身表示一个未绑定的构造类型，使用类型参数为其构造形成许多不同类型的 “蓝图”。使用泛型构造时，需要为类型参数绑定具体类型名称。

借助泛型，可以根据要处理的精确数据类型设计方法、委托、类、结构或接口，以提高代码的可重用性和类型安全性。泛型是为所存储或使用的一个或多个类型具有占位符（类型形参）的类、结构、接口、方法和委托。例如泛型集合类可以将类型形参用作其存储的对象类型的占位符，泛型方法可将其类型形参用作其返回值的类型或用作其形参之一的类型。

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

未绑定泛型类型本身不是类型，仅作为创建绑定的构造类型提供模板而存在，因此不能作为变量、参数、返回类型、或其他类的基类型。未绑定的泛型类型只能在 `typeof` 中使用。

```csharp
Sample s = new(); // 非泛型
Sample<int> s2 = new();  // 泛型 

class Sample;
class Sample<T>;
```

在泛型类型中声明的嵌套类型即使不直接指定类型参数，嵌套类型也被认为是泛型构造类型。

```csharp
class Sample<T>
{
    public class Nested;
}
class Sample: Sample<int>.Nested;
```

>---

### 封闭类型和开放类型

所有类型都可以分为开放式类型或封闭式类型。开放式类型是包含类型参数的类型，具体含义为：
- 类型参数定义开放式类型。
- 当前仅当数组的元素类型是开放式类型时，该数组为开放式类型。
- 当前仅当一个或多个类型参数是开放式类型时，构造类型才是开放式类型。

在运行时，泛型类型声明中的所有代码在通过将类型自变量应用于泛型声明而创建的封闭式构造类型的上下文中执行。泛型类型中的每个类型形参都绑定到特定的运行时类型。所有语句和表达式的运行时处理始终出现在封闭式类型中，并且开放式类型仅在编译时处理期间出现。

每个封闭式构造类型都有自己的静态变量集，它们不与任何其他封闭构造类型共享。由于开放式类型在运行时不存在，因此没有与开放式类型关联的静态变量。如果两个封闭式构造类型是从同一个未绑定的泛型类型构造的，则这两个封闭式构造类型都是相同的类型，并且其对应的类型参数是相同的类型。

作为一种类型，类型参数纯粹是编译时构造。在运行时，每个类型参数都绑定到泛型类型的类型参数来指定的运行时类型。因此，在运行时，用类型参数声明的变量的类型将是封闭构造类型。所有涉及类型参数的语句和表达式的运行时执行都使用作为该参数的类型实参提供的类型。

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

>---

### 类型约束

只要引用构造类型或泛型方法，就会根据泛型类型或方法上声明的类型参数约束检查所提供的类型实参。约束告知编译器类型参数必须具备的功能。在没有任何约束的情况下，类型参数可以是任何类型。约束指定类型参数的功能和预期，声明这些约束意味着可以使用约束类型的操作和方法调用。

可以在泛型的定义中使用 `where` 子句指定对类型参数的参数类型约束。对于每个 `where` 子句，根据每个约束检查类型形参对应的类型实参。如果给定的类型参数不满足一个或多个类型参数的约束，则会发生编译时错误。

```csharp
interface ISample<T> where T : class;
// 指定类型参数必须是引用类型
```

可以对类型参数指定的约束有：
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

#### 无约束的类型参数注释 `?` 和 default 约束

在 C# 8 中，`?` 批注只能用于显式约束为值类型或引用类型的类型参数。在 C#9 中，`?` 批注可应用于任何类型参数，而不受约束。除非在类型参数中显式地约束为 `struct`，否则注释只能在 `#nullable enable` 的上下文中使用。

```csharp
static T? FirstOrDefault<T>(this IEnumerable<T> collection) { ... };   // 不受约束的类型参数批注
```

如果类型参数 `T` 替换为引用类型，则 `T?` 表示该引用类型的可空实例。

```csharp
var s1 = new string[0].FirstOrDefault();  // string? s1
var s2 = new string?[0].FirstOrDefault(); // string? s2
```

如果 `T` 用值类型替换，则 `T?` 表示为 `T` 的一个实例。 

```csharp
var i1 = new int[0].FirstOrDefault();   // int i1
var i2 = new int?[0].FirstOrDefault();  // int? i2
```

如果 `T` 使用批注类型替换 `U?`，则 `T?` 表示批注的类型 `U?` 而不是 `U??`。如果 `T` 将替换为类型 `U`，则 `T?` 表示 `U?`，即使在上下文中也是如此 `#nullable disable`。 

```csharp
var u1 = new U[0].FirstOrDefault();  // U? u1
var u2 = new U?[0].FirstOrDefault(); // U? u2, 例如 T 是 int?, 则 'int?'? 仍表示 int? 
#nullable disable
var u3 = new U[0].FirstOrDefault();  // U? u3, 例如 T 是 int, 则 'int'? 表示 int?
```

对于 `T?` 的返回值，相当于 `[MaybeNull] T`。对于参数 `T?`，相当于 `[AllowNull] T`。

```csharp
using System.Diagnostics.CodeAnalysis;
public abstract class A
{
    [return: MaybeNull] public abstract T F1<T>();
    public abstract void F2<T>([AllowNull] T t);
}
public class B : A
{
    public override T? F1<T>() where T : default { return default; }   // matches A.F1<T>()
    public override void F2<T>(T? t) where T : default { }    // matches A.F2<T>()
}
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

#### 约束继承

对于泛型类型的类型参数和它们的约束，都不会被派生类继承，因为类型参数不是成员。派生泛型的类型参数是其泛型基类的类型参数，因此类型参数必须具有等同（或更强）于基类的约束。 

```csharp
class A;
class B : A;

class A<T>;
class B<T> : A<T> where T : A;
class C<T> : B<T> /* where T : B*/;    
// 类型参数的约束不被继承，可以声明约束为同等或更强的限制
```

而基类的虚泛型方法或接口泛型方法被继承并重写或实现时，重写或显式接口实现方法的约束是从基方法继承的，因此不能直接指定这些约束，除非指定 `class` 或 `struct` 约束。

```csharp
class Sample
{
    public virtual void FunA<T>() where T : Sample { }
    public virtual void FunB<T>() where T : struct { }
}
class Derived : Sample
{
    public override void FunA<T>() where T : Sample // err
        => base.FunA<T>();
    public override void FunB<T>() /*where T : struct*/  // okay
        => base.FunB<T>();
}
```

在泛型类继承的情况下，不仅可以保留基类本来的约束（这是必需的），还可添加额外的约束，从而对派生类的类型参数进行更大的限制。但重写虚泛型方法时，需遵守和基类方法完全一样的约束。额外的约束会破坏多态性，所以不允许新增约束。另外，重写方法的类型参数约束是隐式继承的。

#### 泛型类型中的静态成员

使用泛型类型时指定类型参数时，运行时将创建该类型参数的封闭式构造类型。从同一泛型类型的构建的不同构造类型之间，各构造泛型类型的静态成员（包括静态构造函数、字段、方法、属性等）独立存在。在首次调用该类型时，会首先调用它的静态构造函数。对于泛型接口类型的不能构造类型之间，静态成员（非抽象）也是相互独立的。

```csharp
interface ISample<T>
{
    static ISample() => Console.WriteLine($"Static ISample() >> {typeof(T).Name}");
    public static T? Default { get; set; } = default;
}
class Sample<T>
{
    static Sample() => Console.WriteLine($"Static Sample() >> {typeof(T).Name}");
    public static T? Default { get; set; } = default;
}
class Program
{
    static void Main(string[] args)
    {
        var I1 = ISample<int>.Default;
        Console.WriteLine("-------------");
        var I2 = ISample<string>.Default;
        Console.WriteLine("-------------");
        var s1 = Sample<float>.Default;
        Console.WriteLine("-------------");
        var s2 = Sample<object>.Default;
    }
    /*
    Static ISample() >> Int32
    -------------
    Static ISample() >> String
    -------------
    Static Sample() >> Single
    -------------
    Static Sample() >> Object
    */
}
```

>---

### 协变与逆变

借助泛型类型参数的协变和逆变，可以使用类型自变量的派生程度比目标构造类型更高（协变）或更低（逆变）的构造泛型类型。协变和逆变统称为 “变体”，未标记为协变或逆变的泛型类型参数称为 “固定参数” 。

协变和逆变类型参数仅限于泛型接口和泛型委托类型，变体仅适用于与引用类型。当类型参数指定为值类型时，该类型参数对于生成的构造类型是不可变的。

使用 `in` 关键字指定类型参数是逆变的，逆变的类型参数可以用作泛型接口的方法或泛型委托的参数类型。`out` 关键字指定类型参数是协变的，协变的类型参数可用作接口方法的返回类型。

```csharp
delegate TResult GenericDelegate<in T, out TResult>(T arg);
interface IGeneric<in T, out TResult>
{
    TResult GetResult(T arg);
}
```

协变和逆变能够实现委托类型、泛型接口类型和泛型类型参数的隐式引用转换。

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

#### 数组的协变

数组的协变使派生程度更大的类型的数组能够隐式转换为派生程度更小的类型的数组。

```csharp
IEnumerable<object> e = new List<string>();
IEnumerable<object> e2 = new List<int>();  // CS0266，值类型不支持协变
IEnumerable<object>[] enumerables = new List<string>[] { }; // 数组的协变
```

>---

### 泛型的内部机制

泛型类的类型参数成了元数据，CLR 在需要时会利用它们构造恰当的类。所以，泛型支持继承、多态性以及封装。可用泛型定义方法、属性、字段、类、接口和委托。泛型类编译后与普通类无太大差异，编译结果无非就是元数据和参数化的 CIL。

```csharp
// csharp
class Sample<T> where T : ISample
{
    private T[] _items;
    // rest ...
}
// MSIL
.class private auto ansi beforefieldinit 
    Sample`1<(ISample) T>     // 约束，`1 表示类型参数的数目，表示一个占位
	extends [System.Runtime]System.Object
{
    // rest ...
    .field private !T[] _items   // ! 标记占位的位置
    // ...
}
```

#### 实例化基于值类型的泛型

用值类型作为类型参数首次构造一个泛型类型时，CLR 会将指定的类型参数放到 CIL 中合适的位置，从而创建一个具体化的泛型类型。CLR 会针对每个新的 “参数值类型” 创建一个新的具体化泛型类型。使用具体化值类型的类，好处在于能获得较好的性能。代码能避免转换和装箱，因为每个具体的泛型类都原生包含值类型。

#### 实例化基于引用类型的泛型

对于引用类型，泛型的工作方式稍有不同。使用引用类型作为类型参数首次构造一个泛型类型时，CLR 会在 CIL 代码中用 `object` 引用替换类型参数来创建一个具体化的泛型类型（而不是基于所提供的类型实参来创建一个具体化的泛型类型）。之后每次用引用类型参数实例化一个构造好的类型，CLR 都重用之前生成好的泛型类型的版本，即使提供的引用类型与第一次不同。

---
## record 记录类型

从 C#9 开始，可以使用 `record` 修饰符定义一个引用类型，用来提供用于封装数据的内置功能。C#10 允许 `record class` 语法作为同义词来阐明引用类型，并允许 `record struct` 使用相同功能定义值类型。

```ANTLR
// 记录
record_class_declaration
    : [ attributes ]? class_modifier* partial? record_type identifier <type_parameter_list>? ( parameter_list? )? 
      record_bases?  type_parameter_constraints* { record_body }
record_type
    : record 'or' record class  
record_bases
    | record <class>? identifier : record_class_base, interface_bases 
    ;

// 记录结构
record_struct_declaration
    : [ attributes ]? struct_modifier* partial? record_type identifier <type_parameter_list>? ( parameter_list? )? 
      record_bases?  type_parameter_constraints* { record_body }
record_type
    : record struct
record_bases
    | record struct identifier : interface_bases
    ;
```

记录不能从类继承，除非是 `object`，而类不能从记录继承。记录可以从其他记录继承。记录的定义声明中可以包含一组参数列表（主构造函数），以构造位置记录。该记录参数不能使用 `ref`、`out`、`this` 修饰，可以使用 `in` 或 `params` 修饰。

```csharp
// 记录
abstract record BaseRecord;  
record Sample<T>(int X, in T Y) : BaseRecord, IDisposable where T : unmanaged
{
    public void Dispose() { }
}

// 记录结构
record struct Sample(params int[] Values) : IEnumerable<int>
{
    public IEnumerator<int> GetEnumerator() => Values.AsEnumerable().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
```

>---

### 位置记录

位置记录：在记录上声明主构造函数时，编译器会为记录类型自动生成一个位置构造函数，同时根据位置参数自动生成一个解构函数 `Deconstruct` 以支持将位置记录解构为元组，并在该位置记录中为主构造函数的参数生成公共属性：
- 对于 `record`，编译器为位置参数生成 `get/init` 公共属性。
- 对于 `record struct`，编译器为位置参数生成 `get/set` 公共属性。
- 对于 `readonly record struct`，编译器为位置参数生成 `get/init` 公共属性。

```csharp
public record Person(string FirstName, string LastName);
// 相当于
public record Person{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    // 结构函数在记录中自动生成，可以声明方法重载或显式声明默认的 Deconstruct
    public void Deconstruct(out string firstName, out string lastName) 
        => (firstName, lastName) = (FirstName, LastName);
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

若要覆盖编译器自动生成的属性，可以在源中自行定义同名的属性，并从记录的位置参数初始化该属性。

```csharp
public record Person(string FirstName, string LastName, string Id)
{
    internal string Id { get; init; } = Id;
}
```

#### 位置记录中的解构函数

为了支持将 `record` 对象能解构成元组，我们给 `record` 添加解构函数 `Deconstruct`。声明主构造函数的记录定义为位置记录，该位置记录会为主构造函数中的位置参数自动生成一个解构函数。

- 显式声明一个解构函数。

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

> 重定义解构函数或重载解构函数 `Deconstruct`

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

>---

### 记录的相等性

对于 `class` 类型，两个对象引用内存中的同一对象，则这两个对象相等。
对于 `struct` 类型，两个对象是相同的类型并且存储相同的值，则这两个对象相等。
对于 `record` 类型，如果两个对象是相同的类型且存储相同的值，则这两个对象相等。

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

为实现值相等性，编译器为记录类型合成了几种方法：
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

>---

### 记录的复制与克隆

若需要复制包含一些修改的实例，可以使用 `with` 表达式来实现非破坏性变化。`with` 表达式创建一个新的记录实例，该实例是现有记录实例的一个副本，并修改了指定的属性或字段。

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

`with` 表达式可以设置位置属性或使用标准属性语法创建的属性。显式声明属性必须有一个 `init` 或 `set` 访问器才能在 `with` 表达式中进行更改。`with` 表达式的结果是一个浅的副本，这意味着对于引用属性，只复制对实例的引用。原始记录和副本最终都具有对同一对象的引用。

记录类型包含两个复制成员：
- 接受记录类型的单个参数的构造函数 `recordType(recordType origin)`，它被称为 “复制构造函数”。
- 具有编译器保留名称的合成公共无参实例 `Clone` 方法。

复制构造函数的目的是将状态从目标源对象复制到正在创建的新实例，这个构造函数不运行记录声明中存在的任何实例字段或属性的初始值项。若没有显式声明复制构造函数，则编辑器将自动合成。密封记录的复制构造函数为 `private`，可继承的记录则是 `protected`。

虚拟克隆方法返回由复制构造函数初始化的新记录。用户不能替代克隆方法，也不能在任意记录类型中创建名为 `Clone` 的成员。`Clone` 方法是由编译器自动合成的，当使用 `with` 表达式时，编译器将创建调用克隆方法的代码，而 `Clone` 方法将返回调用复制构造函数的结果。

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

>---

### 记录的格式化字符串打印

记录类型具有编译器生成的 `ToString` 方法，可显式公共属性和字段的名称和值。`ToString` 方法返回一个格式如下的字符串：`<record type name> { <property name> = <value>, <property name> = <value>, ...}`，其中每个 `<value>` 打印的字符串是属性或字段对应类型的 `ToString()`。

为了实现此功能，编译器在 `record class` 类型中合成了一个虚拟 `PrintMembers` 方法和一个 `ToString` 替代，此成员在 `record struct` 类型中为 `private`。

```csharp
using System.Runtime.CompilerServices;
using System.Text;

Console.WriteLine(new Point(0,0));  // Point { x = 0, y = 0 }
public record struct Point(int x, int y)
{
    [CompilerGenerated]
    public override readonly string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("Point");
        stringBuilder.Append(" { ");
        if (PrintMembers(stringBuilder))
        {
            stringBuilder.Append(' ');
        }
        stringBuilder.Append('}');
        return stringBuilder.ToString();
    }

    [CompilerGenerated]
    private readonly bool PrintMembers(StringBuilder builder)
    {
        builder.Append("x = ");
        builder.Append(x.ToString());
        builder.Append(", y = ");
        builder.Append(y.ToString());
        return true;
    }
}
```

> 自定义 ToString

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

> 自定义 PrintMembers

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

>---

### 继承

一条记录可以从另一条记录继承。派生记录为基记录主构造函数中的所有参数声明位置参数，基记录声明并初始化这些属性；派生记录不会隐藏它们，而只会创建和初始化未在其基记录中声明的参数的属性。

要使两个记录变量相等，运行时类型必须相等。包含变量的类型可能不同，但相等性测试依赖于实际对象的运行时类型，而不是声明的变量类型。

`with` 表达式结果的运行时类型与表达式操作数相同：运行时类型的所有属性都会被复制，但用户只能设置编译时类型的属性。

派生记录类型的合成 `PrintMembers` 方法并调用基实现 `base.PrintMembers()`。结果是派生类型和基类型的所有公共属性和字段都包含在 `ToString` 输出中。派生记录也会重新合成基记录的 `EqualityContract`、`GetHashCode`、`Deconstruct` 方法。 

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
        Console.WriteLine(teacher == student);       // output: False
        Student student2 = new Student("Nancy", "Davolio", 3);
        Console.WriteLine(student2 == student);      // output: True

        /* with 表达式 */
        Person clone_teacher = teacher with { FirstName = "Tom" }; // 无法定义 Grade，虽然在运行时类型包含此属性
        Teacher teacher2 = (Teacher)teacher with { Grade = 6 };
        Console.WriteLine(teacher2);
        // output: Teacher { FirstName = Nancy, LastName = Davolio, Grade = 6 }

        /* 解构函数 */
        var (first, second) = (Teacher)teacher;       // 支持基记录的解构函数
        var (first2, second2, grade) = (Teacher)teacher; // 在 Teacher 重新生成的解构函数
    }
}
```

---
## 指针类型

C# 的核心语言与 C/C++ 的显著区别在于它没有将指针作为数据类型。相反，C# 提供了引用和创建由垃圾收集器管理的对象的能力。这种设计加上其他特性，使 C# 成为一种比 C/C++ 更安全的语言。在核心 C# 语言中，不可能有未初始化的变量、“悬空” 指针（被释放或删除的内存区域）或超出数组边界的索引表达式。因此，经常困扰 C/C++ 程序的所有类型的 bug 都被消除了。

尽管实际上 C/C++ 中的每个指针类型结构在 C# 中都有对应的引用类型，但在某些情况下，必须访问指针类型。例如，如果不访问指针，与底层操作系统接口、访问内存映射设备或实现时间关键型算法可能是不可能或不实际的。为了满足这种需求，C# 提供了编写不安全代码的能力。

在不安全代码中，可以声明和操作指针，执行指针和整型之间的转换，获取变量的地址，等等。从某种意义上说，编写不安全代码很像在 C# 程序中编写 C 代码。从开发人员和用户的角度来看，

不安全代码实际上是一个 “安全” 的特性。不安全的代码应该用 `unsafe` 标记清楚，这样开发人员就不可能不小心使用不安全的特性，并且执行引擎的工作是确保不安全的代码不能在不受信任的环境中执行。

>---

### 不安全上下文

C# 支持不安全上下文，用户可在其中编写不可验证的代码。在不安全的上下文中，代码可使用指针、分配和释放内存块，以及使用函数指针调用方法。可以将方法、类型和代码块定义为不安全。

通过在类型、成员或局部函数的声明中包含不安全修饰符 `unsafe`，或使用 `unsafe { ... }` 语句引入不安全上下文。

`unsafe` 修饰符可以标记类型声明（类、结构、接口、委托）和成员声明（字段、方法、属性、事件、索引器、运算符、实例构造函数、终结器、静态构造函数、局部函数）的整个文本范围为不安全上下文。也可以在函数成员的块中使用 `unsafe { ... }` 语句引入不安全上下文块。

调用需要指针的本机函数时，需使用不安全代码，因此可能会引发安全风险和稳定性风险。在某些情况下，通过移除数组绑定检查，不安全代码可提高应用程序的性能。

```csharp
int* p;         // p 是指向整数的指针。
int** p;        // p 是指向整数的指针的指针。
int*[] p;       // p 是指向整数的指针的一维数组。
char* p;        // p 是指向字符的指针。
void* p;        // p 是指向未知类型的指针。

int* p1, p2, p3;    // Ok
int *p1, *p2, *p3;  // Invalid in C#
```

>---

### 指针声明

在不安全的上下文中，可以声明指针类型或指针类型的数组：

```ANTLR
pointer_type
    : value_type (*)+
    | void (*)*
```

与引用（引用类型的值）不同，指针不受垃圾收集器的跟踪，垃圾收集器不知道指针和它们所指向的数据。因此，不允许指针指向引用或包含引用的结构体。指针类型本身是非托管类型，因此一个指针类型可以指向另一个指针类型。

指针类型是一种单独的类型。与引用类型和值类型不同，指针类型不从对象继承，并且指针类型和对象之间不存在转换。特别是，指针不支持装箱和拆箱。但是，允许在不同指针类型之间以及指针类型与整型之间进行转换。

```csharp
unsafe struct Sample
{
    byte* pb;
    char* pc;
    int** pptr;
    int*[] parr;
    void* p;

    Sample* pS;
}
```

类型为 `T*` 的指针的值表示 `T` 类型变量的地址。地址运算符 `&` 用于获取类型变量的地址，指针间接操作符 `*` 可用于访问该变量。

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

`void*` 类型表示指向未知类型的指针。由于指向的类型未知，间接操作符不能应用于 `void*` 类型的指针，也不能在这种指针上执行任何算术运算。但是，`void*` 类型的指针可以被强制转换为任何其他指针类型，并与其他指针类型的值进行比较。

指针类型不能用作类型参数，不能用作动态绑定操作的子表达式的类型，不能则用作扩展方法的第一个形参的类型，不能是匿名类型的元素的值。但是可以用作是 `volatile` 字段的类型，动态类型的指针。

```csharp
unsafe class Sampple
{
    void* unknown;
    volatile dynamic* pd;
}
```

指针类型可以作为 `in`、`ref`、`out` 的参数传递，但是可能会导致未定义行为。指针可能被设置为指向一个局部变量，而该局部变量在被调用的方法返回时不再存在，或者指针指向的固定对象不再固定。

```csharp
class Sample
{
    static int value = 20;
    unsafe static void F(out int* pi1, ref int* pi2)
    {
        int i = 10;
        pi1 = &i;       // return address of local variable
        fixed (int* pj = &value)
            pi2 = pj;   // return address that will soon not be fixed
    }
    static void newFunInStack()
    {
        float i = 3.1415f;
    }
    static void Main()
    {
        int i = 15;
        unsafe
        {
            int* px1;
            int* px2 = &i;
            F(out px1, ref px2);
            newFunInStack();
            int v1 = *px1; // undefined
            Console.WriteLine(v1);  
            int v2 = *px2; // undefined
            Console.WriteLine(v2);
        }
    }
}
```

>---

### 指针操作

在不安全的上下文中，有几种方式可用于操作所有 **非函数指针的指针类型**：
- 指针间接操作符 `*` 用于访问指针类型指向的值。
- 指针成员访问 `->` 用于通过指针访问结构体的成员。
- `[]` 操作符用于索引指针。
- 地址运算符符 `&` 可用于获取变量的地址。
- `++` 和 `--` 运算符可用于指针的自增和自减操作。
- 二元 `+` 和 `-` 运算符用于执行指针和整数的算数。
- `==`、`!=`、`<=` 和 `>=` 操作符可用于比较指针。
- 可以使用 `stackalloc` 操作符从调用堆栈中分配内存，并赋值给指针类型或 `Span<T>` 和 `ReadOnlySpan<T>`。
- `fixed` 语句可以用来临时固定一个变量，以便获得它的地址。

#### 固定与可移动变量

地址运算符 `&` 和固定 `fixed` 语句将变量分为两类：固定变量和可移动变量：
- 固定变量驻留在不受垃圾收集器操作影响的存储位置（固定变量的例子包括局部变量、值形参和通过解引用指针创建的变量）。
- 可移动变量驻留在由垃圾收集器重新定位或处理的存储位置中（可移动变量的例子包括对象中的字段、数组中的元素、引用传递的参数）。

`&` 运算符允许不受限制地获取固定变量的地址。由于可移动变量会被垃圾回收器重新定位或处理，因此只能通过固定语句获得可移动变量的地址，并且该地址仅在该固定语句的持续时间内有效。

```csharp
class Sample
{
    static unsafe void Fun(int len, int[] arr)
    {
        int * plen = &len; // 固定变量
        fixed (int* p = arr)  // 可移动变量
        {
            int * pArr = p;
            for (int i = 0;i < len; i++)
                Console.WriteLine(pArr[i]); ;
        }
    }
    static void Main(string[] args)
    {
        Fun(10, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
    }
}
```

#### 结构成员访问

对于指向结构体的指针，可以通过 `->` 访问结构体的成员。`p->M` 的等价于 `(*p).M`。 

```csharp
class Test
{
    struct Point
    {
        public int x;
        public int y;
        public override string ToString() => $"({x},{y})";
    }
    static void Main()
    {
        Point point;
        unsafe
        {
            Point* p = &point;
            p->x = 10;
            //  (*p).x = 10;
            p->y = 20;
            //  (*p).y = 20;
            Console.WriteLine(p->ToString());
        }
    }
}
```

#### 数组元素访问

对于指向数组类型的指针，可以像数组元素访问一样，通过索引器语法使用指针访问数组元素。形式为 `P[E]` 的指针元素访问被精确地求值为 `(P + E)`。

指针元素访问操作符不检查越界错误，并且访问越界元素时的行为未定义。

```csharp
class Test
{
    static void Main()
    {
        unsafe
        {
            char* p = stackalloc char[256];
            for (int i = 0; i < 256; i++)
            {
                p[i] = (char)i;
                //  *(p + i) = (char)i;
            }
        }
    }
}
```

#### 指针算数

给定一个指针类型为 `T*` 的表达式 `p` 和一个类型为整数类型的表达式 `N`，表达式 `p + N` 计算类型为 `T*` 的指针值，该指针值是由 `p` 给出的地址加上 `N * sizeof(T)` 得到的。表达式 `p - N` 计算类型为 `T*` 的指针值，该指针值是由 `p` 给出的地址减去 `N * sizeof(T)` 得到的。

给定指针类型为 `T*` 的两个表达式 `P` 和 `Q`，表达式 `P - Q` 计算 `P` 和 `Q` 给出的地址之差，然后将该差除以 `sizeof(T)`。结果的类型总是 `long`。实际上，`P - Q` 计算为 `((long)(P) - (long)(Q)) / sizeof(T)`。

如果指针算术操作溢出指针类型的域，则以实现定义的方式截断结果，但不会产生异常。

```csharp
class Sample
{
    static unsafe void Main()
    {
        int* values = stackalloc int[20];
        int* p = &values[1];
        int* q = &values[15];
        Console.WriteLine($"p - q = {p - q}");  // -14
        Console.WriteLine($"q - p = {q - p}");  // 14
    }
}
```

#### 指针比较

比较运算符比较两个操作数给出的地址，就像它们是无符号整数一样。指向高位地址的指针大于指向低位地址的指针。

```csharp
class Sample
{
    static unsafe void Main()
    {
        int* values = stackalloc int[20];
        int* p = &values[1];
        int* q = &values[15];
        Console.WriteLine(p > q); // false
    }
}
```

#### fixed 语句

在不安全的上下文中，固定语句允许使用一个额外的构造，即 `fixed` 语句，它用于 “固定”一个可移动的变量，使其地址在语句期间保持不变。

每个固定指针声明给定指针类型的一个局部变量，并用相应计算的地址初始化该局部变量。由固定语句声明的局部变量被认为是只读的。如果内嵌语句试图修改该局部变量（通过赋值或 `++` 和 `--` 运算符）或将其作为 `ref`、`in`、`out` 参数传递，则会发生编译时错误。

`fixed` 语句可防止垃圾回收器重新定位可移动变量，并声明指向该变量的指针。固定变量的地址在语句的持续时间内不会更改。只能在相应的 `fixed` 语句中使用声明的指针，且声明的指针是只读的，无法修改。

`fixed` 而可初始化声明使用数组的指针、使用变量的地址、使用实现名为 `GetPinnableReference` 的方法的类型实例（方法返回非托管类型的 `ref` 变量，例如 .NET 类型 `System.Span<T>` 和 `System.ReadOnlySpan<T>`）、使用字符串、使用固定大小的缓冲区（堆栈上声明的 `stackalloc` 内存不需要固定）。

> 使用数组

```csharp
unsafe
{
    int[] arr = [10, 20, 30, 40, 50];
    fixed (int* p = arr)
    {
        int index = 0;
        foreach (int i in arr)
        {
            p[index] = i * i; 
            index++;
        }
        Console.WriteLine(string.Join(", ", arr));
        // Output: 100, 400, 900, 1600, 2500
    }
}
```

> 使用变量的地址

```csharp
unsafe
{
    int[] numbers = { 10, 20, 30 };
    fixed (int* toFirst = &numbers[0], toLast = &numbers[^1])
        Console.WriteLine(toLast - toFirst);  // output: 2
}
```

> 使用实现名为 `GetPinnableReference` 的方法的类型实例

```csharp
NumberArray arr = new(1, 2, 3, 4, 5, 6);
unsafe
{
    fixed(int* p = arr)
        for(int i = 0;i< 6; i++)
            Console.WriteLine(p[i]);
}
record NumberArray(params int[] arr)
{
    public ref int GetPinnableReference() => ref arr[0];
}
```

> 使用字符串

```csharp
ToUpper("Hello, World"); // Output: HELLO, WORLD

unsafe static void ToUpper(string str)
{
    fixed(char* f = str)
    {
        int index = 0;
        foreach (char c in str)
            f[index] = char.ToUpper(f[index++]);
    }
    Console.WriteLine(str);
}
```

>---

### 固定大小的缓冲区

可以使用 `fixed` 关键字来创建在数据结构中具有固定大小的数组的缓冲区。当编写与其他语言或平台的数据源进行互操作的方法时，固定大小的缓冲区很有用。

固定大小的缓冲区可以采用允许用于常规结构成员的任何属性或修饰符。唯一的限制是数组类型必须为 `bool`、`byte`、`char`、`short`、`int`、`long`、`sbyte`、`ushort`、`uint`、`ulong`、`float` 或 `double`。
  
```csharp
internal unsafe struct Buffer
{
    public fixed char fixedBuffer[128];
}
```

在安全代码中，包含数组的 C# 结构不包含该数组的元素，而是包含对该数组的引用。当在不安全的代码块中使用数组时，可以在结构中嵌入固定大小的数组。使用 `fixed` 语句获取指向数组第一个元素的指针，通过此指针访问数组的元素。`fixed` 语句将 `fixedBuffer` 实例字段固定到内存中的特定位置。

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

固定大小的缓冲区使用 `System.Runtime.CompilerServices.UnsafeValueTypeAttribute` 进行编译，它指示公共语言运行时 CLR 某个类型包含可能溢出的非托管数组。

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

与固定缓冲区不同的是，使用 `stackalloc` 分配的内存还会在 CLR 中自动启用缓冲区溢出检测功能。

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

>--- 

### 函数指针

C# 提供 `delegate` 委托类型来定义安全函数指针对象。调用委托时，需要实例化从 `System.Delegate` 派生的类型并对其 `Invoke` 方法进行虚拟方法调用，该虚拟调用使用 IL 指令 `callvirt`

可以使用 `delegate*` 语法声明函数指针。编译器将使用 IL 指令 `calli` 指令来调用函数，而不是实例化为委托对象并调用 `Invoke`。在性能关键的代码路径中，使用 IL 指令 `calli` 效率更高。

```csharp
// 委托定义参数
public static T Combine<T>(Func<T, T, T> combinator, T left, T right) => combinator(left, right);
// 函数指针定义参数
public static T UnsafeCombine<T>(delegate*<T, T, T> combinator, T left, T right) => combinator(left, right);
```

函数指针只能在 `unsafe` 上下文中声明，只能在静态成员方法或静态本地方法使用地址运算符 `&`。

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

#### 函数指针语法

```ANTLR
delegate* calling_convention_specifier? <parameter_list, return_type> 

calling_convention_specifier? : 可选的调用约定说明符, 默认为 managed
    managed : 默认调用约定
    unmanaged : 非托管调用约定, 未显式指定调用约定类别, 则使用运行时平台默认语法
    unmanaged [ Calling_convertion <,Calling_convertion ...>? ] : 指定特定的非托管调用约定, 一到若干个进行组合

Calling_convertion : 调用约定
    Cdecl : 调用方清理堆栈
    stdcall : 被调用方清理堆栈, 这是从托管代码调用非托管函数的默认约定
    Thiscall : 指定方法调用的第一个参数是 this 指针, 该指针存储在寄存器 ECX 中
    Fastcall : 调用约定指定在寄存器中传递函数的参数 (如果可能), NET 可能不支持 
    MemberFunction : 指示调用函数变体
    SuppressGCTransition : 指示抑制 GC 转换作为调用约定的一部分
```

ECMA-335 将 `Calling_convertion` *调用约定* 定义为函数指针类型签名的一部分。默认的调用约定是 `managed`，非托管调用约定可以通过在 `delegate*` 语法后放置一个 `unmanaged` 关键字来指定，该关键字将使用运行时平台默认的调用约定类别。

```csharp
unsafe
{
    // 此方法具有托管调用约定。managed 可省略
    delegate* managed<int, int> pManagedFun;

    // 此方法将使用运行时平台上的默认非托管调用约定。这取决于平台和体系结构，并由 CLR 在运行时确定。
    delegate* unmanaged<int, int> pUnmanagedFun;
}
```

如果没有提供 `calling_convention_specifier`，则使用默认值 `managed`。

```csharp
delegate int Func1(string s);
delegate Func1 Func2(Func1 f);

// Function pointer equivalent without calling convention
delegate*<string, int>;
delegate*<delegate*<string, int>, delegate*<string, int>>;

// Function pointer equivalent with calling convention
delegate* managed<string, int>;
delegate*<delegate* managed<string, int>, delegate*<string, int>>;
```

可以为 `unmanaged` 非托管调用指定特定的约定类别：通过在 `System.Runtime.CompilerServices` 命名空间中以 `CallConv` 开头的任何类型并去掉去掉 `CallConv` 前缀后的名称，做为 `unmanaged [Calling_convertion <,Calling_convertion>]` 声明的 `Calling_convertion`。

```csharp
using System.Runtime.CompilerServices;

// 非托管调用约定类别
CallConvCdecl Cdecl;
CallConvFastcall Fastcall;
CallConvStdcall Stdcall;
CallConvThiscall Thiscall;
CallConvMemberFunction MemberFunction;
CallConvSuppressGCTransition SuppressGCTransition;

unsafe
{
    // 此方法将使用 Cdecl 调用约定,
    // Cdecl 映射到 System.Runtime.CompilerServices.CallConvCdecl
    delegate* unmanaged[Cdecl]<int, int> pFunCdecl;

    // 此方法将使用 Stdcall 调用约定，并抑制 GC 转换,
    // Stdcall 映射到 System.Runtime.CompilerServices.CallConvStdcall
    // SuppressGCTransition 映射到 System.Runtime.Compilerservices.Callconvsuppressgctransition
    delegate* unmanaged[Stdcall, SuppressGCTransition]<int, int> pFunStdcall;
}
```

函数指针类型之间的转换是基于它们的签名（包括调用约定）完成的。

```csharp
unsafe class Example
{
    void Conversions()
    {
        delegate*<int, int, int> p1 = ...;
        delegate* managed<int, int, int> p2 = ...;
        delegate* unmanaged<int, int, int> p3 = ...;

        p1 = p2; // okay : p1, p2 具有相同的签名 
        Console.WriteLine(p2 == p1); // True
        p2 = p3; // error : 调用约定不兼容
    }
}
```

`delegate*` 类型是指针类型，这意味着它具有标准类型的所有功能和限制：
- 功能：
  - 指针仅在不安全的上下文中有效。
  - 包含 `delegate*` 参数或返回类型的方法只能从不安全的上下文中调用。
  - 不能转换为 `object`。
  - 不能用作泛型类型参数。
  - 可以隐式转换 `delegate*` 到 `void*`。
  - 可以显式转换 `void*` 到 `delegate*`。
- 限制：
  - 自定义特性不能应用于 `delegate*` 或它的其任何元素。
  - 不能将 `delegate*` 参数标记为 `params`。
  - `delegate*` 类型具有普通指针类型的所有限制。
  - 指针运算不能直接在函数指针类型上执行。
  - 仅 `==`、`!=`、`<`、`>`、`<=`、`>=` 运算符可用于比较函数指针。

#### 函数指针的目标方法

允许将方法组作为 `&` 地址运算符的操作数，表达式返回类型是一个函数指针类型 `delegate*`，它具有与目标方法相同的签名和托管调用约定。

在不安全的上下文中，如果满足以下所有条件，则方法 `M` 与函数指针类型 `F` 兼容：
- `M` 和 `F` 具有相同数量的参数，并且 `M` 中的每个参数与 `F` 中对应的参数具有相同的 `ref`、`out` 或 `in` 修饰符。
- 对于每个值形参，存在从 `M` 中的形参类型到 `F` 中相应形参类型的恒等转换、隐式引用转换或隐式指针转换。
- 对于每一个 `ref`、`out` 或 `in` 形参，`M` 中的形参类型与 `F` 中对应的形参类型相同。
- 如果返回类型是按值返回（无 `ref` 或 `ref readonly`），则存在从 `F` 的返回类型到 `M` 的返回类型的恒等、隐式引用或隐式指针转换。
- 如果返回类型是引用（`ref` 或 `ref readonly`），则 `F` 的返回类型和修饰符与 `M` 的返回类型和修饰符相同。
- `M` 的调用约定与 `F` 的调用约定相同。这既包括调用约定位（`unmanaged` 或 `managed`），也包括在非托管标识符中指定的任何调用约定类别。
- `M` 是静态方法。

```csharp
unsafe class Util
{
    public static void Log() => Console.WriteLine("Log");
    public static void Log(string mess) => Console.WriteLine(mess);
    public static void Log(int i) => Console.WriteLine(i);

    static void Main()
    {
        delegate*<void> a1 = &Log; // Log()
        delegate*<int, void> a2 = &Log; // Log(int i)

        // Error: 从方法组 Log 到 void* 的模糊转换
        void* v = &Log;
    }
}
```

#### 调用约定的元数据表示

调用约定通过签名中的 `CallKind` 标志和签名开头的零个或多个 `modopts` 的组合在元数据中的方法签名中进行编码。ECMA-335 目前在 `CallKind` 标志中声明了以下元素。其中，C# 中的函数指针将支持除 `varargs` 以外的所有变量：

```ANTLR
CallKind
   : default
   | unmanaged cdecl
   | unmanaged fastcall
   | unmanaged thiscall
   | unmanaged stdcall
   | varargs
   ;
```

> 从 `calling_conventions_specifier` 映射到 `CallKind`

省略的 `calling_convention_specifier` 或指定为 `managed` 的 `calling_convention_specifier` 映射到默认的 `CallKind`。这是任何未归属于 `UnmanagedCallersOnlyAttribute` 的方法的默认 `CallKind`。

```csharp
unsafe class Sample
{
    public static delegate* managed<string, void> pWriteLine = &WriteLine;

    static void WriteLine(string mess) => Console.WriteLine(mess);
}
```

标记有 `UnmanagedCallersOnlyAttribute` 的任何方法均可从 `Native` 代码中直接调用。可以使用 C# 的 address-of 运算符 `&` 将函数加载到局部变量，并作为回调传递给 `Native` 方法。

```csharp
unsafe class Sample
{
    public static delegate* unmanaged[Cdecl]<int, int> pFun1;

    // Target will be invoked using the cdecl calling convention
    [UnmanagedCallersOnly(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static int Fun(int val) => val;

    [DllImport("NativeLibrary", EntryPoint = "NativePointer")]
    internal static extern void NativeMethod(delegate* unmanaged[Cdecl]<int, int> pFun);

    static void Main()
    {
        pFun1 = &Fun;
        // Calling in C#
        Console.WriteLine(pFun1(1000));  
        // or Callback of Native Method 
        NativeMethod(pFun1);
    }
}
```

C# 识别 4 种特殊标识符，并映射到 ECMA-335 中特定的现有非托管 `CallKind`。为了实现这种映射，必须单独指定这些标识符，不能指定其他标识符，并且将此标识编码到 `unmanaged` 的 `Calling_convertion` 规范中。这些标识符是 `Cdecl`、`Thiscall`、`Stdcall` 和`Fastcall`，它们分别对应于 `unmanaged Cdecl`、`unmanaged Thiscall`、`unmanaged Stdcall` 和 `unmanaged Fastcall`。

如果指定了多个标识符，或者单个标识符不是特殊标识符（例如 `MemberFunction`、`SuppressGCTransition`），则在标识符上加上 `CallConv` 前缀，并在 `System.Runtime.CompilerServices` 查找相应的类型定义（例如 `CallConvMemberFunction`）。这些类型必须来自程序的核心库，有效组合的集合依赖于平台。

#### UnmanagedCallersOnlyAttribute

`UnmanagedCallersOnlyAttribute` 是 CLR 使用的一个特性，用来指示一个方法应该用特定的调用约定来调用。编译器对该特性有以下支持和限制：
- 在 C# 中直接调用带有此特性标记的方法是错误的。用户必须获得一个指向该方法的函数指针，然后调用该指针。
- 将特性应用于普通静态方法或普通静态局部函数以外的任何程序元素都是错误的。C# 编译器会将从带有此特性的元数据中导入的任何非静态或静态非普通方法标记为语言不支持。
- 如果特性标记的方法具有非 `unmanaged` 类型的参数或返回类型，则会产生错误。
- 用特性标记泛型类型的方法是错误的。
- 将标记有该特性的方法转换为委托类型是错误的。
- 不满足在元数据中调用约定模块要求的 `UnmanagedCallersOnly.CallConvs` 的类型都是错误的。

当确定用有效的 `UnmanagedCallersOnly` 特性标记的方法的调用约定时，编译器对 `CallConvs` 属性中指定的类型执行以下检查，以确定应该用于确定调用约定的有效 `CallKind` 和 `modopt`：

- 如果没有指定类型，`CallKind` 将被视为非托管默认调用约定。在函数指针类型的开始处没有调用约定 `modopt`。

  ```csharp
  unsafe class Sample
  {
      public static delegate* unmanaged <int, int> pFun;>
  
      [UnmanagedCallersOnly()]
      public static int Fun(int val) => val;
      static void Main()
      {
          pFun = &Fun;
          Console.WriteLine(pFun(10010)); 
      }
  }
  ```

- 如果指定了一种类型，并且该类型为 `CallConvCdecl`、`CallConvThiscall`、`CallConvStdcall` 或 `CallConvFastcall` 中的一个，那么 `CallKind` 将分别被视为 `unmanaged Cdecl`、`unmanaged Thiscall`、`unmanaged Stdcall` 和 `unmanaged Fastcall`。在函数指针类型的开始处没有调用约定 `modopt`。
  - `CallConvCdecl` 指示调用者清理堆栈。这允许调用带有 `varargs` 的函数。
  - `CallConvThiscall` 指示使用 `ThisCall` 调用约定时，方法调用的第一个参数是 `this` 指针，该指针存储在寄存器 `ECX` 中。方法调用的其他参数将推送到堆栈上。此调用约定用于对从非托管 DLL 导出的类调用方法。
  - `CallConvStdcall` 指示被调用者清除堆栈。这是从托管代码调用非托管函数的默认约定。
  - `CallConvFastcall` 指示使用 `Fastcall` 调用约定时，指定函数的参数在可能的情况下通过寄存器传递。`CallConvFastcall` 调用在当前 `.NET` 不受支持。

  ```csharp
  unsafe class Sample
  {
      public static delegate* unmanaged[Cdecl]<int, int> pFun;
  
      [UnmanagedCallersOnly(CallConvs = new[] {typeof(CallConvCdecl) })]
      public static int Fun(int val) => val;
      static void Main()
      {
          pFun = &Fun;
          Console.WriteLine(pFun(10010));
      }
  }
  ```

- 如果指定了多个类型，或者单个类型没有被命名为上面特别调用的类型之一（例如 `MemberFunction`、`SuppressGCTransition`），那么 `CallKind` 将被视为非托管默认调用约定，指定的类型的联合将被视为函数指针类型开头的 `modopt`。
  - `SuppressGCTransition` 指示方法应禁止 GC 转换作为调用约定的一部分。该方法只能在非托管代码中使用。 
  - `MemberFunction` 指示所使用的调用约定是成员函数变体。

  ```csharp
  unsafe class Sample
  {
      public static delegate* unmanaged[Cdecl, MemberFunction]<int, int> pFun;
  
      [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl), typeof(CallConvMemberFunction) })]
      public static int Fun(int val) => val;
      static void Main()
      {
          pFun = &Fun;
          Console.WriteLine(pFun(10010));
      }
  }
  ```

- 最终，编译器查看这个有效的 `CallKind` 和 `modopt` 集合，并使用正常的元数据规则来确定函数指针类型的最终调用约定。


#### UnmanagedCallConvAttribute 

`UnmanagedCallConvAttribute` 指定 ·NET 调用非托管代码中实现的 P/Invoke 方法（本机函数）所需的调用约定。这些方法的调用约定为 `managed`。

当此特性应用于带有 `DllImportAttribute` 的方法，其中 `CallingConvention` 设置为 `Winapi` 时，.NET 运行时将使用 `UnmanagedCallConvAttribute.CallConvs` 来确定 P/Invoke 的调用约定。如果应用于没有 `DllImportAttribute` 或 `CallingConvention` 设置为 `Winapi` 以外的其他内容的方法，则忽略此特性。

```csharp
unsafe class Sample
{
    public static delegate*<int, int> pFun;
    public static delegate*<int, int> pFun2;

    // Target will be invoked using the stdcall calling convention
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvStdcall) })]
    [DllImport("NativeLibrary", EntryPoint = "native_function_stdcall")]
    // 上述特性组合等效于 [DllImport("NativeLibrary", EntryPoint = "native_function_stdcall", CallingConvention = CallingConvention.StdCall)]
    internal static extern int NativeFunction(int arg);

    // Target will be invoked using the stdcall calling convention and with the GC transition suppressed
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvStdcall), typeof(CallConvSuppressGCTransition) })]
    [DllImport("NativeLibrary", EntryPoint = "native_function_stdcall", CallingConvention = CallingConvention.Winapi)]
    internal static extern int NativeFunction_NoGCTransition(int arg);

    static void Main()
    {
        pFun = &NativeFunction;
        pFun2 = &NativeFunction_NoGCTransition;

        pFun(10010);
        pFun2(10086);
    }
}
```

---
## 表达式树类型

表达式树允许将 Lambda 表达式表示为数据结构，而不是可执行代码。表达式树类型是 `System.Linq.Expressions.LambdaExpression` 或 `System.Linq.Expressions.Expression<TDelegate>`，其中 `TDelegate` 是任意委托类型。

若存在从 `Lambda` 表达式到委托类型 `D` 的转换，则也存在到表达式树类型 `Expression<TDelegate>` 的转换。将 Lambda 表达式转换为委托类型会生成一个引用 Lambda 表达式可执行代码的委托，而将 Lambda 表达式转换为表达式树类型，则构造为一个表达式树。

```csharp
using System.Linq.Expressions;

Func<int, int> Del = x => x + 1;  // Code
Expression<Func<int, int>> Exp = x => x + 1;  // Data
```

将 Lambda 表达式转换为表达式树类型会生成表达式树。更准确地说，Lambda 表达式转换的求值产生一个表示 Lambda 表达式本身结构的对象结构。

并非每个 Lambda 表达式都可以转换为表达式树类型。虽然 Lambda 表达式始终存在到兼容委托类型的转换，但由于特定于实现的原因，它转换表达式树时可能在编译时失败。

>---

### 表达式树的限制

- 不能调用没有实现声明的分部方法、调用已移除的条件方法（`Conditional`）、调用局部函数、调用 `ref` 返回的方法、属性或索引器、调用使用可选参数的方法、调用包含命名参数规范的方法、调用省略 `ref` 的 COM 方法、。
- 不能使用 Lambda 语句、异步 Lambda 表达式、引用 `ref` 返回的 Lambda、使用引用传递（`in`、`out`、`ref`）参数的 Lambda、具有特性的 Lambda、
- 不能使用 `base` 访问、赋值操作、`dynamic` 动态操作、模式匹配、元组字面值和元组操作（相等、不等、转换）、`??=` 空合并运算符、`?.` 空传播运算符、索引和范围运算符、不安全的指针操作、不能包含左侧为 `null` 或 `default` 字面量的 `??` 合并运算符。
- 不能使用 `throw` 表达式、`with` 表达式、`switch` 表达式、匿名方法表达式、多维数组和字典的初始值设定项、不支持扩展 `Add` 的集合初始值设定项。
- 不能使用 `ref struct` 类型、调用访问或声明内联数组、弃元 `_` 声明、
- 无法访问静态抽象或虚拟的接口成员、不能包含模式 `System.Index` 或 `System.Range` 索引器访问、不能包含内插字符串处理程序转换、不能在方法组上使用 `&` 地址运算、不能包含索引属性。
- 表达式树的类型参数不能是非委托类型。

>---

### 执行表达式树

表达式树是表示一些代码的数据结构，它不是经过编译且可执行的代码。若要执行由表达式树表示的 .NET 代码，必须将其转换为可执行的 IL 指令。执行表达式树可能返回一个值，或仅仅是执行操作。

仅可以执行表示 Lambda 表达式的表达式树。若要执行表达式树，调用 `Expression<TDelegate>` 的 `Compile` 方法创建一个 `TDelegate` 类型的委托。`LambdaExpression` 可以表示未知类型的委托，调用 `Compile` 返回一个 `Delegate` 类型。`Delegate` 应使用 `DynamicInvoke` 调用而不是直接调用 `Invoke`。

```csharp
Expression<Action<string>> exp= (string mess) => Console.Write(mess);
Action<string> del = exp.Compile();
del.Invoke("Hello");

LambdaExpression exp2 = (string mess) => Console.Write(mess);
Delegate del2 = exp2.Compile();
del2.DynamicInvoke(" World");
```

如果表达式树不表示 Lambda 表达式，可以通过调用 `Lambda<TDelegate>(Expression, IEnumerable<ParameterExpression>)` 方法创建一个新的 Lambda 表达式。


>---

## 变量

C# 定义了 8 种变量：静态变量、实例变量、数组元素、值参数、输入参数、引用参数、输出参数和局部变量。

```csharp
class Sample
{
    static int x;  // 静态变量 
    private int y; // 实例变量
    void Fun(
        int[] v,   // v[0] 数组元素
        int a,     // 值参数
        in int b,  // 输入参数
        ref int c, // 引用参数
        out int d) // 输出参数
    {
        int i = 10;  // 局部变量
        d = a + c++ + b;
    }
}
```

>---

### 变量类别
#### 静态变量 

静态变量使用 `static` 声明，静态变量在其包含类型的静态构造函数执行之前存在，并且在关联的零一程序域不存在时停止存在。静态变量的初始值是变量类型的默认值。

由于明确赋值的目的，静态变量被视为初始赋值。

#### 实例字段

未声明静态 `static` 修饰符的字段是实例字段。

类的实例字段在类的新实例创建时存在，当没有对该实例的引用且实例的终结器已经执行时，实例变量停止存在。类的实例变量的初始值是变量类型的默认值。由于明确赋值的目的，也被认为是初始赋值。

结构体的实例变量与它所属的结构体变量具有完全相同的生存期，其实例变量的初始赋值状态与包含结构体变量的初始赋值状态相同。当结构体变量被认为是初始赋值时，它的实例变量也是；若是初始未赋值的，它的实例变量也是为赋值的。

#### 数组元素

数组的元素在创建数组实例时开始存在，在没有对数组实例的引用时停止存在。数组中每个元素的初始值是数组元素类型的默认值。
由于明确赋值的目的，数组元素被认为是初始赋值。

#### 值参数

没有 `ref`、`in`、`out` 修饰的形参是值参数。值参数在调用函数成员或匿名函数时产生，并使用调用
中给出的实参的值进行初始化。当函数体执行完成时，值参数通常不再存在。若值参数被一个匿名函数捕获，它的生命周期至少会延长到从该匿名函数创建的委托或表达式树符合垃圾回收的条件。

由于明确赋值的目的，一个值参数被视为初始赋值。

#### 引用参数

用 `ref` 修饰声明的形参是引用参数。引用参数是在调用函数成员、委托、匿名方法或局部函数时产生的引用变量。引用参数不会创建新的存储位置，它与给定调用中的参数变量表示相同的存储位置。当函数体执行完成时，引用参数也不存在，且引用参数不会被捕获。

变量在作为函数成员或委托调用的引用参数传递之前必须明确赋值。由于明确赋值的目的，引用参数被视为初始赋值。

对于结构类型的实例方法或实例访问器中，`this` 的行为与引用参数完全相同。

#### 输出参数

用 `out` 修饰声明的形参是输出参数。输出参数是在调用函数成员、委托、匿名方法或局部函数时产生的引用变量。当函数体执行完成时，输出参数不再存在，且输出参数不会被捕获。

输出参数的赋值规则：
- 在函数成员或委托调用中将变量作为输出参数传递之前，不需要明确赋值。
- 在函数成员或委托调用正常完成之后，作为输出参数传递的每个变量都被认为是在该执行路径中分配的。
- 在函数成员或匿名函数中，输出参数最初被认为是未分配的。
- 函数成员、匿名函数或局部函数的每个输出参数必须在函数正常返回之前明确赋值。

#### 输入参数

用 `in` 修饰声明的参数是输入参数。输入参数是在调用函数成员、委托、匿名函数或局部函数时产生的引用变量，其引用被初始化该调用中作为实参给出的变量引用。当函数体执行完成时，输入参数不再存在，且输入参数不会被捕获。

变量在作为函数成员或委托调用的输入参数传递之前必须明确赋值。出于明确赋值的目的，输入参数被视为初始赋值。

#### 局部变量

局部变量在函数体、语句块中声明出现。局部变量的生命周期是程序执行期间保证为其保存存储空间的部分。此生命周期从进入与其关联的作用域开始扩展，至少到该作用域的执行以某种方式结束为止。若局部变量被匿名函数捕获，那么它的生命周期至少延续到从匿名函数创建的委托或表达式树，以及引用捕获变量的任何其他对象符合垃圾回收条件为止。

每次进入局部变量的作用域时都会实例化它。对于 `foreach` 的迭代变量，每次迭代都会创建一个新的只读变量。局部变量在使用前必须明确赋值。

> 弃元

弃元 `_` 是一个没有名称的局部变量，由声明表达式引入。弃元也可以作为 `out` 参数传递。由于弃元没有被明确赋值，所以访问它的值始终是错误的。但有些声明中 `_` 是一个有效的标识符，此时 `_` 是一个明确赋值的变量存在，在其作用域范围内，弃元无法使用。

>---

### 明确赋值

在函数成员或匿名函数的可执行代码中的给定位置，如果编译器可以通过特定的静态流分析，证明该变量已被自动初始化或已成为至少一次赋值的目标，则称该变量已被明确赋值：
- 初始赋值的变量总是被认为是明确赋值的。
- 初始未赋值的变量，如果在指定位置由所有可能的执行路径中包含以下行为之一时，被认为是在给定位置明确赋值：
  - 简单的赋值操作，其中变量是左操作数。
  - 将变量作为输出参数传递的调用表达式或对象创建表达式。
  - 对于局部变量，为变量做一个局部变量声明，并包含变量初始化式。

在以下上下文中，明确赋值是必需的：
- 变量必须在获取其值的每个位置明确赋值。
- 变量必须在作为引用参数、输入参数传递的每个位置进行明确赋值。
- 函数成员的所有输出参数都必须在函数成员通过语句返回前明确赋值。
- 结构类型的实例构造函数 `this` 变量必须在该实例构造函数返回的每个位置明确赋值，结构变量被认为是明确赋值的前提是该实例变量包含的所有结构类型变量都被视为是明确赋值。

> 初始分配的变量

静态变量、类实例变量、初始分配的结构实例变量、数组元素、值参数、引用参数、输入参数、`catch` 或 `foreach` 子句的变量等。

> 初始未分配的变量

初始未分配的结构实例变量、输出参数、局部变量。

>---

### 隐式变量声明

声明局部变量时，可以让编译器从初始化表达式推断出变量的类型。使用 `var` 关键字隐式声明变量，隐式变量只能应用于局部变量声明。`var` 的常用于接收函数返回、类型推断、模式匹配、匿名类型的声明、隐式变量的声明等。

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

可以使用 `var` 作为 `out` 参数传递。隐式类型输出变量的类型是重载解析选择的方法签名中相应参数的类型。当无法重载决策时（例如发生歧义）需要显式输入变量参数的类型。

```csharp
class Sample
{
    static void Fun(out int num)
    {
        num = 1;
    }
    static void Fun(out string mess)
    {
        mess = "";
    }
    static void Main(string[] args)
    {
        Fun(out var num);  // 隐式声明，歧义
        Fun(out string mess);  // 显式输入类型
    }
}
```

>---

### 变量引用的原子性

`bool`、`char`、`byte`、`sbyte`、`short`、`ushort`、`int`、`uint`、`float` 和引用类型的读取和写入是原子的，具有前面列表中基础类型的枚举类型的读写也是原子的，本机大小的整数 `nint`、`unint` 的读写也是原子的；`long`、`ulong`、`double`、`decimal` 和用户定义类型的读写不能保证为原子性。

>---

### 引用变量和引用返回

引用变量是指对另一个变量的变量。引用变量是 `ref` 修饰的局部变量。引用变量存储对变量的引用，而不是变量的值。

当在需要值的地方使用引用变量时，将返回其引用的值。当引用变量是赋值的目标时，它就是被赋值的引用对象。若要更改引用变量的引用对象时，使用 `= ref` 进行更改关联引用。

```csharp
int num1 = 10;
int num2 = 20;

ref int pnum = ref num1;
pnum = 100;
Console.WriteLine(num1);  // 100
Console.WriteLine(num2);  // 20
pnum = ref num2;
pnum = 10010;
Console.WriteLine(num1);  // 100
Console.WriteLine(num2);  // 10010
```

引用返回是由 `return ref` 方法返回的引用变量。

```csharp
int[] arr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
ref int arr5 = ref Fun();

arr5 = 10086;
Console.WriteLine(arr[5]);  // 10086

ref int Fun()
{
    // element is a reference variable that refers to arr[5]
    ref int element = ref arr[5];
    // return reference to arr[5];
    return ref element; 
}
```

---
## 安全规则规范

在编译时，编译器将表达式允许转义到哪个作用域的概念与每个表达式关联起来，被称为 *safe-to-escape* 安全转义。类似地，对于每个左值 *Lvalue*，编译器把允许对 *Lvalue* 的引用（`ref`）转义到哪个作用域的概念，称之为 *ref-safe-to-escape*。对于给定的左值表达式，它们的引用安全转义范围有所不同。

基本的安全机制执行指出：给定从具有安全转义范围 `S1` 的表达式 `E1` 到具有安全转义范围 `S2` 的左值表达式 `E2` 的赋值过程 `E2 = E1`，如果 `S2` 的作用域范围大于 `S1`，则无法保证安全地通过赋值操作进行 `E1` 到 `E2` 的转义。对于引用转义同样如此。通过构造，两个作用域 `S1` 和 `S2` 处于嵌套关系，因为合法表达式总是可以从封闭该表达式式的作用域安全返回（*safe-to-return*）。

>---

### safe-to-escape & ref-safe-to-escape 

*safe-to-escape* 表示一个范围，它包含一个表达式，值可以安全地转义到该表达式。如果这个范围是整个方法，则从该方法返回的值是安全的。不是 `ref struct` 类型的表达式始终可以从执行方法结束时安全返回。

*ref-safe-to-escape* 表示一个范围，它包含一个左值 *Lvalue* 表达式，对于要转义的 *Lvalue* 的引用 `ref` 值来说，转义到该范围是安全的。如果该作用域是整个方法，则可以安全地从该方法返回对 *Lvalue* 的引用。

编译器通过追踪计算每个值的 *safe-to-escape* 范围和对值的引用 *ref-safe-to-escape* 范围来防止对无效内存位置的访问而导致的意外泄漏。这些安全转义范围定义了一个值可以安全转义的最大范围。若违反了这个安全范围约束，编译器会发出诊断错误。常见的作用域范围有 *current method*、*calling method*、*return only*：
- 具有 *calling method* 安全转义范围的值允许在程序的任何地方转义，可以将该值作为方法返回、或分配给某个右值、或作为参数传递给其他方法。
- 具有 *return only* 安全转义范围的值只能通过 `return` 语句返回、或赋值给 `out` 参数从当前方法进行转义。
- 具有 *current method* 安全转义范围的值不允许从当前方法的堆栈外转义。   

对于大多数的常规值，其安全转义范围为 *calling method*，它几乎允许被返回一个值给调用方（包含 `out` 传递），或将该值分配给某个字段。某些类型的值仅在当前执行方法的堆栈中有效（`Span<T>` 等 `ref struct` 类型，`stackalloc` 表达式，`scoped` 变量或参数等）。

对于赋值操作 `x = y`，编译器会检查左值的 *safe-to-escape* 是否小于等于右值的安全转义范围；对于 `= ref` 赋值，编译器将检查左值的 *ref-safe-context* 是否小于等于右值的引用安全转义范围；对于方法返回 `return expr`，`expr` 至少为 *return only* 的安全转义范围；对于 `return ref expr`，`expr`至少为 *return only* 的引用转义范围。

> safe-to-escape

```csharp
ref struct Sample
{
    int Value { get; set; }
    Span<int> Span { get; set; }

    // safe-to-escape of 'x' is "calling method" hence the 'x' is allowed to 'escape' the current method:
    static int CallerContext_Example(Sample c)
    {
        var x = 123;
        c.Value = x; 
        return x;
    }

    // safe-to-escape of 'x' is "current method" hence the 'x' is not allowed to 'escape' the current method:
    static Span<int> FunctionMember_Example(Sample s)
    {
        Span<int> x = stackalloc[] { 1, 2, 3 }; // implicit scoped
        s.Span = x; // ERR
        return x;  // ERR
    }
}
```

> ref-safe-to-escape

```csharp
ref struct Sample
{
    ref int refValue;
    Span<int> Span { get; set; }

    // References passed in as fields on ref-struct parameters have ref-safe-to-escape of "calling method".
    // This makes sense because clearly the references ALREADY exist outside this method, they were set when they were passed in.
    static ref int CallingMethodExample(Sample s1, Sample s2)
    {
        s2.refValue = ref s1.refValue;  // 'ref s1.Value' has a ref-safe-to-escape of "calling method", so this is permitted
        return ref s1.refValue;  // 'ref s1.Value' has a ref-safe-to-escape of "calling method", so this is permitted
    }

    // References passed in directly to methods as parameters have ref-safe-to-escape of "return only".
    // "return only" lies somewhere inbetween "current method" and "calling method" -- the ref is allowed
    // to escape the current method but ONLY via return statement.
    static ref int ReturnOnlyExample(ref int x, Sample s)
    {
        s.refValue = ref x; // ERR: 'ref x' has a ref-safe-to-escape of "return only", so this does not compile
        return ref x; // 'ref x' has a ref-safe-to-escape of "return only", so this is permitted
    }

    // References to stack variables will always have a ref-safe-to-escape scope of "current method"
    // (meaning those references can exist only within the current method but may not escape any further).
    static ref int CurrentMethodExample(Sample s, scoped ref int rv)
    {
        var x = 123;
        s.refValue = ref x;

        return ref x; // ERR: 'ref x' has a ref-safe-to-escape of "current method",
                      // which means the reference is NOT allowed to escape OUTSIDE the current method, so this does not compile
    }
}
```

*Lvalue* 表达式的 *ref-safe-to-escape* 范围永远不能大于相同值的 *safe-to-escape* 范围。这意味着当规范限制值的 *safe-to-escape* 时，它也隐式地限制了*ref-safe-to-escape*。但 *ref-safe-to-escape* 的范围可以小于 *safe-to-escape*。例如一个非 `ref` 局部变量可以 *safe-to-escape* 到方法外部，但只能 *ref-safe-to-escape* 到方法内部。

>---
### safe-context & ref-safe-context

> ECMA 对先前 C# 规范中 *safe-to-escape* 更名为 *safe-context*，*ref-safe-to-escape* 更名为 *ref-safe-context*。一个值的安全上下文可分为 *declaration-block* 声明块、*function-member* 函数域、*return-only* 仅返回、*caller-context* 调用方，自声明块（最窄）到调用方（最宽）。函数成员（方法、属性、索引器、构造函数、实例构造函数、运算符等）返回的安全上下文应至少为 *return-only*。
> 
> *calling method* 等效于 *caller-context*，*safe-to-return* 等效于 *caller-context*，*current method* 等效于 *function-member*。

在安全的上下文中，每个值的 *safe-context* 记录了该值可以安全转义的作用域，每个值的引用 *ref-safe-context* 记录了该值的引用可以安全转义的作用域。编译器通过对程序文本的静态分析来确定值的 *safe-context*（反映了变量在运行时的生存期）和值引用的 *ref-safe-context*（反映了变量引用在运行时的生存期）。有以下类别的安全上下文：
- 具有 *caller-context* 的值（引用）可以在程序中的任何地方转义，例如作为返回值 `return`（`return ref`）、分配给某个字段或作为实参传递给其他方法（`= ref`）等。类型不是 `ref struct` 的表达式总是具有 *caller-context* 的 *safe-context*。 
  
- 具有 *return-only* 的变量，可以通过方法返回或通过 `out` 参数传递到调用方。

- 具有 *function-member* 的变量不允许从当前执行方法转义到调用方。

- *declaration-block* 指的是在块中声明的局部变量的作用域，以及该作用域中的任何嵌套嵌入语句。局部变量（未被捕获而被提升）的生存期在声明块结束处终止。对局部变量的引用的 *ref-safe-context* 是该局部变量的作用域。只有当引用变量在该变量的 *ref-safe-context* 中声明时，对局部变量的引用才是引用变量的有效引用。

对于任何变量，该变量的 *ref-safe-context* 是对该变量引用的有效上下文。所有引用变量都遵循安全规则，以确保引用变量本身的 *ref-safe-context* 不大于其变量值的 *ref-safe-context*。一个值的 *ref-safe-context* 永远不会大于其 *safe-context*。

#### return-only

C#11 规范设计中要求引入一个新的安全上下文：*return-only* 仅返回。它类似于 *caller-context*，因为它可以通过返回转义，但它只能通过 `return` 语句返回。

*return-only* 设计的一个细节是，它是一个比 *function-member* 宽，但比 *caller-context* 窄的上下文类别。`return` 语句的表达式必须至少是 *return-only*。因此，大多数现有的规则都会失效。例如，从具有 *return-only* 的 *safe-context* 表达式赋值到 `ref` 参数将失败，因为它比 `ref` 形参的 *safe-context*（*caller-context*）窄。

有三个位置的参数默认为 *return-only*：
- `ref`、`in` 参数具有一个 *return-only* 的 *ref-safe-context*。

  ```csharp
  ref readonly int Fun1(ref int v1, in int v2) // ref-safe-context is return-only
  {
      Span<int> rS1;   // ref struct 在未初始化的声明点的 safe-context 是 caller-context
      ReadOnlySpan<int> roS1; // ref struct 在未初始化的声明点的 safe-context 是 caller-context
  
      rS1 = new Span<int>(ref v1);          // err: return-only to caller-context
      roS1 = new ReadOnlySpan<int>(in v2);  // err: return-only to caller-context
  
      Span<int> rS2 = new Span<int>(ref v1);   // okay: rS2 is return-only
      ReadOnlySpan<int> roS2 = new ReadOnlySpan<int>(in v2);  // okay: rS2 is return-only
  
      return ref v1;  // okay
      return ref v2;  // okay
  }
  ```

- `ref struct` 类型的 `out` 形参将具有 *return-only* 的 *safe-context*。这允许 `return` 和 `out` 具有相同的效益。由于 `out` 是 `scoped` 作用域，所以 *ref-safe-context* 仍然比 *safe-context* 窄。

  ```csharp
  Span<int> Fun2(out Span<int> S, out Span<int> S2, ref Span<int> S3 /* safe-context is caller-context */)
  {
      S = default;
  
      S2 = S;  // okay // safe-context: return-only to return-only
  
      S3 = S;  // err  // safe-context: return-only to caller-context
  
      Span<int> rS;  // ref struct 在未初始化的声明点的 safe-context 是 caller-context
      rS = S;  // err  // safe-context: return-only to caller-context
  
      Span<int> rS2 = S;  // ref struct 在声明时具有和初始化项相同的 safe-context: return-only
      rS = rS2; // err
  
      return S;  // return-only
      return rS2;  // return-only
  }
  ```

- 结构实例构造函数中的 `this` 具有 *return-only* 的 *safe-context*。`this` 在结构的实例构造函数中被视为 `out` 参数（在结构实例方法中则被视为 `scoped ref` 形参）。

  ```csharp
  ref struct Sample
  {
      public Sample(out Sample outS /* safe-context: caller-context*/, 
                    ref Sample refS /* safe-context: caller-context*/)
      {
          refS = this; // err: The safe-context of this is return-only
          
          outS = this; // okay
      }
  }
  ```

<br>

任何赋值给 `out` 参数的表达式必需至少具有 *return-only* 的 *safe-context*。类型不是 `ref struct` 类型的表达式总是具有一个 *caller-context* 的 *safe-context*。

```csharp
void Fun(out int oV /* safe-context is caller-context */, 
        out Span<int> oS /* safe-context is return-only */)
{
    oV = 10010; // expr is caller-context 
    oS = stackalloc int[10]; // err: function-member < return-only
    oS = default; // okay: safe-context of default is caller-context
}
```

#### scoped 作用域修饰符

关键字 `scoped` 将用于限制变量值的生存期。它可以作用于引用传递参数、`ref` 局部变量、`ref struct` 的参数或局部变量，并且具有将 *ref-safe-context* 或 *safe-context* 的生存期限制为 *function-member* 当前方法。例如：

| Parameter or Local       | ref-safe-context  | safe-context      |
| ------------------------ | ----------------- | ----------------- |
| `Span<int> s`            | *function-member* | *caller-context*  |
| `scoped Span<int> s`     | *function-member* | *function-member* |
| `ref Span<int> s`        | *caller-context*  | *caller-context*  |
| `scoped ref Span<int> s` | *function-member* | *caller-context*  |

在这种关系中，一个值的 *ref-safe-context* 永远不会比它的 *safe-context* 更宽。函数成员的 `out` 参数或结构的实例方法中的 `this` 被视为隐式声明的 `scoped ref`。   

```csharp
class Sample
{
    // ref-safe-context of 'ref parameter' is caller-context
    //Span<int> CreateSpan(ref int parameter) => default;

    // ref-safe-context of 'scoped ref parameter' is function-member
    Span<int> CreateSpan(scoped ref int parameter) => default;
    // the implementation of this method isn't relevant to callers.
    
    Span<int> BadUseExamples(int parameter)
    // ref-safe-context of parameter is function-member
    {
        // Legal in C# 10 and legal in C# 11 due to scoped ref
        return CreateSpan(ref parameter);

        // Legal in C# 10 and legal in C# 11 due to scoped ref
        int local = 42; // 
        return CreateSpan(ref local);

        // Legal in C# 10 and legal in C# 11 due to scoped ref
        Span<int> span = stackalloc int[42];
        return CreateSpan(ref span[0]);
    }
}
```

扩展方法中 `this` 修饰的 `ref` 结构类型参数可以声明为 `scoped ref T`。

```csharp
static class Ext
{
    //  ref-safe-context of 'this scoped ref s' is function-member.
    public static ref Sample Fun(this scoped ref Sample s) => ref s; // err

    // ref-safe-context of 'this ref s' is return-only.
    public static ref Sample Fun2(this ref Sample s) => ref s;
}
```

`scoped` 将 `ref struct` 局部变量的 *safe-context* 或 `ref` 局部变量的 *ref-safe-context* 限制为 *function-member*，而忽略初始化项的生存期。

```csharp
Span<int> ScopedLocalExamples()
{
    // Okay: default 表达式的 safe-context 是 *caller-context*，因此 span1 具有相同的安全上下文
    Span<int> span1 = default;
    return span1;

    // Error: `span2` 是 *function-member*. 即使 default 表达式是 *caller-context*
    // span2 的安全上下文被 scoped 声明限制为 *function-member*
    scoped Span<int> span2 = default;
    return span2;  // function-member < caller-context

    // stackalloc 表达式的 safe-context 是 *function-member*, span3 的 safe-context 是 *function-member*
    // 因此 `span3` 与 `span4` 在声明上是等效的
    Span<int> span3 = stackalloc int[42];
    scoped Span<int> span4 = stackalloc int[42];
    return span3;  // function-member < caller-context
}
```

`scoped` 作用域修饰符不能用于修饰返回值、字段、数组元素等。`scoped` 可以影响任意的 `ref`、`in` 参数、`ref` 局部变量、类型是 `ref struct` 的参数或局部变量。非 `ref struct` 的值总是可以安全返回的。`out` 参数被视为隐式 `scoped ref`。

```csharp
interface ISample
{
    void Fun1(scoped in int v1, scoped out int v2);
    void Fun2(scoped ref int v1, scoped Span<int> s);
    void Fun3(scoped int v); // Error: CS9048
}
```

方法的 `out` 参数、结构类型实例方法的 `this` 变量（包含其他实例成员）被隐式的声明为 `scoped`，因此它们不能通过 `return ref` 转义。

```csharp
using System.Diagnostics.CodeAnalysis;
struct Sample
{
    ref int Fun(out int i) // 'out int i' 相等于 'scoped out int is'
    {
        i = 10010;
        return ref i;  // Error, function-member
    }

    ref Sample refThis() => ref this; // Error, function-member
}
```

> ScopedRef 特性

包含 `scoped` 注释的参数将通过特性 `ScopedRefAttribute` 发送到元数据中。该特性由编译器使用并生成，在编译单元中无法使用。编译器将对带 `scoped` 语法的参数发出此属性。只有当语法导致值与其默认状态不同时，才会触发此行为。例如，`scoped out` 将不发出任何特性。

#### UnscopedRef 特性

可以为结构实例方法、结构实例属性（不能是 `init` 属性）、引用传递的参数标记 `UnscopedRefAttribute` 特性，以提供与 `scoped` 注释相反的注释，这可以应用于任何 `ref`，并将标记对象的 *ref-safe-context* 更改为比默认值更宽的一级。例如：
- 如果应用于结构的实例方法或属性，`[UnscopedRef]` 将修改隐式 `scoped ref this` 参数的 *ref-safe-context* 为 *return-only*。
  
  ```csharp
  ref struct RSample
  {
      int value;
      [UnscopedRef] ref int Value => ref this.value;
      [UnscopedRef] ref RSample This => ref this;
      public void Fun(ref RSample s)
      {
          s = ref this; // err; ref this is function-member
      }
  }
  ```

- 如果应用于 `ref`、`in` 参数，它将修改参数的 *ref-safe-context* 为 *caller-context*。默认是 *return-only*。

  ```csharp
  ref struct RSample
  {
      public RSample(ref int v1, in int v2) { }
      public static RSample Create(ref int v, in int v2)
      {
          return new RSample(ref v, in v2);  // return-only
      }
      public static ref RSample UnscopedRefCreate(ref RSample rs, [UnscopedRef] ref int v, [UnscopedRef] in int v2)
      {
          rs = Create(ref v, in v2); // UnscopedRef
          return ref rs;  // return-only
      }
  }
  ```

- 如果应用于 `out` 参数，它将修改参数的 *ref-safe-context* 为 *return-only*。默认是隐式的 `scoped`（*function-member*）。

  ```csharp
  ref struct RSample
  {
      public ref int Fun(out int v)
      {
          v = 10010;
          return ref v;  // err: function-member
      }
      public ref int Fun1([UnscopedRef] out int v)  // return-only
      {
          v = 10010;
          return ref v;
      }
  }
  ```


`UnscopedRef` 特性不能用于：
- 非结构体上声明的成员。
- 结构体的静态成员、`init` 属性、构造函数。
- 标记为 `scoped` 的参数、值参数。
- 没有隐式作用域（即 *caller-context*）的引用传递参数。

>---
### 变量与方法返回的安全分配原则
#### 赋值操作与方法返回

对于变量的分配操作 `e1 = e2`，`e2` 的 *safe-context* 范围至少要和 `e1` 的 *safe-context* 范围一样大。

```csharp
class Sample
{
    void Fun(out Span<int> v)  // return-only
    {   
        Span<int> span = stackalloc int[10];  // function-member
        v = span;  // err, scoped : v > span
    }
}
```

对于方法返回 `return e`，`e` 的 `safe-context` 至少为 *return-only*。
  
```csharp
class Sample
{
    Span<int> Fun()  // return : return-only at least 
    {
        Span<int> span = stackalloc int[10];  // function-member
        return span; // err, scoped: return-only > span
    }
}
```

#### ref 引用重赋值和 ref 方法返回

`= ref` 操作的左操作数必须是绑定到一个局部变量、一个 `ref` 参数（非 `this`），一个 `out` 参数或一个 `ref` 字段。对于形式为 `e1 = ref e2` 引用重赋值，必须满足：
- `e2` 的 *ref-safe-context* 至少与 `e1` 的 *ref-safe-context* 相同，即 `ref e1 <= ref e2`。
- `e1` 必须具有与 `e2` 相同的 *safe-context*。

```csharp
/**
*  caller-context    $cm
*  return-only       $ro
*  function-member   $local
*  $cm > $ro > $local
*  RC : ref-safe-context
*  SC : safe-context
*  [UnscopedRef] : unscoped
*/
void Test(                                          // safe-context      ref-safe-context
    ref int v,                                      //  $cm                $ro
    ref Span<int> s,                                //  $cm                $ro
    out Span<int> os)                               //  $ro                $local
{
    os = default;
    Span<int> ls0 = s;                              //  $cm                $local
    Span<int> ls1 = default;                        //  $cm                $local
    Span<int> ls2 = new Span<int>(reference: ref v);//  $ro                $local
    Span<int> ls3 = stackalloc int[1];              //  $local             $local 

    s = ref ls1;  // err;  SC 相同, 但是 RC(s:$ro) > RC(ls1:$local)
    s = ref ls2;  // err;  RC(s:$ro) > RC(ls2:$local)

    s = ls1;      // okay; SC both $cm
    s = ls2;      // err;  SC(e1:$cm) > SC(e2:$ro)

    ref var rls = ref ls1;   // rls : SC = $cm, RC = $local
    rls = s;      // okay; both $cm
    rls = ls2;    // err;  $cm > $ro
    rls = ref s;  // okay; RC: $local < $ro, SC: both $cm
    rls = ref ls3;// err;  RC: both $local, SC: $cm != $local

    ref var rls2 = ref ls2;  // rls2: safe-context = $ro, ref-safe-context = $local
    rls2 = s;     // okay;
    rls2 = ls3;   // err; SC: $ro > $local
    rls2 = ref s;   // err; RC: $local < $ro, SC: $ro != $cm
    rls2 = ref os;  // okay; RC: $local = $local, SC: both $ro
}
```

对于方法返回 `return ref e`，`e` 的 `ref-safe-context` 至少为 *return-only*。结构类型实例方法中的 `this` 及其字段的安全上下文是 *function-member*，若要通过 `return ref` 方式返回值的引用，需要将实例方法标记为 `[UnscopedRef]`。

```csharp
struct Sample
{
    int value;
    ref int Fun(ref int v1, out int v2)
    {
        // v1 ref scoped: return-only
        // v2 ref scoped: function-member
        // this ref scoped: function-member
        v2 = 0;
        return ref v2;  // err
        return ref value;  // err
        return ref v1;  // okay
    }

    [UnscopedRef] ref int RefValue() => ref value;
    [UnscopedRef] ref Sample RefThis() => ref this;
}
```

>---
### 泛型生命周期模型

生命周期最自然的表达方式是使用类型。在给定的程序的生命周期内对生命周期类型（*lifetime type*）的检查安全的。通常不会直接讨论生命周期类型，有一些类别的生命周期随特定的实例化点的不同而变化，我们将这些变化的生命周期称为泛型生命周期 *generic lifetimes*，用泛型参数表示，C# 不提供表示生命周期泛化的语法，因此定义一个从 C# 到包含显式泛型参数扩展的低级语言的隐式转换。

例如使用语法 `$a` 引用了一个名为 `a` 的生命周期，它本身是一个没有意义的生命周期，但可以通过 `where $a : $b` 语法与其他生命周期建立关系，表明 `$a` 的生命周期可以转换为 `$b`，也就是说 `$a` 的生命周期至少和 `$b` 一样长。有以下一些预定义的生命周期：
- `$heap`：堆上存在的任何值的生命周期，可用于任何上下文和方法签名。
- `$local`：方法堆栈中存在的任何值的生命周期，它实际上是 *function-member* 的名称占位。它可以隐式地定义在方法中，可以出现在方法签名中，除了任何的输出位置（前身是 *current method*）。
- `$ro`：用于 *return-only* 的名称位置占位（前身是 *return only*）。
- `$cm`：用于 *caller-context* 的名称位置占位（前身是 *calling method*）。

生命周期之间有一些预定义的关系：
- `where $heap : $a` 表示所有类别的生命周期 `$a`。
- `where $cm : $ro`。
- `where $x : $local` 表示所有预定义的生命周期，除非显式定义，否则用户定义的生命周期与 `$local` 没有关系。

在类型上定义的生命周期可以是不变的（*invariant*），也可以是协变的（*covariant*）。它们使用与泛型参数相同的语法表示：

```csharp
// $this is covariant
// $a is invariant
ref struct Sample<out $this, $a> 
```

类型定义上的 `$this` 不是预定义的，它在定义时存在一些与之相关的规则：
- 它必须是第一个生命周期参数。
- 它一定是协变的：`out $this`。
- `ref` 字段的生命周期必须可转换为 `$this`。
- 所有非 `ref` 字段的 `$this` 必须是 `$heap` 或 `$this`。

一个引用的生命周期由提供引用的生命周期来提供。一个引用堆的 `ref` 表示为 `ref<$heap>`。

在模型中定义构造函数时，将为方法使用 `new` 这个名称。必须为包含一个参数列表的构造值提供构造函数实参，这对于表达构造函数输入与构造值之间的关系是紧密联系的。模型将使用 `Span<$a> new<$ro>` 而不是 `Span<$a><$ro>`，构造函数中的 `this` 的类型（包括生命周期）将是定义的返回值。

#### 生命周期模型的规则定义

生命周期的基本规则定义为：
- 所有的生命周期在语法上都表示为泛型参数，位于类型参数之前。除了 `$heap` 和 `$local` 之外，对于预定义的生命周期均是如此。
- 所有不是 `ref struct` 类型的类型 `T` 隐式具有 `T<$heap>` 生命周期。
- 对于定义为 `ref<$l0> T<$l1, $l2, ..., $ln>` 的 `ref` 字段：
  - 从 `$l1` 到 `$ln` 的生命周期类型必须是不变的；
  - `$l0` 必须可转换为 `$this`。
- 对于定义为 `ref<$a> T<$b, ...>` 的引用，`$b` 必须可以转换为 `$a`。
- `ref` 变量的生命周期由：
  - 对于 `ref` 的局部变量、参数或 `ref<$a> T` 的返回类型，它们的生命周期是 `$a`。
  - `$heap` 用于所有的引用类型或引用类型的字段。
  - `$local` 用于其他的所有类型。
- 当底层类型的转换合法时，赋值或返回也是合法的。
- 表达式的生命周期可以通过强制转换来显式设置：
  - `(T<$a> expr)` 的生命周期显式定义为 `T<...>` 的 `$a`。
  - `ref<$a> (T<$b>)expr` 的生命周期是 `T<...>` 的 `$b`，它的引用生命周期为 `$a`。

对于声明周期而言，`ref` 被认为是表达式的一部分，以便进行转换，它在逻辑上表示为将 `ref<$a> T<...>` 转换为 `ref<$a, T<...>>`，其中 `$a` 是协变的，`T` 是不变的。

#### C# 语法映射到底层模型

没有显式生命周期参数的类型被视为定义了 `out $this`，并应用于该类型的所有字段。包含 `ref` 字段的类型必须定义显式的生命周期参数。这些规则支持了现有的不变量，即对于所有类型，`T` 可以赋值给 `scoped T` 的变量，映射到 `T<$a, ...>` 赋值给 `T<$local, ...>` 并在已知的所有生命周期内转换为 `$local`。

对于 `S<out $this, ...>`类型中的实例方法中的 `this` 被隐式定义为：
- 一般实例方法：`ref<$local> S<$cm, ...>`。
- `[UnscopedRef]` 标记的实例方法：`ref<$ro> S<$cm, ...>`。

由于实例方法缺少显式 `this` 参数因此使用强制隐式定义，实例中考虑将编写为静态方法并将 `this` 作为显式参数。

```csharp
ref struct S<out $this>
{
    // Implicit this can make discussion confusing 
    void M<$ro, $cm>(ref<$ro> S<$cm> s) {  }

    // Rewrite as explicit this to simplify discussion
    static void M<$ro, $cm>(ref<$local> S<$cm> this, ref<$ro> S<$cm> s) { }
}
```

C# 语法中以以下方式映射到模型：
- `ref` 参数的引用生命周期为 `$ro`。
- `ref struct` 类型的参数具有 `this` 相同的生命周期 `$cm`。
- `ref return` 返回的引用生命周期为 `$ro`。
- `ref struct` 类型的返回类型具有值生命周期 `$ro`。
- `scoped` 注释的参数或一个 `ref` 将更改引用生命周期为 `$local`。

> `ref` 映射

```csharp
ref int M1(ref int i) => ...

// Maps to the following. 
ref<$ro> int Identity<$ro>(ref<$ro> int i)
{
    // okay: has ref lifetime $ro which is equal to $ro
    return ref i;

    // okay: has ref lifetime $heap which convertible $ro
    int[] array = new int[42];
    return ref array[0];

    // error: has ref lifetime $local which has no conversion to $a hence 
    // it's illegal
    int local = 42;
    return ref local;
}
```

> `ref struct` 映射

```csharp
ref struct S
{
    ref int Field;
    S(ref int f)
    {
        Field = ref f;
    }
}
S M2(ref int i, S span1, scoped S span2) => ...

// Maps to 

ref struct S<out $this>
{
    // Implicitly 
    ref<$this> int Field;
    S<$ro> new<$ro>(ref<$ro> int f)
    {
        Field = ref f;
    }
}
S<$ro> M2<$ro>(ref<$ro> int i, S<$ro> span1, S<$local> span2)
{
    // okay: types match exactly
    return span1;

    // error: has lifetime $local which has no conversion to $ro
    return span2;

    // okay: type S<$heap> has a conversion to S<$ro> because $heap has a
    // conversion to $ro and the first lifetime parameter of S<> is covariant
    return default(S<$heap>)

    // okay: the ref lifetime of ref $i is $ro so this is just an identity conversion
    S<$ro> local = new S<$ro>(ref $i);
    return local;

    int[] array = new int[42];
    // okay: S<$heap> is convertible to S<$ro>
    return new S<$heap>(ref<$heap> array[0]);

    // okay: the parameter of the ctor is $ro ref int and the argument is $heap ref int. These 
    // are convertible.
    return new S<$ro>(ref<$heap> array[0]);

    // error: has ref lifetime $local which has no conversion to $a hence 
    // it's illegal
    int local = 42;
    return ref local;
}
```

> 循环自分配问题

```csharp
ref struct S
{
    int field;
    ref int refField;

    static void SelfAssign(ref S s)
    {
        s.refField = ref s.field;
    }
}

// Maps to 

ref struct S<out $this>
{
    int field;
    ref<$this> int refField;

    static void SelfAssign<$ro, $cm>(ref<$ro> S<$cm> s)
    {
        // error: the types work out here to ref<$cm> int = ref<$ro> int and that is 
        // illegal as $ro has no conversion to $cm (the relationship is the other direction)
        s.refField = ref<$ro> s.field;
    }
}
```

> 参数捕获问题 

```csharp
ref struct S
{
    ref int refField;

    void Use(ref int parameter)
    {
        // error: this needs to be an error else every call to this.Use(ref local) would fail 
        // because compiler would assume the `ref` was captured by ref.
        this.refField = ref param;
    }
}

// Maps to 

ref struct S<out $this>
{
    ref<$this> int refField;
    
    // Using static form of this method signature so the type of this is explicit. 
    static void Use<$ro, $cm>(ref<$local> S<$cm> @this, ref<$ro> int param)
    {
        // error: the types here are:
        //  - refField is ref<$cm> int
        //  - ref param is ref<$ro> int
        // That means the RHS is not convertible to the LHS ($ro is not covertible to $cm) and 
        // hence this reassignment is illegal
        @this.refField = ref<$ro> param;
    }
}
```



>---

### 方法参数匹配原则

从 C#11 开始可以在 `ref struct` 声明 `ref` 字段，变量的值可以通过 `ref struct` 类型参数的引用或返回进行转义。其中有一个 `out` 或 `ref` 参数是 `ref struct` 类型，那么所有的 ***ref-likes*** 参数需要具有相同的生命周期。例如，当 `ref` 形参是 `ref struct` 类型时，它们有可能会发生数据交换，因此在调用点必须确保所有潜在的交换都是兼容的。若语言没有强制要求参数匹配，则可能会发生：

```csharp
void Fun(ref Span<int> s1)
{
    Span<int> s2 = stackalloc int[1];
    Swap(ref s1, ref s2);   //err; 这会导致将 stackalloc 分配给 s1, 并被允许转义到 Fun 的调用方
}
void Swap(ref Span<int> s1, ref Span<int> s2)
{
    s1 = ref s2;
}
```

对 `ref` 的参数（`ref struct` 类型）的分析包括实例方法中的接收方，由于它可以用来存储作为参数传入的值，像 `ref` 参数一样。

```csharp
void Broken(ref S s)
{
    Span<int> span = stackalloc int[1];

    // The result of a stackalloc is stored in s.Span
    // and escaped to the caller of Broken
    s.Set(span);  // err; s 作为接收方可能会存储 span
}
ref struct S
{
    public Span<int> Span;
    public void Set(Span<int> span)
    {
        Span = span;
    }
}
```

如果接收方是 `readonly ref struct` 结构体，则将其参数视为 `in`，而不是 `ref`。在这种情况下，接收方 `s` 不能用于存储来自其他参数的值。因此 `s` 为 `readonly` 时，`s.Set(span)` 是合法的，因为 `span` 不会被存储到 `s` 的任何地方。

```csharp
void Broken(ref S s)
{
    Span<int> span = stackalloc int[1];
    s.Set(span);  // okay; 接收方不能存储 span
}
readonly ref struct S
{
    public readonly Span<int> Span;
    public void Set(Span<int> span)
    {
        //Span = span;
    }
}
```

在计算调用方法返回值的 *safe-context* 或 *ref-safe-context* 时，由于 `scoped` 或 `[UnscopedRef]` 注释的参数 `p` 的影响（包括 `ref struct` 中隐式传递的 `this`），对于传递给形参 `p` 的给定实参 `expr`：
- 若 `p` 是 `scoped ref`、`scoped in` 参数，在计算返回值的 *ref-safe-context* 时不考虑 `expr` 提供的 *ref-safe-context*。
- 若 `p` 是 `scoped` 参数，在计算返回值的 *safe-context* 时不考虑 `expr` 提供的 *safe-context*。
- 若 `p` 是 `out` 参数，在计算返回值的上下文时不考虑 `expr` 提供的 *safe-context* 或 *ref-safe-context*。
- 若 `p` 是 `[UnscopedRef]` 注释 `out` 参数，在计算返回值的 *safe-context* 时不考虑 `expr` 提供的 *safe-context*。
- 对于结构类型的 `[UnscopedRef]` 实例方法，`this` 视为 `ref` 参数，其他任意类型的实例方法中，`this` 视为 `scoped ref` 参数。

**方法参数必须匹配** 可以解释为：在调用方法时，所有非 `scoped` 的 `ref struct` 类型的参数的 *safe-context*、`ref` 参数的 *ref-safe-context*、`[UnscopedRef] out` 参数的 *ref-safe-context* 必须大于或等于所有 `ref ref-struct` 参数的 *safe-context*。

对于任何方法调用 `e.M(p1,p2,...)`
- 返回值的 *safe-context* `E` 是以下上下文中最窄的：
  (1) *caller-context*。
  (2) 所有参数的 *safe-context*。
  (3) 所有非 `scoped` 的引用（`ref`、`in`、`[UnscopedRef] out`）参数的实参表达式的 *ref-safe-context*。
- 所有类型是 `ref struct` 的 `out` 参数必须由处于该 *safe-context* 范围的值赋值。

上述规则不包含 `ref struct` 结构的 `this`，由于在 `ref struct` 的实例方法中总会涉及一个 `ref struct` 类型的 `this`，因此调用 `ref struct` 的实例方法总是要考虑 “方法参数必须匹配” 的原则。计算 `ref struct` 实例方法返回值的 *safe-context* 时：
- `scoped ref this` 不贡献 *ref-safe-context*，贡献 *safe-context*。
- `[UnscopedRef] ref this` 贡献 *ref-safe-context*，

如果不匹配的签名使用 C#11 的 *ref-safe-context* 规则，则将不匹配问题报告为错误，否则为警告。

编译器在以下情况下分析并报告使用方法时（包括重写方法、接口实现、委托转换）的不安全的参数组合：
- 方法返回一个 `ref struct` 类型或 `ref/ref readonly` 返回时，并且该方法至少有一个额外的 `ref`、`in`、`out` 参数，或一个 `ref struct` 的参数
- 或者该方法包含一个 `ref struct` 类型的 `ref` 或 `out` 参数，并且该方法至少有一个额外的 `ref`、`in`、`out` 参数（`ref struct` 实例方法中的 `this` 被视为 `ref` 传递），或一个 `ref struct` 的参数。

> *ref struct* 中的方法匹配分析

```csharp

using System.Diagnostics.CodeAnalysis;

ref struct RSample
{
    /**
    *  caller-context    $cm
    *  return-only       $ro
    *  function-member   $local
    *  $cm > $ro > $local
    *  RC : ref-safe-context
    *  SC : safe-context
    *  [UnscopedRef] : unscoped
    */
    // 忽略以下方法的实现
    Span<int> Fun1(ref Span<int> S1) => throw new Exception();
    Span<int> Fun2(scoped ref Span<int> S1, out Span<int> S2) => throw new Exception();
    Span<int> Fun3(scoped ref Span<int> S1, [UnscopedRef] out Span<int> S2) => throw new Exception();
    [UnscopedRef] Span<int> Fun4(ref Span<int> S1, out Span<int> S2) => throw new Exception();
    Span<int> Fun5(Span<int> S1, out Span<int> S2) => throw new Exception();
    Span<int> Fun6(Span<int> S1, ref Span<int> S2) => throw new Exception();
    Span<int> Fun7(ref Span<int> S1, [UnscopedRef] out Span<int> S2) => throw new Exception();
    Span<int> Fun8(scoped Span<int> S1, out Span<int> S2) => throw new Exception();

    Span<int> span;  // SC: SC(this), RC: SC(this)
    ref int RValue;  // SC: $cm, RC: RC(this)
    int value;  // SC: $cm, RC: SC(this)

    Span<int> Test(
           // scoped ref this.span           // SC: $cm   RC: $local
           // unscoped ref this.span         // SC: $cm   RC: $ro
           ref Span<int> s1,                 // SC: $cm   RC: $ro
           out Span<int> s2,                 // SC: $ro   RC: $local
           [UnscopedRef] out Span<int> s3,   // SC: $ro   RC: $ro
           scoped Span<int> s4,              // SC: $local RC: $local
           scoped ref Span<int> s5)          // SC: $cm,  RC: $local
    {

        s2 = default(Span<int>);
        s3 = default(Span<int>);
        Span<int> ls1 = default(Span<int>);    // SC: $cm, RC: $local 
        Span<int> ls2 = new Span<int>(ref value);  // SC: $local, RC: $local
        Span<int> ls3 = new Span<int>(ref RValue); // SC: $cm, RC: $local
        Span<int> ls4 = stackalloc int[1];     // SC: $local, RC: $local

        // Fun1 分析：参数列表为 `scoped ref this`,`ref S1`
        //      this: SC = $cm, 不贡献 RC
        //      S1: 要求 SC 是 $cm
        //      return: SC 是 RC(S1)
        // 其中 $cm 参数包含: this.span,s1,s5,ls1,ls3
        Fun1(S1: ref this.span);  // SC(return) = RC(this.span) = $local
        return Fun1(S1: ref s1);  // SC = RC(s1) = $ro
        Fun1(S1: ref s5);   // $local
        Fun1(S1: ref ls1);  // $local
        Fun1(S1: ref ls3);  // $local

        // Fun2 分析: `scoped ref this`,`scoped ref S1`,`out S2`
        //      this: SC = $cm, 不贡献 RC
        //      S1: 要求 SC 是 $cm, 不贡献 RC
        //      S2: 不贡献 RC 和 SC, 可以是 $cm 范围内的任何左值
        //      return: SC 是 $cm, 因此 out 参数可以是任意 $cm 范围内的值
        return Fun2(S1: ref this.span, S2: out s1);
        return Fun2(S1: ref s1, S2: out s4);
        return Fun2(S1: ref ls3, S2: out ls4);

        // Fun3 分析: `scoped ref this`,`scoped ref S1`,`unscoped out S2`
        //      this: SC = $cm, 不贡献 RC
        //      S1: 要求 SC 是 $cm, 不贡献 RC
        //      S2: 不贡献 SC, 可以是 $cm 下的任意左值
        //      return: SC 是 RC(S2)
        Fun3(S1: ref s1, S2: out s2);  // $local
        return Fun3(S1: ref s5, S2: out s1);  // $ro
        return Fun3(S1: ref ls3, S2: out s3); // $ro

        //// Fun4 分析: `unscoped ref this`,`ref S1`,`out S2`
        ////      this: SC = $cm, 贡献 RC
        ////      S1: SC 必须是 $cm
        ////      S2: 不贡献 RC 和 SC
        ////      return: SC 是 RC(this), RC(S1) 中的最小值 $local, 因此 out 参数只能是 $local
        Fun4(S1: ref this.span, out s4);  // $local
        Fun4(S1: ref s1, out _);     // $local
        Fun4(S1: ref ls3, out ls4);  // $local

        // Fun5 分析: `scoped ref this`,`S1`,`out S2`
        //      this: SC = $cm, 不贡献 RC
        //      S1: 要求 SC 是 $cm, 不贡献 RC
        //      S2: 不贡献 RC 和 SC, 可以是 $cm 范围内的任何左值
        //      return: SC 是 $cm
        return Fun5(S1: this.span, S2: out ls4);
        return Fun5(S1: s1, S2: out ls4);
        return Fun5(S1: ls3, S2: out s1);

        // Fun6 分析: `scoped ref this`,`S1`,`ref S2`
        //      this: SC = $cm, 不贡献 RC
        //      S1: 要求 SC 是 $cm, 不贡献 RC
        //      S2: 要求 SC 是 $cm
        //      return: SC 是 RC(S2)
        return Fun6(S1: ls3, S2: ref s1); // $ro
        Fun6(S1: ls3, S2: ref s5);        // $local

        // Fun7 分析: `scoped ref this`,`ref S1`,`unscoped out s2`
        //      this: SC = $cm, 不贡献 RC
        //      S1: 要求 SC 是 $cm
        //      S2: 不贡献 SC, SC(S2) 的范围不超过 RC(return)
        //      return: SC 是 S1,S2 中最窄的 RC
        // 这类情况限制 RC(S1) >= SC(S2) >= RC(S2), SC(return) = Min{RC(S1), RC(S2)}, SC(S2) <= SC(return)
        // 因此, SC(S2) == RC(S2), SC(return) == RC(S2)
        Fun7(S1: ref s1, S2: out ls2);         // $local
        Fun7(S1: ref this.span, S2: out ls4);  // $local
        Fun7(S1: ref s5, S2: out _);     // $local

        // Fun8 分析: `scoped ref this`,`scoped S1`, `out S2`
        //      this: SC = $cm, 不贡献 RC
        //      S1: 要求 SC 是 $cm, 不贡献 RC
        //      S2: 不贡献 RC 和 SC, 可以是 $cm 范围内的任何左值
        //      return: SC 是 $cm
        return Fun8(S1: this.span, S2: out ls4);
        return Fun8(S1: s1, S2: out ls4);
        return Fun8(S1: ls3, S2: out s1);
    }
}
```

> 非 *ref struct* 方法匹配分析

```csharp
using System.Diagnostics.CodeAnalysis;
class Sample
{
    /**
    *  caller-context    $cm
    *  return-only       $ro
    *  function-member   $local
    *  $cm > $ro > $local
    *  RC : ref-safe-context
    *  SC : safe-context
    *  [UnscopedRef] : unscoped
    */
    Span<int> Fun1(ref Span<int> s) => s;
    Span<int> Fun2(scoped ref Span<int> s, out Span<int> s2) { s2 = s; return default; }
    Span<int> Fun3(scoped ref Span<int> s, [UnscopedRef] out Span<int> s2) { s2 = s; return default; }
    Span<int> Fun4(ref Span<int> s, out Span<int> s2) { s2 = s; return default; }
    Span<int> Fun5(Span<int> s, out Span<int> s2) { s2 = s; return default; }
    Span<int> Fun6(Span<int> s, ref Span<int> s2) { s2 = s; return default; }
    Span<int> Fun7(ref Span<int> s, [UnscopedRef] out Span<int> s2) { s2 = s; return default; }

    Span<int> Test(                                     // safe-context      ref-safe-context
        ref int v,                                      //  $cm                $ro
        ref Span<int> s,                                //  $cm                $ro
        out Span<int> os)                               //  $ro                $local
    {
        os = default;
        Span<int> ls0 = s;                              //  $cm                $local
        Span<int> ls1 = default;                        //  $cm                $local
        Span<int> ls2 = new Span<int>(reference: ref v);//  $ro                $local
        Span<int> ls3 = stackalloc int[1];              //  $local             $local 

        var rt = Fun1(s: ref s);
        // Fun1 分析：参数列表为 `ref s`
        //      s: 任何 $cm 范围的值
        //      return: SC 和 s 的 RC 相同  
        return Fun1(s: ref s);    // SC: $ro
        Fun1(s: ref ls3);  // SC: $local

        // Fun2 分析: `scoped ref s`,`out s2`
        //      s: 任何 $cm 范围的值, 不贡献 RC
        //      s2: 不贡献 RC 和 SC, SC 是 SC(s) 范围内的任何值 
        //      return: SC 是所有提供 SC 参数的 SC 中最小的, 因此 SC 和 s 相同
        Fun2(s: ref ls3, s2: out ls3);    // s:$local, 因此 s2 只能是 $local, return:$local
        return Fun2(s: ref os, s2: out os /* ls2,ls3 */);   // s:$ro, SC(s2) <= $ro, return:$ro
        return Fun2(s: ref ls2, s2: out os /* ls2,ls3 */);  // s:$ro,  SC(s2) <= $ro, return:ro 

        // Fun3 分析: `scoped ref s`,`unscoped out s2`
        //      s: 任何 $cm 范围的值, 不贡献 RC
        //      s2: 不贡献 SC, SC 是 SC(s) 范围内的任何值 
        //      return: SC 是 RC(s2) 
        Fun3(s: ref ls3, s2: out ls3);  // SC(ls3):$local, 因此 s2 只能是 $local, return: $local
        Fun3(s: ref os, s2: out ls2);   // SC(os):$ro, SC(s2) <= $ro, return:RC(s2) $local

        // Fun4 分析: `ref s`,`out s2`
        //      s: 任何 $cm 范围的值
        //      s2: 不贡献 RC 和 SC
        //      return: SC 是 RC(s), SC(s2) 是 RC(s) 范围内的任何值
        Fun4(s: ref ls1, s2: out ls3);   // return:RC(ls1) = $local
        return Fun4(s: ref s, s2: out os /* ls2,ls3 */);   // return:RC(s) = $ro, SC(s2) <= $ro

        // Fun5 分析: `s`,`out s2`
        //      s: 任何 $cm 范围的值, 不贡献 RC
        //      s2: 不贡献 RC 和 SC
        //      return: SC 是 SC(s), s2 是 SC(s) 范围的任何值
        Fun5(s: ls3, s2: out ls3);       // return:$local
        return Fun5(s: ls2, s2: out os); // SC(s2) <= SC(s) , return:$ro

        // Fun6 分析: `s`,`ref s2`
        //      s: 任何 $cm 范围的值, 不贡献 RC
        //      s2: SC <= SC(s) 
        //      return: SC 与 RC(s2) 相同
        Fun6(s: ls3, s2: ref ls3);      // return:$local
        Fun6(s: s, s2: ref os);         // return:$local
        return Fun6(s: s, s2: ref s);   // return:$ro

        // Fun7 分析: `ref s`,`unscoped out s2`
        //      s: 任何 $cm 范围的值
        //      s2: 不贡献 SC, SC(s2) <= RC(s) 
        //      return: SC 是 RC(s2)
        Fun7(s: ref ls3, s2: out ls3);  // return:$local
        Fun7(s: ref s, s2: out os);     // return:$local
    }
}
```

>---
### 变量的安全上下文由让它的声明方式决定

#### 方法参数

使用方法参数的 *Lvalue* 表达式表示的 *ref-safe-context*：
- 如果是 `ref` 或 `in` 参数，它的 *ref-safe-context* 是：
  - 默认的 *return-only*。
  - `scoped` 注释的 *function-member*。
  - `[UnscopedRef]` 注释的 *caller-context*。
- 如果是一个 `out` 参数，它的 *ref-safe-context* 是：
  - 隐式 `scoped` 的 *function-member*。
  - 显式 `[UnscopedRef]` 的 *return-only*。
- 结构类型实例方法中的 `this` 被视为 `scoped ref` 参数传递：
  - `this` 的 *ref-safe-context* 是 *function-member*。
  - 结构类型中 `[UnscopedRef]` 标记的实例方法中，`this` 的 *ref-safe-context* 是 *return-only*。
- 类类型实例方法中的 `this` 被视为 `scoped ref` 参数传递，它的 *ref-safe-context* 是 *function-member*。
- 如果是一个值形参，则它的 *ref-safe-context* 是 *function-member*。

使用方法参数的 *Rvalue* 表达式的 *safe-context* 是：
- `ref struct` 类型的 `out` 参数具有 *return-only* 的 *safe-context*。
- 否则，它的 *safe-context* 是 *caller-context*。

#### 局部变量

使用局部变量的 *Lvalue* 表达式的 *ref-safe-context*：
- 如果变量是非 `scoped` 注释的 `ref` 引用变量，它的 *ref-safe-context* 与其初始化项的 *ref-safe-context* 相同。
- `scoped` 注释的 `ref` 变量或 `ref scoped` 类型的变量，它的 *ref-safe-context* 为 *function-context*，并忽略其初始化项的 *ref-safe-context*（如果有）。 
- 否则它的 *ref-safe-context* 是 *declaration-block*。

使用局部变量的 *Rvalue* 表达式的 *safe-context*：
- 如果不是 `ref struct` 类型，它的 *safe-context* 为 *caller-context*。
- 如果变量是 `foreach` 循环的迭代变量，则该变量的 *safe-context* 与 `foreach` 循环体的 *safe-context* 相同。
- 未被 `scoped` 注释的 `ref struct` 类型的局部变量具有：
  - 如果变量声明时未初始化，那么它的 *safe-Context* 为 *caller-context*。
  - 如果变量声明有初始化项，那么它的 *safe-Context* 与该初始化项的 *safe-Context* 相同。
  - 当变量被 `scoped` 注释时，它的 *safe-context* 为 *function-member*。

#### 成员字段

使用字段 `e.F` 的 *Lvalue* 表达式 `= ref e.F` 的 *ref-safe-context*：
- 如果 `e` 是 `ref struct` 类型且 `F` 是一个 `ref` 字段，它的 *ref-safe-context* 是 `e` 的 *safe-context*。
  
  ```csharp
  /**
   * SC = safe-context, RC = ref-safe-context 
   * $cm = caller-context
   * $local = function-member
   * $ro = return-only
   */
  ref struct RS
  {
      public ref int RefField; // SC: $cm, RC: SC(this)
  }
  class Sample
  {
      void GetRef(
          ref RS rs,    // SC: $cm, RC: $ro
          out RS rs2)   // SC: $ro, RC: $local
      {
          rs2 = default(RS);
          RS lrs1 = default(RS);        // SC: $cm, RC: $local
          scoped RS lrs2 = default(RS); // SC: $local, RC: $local
  
          rs.RefField = ref lrs2.RefField;  // err; RC(LHS:$cm) > RC(RHS:$local)
          lrs2.RefField = ref rs.RefField;  // okay
  
          rs2.RefField = ref lrs2.RefField;   // err; RC(LHS:$ro) > RC(RHS:$local)
          lrs2.RefField = ref rs2.RefField;   // okay
  
          lrs1.RefField = ref lrs2.RefField;  // err; RC(LHS:$cm) > RC(RHS:$local)
          lrs2.RefField = ref lrs1.RefField;  // okay
  
          lrs1.RefField = ref rs2.RefField;  // err; RC(LHS:$cm) > RC(RHS:$ro)
          rs2.RefField = ref lrs1.RefField;  // okay
      }
  }
  ```

- 如果 `e` 是引用类型，则 `F` 的 *ref-safe-context* 是 *caller-context*。

  ```csharp
  ref struct RSample
  {
      ref int RefValue;
      class Sample { public int Value; }
  
      void GetRef(ref Sample s1, Sample s2, scoped ref Sample s3)
      {
          Sample ls = new Sample();
  
          // RC both are $cm
          RefValue = ref s1.Value;
          RefValue = ref s2.Value;
          RefValue = ref s3.Value;
          RefValue = ref ls.Value;
      }
  }
  ```

- 如果 `e` 是值类型，则 `F` 的 *ref-safe-context* 与 `e` 的 *ref-safe-context* 相同。`[UnscopedRef]` 标记后的结构实例方法中，`this` 及其字段的 *ref-safe-context* 提升为 *return-only*。     

  ```csharp
  /**
   * SC = safe-context, RC = ref-safe-context 
   * $cm = caller-context
   * $local = function-member
   * $ro = return-only
   */
  using System.Diagnostics.CodeAnalysis;
  using Span = System.Span<int>;
  struct Sample
  {
      int Value;  // SC: $cm, RC: RC(this)
  
      ref struct RS
      {
          // ==== Ref non-ref-struct Field ====
          public ref int RefValue;  // SC: $cm, RC: SC(this)
          ref int GetIntRef(
              // scoped ref this     // SC: $cm, RC: $local
              ref int rValue,        // SC: $cm, RC: $ro
              ref Sample s1,         // SC: $cm, RC: $ro
              Sample s2,             // SC: $cm, RC: $local
              scoped ref Sample s3,  // SC: $cm, RC: $local
              out Sample s4)         // SC: $cm, RC: $local
          {
              s4 = default(Sample);
              Sample ls = new Sample();  // SC: $cm, RC: $local
  
              // SC both $cm
              RefValue = ref s1.Value;  // err; RC(LHS:$cm) > RC(RHS:$ro)
              RefValue = ref s2.Value;  // err; RC(LHS:$cm) > RC(RHS:$local)
              RefValue = ref s3.Value;  // err; RC(LHS:$cm) > RC(RHS:$local)
              RefValue = ref s4.Value;  // err; RC(LHS:$cm) > RC(RHS:$local)
              RefValue = ref ls.Value;  // err; RC(LHS:$cm) > RC(RHS:$local)
  
              rValue = ref s1.Value;  // okay; RC(LHS:$ro) == RC(RHS:$ro)
              rValue = ref s2.Value;  // err;  RC(LHS:$ro) > RC(RHS:$local)
              rValue = ref ls.Value;  // err;  RC(LHS:$ro) > RC(RHS:$local)
  
              int lv = 0;
              ref int lrValue = ref lv;  // SC: $cm, RC: $local
              lrValue = ref ls.Value;    // okay; RC both local
  
              ref int lrValue2 = ref s1.Value;  // SC: $cm, RC = RC(s1.Value) = $ro
              lrValue2 = ref rValue;   // okay;
              lrValue2 = ref lrValue;  // err; RC(LHS:$ro) > RC(RHS:$local)
  
              // $ro return okay
              return ref rValue;
              return ref s1.Value;
              return ref lrValue2;
              return ref this.RefValue;  // RC: $cm
  
              // err return;
              return ref s2.Value;  // RC: $local
              return ref s3.Value;  // RC: $local
              return ref lrValue;   // RC: $local
          }
  
          // ==== Ref ref-struct Field ====
          Span<int> span;  // SC: $cm, RC: SC(this)
                           // using Span = System.Span<int> at file start
          ref Span<int> RefSpan(
               // scoped ref this     // SC: $cm, RC: $local
               ref Span<int> rValue,  // SC: $cm, RC: $ro
               ref RS s1,             // SC: $cm, RC: $ro
               RS s2,                 // SC: $cm, RC: $local
               scoped ref RS s3,      // SC: $cm, RC: $local
               out RS s4,             // SC: $ro, RC: $local
               out Span<int> sp,      // SC: $ro, RC: $local
               [UnscopedRef] out Span<int> sp2,  // SC: $ro, RC: $ro
               [UnscopedRef] out RS s5)          // SC: $ro, RC: $ro
          {
              sp = default(Span);
              sp2 = default(Span);
              s4 = default(RS);
              s5 = default(RS);
              Span ls1 = default(Span);     // SC: $cm, RC: $local
              Span ls2 = stackalloc int[1]; // SC: $local, $RC: local
  
              // SC: $cm, RC: $local
              ref Span rs = ref span;  // SC: SC(this) = $cm, RC = RC(this) = $local
              rs = ref rValue;   // okay; same SC AND RC(LHS) < RC(RHS)
              rs = ref s1.span;  // okay; same SC AND RC(LHS:local) < RC(RHS:$ro)  
              rs = ref s2.span;  // okay; same SC AND RC both $local
              rs = ref sp;       // err; SC(LHS:$cm) != SC(RHS:$ro)
              rs = ref ls1;      // okay; same SC AND RC(LHS:local) == RC(RHS:$local)  
              rs = ref ls2;      // err; SC(LHS:$cm) != SC(RHS:$local)
  
              // SC: $cm, RC: $ro
              ref Span rs2 = ref rValue;  // SC: $cm, RC: $ro
              rs2 = ref s1.span;  // RC == RC(RHS:$ro)  
              rs2 = ref sp2;      // same RC, but different SC
              rs2 = ref s2.span;  // RC > RHS:$local
  
              // SC: $ro, RC: $local
              ref Span rs3 = ref sp;
              rs3 = ref ls2;  // SC(ls2) = $local
              rs3 = ref sp2;  // okay; SC both $ro, RC(LHS:$local) < RC(RHS:$ro)
              rs3 = ref s5.span;  // okay
  
              // SC: $local, RC: $local
              ref Span rs4 = ref ls2;
              // .... 任意 span 无法转换为 rs4
  
              // okay return 
              return ref s1.span;  // RC: $ro
              return ref sp2;      // RC: $ro
              return ref s5.span;  // RC: $ro
              return ref rValue;
  
              // err return
              return ref span;  // RC(Field) = $RC(this) = $local, 若是 [UnscopedRef] 则 $RC(this) = $ro
              return ref s2.span;  
              return ref ls1;
              return ref sp;
              return ref s4.span;  // RC: $local
          }
      }
      // ====  Ref This ====
      ref int NonRefThisField() => ref Value;  // err; RC(Field) = RC(this) = $local
      [UnscopedRef] ref int RefThisField() => ref this.Value;  // okay; RC(this) = $ro
      [UnscopedRef] ref Sample RefThis() => ref this;
  }
  ```

使用字段 `e.F` 的 *Rvalue* 表达式 `= e.F`：
- 字段的类型是 `ref struct` 的 *safe-context* 与 `e` 的 *safe-context* 相同。
- 否则，它的 *safe-context* 是 *caller-context*。
 
```csharp
/**
 * SC = safe-context, RC = ref-safe-context 
 * $cm = caller-context
 * $local = function-member
 * $ro = return-only
 */
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Span = System.Span<int>;

ref struct RS
{
    // ==== ref-struct Field ====
    Span<int> span;  // SC: SC(this), RC: SC(this)
                     // using Span = System.Span<int> at file start
    Span<int> RefSpan(
         // scoped ref this     // SC: $cm
         ref Span<int> rValue,  // SC: $cm
         ref RS s1,             // SC: $cm
         RS s2,                 // SC: $cm
         out RS s3,             // SC: $ro
         out Span<int> sp,      // SC: $ro
         scoped Span<int> span1)  // SC: $local
    {
        sp = default(Span);
        s3 = default(RS);
        Span ls1 = default(Span);     // SC: $cm
        Span ls2 = stackalloc int[1]; // SC: $local

        // SC: $cm, SC(span) = $cm
        span = rValue;   // okay; both $cm
        span = s1.span;  // okay;
        span = s2.span;  // okay; 
        span = s3.span;  // err; SC(LHS:$cm) > SC(RHS:$ro)
        span = sp;       // err; SC(LHS:$cm) > SC(RHS:$ro)
        span = ls1;      // okay; 
        span = ls2;      // err; SC(LHS:$cm) > SC(RHS:$local)
        span = span1;    // err; SC(RHS:$local)

        // SC: $ro, SC(s3.span) = $ro
        s3.span = span;  // okay
        s3.span = sp;    // okay; both $ro
        s3.span = ls2;   // err; SC(ls2) = $local

        // SC: $local
        scoped Span lspan = span;  // SC: $local
        lspan = ls2;  // okay; SC: $local
        sp = lspan;   // err;  SC(sp) = $ro
        lspan = span1;  // okay;
        sp = span1;   // err;  SC(RHS) = $local

        // okay return 
        return span;     // $cm
        return s1.span;  // $cm
        return rValue;   // $cm
        return s3.span;  // $ro
        return sp;       // $ro

        // err return
        return ls2;    // $local
        return lspan;  // $local
        return span1;  // $local
    }
}
```

#### 推断声明表达式的安全上下文

具有输出参数的方法 `M(x, out var y)` 或解构函数 `(var x, var y) = M()` 的推断声明表达式 `var 'Rvalue'` 的 *safe-context* 是：
- *caller-context*。
- 如果 `out` 参数的类型是 `ref struct`：
  - 如果 `out` 变量的实参标记为 `out scoped var x`，它的安全上下文是 *function-member* 或更窄的 *declaration-block*。
  - 否则需要考虑方法中包含调用的其他所有参数，且包含接收方，它的安全上下文是以下最窄的：
    - 任何非 `out` 参数的 *safe-context*。 
  - 任何参数的 *ref-safe-context*。

由 `scoped` 修饰符产生的局部上下文是可能用于变量的最窄的上下文，任何更窄的上下文都意味着表达式引用的变量只能在比表达式更窄的上下文中声明。

```csharp
ref struct RSample
{
    public RSample(ref int x) { } // assumed to be able to capture 'x'
    static void Fun(RSample input, out RSample output) => output = input;
    static RSample FunLocal(ref int v)
    {
        var i = 0;
        var lrs = new RSample(ref i);  // safe-context of 'lrs' is function-member
        Fun(lrs, out var rs_out); // safe-context of 'rs_out' is function-member
        return rs_out;  // err;  

        var lrs2 = new RSample(ref v);  // safe-context of 'lrs2' is return-only
        Fun(lrs2, out var rs_out2);  // safe-context of 'rs_out2' is return-only
        return rs_out2;  // okay
    }
    static RSample FunCm(RSample rs)
    {
        Fun(rs, out var rs_out); // safe-context of 'rs' is caller-context
        return rs_out;  // okay
    }
    static RSample FunScoped(RSample rs)
    {
        // 'scoped' modifier forces safe-context of 'rs2' to the current local context (function-member or narrower).
        Fun(rs, out scoped var rs_scoped);  // scoped out var is function-context
        return rs_scoped; // err;
    }
}
```

#### 函数成员调用

属性调用（`get` 或 `set`）被视为对底层方法的方法调用。调用构造函数的 `new` 表达式被视为对正在构造类型的方法调用，如果存在初始化项，则它的 *safe-context* 不能大于对象初始化项的所有参数和操作数中最窄的 *safe-context*。用户定义的运算符操作被视为方法调用。实例方法中包含隐式参数 `ref this`，构造函数包含隐式参数 `out this`。

对于方法调用 `var rt = e.M(e1,e2,...)` 产生的值 `rt`，当 `M()` 方法不返回 `return ref ref-struct` 时，方法返回值的 *safe-context* 与以下上下文中最窄的相同：
- *caller-context*。
- 当返回类型是 `ref struct` 时，由所有参数表达式提供的 *safe-context*。
- 当返回类型是 `ref struct` 时，由所有非 `scoped` 注释的 `ref` 参数、`[UnscopedRef] this` 隐式参数、`in` 参数、`[UnscopedRef] out` 参数提供提供的 *ref-safe-context*。
- 一般而言参数的 *safe-context* 大于等于它的 *ref-safe-context*，因此引用传递的参数只考虑它的 *ref-safe-context*。

```csharp
using System.Diagnostics.CodeAnalysis;
ref struct RSample
{
    Span<int> Span;  // SC: caller-context;
    Span<int> Fun1(int v, ref int v2) => default(Span<int>);
    Span<int> Fun2(int v, ref int v2, scoped ref int v3) => default(Span<int>);
    [UnscopedRef] Span<int> Fun3(int v, ref int v2) => default(Span<int>);
    Span<int> Fun4(int v, ref int v2, out int v3) => throw new Exception();
    Span<int> Fun5(int v, ref int v2, [UnscopedRef] out int v3) => throw new Exception();
    Span<int> Fun6(Span<int> s, ref int v2, [UnscopedRef] out int v3) => throw new Exception();

    Span<int> Create(ref int v,             // SC: $cm,  RC: $ro
        [UnscopedRef] ref int v_unscoped,   // SC: $cm,  RC: $cm
        scoped ref int v_scoped)            // SC: $cm,  RC: $local
    {
        // Fun1 分析: 参数列表为 `scoped ref this`,`v`,`ref v2`
        // return 的 SC 是 SC(this),SC(v:$cm),RC(v2) 中最窄的
        var Fun1_rt1 = Fun1(v, ref v);  // $ro
        var Fun1_rt2 = Fun1(v, ref v_scoped);  // $local
        var Fun1_rt3 = Fun1(v, ref v_unscoped); // $cm
        return Fun1_rt1;  // okay
        return Fun1_rt3;  // okay
        Fun1_rt1 = Fun1_rt2; // err; SC(LHS) > SC(RHS) : $ro > $local
        Fun1_rt3 = Fun1_rt1; // err; SC(LHS) > SC(RHS) : $cm > $ro

        // Fun2 分析: `scoped ref this`,`v`,`ref v2`,`scoped ref v3`
        // return 的 SC 是 SC(this),SC(v:$cm),RC(v2),SC(v3) 中最窄的
        return Fun2(v, ref v_unscoped, ref v_scoped); // $cm
        return Fun2(v_scoped, ref v, ref v_scoped); // $ro
        return Fun2(v_scoped, ref v_scoped, ref v_scoped); // $local

        // Fun3 分析: `unscoped ref this`,`v`,`ref v2`
        // return 的 SC 是 RC(this),SC(v:$cm),RC(v2) 中最窄的
        Fun3(v_scoped, ref v);  // $local; RC(this) = $local in this context, but in Fun3 RC(Fun3.this) is $ro

        // Fun4 分析: `scoped ref this`,`v`,`ref v2`,`out v3`
        // return 的 SC 是 SC(this),SC(v),RC(v2) 中最窄的
        return Fun4(v_scoped, ref v, out v_scoped);  // $ro
        return Fun4(v_scoped, ref v_unscoped, out v_scoped);  // $cm
        return Fun4(v_scoped, ref v_scoped, out v_scoped);  // $local

        // Fun5 分析: `scoped ref this`,`v`,`ref v2`,`unscoped out v3`
        // return 的 SC 是 SC(this),SC(v),RC(v2),RC(v3) 中最窄的
        return Fun5(v_scoped, ref v_unscoped, out v_scoped);  // $local
        return Fun5(v_scoped, ref v_unscoped, out v);  // $ro
        return Fun5(v_scoped, ref v_unscoped, out v_unscoped);  // $cm
        return Fun5(v_scoped, ref v_scoped, out v_unscoped);  // $local
        return Fun5(v_scoped, ref v, out v_unscoped);  // $ro

        // Fun6 分析: `scoped ref this`,`s`,`ref v2`,`unscoped out v3`
        // return 的 SC 是 SC(this),SC(s),RC(v2),RC(v3) 中最窄的
        return Fun6(new Span<int>(ref v), ref v_unscoped, out v_unscoped); // $ro
        return Fun6(new Span<int>(ref v_scoped), ref v_unscoped, out v_unscoped); // $local
        return Fun6(new Span<int>(ref v_scoped), ref v_unscoped, out v_unscoped); // $local
        return Fun6(new Span<int>(ref v_unscoped), ref v_unscoped, out v_unscoped); // $cm
        return Fun6(new Span<int>(ref v_unscoped), ref v_unscoped, out _); // $local
        // 弃元被视为局部变量，它的 RC 是 $local
    }
}
```

对于方法调用 `var rt = e.M(e1,e2,...)` 产生的值 `rt`，当 `M()` 方法返回 `return ref ref-struct` 时，方法返回值的 *safe-context* 与所有类型 是 `ref struct` 的 `ref` 参数、`in` 参数、（如果是）`ref struct` 结构的实例方法中的 `this` 的 *safe-context* 相同。

```csharp
using System.Diagnostics.CodeAnalysis;
struct Sample
{
    ref Span<int> Fun1(Span<int> v, ref Span<int> v2) => throw new Exception();
    ref Span<int> Fun2(Span<int> v, ref Span<int> v2, scoped ref Span<int> v3) => throw new Exception();
    ref Span<int> Fun3(Span<int> v, ref Span<int> v2, out Span<int> v3) => throw new Exception();
    ref Span<int> Fun4(Span<int> v, ref Span<int> v2, [UnscopedRef] out Span<int> v3) => throw new Exception();

    Span<int> Create(ref Span<int> r,             // SC: $cm,  RC: $ro
        [UnscopedRef] ref Span<int> r_unscoped,   // SC: $cm,  RC: $cm
        scoped ref Span<int> r_scoped,            // SC: $cm,  RC: $local
        out Span<int> o_scoped,                   // SC: $ro,  RC: $local
        [UnscopedRef] out Span<int> o_unscoped,   // SC: $ro,  RC: $ro
        scoped Span<int> v_scoped)                // SC: $local, RC: $local
    {
        // 上下文范围降序 r_unscoped > r > r_scoped > o_unscoped > o_scoped > v_scoped
        o_scoped = default;
        o_unscoped = default;
        // Fun1 分析: 类型为 ref struct 的 ref-likes 参数为 `ref v2`
        // return 的 SC 与 SC(v2) 相同, v 的 SC >= SC(v2)
        return Fun1(v: r, v2: ref r_scoped);  // $cm
        return Fun1(v: r_scoped, v2: ref o_scoped);   // $ro
        return Fun1(v: o_unscoped, v2: ref o_unscoped); // $ro
        Fun1(v: r_unscoped, v2: ref v_scoped); // $local
        Fun1(v: v_scoped, v2: ref v_scoped);   // $local

        // Fun2 分析: `ref v2`,`scoped ref v3`
        // return 的 SC 与 SC(v2), SC(v3) 相同, SC(v) must >= SC(return) 
        return Fun2(v: r, ref r_scoped, ref r_scoped); // $cm
        return Fun2(v: o_scoped, ref o_unscoped, ref o_scoped);  // $ro
        Fun2(v: r_unscoped, v2: ref v_scoped, v3: ref v_scoped); // $local
        Fun2(v: v_scoped, v2: ref v_scoped, v3: ref v_scoped); // $local

        // Fun3 分析: `ref v2`,`out v3`
        // return 的 SC 与 SC(v2) 相同, MUST: SC(v) >= SC(return) >= SC(out), RC(ref) >= RC(out)
        return Fun3(r_unscoped, ref r_unscoped, out r_unscoped);  // $cm
        return Fun3(o_scoped, ref o_scoped, out v_scoped);  // $ro
        Fun3(v_scoped, ref v_scoped, out v_scoped);  // $local

        // Fun4 分析: `ref v2`,`unscoped out v3`
        // return 的 SC 与 SC(v2) 相同, MUST: SC(v) >= SC(return) >= SC(out), RC(ref) >= RC(out)
        return Fun4(r_unscoped, ref r_unscoped, out r_unscoped);  // $cm
        return Fun4(r_unscoped, ref o_scoped, out v_scoped);  // $ro
        Fun4(v_scoped, ref v_scoped, out v_scoped);  // $local
    }
}
```

对于方法调用 `= ref e.M(e1,e2,...)` 产生的 *Lvalue*，当 `M()` 方法不返回 `return ref ref-struct` 时，方法返回值的 *ref-safe-context* 是以下上下文中最窄的：
- *caller-context*。
- 所有参数的 *safe-context*。
- 所有非 `scoped` 注释的 `ref`、`in`、`[UnscopedRef] out`、参数的 *ref-safe-cotext*，如果是 `[UnscopedRef]` 方法，则包含 `ref this` 的 *ref -safe-context*。

```csharp
using System.Diagnostics.CodeAnalysis;
struct Sample
{
    ref int Fun1(int v, ref int v2) => throw new Exception();
    ref int Fun2(int v, ref int v2, scoped ref int v3) => throw new Exception();
    [UnscopedRef] ref int Fun3(int v, ref int v2) => throw new Exception();
    ref int Fun4(Span<int> v, int v2) => throw new Exception();
    ref int Fun5(Span<int> v, ref int v2, [UnscopedRef] out int v3) => throw new Exception();

    int Value; // SC: $cm, RC: RC(this)
    ref int Create(
         // scoped ref this.Value               // SC: $cm,  RC: local
         // unscoped ref this.Value             // SC: $cm,  RC: $ro
         ref int rv,                            // SC: $cm,  RC: $ro
         scoped ref int rv_scoped,              // SC: $cm,  RC: $local
         [UnscopedRef] ref int rv_unscoped)     // SC: $cm,  RC: $cm
    {
        // Fun1 分析: 参数列表为 `scoped ref this`,`v`,`ref v2`
        // return 的 RC 是 SC(this),SC(v:$cm),RC(v2) 中最窄的
        return ref Fun1(Value, ref rv); // $ro
        _ = ref Fun1(Value, ref rv_scoped); // $local
        return ref Fun1(Value, ref rv_unscoped); // $cm

        // Fun2 分析: `scoped ref this`,`v`,`ref v2`,`scoped ref v3`
        // return 的 RC 是 SC(this),SC(v:$cm),RC(v2),SC(v3) 中最窄的
        return ref Fun2(Value, ref rv_unscoped, ref rv); // $cm
        return ref Fun2(Value, ref rv, ref rv_scoped); // $ro
        _ = ref Fun2(Value, ref rv_scoped, ref rv_unscoped); // $local

        // Fun3 分析: `unscoped ref this`,`v`,`ref v2`
        // return 的 RC 是 RC(this), RC(this) = $local
        _ = ref Fun3(Value, ref rv);  // $local
        _ = ref Fun3(Value, ref rv_scoped); // $local; 

        // Fun4 分析: `scoped ref this`,`v`
        // return 的 RC 是 SC(this),SC(v) 中最窄的
        return ref Fun4(new Span<int>(ref rv), Value);  // $ro
        _ = ref Fun4(new Span<int>(ref rv_scoped), Value);  // $local
        return ref Fun4(new Span<int>(ref rv_unscoped), Value);  // $cm

        // Fun5 分析: `scoped ref this`,`v`,`ref v2`,`unscoped out v3`
        // return 的 RC 是 SC(this),SC(v),RC(v2),RC(v3) 中最窄的
        _ = ref Fun5(new Span<int>(ref rv_unscoped), ref rv_unscoped, out rv_scoped);  // $local
        return ref Fun5(new Span<int>(ref rv_unscoped), ref rv_unscoped, out rv);  // $ro
        return ref Fun5(new Span<int>(ref rv_unscoped), ref rv_unscoped, out rv_unscoped);  // $cm
        _ =  ref Fun5(new Span<int>(ref rv_unscoped), ref rv_unscoped, out _); // $local
    }
}
```

对于方法调用 `= ref e.M(e1,e2,...)` 产生的 *Lvalue*，当 `M()` 方法返回 `return ref ref-struct` 时，方法返回值的 *ref-safe-context* 与所有类型是 `ref struct` 的 `ref` 参数、`in` 参数、`[UnscopedRef] out`、（方法如果是）`ref struct` 结构的实例方法中的 `this` 的 *ref-safe-context* 中最窄的。

```csharp
using System.Diagnostics.CodeAnalysis;
struct Sample
{
    ref Span<int> Fun1(Span<int> v, ref Span<int> v2) => throw new Exception();
    ref Span<int> Fun2(Span<int> v, ref Span<int> v2, scoped ref Span<int> v3) => throw new Exception();
    ref Span<int> Fun3(Span<int> v, ref Span<int> v2, out Span<int> v3) => throw new Exception();
    ref Span<int> Fun4(Span<int> v, ref Span<int> v2, [UnscopedRef] out Span<int> v3) => throw new Exception();

    ref Span<int> Create(ref Span<int> r,             // SC: $cm,  RC: $ro
         [UnscopedRef] ref Span<int> r_unscoped,      // SC: $cm,  RC: $cm
         scoped ref Span<int> r_scoped,               // SC: $cm,  RC: $local
         out Span<int> o_scoped,                      // SC: $ro,  RC: $local
         [UnscopedRef] out Span<int> o_unscoped,      // SC: $ro,  RC: $ro
         scoped Span<int> v_scoped)                   // SC: $local, RC: $local
    {
        // 上下文范围降序 r_unscoped > r > r_scoped > o_unscoped > o_scoped > v_scoped
        o_scoped = default;
        o_unscoped = default;
        // Fun1 分析: 类型为 ref struct 的 ref-likes 参数为 `ref v2`
        // return ref 的 RC 是 RC(v2) 
        return ref Fun1(v: r, v2: ref r_unscoped);  // $cm
        return ref Fun1(v: r_scoped, v2: ref r);    // $ro
        return ref Fun1(v: o_unscoped, v2: ref o_unscoped); // $ro
        return ref Fun1(v: r_unscoped, v2: ref r_scoped); // $local
        return ref Fun1(v: v_scoped, v2: ref v_scoped);   // $local

        // Fun2 分析: `ref v2`,`scoped ref v3`
        // return ref 的 RC 与 RC(v2) 相同, SC(v) must >= SC(return) 
        return ref Fun2(v: r, ref r_scoped, ref r_scoped); // $local
        return ref Fun2(v: r_scoped, ref r_unscoped, ref r_unscoped);  // $cm
        return ref Fun2(v: o_scoped, ref o_unscoped, ref o_scoped);  // $ro
        return ref Fun2(v: r_unscoped, v2: ref v_scoped, v3: ref v_scoped); // $local
        return ref Fun2(v: v_scoped, v2: ref v_scoped, v3: ref v_scoped); // $local

        // Fun3 分析: `ref v2`,`out v3`
        // return ref 的 RC 与 RC(v2) 相同, MUST: SC(v) >= SC(return) >= RC(out), RC(ref) >= RC(out)
        return ref Fun3(r_unscoped, ref r_unscoped, out r_unscoped);  // $cm
        return ref Fun3(o_scoped, ref o_unscoped, out v_scoped);  // $ro
        return ref Fun3(v_scoped, ref v_scoped, out v_scoped);  // $local

        // Fun4 分析: `ref v2`,`unscoped out v3`
        // return ref 的 RC 是 RC(v2),RC(v3) 中最窄的, MUST: SC(v) >= SC(return) >= RC(out), RC(ref) >= RC(out)
        return ref Fun4(r_unscoped, ref r_unscoped, out r_unscoped);  // $cm
        return ref Fun4(r_unscoped, ref r, out o_unscoped);  // $ro
        return ref Fun4(r_unscoped, ref o_unscoped, out v_scoped);  // $local
        return ref Fun4(r_unscoped, ref o_unscoped, out o_unscoped);  // $ro
        return ref Fun4(v_scoped, ref v_scoped, out v_scoped);  // $local
    }
}
```

#### 运算符和表达式

对于产生右值的运算符结果（例如 `e1 + e2` 或 `c ? e1 : e2`），结果的 *safe-context* 范围与运算符所有操作数中最窄的 *safe-context*。因此一元运算符的结果和运算符操作数的 *safe-context*。

使用运算符结果的 *Lvalue* 表达式（例如 `c? ref e1 : ref e2`），结果的 *ref-safe-context* 范围是所有操作数中最窄的 *ref-safe-context*。对于运算符的操作数 `e1` 和 `e2`，要求它们的 *safe-context* 必须一致。

`stackalloc` 表达式的值是一个 *Rvalue*，它的 *safe-context* 是 *function-member*，可以安全地转义到方法堆栈的上层调用，而无法从该方法转义到调用方。

`default` 表达式的 *safe-context* 是 *caller-context*。


```csharp
static Span<int> CreateSpanExample1(ref int i)
{
    var result = new Span<int>(ref i);
    return result;
}
static Span<int> CreateSpanExample2(ref int i)
{
    Span<int> result;
    result = new Span<int>(ref i); // Fails to compile on this line
    return result;
}
static Span<int> CreateSpanExample3(ref int i)
{
    Span<int> result = stackalloc int[0];
    result = new Span<int>(ref i);
    return result; // Fails to compile on this line
}
```

>---
### 附录 

预定义：
- `rs` 为 `ref struct` 类型的变量。
- `v` 为非 `ref struct` 的变量。
- `unscopd` 为 `[UnscopedRef]`。
- `EInit` 为初始化设定项。 

> 参数

| Parameter            | safe-context       | ref-safe-context  |
| -------------------- | ------------------ | ----------------- |
| `rs`                 | *caller-context*   | *function-member* |
| `scoped rs`          | *function-context* | *function-member* |
| `ref/in rs`          | *caller-context*   | *return-only*     |
| `scoped ref/in rs`   | *caller-context*   | *function-member* |
| `unscoped ref/in rs` | *caller-context*   | *caller-context*  |
| `out rs`             | *return-only*      | *function-member* |
| `unscoped out rs`    | *return-only*      | *return-only*     |
| `v`                  | *caller-context*   | *function-member* |
| `ref/in v`           | *caller-context*   | *return-only*     |
| `scoped ref/in v`    | *caller-context*   | *function-member* |
| `unscoped ref/in v`  | *caller-context*   | *caller-context*  |
| `out v`              | *caller-context*   | *function-member* |
| `unscoped out v`     | *caller-context*   | *return-only*     |

> 局部变量

| Local                | safe-context      | ref-safe-context  |
| -------------------- | ----------------- | ----------------- |
| `rs = EInit`         | *SC of EInit*     | *function-member* |
| `ref rs = ref EInit` | *SC of EInit*     | *RC of EInit*     |
| `scoped rs`          | *function-member* | *function-member* |
| `scoped ref rs`      | *function-member* | *function-member* |
| `v`                  | *caller-context*  | *function-member* |
| `ref v = ref EInit`  | *caller-context*  | *RC of EInit*     |
| `scoped ref v`       | *caller-context*  | *function-member* |

> 字段

| Field                 | safe-context     | ref-safe-context |
| --------------------- | ---------------- | ---------------- |
| `struct.Field`        | *caller-context* | *RC of struct*   |
| `class.Field`         | *caller-context* | *caller-context* |
| `rs.ref-struct_Field` | *SC of rs*       | *RC of struct*   |
| `rs.Ref_Field`        | *caller-context* | *RC of rs*       |

---