# CSharp 语言介绍

```csharp
using System;
class Hello{
    static void Main(){
        Console.WriteLine("Hello World!");
    }
}
```

---
## C# 语言简介

C# 源于 C 语言系列，是一种面向对象的、面向组件的、类型安全的新式编程语言。CSharp 作为 .NET Framework 计划的一部分，微软于 2000 年 7 月发布了第一个 C# 实现版本。

随着 C# 的发展，它的主要设计目的在于：C# 旨在成为一种简单、现代、通用型、面向对象的编程语言，用于开发适合在分布式环境中部署的软件组件。C# 也适合为托管和嵌入式系统编写应用程序，范围从使用复杂操作系统的大型应用程序，到具有专用功能的小型应用程序。C# 应用程序在内存和处理能力需求方面力求经济，但是并不打算在性能和大小上与 C 或汇编语言直接竞争。

---
## .NET 体系结构

C# 程序在 .NET 上运行，而 .NET 是名为公共语言运行时（Common Language Runtime，CLR）的虚拟执行系统和一组类库。CLR 是 Microsoft 对公共语言基础结构（CLI）国际标准的实现。CLI 是创建执行和开发环境的基础，语言和库可以在其中无缝地协同工作。

C# 源代码被编译成符合 CLI 规范的中间语言（IL）。IL 代码和资源（如位图和字符串）存储在扩展名通常为 `.dll` 的程序集中。程序集包含一个介绍程序集的类型、版本和区域性的清单。

执行 C# 程序时，程序集将加载到 CLR。CLR 会直接执行实时（JIT）编译，将 IL 代码转换成本机指令。CLR 可提供自动垃圾回收、异常处理和资源管理相关的服务。CLR 执行的代码有时称为 “托管代码”，而 “非托管代码” 被编译成面向特定平台的本机语言。

语言互操作性是 .NET 的一项重要功能。C# 编译器生成的 IL 代码符合公共类型规范（CTS）。通过 C# 生成的 IL 代码可以与通过 .NET 版本的 F#、Visual Basic、C++ 生成的代码进行交互。还有 20 多种与 CTS 兼容的语言。单个程序集可包含多个用不同 .NET 语言编写的模块。这些类型可以相互引用，就像它们是用同一种语言编写的一样。

.NET 还包含大量库，这些库的功能包括文件输入输出、字符串控制、XML 分析、Web 应用程序框架和 Windows 窗体控件。典型的 C# 应用程序广泛使用 .NET 类库来处理常见的 “管道” 零碎工作。

---
## 术语与定义

术语与定义参考 [ISO/IEC 2382:2022](https://standards.iso.org/ittf/PubliclyAvailableStandards/)。

- **application** – 应用程序：具有入口点的程序集。
- **application domain** – 应用程序域：提供一种边界，用以隔离保存不同应用程序的状态。

- **behavior** – 行为：外在的外观或行为。
- **behavior, implementation-defined** – 实现定义行为：定义某种行为的规范和文档支持，并明确该行为的实现定义。
- **behavior, undefined** – 未定义行为：在使用了不可移植的、或错误的构造、或错误的数据时的行为，没有该行为的定义规定。
- **behavior, unspecified** – 不明确行为：可能存在两种或以上可能性的行为，并且在任何情况下都没有对所选择的行为施加进一步的要求。

- **assembly** – 程序集：由编译器作为程序编译结果输出的一个或多个文件。
- **class library** – 类库：可由其他程序集使用的程序集。
- **module** – 模块：由编译器生成的程序集的内容。一些实现可能具有生成包含多个模块的程序集的功能。

- **compilation unit** – 编译单元：输入到编译器的 Unicode 字符的有序序列。
- **implementation** – 执行：一组特定的软件（在特定的控制选项下运行在特定的翻译环境中），它为特定的执行环境执行程序的翻译，并支持方法的执行。
- **program** – 程序：呈现给编译器并由执行环境运行或执行的一个或多个编译单元。
- **namespace** – 命名空间：将相关程序元素分组的逻辑组织系统。

- **character** – 字符：在非 Unicode 编码上下文中，指该编码中字符的含义；或在字符字面量或 `char` 类型值的上下文中，`U+0000` 到 `U+FFFF` 范围内的 Unicode 码点（包括替代码点），即 UTF-16 代码单元；或仅表示为 Unicode 码点。
- **argument** – 实参：需要将实参的值或引用传递给方法、构造函数或索引器等的形参，可是传递变量或者是表达式的值。
- **parameter** – 形参：作为方法、实例构造函数、操作符或索引器定义的一部分声明的变量，用于传递变量的值给函数成员。
- **unsafe code** – 不安全代码：允许执行诸如声明和操作指针、在指针和整型之间进行转换以及获取变量地址等较低级操作的代码。

- **diagnostic message** – 诊断讯息：消息输出系统的实现定义的子集。
- **warning, compile-time** – 编译时警告：在程序翻译期间报告的信息性消息，其目的是识别程序元素的潜在可疑用法。
- **error, compile-time** – 编译时错误：程序编译时中报告的错误。
- **exception** – 异常：程序执行期间报告的异常情况

---
## C# 词法结构

### C# Programs

C# 程序由一个或多个源文件组成，称之为编译单元（compilation unit）。C# 主要使用两个语法来构成 C# 程序：词法语法（Lexical grammar）定义了如何组合 Unicode 字符以表示行结束、空白、注释、标记和预处理指令。句式语法（Syntactic grammar）定义了如何将词汇语法产生的标记组合形成 C# 程序。

C# 的程序编译分为三个步骤：

  1. 转换（Transformation）：将源文件从特定的字符表和编码方案转换为 Unicode 字符序列（可以接受 UTF-8、UTF-16、UTF-16 或非 Unicode 字符映射的编码形式）。
  2. 词法分析（Lexical analysis）：将 Unicode 输入字符流转换为 tokens 标记流。
  3. 语法分析（Syntactic analysis）：将 tokens 标记流转换为可执行代码。

> Unicode NULL 字符 `U+0000` 的处理是特定于实现的，应使用 `\0` 或 `\u0000`。

<br>

### Tokens

标记（tokens）包含了标识符（identifiers）、关键字（keywords）、文本（literals）、运算符（operators）和标点（punctuators），空白和注释不属于标记，仅用来充当标记的分隔符。

#### Unicode 字符转义序列

Unicode 字符转义序列表示一个 Unicode 字符。Unicode 字符转义序列在标识符、字符文本，字符串文本中处理，在其他位置如运算符、标点、关键字不会被处理。
Unicode 转义序列表示由 `\u` 或 `\U` 和十六进制数组成单个 Unicode 字符，C# 使用 Unicode 码位的 16 位编码，字符文本中不允许使用 `U+10000` 到 `U+10FFFF` 范围内的 Unicode 字符，而是使用字符串文本中的 Unicode 代理项对来表示。不支持 `0x10FFFF` 以上的码位的 Unicode 字符。

> *标识符*

标识符（identifiers）可以由 `_` 开头，由数字字符和字母字符组合的字符序列，不能是数字作为开头。可以在标识符中使用 Unicode 转义序列；使用 `@` 开头时，允许关键字作为标识符。

```csharp
class @class
{
    static void @static(bool @bool)
    {
        Console.WriteLine(@bool ? "true" : "false");
    }
    static void main(String[] args)
    {
        cl\u0061ss.st\u0061tic(true);
    }
}
```

#### 关键字

关键字（keyword）是保留的类似于标识符的字符序列，不能用作标识符，除非以 @ 字符开头。

```ANTLR
keyword
    : 'abstract' | 'as'       | 'base'       | 'bool'      | 'break'
    | 'byte'     | 'case'     | 'catch'      | 'char'      | 'checked'
    | 'class'    | 'const'    | 'continue'   | 'decimal'   | 'default'
    | 'delegate' | 'do'       | 'double'     | 'else'      | 'enum'
    | 'event'    | 'explicit' | 'extern'     | 'false'     | 'finally'
    | 'fixed'    | 'float'    | 'for'        | 'foreach'   | 'goto'
    | 'if'       | 'implicit' | 'in'         | 'int'       | 'interface'
    | 'internal' | 'is'       | 'lock'       | 'long'      | 'namespace'
    | 'new'      | 'null'     | 'object'     | 'operator'  | 'out'
    | 'override' | 'params'   | 'private'    | 'protected' | 'public'
    | 'readonly' | 'ref'      | 'return'     | 'sbyte'     | 'sealed'
    | 'short'    | 'sizeof'   | 'stackalloc' | 'static'    | 'string'
    | 'struct'   | 'switch'   | 'this'       | 'throw'     | 'true'
    | 'try'      | 'typeof'   | 'uint'       | 'ulong'     | 'unchecked'
    | 'unsafe'   | 'ushort'   | 'using'      | 'virtual'   | 'void'
    | 'volatile' | 'while'
    ;
```

上下文关键字（contextual keyword）是类似于标识符的字符序列，在某些上下文中具有特殊含义，但不保留。可以在某些上下文用作标识符，也可以使用 `@` 前缀。当用作上下文关键字时，这些标识符不能包含 Unicode 转义序列。

```ANTLR
contextual_keyword
    : 'add'    | 'alias'      | 'ascending' | 'async'     | 'await'
    | 'by'     | 'descending' | 'dynamic'   | 'equals'    | 'from'
    | 'get'    | 'global'     | 'group'     | 'into'      | 'join'
    | 'let'    | 'nameof'     | 'on'        | 'orderby'   | 'partial'
    | 'remove' | 'select'     | 'set'       | 'unmanaged' | 'value'
    | 'var'    | 'when'       | 'where'     | 'yield'
    ;
```

#### 文本

文本表示为值的源代码表示形式。包含有：
- 布尔值文本：`true` 和 `false`。
- 整数文本：十进制表示序列（0-9）、十六进制表示序列（0x0-0xf），整数序列可以添加无符号数（`u` 后缀）或长整数（`L` 后缀）。
- 实数文本：表示实数序列，写入类型为 `float`（`f` 后缀）、`double`（可选 `d` 后缀）、`decimal`（`m` 后缀）。
- 字符文本：字符文本表示单个字符 `'c'`，包含 11 个可能的简单转义序列（单引号 `\'`、双引号 `\"`、反斜杠 `\\`、Null `\0`、警报 `\a`、退格符 `\b`、换页符 `\f`、换行符 `\n`、回车符 `\r`、水平制表符 `\t`、垂直制表符 `\v`）
- 字符串文本：包含常规字符串文本与原义字符串文本，常规字符串中可能包含简单的转义序列、十六进制 `\x XXX`或 Unicode `\u UUUU` 的转义序列；原义字符串以 `@` 作为前缀，除 `""` 转义为双引号 `"`，其他的字符按原义解释。 
- 内插字符串文本：字符串字面量以 `$` 作为前缀，可以内插表达式 `{expr}` 在运行时进行计算，并将结果转换为字符串字面值。
- Null 文本：表示值 `null`。

> *运算符和标点符号*

表达式中使用运算符来描述涉及一个或多个操作数的操作。标点符号用于分组和分隔。

```ANTLR
operator_or_punctuator
    : '{'   | '}'   | '['   | ']'   | '('   | ')'   | '.'   | '->'  | ','   | ':'   | ';'  
    | '+'   | '-'   | '*'   | '/'   | '%'   | '&'   | '|'   | '^'   | '!'   | '~'   | '='   
    | '+='  | '-='  | '*='  | '/='  | '%='  | '&='  | '|='  | '^='  | '~='  | '?'   | '??'     
    | '<'   | '<='  | '>'   | '>='  | '!='  | '=='  | '++'  | '--'  | '&&'  | '||'  | '??='
    | '>>'  | '<<'  | '>>=' | '<<=' | '>>>' | '>>>=' 
    ;
```

<br>

### 预处理指令

预处理指令提供了有条件地跳过部分编译单元、报告错误和警告条件以及描述源代码的不同区域的能力。在 C# 中，没有单独的预处理步骤，预处理指令作为词法分析阶段的一部分进行处理。可用的预处理指令有：

- `#if`、`#elif`、`else`、`endif`：指示在源代码中的哪些部分可以有条件地跳过。
- `#define`、`#undef`：分别用于定义和取消定义条件编译符号。
- `#error`：用于发出错误。
- `#warning`：用于发出警告。
- `#region`、`endregion`：用于显式标记或折叠源代码的某些部分。
- `#line`：用于控制错误和警告发出的行号。
- `#nullable`：用于控制可为空注释上下文和可为空警告上下文。
- `#pragma`：用于向编译器指定可选的上下文信息。

#### 条件编译指令

`#if`、`#elif`、`#else` 和 `#endif` 条件编译指令指令用于有条件地包含或排除源文件的某些部分。可以通过预处理表达式和条件编译符号来控制条件编译功能：

- 条件编译符号存在 `defined` 和 `undefined` 两种状态。在编译单元的词法分析开始时，条件编译符号是未定义的（除非它由编译器选项等外部机制显式定义），在处理到 `#defined` 指令时，该指令中指定的条件编译符号将在该编译单元中保持定义，直到处理相同符号的 `#undef` 指令，或直到编译单元的末尾。`#define` 定义的条件编译符号仅对当前编译单元有效。已定义的符号布尔值为 `true`，未定义的符号布尔值为 `false`。

- 预处理表达式可以出现在 `#if` 和 `#elif` 指令中，表达式可以使用 `!`、`==`、`!=`、`&&`、`||` 操作符，括号用以分组。表达式求值总是产生一个布尔值，仅可以引用的是条件编译符号和布尔字面值。

```csharp
#define MYTEST
using System;
public class MyClass
{
    static void Main()
    {
#if DEBUG == true
        Console.WriteLine("DEBUG is defined");
#elif (!DEBUG && MYTEST)
        Console.WriteLine("MYTEST is defined");
#elif (DEBUG && MYTEST)
        Console.WriteLine("DEBUG and MYTEST are defined");
#else
        Console.WriteLine("DEBUG and MYTEST are not defined");
#endif
    }
}
```

#### 定义指令

使用 `#define` 指令来定义条件编译符号，`#undef` 指令来取消定义条件编译符号。符号可用于指定编译的条件，可通过 `#if` 或 `#elif` 测试符号。`#define` 和 `#undef` 必须用在源文件的开头定义。

```csharp
#define VERBOSE

#if VERBOSE
   Console.WriteLine("Verbose output version");
#endif
```

> `<DefineConstants>`

- 可以通过 `DefineConstants` 编译器选项来定义项目级别的符号，通过 `#undef` 取消定义符号，作用域仅当前 编译单元。

```xml
<DefineConstants>name;name2;name3</DefineConstants>
```

> `ConditionalAttribute`

可以使用 `ConditionalAttribute` 来执行条件编译。使用特性之前需要在此之前定义符号，可以使用 `#define` 定义的符号，也可以是编译器选项设置的符号。`ConditionalAttribute` 仅对特性类或 `void` 方法有效。

```csharp
//#define CONDITION1
#define CONDITION2
using System.Diagnostics;

class Test
{
    static void Main()
    {
        Console.WriteLine("Calling Method1");
        Method1(3);
        Console.WriteLine("Calling Method2");
        Method2();

        Console.WriteLine("Using the Debug class");
        Trace.Listeners.Add(new ConsoleTraceListener());
        Debug.WriteLine("DEBUG is defined");
    }

    [Conditional("CONDITION1")] 
    public static void Method1(int x) => Console.WriteLine("CONDITION1 is defined");
    [Conditional("CONDITION1"), Conditional("CONDITION2")]
    public static void Method2() => Console.WriteLine("CONDITION1 or CONDITION2 is defined");
}

/*
When compiled as shown, the application (named ConsoleApp)
produces the following output.

Calling Method1
// CONDITION1 is defined
Calling Method2
CONDITION1 or CONDITION2 is defined
Using the Debug class
DEBUG is defined
*/
```

#### 诊断指令

诊断指令用于显式生成错误和警告消息，其报告方式与其他编译时错误和警告的方式相同。`#warning` 使用指定的消息生成编译器警告。`#error` 使用指定的消息生成编译器错误。

```csharp
#warning Code review needed before check-in

#if Debug && Retail
    #error A build can't be both debug and retail
#endif

class Test {...}
```

#### 区域指令

区域指令用于显式标记源代码区域。可以使用 `#region` 和 `#endregion` 预处理器指令来标记可折叠的源代码区域。

```csharp
#region MyClass definition 
public class MyClass
{
    static void Main()
    {
    }
}
#endregion 
```

#### 行指令

行指令可用于更改编译器在输出（如警告和错误）中报告的行号和源文件名，这些信息也被调用者信息属性使用。借助 `#line`，可修改编译器的行号及（可选）用于错误和警告的文件名输出。

```csharp
class MainClass
{
    static void Main()
    {
#line 200 "Special"
        int i;
        int j;
#line default
        char c;
        float f;
#line hidden // numbering not affected
        string s;
        double d;
    }
}
/*
Special(200,13): warning CS0168: The variable 'i' is declared but never used
Special(201,13): warning CS0168: The variable 'j' is declared but never used
MainClass.cs(9,14): warning CS0168: The variable 'c' is declared but never used
MainClass.cs(10,15): warning CS0168: The variable 'f' is declared but never used
MainClass.cs(12,16): warning CS0168: The variable 's' is declared but never used
MainClass.cs(13,16): warning CS0168: The variable 'd' is declared but never used
*/
```

> 行指令语法

- `#line number filename` 指示下一行的行号强制设为 number，在下一个 `#line` 指令前，文件名都会报告为 filename。
- `#line default` 指令将行号恢复至默认行号，并对上一指令重新编号的行进行计数。
- `#line hidden` 指令能对调试程序隐藏连续行，当开发者逐行执行代码时，介于 `#line hidden` 和下一 `#line` 指令（假设它不是其他 `#line hidden` 指令）间的任何行都将被跳过。`#line hidden` 指令不影响错误报告中的文件名或行号。 
- `#line filename` 指令可指定要在编译器输出中显示的文件名。默认情况下，将使用源代码文件的实际名称。文件名必须在行号之后。

> C#10 新形式的 `#line` 指令

```csharp
#line (1, 1) - (5, 60) 10 "partial-class.cs"
/*34567*/int b = 0;
```

- `(1, 1)`：指令后面行上的第一个字符的起始行和列。
- `(5, 60)`：标记区域的结束行和列。
- `10`：将使 `#line` 指令生效的列偏移量。
- `"partial-class.cs"`：输出文件的名称。

#### 可为空的上下文

`#nullable` 预处理器指令将设置可为空注释上下文和可为空警告上下文。此指令控制是否可为空注释是否有效（引用类型的 `?` 注释），以及是否给出为 `Null` 性警告。

可在项目级别设置 `<Nullable>`，预处理器指令 `#nullable` 优先于项目级设置，用于设置其控制的上下文，直到另一个 `#nullable` 替代它，或到源文件的末尾。

```csharp
#nullable disable      // 将可为空注释和警告上下文设置为“已禁用”。
#nullable enable       // 将可为空注释和警告上下文设置为“已启用”。
#nullable restore      // 将可为空注释和警告上下文还原为项目设置。
#nullable disable annotations   // 将可为空注释上下文设置为“已禁用”。
#nullable enable annotations    // 将可为空注释上下文设置为“已启用”。
#nullable restore annotations   // 将可为空注释上下文还原为项目设置。
#nullable disable warnings    // 将可为空警告上下文设置为“已禁用”。
#nullable enable warnings     // 将可为空警告上下文设置为“已启用”。
#nullable restore warnings    // 将可为空警告上下文还原为项目设置。
```

#### 编译指示指令

`#pragma` 预处理指令用于指定编译器的可选上下文信息。`#pragma` 为编译器给出特殊指令以编译它所在的文件，这些指令必须受编译器支持。语法：`#pragma pragma-name pragma-arguments`。
  
> `#pragma warning`

- `#pragma warning` 可以启用或禁用特定警告。

```csharp
#pragma warning disable warning-list    // 禁用
#pragma warning restore warning-list    // 启用
```

- `warning-list` 是以逗号分隔的警告编号的列表。

```csharp
using System;

#pragma warning disable 414, CS3021  // CS 前缀可选，禁用
[CLSCompliant(false)]
public class C
{
    int i = 1;
    static void Main(){}
}
#pragma warning restore CS3021  // 启用
[CLSCompliant(false)]  // CS3021
public class D
{
    int i = 1;
    public static void F(){}
}
```

> `#pragma checksum`

- `#pragma checksum`：生成校验和。

```csharp
#pragma checksum "filename" "{guid}" "checksum bytes"
```

- `filename` 是需要监视更改或更新的文件的名称，`guid` 是哈希算法的全局唯一标识符 GUID，`checksum bytes` 是表示校验和字节的十六进制数字的字符串（偶数个）。
- Visual Studio 调试器使用校验和确保它可始终找到正确的源。编译器为源文件计算校验和，然后将输出发出到程序数据库 PDB 文件。调试器随后使用 PDB 针对它为源文件计算的校验和进行比较。
- 如果编译器在文件中没有找到 `#pragma checksum` 指令，它将计算校验和，并将该值写入 PDB 文件。

---
### 面向对象编程

C# 是面向对象的编程语言。面向对象编程的四项基本原则为：
- 抽象：将实体的相关特性和交互建模为类，以定义系统的抽象表示。
- 封装：隐藏对象的内部状态和功能，并仅允许通过一组公共函数进行访问。
- 继承：根据现有抽象创建新抽象的能力。
- 多态：跨多个抽象以不同方式实现继承属性或方法的能力。

---