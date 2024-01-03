# CLI：元数据定义和语义

此规范提供了元数据的规范性描述：其物理布局 (以文件格式) 、其逻辑内容 (作为一组表格及关系) 、以及其语义 (从假设的汇编器 ILASM)。

此部分专注于元数据的语义和结构。元数据的语义决定了 VES 的大部分操作，使用 CIL 的汇编语言 ILAsm 的语法进行描述。ILAsm 语法本身被视为此国际标准的规范性部分。

汇编语言实际上只是用于指定文件中元数据和该文件中的 CIL 指令的语法。指定的 ILAsm 提供了一种直接交换为 CLI 编写的程序的方法，从而无需使用高级语言；它还提供了一种方便的方式来表达示例。元数据的语义也可以独立于存储元数据的实际格式直接进行描述。

---
## 1. 验证和校验

**验证** (_Validation_) 是指对任何文件应用一组测试，以检查文件的格式、元数据和 CIL 是否自一致性。这些测试目的在于确保文件符合本规范的要求。当符合 CLI 规范的实现面临一个不符合规范的文件时，其行为是未指定的。

**校验** (_Verification_) 是指检查 CIL 及其相关元数据，以确保 CIL 代码序列不允许访问程序逻辑地址空间之外的内存。结合验证测试，校验确保程序不能访问未被授权访问的内存或其他资源。

底层类型系统健全性的数学证明是可能的，并为验证要求提供了基础。除了这些规则，此标准将以下内容视为未指定：
 * 执行此类算法的时间 (如果有)。
 * 在验证失败时，符合规范的实现应该做什么。

下图使这种关系更清晰：

 ![](./.img/正确CIL和可验证CIL之间的关系.png)

在上图中，最外层包含了所有由 ILAsm 语法允许的代码。下一个内圈代表所有正确有效的 CIL 代码。次内圈代表所有类型安全的代码。最内层包含所有可验证的代码。类型安全代码和可验证代码之间的区别是 **可证明性** (_provability_)：通过 VES 验证算法的代码按定义是 *可验证的*；但是，这个简单的算法也会拒绝某些代码，即使更深入的分析会将其揭示为真正的类型安全。

验证过程非常严格。有许多程序可以通过验证，但会在校验中失败。VES 不能保证这些程序不会访问它们未被授权访问的内存或资源。它们可能已经被正确地构造，以使它们不访问这些资源。因此，是否安全地运行这些程序是信任的问题，而不是数学证明的问题。通常符合 CLI 规范的实现能够允许执行 *不可验证的代码* (有效的代码，但未通过校验)，尽管这可能受限于部分不属于此标准管理的信任控制。符合 CLI 规范的实现应允许执行可验证的代码，尽管它们可能受限于一些额外的实现所指定的信任控制。

---
## 2. Hello World

一个简单的 ILAsm。`"Hello world!"` 通过调用 `System.Console` 类中的静态方法 `WriteLine` 来写入的，该类是标准程序集 `mscorlib` 的一部分。

```cil
.assembly extern mscorlib {}
.assembly hello {}
.method static public void main() cil managed
{   
    .entrypoint
    .maxstack 1

    ldstr "Hello world!"
    call void [mscorlib]System.Console::WriteLine(class System.String)
    ret
}
```

**.assembly extern** 声明引用了一个外部程序集 `mscorlib`，它包含了 `System.Console` 的定义。第二行的 **.assembly** 声明了这个程序的程序集的名称，程序集是 CLI 可执行内容的部署单元。**.method** 声明定义了全局方法 `main`，其主体紧随其后。主体的第一行表明这个方法是程序集的入口点 (**.entrypoint**)，主体的第二行指定它最多需要一个堆栈槽 (**.maxstack**)。

`main` 方法只包含三个指令：`ldstr`、`call` 和 `ret`。`ldstr` 指令将字符串常量 `"Hello world!"` 压入堆栈，`call` 指令调用 `System.Console::WriteLine`，将字符串作为其唯一的参数传递。在 CIL 中，字符串字面量是标准类 `System.String` 的实例。`call` 指令应包含被调用方法的完整签名。最后一个指令，`ret` 表明从 `main` 返回。

---
## 3. 通用语法
### 3.1. 通用语法表示法

本部分使用了 BNF 语法表示法的修改形式，以下是这种表示法的简要总结：
- 终端符号以等宽字体书写，例如 **.assembly**、**extern** 和 **float64**；仅由标点字符组成的终端符号被包含在单引号中，例如 `':'`、`'['` 和 `'('`。
- 语法类别的名称以大写和斜体表示，例如 _ClassDecl_，并应被实际类别的实例替换。
- 放在 "`[ ]`" 括号中的项 (例如 [ _Filename_ ] 和 [ _Float_ ]) 是可选的，任何后跟 " * " 的项 (例如 _HexByte_* 和 [ `'.'` _Id_ ]*) 可以出现零次或多次。
- 字符 `|` 表示其两侧的项目都是可以接受的 (例如 `true` | `false`)。
- 选项按字母顺序排序 (按 ASCII 顺序，且不区分大小写)。如果规则以可选项开始，那么在排序时不会考虑这个可选项。

ILAsm 是一种区分大小写的语言，所有的终端符号都应该与本条款中指定的大小写一致。一条像

<pre>
    <em>Top</em> ::= <em>Int32</em> | float <em>Float</em> | floats [ <em>Float</em> [ ',' <em>Float</em> ]* ] | else <em>QSTRING</em>
</pre>

这样的语法会认为以下都是有效的：

```cil
    12
    float 3
    float –4.3e7
    floats
    floats 2.4
    floats 2.4, 3.7
    else "Something \t weird"
```

但以下所有的都是无效的：

```cil
    3, 4
    else 3
    float 4.3, 2.4
    float else
    stuff
```

>---
### 3.2. 基本语法类别

这些类别用于描述输入的语法约束，用于传达元数据中编码信息的逻辑限制：

- _Int32_ 是一个十进制数或 "`0xF`" 十六进制数，应该用 32 位表示。ILAsm 没有 8 位或 16 位整数常量的概念。但是，有时会需要这样常量的情况 (如 `int8(...)` 和 `int16(...)`) 并接受一个 _Int32_，且只使用最低有效字节。

+ _Int64_ 是一个十进制数或 "`0xF`" 十六进制数，应该用 64 位表示。

- _HexByte_ 是一个十六进制数，是来自集合 `0` ~ `9`，`a` ~ `f` 或 `A` ~ `F` 的一对字符。

+ _RealNumber_ 是任何对浮点数的语法表示，与所有其他语法类别的表示都不同。在这个部分，用句点 (`.`) 来分隔整数部分和小数部分，用 "`e`" 或 "`E`" 来分隔尾数和指数。句点或尾数分隔符 (但不能同时省略) 可以省略。一个完整的汇编器可能还提供无穷大和 **NaN** 的语法。

- _QSTRING_ 是一个被双引号 (`"`) 标记包围的字符串。在字符串中，支持转义字符 (`\t`
  水平制表符、`\n` 换行符、`\XXX` 三个八进制数字以表示一个具有该值的字节。`+` 运算符可以用来拼接字符串字面量。一个长字符串可以通过在每行使用 `+` 和一个新的字符串来分割成多行；另一种方法是在一行的最后一个字符使用 `\`，在这种情况下，该字符和其后的换行符不会被输入到生成的字符串中。在 `\` 和下一行的第一个非空白字符之间的任何空白字符 (空格，换行，回车，和制表符) 都会被忽略。要在 _QSTRING_ 中包含一个双引号字符，需要使用八进制转义序列。一个完整的汇编器需要处理支持 Unicode 编码所需的全部问题。

   ```cil
   // "`Hello World from CIL!`"
   ldstr "Hello " + "World " +
   "from CIL!"
   // or
   ldstr "Hello World\
         \040from CIL!"
   ```
+ _SQSTRING_ 就像 _QSTRING_ 一样，只是 _SQSTRING_ 使用单引号 (`'`) 标记而不是双引号。要在 _SQSTRING_ 中包含一个单引号字符，使用八进制转义序列。
  
- _ID_ 是一个连续的字符字符串，它以字母字符 (`A` ~ `Z`，`a` ~ `z`) 或 "`_`"、"`$`"、"`@`"、"`` ` ``" 或 "`?`" 开头，后随任意数量的字母数字字符 (`A` ~ `Z`，`a` ~ `z`，`0` ~ `9`) 或字符 "`_`"、"`$`"、"`@`"、"`` ` ``"，和 "`?`"。_ID_ 只有两种用途：
   * 作为 CIL 指令的标签。
   * 作为 _Id_。

>---
### 3.3. 标识符

标识符用于命名实体。简单的标识符等同于 _ID_。然而，ILAsm 语法允许使用 Unicode 字符集组成任何标识符。标识符应该放在单引号内。以下是这种语法的总结：
<pre>
   <em id="Id">Id</em> ::= <em>ID</em> | <em>SQSTRING</em>
</pre>

只有当关键字出现在单引号中时，才能将关键字用作标识符。通过用点 (`.`) 分隔相邻的两个 *Id*，可以将几个 *Id* 组合成一个更大的 _Id_。以这种方式形成的 _Id_ 被称为 _DottedName_。

<pre>
   <em id="DottedName">DottedName</em> ::= <em>Id</em> [ '.' <em>Id</em> ]*
</pre>
 
_DottedName_ 是为了方便提供的，因为 "`.`" 可以使用 _SQSTRING_ 语法包含在 _Id_ 中。在语法中，当 `'.'` 被认为是一个常见字符时 (例如，在完全限定类型名中)，使用 _DottedName_。以下是简单的标识符：

 ```
 A   Test   $Test   @Foo?   ?_X_   MyType`1

 'Weird Identifier'   'Odd\102Char'   'Embedded\nReturn'

 System.Console   'My Project'.'My Component'.'My Name'   System.IComparable`1
 ```

>---
### 3.4. 标签和标签列表
<a id="id"></a>

标签作为一种编程便利提供，它们代表元数据中编码的一个数字。标签所代表的值通常是从当前方法的开始到某个偏移量的字节数，尽管精确的编码方式取决于标签出现在逻辑元数据结构或 CIL 流中的位置。

一个简单的标签是一个特殊的名字，代表一个地址。在语法上，标签等同于 _Id_。因此，标签可以被单引号引起来，并且可以包含 Unicode 字符。

标签列表是由逗号分隔的，可以是任何组合的简单标签。

<pre>
   <em id="LabelOrOffset">LabelOrOffset</em> ::= <em>Id</em>
   <em id="Labels">Labels</em> ::= <em>LabelOrOffset</em> [ ',' <em>LabelOrOffset</em> ]*
</pre>

在一个真正的汇编器中，_LabelOrOffset_ 的语法可能允许直接指定一个数字，而不是所需的符号标签。

ILAsm 区分两种类型的标签：代码标签和数据标签。代码标签后面跟着一个冒号 ("`:`")，代表要执行的指令的地址。代码标签出现在指令之前，它们代表紧接标签的指令地址。一个特定的代码标签名在一个方法中不能声明多次。

与代码标签相反，数据标签指定了一块数据位置，并且不包含冒号字符。数据标签不能用作代码标签，代码标签不能用作数据标签。一个特定的数据标签名在一个模块中不能声明多次。

<pre>
   <em id="CodeLabel">CodeLabel</em> ::= <em>Id</em> ':'
   <em id="DataLabel">DataLabel</em> ::= <em>Id</em>
</pre>

以下定义了一个代码标签，`ldstr_label`，它代表 `ldstr` 指令的地址：

```cil
ldstr_label: ldstr "A label"
```

>---
### 3.5. 十六进制字节列表

字节列表仅由一个或多个十六进制字节组成。

<pre>
    <em id="Bytes">Bytes</em> ::= <em>HexByte</em> [ <em>HexByte</em>* ]
</pre>

>---
### 3.6. 浮点数

有两种不同的方式来指定一个浮点数：
 1. 作为一个 _RealNumber_。
 2. 使用关键字 **float32** 或 **float64**，后随一个括号中的整数，其中整数值是所需浮点数的二进制表示。例如，`float32(1)` 结果是 4 字节值 `1.401298E-45`，而 `float64(1)` 的结果是 8 字节值 `4.94065645841247E-324`。

<pre>
    <em>Float32</em> ::= <em>RealNumber</em> | float32 '(' <em>Int32</em>  ')'
    <em>Float64</em> ::= <em>RealNumber</em> | float64 '(' <em>Int64</em>  ')'
</pre>

例如：

```cil
    5.5
    1.1e10
    float64(128)  // 这会产生一个 8 字节的值，其位序列与整数值 128 的位序列相同。
```

>---
### 3.7. 源文件行信息
<a id="ExternSourceDecl"></a>

元数据并未编码关于变量的词法范围或源代码行号到 CIL 指令的映射信息。然而为了创建信息的替代编码，指定一个汇编器语法来提供这些信息是有用的。

**.line** 接受一个行号，可选后随一个列号 (前面带有冒号)，可选后随一个单引号字符串，该字符串指定行号所引用的文件的名称：

<pre>
    <em>ExternSourceDecl</em> ::= .line <em>Int32</em> [ ':' <em>Int32</em> ][ <em>SQSTRING</em> ]
</pre>

>---
### 3.8. 文件名
<a id="Filename"></a>

某些语法元素需要提供一个文件名。文件名就像任何其他名称一样，其中 "`.`" 被视为正常的组成字符。文件名的具体语法遵循底层操作系统的规范。

<pre>
    <em>Filename</em> ::= <em>DottedName</em>
</pre>

>---
### 3.9. 特性和元数据

类型及其成员使用特性在它们定义上附加了描述性信息。最常见的特性是预定义的，并且在元数据中有特定的编码。此外，元数据提供了一种方式，使用几种不同的编码将用户定义的特性附加到元数据上。

从语法角度来看，有几种在 ILAsm 中指定特性的方式：

 * **使用内置在 ILAsm 中的特殊语法**。例如，在 _ClassAttr_ 中的关键字 **private** 指定类型的可见性应被限制为只允许在定义的程序集中访问。

 - **在 ILAsm 中使用通用语法**。例如非终结符 _CustomDecl_ 描述了这种语法。对于一些称为伪自定义特性的特性，这个语法实际上会在元数据中设置特殊的编码。

 * **安全特性被特殊处理**。在 ILAsm 中有特殊的语法，允许直接描述表示安全特性的 XML。虽然所有其它在标准库或用户提供的扩展中定义的特性都使用一个公共机制在元数据中编码，但安全特性 (直接或间接继承自 `System.Security.Permissions.SecurityAttribute`) 应按照 [「_DeclSecurity: 0x0E_」](#DeclSecurity_0x0E) 中的描述进行编码。

>---
### 3.10. ILAsm 源文件
<a id="il-top-impl"></a>

_ilasm_ 的输入是一系列的顶级声明，如下所定义：

<pre>
    <em id="ILFile">ILFile</em> ::= <em>Decl</em>*
</pre>

下面显示了顶级声明的完整语法。参考子项包含了此语法对应产生式的详细信息。这些产生式以 `'.'` 前缀的名字开始。这样的名字被称为指令。

 | _Decl_ ::=                                                                                 | 参考                      |
 | :----------------------------------------------------------------------------------------- | ------------------------- |
 | `.assembly` _DottedName_ `'{'` _AsmDecl_* `'}'`                                            | §[[↗]](#assembly)         |
 | \| `.assembly extern` _DottedName_ `'{'` _AsmRefDecl_* `'}'`                               | §[[↗]](#assembly-extern)  |
 | \| `.class` _ClassHeader_ `'{'` _ClassMember_* `'}'`                                       | §[[↗]](#class)            |
 | \| `.class extern` _ExportAttr_ _DottedName_ `'{'` _ExternClassDecl_* `'}'`                | §[[↗]](#class-extern)     |
 | \| `.corflags` _Int32_                                                                     | §[[↗]](#corflags)         |
 | \| `.custom` _CustomDecl_                                                                  | §[[↗]](#custom)           |
 | \| `.data` _DataDecl_                                                                      | §[[↗]](#data)             |
 | \| `.field` _FieldDecl_                                                                    | §[[↗]](#field)            |
 | \| `.file` [ `nometadata` ] _Filename_ `.hash` `'='` `'('` _Bytes_ `')'` [ `.entrypoint` ] | §[[↗]](#file)             |
 | \| `.method` _MethodHeader_ `'{'` _MethodBodyItem_* `'}'`                                  | §[[↗]](#method)           |
 | \| `.module` [ _Filename_ ]                                                                | §[[↗]](#module)           |
 | \| `.module extern` _Filename_                                                             | §[[↗]](#module-extern)    |
 | \| `.mresource` [ `public`  \| `private` ] _DottedName_ `'{'` _ManResDecl_* `'}'`          | §[[↗]](#mresource)        |
 | \| `.subsystem` _Int32_                                                                    | §[[↗]](#subsystem)        |
 | \| `.vtfixup` _VTFixupDecl_                                                                | §[[↗]](#vtfixup)          |
 | \| _ExternSourceDecl_                                                                      | §[[↗]](#ExternSourceDecl) |
 | \| _SecurityDecl_                                                                          | §[[↗]](#SecurityDecl)     |

---
## 4. 程序集、清单和模块

在 CLI 中，程序集和模块是分组构造，每个都在 CLI 中扮演不同的角色。

**程序集** (_assembly_) 是作为一个单元部署的一个或多个模块的集合。程序集总是包含一个 **清单** (_manifest_)，该清单指定：

 * 程序集的版本、名称、区域性和安全要求。
 * (如果有的话) 哪些其他文件属于程序集，以及每个文件的加密哈希。清单本身位于文件的元数据部分，该文件始终是程序集的一部分。
 * 要从程序集中导出的其他文件中定义的类型。在与清单相同的文件中定义的类型是基于类型本身的特性进行导出的。
 * (可选的) 清单本身的数字签名，以及用于计算它的公钥。

**模块** (_module_) 是一个包含在程序集中指定格式的可执行内容的单个文件。如果模块包含一个清单，那么它还指定了构成程序集的模块 (包括它自己)。一个程序集的所有组成文件中只能包含一个清单。对于要执行的程序集 (而不是简单地被动态加载)，清单应该位于包含入口点的模块中。

虽然一些编程语言引入了 *namespace* 命名空间的概念，但 CLI 对这个概念的唯一支持是作为元数据编码技术。类型名称总是由包含它们定义的程序集的完全限定名称来指定。

> 下面是一个关于模块、程序集和文件之间关系的图示：

 ![](./.img/模块和文件的引用关系.png)

图中显示了八个文件，以 _M_ 开头的文件表示一个模块。以 _F_ 开头的文件可以表示为资源文件 (如位图) 或其他不包含 CIL 代码的文件。

文件 _M1_ 和 _M4_ 除了声明模块外，还声明了一个程序集，分别是程序集 _A_ 和 _B_。_M1_ 和 _M4_ 中的程序集声明引用了其他模块，用直线显示。例如，程序集 _A_ 引用了 _M2_ 和 _M3_，程序集 _B_ 引用了 _M3_ 和 _M5_。这两个程序集都引用了 _M3_。

通常，一个模块只属于一个程序集，但是也可以在程序集之间共享。当程序集 _A_ 在运行时加载时，将为它加载 _M3_ 的一个实例。当程序集 _B_ 加载到同一个应用程序域中时，_M3_ 可能同时被程序集 _A_ 和 _B_ 共享。两个程序集也都引用了 _F2_，对此适用类似的规则。

模块 _M2_ 引用了 _F1_，用虚线显示。因此，当执行 _A_ 时，_F1_ 将作为程序集 _A_ 的一部分加载。因此，文件引用也应该出现在程序集声明中。同样，_M5_ 引用了另一个模块 _M6_，当执行 _B_ 时，_M6_ 将成为 _B_ 的一部分。由此可见，程序集 _B_ 也应该有一个对 _M6_ 的模块引用。

>---
### 4.1. 程序集定义
<a id="assembly"></a>

程序集是指包含元数据中清单的模块。清单的信息是从以下部分的语法中创建的：

 | _Decl_ ::=                                                                                 | 参考                     |
 | :----------------------------------------------------------------------------------------- | ------------------------ |
 | `.assembly` _DottedName_ `'{'` _AsmDecl_* `'}'`                                            | §[[↗]](#assembly)        |
 | \| `.assembly extern` _DottedName_ `'{'` _AsmRefDecl_* `'}'`                               | §[[↗]](#assembly-extern) |
 | \| `.corflags` _Int32_                                                                     | §[[↗]](#corflags)        |
 | \| `.file` [ `nometadata` ] _Filename_ `.hash` `'='` `'('` _Bytes_ `')'` [ `.entrypoint` ] | §[[↗]](#file)            |
 | \| `.module extern` _Filename_                                                             | §[[↗]](#module-extern)   |
 | \| `.mresource` [ `public` \| `private` ] _DottedName_ `'{'` _ManResDecl_* `'}'`           | §[[↗]](#mresource)       |
 | \| `.subsystem` _Int32_                                                                    | §[[↗]](#subsystem)       |
 | \| ...                                                                                     |                          |

**.assembly** 指令声明了清单，并指定当前模块属于哪个程序集。一个模块最多只能包含一个 **.assembly** 指令。_DottedName_ 指定了程序集的名称。由于一些平台以不区分大小写的方式处理名称，因此不应声明只在大小写上有区别的两个程序集。

<a id = "corflags"></a>**.corflags** 指令在输出 PE 文件的 CLI 头部设置一个字段。符合 CLI 规范的实现应该期望这个字段的值为 1。为了向后兼容，保留了三个最低有效位。未来这个标准的版本可能会为 8 到 65,535 之间的值提供定义。因此，实验性和非标准的使用应该使用大于 65,535 的值。

<a id="subsystem"></a>**.subsystem** 指令只在程序集直接执行时使用 (而不是作为其他程序的库使用)。这个指令通过在 PE 文件头部存储指定值来指定程序所需的应用程序环境类型。虽然可以提供任何 32 位整数值，但符合 CLI 规范的实现只需要认同以下两个值：
 * 如果值为 2，程序应该使用适合具有图形用户界面的应用程序的任何约定来运行。
 * 如果值为 3，程序应该使用适合具有直接控制台连接的应用程序的任何约定来运行。

```cil
.assembly CountDown
{ 
    //...
    .hash algorithm 32772
    .ver 1:0:0:0
}
.file Counter.dll .hash = (BA D9 7D 77 31 1C 85 4C 26 9C 49 E7  02 BE E7 52 3A CB 17 AF)
.subsystem 0x0003 // WindowsCui
.corflags 0x00000001 // ILOnly
```

#### 4.1.1. *AsmDecl*：程序集信息
<a id="AsmDecl"></a>

 | _AsmDecl_ ::=                                               | 描述                               | 参考                    |
 | :---------------------------------------------------------- | ---------------------------------- | ----------------------- |
 | `.custom` _CustomDecl_                                      | 自定义特性                         | §[[↗]](#custom)         |
 | \| `.hash algorithm` _Int32_                                | 在 **.file** 指令中使用的哈希算法  | §[[↗]](#hash-algorithm) |
 | \| `.culture` _QSTRING_                                     | 为其构建此程序集的区域性           | §[[↗]](#culture)        |
 | \| `.publickey` `'='` `'('` _Bytes_ `')'`                   | 发起者公钥。                       | §[[↗]](#publickey)      |
 | \| `.ver` _Int32_ `':'` _Int32_ `':'` _Int32_ `':'` _Int32_ | 主版本号、次版本号、构建号和修订号 | §[[↗]](#ver)            |
 | \| _SecurityDecl_                                           | 所需、期望或禁止的权限             | §[[↗]](#SecurityDecl)   |

##### 4.1.1.1. hash algorithm：哈希算法
<a id="hash-algorithm"></a>

<pre>
    <em>AsmDecl</em> ::= .hash algorithm <em>Int32</em> | ...
</pre>

当一个程序集由多个文件组成时，程序集的清单会指定除自身文件外的每个文件的名称和内容的加密哈希。用于计算哈希的算法可以被指定，并且对于包含在程序集中的所有文件，这个算法应该是相同的。所有的值都保留用于未来的使用，符合 CLI 规范的实现应该使用 SHA-1 哈希函数，并且应该使用值 32772 (0x8004) 来指定这个算法。

在标准化时，SHA-1 被选为最广泛可用的最佳技术。选择了一个单一算法原因是所有符合 CLI 的实现都需要实现所有算法，以确保可执行映像的可移植性。

```cil
.assembly Test
{ 
    //...
	.hash algorithm 0x00008004 // SHA1
    //...
}
```

##### 4.1.1.2. culture：区域性
<a id="culture"></a>

<pre>
    <em>AsmDecl</em> ::= .culture <em>QSTRING</em> | ...
</pre>

当存在时，这表示程序集已经针对特定的区域性进行了定制。这里应该使用的字符串是被 `System.Globalization.CultureInfo` 类接受的那些字符串。当用于比较程序集引用和程序集定义时，这些字符串应该以不区分大小写的方式进行比较。

Culture 名称遵循 [IETF RFC1766](https://datatracker.ietf.org/doc/rfc1766/) 的名称。它的格式为 “`<language>-<country/region>`"，其中 `<language>` 是 [ISO 639-1](https://www.iso.org/iso-639-language-code) 中指定的小写两字母代码。`<country/region>` 是 [ISO 3166](https://www.iso.org/iso-3166-country-codes.html) 中的大写两字母代码。

##### 4.1.1.3. publickey：发起者公钥
<a id="publickey"></a>

<pre>
    <em>AsmDecl</em> ::= .publickey '(' <em>Bytes</em> ')' | ...
</pre>

CLI 元数据允许程序集的发起者计算该程序集的加密哈希 (使用 SHA-1 哈希函数)，然后使用 RSA 算法和发起者选择的 ***公钥*** / ***私钥*** 对对其进行加密。这个结果 (一个 SHA1/RSA 数字签名) 可以与 RSA 算法所需的密钥对的公共部分一起存储在元数据中。

**.publickey** 指令用于指定用于计算签名的公钥。为了计算哈希，先将签名清零，然后计算哈希，最后将结果存储到签名中。

标准库中的所有程序集都使用公钥 `00 00 00 00 00 00 00 00 04 00 00 00 00 00 00 00`。在这个标准中，这个密钥被称为 **标准公钥** (_Standard Public Key_)。

对程序集的引用在编译时捕获了其中一些信息。在运行时，程序集引用中包含的信息可以与在运行时定位的程序集的清单中的信息结合起来，以确保在创建引用时 (编译时) 和解析时 (运行时) 看到的程序集都使用了相同的私钥。

强名称 (_Strong Name_,*SN*) 签名过程使用标准的哈希和密码算法进行强名称签名，大部分 PE 文件上都生成了 SHA-1 哈希。该哈希值使用 SN 私钥进行 RSA 签名。出于验证目的，公钥以及签名的哈希值都存储在 PE 文件中。

除以下部分外，PE 文件的所有部分都被哈希：

* ***Authenticode Signature entry*** (授权码签名)：PE 文件可以使用授权码签名。授权码签名包含在 PE Header Data Directory 的偏移 128 的 8 字节条目 ("Certificate Table")，以及 PE 文件在此目录项指定范围内的内容。在符合规范的 PE 文件中，此条目应为零。

 * ***Strong Name Blob*** (强名称)：CLI Header 文件的偏移 32 的 8 字节条目 ("StrongNameSignature")，以及 PE 文件中此 RVA 包含的哈希数据的内容。如果 8 字节条目为 0，则没有关联的强名称签名。

 * ***PE Header Checksum*** (校验和)：PE Header Windows NT-Specific Fields 的偏移 64 的 4 字节条目 ("File Checksum")。在符合规范的 PE 文件中，此条目应为零。

##### 4.1.1.4. ver：程序集版本
<a id="ver"></a>

<pre>
    <em>AsmDecl</em> ::= .ver <em>Int32</em> ':' <em>Int32</em> ':' <em>Int32</em> ':' <em>Int32</em> | ...
</pre>

 
程序集的版本号被指定为四个 32 位整数。这个版本号应在编译时捕获，并用作编译模块内对程序集的所有引用的一部分。

所有标准化的程序集应将最后两个 32 位整数设置为 0。此标准对版本号的使用没有其他要求，尽管建议个别实现者应避免将最后两个整数都设置为 0，以避免与此标准的未来版本发生冲突。

如果为标准化的程序集添加了任何额外的功能或需要实现 VES 的任何额外特性，那么此标准的未来版本应更改为标准化程序集指定的前两个整数中的一个或两个。此外，此标准的未来版本应更改为 **mscorlib** 程序集指定的前两个整数中的一个或两个，以便其版本号可以 (如果需要) 用来区分运行程序所需的执行引擎的不同版本。

一个符合规范的实现可以完全忽略版本号，或者在绑定时要求它们必须精确匹配，或者它可以表现出任何其他被认为合适的行为。按照惯例：
 1. 这些整数中的第一个被认为是 **主版本号** (_major_)，具有相同名称但主版本号不同的程序集是不可互换的。例如，假设一个不能向后兼容的产品的显著重写。
 2. 这些整数中的第二个被认为是 **次版本号** (_minor_)，具有相同名称和主版本号，但次版本号不同的程序集，表示有显著增强且认为是向后兼容的。例如，一个产品的 “点发布” 或一个完全向后兼容的新版本。
 3. 这些整数中的第三个被认为是 **构建号** (_build_)，只有构建号不同的程序集，认为是从相同的源重新编译。例如，处理器、平台或编译器的改变。
 4. 这些整数中的第四个被认为是 **修订号** (_revision_)，具有相同的名称、主版本号和次版本号，但修订号不同的程序集，认为是完全可互换的。例如，修复以前发布的程序集中的安全漏洞。

#### 4.1.2. mresource：清单资源
<a id="mresource"></a>

**清单资源** (_manifest resource_) 只是与程序集相关联的一个命名数据项。清单资源是使用 **.mresource** 指令引入的，该指令将清单资源添加到由的 **.assembly** 声明开始的程序集清单中。

<pre>
    <em>Decl</em> ::= .mresource  [ public | private ]  <em>DottedName</em> '{' <em>ManResDecl</em> '}' | ...
</pre>

如果清单资源被声明为 **public**，则它将从程序集中导出。如果它被声明为 **private**，则不会导出，此时，它只能在程序集内部使用。_DottedName_ 是资源的名称。

 | _ManResDecl_ ::=                     | 描述                                                            | 参考                     |
 | :----------------------------------- | --------------------------------------------------------------- | ------------------------ |
 | `.assembly extern` _DottedName_      | 显示资源在名为 _DottedName_ 的外部程序集中。                    | §[[↗]](#assembly-extern) |
 | \| `.custom` _CustomDecl_            | 自定义特性。                                                    | §[[↗]](#CustomDecl)      |
 | \| `.file` _DottedName_ `at` _Int32_ | 显示资源在名为 _DottedName_ 的文件中，位于 _Int32_ 字节偏移处。 |                          |

对于存储在非模块文件中的资源 (例如，附加的文本文件)，应使用单独的 (顶级) **.file** <sup>[[↗]](#file)</sup> 在清单中声明文件，并且字节偏移量应为零。在另一个程序集中定义的资源可以使用 **.assembly extern** 进行引用，这要求程序集必须已在单独的 (顶级) **.assembly extern** 指令中定义。

#### 4.1.3. file：程序集关联文件
<a id="file"></a>

程序集可以与其他文件 (如文档和执行期间使用的其他文件) 关联。**.file** 声明用于将对此类文件的引用添加到程序集的清单中：

<pre>
    <em>Decl</em> ::= .file  [ nometadata ]  <em>Filename</em> .hash '=' '(' <em>Bytes</em> ')' [ .entrypoint ] | ...
</pre>
 
如果文件不是符合此规范的模块，则指定特性 **nometadata**。标记为 **nometadata** 的文件可以有任何格式，它们被视为纯数据文件。

<a id="hash"></a>**.hash** 后面的 _Bytes_ 指定了为文件计算的哈希值。VES 应在访问此文件之前重新计算此哈希值，如果两者不匹配，行为是未指定的。用于计算此哈希值的算法由 **.hash algorithm** 指定。

如果指定 **.entrypoint** 指令，则表示多模块程序集的入口点包含在此文件中。

>---
### 4.2. assembly extern：引用程序集
<a id="assembly-extern"></a>

<pre>
    <em>Decl</em> ::= .assembly extern <em>DottedName</em> [ as <em>DottedName</em> ] '{' <em>AsmRefDecl</em>* '}' | ...
</pre>

程序集通过元数据协调其包含的文件对其它程序集的所有访问，它要求执行程序集的清单中包含的执行代码引用的任何程序集的声明来完成的。顶级 **.assembly extern** 声明用于此目的。可选的 **as** 子句提供了一个别名，允许 ILAsm 寻址具有相同名称，但版本、区域性等不同的外部程序集。

在 **.assembly extern** 中使用的 _DottedName_ 应与 **.assembly** 指令声明的程序集名称完全匹配，区分大小写。一个程序集可能被存储在一个文件中，即使文件系统是不区分大小写的，但在元数据内部存储的名称是区分大小写的，并且应完全匹配。

 | _AsmRefDecl_ ::=                                            | 描述                                 | 参考                    |
 | :---------------------------------------------------------- | ------------------------------------ | ----------------------- |
 | `.hash` `'='` `'('` _Bytes_ `')'`                           | 引用程序集的哈希                     | §[[↗]](#hash)           |
 | \| `.custom` _CustomDecl_                                   | 自定义特性                           | §[[↗]](#custom)         |
 | \| `.culture` _QSTRING_                                     | 引用的程序集的区域性                 | §[[↗]](#culture)        |
 | \| `.publickeytoken` `'='` `'('` _Bytes_ `')'`              | 发起者公钥的 SHA-1 哈希的低 8 字节。 | §[[↗]](#publickeytoken) |
 | \| `.publickey` `'='` `'('` _Bytes_ `')'`                   | 发起者的完整公钥                     | §[[↗]](#publickey)      |
 | \| `.ver` _Int32_ `':'` _Int32_ `':'` _Int32_ `':'` _Int32_ | 主版本号，次版本号，构建号和修订号   | §[[↗]](#ver)            |

这些声明与 **.assembly** 声明相同，除了添加了 <a id="publickeytoken"></a>**.publickeytoken**。此声明用于在程序集引用中存储发起者公钥的 SHA-1 哈希的低 8 字节，而不是完整的公钥。

程序集引用可以存储完整的公钥或 8 字节的公钥 _token_  *Public Key Token*。两个都可以用来验证在编译时或运行时为程序集签名的同一私钥。两者并不需要同时存在，虽然两者都可以存储。

符合 CLI 规范的实现不需要执行这个验证，但它可以这样做，且它可以拒绝加载任何验证失败的程序集。符合 CLI 规范的实现也可以拒绝允许访问一个程序集，除非程序集引用包含公钥或公钥 _token_ 。无论是使用公钥还是公钥 _token_ ，符合 CLI 规范的实现都应该做出相同的访问决策。

存储在程序集引用中的公钥或公钥 _token_ 用于确保被引用的程序集和实际在运行时使用的程序集都是由拥有同一私钥的实体产生的，因此可以假定它们是为了相同的目的。虽然完整的公钥在密码学上更安全，但它在引用中需要更多的存储空间。使用公钥 _token_ 可以减少存储引用所需的空间，同时只稍微削弱了验证过程。

为了验证程序集的内容自创建以来没有被篡改，应使用的是程序集自身标识中的完整公钥，而不是存储在对程序集的引用中的公钥或公钥 _token_ 。

```CIL
.assembly extern MyComponents 
{ 
    .publickeytoken = (BB AA BB EE 11 22 33 00)
    .hash = (2A 71 E9 47 F5 15 E6 07 35 E4 CB E3 B4 A1 D3 7F 7F A0 9C 24)
    .ver 2:10:2002:0
}
```

>---
### 4.3. module：模块声明
<a id= "module"></a>

所有的 CIL 文件都是模块，并通过元数据中携带的逻辑名称引用，而不是通过它们的文件名。

<pre>
    <em>Decl</em> ::= .module <em>Filename</em> | ...
</pre>

```cil
.module CountDown.exe
```

>---
### 4.4. module extern：模块引用
<a id= "module-extern"></a>

当一个项目在当前程序集中，但是属于除包含清单之外的其他模块时，应使用 **.module extern** 指令在程序集的清单中声明已定义模块。引用程序集的 **.module extern** 指令中使用的名称应与已定义模块的 **.module** 指令中使用的名称完全匹配。

<pre>
    <em>Decl</em> ::= .module extern <em>Filename</em> | ...
</pre>

```cil
.module extern Counter.dll
```

>---
### 4.5. 在模块或程序集内部的声明
<a id="assembly-inside-decl"></a>

在模块或程序集内部的声明由以下语法指定。

 | _Decl_ ::=                                                | 参考                      |
 | :-------------------------------------------------------- | ------------------------- |
 | \| `.class` _ClassHeader_ `'{'` _ClassMember_* `'}'`      | §[[↗]](#class)            |
 | \| `.custom` _CustomDecl_                                 | §[[↗]](#custom)           |
 | \| `.data` _DataDecl_                                     | §[[↗]](#data)             |
 | \| `.field` _FieldDecl_                                   | §[[↗]](#field)            |
 | \| `.method` _MethodHeader_ `'{'` _MethodBodyItem_* `'}'` | §[[↗]](#method)           |
 | \| _ExternSourceDecl_                                     | §[[↗]](#ExternSourceDecl) |
 | \| _SecurityDecl_                                         | §[[↗]](#SecurityDecl)     |
 | \| ...                                                    |                           |

### 4.6. class extern：导出类型定义
<a id="class-extern"></a>

清单模块 (每个程序集只能有一个) 包含 **.assembly** 指令。如果要导出在程序集的任何其他模块中定义的类型，需要在程序集的清单中有一个清单项声明。以下语法用于在清单中构造这样的项：

<pre>
    <em>Decl</em> ::= .class extern <em>ExportAttr</em> <em>DottedName</em> { <em>ExportClassDecl</em> } | ...
    <em>ExternClassDecl</em> ::= .file <em>DottedName</em>
    <em>ExternClassDecl</em> ::= .class extern <em>DottedName</em> | .custom <em>CustomDecl</em>
</pre>
 
_ExportAttr_ 值应为 **public** 或 **nested public**，并应与类型的可见性匹配。

例如，假设一个程序集由两个模块 `A.EXE` 和 `B.DLL` 组成。`A.EXE` 包含清单。公共类 `Foo` 在 `B.DLL` 中定义。为了导出它，必须在 `A.EXE` 中包含一个 **.class extern** 指令。在 `A.EXE` 中定义的公共类 `Bar` 则不需要任何 **.class extern** 指令。

工具应该能够检索单个清单模块，以确定程序集定义的类型的完整集合。因此，程序集内其他模块的信息都可以在清单模块中被复制。按照惯例，清单模块也被称为程序集。     

### 4.7. class extern forwarder：类型转发器
<a id="class-extern-forwarder"></a>

类型转发器 (*type forwarders*) 表示原本在此程序集中的类型现在位于另一个程序集中，VES 应将类型的引用解析到另一个程序集。类型转发信息存储在 _ExportedType_ 表中。以下语法用于在 _ExportedType_ 表中构造清单项：

<pre>
    <em>Decl</em> ::= .class extern forwarder <em>DottedName</em> '{' .assembly extern <em>DottedName</em> '}' | ...
</pre>

如果类型移动到另一个程序集，类型转发器允许引用该类型的原始程序集的程序集，并且能够正确运行而无需重新编译。

---
## 5. 类型和签名

元数据提供了定义和引用类型的机制。无论该类型是接口、类还是值类型，用于引用类型的机制分为两部分：

 * 用户定义的类型的逻辑描述，这些类型被引用，但通常不在当前模块中定义。这些信息存储在元数据的一个表中 ([「_TypeRef: 0x01_」](#TypeRef_0x01))。
 * 对一个或多个类型引用以及各种修饰符进行编码的签名。非终结符 **Type** 中对签名进行了描述。

### 5.1. Type：类型
<a id= "Type"></a>

以下语法完全指定了 CLI 系统的所有内置类型 (包括指针类型)。它还显示了可以在 CLI 系统中定义的用户定义类型的语法：

 | *Type* ::=                                                   | 描述                                                                                                          | 参考                                |
 | :----------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------- | ----------------------------------- |
 | `'!'` _Int32_                                                | 类型定义中的泛型参数，从 0 开始按索引访问                                                                     | §[[↗]](#!)                          |
 | \| `'!!'` _Int32_                                            | 方法定义中的泛型参数，从 0 开始按索引访问                                                                     | §[[↗]](#!!)                         |
 | \| `bool`                                                    | 布尔值                                                                                                        | §[[↗]](#build-in)                   |
 | \| `char`                                                    | 16 位 Unicode 代码点                                                                                          | §[[↗]](#build-in)                   |
 | \| `class` _TypeReference_                                   | 用户定义的引用类型                                                                                            | §[[↗]](#TypeReference)              |
 | \| `float32`                                                 | 32 位浮点数                                                                                                   | §[[↗]](#build-in)                   |
 | \| `float64`                                                 | 64 位浮点数                                                                                                   | §[[↗]](#build-in)                   |
 | \| `int8`                                                    | 有符号 8 位整数                                                                                               | §[[↗]](#build-in)                   |
 | \| `int16`                                                   | 有符号 16 位整数                                                                                              | §[[↗]](#build-in)                   |
 | \| `int32`                                                   | 有符号 32 位整数                                                                                              | §[[↗]](#build-in)                   |
 | \| `int64`                                                   | 有符号 64 位整数                                                                                              | §[[↗]](#build-in)                   |
 | \| `method` _CallConv_ _Type_ `'*'` `'('` _Parameters_ `')'` | 方法指针                                                                                                      | §[[↗]](#method-pointer)             |
 | \| `native int`                                              | 32 位或 64 位有符号整数，其大小是平台特定的                                                                   | §[[↗]](#build-in)                   |
 | \| `native unsigned int`                                     | 32 位或 64 位无符号整数，其大小是平台特定的                                                                   | §[[↗]](#build-in)                   |
 | \| `object`                                                  | 参见 `System.Object`                                                                                          |                                     |
 | \| `string`                                                  | 参见 `System.String`                                                                                          |                                     |
 | \| _Type_ `'&'`                                              | 指向 _Type_ 的托管指针。_Type_ 不应是托管指针类型或 **typedref**                                              | §[[↗]](#pointer)                    |
 | \| *Type* `'*'`                                              | 指向 *Type* 的非托管指针                                                                                      | §[[↗]](#pointer)                    |
 | \| *Type* `'<'` *GenArgs* `'>'`                              | 泛型类型的实例化                                                                                              | §[[↗]](#GenArgs)                    |
 | \| _Type_ `'['` [ *Bound* [ `','` *Bound* ]*] `']'`          | _Type_ 的数组，可选的秩 (维数) 和边界。                                                                       | §[数组](#array) 和 §[向零](#vector) |
 | \| _Type_ `modopt` `'('` _TypeReference_ `')'`               | 调用方可以忽略的自定义修饰符。                                                                                | §[[↗]](#modreq&modopt)              |
 | \| _Type_ `modreq` `'('` _TypeReference_ `')'`               | 调用方可以识别的自定义修饰符。                                                                                | §[[↗]](#modreq&modopt)              |
 | \| _Type_ `pinned`                                           | 仅用于局部变量。垃圾收集器不应移动引用的值。                                                                  | §[[↗]](#pinned)                     |
 | \| `typedref`                                                | 类型引用 (即，类型为 `System.TypedReference` 的值)，由 `mkrefany` 创建并由 `refanytype` 或 `refanyval` 使用。 | §[[↗]](#build-in)                   |
 | \| `valuetype` _TypeReference_                               | (未装箱的) 用户定义的值类型                                                                                   | §[[↗]](#valuetype-semantics)        |
 | \| `unsigned int8`                                           | 无符号 8 位整数                                                                                               | §[[↗]](#build-in)                   |
 | \| `unsigned int16`                                          | 无符号 16 位整数                                                                                              | §[[↗]](#build-in)                   |
 | \| `unsigned int32`                                          | 无符号 32 位整数                                                                                              | §[[↗]](#build-in)                   |
 | \| `unsigned int64`                                          | 无符号 64 位整数                                                                                              | §[[↗]](#build-in)                   |
 | \| `void`                                                    | 无类型。只允许作为返回类型或作为 `void *` 的一部分                                                            | §[[↗]](#build-in)                   |

在几种情况下，语法允许使用稍微简单一些的表示法来指定类型；例如，"`System.GC`" 可以代替 "`class System.GC`"。这样的表示法被称为 **类型规范** (_type specifications_)：

 | _TypeSpec_ ::=                         | 参考                   |
 | :------------------------------------- | ---------------------- |
 | `'['` [ `.module` ] *DottedName* `']'` | §[[↗]](#TypeReference) |
 | \| _TypeReference_                     | §[[↗]](#build-in)      |
 | \| _Type_                              | §[[↗]](#Type)          |

#### 5.1.1. modreq & modopt
<a id="modreq&modopt"></a>

使用 **modreq** ("*required modifier*"，必需修饰符) 和 **modopt** ("optional modifier"，可选修饰符) 定义的自定义修饰符与自定义特性相似，不同之处在于修饰符是签名的一部分，而不是附加到声明上的。每个修饰符都将一个类型引用与签名中的一个项目关联起来。

CLI 本身应以相同的方式处理必需和可选修饰符。仅通过添加一个自定义修饰符 (必需或可选) 而进行区别的两个签名不应被认为是匹配的。自定义修饰符对 VES 的操作没有其他影响。

必需和可选修饰符之间的区别对于 CLI 以外的处理元数据的工具 (通常是编译器和程序分析器) 很重要。必需修饰符表示被修改的项目有一种特殊的语义，不应被忽略，而可选修饰符可以简单地被忽略。例如，C 编程语言中的 `const` 限定符可以用一个可选修饰符来建模，例如被调用方法的参数有一个 `const` 限定，但调用方不需要以任何特殊的方式处理它。另一方面，应该在 C++ 中复制构造的参数应用一个必需的自定义特性标记，因为它是进行调用的调用方。    

#### 5.1.2. pinned
<a id="pinned"></a>

**pinned** 的签名编码只能出现在描述局部变量的签名中。当一个具有 **pinned** 局部变量的方法正在执行时，VES 不应重新定位局部变量引用的对象。也就是说，如果 CLI 的实现使用了一个可以移动对象的垃圾收集器，那么收集器不应移动被正在活跃的局部固定变量引用的对象。

如果非托管指针被用来解引用托管对象，这些对象应该被固定。例如，当一个托管对象被传递给一个操作非托管数据的方法时，就需要将该托管对象固定。

>---
### 5.2. 内置类型
<a id="build-in"></a>

CLI 的内置类型在基础类库中有对应的值类型定义。在签名中应该只使用它们的特殊编码来引用它们 (即，不使用通用的 **valuetype** _TypeReference_ 语法)。

>---
### 5.3. TypeReference：用户定义类型的引用
<a id="TypeReference"></a>

用户定义的类型是通过使用它们的完全名称和解析范围引用的，或者使用同一模块中的类型定义。_TypeReference_ 用于捕获完全名称和解析范围：

<pre>
    <em>TypeReference</em> ::= [ <em>ResolutionScope</em> ] <em>DottedName</em> [ '/' <em>DottedName</em> ]*
    <em id="ResolutionScope">ResolutionScope</em> ::= '[' .module <em>Filename</em> ']' | '[' <em>AssemblyRefName</em> ']'
    <em id="AssemblyRefName">AssemblyRefName</em> ::= <em>DottedName</em>
</pre>

以下解析范围是为非嵌套类型指定的：
 * **当前模块 (也是程序集)**：这是最常见的情况，如果没有指定解析范围，则默认为此。只有当定义出现在与引用相同的模块中时，类型才会被解析为定义。引用同一模块和程序集中类型的类型引用最好使用类型定义表示。在不可能 (例如，当引用具有编译器控制的可访问性的嵌套类型时) 或方便 (例如，在一些一次性编译器中) 的情况下，这些类型引用是等效的且可以使用。

 + **不同的模块，当前程序集**：解析范围应是模块引用，语法使用符号 **[** **.module** _Filename_ **]**。只有当引用的模块和类型已经被当前程序集声明，且在程序集的清单中有条目时，类型才会被解析为定义。在这种情况下，清单并未与引用模块一起物理存储。

 * **不同的程序集**：解析范围应是程序集引用，语法使用符号 **[** _AssemblyRefName_ **]**。引用的程序集应该在当前程序集的清单中声明，类型应该在引用的程序集的清单中声明，并且类型应该被标记为从该程序集导出。

 - 对于嵌套类型，解析范围总是封闭类型。这在语法上通过使用 ("`/`") 来分隔封闭类型名称和嵌套类型的名称来表示。

在基础类库中定义的类型 `System.Console` (在名为 `mscorlib` 的程序集中找到) ：

```cil
.assembly extern mscorlib { }
.class [mscorlib]System.Console 
```

引用当前程序集中名为 `x` 的模块中名为 `C.D` 的类型：

```cil
.module extern x
.class [.module x]C.D
```

引用另一个名为 `MyAssembly` 程序集中的 `Foo.Bar` 类型的嵌套 `C` 类型：

```cil
.assembly extern MyAssembly { }
.class [MyAssembly]Foo.Bar/C
```

>---
### 5.4. 本地数据类型
<a id="method-marshal"></a>

一些 CLI 的实现被托管在现有的操作系统或运行时平台之上，这些平台指定了执行某些功能所需的数据类型。元数据通过指定如何将 CLI 的内置和用户定义类型 **封送** (_marshalled_) 到本地数据类型或如何将从本地数据类型封送回 CIL 数据类型，来与这些 *本地数据类型* (_Native data types_) 进行交互。这种封送信息可以被指定为 (使用关键字 **marshal**) ：
 * **方法的返回类型**，表示实际返回了一个本地数据类型，并且应该被封送回指定的 CLI 数据类型。
 - **方法的参数**，表示由调用方提供的 CLI 数据类型被封送到指定的本地数据类型。如果参数是通过引用传递的，那么在调用完成时，更新的值应该从本地数据类型封送回 CLI 数据类型。
 * **用户定义类型的字段**，表示任何试图将其中包含的对象传递给平台方法的尝试都应生成对象的副本，并用指定的本地数据类型替代字段。如果对象是通过引用传递的，那么在调用完成时，更新的值应被封送回 CLI 数据类型。

以下表格列出了 CLI 支持的所有本地类型，并为每个类型提供了描述。所有在 0 ~ 63 (包含) 范围内的编码值都保留用于与现有 CLI 实现的向后兼容。64 ~ 127 范围内的值保留用于此标准和相关标准的未来使用。

 | _NativeType_ ::=                                  | 描述                                                                                                                                                                                                                | 对应类库中 `UnmanagedType` 的枚举项名称 |
 | :------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------- |
 | `'['` `']'`                                       | 本地数组。类型和大小在运行时由实际封送的数组确定。                                                                                                                                                                  | `LPArray`                               |
 | \| `bool`                                         | 布尔值。4 字节整数值，其中任何非零值表示 TRUE，0 表示 FALSE。                                                                                                                                                       | `Bool`                                  |
 | \| `float32`                                      | 32 位浮点数。                                                                                                                                                                                                       | `R4`                                    |
 | \| `float64`                                      | 64 位浮点数。                                                                                                                                                                                                       | `R8`                                    |
 | \| [ `unsigned` ] `int`                           | 有符号或无符号整数，大小足以在平台上保存一个指针                                                                                                                                                                    | `SysUInt` 或 `SysInt`                   |
 | \| [ `unsigned` ] `int8`                          | 有符号或无符号 8 位整数                                                                                                                                                                                             | `U1` 或 `I1`                            |
 | \| [ `unsigned` ] `int16`                         | 有符号或无符号 16 位整数                                                                                                                                                                                            | `U2` 或 `I2`                            |
 | \| [ `unsigned` ] `int32`                         | 有符号或无符号 32 位整数                                                                                                                                                                                            | `U4` 或 `I4`                            |
 | \| [ `unsigned` ] `int64`                         | 有符号或无符号 64 位整数                                                                                                                                                                                            | `U8` 或 `I8`                            |
 | \| `lpstr`                                        | 指向以 null 结尾的 ANSI 字符数组的指针。代码页是实现特定的。                                                                                                                                                        | `LPStr`                                 |
 | \| `lpwstr`                                       | 指向以 null 结尾的 Unicode 字符数组的指针。字符编码是实现特定的。                                                                                                                                                   | `LPWStr`                                |
 | \| `method`                                       | 函数指针。                                                                                                                                                                                                          | `FunctionPtr`                           |
 | \| _NativeType_ `'['` `']'`                       | _NativeType_ 的数组。长度在运行时由实际封送的数组的大小确定。                                                                                                                                                       | `LPArray`                               |
 | \| _NativeType_ `'['` _Int32_ `']'`               | 长度为 _Int32_ 的 _NativeType_ 的数组。                                                                                                                                                                             | `LPArray`                               |
 | \| _NativeType_ `'['` `'+'` _Int32_ `']'`         | 具有运行时提供的元素大小的 _NativeType_ 的数组。_Int32_ 指定了当前方法的一个参数 (从参数编号 0 开始计数)，在运行时，该参数将包含数组元素的大小 (以字节为单位)。只能应用于方法，不能应用于字段。                     | `LPArray`                               |
 | \| _NativeType_ `'['` _Int32_ `'+'` _Int32_ `']'` | 具有运行时提供的元素大小的 _NativeType_ 的数组。第一个 _Int32_ 指定了数组中的元素数量。第二个 _Int32_ 指定了当前方法的哪个参数 (从参数编号 0 开始计数) 将指定数组中的额外元素数量。只能应用于方法，不能应用于字段。 | `LPArray`                               |

方法 `M1` 接受两个参数：一个 `int32`，和一个包含 5 个 `bool` 的数组。

```csharp
int M1([MarshalAs(UnmanagedType.I4)] int v1, 
       [MarshalAs(UnmanagedType.LPArray, SizeConst = 5)] bool[] arr);
```
```cil
.method int32 M1( int32 marshal(int32) v1, 
                  bool[] marshal([5]) arr)
```

方法 `M2` 接受两个参数：一个 `int32`，和一个 `bool` 的数组：该数组中的元素数量由第一个参数的值给出。

```csharp
int M2([MarshalAs(UnmanagedType.I4)] int v1, 
       [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] bool[] arr)
```
```cil
.method int32 M2( int32 marshal(int32) v1, 
                  bool[] marshal(bool[+1]) arr)
```

方法 `M3` 接受两个参数：一个 `int32`，和一个 `bool` 的数组：该数组中的元素数量由 7 加上第一个参数的值给出。

```csharp
int M3([MarshalAs(UnmanagedType.I4)] int v1, 
       [MarshalAs(UnmanagedType.LPArray, SizeConst = 7, SizeParamIndex = 1)] bool[] arr);

```
```cil
 .method int32 M3( int32 marshal(int32) v1, 
                   bool[] marshal(bool[7+1]) arr)
```

---
## 6. 可见性，可访问性和隐藏
<a id="visibility-accessibility-hide"></a>

[第一部分](./01_CLI%20基本概念和体系结构.md/#visible) 中指定了可见性和可访问性。除了这些特性，元数据还存储了关于方法名隐藏的信息。**隐藏** (_hiding_) 控制了从基类型继承的哪些方法名可用于编译时的名称绑定。

>---
### 6.1. 顶级类型的可见性和嵌套类型的可访问性

*可见性* (_Visibility_) 只附加到顶级类型，只有两种可能：对同一程序集内的类型可见，或不受程序集影响的外部类型可见。

```csharp
public class PublicAccessibility;
internal struct AssemblyAccessibility;
```
```cil
.class public {...} PublicAccessibility extends [System.Runtime]System.Object {...}
.class private {...} AssemblyAccessibility extends [System.Runtime]System.ValueType {...}
```

对于嵌套类型 (即，是另一种类型的成员的类型)，嵌套类型具有一个 **可访问性** (_accessibility_)，进一步细化了可以引用该类型的方法集范围。嵌套类型可以具有七种可访问性模式，但没有自己的直接可见性特性，它使用其封闭类型的可见性。

因为顶级类型的可见性控制了其所有成员名称的可见性，所以嵌套类型不能比它的封闭类型更可见。也就是说，如果封闭类型只在程序集内可见，那么具有 **public** 可访问性的嵌套类型仍然只在该程序集内可用。相反，具有 **assembly** 可访问性的嵌套类型即使其封闭类型在程序集外可见，也限制在程序集内使用。

为了使所有类型的编码一致且紧凑，顶级类型的可见性和嵌套类型的可访问性使用逻辑模型中的相同机制进行编码，参见  [***TypeAttributes***](#TypeAttributes)。

>---
### 6.2. 可访问性

可访问性直接在元数据中编码，参见 [「_MethodDef: 0x06_」](#MethodDef_0x06)。

>---
### 6.3. 隐藏

隐藏是一个编译时概念，适用于类型的单个方法。CTS 指定了两种隐藏机制，由单个位指定：

 * *hide-by-name*，意味着在给定类型中引入的名称会隐藏所有具有相同名称的同类继承成员。
 * *hide-by-name-and-sig*，意味着在给定类型中引入的名称会隐藏任何同类的继承成员，它们具有完全相同的类型 (在嵌套类型和字段的情况下) 或签名 (在方法、属性和事件的情况下)。

没有运行时对隐藏的支持。符合 CLI 规范的实现会将所有引用视为标记了 *hide-by-name-and-sig* 的名称，对应于 **hidebysig**。期望实现 *hide-by-name* 效果的编译器可以通过标记方法定义为 **newslot** 特性，并正确选择用于解析方法引用的类型来实现。

```csharp
class Base
{
    public virtual void HideByName() { }
    public void HideBySig() { }
}
class Derive : Base
{
    public override void HideByName() { };
    public new int HideBySig() => default;
}
```
```cil
.class private {...} Base
	extends [System.Runtime]System.Object
{
	.method public hidebysig newslot virtual 
		instance void HideByName () cil managed {...}
	.method public hidebysig 
		instance void HideBySig () cil managed {...}
} 
.class private {...} Derive extends Base
{
	.method public hidebysig virtual 
		instance void HideByName () cil managed {...}
	.method public hidebysig 
		instance int HideBySig () cil managed {...}
}
```

---
## 7. 泛型
<a id="generic-type"></a>

泛型允许使用一种模式来定义一整套类型和方法，其中包含一中叫 **泛型参数** (_generic parameters_) 的占位符。这些泛型参数根据需求被特定类型替换，以实例化实际需要的泛型构造。例如，`class List<T>{}`，表示可能的 `List` 族，`List<string>`、`List<int>` 和 `List<Button>` 是三种可能的实例化。然而，这些类型的 CLS-*compliant* 名称实际上是类 ``List`1<T>``, ``List`1<string>``, ``List`1<int>`` 和 ``List`1<Button>``。

泛型类型由一个名称后跟一个 `<…>` 分隔的泛型参数列表组成，如 `C<T>`。在同一范围内，不应定义具有相同名称但泛型参数数量不同的两个或多个泛型类型。然而，为了在源语言级别允许对泛型参数数目进行重载，定义了 CLS Rule 43 来将泛型类型名称映射到唯一的 CIL 名称。该规则规定，具有一个或多个泛型参数的类型 `C` 的 CLS-*compliant* 名称，应该有一个形式为 "`` `n ``" 的后缀，其中 `n` 是一个十进制整数常量 (没有前导零)，表示 `C` 具有的泛型参数的数量。例如：类型 `C`，`C<T>` 和 `C<K,V>` 的 CLS 规范名称分别是 `C`，``C`1<T>`` 和 ``C`2<K,V>``。所有标准库类型的名称都是符合 CLS 规范的；例如，``System.Collections.Generic.IEnumerable`1<T>``。

在详细讨论泛型之前，这里是一些新术语的定义：
 * ``public class List`1<T> {}`` 是一个 *泛型类型定义*。
 * `<T>` 是一个泛型参数列表，`T` 是一个泛型参数。
 * ``List`1<T>`` 是一个 *泛型类型*；它有时被称为 *泛型类型* 或 *开放泛型类型*，因为它至少有一个泛型参数。
 * ``List`1<int>`` 是一个 *封闭泛型类型*，因为它没有未绑定的泛型参数。 (它有时被称为 *实例化的* 泛型类型或泛型类型 *实例化*)。
 * 泛型包括既不严格开放也不严格封闭的泛型类型；例如，从给定 ``.public class B`2<T,U> {}`` 的基类 `B` 派生的 ``.public class D`1<V> extends B`2<!0,int32> {}`` 类型。
 * 如果需要区分泛型类型和普通类型，后者被称为 *非泛型类型*。
 * `<int>` 是一个泛型参数列表，`int` 是一个泛型参数。
 * 本标准保持了泛型参数和泛型实参之间的区别。如果可能的话，当谈到 ``List`1<int>`` 时，使用短语 "`int` 是用于泛型参数 `T` 的类型"。 (在反射中，这有时被称为 "`T` 被绑定到 `int`") 
 * "`(C1, …, Cn) T`" 中的 `(C1, …, Cn)` 是对泛型参数 `T` 的 _泛型参数约束_。

```csharp
class C<S,T> where S: I1,I2 where T : Base, I3;
```
```cil
class C`2<(I1,I2) S, (Base,I3) T> { … }
```

这表示一个名为 `C` 的类，有两个泛型参数，`S` 和 `T`。`S` 被约束为实现两个接口，`I1` 和 `I2`。`T` 被约束为从类 `Base` 派生，并且还要实现接口 `I3`。

在泛型类型定义中，其泛型参数是通过它们的索引引用的。泛型参数零被引用为 `!0`，泛型参数一被引用为 `!1`，依此类推。同样，在泛型方法定义的主体中，其泛型参数是通过它们的索引引用的；泛型参数零被引用为 `!!0`，泛型参数一被引用为 `!!1`，依此类推。

```csharp
class C<S, T> : Base<S, T>
{
    public void Fun<K>(K k) { }
}
```
```cil
.class private {...} C`2<S, T> extends class Base`2<!0, !1>
{
    .method public hidebysig instance void Fun<K> (!!K k) cil managed {...}
}
```

### 7.1. 泛型类型定义
<a id="generic-define"></a>

泛型类型定义是包含泛型参数的定义。每个这样的泛型参数可以有一个名称和一个可选的约束集 —— 泛型参数应该是 *可赋值给* 的类型。也允许使用可选的变体符号。泛型参数在以下声明中是有效的：

 * 其约束 (例如，``.class … C`1<(class IComparable`1<!0>) T>)``) 

 * 任何基类，类型定义从其派生 (例如，``.class … MultiSet`1<T> extends class Set`1<!0[]>``) 

 * 任何类型定义实现的接口 (例如，``.class … Hashtable`2<K,D> implements class IDictionary`2<!0,!1>``) 

 * 所有成员 (实例和静态字段、方法、构造函数、属性和事件)，除了嵌套类。C# 允许在嵌套类中使用来自封闭类的泛型参数，但在元数据中为嵌套类定义添加任何所需的额外泛型参数。

泛型类型定义可以包括静态、实例和虚方法。

泛型类型定义受到以下限制：

 * 泛型参数本身不能用于指定基类或任何实现的接口。例如，``.class … G`1<T> extends !0`` 是无效的。然而，当基类或接口嵌套在另一个泛型类型中时，使用该泛型参数是有效的。例如，``.class … G`1<T> extends class H`1<!0>`` 和 ``.class … G`1<T> extends class B`2<!0,int32>`` 是有效的。这允许在定义时而不是在实例化时检查泛型类型的有效性。例如，在 ``.class … G`1<T> extends !0`` 中，派生不知道哪些方法将覆盖其他方法，因为关于基类的信息不可用；实际上，甚至不知道 `T` 是否是一个类：它可能是一个数组或一个接口。同样对于 ``.class … C`2<(!1)T,U>``，对 `U` 的定义一无所知。

 + 可变参数方法不能是泛型类型的成员。实现这个特性需要相当大的努力。由于可变参数在目标 CLI 的语言中的使用非常有限，所以决定从泛型类型中排除可变参数方法。

 * 当忽略泛型参数时，继承或接口层次结构中不应有循环。假设定义一个图，其节点是可能的泛型 (但开放的) 类和接口，其边是以下内容：

     * 如果一个 (可能的泛型) 类或接口 _D_ 扩展或实现一个类或接口 _B_，则从 _D_ 到 _B_ 添加一条边。
     * 如果一个 (可能的泛型) 类或接口 _D_ 扩展或实现一个实例化的类或接口 _B_&lt;_T1_, ..., _Tn_&gt;，则从 _D_ 到 _B_ 添加一条边。
     * 如果图中不包含循环，则图是有效的。 (这个算法是非泛型类型规则的自然推广。) 

>---
### 7.2. 泛型和递归继承图
<a id="generic-inherit"></a>

尽管继承图不能直接循环，但在父类或接口中给出的实例化可能引入直接或间接的循环依赖，其中一些是允许的 (例如，`C : IComparable<C>`)，而一些是不允许的 (例如，给定 `class B<U>` 时，`class A<T> : B<A<A<T>>>`)。

每个类型定义都应生成一个有限的实例化闭包。实例化闭包定义如下：

 1. 创建一个包含单个泛型类型定义的集合。
 2. 通过添加在集合中所有类型的基类和实现接口的类型签名中引用的所有泛型类型，形成这个集合的闭包。在这个集合中包括嵌套实例化，所以一个引用类型 `Stack<List<T>>` 实际上既实例化 `List<T>` 也实例化 `Stack<List<T>>`。
 3. 构造一个图：
     * 其节点是集合中类型的形式类型参数。根据需要使用 ***α-renaming*** (又 ***α-conversion***) 来避免名称冲突。
     * 如果 _T_ 出现为在某个引用类型 _D_\<&hellip;, _U_, &hellip;\> 中要替换 _U_ 的实际类型参数，从 _T_ 到 _U_ 添加一个非扩展边 (&rarr;)。
     * 如果 _T_ 出现在引用类型 _D_\<&hellip;, _U_, &hellip;\> 中要替换 _U_ 的实际类型参数内部 (但不作为)，从 _T_ 到 _U_ 添加一个扩展边 (&rArr;)。

一个扩展循环是在实例化闭包中包含至少一个扩展边 (&rArr;) 的循环。当且仅当上述构造的图中不包含扩展循环时，系统的实例化闭包是有限的。

> 扩展循环示例

```csharp
class B<U>;
class A<T> : B<A<A<T>>>;
```

生成边 (使用 &rArr; 表示扩展边，&rarr; 表示非扩展边) 

 * `T` &rarr; `T` (由引用类型 `A<T>` 生成)
 * `T` &rArr; `T` (由引用类型 `A<A<T>>` 生成)，是一个扩展循环
 * `T` &rArr; `U` (由引用类型 `B<A<A<T>>>` 生成)

这个图包含一个扩展循环，所以实例化闭包是无限的。

> 有限闭包示例

```csharp
class B<U>;
class A<T> : B<A<T>>;
```

生成边 (使用 &rArr; 表示扩展边，&rarr; 表示非扩展边) 

 * `T` &rarr; `T` (由引用类型 `A<T>` 生成)
 * `T` &rArr; `U` (由引用类型 `B<A<T>>` 生成)

这个图不包含扩展循环，所以实例化闭包是有限的。

> 非扩展循环示例

```csharp
class P<T>
class C<U,V> : P<D<V,U>>
class D<W,X> : P<C<W,X>>
```

生成边 (使用 &rArr; 表示扩展边，&rarr; 表示非扩展边) 

 * `U` &rarr; `X`, `V` &rarr; `W`, `U` &rArr; `T`, `V` &rArr; `T` (由引用类型 `D<V,U>` 和 `P<D<V,U>>` 生成)
 * `W` &rarr; `U`, `X` &rarr; `V`, `W` &rArr; `T`, `X` &rArr; `T` (由引用类型 `C<W,X>` 和 `P<C<W,X>>` 生成)

这个图包含非扩展循环 (例如 `U` &rarr; `X` &rarr; `V` &rarr; `W` &rarr; `U`)，但没有扩展循环，所以实例化闭包是有限的。

>---
### 7.3. 泛型方法定义

泛型方法定义是包含泛型参数列表的定义。泛型方法可以在非泛型类型中定义；或者在泛型类型中定义，在这种情况下，方法的泛型参数应该是其包含类型泛型参数的补充。与泛型类型定义一样，泛型方法定义上的每个泛型参数都有一个名称和一个可选的约束集。

泛型方法可以是静态的、实例的或虚拟的。类的静态或实例构造器 (分别为 `.cctor` 或 `.ctor`) 不能是泛型的。

方法的泛型参数在方法的签名和主体以及泛型参数约束中都是有效的。签名包括方法的返回类型。所以在 ``.method … !!0 M`1<T>() { … }`` 中，`!!0` 是有效的 —— 它是 ``M`1<T>`` 的泛型参数，即使它在声明中先于该参数。

泛型实例 (虚拟和非虚拟) 方法可以定义为泛型类型的成员，在这种情况下，泛型类型和泛型方法的泛型参数都在方法签名和主体以及方法泛型参数的约束中是有效的。

>---
### 7.4. GenArgs：实例化泛型类型 
<a id="GenArgs"></a>

_GenArgs_ 用于表示泛型参数列表：

<pre>
    <em>GenArgs</em> ::= <em>Type</em> [ ',' <em>Type</em> ]*
</pre>

如果一个类型不包含泛型参数，我们说它是 **封闭的** (_closed_)；否则，它是 **开放的** (_open_)。

给定的泛型类型定义可以用 **泛型参数** (_generic arguments_) 进行 **实例化** (_instantiated_)，以产生一个实例化类型。在实例化中的泛型参数的数量应与类型或方法定义中指定的泛型参数的数量相匹配。

```cil
// C# class: new MyList<int>()
newobj instance void class MyList`1<int32>::.ctor()
// C# struct: new Pair<int, Pair<string, int>>();
initobj valuetype Pair`2<int32, valuetype Pair<string,int32>>
```

CLI 不支持部分实例化泛型类型。并且，泛型类型不应在元数据签名的二进制信息中的任何地方出现未实例化。

以下类型不能用作实例化 (泛型类型或方法) 的参数：
 * 托管指针 **byref** 类型 (例如，``System.Generic.Collection.List`1<string&>`` 是无效的)。**byref** 参见第一部分 [[↗]](./01_CLI%20基本概念和体系结构.md/#byref)
 * **byref-like** 类型，即包含可以指向 CIL 求值堆栈的字段的值类型 (例如，`List<System.RuntimeArgumentHandle>` 是无效的)。**byref-like** 参见第一部分 [[↗]](./01_CLI%20基本概念和体系结构.md/#byref-like)
 * 类型引用 **typed reference** 类型 (例如，`List<System.TypedReference>` 是无效的)。**typed reference** 参见第一部分 [[↗]](./01_CLI%20基本概念和体系结构.md/#typedref)
 * 非托管指针 "`*`" 类型 (例如，`List<int32*>` 是无效的)
 * `void` (例如，`List<System.Void>` 是无效的) 

**byref**s 类型不能用作泛型参数，实际上大多数的实例化将是无效的。由于在泛型类型中不允许将 **byrefs** 作为字段类型或方法返回类型，因此例如在 ``MyList`1<string&>`` 的定义中，不能声明类型为 `!0` 的字段，也不能返回类型为 `!0` 的方法。

非托管指针被禁止，因为根据当前的规范，非托管指针在技术上不是 `System.Object` 的子类。这个限制可以被取消，但是目前运行时强制执行这个限制。
 
实例化类型的对象应携带足够的信息，以便在运行时恢复它们的确切类型 (包括它们的泛型参数的类型和数量)。这是为了正确实现类型转换和实例测试，以及在反射能力 (`System.Object::GetType`) 中使用。

>---
### 7.5. 泛型参数的协变和逆变
<a id="variant"></a>

CLI 支持泛型参数的协变和逆变，但只在接口和委托类的签名中。符号 "`+`" 用于表示协变的泛型参数，而 "`-`" 用于表示逆变的泛型参数。一般在方法中协变的泛型参数可以用于方法返回，不能用作方法参数；而逆变的泛型参数可以用于方法参数，而不能用作方法返回。

假设有一个泛型接口，它在其一个泛型参数中是协变的，如 ``IA`1<+T>``。只要 ``GenArgB`` := ``GenArgA`` 符合赋值兼容性，那么所有的实例都满足 ``IA`1<GenArgB>`` := ``IA`1<GenArgA>``。例如类型为 ``IA`1<string>`` 的实例可以赋值给类型为 ``IA`1<object>`` 的局部变量。

泛型逆变在相反的意义上操作：假设有一个逆变的接口 ``IB`1<-T>``，只要 ``GenArgA`` := ``GenArgB``，那么 ``IB`1<GenArgB>`` := ``IB`1<GenArgA>``。

```csharp
// 协变参数可以用作结果类型
interface IEnumerator<+T>  // C# 使用 out 表示协变，因此实际写作 IEnumerator<out T>
{
    T Current { get; }
    bool MoveNext();
}
// 协变参数可以用在协变的结果类型中
interface IEnumerable<+T>
{
    IEnumerator<T> GetEnumerator();
}
// 逆变参数可以用作参数类型
interface IComparer<-T> // C# 使用 in 表示逆变，因此实际写作 IComparer<in T>
{
    bool Compare(T x, T y);
}
// 逆变参数可以用在逆变的接口类型中
interface IKeyComparer<-T> : IComparer<T>
{
    bool Equals(T x, T y);
    int GetHashCode(T obj);
}
// 一个逆变的委托类型
delegate void EventHandler<-T>(T arg);
// 没有注解表示非变化。非变量参数可以用在任何地方。
// 以下类型应该是非变量的，因为T出现在方法参数中
// 以及在协变接口类型中
interface ICollection<T> : IEnumerable<T>
{
    void CopyTo(T[] array, int index);
    int Count { get; }
}
```

>---
### 7.6. 实例化类型的赋值兼容性

假设 `Employee` := `Manager`，

```csharp
IEnumerable<Manager> eManager = ...
IEnumerable<Employee> eEmployee = eManager;               // 协变

IComparer<object> objComp = ...
IComparer<string> strComp = objComp;                      // 逆变

EventHandler<Employee> employeeHandler = ...
EventHandler<Manager> managerHandler = employeeHandler;   // 逆变
```

则给定以下内容：

```csharp
// 接口变体
interface IConverter<-T,+U> {
    U Convert(T x);
}
IConverter<string, object> := IConverter<object, string>

// 委托变体
delegate U Function<-T,+U>(T arg);
Function<string, object> := Function<object, string>.
```

则给定以下内容：

```csharp
IComparer<object> objComp = ...
// 逆变和接口继承
IKeyComparer<string> strKeyComp = objComp; 

IEnumerable<string[]> strArrEnum = …
// 在 IEnumerable 和数组上的协变
IEnumerable<object[]> objArrEnum = strArrEnum;

IEnumerable<string>[] strEnumArr = ...
// 在 IEnumerable 和数组上的协变
IEnumerable<object>[] objEnumArr = strEnumArr; 

IComparer<object[]> objArrComp = ...
// 在 IComparer 和数组上的逆变
IComparer<string[]> strArrComp = objArrComp; 

IComparer<object>[] objCompArr = ...
// 在 IComparer 和数组上的逆变
IComparer<string>[] strCompArr = objCompArr;
```

>---
### 7.7. 成员签名的有效性

为了实现类型安全，有必要对协变和逆变泛型类型的成员签名的良构性施加额外的要求。

 * 协变参数只能出现在类型定义的 "*producer* "、"*reader* " 或 "*getter* " 位置；即在
     * 方法的结果类型
     * 继承的接口

 - 逆变参数只能出现在类型定义的 "*consumer* "、"*writer* " 或 "*setter* " 位置；即在
     * 方法的参数类型

 * 非变量参数可以出现在任何地方。

> generic type definition

**泛型类型定义** (_generic type definition_)：如果 _G_ 是一个接口或委托类型，给定 _S_ = \<_var_<sub>1</sub> _T_<sub>1</sub>, &hellip;, _var_<sub>_n_</sub> _T_<sub>_n_</sub>\> (其中 _var_<sub>_n_</sub> 是 `+`、`-` 或 *无*)，如果以下每条都成立，则泛型类型定义 _G_\<_var_<sub>1</sub> _T_<sub>1</sub>, &hellip;, _var_<sub>_n_</sub> _T_<sub>_n_</sub>\> 是有效的：
 * 每个实例方法和虚方法声明都相对于 _S_ 是有效的
 * 每个继承的接口声明都相对于 _S_ 是有效的
 * 对静态成员、实例构造器或类型自己的泛型参数约束没有限制。

给定带注解的泛型参数 _S_ = \<_var_<sub>1</sub> _T_<sub>1</sub>, &hellip;, _var_<sub>_n_</sub> _T_<sub>_n_</sub>\>，我们定义类型定义的各个组件相对于 _S_ 都是有效的。我们定义一个对注解的否定操作，写作 &not;_S_，表示 “将负数翻转为正数，正数翻转为负数”。定义：
 * “相对于 _S_ 有效” 表示具有 “协变行为”
 * “相对于 &not;_S_ 有效” 表示具有 “逆变行为”
 * “相对于 _S_ 和 &not;_S_ 有效” 表示具有 “非变体行为”。即表示，所有出现的泛型参数都应该是非变体。

> Method

**方法**：若要方法签名 _t_ _method_(_t_<sub>1</sub>, &hellip;, _t_<sub>_n_</sub>) 相对于 _S_ 是有效的，则需要满足：
 * 它的结果类型签名 _t_ 相对于 _S_ 是有效的；并且
 * 每个参数类型签名 _t_<sub>_i_</sub> 相对于 &not;_S_ 是有效的。
 * 每个方法泛型参数约束类型 _t_<sub>_j_</sub> 相对于 &not;_S_ 是有效的。

结果表现为协变，参数表现为逆变。泛型参数的约束也表现为逆变。

> Type signatures

**类型签名**：如果 _t_ 是
 * 非泛型类型 (例如，普通的类或值类型)，
 * 泛型参数 _T_<sub>_i_</sub>，其中 _var_<sub>_i_</sub> 是 `+` 或 _无_ (即，它是一个标记为协变或非变体的泛型参数)，
 * 数组类型 _u_ [ ]，并且 _u_ 相对于 _S_ 是有效的；如果数组类型表现为协变，
 * 闭合的泛型类型 _G_\<_t_<sub>1</sub>,&hellip;,_t_<sub>_n_</sub>\>，对于每一个都满足时：
   * 如果 _G_ 的第 _i_ 个参数被声明为协变，则 _t_<sub>i</sub> 相对于 _S_ 是有效的；
   * 如果 _G_ 的第 _i_ 个参数被声明为逆变，则 _t_<sub>_i_</sub> 相对于 &not;_S_ 是有效的；
   * 如果 _G_ 的第 _i_ 个参数被声明为非变体，则 _t_<sub>_i_</sub> 相对于 _S_ 和相对于 &not;_S_ 是有效的；

则类型签名 _t_ 相对于 _S_ 是有效的。

>---
### 7.8. 签名和绑定

泛型类型的成员 (字段和方法) 在 CIL 指令中使用元数据 _token_ 引用，该 _token_ 指定了 _MemberRef_ 中的一个项。抽象地说，引用由两部分组成：
 1. 声明成员的类型，在这种情况下，是泛型类型定义的实例化。例如：``IComparer`1<String>``。
 2. 成员的名称和泛型 (未实例化) 签名。例如：``int32 Compare(!0,!0)``。

对于不同的成员，在实例化时可能具有相同的类型，它们可以通过 _MemberRef_ 区分。

```cil
.class public C`2<S,T> {
    .field string f
    .field !0 f
    .method instance void m(!0 x) {...}
    .method instance void m(!1 x) {...}
    .method instance void m(string x) {...}
}
```

封闭类型 ``C`2<string,string>`` 是有效的：它有三个名为 `m` 的方法，所有的参数类型都相同；和两个名为 `f` 的字段，类型相同。它们都通过上述的 _MemberRef_ 编码区分：

```cil
string C`2<string, string>::f
!0  C<string, string>::f
void C`2<string, string>::m(!0)
void C`2<string, string>::m(!1)
void C`2<string, string>::m(string)
```

源语言可能如何解决这种重载的方式取决于每种单独的语言。有许多语言可能不允许这样的重载。

>---
### 7.9. 继承和重写

在泛型存在的情况下，这个定义以明显的方式扩展。具体来说，为了确定一个成员是否隐藏 (对于静态或实例成员) 或重写 (对于虚方法) 来自基类或接口的成员，只需将每个泛型参数替换为其泛型实参，并比较结果成员签名。

假设有一个基类 `B` 和一个派生类 `D` 的定义。

```cil
.class B
{ .method public virtual void V(int32 i) { … } }

.class D extends B
{ .method public virtual void V(int32 i) { … } }
```

在类 `D` 中，`D.V` 重写了继承的方法 `B.V`，因为它们的名称和签名匹配。

在泛型存在的情况下，这个简单的例子如何扩展？其中类 `D` 派生自泛型实例化：

```cil
.class B`1<T>
{ .method public virtual void V(!0) { … } }

.class D extends B`1<int32>
{ .method public virtual void V(int32) { … } }

.class E extends B`1<string>
{ .method public virtual void V(int32) { … } }
```

类 `D` 派生自 `B<int32>`。并且类 `B<int32>` 定义了方法：

```cil
public virtual void V(int32 t) { … }
```

我们只需将 `B` 的泛型参数 `T` 替换为特定的泛型参数 `int32`，它与方法 `D.V` 匹配 (名称和签名相同)。因此，由于上述非泛型示例中的相同原因，很明显 `D.V` 重写了继承的方法 `B.V`。

与此形成对比的是类 `E`，它派生自 `B<string>`。在这种情况下，用字符串替换 `B` 的 `T`，我们看到 `B.V` 有这个签名：

```cil
public virtual void V(string t) { … }
```

这个签名与方法 `E.V` 不同，因此它并没有重写基类的 `B.V` 方法。

如果在替换基类泛型参数后，两个方法导致相同的名称和签名 (包括返回类型)，则可能导致类型定义无效。以下内容说明了这一点：

```cil
.class B`1<T>
{ 
    .method public virtual void V(!0 t)     { … }
    .method public virtual void V(string x) { … }
}

.class D extends B`1<string> { } 
```

类 `D` 在某些语言中可能无效，因为它将从 `B<string>` 继承两个具有相同签名的方法：

```cil
void V(string)
```

然而，下面的 `D` 版本是有效的：

```cil
.class D extends B`1<string>
{ 
    .method public virtual void  V(string t)  { … }
    .method public virtual void  W(string t)
    {   
        …
        .override  method instance void class B`1<string>::V(!0)
        …
    }
}
```

当重写泛型方法 (即，具有自己的泛型参数的方法) 时，泛型参数的数量必须完全匹配被重写的方法。如果被重写的泛型方法对其泛型参数有一个或多个约束，那么：
 * 重写方法只能对相同的泛型参数有约束；
 * 由重写方法指定的任何泛型参数的约束都不能比被重写方法为相同泛型参数指定的约束更严格； 

在重写方法的主体中，只有直接在其签名中指定的约束才适用。当一个方法被调用时，将执行与 `call` 或 `callvirt` 指令中的元数据 _token_ 关联的约束。

>---
### 7.10. 显式方法重写

无论是泛型还是非泛型，类型都可以使用显式重写来实现特定的虚方法 (无论该方法是在接口还是基类中引入的)。

在泛型存在的情况下，重写规则的扩展如下：

 * 如果实现方法是非泛型类型或封闭泛型类型的一部分，那么声明方法应该是该类型的基类或该类型实现的接口的一部分。

    ```cil
    .class interface I`1<T>
    { 
        .method public abstract virtual void M(!0) {}
    }
   
    .class C implements class I`1<string>
    { 
        .override method instance void class I`1<string>::M(!0) with
               method instance void class C::MInC(string)
        .method virtual void MInC(string s)
        { 
            ldstr "I.M"
            call void [mscorlib]System.Console::WriteLine(string)
            ret
        }
    }
    ```

 * 如果实现方法是泛型的，那么声明的方法也应该是泛型的，并且应该具有与被重写的方法相同数量的方法泛型参数。实现方法和声明方法都不应该是实例化的泛型方法。这意味着实例化的泛型方法不能用于实现接口方法，也不可能为具有特定泛型参数的泛型方法实例化提供特殊方法。
   
    ```cil
    .class interface I
    { 
        .method public abstract virtual void M<T>(!!0) {}
        .method public abstract virtual void N() {}
    }
    ```

   下面的任何一个 **.override** 语句都是不允许的

    ```cil
    .class C implements class I`1<string>
    { 
        .override class I::M<string> with instance void class C::MInC(string)
        .override class I::N with instance void class C::MyFn<string>
        .method virtual void MInC(string s) { … }
        .method virtual void MyFn<T>() { … }
    }
    ```

>---
### 7.11. 泛型参数的约束

在泛型类或泛型方法上声明的泛型参数可以被一个或多个类型 (参考 [「_GenericParamConstraint: 0x2C_」](#GenericParamConstraint_0x2C))，和一个或多个 [*特殊约束*](#special-genpars) **约束** (_constrained_)。泛型参数只能使用满足所有指定特殊约束并且是 *可赋值给* (当装箱时) 每个声明的约束的泛型参数实例化。

泛型参数约束应至少具有与泛型类型定义或泛型方法定义本身相同的可见性。

对泛型参数约束没有其他限制。特别是，以下用法是有效的：
 * 泛型类的泛型参数的约束可以递归引用泛型参数，甚至可以引用类本身。

   ```cil
   .class public Set`1<(class IComparable`1<!0>) T> { … }

   // 表示只能由派生类实例化！
   .class public C`1<(class C<!0>) T> {} 
   .class public D extends C`1<class D> { … } 
   ```

 * 泛型方法的泛型参数约束可以递归引用泛型方法和其封闭类 (如果是泛型) 的泛型参数。约束也可以引用封闭类本身。

   ```cil
   .class public A`1<T> {
       .method public void M<(class IDictionary<!0,!!0>) U>() {}
   }
   ```

 * 泛型参数约束可以是泛型参数或非泛型类型，如数组。

   ```cil
   .class public List`1<T> {
       // U 的约束是 T 本身
       .method public void AddRange<(!0) U>(class IEnumerable`1<!!0> items) { … }
   }
   ```

泛型参数可以有多个约束：最多继承一个基类 (如果没有指定，CLI 默认继承自 `System.Object`) ；并实现零个或多个接口。以下声明显示了一个泛型类 `OrderedSet<T>`，其中泛型参数 `T` 被约束为既继承自类 `Employee`，又实现接口 `IComparable<T>`：

 ```cil
 .class OrderedSet`1<(Employee, class [mscorlib]System.IComparable`1<!0>) T> { … }
 ```
 
对泛型参数的约束只限制了泛型参数可以用哪些类型实例化。**验证** 要求，通过满足约束的已知泛型参数提供的字段、属性或方法，不能通过泛型参数直接访问 / 调用，除非它最先被装箱，或者 `callvirt` 指令的前缀是 `constrained.` 前缀指令。

>---
### 7.12. 对泛型类型的成员的引用

引用泛型类型成员的 CIL 指令被一般化，以允许引用封闭构造类型的成员。引用中指定的泛型参数的数量应与泛型类型定义中指定的类型参数数量匹配。引用方法的 CIL 指令被一般化，以允许引用封闭构造类型的泛型方法。

---
## 8. 类型定义 
<a id="class"></a>

类型 (即，类、值类型和接口) 可以在模块的顶级定义：

<pre>
    <em>Decl</em> ::= .class <em>ClassHeader</em> '{' <em>ClassMember</em>* '}' | ...
</pre>

此声明创建的逻辑元数据表在 [「_TypeDef: 0x02_」](#TypeDef_0x02) 中指定。

出于历史原因，用于定义类型的许多语法类别在其名称中错误地使用了 “class” 而不是 “type”。所有的类都是类型，但是 “types” 是一个更广泛的术语，包括值类型和接口。

>---
### 8.1. ClassHeader：Type Header

***Type Header*** 包括：

 * 任意数量的类型特性
 * 可选的泛型参数
 * 一个名称 (一个 _Id_) 
 * 一个基类型 (或基类类型)，默认为 `[mscorlib]System.Object`，以及
 * 一个可选的接口列表，该类型及其所有派生类型都应满足这些接口的协议。

<pre>
    <em>ClassHeader</em> ::= <em>ClassAttr</em>I* <em>Id</em> [ '<' <em>GenPars</em> '>' ] [ extends <em>TypeSpec</em> [ implements <em>TypeSpec</em> ] [ ',' <em>TypeSpec</em> ]* ]
</pre>

在定义泛型类型时使用可选的泛型参数 _GenPars_ [[↗]](#special-genpars)。

<a id="extends"></a>**extends** 关键字指定了类型的 **基类型** (_base Type_)。一个类型应该只从另一个类型扩展。如果没有指定类型，*ilasm* 将添加一个 **extends** 子句使类型继承自 `System.Object`。

<a id="implements"></a>**implements** 关键字指定了类型的 **接口** (_interfaces_)。在此处列出的接口，类型为其声明的所有具体实现都将支持该接口的协议，包括提供该接口声明的任何虚方法的实现。

在 **implements** 关键字后的 _TypeSpec_ 的从左到右的顺序在 [「_InterfaceImpl: 0x09_」](#InterfaceImpl_0x09) 表中被保留为从上到下的顺序。这是为了支持接口调度中的 [差异解析](#internal-virtual) 时所必需的。

下面这段代码声明了类 `CounterTextBox`，它扩展了程序集 `System.Windows.Forms` 中的类 `System.Windows.Forms.TextBox`，并实现了当前程序集的模块 `Counter` 中的接口 `CountDisplay`。特性 **private**、**auto** 和 **autochar** 在后面的子小节中有描述。

```cil
.class private auto autochar CounterTextBox
    extends [System.Windows.Forms]System.Windows.Forms.TextBox
    implements [.module Counter]CountDisplay
{ // 类的主体 }
```

类型可以附加任意数量的自定义特性。自定义特性的附加方式如 [**custom**](#custom) 节所述。类型的其他 (预定义) 特性可以分为指定可见性、类型布局信息、类型语义信息、继承规则、互操作信息和特殊处理信息的特性。以下各小节对每组预定义特性提供了更多信息。

 | _ClassAttr_ ::=         | 描述                                                 | 参考                                   |
 | :---------------------- | ---------------------------------------------------- | -------------------------------------- |
 | `abstract`              | 类型是抽象的。                                       | §[[↗]](#abstract)                      |
 | \| `ansi`               | 将字符串作为 ANSI 封送到平台。                       | §[[↗]](#ansi)                          |
 | \| `auto`               | 字段的布局由 CLI 自动提供。                          | §[[↗]](#auto)                          |
 | \| `autochar`           | 将字符串作为 ANSI 或 Unicode (平台特定) 封送到平台。 | §[[↗]](#autochar)                      |
 | \| `beforefieldinit`    | 在调用静态方法之前不需要初始化类型。                 | §[[↗]](#beforefieldinit)               |
 | \| `explicit`           | 字段的布局是明确提供的。                             | §[[↗]](#explicit)                      |
 | \| `interface`          | 声明一个接口。                                       | §[[↗]](#type-semantics-attr)           |
 | \| `nested assembly`    | 嵌套类型的 *assembly* 可访问性。                     | §[[↗]](#visibility-accessibility-attr) |
 | \| `nested famandassem` | 嵌套类型的 *family* 和 *assembly* 可访问性。         | §[[↗]](#visibility-accessibility-attr) |
 | \| `nested family`      | 嵌套类型的 *family* 可访问性。                       | §[[↗]](#visibility-accessibility-attr) |
 | \| `nested famorassem`  | 嵌套类型的 *family* 或 *assembly* 可访问性。         | §[[↗]](#visibility-accessibility-attr) |
 | \| `nested private`     | 嵌套类型的 *private* 可访问性。                      | §[[↗]](#visibility-accessibility-attr) |
 | \| `nested public`      | 嵌套类型的 *public* 可访问性。                       | §[[↗]](#visibility-accessibility-attr) |
 | \| `private`            | 顶级类型的 *private* 可见性。                        | §[[↗]](#visibility-accessibility-attr) |
 | \| `public`             | 顶级类型的 *public* 可见性。                         | §[[↗]](#visibility-accessibility-attr) |
 | \| `rtspecialname`      | 运行时的特殊处理。                                   | §[[↗]](#rtspecialname)                 |
 | \| `sealed`             | 该类型不能被派生。                                   | §[[↗]](#sealed)                        |
 | \| `sequential`         | 字段的布局是顺序的。                                 | §[[↗]](#sequential)                    |
 | \| `serializable`       | 保留，表示此类型可以被序列化。                       | §[[↗]](#serializable)                  |
 | \| `specialname`        | 工具可能会进行特殊处理。                             | §[[↗]](#specialname)                   |
 | \| `unicode`            | 将字符串作为 Unicode 封送到平台。                    | §[[↗]](#unicode)                       |

#### 8.1.1. ClassAttr：可见性和可访问性特性
<a id="visibility-accessibility-attr"></a>

<pre>
    <em>ClassAttr</em> ::= ... | nested assembly | nested famandassem | nested family | nested famorassem | nested private | nested public | private | public 
</pre>

可见性和可访问性参考第一部分的 [可见性与可访问性](./01_CLI%20基本概念和体系结构.md/#visible)。

一个不在其他类型内部的类型应该有且仅有一个可见性 (**private** 或 **public**)，并且不应该有可访问性。嵌套类型不应该有可见性，但是应该有且仅有一个可访问性特性 **nested assembly**、**nested famandassem**、**nested family**、**nested famorassem**、**nested private** 或 **nested public**。顶级类型的默认可见性是 **private**。嵌套类型的默认可访问性是 **nested private**。

```csharp
// file: Test.cs
file class CompilerControlledVisibility;
public class PublicAccessibility
{
    public class PublicVisibility;
    internal class AssemblyVisibility;
    internal protected class FamilyOrAssemblyVisibility;
    protected class FamilyVisibility;
    private protected class  FamilyAndAssemblyVisibility;
    private class PrivateVisibility;
}
```
```cil
.class private {...}  // 限制在 Test.cs 编译单元内
'<Test>FE0E857C12E4F36F091DEE5BFB3AA3BEF22492C30764B343CB0BDF28898FC3C92__CompilerControlledVisibility'
	extends [System.Runtime]System.Object {...}

.class public {...} PublicVisibility extends [System.Runtime]System.Object
{
	// Nested Types
	.class nested public {...} PublicVisibility	extends [System.Runtime]System.Object {...}
	.class nested assembly {...} AssemblyVisibility extends [System.Runtime]System.Object {...}
	.class nested famorassem {...} FamilyOrAssemblyVisibility extends [System.Runtime]System.Object {...}
	.class nested family {...} FamilyVisibility extends [System.Runtime]System.Object {...}
	.class nested famandassem {...} FamilyAndAssemblyVisibility extends [System.Runtime]System.Object {...}
	.class nested private {...} PrivateVisibility extends [System.Runtime]System.Object {...}
}
```

#### 8.1.2. ClassAttr：类型布局特性
<a id="layout-attr"></a>

<pre>
    <em>ClassAttr</em> ::= ... | auto | explicit | sequential
</pre>
 
类型布局指定了类型实例的字段如何排列。给定的类型应该只指定一个布局特性。按照惯例，如果没有指定布局特性，*ilasm* 将提供 **auto**。布局特性包括：
- **auto**<a id="auto"></a>：布局应由 CLI 完成，没有用户提供的约束。
- **explicit**<a id="explicit"></a>：字段的布局是 [明确提供](#ctrl-layout) 的。然而，泛型类型不应该有明确的布局。
- **sequential**<a id="sequential"></a>：CLI 应该根据逻辑元数据表 ([「_Field: 0x04_」"](#Field_0x04)) 中字段的顺序来排列字段。

默认的 **auto** 布局应该为正在执行代码的平台提供最佳布局。**sequential** 布局旨在指示 CLI 在单个平台上匹配 C 和 C++ 等语言遵循的布局规则，这在仍然保证可验证布局的情况下是可能的。**explicit** 布局允许 CIL 生成器指定精确的布局语义。

```csharp
[StructLayout(LayoutKind.Auto)]
class AutoLayoutClass;
[StructLayout(LayoutKind.Explicit)]
struct ExplicitLayoutStruct;
[StructLayout(LayoutKind.Sequential)]
record struct SequentialLayoutRecord;
```
```cil
.class private auto {...} AutoLayoutClass 
    extends [System.Runtime]System.Object {...}
.class private explicit {...} ExplicitLayoutStruct 
    extends [System.Runtime]System.ValueType {...}
.class private sequential {...} SequentialLayoutRecord
	extends [System.Runtime]System.ValueType
	implements class [System.Runtime]System.IEquatable`1<valuetype SequentialLayoutRecord> {...}
```

#### 8.1.3. ClassAttr：类型语义特性
<a id="type-semantics-attr"></a>

<pre>
    <em>ClassAttr</em> ::= ... | interface
</pre>
 
类型语义特性指定应定义接口、类还是值类型。接口语义特性指定一个接口。如果此特性不存在，并且定义扩展 (直接或间接) `System.ValueType`，并且定义不是 `System.Enum`，则应定义一个值类型。否则，应定义一个类。

`System.IComparable` 是一个接口，因为接口语义特性存在。

 ```cil
 .class interface public abstract auto ansi 'System.IComparable' { … }
 ```

`System.Double` 直接扩展 `System.ValueType`；`System.Double` 不是类型 `System.Enum`；所以 `System.Double` 是一个值类型。

 ```cil
 .class public sequential ansi serializable sealed beforefieldinit
     'System.Double' extends System.ValueType implements System.IComparable,
      … { … }
 ```

尽管 `System.Enum` 直接扩展 `System.ValueType`，但 `System.Enum` 不是值类型，所以它是一个类。

 ```cil
 .class public abstract auto ansi serializable beforefieldinit 'System.Enum'
     extends System.ValueType implements System.IComparable, … { … }
 ```

`System.Random` 是一个类，因为它不是接口或值类型。

 ```cil
 .class public auto ansi serializable beforefieldinit 'System.Random'
     extends System.Object { … }
 ```

注意，值类型的运行时大小不应超过 1 MByte (0x100000 字节)。

#### 8.1.4. ClassAttr：继承特性
<a id="Inheritance-attr"></a>

<pre>
    <em>ClassAttr</em> ::= ... | abstract | sealed
</pre>

指定特殊语义的特性是 **abstract** 和 **sealed**。这些特性可以一起使用：
- **abstract** <a id="abstract"></a>指定此类型不应实例化。如果一个类型包含抽象方法，那么该类型应声明为抽象类型。
- **sealed** <a id="sealed"></a>指定一个类型不应有派生类。所有值类型都应该是密封的。

密封类型的虚方法实际上是实例方法，因为它们不能被重写。框架作者应谨慎使用密封类，因为它们不提供方便的用户可扩展性构建块。当一个类 (通常是多个接口) 的一组虚方法的实现变得相互依赖或严重依赖潜在派生类无法看到的实现细节时，可能需要密封类。

一个既是抽象又是密封的类型应该只有静态成员，并且作为某些语言所称的 “命名空间” 或 “静态类”。

```csharp
namespace MySpace
{
    class MyClass;
    struct MyStruct;
    sealed class MySealedClass;
    abstract class MyAbstractClass;
}
```
```cil
.namespace MySpace
{
	.class private auto ansi beforefieldinit MySpace.MyClass
		extends [System.Runtime]System.Object { ... }
	
    .class private sequential ansi sealed beforefieldinit MySpace.MyStruct
		extends [System.Runtime]System.ValueType { ... }
	
    .class private auto ansi sealed beforefieldinit MySpace.MySealedClass
		extends [System.Runtime]System.Object { ... }

	.class private auto ansi abstract beforefieldinit MySpace.MyAbstractClass
		extends [System.Runtime]System.Object
	{
		// Methods
		.method public hidebysig newslot abstract virtual 
			instance void MyAbMethod () cil managed { }
        ...
    }
}        
```

#### 8.1.5. ClassAttr：互操作特性
<a id="interoperation-attr"></a>

<pre>
    <em>ClassAttr</em> ::= ... | ansi | autochar | unicode
</pre>

这些特性用于与非托管代码的互操作。它们指定了在对类调用方法 (静态的、实例的或虚拟的) 时，如果该方法的参数或返回类型为 `System.String` 并且本身没有指定封送行为，应使用的默认行为。任何类型只能指定一个值，**ansi** 是默认值。互操作特性包括：
- **ansi** <a id="ansi"></a>指定封送应从 ANSI 字符串到 ANSI 字符串。
- **autochar** <a id="autochar"></a>根据运行 CLI 的平台，指定封送行为 (ANSI 或 Unicode)。
- **unicode** <a id="unicode"></a>指定封送应从 Unicode 字符串到 Unicode 字符串。

```csharp
[module: DefaultCharSet(CharSet.Unicode)]
class DefaultInteroper
{
    [DllImport("MyLib", CharSet = CharSet.Unicode)] // from native (or other) to C#
    internal extern static string ImportMethod();
}
[StructLayout(LayoutKind.Auto, CharSet = CharSet.Ansi)]
class AnsiInteroper;
[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]  // autochar
class AutocharInteroper;
[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]  // unicode
class UnicodeInteroper;
```
```cil
// DefaultInteroper use module-settings 
.class private auto unicode beforefieldinit DefaultInteroper
	extends [System.Runtime]System.Object
{
    // ImportMethod use unicode
	.method assembly hidebysig static pinvokeimpl("MyLib" unicode winapi) 
		string ImportMethod () cil managed preservesig {}
    ...
} 
// AnsiInteroper use ansi
.class private auto ansi beforefieldinit AnsiInteroper
	extends [System.Runtime]System.Object { ... }

// AutocharInteroper use autochar
.class private auto autochar beforefieldinit AutocharInteroper
	extends [System.Runtime]System.Object { ... }

// UnicodeInteroper use unicode
.class private auto unicode beforefieldinit UnicodeInteroper
	extends [System.Runtime]System.Object { ... }
```

除了这三个特性，*类型 _token_ * [_TypeAttributes_](#TypeAttributes) 还指定了一组额外的位模式 (`CustomFormatClass` 和 `CustomStringFormatMask`)，它们没有标准化的含义。如果这些位被设置，但是语言实现没有提供对它们的支持，将抛出 `System.NotSupportedException` 异常。

#### 8.1.6. ClassAttr：特殊处理特性
<a id="special-handling-attr"></a>

<pre>
    <em>ClassAttr</em> ::= ... | beforefieldinit | rtspecialname | serializable | specialname
</pre>

这些特性可以以任何方式组合。
- **beforefieldinit** <a id="beforefieldinit"></a>指示 CLI 在调用静态方法之前不需要初始化类型。参考 [_类型初始化器_](#type-initializer)
- **rtspecialname** <a id="rtspecialname"></a>表示此项的名称对 CLI 具有特殊意义。目前没有定义特殊类型名称；这是为未来使用准备的。任何标记为 **rtspecialname** 的项目也应标记为 **specialname**。
- **serializable** <a id="serializable"></a>保留供将来使用，以指示类型的字段要序列化到数据流中 (如果实现提供了此类支持)。
- **specialname** <a id="specialname"></a>表示此项的名称对 CLI 以外的工具可能具有特殊意义。

如果一个特性项被 CLI 特殊对待，那么工具也应该意识到这一点。反之则不然。

#### 8.1.7. GenPars：泛型参数
<a id="special-genpars"></a>

在定义泛型类型时，会包含泛型参数。

<pre>
    <em>GenPars</em> ::= <em>GenPar</em> [ ',' <em>GenPars</em> ]
</pre>

_GenPar_ 非终结符有以下产生式：

<pre>
    <em>GenPars</em> ::= [ <em>GenParAttribs</em> ]* [ '(' [ <em>GenConstraints</em> ] ')' ] <em>Id</em>
    <em>GenParAttribs</em> ::= '+' | '-' | class | valuetype | .ctor
    <em>GenConstraints</em> ::= <em>Type</em> [ ',' <em>GemContraints</em> ]
</pre>


**`+`** 表示协变的泛型参数。**`-`** 表示逆变的泛型参数。参考 [_协变与逆变_](#variant)

**class** 是一个特殊目的的约束，将 _Id_ 约束为引用类型。这包括通过类或基类型约束本身被约束为引用类型的类型参数。
**valuetype** 是一个特殊目的的约束，将 _Id_ 约束为值类型，但该类型不得为 `System.Nullable<T>` 或 `System.Nullable<T>` 的任何具体封闭类型。这包括本身被约束为值类型的类型参数。
**.ctor** 是一个特殊目的的约束，将 _Id_ 约束为具有公共无参数构造函数 (默认构造函数) 的具体引用类型 (即，非抽象) 或值类型。这包括本身被约束为具体引用类型或值类型的类型参数。

**class** 和 **valuetype** 不应同时为同一 _Id_ 指定。

 ```cil
 .class C< + class .ctor (class System.IComparable`1<!0>) T > { … }
 ```

这声明了一个泛型类 `C<T>`，它有一个名为 `T` 的协变泛型参数。`T` 被约束为必须实现 ``System.IComparable`1<T>``, 并且必须是具有公共默认构造函数的具体类。

在 _GenPars_ 产生式中，_Id_ 不应有重复。

```csharp
class GeneClassA<T> where T: class;   // class 约束
class GeneClassB<T> where T: struct;  // valuetype 约束
class GeneClassC<T> where T: new();   // .ctor 约束
```
```cil
// class constraint
.class private auto ansi beforefieldinit GeneClassA`1<class T>  
	extends [System.Runtime]System.Object { ... }

// valuetype constraint
.class private auto ansi beforefieldinit GeneClassB`1<valuetype .ctor ([System.Runtime]System.ValueType) T>
	extends [System.Runtime]System.Object { ... }

// .ctor constraint
.class private auto ansi beforefieldinit GeneClassC`1<.ctor T>
	extends [System.Runtime]System.Object { ... }    
```

给定接口 `I1` 和 `I2`，以及类 `Base` 的适当定义，以下代码定义了一个类 `Dict`，它有两个泛型参数，`K` 和 `V`，其中 `K` 被约束为实现接口 `I1` 和 `I2`，`V` 被约束为派生自类 `Base`：

 ```cil
 .class Dict`2<(I1,I2)K, (Base)V> { … }
 ```

以下表格显示了一组代表性类型的类型和特殊约束的有效组合。第一组行 (类型约束 `System.Object`) 适用于未指定基类约束或基类约束为 `System.Object` 的情况。符号 &check; 表示 “设置”，符号 &cross; 表示 “未设置”，符号 * 表示 “设置或未设置” 或 “不关心”。

 |      类型约束      | 特殊约束  |               |           | 含义                                                                                      |
 | :----------------: | :-------: | :-----------: | :-------: | ----------------------------------------------------------------------------------------- |
 |                    | **class** | **valuetype** | **.ctor** |
 | (`System.Object`)  |  &cross;  |    &cross;    |  &cross;  | 任何类型                                                                                  |
 |                    |  &check;  |    &cross;    |  &cross;  | 任何引用类型                                                                              |
 |                    |  &check;  |    &cross;    |  &check;  | 有默认构造函数的任何引用类型                                                              |
 |                    |  &cross;  |    &check;    |     *     | 除 `System.Nullable<T>` 外的任何值类型                                                    |
 |                    |  &cross;  |    &cross;    |  &check;  | 有公共默认构造函数的任何类型                                                              |
 |                    |  &check;  |    &check;    |     *     | 无效                                                                                      |
 | `System.ValueType` |  &cross;  |    &cross;    |  &check;  | 包括 `System.Nullable<T>` 的任何值类型                                                    |
 |                    |  &cross;  |    &check;    |     *     | 除 `System.Nullable<T>` 外的任何值类型                                                    |
 |                    |  &cross;  |    &cross;    |  &cross;  | 任何值类型和 `System.ValueType`，以及 `System.Enum`                                       |
 |                    |  &check;  |    &cross;    |  &cross;  | 只有 `System.ValueType` 和 `System.Enum`                                                  |
 |                    |  &check;  |    &cross;    |  &check;  | 没有意义：不能实例化 (没有可实例化的引用类型可以从`System.ValueType`派生)                 |
 |                    |  &check;  |    &check;    |     *     | 无效                                                                                      |
 |   `System.Enum`    |  &cross;  |    &cross;    |  &check;  | 任何枚举类型                                                                              |
 |                    |  &cross;  |    &check;    |     *     |
 |                    |  &cross;  |    &cross;    |  &cross;  | 任何枚举类型和 `System.Enum`                                                              |
 |                    |  &check;  |    &cross;    |  &cross;  | 只有 `System.Enum`                                                                        |
 |                    |  &check;  |    &cross;    |  &check;  | 没有意义：不能实例化 (没有可实例化的引用类型可以从`System.Enum`派生)                      |
 |                    |  &check;  |    &check;    |     *     | 无效                                                                                      |
 | `System.Exception` |  &cross;  |    &cross;    |  &cross;  | `System.Exception`，或任何从 `System.Exception`派生的类                                   |
 |                    |  &cross;  |    &cross;    |  &check;  | 有公共默认构造函数的任何 `System.Exception`                                               |
 |                    |  &check;  |    &cross;    |  &cross;  | `System.Exception`，或任何从 `System.Exception`派生的类。这与未指定类约束时的结果完全相同 |
 |                    |  &check;  |    &cross;    |  &check;  | 有公共默认构造函数的任何`Exception`                                                       |
 |                    |  &cross;  |    &check;    |     *     | 没有意义：不能实例化 (值类型不能从引用类型派生)                                           |
 |                    |  &check;  |    &check;    |     *     | 无效                                                                                      |
 | `System.Delegate`  |  &cross;  |    &cross;    |  &cross;  | `System.Delegate`，或任何从 `System.Delegate`派生的类                                     |
 |                    |  &cross;  |    &cross;    |  &check;  | 没有意义：不能实例化 (没有默认构造函数)                                                   |
 |                    |  &check;  |    &cross;    |  &cross;  | `System.Delegate`，或任何从 `System.Delegate`派生的类                                     |
 |                    |  &check;  |    &cross;    |  &check;  | 有公共 **.ctor** 的任何 `Delegate`。对于已知的委托 (`System.Delegate`) 无效               |
 |                    |  &cross;  |    &check;    |     *     | 没有意义：不能实例化 (值类型不能从引用类型派生)                                           |
 |                    |  &check;  |    &check;    |     *     | 无效                                                                                      |
 |   `System.Array`   |  &cross;  |    &cross;    |  &cross;  | 任何数组                                                                                  |
 |                    |     *     |    &cross;    |  &check;  | 没有意义：不能实例化 (没有默认构造函数)                                                   |
 |                    |  &check;  |    &cross;    |  &cross;  | 任何数组                                                                                  |
 |                    |  &cross;  |    &check;    |     *     | 没有意义：不能实例化 (值类型不能从引用类型派生)                                           |
 |                    |  &check;  |    &check;    |     *     | 无效                                                                                      |



以下实例化是否被允许或禁止，取决于约束。在所有这些实例中，声明本身是允许的。标记为无效的项目表示尝试实例化指定类型失败，而标记为有效的项目则没有。

> **valuetype** 约束组合

 ```cil
 .class public auto ansi beforefieldinit Bar`1<valuetype T>
        Valid      ldtoken  class Bar`1<int32>                           
        Invalid    ldtoken  class Bar`1<class [mscorlib]System.Exception>
        Invalid    ldtoken  class Bar`1<Nullable`1<int32>>               
        Invalid    ldtoken  class Bar`1<class [mscorlib]System.ValueType>
 ```

> **class** 约束组合

 ```cil
 .class public auto ansi beforefieldinit 'Bar`1'<class T>
        Invalid    ldtoken  class Bar`1<int32>                              
        Valid      ldtoken  class Bar`1<class [mscorlib]System.Exception>
        Invalid    ldtoken  class Bar`1<valuetype [mscorlib]System.Nullable`1<int32>>
        Valid      ldtoken  class Bar`1<class [mscorlib]System.ValueType>
 ```

> **class System.ValueType** 约束组合

 ```cil
 .class public auto ansi beforefieldinit Bar`1<(class  [mscorlib]System.ValueType) T>
        Valid       ldtoken  class Bar`1<int32>
        Invalid     ldtoken  class Bar`1<class [mscorlib]System.Exception>
        Valid       ldtoken  class Bar`1<valuetype [mscorlib]System.Nullable`1<int32>>
        Valid       ldtoken  class Bar`1<class [mscorlib]System.ValueType>
 ```

> **class (int32)** 约束组合

 ```cil
 .class public auto ansi beforefieldinit Bar`1<class (int32)> T>
        Invalid     ldtoken  class Bar`1<int32>
        Invalid     ldtoken  class Bar`1<class [mscorlib]System.Exception> 
        Invalid     ldtoken  class Bar`1<valuetype [mscorlib]System.Nullable`1<int32>>
        Invalid     ldtoken  class Bar`1<class [mscorlib]System.ValueType>     
 ```

此类型无法实例化，因为没有引用类型可以扩展 `int32`

> **valuetype System.Exception** 约束组合 

 ```cil
 .class public auto ansi beforefieldinit Bar`1<valuetype (class [mscorlib]System.Exception)> T>
        Invalid     ldtoken  class Bar`1<int32>                   
        Invalid     ldtoken  class Bar`1<class [mscorlib]System.Exception>            
        Invalid     ldtoken  class Bar`1<valuetype [mscorlib]System.Nullable`1<int32>> 
        Invalid     ldtoken  class Bar`1<class [mscorlib]System.ValueType>            
 ```

此类型无法实例化，因为没有值类型可以扩展 `System.Exception`

> **.ctor Foo** 约束组合

 ```cil
 // 其中 Foo 没有公共的 .ctor，但是 `FooBar`，它从 `Foo` 派生，有一个公共的 .ctor：
 .class public auto ansi beforefieldinit Bar`1<.ctor (class Foo) T>
        Invalid     ldtoken  class Bar`1<class Foo>
        Valid       ldtoken  class Bar`1<class FooBar>
 ```

>---
### 8.2. 类型定义的主体
<a id="class-type-member"></a>

一个类型可以包含任意数量的进一步声明。指令 **.event**，**.field**，**.method** 和 **.property** 用于声明类型的成员。类型声明中的 **.class** 指令用于创建 [嵌套类型](#nested-types)。

 | _ClassMember_ ::=                                                                                                              | 描述                                                           | 参考                      |
 | :----------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------- | ------------------------- |
 | `.class` _ClassHeader_ `'{'` _ClassMember_* `'}'`                                                                              | 定义一个嵌套类型。                                             | §[[↗]](#nested-types)     |
 | \| `.custom` _CustomDecl_                                                                                                      | 自定义特性。                                                   | §[[↗]](#custom)           |
 | \| `.data` _DataDecl_                                                                                                          | 定义与类型关联的静态数据。                                     | §[[↗]](#data)             |
 | \| `.event` _EventHeader_ `'{'` _EventMember_* `'}'`                                                                           | 声明一个事件。                                                 | §[[↗]](#event)            |
 | \| `.field` _FieldDecl_                                                                                                        | 声明属于类型的字段。                                           | §[[↗]](#field)            |
 | \| `.method` _MethodHeader_ `'{'` _MethodBodyItem_* `'}'`                                                                      | 声明类型的方法。                                               | §[[↗]](#method)           |
 | \| `.override` _TypeSpec_ `'::'` _MethodName_ `with` _CallConv_ _Type_ _TypeSpec_ `'::'` _MethodName_ `'('` _Parameters_ `')'` | 指定第一个方法被第二个方法的定义覆盖。                         | §[[↗]](#override)         |
 | \| `.pack` _Int32_                                                                                                             | 用于字段的显式布局。                                           | §[[↗]](#pack)             |
 | \| `.param type` `'['` _Int32_ `']'`                                                                                           | 为泛型类型指定一个类型参数；用于将自定义特性与该类型参数关联。 | §[[↗]](#param-type)       |
 | \| `.property` _PropHeader_ `'{'` _PropMember_* `'}'`                                                                          | 声明类型的属性。                                               | §[[↗]](#property)         |
 | \| `.size` _Int32_                                                                                                             | 用于字段的显式布局。                                           | §[[↗]](#size)             |
 | \| _ExternSourceDecl_                                                                                                          | 源代码行信息。                                                 | §[[↗]](#ExternSourceDecl) |
 | \| _SecurityDecl_                                                                                                              | 声明性安全权限。                                               | §[[↗]](#SecurityDecl)     |

在 **.class** 类型声明 [[↗]](#class) 中，**.method** 定义的自上而下的顺序在 [「_MethodDef: 0x06_」](#MethodDef_0x06) 表中保留。这是支持接口调度中的差异解析 [[↗]](#internal-virtual) 所必需的。

>---
### 8.3. 引入和重写虚方法

通过提供方法的直接实现 (使用 [方法定义](#MethodHeader)) 并且不指定它为 **newslot** [[↗]](#newslot)，可以重写基类型的虚方法。也可以使用 **.override** [[↗]](#override) 使用现有的方法体来实现给定的虚声明。

#### 8.3.1. 引入虚方法
<a id="impl-newslot-virtual"></a>

通过定义虚方法 [[↗]](#MethodHeader) 在继承层次中引入虚方法。定义可以标记为 **newslot**，以始终为定义类及其派生类创建新的虚方法：
 * 如果定义被标记为 **newslot**，则定义始终创建新的虚方法，即使基类提供了匹配的虚方法。通过包含方法定义的类或通过从该类派生的类对虚方法的引用，都指向新的定义 (除非在派生类中被 **newslot** 定义隐藏)。任何不通过包含方法定义的类，也不通过其派生类对虚方法的引用，都指向原始定义。
 * 如果定义没有被标记为 **newslot**，则定义只有在未从基类继承相同名称和签名的虚方法时才创建新的虚方法。因此，当虚方法被标记为 **newslot** 时，其引入不会影响其基类中匹配虚方法的任何现有引用。

#### 8.3.2. .override 指令
<a id="override"></a>

**.override** 指令指定在此类型中，一个虚方法应由具有相同签名但名称不同的虚方法实现 (重写)。此指令可用于为从基类继承的虚方法或此类型实现的接口中指定的虚方法提供实现。**.override** 指令在元数据中指定了 **方法实现** (_Method Implementation_，[_MethodImpl_](#MethodImpl))。

<pre>
    <em>ClassMember</em> ::= .override <em>TypeSpec</em> '::' <em>MethodName</em> with <em>CallConv</em> <em>Type</em> <em>TypeSpec</em> '::' <em>MethodName</em> '(' <em>Parameters</em> ')'

    <em>ClassMember</em> ::= .override method <em>CallConv</em> <em>Type</em> <em>TypeSpec</em> '::' <em>MethodName</em> <em>GenArity</em> (' <em>Parameters</em> ')' 
                        with method <em>CallConv</em> <em>Type</em> <em>TypeSpec</em> '::' <em>MethodName</em> <em>GenArity</em> (' <em>Parameters</em> ')' | ...

    <em>GenArity</em> ::= [ '<' '[' <em>Int32</em> ']' '>' ]
</pre>
 
_Int32_ 是泛型参数的数量。第一对 _TypeSpec_::_MethodName_ 指定正在被的重写虚方法，是从当前类型实现的接口中的虚方法或继承的虚方法。剩余的信息指定提供实现的虚方法。

虽然这里指定的语法 (以及实际的元数据格式 [[↗]](#MethodImpl_0x19)) 允许任何虚方法用于提供实现，但符合规范的程序应提供直接在包含 **.override** 指令的类型上实现的虚方法。

元数据的设计比所有 VES 的实现所期望的更具表现力。下面显示了 **.override** 指令的典型用法。为接口中声明的方法提供了实现。

 ```cil
 .class interface I
 { 
    .method public virtual abstract void M() cil managed {}
 }
.class C implements I
 { 
    .method virtual public void M2()
    { // body of M2  }
    .override I::M with instance void C::M2()
 }
 ```

**.override** 指令指定 `C::M2` 主体应提供实现，用于在 `C` 类的对象上实现 `I::M`。

#### 8.3.3. 可访问性和重写

如果指定了 **strict**  _token_  [[↗]](#MethodAttributes)，则只有可访问的虚方法可以被重写。

如果一个类型通过 _MethodImpl_ 重写了一个继承的方法，它可以 *扩大* 或 *缩小* 那个方法的可访问性。

如果一个类型通过非 _MethodImpl_ 的方式覆盖了一个继承的方法，它可以扩大，但不能缩小该方法的可访问性。如果一个类型的客户端被允许访问该类型的一个方法，那么它也应该能够在任何派生类型中访问该方法 (通过名称和签名标识)。下表在这个上下文中指定了 *narrow* 和 *widen* —— "Yes" 表示派生类可以应用那种可访问性，"No" 表示它是无效的。


 | 派生类\基类型可访问性   | Compiler-controlled | private | family | assembly | famandassem | famorassem | public |
 | ----------------------- | ------------------- | ------- | ------ | -------- | ----------- | ---------- | ------ |
 | **Compiler-controlled** | 见注释 3            | No      | No     | No       | No          | No         | No     |
 | **private**             | 见注释 3            | Yes     | No     | No       | No          | No         | No     |
 | **family**              | 见注释 3            | Yes     | Yes    | No       | Yes         | 见注释 1   | No     |
 | **assembly**            | 见注释 3            | Yes     | No     | 见注释 2 | 见注释 2    | No         | No     |
 | **famandassem**         | 见注释 3            | Yes     | No     | No       | 见注释 2    | No         | No     |
 | **famorassem**          | 见注释 3            | Yes     | Yes    | 见注释 2 | Yes         | Yes        | No     |
 | **public**              | 见注释 3            | Yes     | Yes    | Yes      | Yes         | Yes        | Yes    |

 1. "Yes"，前提是两者在不同的程序集中；否则，"No"。
 2. "Yes"，前提是两者在同一个程序集中；否则，"No"。
 3. "Yes"，前提是两者在同一个模块中；否则，"No"。
   PS: 即使可能无法由派生类访问，一个方法也可以被重写。


如果一个方法具有 **assembly** 可访问性，那么如果它被一个不同程序集中的方法重写，那么它应该具有 **public** 可访问性。类似的规则适用于 **famandassem**，其中也允许 **famorassem** 在程序集外部。在这两种情况下，**assembly** 或 **famandassem** 分别可以在同一个程序集内部使用。

对 **famorassem** 有一个特殊的规则，如表中所示。这是派生类显然缩小了可访问性的唯一情况。一个 **famorassem** 方法可以被另一个程序集中的类型以 **family** 可访问性重写。因为没有办法指定 **family** 或特定的其他程序集，所以无法指定可访问性应该保持不变。为了避免缩小访问，将需要指定一个 **public** 的可访问性，这将强制扩大访问，即使这不是预期的。作为一种妥协，允许 **family** 单独的轻微缩小。

#### 8.3.4. 重写对派生类的影响

当在父类型中重写一个方法时，重写将根据以下规则应用于派生类：
 * 如果派生类提供了一个虚方法的实现，那么该方法不受父类型中该方法的任何重写的影响。
 * 否则，如果父类型中重写了该方法，那么重写将被继承，但受到派生类中的任何重写的限制。这意味着，如果父类型用方法 _B_ 重写了方法 _A_，并且派生类没有提供方法 _A_ 的实现或重写，但提供了方法 _B_ 的重写实现，那么在派生类中，将是派生类的 _B_ 实现重写方法 _A_。这可以被认为是虚槽覆盖。

考虑以下内容 (为了清晰起见，摘录出来；所有方法都声明为 **public hidebysig virtual instance**) ：

 ```cil
 .class interface I
 {
    .method newslot abstract void foo() {...}
 }
 .class A implements I
 {
    .method newslot void foo() {...}
 }
 .class B extends A
 { 
    .method newslot void foo1() {.override I::foo ... }
 }
 .class C extends B
 {
    .method void foo1() {...}
    .method void foo2() {.override A::foo ... }
 }
 .class D extends C
 {
    .method newslot void foo() {...}
    .method void foo1(){...}
    .method void foo2(){...}
 }
 ```

对于这个示例，以下表格展示了对各种类型的对象调用的行为样本：

 | 对象类型 | 方法调用 (`callvirt`) | 调用的方法 | 注释                                                                            |
 | -------- | --------------------- | ---------- | ------------------------------------------------------------------------------- |
 | `B`      | `I::foo()`            | `B::foo1`  | 显式重写                                                                        |
 | `C`      | `I::foo()`            | `C::foo1`  | 从 `B` 继承了对虚函数 `foo1` 的 `I::foo` 重写                                   |
 | `C`      | `A::foo()`            | `C::foo2`  | 显式重写                                                                        |
 | `C`      | `B::foo1()`           | `C::foo1`  | 虚重写                                                                          |
 | `D`      | `I::foo()`            | `D::foo1`  | 继承了对虚函数 `foo1` 的 `I::foo` 重写                                          |
 | `D`      | `A::foo()`            | `D::foo2`  | 对虚 `C::foo2` 的 `A::foo` 显式重写 (`D::foo` 不重写这个，因为它是 **newslot**) |
 | `D`      | `B::foo1()`           | `D::foo1`  | 虚重写                                                                          |
 | `D`      | `C::foo1()`           | `D::foo1`  | 虚重写                                                                          |

>---
### 8.4. 方法实现要求

一个类型 (具体或抽象) 可以提供
 * 它引入的实例、静态和虚方法的实现
 * 它指定对接口方法的实现，或指定对基类方法的实现
 * 从基类继承的虚方法的替代实现
 * 从抽象基类型继承的抽象方法的实现

一个具体 (即，非抽象) 类型应直接或通过继承实现
 * 由类型本身声明的所有方法
 * 该类型实现的接口的所有虚方法
 * 该类型从其基类型继承的所有虚方法

>---
### 8.5. 特殊成员

有三种特殊成员，它们都是可以作为类型的一部分定义的方法：实例构造器、实例终结器和类型初始化器。

#### 8.5.1. 实例构造器
<a id="type-constructor"></a>

**实例构造器** (_instance constructor_) 初始化一个类型的实例，并在通过 `newobj` 指令创建一个类型的实例时被调用。实例构造器应该是一个实例方法 (不是静态或虚方法)，它应该被命名为 `.ctor`，并被标记为 **instance**、**rtspecialname** 和 **specialname** [[↗]](#special-handle-attr)。

实例构造器可以有参数，但不应有返回值。实例构造器不能接受泛型类型参数。实例构造器可以被重载 (即，一个类型可以有多个实例构造器)。一个类型的每个实例构造器都应有唯一的签名。与其他方法不同，注释为 **initonly** 特性 [[↗]](#initonly-field) 类型的字段可以在实例构造器内被写入。

下面显示了一个不带任何参数的实例构造器的定义：

```cil
.class X {
    .method public rtspecialname specialname instance void .ctor() cil managed
    { 
        .maxstack 1
        // call super constructor
        ldarg.0  // load this pointer
        call instance void [mscorlib]System.Object::.ctor()
        // do other initialization work
        ret
    }
}
```

#### 8.5.2. 实例终结器

终结器的行为在第一部分 [终结器](./01_CLI%20基本概念和体系结构.md/#finalizer) 中有规定。对于特定类型的 *finalize* 方法，是通过在 `System.Object` 中重写虚方法 `Finalize` 来指定的。

#### 8.5.3. 类型初始化器
<a id="type-initializer"></a>

一个类型 (类，接口，或值类型) 可以包含一个特殊的方法叫做 **类型初始化器** (_type initializer_)，用于初始化类型本身。这个方法应该是静态的，不接受参数，无返回值，被标记为 **rtspecialname** 和 **specialname** [[↗]](#special-handle-attr)，并且被命名为 `.cctor`。

就像实例构造器一样，类型初始化器中可以写入类型被 **initonly** 特性注释的静态字段 [[↗]](#initonly-field)。

下面展示了一个类型初始化器的定义：

```cil
.class public EngineeringData extends [mscorlib]System.Object
{
    .field private static initonly float64[] coefficient
    .method private specialname rtspecialname static void .cctor() cil managed
    {
        .maxstack 1 

        // 分配一个包含 4 个 Double 的数组
        ldc.i4.4
        newarr     [mscorlib]System.Double
        // 将 initonly 字段指向新数组
        stsfld     float64[] EngineeringData::coefficient
        // 初始化数组元素的代码在这里
        ...
        ret
    }
}
```

类型初始化器通常是简单的方法，从存储的常量或通过简单计算来初始化类型的静态字段。但是，对于类型初始化器中允许使用的代码没有限制。

##### 8.5.3.1. 类型初始化保证
<a id="type-init-guarantees"></a>

CLI 将提供以下关于类型初始化的保证，参见 [_宽松保证_](#relaxed-guarantees) 和 [_竞争与死锁_](#races-and-deadlocks)：

 > I. 类型初始化器何时被执行在第一部分的 [_类型初始化方法_](./01_CLI%20基本概念和体系结构.md/#cctor-init) 中有规定。

 > III. 对于任何给定的类型，类型初始化器应该只执行一次，除非被用户代码明确调用。

 > VI. 在类型初始化器完成执行之前，除了那些直接或间接从类型初始化器调用的方法外，没有其他方法能够访问类型的成员。

##### 8.5.3.2. 宽松保证
<a id="relaxed-guarantees"></a>

可以使用特性 **beforefieldinit** [[↗]](#beforefieldinit) 标记一个类型，以表示在 [_类型初始化保证_](#type-init-guarantees) 中指定的保证不是必需的。特别是，不需要提供上述中最后的 VI 保证：在调用或引用静态方法之前，不需要执行类型初始化器。

当代码可以在多个应用程序域中执行时，确保这个最后的 VI 保证变得特别昂贵。同时，对大量托管代码的检查表明，这个 VI 保证很少需要，因为类型初始化器几乎总是用于初始化静态字段的简单方法。因此，让 CIL 生成器 (可能是程序员) 决定是否需要这个保证，可以在需要时提供效率，但要付出一致性保证的代价。

##### 8.5.3.3. 竞争和死锁
<a id="races-and-deadlocks"></a>

除了指定的 [_类型初始化保证_](#type-init-guarantees) 外，CLI 还应确保从类型初始化器调用的代码有两个进一步的保证：

 > III. 类型的静态变量在任何访问之前都处于已知状态。
 
 > V. 仅类型初始化本身不会创建死锁，除非在类型初始化器调用的代码中 (直接或间接) 显式调用阻塞操作。

考虑以下两个类定义：

```cil
.class public A extends [mscorlib]System.Object
{ 
    .field static public class A a
    .field static public class B b
    .method public static rtspecialname specialname void .cctor ()
    { 
        ldnull   // b = null
        stsfld class B A::b
        ldsfld class A B::a // a = B.a
        stsfld class A A::a
        ret
    }
}

.class public B extends [mscorlib]System.Object
{ 
    .field static public class A a
    .field static public class B b
    .method public static rtspecialname specialname void .cctor ()
    { 
        ldnull   // a = null
        stsfld class A B::a
        ldsfld class B A::b // b = A.b
        stsfld class B B::b
        ret
    }
}
```

加载这两个类后，尝试引用任何静态字段都会导致问题，因为 `A` 和 `B` 的类型初始化器都要求先调用另一个的类型初始化器。要求在其初始化完成之前不允许访问类型将创建死锁情况。相反，CLI 提供了一个较弱的保证：初始化器将开始运行，但不必完成。但是，仅此一点就会使类型的完全未初始化状态可见，这将使得保证可重复的结果变得困难。

当类型初始化在多线程系统中进行时，存在类似但更复杂的问题。在这些情况下，例如，两个单独的线程可能开始尝试访问不同类型 (`A` 和 `B`) 的静态变量，然后每个线程都必须等待另一个完成初始化。

以下是一个粗略的算法概述，以确保上述第 I 点和第 II 点：
- 在类加载时 (因此在初始化时间之前) 将零或 `null` 存储到类型的所有静态字段中。
- 如果类型已初始化，表示完成。
  - a. 如果类型尚未初始化，尝试获取初始化锁。
  - b. 如果成功，记录此线程负责初始化类型并继续执行步骤 c。
      - 如果不成功，看看这个线程或任何等待这个线程完成的线程是否已经持有锁。
      - 如果是，则返回，因为阻塞将创建死锁。这个线程现在将看到类型的不完全初始化状态，但不会出现死锁。
      - 如果不是，阻塞直到类型初始化然后返回。
  - c. 初始化基类类型，然后初始化此类型实现的所有接口。
  - d. 执行此类型的类型初始化代码。
  - f. 将类型标记为已初始化，并释放初始化锁，唤醒任何等待此类型初始化的线程，然后返回。

>---
### 8.6. 嵌套类型

嵌套类型在第一部分 [[↗]](#nested-types) 中有规定。关于与嵌套类型相关的逻辑表的信息，请参见 [「_NestedClass: 0x29_」](#NestedClass_0x29)。

嵌套类型不与其封闭类型的实例相关联。嵌套类型有自己的基类型，并且可以独立于其封闭类型进行实例化。这意味着，封闭类型的实例成员不能使用嵌套类型的 **this** 指针访问。

只要那些成员是静态的，或者嵌套类型有对封闭类型实例的引用，嵌套类型就可以访问其封闭类型的任何成员，包括私有成员。因此通过使用嵌套类型，一个类型可以给另一个类型访问其私有成员的权限。

另一方面，封闭类型不能访问嵌套类型的任何 **private** 或 **family** 成员。只有具有 **assembly**、**famorassem** 或 **public** 可访问性的成员才能被封闭类型访问。

下面显示了在另一个类中声明的类。每个类都声明了一个字段。嵌套类可以访问这两个字段，而封闭类无法访问封闭类的字段 `b`。

 ```cil
 .class public auto ansi X
 { 
     .field static private int32 a
     .class auto ansi nested public Y
     { 
         .field static private int32 b
         // ...
     }
 }
 ```

>---
### 8.7. 控制实例布局
<a id="ctrl-layout"></a>

CLI 支持顺序和显式布局控制，参见 [_类型布局特性_](#layout-attr)。对于显式布局，还需要指定实例的精确布局；另请参见  [「_FieldRVA: 0x1D_」](#FieldRVA_0x1D) 和 [「_FieldLayout: 0x10_」](#FieldLayout_0x10)。

<pre>
    <em>FieldDecl</em> ::= [ '[' <em>Int32</em> ']' ] <em>FieldAttr</em>* <em>Type</em> <em>Id</em>
</pre>

声明开始处的方括号中指定的可选 `int32` 指定了从类型实例的开始的字节偏移量。对于给定类型 _t_，这个开始指的是在类型 _t_ 中显式定义的成员集的开始，排除了所有在类型 _t_ 直接或间接继承的任何类型中定义的成员。这种形式的显式布局控制不应与使用 **at** 符号指定的全局字段一起使用 [[↗]](#at-field)。

偏移值应为非负数。可以以这种方式重叠字段，尽管对象引用占用的偏移量不应与内置值类型占用的偏移量或另一个对象引用的一部分重叠。一个对象引用可以完全重叠另一个对象引用，但这是不可验证的。

可以使用指针算术和 `ldind` 间接加载字段，或使用指针和 `stind` 间接存储字段来访问字段。参见 [「_FieldLayout: 0x10_」](#FieldLayout_0x10) 和 [「_FieldRVA: 0x1D_」](#FieldRVA_0x1D) 了解此信息的编码。对于显式布局，每个字段都应分配一个偏移量。

<a id="pack"></a>**.pack** 指令指定字段应放置在运行时对象的字节地址上，这些地址是指定数字的倍数，或者是该字段类型的自然对齐 (以较小者为准)。例如，`.pack 2` 将允许 32 位宽的字段在偶数地址上开始，而没有任何 **.pack** 指令，它们将被自然对齐；也就是说，放置在 4 的倍数的地址上。**.pack** 后面的整数应为以下之一：0、1、2、4、8、16、32、64 或 128。零值表示使用的 _pack_ 大小应匹配当前平台的默认值。任何具有显式布局控制的类型都不应提供 **.pack** 指令。

<a id="size"></a>**.size** 指令指示最小大小，并表示允许填充。因此，分配的内存量是布局计算的大小和 **.size** 指令的最大值。如果此指令用于值类型，那么大小应小于 1 MByte。

控制实例布局的元数据不是 “提示”，它是 VES 的一个组成部分，所有符合 CLI 的实现都应支持。

> 下面的类使用其字段的顺序布局：

```csharp
[StructLayout(LayoutKind.Sequential)]
public class SequentialClass
{
    public int a;
    public int b;
}
```
```cil
.class sequential public SequentialClass
{ 
    .field public int32 a  // store at offset 0 bytes
    .field public int32 b  // store at offset 4 bytes
}
```

> 下面的类使用其字段的显式布局：

```csharp
[StructLayout(LayoutKind.Explicit)]
public class ExplicitClass
{
    [FieldOffset(0)]
    public int a;
    [FieldOffset(6)]
    public int b;
}
```
```cil
.class explicit public ExplicitClass
{ 
    .field [0] public int32 a // store at offset 0 bytes
    .field [6] public int32 b // store at offset 6 bytes
}
```

> 下面的值类型使用 **.pack** 将其字段打包在一起：

```csharp
[StructLayout(LayoutKind.Auto, Pack = 2)]
public struct PackStruct
{
    public int a;
    public int b;
}
```
```cil
.class value sealed public PackStruct extends [mscorlib]System.ValueType
{ 
    .pack 2
  
    .field  public int8  a  // store at offset 0 bytes
    .field  public int32 b // store at offset 2 bytes (not 4)
}
```

> 下面的类指定了一个连续的 16 字节块：

```csharp
[StructLayout(LayoutKind.Auto, Size = 16)]
public class BlobClass { }
```
```cil
.class public BlobClass
{ 
    .size 16
}
```

>---
### 8.8. 全局字段和方法

除了具有静态成员的类型外，许多语言都有数据和方法不是类型一部分的概念。这些被称为 **全局** (_global_) 字段和方法。

识别 CLI 中的全局字段和方法的最简单方法是想象它们只是一个不可见的 **抽象** 公共类的成员。实际上，CLI 定义了这样一个特殊的类，名为 `<Module>`，它没有基类型，也不实现任何接口。这个类是一个顶级类，它不是嵌套的。唯一明显的区别在于当多个模块合并在一起时，如何处理这个特殊类的定义，就像类加载器所做的那样。这个过程被称为 **元数据合并** (_metadata merging_)。

对于普通类型，如果元数据合并了同一类型的两个定义，它只是丢弃一个定义，假设它们是等价的，并且在使用类型时没有发现任何异常。然而，对于持有全局成员的特殊类，成员在合并时跨所有模块联合。如果同一个名字似乎在多个模块中为跨模块使用而定义，那么就会出现错误。

详细来说：
 * 如果不存在相同种类 (字段或方法) 、名称和签名的成员，那么将此成员添加到输出类中。
 * 如果有重复项，并且除 **compiler-controlled** 之外的可访问性不超过一个，那么将它们全部添加到输出类中。
 * 如果有重复项，并且两个或更多项的可访问性不是 **compiler-controlled**，则发生错误。

严格来说，CLI 不支持全局静态变量，即使全局字段和方法可能被认为是这样。模块中的所有全局字段和方法都由制造的类 "`<Module>`" 拥有。然而，每个模块都有自己的 "`<Module>`" 类。甚至没有办法引用另一个模块中这样的一个全局字段或方法 (早期绑定)。但是，可以通过反射 (后期绑定) 访问到它们。

```csharp
Console.WriteLine(typeof(int).Module.Name);  // System.Private.CoreLib.dll
```

---
## 9. 类的语义
<a id="class-semantics"></a>

如第一部分的 [_类型继承_](./01_CLI%20基本概念和体系结构.md/#class-inherit) 所述，类在一个在继承层次结构中定义类型。一个类 (除了内置类 `System.Object` 和特殊类 `<Module>`) 应该声明一个确切的基类。一个类应该声明它实现的零个或多个接口 [[↗]](#interface-semantics)。一个具体类可以被实例化来创建一个对象，但是一个 **抽象** 类 (**abstract** [[↗]](#Inheritance-attr)) 不应该被实例化。一个类可以定义字段 (静态的或实例的)、方法 (静态的、实例的或虚拟的)、事件、属性和嵌套类型 (类、值类型或接口)。

类的实例 (即，对象) 只能通过显式使用 `newobj` 指令创建。当创建一个使用类类型的变量或字段 (例如，调用一个具有类类型的局部变量的方法) 时，该值最初应该为 `null`，这是一个特殊的值，它与所有类类型相等，即使它不是任何特定类的实例。

---
## 10. 接口的语义
<a id= "interface-semantics"></a>

如第一部分的 [_接口定义_](./01_CLI%20基本概念和体系结构.md/#interface-definition) 所述，每个接口都定义了其他类型可以实现的协议。接口可以有静态字段和方法，但不能有实例字段或方法。接口可以定义虚方法，但只有当这些方法是 **abstract** 时才可以，参考第一部分的 [_方法定义_](./01_CLI%20基本概念和体系结构.md/#method-difinition) 和 [*MethodAttr : abstract*](#MethAttr-abstract)。

接口不能定义实例字段的原因与 CLI 不支持基类型的多重继承的原因相同：在动态加载数据类型的情况下，没有已知的实现技术既能在使用时高效，又能在不使用时没有成本。相比之下，提供静态字段和方法不需要影响实例的布局，因此不会发生这些问题。

接口可以嵌套在任何类型 (接口、类或值类型) 内部。

>---
### 10.1. 实现接口

类和值类型应 **实现** (_implement_) 零个或多个接口。实现接口意味着类或值类型的所有具体实例都应为接口中声明的每个 **抽象** (_abstract_) 虚方法提供实现。为了实现接口，类或值类型应显式声明实现 (在其类型定义中使用 **implements** 特性 [[↗]](#implements)) 或从已提供接口实现的基类派生。

**抽象** 类 (因为它不能被实例化) 可以不需要为它继承接口中的虚方法提供实现，而从它派生的任何具体类都应该提供实现。

仅为接口的所有 **抽象** 方法提供实现并不足以使类型实现该接口。从概念上讲，接口代表了一个协议，相比在 **抽象** 方法集中捕获的需求，接口可以具有更多的要求。从实现的角度来看，这允许类型的布局只受那些显式声明的接口的约束。

接口应声明它们需要实现零个或多个其他接口。如果一个接口 _A_ 声明它需要实现的另一个接口 _B_，那么 _A_ 隐式声明它需要实现 _B_ 所需的所有接口。如果类或值类型声明它实现了 _A_，那么所有具体实例都应提供 _A_ 和 _A_ 中隐式所需的所有接口中声明的虚方法的实现。类不需要显式声明它实现了 _A_ 所要求的所有接口。

下面的类实现了在模块 `Counter` 中定义的接口 `IStartStopEventSource`。

```cil
.class private auto autochar StartStopButton
    extends [System.Windows.Forms]System.Windows.Forms.Button
    implements [.module Counter]IstartStopEventSource
{ // class body }
```

>---
### 10.2. 在接口上实现虚方法
<a id="internal-virtual"></a>

实现接口的类需要为该接口定义的 **抽象** 虚方法提供实现。有三种提供此实现的机制：
 * 直接指定实现，使用与接口中出现的相同的名称和签名。
 * 从基类型继承现有的实现。
 * 使用显式方法实现 (_MethodImpl_ [[↗]](#MethodImpl))。

如果由于类型参数的差异，给定接口方法有多个实现，那么类上接口声明的顺序以及方法声明的顺序，决定了哪个方法被调用。在接口方法调用的规范中使用了以下术语 (参考示例 [[↗]](#interface-impl-exam))：

 * 对于类型 _T_ 实现 _I_<sub>1</sub>,&hellip;,_I_<sub>_n_</sub>，_n_ ≥ 0，_I_<sub>_x_</sub> 被称为类型的 **显式接口** (_explicit interfaces_)，并形成一个有序列表；_I_<sub>_x_</sub> 是为 _T_ 列在 _InterfaceImpl_ [[↗]](#InterfaceImpl_0x09) 表项中的接口，按行从上到下排序。

 - 类型 _T_ 的 **继承 / 实现树** (_inheritance/implements tree_) 是按以下方式形成的 _n_ - 叉树：
     * 树的根是 _T_
     * 如果 _T_ 派生自 _S_；即其 _TypeDef_ [[↗]](#TypeDef_0x02) 表中 _Extends_ 字段引用 _S_；那么根节点的第一个子节点是类型 _S_ 的继承 / 实现树。
     * 如果 _T_ 有一个或多个显式接口，_I_<sub>_x_</sub>，那么每个 _I_<sub>_x_</sub> 的继承 / 实现树是根节点的子节点，按顺序排列。

 * 类型 _T_ 的接口和超类的 **类型声明顺序** (_type declaration order_) 是类型 _T_ 的继承 / 实现树的后序深度优先遍历，任何类型的第二个和后续的重复项被省略。具有不同类型参数的同一接口的出现不被视为重复。一个类可以通过指定不同的泛型参数，提供同一接口的多个实现。这会产生一个同一接口方法的方法列表。

 - 类型 _T_ 的方法的 **方法声明顺序** (_method declaration order_) 是其基类型 (如果有) 的方法声明顺序，后面跟着 _T_ 的非重写方法 (按照它们在 _T_ 的 _MethodDef_ [[↗]](#MethodDef_0x06) 表中从上到下列出的顺序)。

VES 将使用以下算法来确定接口抽象虚方法在类的开放形式上的适当实现：

 * 创建一个接口表，该表为接口定义的每个虚方法提供一个空列表。

 - 如果接口是此类的显式声明接口 (**explicit interface**)：

     * 如果类定义了任何名称和签名与接口上的虚方法匹配的 **public virtual** 方法，那么将这些方法按 _类型声明顺序_ 添加到该接口方法的列表中。对于顺序相关的示例，参见示例 6 [[↗]](#interface-impl-exam-6)。
     * 如果此类 (直接或继承) 上有任何公共 **virtual** 方法，其名称和签名与接口方法相同，并且其泛型类型参数与此类或其继承链中的任何类的该接口方法的现有列表中的任何方法不完全匹配，那么将它们按 _类型声明顺序_ 添加到接口上相应方法的列表中。
     * 如果有多个具有相同名称、签名和泛型类型参数的方法，只有在 _方法声明顺序_ 中的最后一个方法被添加到列表中。对于重复方法的示例，参见示例 4 [[↗]](#interface-impl-exam-4) 。
     * 应用为此类指定的所有 *MethodImpl*s，将显式指定的虚方法放入到该方法的接口列表中，代替那些继承的或通过名称匹配选择的具有相同泛型类型参数的方法。如果对于同一接口方法有多个方法 (即，具有不同的泛型类型参数)，则将它们按照关联接口的 _类型声明顺序_  放入列表中。
     * 如果当前类不是 **abstract** 并且对于此类和其所有继承链中的所有类，仍有任何接口方法具有空插槽 (即，具有空列表的插槽)，那么程序无效。

当调用接口方法时，VES 将使用以下算法来确定要调用的适当方法：

 * 从调用接口方法的实例的运行时类开始，使用其如上构造的接口表，并替换调用类指定的任何泛型参数 (如果有) ： 

    1. 对于与接口方法关联的列表中的每个方法，如果存在一个方法，其泛型类型参数对于此实例化完全匹配 (或者没有泛型类型参数)，那么调用第一个方法。一旦替换了泛型参数，列表中可能有重复项，在这种情况下，将调用第一个匹配的方法。
    2. 否则，如果列表中存在一个方法，其泛型类型参数具有正确的协变关系，那么调用列表中的第一个此类方法。
    3. 如果在此类中找不到方法，返回到第 1 步，使用继承链中的下一个类 (即，当前类的 _Extends_ 字段) 
    4. 如果找不到方法，那么引发 `System.InvalidCastException`

在存在泛型类型参数的情况下，在泛型类型参数完全实例化时且匹配的情况下，类上隐式实现接口的方法可能优先于基类型中显式实现接口的方法。参见示例 3 [[↗]](#interface-impl-exam-3).

在存在变体接口的情况下，按变体匹配的类上方法可能优先于基类型中精确匹配的方法。参见示例 5 [[↗]](#interface-impl-exam-5)。

一个类型可能提供同一接口的具有相同泛型参数的多个实现。在这种情况下，是声明的顺序决定了接口方法调用时应使用哪个实现。这意味着改变声明顺序可以改变行为。参见示例 6 [[↗]](#interface-impl-exam-6)。

#### 10.2.1. 接口实现示例
<a id="interface-impl-exam"></a>

这些示例说明了解析接口调用规则的应用。示例使用了 ilasm 语法的缩写形式 (例如，`I<T>` 代替 ``I`1<T>``，'`:`' 作为扩展或实现的缩写)，并且 **继承 / 实现树** 图省略了 `System.Object`。

以下是使用的接口：

```cil
IExp<T> { void M() {} }       // 声明方法 M 的接口
IImp<T> : IExp<T> {}          // 需要 IExp<T> 的接口
IVar<-T> { void P(T) {} }     // 带有方法 P 的逆变接口
IVarImp : IVar<A> {}          // 隐式变体接口 
```

以下简单类型用作泛型类型参数 (为了简洁，像 `I<class A>` 这样的实例化被缩写为 `I<A>`，所以读者应注意 `A`，`B` 和 `C` 是实际类型，而不是类型参数) ：

```cil
A {}
B : A {}
C : B {}
```

以下类型用于说明接口的实现：

```cil
abstract S1<T,U> : IExp<!0> {
    void MImpl() {.override IExp<!0>::M()...}
    void P(!0){...}
    void P(!1){...}
}
S2 : S1<C,C>, IImp<C>, IVar<C> {
    void M(){...}
}
S3 : S2, IExp<C>, IVar<A> {
    void M(){...}
    newslot void P(A){...}
}
S4<V> : S1<A,B>, IVarImp, IVar<B>, IImp<!0> {
     newslot void M(){...}
}
```

**显式接口**：类型的显式接口是直接列在其实现列表中的接口 (例如，对于 `S2`，它只是 `IImp<C>` 和 `IVar<C>`，而不是 `IExp<C>`，尽管它是 `IImp<C>` 所需的，并通过父类型 `S1<C,C>` 隐式实现)。

**继承/实现树**：以下是 `S3` 和 `S4` 的继承 / 实现树 (事实上，在这个示例中，`S2` 的树是 `S3` 的树的一个真子集) ：

  ![继承树](./.img/继承树.png)

**类型声明顺序**：类型 `S2`，`S3` 和 `S4` 的类型声明顺序如下：

```
    S2    : IExp<C>, S1<C,C>, IImp<C>, IVar<C>, S2
    S3    : IExp<C>, S1<C,C>, IImp<C>, IVar<C>, S2, IVar<A>, S3
    S4<V> : IExp<A>, S1<A,B>, IVar<A>, IVarImp, IVar<B>, IExp<!0>, IImp<!0>, S4<!0>
```

`IExp<C>` 在 `S3` 的类型声明顺序中只出现一次，尽管它出现在 `IImp<C>` 下的树中。这是因为第二次出现是重复的。然而，`IExp<!0>` 出现在 `S4<V>` 的继承 / 实现树中，因为它不是 `IExp<A>` 的重复。

**方法声明顺序**：这些类型的方法声明顺序如下：

```
    S1<T,U>: S1<!0,!1>::MImpl(), S1<!0,!1>::P(!0), S1<!0,!1>::P(!1)
    S2     : S1<C,C>::MImpl(), S1<C,C>::P(!0:C), S1<C,C>::P(!1:C), S2::M()
    S3     : S1<C,C>::MImpl(), S1<C,C>::P(!0:C), S1<C,C>::P(!1:C), S3::M(), S3::P(A)
    S4<V>  : S1<A,B>::MImpl(), S1<A,B>::P(A), S1<A,B>::P(B), S4<!0>::M()
```

注意，**newslot** 方法在列表中单独出现，而重写则在列表中替换被重写的方法。上面的列表显示了扩展或实现类型声明的泛型参数替换，但使用 `!n` 表示法来标识定义类型中的原始类型参数，其中它是模糊的 (例如，`S1<C,C>::P(!0:C)` 指的是 `S1` 中的第一个 `P` 方法，其中第一个类型参数绑定到类型 `C`)。

以下是接口表：

 | 类        | 接口方法        | 实现列表                                                   |
 | --------- | --------------- | ---------------------------------------------------------- |
 | `S1<T,U>` | `IExp<T>::M()`  | `(IExp<!0>)S1<!0,!1>::MImpl()`                             |
 | `S2`      | `IVar<T>::P(T)` | `(IVar<C>)S1<C,C>::P(!1:C)`                                |
 | `S3`      | `IExp<T>::M()`  | `(IExp<C>)S3::M()`                                         |
 | &nbsp;    | `IVar<T>::P(T)` | `(IVar<A>)S3::P(A)`                                        |
 | `S4<V>`   | `IExp<T>::M()`  | `(IExp<!0>)S4<!0>::M()`                                    |
 | &nbsp;    | `IVar<T>::P(T)` | `(IVar<A>)S1<A,B>::P(!0:A)`<br>`(IVar<B>)S1<A,B>::P(!1:B)` |

以下是几个代码序列。这些序列假设 `a`、`c`、`s2`、`s3` 和 `s4` 分别是类型 `A`、`C`、`S2`、`S3` 和 `S4<A>` 的局部变量的索引。

> case 1：隐式实现<a id= "interface-impl-exam-1"></a>

 ```cil
 ldloc      s2
 callvirt   IExp<C>::M()  // 1: Calls S1<!0,!1>::MImpl()
 ```

尽管 `S2` 为 `IExp<C>::M()` 提供了一个匹配的方法，但它没有被添加到实现列表中，因为 `IExp<C>` 不是 `S2` 的显式接口，而且父类型 `S1<C,C>` 已经提供了一个匹配。

> case 2：显式实现<a id= "interface-impl-exam-2"></a>

 ```cil
 ldloc      s3
 callvirt   IExp<C>::M()  // 2: Calls S3::M()
 ```

对于 `S3` 的情况就不同了，因为 `IExp<C>` 是 `S3` 的显式接口，所以它的匹配 `M()` 方法被添加到了实现列表中。

> case 3：隐式实现与不同类型参数<a id= "interface-impl-exam-3"></a>

 ```cil
 ldloc      s4
 callvirt   IExp<A>::M()  // 3: Calls S4<A>::M()
 ```

`S4<V>` 是一个稍微不同的情况。虽然它只是隐式地实现了 `IExp<!0>`，但它在类型参数上与其父类实现的 `IExp<A>` 不同 (即，父类实例化固定为 `IExp<A>`，而隐式实现未绑定为 `IExp<!0>`)。所以它的匹配 `M()` 被添加到了列表中，并且即使当 `S4` 用显式父类实现的匹配类型参数实例化时，也会调用它，因为接口表是从开放类型构造的。

> case 4：实例化后的重复方法 (方法顺序) <a id= "interface-impl-exam-4"></a>

 ```cil
 ldloc      s2
 ldloc      c
 callvirt   IVar<C>::P(C) // 4: Calls S1<C,C>::P(!1:C)
 ```

`S1<C,C>` 上的两个 `P` 方法都匹配 `IVar<C>::P(C)`，保留最后一个匹配的方法。

> case 5：变体匹配 vs. 父类上的精确匹配<a id= "interface-impl-exam-5"></a>

 ```cil
 ldloc      s3
 ldloc      c
 callvirt   IVar<C>::P(C) // 5: Calls S3::P(A)
 ```

尽管 `S3::P(A)` 是 `IVar<C>::P(C)` 的变体匹配，但 `S2<A,B::IVar<A>::P(A)` 在其父类上是精确匹配。然而，变体匹配是在搜索父类型之前找到的。

> case 6：接口声明顺序<a id= "interface-impl-exam-6"></a>

 ```cil
 ldloc      s4
 ldloc      c
 callvirt   IVar<C>::P(C) // 6: Calls S1<A,B>::P(!0:A)
 ```

尽管 `IVar<A>` 不是 `S4<A>` 的显式接口，但它在接口顺序中排在 `IVar<B>` 之前。这就是为什么调用解析为 `S1<A,B>::P(!0:A)`，而不是 `S1<A,B>::P(!1:B)`。注意，这与 `S4<V>` 的类型参数无关，它只影响 `IImp<!0>` 接口实现。


---
## 11. 值类型的语义
<a id="valuetype-semantics"></a>

与引用类型不同，值类型 [[↗]](./01_CLI%20基本概念和体系结构.md/#valuetype-and-reference) 不是通过使用引用来访问，而是直接存储在该类型的位置中。

值类型用于描述小数据项的类型。它们可以与 C++ 中的结构体 (而不是指向结构体的指针) 类型进行比较。与引用类型相比，值类型的访问速度更快，因为没有额外的间接引用。作为数组的元素，它们不需要为指针以及数据本身分配内存。典型的值类型有复数、几何点和日期。

像其他类型一样，值类型可以有字段 (静态或实例) 、方法 (静态、实例或虚拟) 、属性、事件和嵌套类型。某个值类型的值可以通过一个称为 **装箱** (_boxing_) 的过程转换为相应引用类型的实例 (当定义值类型时，VES 会自动创建一个类作为其 **装箱形式**，_boxed form_)。装箱的值类型可以通过一个称为 **拆箱** (_unboxing_) 的过程转换回其值类型表示形式，即 **拆箱形式** (_unboxed form_)。值类型应该是密封的，并且它们应该有一个基类型，要么是 `System.ValueType`，要么是 `System.Enum`。值类型应该实现零个或多个接口，但这只在它们的装箱形式中有意义。

未装箱的值类型不被视为另一种类型的子类型，对未装箱的值类型使用 `isinst` 指令是无效的。然而，`isinst` 指令可以用于装箱的值类型。未装箱的值类型不应被赋值为 *null*，并且它们不应与 *null* 进行比较。

值类型支持与引用类型相同的布局控制 [[↗]](#ctrl-layout)。这在从本地代码导入值时尤其重要。

由于值类型表示数据的直接布局，因此不允许递归结构定义，例如 (在 C# 中) `struct S {S x; S y;}`。结构体应该有一个非循环的有限 **展平图** (***flattening graph***)：

对于值类型 *S*，定义 *S* 的展平图 *G* 为最小的有向图，满足：
 * *S* 在 *G* 中。
 * 每当 *T* 在 *G* 中，并且 *T* 有值类型 *X* 的实例字段，那么 *X* 在 *G* 中，并且从 *T* 到 *X* 有一条边。
 * 每当 *T* 在 *G* 中，并且 *T* 有值类型 *Y* 的静态字段，那么 *Y* 在 *G* 中。

```csharp
class C<U> { }
struct S1<V> {
    S1<V> x;
}
struct S2<V> {
    static S2<V> x;
}
struct S3<V> {
    static S3<C<V>> x;
}
struct S4<V> {
    S4<C<V>>[] x;
}
```

结构类型 `S1` 有一个有限但循环的展平图，是无效的；`S2` 有一个有限的非循环展平图，是有效的；`S3` 有一个无限的非循环展平图，是无效的；`S4` 有一个有限的非循环展平图，是有效的，因为字段 `S4<C<V>>.x` 是引用类型，而不是值类型。

`C<U>` 类型对于示例并不是严格必需的，但如果没有使用它，可能不清楚以下类型的问题是从字段类型中 `S3<...>` 的内部出现还是外部出现。

```csharp
struct S3<V> {
    static S3<S3<V>> x;
}
```

>---
### 11.1. 值类型的引用

值类型的非装箱形式应使用 **valuetype** 关键字后跟类型引用来引用。值类型的装箱形式应使用 **boxed** 关键字后跟类型引用来引用。

<pre>
    <em>ValueTypeReference</em> ::=  boxed <em>TypeReference</em> | valuetype <em>TypeReference</em> 
</pre>

```csharp
struct S
{   void Fun(S s, ref S rs, ValueType os, ref ValueType ros) { } }
```
```cil
.class S extends [System.Runtime]System.ValueType{
    .method instance void Fun (
			valuetype S s,
			valuetype S& rs,
			class [System.Runtime]System.ValueType os,
			class [System.Runtime]System.ValueType& ros
        ) cil managed { ... }        
}
```

>---
### 11.2. 初始化值类型

与类一样，值类型可以具有实例构造器 [[↗]](#type-constructor) 和类型初始化器 [[↗]](#type-initializer)。但不同于类的是，值类型中类类型的字段会自动初始化为 *null*，而以下规则是关于 (未装箱的) 值类型初始化的唯一保证：
 * 当类型被加载 [[↗]](#races-and-deadlocks) 时，静态变量应被初始化为零，因此当类型被加载时，类型为值类型的静态变量将被初始化为零。
 * 如果 _method header_ [[↗]](#method-header) 中设置了 **localsinit** 位，则局部变量应被初始化为零。
 * 数组应被初始化为零。
 * 在调用实例构造器之前，类的实例 (即，对象) 应被初始化为零。

保证未装箱值类型的自动初始化既困难又昂贵，尤其是在支持线程本地存储并允许在 CLI 外部创建线程并传递给 CLI 进行管理的平台上。
 
装箱的值类型是类，并遵循类的规则。

`initobj` 指令在程序控制下执行零初始化。如果值类型有一个构造器，那么可以创建其未装箱类型的实例，就像对类一样。`newobj` 指令与初始化器及其参数一起使用，以分配和初始化实例。值类型的实例将在堆栈上分配。基类库提供了 `System.Array.Initialize` 方法来将未装箱值类型的数组中的所有实例初始化为零。

下面的代码声明并初始化了三个值类型变量。第一个变量被初始化为零，第二个通过调用实例构造器进行初始化，第三个通过在堆栈上创建对象并将其存储到局部变量中进行初始化。

```cil
.assembly Test { }
.assembly extern System.Drawing {
    .ver 1:0:3102:0
    .publickeytoken = (b03f5f7f11d50a3a)
}
 
.method public static void Start()
{   
    .maxstack 3
    .entrypoint
    .locals init (valuetype [System.Drawing]System.Drawing.Size Zero,
                  valuetype [System.Drawing]System.Drawing.Size Init,
                  valuetype [System.Drawing]System.Drawing.Size Store)

    // Zero initialize the local named Zero
    ldloca Zero           // load address of local variable
    initobj valuetype [System.Drawing]System.Drawing.Size

    // Call the initializer on the local named Init
    ldloca Init           // load address of local variable
    ldc.i4 425            // load argument 1 (width)
    ldc.i4 300            // load argument 2 (height) 
    call instance void [System.Drawing]System.Drawing.Size::.ctor(int32, int32) 

    // Create a new instance on the stack and store into Store. Note that
    // stobj is used here - but one could equally well use stloc, stfld, etc.
    ldloca Store
    ldc.i4 425            // load argument 1 (width)
    ldc.i4 300            // load argument 2 (height)
    newobj instance void [System.Drawing]System.Drawing.Size::.ctor(int32, int32)
    stobj valuetype [System.Drawing]System.Drawing.Size
    ret
}
```

### 11.3. 值类型的方法

值类型可以有静态、实例和虚方法。值类型的静态方法的定义和调用方式与类类型的静态方法相同。与类一样，可以使用 `call` 指令调用装箱或未装箱值类型的实例和虚方法。`callvirt` 指令不应用于未装箱的值类型，但可以用于装箱的值类型。

类的实例和虚方法应被编码为期望将类实例的引用作为 `this` 指针。相比之下，值类型的实例和虚方法应被编码为期望将值类型的未装箱实例的托管指针。当将装箱值类型作为 `this` 指针传递给由未装箱值类型提供实现的虚方法时，CLI 应将装箱值类型转换为未装箱值类型的托管指针。

此操作与拆箱实例相同，因为 `unbox` 指令被定义为返回一个与原始装箱实例共享内存的值类型的托管指针。下图表示值类型的装箱和未装箱表示之间的关系。

 ![装箱与拆箱](./.img/装箱与拆箱.png)

在值类型上使用实例方法的一个重要用途是改变实例的内部状态。如果使用未装箱值类型的实例作为 `this` 指针，这是无法做到的，因为它将操作值的副本，而不是原始值：当将未装箱的值类型作为参数传递时，它们会被复制。

虚方法被用来使多种类型能够共享实现代码，这要求所有实现虚方法的类共享由首次引入方法的类定义的公共表示。由于值类型可以 (并且在基类库中确实可以) 实现 `System.Object` 上定义的接口和虚方法，因此重要的是虚方法可以使用装箱的值类型进行调用，因此可以像操作实现接口的任何其他类型一样操作它。这也致使要求执行环境 (_Execution Environment_) 在虚拟调用上自动拆箱值类型。

> 给定 CIL 指令和实例方法声明类型下的 `this` 指针类型

 | &nbsp;     | 值类型 (装箱或未装箱) 方法 | 接口方法 | 对象类型方法 |
 | ---------- | -------------------------- | -------- | ------------ |
 | `call`     | 值类型的托管指针           | 无效     | 对象引用     |
 | `callvirt` | 值类型的托管指针           | 对象引用 | 对象引用     |

以下将值类型 `int32` 的整数转换为字符串。`int32` 对应于基类库中定义的未装箱值类型 `System.Int32`。假设整数声明为：

 ```cil
 .locals init (int32 x)
 ```

然后如下所示进行调用：

 ```cil
 ldloca x  // load managed pointer to local variable
 call instance string valuetype [mscorlib]System.Int32::ToString()
 ```

然而，如果使用 `System.Object` (一个类) 作为类型引用，而不是 `System.Int32` (一个值类型)，则在调用之前应将 `x` 的值装箱，代码变为：

 ```cil
 ldloc x
 box valuetype [mscorlib]System.Int32
 callvirt instance string [mscorlib]System.Object::ToString()
 ```

---
## 12. 特殊类型的语义

特殊类型是指那些从 CIL 引用但没有提供定义的类型：VES 根据来自引用的可用信息自动提供定义。

>---
### 12.1. 向量
<a id="vector"></a>

<pre>
    <em>Type</em> ::= ... | <em>Type</em> '[' ']' 
</pre>

向量是具有零下界的单维数组。它们在 CIL 指令中有直接支持 (`newarr`、`ldelem`、`stelem` 和 `ldelema`)。CIL 框架还提供了处理多维数组和具有非零下界的单维数组的方法 [[↗]](#array)。如果两个向量的元素类型相同，无论它们的实际上界如何，它们都具有相同的类型。

向量具有固定的大小和元素类型，这些在创建时确定。所有的 CIL 指令都应该遵守这些值。也就是说，它们应该可靠地检测以下尝试：索引超出向量的末尾，将错误类型的数据存储到向量的元素中，以及获取数据类型不正确的向量元素的地址。

声明一个字符串的向量：

 ```cil
 .field string[] errorStrings
 ```

声明一个函数指针的向量：

 ```cil
 .field method instance void*(int32) [] myVec
 ```

创建一个包含 4 个字符串的向量，并将其存储到字段 `errorStrings` 中。这 4 个字符串位于 `errorStrings[0]` 到 `errorStrings[3]`：

 ```cil
 ldc.i4.4
 newarr string
 stfld string[] CountDownForm::errorStrings
 ```

将字符串 `"First"` 存储到 `errorStrings[0]`：

 ```cil
 ldfld string[] CountDownForm::errorStrings
 ldc.i4.0
 ldstr "First"
 stelem
 ```

向量是 `System.Array` 的子类型，`System.Array` 是 CLI 预定义的一个抽象类。它提供了可以应用于所有向量的几个方法。

>---
### 12.2. 数组
<a id="array"></a>

尽管向量通过 CIL 指令直接支持，但 VES 通过创建抽象类 `System.Array` 的子类型来支持所有其他数组。

<pre>
    <em>Type</em> ::= ... | <em>Type</em> '[' [ <em>Bound</em> [ ',' <em>Bound</em> ]*] ']' 
</pre>

数组的 *rank* 是其维度的数量。CLI 不支持秩为 0 的数组。数组 (向量除外) 的类型应由其元素的类型和维度的数量确定。

 | _Bound_ ::=                | 描述                                                   |
 | :------------------------- | ------------------------------------------------------ |
 | `'...'`                    | 下界和上界未指定。在多维数组的情况下，可以省略 `'...'` |
 | \| _Int32_                 | 零下界，_Int32_ 上界                                   |
 | \| _Int32_ `'...'`         | 只指定了下界                                           |
 | \| _Int32_ `'...'` _Int32_ | 同时指定了上下界                                       |

VES 为数组创建的类包含几个由 VES 提供实现的方法：
 * 一个构造函数，它接受一系列 `int32` 参数，每个维度一个，这些参数指定从第一个维度开始的每个维度中的元素数量。假定下界为零。
 * 一个构造函数，它接受的 `int32` 参数的数量是数组维度的两倍。这些参数成对出现 —— 每个维度一对 —— 每对的第一个参数指定该维度的下界，第二个参数指定该维度的元素总数。注意向量不是用这个构造函数创建的，因为向量假定下界为零。
 * 一个 `Get` 方法，它接受一系列 `int32` 参数，每个维度一个，并返回一个值，其类型是数组的元素类型。此方法用于访问数组的特定元素，其中参数指定要返回的元素的每个维度的索引，从第一个开始。
 * 一个 `Set` 方法，它接受一系列 `int32` 参数，每个维度一个，后面跟着一个值，其类型是数组的元素类型。`Set` 的返回类型是 `void`。此方法用于设置数组的特定元素，其中参数指定要设置的元素的每个维度的索引，从第一个开始，最后一个参数指定要存储到目标元素中的值。
 * 一个 `Address` 方法，它接受一系列 `int32` 参数，每个维度一个，其返回类型是数组元素类型的托管指针。此方法用于返回数组的特定元素的托管指针，其中参数指定要返回其地址的元素的每个维度的索引，从第一个开始。

下面创建了一个字符串的数组 `MyArray`，有两个维度，索引为 5…10 和 3…7。然后它将字符串 `"One"` 存储到 `MyArray[5, 3]` 中，检索它并打印出来。然后它计算 `MyArray[5, 4]` 的地址，将 `"Test"` 存储到其中，检索它，并打印出来。

```csharp
// 可以视为在 CSharp 中的等效代码
static void Start()
{
    string[,] myArray = (string[,]) Array.CreateInstance(typeof(string), [6, 5], [5, 3]);
    myArray[5,3] = "One";
    Console.WriteLine(myArray[5, 3]);
    ref var ro  = ref myArray[5, 4];
    ro = "Test";
    Console.WriteLine(myArray[5, 4]);
}
```
```cil
.assembly Test { }
.assembly extern mscorlib { }

.method public static void Start()
{ 
    .maxstack 5
    .entrypoint
    .locals (class [mscorlib]System.String[,] myArray)

    ldc.i4.5 // 加载维度 1 的下界
    ldc.i4.6 // 加载维度 1 的 (上界 - 下界 + 1) 
    ldc.i4.3 // 加载维度 2 的下界
    ldc.i4.5 // 加载维度 2 的 (上界 - 下界 + 1) 
    newobj instance void string[,]::.ctor(int32, int32, int32, int32)
    stloc  myArray

    ldloc myArray
    ldc.i4.5
    ldc.i4.3
    ldstr "One"
    call instance void string[,]::Set(int32, int32, string)

    ldloc myArray
    ldc.i4.5
    ldc.i4.3
    call instance string string[,]::Get(int32, int32)
    call void [mscorlib]System.Console::WriteLine(string)

    ldloc myArray
    ldc.i4.5
    ldc.i4.4
    call instance string & string[,]::Address(int32, int32)
    ldstr "Test"
    stind.ref

    ldloc myArray
    ldc.i4.5
    ldc.i4.4
    call instance string string[,]::Get(int32, int32)
    call void [mscorlib]System.Console::WriteLine(string)

    ret
}
```

在多维数组中，元素可以被认为是在连续的内存中布局的，但是数组的数组则不同 —— 数组的每个维度 (除最后一个外) 都持有一个数组引用。例如：

 ![数组](./.img/数组分配.png)

左边是一个 [6, 10] 的矩形数组。右边不是一个而是五个数组。垂直数组是一个数组的数组，它引用了四个水平数组。垂直数组的第一和第二元素都引用了同一个水平数组。

多维数组的所有维度都应该有相同的大小。但是在数组的数组中，可以引用不同大小的数组。例如，右边的图显示了垂直数组引用了长度为 7，7，2，null (即，没有数组)，4 和 1 的数组。

在 CIL 指令集或 VES 中，这些所谓的 *锯齿形数组* 没有特殊的支持。它们只是向量，其元素可以引用其他 (递归地) 锯齿形数组。

>---
### 12.3. 枚举

**枚举** (*enumeration*，简称 *enum*) 定义了一组具有相同类型的符号。一个类型当且仅当它的直接基类型为 `System.Enum` 时才是枚举。由于 `System.Enum` 本身的直接基类型为 `System.ValueType`，枚举是值类型。枚举的符号由 **底层** (_underlying_) 整数类型 (`bool`, `char`, `int8`, `unsigned int8`, `int16`, `unsigned int16`, `int32`, `unsigned int32`, `int64`, `unsigned int64`, `native int`, `unsigned native int`) 之一表示。

与 Pascal 不同，CLI 并不保证枚举类型的值是与其中一个符号对应的整数。实际上，CLS 定义了一种使用枚举来表示位标志的约定，这些位标志可以组合形成枚举类型本身未命名的整数值 [[↗]](./01_CLI%20基本概念和体系结构.md/#enum-flags)。

枚举遵守其他值类型之外的一些额外限制。枚举只能包含字段作为成员 (它们甚至不能定义类型初始化器或实例构造器)；它们不应实现任何接口；它们应具有自动字段布局；它们应有且只有一个实例字段，且该字段应为枚举的底层类型；所有其他字段应为 **static** 和 **literal** [[↗]](#field-attr)；并且它们不应使用 `initobj` 指令进行初始化。这些限制使得枚举具有非常高效的实现。

单个必需的实例字段存储枚举实例的值。枚举的 **static literal** 字段声明了枚举符号到底层值的映射。所有这些字段都应具有枚举的类型，并应具有为它们赋值的字段初始化元数据 [[↗]](#field-init)。

出于绑定目的 (例如，为了从用于调用它的方法引用中定位方法定义)，枚举应与其底层类型区分开来。对于所有其他目的，包括代码的验证和执行，未装箱的枚举可以自由地与其底层类型互换。枚举可以被装箱为相应的装箱实例类型，但是这种类型不与底层类型的装箱类型相同，因此装箱不会丢失枚举的原始类型。

下面示例声明一个枚举类型，然后创建该类型的局部变量。将底层类型的常量存储到枚举中 (显示为从底层类型到枚举类型的自动强制转换)。返回加载枚举并将其作为底层类型打印 (显示自动强制转换返回)。最后，加载枚举的地址并提取实例字段的内容，然后将其打印出来。

```cil
.assembly Test { }
.assembly extern mscorlib { }

.class sealed public ErrorCodes extends [mscorlib]System.Enum
{ 
    .field public unsigned int8 MyValue
    .field public static literal valuetype ErrorCodes no_error = int8(0)
    .field public static literal valuetype ErrorCodes format_error = int8(1)
    .field public static literal valuetype ErrorCodes overflow_error = int8(2)
    .field public static literal valuetype ErrorCodes nonpositive_error = int8(3)
}

.method public static void Start()
{ 
    .maxstack 5
    .entrypoint
    .locals init (valuetype ErrorCodes errorCode)

    ldc.i4.1           // load 1 (= format_error)
    stloc errorCode    // store in local, note conversion to enum
    ldloc errorCode
    call void [mscorlib]System.Console::WriteLine(int32)
    ldloca errorCode   // address of enum
    ldfld unsigned int8 valuetype ErrorCodes::MyValue
    call void [mscorlib]System.Console::WriteLine(int32)
    ret
}
```

>---
### 12.4. 指针类型
<a id="pointer"></a>

<pre>
    <em>Type</em> ::= ... | <em>Type</em>'&' | <em>Type</em>'*'
</pre>

**指针类型** (_pointer type_) 应通过指定一个包含其指向位置的类型签名来定义。*指针* 可以是托管的 (_managed_，报告给 CLI 垃圾收集器，由 `&` 表示) 或 **非托管的** (_unmanaged_，不报告给 CLI 垃圾收集器，由 `*` 表示) 

**指针** (_pointers_) 可以包含字段 (对象或值类型) 的地址，或数组元素的地址。指针与对象引用的不同之处在于，它们不指向整个类型实例，而是指向实例的内部。CLI 提供了两种对指针的类型安全操作：
 * 加载指针引用位置的值。
 * 将值 _V_ 存储到指针 _P_ 引用的位置，其中 _V_ 的类型是 *可赋值给* _P_ 引用的类型。

对于指向同一数组或对象的指针，支持以下算术操作：

 * 将一个整数值加到一个指针上 (其中该值被解释为字节数)，结果是同一种类的指针
 * 从指针中减去一个整数值 (其中该值被解释为字节数)，结果是同一种类的指针。不允许从整数值中减去一个指针。
 * 两个指针，无论种类如何，都可以相互减去，产生一个整数值，该值指定它们引用的地址之间的字节数。

指针在 32 位架构上与 `unsigned int32` 兼容，在 64 位架构上与 `unsigned int64` 兼容。它们最好被视为 `unsigned int`，其大小根据运行时机器架构的不同而变化。

CIL 指令集包含计算字段、局部变量、参数和向量元素地址的指令：

 | 指令      | 描述               |
 | --------- | ------------------ |
 | `ldarga`  | 加载参数的地址     |
 | `ldelema` | 加载向量元素的地址 |
 | `ldflda`  | 加载字段的地址     |
 | `ldloca`  | 加载局部变量的地址 |
 | `ldsflda` | 加载静态字段的地址 |

一旦一个指针被加载到堆栈上，`ldind` 种类的指令可以用来加载它指向的数据项。同样，`stind` 种类的指令可以用来将数据存储到位置。

如果地址不在当前应用程序域内，CLI 将为 `ldflda` 指令抛出 `InvalidOperationException`。这种情况通常只出现在使用基类型为 `System.MarshalByRefObject` 的对象时。

#### 12.4.1. 非托管指针
<a id="unmanaged-pointer"></a>

非托管指针 (`*`) 是 C 和 C++ 等语言中使用的传统指针。它们的使用没有限制，尽管在大多数情况下，它们可能会导致无法验证的代码。虽然将包含非托管指针的位置标记为无符号整数是完全有效的 (实际上，VES 就是这样处理它们的)，但通常最好将它们标记为指向特定数据类型的非托管指针。这是通过在返回值、局部变量或参数的签名中使用 `*` 来实现，或者在字段或数组元素中使用指针类型来实现。

 * 非托管指针不会被报告给垃圾收集器，并且可以以任何方式使用，就像整数一样。
 * 可验证的代码不能解引用非托管指针。
 * 未验证   的代码可以将非托管指针传递给期望托管指针的方法。只有在以下情况之一为真时，这才是安全的：
     1. 非托管指针引用的内存不在 CLI 用于存储对象实例的内存中 (“垃圾收集内存” 或 “托管内存”)。
     2. 非托管指针包含对象内字段的地址。
     3. 非托管指针包含数组元素的地址。
     4. 非托管指针包含数组中最后一个元素后面的元素所在的地址。

#### 12.4.2. 托管指针
<a id="managed-pointer"></a>

托管指针 (`&`) 可以指向值类型的实例、对象的字段、值类型的字段、数组的元素，或者存储数组末尾刚过去的元素的地址 (用于指向托管数组的指针索引)。托管指针不能为 *null*，即使它们不指向托管内存，也应该报告给垃圾收集器。

托管指针是通过在返回值、局部变量或参数的签名中使用 `&`，或者在字段或数组元素中使用 **byref** 类型来指定的。

 * 托管指针可以作为参数传递，存储在局部变量中，并作为值返回。
 * 如果参数是通过引用传递的，那么相应的参数就是一个托管指针。
 * 托管指针不能存储在静态变量、数组元素或对象或值类型的字段中。
 * 托管指针与对象引用不可互换。
 * 托管指针不能指向另一个托管指针，但它可以指向对象引用或值类型。
 * 托管指针可以指向局部变量，或方法参数
 * 不指向托管内存的托管指针可以被转换 (使用 `conv.u` 或 `conv.ovf.u`) 为非托管指针，但这是不可验证的。
 * 错误地将托管指针转换为非托管指针的未验证代码可能会严重损害 CLI 的完整性。

### 12.5. 方法指针
<a id="method-pointer"></a>

<pre>
    <em>Type</em> ::= ... | method <em>CallConv</em> <em>Type</em> '*' '(' <em>Parameters</em> ')'
</pre>

方法指针类型的变量应存储指向方法入口点的地址，该方法的签名与方法指针的类型 **方法签名兼容** (*method-signature-compatible-with*)。可以使用 `ldftn` 指令获取静态或实例方法的指针，而可以使用 `ldvirtftn` 指令获取虚方法的指针。可以使用 `calli` 指令并通过方法指针调用方法。

像其他指针一样，方法指针在 64 位架构上与 `unsigned int64` 兼容，在 32 位架构上与 `unsigned int32` 兼容。然而，首选的用法是 `unsigned native int`，它在 32 位和 64 位架构上都可以工作。

以下示例使用指针调用方法。方法 `MakeDecision::Decide` 返回一个指向 `AddOne` 或 `Negate` 的方法指针，每次调用时交替。主程序调用 `MakeDecision::Decide` 三次，每次调用后使用 `calli` 指令调用指定的方法。打印的输出是 "`-1 2 -1`" 时，表示成功的交替调用。

```cil
.assembly Test { }
.assembly extern mscorlib { }
 
.method public static int32 AddOne(int32 Input)
{ 
    .maxstack 5

    ldarg Input
    ldc.i4.1
    add
    ret
}

.method public static int32 Negate(int32 Input)
{ 
    .maxstack 5

    ldarg Input
    neg
    ret
}

.class value sealed public MakeDecision extends
       [mscorlib]System.ValueType
{ 
    .field static bool Oscillate
    .method public static method int32 *(int32) Decide()
    { 
        ldsfld bool valuetype MakeDecision::Oscillate
        dup
        not
        stsfld bool valuetype MakeDecision::Oscillate
        brfalse NegateIt
        ldftn int32 AddOne(int32)
        ret

    NegateIt:
        ldftn int32 Negate(int32)
        ret
    }
}

.method public static void Start()
{ 
    .maxstack 2
    .entrypoint

    ldc.i4.1
    call method int32 *(int32) valuetype MakeDecision::Decide()
    calli int32(int32)
    call  void [mscorlib]System.Console::WriteLine(int32)

    ldc.i4.1
    call method int32 *(int32) valuetype MakeDecision::Decide()
    calli int32(int32)
    call  void [mscorlib]System.Console::WriteLine(int32)

    ldc.i4.1
    call method int32 *(int32) valuetype MakeDecision::Decide()
    calli int32(int32)
    call  void [mscorlib]System.Console::WriteLine(int32)

    ret
}
```

### 12.6. 委托
<a id= "delegate"></a>

委托 [[↗]](./01_CLI%20基本概念和体系结构.md/#delegate-types) 是函数指针的面向对象等价物。与函数指针不同，委托是面向对象的、类型安全的，并且安全。委托是引用类型，并以类的形式声明。委托应具有 `System.Delegate` 基类型。

委托应声明为密封的，委托应具有的唯一成员是这里指定的前两个或所有四个方法。这些方法应声明为 **runtime** 和 **managed** (参见 [_方法的实现特性_](#method-init-attr) )。它们不应有主体，因为该主体将由 VES 自动创建。委托上可用的其他方法是从基类库中的类 `System.Delegate` 继承的。委托方法包括：
 * 实例构造函数 (名为 `.ctor` 并标记为 **specialname** 和 **rtspecialname**，参见 [_类型构造器_](#type-constructor) ) 应恰好接受两个参数，第一个参数的类型为 `System.Object`，第二个参数的类型为 `System.IntPtr`。当实际调用时 (通过 `newobj` 指令)，第一个参数应是定义目标方法的类 (或其派生类) 的实例，第二个参数应是要调用方法的方法指针。
 + `Invoke`方法应为 **virtual**，其签名约束了可以绑定的目标方法，参见 [_委托签名的兼容性_](#delegate-signature)。验证器将对委托上 `Invoke` 方法的调用等同于对任何其他方法的调用。

 * 如果存在，`BeginInvoke` 方法 [[↗]](#delegate-begininvoke) 应为 **virtual** 的，并且具有与 `Invoke` 方法相关但不相同的签名。签名有两处不同。首先，它的返回类型应为 `System.IAsyncResult`。其次，在 `Invoke` 的参数之后还有两个额外的参数：第一个类型为 `System.AsyncCallback`，第二个类型为 `System.Object`。

 + `EndInvoke` 方法 [[↗]](#delegate-endinvoke) 应为 **virtual**，并具有与 `Invoke` 方法相同的返回类型。它应接受与 `Invoke` 中托管指针的参数作为参数，与它们在 `Invoke` 的签名中出现的顺序相同。此外，应有一个额外的类型为 `System.IAsyncResult` 的参数。

除非另有说明，标准委托类型应提供两个可选的异步方法：`BeginInvoke` 和 `EndInvoke`。

下面声明了一个用于调用接受单个整数并返回无结果的函数的委托。它提供了所有四个方法，因此可以同步或异步调用。因为没有参数是通过引用传递的 (即，作为托管指针)，所以 `EndInvoke` 没有额外的参数。

<a id="dele-exam"></a>

```csharp
delegate void StartStopEventHandler(int action);
```
```cil
.assembly Test { }
.assembly extern mscorlib { }

.class private sealed StartStopEventHandler extends [mscorlib]System.Delegate
{ 
    .method public specialname rtspecialname instance void .ctor(object Instance,  
        native int Method) runtime managed {}
  
    .method public virtual void Invoke(int32 action) runtime managed {}
    
    .method public virtual class [mscorlib]System.IAsyncResult BeginInvoke(
        int32 action, class [mscorlib]System.AsyncCallback callback,
        object Instance) runtime managed {}
  
    .method public virtual void EndInvoke(class
        [mscorlib]System.IAsyncResult result) runtime managed {}
}
```

与任何类一样，使用 `newobj` 指令与实例构造函数一起创建委托实例。构造函数的第一个参数应该是要调用该方法的对象，如果方法是静态方法，则该参数应为 null。第二个参数应该是指向对应类上的方法的方法指针，并且具有与被实例化的委托类相匹配的签名。

#### 12.6.1. 委托签名兼容性
<a id="delegate-signature"></a>

本节定义了 ***委托可赋值给*** (_delegate-assignable-to_) 的关系，它是 *方法签名兼容* ([[↗]](./01_CLI%20基本概念和体系结构.md/#method-signature-compatible-with)) 的变体，并涵盖了委托构造。

委托通过 `newobj` 指令绑定到目标方法，如果目标是实例方法，则传递目标方法的方法指针和对象引用；如果目标是静态方法，则传递目标方法指针和 null。方法的签名可用时，通过 `ldftn`、`ldvirtftn` 或任一的 *load* IL 指令将目标方法加载到求值堆栈上。

**委托的签名** (_signature of a delegate_) 是委托类型上的 `Invoke` 方法的签名。签名不包括在委托创建时绑定的 **this** 指针的类型 (如果有)。

委托只能在以下情况下可验证时才能绑定到目标方法：
 1. 目标方法的签名是 *委托可赋值给* 委托的签名；
 2. 如果目标是实例方法，对象引用的验证类型是 *验证器可赋值给* (【】1.8.1.2.3)) 目标方法的 `this` 签名；如果目标方法是静态方法，对象引用的验证类型则是 `null`。

委托构造的特殊验证规则由 `newobj` (【】§[III.4.21]()) 指令捕获。

*委托可赋值给* 关系是根据参数类型定义的，忽略任何 **this** 参数 (如果有) 、返回类型和调用约定。自定义修饰符不被视为必要，也不会影响兼容性。

当且仅当满足以下所有条件时，类型为 _T_ 的目标方法或委托是 *委托可赋值给* 类型为 _D_ 的委托：
 1. _T_ 和 _D_ 的调用约定应完全匹配，忽略静态方法和实例方法之间的区别 (即，**this** 参数 (如果有) 不被特殊对待)。*委托可赋值给* 不考虑 **this** 的类型 (如果有)，这是由上述额外的验证规则所涵盖的。
 2. _T_ 和 _D_ 具有相同数量的参数，如果 _T_ 是方法，则忽略任何 `this` 参数。
 3. 对于 _T_ 的每个参数类型 _U_，如果 _T_ 是方法，则忽略任何 `this` 参数，以及 _D_ 的对应类型 _V_，_U_ 是 *可赋值给*[[↗]](./01_CLI%20基本概念和体系结构.md/#assignable-to) _V_。

 4. _T_ 的返回类型 _U_ 和 _D_ 的返回类型 _V_，_V_ 是 *可赋值给* _U_。

#### 12.6.2. 对委托的同步调用

对委托的同步调用方式对应于常规的方法调用，通过在委托上调用名为 `Invoke` 的虚方法来执行。委托本身是此调用的第一个参数 (它充当 **this** 指针)，后面跟着签名中指定的其他参数。进行此调用时，调用方将阻塞，直到被调用的方法返回。被调用的方法应在与调用方相同的线程上执行。

继续前面的 [示例](#dele-exam)，定义一个类 `Test`，该类声明了一个方法 `onStartStop`，适合用作委托的目标。

```cil
.class public Test
{ 
    .field public int32 MyData
    .method public void onStartStop(int32 action)
    { 
        // put your code here
        ret
    }
    .method public specialname rtspecialname instance void .ctor(int32 Data)
    {   
        // call base class constructor, store state, etc.
        ret        
    }
}
```

然后定义一个主程序。这个程序构造了一个 `Test` 的实例，然后创建了一个指向该实例的 `onStartStop` 方法的委托。最后，调用委托。

```cil
.method public static void Start()
{ 
    .maxstack 3
    .entrypoint
    .locals (
        class StartStopEventHandler DelegateOne,
        class Test InstanceOne )
    
    // Create instance of Test class
    ldc.i4.1
    newobj instance void Test::.ctor(int32)
    stloc InstanceOne 

    // Create delegate to onStartStop method of that class
    ldloc InstanceOne
    ldftn instance void Test::onStartStop(int32)
    newobj void StartStopEventHandler::.ctor(object, native int)
    stloc DelegateOne

    // Invoke the delegate, passing 100 as an argument
    ldloc DelegateOne
    ldc.i4 100
    callvirt instance void StartStopEventHandler::Invoke(int32)
    ret
}
```

注意，上面的示例创建了一个非虚函数的委托。如果 `onStartStop` 是一个虚函数，使用以下代码序列代替：

```cil
    ldloc InstanceOne
    dup
    ldvirtftn instance void Test::onStartStop(int32)
    newobj void StartStopEventHandler::.ctor(object, native int)
    stloc DelegateOne

    // Invoke the delegate, passing 100 as an argument
    ldloc DelegateOne
```

上面的代码序列应使用 `dup` —— 而不是两次 `ldloc InstanceOne`。`dup` 代码序列很容易被识别为类型安全的，而替代方案则需要更复杂的分析。代码的可验证性在第三部分 [【】]() 中讨论。

#### 12.6.3. 委托的异步调用

在异步模式下，调用被分派，调用方将继续执行而不等待方法返回。被调用的方法将在一个单独的线程上执行。使用 `BeginInvoke` 和 `EndInvoke` 方法异步调用委托。

如果调用方线程在被调用方完成之前终止，被调用方线程不受影响。被调用方线程继续执行并静默终止。被调用方可以抛出异常，任何未处理的异常通过 `EndInvoke` 方法传播到调用方。

##### 12.6.3.1. BeginInvoke 方法
<a id="delegate-begininvoke"></a>

对委托的异步调用应该从对 `BeginInvoke` 方法的虚调用开始。`BeginInvoke` 类似于 `Invoke` 方法，但有两个不同之处：
 * 它有两个额外的参数，附加到列表中，类型为 `System.AsyncCallback` 和 `System.Object`。
 * 方法的返回类型是 `System.IAsyncResult`。

尽管 `BeginInvoke` 方法因此包括表示返回值的参数，但这些值不会被此方法更新。结果反而是从 `EndInvoke` 方法获取的。与同步调用不同，异步调用应该为调用方提供一种确定调用何时完成的方式。CLI 提供了两种这样的机制。第一种是通过调用返回的结果对象，这个对象是接口 `System.IAsyncResult` 的一个实例，可以用来等待计算结果，也可以查询该结果值以获取方法调用的当前状态，并且它包含传递给 `BeginInvoke` 调用的 `System.Object` 值。

第二种机制是通过传递给 `BeginInvoke` 的 `System.AsyncCallback` 委托。当计算完成或者已经引发了异常致使结果不可用时，VES 将调用这个委托。传递给这个回调委托的值与调用 `BeginInvoke` 返回的值相同。可以为 `System.AsyncCallback` 传递 null 值，以表示 VES 不需要提供回调。

这种模型支持对异步调用的轮询方法 (通过检查返回的 `System.IAsyncResult` 的状态) 和事件驱动方法 (通过提供 `System.AsyncCallback`)。

同步调用通过其返回值和输出参数返回信息。输出参数在 CLI 中表示为具有托管指针类型的参数。只有当 VES 信号异步调用已成功完成时，返回的值和输出参数的值才可用。它们是通过调用开启异步调用委托上的 `EndInvoke` 方法来检索的。

##### 12.6.3.2. EndInvoke 方法
<a id="delegate-endinvoke"></a>

`EndInvoke` 方法可以在 `BeginInvoke` 之后的任何时间被调用。它将暂停调用它的线程，直到异步调用完成。如果调用成功完成，`EndInvoke` 将返回同步调用委托时本应返回的值，而其托管指针参数将指向同步调用的 `out` 参数本应返回的值。

`EndInvoke` 需要作为参数的是原始调用 `BeginInvoke` 返回的值 (这样可以区分对同一委托的不同调用，因为它们可以并发执行)，以及作为参数传递的任何托管指针 (这样可以提供它们的返回值)。

---
## 13. 方法定义、方法引用和方法调用
<a id="method"></a>

方法可以在全局级别 (在任何类型之外) 定义：

<pre>
    <em>Decl</em> ::= ... | .method <em>MethodHeader</em> '{' <em>MethodBodyItem</em>* '}'
</pre>

也可以在类型内部定义：

<pre>
    <em>ClassMember</em> ::= ... | .method <em>MethodHeader</em> '{' <em>MethodBodyItem</em>* '}'
</pre>

>---
### 13.1. 方法描述符

方法描述符在 ILAsm 中有四种结构与方法相关。这些结构对应于不同的元数据结构。参见 [_元数据逻辑格式_](#metadata-format-others)。

#### 13.1.1. MethodDecl：方法声明
<a id= "MethodDecl"></a>

_MethodDecl_ 或方法声明提供了方法名称和签名 (参数和返回类型)，但没有提供其主体。也就是说，方法声明提供了一个 _MethodHeader_，但没有 _MethodBodyItems_。它们在调用点被用来指定一个调用目标 (`call` 或 `callvirt` 指令) 或声明一个抽象方法。_MethodDecl_ 在元数据中没有直接的逻辑对应项：它可以是 _Method_ 或 _MethodRef_。

#### 13.1.2. Method：方法定义
<a id="Method"></a>

一个 _Method_，或者说方法定义，提供了方法名、特性、签名和主体。也就是说，一个方法定义提供了一个 _MethodHeader_ 以及一个或多个 _MethodBodyItems_。主体包括方法的 CIL 指令、异常处理程序、局部变量信息，以及关于方法的其他运行时或自定义元数据。参见 [「_MethodDef: 0x06_」](#MethodDef_0x06)。

#### 13.1.3. MethodRef：方法引用
<a id ="MethodRef"></a>

_MethodRef_ 或方法引用，是对方法的引用。当调用一个方法且该方法的定义位于另一个模块或程序集中时，就会使用它。在运行时调用方法之前，VES 应将 _MethodRef_ 解析为 _Method_。如果找不到匹配的 _Method_，VES 将抛出 `System.MissingMethodException`。参见 [「_MemberRef: 0x0A_」](#MemberRef_0x0A)。

#### 13.1.4. MethodImpl：方法实现
<a id="MethodImpl"></a>

_MethodImpl_ 或方法实现为现有的虚方法提供可执行的主体。它将一个 _Method_ (代表主体) 与一个 _MethodDecl_ 或 _Method_ (代表虚方法) 关联起来。当默认机制 (通过名称和签名匹配) 不能提供正确的结果时，_MethodImpl_ 用于为继承的虚方法或接口的虚方法提供实现。参见 [「_MethodImpl: 0x19_」](#MethodImpl_0x19)。

>---
### 13.2. 静态、实例和虚方法
<a id="method-static-instance-virtual"></a>

静态方法是与类型关联的方法，而不是与其实例关联的。

实例方法与类型的实例关联：在实例方法的主体内，可以引用正在操作的特定实例 (通过 *this* 指针)。因此，实例方法只能在类或值类型中定义，而不能在接口或类型之外 (即全局) 定义。然而要注意：
 - 类 (包括装箱的值类型) 上的实例方法，默认情况下有一个 *this* 指针，它是对定义该方法的类的对象引用。
 -  (未装箱的) 值类型上的实例方法，默认情况下有一个 *this* 指针，它是对定义该方法类型的实例的托管指针。
 - 有一种特殊的编码，由调用约定中的语法项 **explicit** 表示，调用约定用于指定 *this* 指针的类型，以覆盖这里指定的默认值。
 - *this* 指针可以为 `null`。

虚方法与类型的实例关联，与实例方法的方式非常相似。与实例方法不同的是，可以以一种方式调用虚方法，即方法的实现将由 VES 在运行时根据用于 *this* 指针的对象类型来选择。当通过 `callvirt` 指令调用时，实现虚方法的特定 _Method_ 在运行时动态确定 (虚调用)；而当通过 `call` 指令调用时，绑定是在编译时决定的。

只有在虚调用 (仅限) 时，继承的概念才变得重要。派生类可以重写从其基类继承的虚方法，提供方法的新实现。方法特性 **newslot** 指定 CLI 不应重写基类型的虚方法定义，而应将新定义视为独立的虚方法定义。

抽象虚方法 (只能在抽象类或接口中定义) 只能通过 `callvirt` 指令调用。同样，抽象虚方法的地址应通过 `ldvirtftn` 指令计算，不应使用 `ldftn` 指令。

对于具体的虚方法，总是可以从包含其定义的类中找到一个可用的实现，因此在运行时不需要特定提供一个类的可用实例。但是，抽象虚方法只能从实现适当接口的子类型或类中获取实现，因此必须提供一个具体实现该方法的类的实例。 

>---
### 13.3. CallConv：调用约定
<a id="calling-convention"></a>

<pre>
    <em>CallConv</em> ::= [ instance [ explicit ]] [ <em>CallKind</em> ]
</pre>

调用约定指定了方法要求它的参数应如何从调用方传递给被调用的方法。调用约定由两部分组成：第一部分处理 *this* 指针的存在和类型，第二部分涉及传输参数的机制。

如果存在 **instance** 特性，它表示应将 *this* 指针传递给方法。这个特性应该用于实例方法和虚方法。

通常，参数列表 (总是跟在调用约定后面) 不提供关于 *this* 指针类型的信息，它可以从其他信息中推断出来。但是，当指定了 **instance explicit** 组合时，随后的参数列表中的第一个类型指定了 *this* 指针的类型，随后的项指定了其他参数本身的类型。

<pre>
    <em>CallKind</em> ::= default | unmanaged cdecl | unmanaged fastcall | unmanaged stdcall | unmanaged thiscall | vararg
</pre>

托管代码只能有 **default** 或 **vararg** 调用种类。**default** 应在所有情况下使用，除非一个方法接受任意数量的参数，在这种情况下应使用 **vararg**。在处理 CLI 之外实现的方法时，能够指定所需的调用约定是很重要的。因此，调用种类有 16 种可能的编码。其中两种用于托管调用种类。有四种在许多平台上具有定义的含义，如：

 * **unmanaged cdecl** 是 Standard C 使用的调用约定
 * **unmanaged stdcall** 指定了一个标准的 C++ 调用
 * **unmanaged fastcall** 是一种特殊优化的 C++ 调用约定
 * **unmanaged thiscall** 是一个将 *this* 指针传递给方法的 C++ 调用

另外四种保留给现有的调用约定，但它们的使用并不是为了最大限度地实现可移植。还有四种保留给未来的标准化，两种用于非标准的实验性使用。(在这个上下文中，"可移植" 是指在所有符合 CLI 的实现上都可用的特性。) 

>---
### 13.4. MethodHeader：定义方法
<a id="MethodHeader"></a>

<pre>
    <em>MethodHeader</em> ::= <em>MethAttr</em>* [ <em>CallConv</em> ] <em>Type</em> 
                            [ marshal '(' [ <em>NativeType</em> ] ')' ] 
                            <em>MethodName</em> [ '<' <em>GenPars</em> '>' ] '(' <em>Parameters</em> ')' <em>ImplAttr</em>*
</pre>

_MethodHeader_ 包括：

 * 调用约定 (_CallConv_，参见 [[↗]](#calling-convention))
 * 任意数量的预定义方法特性 (_MethAttr_，参见 [[↗]](#MethAttr))
 * 带有可选特性的返回类型；没有返回值的方法应使用 **void** 作为返回类型
 * 可选的封送信息 (参见 [[↗]](#method-marshal))
 * 方法名称
 * 可选的泛型参数 (在定义泛型方法时，参见 [[↗]](#special-genpars)) 
 * 签名
 * 以及任意数量的实现特性 (_ImplAttr_，参见 [[↗]](#method-init-attr))


<pre>
    <em>MethodName</em> ::= .cctor | .ctor | <em>DottedName</em> 
</pre>

方法名称可以是简单名称，也可以是用于实例构造函数和类型初始化器的特殊名称。

<pre>
    <em>Parameters</em> ::= [ <em>Param</em> [ ',' <em>Param</em> ]* ]
    <em>Param</em> ::= ... | [ <em>ParamAttr</em>* ] <em>Type</em> [ marshal '(' <em>NativeType</em> ')' ] [ <em>Id</em> ]
</pre>

一个方法可以定义零个或若干方法参数，每个参数可以包含可选的参数特性和可选的封送信息。_Id_ (如果存在) 是参数的名称。参数可以通过使用其名称或参数的从零开始的索引来引用。在 CIL 指令中，它总是使用从零开始的索引进行编码 (使用名称是为了方便在 ILAsm 中使用)。

与调用 **vararg** 方法不同，**vararg** 方法的定义不包括任何省略号 ("`…`") 

<pre>
    <em>ParamAttr</em> ::= '[' in ']' | '[' opt ']' | '[' out ']' 
</pre>
```csharp
interface ISample
{
    void Fun<T>(in int v, out int v3, [Optional] T t, int v4 = 10010);
}
```
```cil
.class interface abstract ISample
{
	.method public hidebysig newslot abstract virtual 
		instance void Fun<T> (
			[in] int32& modreq([System.Runtime]System.Runtime.InteropServices.InAttribute) v,
			[out] int32& v3,
			[opt] !!T t,
			[opt] int32 v4
		) cil managed 
	{
		.param [4] = int32(10010)
	} 
}
```

参数特性应附加到参数上 (参见 [「_Param: 0x08_」](#Param_0x08))，因此它们不是方法签名的一部分。

与参数特性不同的是，自定义修饰符 (**modopt** 和 **modreq**) 是方法签名的一部分。因此，这些修饰符构成了方法协议的一部分，而参数特性则不是。

**in** 和 **out** 只能附加到指针类型 (托管或非托管) 的参数上。它们指定参数是打算向方法提供输入，还是从方法返回一个值，或两者都有。如果没有指定，则假定为 **in**。CLI 本身不强制这些位的语义，尽管它们可以用于优化性能，特别是在调用点和方法位于不同应用程序域、进程或计算机的场景中。

**opt** 指定此参数在从最终用户的角度来看是可选的。要提供的值使用 **.param** 语法存储 (参见 [[↗]](#param))。

#### 13.4.1. MethodBodyItem：方法体
<a id="MethodBody"></a>

方法体应包含程序的指令。它也可以包含标签、附加的语法形式和许多可以为 *ilasm* 提供额外信息且有助于编译某些语言方法的指令。

 | _MethodBodyItem_ ::=                                                                                       | 描述                                                   | 参考                      |
 | :--------------------------------------------------------------------------------------------------------- | ------------------------------------------------------ | ------------------------- |
 | `.custom` _CustomDecl_                                                                                     | 自定义特性的定义。                                     | §[[↗]](#custom)           |
 | \| `.data` _DataDecl_                                                                                      | 将数据发出到数据段。                                   | §[[↗]](#data)             |
 | \| `.emitbyte` _Int32_                                                                                     | 将一个无符号字节发出到方法的代码段。                   | §[[↗]](#emitbyte)         |
 | \| `.entrypoint`                                                                                           | 指定此方法是应用程序的入口点 (只允许一个这样的方法)。  | §[[↗]](#entrypoint)       |
 | \| `.locals` [ `init` ] `'('` _LocalsSignature_ `')'`                                                      | 为此方法定义一组局部变量。                             | §[[↗]](#locals)           |
 | \| `.maxstack` _Int32_                                                                                     | `int32` 指定在执行方法期间求值堆栈上的元素的最大数量。 |                           |
 | \| `.override` _TypeSpec_ `'::'` _MethodName_                                                              | 使用当前方法作为指定方法的实现。                       | §[[↗]](#override)         |
 | \| `.override method` _CallConv_ _Type_ _TypeSpec_ `'::'` _MethodName_ _GenArity_ `'('` _Parameters_ `')'` | 使用当前方法作为指定方法的实现。                       | §[[↗]](#override)         |
 | \| `.param` `'['` _Int32_ `']'` [ `'='` _FieldInit_ ]                                                      | 为参数 _Int32_ 存储一个常量 _FieldInit_ 值             | §[[↗]](#param)            |
 | \| `.param type` `'['` _Int32_ `']'`                                                                       | 指定泛型方法的类型参数。                               | §[[↗]](#param-type)       |
 | \| _ExternSourceDecl_                                                                                      | `.line` 或 `#line`。                                   | §[[↗]](#ExternSourceDecl) |
 | \| _Instr_                                                                                                 | 一条指令。                                             | 参见第四部分【】          |
 | \| _Id_ `':'`                                                                                              | 一个标签。                                             | §[[↗]](#id)               |
 | \| _ScopeBlock_                                                                                            | 局部变量的词法范围。                                   | §[[↗]](#ScopeBlock)       |
 | \| _SecurityDecl_                                                                                          | `.permission` 或 `.permissionset`。                    | §[[↗]](#SecurityDecl)     |
 | \| _SEHBlock_                                                                                              | 一个异常块。                                           | §[[↗]](#SEHBlock)         |

##### 13.4.1.1. .emitbyte 指令
<a id="emitbyte"></a>

<pre>
    <em>MethodBodyItem</em> ::= ... | .emitbyte <em>Int32</em>
</pre>
 
此指令会导致一个无符号的 8 位值直接被发射到方法的 CIL 流中，就在指令出现的地方。**.emitbyte** 指令用于生成测试。在生成常规程序时不需要它。

##### 13.4.1.2. .entrypoint 指令
<a id="entrypoint"></a>

<pre>
    <em>MethodBodyItem</em> ::= ... | .entrypoint
</pre>

**.entrypoint** 指令将当前方法 (应为静态方法) 标记为应用程序的入口点。VES 将调用此方法来启动应用程序。一个可执行文件应该有且只有一个入口点方法；库中的入口点方法不会被 VES 特别处理。这个入口点方法可以是全局方法，也可以出现在类型内部。 (指令的效果是将此方法的元数据 _token_ 放入 PE 文件的 CLI 头部) 

入口点方法应该不接受参数，或者接受一个字符串的向量。如果它接受一个字符串的向量，那么这些字符串索引 0 包含的第一个参数应该代表可执行文件的参数。指定这些参数的机制是特定于平台的，并未在此处指定。

入口点方法的返回类型应该是 **void**，**int32**，或 **unsigned int32**。如果返回了 **int32** 或 **unsigned int32**，可执行文件可以向主机环境返回一个退出代码。值 0 应该表示应用程序正常终止。

入口点方法的可访问性不应阻止其在开始执行时的使用。一旦开始，VES 应该像对待任何其他方法一样对待入口点。入口点方法不能在泛型类中定义。

下面的代码打印出第一个参数并成功返回到操作系统：

```cil
.method public static int32 MyEntry(string[] s) cil managed
{ 
    .entrypoint
    .maxstack 2
  
    ldarg.0                  // load and print the first argument
    ldc.i4.0
    ldelem.ref
    call void [mscorlib]System.Console::WriteLine(string)
    ldc.i4.0                 // return success
    ret
}
```

##### 13.4.1.3. .locals 指令
<a id="locals"></a>

**.locals** 语句为当前方法声明一个或多个局部变量。

<pre>
    <em>MethodBodyItem</em> ::= ... | .locals [ init ] '(' LocalsSignature ')'
    <em>LocalsSignature</em> ::= <em>Local</em> [ ',' <em>Local</em> ]*
    <em>Local</em> ::= <em>Type</em> [ <em>Id</em> ]
</pre>

如果存在，_Id_ 是相应局部变量的名称。如果指定了 **init**，则根据它们的类型将变量初始化为默认值：引用类型初始化为 `null`，值类型被清零。  

可验证的方法应包含 **init** 关键字。参见第三部分 【】。

下面的代码声明了 4 个局部变量，每个变量都将被初始化为其默认值：

```csharp
void Fun()
{
    int i,j; float f; long[] vect;
}
```
```cil
.method private hidebysig instance void Fun () cil managed 
{
	.maxstack 0
	.locals init (int32 i, int32 j, float32 f, int64[] vect)
    // ...
}
```

如果未指定 **init** 指令，则指示局部变量的声明时不需要初始化为默认值。

```csharp
[SkipLocalsInit]
void Fun()
{
    int i,j; float f; long[] vect;
}
```
```cil
.method private hidebysig instance void Fun () cil managed 
{
	.maxstack 0
	.locals (int32 i, int32 j, float32 f, int64[] vect)
    // ...
}
```

##### 13.4.1.4. .param 指令
<a id="param"></a>

<pre>
    <em>MethodBodyItem</em> ::= ... | .param '[' <em>Int32</em> ']' [ '=' <em>FieldInit</em> ]
</pre>

此指令在元数据中存储与方法参数编号 _Int32_ 关联的常量值，参见 [「_Constant: 0x0B_」](#Constant_0x0B)。虽然 CLI 要求为参数提供一个值，但一些工具可以使用此特性的存在来表明是工具而不是用户打算提供参数的值。与 CIL 指令不同，**.param** 使用索引 0 来指定方法的返回值，使用索引 1 来指定方法的第一个参数，使用索引 2 来指定方法的第二个参数，依此类推。

CLI 对这些值没有任何语义附加，完全由编译器来实现他们希望的任何语义 (例如，所谓的默认参数值)。

```csharp
public static void Fun([Optional, DefaultParameterValue(10086)] int v1, int v2 = 10010) { } 
```
```cil
.method public hidebysig static
	int32 Fun (
		[opt] int32 v1,
		[opt] int32 v2
	) cil managed 
{
	.param [1] = int32(10086)
	.param [2] = int32(10010)
    //...
} 
```

##### 13.4.1.5. .param type 指令
<a id="param-type"></a>

<pre>
    <em>MethodBodyItem</em> ::= ... | .param type '[' <em>Int32</em> ']'
</pre>

此指令允许为泛型类型或方法指定类型参数。_Int32_ 是应用该指令的类型或方法参数的基于 1 的序数。此指令与 **.custom** 指令一起使用，以将自定义特性与类型参数关联。

当在类范围内使用 **.param type** 指令时，它指的是该类的类型参数。当在类定义内的方法范围内使用该指令时，它指的是该方法的类型参数。否则，程序格式不正确。

```csharp
public class G<T, U> where T : notnull
{
#nullable disable
    public void Foo<M>(M m) { }
#nullable enable
}
```
```cil
.class public G`2<T, U> extends [System.Runtime]System.Object
{
	.param type U // or [2] : refer to U
		.custom instance void System.Runtime.CompilerServices.NullableAttribute::.ctor(uint8) = (01 00 02 00 00)
	// Methods
	.method public instance void Foo<M> (!!M m) cil managed 
	{
		.param type M  // or [1] : refer to M 
			.custom instance void System.Runtime.CompilerServices.NullableAttribute::.ctor(uint8) = (01 00 02 00 00)
        ...
	} 
    ...
}
```

#### 13.4.2. 方法上的预定义特性
<a id="MethAttr"></a>

 | _MethAttr_ ::=                                                        | 描述                                     | 参考                       |
 | :-------------------------------------------------------------------- | ---------------------------------------- | -------------------------- |
 | `abstract`                                                            | 方法是抽象的 (也必须是虚的)。            | §[[↗]](#MethAttr-abstract) |
 | \| `assembly`                                                         | 程序集可访问性                           | §[[↗]](#accessibility)     |
 | \| `compilercontrolled`                                               | 编译器控制的可访问性。                   | §[[↗]](#accessibility)     |
 | \| `famandassem`                                                      | **family** 和 **assembly** 可访问性      | §[[↗]](#accessibility)     |
 | \| `family`                                                           | **family** 可访问性                      | §[[↗]](#accessibility)     |
 | \| `famorassem`                                                       | **family** 或 **assembly** 可访问性      | §[[↗]](#accessibility)     |
 | \| `final`                                                            | 此虚方法不能被派生类重写。               | §[[↗]](#final)             |
 | \| `hidebysig`                                                        | 通过签名隐藏。运行时忽略。               | §[[↗]](#hidebysig)         |
 | \| `newslot`                                                          | 指定此方法应在虚方法表中获取新的插槽。   | §[[↗]](#newslot)           |
 | \| `pinvokeimpl` `'('` _QSTRING_ [ `as` _QSTRING_ ] _PinvAttr_* `')'` | 方法实际上是在底层平台的本地代码中实现的 | §[[↗]](#pinvokeimpl)       |
 | \| `private`                                                          | 私有可访问性                             | §[[↗]](#accessibility)     |
 | \| `public`                                                           | 公共可访问性。                           | §[[↗]](#accessibility)     |
 | \| `rtspecialname`                                                    | 方法名需要由运行时以特殊方式处理。       | §[[↗]](#rtspecialname)     |
 | \| `specialname`                                                      | 方法名需要由某些工具以特殊方式处理。     | §[[↗]](#specialname)       |
 | \| `static`                                                           | 方法是 **static** 的。                   | §[[↗]](#method-contract)   |
 | \| `virtual`                                                          | 方法是 **virtual** 的。                  | §[[↗]](#method-contract)   |
 | \| `strict`                                                           | 在重写时检查可访问性                     | §[[↗]](#method-contract)   |

以下预定义特性的组合是无效的：

 * **static** 与 **final**、**newslot** 或 **virtual** 的任何一个组合
 * **abstract** 与 **final** 或 **pinvokeimpl** 的任何一个组合
 * **compilercontrolled** 与 **final**、**rtspecialname**、**specialname** 或 **virtual** 的任何一个组合

##### 13.4.2.1. 可访问性信息
<a id="accessibility"></a>

<pre>
    <em>MethAttr</em> ::= ... | assembly | compilercontrolled | famandassem | family | famorassem | private | public
</pre>

这些特性中只有一个可以应用到给定的方法上。参见第一部分的 [_可见性和可访问性_](./01_CLI%20基本概念和体系结构.md/#visible)。这些特性定义了方法的可访问性，即它们规定了哪些代码可以访问该方法。例如，`public` 特性表示任何代码都可以访问该方法，而 `private` 特性表示只有定义该方法的类的代码才能访问该方法。其他特性提供了更复杂的访问控制，允许在类的继承层次结构中的不同级别进行访问。

##### 13.4.2.2. 方法协议特性
<a id="method-contract"></a>

<pre>
    <em>MethAttr</em> ::= ... | final | hidebysig | static | virtual | strict
</pre>

这些特性可以组合，除非一个方法既是 **static** 又是 **virtual**；只有 **virtual** 方法才能是 **final** 或 **strict**；并且 **abstract** 方法不能是 **final**。

**final** <a id="final"></a>方法不能被此类型的派生类重写。

**hidebysig** <a id="hidebysig"></a>是为工具的使用提供的，被 VES 忽略。它指定声明的方法隐藏所有具有匹配方法签名的基类类型的方法；当省略时，方法应隐藏所有同名的方法，无论签名如何。一些语言 (如 C++) 使用 *hide-by-name* 语义，而其他语言 (如 C#、Java&trade;) 使用 *hide-by-name-and-signature* 语义。

**static** 和 **virtual** 在 [_静态、实例和虚方法_](#method-static-instance-virtual) 中有描述。

只有当它们也是可访问的时，**strict virtual** 方法才能被重写。参见 §[II.23.1.10](ii.23.1.10-flags-for-methods-methodattributes.md)。

```csharp
interface ISample<T, U> where U : ISample<T, U>
{
    static virtual void StaticVirtual(T t) { }
    static abstract U Instance { get; }
}
abstract class BaseSample
{
    internal virtual void Fun() => Console.WriteLine("Base.Fun");
}
class Sample : BaseSample, ISample<int, Sample>
{
    private static readonly Sample s_instance = new Sample();
    public static Sample Instance => s_instance;
    // 重写接口静态虚方法
    static void ISample<int, Sample>.StaticVirtual(int t)
    {
        Console.WriteLine(t);
    }
    // 重写基类方法，并密封
    internal sealed override void Fun() => Console.WriteLine("Sample.Fun");
}
```
```cil
// 接口定义
.class interface private abstract ISample`1<T>
{
    // 静态虚拟方法
	.method public hidebysig virtual static 
		void StaticVirtual (!T t) cil managed { ... }
    // 静态抽象属性
    .method public hidebysig specialname abstract virtual static 
		!U get_Instance () cil managed { ... }
}
// 基类定义
.class private abstract BaseSample extends [System.Runtime]System.Object
{
    // 虚方法定义
	.method assembly hidebysig newslot strict virtual 
		instance void Fun () cil managed { ... }
}
// 实现类定义
.class private Sample extends BaseSample implements class ISample`2<int32, class Sample>
{
    .field private static initonly class Sample s_instance
    .property class Sample Instance() { ... }  // 属性声明
    .method public hidebysig specialname static class Sample get_Instance () cil managed 
	{
		.override method !1 class ISample`2<int32, class Sample>::get_Instance() // 实现静态抽象属性
        ...
    }
    .method private hidebysig static void 'ISample<System.Int32,Sample>.StaticVirtual' (int32 t) cil managed 
	{
		.override method void class ISample`2<int32, class Sample>::StaticVirtual(!0)  // 重写静态虚方法
        ...
    }
    .method assembly final hidebysig virtual instance void Fun () cil managed { ... }  // 密封并重写父类虚方法
}
```

##### 13.4.2.3. 覆盖行为
<a id="newslot"></a>

<pre>
    <em>MethAttr</em> ::= ... | newslot virtual
</pre>

**newslot** 只能与 **virtual** 方法一起使用。参见 [_引入虚方法_](#impl-newslot-virtual)。

```csharp
class Base
{
    public virtual void Fun() => Console.WriteLine("Base.FunA");
}
class Derived : Base
{
    public new virtual void Fun() => Console.WriteLine("Derived.FunB");
    static void Main(string[] args)
    {
        Base b = new Derived();
        b.Fun();   // Base.FunA
    }
}
```
```cil
.class private Base
{
	.method public hidebysig newslot virtual instance void Fun ()
        cil managed { ... }   // 基类虚方法声明
}
.class Derived extends Base
{
	.method public hidebysig newslot virtual instance void Fun () 
        cil managed { ... }   // 子类覆盖声明
}
```


##### 13.4.2.4. 方法特性
<a id="MethAttr-abstract"></a>

<pre>
    <em>MethAttr</em> ::= ... | abstract
</pre>

**abstract** 只能与不是 **final** 的 **virtual** 方法一起使用。它指定方法在定义类型中没有提供实现，但必须由派生类提供。**abstract** 方法只能出现在 **abstract** 类型中 [[↗]](#Inheritance-attr)。

##### 13.4.2.5. 互操作特性
<a id ="pinvokeimpl"></a>

<pre>
    <em>MethAttr</em> ::= ... | pinvokeimpl '(' <em>QSTRING</em> [ as <em>QSTRING</em> ] <em>PinvAttr</em>* ')'
</pre>

参见 [_函数指针调用方法_](#method-pointer-pinvokeimpl) 和 [「_ImplMap: 0x1C_」](#ImplMap_0x1C)。

##### 13.4.2.6. 特殊处理特性
<a id="special-handle-attr"></a>

<pre>
    <em>MethAttr</em> ::= ... | rtspecialname | specialname
</pre>

**rtspecialname** <a id="rtspecialname"></a>特性指定运行时应以特殊方式处理方法名。特殊名称的例子包括 `.ctor` (对象构造函数) 和 `.cctor` (类型初始化器)。

**specialname** <a id="specialname"></a>表示此方法的名称对某些工具有特殊含义。

```csharp
struct Counter
{
    public int counter;
    static Counter() { } // .cctor
    public Counter(int num) => counter = num; // .ctor
    public static Counter operator ++(Counter c)  // op_Increment
    {
        c.counter++;
        return c;
    }
}
```
```cil
.class private sealed Counter extends [System.Runtime]System.ValueType
{
	// Fields
	.field public int32 counter

	// Methods
	.method private hidebysig specialname rtspecialname static 
		void .cctor () cil managed { ... }
	.method public hidebysig specialname rtspecialname 
		instance void .ctor (int32 num) cil managed { ...}
	.method public hidebysig specialname static 
		valuetype Counter op_Increment (valuetype Counter c) cil managed { ... }
} 
```

#### 13.4.3. 方法的实现特性
<a id="method-init-attr"></a>

 | _ImplAttr_ ::=      | 描述                                   | 条款                                 |
 | :------------------ | -------------------------------------- | ------------------------------------ |
 | `cil`               | 方法包含标准的 CIL 代码。              | §[[↗]](#code-impl-attr)              |
 | \| `forwardref`     | 此方法的主体没有在此声明中指定。       | §[[↗]](#method-impl-info)            |
 | \| `internalcall`   | 表示方法主体由 CLI 本身提供            | §[[↗]](#method-impl-info)            |
 | \| `managed`        | 方法是一个托管方法。                   | §[[↗]](#method-managed-or-unmanaged) |
 | \| `native`         | 方法包含本地代码。                     | §[[↗]](#code-impl-attr)              |
 | \| `noinlining`     | 运行时不应将方法内联展开。             | §[[↗]](#method-impl-info)            |
 | \| `nooptimization` | 在生成本地代码时，运行时不应优化方法。 | §[[↗]](#method-impl-info)            |
 | \| `runtime`        | 方法的主体未定义，但由运行时生成。     | §[[↗]](#code-impl-attr)              |
 | \| `synchronized`   | 方法应以单线程方式执行。               | §[[↗]](#method-impl-info)            |
 | \| `unmanaged`      | 指定方法是非托管的。                   | §[[↗]](#method-managed-or-unmanaged) |

##### 13.4.3.1. 代码实现方式
<a id="code-impl-attr"></a>

<pre>
    <em>ImplAttr</em> ::= ... | cil | native | runtime
</pre>

这些特性是互斥的；它们指定了方法包含的代码类型。

**cil** 指定方法体由 cil 代码组成。除非方法被声明为 **abstract**，否则如果使用 **cil**，就必须提供方法体。

**native** 指定方法是使用与其生成的特定处理器相关的本地代码实现的。**native** 方法不应有主体，而应引用声明主体的本地方法。通常，CLI 的 **P/Invoke** 功能用于引用本地方法，参见 [_平台调用_](#method-pinvoke)。

**runtime** 指定方法的实现由运行时自动提供，主要用于委托的方法，参见 [_委托_](#delegate)。

##### 13.4.3.2. 托管或非托管
<a id="method-managed-or-unmanaged"></a>

<pre>
    <em>ImplAttr</em> ::= ... | cil | managed | unmanaged
</pre>

这些不能组合。使用 CIL 实现的方法是 **managed** 的。**unmanaged** 主要用于 P/Invoke，参见 [_平台调用_](#method-pinvoke)。

##### 13.4.3.3. 方法实现信息
<a id="method-impl-info"></a>

<pre>
    <em>ImplAttr</em> ::= ... | forwardref | internalcall | noinlining | nooptimization | synchronized
</pre>

这些特性可以组合：

- **forwardref** <a id="forwardref"></a>指定方法的主体在其他地方提供。当由 VES 加载程序集时，此特性不应存在。它用于将分别编译的模块组合并解析前向引用的工具 (如静态链接器)。

- **internalcall** <a id="internalcall"></a>指定方法主体由 CLI 本身提供 (通常用于系统库中的低级方法)。它不能应用于打算跨 CLI 实现使用的方法。

- **noinlining** <a id="noinlining"></a>指定运行时不应内联此方法。内联是指将调用指令替换为被调用方法的主体的过程，这可以由运行时出于优化目的而完成。**noinlining** 指定此方法的主体不应被 CIL-to-native-code 的编译器包含到任何调用方法的代码中；它应保持为一个单独的例程。

- **nooptimization** <a id="nooptimization"></a>指定 CIL-to-native-code 的编译器不应执行代码优化。指定一个非内联方法可以确保它在调试 (例如，显示堆栈跟踪) 和分析中保持 “可见”。它还为程序员提供了一种机制，可以覆盖 CIL-to-native-code 编译器用于内联的默认启发式方法。

- **synchronized** <a id="synchronized"></a>指定方法的整个主体应该是单线程的。如果此方法是实例方法或虚方法，则在进入方法之前应获取对象上的锁。如果此方法是静态方法，则在进入方法之前应获取其封闭类型上的锁。如果无法获取锁，请求线程不应继续进行，直到它被授予锁。这可能导致死锁。当方法退出时，无论是通过正常返回还是异常，锁都会被释放。使用 `tail.` 调用退出同步方法应该被实现为没有指定 `tail.`。

#### 13.4.4. 方法作用域块
<a od="ScopeBlock"></a>

<pre>
    <em>ScopeBlock</em> ::= '{' <em>MethodBodyItem</em>* '}'
</pre>

_ScopeBlock_ 用于将方法体的元素组合在一起。例如用于指定构成异常处理程序主体的代码序列。

#### 13.4.5. vararg 方法

**vararg** 方法接受可变数量的参数。它们应使用 **vararg** 调用约定。

在每个调用点，都应使用方法引用来描述传递的固定参数和可变参数的类型。参数列表的固定部分应与额外的参数用省略号分隔 [[↗]](./01_CLI%20基本概念和体系结构.md/#method-sign)。

方法引用由 _MethodRef_ [[↗]](#MemberRef_0x0A) 或 _MethodDef_ [[↗]](#MethodDef_0x06) 表示。即使方法在同一程序集中定义，也可能需要 _MethodRef_，因为 _MethodDef_ 只描述参数列表的固定部分。如果调用点没有传递任何额外的参数，那么它可以使用 _MethodDef_ 来调用在同一程序集中定义的 **vararg** 方法。

**vararg** 参数应通过使用 CIL 指令 `arglist` 获取参数列表的句柄来访问【第三部分】。句柄可以用于创建 `System.ArgIterator` 值类型的实例，该实例提供了一种类型安全的机制来访问参数【第四部分】。

下面的示例显示了如何声明一个 **vararg** 方法，以及如何访问第一个 **vararg** 参数，假设至少传递了一个额外的参数给该方法：

```cil
.method public static vararg void MyMethod(int32 required) 
{
    .maxstack 3
    .locals init (valuetype [mscorlib]System.ArgIterator it, int32 x)

    ldloca it    // 初始化迭代器
    initobj  valuetype [mscorlib]System.ArgIterator
    ldloca it
    arglist     // 获取参数句柄
    call instance void [mscorlib]System.ArgIterator::.ctor(valuetype
        [mscorlib]System.RuntimeArgumentHandle)   // 调用迭代器的构造函数
    /* 当检索到参数时，参数值将存储在 x 中，所以加载 x 的地址 */
    ldloca x
    ldloca it
    // 检索参数，对于 required 的参数不重要
    call instance typedref [mscorlib]System.ArgIterator::GetNextArg() 
    call object [mscorlib]System.TypedReference::ToObject(typedref)  /* 检索对象 */
    castclass [mscorlib]System.Int32  // 类型转换并拆箱
    unbox int32
    cpobj int32                       // 将值复制到 x 中
    // 第一个 vararg 参数存储在 x 中
    ret
}
```

>---
### 13.5. 非托管方法
<a id="unmanaged-method"></a>

除了支持托管代码和托管数据外，CLI 还提供了从底层平台访问预先存在的本地代码 (称为非托管代码) 的功能。这些功能必然是特定于平台的，因此在这里仅部分指定。

此标准指定：
 * 文件格式中的一种机制，用于向托管代码提供函数指针，该函数指针可以从非托管代码中调用 [[↗]](#method-transition-thunks)。
 * 一种将某些方法定义标记为在非托管代码中实现的机制 (称为 **平台调用** (_platform invoke_) [[↗]](#method-pinvoke) )。
 * 一种标记调用点的机制，并使用方法指针以指示调用是对非托管方法的调用 [[↗]](#method-pointer-pinvokeimpl)。
 * 一小部分预定义的数据类型，可以在所有 CLI 的实现上使用这些机制进行传递 (封送) [[↗]](#data-type-marshaling)。类型集合可以通过使用自定义特性和修饰符进行扩展，但这些扩展是特定于平台的。

#### 13.5.1. 方法转换嵌入块
<a id="method-transition-thunks"></a>

由于此机制不是 CLI **内核分析** (_Kernel Profile_) 的一部分，因此可能不会出现在所有符合 CLI 的实现中。参见第四部分 【】。

为了从非托管代码调用托管代码，一些平台需要执行特定的转换序列。此外，一些平台要求转换数据类型的表示 (数据封送)。这两个问题都通过 **.vtfixup** 指令解决。此指令可以出现多次，但只能在 CIL 程序集文件的顶级出现，如下面的语法所示：

<pre>
    <em>Decl</em> ::= .vtfixup <em>VTFixupDecl</em> | ... <a href="#il-top-impl">[↗]</a>
</pre>

**.vtfixup** <a id="vtfixup"></a>指令声明在某个内存位置有一个表，其中包含引用方法的元数据 _token_ ，这些方法被转换为方法指针。当包含 **.vtfixup** 指令的文件加载到内存中执行时，CLI 将自动进行此转换。声明指定了表中的条目数量、所需的方法指针的类型、表中条目的宽度和表的位置：

<pre>
    <em>VTFixupDecl</em> ::= [ <em>Int32</em> ] <em>VTFixupAttr</em>* at <em>DataLabel</em>
    <em>VTFixupAttr</em> ::= fromunmanaged | int32 | int64
</pre>

特性 **int32** 和 **int64** 是互斥的，**int32** 是默认值。这些特性指定了表中每个插槽的宽度。每个插槽包含一个 32 位的元数据 _token_  (如果表有 64 位插槽，则用零填充)，CLI 将其转换为与插槽同宽的方法指针。

如果指定了 **fromunmanaged**，CLI 将生成一个嵌入块，该嵌入块将非托管方法调用转换为托管调用，调用该方法并将结果返回到非托管环境。嵌入块还将以平台调用所描述的特定于平台的方式执行数据封送。

ILAsm 语法没有指定创建 _token_ 表的机制，但编译器可以简单地将 _token_ 作为字节文字发出到使用 **.data** 指令指定的块中。

#### 13.5.2. 平台调用
<a id="method-pinvoke"></a>

使用 CLI 的 *平台调用* (也称为 PInvoke 或 p/invoke) 功能可以调用在本地代码中定义的方法。平台调用将从托管状态切换到非托管状态，然后再切换回来，并处理必要的数据封送。需要使用 PInvoke 调用的方法被标记为 **pinvokeimpl**。此外，这些方法应具有实现特性 **native** 和 **unmanaged**。

<pre>
    <em>MethAttr</em> ::= pinvokeimpl '(' <em>QSTRING</em> [ as <em>QSTRING</em> ] <em>PinvAttr</em>* ')' | ... <a href="#param">[↗]</a>
</pre>

第一个引号内的字符串是一个平台特定的描述，指示方法的实现位于何处 (例如，在 Microsoft Windows&trade; 上，这将是实现该方法的 DLL 的名称)。第二个 (可选) 字符串是该方法在该平台上存在的名称，因为平台可以使用名称混淆规则，使得在托管程序中出现的名称与在本地实现中看到的名称不同 (例如，当本地代码由 C++ 编译器生成时，这是常见的)。

只有静态方法，定义在全局范围 (即，类型之外)，可以被标记为 **pinvokeimpl**。声明为 **pinvokeimpl** 的方法不应在定义的一部分中指定主体。

 | _PinvAttr_ ::=   | 描述 (平台特定，仅供参考)        |
 | :--------------- | -------------------------------- |
 | `ansi`           | ANSI 字符集。                    |
 | \| `autochar`    | 自动确定字符集。                 |
 | \| `cdecl`       | 标准 C 风格调用                  |
 | \| `fastcall`    | C 风格快速调用。                 |
 | \| `stdcall`     | 标准 C++ 风格调用。              |
 | \| `thiscall`    | 方法接受一个隐式的 *this* 指针。 |
 | \| `unicode`     | Unicode 字符集。                 |
 | \| `platformapi` | 使用适合目标平台的调用约定。     |

特性 **ansi**、**autochar** 和 **unicode** 是互斥的。它们决定了调用此方法时如何调用封送字符串：**ansi** 是本地代码将接收 (可能也会返回) 一个与 ANSI 字符集编码的字符串对应的平台特定表示 (通常，这将与 C 或 C++ 字符串常量的表示相匹配)；**autochar** 是特定于底层平台的最自然表示的平台特定表示；而 **unicode** 是用于该平台上的 Unicode 方法的字符串编码对应的平台特定表示。

特性 **cdecl**、**fastcall**、**stdcall**、**thiscall** 和 **platformapi** 是互斥的。它们是平台特定的，并指定本地代码的调用约定。

下面显示了位于 Microsoft Windows&trade; DLL `user32.dll` 中的方法 `MessageBeep` 的声明：

```cil
.method public static pinvokeimpl("user32.dll" stdcall) int8
       MessageBeep(unsigned int32) native unmanaged {}
```

#### 13.5.3. 通过函数指针调用方法
<a id="method-pointer-pinvokeimpl"></a>

可以通过函数指针调用非托管方法。使用指针调用托管方法或非托管方法之间没有区别。然而，非托管方法需要声明为 **pinvokeimpl**。使用函数指针调用托管方法在 [_方法指针_](#method-pointer) 中有描述。

#### 13.5.4. 数据类型封送
<a id="data-type-marshaling"></a>

虽然数据类型封送必然是特定于平台的，但此标准规定了一组最小的数据类型，所有符合 CLI 的实现都应支持这些数据类型。可以使用自定义特性和 / 或自定义修饰符以特定于平台的方式支持额外的数据类型，以指定特定实现所需的任何特殊处理。

以下数据类型应由所有符合 CLI 的实现进行封送；它们符合的本地数据类型是特定于实现的：

 * 所有整数数据类型 (**int8**，**int16**，**unsigned int8**，**bool**，**char** 等)，包括本地整数类型。
 * 枚举，作为其底层数据类型。
 * 所有浮点数据类型 (**float32** 和 **float64**)，如果 CLI 实现支持托管代码。
 * 类型 **string**。
 * 对上述任何类型的非托管指针。

此外，以下类型应支持从托管代码到非托管代码的封送，但不必在反向支持 (即，作为调用非托管方法时的返回类型或作为从非托管方法调用到托管方法时的参数) ：
 * 上述任何类型的一维零基数组。
 * 委托 (从非托管代码调用到委托的机制是特定于平台的；不应假定封送委托将生成可以直接从非托管代码使用的函数指针)。

最后，类型 `System.Runtime.InteropServices.GCHandle` 可用于将对象封送到非托管代码。非托管代码接收一个特定于平台的数据类型，该数据类型可以用作特定对象的 “不透明句柄”。参见第四部分 【】。

---

## 14. 字段定义和字段引用
<a id="field"></a>

字段是存储程序数据的类型化内存位置 (_typed memory locations_)。CLI 允许声明实例字段和静态字段。静态字段与类型关联，并在该类型的所有实例之间共享，而实例字段与该类型的特定实例关联。一旦实例化，每个实例就有其每个实例字段的自己的副本。CLI 还支持全局字段，这些字段是在任何类型定义之外声明的。全局字段应该是静态的。

字段由 **.field** 指令定义：[[↗]](#Field_0x04)

<pre>
    <em>Field</em> ::= .field <em>FieldDecl</em>
    <em>FieldDecl</em> ::= [ '[' <em>Int32</em> ']' ] <em>FieldAttr</em>* <em>Type</em> <em>Id</em> [ '=' <em>FieldInit</em> | at <em>DataLabel</em> ]
</pre>

_FieldDecl_ 有以下部分：
 * 一个可选的整数，指定字段在实例中的字节偏移量 [[↗]](#ctrl-layout)。如果存在，包含此字段的类型应具有 **explicit** 布局特性。对于全局或静态字段，不应提供偏移量。
 * 任意数量的字段特性 [[↗]](#field-init)。
 * 类型。
 * 名称。
 * 可选地，一个 _FieldInit_ [[↗]](#field-init) 子句或一个 [_DataLabel_](#DataLabel) 子句。

全局字段应该有一个与之关联的数据标签。这指定了 PE 文件中该字段的数据位置。类型的静态字段可以 (但非必需) 分配数据标签。

 ```cil
 .field private class [.module Counter.dll]Counter counter
 .field public static initonly int32 pointCount
 .field private int32 xOrigin
 .field public static int32 count at D_0001B040
 ```

>---
### 14.1. 字段的特性
<a id="field-attr"></a>

字段的特性指定了关于可访问性、协议信息、互操作特性以及特殊处理的信息。

以下各小节包含了关于字段的每组预定义特性的额外信息。

 | _FieldAttr_ ::=                       | 描述                                       | 子句                               |
 | :------------------------------------ | ------------------------------------------ | ---------------------------------- |
 | `assembly`                            | **assembly** 可访问性。                    | §[[↗]](#field-accessibility)       |
 | \| `famandassem`                      | **family** 和 **assembly** 可访问性。      | §[[↗]](#field-accessibility)       |
 | \| `family`                           | **family** 可访问性。                      | §[[↗]](#field-accessibility)       |
 | \| `famorassem`                       | **family** 或 **assembly** 可访问性。      | §[[↗]](#field-accessibility)       |
 | \| `initonly`                         | 标记为常量字段。                           | §[[↗]](#field-contract)            |
 | \| `literal`                          | 指定元数据字段。此字段在运行时不分配内存。 | §[[↗]](#field-contract)            |
 | \| `marshal` `'('` _NativeType_ `')'` | 封送处理信息。                             | §[[↗]](#field-marshal)             |
 | \| `notserialized`                    | 保留 (表示此字段不应被序列化)。            | §[[↗]](#field-contract)            |
 | \| `private`                          | **private** 可访问性。                     | §[[↗]](#field-accessibility)       |
 | \| `compilercontrolled`               | 编译器控制的可访问性。                     | §[[↗]](#field-accessibility)       |
 | \| `public`                           | **public** 可访问性。                      | §[[↗]](#field-accessibility)       |
 | \| `rtspecialname`                    | 运行时的特殊处理。                         | §[[↗]](#field-special-handle-attr) |
 | \| `specialname`                      | 其他工具的特殊名称。                       | §[[↗]](#field-special-handle-attr) |
 | \| `static`                           | 静态字段。                                 | §[[↗]](#field-contract)            |

#### 14.1.1. 可访问性信息
<a id="field-accessibility"></a>

可访问性特性包括 **assembly**、**famandassem**、**family**、**famorassem**、**private**、**compilercontrolled** 和 **public**。这些特性是互斥的。可访问性特性在 [[↗]](#visibility-accessibility-hide) 中有描述。


#### 14.1.2. 字段协议特性
<a id="field-contract"></a>

字段协议特性有 **initonly**，**literal**，**static** 和 **notserialized**。这些特性可以组合；然而，只有 **static** 字段可以是 **literal**。默认情况下，实例字段可以被序列化。

**static**<a id="static-field"></a> 指定字段与类型本身相关联，而不是与类型的实例相关联。静态字段可以在没有类型实例的情况下访问，例如通过静态方法。因此，在应用程序域内，静态字段在类型的所有实例之间共享，任何对此字段的修改都会影响所有实例。如果没有指定 **static**，则创建一个实例字段。

**initonly**<a id="initonly-field"></a> 标记了在初始化后是常量的字段。这些字段只能在构造函数内部发生变化。如果字段是静态字段，那么它只能在声明类型的类型初始化器内部发生变化。如果它是一个实例字段，那么它只能在声明类型的一个实例构造函数中发生变化。它不应在任何其他方法或任何其他构造函数中发生变化，包括派生类的构造函数。在 **initonly** 字段上使用 `ldflda` 或 `ldsflda` 会使代码无法验证。在无法验证的代码中，VES 不需要检查 **initonly** 字段是否在构造函数之外发生变化。如果一个方法改变了一个常量的值，VES 不需要报告任何错误。然而，这样的代码是无效的。

**literal**<a id="literal-field"></a> 指定此字段表示一个常量值；这样的字段应该被赋值。与 **initonly** 字段相比，**literal** 字段在运行时不存在。它们没有分配内存。**literal** 字段成为元数据的一部分，但不能被代码访问。**literal** 字段通过使用 _FieldInit_ 语法 [[↗]](#field-init) 赋值。

生成 CIL 的工具有责任将源代码中对 **literal** 值的引用替换为其实际值。因此，更改 **literal** 值的字段需要重新编译任何引用该常量的代码。因此，**literal** 值不具有版本弹性。

```csharp
[Serializable]
class Sample
{
    static int StaticField;
    readonly int InitonlyField = 10010;
    const int LiteralField = 10086;
}
```
```cil
.class private serializable Sample
{
	// Fields
	.field private static int32 StaticField
	.field private initonly int32 InitonlyField
	.field private static literal int32 LiteralField = int32(10086)
}
```

#### 14.1.3. 互操作属性
<a id="field-marshal"></a>

存在一个用于与预先存在的本地应用程序互操作的特性；它是特定于平台的，不能用在 CLI 的多个实现上运行的代码中。该特性是 **marshal**，它指定当字段的内容传递给非托管代码时，应将其转换为指定的本地数据类型。每个符合 CLI 的实现都有其默认的封送规则和使用 **marshal** 特性进行指定自动转换的限制。另请参阅 [_数据类型封送_](#data-type-marshaling)。

并非所有 CLI 的实现都需要对用户定义的类型进行封送。它在此标准中指定，以便选择提供它的实现将以一致的方式控制其行为。虽然这不足以保证使用此功能的代码的可移植性，但它确实增加了此类代码可能具有可移植性的可能性。

#### 14.1.4. 特殊处理特性
<a id="field-special-handle-attr"></a>

**rtspecialname** 特性表示运行时应以特殊方式处理字段名称。

目前没有需要用 **rtspecialname** 标记的字段名称。它是为了扩展未来的标准化，以及增加字段和方法声明之间的一致性 (实例和类型初始化方法可以用这个特征标记)。按照惯例，枚举的单个实例字段被命名为 "`value__`" 并用 **rtspecialname** 标记。

**specialname** 特性表示字段名称对于运行时以外的工具有特殊含义，通常是因为它标记了对 CLS 有意义的名称。

```csharp
enum SpecialEnum
{
    None, Item1, Item2
}
```
```cil
.class private SpecialEnum extends [System.Runtime]System.Enum
{
	// Fields
	.field public specialname rtspecialname int32 value__
	.field public static literal valuetype SpecialEnum None = int32(0)
	.field public static literal valuetype SpecialEnum Item1 = int32(1)
	.field public static literal valuetype SpecialEnum Item2 = int32(2)
}
```

>---
### 14.2. 字段初始化元数据
<a id="field-init"></a>

_FieldInit_ 元数据可以选择性地添加到字段声明中。此功能的使用不应与数据标签结合使用。

_FieldInit_ 信息存储在元数据中，可以从元数据中查询此信息。但是，CLI 不使用此信息来自动初始化相应的字段。字段初始化器通常与 **literal** 字段或具有默认值的参数一起使用。参见  [「_Constant: 0x0B_」](#Constant_0x0B)。

下表列出了字段初始化器的选项。尽管类型和字段初始化器都存储在元数据中，但并无要求它们匹配。任何的导入编译器都负责将存储的值强制转换为目标字段类型。下表中的描述列提供了额外的信息。

 | _FieldInit_ ::=                               | 描述                                                                     |
 | :-------------------------------------------- | ------------------------------------------------------------------------ |
 | `bool` `'('` `true` \| `false` `')'`          | 布尔值，编码为真或假                                                     |
 | \| `bytearray` `'('` _Bytes_ `')'`            | 字节字符串，存储时不进行转换。可以用一个零字节进行填充，使总字节数为偶数 |
 | \| `char` `'('` _Int32_ `')'`                 | 16 位无符号整数 (Unicode 字符)                                           |
 | \| `float32` `'('` _Float64_ `')'`            | 32 位浮点数，括号中指定浮点数。                                          |
 | \| `float32` `'('` _Int32_ `')'`              | _Int32_ 是 float 的二进制表示                                            |
 | \| `float64` `'('` _Float64_ `')'`            | 64 位浮点数，括号中指定浮点数。                                          |
 | \| `float64` `'('` _Int64_ `')'`              | _Int64_ 是 double 的二进制表示                                           |
 | \| [ `unsigned` ] `int8` `'('` _Int32_ `')'`  | 8 位整数，括号中指定值。                                                 |
 | \| [ `unsigned` ] `int16` `'('` _Int32_ `')'` | 16 位整数，括号中指定值。                                                |
 | \| [ `unsigned` ] `int32` `'('` _Int32_ `')'` | 32 位整数，括号中指定值。                                                |
 | \| [ `unsigned` ] `int64` `'('` _Int64_ `')'` | 64 位整数，括号中指定值。                                                |
 | \| _QSTRING_                                  | 字符串。_QSTRING_ 存储为 Unicode                                         |
 | \| `nullref`                                  | 空对象引用                                                               |

下面显示了这个的典型用法：

```csharp
const bool Boolen = true;
const char Char = '\0';
const float Float = 3.1415f;
const float Float32Binary = 0b_11111111;
const double Float64 = 1.23456;
const double Float64Binary = 0b_10101010;
const byte UInt8 = 8;
const short Int16 = 16;
const int Int32 = 32;
const ulong UInt64 = 64ul;
const string Str = "World";
const string SNull = null;
```
```cil
// Fields
.field private static literal bool Boolen = bool(true)
.field private static literal char Char = char(0)
.field private static literal float32 Float = float32(3.1415)
.field private static literal float32 Float32Binary = float32(255)
.field private static literal float64 Float64 = float64(1.23456)
.field private static literal float64 Float64Binary = float64(170)
.field private static literal uint8 UInt8 = uint8(8)
.field private static literal int16 Int16 = int16(16)
.field private static literal int32 Int32 = int32(32)
.field private static literal uint64 UInt64 = uint64(64)
.field private static literal string Str = "World"
.field private static literal string SNull = nullref
```

这些 **literal** 字段，运行时不为它们分配内存。工具和编译器可以查找这些值，并检测到它的值和类型信息。

>---
### 14.3. 在 PE 文件中嵌入数据
<a id="data"></a>

有几种方式可以声明存储在 PE 文件中的数据字段。在所有情况下，都使用 **.data** 指令。可以通过在顶层使用 **.data** 指令将数据嵌入到 PE 文件中。

<pre>
    <em>Decl</em> ::= .data <em>DataDecl</em> | ... <a href="#class-extern">[↗]</a>
</pre>

数据也可以作为类型的一部分进行声明：

<pre>
    <em>ClassMember</em> ::= .data <em>DataDecl</em> | ... <a href="#class-type-member">[↗]</a>
</pre>

另一种选择是在方法内部声明数据：

<pre>
    <em>MethodBodyItem</em> ::= .data <em>DataDecl</em> | ... <a href="#MethodBody">[↗]</a>
</pre>

#### 14.3.1. 数据声明

**.data** 指令包含一个可选的数据标签和定义实际数据的主体。如果代码要访问数据，则应使用数据标签。

<pre>
    <em>DataDecl</em> ::= [ <em>DataLabel</em> '=' ] <em>DdBody</em>
</pre>

主体由一个数据项或括号中的数据项列表组成，数据项列表类似于数组。

<pre>
    <em>DdBody</em> ::= <em>DdItem</em> | '{' <em>DdItemList</em> '}'
</pre>

项目列表由任意数量的项目组成：

<pre>
    <em>DdItemList</em> ::= <em>DdItem</em> [ ',' <em>DdItemList</em> ]
</pre>

列表可以用来声明与一个标签关联的多个数据项，数据项将按照声明的顺序布局。第一个数据项可以直接通过标签访问。要访问其他项目时需要使用指针算术，将每个数据项的大小加到列表中的下一个项。使用指针算术将使应用程序无法验证。如果要在之后引用每个数据项，它应该有一个 _DataLabel_ [[↗]](#DataLabel)；如果要在数据项之间插入对齐填充，可以省略 _DataLabel_。 

数据项声明数据的类型，并在括号中提供数据。如果数据项列表包含相同类型和初始值的项，以下语法可以用作某些类型的快捷方式：在声明后的 `[ int32 ]` 括号中放入数据项应复制的次数。

 | _DdItem_ ::=                                                   | 描述                 |
 | :------------------------------------------------------------- | -------------------- |
 | `'&'` `'(` _Id_ `')'`                                          | 标签的地址           |
 | \| `bytearray` `'('` _Bytes_ `')'`                             | 字节数组             |
 | \| `char` `'*'` `'('` _QSTRING_ `')'`                          | (Unicode) 字符数组   |
 | \| `float32` [ `'('` _Float64_ `')'` ] [ `'['` _Int32_ `']'` ] | 可复制的 32 位浮点数 |
 | \| `float64` [ `'('` _Float64_ `')'` ] [ `'['` _Int32_ `']'` ] | 可复制的 64 位浮点数 |
 | \| `int8` [ `'('` _Int32_ `')'` ] [ `'['` _Int32_ `']'` ]      | 可复制的 8 位整数    |
 | \| `int16` [ `'('` _Int32_ `')'` ] [ `'['` _Int32_ `']'` ]     | 可复制的 16 位整数   |
 | \| `int32` [ `'('` _Int32_ `')'` ] [ `'['` _Int32_ `']'` ]     | 可复制的 32 位整数   |
 | \| `int64` [ `'('` _Int64_ `')'` ] [ `'['` _Int32_ `']'` ]     | 可复制的 64 位整数   |

以下声明了一个值为 123 的 32 位有符号整数：

 ```cil
 .data theInt = int32(123)
 ```

以下声明了 10 个值为 3 的 8 位无符号整数的复制：

 ```cil
 .data theBytes = int8 (3) [10]
 ```

#### 14.3.2. 从 PE 文件中访问数据
<a id= "at-field"></a>

使用 **.data** 指令在 PE 文件中存储的数据可以通过在数据的特定位置声明的静态变量 (全局的或类型的成员) 来访问：

<pre>
    <em>FieldDecl</em> ::= <em>FieldAttr</em>* <em>Type</em> <em>Id</em> at <em>DataLabel</em> 
</pre>

然后，程序可以像访问任何其他静态变量一样访问数据，以及使用诸如 `ldsfld`，`ldsflda` 等指令。从 PE 文件内部访问数据的能力可能受到平台特定规则的限制，通常与 PE 文件格式本身的部分访问权限有关。

下面访问了在上节的示例中声明的数据。首先需要为数据声明一个静态变量，例如，全局静态变量：

 ```cil
 .field public static int32 myInt at theInt
 ```

然后可以使用静态变量来加载数据：

 ```cil
 // 数据在堆栈上
 ldsfld int32 myInt
 ```
>---
### 14.4. 非 **literal** 静态数据的初始化

许多支持静态数据的语言都提供了一种在程序开始执行之前初始化该数据的方法。有三种常见的机制可以做到这一点，CLI 都支持这三种机制。

#### 14.4.1. 链接时已知的数据

当在程序链接 (或对于没有链接步骤的语言，在编译) 期间已知要存储到静态数据中的正确值时，实际值可以直接存储到 PE 文件中，通常存储到数据区域 [[↗]](#data)。对变量的引用直接指向将此数据放置在内存中的位置，如果文件在链接器假定的地址之外的地址加载，则使用操作系统提供的修复机制来调整对此区域的任何引用。

在 CLI 中，如果静态变量是基础数值类型之一，或者是具有 *explicit* 显式类型布局且没有在值类型中嵌入任何托管对象的嵌入式引用，那么可以直接使用这种技术。在这种情况下，数据像往常一样布局在数据区域中，静态变量通过使用字段声明中的数据标签 (使用 **at** 语法) 分配一个特定的 RVA (即，从 PE 文件开始的偏移量)。

然而，这种机制不能很好地与 CLI 的应用程序域概念交互。应用程序域旨在通过将同一操作系统进程中运行的两个应用程序隔离开来以确保它们没有共享数据。由于 PE 文件在整个进程中是共享的，因此通过此机制访问的任何数据对进程中的所有应用程序域都是可见的，从而违反了应用程序域隔离边界。

>---
### 14.5. 加载时已知的数据

如果在加载 PE 文件之前不知道正确的值时 (例如，如果它包含基于多个 PE 文件的加载地址计算的值)，可以提供任意代码在 PE 文件加载时运行，但是这种机制是特定于平台的，并且可能无法在所有符合 CLI 的实现中使用。

#### 14.5.1. 运行时已知的数据

当正确的值不能在类型布局计算出来之前确定时，用户应在类型初始化器中提供部分代码用来初始化静态数据。关于类型初始化的保证在 [[↗]](#type-init-guarantees) 中有所描述。如下文所述，全局静态变量在 CLI 中被建模为属于某种类型，因此同样的保证适用于全局静态变量和类型静态变量。

由于在首次引用类型之前不需要进行托管类型的布局，因此无法通过简单地在 PE 文件中布局数据来静态初始化托管类型。相反，有一个类型初始化过程，按照以下步骤进行：
 1. 所有静态变量都被置零。
 2. 如果有的话，调用用户提供的类型初始化过程，如 [_类型初始化器_](#type-initializer) 中所述。

在类型初始化过程中有几种技术：

 * ***生成显式代码*** (_Generate explicit code_)，将常量存储到静态变量的适当字段中。对于小型数据结构，这可能是有效的，但它要求将初始化器转换为本地代码，这可能会导致一些代码空间和执行时间的问题。
 * ***装箱值类型*** (_Box value types_)。当静态变量只是基础数值类型或具有显式布局的值类型的装箱版本时，引入一个已知 RVA 的额外静态变量，该变量保存未装箱的实例，然后简单地使用 **box** 指令创建装箱副本。
 * ***从静态本地数组的数据中创建托管数组*** (_Create a managed array from a static native array of data_)。这可以通过将本地数组封送到托管数组来完成。要使用的特定的封送处理取决于本地数组。例如，它可以是 *safearray*。
 * ***默认初始化值类型的托管数组*** (_Default initialize a managed array of a value type_)。基类库提供了一个方法，该方法将每个未装箱值类型的数组元素的存储空间归零 (`System.Runtime.CompilerServices.InitializeArray`)。

---
## 15. 属性定义
<a id="property"></a>

属性是通过使用 **.property** 指令声明的。属性只能在类型内部声明，不支持全局属性。

<pre>
    <em>ClassMember</em> ::= .property <em>PropHeader</em> '{' <em>PropMember</em>* '}'
</pre>

请参阅 [「_Property: 0x17_」](#Property_0x17) 和 [「_PropertyMap: 0x15_」](#PropertyMap_0x15) 了解如何在元数据中存储属性信息。

<pre>
    <em>PropHeader</em> ::= [ specialname ] [ rtspecialname ] <em>CallConv</em> <em>Type</em> <em>Id</em> '(' <em>Parameters</em> ')'
</pre>

**.property** 指令指定了调用约定 (§[II.15.3](ii.15.3-calling-convention.md)) 、类型、名称和括号中的参数。`specialname` 将属性标记为对其他工具*特殊*，而 `rtspecialname` 将属性标记为对 CLI *特殊*。属性的签名 (即，_PropHeader_ 产生) 应与属性的 **.get** 方法的签名匹配 (见下文) 

目前没有需要用 `rtspecialname` 标记的属性名称。它是为了扩展未来的标准化，以及增加属性和方法声明之间的一致性 (实例和类型初始化方法应该用这个特性标记)。CLI 对构成属性的方法没有限制，但 CLS 规定了一组一致性约束。

一个属性可以在其主体中包含任意数量的方法。下表显示了如何识别这些方法，并提供了每种项目的简短描述：

 | _PropMember_ ::=                                                                          | 描述                                           | 参考                      |
 | :---------------------------------------------------------------------------------------- | ---------------------------------------------- | ------------------------- |
 | \| `.custom` _CustomDecl_                                                                 | 自定义特性。                                   | §[[↗]](#custom)           |
 | \| `.get` _CallConv_ _Type_ [ _TypeSpec_ `'::'` ] _MethodName_ `'('` _Parameters_ `')'`   | 指定属性的 *getter*。                          |                           |
 | \| `.other` _CallConv_ _Type_ [ _TypeSpec_ `'::'` ] _MethodName_ `'('` _Parameters_ `')'` | 指定属性的除 *getter* 或 *setter* 之外的方法。 |                           |
 | \| `.set` _CallConv_ _Type_ [ _TypeSpec_ `'::'` ] _MethodName_ `'('` _Parameters_ `')'`   | 指定属性的 *setter*。                          |                           |
 | \| _ExternSourceDecl_                                                                     | `.line` 或 `#line`                             | §[[↗]](#ExternSourceDecl) |

**.get** 指定了此属性的 *getter*。_TypeSpec_ 默认为当前类型。一个属性只能指定一个 *getter*。为了符合 CLS，*getter* 的定义应被标记为 `specialname`。

**.set** 指定了此属性的 *setter*。_TypeSpec_ 默认为当前类型。一个属性只能指定一个 *setter*。为了符合 CLS，*setter* 的定义应被标记为 `specialname`。

**.other** 用于指定此属性包含的任何其他方法。

此外，还可以指定自定义特性或源行声明。

这显示了属性 `count` 的声明。

```cil
.class public auto autochar MyCount extends [mscorlib]System.Object 
{
    .method virtual hidebysig public specialname instance int32 get_Count() 
    { // getter 的主体 }

    .method virtual hidebysig public specialname instance void set_Count(int32 newCount)
    { // setter 的主体 } 

    .method virtual hidebysig public instance void reset_Count() 
    { // refresh 方法的主体 } 

    // 属性的声明
    .property int32 Count() 
    {
        .get instance int32 MyCount::get_Count()
        .set instance void MyCount::set_Count(int32)
        .other instance void MyCount::reset_Count()
    }
}
```

---
## 16. 事件定义
<a id="event"></a>

事件是在类型内部使用 **.event** 指令声明的；没有全局事件。

<pre>
    <em>ClassMember</em> ::= .event <em>EventHeader</em> '{' <em>EventMember</em>* '}' | ... <a href="#generic-type">[↗]</a>
</pre>

参见 [「_Event: 0x14_」](#Event_0x14) 和 [「_DeclSecurity: 0x0E_」](#DeclSecurity_0x0E) 了解如何在元数据中存储事件信息。

<pre>
    <em>EventHeader</em> ::= [ specialname ] [ rtspecialname ] [ <em>TypeSpec</em> ] <em>Id</em>
</pre>
 
在典型的使用中，_TypeSpec_ (如果存在) 标识了一个委托，其签名与传递给事件的 **.fire** 方法的参数匹配。

事件 *head* 可以包含关键字 **specialname** 或 **rtspecialname**。**specialname** 为其他工具标记属性的名称，而 **rtspecialname** 为运行时标记事件的名称为特殊。

目前没有需要用 **rtspecialname** 标记的事件名称。它是为了扩展未来的标准化，以及增加事件和方法 (实例和类型初始化方法应该用这个特性标记) 声明之间的一致性而提供的。

 | _EventMember_ ::=                                                                            | 描述               | 条款                      |
 | :------------------------------------------------------------------------------------------- | ------------------ | ------------------------- |
 | `.addon` _CallConv_ _Type_ [ _TypeSpec_ `'::'` ] _MethodName_ `'('` _Parameters_ `')'`       | 事件的添加方法。   |
 | \| `.custom` _CustomDecl_                                                                    | 自定义特性。       | §[[↗]](#custom)           |
 | \| `.fire` _CallConv_ _Type_ [ _TypeSpec_ `'::'` ] _MethodName_ `'('` _Parameters_ `')'`     | 事件的触发方法。   |
 | \| `.other` _CallConv_ _Type_ [ _TypeSpec_ `'::'` ] _MethodName_ `'('` _Parameters_ `')'`    | 其他方法。         |
 | \| `.removeon` _CallConv_ _Type_ [ _TypeSpec_ `'::'` ] _MethodName_ `'('` _Parameters_ `')'` | 事件的移除方法。   |
 | \| _ExternSourceDecl_                                                                        | `.line` 或 `#line` | §[[↗]](#ExternSourceDecl) |

**.addon** 指令指定 *add* 方法，_TypeSpec_ 默认为与事件相同的类型。CLS 为事件指定命名约定和一致性约束，并要求 *add* 方法的定义被标记为 **specialname**。

**.removeon** 指令指定 *remove* 方法，_TypeSpec_ 默认为与事件相同的类型。CLS 为事件指定命名约定和一致性约束，并要求 *remove* 方法的定义被标记为 **specialname**。

**.fire** 指令指定 *fire* 方法，_TypeSpec_ 默认为与事件相同的类型。CLS 为事件指定命名约定和一致性约束，并要求 *fire* 方法的定义被标记为 **specialname**。

事件可以包含任意数量的其他方法，这些方法使用 **.other** 指令指定。从 CLI 的角度来看，这些方法只通过事件彼此关联。如果它们有特殊的语义，这需要由实现者来记录。事件也可以有与它们关联的自定义特性，并且也可以声明源行信息。

下面展示了如何声明一个事件，其对应的委托，以及事件的 *add*，*remove* 和 *fire* 方法的典型实现。事件和方法在名为 `Counter` 的类中声明。

```cil
// 委托
.class private sealed auto autochar TimeUpEventHandler extends [mscorlib]System.Delegate 
{
    .method public hidebysig specialname rtspecialname instance void 
        .ctor(object 'object', native int 'method') runtime managed {}

    .method public hidebysig virtual instance void 
        Invoke() runtime managed {}

    .method public hidebysig newslot virtual instance class [mscorlib]System.IAsyncResult 
        BeginInvoke(class [mscorlib]System.AsyncCallback callback, object 'object') runtime managed {}

    .method public hidebysig newslot virtual instance void 
        EndInvoke(class [mscorlib]System.IAsyncResult result) runtime managed {}
}

// 声明事件的类
.class public auto autochar Counter extends [mscorlib]System.Object 
{
    // 存储处理程序的字段，初始化为 null
    .field private class TimeUpEventHandler timeUpEventHandler

    // 事件声明
    .event TimeUpEventHandler startStopEvent 
    {
        .addon instance void Counter::add_TimeUp(class TimeUpEventHandler 'handler')
        .removeon instance void Counter::remove_TimeUp(class TimeUpEventHandler 'handler')
        .fire instance void Counter::fire_TimeUpEvent()
    }

    // add 方法，将处理程序与现有的委托组合
    .method public hidebysig virtual specialname instance void 
        add_TimeUp(class TimeUpEventHandler 'handler')
    {
        .maxstack 4
        
        ldarg.0
        dup
        ldfld class TimeUpEventHandler Counter::TimeUpEventHandler
        ldarg 'handler'
        call class[mscorlib]System.Delegate 
            [mscorlib]System.Delegate::Combine(
                class [mscorlib]System.Delegate, 
                class [mscorlib]System.Delegate)
        castclass TimeUpEventHandler
        stfld class TimeUpEventHandler Counter::timeUpEventHandler
        ret
    }

    // remove 方法，从委托中移除处理程序
    .method virtual public specialname void 
        remove_TimeUp(class TimeUpEventHandler 'handler') 
    {
        .maxstack 4
        
        ldarg.0
        dup
        ldfld class TimeUpEventHandler Counter::timeUpEventHandler
        ldarg 'handler'
        call class[mscorlib]System.Delegate 
            [mscorlib]System.Delegate::Remove(
                class [mscorlib]System.Delegate, 
                class [mscorlib]System.Delegate)
        castclass TimeUpEventHandler
        stfld class TimeUpEventHandler Counter::timeUpEventHandler
        ret
    }

    // fire 方法
    .method virtual family specialname void fire_TimeUpEvent() 
    {
        .maxstack 3
        
        ldarg.0
        ldfld class TimeUpEventHandler Counter::timeUpEventHandler
        callvirt instance void TimeUpEventHandler::Invoke()
        ret
    }
} // Counter 类结束
```

---
## 17. 异常处理
<a id="SEHBlock"></a>

在 CLI 中，方法可以定义一系列被称为 _受保护_ 的 CIL 指令。这被称为 _try block_，它可以将一个或多个处理程序 _handlers_ 与该 _try block_ 关联。如果在 _try block_ 内的任何地方执行过程中发生异常，就会创建一个描述问题的异常对象。然后由 CLI 接管，并将控制权从抛出异常的点转移到愿意处理该异常的代码块。

没有两个处理程序 (**fault**，**filter**，**catch** 或 **finally**) 可以有相同的起始地址。当发生异常时，需要将执行地址转换为发生异常的最正确词法嵌套的 **try** 块。

<pre>
    <em>SEHBlock</em> ::= <em>TryBlock</em> <em>SEHClause</em> [ <em>SEHClause</em>* ]
</pre>

接下来的几个子小节通过描述参与异常处理的五种代码块：**try**，**catch**，**filter**，**finally** 和 **fault**，来扩展这个简单的描述。给定的 _TryBlock_ 可以有多少个以及什么类型的 _SEHClause_ 是有限制的，参见第一部分的 [_异常处理_](./01_CLI%20基本概念和体系结构.md/#exception-handle)。

以下是详细描述的剩余语法项：

> _TryBlock_

 | _TryBlock_ ::=              | 描述                                   |
 | :-------------------------- | -------------------------------------- |
 | `.try` _Label_ `to` _Label_ | 保护从第一个标签到第二个标签之前的区域 |
 | \| `.try` _ScopeBlock_      | _ScopeBlock_ 是受保护的                |

> _SEHClause_

 | _SEHClause_ ::=                        | 描述                             |
 | :------------------------------------- | -------------------------------- |
 | `catch` _TypeReference_ _HandlerBlock_ | 捕获指定类型的所有对象           |
 | \| `fault` _HandlerBlock_              | 处理所有异常，但不处理正常退出   |
 | \| `filter` _Label_ _HandlerBlock_     | 只有在过滤器成功时才进入处理程序 |
 | \| `finally` _HandlerBlock_            | 处理所有异常并正常退出           |

> _HandlerBlock_

 | _HandlerBlock_ ::=             | 描述                                     |
 | :----------------------------- | ---------------------------------------- |
 | `handler` _Label_ `to` _Label_ | 处理程序范围从第一个标签到第二个标签之前 |
 | \| _ScopeBlock_                | _ScopeBlock_ 是处理程序块                |

>---
### 17.1. Protected 块

*try*，或 *protected*，或 *guarded* 块是用 **.try** 指令声明的。

<pre>
    <em>TryBlock</em> ::= .try <em>Label</em> to <em>Label</em>
</pre>

在第一种情况下，受保护的块由两个标签分隔。第一个标签是要受保护的第一条指令，而第二个标签是要受保护的最后一条指令之后的指令。这两个标签都应在此点之前定义。

<pre>
    <em>TryBlock</em> ::= .try <em>ScopeBlock</em>
</pre>

第二种情况在 **.try** 指令后使用范围块 [[↗]](#ScopeBlock) —— 该范围内的指令是要受保护的指令。

>---
### 17.2. Handler 块

<pre>
    <em>TryBlock</em> ::= handler <em>Label</em> to <em>Label</em>
</pre>

在第一种情况下，标签包围了处理程序块的指令，第一个标签是处理程序的第一条指令，而第二个标签是处理程序后面的指令。

<pre>
    <em>HandlerBlock</em> ::= <em>ScopeBlock</em>
</pre>

在第二种情况下，处理程序块只是一个作用域块。

>---
### 17.3. Catch 块

使用 **catch** 关键字声明 _catch_ 块。这指定了该子句设计用来处理的异常对象的类型，以及处理程序代码本身。

<pre>
    <em>SEHClause</em> ::= catch <em>TypeReference</em> <em>HandlerBlock</em>
</pre>

```cil
.try 
{
    ...                          // 受保护的指令
    leave exitSEH                // 正常退出
}
catch [mscorlib]System.FormatException 
{
    ...                          // 处理异常
    pop                          // 弹出异常对象
    leave exitSEH                // 离开 catch 处理程序
}
exitSEH:                         // 在此处继续
```

>---
### 17.4. Filter 块

使用 **filter** 关键字声明 _filter_ 块。

<pre>
    <em>SEHClause</em> ::= ... | filter <em>Label</em> <em>HandlerBlock</em> 
    <em>SEHClause</em> ::= ... | filter <em>Scope</em> <em>HandlerBlock</em> 
</pre>

过滤代码从指定的标签开始，结束于处理程序块的第一条指令。CLI 要求过滤块在 CIL 流中必须紧接其对应的处理程序块。 

```cil
.method public static void M() 
{
    .try {
        …              // 受保护的指令
        leave exitSEH  // 正常退出
    }
    filter {
        …              // 决定是否处理
        pop            // 弹出异常对象
        ldc.i4.1       // EXCEPTION_EXECUTE_HANDLER
        endfilter      // 向 CLI 返回答案
    }
    {
        …              // 处理异常
        pop            // 弹出异常对象
        leave exitSEH  // 离开过滤处理程序
    }
exitSEH:
    …
}
```

>---
### 17.5. Finally 块

使用 **finally** 关键字声明 *finally* 块。这指定了处理程序代码。

<pre>
    <em>SEHClause</em> ::= ... | finally <em>HandlerBlock</em> 
</pre>

在 **finally** 处理程序中可以执行的最后一个可能的 CIL 指令应该是 `endfinally`。

```cil
.try
{
    …              // 受保护的指令
    leave exitTry  // 应使用 leave
}
finally 
{
    …              // finally 处理程序
    endfinally
}
exitTry:           // 回到正常
```

>---
### 17.6. Fault 处理程序

使用 **fault** 关键字声明一个 *fault* 块。这指定了处理程序代码，具有以下语法：

<pre>
    <em>SEHClause</em> ::= ... | fault <em>HandlerBlock</em> 
</pre>

在 **fault** 处理程序中可以执行的最后一个可能的 CIL 指令应该是 `endfault`。

```cil
.method public static void M() 
{
startTry:
    …                        // 受保护的指令
    leave exitSEH            // 应使用 leave
endTry:

startFault:
    …                        // fault 处理程序指令
endfault

endFault:
    .try startTry to endTry fault handler startFault to endFault
exitSEH:                     // 回到正常
}
```

---
## 18. 声明性安全
<a id="SecurityDecl"></a>

许多针对 CLI 的语言使用特性语法将声明性安全特性附加到元数据中的项。这些信息实际上是由编译器转换为基于 XML 的表示形式，并存储在元数据中，参见 [「_DeclSecurity: 0x0E_」](#DeclSecurity_0x0E)。相比之下，*ilasm* 要求在其输入中表示转换信息。

<pre>
    <em>SecurityDecl</em> ::= .permissionset <em>SecAction</em> = '(' <em>Bytes</em> ')'
    <em>SecurityDecl</em> ::= .permission <em>SecAction</em> <em>TypeReference</em> '(' <em>NameValPairs</em> ')'

    <em>NameValPairs</em> ::= <em>NameValPair</em> [ ',' <em>NameValPair</em> ]
    <em>NameValPair</em> ::= <em>SQSTRING</em> `=` <em>SQSTRING</em>
</pre>

在 **.permission** 中，_TypeReference_ 指定了权限类，*NameValPair*s 指定了设置。参见 [「_DeclSecurity: 0x0E_」](#DeclSecurity_0x0E)。

在 **.permissionset** 中，字节指定了安全设置的编码版本：

 | _SecAction_ ::=   | 描述                           |
 | :---------------- | ------------------------------ |
 | `assert`          | 断言权限，以便调用方不需要它。 |
 | \| `demand`       | 要求所有调用方的权限。         |
 | \| `deny`         | 拒绝权限，以便检查失败。       |
 | \| `inheritcheck` | 要求派生类的权限。             |
 | \| `linkcheck`    | 要求调用方的权限。             |
 | \| `permitonly`   | 减少权限，以便检查失败。       |
 | \| `reqopt`       | 请求可选的额外权限。           |
 | \| `reqrefuse`    | 拒绝被授予这些权限。           |

---
## 19. 自定义特性
<a id= "custom"></a>

自定义特性向元数据添加用户定义的注解。自定义特性允许将类型的实例与元数据的任何元素一起存储。这种机制可以用于在编译时存储特定于应用程序的信息，并在运行时或当其他工具读取元数据时访问它。虽然任何用户定义的类型都可以用作特性，但是 CLS 遵从性要求特性是基类为 `System.Attribute` 类型的实例。CLI 预定义了一些特性类型并使用它们来控制运行时的行为。一些语言预定义特性类型来表示 CTS 中没有直接表示的语言特性。用户或其他工具可以自定义和使用额外的特性类型。

自定义特性是使用 **.custom** 指令声明的，后面跟着类型构造函数的方法声明，然后是可选的括号中的 _Bytes_：

<pre>
    <em>CustomDecl</em> ::= <em>Ctor</em> [ '=' '(' <em>Bytes</em> ')' ]
</pre>

_Ctor_ 项表示方法声明，特定于方法名为 `.ctor` 的情况。

```cil
custom instance void myAttribute::.ctor(bool, bool) = ( 01 00 00 01 00 00 )
```

自定义特性可以附加到元数据中的任何项，除了自定义特性本身。自定义特性可以附加到程序集、模块、类、接口、值类型、方法、字段、属性、泛型参数和事件 (自定义特性附加到紧接前面的声明上)。 

如果构造函数不带参数，则不需要 _Bytes_ 项。在这种情况下，重要的只是自定义特性的存在。如果构造函数带有参数，它们的值应在 _Bytes_ 项中指定。

下面显示了一个类，它被标记了一个名为 `System.CLSCompliantAttribute` 的特性，以及包含一个被标记名为 `System.ObsoleteAttribute` 特性的方法。

```cil
class public MyClass extends [mscorlib]System.Object
{ 
    .custom instance void [mscorlib]System.CLSCompliantAttribute::.ctor(bool) =
        ( 01 00 01 00 00 )
    
    .method public static void CalculateTotals() cil managed
    { 
        .custom instance void [mscorlib]System.ObsoleteAttribute::.ctor() = ( 01 00 00 00 )
        ret
    }
}
```

>---
### 19.1. CLS 约定：自定义特性的使用

CLS 对自定义特性的使用施加了某些约定，以便改进跨语言操作。

>---
### 19.2. CLI 使用的特性

有两种类型的自定义特性，称为 _非伪自定义特性_ 和 _伪自定义特性_。在定义时，自定义特性和伪自定义特性被不同地处理，如下所示：

 * 自定义特性直接存储到元数据中；保存其定义数据的 *二进制对象* (_blob_) 按原样存储。稍后可以检索该 *二进制对象*。
 * 伪自定义特性被识别是因为其名称是短列表中的一个。而不是直接将其 *二进制对象* 存储在元数据中，而是解析该 *二进制对象*，并使用其中包含的信息来设置元数据表中的位和 / 或字段。然后丢弃该 *二进制对象*；稍后则无法被检索。

因此，伪自定义特性用于捕获用户指令，使用编译器为非伪自定义特性提供的相同语法，但这些用户指令然后被存储到元数据表的更加高效的空间形式中。在运行时检查表的速度比检查非伪自定义特性速度快。

许多自定义特性是由更高层的软件发明的。它们被 CLI 存储并返回，而 CLI 不知道也不关心它们的 “含义”。但是所有伪自定义特性，加上一组非伪自定义特性，对编译器和 CLI 都特别感兴趣。这样自定义特性的一个例子是 `System.Reflection.DefaultMemberAttribute`。它作为一个非伪自定义特性的 *二进制对象* 被存储在元数据中，但是反射在调用类型的默认成员 (特性) 时，使用这个自定义特性。

以下的子小节列出了所有的伪自定义特性和 _显著_ 的自定义特性，其中 _显著_ 意味着 CLI 和 / 或编译器直接关注它们，并以某种方式影响它们的行为。为了防止将来的名称冲突，`System` 命名空间中的所有自定义特性都保留用于标准化。

#### 19.2.1. 伪自定义特性
<a id="pseudo-custom-attr"></a>

下表列出了 CLI 的伪自定义特性。 并非所有这些特性都在此标准中指定，但所有的名称都是保留的，不得用于其他目的。它们在 `System.Reflection`，`System.Runtime.CompilerServices` 和 `System.Runtime.InteropServices` 命名空间中定义。

 | 特性                           | 描述                                                 |
 | ------------------------------ | ---------------------------------------------------- |
 | `AssemblyAlgorithmIDAttribute` | 记录使用的哈希算法的 ID (仅保留)                     |
 | `AssemblyFlagsAttribute`       | 记录此程序集的标志 (仅保留)                          |
 | `DllImportAttribute`           | 提供关于在非托管库中实现的代码的信息                 |
 | `FieldOffsetAttribute`         | 指定字段在其封闭类或值类型中的字节偏移量             |
 | `InAttribute`                  | 表示方法参数是 `[in]` 参数                           |
 | `MarshalAsAttribute`           | 指定数据项在托管代码和非托管代码之间如何进行封送     |
 | `MethodImplAttribute`          | 指定方法实现的详细信息                               |
 | `OutAttribute`                 | 表示方法参数是 `[out]` 参数                          |
 | `StructLayoutAttribute`        | 允许调用者控制类或值类型的字段在托管内存中的布局方式 |

这些特性影响元数据中的位和字段，如下所示：

 * `AssemblyAlgorithmIDAttribute`：设置 _Assembly_._HashAlgId_ 字段。
 * `AssemblyFlagsAttribute`：设置 _Assembly_._Flags_ 字段。
 * `DllImportAttribute`：为带有特性的方法设置 _Method_._Flags_._PinvokeImpl_ 位；还在 _ImplMap_ 表中添加新行 (设置 _MappingFlags_，_MemberForwarded_，_ImportName_ 和 _ImportScope_ 列)。
 * `FieldOffsetAttribute`：为带有特性的字段设置 _FieldLayout_._Offset_ 值。
 * `InAttribute`：为带有特性的参数设置 _Param_._Flags_._In_ 位。
 * `MarshalAsAttribute`：为带有特性的字段设置 _Field_._Flags_._HasFieldMarshal_ 位 (或为带有特性的参数设置 _Param_._Flags_._HasFieldMarshal_ 位)；还在 _FieldMarshal_ 表中为 _Parent_ 和 _NativeType_ 列输入新行。
 * `MethodImplAttribute`：设置带有特性的方法的 _Method_._ImplFlags_ 字段。
 * `OutAttribute`：为带有特性的参数设置 _Param_._Flags_._Out_ 位。
 * `StructLayoutAttribute`：为带有特性的类型设置 _TypeDef_._Flags_._LayoutMask_ 子字段，以及可选的 _TypeDef_._Flags_._StringFormatMask_ 子字段，_ClassLayout_._PackingSize_ 和 _ClassLayout_._ClassSize_ 字段。

#### 19.2.2. CLS 定义的自定义特性

CLS 指定了某些自定义特性，并要求符合规范的语言支持它们。这些特性位于 `System` 下。

 | 特性                      | 描述                                                         |
 | ------------------------- | ------------------------------------------------------------ |
 | `AttributeUsageAttribute` | 用于指定一个特性类的预期用途。                               |
 | `ObsoleteAttribute`       | 表示一个元素不应被使用。                                     |
 | `CLSCompliantAttribute`   | 通过特性对象上的实例字段，指示一个元素是否被声明为符合 CLS。 |

#### 19.2.3. 安全性的自定义特性

以下自定义特性在 `System.Net` 和 `System.Security.Permissions` 命名空间中定义。这些都是基类，在程序集中找到的安全特性的实例将是这些类的子类。

 | 特性                             | 描述                                                                     |
 | -------------------------------- | ------------------------------------------------------------------------ |
 | `CodeAccessSecurityAttribute`    | 这是使用自定义特性进行声明式安全的基本特性类。                           |
 | `DnsPermissionAttribute`         | 用于 `DnsPermission` 的声明式安全的自定义特性类                          |
 | `EnvironmentPermissionAttribute` | 用于 `EnvironmentPermission` 的声明式安全的自定义特性类。                |
 | `FileIOPermissionAttribute`      | 用于 `FileIOPermission` 的声明式安全的自定义特性类。                     |
 | `ReflectionPermissionAttribute`  | 用于 `ReflectionPermission` 的声明式安全的自定义特性类。                 |
 | `SecurityAttribute`              | 这是声明式安全的基本特性类，`CodeAccessSecurityAttribute` 是从它派生的。 |
 | `SecurityPermissionAttribute`    | 指示被特性化的方法是否可以影响安全设置                                   |
 | `SocketPermissionAttribute`      | 用于 `SocketPermission` 的声明式安全的自定义特性类。                     |
 | `WebPermissionAttribute`         | 用于 `WebPermission` 的声明式安全的自定义特性类。                        |

任何其他与安全性相关的自定义特性包含在程序集中 (即，任何从 `System.Security.Permissions.SecurityAttribute` 派生的自定义特性)，都可能导致符合 CLI 的实现在加载时拒绝此类程序集，或者如果尝试访问这些与安全性相关的自定义特性，在运行时抛出异常。这个声明对于任何无法解析的自定义特性都是成立的；与安全性相关的自定义特性只是一个特殊的情况。 

#### 19.2.4. 用于 TLS 的自定义特性

在 `System` 命名空间中定义了一个表示 TLS (线程局部存储) 字段的自定义特性。

 | 特性                    | 描述                           |
 | ----------------------- | ------------------------------ |
 | `ThreadStaticAttribute` | 提供相对于线程的类型成员字段。 |

#### 19.2.5. 其他的自定义特性

以下自定义特性控制 CLI 的各个方面：

 | 特性                              | 命名空间                          | 描述                                                                     |
 | --------------------------------- | --------------------------------- | ------------------------------------------------------------------------ |
 | `ConditionalAttribute`            | `System.Diagnostics`              | 用于标记方法为可调用，基于某些编译时条件。如果条件为假，方法将不会被调用 |
 | `DecimalConstantAttribute`        | `System.Runtime.CompilerServices` | 在元数据中存储十进制常数的值                                             |
 | `DefaultMemberAttribute`          | `System.Reflection`               | 定义了一个类型的成员，该成员是反射的 `InvokeMember` 使用的默认成员。     |
 | `CompilationRelaxationsAttribute` | `System.Runtime.CompilerServices` | 指示来自指令检查的异常是严格的还是宽松的。                               |
 | `FlagsAttribute`                  | `System`                          | 表示枚举应被视为位字段；也就是说，一组标志                               |
 | `IndexerNameAttribute`            | `System.Runtime.CompilerServices` | 在不直接支持属性的编程语言中，指示具有一个或多个参数的属性的名称         |
 | `ParamArrayAttribute`             | `System`                          | 表示该方法将允许在其调用中使用可变数量的参数                             |

---




## 24. end

---
[[↗]](#)
[「__」](#)

<pre>
    <em>GenArgs</em> ::= <em>Type</em> [ ',' <em>Type</em> ]*
</pre>

> *table* 示例，以下示例中的索引值被替换成对应堆中的值

| *Token*  | *Generation* | *Name*        | *Mvid*   | *EncId*  | *EncBaseId* |
| :------- | :----------- | :------------ | :------- | :------- | :---------- |