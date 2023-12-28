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

 * **安全特性被特殊处理**。在 ILAsm 中有特殊的语法，允许直接描述表示安全特性的 XML。虽然所有其它在标准库或用户提供的扩展中定义的特性都使用一个公共机制在元数据中编码，但安全特性 (直接或间接继承自 `System.Security.Permissions.SecurityAttribute`) 应按照 [「DeclSecurity: 0x0E」](#DeclSecurity_0x0E) 中的描述进行编码。

>---
### 3.10. ILAsm 源文件

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
### 4.1. assembly：定义一个程序集
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

程序集引用可以存储完整的公钥或 8 字节的公钥标志 *Public Key Token*。两个都可以用来验证在编译时或运行时为程序集签名的同一私钥。两者并不需要同时存在，虽然两者都可以存储。

符合 CLI 规范的实现不需要执行这个验证，但它可以这样做，且它可以拒绝加载任何验证失败的程序集。符合 CLI 规范的实现也可以拒绝允许访问一个程序集，除非程序集引用包含公钥或公钥标志。无论是使用公钥还是公钥标志，符合 CLI 规范的实现都应该做出相同的访问决策。

存储在程序集引用中的公钥或公钥标志用于确保被引用的程序集和实际在运行时使用的程序集都是由拥有同一私钥的实体产生的，因此可以假定它们是为了相同的目的。虽然完整的公钥在密码学上更安全，但它在引用中需要更多的存储空间。使用公钥标志可以减少存储引用所需的空间，同时只稍微削弱了验证过程。

为了验证程序集的内容自创建以来没有被篡改，应使用的是程序集自身标识中的完整公钥，而不是存储在对程序集的引用中的公钥或公钥标志。

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

 * 用户定义的类型的逻辑描述，这些类型被引用，但通常不在当前模块中定义。这些信息存储在元数据的一个表中 ([「TypeRef: 0x01」](#TypeRef_0x01))。
 * 对一个或多个类型引用以及各种修饰符进行编码的签名。非终结符 **Type** 中对签名进行了描述。

### 5.1. *Type*：类型
<a id= "Type"></a>

以下语法完全指定了 CLI 系统的所有内置类型 (包括指针类型)。它还显示了可以在 CLI 系统中定义的用户定义类型的语法：

 | *Type* ::=                                                   | 描述                                                                                                          | 参考                                |
 | ------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------- | ----------------------------------- |
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
 | \| `valuetype` _TypeReference_                               | (未装箱的) 用户定义的值类型                                                                                   | §[[↗]](#valuetype)                  |
 | \| `unsigned int8`                                           | 无符号 8 位整数                                                                                               | §[[↗]](#build-in)                   |
 | \| `unsigned int16`                                          | 无符号 16 位整数                                                                                              | §[[↗]](#build-in)                   |
 | \| `unsigned int32`                                          | 无符号 32 位整数                                                                                              | §[[↗]](#build-in)                   |
 | \| `unsigned int64`                                          | 无符号 64 位整数                                                                                              | §[[↗]](#build-in)                   |
 | \| `void`                                                    | 无类型。只允许作为返回类型或作为 `void *` 的一部分                                                            | §[[↗]](#build-in)                   |

在几种情况下，语法允许使用稍微简单一些的表示法来指定类型；例如，"`System.GC`" 可以代替 "`class System.GC`"。这样的表示法被称为 **类型规范** (_type specifications_)：

 | _TypeSpec_ ::=                         | 参考                   |
 | -------------------------------------- | ---------------------- |
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

可访问性直接在元数据中编码，参见 [「MethodDef: 0x06」](#MethodDef_0x06)。

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

泛型方法可以是静态的、实例的或虚拟的。类的静态或实例构造函数 (分别为 `.cctor` 或 `.ctor`) 不能是泛型的。

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
 * 对静态成员、实例构造函数或类型自己的泛型参数约束没有限制。

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

---
### 7.8. 签名和绑定

泛型类型的成员 (字段和方法) 在 CIL 指令中使用元数据标志引用，该标志指定了 _MemberRef_ 中的一个项。抽象地说，引用由两部分组成：
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

在重写方法的主体中，只有直接在其签名中指定的约束才适用。当一个方法被调用时，将执行与 `call` 或 `callvirt` 指令中的元数据标志关联的约束。

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

在泛型类或泛型方法上声明的泛型参数可以被一个或多个类型 (参考 [「GenericParamConstraint: 0x2C」](#GenericParamConstraint_0x2C))，和一个或多个 [*特殊约束*](#special-genpars) **约束** (_constrained_)。泛型参数只能使用满足所有指定特殊约束并且是 *可赋值给* (当装箱时) 每个声明的约束的泛型参数实例化。

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
## 8. class：类型定义 
<a id="class"></a>

类型 (即，类、值类型和接口) 可以在模块的顶级定义：

<pre>
    <em>Decl</em> ::= .class <em>ClassHeader</em> '{' <em>ClassMember</em>* '}' | ...
</pre>

此声明创建的逻辑元数据表在 [「TypeDef: 0x02」](#TypeDef_0x02) 中指定。

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

在 **implements** 关键字后的 _TypeSpec_ 的从左到右的顺序在 [「InterfaceImpl: 0x09」](#InterfaceImpl_0x09) 表中被保留为从上到下的顺序。这是为了支持接口调度中的 [差异解析](#internal-virtual) 时所必需的。

下面这段代码声明了类 `CounterTextBox`，它扩展了程序集 `System.Windows.Forms` 中的类 `System.Windows.Forms.TextBox`，并实现了当前程序集的模块 `Counter` 中的接口 `CountDisplay`。特性 **private**、**auto** 和 **autochar** 在后面的子小节中有描述。

```cil
.class private auto autochar CounterTextBox
    extends [System.Windows.Forms]System.Windows.Forms.TextBox
    implements [.module Counter]CountDisplay
{ // 类的主体 }
```

类型可以附加任意数量的自定义特性。自定义特性的附加方式如 [**custom**](#custom) 节所述。类型的其他 (预定义) 特性可以分为指定可见性、类型布局信息、类型语义信息、继承规则、互操作信息和特殊处理信息的特性。以下各小节对每组预定义特性提供了更多信息。

 | _ClassAttr_ ::=         | 描述                                                 | 参考                                   |
 | ----------------------- | ---------------------------------------------------- | -------------------------------------- |
 | `abstract`              | 类型是抽象的。                                       | §[[↗]](#abstract)                      |
 | \| `ansi`               | 将字符串作为 ANSI 编组到平台。                       | §[[↗]](#ansi)                          |
 | \| `auto`               | 字段的布局由 CLI 自动提供。                          | §[[↗]](#auto)                          |
 | \| `autochar`           | 将字符串作为 ANSI 或 Unicode (平台特定) 编组到平台。 | §[[↗]](#autochar)                      |
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
 | \| `unicode`            | 将字符串作为 Unicode 编组到平台。                    | §[[↗]](#unicode)                       |

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
- **sequential**<a id="sequential"></a>：CLI 应该根据逻辑元数据表 ([「Field: 0x04」"](#Field_0x04)) 中字段的顺序来排列字段。

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

除了这三个特性，*类型标志* [_TypeAttributes_](#TypeAttributes) 还指定了一组额外的位模式 (`CustomFormatClass` 和 `CustomStringFormatMask`)，它们没有标准化的含义。如果这些位被设置，但是语言实现没有提供对它们的支持，将抛出 `System.NotSupportedException` 异常。

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

一个类型可以包含任意数量的进一步声明。指令 **.event**，**.field**，**.method** 和 **.property** 用于声明类型的成员。类型声明中的 **.class** 指令用于创建 [嵌套类型](#nested-types)。

 | _ClassMember_ ::=                                                                                                              | 描述                                                           | 参考      |
 | :------------------------------------------------------------------------------------------------------------------------------ | -------------------------------------------------------------- | --------- |
 | `.class` _ClassHeader_ `'{'` _ClassMember_* `'}'`                                                                              | 定义一个嵌套类型。                                             | §[[↗]](#nested-types) |
 | \| `.custom` _CustomDecl_                                                                                                      | 自定义特性。                                                   | §[[↗]](#custom) |
 | \| `.data` _DataDecl_                                                                                                          | 定义与类型关联的静态数据。                                     | §[[↗]](#data) |
 | \| `.event` _EventHeader_ `'{'` _EventMember_* `'}'`                                                                           | 声明一个事件。                                                 | §[[↗]](#event) |
 | \| `.field` _FieldDecl_                                                                                                        | 声明属于类型的字段。                                           | §[[↗]](#field) |
 | \| `.method` _MethodHeader_ `'{'` _MethodBodyItem_* `'}'`                                                                      | 声明类型的方法。                                               | §[[↗]](#method) |
 | \| `.override` _TypeSpec_ `'::'` _MethodName_ `with` _CallConv_ _Type_ _TypeSpec_ `'::'` _MethodName_ `'('` _Parameters_ `')'` | 指定第一个方法被第二个方法的定义覆盖。                         | §[[↗]](#override) |
 | \| `.pack` _Int32_                                                                                                             | 用于字段的显式布局。                                           | §[[↗]](#pack) |
 | \| `.param type` `'['` _Int32_ `']'`                                                                                           | 为泛型类型指定一个类型参数；用于将自定义特性与该类型参数关联。 | §[[↗]](#param-type) |
 | \| `.property` _PropHeader_ `'{'` _PropMember_* `'}'`                                                                          | 声明类型的属性。                                               | §[[↗]](#property) |
 | \| `.size` _Int32_                                                                                                             | 用于字段的显式布局。                                           | §[[↗]](#size) |
 | \| _ExternSourceDecl_                                                                                                          | 源代码行信息。                                                 | §[[↗]](#ExternSourceDecl) |
 | \| _SecurityDecl_                                                                                                              | 声明性安全权限。                                               | §[[↗]](#SecurityDecl) |

在 **.class** 类型声明 [[↗]](#class) 中，**.method** 定义的自上而下的顺序在 [「MethodDef: 0x06」](#MethodDef_0x06) 表中保留。这是支持接口调度中的差异解析 [[↗]](#internal-virtual) 所必需的。

>---
### 8.3. 引入和重写虚方法

通过提供方法的直接实现 (使用 [方法定义](#MethodHeader)) 并且不指定它为 **newslot** [[↗]](#newslot)，可以重写基类型的虚方法。也可以使用 **.override** [[↗]](#override) 使用现有的方法体来实现给定的虚声明。

#### 8.3.1. 引入虚方法

通过定义虚方法 [[↗]](#MethodHeader) 在继承层次中引入虚方法。定义可以标记为 **newslot**，以始终为定义类及其派生类创建新的虚方法：
 * 如果定义被标记为 **newslot**，则定义始终创建新的虚方法，即使基类提供了匹配的虚方法。通过包含方法定义的类或通过从该类派生的类对虚方法的引用，都指向新的定义 (除非在派生类中被 **newslot** 定义隐藏)。任何不通过包含方法定义的类，也不通过其派生类对虚方法的引用，都指向原始定义。
 * 如果定义没有被标记为 **newslot**，则定义只有在没有从基类继承相同名称和签名的虚方法时才创建新的虚方法。因此，当虚方法被标记为 **newslot** 时，其引入不会影响其基类中匹配虚方法的任何现有引用。

#### 8.3.2. .override 指令
<a id="override"></a>

**.override** 指令指定在此类型中，一个虚方法应由具有相同签名但名称不同的虚方法实现 (覆盖)。此指令可用于为从基类继承的虚方法或此类型实现的接口中指定的虚方法提供实现。**.override** 指令在元数据中指定了 **方法实现** (_Method Implementation_，[_MethodImpl_](#MethodImpl))。

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

如果指定了 **strict** 标志 [[↗]](#MethodAttributes)，则只有可访问的虚方法可以被重写。

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
 * 它已指定将实现的接口中声明的方法的实现，或者其基类型已指定将实现的方法的实现
 * 从其基类继承的虚方法的替代实现
 * 从未提供实现的抽象基类型继承的虚方法的实现

一个具体 (即，非抽象) 类型应通过直接或通过继承实现
 * 由类型本身声明的所有方法
 * 该类型实现的接口的所有虚方法
 * 该类型从其基类型继承的所有虚方法

>---
### 8.5. 特殊成员

有三种特殊成员，它们都是可以作为类型的一部分定义的方法：实例构造器、实例终结器和类型初始化器。

#### 8.5.1. 实例构造函数

**实例构造函数** (_instance constructor_) 初始化一个类型的实例，并在通过 `newobj` 指令创建一个类型的实例时被调用【】。实例构造函数应该是一个实例方法 (不是静态或虚方法)，它应该被命名为 `.ctor`，并被标记为 **instance**、**rtspecialname** 和 **specialname**【】。实例构造函数可以有参数，但不应返回值。实例构造函数不能接受泛型类型参数。实例构造函数可以被重载 (即，一个类型可以有多个实例构造函数)。一个类型的每个实例构造函数都应该有一个唯一的签名。与其他方法不同，实例构造函数可以写入被标记为 **initonly** 特性的类型的字段【】。

下面显示了一个不带任何参数的实例构造函数的定义：

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

终结器的行为在【】中有规定。对于特定类型的 *finalize* 方法，是通过在 `System.Object` 中重写虚方法 `Finalize` 来指定的。

#### 8.5.3. 类型初始化器
<a id="type-initializer"></a>

一个类型 (类，接口，或值类型) 可以包含一个特殊的方法叫做 **类型初始化器** (_type initializer_)，用于初始化类型本身。这个方法应该是静态的，不接受参数，无返回值，被标记为 **rtspecialname** 和 **specialname** 【】，并且被命名为 `.cctor`。

就像实例构造函数一样，类型初始化器可以写入其类型的被 **initonly** 特性标记的静态字段【】。

下面展示了一个类型初始化器的定义：

```cil
.class public EngineeringData extends [mscorlib]System.Object
{
    .field private static initonly float64[] coefficient
    .method private specialname rtspecialname static void .cctor() cil managed
    {
        .maxstack 1 

        // 分配一个包含4个Double的数组
        ldc.i4.4
        newarr     [mscorlib]System.Double
        // 将 initonly 字段指向新数组
        stsfld     float64[] EngineeringData::coefficient
        // 初始化数组元素的代码在这里
        ret
    }
}
```

类型初始化器通常是简单的方法，从存储的常量或通过简单的计算初始化类型的静态字段。然而，对于类型初始化器中允许的代码没有限制。

##### 8.5.3.1. 类型初始化保证

CLI 将提供以下关于类型初始化的保证【】 和 【】：
 1. 类型初始化器何时被执行在 【】 中有规定。
 3. 对于任何给定的类型，类型初始化器应该只执行一次，除非被用户代码明确调用。
 4. 在类型初始化器完成执行之前，除了那些直接或间接从类型初始化器调用的方法外，没有其他方法能够访问类型的成员。

##### 8.5.3.2. 宽松的保证
<a id=beforefieldinit_info></a>

可以使用特性 **beforefieldinit**【】标记一个类型，以表示在 【】 中指定的保证不是必需的。特别是，不需要提供上述最后的要求：在调用或引用静态方法之前，不需要执行类型初始化器。

当代码可以在多个应用程序域中执行时，确保这个最后的保证变得特别昂贵。同时，对大量托管代码的检查表明，这个最后的保证很少需要，因为类型初始化器几乎总是用于初始化静态字段的简单方法。因此，让 CIL 生成器 (因此，可能是程序员) 决定是否需要这个保证，可以在需要时提供效率，但要付出一致性保证的代价。

##### 8.5.3.3. 竞争和死锁

除了在【】中指定的类型初始化保证外，CLI 还应确保从类型初始化器调用的代码有两个进一步的保证：
 1. 类型的静态变量在任何访问之前都处于已知状态。
 2. 仅类型初始化本身不会创建死锁，除非从类型初始化器 (直接或间接) 调用的某些代码明确调用阻塞操作。

考虑以下两个类定义：

```cil
.class public A extends [mscorlib]System.Object
{ 
    .field static public class A a
    .field static public class B b
    .method public static rtspecialname specialname void .cctor ()
    { ldnull   // b=null
        stsfld class B A::b
        ldsfld class A B::a // a=B.a
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
        ldnull   // a=null
        stsfld class A B::a
        ldsfld class B A::b // b=A.b
        stsfld class B B::b
        ret
    }
}
```

加载这两个类后，尝试引用任何静态字段都会导致问题，因为 `A` 和 `B` 的类型初始化器都要求先调用另一个的类型初始化器。要求在其初始化完成之前不允许访问类型将创建死锁情况。相反，CLI 提供了一个较弱的保证：初始化器将开始运行，但不必完成。但是，仅此一点就会使类型的完全未初始化状态可见，这将使得保证可重复结果变得困难。

当类型初始化在多线程系统中进行时，存在类似但更复杂的问题。在这些情况下，例如，两个单独的线程可能开始尝试访问不同类型 (`A` 和 `B`) 的静态变量，然后每个线程都必须等待另一个完成初始化。

以下是一个粗略的算法概述，以确保上述第 1 点和第 2 点：
- 在类加载时 (因此在初始化时间之前) 将零或 `null` 存储到类型的所有静态字段中。
- 如果类型已初始化，表示完成。
  - i. 如果类型尚未初始化，尝试获取初始化锁。
  - ii. 如果成功，记录此线程负责初始化类型并继续执行步骤 iii。
      - 如果不成功，看看这个线程或任何等待这个线程完成的线程是否已经持有锁。
      - 如果是，则返回，因为阻塞将创建死锁。这个线程现在将看到类型的不完全初始化状态，但不会出现死锁。
      - 如果不是，阻塞直到类型初始化然后返回。
  - iii. 初始化基类类型，然后初始化此类型实现的所有接口。
  - iv. 执行此类型的类型初始化代码。
  - v. 将类型标记为已初始化，释放初始化锁，唤醒任何等待此类型初始化的线程，然后返回。

>---
### 8.6. 嵌套类型
<a id="nested-types"></a>

嵌套类型在 【】 中有规定。关于与嵌套类型相关的逻辑表的信息，请参见【】。

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

CLI 支持顺序和显式布局控制，参见 【】。对于显式布局，还需要指定实例的精确布局；另请参见 【】 和 【】。

<pre>
    <em>FieldDecl</em> ::= [ '[' <em>Int32</em> ']' ] <em>FieldAttr</em>* <em>Type</em> <em>Id</em>
</pre>

声明开始处的方括号中指定的可选 `int32` 指定了从类型实例的开始的字节偏移量。 (对于给定类型 _t_，这个开始指的是在类型 _t_ 中明确定义的成员集的开始，排除了所有在类型 _t_ 直接或间接继承的任何类型中定义的成员。) 这种形式的显式布局控制不应与使用 **at** 符号指定的全局字段一起使用【】。

偏移值应为非负数。可以以这种方式重叠字段，尽管对象引用占用的偏移量不应与内置值类型占用的偏移量或另一个对象引用的一部分重叠。虽然一个对象引用可以完全重叠另一个对象引用，但这是不可验证的。

可以使用指针算术和 `ldind` 间接加载字段或 `stind` 间接存储字段来访问字段【】。参见 【】 和 【】 了解此信息的编码。对于显式布局，每个字段都应分配一个偏移量。

<a id="pack"></a>**.pack** 指令指定字段应放置在运行时对象的字节地址上，这些地址是指定数字的倍数，或者是该字段类型的自然对齐，以较小者为准。例如，`.pack 2` 将允许 32 位宽的字段在偶数地址上开始，而没有任何 **.pack** 指令，它们将被自然对齐；也就是说，放置在 4 的倍数的地址上。**.pack** 后面的整数应为以下之一：0、1、2、4、8、16、32、64 或 128。零值表示使用的 _pack_ 大小应匹配当前平台的默认值。任何具有显式布局控制的类型都不应提供 **.pack** 指令。

<a id="size"></a>**.size** 指令指示最小大小，并表示允许填充。因此，分配的内存量是布局计算的大小和 **.size** 指令的最大值。请注意，如果此指令用于值类型，那么大小应小于 1 MByte。

控制实例布局的元数据不是 “提示”，它是 VES 的一个组成部分，所有符合 CLI 的实现都应支持。

下面的类使用其字段的顺序布局：

 ```cil
 .class sequential public SequentialClass
 { .field public int32 a  // store at offset 0 bytes
   .field public int32 b  // store at offset 4 bytes
 }
 ```

下面的类使用其字段的显式布局：

 ```cil
 .class explicit public ExplicitClass
 { .field [0] public int32 a // store at offset 0 bytes
   .field [6] public int32 b // store at offset 6 bytes
 }
 ```

下面的值类型使用 **.pack** 将其字段打包在一起：

 ```cil
 .class value sealed public MyClass extends [mscorlib]System.ValueType
 { .pack 2
   .field  public int8  a  // store at offset 0 bytes
   .field  public int32 b // store at offset 2 bytes (not 4)
 }
 ```

下面的类指定了一个连续的 16 字节块：

 ```cil
 .class public BlobClass
 { .size 16
 }
 ```

>---
### 8.8. 全局字段和方法

除了具有静态成员的类型外，许多语言都有数据和方法不是类型一部分的概念。这些被称为 **全局** (_global_) 字段和方法。

识别 CLI 中的全局字段和方法的最简单方法是想象它们只是一个不可见的 **抽象** 公共类的成员。实际上，CLI 定义了这样一个特殊的类，名为 `<Module>`，它没有基类型，也不实现任何接口。这个类是一个顶级类，它不是嵌套的。唯一明显的区别在于当多个模块合并在一起时，如何处理这个特殊类的定义，就像类加载器所做的那样。这个过程被称为 **元数据合并** (_metadata merging_)。

对于普通类型，如果元数据合并了同一类型的两个定义，它只是丢弃一个定义，假设它们是等价的，并且在使用类型时没有发现任何异常。然而，对于持有全局成员的特殊类，成员在合并时跨所有模块联合。如果同一个名字似乎在多个模块中为跨模块使用而定义，那么就会出现错误。

详细来说：
 * 如果不存在相同种类 (字段或方法) 、名称和签名的成员，那么将此成员添加到输出类中。
 * 如果有重复项，并且除 **compilercontrolled** 之外的可访问性不超过一个，那么将它们全部添加到输出类中。
 * 如果有重复项，并且两个或更多项的可访问性不是 **compilercontrolled**，则发生错误。

严格来说，CLI 不支持全局静态变量，即使全局字段和方法可能被认为是这样。模块中的所有全局字段和方法都由制造的类 "`<Module>`" 拥有。然而，每个模块都有自己的 "`<Module>`" 类。甚至没有办法引用另一个模块中这样的一个全局字段或方法 (早期绑定)。但是，可以通过反射 (后期绑定) 访问到它们。







## 24. end

---

<pre>
    <em>GenArgs</em> ::= <em>Type</em> [ ',' <em>Type</em> ]*
</pre>