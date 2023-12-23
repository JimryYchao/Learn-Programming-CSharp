# CSharp 特性

---
## 1. 特性简介

像类或成员的可访问性一样，C# 使用一种新的声明性信息，称为特性。可以将特性附加到各种程序实体，并在运行时环境中检索特性信息。

特性是通过特性类的声明来定义的，特性类可以有位置参数和命名参数。使用特性规范，将特性附加到 C# 程序中的实体上，并且可以在运行时作为特性实例进行检索。

>---

### 1.1. Attribute 类

直接或间接派生自抽象类 `System.Attribute` 的类被称为特性类。特性类的声明定义了一种可以在程序实体上的新特性。按照惯例，特性类以 `Attribute` 后缀命名，特性的使用可以包含或省略这个后缀。

从 C#11 开始可以声明泛型特性类。泛型特性类在标记程序成员时，必须是封闭类型（即需要提供具体类型参数）。

>---

### 1.2. 位置参数与命名参数

特性类具有位置参数和命名参数，特性类的每个公共构造函数都为该类定义了一个有效的位置参数序列。命名参数由特性类的非静态、公共和读写的字段或属性定义。

定义特性类时，可以在特性类上附加 `AttributeUsage` 来控制它的使用方式。

```csharp
[AttributeUsage(AttributeTargets.Class)]
public class AuthorAttribute(string name) : Attribute
{
    public string Name { get; } = name;  // name 位置参数初始化
    public string Version;  // 命名参数
}

[Author("Hello", Version = "1.0.0.0")]
class Sample;
```

#### 1.2.1. 特性参数类型

特性类的位置参数和命名参数的类型仅限于：
- 以下类型之一：`bool`、`char`、`byte`、`sbyte`、`short`、`ushort`、`int`、`uint`、`long`、`ulong`、`float`、`double`、`object`、`System.Type`、枚举类型。
- 上述类型的一维数组。
- 非特性参数类型的构造函数参数或公共字段不能作为特性规范中的位置参数或命名参数。

位置参数或命名参数的特性参数值不能是类型参数或开放式泛型。`Type` 的特性参数可以是未绑定的泛型类型。

```csharp
class SampleAttribute(Type type) : Attribute
{
    public Type type { get; } = type;
}
class Example<T>
{
    [Sample(typeof(Example<T>))] int v1;   //err 
    [Sample(typeof(T))] int v2;   //err 

    [Sample(typeof(Example<int>))] int v4;   
    [Sample(typeof(Example<>))] int v3;   
}
```

>---

### 1.3. 特性规范

*特性规范* 是指将特性应用到程序元素的声明。特性是为声明指定的一段附加声明性信息。可以在全局范围内（在包含它的程序集或模块上）、类型声明、类型成员声明、属性或索引器访问器声明、事件访问器声明、形式参数列表的元素、类型参数列表的元素上指定特性。

```ANTLR
global_attributes
    : [ global_attribute_target : attribute_list ]+
global_attribute_target
    : assembly  
    | module  
    ;

attributes
    : [ <attribute_target :>? attribute_list ]+
attribute_target
    : field
    | event
    | method
    | param
    | property
    | return
    | type
    | typevar  
    ;
```

每个 `attribute` 由位置参数和命名参数的可选列表组成，位置参数在前，命名参数在后。

当特性放置在全局级别时，需要指定全局属性目标标识：
- `assembly`：特性的目标中包含程序集。
- `module`：特性的目标中包含模块。

其他标准化的特性目标包含 `event`、`field`、`method`、`param`、`property`、`return`、`type`、`typevar`。这些目标只能在以下上下文中使用：
- `event`：事件。
- `field`：字段、类似字段的事件、自动实现的属性。
- `method`：构造函数、终结器、方法、运算符、属性或索引器的访问器、事件的访问器、类似字段的事件。
- `param`：属性或索引器的 `set` 访问器、事件的访问器、构造函数参数、方法参数、运算符参数。
- `property`：属性、索引器。
- `return`：委托、方法、运算符、属性或索引器的 `get` 访问器。
- `type`：委托、类、结构、枚举和接口。
- `typevar`：类型参数。

某些上下文允许在多个目标上指定一个属性，程序可以通过包含 `attribute_target` 显式地指定目标。如果没有指定特性目标，则使用默认值。例如，对于委托声明上的特性，默认目标是委托。否则，当目标是 `method`，则是委托；当目标是 `return` 则是委托的返回值。

程序元素的默认特性目标以及可以显式指定的目标声明：
- 委托默认是委托本身 `type`，可以指定 `type`、`return`。
- 方法默认是方法本身 `method`，可以指定 `method`、`return`。
- 运算符默认是运算符本身 `method`，可以指定 `method`、`return`。
- `get` 访问器默认是关联的方法 `method`，可以指定 `method`、`return`。
- `set` 访问器默认是关联的方法 `method`，可以指定 `method`、`param`。
- 自动实现的属性默认是属性 `property`，可以指定 `property`、`field`。其他属性的默认仅是 `property`。
- 字段事件默认是事件本身 `event`，可以指定 `method`、`event`、`field`。有访问器的事件默认仅是 `event`。
- 事件的访问器默认是关联的方法 `method`，可以指定 `method`、`return`、`param`。

不兼容的特性目标将会被忽略。

```csharp
[AttributeUsage(AttributeTargets.All)]
public class TargetAttribute : Attribute;

[type: Target]
class Sample<[typevar: Target] T> where T : notnull
{
    [field: Target] public T AutoValue { get; }

    public T Value
    {
        [return: Target]
        get => default!;
        [param: Target]
        set { }
    }

    [field: Target] event Action fieldEvent;

    event Action accessorEvent
    {
        [return: Target]
        add { }
        [method: Target]
        remove { }
    }
}
```

#### 1.3.1. 命名规范与歧义

按照惯例，特性类的命名以 `Attribute` 作为后缀。使用特性时，后缀 `Attribute` 可以包含或省略。当特性名称前附加 `@` 时，无后缀的名称可能无法被编译器识别，编译器会将该命名视为 *type_name* 并查找完整名称下的特性类。

```csharp
class SampleAttribute : Attribute;

[Sample] 
class ExampleA;  // refer to SampleAttribute

[SampleAttribute] 
class ExampleB;  // refer to SampleAttribute

[@Sample] 
class ExampleC;  // err: no attribute
```

如果一个特性类省略后缀的名称是一个类型或另一个特性类（未使用命名惯例），则会发生歧义。此时 `@` 逐字标识会很有用。

```csharp
class Sample : Attribute;
class SampleAttribute : Attribute;

[Sample] 
class ExampleA;  // 歧义

[SampleAttribute] 
class ExampleB;  // refer to SampleAttribute

[@Sample] 
class ExampleC;  // refer to Sample

[@SampleAttribute] 
class ExampleD;  // refer to SampleAttribute
```

>---

### 1.4. 特性实例

特性实例是在运行时表示特性的实例。一个特性由特性类、位置参数和命名参数定义。特性实例是使用位置参数和命名参数初始化的特性类的实例。

特性实例的检索涉及编译时和运行时处理。

#### 1.4.1. 特性的编译

编译一个在程序实体 `E` 上指定的特性，包含一个特性类 `T`、位置参数列表 `P` 和命名参数列表 `N`，通过以下步骤编译成程序集 `A`：
- 按照编译时处理步骤编译形式为 `new T(P)` 的对象创建表达式。这些步骤要么导致编译时错误，要么确定可以在运行时调用的实例构造函数 `C`。
- 如果 `C` 没有公共可访问性，则会发生编译时错误。
- 对于命名参数列表的每个参数 `Arg`，在特性类 `T` 中查找以 `Arg` 的标识符为名称的非静态公共的读写属性或字段。若未找到，则发生编译时错误。
- 如果位置参数列表或命名参数列表中的某个参数的类型是 `string`，并且参数的值不是 Unicode 标准定义的格式良好的，则编译的值是否等于检索的运行时值是实现定义的。例如，一个字符串如果包含高代理的 UTF-16 代码单元，而后面没有紧跟一个代理的代码单元，那么这个字符串就不是格式良好的。
- 将以下信息（用于特性的运行时实例化）作为编译包含该特性的程序的结果存储在编译器的汇编输出中：特性类 `T`、`T` 上的实例构造函数 `C`、位置参数列表 `P`、命名参数列表 `N` 和相关的程序实体 `E`，其值在编译时完全解析。

#### 1.4.2. 运行时检索特性实例

由 `T`、`C`、`P`、`N` 表示并与 `E` 相关联的特性实例可以在运行时通过以下步骤从程序集 `A` 中检索：
- 遵循执行 `new T(P)` 的形式的对象构造表达式的运行时处理步骤，使用实例构造函数 `C` 和编译时确定的值。这些步骤要么导致异常，要么产生 `T` 的实例 `O`。
- 对于 `N` 中的每个参数 `Arg`，按顺序：
  - 使用 `Arg` 的标识符 `Name`，在 `O` 上查找标识为 `Name` 的非静态公共读写字段或属性，并将 `Arg` 的值赋值查找的结果。若未查找到这样名称的字段或属性，则抛出异常。
  - 结果是特性类 `T` 的一个实例 `O`，且使用 `N` 和 `P` 进行初始化。 

```csharp
[AttributeUsage(AttributeTargets.Class)]
public class HelpAttribute(string url) : Attribute
{
    public string Topic { get; set; }
    public string Url { get; } = url;
}

[Help("https://github.com/dotnet", Topic = "Help")]
public sealed class InterrogateHelpUrls
{
    public static void Main(string[] args)
    {
        Type helpType = typeof(HelpAttribute);

        var attributes = typeof(InterrogateHelpUrls).GetCustomAttributes(helpType, false);
        var helpers = (HelpAttribute[])attributes;
        foreach (var helper in helpers)
        {
            Console.WriteLine($"Url : {helper.Url}");
            Console.WriteLine($"Topic : {helper.Topic}");
        }
    }
}
```

---
## 2. 保留特性

少数特性以某种方式影响语言。这些特性包括：
- `System.AttributeUsageAttribute`：用于描述使用特性类的方式。
- `System.Diagnostics.ConditionalAttribute`：指示编译器应忽略方法调用或属性，除非定义了指定的条件编译符号。
- `System.ObsoleteAttribute`：用于将成员标记为过时的。
- `System.Runtime.CompilerServices.CallerLineNumberAttribute`、`CallerFilePathAttribute`、`CallerMemberNameAttribute`：用于向可选参数提供有关调用上下文的信息。

执行环境可以提供额外的特定于实现的特性，这些特性会影响 C# 程序的执行。

>---

### 2.1. AttributeUsage 特性使用范围

特性 `AttributeUsage` 用于描述可以使用特性类的方式。

```csharp
public sealed class AttributeUsageAttribute : Attribute
{
    public AttributeUsageAttribute(AttributeTargets validOn);

    public bool AllowMultiple { get; set; }
    public bool Inherited { get; set; }
    public AttributeTargets ValidOn { get; }
}
```

- `AllowMultiple` 指定是否可以为给定的程序元素多次放置指示特性。
- `Inherited` 指定指示特性是否可以由派生类和重写成员继承。
- `ValidOn` 指定可以放置指示特性的程序实体元素，可以 `OR` 组合多个元素的集合。

```csharp
[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Struct,
    Inherited = false,          // 无法被派生类继承
    AllowMultiple = true
)]
public class AuthorAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}

[Author("Hello"), Author("World")]
class Base;

class Derived : Base
{
    static void Main(string[] args)
    {
        Type b = typeof(Base);
        Console.WriteLine(b.CustomAttributes.Count());  // 2
        Type t = typeof(Derived);
        Console.WriteLine(t.CustomAttributes.Count());  // 0, Author 特性无法被派生类继承
    }
}
```

未指定的用户定义特性类的默认 `AttributeUsage` 为：

```csharp
[AttributeUsage(AttributeTargets.All,AllowMultiple = false,Inherited = false)]
```

>---

### 2.2. Conditional 条件编译

`Conditional` 指示编译器应忽略方法调用或属性，除非定义了指定的条件编译符号。可以声明条件方法和条件特性类。

```csharp
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class ConditionalAttribute : Attribute
{
    public ConditionalAttribute(string conditionString);
    public string ConditionString { get; }
}
```

> 条件方法

如果在调用点定义了一个或多个与条件方法相关联的条件编译符号，则包含对条件方法的调用，否则省略该调用。

条件方法必须是结构或类声明中的方法，返回类型为 `void`。条件方法可以是虚方法（`virtual` 或  `abstract`），被 `override` 的条件方法是隐式条件特性。`Conditional` 不能修饰 `override` 方法。

条件方法的参数不能包含修饰符，且无法从条件方法创建委托。

```csharp
#define Condition
class Sample
{
    [System.Diagnostics.Conditional("Condition")]
    public static void Fun() => Console.WriteLine("Invoke Fun()");
    static void Main(string[] args)
    {
        Fun();
    }
}
```

对条件方法调用的包含或排除是由调用点的条件编译符号控制的。
 
```csharp
//#define Condition
class Sample
{
    [System.Diagnostics.Conditional("Condition")]
    public static void Fun() => Console.WriteLine("Invoke Fun()");
    static void Main(string[] args)
    {
        Fun();  // 忽略调用
        var t = typeof(Sample);
        var fun = t.GetMethod("Fun");
        if(fun is not null)
            fun.Invoke(null, null);  
    }
}
```

> 条件特性类

可以声明条件编译关联的特性类。如果在规范点定义了一个或多个与条件特性相关联的条件编译符号，则包含对该特性类的特性规范，否则省略该特性。

```csharp
#define Condition
using System.Diagnostics;
[AttributeUsage(AttributeTargets.Class)]
[Conditional("Condition")]
class SampleAttribute(string mess) : Attribute
{
    public string Message => mess;
}

[Sample("Hello World")]
class Example
{
    static void Main(string[] args)
    {
        var t = typeof(Example);
        var atts = t.GetCustomAttributes(typeof(SampleAttribute), false);
        foreach (var att in atts)
        {
            if (att is SampleAttribute sa)
                Console.WriteLine(sa.Message);
        }
    }
}
```

>---

### 2.3. Obsolete 过时

特性 `Obsolete` 用于标记不应再使用的类型和类型成员。如果程序使用了用 `Obsolete` 属性修饰的类型或成员，编译器将发出警告或错误。

```csharp
public sealed class ObsoleteAttribute : Attribute
{
    public ObsoleteAttribute();
    public ObsoleteAttribute(string? message);
    public ObsoleteAttribute(string? message, bool error);

    public string? DiagnosticId { get; set; }
    public bool IsError { get; }
    public string? Message { get; }
    public string? UrlFormat { get; set; }
}
```

- `DiagnosticId` 获取或设置编译器在报告 API 使用情况时将使用的 ID。
- `IsError` 指示编译器是否将使用过时的程序元素视为错误。
- `Message` 获取解决方法消息。
- `UrlFormat` 获取或设置相应文档的 URL。

```csharp
class Sample
{
    [Obsolete("Use fun instead of ObsoleteFun", true)]
    public static void ObsoleteFun() => Console.WriteLine("ObsoleteFun");
    public static void Fun() => Console.WriteLine("Fun");

    static void Main(string[] args)
    {
        //ObsoleteFun();
        Fun();
    }
}
```

>---

### 2.4. Caller-info

对于诸如日志记录和报告之类的目的，函数成员获取关于调用代码的某些编译时信息有时是有用的。*Caller-info* 属性提供了一种透明传递此类信息的方法。

当使用一个 *Caller-info* 特性注释可选参数时，在调用中省略相应的实参并不一定会导致默认参数值被替换。如果有关调用上下文的指定信息可用，则该信息将作为参数值传递。

`CallerArgumentExpressionAttribute(string parameterName)` 指示参数将为另一个参数传递的表达式捕获为字符串。
`CallerFilePathAttribute` 允许获取包含调用方编译时的源文件的完整路径。
`CallerLineNumberAttribute` 允许获取源文件中调用方法的行号。
`CallerMemberNameAttribute` 允许获取方法调用方的方法或属性名称。

```csharp
using System.Runtime.CompilerServices;

class Sample
{
    static void Expression(object expr,
        [CallerArgumentExpression("expr")] string arg = "",
        [CallerFilePath] string path = "",
        [CallerLineNumber] int line = -1,
        [CallerMemberName] string caller = "")
    {
        Console.WriteLine(
            $"The expression {arg} = {expr}\n " +
                  $"    at {caller} in {path}:{line}" );
    }
    static void Main(string[] args)
    {
        Expression(25 * 25);
    }
}
```

在分部方法声明的定义部分和实现部分的参数上使用相同的 *Caller-info* 特性是错误的。只应用定义部分中的调用者信息属性，而只出现在实现部分中的调用者信息属性将被忽略。

调用方信息不影响重载解析。由于调用者的源代码中仍然省略了带有特性的可选参数，因此重载解析会以忽略其他省略的可选参数的相同方式忽略这些参数。

只有在源代码中显式调用函数时才替换调用方信息。隐式调用（如隐式父构造函数调用、委托调用）没有源位置，也不会替换调用方信息。动态绑定的调用不会替代调用方信息。在这种情况下，如果省略了 *Caller-info* 特性参数，则使用该参数的指定默认值。

一个例外是查询表达式。这些被认为是语法展开，如果它们展开的调用省略了带有 *Caller-info* 属性的可选参数，则调用方信息将被替换。所使用的位置是生成调用的查询子句的位置。

```csharp
using System.Runtime.CompilerServices;
partial class Sample
{
    static partial void Expression(object expr,
         [CallerArgumentExpression("expr")] string argu = "",
         [CallerFilePath] string path = "",
         [CallerLineNumber] int line = -1,
         [CallerMemberName] string caller = "");
}
partial class Sample
{
    // 分部
    static partial void Expression(object expr, string argu, string path, int line, string caller)
    {
        Console.WriteLine(
            $"The expression {argu} = {expr}\n " +
                  $"    at {caller} in {path}:{line}");
    }
    static void Main(string[] args)
    {
        // 隐式调用
        var ac = Expression;
        ac.Invoke(25 * 25);
    }
}
```

当 *Call-info* 特性组合放置到同一对象，其中的一个特性有效，而组合中的其他 *Call-info* 特性会被忽略。优先级是 `CallerLineNumber` > `CallerFilePath` > `CallerMemberName` > `CallerArgumentExpression`。

```csharp
[CallerMemberName, CallerFilePath, CallerLineNumber] object p = ...
```

---
## 3. 语言特性

### 3.1. Flags 枚举位标志

枚举支持按位运算，可以使用 `FlagsAttribute` 指示枚举，将其视为位字段（一组标志，位标志枚举）。`Flags` 特性会影响枚举类型的 `ToString()` 和 `Parse` 方法的行为。

```csharp
class Sample
{
    enum SingleHue
    {
        None = 0,
        Black = 1,
        Red = 1 << 1,
        Green = 1 << 2,
        Blue = 1 << 3,
    }
    [Flags]
    enum MultiHue
    {
        None = 0,
        Black = 1,
        Red = 1 << 1,
        Green = 1 << 2,
        Blue = 1 << 3,
    }
    static void Main(string[] args)
    {
        // Display all possible combinations of values.
        Console.WriteLine(
             "All possible combinations of values without FlagsAttribute:");
        for (int val = 0; val <= 16; val++)
            Console.WriteLine("{0,3} - {1:G}", val, (SingleHue)val);

        // Display all combinations of values, and invalid values.
        Console.WriteLine(
             "\nAll possible combinations of values with FlagsAttribute:");
        for (int val = 0; val <= 16; val++)
            Console.WriteLine("{0,3} - {1:G}", val, (MultiHue)val);
    }
}
/**
 All possible combinations of values without FlagsAttribute:
  0 - None
  1 - Black
  2 - Red
  3 - 3
  4 - Green
  5 - 5
  6 - 6
  7 - 7
  8 - Blue
  9 - 9
 10 - 10
 11 - 11
 12 - 12
 13 - 13
 14 - 14
 15 - 15
 16 - 16

All possible combinations of values with FlagsAttribute:
  0 - None
  1 - Black
  2 - Red
  3 - Black, Red
  4 - Green
  5 - Black, Green
  6 - Red, Green
  7 - Black, Red, Green
  8 - Blue
  9 - Black, Blue
 10 - Red, Blue
 11 - Black, Red, Blue
 12 - Green, Blue
 13 - Black, Green, Blue
 14 - Red, Green, Blue
 15 - Black, Red, Green, Blue
 16 - 16
 */
```

>---

### 3.2. SetsRequiredMembers 构造函数

具有必需成员的类型中的所有构造函数，或其基类型指定必需成员的类型中的所有构造函数，在调用该构造函数时必须具有由使用者设置的这些成员。为了使构造函数不受此要求的约束，可以使用 `SetsRequiredMembersAttribute` 对构造函数进行特性标记，从而消除这些要求。编译器不会对该构造函数体进行验证。

`SetsRequiredMemberAttributer` 从构造函数中删除所有需求，并且不以任何方式检查这些需求的有效性。如果需要从具有无效必需成员列表的类型继承，也必需用 `SetsRequiredMembersAttribute` 标记该派生类型的构造函数。

如果构造函数 `C` 链接到一个带有 `SetsRequiredMembersAttribute` 特性标记的 `base` 或 `this` 构造函数，则 `C` 也必须带有 `SetsRequiredMembersAttribute` 特性。

对于记录类型，如果记录类型或其任何基类型具有所需的成员，编译器自动在记录的复制构造函数上标记 `SetsRequiredMembersAttribute`。

```csharp
using System.Diagnostics.CodeAnalysis;
class Sample()
{
    public required string Name { get; set; }

    [SetsRequiredMembers]
    public Sample(string name) : this()=> Name = name;
}
class Derived : Sample
{
    [SetsRequiredMembers]
    public Derived(string name) : base(name) { }

    static void Main(string[] args)
    {
        Sample s = new Sample() { /*Name = "Hello"*/ };  // err
        Derived d = new Derived("Hello");  
    }
}
```

---
## 4. 互操作特性

### 4.1. IndexerName 索引器名称

`IndexerNameAttribute(string indexerName)` 用于为索引器显式指定底层关联属性的名称，默认使用名称 `Item`。

```csharp
using System.Runtime.CompilerServices;
class Sample
{
    [IndexerName("TheItem")]
    public int this[int index] { ... }
}
```

---
## 5. .NET 运行时的特性支持

### 5.1. ModuleInitializer 模块初始化器

尽管 .NET 平台有一个直接支持为程序集（技术上讲，是 `Module`）编写初始化代码的特性，但它在 C# 中没有公开。为了使库 DLL 能够在加载时立即进行一次性初始化，而用户无需显式调用任何东西。当前静态构造函数的一个缺陷在于，运行时必须对带有静态构造函数的类型的使用情况进行额外的检查，以便决定是否需要运行静态构造函数，这增加了明显的开销。

模块初始化器使源代码生成器能够运行一些全局初始化逻辑，而不需要用户显式调用任何东西。通过使用 `ModuleInitializerAttribute` 特性标记一个方法，将其指定为模块初始化器。

```csharp
using System.Runtime.CompilerServices;
class ModuleInitializer
{
    [ModuleInitializer]
    internal static void InitModules()
    {
        // ...
    }
}
```

构成模块初始化器的方法具有一些限制：
- 该方法是无参静态的无返回的普通方法。
- 该方法不能是泛型的，也不能包含在泛型类型中。
- 该方法必须可以从包含该方法的模块中访问，即它必须是 `internal` 或 `public`。因此它不能是静态局部函数。


模块初始化保证除了适用于所有类型初始化器的保证之外，CLI 还应该为模块初始化器提供以下保证：
- 模块初始化器在第一次访问任何静态字段或第一次调用模块中定义的任何方法时或之前执行。
- 除非被用户代码显式调用，否则模块初始化器应该对任何给定模块只运行一次。
- 除了那些从模块初始化器直接或间接调用的方法外，没有任何方法能够在模块的初始化器完成执行之前访问模块中的类型、方法或数据。

当在编译中找到一个或多个具有此属性的有效方法时，编译器将发出一个模块初始化项，该初始化项调用每个带有属性的方法。调用将以保留但确定的顺序发出。若模块初始化项的包含类型具有静态构造函数，则在调用模块初始化项之前预先调用它的静态构造函数。

```csharp
using System.Runtime.CompilerServices;
class ModuleInitializer
{
    static ModuleInitializer()
    {
        Console.WriteLine("Module Initializer");
    }
    [ModuleInitializer]
    public static void ModuleFun1() => Console.WriteLine("ModuleFun1");
    [ModuleInitializer]
    public static void ModuleFun2() => Console.WriteLine("ModuleFun2");
}
public class Sample
{
    static Sample()
    {
        Console.WriteLine("Sample");
    }
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World");
    }
}
/** Output : 
        Module Initializer    // 在 Main 之前调用
        ModuleFun1
        ModuleFun2
        Sample
        Hello, World
 */
```

### 5.2. SkipLocalsInit 抑制 `.locals init` 标志发出

根据 CLR 规范，不包含引用的局部变量不会被 VM / JIT 初始化为特定的值。在没有初始化的情况下从这些变量中读取数据是类型安全的，但除此之外，行为是未定义的，并且是特定于实现的。通常，未初始化的局部变量包含当前被堆栈帧占用的内存中剩余的任何值。这可能导致不确定的行为和难以重现的 bug。

可以通过两种方式 “赋值” 一个局部变量：
- 通过存储一个值；
- 通过指定 `.locals init` 标志，强制从本地内存池分配的所有内容进行零初始化。这包括局部变量和 `stackalloc` 数据。

默认情况下，C# 编译器会在所有声明局部变量的方法上发出 `.locals init` 标志，以指示 JIT 生成一个 prolog，以将所有的局部变量进行零初始化。这种默认行为是安全的，实际上默认情况下，编译器也不会允许用户直接使用未初始化的局部变量。在高性能程序中，强制零初始化的代价是显而易见的，尤其是使用 `stackalloc` 时（不会清理堆栈）。

```csharp
static unsafe void DemoZeroing()
{
    int i;
    Console.WriteLine(*&i);
    // 局部变量默认被零初始化
}
// ========== IL ========== 
.method public hidebysig static 
	void DemoZeroing () cil managed 
{
	// Method begins at RVA 0x20a4
	// Header size: 12
	// Code size: 12 (0xc)
	.maxstack 1
	.locals init (        // .locals init 指令
		[0] int32 i
	)

	IL_0000: nop
	IL_0001: ldloca.s 0
	IL_0003: conv.u
	IL_0004: ldind.i4
	IL_0005: call void [System.Console]System.Console::WriteLine(int32)
	IL_000a: nop
	IL_000b: ret
} // end of method Sample::DemoZeroing
```

从 C#9 开始，可以使用 `System.Runtime.CompilerServices.SkipLocalsInit` 特性，指示编译器在生成元数据时抑制发出 `.locals init` 标志。该特性可以应用于成员函数、类型、或模块级别。

```csharp
// For the project
[module: System.Runtime.CompilerServices.SkipLocalsInit]

// For a class
[System.Runtime.CompilerServices.SkipLocalsInit]
class Sample;

// For a method
[System.Runtime.CompilerServices.SkipLocalsInit]
void Sample() { }
```

抑制 `.locals init` 标志的方法调用。将使用 `.locals` 替换 `.locals init` 指令，即意味着 JIT 不会对该方法的局部变量进行自动零初始化。

```csharp
[System.Runtime.CompilerServices.SkipLocalsInit]
static unsafe void DemoZeroing()
{
    int i;
    Console.WriteLine(*&i); // Unpredictable output as i is not initialized
}
// ========== IL ========== 
.method public hidebysig static 
	void DemoZeroing () cil managed 
{
	.custom instance void [System.Runtime]System.Runtime.CompilerServices.SkipLocalsInitAttribute::.ctor() = (
		01 00 00 00
	)
	// Method begins at RVA 0x20a4
	// Header size: 12
	// Code size: 12 (0xc)
	.maxstack 1
	.locals (               // .locals 替换了 .locals init 指令
		[0] int32 i
	)

	// {
	IL_0000: nop
	// Console.WriteLine(num);
	IL_0001: ldloca.s 0
	IL_0003: conv.u
	IL_0004: ldind.i4
	IL_0005: call void [System.Console]System.Console::WriteLine(int32)
	// }
	IL_000a: nop
	IL_000b: ret
} // end of method Sample::DemoZeroing
```

> 使用该特性是否意味着安全

C# 编译器确保在初始化变量之前不会使用它，因此大多数情况下使用该特性是安全的。有一些例外情况需要手动审查（`unsafe` 代码、`stackalloc`、P/Invoke、具有显式布局的结构）

- 在不安全代码中，可以利用指针使用未初始化的变量。在使用前应检查指针指向地址的变量值，以确保后续代码不会依赖于变量隐式初始化为其默认值的事实，在必要时，可以手动初始化该变量。

  ```csharp
  [SkipLocalsInit]
  static unsafe void Pointer()
  {
      int i;
      int* pointer_i = &i; // ⚠ The value of i is not initialized to 0
  
      int j = 0;
      int* pointer_j = &j; // ok
  }
  ```
- 从 C#8 开始，可以不再不安全的上下文中使用 `stackalloc`（`Span<T>` 和 `ReadOnlySpan<T>`）。

  ```csharp
  using System.Runtime.CompilerServices;
  [module: SkipLocalsInit]
  
  Span<MyStruct> array = stackalloc MyStruct[10];
  array[0].Field1 = 42;           // ⚠ Other fields are uninitialize which could be problematic
  Console.WriteLine(array[0].Field2); // ⚠ Unpredictable output as Field2 is not initialized
  
  array[1] = new MyStruct { Field1 = 42 }; // Ok as the ctor will initialize the values correctly
  
  struct MyStruct
  {
      public int Field1;
      public int Field2;
  }
  ```

- 如果调用包含 `out` 参数的 P/Invoke 方法，则应确保本机方法总是在期望初始化值的情况下写入值。

  ```csharp
  int a;
  NativeMethods.Sample(out a); // ⚠ Be sure that Sample writes the out parameter in any case where you need it
  Console.WriteLine(a); // ⚠ Unpredictable output if Sample doesn't set the value of the variable
  ```

- 如果使用 `StructLayout` 属性设置的大小大于字段的大小，则结构的一部分可能不会被初始化。

  ```csharp
  using System.Runtime.CompilerServices;
  using System.Runtime.InteropServices;
  
  [module: SkipLocalsInit]
  
  [StructLayout(LayoutKind.Sequential, Size = 8)]
  struct Sample
  {
      public int A;
      public Sample(int value)
      {
          A = value;
          // ⚠ Only the first 4 bytes are initialized. The 4 last bytes are not initialized.
      }
      unsafe static void Main(string[] args)
      {
          Sample s = new Sample(10010);
          int* pFirst = (int*)(void*)&s;
          Console.WriteLine(*pFirst);
          Console.WriteLine(*(pFirst + 1));  // 结构的其他部分没有被零初始化
      }
  }
  ```

- 如果在设置显式布局时，结构体中存在漏洞，则结构体的一部分可能不会初始化：

  ```csharp
  using System.Runtime.CompilerServices;
  using System.Runtime.InteropServices;
  
  [module: SkipLocalsInit]
  
  [StructLayout(LayoutKind.Explicit)]
  struct Sample
  {
  
      [FieldOffset(0)] public int A; // 0-3
      // There is a 4 bytes hole in the struct layout (4-7)
      [FieldOffset(8)] public int B; // 8-12
  
      public Sample(int a, int b)
      {
          A = a;
          B = b;
          // ⚠ Bytes 4 to 7 are not initialized
      }
      unsafe static void Main(string[] args)
      {
          Sample s = new Sample(10010, 10086);
          int* pFirst = (int*)(void*)&s;
          Console.WriteLine(*pFirst);        // 0-3   // 10010
          Console.WriteLine(*(pFirst + 1));  // 4-7   // -842150451 or other
          Console.WriteLine(*(pFirst + 2));  // 8-12  // 10086
      }
  }
  ```

>---

### 5.3. CollectionBuilder 集合生成器


---
## 6. 代码分析特性

### 6.1. 可空特性

在很多情况下，明确告诉编译器你需要处理空值，并需要为此添加一些防护措施，比笼统地关闭空值功能或者关闭空值警告更有意义。若
要实现这一点，可以将一些元数据作为特性包含在代码中：
- `AllowNull`：不可空输入参数可能为 null。它仅对属性、索引器、字段或参数有效。
- `DisallowNull`：可空输入参数永远不会为 null。它仅对属性、索引器、字段或参数有效。
- `MaybeNull`：不可空返回值可能为 null。它仅对属性、索引器、字段、参数、返回有效。
- `NotNull`：可空返回值永远不为 null。它仅对属性、索引器、字段、参数、返回有效。
- `MaybeNullWhen`：当满足条件时，一个不可空输入参数可能为 null。它仅对参数有效。
- `NotNullIfNotNull`：如果实参对于指定的形参不为 null，那么返回值不为 null。它仅对属性、索引器、参数、返回有效。

有时候将数据定义为可空或不可空并不能提高程序的健壮性，可以使用可空特性对方法的输入数据或输出数据进行描述。

```csharp
using System.Diagnostics.CodeAnalysis;
class Sample
{
    static bool TryGetDigitAsText(char number, [NotNullWhen(true)] out string? text)
    {
        return (text = number switch
        {
            '0' => "zero",
            '1' => "one",
            '2' => "two",
            '3' => "three",
            '4' => "four",
            '5' => "five",
            '6' => "six",
            '7' => "seven",
            '8' => "eight",
            '9' => "nine",
            _ => null
        }) is string;
    }
    [return: NotNullIfNotNull("text")]
    public static string? TryGetDigitsAsText(string? text)
    {
        if(text is null) return null;
        string rt = "";
        foreach(char c in text) {
            if(TryGetDigitAsText(c, out string? digitText))
            {
                if (rt != "") rt += '-';
                rt += digitText.ToLower(); 
            }
        }
        return rt;
    }
    static void Main(string[] args)
    {
        string t = "132qwe13qwerte31"; // or null
        string rt = TryGetDigitsAsText(t);
        Console.WriteLine(rt.ToLower());
    }
}
```

在泛型编程中，常常标记类型参数是可空类型。由于 `Nullable<T>` 可空值类型与可空引用类型是完全不同的数据类型，声明泛型类型的可空注释（`T?`），则无法判断该泛型类型是值类型或引用类型。可以利用可空特性抑制编译器的空检查警告。

```csharp
class BaseClass
{
    [return: MaybeNull]
    public virtual T Fun<T>([AllowNull] T? t) => t;
}
class DerivedClass : BaseClass
{
    [return: MaybeNull]
    public override T Fun<T>(T? t) where T : default => base.Fun(t);
}
```

>---
### 6.2. Experimental 实验性 API（C#12）

`ExperimentalAttribute` 特性可以应用于程序集中的任何内容，以表明类型、成员或程序集是实验性的。当任何用户使用被标记的目标时，编译器会在调用点发出一个诊断错误，以表明用户正在使用一个将来可能会改变或被删除的内容。

```csharp
using System.Diagnostics.CodeAnalysis;

[Experimental("experimental")]
static void ExperimentalMethod()
{
    Console.WriteLine("ExperimentalMethod");
}
static void Test()
{
    ExperimentalMethod(); // Compiler error here
}
```

该错误可以通过将自己的代码标记为 `[Experimental]` 来绕过诊断，或者通过编译器 `#pragma warning disable 'diagnosticId'` 显式允许实验代码通过编译：

```csharp
using System.Diagnostics.CodeAnalysis;

[Experimental(diagnosticId: "Experimental", UrlFormat = "https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.codeanalysis.experimentalattribute")]
static void ExperimentalMethod()
{
    Console.WriteLine("ExperimentalMethod");
}
static void Test()
{
#pragma warning disable Experimental
    ExperimentalMethod(); // This now compiles, thanks to the pragma warning disable directives.
#pragma warning restore Experimental
}
```

---
## 7. 程序集特性


## 8. 编辑器特性

```System.Runtime.CompilerServices.CompilationRelaxationsAttribute```