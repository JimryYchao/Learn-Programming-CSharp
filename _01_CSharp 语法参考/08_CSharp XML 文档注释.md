# CSharp XML 文档注释

C# 源文件可以具有结构化注释，这些注释生成这些文件中定义的类型的 API 文档。编译器会生成一个 XML 文件，其中包含表示注释和 API 签名的结构化数据。

---
## 创建 XML 文档输出

```csharp
/// <summary>
///  This class performs an important function.
/// </summary>
public class MyClass {}
```

设置 `<GenerateDocumentationFile>` 编译器选项控制编译器是否为库生成 XML 文档文件。若没有通过 `<DocumentationFile>` 指定文件名，生成的 XML 文档会放置在与程序集相同的输出目录中，并具有相同的文件名（DllName.xml）。要使 **IntelliSense** 与文档一起正常运行，文件名必须与程序集名称相同，并且必须与程序集位于同一目录中（程序集引用时）。

```xml
<Project Sdk="Microsoft.NET.Sdk">
    
    <PropertyGroup>
        <TargetFramework>net8.0</TargetFramework>
		<ImplicitUsings>enable</ImplicitUsings>
		<!-- 在程序集根目录生成同名 XML 文档 -->
		<GenerateDocumentationFile>true</GenerateDocumentationFile>
		<Nullable>enable</Nullable>
	</PropertyGroup>

</Project>
```

设置 `<DocumentationFile>` 为包含库文档的 XML 文件指定文件名，并将文档输出到指定路径。如果指定此属性，但将 `GenerateDocumentationFile` 设置为 `false`，编译器将不会生成文档文件。如果指定此属性并省略 `GenerateDocumentationFile` 属性，编译器将生成文档文件（在程序集根目录和文档输出目录各生成一份）。

```xml
<Project Sdk="Microsoft.NET.Sdk">
    
    <PropertyGroup>
        <TargetFramework>net8.0</TargetFramework>
		<ImplicitUsings>enable</ImplicitUsings>
		<!-- 在程序集根目录生成 XML 文档 -->
        <!-- 设置为 false 时只输出文档到指定目录，不在程序集根目录生成 XML 文档 -->
		<GenerateDocumentationFile>true</GenerateDocumentationFile>
        <!-- 指定文档名称和输出路径 -->
		<DocumentationFile>Path/to/MyLib</DocumentationFile>
		<Nullable>enable</Nullable>
	</PropertyGroup>

</Project>
```

可以使用转义字符 `&lt;` 和 `&gt;` 在 XML 文档内容中显示尖括号。

```csharp
/// <summary>
/// The example of &lt;<see href="https://learn.microsoft.com/dotnet/csharp/language-reference/xmldoc/"/>&gt;
/// </summary>
class SeeExample;
```

>---
### ID 字符串

每个类型或成员均存储在输出 XML 文件的元素中，其中每个元素都具有唯一的 ID 字符串用于标识类型或成员。ID 字符串必须考虑运算符、参数、返回值、泛型类型参数、`ref`、`in` 和 `out` 参数。若要对所有这些潜在元素进行编码，编译器应遵循明确定义的用于生成 ID 字符串的规则。处理 XML 文件的程序使用 ID 字符串来标识文档应用于的相应 .NET 元数据或反射项目。


| 字符 | 成员类型    | 描述                                                                          |
| :--- | :---------- | :---------------------------------------------------------------------------- |
| N    | `namespace` | 无法将文档注释添加到命名空间中，但可以在支持的情况下对它们进行 cref 引用。    |
| T    | `type`      | 用户定义类型：类、接口、结构、枚举或委托                                      |
| F    | `Field`     | 成员字段                                                                      |
| P    | `property`  | 包括索引器或其他索引属性                                                      |
| M    | `method`    | 包括特殊方法，如构造函数和运算符                                              |
| E    | `event`     | 成员事件                                                                      |
| !    | 错误字符串  | 字符串的其余部分提供有关错误的信息。C# 编译器将生成无法解析的链接的错误信息。 |

ID 后面紧随成员的完全限定名称。`.` 分隔项目名称、封闭类型和命名空间，成员名本身包含 `.` 时使用 `#` 标记替换（例如 `.ctor` 替换为 `#ctor`）。泛型类型的限定名称后有一个附带的 `'` 并紧跟类型参数的数目。方法中的引用传递参数在其类型名后有一个 `@`，值参数没有后缀。

> 类型 IDs

```csharp
enum Color { Red, Blue, Green }
namespace Acme
{
    interface IProcess { ... }
    struct ValueType { ... }
    class Widget : IProcess
    {
        public class NestedClass { ... }
        public interface IMenuItem { ... }
        public delegate void Del(int i);
        public enum Direction { North, South, East, West }
    }
    class MyList<T>
    {
        class Helper<U, V> { ... }
    }
}
/** --- IDs --------------------------------------
"T:Color"
"T:Acme.IProcess"
"T:Acme.ValueType"
"T:Acme.Widget"
"T:Acme.Widget.NestedClass"
"T:Acme.Widget.IMenuItem"
"T:Acme.Widget.Del"
"T:Acme.Widget.Direction"
"T:Acme.MyList`1"
"T:Acme.MyList`1.Helper`2"
*/
```

> 字段 IDs

```csharp
namespace Acme
{
 struct ValueType
    {
        private int total;
    }
    class Widget : IProcess
    {
        public class NestedClass
        {
            private int value;
        }
        private string message;
        private static Color defaultColor;
        private const double PI = 3.14159;
        protected readonly double monthlyAverage;
        private long[] array1;
        private Widget[,] array2;
        private unsafe int* pCount;
        private unsafe float** ppValues;
    }
}
/** --- IDs --------------------------------------
"F:Acme.ValueType.total"
"F:Acme.Widget.NestedClass.value"
"F:Acme.Widget.message"
"F:Acme.Widget.defaultColor"
"F:Acme.Widget.PI"
"F:Acme.Widget.monthlyAverage"
"F:Acme.Widget.array1"
"F:Acme.Widget.array2"
"F:Acme.Widget.pCount"
"F:Acme.Widget.ppValues"
*/
```

> 构造函数 IDs

```csharp
namespace Acme
{
    class Widget : IProcess
    {
        static Widget() { ... }
        public Widget() { ... }
        public Widget(string s) { ... }
    }
}
/** --- IDs --------------------------------------
"M:Acme.Widget.#cctor"
"M:Acme.Widget.#ctor"
"M:Acme.Widget.#ctor(System.String)"
*/
```

> 终结器 IDs

```csharp
namespace Acme
{
    class Widget : IProcess
    {
        ~Widget() { ... }
    }
}
/** --- IDs --------------------------------------
"M:Acme.Widget.Finalize"
*/
```

> 方法 IDs

```csharp
namespace Acme
{
    struct ValueType
    {
        public void M(int i) { ... }
    }
    class Widget : IProcess
    {
        public class NestedClass
        {
            public void M(int i) { ... }
        }
        public static void M0() { ... }
        public void M1(char c, out float f, ref ValueType v, in int i) { ... }
        public void M2(short[] x1, int[,] x2, long[][] x3) { ... }
        public void M3(long[][] x3, Widget[][,,] x4) { ... }
        public unsafe void M4(char* pc, Color** pf) { ... }
        public unsafe void M5(void* pv, double*[][,] pd) { ... }
        public void M6(int i, params object[] args) { ... }
    }
    class MyList<T>
    {
        public void Test(T t) { ... }
    }
    class UseList
    {
        public void Process(MyList<int> list) { ... }
        public MyList<T> GetValues<T>(T value) { ... }
    }
}
/** --- IDs --------------------------------------
"M:Acme.ValueType.M(System.Int32)"
"M:Acme.Widget.NestedClass.M(System.Int32)"
"M:Acme.Widget.M0"
"M:Acme.Widget.M1(System.Char,System.Single@,Acme.ValueType@,System.Int32@)"
"M:Acme.Widget.M2(System.Int16[],System.Int32[0:,0:],System.Int64[][])"
"M:Acme.Widget.M3(System.Int64[][],Acme.Widget[0:,0:,0:][])"
"M:Acme.Widget.M4(System.Char*,Color**)"
"M:Acme.Widget.M5(System.Void*,System.Double*[0:,0:][])"
"M:Acme.Widget.M6(System.Int32,System.Object[])"
"M:Acme.MyList`1.Test(`0)"
"M:Acme.UseList.Process(Acme.MyList{System.Int32})"
"M:Acme.UseList.GetValues``1(``0)"
*/
```

> 属性或索引器 IDs

```csharp
namespace Acme
{
    class Widget : IProcess
    {
        public int Width { get { ... } set { ... } }
        public int this[int i] { get { ... } set { ... } }
        public int this[string s, int i] { get { ... } set { ... } }
    }
}
/** --- IDs --------------------------------------
"P:Acme.Widget.Width"
"P:Acme.Widget.Item(System.Int32)"
"P:Acme.Widget.Item(System.String,System.Int32)"
*/
```

> 事件 IDs

```csharp
namespace Acme
{
    class Widget : IProcess
    {
        public event Del AnEvent;
    }
}
/** --- IDs --------------------------------------
"E:Acme.Widget.AnEvent"
*/
```

> 运算符 IDs

- 一元运算符的保留名称：`op_UnaryPlus`、`op_UnaryNegation`、`op_LogicalNot`、`op_OnesComplement`、`op_Increment`、`op_Decrement`、`op_True`、`op_False`。
- 二元运算符的保留名称：`op_Addition`、`op_Subtraction`、`op_Multiply`、`op_Division`、`op_Modulus`、`op_BitwiseAnd`、`op_BitwiseOr`、`op_ExclusiveOr`、`op_LeftShift`、`op_RightShift`、`op_Equality`、`op_Inequality`、`op_LessThan`、`op_LessThanOrEqual`、`op_GreaterThan`、`op_GreaterThanOrEqual`。
- 用户定义转换的保留名称：`op_Explicit`、`op_Implicit`。

```csharp
namespace Acme
{
    class Widget : IProcess
    {
        public static Widget operator +(Widget x) { ... }
        public static Widget operator +(Widget x1, Widget x2) { ... }
        public static explicit operator int(Widget x) { ... }
        public static implicit operator long(Widget x) { ... }

    }
}
/** --- IDs --------------------------------------
"M:Acme.Widget.op_UnaryPlus(Acme.Widget)"
"M:Acme.Widget.op_Addition(Acme.Widget,Acme.Widget)"
"M:Acme.Widget.op_Explicit(Acme.Widget)~System.Int32"
"M:Acme.Widget.op_Implicit(Acme.Widget)~System.Int64"
*/
```

---
## XML 文档标记元素

### 常规标记
#### summary

```xml
<summary>description</summary>
```

`<summary>` 标记中用于描述类型或类型成员，唯一表明类型的信息源。可以使用 `<remarks>` 针对某个类型说明添加补充信息，使用 `cref` 启用文档工具（如 [DocFx]() 和 [Sandcastle]()）来创建指向代码元素的文档页的内部超链接。IntelliSense 会将 `<summary>` 的信息显示在对象浏览器中。

```csharp
namespace MyNamespace
{
    /// <summary>
    /// Enter description here for class X.
    /// ID string generated is "T:MyNamespace.MyClass".
    /// </summary>
    public class MyClass { /*...*/ }
}
```

<br>

#### remarks

```xml
<remarks>
description
</remarks>
```

`<remarks>` 标记用于添加有关某个类型或某个类型成员的信息，从而补充由 `<summary>` 指定的信息，此信息显示在对象浏览器窗口中。此标记可能包含更冗长的说明。可以将 CDATA 部分用于 Markdown 可以更方便地进行编写，DocFx 等工具在 CDATA 部分中处理 Markdown 文本。



>---
### 用于成员的标记
#### returns

```xml
<returns>description</returns>
```

在方法声明的注释中使用 `<returns>` 标记来描述返回值。

```csharp
class MyClass
{
    /// <summary>
    /// MyMethod description.
    /// </summary>
    /// <returns>Default value of the integer type.</returns>
    public static int MyMethod() => default;
}
```

<br>

#### param

```xml
<param name="name">description</param>
```

在方法声明的注释中，使用 `<param>` 标记描述方法的参数，`name` 关联参数的名称，必须与 API 签名匹配。记录多个参数时使用多个 `<param>` 标记。`<param>` 标记的文本显示在 IntelliSense、对象浏览器和代码注释 Web 报表中。

```csharp
class MyClass
{
    /// <summary>
    /// Integer Addition.
    /// </summary>
    /// <param name="a">The integer, a.</param>
    /// <param name="b">The integer, b.</param>
    /// <returns>The sum of a + b</returns>
    public static int Sum(int a, int b) => a + b;
}
```

<br>

#### paramref

```xml
<paramref name="name"/>
```

`<paramref>` 标记提供一种方式，用于指示 `<summary>` 或 `<remarks>` 块等代码注释中的单词引用某个参数。可以处理 XML 文件以明显的方式设置此单词的格式，如使用粗体或斜体。

```csharp
class MyClass
{
    /// <summary>
    /// Integer Addition of <paramref name="a"/> and <paramref name="b"/>.
    /// </summary>
    /// <param name="a">The integer, a.</param>
    /// <param name="b">The integer, b.</param>
    /// <returns>The sum of <paramref name="a"/> + <paramref name="b"/></returns>
    public static int Sum(int a, int b) => a + b;
}
```

<br>

#### exception

```xml
<exception cref="member">description</exception>
```

`<exception>` 标记可用于指定可引发的异常，此标记可应用于方法、属性、事件和索引器的定义，或使用某个类型时可能产生的异常。`cref = "member"` 是对当前编译环境中出现的一个异常的引用，编译器检查是否存在给定的异常，并将 `member` 转换为输出 XML 中规范的元素名称。

```csharp
class MyClass
{
    /// <returns>The sum of <paramref name="a"/> + <paramref name="b"/></returns>
    /// <exception cref="OverflowException">OverflowException</exception>
    public static int Sum(int a, int b) => a + b;
}
```

<br>

#### value

```xml
<value>property-description</value>
```

`<value>` 标记可用于描述属性表示的值。可手动添加 `<value>` 标记，以描述属性表示的值。

```csharp
class MyClass
{
    /// <value>Integer of zero.</value>
    public static int Zero => 0;
}
```

<br>

#### permission

```xml
<permission cref="member">description</permission>
```

`<permission>` 标记成员的允许使用的安全访问范围。

```csharp
public class MyClass
{
    /// <permission cref="MyClass.Test">
    /// Allowable scope of use for inline_Fun.
    /// </permission>
    static void inline_Fun() { }

    public static void Test()
    {
         inline_Fun();
    }
}
```

>---
### 设置文档输出格式
#### para

```xml
<remarks>
    <para>
        This is an introductory paragraph.
    </para>
    <para>
        This paragraph contains more details.
    </para>
</remarks>
```

`<para>` 标记在标记内使用，以创建一个双空格段落。如果需要单空格段落，使用 `<br/>` 标记。

```csharp
namespace MyNamespace
{
    /// <summary>
    /// <br>description of Single line....</br>
    /// <br>Enter description here for class X.</br>
    /// <para>
    /// ID string generated is "T:MyNamespace.MyClass".
    /// </para>
    /// </summary>
    public class MyClass { /*...*/ }
}
```

<br>

#### list

```xml
<list type="bullet|number|table">
    <listheader>
        <term>term</term>
        <description>description</description>
    </listheader>
    <item>
        <term>Assembly</term>
        <description>The library or executable built from a compilation.</description>
    </item>
</list>
```
`<list>` 在标记中使用，其中的元素包括：
- `<list type="">` 中 `type` 指示列表使用的编号类型，可以是 `bullet`、`table`、`number` 中的一种。
- `<listheader>` 块用于定义表或定义列表的标题行，`<term>` 用以提供标题条目，`<description>` 描述标题内容。
- 列表中的每个项均使用 `<item>` 块指定。创建定义列表时，可以同时指定 `<term>` 和 `<description>`。对于表、项目符号列表或编号列表，只需提供 `<description>` 的项。
- 列表或表可根据需要具有多个 `<item>` 块。

```csharp
namespace MyNamespace
{
    /// <summary>
    /// Descriptions of type.
    /// </summary>
    /// <remarks>
    /// <para>These remarks are for the base class.</para>
    /// 
    /// <para>This information applies to all classes that derive from <see cref="BaseInheritDoc"/>:
    /// <list type="">
    ///     <item>
    ///         <term>Point #1</term>
    ///         <description>descriptions...</description>
    ///             <list type="bullet">
    ///                 <item><description>sub description #1</description></item>
    ///                 <item><description>sub description #2</description></item>
    ///             </list>
    ///     </item>
    ///     <item>
    ///         <term>Point #2</term>
    ///         <description>descriptions...</description>
    ///     </item>
    ///     <item>
    ///         <term>Point #3</term>
    ///         <description>descriptions...</description>
    ///     </item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <item><description>Point #1.</description></item>
    /// <item><description>Point #2.</description></item>
    /// <item><description>Point #3.</description></item>
    public class ListDescriptions { }
}
```

<br>

#### c

```xml
<c>text</c>
```

使用 `<c>` 标记可以指示应将说明内的文本标记为代码。使用 `<code>` 指示作为代码的多行文本。

```csharp
class MyClass
{
    /// <summary>
    /// Integer Addition of <paramref name="a"/> and <paramref name="b"/>.
    /// </summary>
    /// <returns>The sum of <c>a + b</c></returns>
    public static int Sum(int a, int b) => a + b;
}
```

<br>

#### code

```xml
<code>
    var index = 5;
    index++;
</code>
```

`<code>` 标记用于指示多行代码。使用 `<c>` 指示应将说明内的单行文本标记为代码。

```csharp
class MyClass
{
    /// <summary>
    /// Integer Addition of <paramref name="a"/> and <paramref name="b"/>.
    /// <code>
    ///     int a = 1, b = 1;
    ///     var sum = MyClass.Sum(a, b);
    /// </code>
    /// </summary>
    /// <returns>The sum of <c>a + b</c></returns>
    public static int Sum(int a, int b) => a + b;
}
```

<br>

#### example

```xml
<example>
This shows how to increment an integer.
<code>
    var index = 5;
    index++;
</code>
</example>
```

借助 `<example>` 标记，可以指定如何使用方法或其他库成员的示例。示例通常涉及到使用 `<code>` 标记。

```csharp
class MyClass
{
    /// <summary>
    /// <para>
    /// Integer Addition of <paramref name="a"/> and <paramref name="b"/>.
    /// </para>
    /// <example>
    /// This shows how to add a and b.
    /// <code>
    ///     int a = 1, b = 1;
    ///     var sum = MyClass.Sum(a, b);
    /// </code>
    /// </example>
    /// </summary>
    /// <returns>The sum of <c>a + b</c></returns>
    public static int Sum(int a, int b) => a + b;
}
```

>---
### 重用文档文本
#### inheritdoc

```xml
<inheritdoc [cref=""] [path=""]/>
```

继承基类、接口和类似方法中的 XML 注释。使用 `<inheritdoc` 不必复制和粘贴重复的 XML 注释，并自动保持 XML 注释同步。将标记 `<inheritdoc>` 添加到类型时，所有成员也将继承注释：
- `cref`：指定要继承文档的成员。当前成员上已定义的标记不会被继承的标记重写。
- `path`：将导致显示节点集的 *XPath* 表达式查询。可以使用此属性筛选要在继承文档中包含或排除的标记。

在基类或接口中添加 XML 注释，并让 `<inheritdoc>` 将注释复制到实现类中。向同步方法添加 XML 注释，并让 `<inheritdoc>` 将注释复制到相同方法的异步版本中。如果要从特定成员复制注释，可以使用 `cref` 特性来指定成员。

```csharp
/// <summary>
/// A base class from which to inherit documentation
/// </summary>
/// <remarks>
/// <para>These remarks are for the base class.</para>
///
/// <para>This information applies to all classes that derive from
/// <see cref="BaseInheritDoc"/>:
/// <list type="bullet">
/// <item><description>Point #1.</description></item>
/// <item><description>Point #2.</description></item>
/// <item><description>Point #3.</description></item>
/// </list>
/// </para>
/// </remarks>
/// <conceptualLink target="86453FFB-B978-4A2A-9EB5-70E118CA8073" />
public class BaseInheritDoc;

/// <summary>
/// This is a derived class with inherited documentation.
/// </summary>
/// <remarks>This will inherit just the last &lt;para&gt; tag from
/// the base class's &lt;remarks&gt; tag:
/// <inheritdoc path="/remarks/para[last()]" />
/// </remarks>
/// <conceptualLink target="86453FFB-B978-4A2A-9EB5-70E118CA8073" />
public class DerivedClassWithInheritedDocs : BaseInheritDoc;
```

<br>

#### include

```xml
<include file='filename' path='tagpath[@name="id"]' />
```

- `filename`：包含文档的 XML 文件的名称。可使用相对于源代码文件的路径限定文件名。
- `tagpath`：`filename` 中标记的路径，它指向标记 `name`。
- `name`：标记中的名称说明符；`name` 将有 `id`。
- `id`：标记的 `ID`。

通过 `<include>` 标记，可在其他文件中引用描述源代码中类型和成员的注释。包含外部文件是对直接在源代码文件中放入文档注释的替代方法。

通过将文档放入不同文件，可以单独从源代码对文档应用源控件。一人可以签出源代码文件，而其他人可以签出文档文件。`<include>` 标记使用 XML *XPath* 语法。

> 外部定义的 XML 文档

```xml
<!-- filename: MyDocs.xml -->
<MyDocs>
    <MyMembers name="test">
        <summary>
        The summary for this type.
        </summary>
    </MyMembers>
    <MyMembers name="test2">
        <summary>
        The summary for this other type.
        </summary>
    </MyMembers>
</MyDocs>
```

> `include` 引用外部文档注释

```csharp
/// <include file='MyDocs.xml' path='MyDocs/MyMembers[@name="test"]/*' />
class Test;

/// <include file='MyDocs.xml' path='MyDocs/MyMembers[@name="test2"]/*' />
class Test2;
```

>---
### 生成链接和引用
#### see

```xml
<see cref="member"/>
<!-- or -->
<see cref="member">Link text</see>
<!-- or -->
<see href="link">Link Text</see>
<!-- or -->
<see langword="keyword"/>
```
- `cref="member"`：对可从当前编译环境调用的成员或字段的引用。
- `href="link"`：指向给定 URL 的可单击链接。例如，`<see href="https://github.com">GitHub</see>` 生成一个可单击的链接，其中包含文本 *GitHub*，该文本链接到 `https://github.com`。
- `langword="keyword"`：语言关键字，例如 `true` 或其他有效关键字之一。

```csharp
/// <summary>
/// The example of &lt;see&gt;
/// </summary>
/// <remarks>
/// <br>see cref = <see cref="SeeExample">SeeCref</see></br>
/// <br>see href = <see href="https://github.com/JimryYchao/Learn-Programming-CSharp">Github</see></br>
/// <br>see href = <see langword="abstract" /></br>
/// </remarks>
class SeeExample;
```

<br>

#### seealso

```xml
<seealso cref="member"/>
<!-- or -->
<seealso href="link">Link Text</seealso>
```

使用 `<seealso>` 标记，可以指定想要在 “*See Also*” 部分中显示的文本。使用 `<see>` 在文本中指定链接。

```csharp
/// <summary>
/// The example of &lt;see&gt;
/// </summary>
/// <remarks>
/// <br>see cref = <see cref="SeeExample">SeeCref</see></br>
/// <para>
/// See also: <seealso href="https://github.com/JimryYchao/Learn-Programming-CSharp">Github</seealso>
/// </para>
/// </remarks>
class SeeExample;
```

>---

### 用于泛型类型和方法的标记
#### typeparam

```xml
<typeparam name="TResult">The type returned from this method</typeparam>
```

在泛型类型或方法声明的注释中，使用 `<typeparam>` 标记来描述类型参数，应为泛型类型或方法的每个类型参数添加标记。`<typeparam>` 标记的文本将显示在 IntelliSense 中。

```csharp
/// <summary>
///  Describes a lambda expression. 
/// </summary>
/// <typeparam name="TDelegate">The type parameter must be a delegate type</typeparam>
class LambdaExpression<TDelegate>;
```

<br>

#### typeparamref

```xml
<typeparamref name="TKey"/>
```

`<typeparamref>` 标记提供一种方式，用于指示 `<summary>` 或 `<remarks>` 块等代码注释中的单词引用某个类型参数。通过此标记，文档文件的使用者可显著设置字体格式，例如采用斜体。

```csharp
class LambdaExpression
{
    /// <summary>
    ///  Compile the <paramref name="expression"/> into a lambda expression
    /// </summary>
    /// <typeparam name="TDelegate">The type parameter must be a delegate type</typeparam>
    /// <returns>A <typeparamref name="TDelegate"/> containing the compiled version of the lambda.</returns>
    public static TDelegate Compile<TDelegate>(Expression<TDelegate> expression) 
        => expression.Compile();
}
```

>---

### 用户定义标记

上述所有标记均表示由 C# 编译器识别的标记，用户可以随意定义自己的标记。Sandcastle 等工具支持其他标记，例如 `<event>` 和 `<note>`，甚至支持编制命名空间文档。自定义或内部文档生成工具也可与标准标记配合使用，并支持 HTML 到 PDF 等多种输出格式。


>---
### XML 标记案例
#### 记录类和接口的层次结构

```csharp
/// <summary>
/// A summary about this class.
/// </summary>
/// <remarks>
/// These remarks would explain more about this class.
/// In this example, these comments also explain the general information about the derived class.
/// </remarks>
public class MainClass;

///<inheritdoc/>
public class DerivedClass : MainClass;

/// <summary>
/// This interface would describe all the methods in its contract.
/// </summary>
/// <remarks>
/// While elided for brevity, each method or property in this interface would contain docs that you want
/// to duplicate in each implementing class.
/// </remarks>
public interface ITestInterface
{
    /// <summary>
    /// This method is part of the test interface.
    /// </summary>
    /// <remarks>
    /// This content would be inherited by classes that implement this interface when the
    /// implementing class uses "inheritdoc"
    /// </remarks>
    /// <returns>The value of <paramref name="arg" /></returns>
    /// <param name="arg">The argument to the method</param>
    int Method(int arg);
}

///<inheritdoc cref="ITestInterface"/>
public class ImplementingClass : ITestInterface
{
    // doc comments are inherited here.
    public int Method(int arg) => arg;
}

/// <summary>
/// This class shows hows you can "inherit" the doc
/// comments from one method in another method.
/// </summary>
/// <remarks>
/// You can inherit all comments, or only a specific tag,
/// represented by an xpath expression.
/// </remarks>
public class InheritOnlyReturns
{
    /// <summary>
    /// In this example, this summary is only visible for this method.
    /// </summary>
    /// <returns>A boolean</returns>
    public static bool MyParentMethod(bool x) { return x; }

    /// <inheritdoc cref="MyParentMethod" path="/returns"/>
    public static bool MyChildMethod() { return false; }
}

/// <Summary>
/// This class shows an example of sharing comments across methods.
/// </Summary>
public class InheritAllButRemarks
{
    /// <summary>
    /// In this example, this summary is visible on all the methods.
    /// </summary>
    /// <remarks>
    /// The remarks can be inherited by other methods using the xpath expression.
    /// </remarks>
    /// <returns>A boolean</returns>
    public static bool MyParentMethod(bool x) { return x; }

    /// <inheritdoc cref="MyParentMethod" path="//*[not(self::remarks)]"/>
    public static bool MyChildMethod() { return false; }
}
```

#### 泛型类型

```csharp
/// <summary>
/// This is a generic class.
/// </summary>
/// <remarks>
/// This example shows how to specify the <see cref="GenericClass{T}"/> type as a cref attribute.
/// In generic classes and methods, you'll often want to reference the generic type, or the type parameter.
/// </remarks>
class GenericClass<T>;

/// <Summary>
/// This shows examples of typeparamref and typeparam tags
/// </Summary>
public class ParamsAndParamRefs
{
    /// <summary>
    /// The GetGenericValue method.
    /// </summary>
    /// <remarks>
    /// This sample shows how to specify the <see cref="GetGenericValue"/> method as a cref attribute.
    /// The parameter and return value are both of an arbitrary type,
    /// <typeparamref name="T"/>
    /// </remarks>
    public static T GetGenericValue<T>(T para) => para;
}
```

---
## XML 文档创建输出工具

### [DocFX](https://dotnet.github.io/docfx/docs/basic-concepts.html)

>---

### [Sandcastle](https://github.com/EWSoftware/SHFB)

>---

### [Doxygen](https://github.com/doxygen/doxygen)

---