## CSharp 预处理器指令

---
### 可为空的上下文

- `#nullable` 预处理器指令将设置可为空注释上下文和可为空警告上下文 。此指令控制是否可为空注释是否有效（引用类型的 `?` 注释），以及是否给出为 `Null` 性警告。
- 可在项目级别设置 `<Nullable>`，预处理器指令 `#nullable` 优先于项目级设置，用于设置其控制的上下文，直到另一个 `#nullable` 替代它，或到源文件的末尾。

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

---
### 条件编译

- 使用四个预处理器指令来控制条件编译：
  - `#if`：打开条件编译，并检查指定的符号。
  - `#elif`：重新指定检查另一个符号。
  - `#else`：条件编译分支。
  - `#endif`：结束条件编译。
- 条件编译的控制用于测试是否定义该符号，在返回 `true` 时，编译相应块的代码。`#if DEBUG` 等价于 `#if (DEBUG == true)`，可以使用 `!`、`&&`、`||` 运算符进行组合和计算。
- 以 `#if` 指令开头的条件指令必须以 `#endif` 指令显式终止，其中可以创建复合条件指令 `#elif`、`#else`。`#if` 可以用于检查 `#define` 定义的符号。

```csharp
#define MYTEST
using System;
public class MyClass
{
    static void Main()
    {
#if (DEBUG && !MYTEST)
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

---
### 定义符号

- 使用 `#define` 指令来定义条件编译的符号，`#undef` 指令来取消定义条件编译的符号。符号可用于指定编译的条件，可通过 `#if` 或 `#elif` 测试符号。`#define` 和 `#undef` 必须用在源文件的开头定义。

```csharp
#define VERBOSE

#if VERBOSE
   Console.WriteLine("Verbose output version");
#endif
```

> `<DefineConstants>`

- 可以通过 `DefineConstants` 编译器选项来定义项目级别的符号，通过 `#undef` 取消定义符号，作用域仅当前文件。

```xml
<DefineConstants>name;name2;name3</DefineConstants>
```

> `ConditionalAttribute`

- 可以使用 `ConditionalAttribute` 来执行条件编译。使用特性之前需要在此之前定义符号，可以使用 `#define` 定义的符号，也可以是编译器选项设置的符号。`ConditionalAttribute` 仅对特性类或 `void` 方法有效。

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

---
### 定义区域

- 可以使用 `#region` 和 `#endregion` 预处理器指令来定义可在大纲中折叠的代码区域。

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

---
### 错误和警告信息

- `#error` 使用指定的消息生成编译器错误，编译器以特殊的方式处理 `#error version` 并报告编译器错误 CS8304，消息中包含使用的编译器和语言版本。

```csharp
#error Deprecated code in this method.
```

- `#warning` 使用指定的消息生成编译器警告。

```csharp
#warning Deprecated code in this method.
```

---
### 行信息

- `#line` 更改编译器消息输出的行号。借助 `#line`，可修改编译器的行号及（可选）用于错误和警告的文件名输出。

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

---
### 杂注

- `#pragma` 为编译器给出特殊指令以编译它所在的文件，这些指令必须受编译器支持。语法：`#pragma pragma-name pragma-arguments`。
  
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