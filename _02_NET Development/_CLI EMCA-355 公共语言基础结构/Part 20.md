

##  20. 元数据逻辑格式：tables
<a id="MetadataTables"></a>

此节定义了描述元数据的结构，以及它们如何交叉索引。这对应于元数据如何在从 PE 文件读入内存后的布局方式。有关 PE 文件本身内部元数据布局的描述，参见 [_元数据物理布局_](#metadata-physical-layout)。

元数据存储在两种结构中：表 (**_Tables_**，记录数组) 和堆 (**_Heaps_**)。任何模块中都有四个堆：***String Heap***，***Blob Heap***，***Userstring Heap*** 和 ***Guid Heap***。前三个是字节数组 (这些堆的有效索引值可能是 0，23，25，39 等)。***Guid Heap*** 是 ***GUID*** 的数组，每个 ***GUID*** 宽 16 字节。它的第一个元素编号为 1，第二个为 2，依此类推。

每个表的每个列的每个条目要么是常数，要么是索引。

常数要么是字面值 (例如，ALG_SID_SHA1 = 4，存储在 **`Assembly`** 表的 *HashAlgId* 列中)，要么是更常见的位掩码。大多数位掩码 (它们几乎都被称为 ***Flags***) 宽 2 字节 (例如，**`Field`** 表中的 ***`Flags`*** 列)，但有几个是 4 字节 (例如，**`TypeDef`** 表中的 ***`Flags`*** 列)。

每个索引值宽度为 2 或 4 字节。索引值指向同一表或另一个表，或者指向四个堆中的一个。只有当用于特定模块时，表中的每个索引值列的大小才会变为 4 字节。因此，如果一个特定列索引的表或表的最高行号适合于 2 字节值，则索引值列只需要宽 2 字节。相反，对于包含 64K 或更多行的表，该表的索引值将宽 4 字节。

表的索引从 1 开始，因此索引 1 表示任何给定元数据表的第一行。索引值为零表示它根本不索引行；也就是说，它的行为就像一个空引用。

索引元数据表的列有两种。有关这些表的物理表示的详细信息，请参见 [[_#~ stream_]](#physicalStream) ：

 * Sample &mdash; 这样的列索引一个表 (唯一)。例如，**`TypeDef`** 表中的 ***`FieldList`*** 列始终索引 **`Field`** 表。因此，该列中的所有值都是简单的整数，这些值给出了目标表中的行号。

 * Coded &mdash; 这样的列可以索引几个表中的任何一个。例如，**`TypeDef`** 表中的 ***`Extends`*** 列可以索引到 **`TypeDef`** 或 **`TypeRef`** 表。该索引值的几位被保留用于定义它的目标表。在大多数情况下，此规范讨论的是索引值在目标表中解码为行号之后的值。规范在描述元数据物理布局的部分中包含了这些编码索引值的描述 [[↗]](#(#metadata-physical-layout))。

元数据保留编译器或代码生成器创建的名称字符串，不做任何更改。本质上，它将每个字符串视为不透明的二进制数据对象 (***blob***)。特别是，它保留了大小写。CLI 对存储在元数据中并随后由 CLI 处理的名称的长度没有限制。

匹配 **`AssemblyRef`** 和 **`ModuleRef`** 到它们对应的 *Assembly* 和 *Module* 应该是不区分大小写的。然而，对于所有其他名称的匹配 (类型，字段，方法，属性，事件) 应该是精确的 &mdash; 所有平台上这个级别的解析都是相同的，无论它们的操作系统或平台是否区分大小写。

表都有一个名称 (例如，“**`Assembly`**”) 和一个数字 (例如，0x20)。每个表的编号都列在以下子小节的标题中。表编号表示它们对应的表在 PE 文件中出现的顺序，并且还有一组位序列 [[↗]](#physicalStream) 用来表示给定的表是否存在。表的编号则是在这组位序列中的位置。

有几个表代表了常规 CLI 文件的扩展。具体来说，**`ENCLog`** 和 **`ENCMap`**，它们出现在一些临时图像中 (这些图像由 “编辑并继续” 或 “增量编译” 的场景中生成)，同时进行调试。这两种表类型被保留以备未来使用。

对一个类型的方法或字段的引用被一起存储在一个名为 **`MemberRef`** 的元数据表中。然而，有时为了更清晰的解释，这个标准区分为了两种引用，分别是 **`MethodRef`** 和 **`FieldRef`**。

某些表需要按主要键 (***Primary Key***) 排序，如下所示：

 | _Table_                      | _Primary Key_           |
 | :--------------------------- | :---------------------- |
 | **`ClassLayout`**            | ***`Parent`***          |
 | **`Constant`**               | ***`Parent`***          |
 | **`CustomAttribute`**        | ***`Parent`***          |
 | **`DeclSecurity`**           | ***`Parent`***          |
 | **`FieldLayout`**            | ***`Field`***           |
 | **`FieldMarshal`**           | ***`Parent`***          |
 | **`FieldRVA`**               | ***`Field`***           |
 | **`GenericParam`**           | ***`Owner`***           |
 | **`GenericParamConstraint`** | ***`Owner`***           |
 | **`ImplMap`**                | ***`MemberForwarded`*** |
 | **`InterfaceImpl`**          | ***`Class`***           |
 | **`MethodImpl`**             | ***`Class`***           |
 | **`MethodSemantics`**        | ***`Association`***     |
 | **`NestedClass`**            | ***`NestedClass`***     |

此外，**`InterfaceImpl`** 表使用 ***`Interface`*** 列作为次要键 (**_Secondary key_**) 进行排序，**`GenericParam`** 表使用 ***`Number`*** 列作为次要键进行排序。

最后，**`TypeDef`** 表有一个特殊的排序约定：封闭类的定义应在其嵌套的所有类的定义之前。

元数据项 (元数据表中的记录) 由元数据 **_token_** 寻址。未编码的元数据 **_token_** 是 4 字节无符号整数，其中最高有效字节包含元数据表索引值，三个最低有效字节包含基于 1 的记录索引值。元数据表及其各自的索引值在后续子小节中描述。

编码的元数据 **_token_** 也包含表和记录索引值，但格式不同。有关编码的详细信息，请参见 [_元数据物理布局_](#metadata-physical-layout)。

>---
### 20.1. 元数据验证规则

下面的子句描述了每种元数据表的模式，并解释了保证发送到任何 PE 文件中的元数据有效的详细规则。检查元数据是否有效可以确保以后的处理 (例如检查 CIL 指令流的类型安全性、构建方法表、CIL-to-native-code 的编译和数据封送处理) 不会导致 CLI 崩溃或以不安全的方式运行。

此外，一些规则用于检查与 CLS 要求的兼容性，即使这些规则与有效元数据无关。这些规则以 |CLS| 标签结尾。

有效元数据的规则引用单个模块。模块是任何元数据的集合，它通常可以被保存到磁盘文件中，包括由编译器和链接器的输出，或脚本编译器的输出 (其中元数据通常只在内存中保存，但实际上从未保存到磁盘文件中)。

这些规则只处理模块内的验证。因此，检查是否符合此标准的软件无需解析在其他模块中定义的引用，或遍历在其他模块中定义的类型层次结构。然而，即使两个模块 A 和 B 分别分析它们各自包含的有效元数据，但是当它们一起审视时，仍然可能出错 (例如，在模块 A 中调用模块 B 中定义的方法，可能通过指定一个调用点签名进行调用，但是该签名与 B 中为该方法定义的签名不匹配)。

所有检查都被分类为 ERROR，WARNING 或 CLS。
 * ERROR 检查报告一些可能会导致 CLI 崩溃或挂起的事物，它可能运行但产生错误的答案，也可能是完全良性的。CLI 的一致性实现可能不接受违反 ERROR 规则的元数据，因此这样的元数据是无效的，并且不可移植。
 - WARNING 检查报告一些事物，它不是真正的错误，但可能是编译器的错误。通常，它表示编译器可以以更紧凑的方式编码相同的信息，或者元数据表示在运行时没有实际使用的构造。所有符合规范的实现都应支持仅违反 WARNING 规则的元数据；因此这样的元数据是有效且可移植的。
 * CLS 检查报告缺乏与 Common Language Specification 的兼容性。这样的元数据是有效且可移植的，但可能存在无法处理它的编程语言，尽管所有符合 CLI 的实现都支持这些构造。

验证规则分为以下几个大类：

 * ***Number of Rows***：行数。少数表只允许一行 (例如，*Module* 表)，大多数表没有这样的限制。

 - ***Unique Rows***：唯一行。任何表都不能包含重复的行，其中 “重复” 是根据其键列或列的组合来定义的。

 * ***Valid Indexes***：有效索引值。作为索引值的列应指向某个有意义的地方，如下所示：

    * 任何行关联 ***String Heap***，***Blob Heap*** 或 ***Userstring Heap*** 的索引值都应指向该堆，索引既不指向在其堆开始之前 (偏移量 0)，也不指向其堆结束之后。
    * 任何行关联 ***Guid Heap*** 的索引值都应该位于 1 和此模块中的最大元素编号之间，包括 1 和最大元素编号。
    * 每个指向另一个元数据表的索引值 (行号) 应位于 0 和该表的 “row-count + 1” 之间 (对于某些表，索引值可以指向任意目标表的末尾，这意味着它没有索引任何事物)。

 + ***Valid BitMasks***：有效位掩码。作为位掩码的列只能设置有效的位排列。

 * ***Valid RVAs***：有效相对虚拟地址。对于分配了 ***RVA*** (相对虚拟地址，表示为从相应的 PE 文件加载到内存的地址开始表示的字节偏移量) 的字段和方法，存在一些限制。

下面列出的一些规则实际上并没有描述什么 —— 例如，有些规则声明某个表允许零行或多行 —— 在这种情况下，检查不可能失败。这样做只是为了完整性，记录这些细节确实已经被处理，而不是被忽视。

CLI 对存储在元数据中且随后由 CLI 的实现处理的名称长度没有限制。

>---
### 20.30. Module: 0x00
<a id="Module_0x00"></a>

> *Module* 表有以下列：

| Column                                         |  Kind   | Size  |       Value       | Description                                      | Link |
| :--------------------------------------------- | :-----: | :---: | :---------------: | :----------------------------------------------- | :--- |
| ***`Token`***                                  | Literal |   4   |     00UUUUUU      | 行编号，高位字节表示表编号，低三位字节是行编号。 |      |
| ***`Generation`***                             | Literal |   2   |         0         | 保留，应为 0。                                   |      |
| ***`Name`***                                   |  Index  |   4   | ***String Heap*** | 索引模块的名称。                                 |      |
| ***`Mvid`***                                   |  Index  |   4   |  ***Guid Heap***  | 用于区分同一模块的两个版本的 ***GUID***。        |      |
| ***`EncId`***<br>/***`GenerationId`***         |  Index  |   4   |  ***Guid Heap***  | 保留，应为 0。                                   |      |
| ***`EncBaseId`***<br>/***`BaseGenerationId`*** |  Index  |   4   |  ***Guid Heap***  | 保留，应为 0。                                   |      |

***`Mvid`*** 列应该索引 ***Guid Heap*** 中的一个唯一 ***GUID*** ([[↗]](#guid-heap))，该 ***GUID*** 标识该模块的实例。符合 CLI 的实现可以在读取时忽略 ***`Mvid`***。应该为每个模块新生成一个 ***`Mvid`*** (使用 ISO/IEC 11578:1996 (附录 A) 或其他兼容算法指定的算法)。

术语 ***GUID*** 表示全局唯一标识符，通常使用其十六进制编码显示的 16 字节长的数字。可以通过几种众所周知的算法生成 ***GUID***，包括在 ***RPC*** 和 ***CORBA*** 中用于 ***UUID*** (通用唯一标识符) 的算法，以及在 ***COM*** 中用于 ***CLSID***、***GUID*** 和 ***IID*** 的算法。

虽然 VES 本身不使用 ***`Mvid`***，但其他工具 (如调试器，这超出了本标准的范围) 依赖于 ***`Mvid`*** 来识别模块之间的不同。

可以将 ***`Generation`***、***`EncId`*** 和 ***`EncBaseId`*** 列写为零，并且可以由符合 CLI 的实现忽略。

**`Module`** 表中的行是程序集中的 **.module** 指令的结果 ([[↗]](#module))。

> 元数据验证规则

| Order | Validation Rule                                                                                                                               | Level |
| :---: | :-------------------------------------------------------------------------------------------------------------------------------------------- | :---: |
|  1.   | **`Module`** 表应该包含一行且只有一行。                                                                                                       | ERROR |
|  2.   | ***`Name`*** 应该索引 ***String Heap*** 中的 non-empty 字符串。此字符串应该与解析到此模块的任何相应 **`ModuleRef`**._`Name`_ 字符串完全匹配。 | ERROR |
|  3.   | ***`Mvid`*** 应该索引 ***Guid Heap*** 中的非空 ***GUID***。                                                                                   | ERROR |

>---

### 20.38. TypeRef: 0x01
<a id="TypeRef_0x01"></a>

> *TypeRef* 表有以下列：

| Column                                      |  Kind   | Size  |       Value       | Description                                                                       | Link                                  |
| :------------------------------------------ | :-----: | :---: | :---------------: | :-------------------------------------------------------------------------------- | :------------------------------------ |
| ***`Token`***                               | Literal |   4   |     01UUUUUU      | 行编号，高位字节表示表编号，低三位字节是行编号。                                  |                                       |
| ***`ResolutionScope`***                     |  Index  |   4   |      Tables       | 索引 **`Module`**，**`ModuleRef`**，**`AssemblyRef`** 或 **`TypeRef`** 表，或空。 | [*ResolutionScope*](#ResolutionScope) |
| ***`TypeName`***<br>/***`Name`***           |  Index  |   4   | ***String Heap*** | 索引引用的类型名称标识。                                                          |                                       |
| ***`TypeNamespace`***<br>/***`Namespace`*** |  Index  |   4   | ***String Heap*** | 索引引用的类型的所属空间名称标识。                                                |                                       |

> 元数据验证规则

| Order | Validation Rule                                                                                                                                                                                  |  Level  |
| :---: | :----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :-----: |
|  1.   | ***`ResolutionScope`*** 应该严格是以下之一：                                                                                                                                                     |         |
|       | 1. null —— **`ExportedType`** 表中应该有一行对应这个类型 —— 它的 ***`Implementation`*** 字段应该包含一个 **`File`** **_token_** 或一个 **`AssemblyRef`** **_token_**，以说明类型是在哪里定义的。 |  ERROR  |
|       | 2. 一个 **`TypeRef`** **_token_**，如果它是一个嵌套类型 (例如，可以通过检查它的 **`TypeDef`** 表中的 ***`Flags`*** 列来确定 —— 可访问性子字段是 `tdNestedXXX` 集合中的一个)。                    |  ERROR  |
|       | 3. 一个 **`ModuleRef`** **_token_**，目标类型在与当前模块相同的程序集中的另一个模块中定义。                                                                                                      |  ERROR  |
|       | 4. 一个 **`Module`** **_token_**，目标类型在当前模块中定义 —— 这在 CLI (“压缩元数据”) 模块中不应该出现。                                                                                         | WARNING |
|       | 5. 一个 **`AssemblyRef`** **_token_**，目标类型在与当前模块不同的程序集中定义。                                                                                                                  |  ERROR  |
|  2.   | ***`TypeName`*** 应该在 ***String Heap*** 中索引一个 non-empty 字符串。                                                                                                                          |  ERROR  |
|  3.   | ***`TypeNamespace`*** 可以为 null，或 non-null。                                                                                                                                                 |         |
|  4.   | 如果非空，***`TypeNamespace`*** 应该在 ***String Heap*** 中索引一个 non-empty 字符串。                                                                                                           |  ERROR  |
|  5.   | ***`TypeName`*** 字符串应该是一个有效的 CLS 标识符。                                                                                                                                             |   CLS   |
|  6.   | 不应该有重复的行，重复的行具有相同的 ***`ResolutionScope`***，***`TypeName`*** 和 ***`TypeNamespace`***。                                                                                        |  ERROR  |
|  7.   | 使用 CLS 冲突标识符规则比较 ***`TypeName`*** 和 ***`TypeNamespace`*** 的字段时，不应出现重复的行。                                                                                               |   CLS   |


>---
### 20.37. TypeDef: 0x02
<a id="TypeDef_0x02"></a>

> *TypeDef* 表有以下列：

| Column                                      |  Kind   | Size  | Value             | Description                                                                                                                                                                                                | Link                                |
| :------------------------------------------ | :-----: | :---: | :---------------- | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :---------------------------------- |
| ***`Token`***                               | Literal |   4   | 02UUUUUU          | 行编号，高位字节表示表编号，低三位字节是行编号。                                                                                                                                                           |                                     |
| ***`Flags`***<br>/***`Attributes`***        | BitMask |   4   | _TypeAttributes_  | _TypeAttributes_ 类型的位掩码。                                                                                                                                                                            | [_TypeAttributes_](#TypeAttributes) |
| ***`TypeName`***<br>/***`Name`***           |  Index  |   4   | ***String Heap*** | 索引类型名称标识。                                                                                                                                                                                         |                                     |
| ***`TypeNamespace`***<br>/***`Namespace`*** |  Index  |   4   | ***String Heap*** | 索引类型的所属空间名称标识。                                                                                                                                                                               |                                     |
| ***`Extends`***<br>/***`BaseType`***        |  Index  |   4   | Tables            | 索引 **`TypeDef`**，**`TypeRef`** 或 **`TypeSpec`** 表。                                                                                                                                                   | [_TypeDefOrRef_](#physicalStream)  |
| ***`FieldList`***                           |  Index  |   4   | **`Field`**       | 标记了由此类型拥有的一连串字段的第一个。该连续运行继续到以下较小的一个：<br>&emsp;i. **`Field`** 表的最后一行<br>&emsp;ii. 通过检查此 **`TypeDef`** 表中下一行的 ***`FieldList`*** 找到的下一组字段。      |                                     |
| ***`MethodList`***                          |  Index  |   4   | **`MethodDef`**   | 标记了由此类型拥有的一连串方法的第一个。该连续运行继续到以下较小的一个：<br>&emsp;i. **`MethodDef`** 表的最后一行<br>&emsp;ii. 通过检查此 **`TypeDef`** 表中下一行的 ***`MethodList`*** 找到的下一组方法。 |                                     |

**`TypeDef`** 表的第一行代表伪类，该伪类 ("`<Module>`") 作为在模块范围内定义的函数和变量的父类。

任何类型都应该是以下之一，并且只能是以下之一：
 * **Class**：***`Flags`***.*`Interface`* = 0，并最终派生自 `System.Object`。 
 * **Interface**：***`Flags`***.*`Interface`* = 1。 
 * **Value type**：最终派生自 `System.ValueType`。

对于任何给定的类型，都有两个独立且不同的指向其他类型的指针链 (这些指针实际上是作为元数据表索引实现的)。这两个链分别是：
 * ***Extension chain***：扩展链，通过 **`TypeDef`** 表的 ***`Extends`*** 列定义。通常，一个派生类继承扩展一个基类 (始终是一个，有且只有一个基类)。 
 * ***Interface chains***：接口链，通过 **`InterfaceImpl`** 表定义。通常，一个类可以实现零个、一个或多个接口。

这两个链 (扩展和接口) 在元数据中始终保持分离。扩展链表示一对一关系，即一个类扩展 (或 “派生自”) 另一个类 (称为其直接基类)。接口链可以表示一对多关系，即一个类可能实现两个或更多接口。

接口也可以实现一个或多个其他接口 —— 元数据通过 **`InterfaceImpl`** 表存储这些链接 (这里的术语有些不适当，因为接口没有涉及 “***Implementation***”；也许更清晰的名称可能是 *Interface* 表，或 *InterfaceInherit* 表) 

另一种稍微专门化的类型是嵌套类型，它在 ILAsm 中被声明为在封闭类型声明中的词法嵌套。类型是否嵌套可以通过其 ***`Flags`***.*`Visibility`* 子字段的值确定 —— 它应该是 { *`NestedPublic`*, *`NestedPrivate`*, *`NestedFamily`*, *`NestedAssembly`*, *`NestedFamANDAssem`*, *`NestedFamORAssem`* } 集合中的一个。

如果类型是泛型，其参数在 **`GenericParam`** 表 [[↗]](#GenericParam_0x2A) 中定义。**`GenericParam`** 表中的条目引用 **`TypeDef`** 表中的条目；**`TypeDef`** 表没有引用 **`GenericParam`** 表。

继承层次结构的根看起来像这样：

![继承层次结构的根](./.img/继承层次结构的根.png)

有一个系统定义的根是 `System.Object`。所有的类和值类型最终都应该从 `System.Object` 派生；类可以从其他类派生 (通过一个单一的，非循环的链) 到任何需要的深度。这个 *Extension* 继承链用实箭头表示。

接口不能相互继承；但是，它们可以有零个或多个必需的接口，这些接口应该被实现。*Interface* 需求链显示为虚箭头。它包含了接口和类或值类型之间的链接 —— 后者被称为实现该接口或多个接口。

常规值类型 (即，排除枚举 — 见后文) 被定义为直接从 `System.ValueType` 派生。常规值类型不能派生到一个以上的深度。另一种表述方式是，用户定义的值类型应该是密封的。用户定义的枚举应该直接从 `System.Enum` 派生。枚举不能在 `System.Enum` 以下派生到一个以上的深度。另一种表述方式是，用户定义的枚举应该是密封的。`System.Enum` 直接从 `System.ValueType` 派生。

用户定义的委托从 `System.Delegate` 派生。委托不能派生到一个以上的深度。

关于声明类型的指令，请参见 [_泛型定义_](#generic-type) 和 [_类型定义_](#class)。

> 元数据验证规则

| Order | Validation Rule                                                                                                                                                                                                                                                                                                          | Level |
| :---: | :----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :---: |
|  1.   | **`TypeDef`** 表可以包含一个或多个行                                                                                                                                                                                                                                                                                     |       |
|  2.   | _Flags:_                                                                                                                                                                                                                                                                                                                 |       |
|       | 1. ***`Flags`*** 只能设置那些指定的值。                                                                                                                                                                                                                                                                                  | ERROR |
|       | 2. 可以设置 0 或 1 个 `SequentialLayout` 和 `ExplicitLayout`；如果没有设置，则默认为 `AutoLayout`。                                                                                                                                                                                                                      | ERROR |
|       | 3. 可以设置 0 或 1 个 `UnicodeClass` 和 `AutoClass`；如果没有设置，则默认为 `AnsiClass`。                                                                                                                                                                                                                                | ERROR |
|       | 4. 如果 ***`Flags`***._`HasSecurity`_ = 1，那么以下条件中至少有一个应该为真：<br>&emsp;&emsp;i.这个类型有一个名为 `SuppressUnmanagedCodeSecurityAttribute` 的自定义特性。<br>&emsp;&emsp;ii. 这个类型有一个名为 `SuppressUnmanagedCodeSecurityAttribute` 的自定义特性。                                                  | ERROR |
|       | 5. 如果这个类型在 **`DeclSecurity`** 表中拥有一行 (或多行)，那么 ***`Flags`***.*`HasSecurity`* 应该是 1。                                                                                                                                                                                                                | ERROR |
|       | 6. 如果这个类型有一个名为 `SuppressUnmanagedCodeSecurityAttribute` 的自定义特性，那么 ***`Flags`***.*`HasSecurity`* 应该是 1。                                                                                                                                                                                           | ERROR |
|       | 7.  注意，接口设置 *`HasSecurity`* 是有效的。然而，安全系统忽略任何附加到该接口的权限请求。                                                                                                                                                                                                                              |       |
|  3.   | ***`Name`*** 应该在 ***String Heap*** 中索引一个 non-empty 字符串。                                                                                                                                                                                                                                                      | ERROR |
|  4.   | ***`TypeName`*** 字符串应该是一个有效的 CLS 标识符。                                                                                                                                                                                                                                                                     |  CLS  |
|  5.   | ***`TypeNamespace`*** 可以为空或非空。                                                                                                                                                                                                                                                                                   | ERROR |
|  6.   | 如果非空，那么 ***`TypeNamespace`*** 应该在 ***String Heap*** 中索引一个 non-empty 字符串。                                                                                                                                                                                                                              |       |
|  7.   | 如果非空，***`TypeNamespace`*** 的字符串应该是一个有效的 CLS 标识符。                                                                                                                                                                                                                                                    |       | CLS                                                                                          |  |
|  8.   | 每个类 (除了 `System.Object` 和特殊类 `<Module>`) 都应该继承一个 (有且只有一个) 其他类 —— 所以对于一个类，***`Extends`*** 应该是非空的。                                                                                                                                                                                 | ERROR |
|  9.   | `System.Object` 应该有一个 ***`Extends`*** 值为 null。                                                                                                                                                                                                                                                                   | ERROR |
|  10.  | `System.ValueType` 应该有一个 ***`Extends`*** 值为 `System.Object`。                                                                                                                                                                                                                                                     | ERROR |
|  11.  | 除了 `System.Object` 和特殊类 `<Module>`，对于任何类，***`Extends`*** 应该索引在 **`TypeDef`**，**`TypeRef`**，或 **`TypeSpec`** 表中的一个有效行，其中有效意味着 1 ≤ row ≤ rowcount。此外，该行本身必须是一个类 (而不是接口或值类型)，该基类不应该被密封 (其 ***`Flags`***.*`Sealed`* 应该是 0)。                       | ERROR |
|  12.  | 一个类不能扩展自身或其子类 (即，它的派生类)，因为这将在层次树中引入循环。                                                                                                                                                                                                                                                | ERROR | (对于泛型类型，参见 [_泛型定义_](#generic-define) 和 [_泛型和递归继承图_](#generic-inherit)) |
|  13.  | 一个接口永远不会扩展另一个类型 —— 所以 ***`Extends`*** 应该为空；接口确实实现了其他接口，但是这种关系是通过 **`InterfaceImpl`** 表捕获的，而不是 ***`Extends`*** 列。                                                                                                                                                    | ERROR |
|  14.  | ***`FieldList`*** 可以为空或非空。                                                                                                                                                                                                                                                                                       |
|  15.  | 一个类或接口可以拥有零个或多个字段。                                                                                                                                                                                                                                                                                     |
|  16.  | 一个值类型应该有一个非零的大小 —— 通过定义至少一个字段，或者提供一个非零的 ***`ClassSize`***。                                                                                                                                                                                                                           | ERROR |
|  17.  | 如果 ***`FieldList`*** 是非空的，它应该索引 **`Field`** 表中的一个有效行，其中有效意味着 1 ≤ row ≤ rowcount+1。                                                                                                                                                                                                          | ERROR |
|  18.  | ***`MethodList`*** 可以为空或非空。                                                                                                                                                                                                                                                                                      |
|  19.  | 一个类型可以拥有零个或多个方法。                                                                                                                                                                                                                                                                                         |
|  20.  | 值类型的运行时大小不应超过 1 MByte (0x100000 字节)。                                                                                                                                                                                                                                                                     | ERROR |
|  21.  | 如果 ***`MethodList`*** 是非空的，它应该索引 **`MethodDef`** 表中的一个有效行，其中有效意味着 1 ≤ row ≤ rowcount+1。                                                                                                                                                                                                     | ERROR |
|  22.  | 一个类如果有一个或多个抽象方法不能被实例化，那么它必须具有 ***`Flags`***.*`Abstract`* = 1。注意类拥有的方法包括从其基类继承以及它实现的接口的所有方法，以及通过其 ***`MethodList`*** 定义的方法。CLI 将在运行时分析类定义；如果它发现一个类有一个或多个抽象方法，但是 ***`Flags`***.*`Abstract`* = 0，它将抛出一个异常。 | ERROR |
|  23.  | 一个接口应该有 ***`Flags`***.*`Abstract`* = 1。                                                                                                                                                                                                                                                                          | ERROR |
|  24.  | 对于一个抽象类型来说，有一个构造方法 (即，一个名为 `.ctor` 的方法) 是有效的。                                                                                                                                                                                                                                            |
|  25.  | 任何非抽象类型 (即 ***`Flags`***.*`Abstract`* = 0) 应该为其抽象类或接口协议要求的每个方法提供一个实现 (提供方法主体)。它的方法可以从其基类继承，从它实现的接口继承，或者由它自己定义。实现可以从其基类继承，或者由它自己定义。                                                                                           | ERROR |
|  26.  | 一个接口 (***`Flags`***.*Interface* = 1) 可以拥有静态字段 (**`Field`**.*Static* = 1) 但不能拥有实例字段 (**`Field`**.*Static* = 0)。                                                                                                                                                                                     | ERROR |
|  27.  | 一个接口不能被密封 (如果 ***`Flags`***.*`Interface`* = 1，那么 ***`Flags`***.*Sealed* 应该是 0)。                                                                                                                                                                                                                        | ERROR |
|  28.  | 一个接口拥有的所有方法 (***`Flags`***.*`Interface`* = 1) 应该是抽象的 (***`Flags`***.*Abstract* = 1)。                                                                                                                                                                                                                   | ERROR |
|  29.  | 在 **`TypeDef`** 表中，基于 ***`TypeNamespace`***+***`TypeName`*** 不应该有重复的行 (除非这是一个嵌套类型 — 见下文)。                                                                                                                                                                                                    | ERROR |
|  30.  | 如果这是一个嵌套类型，那么在 **`TypeDef`** 表中，基于 ***`TypeNamespace`***+***`TypeName`***+_OwnerRowInNestedClassTable_ 不应该有重复的行。                                                                                                                                                                             | ERROR |
|  31.  | 使用 CLS 冲突标识符规则进行比较时，基于 ***`TypeNamespace`***+***`TypeName`*** 的字段，不应该有重复的行 (除非这是一个嵌套类型 — 见下文)。                                                                                                                                                                                |  CLS  |                                                                                              |
|  32.  | 使用 CLS 冲突标识符规则进行比较时，如果是一个嵌套类型，基于 ***`TypeNamespace`***+***`TypeName`***+_OwnerRowInNestedClassTable_ 和 ***`TypeNamespace`***+***`TypeName`*** 的字段，不应该有重复的行。                                                                                                                     |  CLS  |                                                                                              |
|  33.  | 如果 ***`Extends`*** = `System.Enum` (即，类型是用户定义的枚举) 那么：                                                                                                                                                                                                                                                   |
|       | 1. 应该是封闭的 (*`Sealed`* = 1)。                                                                                                                                                                                                                                                                                       | ERROR |
|       | 2. 不应该有自己的任何方法 (***`MethodList`*** 链应该是零长度)。                                                                                                                                                                                                                                                          | ERROR |
|       | 3. 不应该实现任何接口 (此类型在 **`InterfaceImpl`** 表中没有条目)。                                                                                                                                                                                                                                                      | ERROR |
|       | 4. 不应该有任何属性。                                                                                                                                                                                                                                                                                                    | ERROR |
|       | 5. 不应该有任何事件。                                                                                                                                                                                                                                                                                                    | ERROR |
|       | 6. 任何静态字段应该是字面值 (具有 ***`Flags`***.*`Literal`* = 1)。                                                                                                                                                                                                                                                       | ERROR |
|       | 7. 应该有一个或多个 **static literal** 字段，每个字段都具有枚举的类型。                                                                                                                                                                                                                                                  |  CLS  |                                                                                              |
|       | 8. 应该有一个实例字段，为内置整数类型。                                                                                                                                                                                                                                                                                  | ERROR |
|       | 9. 实例字段的 ***`Name`*** 字符串应该是 "`value__`"，该字段应该被标记为 `RTSpecialName`，并且该字段应该具有 CLS 整数类型之一。                                                                                                                                                                                           |  CLS  |                                                                                              |
|       | 10. 除非它们是文字的，否则不应该有任何静态字段。                                                                                                                                                                                                                                                                         | ERROR |
|  34.  | 嵌套类型 (如上所定义) 应该只拥有 **`NestedClass`** 表中的一行，其中 “拥有” 意味着在 **`NestedClass`** 表中的一行，其 ***`NestedClass`*** 列包含此类型定义的    **`TypeDef`**  **_token_**。                                                                                                                              | ERROR |
|  35.  | 值类型应该是封闭的。                                                                                                                                                                                                                                                                                                     | ERROR |


>---
### 20.15. Field: 0x04
<a id="Field_0x04"></a>

> *Field* 表有以下列：

| Column                               |  Kind   | Size  | Value             | Description                                      | Link                                  |
| :----------------------------------- | :-----: | :---: | :---------------- | :----------------------------------------------- | :------------------------------------ |
| ***`Token`***                        | Literal |   4   | 04UUUUUU          | 行编号，高位字节表示表编号，低三位字节是行编号。 |                                       |
| ***`Flags`***<br>/***`Attributes`*** | BitMask |   2   | _FieldAttributes_ | _FieldAttributes_ 类型的位掩码                   | [_FieldAttributes_](#FieldAttributes) |
| ***`Name`***                         |  Index  |   4   | ***String Heap*** | 索引字段的名称                                   |                                       |
| ***`Signature`***                    |  Index  |   4   | ***Blob Heap***   | 索引字段的签名                                   |                                       |

从概念上讲，**`Field`** 表中的每一行都由 **`TypeDef`** 表中的一行 (有且只有一行) 拥有。然而，**`Field`** 表中任何一行的 *owner* 并不存储在 **`Field`** 表本身中。在 **`TypeDef`** 表的每一行中只有一个前向指针 “*forward-pointer*” (***`FieldList`*** 列)，如下图所示。

 ![](./.img/field-figure.png)

**`TypeDef`** 表有 1 ~ 4 行。**`TypeDef`** 表的第一行对应于 CLI 自动插入的伪类型。它用于表示 **`Field`** 表中对应于全局变量的那些行。**`Field`** 表有 1 ~ 6 行：
 - 类型 1 (`<module>` 的伪类型) 拥有 **`Field`** 表中的 1 和 2 行。
 - 类型 2 在 **`Field`** 表中没有任何行，尽管其 ***`FieldList`*** 索引了 **`Field`** 表中的第 3 行。
 - 类型 3 拥有 **`Field`** 表中的 3 ~ 5 行。
 - 类型 4 拥有 **`Field`** 表中的第 6 行。

因此，在 **`Field`** 表中，行 1 和 2 属于类型 1 (全局变量) ；行 3 ~ 5 属于类型 3；行 6 属于类型 4。

**`Field`** 表中的每一行都是由顶级 **.field** 指令 [[↗]](#il-top-impl) 或类型内的 **.field** 指令 [[↗]](#field) 产生的。

> 元数据验证规则

| Order | Validation Rule                                                                                                                                                                                                                                       | Level |
| :---: | :---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :---: |
|  1.   | **`Field`** 表可以包含零行或多行。                                                                                                                                                                                                                    |       |
|  2.   | 在 **`TypeDef`** 表中每一行应有一个 ***`FieldList`***，且只有一个。                                                                                                                                                                                   | ERROR |
|  3.   | **`TypeDef`** 表中的 *owner* 行不能是接口。                                                                                                                                                                                                           |  CLS  |
|  4.   | ***`Flags`*** 只应设置那些指定的值。                                                                                                                                                                                                                  | ERROR |
|  5.   | ***`Flags`*** 的 *`FieldAccessMask`* 子字段应精确地包含 *`CompilerControlled`*、*`Private`*、*`FamANDAssem`*、*`Assembly`*、*`Family`*、*`FamORAssem`* 或 *`Public`* 中的一个 [[↗]](#FieldAttributes)。                                               | ERROR |
|  6.   | ***`Flags`*** 可以设置 *`Literal`* 或 *`InitOnly`* 中的一个或两者都不设置，但不能同时设置两者 [[↗]](#FieldAttributes)。                                                                                                                               | ERROR |
|  7.   | 如果 ***`Flags`***.*`Literal`* = 1，那么 ***`Flags`***.*`Static`* 也应为 1 [[↗]](#FieldAttributes)。                                                                                                                                                  | ERROR |
|  8.   | 如果 ***`Flags`***.*`RTSpecialName`* = 1，那么 ***`Flags`***.*`SpecialName`* 也应为 1 [[↗]](#FieldAttributes)。                                                                                                                                       | ERROR |
|  9.   | 如果 ***`Flags`***.*`HasFieldMarshal`* = 1，那么此行应 “拥有” **`FieldMarshal`** 表中的确切一行 [[↗]](#FieldAttributes)。                                                                                                                             | ERROR |
|  10.  | 如果 ***`Flags`***.*`HasDefault`* = 1，那么此行应 “拥有” **`Constant`** 表中的确切一行 [[↗]](#FieldAttributes)。                                                                                                                                      | ERROR |
|  11.  | 如果 ***`Flags`***.*`HasFieldRVA`* = 1，那么此行应 “拥有” **`FieldRVA`** 表中的确切一行 [[↗]](#FieldAttributes)。                                                                                                                                     | ERROR |
|  12.  | ***`Name`*** 应索引 ***String Heap*** 中的 non-empty 字符串。                                                                                                                                                                                         | ERROR |
|  13.  | ***`Name`*** 字符串应是一个有效的 CLS 标识符。                                                                                                                                                                                                        |  CLS  |
|  14.  | ***`Signature`*** 应索引 ***Blob Heap*** 中的有效字段签名。                                                                                                                                                                                           | ERROR |
|  15.  | 如果 ***`Flags`***.*`CompilerControlled`* = 1 [[↗]](#FieldAttributes)，那么在重复检查中将完全忽略此行。                                                                                                                                               |       |
|  16.  | 如果此字段的 *owner* 是内部生成的类型 `<Module>`，它表示此字段在模块范围内定义 (通常称为全局变量)。在这种情况下：                                                                                                                                     |       |
|       | * ***`Flags`***.*`Static`* 应为 1。                                                                                                                                                                                                                   | ERROR |
|       | * ***`Flags`***.*`MemberAccessMask`* 子字段应为 *`Public`*、*`CompilerControlled`* 或 *`Private`* 中的一个 [[↗]](#FieldAttributes)。                                                                                                                  | ERROR |
|       | * 不允许使用模块范围 (_module scope_) 字段。                                                                                                                                                                                                          |  CLS  |
|  17.  | 基于 _owner_+***`Name`***+***`Signature`***，**`Field`** 表中不应有重复的行，其中 _owner_ 是在 **`TypeDef`** 表中的 _owner_ 行。但如果 ***`Flags`***.*`CompilerControlled`* = 1，那么完全排除此行的重复检查。                                         | ERROR |
|  18.  | 基于 _owner_+***`Name`***，**`Field`** 表中不应有重复的行，其中 ***`Name`*** 字段使用 CLS 冲突标识符规则进行比较。所以例如，"`int i`" 和 "`float i`" 将被视为 CLS 重复。但如果 ***`Flags`***.*`CompilerControlled`* = 1，那么完全排除此行在重复检查。 |  CLS  |
|  19.  | 如果这是一个枚举的字段，那么：                                                                                                                                                                                                                        |       |
|       | * **`TypeDef`** 表中的 *owner* 行应直接派生自 `System.Enum`。                                                                                                                                                                                         | ERROR |
|       | * **`TypeDef`** 表中的 *owner* 行不应有其他实例字段。                                                                                                                                                                                                 |  CLS  |
|       | * 其 ***`Signature`*** 应为 *`ELEMENT_TYPE_U1`*、*`ELEMENT_TYPE_I2`*、*`ELEMENT_TYPE_I4`* 或 *`ELEMENT_TYPE_I8`* 中的一个，参见 [_ELEMENT_TYPE_](#ELEMENT_TYPE)。                                                                                     |  CLS  |
|  20.  | 其 ***`Signature`*** 应为整型。                                                                                                                                                                                                                       | ERROR |

>---
### 20.26. MethodDef: 0x06
<a id="MethodDef_0x06"></a>

> *MethodDef* 表有以下列：

| Column                                       |  Kind   | Size  | Value                  | Description                                                                                                                                                                                             | Link                                            |
| :------------------------------------------- | :-----: | :---: | :--------------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | :---------------------------------------------- |
| ***`Token`***                                | Literal |   4   | 06UUUUUU               | 行编号，高位字节表示表编号，低三位字节是行编号。                                                                                                                                                        |                                                 |
| ***`Flags`***<br>/***`Attributes`***         | BitMask |   2   | _MethodAttributes_     | _MethodAttributes_ 类型的位掩码。                                                                                                                                                                       | [_MethodAttributes_](#MethodAttributes)         |
| ***`ImplFlags`***<br>/***`ImplAttributes`*** | BitMask |   2   | _MethodImplAttributes_ | _MethodImplAttributes_ 类型的位掩码。                                                                                                                                                                   | [_MethodImplAttributes_](#MethodImplAttributes) |
| ***`RVA`***                                  | Literal |   4   | Offset                 | 相对虚拟地址。                                                                                                                                                                                          |                                                 |
| ***`Name`***                                 |  Index  |   4   | ***String Heap***      | 索引方法的名称标识。                                                                                                                                                                                    |                                                 |
| ***`Signature`***                            |  Index  |   4   | ***Blob Heap***        | 索引方法的签名。                                                                                                                                                                                        |                                                 |
| ***`ParamList`***                            |  Index  |   4   | **`Param`**            | 标记了由此方法拥有的一连串参数的第一个。该连续运行继续到以下较小的：<br>&emsp;i. **`Param`** 表的最后一行。<br>&emsp;ii. 下一个参数运行，通过检查 **`MethodDef`** 表中下一行的 ***`ParamList`*** 找到。 |                                                 |

从概念上讲，**`MethodDef`** 表中的每一行都由 **`TypeDef`** 表中的一行拥有 (有且只有一行)。

**`MethodDef`** 表中的行是 **.method** 指令的结果 [[↗]](#method)。当发出 PE 文件的映像时，计算 ***`RVA`*** 列，并指向方法体的 *`COR_ILMETHOD`* 结构 [[↗]](#CIL-physical-layout)。 

如果 ***`Signature`*** 是 *`GENERIC`* (0x10)，则在 **`GenericParam`** 表 [[↗]](#GenericParam_0x2A) 中描述泛型参数。

> 元数据验证规则

| Order | Validation Rule                                                                                                                                                                                                                                                                                                                            | Level |
| :---: | :----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :---: |
|  1.   | **`MethodDef`** 表可以包含零行或多行                                                                                                                                                                                                                                                                                                       |
|  2.   | 在 **`TypeDef`** 表中的 *owner* 行，每一行应该有一个 ***`MethodList`***，且只有一个。                                                                                                                                                                                                                                                      | ERROR |
|  3.   | ***`ImplFlags`*** 只应设置那些指定的值。                                                                                                                                                                                                                                                                                                   | ERROR |
|  4.   | ***`Flags`*** 只应设置那些指定的值。                                                                                                                                                                                                                                                                                                       | ERROR |
|  5.   | 如果 ***`Name`*** 是 `.ctor` 并且方法被标记为 `SpecialName`，那么在 **`GenericParam`** 表中不应该有一行将此 **`MethodDef`** 作为其 *owner*。                                                                                                                                                                                               | ERROR |
|  6.   | ***`Flags`*** 的 *`MemberAccessMask`* 子字段 [[↗]](#MethodAttributes) 应该包含 *`CompilerControlled`*、*`Private`*、*`FamANDAssem`*、*`Assem`*、*`Family`*、*`FamORAssem`* 或 *`Public`* 中的一个。                                                                                                                                        | ERROR |
|  7.   | ***`Flags`*** 中的以下组合位设置是无效的。                                                                                                                                                                                                                                                                                                 | ERROR |
|       | 1. *`Static`* \| *`Final`*                                                                                                                                                                                                                                                                                                                 |       |
|       | 2. *`Static`* \| *`Virtual`*，除了接口中的静态虚或抽象方法                                                                                                                                                                                                                                                                                 |
|       | 3. *`Static`* \| *`NewSlot`*                                                                                                                                                                                                                                                                                                               |
|       | 4. *`Final`* \| *`Abstract`*                                                                                                                                                                                                                                                                                                               |
|       | 5. *`Abstract`* \| *`PinvokeImpl`*                                                                                                                                                                                                                                                                                                         |
|       | 6. *`CompilerControlled`* \| *`SpecialName`*                                                                                                                                                                                                                                                                                               |
|       | 7. *`CompilerControlled`* \| *`RTSpecialName`*                                                                                                                                                                                                                                                                                             |
|  8.   | 抽象方法应该是虚方法。所以，如果 ***`Flags`***.*`Abstract`* = 1 那么 ***`Flags`***.*`Virtual`* 也应该是 1。                                                                                                                                                                                                                                | ERROR |
|  9.   | 如果 ***`Flags`***.*`RTSpecialName`* = 1 那么 ***`Flags`***.*`SpecialName`* 也应该是 1。                                                                                                                                                                                                                                                   | ERROR |
|  10.  | 如果 ***`Flags`***.*`HasSecurity`* = 1，那么以下条件中至少有一个应该为真：                                                                                                                                                                                                                                                                 | ERROR |
|       | * 此方法拥有 **`DeclSecurity`** 表中的至少一行。                                                                                                                                                                                                                                                                                           |
|       | * 此方法具有名为 `SuppressUnmanagedCodeSecurityAttribute` 的自定义特性。                                                                                                                                                                                                                                                                   |
|  11.  | 如果此方法拥有 **`DeclSecurity`** 表中的一行 (或多行) 那么 ***`Flags`***.*`HasSecurity`* 应该是 1。                                                                                                                                                                                                                                        | ERROR |
|  12.  | 如果此方法具有名为 `SuppressUnmanagedCodeSecurityAttribute` 的自定义特性那么 ***`Flags`***.*`HasSecurity`* 应该是 1。                                                                                                                                                                                                                      | ERROR |
|  13.  | 可以具有名为 `DynamicSecurityMethodAttribute` 的自定义特性，但这对其 ***`Flags`***.*`HasSecurity`* 的值没有任何影响。                                                                                                                                                                                                                      |
|  14.  | ***`Name`*** 应索引 ***String Heap*** 中的 non-empty 字符串。                                                                                                                                                                                                                                                                              | ERROR |
|  15.  | 接口不能有实例构造函数。所以，如果这个方法是由接口拥有的，那么它的 ***`Name`*** 不能是 `.ctor`。                                                                                                                                                                                                                                           | ERROR |
|  16.  | ***`Name`*** 字符串应是一个有效的 CLS 标识符 (除非设置了 ***`Flags`***.*`RTSpecialName`* — 例如，`.cctor` 是有效的)。                                                                                                                                                                                                                      |  CLS  |
|  17.  | ***`Signature`*** 应索引 ***Blob Heap*** 中的有效方法签名。                                                                                                                                                                                                                                                                                | ERROR |
|  18.  | 如果 ***`Flags`***.*`CompilerControlled`* = 1，那么在重复检查中完全忽略此行。                                                                                                                                                                                                                                                              |
|  19.  | 如果此方法的 *owner* 是内部生成的类型 `<Module>`，它表示此方法在模块范围内定义。在 C++ 中，该方法被称为全局方法，只能在其编译单元内从其声明点向前引用。在这种情况下：                                                                                                                                                                      |
|       | 1. ***`Flags`***.*`Static`* 应为 1。                                                                                                                                                                                                                                                                                                       | ERROR |
|       | 2. ***`Flags`***.*`Abstract`* 应为 0。                                                                                                                                                                                                                                                                                                     | ERROR |
|       | 3. ***`Flags`***.*`Virtual`* 应为 0。                                                                                                                                                                                                                                                                                                      | ERROR |
|       | 4. ***`Flags`***.*`MemberAccessMask`* 子字段应为 *`CompilerControlled`*、*`Public`* 或 *`Private`* 中的一个。                                                                                                                                                                                                                              | ERROR |
|       | 5. 不允许模块范围方法。                                                                                                                                                                                                                                                                                                                    |  CLS  |
|  20.  | 对于没有身份 (**_identity_**) 的值类型，具有同步方法是没有意义的 (除非它们被装箱)。所以，如果此方法的 *owner* 是一个值类型，那么该方法不能是同步的，即 ***`ImplFlags`***.*`Synchronized`* 应为 0。                                                                                                                                         | ERROR |
|  21.  | 在 **`MethodDef`** 表中，基于 *owner*+***`Name`***+***`Signature`***，不应有重复的行 (其中 *owner* 是在 **`TypeDef`** 表中的拥有行)。注意，***`Signature`*** 编码了方法是否是泛型，对于泛型方法，它编码了泛型参数的数量。如果 ***`Flags`***.*`CompilerControlled`* = 1，那么在重复检查中完全忽略此行。                                     | ERROR |
|  22.  | 在 **`MethodDef`** 表中，基于 *owner*+***`Name`***+***`Signature`***，不应有重复的行，其中 ***`Name`*** 字段使用 CLS 冲突标识符规则进行比较；此外，签名中定义的类型应该是不同的。所以例如，`int i` 和 `float i` 将被视为 CLS 重复；此外忽略了方法的返回类型。如果 ***`Flags`***.*`CompilerControlled`* = 1，那么在重复检查中完全忽略此行。 |  CLS  |
|  23.  | 如果在 ***`Flags`*** 中设置了 *`Final`*、*`NewSlot`* 或 *`Strict`*，那么也应设置 ***`Flags`***.*`Virtual`*。                                                                                                                                                                                                                               | ERROR |
|  24.  | 如果设置了 ***`Flags`***.*`PInvokeImpl`*，那么 ***`Flags`***.*`Virtual`* 应为 0。                                                                                                                                                                                                                                                          | ERROR |
|  25.  | 如果 ***`Flags`***.*`Abstract`* &ne; 1，那么以下条件中必须有一个也为真：                                                                                                                                                                                                                                                                   | ERROR |
|       | * RVA &ne; 0                                                                                                                                                                                                                                                                                                                               |
|       | * ***`Flags`***.*`PInvokeImpl`* = 1                                                                                                                                                                                                                                                                                                        |
|       | * ***`ImplFlags`***.*`Runtime`* = 1                                                                                                                                                                                                                                                                                                        |
|  26.  | 如果方法是 *`CompilerControlled`*，那么 RVA 应为非零或标记为 *`PinvokeImpl`* = 1。                                                                                                                                                                                                                                                         | ERROR |
|  27.  | ***`Signature`*** 应具有以下托管调用约定中的一个：                                                                                                                                                                                                                                                                                         | ERROR |
|       | 1. *`DEFAULT`* (0x0)                                                                                                                                                                                                                                                                                                                       |
|       | 2. *`VARARG`* (0x5)                                                                                                                                                                                                                                                                                                                        |
|       | 3. *`GENERIC`* (0x10)                                                                                                                                                                                                                                                                                                                      |
|  28.  | ***`Signature`*** 应具有调用约定 *`DEFAULT`* (0x0) 或 *`GENERIC`* (0x10)。                                                                                                                                                                                                                                                                 |  CLS  |
|  29.  | ***`Signature`***：当且仅当方法不是 *`Static`* 时，***`Signature`*** 中的调用约定字节的 *`HASTHIS`* (0x20) 位应被设置。                                                                                                                                                                                                                    | ERROR |
|  30.  | ***`Signature`***：如果方法是 *`Static`*，那么调用约定中的 *`HASTHIS`* (0x20) 位应为0 。                                                                                                                                                                                                                                                   | ERROR |
|  31.  | 如果签名中的 *`EXPLICITTHIS`* (0x40) 被设置，那么 *`HASTHIS`* (0x20) 也应被设置 (注意，如果设置了 *`EXPLICITTHIS`*，那么代码是不可验证的)。                                                                                                                                                                                                | ERROR |
|  32.  | *`EXPLICITTHIS`* (0x40) 位只能在函数指针的签名中设置，它的 *MethodDefSig* 前面有 *`FNPTR`* (0x1B) 的签名。                                                                                                                                                                                                                                 | ERROR |
|  33.  | 如果 ***`RVA`*** = 0，那么以下条件之一必须为真：                                                                                                                                                                                                                                                                                           | ERROR |
|       | * ***`Flags`***.*`Abstract`* = 1。                                                                                                                                                                                                                                                                                                         |
|       | * ***`ImplFlags`***.*`Runtime`* = 1。                                                                                                                                                                                                                                                                                                      |
|       | * ***`Flags`***.*`PinvokeImpl`* = 1。                                                                                                                                                                                                                                                                                                      |
|  34.  | 如果 ***`RVA`*** ≠ 0，那么：                                                                                                                                                                                                                                                                                                               | ERROR |
|       | 1. ***`Flags`***.*`Abstract`* 应为 0，并且                                                                                                                                                                                                                                                                                                 |
|       | 2. ***`ImplFlags`***.*`CodeTypeMask`* 应具有以下值之一：*`Native`*，*`CIL`* 或 *`Runtime`*，并且                                                                                                                                                                                                                                           |
|       | 3. ***`RVA`*** 应指向此文件中的 CIL 代码流                                                                                                                                                                                                                                                                                                 |
|  35.  | 如果 ***`Flags`***.*`PinvokeImpl`* = 1 那么。                                                                                                                                                                                                                                                                                              | ERROR |
|       | * ***`RVA`*** = 0 并且方法在 **`ImplMap`** 表中拥有一行                                                                                                                                                                                                                                                                                    |
|  36.  | 如果 ***`Flags`***.*`RTSpecialName`* = 1 那么 ***`Name`*** 应为以下之一：                                                                                                                                                                                                                                                                  | ERROR |
|       | 1. `.ctor` (一个对象构造器方法)                                                                                                                                                                                                                                                                                                            |
|       | 2. `.cctor` (一个类构造器方法)                                                                                                                                                                                                                                                                                                             |
|  37.  | 相反，如果 ***`Name`*** 是上述特殊名称中的任何一个，那么 ***`Flags`***.*`RTSpecialName`* 应被设置。                                                                                                                                                                                                                                        | ERROR |
|  38.  | 如果 ***`Name`*** = `.ctor` (一个对象构造器方法) 那么：                                                                                                                                                                                                                                                                                    |
|       | 1. ***`Signature`*** 中的返回类型应为 *`ELEMENT_TYPE_VOID`* (参见 [_ELEMENT_TYPE_](#ELEMENT_TYPE))。                                                                                                                                                                                                                                       | ERROR |
|       | 2. ***`Flags`***.*`Static`* 应为 0。                                                                                                                                                                                                                                                                                                       | ERROR |
|       | 3. ***`Flags`***.*`Abstract`* 应为 0。                                                                                                                                                                                                                                                                                                     | ERROR |
|       | 4. ***`Flags`***.*`Virtual`* 应为 0。                                                                                                                                                                                                                                                                                                      | ERROR |
|       | 5. "*owner*" 类型应为 **`TypeDef`** 表中的有效类或值类型 (不是 `<Module>` 且不是接口)。                                                                                                                                                                                                                                                    | ERROR |
|       | 6. 对于任何给定的 "*owner*"，可以有零个或多个 `.ctor`。                                                                                                                                                                                                                                                                                    |
|  39.  | 如果 ***`Name`*** = `.cctor` (一个类构造器方法) 那么：                                                                                                                                                                                                                                                                                     |
|       | 1. ***`Signature`*** 中的返回类型应为 *`ELEMENT_TYPE_VOID`* (参见 [_ELEMENT_TYPE_](#ELEMENT_TYPE))。                                                                                                                                                                                                                                       | ERROR |
|       | 2. ***`Signature`*** 的调用约定应为 *`DEFAULT`* (0x0)。                                                                                                                                                                                                                                                                                    | ERROR |
|       | 3. ***`Signature`*** 中不应提供参数。                                                                                                                                                                                                                                                                                                      | ERROR |
|       | 4. ***`Flags`***.*`Static`* 应被设置。                                                                                                                                                                                                                                                                                                     | ERROR |
|       | 5. ***`Flags`***.*`Virtual`* 应被清除。                                                                                                                                                                                                                                                                                                    | ERROR |
|       | 6. ***`Flags`***.*`Abstract`* 应被清除。                                                                                                                                                                                                                                                                                                   | ERROR |
|  40.  | 在 **`TypeDef`** 表中的任何给定行拥有的方法集合中，只能有 0 或 1 个名为 `.cctor` 的方法。                                                                                                                                                                                                                                                  | ERROR |
 
>--- 
### 20.33. Param: 0x08
<a id="Param_0x08"></a>

> *Param* 表有以下列：

| Column                               |  Kind   | Size  | Value             | Description                                      | Link                                  |
| :----------------------------------- | :-----: | :---: | :---------------- | :----------------------------------------------- | :------------------------------------ |
| ***`Token`***                        | Literal |   4   | 08UUUUUU          | 行编号，高位字节表示表编号，低三位字节是行编号。 |                                       |
| ***`Flags`***<br>/***`Attributes`*** | BitMask |   2   | _ParamAttributes_ | _ParamAttributes_ 类型的位掩码。                 | [_ParamAttributes_](#ParamAttributes) |
| ***`Sequence`***                     | Literal |   2   | Number            | 参数的签名类型                                   |                                       |
| ***`Name`***                         |  Index  |   4   | ***String Heap*** | 索引参数的名称                                   |                                       |

从概念上讲，**`Param`** 表中的每一行都由 **`MethodDef`** 表中的一行拥有，且只有一行。

**`Param`** 表中的行是方法声明中的参数 [[↗]](#MethodHeader)，或者是附加到方法的 **.param** 特性 [[↗]](#param) 的结果。

> 元数据验证规则

| Order | Validation Rule                                                                                                                                            |  Level  |
| :---: | :--------------------------------------------------------------------------------------------------------------------------------------------------------- | :-----: |
|  1.   | **`Param`** 表可以包含零行或多行。                                                                                                                         |
|  2.   | 在 **`MethodDef`** 表中的 *owner* 行，每一行应该有一个 ***`ParamList`***，且只有一个。                                                                     |  ERROR  |
|  3.   | ***`Flags`*** 只应设置那些指定的值 (所有组合有效)。                                                                                                        |  ERROR  |
|  4.   | ***`Sequence`*** 应该有一个值 &ge; 0 并且 &le;  *owner* 方法中的参数数量。***`Sequence`*** 值为 0 指的是 *owner* 方法的返回类型；然后从 1 开始编号其参数。 |  ERROR  |
|  5.   | 由同一方法拥有的 **`Param`** 表的连续行应该按照增加的 ***`Sequence`*** 值排序 —— 尽管序列中允许有间隙。                                                    | WARNING |
|  6.   | 如果 ***`Flags`***.*`HasDefault`* = 1，那么此行应该在 **`Constant`** 表中只拥有一行。                                                                      |  ERROR  |
|  7.   | 如果 ***`Flags`***.*`HasDefault`* = 0，那么在 **`Constant`** 表中没有属于这一行的行。                                                                      |  ERROR  |
|  8.   | 如果 ***`Flags`***.*`FieldMarshal`* = 1，那么此行应该在 **`FieldMarshal`** 表中只拥有一行。                                                                |  ERROR  |
|  9.   | ***`Name`*** 可以为 null 或 non-null。                                                                                                                     |
|  10.  | 如果 ***`Name`*** 是非空的，那么它应该索引 ***String Heap*** 中的 non-empty 字符串。                                                                       | WARNING |

>---
### 20.23. InterfaceImpl: 0x09
<a id="InterfaceImpl_0x09"></a>

> *InterfaceImpl* 表有以下列：

| Column            |  Kind   | Size  | Value         | Description                                            | Link                               |
| :---------------- | :-----: | :---: | :------------ | :----------------------------------------------------- | :--------------------------------- |
| ***`Token`***     | Literal |   4   | 09UUUUUU      | 行编号，高位字节表示表编号，低三位字节是行编号。       |                                    |
| ***`Class`***     |  Index  |       | **`TypeDef`** |                                                        |                                    |
| ***`Interface`*** |  Index  |       | Tables        | 索引 **`TypeDef`**，**`TypeRef`** 或 **`TypeSpec`** 表 | [_TypeDefOrRef_](#TypeDefOrRef) |

**`InterfaceImpl`** 表记录类型显式实现的接口。从概念上讲，**`InterfaceImpl`** 表中的每一行都表示 ***`Class`*** 实现了 ***`Interface`***。

> 元数据验证规则

| Order | Validation Rule                                                                                                                                   |  Level  |
| :---: | :------------------------------------------------------------------------------------------------------------------------------------------------ | :-----: |
|  1.   | **`InterfaceImpl`** 表可以包含零行或多行。                                                                                                        |
|  2.   | ***`Class`*** 应为非 null。                                                                                                                       |  ERROR  |
|  3.   | 如果 ***`Class`*** 为非 null，则：                                                                                                                |
|       | 1. ***`Class`*** 应索引 **`TypeDef`** 表中的有效行。                                                                                              |  ERROR  |
|       | 2. ***`Interface`*** 应索引 **`TypeDef`** 或 **`TypeRef`** 表中的有效行。                                                                         |  ERROR  |
|       | 3. ***`Interface`*** 索引的 **`TypeDef`**，**`TypeRef`** 或 **`TypeSpec`** 表中的行应为接口 (***`Flags`***.*`Interface`* = 1)，而不是类或值类型。 |  ERROR  |
|  4.   | 基于非 null 的 ***`Class`*** 和 ***`Interface`*** 值，在 **`InterfaceImpl`** 表中不应有重复项。                                                   | WARNING |
|  5.   | 可以有许多行具有相同的 ***`Class`*** 值 (因为一个类可以实现许多接口)。                                                                            |
|  6.   | 可以有许多行具有相同的 ***`Interface`*** 值 (因为许多类可以实现相同的接口)。                                                                      |


### 20.25. MemberRef: 0x0A
<a id="MemberRef_0x0A"></a>

> *MemberRef* 表有以下列：

| Column                           |  Kind   | Size  | Value             | Description                                                                                | Link                                  |
| :------------------------------- | :-----: | :---: | :---------------- | :----------------------------------------------------------------------------------------- | :------------------------------------ |
| ***`Token`***                    | Literal |   4   | 0AUUUUUU          | 行编号，高位字节表示表编号，低三位字节是行编号。                                           |                                       |
| ***`Class`***<br>/***`Parent`*** |  Index  |   2   | Tables            | 索引 **`MethodDef`**，**`ModuleRef`**，**`TypeDef`**，**`TypeRef`** 或 **`TypeSpec`** 表。 | [_MemberRefParent_](#MemberRefParent) |
| ***`Name`***                     |  Index  |   4   | ***String Heap*** | 索引成员的名称                                                                             |                                       |
| ***`Signature`***                |  Index  |   4   | ***Blob Heap***   | 索引成员的签名                                                                             |                                       |

**`MemberRef`** 表将对类的方法和字段的两种引用合并在一起，分别称为 "**`MethodRef`**" 和 "**`FieldRef`**"。

每当在 CIL 代码中对在另一个模块或程序集中定义的方法或字段进行引用时，都会在 **`MemberRef`** 表中创建一个条目。此外，即使在与调用点相同的模块中定义了具有 *`VARARG`* 签名的方法，也会为其创建一个条目。

> 元数据验证规则

| Order | Validation Rule                                                                                                                                                                                                                                                                                                                                                            |  Level  |
| :---: | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :-----: |
|  1.   | ***`Class`*** 应为以下之一：                                                                                                                                                                                                                                                                                                                                               |  ERROR  |
|       | 1. 如果定义成员的类在另一个模块中定义，则为 **`TypeRef`**  **_token_**。当成员在此相同的模块中定义时，使用 **`TypeRef`**  **_token_** 是不寻常但有效的，在这种情况下，可以使用其 **`TypeDef`**  **_token_** 代替。                                                                                                                                                         |
|       | 2. 如果成员在同一程序集的另一个模块中定义为全局函数或变量，则为 **`ModuleRef`**  **_token_**。                                                                                                                                                                                                                                                                             |
|       | 3. 当用于为在此模块中定义的 **vararg** 方法提供调用点签名时，为 **`MethodDef`**  **_token_**。***`Name`*** 应与相应 **`MethodDef`** 行中的 ***`Name`*** 匹配。***`Signature`*** 应与目标方法定义中的 ***`Signature`*** 匹配。                                                                                                                                              |  ERROR  |
|       | 4. 如果成员是泛型类型的成员，则为 **`TypeSpec`**  **_token_**。                                                                                                                                                                                                                                                                                                            |
|  2.   | ***`Class`*** 不应为 null (因为这将表示对全局函数或变量的未解析引用)。                                                                                                                                                                                                                                                                                                     |  ERROR  |
|  3.   | ***`Name`*** 应索引 ***String Heap*** 中的 non-empty 字符串。                                                                                                                                                                                                                                                                                                              |  ERROR  |
|  4.   | ***`Name`*** 字符串应为有效的 CLS 标识符。                                                                                                                                                                                                                                                                                                                                 |   CLS   |
|  5.   | ***`Signature`*** 应索引 ***Blob Heap*** 中的有效字段或方法签名。特别是，它应嵌入以下调用约定中的一个：                                                                                                                                                                                                                                                                    |  ERROR  |
|       | 1. *`DEFAULT`* (0x0)                                                                                                                                                                                                                                                                                                                                                       |
|       | 2. *`VARARG`* (0x5)                                                                                                                                                                                                                                                                                                                                                        |
|       | 3. *`FIELD`* (0x6)                                                                                                                                                                                                                                                                                                                                                         |
|       | 4. *`GENERIC`* (0x10)                                                                                                                                                                                                                                                                                                                                                      |
|  6.   | **`MemberRef`** 表应不包含重复项，其中重复行具有相同的 ***`Class`***，***`Name`*** 和 ***`Signature`***。                                                                                                                                                                                                                                                                  | WARNING |
|  7.   | ***`Signature`*** 不应具有 *`VARARG`* (0x5) 调用约定。                                                                                                                                                                                                                                                                                                                     |   CLS   |
|  8.   | 不应有重复行，其中 ***`Name`*** 字段使用 CLS 冲突标识符规则进行比较。特别是，CLS 中忽略了返回类型以及参数是否标记为 *`ELEMENT_TYPE_BYREF`* (参见 [_ELEMENT_TYPE_](#ELEMENT_TYPE))。例如，方法 `.method int32 M()` 和 `.method float64 M()` 根据根据 CLS 规则会产生重复行。同样，方法 `.method void N(int32 i)` 和 `.method void N(int32& i)` 也根据 CLS 规则会产生重复行。 |   CLS   |
|  9.   | 如果 ***`Class`*** 和 ***`Name`*** 解析为字段，则该字段的 ***`Flags`***.*`FieldAccessMask`* 子字段中不应有 *`CompilerControlled`* (参见 [_FieldAttributes_](#FieldAttributes)) 的值。                                                                                                                                                                                      |  ERROR  |
|  10.  | 如果 ***`Class`*** 和 ***`Name`*** 解析为方法，则该方法的 ***`Flags`***.*`MemberAccessMask`* (参见 [_MethodAttributes_](#MethodAttributes)) 子字段中不应有 *`CompilerControlled`* 的值。                                                                                                                                                                                   |  ERROR  |
|  11.  | 包含 **`MemberRef`** 定义的类型应为表示实例化类型的 **`TypeSpec`**。                                                                                                                                                                                                                                                                                                       |

>---
### 20.9. Constant: 0x0B
<a id="Constant_0x0B"></a>

> *Constant* 表有以下列：

| Column         |  Kind   | Size  | Value           | Description                                                                                                                                                                                                                                                                                         | Link                              |
| :------------- | :-----: | :---: | :-------------- | :-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :-------------------------------- |
| ***`Token`***  | Literal |   4   | 0BUUUUUU        | 行编号，高位字节表示表编号，低三位字节是行编号。                                                                                                                                                                                                                                                    |                                   |
| ***`Type`***   | Literal |   1   | Number          | 后面跟着一个 1 字节的填充零，参见 [_ELEMENT_TYPE_](#ELEMENT_TYPE)。对于 _ilasm_ 中 _FieldInit_ 的 **nullref** 值 [[↗]](#field-init)，***`Type`*** 的编码是 *`ELEMENT_TYPE_CLASS`*，其 ***`Value`*** 是一个 4 字节的零。与 *`ELEMENT_TYPE_CLASS`* 在签名中的用法不同，这个不是后跟类型 **_token_**。 |                                   |
| ***`Parent`*** |  Index  |   4   | Tables          | 索引 **`Param`**、**`Field`** 或 **`Property`** 表。                                                                                                                                                                                                                                                | [_HasConstant_](#HasConstant) |
| ***`Value`***  |  Index  |   4   | ***Blob Heap*** | 索引常数的字面值。                                                                                                                                                                                                                                                                                  |                                   |

**`Constant`** 表用于存储字段、参数和属性的编译时常量值。

请注意，**`Constant`** 信息并不直接影响运行时行为，尽管它可以通过反射可见 (因此可以用来实现像 `System.Enum.ToString` 这样的功能)。编译器在导入元数据时，在编译时检查这些信息，但如果使用了常量本身的值，它将嵌入到编译器发出的 CIL 流中。在运行时，没有 CIL 指令可以访问 **`Constant`** 表。

每当为父项指定编译时值时，都会在 **`Constant`** 表中为父项创建一行。有关示例，请参见 [[↗]](#field-init)。

> 元数据验证规则

| Order | Validation Rule                                                                                                                                                                                                                                                                                                                                                                                                             | Level |
| :---: | :-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :---: |
|  1.   | ***`Type`*** 应该正好是以下之一：*`ELEMENT_TYPE_BOOLEAN`*，*`ELEMENT_TYPE_CHAR`*，*`ELEMENT_TYPE_I1`*，*`ELEMENT_TYPE_U1`*，*`ELEMENT_TYPE_I2`*，*`ELEMENT_TYPE_U2`*，*`ELEMENT_TYPE_I4`*，*`ELEMENT_TYPE_U4`*，*`ELEMENT_TYPE_I8`*，*`ELEMENT_TYPE_U8`*，*`ELEMENT_TYPE_R4`*，*`ELEMENT_TYPE_R8`*，或 *`ELEMENT_TYPE_STRING`*；或者 *`ELEMENT_TYPE_CLASS`*，其 ***`Value`*** 为零 (参见 [_ELEMENT_TYPE_](#ELEMENT_TYPE))。 | ERROR |
|  2.   | ***`Type`*** 不应该是任何以下的：*`ELEMENT_TYPE_I1`*，*`ELEMENT_TYPE_U2`*，*`ELEMENT_TYPE_U4`*，或 *`ELEMENT_TYPE_U8`* (参见 [_ELEMENT_TYPE_](#ELEMENT_TYPE))。                                                                                                                                                                                                                                                             |  CLS  |
|  3.   | ***`Parent`*** 应该索引 **`Field`**、**`Property`** 或 **`Param`** 表中的有效行。                                                                                                                                                                                                                                                                                                                                           | ERROR |
|  4.   | 基于 ***`Parent`***，不应有重复的行。                                                                                                                                                                                                                                                                                                                                                                                       | ERROR |
|  5.   | ***`Type`*** 应该完全匹配由 ***`Parent`*** 标识的 **`Param`**、**`Field`** 或 **`Property`** 的声明类型 (在父项是枚举的情况下，它应该完全匹配该枚举的底层类型)。                                                                                                                                                                                                                                                            |  CLS  |


>---
### 20.10. CustomAttribute: 0x0C
<a id="CustomAttribute_0x0C"></a>

> *CustomAttribute* 表有以下列：

| Column                               |  Kind   | Size  | Value                | Description                                          | Link                                      |
| :----------------------------------- | :-----: | :---: | :------------------- | :--------------------------------------------------- | :---------------------------------------- |
| ***`Token`***                        | Literal |   4   | 0CUUUUUU             | 行编号，高位字节表示表编号，低三位字节是行编号。     |                                           |
| ***`Parent`***                       |  Index  |   4   | _HasCustomAttribute_ | 索引与 _HasCustomAttribute_ 编码索引关联的元数据表。 | [_HasCustomAttribute_](#HasCustomAttribute)  |
| ***`Type`***<br>/***`Constructor`*** |  Index  |   4   | Tables               | 索引 **`MethodDef`** 或 **`MemberRef`** 表。         | [_CustomAttributeType_](#CustomAttributeType) |
| ***`Value`***                        |  Index  |   4   | ***Blob Heap***      | 索引构建该特性实例所需要的二进制数据。               |                                           |

**`CustomAttribute`** 表存储的数据可以在运行时用来实例化自定义特性 (更准确地说，是指定的自定义特性类的对象)。名为 ***`Type`*** 的列实际上索引了一个构造方法 —— 该构造方法的 *owner* 是自定义特性的类型。

在 **`CustomAttribute`** 表中为父项创建的行由 **.custom** 特性创建，它给出了 ***`Type`*** 列的值，以及可选的 ***`Value`*** 列的值。参见 [_自定义特性_](#custom)。 

> 元数据验证规则

所有二进制值都以小端格式存储 (除了 *`PackedLen`* 项，它们仅用作后续 UTF8 字符串中字节数的计数)。

| Order | Validation Rule                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           | Level |
| :---: | :-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :---: |
|  1.   | 若不需要 **`CustomAttribute`**，那么 ***`Value`*** 可以为空。                                                                                                                                                                                                                                                                                                                                                                                                                                             |
|  2.   | ***`Parent`*** 可以是任何元数据表的索引，除了 **`CustomAttribute`** 表本身。                                                                                                                                                                                                                                                                                                                                                                                                                              | ERROR |
|  3.   | ***`Type`*** 应索引 **`MethodDef`** 或 **`MemberRef`** 表中的有效行。该行应该是一个构造方法 (对于这个信息形成实例的类)。                                                                                                                                                                                                                                                                                                                                                                                  | ERROR |
|  4.   | ***`Value`*** 可以为空或非空。                                                                                                                                                                                                                                                                                                                                                                                                                                                                            |
|  5.   | 如果 ***`Value`*** 是非空的，它应该索引 ***Blob Heap*** 中的一个 *blob*。                                                                                                                                                                                                                                                                                                                                                                                                                                 | ERROR |
|  6.   | 以下规则适用于 ***`Value`*** *blob* 的整体结构 [[↗]](#custom-attr-value)：                                                                                                                                                                                                                                                                                                                                                                                                                                |
|       | * _Prolog_ 应该是 0x0001。                                                                                                                                                                                                                                                                                                                                                                                                                                                                                | ERROR |
|       | * _FixedArg_ 出现次数应该与 _Constructor_ 方法中声明的参数一样多。                                                                                                                                                                                                                                                                                                                                                                                                                                        | ERROR |
|       | * _NumNamed_ 可以是零或更多。                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
|       | * 应该有恰好 _NumNamed_ 个 _NamedArg_ 出现。                                                                                                                                                                                                                                                                                                                                                                                                                                                              | ERROR |
|       | * 每个 _NamedArg_ 应该可以被调用方访问。                                                                                                                                                                                                                                                                                                                                                                                                                                                                  | ERROR |
|       | * 如果 _NumNamed_ = 0，那么 _CustomAttrib_ 中不应该有更多的项。                                                                                                                                                                                                                                                                                                                                                                                                                                           | ERROR |
|  7.   | 以下规则适用于 _FixedArg_ 的结构 [[↗]](#custom-attr-value)：                                                                                                                                                                                                                                                                                                                                                                                                                                              |
|       | * 如果此项不是向量 (单维数组，下界为 0)，那么应该只有一个 _Elem_。                                                                                                                                                                                                                                                                                                                                                                                                                                        | ERROR |
|       | * 如果此项是向量，那么：                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  |
|       | * _NumElem_ 应该是 1 或更多。                                                                                                                                                                                                                                                                                                                                                                                                                                                                             | ERROR |
|       | * 这后面应该有 _NumElem_ 个 _Elem_ 出现。                                                                                                                                                                                                                                                                                                                                                                                                                                                                 | ERROR |
|  8.   | 以下规则适用于 _Elem_ 的结构 [[↗]](#custom-attr-value)：                                                                                                                                                                                                                                                                                                                                                                                                                                                  |
|       | * 如果这是一个简单类型或枚举 (参见 [[↗]](#custom-attr-value) 了解如何定义)，那么 _Elem_ 只包含它的值。                                                                                                                                                                                                                                                                                                                                                                                                    | ERROR |
|       | * 如果这是一个字符串或类型，那么 _Elem_ 包含一个 _SerString_ —— _PackedLen_ 字节计数，后跟 UTF8 字符。                                                                                                                                                                                                                                                                                                                                                                                                    | ERROR |
|       | * 如果这是一个装箱的简单值类型 (`bool`，`char`，`float32`，`float64`，`int8`，`int16`，`int32`，`int64`，`unsigned int8`，`unsigned int16`，`unsigned int32` 或 `unsigned int64`)，那么 *Elem* 包含相应的类型表示符 (*`ELEMENT_TYPE_BOOLEAN`*，*`ELEMENT_TYPE_CHAR`*，*`ELEMENT_TYPE_I1`*，*`ELEMENT_TYPE_U1`*，*`ELEMENT_TYPE_I2`*，*`ELEMENT_TYPE_U2`*，*`ELEMENT_TYPE_I4`*，*`ELEMENT_TYPE_U4`*，*`ELEMENT_TYPE_I8`*，*`ELEMENT_TYPE_U8`*，*`ELEMENT_TYPE_R4`* 或 *`ELEMENT_TYPE_R8`*)，并后跟它的值。 | ERROR |
|  9.   | 以下规则适用于 _NamedArg_ 的结构 [[↗]](#custom-attr-value)：                                                                                                                                                                                                                                                                                                                                                                                                                                              |
|       | * _NamedArg_ 应该以单字节 *`FIELD`* (0x53) 或 *`PROPERTY`* (0x54) 开头，用于标识。                                                                                                                                                                                                                                                                                                                                                                                                                        | ERROR |
|       | * 如果参数种类是装箱的简单值类型，那么字段或属性的类型是 *`ELEMENT_TYPE_BOOLEAN`*，*`ELEMENT_TYPE_CHAR`*，*`ELEMENT_TYPE_I1`*，*`ELEMENT_TYPE_U1`*，*`ELEMENT_TYPE_I2`*，*`ELEMENT_TYPE_U2`*，*`ELEMENT_TYPE_I4`*，*`ELEMENT_TYPE_U4`*，*`ELEMENT_TYPE_I8`*，*`ELEMENT_TYPE_U8`*，*`ELEMENT_TYPE_R4`*，*`ELEMENT_TYPE_R8`*，*`ELEMENT_TYPE_STRING`*，或常数 0x50 (对应于类型为 `System.Type` 的参数) 中的一个。                                                                                           | ERROR |
|       | * 字段或属性的名称，分别与前一项，存储为 _SerString_ —— _PackedLen_ 字节计数，后跟名称的 UTF8 字符。                                                                                                                                                                                                                                                                                                                                                                                                      | ERROR |
|       | * _NamedArg_ 是一个 _FixedArg_ (见上文) 。                                                                                                                                                                                                                                                                                                                                                                                                                                                                | ERROR |

>---
### 20.17. FieldMarshal : 0x0D
<a id="FieldMarshal_0x0D"></a>

**`FieldMarshal`** 表有两列。它将 **`Field`** 或 **`Param`** 表中的现有行 “链接” 到 ***Blob Heap*** 中的信息，该信息定义了该字段或参数 (通常情况下，作为参数编号 0 的方法返回) 在通过 PInvoke 调度调用到 (或从) 非托管代码时应如何进行封送。

请注意，**`FieldMarshal`** 信息仅由与非托管代码进行操作的代码路径使用。为了执行这样的路径，在大多数平台上调用方被安装为具有更高的安全权限。一旦它调用了非托管代码，它就脱离了 CLI 可以检查的范围 —— 它只是被信任不会违反类型系统。

> *FieldMarshal* 表有以下列：

| Column             |  Kind   | Size  | Value           | Description                                      | Link                                  |
| :----------------- | :-----: | :---: | :-------------- | :----------------------------------------------- | :------------------------------------ |
| ***`Token`***      | Literal |   4   | 0DUUUUUU        | 行编号，高位字节表示表编号，低三位字节是行编号。 |                                       |
| ***`Parent`***     |  Index  |   4   | Tables          | 索引 **`Field`** 或 **`Param`** 表。             | [_HasFieldMarshal_](#HasFieldMarshal) |
| ***`NativeType`*** |  Index  |   4   | ***Blob Heap*** | 索引本地类型的二进制数据对象。                   |                                       |

有关 '***blob***' 的详细格式，请参见 [_二进制数据对象_](#blob-description)。

如果父字段的 **.field** 指令指定了 **marshal** 特性 [[↗]](#field-marshal)，则会在 **`FieldMarshal`** 表中创建一行。

> 元数据验证规则

| Order | Validation Rule                                                                                                                                                                          |  Level  |
| :---: | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :-----: |
|  1.   | **`FieldMarshal`** 表可以包含零行或多行。                                                                                                                                                |
|  2.   | ***`Parent`*** 应该索引 **`Field`** 或 **`Param`** 表中的有效行 (***`Parent`*** 值被编码为表示每个索引引用的是这两个表中的哪一个)。                                                      |  ERROR  |
|  3.   | ***`NativeType`*** 应该索引 ***Blob Heap*** 中的非空 '*blob*'。                                                                                                                          |  ERROR  |
|  4.   | 任何两行都不能指向同一个父项。换句话说，在 ***`Parent`*** 值已被解码为确定它们是引用 **`Field`** 表还是 **`Param`** 表之后，没有两行可以指向 **`Field`** 表或 **`Param`** 表中的同一行。 |  ERROR  |
|  5.   | 以下检查适用于 _`MarshalSpec`_ '*blob*'：                                                                                                                                                |
|       | * _`NativeIntrinsic`_ 应该是其生成的常数值之一 [[↗]](#blob-description)。                                                                                                                |  ERROR  |
|       | * 如果是 *`ARRAY`*，那么 *`ArrayElemType`* 应该是其生成的常数值之一。                                                                                                                    |  ERROR  |
|       | * 如果是 *`ARRAY`*，那么 _`ParamNum`_ 可以为零。                                                                                                                                         |
|       | * 如果是 *`ARRAY`*，那么 _`ParamNum`_ 不能小于 0。                                                                                                                                       |  ERROR  |
|       | * 如果是 *`ARRAY`*，并且 _`ParamNum`_ > 0，那么 ***`Parent`*** 应该指向 **`Param`** 表中的一行，而不是 **`Field`** 表。                                                                  |  ERROR  |
|       | * 如果是 *`ARRAY`*，并且 _`ParamNum`_ > 0，那么 _`ParamNum`_ 不能超过父 **`Param`** 是其成员的 **`MethodDef`** (或者如果是 *`VARARG`* 调用，则为 **`MethodRef`**) 提供的参数数量。       |  ERROR  |
|       | * 如果是 *`ARRAY`*，那么 _`ElemMult`_ 应该大于等于 1。                                                                                                                                   |  ERROR  |
|       | * 如果是 *`ARRAY`* 并且 _`ElemMult`_ 不等于 1，则发出警告，因为这可能是一个错误。                                                                                                        | WARNING |
|       | * 如果是 *`ARRAY`* 并且 _`ParamNum`_ = 0，那么 _`NumElem`_ 应该大于等于 1。                                                                                                              |  ERROR  |
|       | * 如果是 *`ARRAY`* 并且 _`ParamNum`_ 不等于 0 并且 _`NumElem`_ 不等于 0，则发出警告，因为这可能是一个错误。                                                                              | WARNING |
     
>---
### 20.11. DeclSecurity: 0x0E
<a id="DeclSecurity_0x0E"></a>

可以将源自 `System.Security.Permissions.SecurityAttribute` (参见第四部分 【】) 的安全特性附加到 **`TypeDef`**、***`Method`*** 或 *Assembly*。此类的所有构造函数都应将 `System.Security.Permissions.SecurityAction` 值作为其第一个参数，描述应对附加到的类型、方法或程序集的权限进行何种操作。代码访问派生自 `System.Security.Permissions.CodeAccessSecurityAttribute` 的安全特性，可以具有任何安全操作。

这些不同的安全操作在 **`DeclSecurity`** 表中被编码为 2 字节的枚举 (见下文)。对于方法、类型或程序集上给定安全操作的所有安全自定义特性应聚集在一起，并创建一个 `System.Security.PermissionSet` 实例，存储在 ***Blob Heap*** 中，并从 **`DeclSecurity`** 表中引用。

从编译器的角度来看，一般流程如下：用户通过某种特定于语言的语法指定自定义特性，该语法编码了对特性的构造函数的调用。如果特性的类型直接或间接派生自 `System.Security.Permissions.SecurityAttribute`，那么它就是一个安全自定义特性，并需要特殊处理，如下所述 (其他自定义特性通过简单地在元数据中记录构造函数来处理，如 [*CustomAttribute*](#CustomAttribute_0x0C) 表所述)。构造特性对象，并提供一个方法 (`CreatePermission`) 将其转换为安全权限对象 (从 `System.Security.Permission` 派生的对象)。所有附加到具有相同安全操作的给定元数据项的权限对象都被组合在一起，形成一个 `System.Security.PermissionSet`。使用其 `ToXML` 方法将此权限集转换为准备存储在 XML 中的形式，以创建 `System.Security.SecurityElement`。最后，使用 `SecurityElement` 上的 `ToString` 方法创建元数据所需的 XML。

> *DeclSecurity* 表有以下列：

| Column                |  Kind   | Size  | Value           | Description                                                | Link                                  |
| :-------------------- | :-----: | :---: | :-------------- | :--------------------------------------------------------- | :------------------------------------ |
| ***`Token`***         | Literal |   4   | 0EUUUUUU        | 行编号，高位字节表示表编号，低三位字节是行编号。           |                                       |
| ***`Parent`***        |  Index  |   4   | Tables          | 索引 **`TypeDef`**、**`MethodDef`** 或 **`Assembly`** 表。 | [_HasDeclSecurity_](#HasDeclSecurity) |
| ***`Action`***        | Literal |   2   | Number          | 表示安全操作类别                                           |                                       |
| ***`PermissionSet`*** |  Index  |   4   | ***Blob Heap*** | 索引它的二进制数据对象。                                   |                                       |

***`Action`*** 是安全操作 (参见 `System.Security.SecurityAction`，第四部分 【】) 的 2 字节表示。值 0 ~ 0xFF 保留供未来标准使用。值 0x20 ~ 0x7F 和 0x100 ~ 0x07FF 用于操作，如果操作不被识别或支持，可以忽略。值 0x80 ~ 0xFF 和 0x0800 ~ 0xFFFF 用于需要执行安全操作的操作；在操作不可用的实现中，不应允许访问程序集、类型或方法。

| 安全操作          | 注释   | 行为解释                                                                           | 有效范围   |
| ----------------- | ------ | ---------------------------------------------------------------------------------- | ---------- |
| Assert            | 1      | 在没有进一步检查的情况下，满足对指定权限的需求。                                   | 方法，类型 |
| Demand            | 1      | 检查调用链中的所有调用者是否已被授予指定的权限，在失败时抛出 `SecurityException`。 | 方法，类型 |
| Deny              | 1      | 在没有进一步检查的情况下，拒绝对指定权限的需求。                                   | 方法，类型 |
| InheritanceDemand | 1      | 为了从类继承或覆盖虚方法，必须授予指定的权限。                                     | 方法，类型 |
| LinkDemand        | 1      | 检查直接调用者是否已被授予指定的权限；在失败时抛出 `SecurityException`。           | 方法，类型 |
| NonCasDemand      | 2      | 检查当前程序集是否已被授予指定的权限；否则抛出 `SecurityException`。               | 方法，类型 |
| NonCasLinkDemand  | 2      | 检查直接调用者是否已被授予指定的权限；否则抛出 `SecurityException`。               | 方法，类型 |
| PrejitGrant       | &nbsp; | 保留供实现特定使用。                                                               | 程序集     |
| PermitOnly        | 1      | 在没有进一步检查的情况下，拒绝对除指定之外的所有权限的需求。                       | 方法，类型 |
| RequestMinimum    | &nbsp; | 指定运行所需的最小权限。                                                           | 程序集     |
| RequestOptional   | &nbsp; | 指定要授予的可选权限。                                                             | 程序集     |
| RequestRefuse     | &nbsp; | 指定不授予的权限。                                                                 | 程序集     |

**注释 1** 指定的特性应派生自 `System.Security.Permissions.CodeAccessSecurityAttribute`。

**注释 2** 指定的特性应派生自 `System.Security.Permissions.SecurityAttribute`，但不应派生自 `System.Security.Permissions.CodeAccessSecurityAttribute`。

***`Parent`*** 是一个元数据 **_token_**，用于标识在其上定义 ***`PermissionSet`*** 中编码的安全自定义特性的 ***`Method`***，_Type_ 或 *Assembly*。

***`PermissionSet`*** 是一个 '*blob*'，其格式如下：
 * 包含一个句点 (.) 的字节。
 * 一个压缩的无符号整数，包含 *blob* 中编码的特性数量。
 * 包含以下内容的特性数组：
    * 一个字符串，它是特性的完全限定类型名称。字符串被编码为一个压缩的无符号整数，以指示大小，后跟一个 UTF8 字符数组。
    * 一组特性，编码为自定义特性的命名参数 [[↗]](#custom-attr-value)，从 _`NumNamed`_ 开始。

权限集包含在特定的 ***`Method`***，*Type* 或 *Assembly* (参见 ***`Parent`***) 上请求的具有 ***`Action`*** 的权限。换句话说，*blob* 将包含 ***`Parent`*** 上具有该特定 ***`Action`*** 的所有特性的编码。

此标准的第一版指定了权限集的 XML 编码。后续实现应继续支持此编码以向后兼容。

**`DeclSecurity`** 表的行是通过附加一个指定 ***`Action`*** 和 ***`PermissionSet`*** 的 **.permission** 或 **.permissionset** 指令到父程序集 [[↗]](#assembly-inside-decl) 或父类型或方法 [[↗]](#class-type-member) 上填充的。

> 元数据验证规则

| Order | Validation Rule                                                                                                                                                            |  Level  |
| :---: | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :-----: |
|  1.   | ***`Action`*** 应该只设置那些指定的值。                                                                                                                                    |  ERROR  |
|  2.   | ***`Parent`*** 应该是 **`TypeDef`**，**`MethodDef`** 或 **`Assembly`** 中的一个。也就是说，它应该索引 **`TypeDef`** 表，**`MethodDef`** 表或 **`Assembly`** 表中的有效行。 |  ERROR  |
|  3.   | 如果 ***`Parent`*** 索引了 **`TypeDef`** 表中的一行，那么该行不应定义接口。安全系统会忽略任何这样的父项；编译器不应发出这样的权限集。                                      | WARNING |
|  4.   | 如果 ***`Parent`*** 索引了一个 **`TypeDef`**，那么它的 **`TypeDef`**.***`Flags`***.*`HasSecurity`* 位应该被设置。                                                          |  ERROR  |
|  5.   | 如果 ***`Parent`*** 索引了一个 **`MethodDef`**，那么它的 **`MethodDef`**.***`Flags`***.*`HasSecurity`* 位应该被设置。                                                      |  ERROR  |
|  6.   | ***`PermissionSet`*** 应该索引 ***Blob Heap*** 中的一个 '*blob*'。                                                                                                         |  ERROR  |
|  7.   | ***`PermissionSet`*** 索引的 '*blob*' 的格式应该表示一个有效的、编码的 CLI 对象图。所有标准化权限的编码形式在 [Partition IV【】]() 中指定。                                |  ERROR  |

>---
### 20.8. ClassLayout: 0x0F
<a id="ClassLayout_0x0F"></a>

**`ClassLayout`** 表用于定义 CLI 应如何布局类或值类型的字段。通常，CLI 可以自由地重新排序和 / 或在为类或值类型定义的字段之间插入间隙。

此功能用于以与非托管 C 结构体完全相同的方式布局托管值类型，从而允许将托管值类型交给非托管代码，然后访问字段，就像该内存块是由非托管代码布局的一样。

**`ClassLayout`** 表中的信息取决于 *owner* 类或值类型中 {*`AutoLayout`*,*`SequentialLayout`*, *`ExplicitLayout`*} 的 ***`Flags`*** 值。如果类型被标记为 *`SequentialLayout`* 或 *`ExplicitLayout`*，则该类型具有布局。如果继承链中的任何类型具有布局，则其所有基类也应具有布局，直到它的直接基类是从 `System.ValueType` 派生的那个基类 (如果它存在于类型的层次结构中)；否则，直接基类是 `System.Object`。

布局不能在链的中间开始。但是在链的任何点停止具有 “布局” 都是有效的。例如，在下面的图表中，类 A 从 `System.Object` 派生；类 B 从 A 派生；类 C 从 B 派生。`System.Object` 没有布局。但是 A，B 和 C 都定义了布局，这是有效的。

 ![有效的布局设置](./.img/有效的布局设置.png)

类 E，F 和 G 的情况类似。G 没有布局，这也是有效的。下图显示了两个无效的设置： 

 ![无效的布局设置](./.img/无效的布局设置.png)

在左边，“具有布局的链” 并未从 “最高” 的类开始。在右边，“具有布局的链” 中有一个 “洞”。

类或值类型的布局信息保存在两个表 (**`ClassLayout`** 和 **`FieldLayout`**) 中，如下图所示：

 ![ClassLayout和FieldLayout](./.img/ClassLayout和FieldLayout.png)

在此示例中，**`ClassLayout`** 表的第 3 行指向 **`TypeDef`** 表的第 2 行 (类的定义，称为 “MyClass”)。**`FieldLayout`** 表的第 4 ~ 6 行指向 **`Field`** 表中的相应行。这说明了 CLI 如何存储在 “MyClass” 中定义的三个字段的显式偏移 (对于拥有类或值类型的每个字段，**`FieldLayout`** 表中总是有一行) 因此，**`ClassLayout`** 表充当 **`TypeDef`** 表中具有布局信息的那些行的扩展；由于许多类没有布局信息，总的来说，这种设计节省了空间。

> *ClassLayout* 表有以下列：

| Column              |  Kind   | Size  | Value         | Description                                      | Link |
| :------------------ | :-----: | :---: | :------------ | :----------------------------------------------- | :--- |
| ***`Token`***       | Literal |   4   | 0FUUUUUU      | 行编号，高位字节表示表编号，低三位字节是行编号。 |      |
| ***`Parent`***      |  Index  |   4   | **`TypeDef`** | 索引具有布局特性的类型定义。                     |      |
| ***`PackingSize`*** | Literal |   2   | Number        | 表示该类型的对齐信息。                           |      |
| ***`ClassSize`***   | Literal |   4   | Number        | 表示该类型的大小。                               |      |

通过在此类型声明的类型声明主体上放置 **.pack** 和 **.size** 指令来定义 **`ClassLayout`** 表的行 [[↗]](#class-type-member)。当省略这些指令中的任何一个时，其对应的值为零。参见 [_控制实例布局_](#ctrl-layout)。 

***`ClassSize`*** 为零并不意味着类的大小为零。这意味着在定义时没有指定 **.size** 指令，在这种情况下，实际大小是从字段类型计算出来的，并考虑到打包大小 (默认或指定) 和目标运行时平台上的自然对齐。

> 元数据验证规则

| Order | Validation Rule                                                                                                                                                                | Level |
| :---: | :----------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :---: |
|  1.   | **`ClassLayout`** 表可以包含零行或多行。                                                                                                                                       |
|  2.   | ***`Parent`*** 应索引 **`TypeDef`** 表中的有效行，对应于类或值类型 (但不对应于接口)。                                                                                          | ERROR |
|  3.   | ***`Parent`*** 索引的类或值类型应为 *`SequentialLayout`* 或 *`ExplicitLayout`* [[↗]](#FieldAttributes)。也就是说，*`AutoLayout`* 类型不应拥有 **`ClassLayout`** 表中的任何行。 | ERROR |
|  4.   | 如果 ***`Parent`*** 索引了一个 *`SequentialLayout`* 类型，那么：                                                                                                               |
|       | * ***`PackingSize`*** 应为 {0, 1, 2, 4, 8, 16, 32, 64, 128} 中的一个。0 表示使用应用程序运行的平台的默认打包大小。                                                             | ERROR |
|       | * 如果 ***`Parent`*** 索引了一个 ValueType，那么 ***`ClassSize`*** 应小于 1 MByte (0x100000 字节)。                                                                            | ERROR |
|  5.   | 如果 ***`Parent`*** 索引了一个 *`ExplicitLayout`* 类型，那么                                                                                                                   |
|       | * 如果 ***`Parent`*** 索引了一个 *ValueType*，那么 ***`ClassSize`*** 应小于 1 MByte (0x100000 字节)。                                                                          | ERROR |
|       | * ***`PackingSize`*** 应为 0。为每个字段提供显式偏移以及打包大小是没有意义的。                                                                                                 | ERROR |
|  6.   | 注意，如果布局没有创建字段重叠的类型，那么 *`ExplicitLayout`* 类型可能会产生可验证的类型。                                                                                     |
|  7.   | 沿着继承链方向的布局应遵循上述规则 (从 “最高” 类型开始，没有 “孔” 等)。                                                                                                        | ERROR |

>---
### 20.16. FieldLayout: 0x10
<a id="FieldLayout_0x10"></a>

> *FieldLayout* 表有以下列：

| Column                                 |  Kind   | Size  | Value       | Description                                      | Link |
| :------------------------------------- | :-----: | :---: | :---------- | :----------------------------------------------- | :--- |
| ***`Token`***                          | Literal |   4   | 10UUUUUU    | 行编号，高位字节表示表编号，低三位字节是行编号。 |      |
| ***`Field`***                          |  Index  |   4   | **`Field`** | 索引具有布局特性的字段。                         |      |
| ***`Offset`***<br>/***`FieldOffSet`*** | Literal |   4   | Number      | 表示从类型开头起的偏移量。                       |      |

请注意，任何类型中的每个字段都由其签名定义。当 CLI 布局类型实例 (即，对象) 时，每个字段是四种类型之一：
 * ***Scalar***：用于任何内置类型的成员，例如 `int32`。字段的大小由该内在类型的实际大小给出，其大小在 1 到 8 字节之间变化。
 * ***ObjectRef***：用于 *`ELEMENT_TYPE_CLASS`*，*`ELEMENT_TYPE_STRING`*，*`ELEMENT_TYPE_OBJECT`*，*`ELEMENT_TYPE_ARRAY`*，*`ELEMENT_TYPE_SZARRAY`*
 * ***Pointer***：用于 *`ELEMENT_TYPE_PTR`*，*`ELEMENT_TYPE_FNPTR`*
 * ***ValueType***：用于 *`ELEMENT_TYPE_VALUETYPE`*。该 *ValueType* 的实例实际上是在此对象中布局的，因此字段的大小是该 *ValueType* 的大小。

请注意，指定显式结构布局的元数据可以在一个平台上有效地使用，但在另一个平台上可能无效，因为这里指定的一些规则取决于特定于平台的对齐规则。

如果父字段的 **.field** 指令已指定字段偏移，则将在 **`FieldLayout`** 表中创建一行。参见 [*字段定义和字段引用*](#field)。

> 元数据验证规则

| Order | Validation Rule                                                                                                                | Level |
| :---: | :----------------------------------------------------------------------------------------------------------------------------- | :---: |
|  1.   | **`FieldLayout`** 表可以包含零行或多行。                                                                                       |
|  2.   | **`FieldLayout`** 表中每行描述的字段的类型应设置 ***`Flags`***.*`ExplicitLayout`* (参见 [_TypeAttributes_](#TypeAttributes))。 | ERROR |
|  3.   | ***`Offset`*** 应为零值或比零大值。                                                                                            | ERROR |
|  4.   | **`Field`** 应索引 **`Field`** 表中的有效行。                                                                                  | ERROR |
|  5.   | **`Field`** 索引的 **`Field`** 表中的行的 ***`Flags`***.*`Static`* 应为非静态 (值为 0)。                                       | ERROR |
|  6.   | 在给定类型拥有的行中，基于 **`Field`** 不应有重复项。也就是说，类型的给定 **`Field`** 不能被赋予两个偏移。                     | ERROR |
|  7.   | 类型 ***ObjectRef*** 的每个字段应在类型内自然对齐。                                                                            | ERROR |
|  8.   | 在给定类型拥有的行中，完全有效的是有几行具有相同的 ***`Offset`*** 值。但是 ***ObjectRef*** 和值类型不能具有相同的偏移。        | ERROR |
|  9.   | *`ExplicitLayout`* 类型的每个字段都应给出偏移；也就是说，它应在 **`FieldLayout`** 表中有一行。                                 | ERROR |

>---
### 20.36. StandAloneSig: 0x11
<a id="StandAloneSig_0x11"></a>

签名存储在元数据 ***Blob Heap*** 中。在大多数情况下，它们由某个表的某个列索引 —— **`Field`**.***`Signature`***、***`Method`***.***`Signature`***、**`MemberRef`**.***`Signature`*** 等。然而，有两种情况需要一个元数据 **_token_** 来表示一个不由任何元数据表索引的签名。**`StandAloneSig`** 表满足了这个需求。它只有一列，该列指向 ***Blob Heap*** 中的一个 ***`Signature`***。

签名应描述以下之一：
 * **一个方法** — 代码生成器为每次 `calli` 指令的调用，在 **`StandAloneSig`** 表中创建一行。该行索引 `calli` 指令的函数指针操作数的调用点签名。
 * **局部变量** — 代码生成器为每个方法在 **`StandAloneSig`** 表中创建一行，以描述其所有的局部变量。ILAsm 中的 **.locals** 指令 [[↗]](#locals) 在 **`StandAloneSig`** 表中生成一行。

> *StandAloneSig* 表有以下列：

| Column            |  Kind   | Size  | Value           | Description                                      | Link |
| :---------------- | :-----: | :---: | :-------------- | :----------------------------------------------- | :--- |
| ***`Token`***     | Literal |   4   | 11UUUUUU        | 行编号，高位字节表示表编号，低三位字节是行编号。 |      |
| ***`Signature`*** |  Index  |   4   | ***Blob Heap*** | 索引其签名的二进制数据对象。                     |      |

```cil
// 在遇到 calli 指令时，ilasm 在 blob 堆中生成一个签名 (DEFAULT，ParamCount = 1，RetType = int32，Param1 = int32)，
// 并由 StandAloneSig 表索引：
.assembly Test {}
.method static int32 AddTen(int32)
{ 
    ldarg.0
    ldc.i4  10
    add
    ret
}
.class Test
{ 
    .method static void main()
    { 
        .entrypoint
        ldc.i4.1
        ldftn int32 AddTen(int32)
        calli int32(int32)
        pop
        ret
    }
}
```

> 元数据验证规则

| Order | Validation Rule                                                                           | Level |
| :---: | :---------------------------------------------------------------------------------------- | :---: |
|  1.   | **`StandAloneSig`** 表可以包含零行或多行。                                                |
|  2.   | ***`Signature`*** 应该索引 ***Blob Heap*** 中的有效签名。                                 | ERROR |
|  3.   | 由 ***`Signature`*** 索引的签名 '*blob*' 应该是一个有效的 *`METHOD`* 或 *`LOCALS`* 签名。 | ERROR |
|  4.   | 允许重复的行。                                                                            |

>---
### 20.12. EventMap: 0x12
<a id="EventMap_0x12"></a>

> *EventMap* 表有以下列：

| Column            |  Kind   | Size  | Value         | Description                                                                                                                                                                                          | Link |
| :---------------- | :-----: | :---: | :------------ | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :--- |
| ***`Token`***     | Literal |   4   | 12UUUUUU      | 行编号，高位字节表示表编号，低三位字节是行编号。                                                                                                                                                     |      |
| ***`Parent`***    |  Index  |   4   | **`TypeDef`** | 索引声明事件的类型。                                                                                                                                                                                 |      |
| ***`EventList`*** |  Index  |   4   | **`Event`**   | 标记了由此类型拥有的一连串事件的第一个。该连续运行继续到以下较小者：<br>&emsp;i. **`Event`** 表的最后一行。<br>&emsp;ii. 通过检查 **`EventMap`** 表中下一行的 ***`EventList`*** 找到的下一连串事件。 |      |

请注意，**`EventMap`** 信息并不直接影响运行时行为；重要的是每个事件包含的方法的信息。**`EventMap`** 和 **`Event`** 表是将 **.event** 指令用于类上的结果 [[↗]](#event)。

> 元数据验证规则

| Order | Validation Rule                                                                     | Level |
| :---: | :---------------------------------------------------------------------------------- | :---: |
|  1.   | **`EventMap`** 表可以包含零行或多行。                                               |
|  2.   | 基于 ***`Parent`***，不应有重复的行 (给定的类只有一个指向其事件列表开始的 “指针”)。 | ERROR |
|  3.   | 基于 ***`EventList`***，不应有重复的行 (不同的类不能在 **`Event`** 表中共享行)。    | ERROR |

>---
### 20.13. Event: 0x14
<a id="Event_0x14"></a>

事件在元数据中的处理方式与属性非常相似；也就是说，它将定义关联到给定类上的一组方法。有两个必需的方法 (`add_` 和 `remove_`) 以及一个可选的方法 (`raise_`)；还允许其他名称的附加方法 [[↗]](#event)。作为事件聚集在一起的所有方法都应在包含事件的类上定义。

在 **`TypeDef`** 表的一行与构成给定事件的方法集合之间的关联关系保存在三个单独的表中 (这与属性类似)，如下所示：

 ![事件表示例](./.img/事件表示例.png)

**`EventMap`** 表的第 3 行索引了左边 **`TypeDef`** 表的第 2 行 (`MyClass`)，同时索引了右边 **`Event`** 表的第 4 行 (一个名为 `DocChanged` 的事件)。这个设置构建了 `MyClass` 有一个名为 `DocChanged` 的事件。但是 **`MethodDef`** 表中的哪些方法被聚集在一起作为 “属于” 事件 `DocChanged`？该关联关系包含在 **`MethodSemantics`** 表中 —— 它的第 2 行索引了右边的事件 `DocChanged`，以及左边 **`MethodDef`** 表的第 2 行 (一个名为 `add_DocChanged` 的方法)。此外，**`MethodSemantics`** 表的第 3 行索引了右边的 `DocChanged`，以及左边 **`MethodDef`** 表的第 3 行 (一个名为 `remove_DocChanged` 的方法)。如图所示，`MyClass` 还有另一个事件，名为 `TimedOut`，有两个方法，`add_TimedOut` 和 `remove_TimedOut`。

**`Event`** 表不仅仅是将其他表中的现有行聚集在一起。**`Event`** 表有 ***`EventFlags`***，***`Name`*** (例如，这里的示例中的 `DocChanged` 和 `TimedOut`) 和 ***`EventType`*** 列。此外，**`MethodSemantics`** 表有一列用于记录它索引的方法是 `add_`，`remove_`，`raise_` 还是其他函数。

> *Event* 表有以下列：

| Column                                    |  Kind   | Size  | Value             | Description                                                                                              | Link                                  |
| :---------------------------------------- | :-----: | :---: | :---------------- | :------------------------------------------------------------------------------------------------------- | :------------------------------------ |
| ***`Token`***                             | Literal |   4   | 14UUUUUU          | 行编号，高位字节表示表编号，低三位字节是行编号。                                                         |                                       |
| ***`EventFlags`***<br>/***`Attributes`*** | BitMask |   2   | _EventAttributes_ | _EventAttributes_ 类型的位掩码。                                                                         | [_EventAttributes_](#EventAttributes) |
| ***`Name`***                              |  Index  |   4   | ***String Heap*** | 索引事件的名称标识。                                                                                     |
| ***`EventType`***<br>/***`Type`***        |  Index  |   4   | Tables            | 索引 **`TypeDef`**，**`TypeRef`** 或 **`TypeSpec`** 表。它对应于事件的类型，而不是对应拥有此事件的类型。 | [_TypeDefOrRef_](#FieldAttributes)    |

请注意，**`Event`** 信息并不直接影响运行时行为；重要的是事件包含的每个方法的信息。**`EventMap`** 和 **`Event`** 表是将 **.event** 指令放在类上的结果 (参见 [_事件定义_](#event))。

> 元数据验证规则

| Order | Validation Rule                                                                                                                 | Level |
| :---: | :------------------------------------------------------------------------------------------------------------------------------ | :---: |
|  1.   | **`Event`** 表可以包含零行或多行。                                                                                              |
|  2.   | **`EventMap`** 表中的每一行在都应有一个 ***`EventList`***，且只有一个。                                                         | ERROR |
|  3.   | ***`EventFlags`*** 只应设置指定的值 (所有组合有效)。                                                                            | ERROR |
|  4.   | ***`Name`*** 应索引 ***String Heap*** 中的 no-empty 字符串。                                                                    | ERROR |
|  5.   | ***`Name`*** 字符串应为有效的 CLS 标识符。                                                                                      |  CLS  |
|  6.   | ***`EventType`*** 可以为 null 或非 null。                                                                                       |
|  7.   | 如果 ***`EventType`*** 为非 null，则它应索引 **`TypeDef`** 或 **`TypeRef`** 表中的有效行。                                      | ERROR |
|  8.   | 如果 ***`EventType`*** 为非 null，则它索引的 **`TypeDef`**，**`TypeRef`** 或 **`TypeSpec`** 表中的行应为类 (不是接口或值类型)。 | ERROR |
|  9.   | 对于每一行，在 **`MethodSemantics`** 表中应有一个 `add_` 和一个 `remove_` 行。                                                  | ERROR |
|  10.  | 对于每一行，可以有零个或一个 `raise_` 行，以及 **`MethodSemantics`** 表中的零个或多个 `other` 行。                              | ERROR |
|  11.  | 在 **`TypeDef`** 表中的给定行拥有的行中，基于 ***`Name`*** 不应有重复项。                                                       | ERROR |
|  12.  | 基于 ***`Name`*** 不应有重复行，其中 ***`Name`*** 字段使用 CLS 冲突标识符规则进行比较。                                         |  CLS  |

>---
### 20.35. PropertyMap: 0x15
<a id="PropertyMap_0x15"></a>

> *PropertyMap* 表有以下列：

| Column               |  Kind   | Size  | Value          | Description                                                                                                                                                                                                           | Link |
| :------------------- | :-----: | :---: | :------------- | :-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :--- |
| ***`Token`***        | Literal |   4   | 15UUUUUU       | 行编号，高位字节表示表编号，低三位字节是行编号。                                                                                                                                                                      |      |
| ***`Parent`***       |  Index  |   4   | **`TypeDef`**  | 索引声明属性的类型。                                                                                                                                                                                                  |      |
| ***`PropertyList`*** |  Index  |   4   | **`Property`** | 标记了由 ***`Parent`*** 拥有的属性的连续运行的第一个。运行继续到以下较小者：<br>&emsp;i. **`Property`** 表的最后一行。<br>&emsp;ii. 通过检查此 **`PropertyMap`** 表中下一行的 ***`PropertyList`*** 找到的下一组属性。 |      |

**`PropertyMap`** 和 **`Property`** 表是将 **.property** 指令放在类上的结果 (参见 [*属性定义*](#property))。

> 元数据验证规则

| Order | Validation Rule                                                                     | Level |
| :---: | :---------------------------------------------------------------------------------- | :---: |
|  1.   | **`PropertyMap`** 表可以包含零行或多行。                                            |
|  2.   | 基于 ***`Parent`*** 不应有重复行 (给定类只有一个指向其属性列表开始的 “指针”)。      | ERROR |
|  3.   | 基于 ***`PropertyList`*** 不应有重复行 (不同的类不能在 **`Property`** 表中共享行)。 | ERROR |

>---
### 20.34. Property: 0x17
<a id="Property_0x17"></a>

在元数据中，属性最好被视为一种手段，用于将定义在类上的方法集合聚在一起，并给它们一个名字，而不是其他。这些方法通常是已经在类上定义的 *get_* 和 *set_* 方法，并像其他方法一样插入到 **`MethodDef`** 表中。这种关联是由三个独立的表维护在一起，如下图所示：

 ![属性表示例](./.img/属性表示例.png)

**`PropertyMap`** 表的第 3 行索引了左边 **`TypeDef`** 表的第 2 行 (`MyClass`)，同时索引了右边 **`Property`** 表的第 4 行 —— 一个名为 Foo 的属性的行。这个设置构建了 `MyClass` 有一个名为 `Foo` 的属性。但是在 **`MethodDef`** 表中，哪些方法被聚集在一起作为 “属于” 属性 `Foo`？这种关联包含在 **`MethodSemantics`** 表中 —— 它的第 2 行索引了右边的属性 `Foo`，和左边 **`MethodDef`** 表的第 2 行 (一个名为 `get_Foo` 的方法)。此外，**`MethodSemantics`** 表的第 3 行索引了右边的 `Foo`，和左边 **`MethodDef`** 表的第 3 行 (一个名为 `set_Foo` 的方法)。如图所示，`MyClass` 还有另一个属性叫 `Bar`，有两个方法，`get_Bar` 和 `set_Bar`。

属性表做的不仅仅是将其他表中已有的行聚集在一起。**`Property`** 表有 ***`Flags`***、***`Name`*** (例如这里的 `Foo` 和 `Bar`) 和 ***`Type`*** 的列。此外，**`MethodSemantics`** 表有一个列来记录它指向的方法是 *set_*、*get_* 还是 *other*。

CLS 引用了实例、虚拟和静态属性。属性的签名 (来自 ***`Type`*** 列) 可以用来区分静态属性，因为实例和虚拟属性在签名中会设置 "*`HASTHIS`*" 位 [[↗]](#MethodDefSig)，而静态属性则不会。实例和虚拟属性之间的区别取决于 *getter* 和 *setter* 方法的签名，CLS 要求它们要么都是虚拟的，要么都是实例的。

> *Property* (0x17) 表有以下列：

| *Token* |      |      |      |
| :------ | :--- | :--- | :--- |

| Column                               |  Kind   | Size  | Value                | Description                                      | Link                                     |
| :----------------------------------- | :-----: | :---: | :------------------- | :----------------------------------------------- | :--------------------------------------- |
| ***`Token`***                        | Literal |   4   | 17UUUUUU             | 行编号，高位字节表示表编号，低三位字节是行编号。 |                                          |
| ***`Flags`***<br>/***`Attributes`*** | BitMask |   2   | _PropertyAttributes_ | _PropertyAttributes_ 类型的位掩码。              | [_PropertyAttributes_](#EventAttributes) |
| ***`Name`***                         |  Index  |   4   | ***String Heap***    | 索引属性的名称标识。                             |                                          |
| ***`Type`***<br>/***`Signature`***   |  Index  |   4   | ***Blob Heap***      | 索引属性签名的二进制数据对象。                   |                                          |

> 元数据验证规则

| Order | Validation Rule                                                                                                                                                                                 | Level |
| :---: | :---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :---: |
|  1.   | **`Property`** 表可以包含零行或多行。                                                                                                                                                           |
|  2.   | 在 **`PropertyMap`** 表中的 *owner* 行，每一行应有一个 ***`PropertyList`***，且只有一个。                                                                                                       | ERROR |
|  3.   | _PropFlags_ 只应设置那些指定的值 (所有组合有效)。                                                                                                                                               | ERROR |
|  4.   | ***`Name`*** 应索引 ***String Heap*** 中的 no-empty 字符串。                                                                                                                                    | ERROR |
|  5.   | ***`Name`*** 字符串应是一个有效的 CLS 标识符。                                                                                                                                                  |  CLS  |
|  6.   | ***`Type`*** 应索引 ***Blob Heap*** 中的非空签名。                                                                                                                                              | ERROR |
|  7.   | 由 ***`Type`*** 索引的签名应是一个有效的属性签名 (即，前导字节的低四位是 0x8)。除了这个前导字节，签名与属性的 `get_` 方法相同。                                                                 | ERROR |
|  8.   | 在由 **`TypeDef`** 表中的给定行拥有的行中，基于 ***`Name`***+***`Type`*** 不应有重复的行。                                                                                                      | ERROR |
|  9.   | 基于 ***`Name`***，不应有重复的行，其中 ***`Name`*** 字段使用 CLS 冲突标识符规则进行比较。特别是，属性不能通过它们的类型进行重载 —— 例如，一个类不能有两个属性，"`int Foo`" 和 "`String Foo`"。 |  CLS  |
>---
### 20.28. MethodSemantics: 0x18
<a id="MethodSemantics_0x18"></a>

> *MethodSemantics* 表有以下列：

| Column              |  Kind   | Size  | Value                       | Description                                      | Link                                                      |
| :------------------ | :-----: | :---: | :-------------------------- | :----------------------------------------------- | :-------------------------------------------------------- |
| ***`Token`***       | Literal |   4   | 18UUUUUU                    | 行编号，高位字节表示表编号，低三位字节是行编号。 |                                                           |
| ***`Semantics`***   | BitMask |   2   | _MethodSemanticsAttributes_ | 描述方法的语义类别。                             | [*MethodSemanticsAttributes*](#MethodSemanticsAttributes) |
| ***`Method`***      |  Index  |   4   | **`MethodDef`**             | 索引关联语义的方法定义。                         |                                                           |
| ***`Association`*** |  Index  |   4   | Tables                      | 索引 **`Event`** 或 **`Property`** 表。          | [_HasSemantics_](#HasSemantics)                        |

**`MethodSemantics`** 表的行由 **.property** [[↗]](#property) 和 **.event** 指令 [[↗]](#event) 填充。有关更多信息，请参见 [_Event: 0x14_](#Event_0x14) 和 [*Property: 0x17*](#Property_0x17)。

> 元数据验证规则

| Order | Validation Rule                                                                                                                                                           | Level |
| :---: | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | :---: |
|  1.   | **`MethodSemantics`** 表可以包含零行或多行。                                                                                                                              |
|  2.   | ***`Semantics`*** 只应设置那些指定的值。                                                                                                                                  | ERROR |
|  3.   | ***`Method`*** 应索引 **`MethodDef`** 表中的有效行，该行应为此行描述的属性或事件的同一类中定义的方法。                                                                    | ERROR |
|  4.   | 对于给定的属性或事件，所有方法应具有相同的可访问性 (即他们的 ***`Flags`*** 行的 *`MemberAccessMask`* 子字段) 并且不能是 *`CompilerControlled`*。                          |  CLS  |
|  5.   | ***`Semantics`***：受以下限制：                                                                                                                                           |
|       | * 如果此行是用于属性的，那么 *`Setter`*、*`Getter`* 或 *`Other`* 中的一个被设置。                                                                                         | ERROR |
|       | * 如果此行是用于事件的，那么 *`AddOn`*、*`RemoveOn`*、*`Fire`* 或 *`Other`* 中的一个被设置。                                                                              | ERROR |
|  6.   | 如果此行是用于事件的，并且其 ***`Semantics`*** 是 *`Addon`* 或 *`RemoveOn`*，那么由 ***`Method`*** 索引的 **`MethodDef`** 表中的行应接受一个委托作为参数，并返回 `void`。 | ERROR |
|  7.   | 如果此行是用于事件的，并且其 ***`Semantics`*** 是 *`Fire`*，那么由 ***`Method`*** 索引的 **`MethodDef`** 表中的行可以返回任何类型。                                       |
|  8.   | 对于每个属性，应有一个 *setter*，或一个 *getter*，或两者都有。                                                                                                            |  CLS  |
|  9.   | 其 ***`Name`*** 是 `xxx`的任何属性的 *getter* 方法，它应被称为 `get_xxx`。                                                                                                |  CLS  |
|  10.  | 其 ***`Name`*** 是 `xxx` 的任何属性的 *setter* 方法，它应被称为 `set_xxx`。                                                                                               |  CLS  |
|  11.  | 如果一个属性提供了 *getter* 和 *setter* 方法，那么这些方法应在 ***`Flags`***.*`MemberAccessMask`* 子字段中具有相同的值。                                                  |  CLS  |
|  12.  | 如果一个属性提供了 *getter* 和 *setter* 方法，那么这些方法应对于他们的 ***`Method`***.***`Flags`***.*`Virtual`* 具有相同的值。                                            |  CLS  |
|  13.  | 任何 *getter* 和 *setter* 方法应具有 ***`Method`***.***`Flags`***.*`SpecialName`* = 1。                                                                                   |  CLS  |
|  14.  | 任何 *getter* 方法应具有与 **`Property`**.***`Type`*** 字段索引的签名匹配的返回类型。                                                                                     |  CLS  |
|  15.  | 任何 *setter* 方法的最后一个参数应具有与 **`Property`**.***`Type`*** 字段索引的签名匹配的类型。                                                                           |  CLS  |
|  16.  | 任何 *setter* 方法应在 ***`Method`***.***`Signature`*** 中具有返回类型 *`ELEMENT_TYPE_VOID`* [[↗]](#ELEMENT_TYPE)。                                                       |  CLS  |
|  17.  | 如果属性被索引，那么 *getter* 和 *setter* 的索引在数量和类型上必须一致。                                                                                                  |  CLS  |
|  18.  | 任何事件的 *AddOn* 方法，其 ***`Name`*** 是 `xxx`，应具有签名：``void add_xxx (<DelegateType> handler)``。                                                                |  CLS  |
|  19.  | 任何事件的 *RemoveOn* 方法，其 ***`Name`*** 是 `xxx`，应具有签名：`void remove_xxx(<DelegateType> handler)`。                                                             |  CLS  |
|  20.  | 任何事件的 *Fire* 方法，其 ***`Name`*** 是 `xxx`，应具有签名：`void raise_xxx(Event e)`。                                                                                 |  CLS  |

>---
### 20.27. MethodImpl: 0x19
<a id="MethodImpl_0x19"></a>

**`MethodImpl`** 表允许编译器覆盖 CLI 提供的默认继承规则。它们最初的用途是允许一个类 `C`，它从接口 `I` 和 `J` 都继承了方法 `M`，为这两个方法提供实现 (而不是在其 ***vtable*** 中只有 `M` 的一个插槽)。然而，_MethodImpls_ 也可以出于其他原因使用，只受限于编译器编写者在下面定义的验证规则的约束内的独创性。

在上面的例子中，***`Class`*** 指定 `C`，***`MethodDeclaration`*** 指定 `I::M`，***`MethodBody`*** 指定为 `I::M` 提供实现的方法 (要么是 `C` 内的一个方法体，要么是 `C` 的基类实现的一个方法体)。

> *MethodImpl* 表有以下列：

| Column                         |  Kind   | Size  | Value         | Description                                      | Link                                 |
| :----------------------------- | :-----: | :---: | :------------ | :----------------------------------------------- | :----------------------------------- |
| ***`Token`***                  | Literal |   4   | 19UUUUUU      | 行编号，高位字节表示表编号，低三位字节是行编号。 |                                      |
| ***`MethodDeclaration`***      |  Index  |   4   | Tables        | 索引 **`MethodDef`** 或 **`MemberRef`** 表。     | [_MethodDefOrRef_](#MethodDefOrRef) |
| ***`MethodBody`***             |  Index  |   4   | Tables        | 索引 **`MethodDef`** 或 **`MemberRef`** 表。     | [_MethodDefOrRef_](#MethodDefOrRef) |
| ***`Class`***<br>/***`Type`*** |  Index  |   4   | **`TypeDef`** | 索引实现方法的类型定义。                         |                                      |

ILAsm 使用 **.override** 指令来指定 **`MethodImpl`** 表的行，参考 [*Override*](#override) 和 [*MethodBody*](#MethodBody)。

> 元数据验证规则

| Order | Validation Rule                                                                                                                                                                     | Level |
| :---: | :---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :---: |
|  1.   | **`MethodImpl`** 表可以包含零行或多行。                                                                                                                                             |
|  2.   | ***`Class`*** 应该索引 **`TypeDef`** 表中的有效行。                                                                                                                                 | ERROR |
|  3.   | ***`MethodBody`*** 应该索引 **`MethodDef`** 或 **`MemberRef`** 表中的有效行。                                                                                                       | ERROR |
|  4.   | 由 ***`MethodDeclaration`*** 索引的方法应该设置 ***`Flags`***.*`Virtual`*。                                                                                                         | ERROR |
|  5.   | 由 ***`MethodDeclaration`*** 索引的方法的 *owner* 类型不应该有 ***`Flags`***.*`Sealed`* = 0。                                                                                       | ERROR |
|  6.   | 由 ***`MethodBody`*** 索引的方法应该是 ***`Class`*** 或 ***`Class`*** 的某个基类的成员 (*MethodImpls* 不允许编译器 “*hook*” 任意方法体)。                                           | ERROR |
|  7.   | 由 ***`MethodBody`*** 索引的方法应该是虚方法。                                                                                                                                      | ERROR |
|  8.   | 由 ***`MethodBody`*** 索引的方法其 ***`Method`***.***`RVA`*** ≠ 0 (例如，不能是通过 PInvoke 到达的非托管方法)。                                                                     | ERROR |
|  9.   | ***`MethodDeclaration`*** 应该索引 ***`Class`*** 的祖先链中的一个方法 (通过其 ***`Extends`*** 链到达) 或 ***`Class`*** 的接口树中的一个方法 (通过其 **`InterfaceImpl`** 条目到达)。 | ERROR |
|  10.  | 由 ***`MethodDeclaration`*** 索引的方法不应该是 **final** (其 ***`Flags`***.`Final` 应该是 0)。                                                                                     | ERROR |
|  11.  | 如果 ***`MethodDeclaration`*** 设置了 *`Strict`* 标志，那么由 ***`MethodDeclaration`*** 索引的方法应该对 ***`Class`*** 是可访问的。                                                 | ERROR |
|  12.  | 由 ***`MethodBody`*** 定义的方法签名应该与 ***`MethodDeclaration`*** 定义的方法签名匹配。                                                                                           | ERROR |
|  13.  | 基于 ***`Class`***+***`MethodDeclaration`*** 不应该有重复的行。                                                                                                                     | ERROR |

>---
### 20.31. ModuleRef: 0x1A
<a id="ModuleRef_0x1A"></a>

> *ModuleRef* 表有以下列：

| Column        |  Kind   | Size  | Value             | Description                                      | Link |
| :------------ | :-----: | :---: | :---------------- | :----------------------------------------------- | :--- |
| ***`Token`*** | Literal |   4   | 1AUUUUUU          | 行编号，高位字节表示表编号，低三位字节是行编号。 |      |
| ***`Name`***  |  Index  |   4   | ***String Heap*** | 索引引用的模块的名称标识。                       |      |

**`ModuleRef`** 表中的行是由 Assembly 中的 **.module extern** 指令 [[↗]](#module-extern) 产生的。

> 元数据验证规则

| Order | Validation Rule                                                                                                                              |  Level  |
| :---: | :------------------------------------------------------------------------------------------------------------------------------------------- | :-----: |
|  1.   | ***`Name`*** 应索引 ***String Heap*** 中的 no-empty 字符串。这个字符串应使 CLI 能够定位目标模块。通常，它可能命名用于保存模块的文件。        |  ERROR  |
|  2.   | ***`Name`*** 不应有重复的行。                                                                                                                | WARNING |
|  3.   | ***`Name`*** 应与 **`File`** 表的 ***`Name`*** 列中的一个条目匹配。此外，该条目应使 CLI 能够定位目标模块。通常它可能命名用于保存模块的文件。 |  ERROR  |

>---
### 20.39. TypeSpec: 0x1B
<a id="TypeSpec_0x1B"></a>

**`TypeSpec`** 表只有一列，它索引了存储在 ***Blob Heap*** 中的一个类型的规范。这为该类型提供了一个元数据 **_token_**  (而不是简单地索引 ***Blob Heap***)。这通常是必需的，例如，对数组操作，如创建或调用数组类的方法。

> *TypeSpec* 表有以下列：

| Column            |  Kind   | Size  | Value           | Description                                      | Link                           |
| :---------------- | :-----: | :---: | :-------------- | :----------------------------------------------- | :----------------------------- |
| ***`Token`***     | Literal |   4   | 1BUUUUUU        | 行编号，高位字节表示表编号，低三位字节是行编号。 |                                |
| ***`Signature`*** |  Index  |   4   | ***Blob Heap*** | 索引该类型特殊签名的二进制数据对象               | [*Type Spec*](#type-spec-blob) |

注意，**`TypeSpec`**  **_token_** 可以与任何接受 **`TypeDef`** 或 **`TypeRef`**  **_token_** 的 CIL 指令一起使用；具体来说，`castclass`，`cpobj`，`initobj`，`isinst`，`ldelema`，`ldobj`，`mkrefany`，`newarr`，`refanyval`，`sizeof`，`stobj`，`box`，和 `unbox`。

> 元数据验证规则

| Order | Validation Rule                                                     | Level |
| :---: | :------------------------------------------------------------------ | :---: |
|  1.   | **`TypeSpec`** 表可以包含零行或多行。                               |
|  2.   | ***`Signature`*** 应该索引 ***Blob Heap*** 中的一个有效的类型规范。 | ERROR |
|  3.   | 基于 ***`Signature`***，不应该有重复的行。                          | ERROR |
    

>---
### 20.22. ImplMap: 0x1C
<a id="ImplMap_0x1C"></a>

**`ImplMap`** 表保存了关于可以从托管代码通过 PInvoke 调度访问的非托管方法的信息。**`ImplMap`** 表的每一行将 ***`MemberForwarded`***(索引 **`MethodDef`** 表中的一行) 与 ***`ImportScope`*** 索引的 (**`ModuleRef`** 的) 某个非托管 DLL  中的例程 (***`ImportName`***) 的名称关联起来。

典型的例子是：将存储在 ***`Method`*** 表的第 N 行的托管方法 (所以 ***`MemberForwarded`*** 将有值 N) 与 DLL "`kernel32`" 中名为 "`GetEnvironmentVariable`" 的例程 (由 ***`ImportName`*** 索引的字符串) 关联起来 (***`ImportScope`*** 索引的 **`ModuleRef`** 表中的字符串)。CLI 拦截对托管方法编号 N 的调用，并将它们转发为对 "`kernel32.dll`" 中名为 "`GetEnvironmentVariable`" 的非托管例程的调用 (包括根据需要封送任何参数)。

CLI 不支持此机制来访问从 DLL 导出的字段，只支持方法。

> *ImplMap* 表有以下列：

| Column                  |  Kind   | Size  | Value                 | Description                                                                                              | Link                                      |
| :---------------------- | :-----: | :---: | :-------------------- | :------------------------------------------------------------------------------------------------------- | :---------------------------------------- |
| ***`Token`***           | Literal |   4   | 1CUUUUUU              | 行编号，高位字节表示表编号，低三位字节是行编号。                                                         |                                           |
| ***`MappingFlags`***    | BitMask |   2   | _PInvokeAttributes_， | _PInvokeAttributes_ 类型的位掩码。                                                                       | [_PInvokeAttributes_](#PInvokeAttributes) |
| ***`MemberForwarded`*** |  Index  |   4   | Tables                | 索引 **`Field`** 或 **`MethodDef`** 表。但是它只会索引 **`MethodDef`** 表，因为不支持 **`Field`** 导出。 | [*MemberForwarded*](#MemberForwarded)     |
| ***`ImportScope`***     |  Index  |   4   | **`ModuleRef`**       | 索引引入方法的模块引用。                                                                                 |                                           |
| ***`ImportName`***      |  Index  |   4   | ***String Heap***     | 索引引入方法的名称标识。                                                                                 |                                           |

对于每个定义了一个指定 ***`MappingFlags`***、***`ImportName`*** 和 ***`ImportScope`*** 的 **.pinvokeimpl** 互操作特性的父方法 [[↗]](#unmanaged-method)，都会在 **`ImplMap`** 表中生成一行。

> 元数据验证规则

| Order | Validation Rule                                                                                                                                                                                                                                          | Level |
| :---: | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :---: |
|  1.   | **`ImplMap`** 可以包含零行或多行。                                                                                                                                                                                                                       |
|  2.   | ***`MappingFlags`*** 只应设置那些指定的值。                                                                                                                                                                                                              | ERROR |
|  3.   | ***`MemberForwarded`*** 应该索引 **`MethodDef`** 表中的有效行。                                                                                                                                                                                          | ERROR |
|  4.   | 在 **`MethodDef`** 表中由 ***`MemberForwarded`*** 索引的行中的 ***`MappingFlags`***.*`CharSetMask`* [[↗]](#PInvokeAttributes) 应该最多设置以下位之一：*`CharSetAnsi`*、*`CharSetUnicode`* 或 *`CharSetAuto`* (如果没有设置，默认为 *`CharSetNotSpec`*)。 | ERROR |
|  5.   | ***`ImportName`*** 应该索引 ***String Heap*** 中的 no-empty 字符串。                                                                                                                                                                                     | ERROR |
|  6.   | ***`ImportScope`*** 应该索引 **`ModuleRef`** 表中的有效行。                                                                                                                                                                                              | ERROR |
|  7.   | 由 ***`MemberForwarded`*** 在 **`MethodDef`** 表中索引的行应该有其 ***`Flags`***.*`PinvokeImpl`* = 1，并且 ***`Flags`***.*`Static`* = 1。                                                                                                                | ERROR |


>---
### 20.18. FieldRVA: 0x1D
<a id="FieldRVA_0x1D"></a>

> *FieldRVA* 表有以下列：

| Column                              |  Kind   | Size  | Value       | Description                                      | Link |
| :---------------------------------- | :-----: | :---: | :---------- | :----------------------------------------------- | :--- |
| ***`Token`***                       | Literal |   4   | 1DUUUUUU    | 行编号，高位字节表示表编号，低三位字节是行编号。 |      |
| ***`RVA`***<br>/***`FieldOffset`*** | Literal |   4   | Number      | 索引的字段的相对虚拟地址。                       |      |
| ***`Field`***                       |  Index  |   4   | **`Field`** | 索引的字段。                                     |      |

从概念上讲，**`FieldRVA`** 表中的每一行都是 **`Field`** 表中的确切一行的扩展，并记录了此字段的初始值存储在图像文件中的 RVA (相对虚拟地址)。

对于每个指定了可选的 **data** 标签的静态父字段，都会创建 **`FieldRVA`** 表中的一行 (参见 [_字段定义和字段引用_](#field))。RVA 列是 PE 文件中数据的相对虚拟地址 (参见 [_在 PE 文件中嵌入数据_](#data))。

> 元数据验证规则

| Order | Validation Rule                                                                                                                                                                                                                                          | Level |
| :---: | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :---: |
|  1.   | ***`RVA`*** 应为非零。                                                                                                                                                                                                                                   | ERROR |
|  2.   | ***`RVA`*** 应指向当前模块的数据区域 (而不是其元数据区域)。                                                                                                                                                                                              | ERROR |
|  3.   | **`Field`** 应索引 **`Field`** 表中的有效行。                                                                                                                                                                                                            | ERROR |
|  4.   | 任何具有 RVA 的字段应为 *ValueType* (而不是类或接口)。此外，它不应有任何私有字段 (同样适用于其自身为 *ValueType* 的任何字段)。如果违反了这些条件，代码可以覆盖该全局静态并访问其私有字段。此外，该 *ValueType* 的任何字段都不能是对象引用 (进入 GC 堆)。 | ERROR |
|  5.   | 只要两个基于 RVA 的字段符合前面的条件，两个 *ValueType* 跨越的内存范围就可以重叠，没有进一步的约束。这实际上不是一个额外的规则；它只是澄清了关于重叠的基于 RVA 的字段的位置                                                                              |

>---
### 20.2. Assembly: 0x20
<a id="Assembly_0x20"></a>

> *Assembly* 表有以下列：

| Column                                      |  Kind   |  Size   | Value                   | Description                                                                               | Link                                                    |
| :------------------------------------------ | :-----: | :-----: | :---------------------- | :---------------------------------------------------------------------------------------- | :------------------------------------------------------ |
| ***`Token`***                               | Literal |    4    | 20UUUUUU                | 行编号，高位字节表示表编号，低三位字节是行编号。                                          |                                                         |
| ***`HashAlgId`***<br>/***`HashAlgorithm`*** | Literal |    4    | _AssemblyHashAlgorithm_ | 表示计算程序集哈希值的算法类型。                                                          | [_AssemblyHashAlgorithm_](#AssemblyHashAlgorithm-value) |
| ***`Flags`***                               | BitMask |    4    | _AssemblyFlags_         | _AssemblyFlags_ 类别的掩码。                                                              | [_AssemblyFlags_](#AssemblyFlags)                       |
| ***`Version`***                             | Literal | 2/2/2/2 | Number                  | ***`MajorVersion`***，***`MinorVersion`*****，***`BuildNumber`***，***`RevisionNumber`*** |                                                         |
| ***`PublicKey`***                           |  Index  |    4    | ***Blob Heap***         | 索引公钥的二进制数据对象。                                                                |                                                         |
| ***`Name`***                                |  Index  |    4    | ***String Heap***       | 索引程序集的名称标识。                                                                    |                                                         |
| ***`Culture`***                             |  Index  |    4    | ***String Heap***       | 索引该程序集的区域性名称标识。                                                            |                                                         |

**`Assembly`** 表使用 **.assembly** 指令定义 (参见 [_程序集定义_](#assembly) )；其列从相应的 **.hash** 算法，**.ver**，**.publickey** 和 **.culture** 中获取 [[↗]](#AsmDecl)。

> 元数据验证规则

| Order | Validation Rule                                                                                          | Level |
| :---: | :------------------------------------------------------------------------------------------------------- | :---: |
|  1.   | **`Assembly`** 表应包含零行或一行。                                                                      | ERROR |
|  2.   | ***`HashAlgId`*** 应为指定的值之一。                                                                     | ERROR |
|  3.   | ***`MajorVersion`***，***`MinorVersion`***，***`BuildNumber`*** 和 ***`RevisionNumber`*** 可以有任何值。 |
|  4.   | ***`Flags`*** 只应设置指定的值。                                                                         | ERROR |
|  5.   | ***`PublicKey`*** 可以为 null 或非 null。                                                                |
|  6.   | ***`Name`*** 应索引 ***String Heap*** 中的 non-empty 字符串。                                            | ERROR |
|  7.   | ***`Name`*** 索引的字符串可以是无限长度。                                                                |
|  8.   | ***`Culture`*** 可以为 null 或非 null。                                                                  |
|  9.   | 如果 ***`Culture`*** 为非 null，它应索引指定列表中的单个字符串 [[↗]](#Culture-values)。                  | ERROR |

***`Name`*** 是一个简单的名称 (例如，“Foo”，没有驱动器字母，没有路径，没有文件扩展名)；在符合 POSIX 的系统上，***`Name`*** 不包含冒号，不包含正斜杠，不包含反斜杠，也不包含句点。

>---
### 20.4. AssemblyProcessor: 0x21
<a id="AssemblyProcessor_0x21"></a>

> *AssemblyProcessor* 表有以下列：

| Column            |  Kind   | Size  | Value    | Description                                      | Link |
| :---------------- | :-----: | :---: | :------- | :----------------------------------------------- | :--- |
| ***`Token`***     | Literal |   4   | 21UUUUUU | 行编号，高位字节表示表编号，低三位字节是行编号。 |      |
| ***`Processor`*** | Literal |   4   | Number   |                                                  |      |  |

此记录不应被发出到任何 PE 文件中。然而，如果它出现在 PE 文件中，应该将其字段视为零。CLI 应该忽略它。

>---
### 20.3. AssemblyOS: 0x22
<a id="AssemblyOS_0x22"></a>

> *AssemblyOS* 表有以下列：

| Column                 |  Kind   | Size  | Value    | Description                                      | Link |
| :--------------------- | :-----: | :---: | :------- | :----------------------------------------------- | :--- |
| ***`Token`***          | Literal |   4   | 22UUUUUU | 行编号，高位字节表示表编号，低三位字节是行编号。 |      |
| ***`OSPlatformID`***   | Literal |   4   |          |                                                  |      |
| ***`OSMajorVersion`*** | Literal |   4   |          |                                                  |      |
| ***`OSMinorVersion`*** | Literal |   4   |          |                                                  |      |

此记录不应被发出到任何 PE 文件中。然而，如果它出现在 PE 文件中，它应被视为所有字段都为零。CLI 将忽略它。

>---
### 20.5. AssemblyRef: 0x23
<a id="AssemblyRef_0x23"></a>

> *AssemblyRef* 表有以下列：

| Column                   |  Kind   |  Size   | Value             | Description                                                                               | Link                              |
| :----------------------- | :-----: | :-----: | :---------------- | :---------------------------------------------------------------------------------------- | :-------------------------------- |
| ***`Token`***            | Literal |    4    | 23UUUUUU          | 行编号，高位字节表示表编号，低三位字节是行编号。                                          |                                   |
| ***`Version`***          | Literal | 2/2/2/2 | Number            | ***`MajorVersion`***，***`MinorVersion`***，***`BuildNumber`***，***`RevisionNumber`***。 |                                   |
| ***`Flags`***            | BitMask |    4    | _AssemblyFlags_   | _AssemblyFlags_ 类型的位掩码。                                                            | [_AssemblyFlags_](#AssemblyFlags) |
| ***`PublicKeyOrToken`*** |  Index  |    4    | ***Blob Heap***   | 指向它的二进制数据对象。表示标识此 *Assembly* 的发起者的公钥或 **_token_**。              |                                   |
| ***`Name`***             |  Index  |    4    | ***String Heap*** | 索引引用的程序集的名称标识。                                                              |                                   |
| ***`HashValue`***        |  Index  |    4    | ***Blob Heap***   | 索引引用的程序集的哈希值的二进制数据对象。                                                |                                   |

该表由 **.assembly extern** 指令定义 [[↗]](#assembly-extern)。其列使用与 **`Assembly`** 表类似的指令填充，除了 ***`PublicKeyOrToken`*** 列，该列使用 **.publickeytoken** 指令定义。

> 元数据验证规则

***`Name`*** 是一个简单的名称 (例如，“Foo”，没有驱动器字母，没有路径，没有文件扩展名) ；在符合 POSIX 的系统上，Name 不包含冒号，不包含正斜杠，不包含反斜杠，也不包含句点。

| Order | Validation Rule                                                                                                                                                                                                           |  Level  |
| :---: | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | :-----: |
|  1.   | ***`MajorVersion`***，***`MinorVersion`***，***`BuildNumber`*** 和 ***`RevisionNumber`*** 可以有任何值。                                                                                                                  |
|  2.   | ***`Flags`*** 只应设置一个位，即 *`PublicKey`* 位 [[↗]](#AssemblyFlags)。所有其他位应为零。                                                                                                                               |  ERROR  |
|  3.   | ***`PublicKeyOrToken`*** 可以为 null 或非 null (注意 ***`Flags`***.*`PublicKey`* 位指定 '*blob*' 是完整的公钥还是短哈希 **_token_**)。                                                                                    |
|  4.   | 如果非 null，则 ***`PublicKeyOrToken`*** 应索引 ***Blob Heap*** 中的有效偏移。                                                                                                                                            |  ERROR  |
|  5.   | ***`Name`*** 应索引 ***String Heap*** 中的 non-empty 字符串 (其长度没有限制)。                                                                                                                                            |  ERROR  |
|  6.   | ***`Culture`*** 可以为 null 或非 null。                                                                                                                                                                                   |
|  7.   | 如果非 null，它应索引指定列表中的单个字符串 [[↗]](#Culture-values)。                                                                                                                                                      |  ERROR  |
|  8.   | ***`HashValue`*** 可以为 null 或非 null。                                                                                                                                                                                 |
|  9.   | 如果非 null，则 ***`HashValue`*** 应索引 ***Blob Heap*** 中的非空 '*blob*'。                                                                                                                                              |  ERROR  |
|  10.  | **`AssemblyRef`** 表不应包含重复项 (其中重复行被视为具有相同的 ***`MajorVersion`***，***`MinorVersion`***，***`BuildNumber`***，***`RevisionNumber`***，***`PublicKeyOrToken`***，***`Name`*** 和 ***`Culture`*** 的行)。 | WARNING |

>---
### 20.7. AssemblyRefProcessor: 0x24
<a id="AssemblyRefProcessor_0x24"></a>

> *AssemblyRefProcessor* 表有以下列：

| Column              |  Kind   | Size  | Value             | Description                                      | Link |
| :------------------ | :-----: | :---: | :---------------- | :----------------------------------------------- | :--- |
| ***`Token`***       | Literal |   4   | 24UUUUUU          | 行编号，高位字节表示表编号，低三位字节是行编号。 |      |
| ***`Processor`***   | Literal |   4   | Number            |                                                  |      |
| ***`AssemblyRef`*** |  Index  |   4   | **`AssemblyRef`** | 索引引用的程序集。                               |      |

这些记录不应被发出到任何 PE 文件中。然而，如果它们出现在 PE 文件中，应该将其字段视为零。CLI 应该忽略它们。

>---
### 20.6. AssemblyRefOS: 0x25
<a id="AssemblyRefOS_0x25"></a>

> *AssemblyRefOS* 表有以下列：

| Column                 |  Kind   | Size  | Value             | Description                                      | Link |
| :--------------------- | :-----: | :---: | :---------------- | :----------------------------------------------- | :--- |
| ***`Token`***          | Literal |   4   | 25UUUUUU          | 行编号，高位字节表示表编号，低三位字节是行编号。 |      |
| ***`OSPlatformId`***   | Literal |   4   | Number            |                                                  |      |
| ***`OSMajorVersion`*** | Literal |   4   | Number            |                                                  |      |
| ***`OSMinorVersion`*** | Literal |   4   | Number            |                                                  |      |
| ***`AssemblyRef`***    |  Index  |   4   | **`AssemblyRef`** | 索引引用的程序集。                               |      |

这些记录不应被发出到任何 PE 文件中。然而，如果它们出现在 PE 文件中，它们应被视为其字段都为零。CLI 应忽略它们。

>---
### 20.19. File: 0x26
<a id="File_0x26"></a>

> *File* 表有以下列：

| Column            |  Kind   | Size  | Value             | Description                                      | Link                                |
| :---------------- | :-----: | :---: | :---------------- | :----------------------------------------------- | :---------------------------------- |
| ***`Token`***     | Literal |   4   | 26UUUUUU          | 行编号，高位字节表示表编号，低三位字节是行编号。 |                                     |
| ***`Flags`***     | BitMask |   4   | _FileAttributes_  | _FileAttributes_ 类型的位掩码。                  | [_FileAttributes_](#FileAttributes) |
| ***`Name`***      |  Index  |   4   | ***String Heap*** | 索引引用文件的名称标识。                         |                                     |
| ***`HashValue`*** |  Index  |   4   | ***Blob Heap***   | 索引引用文件的哈希值二进制数据对象。             |                                     |

**`File`** 表的行是程序集中的 **.file** 指令的结果 [[↗]](#file)。 

> 元数据验证规则

| Order | Validation Rule                                                                                                                                            |  Level  |
| :---: | :--------------------------------------------------------------------------------------------------------------------------------------------------------- | :-----: |
|  1.   | ***`Flags`*** 只应设置那些指定的值 (所有组合有效)。                                                                                                        |  ERROR  |
|  2.   | ***`Name`*** 应该索引 ***String Heap*** 中的 non-empty 字符串。它应该是 `<filename>.<extension>` 的格式 (例如，"`foo.dll`"，但不是 "`c:\utils\foo.dll`")。 |  ERROR  |
|  3.   | ***`HashValue`*** 应该索引 ***Blob Heap*** 中的非空 '*blob*'。                                                                                             |  ERROR  |
|  4.   | 不应该有重复的具有相同 ***`Name`*** 值的行。                                                                                                               |  ERROR  |
|  5.   | 如果此模块包含 **`Assembly`** 表中的一行 (也就是说，如果此模块 “持有清单”)，那么 **`File`** 表中不应该有任何关于此模块的行；也就是说，没有自引用。         |  ERROR  |
|  6.   | 如果 **`File`** 表为空，那么按定义，这是一个单文件程序集。在这种情况下，**`ExportedType`** 表应该为空。                                                    | WARNING |

>---
### 20.14. ExportedType: 0x27
<a id="ExportedType_0x27"></a>

**`ExportedType`** 表为每种类型保存一行：

 1. 在此程序集的其他模块中定义；也就是说，从此程序集中导出。本质上，它存储了此程序集包含的其他模块中所有标记为公共的类型的 **`TypeDef`** 行号。
    
    实际的目标行在 **`TypeDef`** 表中由 ***`TypeDefId`*** (实际上是行号) 和 ***`Implementation`*** (实际上是持有目标 **`TypeDef`** 表的模块) 的组合给出。注意，这是元数据中外部 **_token_** 的唯一出现；也就是说， **_token_** 值在另一个模块中有意义。常规 **_token_** 值是对当前模块中的表的索引；或者

 2. 最初在此程序集中定义，但现在已移至另一个程序集。***`Flags`*** 必须设置 *`IsTypeForwarder`*，并且 ***`Implementation`*** 是一个 **`AssemblyRef`**，表示现在可以在另一个程序集中找到该类型。

类型的全名不需要直接存储。相反，它可以在任何包含的 "." 处分成两部分 (尽管通常这是在全名中的最后一个 "." 处完成的)。"." 前面的部分存储为 ***`TypeNamespace`***，"." 后面的部分存储为 ***`TypeName`***。如果全名中没有 "."，那么 ***`TypeNamespace`*** 应该是空字符串的索引。

> *ExportedType* 表有以下列：

| Column                 |  Kind   | Size  | Value             | Description                                                                                                                                                                                                                                                                                                                  | Link                                 |
| :--------------------- | :-----: | :---: | :---------------- | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :----------------------------------- |
| ***`Token`***          | Literal |   4   | 27UUUUUU          | 行编号，高位字节表示表编号，低三位字节是行编号。                                                                                                                                                                                                                                                                             |                                      |
| ***`Flags`***          | BitMask |   4   | _TypeAttributes_  | _TypeAttributes_ 类型的位掩码。                                                                                                                                                                                                                                                                                              | [_TypeAttributes_](#TypeAttributes)  |
| ***`TypeDefId`***      |  Index  |   4   | **`TypeDef`**     | 指向此程序集的另一个模块中的 **`TypeDef`** 表。此列仅用作提示。如果目标 **`TypeDef`** 表中的条目与此表中的 ***`TypeName`*** 和 ***`TypeNamespace`*** 条目匹配，则解析成功。但是，如果不匹配，CLI 将回退到目标 **`TypeDef`** 表的搜索。如果 ***`Flags`*** 设置了 *`IsTypeForwarder`*，则忽略并应为零。                        |                                      |
| ***`TypeName`***       |  Index  |   4   | ***String Heap*** | 索引类型的名称标识。                                                                                                                                                                                                                                                                                                         |                                      |
| ***`TypeNamespace`***  |  Index  |   4   | ***String Heap*** | 索引类型所属空间的名称标识。                                                                                                                                                                                                                                                                                                 |                                      |
| ***`Implementation`*** |  Index  |   4   | Tables            | 这是一个指向以下表中的任何一个的索引：<br>&emsp;i. **`File`** 表，该条目说明当前程序集中的哪个模块持有 **`TypeDef`**。<br>&emsp;ii. **`ExportedType`** 表，该条目是当前嵌套类型的封闭类型。<br>&emsp;iii. **`AssemblyRef`** 表，该条目说明在哪个程序集中现在可以找到类型 (***`Flags`*** 必须设置 *`IsTypeForwarder`* 标志)。 | [*Implementation*](#Implementation) |

**`ExportedType`** 表中的行是 **.class extern** 指令的结果 [[↗]](#class-extern)。

> 元数据验证规则

术语 "_FullName_" 指的是以下方式创建的字符串：如果 ***`TypeNamespace`*** 为空，则使用 ***`TypeName`***，否则使用 ***`TypeNamespace`***"."***`TypeName`*** 的连接。

| Order | Validation Rule                                                                                                                                                                                                                                                                                         |  Level  |
| :---: | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | :-----: |
|  1.   | **`ExportedType`** 表可以包含零行或多行。                                                                                                                                                                                                                                                               |
|  2.   | **`ExportedType`** 表中不应该有在当前模块中定义的类型的条目 —— 只有在程序集中的其他模块中定义的类型。                                                                                                                                                                                                   |  ERROR  |
|  3.   | ***`Flags`*** 只应设置那些指定的值。                                                                                                                                                                                                                                                                    |  ERROR  |
|  4.   | 如果 ***`Implementation`*** 索引 **`File`** 表，那么 ***`Flags`***.*`VisibilityMask`* 应该是 *`Public`* [[↗]](#TypeAttributes)。                                                                                                                                                                        |  ERROR  |
|  5.   | 如果 ***`Implementation`*** 索引 **`ExportedType`** 表，那么 ***`Flags`***.*`VisibilityMask`* 应该是 *`NestedPublic`* [[↗]](#TypeAttributes)。                                                                                                                                                          |  ERROR  |
|  6.   | 如果非空，***`TypeDefId`*** 应该索引此程序集中的某个模块 (但不是此模块) 中的 **`TypeDef`** 表中的有效行，且所索引的行应该有其 ***`Flags`***.*`Public`* = 1 [[↗]](#TypeAttributes)。                                                                                                                     | WARNING |
|  7.   | ***`TypeName`*** 应该索引 ***String Heap*** 中的 non-empty 字符串。                                                                                                                                                                                                                                     |  ERROR  |
|  8.   | ***`TypeNamespace`*** 可以为空，或非空。                                                                                                                                                                                                                                                                |
|  9.   | 如果 ***`TypeNamespace`*** 是非空的，那么它应该索引 ***String Heap*** 中的 non-empty 字符串。                                                                                                                                                                                                           |  ERROR  |
|  10.  | _FullName_ 应该是一个有效的 CLS 标识符。                                                                                                                                                                                                                                                                |   CLS   |
|  11.  | 如果这是一个嵌套类型，那么 ***`TypeNamespace`*** 应该是空的，***`TypeName`*** 应该表示嵌套类型的未混淆的简单名称。                                                                                                                                                                                      |  ERROR  |
|  12.  | ***`Implementation`*** 应该是一个有效的索引，指向以下任何一个：                                                                                                                                                                                                                                         |  ERROR  |
|       | * **`File`** 表；该文件应该在其 **`TypeDef`** 表中持有目标类型的定义。                                                                                                                                                                                                                                  |
|       | * 当前 **`ExportedType`** 表中的不同行 —— 这标识了当前嵌套类型的封闭类型。                                                                                                                                                                                                                              |
|  13.  | **_FullName_** 应该与 ***`TypeDefId`*** 索引的 **`TypeDef`** 表中的行的相应 **_FullName_** 完全匹配。                                                                                                                                                                                                   |  ERROR  |
|  14.  | 忽略嵌套类型时，基于 **_FullName_** 不应该有重复的行。                                                                                                                                                                                                                                                  |  ERROR  |
|  15.  | 对于嵌套类型，基于 ***`TypeName`*** 和封闭类型不应该有重复的行。                                                                                                                                                                                                                                        |  ERROR  |
|  16.  | 从当前程序集导出的类型的完整列表是 **`ExportedType`** 表与当前 **`TypeDef`** 表中所有公共类型的连接，其中 "public" 指的是 ***`Flags`***.*`VisibilityMask`* 是 *`Public`* 或 *`NestedPublic`*。在这个连接表中，基于 **_FullName_** (如果这是一个嵌套类型，将封闭类型添加到重复检查中) 不应该有重复的行。 |  ERROR  |

>---
### 20.24. ManifestResource: 0x28
<a id="ManifestResource_0x28"></a>

> *ManifestResource* 表有以下列：

| Column                               |  Kind   | Size  | Value                        | Description                                       | Link                                                        |
| :----------------------------------- | :-----: | :---: | :--------------------------- | :------------------------------------------------ | :---------------------------------------------------------- |
| ***`Token`***                        | Literal |   4   | 28UUUUUU                     | 行编号，高位字节表示表编号，低三位字节是行编号。  |                                                             |
| ***`Offset`***                       | Literal |   4   | Offset                       | 此资源记录开始的引用文件内的字节偏移量。          |                                                             |
| ***`Flags`***<br>/***`Attributes`*** | BitMask |   4   | _ManifestResourceAttributes_ | _ManifestResourceAttributes_ 类型的位掩码。       | [_ManifestResourceAttributes_](#ManifestResourceAttributes) |
| ***`Name`***                         |         |   4   | ***String Heap***            | 索引清单资源的名称标识。                          |                                                             |
| ***`Implementation`***               |         |   4   | Tables                       | 索引 **`File`** 表、**`AssemblyRef`** 表或 null。 | [*Implementation*](#Implementation)                        |

***`Offset`*** 指定此资源记录开始的引用文件内的字节偏移量。***`Implementation`*** 指定哪个文件持有此资源。

表中的行是程序集上的 **.mresource** 指令的结果 [[↗]](#mresource)。

> 元数据验证规则

| Order | Validation Rule                                                                                                       | Level |
| :---: | :-------------------------------------------------------------------------------------------------------------------- | :---: |
|  1.   | **`ManifestResource`** 表可以包含零行或多行。                                                                         |
|  2.   | ***`Offset`*** 应该是目标文件中的有效偏移量，从 CLI 头部的资源条目开始。                                              | ERROR |
|  3.   | ***`Flags`*** 只应设置那些指定的值。                                                                                  | ERROR |
|  4.   | ***`Flags`*** 的 *`VisibilityMask`* 子字段 [[↗]](#ManifestResourceAttributes) 应该是 `Public` 或 `Private` 中的一个。 | ERROR |
|  5.   | ***`Name`*** 应该索引 ***String Heap*** 中的 non-empty 字符串。                                                       | ERROR |
|  6.   | ***`Implementation`*** 可以为空或非空 (如果为空，表示资源存储在当前文件中)。                                          |
|  7.   | 如果 ***`Implementation`*** 为空，那么 ***`Offset`*** 应该是当前文件中的有效偏移量，从 CLI 头部的资源条目开始。       | ERROR |
|  8.   | 如果 ***`Implementation`*** 非空，那么它应该索引 **`File`** 或 **`AssemblyRef`** 表中的有效行。                       | ERROR |
|  9.   | 基于 ***`Name`*** 不应该有重复的行。                                                                                  | ERROR |
|  10.  | 如果资源是 **`File`** 表中的索引，***`Offset`*** 应该为零。                                                           | ERROR |

>---
### 20.32. NestedClass: 0x29
<a id="NestedClass_0x29"></a>

> *NestedClass* 表有以下列：

| Column                 |  Kind   | Size  | Value         | Description                                      | Link |
| :--------------------- | :-----: | :---: | :------------ | :----------------------------------------------- | :--- |
| ***`Token`***          | Literal |   4   | 29UUUUUU      | 行编号，高位字节表示表编号，低三位字节是行编号。 |      |
| ***`NestedClass`***    |  Index  |   4   | **`TypeDef`** | 索引嵌套类型的类型定义。                         |      |
| ***`EnclosingClass`*** |  Index  |   4   | **`TypeDef`** | 索引嵌套类型的封闭类型定义 。                    |

***`NestedClass`*** 被定义为在其封闭类型的文本 “内部”。**`NestedClass`** 表记录哪些类型定义嵌套在哪些其他类型定义中。在典型的高级语言中，嵌套类被定义为在其封闭类型的文本  “内部”。

> 元数据验证规则

| Order | Validation Rule                                                                                                                                   |  Level  |
| :---: | :------------------------------------------------------------------------------------------------------------------------------------------------ | :-----: |
|  1.   | **`NestedClass`** 表可以包含零行或多行。                                                                                                          |
|  2.   | ***`NestedClass`*** 应索引 **`TypeDef`** 表中的有效行。                                                                                           |  ERROR  |
|  3.   | ***`EnclosingClass`*** 应索引 **`TypeDef`** 表中的有效行 (特别注意，不允许索引 **`TypeRef`** 表)。                                                |  ERROR  |
|  4.   | 不应有重复行 (即 ***`NestedClass`*** 和 ***`EnclosingClass`*** 的值相同)。                                                                        | WARNING |
|  5.   | 给定类型只能由一个封闭器嵌套。因此，不能有两行具有相同的 ***`NestedClass`*** 值，但它们的 ***`EnclosingClass`*** 值不同。                         |  ERROR  |
|  6.   | 给定类型可以 “拥有” 几种不同的嵌套类型，因此具有两行或多行具有相同的 ***`EnclosingClass`*** 值，但 ***`NestedClass`*** 值不同的情况是完全有效的。 |

>---
### 20.20. GenericParam: 0x2A
<a id="GenericParam_0x2A"></a>

> *GenericParam* 表有以下列：

| Column                               |  Kind   | Size  | Value                    | Description                                                                    | Link                                                |
| :----------------------------------- | :-----: | :---: | :----------------------- | :----------------------------------------------------------------------------- | :-------------------------------------------------- |
| ***`Token`***                        | Literal |   4   | 2AUUUUUU                 | 行编号，高位字节表示表编号，低三位字节是行编号。                               |                                                     |
| ***`Number`***                       | Literal |   2   | Number Index             | 表示泛型参数的索引编号。                                                       |                                                     |
| ***`Flags`***<br>/***`Attributes`*** | BitMask |   2   | _GenericParamAttributes_ | _GenericParamAttributes_ 类型的位掩码。                                        | [_GenericParamAttributes_](#GenericParamAttributes) |
| ***`Owner`***                        |  Index  |   4   | Tables                   | 索引 **`TypeDef`** 或 **`MethodDef`** 表，指定此泛型参数适用的类型或方法。     | [_TypeOrMethodDef_](#TypeOrMethodDef)               |
| ***`Name`***                         |  Index  |   4   | ***String Heap***        | 非空索引值。索引泛型参数的名称。这完全是描述性的，只由源语言编译器和反射使用。 |                                                     |

以下是其他的限制：

 * ***`Owner`*** 不能是非嵌套的枚举类型；并且
 * 如果 ***`Owner`*** 是嵌套的枚举类型，那么 ***`Number`*** 必须小于或等于封闭类的泛型参数的数量。

泛型枚举类型的作用很小，通常只存在于满足 CLS Rule 42。这些额外的限制约束了枚举类型的通用性，同时允许满足 CLS Rule 42。**`GenericParam`** 表存储了在泛型类型定义和泛型方法定义中使用的泛型参数。这些泛型参数可以被约束 (即，泛型参数应扩展某个类和 / 或实现某些接口) 或无约束。这样的约束存储在 **`GenericParamConstraint`** 表中。 

从概念上讲，**`GenericParam`** 表中的每一行都属于 **`TypeDef`** 或 **`MethodDef`** 表中的一行，且只有一行拥有。

 ```cil
 .class Dict`2<([mscorlib]System.IComparable) K, V>
 ```

类 `Dict` 的泛型参数 `K` 被约束为实现 `System.IComparable`。

 ```cil
 .method static void ReverseArray<T>(!!0[] 'array')
 ```

泛型方法 `ReverseArray` 的泛型参数 `T` 没有约束。 

> 元数据验证规则

| Order | Validation Rule                                                                                                             | Level |
| :---: | :-------------------------------------------------------------------------------------------------------------------------- | :---: |
|  1.   | **`GenericParam`** 表可以包含零行或多行。                                                                                   |
|  2.   | 每一行应有一个，且只有一个，在 **`TypeDef`** 或 **`MethodDef`** 表中的 *owner* 行 (即，没有行共享)。                        | ERROR |
|  3.   | 每个泛型类型应在 **`GenericParam`** 表中为其每个泛型参数拥有一行。                                                          | ERROR |
|  4.   | 每个泛型方法应在 **`GenericParam`** 表中为其每个泛型参数拥有一行。                                                          | ERROR |
|  5.   | ***`Flags`*** 可以持有 *`Covariant`* 或 *`Contravariant`* 的值，但只有当 *owner* 行对应于泛型接口或泛型委托类时才能这样做。 | ERROR |
|  6.   | 否则，***`Flags`*** 应持有 *`None`* 值，表示非变量 (即，参数是非变量或 *owner* 是非委托类、值类型或泛型方法)。              | ERROR |
|  7.   | 如果 ***`Flags`*** == *`Covariant`*，那么相应的泛型参数只能作为以下内容出现在类型定义中：                                   | ERROR |
|       | * 方法的结果类型                                                                                                            |
|       | * 继承接口的泛型参数                                                                                                        |
|  8.   | 如果 ***`Flags`*** == *`Contravariant`*，那么相应的泛型参数只能作为方法的参数出现在类型定义中。                             | ERROR |
|  9.   | ***`Number`*** 应有一个值 &ge; 0 且 <  *owner* 类型或方法的泛型参数的数量。                                                 | ERROR |
|  10.  | 同一方法拥有的 **`GenericParam`** 表中的连续行应按照 ***`Number`*** 值的增加顺序排序；***`Number`*** 序列中不应有间隙。     | ERROR |
|  11.  | ***`Name`*** 是索引 ***String Heap*** 中的 no-empty 字符串                                                                  | ERROR |
|  12.  | 基于 ***`Owner`***+***`Name`***，不应有重复的行 。                                                                          | ERROR |
|  13.  | 基于 ***`Owner`***+***`Number`***，不应有重复的行。                                                                         | ERROR |

>---
### 20.29. MethodSpec: 0x2B
<a id="MethodSpec0x2B"></a>

> *MethodSpec* 表有以下列：

| Column                                      |  Kind   | Size  | Value           | Description                                                                                            | Link                                 |
| :------------------------------------------ | :-----: | :---: | :-------------- | :----------------------------------------------------------------------------------------------------- | :----------------------------------- |
| ***`Token`***                               | Literal |   4   | 2BUUUUUU        | 行编号，高位字节表示表编号，低三位字节是行编号。                                                       |                                      |
| ***`Method`***                              |  Index  |   4   | Tables          | 索引 **`MethodDef`** 或 **`MemberRef`** 表。指定此行引用的泛型方法；也就是说，此行是哪个泛型方法的实例 | [_MethodDefOrRef_](#MethodDefOrRef) |
| ***`Instantiation`***<br>/***`Signature`*** |  Index  |   4   | ***Blob Heap*** | 索引方法特殊签名的二进制数据对象。                                                                     | [*MethodSpec*](#MethodSpec-blob)     |

**`MethodSpec`** 表记录实例化泛型方法的签名。泛型方法的每个唯一实例 (即，***`Method`*** 和 ***`Instantiation`*** 的组合) 应由表中的单个行表示。

> 元数据验证规则

| Order | Validation Rule                                                                                      | Level |
| :---: | :--------------------------------------------------------------------------------------------------- | :---: |
|  1.   | **`MethodSpec`** 表可以包含零行或多行。                                                              |
|  2.   | 一个或多个行可以引用 **`MethodDef`** 或 **`MemberRef`** 表中的相同行。可以有同一泛型方法的多个实例。 |
|  3.   | 存储在 ***`Instantiation`*** 中的签名应为 ***`Method`*** 存储的泛型方法的签名的有效实例。            | ERROR |
|  4.   | 基于 ***`Method`***+***`Instantiation`*** 不应有重复行。                                             | ERROR |

>---
### 20.21. GenericParamConstraint: 0x2C
<a id="GenericParamConstraint_0x2C"></a>

> *GenericParamConstraint* 表有以下列：

| Column                              |  Kind   | Size  | Value              | Description                                                                                                                  | Link                               |
| :---------------------------------- | :-----: | :---: | :----------------- | :--------------------------------------------------------------------------------------------------------------------------- | :--------------------------------- |
| ***`Token`***                       | Literal |   4   | 2CUUUUUU           | 行编号，高位字节表示表编号，低三位字节是行编号。                                                                             |                                    |
| ***`Owner`***                       |  Index  |   4   | **`GenericParam`** | 索引此行引用的泛型参数。                                                                                                     |                                    |
| ***`Constraint`***<br>/***`Type`*** |  Index  |   4   | Tables             | 索引 **`TypeDef`**，**`TypeRef`** 或 **`TypeSpec`** 表，指定此泛型参数受限于应从哪个类派生，或此泛型参数受限于实现哪个接口。 | [_TypeDefOrRef_](#TypeDefOrRef) |

**`GenericParamConstraint`** 表记录每个泛型参数的约束。每个泛型参数可以约束为从零个或一个类派生。每个泛型参数可以约束为实现零个或多个接口。

从概念上讲，**`GenericParamConstraint`** 表中的每一行都由 **`GenericParam`** 表中的一行 “拥有”。给定 ***`Owner`*** 的 **`GenericParamConstraint`** 表中的所有行应引用不同的约束。

请注意，如果 ***`Constraint`*** 是对 `System.ValueType` 的 **`TypeRef`**，那么它意味着约束类型应为 `System.ValueType`，或其子类型之一。然而，由于 `System.ValueType` 本身是引用类型，这种特定机制并不能保证类型是非引用类型。


> 元数据验证规则

| Order | Validation Rule                                                                                                            | Level |
| :---: | :------------------------------------------------------------------------------------------------------------------------- | :---: |
|  1.   | **`GenericParamConstraint`** 表可以包含零行或多行。                                                                        |
|  2.   | 每一行在 **`GenericParam`** 表中都应有一个且只有一个 *owner* 行 (即，没有行共享)  。                                       | ERROR |
|  3.   | **`GenericParam`** 表中的每一行应 “拥有” **`GenericParamConstraint`** 表中的一个单独行，对应于该泛型参数具有的每个约束。   | ERROR |
|  4.   | 在 **`GenericParam`** 表中的给定行拥有的 **`GenericParamConstraint`** 表中的所有行应形成一个连续的范围 (行)。              | ERROR |
|  5.   | 任何泛型参数 (对应于 **`GenericParam`** 表中的一行) 应拥有 **`GenericParamConstraint`** 表中的零行或一行，对应于类约束。   | ERROR |
|  6.   | 任何泛型参数 (对应于 **`GenericParam`** 表中的一行) 应拥有 **`GenericParamConstraint`** 表中的零行或多行，对应于接口约束。 | ERROR |
|  7.   | 基于 ***`Owner`***+***`Constraint`*** 不应有重复行。                                                                       | ERROR |
|  8.   | 约束不应引用 `System.Void`。                                                                                               | ERROR |

---
## 21. 元数据逻辑格式：子结构
<a id="metadata-format-others"></a>

>---
### 21.1. BitMasks & Flags

此小节解释了元数据表中使用的 ***Flags*** 和 ***BitMasks***。当符合规范的实现遇到未在此标准中指定的元数据结构 (如 *Flags*) 时，实现的行为是未指定的。

#### 21.1.1. AssemblyHashAlgorithm
<a id="AssemblyHashAlgorithm-value"></a>

 | Algorithm              | Value  |
 | :--------------------- | :----: |
 | *`None`*               | 0x0000 |
 | *`Reserved`* (*`MD5`*) | 0x8003 |
 | *`SHA1`*               | 0x8004 |

#### 21.1.2. AssemblyFlags
<a id="AssemblyFlags"></a>

 | Flag                           | Value  | Description                                                                                                                         |
 | :----------------------------- | :----: | :---------------------------------------------------------------------------------------------------------------------------------- |
 | *`PublicKey`*                  | 0x0001 | 程序集引用包含完整的 (未哈希的) 公钥。                                                                                              |
 | *`Retargetable`*               | 0x0100 | 运行时使用的此程序集的实现不预期与编译时看到的版本匹配。(参见此表后的文本。)                                                        |
 | *`DisableJITcompileOptimizer`* | 0x4000 | 保留 (符合 CLI 的规范的实现可以在读取时忽略此设置；一些实现可能使用此位来指示 CIL-to-native-code 编译器不应生成优化的代码)          |
 | *`EnableJITcompileTracking`*   | 0x8000 | 保留 (符合 CLI 的规范的实现可以在读取时忽略此设置；一些实现可能使用此位来指示 CIL-to-native-code 编译器应生成 CIL 到本地代码的映射) |

#### 21.1.3. Culture 
<a id="Culture-values"></a>

 |  Cultures  |   &nbsp;   | &nbsp;  |   &nbsp;   |   &nbsp;   |  &nbsp;  |   &nbsp;   | &nbsp;  |
 | :--------: | :--------: | :-----: | :--------: | :--------: | :------: | :--------: | :-----: |
 |  `ar-SA`   |  `ar-IQ`   | `ar-EG` |  `ar-LY`   |  `ar-DZ`   | `ar-MA`  |  `ar-TN`   | `ar-OM` |
 |  `ar-YE`   |  `ar-SY`   | `ar-JO` |  `ar-LB`   |  `ar-KW`   | `ar-AE`  |  `ar-BH`   | `ar-QA` |
 |  `bg-BG`   |  `ca-ES`   | `zh-TW` |  `zh-CN`   |  `zh-HK`   | `zh-SG`  |  `zh-MO`   | `cs-CZ` |
 |  `da-DK`   |  `de-DE`   | `de-CH` |  `de-AT`   |  `de-LU`   | `de-LI`  |  `el-GR`   | `en-US` |
 |  `en-GB`   |  `en-AU`   | `en-CA` |  `en-NZ`   |  `en-IE`   | `en-ZA`  |  `en-JM`   | `en-CB` |
 |  `en-BZ`   |  `en-TT`   | `en-ZW` |  `en-PH`   | `es-ES-Ts` | `es-MX`  | `es-ES-Is` | `es-GT` |
 |  `es-CR`   |  `es-PA`   | `es-DO` |  `es-VE`   |  `es-CO`   | `es-PE`  |  `es-AR`   | `es-EC` |
 |  `es-CL`   |  `es-UY`   | `es-PY` |  `es-BO`   |  `es-SV`   | `es-HN`  |  `es-NI`   | `es-PR` |
 |  `fi-FI`   |  `fr-FR`   | `fr-BE` |  `fr-CA`   |  `fr-CH`   | `fr-LU`  |  `fr-MC`   | `he-IL` |
 |  `hu-HU`   |  `is-IS`   | `it-IT` |  `it-CH`   |  `ja-JP`   | `ko-KR`  |  `nl-NL`   | `nl-BE` |
 |  `nb-NO`   |  `nn-NO`   | `pl-PL` |  `pt-BR`   |  `pt-PT`   | `ro-RO`  |  `ru-RU`   | `hr-HR` |
 | `lt-sr-SP` | `cy-sr-SP` | `sk-SK` |  `sq-AL`   |  `sv-SE`   | `sv-FI`  |  `th-TH`   | `tr-TR` |
 |  `ur-PK`   |  `id-ID`   | `uk-UA` |  `be-BY`   |  `sl-SI`   | `et-EE`  |  `lv-LV`   | `lt-LT` |
 |  `fa-IR`   |  `vi-VN`   | `hy-AM` | `lt-az-AZ` | `cy-az-AZ` | `eu-ES`  |  `mk-MK`   | `af-ZA` |
 |  `ka-GE`   |  `fo-FO`   | `hi-IN` |  `ms-MY`   |  `ms-BN`   | `kk-KZ`  |  `ky-KZ`   | `sw-KE` |
 | `lt-uz-UZ` | `cy-uz-UZ` | `tt-TA` |  `pa-IN`   |  `gu-IN`   | `ta-IN`  |  `te-IN`   | `kn-IN` |
 |  `mr-IN`   |  `sa-IN`   | `mn-MN` |  `gl-ES`   |  `kok-IN`  | `syr-SY` |  `div-MV`  |

**关于 RFC 1766，区域名称的注释**：典型的字符串将是 "``en-US``"。第一部分 (例子中的"`en`") 使用 ISO 639 字符 (小写的拉丁字母字符。不使用带有变音符号的或修改过的字符)。第二部分 (例子中的 "`US`") 使用 ISO 3166 字符 (类似于 ISO 639，但是大写) ；也就是说，ASCII 字符 `a` ~ `z` 和 `A` ~ `Z`。然而，虽然 RFC 1766 建议第一部分使用小写，第二部分使用大写，但它允许混合大小写。因此，验证规则只检查 ***`Culture`*** 是否是上面列表中的字符串之一 — 但是检查是完全 “不区分大小写” 的。

#### 21.1.4. Flags for Events [EventAttributes]
<a id="EventAttributes"></a>

 | Flag              | Value  | Description                            |
 | :---------------- | :----: | :------------------------------------- |
 | *`SpecialName`*   | 0x0200 | 事件是特殊的。                         |
 | *`RTSpecialName`* | 0x0400 | CLI 提供 '特殊' 行为，取决于事件的名称 |

#### 21.1.5. Fields [FieldAttributes]
<a id="FieldAttributes"></a>

  | Flag                         | Value  | Description                                       |
  | :--------------------------- | :----: | :------------------------------------------------ |
  | *`FieldAccessMask`*          | 0x0007 | 这 3 位包含以下 Value 之一：                      |
  | &emsp;*`CompilerControlled`* | 0x0000 | 成员不可引用                                      |
  | &emsp;*`Private`*            | 0x0001 | 仅父类型可访问                                    |
  | &emsp;*`FamANDAssem`*        | 0x0002 | 仅类型和此程序集内部的子类型可访问                |
  | &emsp;*`Assembly`*           | 0x0003 | 程序集内部可访问                                  |
  | &emsp;*`Family`*             | 0x0004 | 仅类型和子类型可访问                              |
  | &emsp;*`FamORAssem`*         | 0x0005 | 任意子类型或程序集内部可访问                      |
  | &emsp;*`Public`*             | 0x0006 | 公共可访问性                                      |
  | **Field modifiers**          | &nbsp; | &nbsp;                                            |
  | &emsp;*`Static`*             | 0x0010 | 在类型上定义静态                                  |
  | &emsp;*`InitOnly`*           | 0x0020 | 字段仅初始化，初始化后不能写入                    |
  | &emsp;*`Literal`*            | 0x0040 | 字段值是编译时常量                                |
  | &emsp;*`NotSerialized`*      | 0x0080 | 保留 (用于指示当类型被远程化时，不应序列化此字段) |
  | &emsp;*`SpecialName`*        | 0x0200 | 字段是特殊的                                      |
  | **Interop attributes**       | &nbsp; | &nbsp;                                            |
  | &emsp;*`PInvokeImpl`*        | 0x2000 | 实现通过 PInvoke 转发。                           |
  | **Additional flags**         | &nbsp; | &nbsp;                                            |
  | &emsp;*`RTSpecialName`*      | 0x0400 | CLI 提供 '特殊' 行为，取决于字段的名称            |
  | &emsp;*`HasFieldMarshal`*    | 0x1000 | 字段有封送处理信息                                |
  | &emsp;*`HasDefault`*         | 0x8000 | 字段有默认值                                      |
  | &emsp;*`HasFieldRVA`*        | 0x0100 | 字段有 RVA                                        |


#### 21.1.6. Files [FileAttributes]
<a id="FileAttributes"></a>

 | Flag                   | Value  | Description                              |
 | :--------------------- | :----: | :--------------------------------------- |
 | *`ContainsMetaData`*   | 0x0000 | 这不是一个资源文件                       |
 | *`ContainsNoMetaData`* | 0x0001 | 这是一个资源文件或其他不包含元数据的文件 |

#### 21.1.7. Generic Parameters [GenericParamAttributes]
<a id="GenericParamAttributes"></a>

| Flag                                     | Value  | Description                        |
| :--------------------------------------- | :----: | :--------------------------------- |
| *`VarianceMask`*                         | 0x0003 | 这两位包含以下 Value 之一：        |
| &emsp;*`None`*                           | 0x0000 | 泛型参数是非变体，并且没有特殊约束 |
| &emsp;*`Covariant`*                      | 0x0001 | 泛型参数是协变的                   |
| &emsp;*`Contravariant`*                  | 0x0002 | 泛型参数是逆变的                   |
| *`SpecialConstraintMask`*                | 0x001C | 这三位包含以下 Value 之一：        |
| &emsp;*`ReferenceTypeConstraint`*        | 0x0004 | 泛型参数具有 `class` 特殊约束      |
| &emsp;*`NotNullableValueTypeConstraint`* | 0x0008 | 泛型参数具有 `valuetype` 特殊约束  |
| &emsp;*`DefaultConstructorConstraint`*   | 0x0010 | 泛型参数具有 `.ctor` 特殊约束      |

#### 21.1.8. ImplMap [PInvokeAttributes]
<a id="PInvokeAttributes"></a>
 | Flag                          | Value  | Description                                                         |
 | :---------------------------- | :----: | :------------------------------------------------------------------ |
 | *`NoMangle`*                  | 0x0001 | PInvoke 将使用指定的成员名称                                        |
 | **Character set**             | &nbsp; | &nbsp;                                                              |
 | *`CharSetMask`*               | 0x0006 | 这是一个资源文件或其他不包含元数据的文件。这两位包含以下Value之一： |
 | &emsp;*`CharSetNotSpec`*      | 0x0000 | &nbsp;                                                              |
 | &emsp;*`CharSetAnsi`*         | 0x0002 | &nbsp;                                                              |
 | &emsp;*`CharSetUnicode`*      | 0x0004 | &nbsp;                                                              |
 | &emsp;*`CharSetAuto`*         | 0x0006 | &nbsp;                                                              |
 | *`SupportsLastError`*         | 0x0040 | 关于目标函数的信息。对字段不相关                                    |
 | **Calling convention**        | &nbsp; | &nbsp;                                                              |
 | *`CallConvMask`*              | 0x0700 | 这三位包含以下 Value 之一：                                         |
 | &emsp;*`CallConvPlatformapi`* | 0x0100 | &nbsp;                                                              |
 | &emsp;*`CallConvCdecl`*       | 0x0200 | &nbsp;                                                              |
 | &emsp;*`CallConvStdcall`*     | 0x0300 | &nbsp;                                                              |
 | &emsp;*`CallConvThiscall`*    | 0x0400 | &nbsp;                                                              |
 | &emsp;*`CallConvFastcall`*    | 0x0500 | &nbsp;                                                              |

#### 21.1.9. ManifestResource [ManifestResourceAttributes]
<a id="ManifestResourceAttributes"></a>

 | Flag               | Value  | Description                 |
 | :----------------- | :----: | :-------------------------- |
 | *`VisibilityMask`* | 0x0007 | 这三位包含以下 Value 之一： |
 | &emsp;*`Public`*   | 0x0001 | 资源从程序集中导出          |
 | &emsp;*`Private`*  | 0x0002 | 资源对程序集是私有的        |

#### 21.1.10. Methods [MethodAttributes]
<a id="MethodAttributes"></a>

 | Flag                         | Value  | Description                                                 |
 | :--------------------------- | :----: | :---------------------------------------------------------- |
 | *`MemberAccessMask`*         | 0x0007 | 这3位包含以下 Value 之一：                                  |
 | &emsp;*`CompilerControlled`* | 0x0000 | 成员不可引用                                                |
 | &emsp;*`Private`*            | 0x0001 | 仅父类型可访问                                              |
 | &emsp;*`FamANDAssem`*        | 0x0002 | 仅此程序集中的子类型可访问                                  |
 | &emsp;*`Assem`*              | 0x0003 | 程序集内部可访问                                            |
 | &emsp;*`Family`*             | 0x0004 | 仅类型和子类型可访问                                        |
 | &emsp;*`FamORAssem`*         | 0x0005 | 任意子类型或者程序集内部可访问                              |
 | &emsp;*`Public`*             | 0x0006 | 公共可访问性                                                |
 | **Member modifiers**         |        |                                                             |
 | *`Static`*                   | 0x0010 | 在类型上定义，否则每个实例                                  |
 | *`Final`*                    | 0x0020 | 方法不能被重写                                              |
 | *`Virtual`*                  | 0x0040 | 方法是虚拟的                                                |
 | *`HideBySig`*                | 0x0080 | 方法通过名称+签名隐藏，否则只通过名称隐藏                   |
 | *`Strict`*                   | 0x0200 | 方法只有在也可访问时才能被重写                              |
 | *`Abstract`*                 | 0x0400 | 方法不提供实现                                              |
 | *`SpecialName`*              | 0x0800 | 方法是特殊的                                                |
 | *`VtableLayoutMask`*         | 0x0100 | 使用此掩码检索 ***vtable*** 特性。此位包含以下 Value 之一： |
 | &emsp;*`ReuseSlot`*          | 0x0000 | 方法重用 ***vtable*** 中的现有槽                            |
 | &emsp;*`NewSlot`*            | 0x0100 | 方法总是在 ***vtable*** 中获取新槽                          |
 | **Interop attributes**       | &nbsp; | &nbsp;                                                      |
 | *`PInvokeImpl`*              | 0x2000 | 实现通过 PInvoke 转发                                       |
 | *`UnmanagedExport`*          | 0x0008 | 保留：对于符合规范的实现，应为零                            |
 | **Additional flags**         | &nbsp; | &nbsp;                                                      |
 | *`RTSpecialName`*            | 0x1000 | CLI 提供 '特殊' 行为，取决于方法的名称                      |
 | *`HasSecurity`*              | 0x4000 | 方法与其关联的安全性                                        |
 | *`RequireSecObject`*         | 0x8000 | 方法调用包含安全代码的另一种方法                            |

#### 21.1.11. MethodImpl [MethodImplAttributes]
<a id="MethodImplAttributes"></a>

 | Flag                                    | Value  | Description                                                 |
 | :-------------------------------------- | :----: | :---------------------------------------------------------- |
 | *`CodeTypeMask`*                        | 0x0003 | 这两位包含以下Value之一：                                   |
 | &emsp;*`IL`*                            | 0x0000 | 方法实现是 CIL                                              |
 | &emsp;*`Native`*                        | 0x0001 | 方法实现是本地的                                            |
 | &emsp;*`OPTIL`*                         | 0x0002 | 保留：在符合规范的实现中应为零                              |
 | &emsp;*`Runtime`*                       | 0x0003 | 方法实现由运行时提供                                        |
 | *`ManagedMask`*                         | 0x0004 | 指定代码是托管的还是非托管的标志。这一位包含以下Value之一： |
 | &emsp;*`Unmanaged`*                     | 0x0004 | 方法实现是非托管的，否则是托管的                            |
 | &emsp;*`Managed`*                       | 0x0000 | 方法实现是托管的                                            |
 | **Implementation info<br> and interop** | &nbsp; | &nbsp;                                                      |
 | *`ForwardRef`*                          | 0x0010 | 表示方法已定义；主要用于合并场景                            |
 | *`PreserveSig`*                         | 0x0080 | 保留：符合规范的实现可以忽略                                |
 | *`InternalCall`*                        | 0x1000 | 保留：在符合规范的实现中应为零                              |
 | *`Synchronized`*                        | 0x0020 | 方法在主体中是单线程的                                      |
 | *`NoInlining`*                          | 0x0008 | 方法不能内联                                                |
 | *`MaxMethodImplVal`*                    | 0xffff | 范围检查 Value                                              |
 | *`NoOptimization`*                      | 0x0040 | 在生成本地代码时，方法不会被优化                            |

####  21.1.12. MethodSemantics [MethodSemanticsAttributes]
<a id="MethodSemanticsAttributes"></a>

 | Flag         | Value  | Description                                                                  |
 | :----------- | :----: | :--------------------------------------------------------------------------- |
 | *`Setter`*   | 0x0001 | 属性的设置器                                                                 |
 | *`Getter`*   | 0x0002 | 属性的获取器                                                                 |
 | *`Other`*    | 0x0004 | 属性或事件的其他方法                                                         |
 | *`AddOn`*    | 0x0008 | 事件的 AddOn 方法。这指的是事件所需的 `add_` 方法。 [[↗]](#Event_0x14)       |
 | *`RemoveOn`* | 0x0010 | 事件的 RemoveOn 方法。这指的是事件所需的 `remove_` 方法。 [[↗]](#Event_0x14) |
 | *`Fire`*     | 0x0020 | 事件的 Fire 方法。这指的是事件的可选 `raise_` 方法。 [[↗]](#Event_0x14)      |

#### 21.1.13. Params [ParamAttributes]
<a id="ParamAttributes"></a>

 | Flag                | Value  | Description                    |
 | :------------------ | :----: | :----------------------------- |
 | *`In`*              | 0x0001 | 参数是 `[in]`                  |
 | *`Out`*             | 0x0002 | 参数是 `[out]`                 |
 | *`Optional`*        | 0x0010 | 参数是可选的                   |
 | *`HasDefault`*      | 0x1000 | 参数有默认 Value               |
 | *`HasFieldMarshal`* | 0x2000 | 参数有 ***`FieldMarshal`***    |
 | *`Unused`*          | 0xcfe0 | 保留：在符合规范的实现中应为零 |

#### 21.1.14. Properties [PropertyAttributes]
<a id="PropertyAttributes"></a>

 | Flag              | Value  | Description                            |
 | :---------------- | :----: | :------------------------------------- |
 | *`SpecialName`*   | 0x0200 | 属性是特殊的                           |
 | *`RTSpecialName`* | 0x0400 | 运行时 (元数据内部 API) 应检查名称编码 |
 | *`HasDefault`*    | 0x1000 | 属性有默认 Value                       |
 | *`Unused`*        | 0xe9ff | 保留：在符合规范的实现中应为零         |

#### 21.1.15. Types [TypeAttributes]
<a id="TypeAttributes"></a>

 | Flag                                                     |   Value    | Description                                                                      |
 | :------------------------------------------------------- | :--------: | :------------------------------------------------------------------------------- |
 | **Visibility attributes**                                |   &nbsp;   | &nbsp;                                                                           |
 | *`VisibilityMask`*                                       | 0x00000007 | 使用此掩码检索可见性信息。这 3 位包含以下 Value 之一：                           |
 | &emsp;*`NotPublic`*                                      | 0x00000000 | 类没有公共范围                                                                   |
 | &emsp;*`Public`*                                         | 0x00000001 | 类具有公共范围                                                                   |
 | &emsp;*`NestedPublic`*                                   | 0x00000002 | 类是具有公共可见性的嵌套类                                                       |
 | &emsp;*`NestedPrivate`*                                  | 0x00000003 | 类是具有私有可见性的嵌套类                                                       |
 | &emsp;*`NestedFamily`*                                   | 0x00000004 | 类是具有家族可见性的嵌套类                                                       |
 | &emsp;*`NestedAssembly`*                                 | 0x00000005 | 类是具有程序集可见性的嵌套类                                                     |
 | &emsp;*`NestedFamANDAssem`*                              | 0x00000006 | 类是具有家族和程序集可见性的嵌套类                                               |
 | &emsp;*`NestedFamORAssem`*                               | 0x00000007 | 类是具有家族或程序集可见性的嵌套类                                               |
 | **Class layout attributes**                              |   &nbsp;   | &nbsp;                                                                           |
 | *`LayoutMask`*                                           | 0x00000018 | 使用此掩码检索类布局信息。这 2 位包含以下 Value 之一：                           |
 | &emsp;*`AutoLayout`*                                     | 0x00000000 | 类字段是自动布局的                                                               |
 | &emsp;*`SequentialLayout`*                               | 0x00000008 | 类字段是顺序布局的                                                               |
 | &emsp;*`ExplicitLayout`*                                 | 0x00000010 | 布局是显式提供的                                                                 |
 | **Class semantics attributes**                           |   &nbsp;   | &nbsp;                                                                           |
 | *`ClassSemanticsMask`*                                   | 0x00000020 | 使用此掩码检索类语义信息。此位包含以下 Value 之一：                              |
 | &emsp;*`Class`*                                          | 0x00000000 | 类型是类                                                                         |
 | &emsp;*`Interface`*                                      | 0x00000020 | 类型是接口                                                                       |
 | **Special semantics in addition<br> to class semantics** |   &nbsp;   | &nbsp;                                                                           |
 | *`Abstract`*                                             | 0x00000080 | 类是抽象的                                                                       |
 | *`Sealed`*                                               | 0x00000100 | 类不能被扩展                                                                     |
 | *`SpecialName`*                                          | 0x00000400 | 类名是特殊的                                                                     |
 | **Implementation Attributes**                            |   &nbsp;   | &nbsp;                                                                           |
 | *`Import`*                                               | 0x00001000 | 类/接口是导入的                                                                  |
 | *`Serializable`*                                         | 0x00002000 | 保留 (类是可序列化的)                                                            |
 | **String formatting Attributes**                         |   &nbsp;   | &nbsp;                                                                           |
 | *`StringFormatMask`*                                     | 0x00030000 | 使用此掩码检索用于本地互操作的字符串信息。这 2 位包含以下 Value 之一：           |
 | &emsp;*`AnsiClass`*                                      | 0x00000000 | `LPSTR` 被解释为 ANSI                                                            |
 | &emsp;`UnicodeClaiss`                                    | 0x00010000 | `LPSTR` 被解释为 Unicode                                                         |
 | &emsp;*`AutoClass`*                                      | 0x00020000 | `LPSTR` 自动解释                                                                 |
 | &emsp;*`CustomFormatClass`*                              | 0x00030000 | 由 *`CustomStringFormatMask`* 指定的非标准编码                                   |
 | *`CustomStringFormatMask`*                               | 0x00C00000 | 使用此掩码检索用于本地互操作的非标准编码信息。这 2 位的 Value 的含义是未指定的。 |
 | **Class Initialization Attributes**                      |   &nbsp;   | &nbsp;                                                                           |
 | *`BeforeFieldInit`*                                      | 0x00100000 | 在第一次静态字段访问之前初始化类                                                 |
 | **Additional Flags**                                     |   &nbsp;   | &nbsp;                                                                           |
 | *`RTSpecialName`*                                        | 0x00000800 | CLI 提供 '特殊' 行为，取决于类型的名称                                           |
 | *`HasSecurity`*                                          | 0x00040000 | 类型具有与其关联的安全性                                                         |
 | *`IsTypeForwarder`*                                      | 0x00200000 | 此 **`ExportedType`** 条目是类型转发器                                           |

 #### 21.1.16. 签名中使用的元素类型
<a id="ELEMENT_TYPE"></a>

下表列出了 `ELEMENT_TYPE` 常量的值。这些在元数据签名的 *Blobs* 中被广泛使用。

 | Name                         | Value | 备注                                                                                                                               |
 | :--------------------------- | :---: | :--------------------------------------------------------------------------------------------------------------------------------- |
 | *`ELEMENT_TYPE_END`*         | 0x00  | 标记列表的结束                                                                                                                     |
 | *`ELEMENT_TYPE_VOID`*        | 0x01  | &nbsp;                                                                                                                             |
 | *`ELEMENT_TYPE_BOOLEAN`*     | 0x02  | &nbsp;                                                                                                                             |
 | *`ELEMENT_TYPE_CHAR`*        | 0x03  | &nbsp;                                                                                                                             |
 | *`ELEMENT_TYPE_I1`*          | 0x04  | &nbsp;                                                                                                                             |
 | *`ELEMENT_TYPE_U1`*          | 0x05  | &nbsp;                                                                                                                             |
 | *`ELEMENT_TYPE_I2`*          | 0x06  | &nbsp;                                                                                                                             |
 | *`ELEMENT_TYPE_U2`*          | 0x07  | &nbsp;                                                                                                                             |
 | *`ELEMENT_TYPE_I4`*          | 0x08  | &nbsp;                                                                                                                             |
 | *`ELEMENT_TYPE_U4`*          | 0x09  | &nbsp;                                                                                                                             |
 | *`ELEMENT_TYPE_I8`*          | 0x0a  | &nbsp;                                                                                                                             |
 | *`ELEMENT_TYPE_U8`*          | 0x0b  | &nbsp;                                                                                                                             |
 | *`ELEMENT_TYPE_R4`*          | 0x0c  | &nbsp;                                                                                                                             |
 | *`ELEMENT_TYPE_R8`*          | 0x0d  | &nbsp;                                                                                                                             |
 | *`ELEMENT_TYPE_STRING`*      | 0x0e  | &nbsp;                                                                                                                             |
 | *`ELEMENT_TYPE_PTR`*         | 0x0f  | 后跟 *type*                                                                                                                        |
 | *`ELEMENT_TYPE_BYREF`*       | 0x10  | 后跟 *type*                                                                                                                        |
 | *`ELEMENT_TYPE_VALUETYPE`*   | 0x11  | 后跟 _TypeDef_ 或 _TypeRef_  _token_                                                                                               |
 | *`ELEMENT_TYPE_CLASS`*       | 0x12  | 后跟 _TypeDef_ 或 _TypeRef_  _token_                                                                                               |
 | *`ELEMENT_TYPE_VAR`*         | 0x13  | 泛型类型定义中的泛型参数，表示为 _number_ (压缩的无符号整数)                                                                       |
 | *`ELEMENT_TYPE_ARRAY`*       | 0x14  | *type* *rank* *boundsCount* *bound1* &hellip; *loCount* *lo1* &hellip;                                                             |
 | *`ELEMENT_TYPE_GENERICINST`* | 0x15  | 泛型类型实例化。后跟 *type* *type-arg-count* *type-1* &hellip; *type-n*                                                            |
 | *`ELEMENT_TYPE_TYPEDBYREF`*  | 0x16  | &nbsp;                                                                                                                             |
 | *`ELEMENT_TYPE_I`*           | 0x18  | `System.IntPtr`                                                                                                                    |
 | *`ELEMENT_TYPE_U`*           | 0x19  | `System.UIntPtr`                                                                                                                   |
 | *`ELEMENT_TYPE_FNPTR`*       | 0x1b  | 后跟完整的方法签名                                                                                                                 |
 | *`ELEMENT_TYPE_OBJECT`*      | 0x1c  | `System.Object`                                                                                                                    |
 | *`ELEMENT_TYPE_SZARRAY`*     | 0x1d  | 单维数组，下界为0                                                                                                                  |
 | *`ELEMENT_TYPE_MVAR`*        | 0x1e  | 泛型方法定义中的泛型参数，表示为 *number* (压缩的无符号整数)                                                                       |
 | *`ELEMENT_TYPE_CMOD_REQD`*   | 0x1f  | 必需的修饰符：后跟 _TypeDef_ 或 _TypeRef_  _token_                                                                                 |
 | *`ELEMENT_TYPE_CMOD_OPT`*    | 0x20  | 可选的修饰符：后跟 _TypeDef_ 或 _TypeRef_  _token_                                                                                 |
 | *`ELEMENT_TYPE_INTERNAL`*    | 0x21  | 在 CLI 中实现                                                                                                                      |
 | *`ELEMENT_TYPE_MODIFIER`*    | 0x40  | 与后续元素类型进行或运算                                                                                                           |
 | *`ELEMENT_TYPE_SENTINEL`*    | 0x41  | ***vararg*** 方法签名的标志                                                                                                        |
 | *`ELEMENT_TYPE_PINNED`*      | 0x45  | 表示指向固定对象的局部变量                                                                                                         |
 | &nbsp;                       | 0x50  | 表示类型为 `System.Type` 的参数。                                                                                                  |
 | &nbsp;                       | 0x51  | 在自定义特性中用于指定装箱的对象 (§[*CustomAttribute*](#custom-attr-value))。                                                      |
 | &nbsp;                       | 0x52  | 保留                                                                                                                               |
 | &nbsp;                       | 0x53  | 在自定义特性中用于表示 *`FIELD`* (§[*CustomAttribute_0x0C*](#CustomAttribute_0x0C), §[*CustomAttribute*](#custom-attr-value))。    |
 | &nbsp;                       | 0x54  | 在自定义特性中用于表示 *`PROPERTY`* (§[*CustomAttribute_0x0C*](#CustomAttribute_0x0C), §[*CustomAttribute*](#custom-attr-value))。 |
 | &nbsp;                       | 0x55  | 在自定义特性中用于指定枚举 (§[*CustomAttribute*](#custom-attr-value))。                                                            |

### 21.2. Blobs & Signatures

***签名*** (***Signature***) 通常用来描述函数或方法的类型信息；也就是，它的每个参数的类型，以及返回值的类型。在元数据中，签名也用来描述字段、属性和局部变量的类型信息。每个签名都存储为 ***Blob Heap*** 中的一个 (计数) 字节数组。有几种类型的签名，如下：

 * _MethodRefSig_ (只有在 ***VARARG*** 调用中才与 _MethodDefSig_ 不同)
 * _MethodDefSig_
 * _FieldSig_
 * _PropertySig_
 * _LocalVarSig_
 * *TypeSpec*

 * *MethodSpec*

签名 '*blob*' 的第一个字节的值表示它是什么类型的签名。它的最低 4 位包含以下之一：
- *`C`*，*`DEFAULT`*，*`FASTCALL`*，*`STDCALL`*，*`THISCALL`*，或 *`VARARG`*  (其值在 §[*StandAloneMethodSig*](#StandAloneMethodSig) 中定义)，这些都是方法签名的限定符；
- *`FIELD`*，表示字段签名 (其值在 §[*FieldSig*](#FieldSig) 中定义)；
- 或 *`PROPERTY`*，表示属性签名 (其值在 §[*PropertySig*](#PropertySig) 中定义)。

本小节定义了每种类型的签名的二进制 '*blob*' 格式。在伴随许多定义的语法图中，使用灰色标记的阴影块将本来会是多个图的内容合并到一个图中；附带的文本描述了阴影块的使用。

在将签名存储到 ***Blob Heap*** 中之前，通过压缩签名中嵌入的整数来压缩签名 (如下所述)。可编码的无符号整数的最大长度为 29 位，0x1FFFFFFF。对于有符号整数，如在 *ArrayShape* (§[*ArrayShape*](#ArrayShape)) 中出现的，范围是 -2<sup>28</sup> (0xF0000000) 到 2<sup>28</sup> -1 (0x0FFFFFFF)。使用的压缩算法如下 (位 0 是最低有效位)：
<a id="blob-reduce"></a>

 * 对于无符号整数：
   * 如果 Value 在 0 (0x00) 和 127 (0x7F) 之间，包含两者，编码为一个字节的整数 (位 7 清零，Value 保存在位 6 到位 0)
   * 如果 Value 在 2<sup>8</sup> (0x80) 和 2<sup>14</sup> -1 (0x3FFF) 之间，包含两者，编码为一个 2 字节的整数，位 15 设为 1，位 14 清零 (Value 保存在位 13 到位 0)
   * 否则，编码为一个 4 字节的整数，位 31 设为 1，位 30 设为 1，位 29 清零 (Value 保存在位 28 到位 0)

 + 对于有符号整数：
   * 如果 Value 在 -2<sup>6</sup> 和 2<sup>6</sup> -1 之间，包含两者：
      * 将 Value 表示为一个 7 位的 2 的补数，给出 0x40 (-2<sup>6</sup>) 到 0x3F (2<sup>6</sup> -1)；
      * 将这个 Value 左移 1 位，给出 0x01 (-2<sup>6</sup>) 到 0x7E (2<sup>6</sup> -1)；
      * 编码为一个字节的整数，位 7 清零，旋转后的 Value 在位 6 到位 0，给出 0x01 (-2<sup>6</sup>) 到 0x7E (2<sup>6</sup> -1)。

   * 如果 Value 在 -2<sup>13</sup> 和 2<sup>13</sup> -1 之间，包含两者：

      * 将 Value 表示为一个 14 位的 2 的补数，给出 0x2000 (-2<sup>13</sup>) 到 0x1FFF (2<sup>13</sup> -1)；
      * 将这个 Value 左移 1 位，给出 0x0001 (-2<sup>13</sup>) 到 0x3FFE (2<sup>13</sup> -1)；
      * 编码为一个两字节的整数：位 15 设为 1，位 14 清零，旋转后的 Value 在位 13 到位 0，给出 0x8001 (-2<sup>13</sup>) 到 0xBFFE (2<sup>13</sup> -1)。

   * 如果 Value 在 -2<sup>28</sup> 和 2<sup>28</sup> -1 之间，包含两者：

      * 将 Value 表示为一个 29 位的 2 的补数，给出 0x10000000 (-2<sup>28</sup>) 到 0xFFFFFFF (2<sup>28</sup> -1)；
      * 将这个 Value 左移 1 位，给出 0x00000001 (-2<sup>28</sup>) 到 0x1FFFFFFE (2<sup>28</sup> -1)；
      * 编码为一个四字节的整数：位 31 设为 1，位 30 设为 1，位 29 清零，旋转后的 Value 在位 28 到位 0，给出 0xC0000001 (-2<sup>28</sup>) 到 0xDFFFFFFE (2<sup>28</sup> -1)。

* 空字符串应该用保留的单字节 0xFF 表示，后面没有跟随的数据。

下面的表格显示了几个例子。第一列给出了一个 Value，以熟悉的 (类 C) 十六进制表示法表示。第二列显示了相应的压缩结果，就像它会出现在 PE 文件中一样，结果的连续字节位于文件中逐渐增大的字节偏移处。这与在 PE 文件中布局常规二进制整数的顺序相反。

无符号示例：

 | 原始 Value  | 压缩表示      |
 | ----------- | ------------- |
 | 0x03        | 03            |
 | 0x7F        | 7F (7 位设定) |
 | 0x80        | 8080          |
 | 0x2E57      | AE57          |
 | 0x3FFF      | BFFF          |
 | 0x4000      | C000 4000     |
 | 0x1FFF FFFF | DFFF FFFF     |

有符号示例：

 | 原始 Value | 压缩表示  |
 | ---------- | --------- |
 | 3          | 06        |
 | -3         | 7B        |
 | 64         | 8080      |
 | -64        | 01        |
 | 8192       | C000 4000 |
 | -8192      | 8001      |
 | 268435455  | DFFF FFFE |
 | -268435456 | C000 0001 |


"压缩" 字段的最高有效位 (在 PE 文件中首次遇到的位) 可以揭示它占用 1、2 还是 4 个字节，以及它的 Value。为了使这个工作，如上所述，"压缩" Value 以大端顺序存储；即，最高有效字节位于文件中的最小偏移处。

签名广泛使用名为 *`ELEMENT_TYPE_xxx`* 的常量 Value [[↗]](#ELEMENT_TYPE)。特别地，签名包括两个修饰符，称为：
 * *`ELEMENT_TYPE_BYREF`* —— 这个元素是一个托管指针。这个修饰符只能出现在 _LocalVarSig_ [[↗]](#LocalVarSig)，*Param* [[↗]](#Param-image) 或 _RetType_ [[↗]](#RetType-image) 的定义中。它不应该出现在 *Field* [[↗]](#FieldSig) 的定义中。
 * *`ELEMENT_TYPE_PTR`* —— 这个元素是一个非托管指针。这个修饰符可以出现在 _LocalVarSig_ [[↗]](#LocalVarSig)，*Param* [[↗]](#Param-image)，_RetType_ [[↗]](#RetType-image) 或 *Field* [[↗]](#FieldSig) 的定义中。

#### 21.2.1. MethodDefSig
<a id="MethodDefSig"></a>

_MethodDefSig_ 由 **`MethodDef`**.***`Signature`*** 列索引。它捕获方法或全局函数的签名。_MethodDefSig_ 的语法图如下：

 ![MethodDefSig](./.img/MethodDefSig.png)

此图使用以下缩写：

 * *`HASTHIS`* = 0x20，用于在调用约定中编码关键字 **instance** [[↗]](#calling-convention)。
 * *`EXPLICITTHIS`* = 0x40，用于在调用约定中编码关键字 **explicit** [[↗]](#calling-convention)。
 * *`DEFAULT`* = 0x0，用于在调用约定中编码关键字 **default** [[↗]](#calling-convention)。
 * *`VARARG`* = 0x5，用于在调用约定中编码关键字 **vararg** [[↗]](#calling-convention)。
 * *`GENERIC`* = 0x10，用于表示方法有一个或多个泛型参数。

签名的第一个字节包含 *`HASTHIS`*，*`EXPLICITTHIS`* 和调用约定 (*`DEFAULT`*，*`VARARG`* 或 *`GENERIC`*) 的位。这些位一起进行 *OR* 运算。

_GenParamCount_ 是方法的泛型参数的数量。这是一个压缩的无符号整数。

对于泛型方法，**`MethodDef`** 和 **`MemberRef`** 都应包含 *`GENERIC`* 调用约定，以及 _GenParamCount_；这些对于绑定很重要 —— 它们使 CLI 能够根据泛型方法包含的泛型参数的数量进行重载。

_ParamCount_ 是一个无符号整数，它保存参数的数量 (0 个或更多)。它可以是 0 到 0x1FFFFFFF 之间的任何数字。编译器也会压缩它  —— 在存储到 '*blob*' 之前 (*ParamCount* 只计算方法参数 —— 它不包括方法的返回类型)

_RetType_ 项描述方法的返回值的类型 [[↗]](#RetType-image)。

*Param* 项描述每个方法参数的类型。应该有 _ParamCount_ 个 *Param* 项的实例 [[↗]](#Param-image)。

#### 21.2.2. MethodRefSig
<a id="MethodRefSig"></a>

_MethodRefSig_ 是由 **`MemberRef`**.***`Signature`*** 列索引的。这提供了方法的 *call site* 签名。通常，这个调用点签名应该与目标方法定义中指定的签名完全匹配。例如，如果定义了一个方法 `Foo`，它接受两个 `unsigned int32` 并返回 `void`；那么任何调用点都应该索引一个签名，该签名接受恰好两个 `unsigned int32` 并返回 `void`。在这种情况下，_MethodRefSig_ 的语法图与 _MethodDefSig_ 的语法图相同。

只有对于具有 *`VARARG`* 调用约定的方法，调用点的签名才会与其定义处的签名不同。在这种情况下，调用点签名被扩展以包含关于额外 *`VARARG`* 参数的信息 (例如，对应于 C 语法中的 "`...`")。这种情况的语法图如下：

 ![MethodRefSig](./.img/MethodRefSig.png)

此图使用以下缩写：
 * *`HASTHIS`* = 0x20，用于在调用约定中编码关键字 **instance** [[↗]](#calling-convention)。
 * *`EXPLICITTHIS`* = 0x40，用于在调用约定中编码关键字 **explicit** [[↗]](#calling-convention)。
 * *`VARARG`* = 0x5，用于在调用约定中编码关键字 **vararg** [[↗]](#calling-convention)。
 * *`SENTINEL`* = 0x41 [[↗]](#ELEMENT_TYPE)，用于在参数列表中编码 "`...`"。

签名的第一个字节保存了 *`HASTHIS`*，*`EXPLICITTHIS`* 和调用约定 *`VARARG`* 的位。这些位一起进行 *OR* 运算。

_ParamCount_ 是一个无符号整数，保存了参数的数量 (0 或更多)。它可以是 0 到 0x1FFFFFFF 之间的任何数字。在存储到 '*blob*' 中之前，编译器会压缩它。_ParamCount_ 只计算方法参数的数量，它不包括方法的返回类型。

*Param* 项Description了每个方法参数的类型。应该有 _ParamCount_ 个 *Param* 项的实例。

*Param* 项描述了每个方法参数的类型。应该有 _ParamCount_ 个 *Param* 项的实例。这开始就像一个 *`VARARG`* 方法的 _MethodDefSig_。但随后附加了一个 *`SENTINEL`*  _token_ ，后跟额外的 *Param* 项来描述额外的 *`VARARG`* 参数。注意，_ParamCount_ 项应该指示签名中 *Param* 项的总数 — 在 *`SENTINEL`* 字节 (0x41) 之前和之后。

在罕见的情况下，如果调用点没有提供额外的参数，签名不应包含 *`SENTINEL`* (这是由下箭头显示的路径，它绕过 *`SENTINEL`* 并到达 _MethodRefSig_ 定义的末尾)。


#### 21.2.3. StandAloneMethodSig
<a id ="StandAloneMethodSig"></a>

_StandAloneMethodSig_ 由 **`StandAloneSig`**.***`Signature`*** 列索引。它通常在执行 `calli` 指令之前创建。它类似于 _MethodRefSig_，因为它表示一个调用点签名，但是它的调用约定可以指定一个非托管目标 (`calli` 指令调用托管代码或非托管代码)。它的语法图如下：

 ![StandAloneMethodSig](./.img/StandAloneMethodSig.png)

此图使用以下缩写 [[↗]](#calling-convention)：

 * *`HASTHIS`* 对应 0x20
 * *`EXPLICITTHIS`* 对应 0x40
 * *`DEFAULT`* 对应 0x0
 * *`VARARG`* 对应 0x5
 * *`C`* 对应 0x1
 * *`STDCALL`* 对应 0x2
 * *`THISCALL`* 对应 0x3
 * *`FASTCALL`* 对应 0x4
 * *`SENTINEL`* 对应 0x41 [[↗]](#ELEMENT_TYPE)。

签名的第一个字节包含 *`HASTHIS`*，*`EXPLICITTHIS`* 和调用约定的位 - *`DEFAULT`*，*`VARARG`*，*`C`*，*`STDCALL`*，*`THISCALL`* 或 *`FASTCALL`*。这些位一起进行 *OR* 运算。

_ParamCount_ 是一个无符号整数，它包含了非可变参数和可变参数的数量，并合并在一起。它可以是 0 到 0x1FFFFFFF 之间的任何数字。编译器在存储到 *blob* 之前会压缩它 (_ParamCount_ 只计算方法参数，它不包括方法的返回类型)。

第一个 *Param* 项描述了每个方法的非可变参数的类型。第二个 (可选的) *Param* 项描述了每个方法的可变参数的类型。应该有 _ParamCount_ 个 *Param* 实例 [[↗]](#Param-image)。

这是各种方法签名中最复杂的一个。在这个图中，两个单独的图被合并成一个，使用灰色颜色标记来区分它们。因此，对于以下的调用约定：*`DEFAULT`* (托管)，*`STDCALL`*，*`THISCALL`* 和 *`FASTCALL`* (非托管)，签名在 *`SENTINEL`* 项之前就结束了 (这些都是非可变参数签名)。然而，对于托管和非托管的可变参数调用约定：

*`VARARG`* (托管) 和 *`C`* (非托管)，签名可以包含 *`SENTINEL`* 和最后的 *Param* 项 (然而，它们并不是必需的)。这些选项由语法图中的框的阴影表示。

在罕见的情况下，如果调用点没有提供额外的参数，签名不应该包含 *`SENTINEL`* (这是由下箭头显示的路径，它绕过 *`SENTINEL`* 并到达 _StandAloneMethodSig_ 定义的结束)。

#### 21.2.4. FieldSig
<a id="FieldSig"></a>

_FieldSig_ 由 **`Field`**.***`Signature`*** 列索引，或由 **`MemberRef`**.***`Signature`*** 列索引 (当然，这是在它指定对字段的引用，而不是方法的情况下)。签名捕获了字段的定义。字段可以是类中的静态或实例字段，也可以是全局变量。_FieldSig_ 的语法图如下：

 ![FieldSig](./.img/FieldSig.png)

此图使用以下缩写：

 * *`FIELD`* 代表 0x6

_CustomMod_ 在 §[_CustomMod_](#CustomMod) 中定义。*Type* 在 §[_Type_](#Type-image) 中定义。

#### 21.2.5. PropertySig
<a id="PropertySig"></a>

_PropertySig_ 是由 **`Property`**.***`Type`*** 列索引的。它捕获了属性的类型信息 — 本质上，它是其 _getter_ 方法的签名：

 * 提供给其 *getter* 方法的参数数量
 * 属性的基类型 (_getter_ 方法返回的类型)
 * *getter* 方法中每个参数的类型信息 (即，索引参数)

注意，getter 和 setter 的签名之间的关系如下：
 * *getter* 的 _ParamCount_ 参数的类型与 *setter* 的前 _ParamCount_ 参数的类型完全相同
 * *getter* 的返回类型与提供给 *setter* 的最后一个参数的类型完全相同

_PropertySig_ 的语法图如下：

 ![PropertySig](./.img/PropertySig.png)

签名的第一个字节保存了 *`HASTHIS`* 和 *`PROPERTY`* 的位。这些位一起进行 *OR* 运算。

*Type* 指定了此属性的 *getter* 方法返回的类型。*Type* 在 §[*Type*](#Type-image) 中定义。

*Param* 在 §[*Param*](#Param-image) 中定义。

_ParamCount_ 是一个压缩的无符号整数，保存了 *getter* 方法中的索引参数数量 (0 或更多)。_ParamCount_ 只计算方法参数的数量，它不包括方法的属性的基类型。

#### 21.2.6. LocalVarSig
<a id="LocalVarSig"></a>

_LocalVarSig_ 由 **`StandAloneSig`**.***`Signature`*** 列索引。它捕获了方法中所有局部变量的类型。它的语法图如下：

 ![LocalVarSig](./.img/LocalVarSig.png)

此图使用以下缩写：
 * *`LOCAL_SIG`* 代表 0x7，用于 **.locals** 指令 [[↗]](#locals)。
 * *`BYREF`* 代表 *`ELEMENT_TYPE_BYREF`* [[↗]](#ELEMENT_TYPE)。

*Constraint* 在 §[*Constraint*](#Constraint) 中定义。

*Type* 在 §[*Type*](#Type-image) 中定义。

_Count_ 是一个压缩的无符号整数，它保存了局部变量的数量。它可以是 1 到 0xFFFE 之间的任何数字。

_LocalVarSig_ 中应该有 _Count_ 个 *Type* 的实例。

#### 21.2.7. CustomMod
<a id="CustomMod"></a>

_Signatures_ 中的 _CustomMod_ (自定义修饰符) 项的语法图如下：

 ![CustomMod](./.img/CustomMod.png)

此图使用以下缩写：
 * *`CMOD_OPT`* 代表 *`ELEMENT_TYPE_CMOD_OPT`*。 
 * *`CMOD_REQD`* 代表 *`ELEMENT_TYPE_CMOD_REQD`*。

*`CMOD_OPT`* 或 *`CMOD_REQD`* 的值是压缩的。*`CMOD_OPT`* 或 *`CMOD_REQD`* 后面跟着一个元数据 _token_ ，该 _token_ 索引 **`TypeDef`** 表或 **`TypeRef`** 表中的一行。然而，这些 _token_ 是编码和压缩的 — 参见 §[*TypeDefOrRefOrSpecEncoded*](#TypeDefOrRefOrSpecEncoded)

如果 CustomModifier 标记为 *`CMOD_OPT`*，那么任何导入编译器都可以完全忽略它。相反，如果 CustomModifier 标记为 *`CMOD_REQD`*，任何导入编译器都应该 “识别” 此 CustomModifier 所暗示的语义，以便引用周围的 *Signature*。

#### 21.2.8. TypeDefOrRefOrSpecEncoded
<a id="TypeDefOrRefOrSpecEncoded"></a>

这些项是在签名中存储 _TypeDef_，_TypeRef_ 或 *TypeSpec*  _token_ 的紧凑方式 [[↗]](#Type-image)。考虑一个常规的 _TypeRef_  _token_ ，例如 0x01000012。最高字节 0x01 表示这是一个 _TypeRef_  _token_ 中支持的元数据 _token_ 类型列表。较低的 3 字节 (0x000012) 索引 **`TypeRef`** 表中的行号 0x12。

这个 _TypeRef_  _token_ 的编码版本如下构造：

 1. 将此 _token_ 索引的表编码为最低有效的 2 位。要使用的位 Value 是 0，1 和 2，分别指定目标表是 **`TypeDef`**，**`TypeRef`** 或 **`TypeSpec`** 表。
 2. 将 3 字节行索引 (在此示例中为 0x000012) 左移 2 位，并将其与步骤 1 中的 2 位编码进行 OR 操作。
 3. 压缩结果 Value。

此示例产生以下编码 Value：

 ```
 a)  编码 = TypeRef 表的 Value = 0x01 (来自上述 1.)
 b)  编码 = ( 0x000012 << 2 ) |  0x01
             = 0x48 | 0x01
             = 0x49
 c)  编码 = Compress (0x49)
             = 0x49
 ```

所以，与原始的常规 _TypeRef_ _token_ 值 0x01000012 不同，需要在签名 '*blob*' 中占用 4 字节的空间，这个 _TypeRef_ _token_ 被编码为一个单字节。

#### 21.2.9. Constraint
<a id="Constraint"></a>

在签名中，*Constraint* 项目前只有一个可能的 Value，即 *`ELEMENT_TYPE_PINNED`*，它指定目标类型在运行时堆中被固定，不会被垃圾收集的操作移动。

*Constraint* 只能在 _LocalVarSig_ 中应用 (不能在 _FieldSig_ 中应用)。局部变量的类型要么是引用类型 (换句话说，它指向实际的变量 — 例如，一个对象或一个字符串) ；要么包含 *`BYREF`* 项。原因是局部变量分配在运行时栈上 — 它们从不从运行时堆中分配；所以除非局部变量指向在 GC 堆中分配的对象，否则固定没有意义。

#### 21.2.10. Param
<a id="Param-image"></a>

签名中的 *Param*  (参数) 项有以下语法图：

 ![Param](./.img/Param.png)

此图使用以下缩写：
 * *`BYREF`* 代表 0x10 [[↗]](#ELEMENT_TYPE)
 * *`TYPEDBYREF`* 代表 0x16 [[↗]](#ELEMENT_TYPE)

_CustomMod_ 在 §[*CustomMod*](#CustomMod) 中定义

*Type* 在 §[*Type*](#Type-image) 中定义。

#### 21.2.11. RetType
<a id="RetType-image"></a>

_RetType_ (返回类型) 项在签名中的语法图如下：

 ![_RetType_](./.img/RetType.png)

_RetType_ 与 *Param* 相同，除了它可以包含类型 *`VOID`* 的额外可能性。

此图使用以下缩写：

 * *`BYREF`* 对应 *`ELEMENT_TYPE_BYREF`*
 * *`TYPEDBYREF`* 对应 *`ELEMENT_TYPE_TYPEDBYREF`* 
 * *`VOID`* 对应 *`ELEMENT_TYPE_VOID`* 

#### 21.2.12. Type
<a id="Type-image"></a>

*Type* 在签名中的编码如下 (*`I1`* 是 *`ELEMENT_TYPE_I1`* 的缩写，*`U1`* 是 *`ELEMENT_TYPE_U1`* 的缩写，依此类推)：

 | *Type* ::= | 
 | :----
 | *`BOOLEAN`* \| *`CHAR`* \| *`I1`* \| *`U1`* \| *`I2`* \| *`U2`* \| *`I4`* \| *`U4`* \| *`I8`* \| *`U8`* \| *`R4`* \| *`R8`* \| *`I`* \| *`U`*
 | \| *`ARRAY`* *Type* _ArrayShape_ (通用数组，参见 §[*ArrayShape*](#ArrayShape))
 | \| *`CLASS`* _TypeDefOrRefOrSpecEncoded_
 | \| *`FNPTR`* _MethodDefSig_
 | \| *`FNPTR`* _MethodRefSig_
 | \| *`GENERICINST`* (*`CLASS`* \| *`VALUETYPE`*) _TypeDefOrRefOrSpecEncoded_ _GenArgCount_ *Type* *
 | \| *`MVAR`* _number_
 | \| *`OBJECT`*
 | \| *`PTR`* _CustomMod_* *Type*
 | \| *`PTR`* _CustomMod_* *`VOID`*
 | \| *`STRING`*
 | \| *`SZARRAY`* _CustomMod_* *Type* (单维，零基数组，即向量)
 | \| *`VALUETYPE`* _TypeDefOrRefOrSpecEncoded_
 | \| *`VAR`* _number_

_GenArgCount_ 非终结符是一个无符号整数 Value (压缩)，指定此签名中的泛型参数的数量。在 *`MVAR`* 或 *`VAR`* 后面的 number 非终结符是一个无符号整数 Value (压缩)。

#### 21.2.13. ArrayShape
<a id="ArrayShape"></a>

_ArrayShape_ 的语法图如下：

 ![ArrayShape](./.img/ArrayShape.png)

_Rank_ 是一个无符号整数 (以压缩形式存储)，它指定数组的维数 (应为 1 个或更多)。

_NumSizes_ 是一个压缩的无符号整数，表示有多少维度有指定的大小 (应为 0 个或更多)。

_Size_ 是一个压缩的无符号整数，指定该维度的大小 — 序列从第一维开始，总共有 _NumSizes_ 个项目。

类似地，_NumLoBounds_ 是一个压缩的无符号整数，表示有多少维度有指定的下界 (应为 0 个或更多)。

_LoBound_ 是一个压缩的有符号整数，指定该维度的下界 — 序列从第一维开始，总共有 _NumLoBounds_ 个项目。

这两个序列中的任何维度都不能被跳过，但指定的维度数量可以小于 _Rank_。

以下是一些示例，所有示例的元素类型都是 `int32`：

 | &nbsp;               | Type  | Rank  | NumSizes |  Size  | NumLoBounds | LoBound |
 | -------------------- | :---: | :---: | :------: | :----: | :---------: | :-----: |
 | `[0...2]`            | `I4`  |  `1`  |   `1`    |  `3`   |     `0`     | &nbsp;  |
 | [,,,,,,]             | `I4`  |  `7`  |   `0`    | &nbsp; |     `0`     | &nbsp;  |
 | `[0...3, 0...2,,,,]` | `I4`  |  `6`  |   `2`    | `4 3`  |     `2`     |  `0 0`  |
 | `[1...2, 6...8]`     | `I4`  |  `2`  |   `2`    | `2 3`  |     `2`     |  `1 6`  |
 | `[5, 3...5, , ]`     | `I4`  |  `4`  |   `2`    | `5 3`  |     `2`     |  `0 3`  |

数组定义可以嵌套，因为类型本身可以是数组。

#### 21.2.14. TypeSpec
<a id="type-spec-blob"></a>

在 ***Blob Heap*** 中，由 *TypeSpec* _token_ 索引的签名具有以下格式：

 | _TypeSpecBlob_ ::=
 | :----
 | *`PTR`* _CustomMod_* *`VOID`*
 |  \| *`PTR`* _CustomMod_* *Type*
 | \| *`FNPTR`* _MethodDefSig_
 | \| *`FNPTR`* _MethodRefSig_
 | \| *`ARRAY`* *Type* _ArrayShape_
 | \| *`SZARRAY`* _CustomMod_* *Type*
 | \| *`GENERICINST`* ( *`CLASS`* \| *`VALUETYPE`* ) _TypeDefOrRefOrSpecEncoded_ _GenArgCount_ *Type* *Type* *

为了紧凑，此列表省略了 *`ELEMENT_TYPE_`* 前缀。所以，例如，*`PTR`* 是 *`ELEMENT_TYPE_PTR`* 的简写。请注意，*TypeSpecBlob* 不以调用约定字节开始，因此它与存储到元数据中的各种其他签名不同。

#### 21.2.15. MethodSpec
<a id="MethodSpec-blob"></a>

由 *MethodSpec* _token_ 索引的 ***Blob Heap*** 中的签名具有以下格式

 | _MethodSpecBlob_ ::=
 | :----
 | *`GENERICINST`* _GenArgCount_ *Type* *Type* *

*`GENERICINST`* 的 Value 为 0x0A。在 Microsoft CLR 实现中，这个 Value 被称为 *`IMAGE_CEE_CS_CALLCONV_GENERICINST`*。 

_GenArgCount_ 是一个压缩的无符号整数，表示方法中的泛型参数的数量。然后，*blob* 指定了实例化的类型，重复 _GenArgCount_ 次。

#### 21.2.16. Short form signatures

签名的一般规范在如何编码某些项上留有一些余地。例如，将 String 编码为以下两种形式之一似乎是有效的：

 * 长格式：(*`ELEMENT_TYPE_CLASS`*, **TypeRef-to-**`System.String`)
 * 短格式：*`ELEMENT_TYPE_STRING`*

只有短格式是有效的。下表显示了应该使用哪些短格式来代替每个长格式项。通常，为了紧凑，这里省略了 *`ELEMENT_TYPE_`* 前缀 — 所以 *`VALUETYPE`* 是 *`ELEMENT_TYPE_VALUETYPE`* 的简写)

 | 长格式        | &nbsp;                  | 短格式         |
 | :------------ | :---------------------- | :------------- |
 | **Prefix**    | **TypeRef-to-**         |
 | *`CLASS`*     | `System.String`         | *`STRING`*     |
 | *`CLASS`*     | `System.Object`         | *`OBJECT`*     |
 | *`VALUETYPE`* | `System.Void`           | *`VOID`*       |
 | *`VALUETYPE`* | `System.Boolean`        | *`BOOLEAN`*    |
 | *`VALUETYPE`* | `System.Char`           | *`CHAR`*       |
 | *`VALUETYPE`* | `System.Byte`           | *`U1`*         |
 | *`VALUETYPE`* | `System.Sbyte`          | *`I1`*         |
 | *`VALUETYPE`* | `System.Int16`          | *`I2`*         |
 | *`VALUETYPE`* | `System.UInt16`         | *`U2`*         |
 | *`VALUETYPE`* | `System.Int32`          | *`I4`*         |
 | *`VALUETYPE`* | `System.UInt32`         | *`U4`*         |
 | *`VALUETYPE`* | `System.Int64`          | *`I8`*         |
 | *`VALUETYPE`* | `System.UInt64`         | *`U8`*         |
 | *`VALUETYPE`* | `System.IntPtr`         | *`I`*          |
 | *`VALUETYPE`* | `System.UIntPtr`        | *`U`*          |
 | *`VALUETYPE`* | `System.TypedReference` | *`TYPEDBYREF`* |

### 21.3. Custom attributes
<a id="custom-attr-value"></a>

自定义特性具有以下语法图：

 ![Custom attributes](./.img/Custom-attributes.png)

所有二进制 Value 都以小端格式存储 (除了 _PackedLen_ 项目，它们仅用作后续 UTF8 字符串中字节数的计数)。如果没有指定字段、参数或属性，则整个特性表示为空 *blob*。

_CustomAttrib_ 以 *Prolog* 开始 - 一个无符号的 *int16*，Value 为 0x0001。

接下来是构造方法的固定参数的描述。通过检查 **`MethodDef`** 表中该构造函数的行，可以找到它们的数量和类型；这些信息在 _CustomAttrib_ 本身中并未重复。如语法图所示，可以有零个或多个 _FixedArgs_。 (注意，*`VARARG`* 构造方法在自定义特性的定义中是不允许的。)

接下来是可选的 "命名" 字段和属性的描述。这开始于 _NumNamed_ — 一个无符号的 *int16*，给出后面跟随的 "命名" 属性或字段的数量。注意，_NumNamed_ 总是存在的。Value 为零表示没有要跟随的 "命名" 属性或字段 (当然，在这种情况下，_CustomAttrib_ 将在 _NumNamed_ 之后立即结束)。在 _NumNamed_ 为非零的情况下，它后面跟随 _NumNamed_ 重复的 _NamedArgs_。

 ![_FixedArg_](./.img/_FixedArg_.png)

每个 _FixedArg_ 的格式取决于该参数是否为 *`SZARRAY`* — 这在语法图的下方和上方路径中分别显示。因此，每个 _FixedArg_ 要么是单个 _Elem_，要么是 _NumElem_ 重复的 _Elem_。

(*`SZARRAY`* 是单字节 0x1D，表示向量 - 即下界为零的单维数组。)

_NumElem_ 是一个无符号的 _int32_，指定 *`SZARRAY`* 中的元素数量，或者 0xFFFFFFFF 表示该 Value 为 null。

 ![_Elem_](./.img/_Elem_.png)

_Elem_ 采用此图中的一种形式，如下所示：

 * 如果参数种类是简单的 (上述图表的第一行)  (**bool**，**char**，**float32**，**float64**，**int8**，**int16**，**int32**，**int64**，**unsigned int8**，**unsigned int16**，**unsigned int32** 或 **unsigned int64**)，那么 '*blob*' 包含其二进制 Value (_Val_)。**bool** 是一个字节，Value 为 0 (假) 或 1 (真) ；**char** 是一个两字节的 Unicode 字符；其他的含义很明显。如果参数种类是 **enum**，也使用这种模式 - 只需存储枚举的底层整数类型的 Value。

 * 如果参数种类是 _string_， (上述图表的中间行) 那么 *blob* 包含一个 _SerString_ — 一个 _PackedLen_ 字节计数，后面跟着 UTF8 字符。如果字符串为 null，其 _PackedLen_ 的 Value 为 0xFF (没有后续字符)。如果字符串为空 ("")，那么 _PackedLen_ 的 Value 为 0x00 (没有后续字符)。

 * 如果参数种类是 `System.Type`， (同样，上述图表的中间行) 其 Value 以 _SerString_ 的形式存储 (如上一段所定义)，表示其规范名称。规范名称是其完整类型名称，后面可选地跟着定义它的程序集，其版本，区域性和公钥 _token_ 。如果省略了程序集名称，CLI 首先在当前程序集中查找，然后在系统库 (`mscorlib`) 中查找；在这两种特殊情况下，允许省略程序集名称，版本，区域性和公钥 _token_ 。

 * 如果参数种类是 `System.Object`，(上述图表的第三行) 存储的 Value 表示该值类型的 "装箱" 实例。在这种情况下，*blob* 包含实际类型的 _FieldOrPropType_ (见下文)，后面跟着参数的未装箱 Value。在这种情况下，不可能传递 nullValue。

* 如果类型是一个装箱的简单值类型 (**bool**，**char**，**float32**，**float64**，**int8**，**int16**，**int32**，**int64**，**unsigned int8**，**unsigned int16**，**unsigned int32** 或 **unsigned int64**)，那么 _FieldOrPropType_ 紧接着是一个字节，该字节包含 Value 0x51。_FieldOrPropType_ 必须恰好是以下之一：*`ELEMENT_TYPE_BOOLEAN`*，*`ELEMENT_TYPE_CHAR`*，*`ELEMENT_TYPE_I1`*，*`ELEMENT_TYPE_U1`*，*`ELEMENT_TYPE_I2`*，*`ELEMENT_TYPE_U2`*，*`ELEMENT_TYPE_I4`*，*`ELEMENT_TYPE_U4`*，*`ELEMENT_TYPE_I8`*，*`ELEMENT_TYPE_U8`*，*`ELEMENT_TYPE_R4`*，*`ELEMENT_TYPE_R8`*，*`ELEMENT_TYPE_STRING`*。单维零基数组被指定为一个字节 0x1D，后面跟着元素类型的 _FieldOrPropTypeof_。枚举被指定为一个字节 0x55，后面跟着一个 _SerString_。参见 [*ELEMENT_TYPE*](#ELEMENT_TYPE)。

 ![_NamedArg_](./.img/_NamedArg_.png)

_NamedArg_ 只是一个 _FixedArg_ (上面讨论过)，前面有信息来标识它代表哪个字段或属性。记住，CLI 允许字段和属性具有相同的名称；所以我们需要一种方法来消除这种情况的歧义。

 * *`FIELD`* 是单字节 0x53。
 * *`PROPERTY`* 是单字节 0x54。

_FieldOrPropName_ 是字段或属性的名称，存储为 _SerString_ (上面定义)。涉及自定义特性一些示例包含在 [第四部分的附录 B](【】) 中。

### 21.4. Marshalling descriptors
<a id="blob-description"></a>

***Marshalling*** 描述符类似于签名 — 它是二进制数据的 '*blob*'。它描述了当通过 PInvoke 调度调用到或从非托管代码时，应如何封送字段或参数 (通常情况下，作为参数编号 0 的方法返回也包括在内)。ILAsm 语法 **marshal** 可以用来创建 ***Marshalling*** 描述符，伪自定义特性 `MarshalAsAttribute` 也可以用来创建 ***Marshalling*** 描述符 [[↗]](#pseudo-custom-attr)。

注意，符合 CLI 规范的实现只需要支持封送前面指定的类型 [[↗]](#data-type-marshaling)。

***Marshalling*** 描述符使用名为 *`NATIVE_TYPE_xxx`* 的常量。它们的名称和值列在下表中：

 | Name                    | Value |
 | :---------------------- | :---: |
 | *`NATIVE_TYPE_BOOLEAN`* | 0x02  |
 | *`NATIVE_TYPE_I1`*      | 0x03  |
 | *`NATIVE_TYPE_U1`*      | 0x04  |
 | *`NATIVE_TYPE_I2`*      | 0x05  |
 | *`NATIVE_TYPE_U2`*      | 0x06  |
 | *`NATIVE_TYPE_I4`*      | 0x07  |
 | *`NATIVE_TYPE_U4`*      | 0x08  |
 | *`NATIVE_TYPE_I8`*      | 0x09  |
 | *`NATIVE_TYPE_U8`*      | 0x0a  |
 | *`NATIVE_TYPE_R4`*      | 0x0b  |
 | *`NATIVE_TYPE_R8`*      | 0x0c  |
 | *`NATIVE_TYPE_LPSTR`*   | 0x14  |
 | *`NATIVE_TYPE_LPWSTR`*  | 0x15  |
 | *`NATIVE_TYPE_INT`*     | 0x1f  |
 | *`NATIVE_TYPE_UINT`*    | 0x20  |
 | *`NATIVE_TYPE_FUNC`*    | 0x26  |
 | *`NATIVE_TYPE_ARRAY`*   | 0x2a  |

'*blob*' 的格式如下

 | _MarshalSpec_ ::=
 | :----
 | _NativeIntrinsic_
 | \| *`ARRAY`* _ArrayElemType_
 | \| *`ARRAY`* _ArrayElemType_ _ParamNum_
 | \| *`ARRAY`* _ArrayElemType_ _ParamNum_ _NumElem_

 | _NativeIntrinsic_ ::=
 | :----
 | *`BOOLEAN`* \| *`I1`* \| *`U1`* \| *`I2`* \| *`U2`* \| *`I4`* \| *`U4`* \| *`I8`* \| *`U8`* \| *`R4`* \| *`R8`* \| *`LPSTR`* \| *`LPSTR`* \| *`INT`* \| *`UINT`* \| *`FUNC`*

为了紧凑，上述列表中省略了 *`NATIVE_TYPE_`* 前缀；例如，*`ARRAY`* 是 *`NATIVE_TYPE_ARRAY`* 的简写。

 | _ArrayElemType_ ::=
 | :----
 | _NativeIntrinsic_

_ParamNum_ 是一个无符号整数 (以 §[*Blobs*](#blob-reduce) 描述的方式压缩)，指定在方法调用中提供数组中元素数量的参数 — 参见下文。

_NumElem_ 是一个无符号整数 (以 §[*Blobs*](#blob-reduce) 描述的方式压缩)，指定元素或附加元素的数量 — 参见下文。

例如，在方法声明中：

 ```cil
 .method void M(int32[] ar1, int32 size1, unsigned int8[] ar2, int32 size2) { … }
 ```

`ar1` 参数可能拥有 **`FieldMarshal`** 表中的一行，该行索引 ***Blob Heap*** 中的 _MarshalSpec_，格式为：

 ```
 ARRAY  MAX  2  1
 ```

这表示参数被编组为 *`NATIVE_TYPE_ARRAY`*。关于每个元素的类型没有额外的信息 (由 *`NATIVE_TYPE_MAX`* 表示)。_ParamNum_ 的 Value 为 2，这表示方法中的参数编号 2 (名为 `size1` 的参数) 将指定实际数组中的元素数量 — 假设在特定调用中其 Value 为 42。_NumElem_ 的 Value 为 0。数组的总大小 (以字节为单位) 由以下公式给出：

 ```csharp
 if ParamNum = 0
    SizeInBytes = NumElem * sizeof (elem)
 else
    SizeInBytes = ( @ParamNum +  NumElem ) * sizeof (elem)
 endif
 ```

这里使用 `@ParamNum` 语法表示传入参数编号 _ParamNum_ 的 Value — 在这个例子中，它将是 42。每个元素的大小是从 `Foo` 的签名中的 `ar1` 参数的元数据计算出来的 — 是大小为 4 字节的 *`ELEMENT_TYPE_I4`*。

---
## 22. 元数据物理布局
<a id="metadata-physical-layout"></a>

元数据的物理磁盘表示是对逻辑表示的直接反映，逻辑表示在 §[Tables](#MetadataTables) 和 §[Others](#metadata-format-others) 中描述。也就是说，数据存储在表示元数据表和堆的流中。主要的复杂性在于，逻辑表示从索引到表和列所需的字节数中抽象出来，而物理表示必须通过定义如何将逻辑元数据堆和表映射到它们的物理表示来明确处理这个问题。

除非另有说明，所有的二进制值都以小端格式存储。

>---
### 22.1. 固定字段
<a id="Fixed-Field"></a>

完整的 CLI 组件 (元数据和 CIL 指令) 存储在当前可移植可执行 (PE) 文件格式的一个子集中 [[↗]](#pe-extension)。由于这种传递，元数据的物理表示中的一些字段具有固定值。在写入这些字段时，最好将它们设置为指示的值，读取时应忽略它们。

>---
### 22.2. File headers

#### 22.2.1. 元数据根

物理元数据的根开始于一个 ***Magic Signature***，接着是几个字节的 *Version* 和其他杂项信息，然后是一个计数和一个 ***Stream Headers*** 数组，每个流对应一个 ***Header***。实际的编码表和堆存储在流中，这些流紧接着这个 ***Header*** 数组。

 | Offset   | Size    | Field             | Description                                                                                                                                                      |
 | :------- | :------ | :---------------- | :--------------------------------------------------------------------------------------------------------------------------------------------------------------- |
 | 0        | 4       | **Signature**     | 物理元数据的 ***Magic Signature***：0x424A5342。                                                                                                                 |
 | 4        | 2       | **MajorVersion**  | 主版本，1 (读取时忽略)                                                                                                                                           |
 | 6        | 2       | **MinorVersion**  | 次版本，1 (读取时忽略)                                                                                                                                           |
 | 8        | 4       | **Reserved**      | 保留，始终为 0 [[↗]](#Fixed-Field)。                                                                                                                             |
 | 12       | 4       | **Length**        | 分配用来保存版本字符串 (包括空终止符) 的字节数，称之为 *x*。<p>称字符串 (包括终止符) 的长度为 *m* (我们要求 *m* &le; 255) ；长度 *x* 是 *m* 向上舍入到四的倍数。 |
 | 16       | *m*     | **Version**       | 长度为 *m* 的 UTF8 编码的空终止版本字符串 (见上文)                                                                                                               |
 | 16+*m*   | *x*-*m* | &nbsp;            | 填充到下一个4字节边界。                                                                                                                                          |
 | 16+*x*   | 2       | **Flags**         | 保留，始终为 0 [[↗]](#Fixed-Field)。                                                                                                                             |
 | 16+*x*+2 | 2       | **Streams**       | 流的数量，比如说 *n*。                                                                                                                                           |
 | 16+*x*+4 | &nbsp;  | **StreamHeaders** | *n* 个 `StreamHdr` 结构的数组。                                                                                                                                  |

对于任何打算在任何符合规范的 CLI 实现上执行的文件，版本字符串应为 "`Standard CLI 2005`"，所有符合规范的 CLI 实现都应接受使用此版本字符串的文件。当文件受限于特定于供应商的 CLI 实现时，应使用其他字符串。此标准的未来版本将指定不同的字符串，但它们应以 "`Standard CLI`" 开始。指定附加功能的其他标准应指定以 "`Standard□`" 开始的特定版本字符串，其中 "`□`" 表示一个空格。提供实现特定扩展的供应商应提供一个不以 "`Standard□`" 开始的版本字符串。对于此标准的第一个版本，版本字符串是 "`Standard CLI 2002`"。

#### 22.2.2. Stream Header

流头提供了表或堆的名称，位置和长度。请注意，流头结构的长度不是固定的，而取决于其名称字段的长度 (一个可变长度的空终止字符串)。

 | Offset | Size   | Field      | Description                                                                                              |
 | :----- | :----- | :--------- | :------------------------------------------------------------------------------------------------------- |
 | 0      | 4      | **Offset** | 从元数据根的开始到此流的开始的内存偏移。                                                                 |
 | 4      | 4      | **Size**   | 此流的字节大小，应为 4 的倍数。                                                                          |
 | 8      | &nbsp; | **Name**   | 流的名称，为 ASCII 字符的空终止可变长度数组，用 `\0` 字符填充到下一个 4 字节边界。名称限制为 32 个字符。 |

逻辑表和堆都存储在流中。有五种可能的流。一个名为 "`#Strings`" 的流头，指向存储标识符字符串的 ***String Heap*** 的物理表示；一个名为 "`#US`" 的流头，指向 ***Userstring Heap*** 的物理表示；一个名为 "`#Blob`" 的流头，指向 ***Blob Heap*** 的物理表示，一个名为 "`#GUID`" 的流头，指向 ***GUID Heap*** 的物理表示；以及一个名为"`#~`"的流头，指向一组表的物理表示。

每种类型的流最多只能出现一次，也就是说，元数据文件不应包含两个 "`#US`" 流，或五个 "`#Blob`" 流。如果流是空的，则不需要存在。

#### 22.2.3. #Strings Heap

由 "`#Strings`" 标头指向的字节流是逻辑 ***String Heap*** 的物理表示。物理堆可以包含垃圾，也就是说，它可以包含从任何表都无法访问的部分，但是从表可以访问的部分应该包含一个有效的空终止 UTF8 字符串。当 `#Strings` 堆存在时，第一个条目总是空字符串 (即，`\0`)。

#### 22.2.4. #US Heap 和 #Blob Heap

由 "`#US`" 或 "`#Blob`" 头指向的字节流分别是逻辑 ***Userstring Heap*** 和 ***Blob Heap*** 的物理表示。只要从任何表中可达的任何部分包含有效的 '*blob*'，这两个堆都可以包含垃圾。单个 *blob* 的长度在前几个字节中编码：

 * 如果 '*blob*' 的第一个字节是 _0bbbbbbb<sub>2</sub>_，那么 '*blob*' 的其余部分包含 _bbbbbbb<sub>2</sub>_ 字节的实际数据。

 * 如果 '*blob*' 的前两个字节是 _10bbbbbb<sub>2</sub>_ 和 *x*，那么 '*blob*' 的其余部分包含 (_bbbbbb<sub>2</sub>_ << 8 + *x*) 字节的实际数据。

 * 如果 '*blob*' 的前四个字节是 _110bbbbb<sub>2</sub>_, *x*, *y*, 和 *z*，那么 '*blob*' 的其余部分包含 (_bbbbb<sub>2</sub>_ << 24 + *x* << 16 + *y* << 8 + *z*) 字节的实际数据。

这两个堆中的第一个条目是空的 '*blob*'，由单个字节 0x00 组成。

 `#US` (用户字符串) 堆中的字符串使用 16 位 Unicode 编码。每个字符串的计数是字符串中的字节数 (不是字符)。此外，还有一个额外的终止字节 (因此所有字节计数都是奇数，而不是偶数)。这个最后的字节只有在字符串中的任何 UTF16 字符的顶字节设置了任何位，或者其低字节是以下任何一个：0x01 ~ 0x08, 0x0E ~ 0x1F, 0x27, 0x2D, 0x7F 时，才持有值 1。否则，它持有 0。1 表示需要超出通常为 8 位编码集提供的处理的 Unicode 字符。

#### 22.2.5. #GUID 堆
<a id="guid-heap"></a>

"`#GUID`" 标头指向一系列 128 位的 GUID。在流中可能存储了无法访问的 GUID。

#### 22.2.6. #~ 流
<a id="physicalStream"></a>

"`#~`" 流包含逻辑元数据表的实际物理表示 [[↗]](#MetadataTables)。"`#~`" 流具有以下顶级结构：

 | Offset    | Size   | Field            | Description                                            |
 | :-------- | :----- | :--------------- | :----------------------------------------------------- |
 | 0         | 4      | **Reserved**     | 保留，始终为 0 [[↗]](#Fixed-Field)。                   |
 | 4         | 1      | **MajorVersion** | 表模式的主版本；应为 2 [[↗]](#Fixed-Field)。           |
 | 5         | 1      | **MinorVersion** | 表模式的次版本；应为 0 [[↗]](#Fixed-Field)。           |
 | 6         | 1      | **HeapSizes**    | 堆大小的位向量。                                       |
 | 7         | 1      | **Reserved**     | 保留，始终为 1 [[↗]](#Fixed-Field)。                   |
 | 8         | 8      | **Valid**        | 存在表的位向量，让 *n* 是 1 的位的数量。               |
 | 16        | 8      | **Sorted**       | 已排序表的位向量。                                     |
 | 24        | 4\**n* | **Rows**         | 指示每个存在表的行数的 *n* 个 4 字节无符号整数的数组。 |
 | 24+4\**n* | &nbsp; | **Tables**       | 物理表的序列。                                         |

_HeapSizes_ 字段是一个位向量，它编码了各种堆的索引的宽度。如果位 0 被设置，"`#Strings`" 堆的索引宽度为 4 字节；如果位 1 被设置，"`#GUID`" 堆的索引宽度为 4 字节；如果位 2 被设置，"`#Blob`" 堆的索引宽度为 4 字节。相反，如果未设置特定堆的 _HeapSizes_ 位，那么该堆的索引宽度为 2 字节。

 | Heap size flag | Description                                 |
 | :------------- | :------------------------------------------ |
 | 0x01           | "`#Strings`" 流的大小 &ge; 2<sup>16</sup>。 |
 | 0x02           | "`#GUID`" 流的大小 &ge; 2<sup>16</sup>。    |
 | 0x04           | "`#Blob`" 流的大小 &ge; 2<sup>16</sup>。    |

_Valid_ 字段是一个 64 位的位向量，对于存储在流中的每个表，都有一个特定的位被设置；表到索引的映射在 [_Metadata Tables_](#MetadataTables) 的开始处给出。例如，当 **`DeclSecurity`** 表在逻辑元数据中存在时，应在 *Valid* 向量中设置位 0x0e。在 _Valid_ 中包含不存在的表是无效的，因此所有位于 0x2c 以上的位都应为零。

_Rows_ 数组包含每个存在的表的行数。在将物理元数据解码为逻辑元数据时，_Valid_ 中 1 的数量表示 _Rows_ 数组中的元素数量。

在编码逻辑表的过程中，一个关键的方面是其模式。每个表的模式在 [_Metadata Tables_](#MetadataTables) 中给出。例如，分配索引 0x02 的表是一个 **`TypeDef`** 表，根据其在 [*TypeDef_0x02*](#TypeDef_0x02) 中的规范，它具有以下列：一个 4 字节宽的标志，一个指向 ***String Heap*** 的索引，另一个指向 ***String Heap*** 的索引，一个指向 **`TypeDef`**、**`TypeRef`** 或 **`TypeSpec`** 表的索引，一个指向 **`Field`** 表的索引，以及一个指向 **`MethodDef`** 表的索引。

具有 *n* 列和 *m* 行的表的物理表示，其模式为 (*C*<sub>0</sub>,&hellip;,*C*<sub>*n*-1</sub>)，由每个行的物理表示的连接组成。具有模式 (*C*<sub>0</sub>,&hellip;,*C*<sub>n-1</sub>) 的行的物理表示是每个元素的物理表示的连接。在类型为 *C* 的列中，行单元 *e* 的物理表示定义如下：
 * 如果 *e* 是一个常量，它使用其列类型 *C* 指定的字节数存储 (即，_PropertyAttributes_ 类型的2位掩码)
 * 如果 *e* 是一个指向 ***GUID Heap***、***Blob Heap*** 或 ***String Heap*** 的索引，它使用 _HeapSizes_ 字段定义的字节数存储。
 * 如果 *e* 是一个简单的指向索引为 *i* 的表的索引，如果表 *i* 的行数小于 216，则使用 2 字节存储，否则使用 4 字节存储。
 * 如果 *e* 是一个编码索引，它指向 *n* 个可能的表 *t*<sub>0</sub>,&hellip;*t*<sub>*n*-1</sub> 中的表 *t*<sub>*i*</sub>，那么它被存储为 *e* << (log *n*) | tag{ *t*<sub>0</sub>,&hellip;*t*<sub>*n*-1</sub> } [ *t*<sub>*i*</sub> ]，如果表 *t*<sub>0</sub>,&hellip;*t*<sub>*n*-1</sub> 的最大行数小于 2<sup>(16 – (log *n*))</sup>，则使用 2 字节存储，否则使用 4 字节存储。有限映射族 tag{ *t*<sub>0</sub>,&hellip;*t*<sub>*n*-1</sub> } 在下面定义。注意，解码物理行需要这个映射的逆。(例如，**`Constant`** 表的 ***`Parent`*** 列索引 **`Field`**、**`Param`** 或 **`Property`** 表中的一行。实际的表被编码到数字的低 2 位，使用值：0 => **`Field`**，1 => **`Param`**，2 => **`Property`**。剩余的位保存了被索引的实际行号。例如，值 0x321，索引 **`Param`** 表中的行号 0xC8。)

> *TypeDefOrRef*  <a id="TypeDefOrRef"></a>

 | TypeDefOrRef: 2 bits to encode tag |  Tag  |
 | :--------------------------------- | :---: |
 | **`TypeDef`**                      |   0   |
 | **`TypeRef`**                      |   1   |
 | **`TypeSpec`**                     |   2   |

> *HasConstant* <a id="HasConstant"></a>

 | HasConstant: 2 bits to encode tag |  Tag  |
 | :-------------------------------- | :---: |
 | **`Field`**                       |   0   |
 | **`Param`**                       |   1   |
 | **`Property`**                    |   2   |

> *HasCustomAttribute* <a id="HasCustomAttribute"></a>

| HasCustomAttribute: 5 bits to encode tag |  Tag  |
| :--------------------------------------- | :---: |
| **`MethodDef`**                          |   0   |
| **`Field`**                              |   1   |
| **`TypeRef`**                            |   2   |
| **`TypeDef`**                            |   3   |
| **`Param`**                              |   4   |
| **`InterfaceImpl`**                      |   5   |
| **`MemberRef`**                          |   6   |
| **`Module`**                             |   7   |
| **`Permission`**                         |   8   |
| **`Property`**                           |   9   |
| **`Event`**                              |  10   |
| **`StandAloneSig`**                      |  11   |
| **`ModuleRef`**                          |  12   |
| **`TypeSpec`**                           |  13   |
| **`Assembly`**                           |  14   |
| **`AssemblyRef`**                        |  15   |
| **`File`**                               |  16   |
| **`ExportedType`**                       |  17   |
| **`ManifestResource`**                   |  18   |
| **`GenericParam`**                       |  19   |
| **`GenericParamConstraint`**             |  20   |
| **`MethodSpec`**                         |  21   |

 _HasCustomAttributes_ 只有对应于用户源程序中的项目的表的值；例如，可以将特性附加到 **`TypeDef`** 表和 **`Field`** 表，但不能附加到 **`ClassLayout`** 表。因此，上面的枚举中缺少一些表类型。

> *HasFieldMarshall* <a id="HasFieldMarshall"></a>

 | HasFieldMarshall: 1 bit to encode tag |  Tag  |
 | :------------------------------------ | :---: |
 | **`Field`**                           |   0   |
 | **`Param`**                           |   1   |

> *HasDeclSecurity* <a id="HasDeclSecurity"></a>

 | HasDeclSecurity: 2 bits to encode tag |  Tag  |
 | :------------------------------------ | :---: |
 | **`TypeDef`**                         |   0   |
 | **`MethodDef`**                       |   1   |
 | **`Assembly`**                        |   2   |

> *MemberRefParent* <a id="MemberRefParent"></a>

 | MemberRefParent: 3 bits to encode tag |  Tag  |
 | :------------------------------------ | :---: |
 | **`TypeDef`**                         |   0   |
 | **`TypeRef`**                         |   1   |
 | **`ModuleRef`**                       |   2   |
 | **`MethodDef`**                       |   3   |
 | **`TypeSpec`**                        |   4   |

> *HasSemantics* <a id="HasSemantics"></a>

 | HasSemantics: 1 bit to encode tag |  Tag  |
 | :-------------------------------- | :---: |
 | **`Event`**                       |   0   |
 | **`Property`**                    |   1   |

> *MethodDefOrRef* <a id="MethodDefOrRef"></a>

 | MethodDefOrRef: 1 bit to encode tag |  Tag  |
 | :---------------------------------- | :---: |
 | **`MethodDef`**                     |   0   |
 | **`MemberRef`**                     |   1   |

> *MemberForwarded* <a id="MemberForwarded"></a>

 | MemberForwarded: 1 bit to encode tag |  Tag  |
 | :----------------------------------- | :---: |
 | **`Field`**                          |   0   |
 | **`MethodDef`**                      |   1   |

> *Implementation* <a id="Implementation"></a>

 | Implementation: 2 bits to encode tag |  Tag  |
 | :----------------------------------- | :---: |
 | **`File`**                           |   0   |
 | **`AssemblyRef`**                    |   1   |
 | **`ExportedType`**                   |   2   |

> *CustomAttributeType* <a id="CustomAttributeType"></a>

 | CustomAttributeType: 3 bits to encode tag |  Tag  |
 | :---------------------------------------- | :---: |
 | Not used                                  |   0   |
 | Not used                                  |   1   |
 | **`MethodDef`**                           |   2   |
 | **`MemberRef`**                           |   3   |
 | Not used                                  |   4   |

> *ResolutionScope* <a id="ResolutionScope"></a>

 | ResolutionScope: 2 bits to encode tag |  Tag  |
 | :------------------------------------ | :---: |
 | **`Module`**                          |   0   |
 | **`ModuleRef`**                       |   1   |
 | **`AssemblyRef`**                     |   2   |
 | **`TypeRef`**                         |   3   |

> *TypeOrMethodDef* <a id="TypeOrMethodDef"></a>

 | TypeOrMethodDef: 1 bit to encode tag |  Tag  |
 | :----------------------------------- | :---: |
 | **`TypeDef`**                        |   0   |
 | **`MethodDef`**                      |   1   |

---
## 23. PE 文件格式的扩展
<a id="pe-extension"></a>

CLI组件的文件格式是当前可移植可执行 (PE) 文件格式的严格扩展。这种扩展的PE格式使操作系统能够识别运行时图像，适应以CIL或本地代码发出的代码，并将运行时元数据作为发出的代码的组成部分。还有一些关于完整的Windows PE/COFF文件格式子集的规范，详细到工具或编译器可以使用这些规范来发出有效的CLI图像。

PE格式经常使用RVA (相对虚拟地址) 这个术语。RVA是一个项目*一旦加载到内存中*的地址，从中减去了图像文件的基地址 (即，从文件加载的基地址开始的偏移)。一个项目的RVA几乎总是与其在磁盘上的文件位置不同。要计算具有RVA *r*的项目的文件位置，搜索PE文件中的所有部分，找到RVA *s*，长度 *l* 和文件位置 *p* 的部分，其中RVA位于，即 *s* &le; *r* < *s*+*l*。然后，该项目的文件位置由 *p*+(*r*-*s*)给出。

除非另有说明，否则所有二进制值都以小端格式存储。

> _结束信息性文本。_

### 23.1. 运行时文件格式的结构

下图提供了 CLI 文件格式的高级视图。所有运行时图像都包含以下内容：

 * PE 头，其中包含如何在运行时文件中设置字段值的具体指南。

 * 一个 CLI 头，其中包含所有特定于运行时的数据条目。运行时头是只读的，应放置在任何只读部分中。

 * 包含头描述的实际数据的部分，包括导入/导出、数据和代码。

 ![CLI 文件格式的高级视图](ii.25.1-structure-of-the-runtime-file-format-figure-1.png)

使用 PE 头中的 CLI 头目录条目找到 CLI 头 (§[II.25.3.3](ii.25.3.3-cli-header.md))。CLI 头反过来包含了图像其余部分的运行时数据的地址和大小 (对于元数据，参见 §[II.24](ii.24-metadata-physical-layout.md)；对于 CIL，参见 §[II.25.4](ii.25.4-common-intermediate-language-physical-layout.md))。请注意，基于部分的属性 (如只读与执行等)，运行时数据可以与 PE 格式的其他数据合并到 PE 格式的其他区域中。

### 23.2. PE 头

PE 图像以 MS-DOS 头开始，接着是 PE 签名，然后是 PE 文件头，然后是 PE 可选头，最后是 PE 节头。

#### 23.2.1. MS-DOS 头

PE 格式以一个 MS-DOS 存根开始，精确地放置在模块的前面的 128 字节。在 DOS 头的偏移 0x3c 处是一个 4 字节的无符号整数偏移，_lfanew_，指向 PE 签名 (应为 "`PE\0\0`")，紧接着是 PE 文件头。

| &nbsp; | &nbsp; | &nbsp; | &nbsp; | &nbsp;   | &nbsp; | &nbsp; | &nbsp; |
| ------ | ------ | ------ | ------ | -------- | ------ | ------ | ------ |
| 0x4d   | 0x5a   | 0x90   | 0x00   | 0x03     | 0x00   | 0x00   | 0x00   |
| 0x04   | 0x00   | 0x00   | 0x00   | 0xFF     | 0xFF   | 0x00   | 0x00   |
| 0xb8   | 0x00   | 0x00   | 0x00   | 0x00     | 0x00   | 0x00   | 0x00   |
| 0x40   | 0x00   | 0x00   | 0x00   | 0x00     | 0x00   | 0x00   | 0x00   |
| 0x00   | 0x00   | 0x00   | 0x00   | 0x00     | 0x00   | 0x00   | 0x00   |
| 0x00   | 0x00   | 0x00   | 0x00   | 0x00     | 0x00   | 0x00   | 0x00   |
| 0x00   | 0x00   | 0x00   | 0x00   | 0x00     | 0x00   | 0x00   | 0x00   |
| 0x00   | 0x00   | 0x00   | 0x00   | _lfanew_ | &nbsp; | &nbsp; | &nbsp; |
| 0x0e   | 0x1f   | 0xba   | 0x0e   | 0x00     | 0xb4   | 0x09   | 0xcd   |
| 0x21   | 0xb8   | 0x01   | 0x4c   | 0xcd     | 0x21   | 0x54   | 0x68   |
| 0x69   | 0x73   | 0x20   | 0x70   | 0x72     | 0x6f   | 0x67   | 0x72   |
| 0x61   | 0x6d   | 0x20   | 0x63   | 0x61     | 0x6e   | 0x6e   | 0x6f   |
| 0x74   | 0x20   | 0x62   | 0x65   | 0x20     | 0x72   | 0x75   | 0x6e   |
| 0x20   | 0x69   | 0x6e   | 0x20   | 0x44     | 0x4f   | 0x53   | 0x20   |
| 0x6d   | 0x6f   | 0x64   | 0x65   | 0x2e     | 0x0d   | 0x0d   | 0x0a   |
| 0x24   | 0x00   | 0x00   | 0x00   | 0x00     | 0x00   | 0x00   | 0x00   |

#### 23.2.2. PE文件头

在PE签名之后紧接着是PE文件头，包括以下内容：

 | 偏移 | 大小 | 字段                    | 描述                                                                       |
 | ---- | ---- | ----------------------- | -------------------------------------------------------------------------- |
 | 0    | 2    | Machine                 | 总是0x14c。                                                                |
 | 2    | 2    | Number of Sections      | 节的数量；表示紧接在头部之后的节表的大小。                                 |
 | 4    | 4    | Time/Date Stamp         | 文件创建的时间和日期，以1970年1月1日00:00:00以来的秒数表示，或为0。        |
 | 8    | 4    | Pointer to Symbol Table | 总是0 (§[II.24.1](ii.24.1-fixed-fields.md))。                              |
 | 12   | 4    | Number of Symbols       | 总是0 (§[II.24.1](ii.24.1-fixed-fields.md))。                              |
 | 16   | 2    | Optional Header Size    | 可选头的大小，格式在下面描述。                                             |
 | 18   | 2    | Characteristics         | 标志，表示文件的属性，参见§[II.25.2.2.1](ii.25.2.2.1-characteristics.md)。 |

##### 23.2.2.1. 特性

一个仅 CIL 的 DLL 将标志 0x2000 设置为 1，而一个仅 CIL 的 `.exe` 将标志 0x2000 设置为零：

 | 标志                          | 值     | 描述                                                                   |
 | ----------------------------- | ------ | ---------------------------------------------------------------------- |
 | `IMAGE_FILE_RELOCS_STRIPPED`  | 0x0001 | 应为零                                                                 |
 | `IMAGE_FILE_EXECUTABLE_IMAGE` | 0x0002 | 应为一                                                                 |
 | `IMAGE_FILE_32BIT_MACHINE`    | 0x0100 | 当且仅当 `COMIMAGE_FLAGS_32BITREQUIRED` 为一时，应为一 (§[25.3.3.1]()) |
 | `IMAGE_FILE_DLL`              | 0x2000 | 图像文件是一个动态链接库 (DLL)。                                       |

对于上述未提到的标志，标志 0x0010，0x0020，0x0400 和 0x0800 是实现特定的，所有其他的应该为零 (§[II.24.1](ii.24.1-fixed-fields.md))。


#### 23.2.3. PE 可选头

紧接着 PE 头是 PE 可选头。此头包含以下信息：

 | 偏移 | 大小 | 头部分      | 描述                                                                                                                  |
 | ---- | ---- | ----------- | --------------------------------------------------------------------------------------------------------------------- |
 | 0    | 28   | 标准字段    | 这些定义了 PE 文件的一般属性，参见 §[II.25.2.3.1](ii.25.2.3.1-pe-header-standard-fields.md)。                         |
 | 28   | 68   | NT 特定字段 | 这些包括支持 Windows 的特定功能的附加字段，参见 §[II.25.2.3.2](ii.25.2.3.2-pe-header-windows-nt-specific-fields.md)。 |
 | 96   | 128  | 数据目录    | 这些字段是地址/大小对，用于在图像文件中找到的特殊表 (例如，导入表和导出表)。                                          |

##### 23.2.3.1. PE 头标准字段

所有 PE 文件都需要这些字段，并包含以下信息：

 | 偏移 | 大小 | 字段                    | 描述                                                                                              |
 | ---- | ---- | ----------------------- | ------------------------------------------------------------------------------------------------- |
 | 0    | 2    | Magic                   | 始终为 0x10B。                                                                                    |
 | 2    | 1    | LMajor                  | 始终为 6 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                  |
 | 3    | 1    | LMinor                  | 始终为 0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                  |
 | 4    | 4    | Code Size               | 代码 (文本) 部分的大小，或者如果有多个部分，则为所有代码部分的总和。                              |
 | 8    | 4    | Initialized Data Size   | 初始化数据部分的大小，或者如果有多个数据部分，则为所有这些部分的总和。                            |
 | 12   | 4    | Uninitialized Data Size | 未初始化数据部分的大小，或者如果有多个未初始化的数据部分，则为所有这些部分的总和。                |
 | 16   | 4    | Entry Point RVA         | 入口点 RVA，需要指向字节 0xFF 0x25，后面跟着在标记为执行/读取的部分中的 RVA，对于 EXE 或 DLL 为 0 |
 | 20   | 4    | Base Of Code            | 代码部分的 RVA。 (这是对加载器的提示。)                                                           |
 | 24   | 4    | Base Of Data            | 数据部分的 RVA。 (这是对加载器的提示。)                                                           |

> _这只包含信息性文本。_

入口点 RVA 应始终是 x86 入口点存根或 0。在非 CLI 知道的平台上，此存根将调用 `mscoree` 的入口点 API (`_CorExeMain` 或 `_CorDllMain`)。`mscoree` 入口点将使用模块句柄从图像加载元数据，并调用 CLI 头中指定的入口点。

> _结束信息性文本。_

##### 23.2.3.2. PE头部 Windows NT特定字段

这些字段是Windows NT特定的：

 | 偏移量 | 大小 | 字段                       | 描述                                                                                                     |
 | ------ | ---- | -------------------------- | -------------------------------------------------------------------------------------------------------- |
 | 28     | 4    | Image Base                 | 应为0x10000的倍数。                                                                                      |
 | 32     | 4    | Section Alignment          | 应大于File Alignment。                                                                                   |
 | 36     | 4    | File Alignment             | 应为0x200 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                        |
 | 40     | 2    | OS Major                   | 应为5 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                            |
 | 42     | 2    | OS Minor                   | 应为0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                            |
 | 44     | 2    | User Major                 | 应为0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                            |
 | 46     | 2    | User Minor                 | 应为0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                            |
 | 48     | 2    | SubSys Major               | 应为5 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                            |
 | 50     | 2    | SubSys Minor               | 应为0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                            |
 | 52     | 4    | Reserved                   | 应为零                                                                                                   |
 | 56     | 4    | Image Size                 | 图像的字节大小，包括所有头和填充；应为Section Alignment的倍数。                                          |
 | 60     | 4    | Header Size                | MS-DOS头、PE头、PE可选头和填充的组合大小；应为文件对齐的倍数。                                           |
 | 64     | 4    | File Checksum              | 应为0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                            |
 | 68     | 2    | SubSystem                  | 运行此图像所需的子系统。应为`IMAGE_SUBSYSTEM_WINDOWS_CUI` (0x3) 或 `IMAGE_SUBSYSTEM_WINDOWS_GUI` (0x2)。 |
 | 70     | 2    | DLL Flags                  | 位0x100f应为零。                                                                                         |
 | 72     | 4    | Stack Reserve Size         | 应为0x100000 (1Mb) (§[II.24.1](ii.24.1-fixed-fields.md))。                                               |
 | 76     | 4    | Stack Commit Size          | 应为0x1000 (4Kb) (§[II.24.1](ii.24.1-fixed-fields.md))。                                                 |
 | 80     | 4    | Heap Reserve Size          | 应为0x100000 (1Mb) (§[II.24.1](ii.24.1-fixed-fields.md))。                                               |
 | 84     | 4    | Heap Commit Size           | 应为0x1000 (4Kb) (§[II.24.1](ii.24.1-fixed-fields.md))。                                                 |
 | 88     | 4    | Loader Flags               | 应为0                                                                                                    |
 | 92     | 4    | Number of Data Directories | 应为0x10                                                                                                 |


##### 23.2.3.3. PE 头数据目录

可选的头数据目录给出了在 PE 文件的各个部分中出现的几个表的地址和大小。每个数据目录条目按顺序包含它所描述的结构的 RVA 和大小。

 | 偏移量 | 大小 | 字段           | 描述                                                                                                  |
 | ------ | ---- | -------------- | ----------------------------------------------------------------------------------------------------- |
 | 96     | 8    | 导出表         | 始终为0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                       |
 | 104    | 8    | 导入表         | 导入表的 RVA 和大小，(§[II.25.3.1](ii.25.3.1-import-table-and-import-address-table-iat.md))。         |
 | 112    | 8    | 资源表         | 始终为0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                       |
 | 120    | 8    | 异常表         | 始终为0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                       |
 | 128    | 8    | 证书表         | 始终为0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                       |
 | 136    | 8    | 基址重定位表   | 重定位表；如果未使用则设置为0 (§)。                                                                   |
 | 144    | 8    | 调试           | 始终为0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                       |
 | 152    | 8    | 版权           | 始终为0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                       |
 | 160    | 8    | 全局指针       | 始终为0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                       |
 | 168    | 8    | TLS 表         | 始终为0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                       |
 | 176    | 8    | 加载配置表     | 始终为0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                       |
 | 184    | 8    | 绑定导入       | 始终为0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                       |
 | 192    | 8    | IAT            | 导入地址表的 RVA 和大小，(§[II.25.3.1](ii.25.3.1-import-table-and-import-address-table-iat.md))。     |
 | 200    | 8    | 延迟导入描述符 | 始终为0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                       |
 | 208    | 8    | CLI 头         | 带有运行时数据目录的 CLI 头，(§[II.25.3.1](ii.25.3.1-import-table-and-import-address-table-iat.md))。 |
 | 216    | 8    | 保留           | 始终为0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                       |

由目录条目指向的表存储在 PE 文件的一个部分中；这些部分本身由部分头描述。



### 23.3. 节头

紧接在可选头之后的是节表，其中包含了许多节头。这种位置是必需的，因为文件头并未包含指向节表的直接指针；节表的位置是通过计算头部后的第一个字节的位置来确定的。

每个节头都有以下格式，每个条目总共40字节：

 | 偏移量 | 大小 | 字段                 | 描述                                                                                                                                                                                                                                                          |
 | ------ | ---- | -------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
 | 0      | 8    | Name                 | 一个8字节，空值填充的ASCII字符串。如果字符串正好是八个字符长，那么就没有终止空值。                                                                                                                                                                            |
 | 8      | 4    | VirtualSize          | 节的总字节大小。如果此值大于SizeOfRawData，节将被零填充。                                                                                                                                                                                                     |
 | 12     | 4    | VirtualAddress       | 对于可执行图像，这是加载到内存中的节的第一个字节的地址，相对于图像基址。                                                                                                                                                                                      |
 | 16     | 4    | SizeOfRawData        | 磁盘上初始化数据的大小，以字节为单位，应为PE头中的FileAlignment的倍数。如果这个值小于VirtualSize，那么节的剩余部分将被零填充。因为这个字段是四舍五入的，而VirtualSize字段不是，所以这个值可能大于VirtualSize。当一个节只包含未初始化的数据时，这个字段应为0。 |
 | 20     | 4    | PointerToRawData     | PE文件中节的第一页的偏移量。这应该是可选头中的FileAlignment的倍数。当一个节只包含未初始化的数据时，这个字段应为0。                                                                                                                                            |
 | 24     | 4    | PointerToRelocations | 应为0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                                                                                                                                                                                 |
 | 28     | 4    | PointerToLinenumbers | 应为0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                                                                                                                                                                                 |
 | 32     | 2    | NumberOfRelocations  | 应为0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                                                                                                                                                                                 |
 | 34     | 2    | NumberOfLinenumbers  | 应为0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                                                                                                                                                                                 |
 | 36     | 4    | Characteristics      | 描述节特性的标志；见下文。                                                                                                                                                                                                                                    |

下表定义了节可能的特性。

 | 标志                               | 值         | 描述                   |
 | ---------------------------------- | ---------- | ---------------------- |
 | `IMAGE_SCN_CNT_CODE`               | 0x00000020 | 节包含代码。           |
 | `IMAGE_SCN_CNT_INITIALIZED_DATA`   | 0x00000040 | 节包含初始化的数据。   |
 | `IMAGE_SCN_CNT_UNINITIALIZED_DATA` | 0x00000080 | 节包含未初始化的数据。 |
 | `IMAGE_SCN_MEM_EXECUTE`            | 0x20000000 | 节可以作为代码执行。   |
 | `IMAGE_SCN_MEM_READ`               | 0x40000000 | 节可以被读取。         |
 | `IMAGE_SCN_MEM_WRITE`              | 0x80000000 | 节可以被写入。         |

#### 23.3.1. 导入表和导入地址表 (IAT)

导入表和导入地址表 (IAT) 用于导入运行时引擎 (`mscoree.dll`) 的 `_CorExeMain` (对于 `.exe`) 或 `_CorDllMain` (对于 `.dll`) 条目。导入表目录条目指向一个元素零终止的导入目录条目数组 (在一般的 PE 文件中，每个导入的 DLL 有一个条目) ：

 | 偏移 | 大小 | 字段               | 描述                                                      |
 | ---- | ---- | ------------------ | --------------------------------------------------------- |
 | 0    | 4    | ImportLookupTable  | 导入查找表的 RVA                                          |
 | 4    | 4    | DateTimeStamp      | 始终为 0 (§[II.24.1](ii.24.1-fixed-fields.md))。          |
 | 8    | 4    | ForwarderChain     | 始终为 0 (§[II.24.1](ii.24.1-fixed-fields.md))。          |
 | 12   | 4    | Name               | 空终止 ASCII 字符串 "`mscoree.dll`" 的 RVA。              |
 | 16   | 4    | ImportAddressTable | 导入地址表的 RVA (这与可选头中的 IAT 描述符的 RVA 相同)。 |
 | 20   | 20   | &nbsp;             | 导入表的结束。应填充为零。                                |

导入查找表和导入地址表 (IAT) 都是一个元素，零终止的 RVA 数组，指向 Hint/Name 表。RVA 的第 31 位应设置为 0。在一般的 PE 文件中，此表中每个导入的符号有一个条目。

 | 偏移 | 大小 | 字段                | 描述                                                                |
 | ---- | ---- | ------------------- | ------------------------------------------------------------------- |
 | 0    | 4    | Hint/Name Table RVA | 指向 Hint/Name 表的 31 位 RVA。第 31 位应设置为 0，表示按名称导入。 |
 | 4    | 4    | &nbsp;              | 表的结束，应填充为零。                                              |

IAT 应位于可执行和可写的部分，因为加载器将用导入符号的实际入口点替换指向 Hint/Name 表的指针。

Hint/Name 表包含导入的 dll-entry 的名称。

 | 偏移 | 大小 | 字段 | 描述                                                                                                                                |
 | ---- | ---- | ---- | ----------------------------------------------------------------------------------------------------------------------------------- |
 | 0    | 2    | Hint | 应为 0。                                                                                                                            |
 | 2    | 变量 | Name | 包含要导入的名称的区分大小写的空终止 ASCII 字符串。对于 `.exe` 文件，应为 "`_CorExeMain`"，对于 `.dll` 文件，应为 "`_CorDllMain`"。 |

#### 23.3.2. 重定位

在纯 CIL 图像中，需要一个类型为 `IMAGE_REL_BASED_HIGHLOW` (0x3) 的修复，用于访问 IAT 加载运行时引擎的 x86 启动存根。当构建混合的 CIL/本机图像或者当图像包含用户数据中的嵌入式 RVA 时，重定位部分也包含这些重定位。

重定位应该在它们自己的部分中，名为 "`.reloc`"，这应该是 PE 文件中的最后一个部分。重定位部分包含一个修复表。修复表被分解成多个修复块。每个块代表一个 4K 页面的修复，每个块应该从 32 位边界开始。

每个修复块开始于以下结构：

 | 偏移量 | 大小 | 字段       | 描述                                                                                                             |
 | ------ | ---- | ---------- | ---------------------------------------------------------------------------------------------------------------- |
 | 0      | 4    | PageRVA    | 需要应用修复的块的 RVA。低 12 位应为零。                                                                         |
 | 4      | 4    | Block Size | 修复块中的总字节数，包括 Page RVA 和 Block Size 字段，以及后面的 Type/Offset 字段，向上取整到最接近的 4 的倍数。 |

然后，Block Size 字段后面跟着 (BlockSize - 8)/2 Type/Offset。每个条目是一个字 (2 字节)，并具有以下结构 (如果需要，插入 2 字节的 0 以填充到 4 字节的长度的倍数) ：

 | 偏移量 | 大小    | 字段   | 描述                                                                                                 |
 | ------ | ------- | ------ | ---------------------------------------------------------------------------------------------------- |
 | 0      | 4 bits  | Type   | 存储在字的高 4 位。值表示要应用哪种类型的修复 (如上所述)                                             |
 | 0      | 12 bits | Offset | 存储在字的剩余 12 位。从块的 Page RVA 字段指定的起始地址的偏移量。此偏移量指定了修复应该应用的位置。 |

#### 23.3.3. CLI 头

CLI 头包含所有特定于运行时的数据条目和其他信息。头应放置在图像的只读、可共享的部分。此头定义如下：

 | 偏移 | 大小 | 字段                    | 描述                                                                      |
 | ---- | ---- | ----------------------- | ------------------------------------------------------------------------- |
 | 0    | 4    | Cb                      | 头的字节大小                                                              |
 | 4    | 2    | MajorRuntimeVersion     | 运行此程序所需的运行时的最小版本，当前为 2。                              |
 | 6    | 2    | MinorRuntimeVersion     | 版本的次要部分，当前为 0。                                                |
 | 8    | 8    | MetaData                | 物理元数据的 RVA 和大小 (§[II.24](ii.24-metadata-physical-layout.md))。   |
 | 16   | 4    | Flags                   | 描述此运行时图像的标志。(§[II.25.3.3.1](ii.25.3.3.1-runtime-flags.md))。  |
 | 20   | 4    | EntryPointToken         | 图像入口点的 _MethodDef_ 或 _File_ 的 _token_                             |
 | 24   | 8    | Resources               | 实现特定资源的 RVA 和大小。                                               |
 | 32   | 8    | StrongNameSignature     | 此 PE 文件的哈希数据的 RVA，由 CLI 加载器用于绑定和版本控制               |
 | 40   | 8    | CodeManagerTable        | 始终为 0 (§[II.24.1](ii.24.1-fixed-fields.md))。                          |
 | 48   | 8    | VTableFixups            | 文件中包含函数指针数组 (例如，vtable 插槽) 的位置的数组的 RVA，参见下文。 |
 | 56   | 8    | ExportAddressTableJumps | 始终为 0 (§[II.24.1](ii.24.1-fixed-fields.md))。                          |
 | 64   | 8    | ManagedNativeHeader     | 始终为 0 (§[II.24.1](ii.24.1-fixed-fields.md))。                          |

##### 23.3.3.1. 运行时标志

以下标志描述了此运行时图像，并被加载器使用。所有未指定的位应为零。

 | 标志                               | 值         | 描述                                                                                                                                                           |
 | ---------------------------------- | ---------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------- |
 | `COMIMAGE_FLAGS_ILONLY`            | 0x00000001 | 应为1。                                                                                                                                                        |
 | `COMIMAGE_FLAGS_32BITREQUIRED`     | 0x00000002 | 图像只能加载到32位进程中，例如，如果有32位的vtablefixups，或者从`native integer`到`int32`的转换。具有64位本地整数的CLI实现应拒绝加载设置了此标志的二进制文件。 |
 | `COMIMAGE_FLAGS_STRONGNAMESIGNED`  | 0x00000008 | 图像有强名称签名。                                                                                                                                             |
 | `COMIMAGE_FLAGS_NATIVE_ENTRYPOINT` | 0x00000010 | 应为0。                                                                                                                                                        |
 | `COMIMAGE_FLAGS_TRACKDEBUGDATA`    | 0x00010000 | 应为0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                                                                                  |

##### 23.3.3.2. 入口点元数据 _token_

 * 入口点 _token_  (§[II.15.4.1.2](ii.15.4.1.2-the-entrypoint-directive.md)) 在多模块程序集的入口点不在清单程序集中时，始终是 _MethodDef_  _token_  (§[II.22.26](ii.22.26-methoddef-0x06.md)) 或 _File_  _token_  (§[II.22.19](ii.22.19-file-0x26.md))。方法的元数据中的签名和实现标志指示如何运行入口。

##### 23.3.3.3. Vtable 修复

某些选择不遵循公共类型系统运行时模型的语言可以拥有需要在 v-table 中表示的虚函数。这些 v-table 是由编译器布局的，而不是由运行时布局的。找到正确的 v-table 插槽并通过该插槽中保存的值间接调用也是由编译器完成的。运行时头中的 **VtableFixups** 字段包含 Vtable 修复数组的位置和大小 (参见 §[II.15.5.1](ii.15.5.1-method-transition-thunks.md))。V-table 应该被发射到 PE 文件的 *读-写* 部分。

此数组中的每个条目描述了指定大小的 v-table 插槽的连续数组。每个插槽开始时都初始化为它们需要调用的方法的元数据 _token_ 值。在图像加载时，运行时加载器将每个条目转换为 CPU 的机器代码的指针，并可以直接调用。

 | 偏移量 | 大小 | 字段               | 描述                     |
 | ------ | ---- | ------------------ | ------------------------ |
 | 0      | 4    | **VirtualAddress** | Vtable 的 RVA            |
 | 4      | 2    | **Size**           | Vtable 中的条目数        |
 | 6      | 2    | **Type**           | 条目的类型，如下表所定义 |

 | 常量                           | 值   | 描述                                                 |
 | ------------------------------ | ---- | ---------------------------------------------------- |
 | `COR_VTABLE_32BIT`             | 0x01 | Vtable 插槽是 32 位的。                              |
 | `COR_VTABLE_64BIT`             | 0x02 | Vtable 插槽是 64 位的。                              |
 | `COR_VTABLE_FROM_UNMANAGED`    | 0x04 | 从非托管代码转换到托管代码。                         |
 | `COR_VTABLE_CALL_MOST_DERIVED` | 0x10 | 调用由 _token_ 描述的最派生的方法 (仅对虚方法有效)。 |

##### 23.3.3.4. 强名称签名

此头部条目指向一个图像的强名称哈希，可以用来确定性地从引用点 (§[II.6.2.1.3](ii.6.2.1.3-originators-public-key.md)) 识别一个模块。

### 23.4. CIL 物理布局
<a id="CIL-physical-layout"></a>

本节包含用于描述 CIL 方法及其异常的数据结构的布局。方法体可以存储在 PE 文件的任何只读部分。元数据中的 _MethodDef_ 记录 (§[II.22.26](ii.22.26-methoddef-0x06.md)) 携带每个方法的 RVA。

一个方法由方法头紧接着方法体组成，可能后面还跟着额外的方法数据部分 (§[II.25.4.5](ii.25.4.5-method-data-section.md))，通常是异常处理数据。如果存在异常处理数据，那么 `CorILMethod_MoreSects` 标志 (§[II.25.4.4](ii.25.4.4-flags-for-method-headers.md)) 应在方法头和之后的每个链式项中指定。

方法头有两种形式 &ndash; tiny (§[II.25.4.2](ii.25.4.2-tiny-format.md)) 和 fat (§[II.25.4.3](ii.25.4.3-fat-format.md))。方法头中的两个最低有效位指示哪种类型存在 (§[II.25.4.1](ii.25.4.1-method-header-type-values.md))。tiny 头长 1 字节，只存储方法的代码大小。如果一个方法没有局部变量，maxstack 是 8 或更小，方法没有异常，方法大小小于 64 字节，并且方法没有高于 0x7 的标志，那么该方法将被赋予一个 tiny 头。fat 头携带完整信息 &ndash; 局部变量签名 _token_ ，maxstack，代码大小，标志。tiny 方法头可以开始于任何字节边界。fat 方法头应开始于 4 字节边界。

#### 23.4.1. 方法头类型值

方法头的第一个字节的最低有效位表示存在哪种类型的头。这两位将且只能是以下的一种：

 | 值                       | 值  | 描述                                                      |
 | ------------------------ | --- | --------------------------------------------------------- |
 | `CorILMethod_TinyFormat` | 0x2 | 方法头是小型的 (§[II.25.4.2](ii.25.4.2-tiny-format.md))。 |
 | `CorILMethod_FatFormat`  | 0x3 | 方法头是大型的 (§[II.25.4.3](ii.25.4.3-fat-format.md))。  |

#### 23.4.2. Tiny 格式

Tiny 头使用 6 位长度编码。以下是所有 tiny 头的特性：

 * 不允许有局部变量

 * 没有异常

 * 没有额外的数据部分

 * 操作数栈的大小不得超过 8 个条目

Tiny 格式头的编码如下：

 | 起始位 | 位数 | 描述                                                                                               |
 | ------ | ---- | -------------------------------------------------------------------------------------------------- |
 | 0      | 2    | 标志 (应设置 `CorILMethod_TinyFormat`，参见 §[II.25.4.4](ii.25.4.4-flags-for-method-headers.md))。 |
 | 2      | 6    | 紧随此头之后的方法体的字节大小。                                                                   |

#### 23.4.3. Fat格式

当Tiny格式不足以满足需求时，就会使用Fat格式。这可能是由于以下一个或多个原因：

 * 方法过大，无法编码大小 (即，至少64字节)

 * 存在异常

 * 存在额外的数据段

 * 存在局部变量

 * 操作数栈需要超过8个条目

Fat头部具有以下结构

 | 偏移量  | 大小    | 字段               | 描述                                                                                                   |
 | ------- | ------- | ------------------ | ------------------------------------------------------------------------------------------------------ |
 | 0       | 12 (位) | **Flags**          | 标志 (`CorILMethod_FatFormat`应在位0:1中设置，参见§[II.25.4.4](ii.25.4.4-flags-for-method-headers.md)) |
 | 12 (位) | 4 (位)  | **Size**           | 以占用的4字节整数的计数表示此头部的大小 (当前为3)                                                      |
 | 2       | 2       | **MaxStack**       | 操作数栈上的最大项数                                                                                   |
 | 4       | 4       | **CodeSize**       | 实际方法体的字节大小                                                                                   |
 | 8       | 4       | **LocalVarSigTok** | 描述方法的局部变量布局的签名的元数据 _token_ 。0表示没有局部变量存在                                   |

#### 23.4.4. 方法头的标志
<a id="method-header"></a>

方法头的第一个字节也可以包含以下仅对Fat格式有效的标志，这些标志指示如何执行该方法：

 | 标志                     | 值   | 描述                                                                      |
 | ------------------------ | ---- | ------------------------------------------------------------------------- |
 | `CorILMethod_FatFormat`  | 0x3  | 方法头是fat。                                                             |
 | `CorILMethod_TinyFormat` | 0x2  | 方法头是tiny。                                                            |
 | `CorILMethod_MoreSects`  | 0x8  | 在此头后面有更多的部分 (§[II.25.4.5](ii.25.4.5-method-data-section.md))。 |
 | `CorILMethod_InitLocals` | 0x10 | 在所有局部变量上调用默认构造函数。                                        |

#### 23.4.5. 方法数据部分

在方法体之后的下一个 4 字节边界处可以有额外的方法数据部分。这些方法数据部分以两字节头开始 (1 字节用于标志，1 字节用于实际数据的长度) 或 4 字节头 (1 字节用于标志，3 字节用于实际数据的长度)。第一个字节确定头的种类，以及实际部分中的数据是什么：

 | 标志                          | 值   | 描述                                                                                                      |
 | ----------------------------- | ---- | --------------------------------------------------------------------------------------------------------- |
 | `CorILMethod_Sect_EHTable`    | 0x1  | 异常处理数据。                                                                                            |
 | `CorILMethod_Sect_OptILTable` | 0x2  | 保留，应为 0。                                                                                            |
 | `CorILMethod_Sect_FatFormat`  | 0x40 | 数据格式是 fat 类型，意味着有一个 3 字节长度的最低有效字节优先格式。如果未设置，头部是小的，长度为 1 字节 |
 | `CorILMethod_Sect_MoreSects`  | 0x80 | 在此当前部分之后还有另一个数据部分                                                                        |

目前，方法数据部分仅用于异常表 (参见 §[II.19](ii.19-exception-handling.md))。小异常头结构的布局如下：

 | 偏移量 | 大小 | 字段         | 描述                                                                            |
 | ------ | ---- | ------------ | ------------------------------------------------------------------------------- |
 | 0      | 1    | **Kind**     | 如上所述的标志。                                                                |
 | 1      | 1    | **DataSize** | 块的数据大小，包括头部，比如说 *n*\*12+4。                                      |
 | 2      | 2    | **Reserved** | 填充，始终为 0。                                                                |
 | 4      | *n*  | **Clauses**  | *n* 个小异常子句 (参见 §[II.25.4.6](ii.25.4.6-exception-handling-clauses.md))。 |

fat 异常头结构的布局如下：

 | 偏移量 | 大小 | 字段         | 描述                                                                               |
 | ------ | ---- | ------------ | ---------------------------------------------------------------------------------- |
 | 0      | 1    | **Kind**     | 使用的是哪种类型的异常块                                                           |
 | 1      | 3    | **DataSize** | 块的数据大小，包括头部，比如说 *n*\*24+4。                                         |
 | 4      | *n*  | **Clauses**  | *n* 个 fat 异常子句 (参见 §[II.25.4.6](ii.25.4.6-exception-handling-clauses.md))。 |

#### 23.4.6. 异常处理条款

异常处理条款也有小型和大型两种版本。

当try块和处理程序代码的大小都小于256字节，且它们的偏移量都小于65536时，应使用小型异常条款。小型异常条款的格式如下：

 | 偏移量 | 大小 | 字段              | 描述                                       |
 | ------ | ---- | ----------------- | ------------------------------------------ |
 | 0      | 2    | **Flags**         | 标志，见下文。                             |
 | 2      | 2    | **TryOffset**     | 从方法体开始的try块的字节偏移量。          |
 | 4      | 1    | **TryLength**     | try块的字节长度                            |
 | 5      | 2    | **HandlerOffset** | 此try块的处理程序的位置                    |
 | 7      | 1    | **HandlerLength** | 处理程序代码的字节大小                     |
 | 8      | 4    | **ClassToken**    | 基于类型的异常处理程序的元数据 _token_     |
 | 8      | 4    | **FilterOffset**  | 基于过滤器的异常处理程序在方法体中的偏移量 |

大型异常处理条款的布局如下：

 | 偏移量 | 大小 | 字段              | 描述                                       |
 | ------ | ---- | ----------------- | ------------------------------------------ |
 | 0      | 4    | **Flags**         | 标志，见下文。                             |
 | 4      | 4    | **TryOffset**     | 从方法体开始的try块的字节偏移量。          |
 | 8      | 4    | **TryLength**     | try块的字节长度                            |
 | 12     | 4    | **HandlerOffset** | 此try块的处理程序的位置                    |
 | 16     | 4    | **HandlerLength** | 处理程序代码的字节大小                     |
 | 20     | 4    | **ClassToken**    | 基于类型的异常处理程序的元数据 _token_     |
 | 20     | 4    | **FilterOffset**  | 基于过滤器的异常处理程序在方法体中的偏移量 |

每个异常处理条款使用以下标志值：

 | 标志                               | 值     | 描述                                |
 | ---------------------------------- | ------ | ----------------------------------- |
 | `COR_ILEXCEPTION_CLAUSE_EXCEPTION` | 0x0000 | 类型化的异常条款                    |
 | `COR_ILEXCEPTION_CLAUSE_FILTER`    | 0x0001 | 异常过滤器和处理程序条款            |
 | `COR_ILEXCEPTION_CLAUSE_FINALLY`   | 0x0002 | 最终条款                            |
 | `COR_ILEXCEPTION_CLAUSE_FAULT`     | 0x0004 | 错误条款 (只在异常时调用的最终条款) |


---