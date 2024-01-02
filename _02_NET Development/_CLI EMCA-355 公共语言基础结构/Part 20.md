

##  20. 元数据逻辑格式：tables

此节定义了描述元数据的结构，以及它们如何交叉索引。这对应于元数据如何在从 PE 文件读入内存后的布局方式。有关 PE 文件本身内部元数据布局的描述，参见 [_元数据物理布局_](#metadata-physical-layout)。

元数据存储在两种结构中：表 (**_Tables_**，记录数组) 和堆 (**_Heaps_**)。任何模块中都有四个堆：***String Heap***，***Blob Heap***，***Userstring Heap*** 和 ***Guid Heap***。前三个是字节数组 (这些堆的有效索引值可能是 0，23，25，39 等)。***Guid Heap*** 是 ***GUID*** 的数组，每个 ***GUID*** 宽 16 字节。它的第一个元素编号为 1，第二个为 2，依此类推。

每个表的每个列的每个条目要么是常数，要么是索引。

常数要么是字面值 (例如，ALG_SID_SHA1 = 4，存储在 **`Assembly`** 表的 *HashAlgId* 列中)，要么是更常见的位掩码。大多数位掩码 (它们几乎都被称为 ***Flags***) 宽 2 字节 (例如，**`Field`** 表中的 ***`Flags`*** 列)，但有几个是 4 字节 (例如，**`TypeDef`** 表中的 ***`Flags`*** 列)。

每个索引值宽度为 2 或 4 字节。索引值指向同一表或另一个表，或者指向四个堆中的一个。只有当用于特定模块时，表中的每个索引值列的大小才会变为 4 字节。因此，如果一个特定列索引的表或表的最高行号适合于 2 字节值，则索引值列只需要宽 2 字节。相反，对于包含 64K 或更多行的表，该表的索引值将宽 4 字节。

表的索引从 1 开始，因此索引 1 表示任何给定元数据表的第一行。索引值为零表示它根本不索引行；也就是说，它的行为就像一个空引用。

索引元数据表的列有两种。有关这些表的物理表示的详细信息，请参见 [[_#~ stream_]](#physical-stream) ：

 * Sample &mdash; 这样的列索引一个表 (唯一)。例如，**`TypeDef`** 表中的 ***`FieldList`*** 列始终索引 **`Field`** 表。因此，该列中的所有值都是简单的整数，这些值给出了目标表中的行号。

 * Coded &mdash; 这样的列可以索引几个表中的任何一个。例如，**`TypeDef`** 表中的 ***`Extends`*** 列可以索引到 **`TypeDef`** 或 **`TypeRef`** 表。该索引值的几位被保留用于定义它的目标表。在大多数情况下，此规范讨论的是索引值在目标表中解码为行号之后的值。规范在描述元数据物理布局的部分中包含了这些编码索引值的描述 [[↗]](#(#metadata-physical-layout))。

元数据保留编译器或代码生成器创建的名称字符串，不做任何更改。本质上，它将每个字符串视为不透明的二进制对象 (***blob***)。特别是，它保留了大小写。CLI 对存储在元数据中并随后由 CLI 处理的名称的长度没有限制。

匹配 **`AssemblyRef`** 和 **`ModuleRef`** 到它们对应的 *Assembly* 和 *Module* 应该是不区分大小写的。然而，对于所有其他名称的匹配 (类型，字段，方法，属性，事件) 应该是精确的 &mdash; 所有平台上这个级别的解析都是相同的，无论它们的操作系统或平台是否区分大小写。

表都有一个名称 (例如，“**`Assembly`**”) 和一个数字 (例如，0x20)。每个表的编号都列在以下子小节的标题中。表编号表示它们对应的表在 PE 文件中出现的顺序，并且还有一组位序列 [[↗]](#physical-stream) 用来表示给定的表是否存在。表的编号则是在这组位序列中的位置。

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

此外，一些规则用于检查与 CLS 要求的兼容性，即使这些规则与有效元数据无关。这些规则以 [CLS] 标签结尾。

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

| Column             | Alias                    | Index?  | Size  | Literal or<br>BitMasks | Description                              | Link |
| :----------------- | :----------------------- | :-----: | :---: | :--------------------: | :--------------------------------------- | :--- |
| ***`Token`***      |                          | &cross; |   4   |        00000UUU        | 行编号，高两位表示表编号，低三位是行编号 |      |
| ***`Generation`*** |                          | &cross; |   2   |           0            | 保留，应为 0                             |      |
| ***`Name`***       |                          | &check; |   4   |  To ***String Heap***  | 模块的名称                               |      |
| ***`Mvid`***       |                          | &check; |   4   |   To ***Guid Heap***   | 用于区分同一模块的两个版本的 ***GUID***  |      |
| ***`EncId`***      | ***`GenerationId`***     | &check; |   4   |   To ***Guid Heap***   | 保留，应为 0                             |      |
| ***`EncBaseId`***  | ***`BaseGenerationId`*** | &check; |   4   |   To ***Guid Heap***   | 保留，应为 0                             |      |

***`Mvid`*** 列应该索引 ***Guid Heap*** 中的一个唯一 ***GUID*** ([[↗]](#guid-heap))，该 ***GUID*** 标识该模块的实例。符合 CLI 的实现可以在读取时忽略 ***`Mvid`***。应该为每个模块新生成一个 ***`Mvid`*** (使用 ISO/IEC 11578:1996 (附录 A) 或其他兼容算法指定的算法)。

术语 ***GUID*** 表示全局唯一标识符，通常使用其十六进制编码显示的 16 字节长的数字。可以通过几种众所周知的算法生成 ***GUID***，包括在 ***RPC*** 和 ***CORBA*** 中用于 ***UUID*** (通用唯一标识符) 的算法，以及在 ***COM*** 中用于 ***CLSID***、***GUID*** 和 ***IID*** 的算法。

虽然 VES 本身不使用 ***`Mvid`***，但其他工具 (如调试器，这超出了本标准的范围) 依赖于 ***`Mvid`*** 来识别模块之间的不同。

可以将 ***`Generation`***、***`EncId`*** 和 ***`EncBaseId`*** 列写为零，并且可以由符合 CLI 的实现忽略。

**`Module`** 表中的行是程序集中的 **.module** 指令的结果 ([[↗]](#module))。

> 元数据验证规则

| Order | Validation Rule                                                                                                                               | Level |
| :---: | :-------------------------------------------------------------------------------------------------------------------------------------------- | :---: |
|   1   | **`Module`** 表应该包含一行且只有一行。                                                                                                       | ERROR |
|   2   | ***`Name`*** 应该索引 ***String Heap*** 中的 non-empty 字符串。此字符串应该与解析到此模块的任何相应 **`ModuleRef`**._`Name`_ 字符串完全匹配。 | ERROR |
|   3   | ***`Mvid`*** 应该索引 ***Guid Heap*** 中的非空 ***GUID***。                                                                                   | ERROR |

>---

### 20.38. TypeRef: 0x01
<a id="TypeRef_0x01"></a>

| Column                  | Alias             | Index?  | Size  | Literal or<br>BitMasks | Description                                                                     | Link                    |
| :---------------------- | :---------------- | :-----: | :---: | :--------------------: | :------------------------------------------------------------------------------ | :---------------------- |
| ***`Token`***           |                   | &cross; |   4   |        01000UUU        | 行编号，高两位表示表编号，低三位是行编号                                        |                         |
| ***`ResolutionScope`*** |                   | &check; |       |    To Other Tables     | 索引 **`Module`**，**`ModuleRef`**，**`AssemblyRef`** 或 **`TypeRef`** 表，或空 | [[↗]](#physical-stream) |
| ***`TypeName`***        | ***`Name`***      | &check; |       |  To ***String Heap***  | 表示引用的类型的名称标识                                                        |                         |
| ***`TypeNamespace`***   | ***`Namespace`*** | &check; |       |  To ***String Heap***  | 表示引用的类型的所属名称空间                                                    |                         |

> 元数据验证规则
 1. _ResolutionScope_ 应该严格是以下之一：
    1. null —— 在这种情况下，_ExportedType_ 表中应该有一行对应这个类型 —— 它的 _Implementation_ 字段应该包含一个 _File_ **_token_** 或一个 **`AssemblyRef`** **_token_**，以说明类型是在哪里定义的。[ERROR]
    2. 一个 **`TypeRef`** **_token_**，如果它是一个嵌套类型 (例如，可以通过检查它的 _TypeDef_ 表中的 _Flags_ 列来确定 —— 可访问性子字段是 `tdNestedXXX` 集合中的一个)。[ERROR]
    3. 一个 **`ModuleRef`** **_token_**，目标类型在与当前模块相同的程序集中的另一个模块中定义。[ERROR]
    4. 一个 **`Module`** **_token_**，目标类型在当前模块中定义 —— 这在 CLI (“压缩元数据”) 模块中不应该出现。[WARNING]
    5. 一个 **`AssemblyRef`** **_token_**，目标类型在与当前模块不同的程序集中定义。[ERROR]

 2. _TypeName_ 应该在 ***String*** 堆中索引一个 non-empty 字符串。[ERROR]
 3. _TypeNamespace_ 可以为 null，或 non-null。
 4. 如果非空，_TypeNamespace_ 应该在 ***String*** 堆中索引一个 non-empty 字符串。[ERROR]
 5. _TypeName_ 字符串应该是一个有效的 CLS 标识符。[CLS]
 6. 不应该有重复的行，重复的行具有相同的 _ResolutionScope_，_TypeName_ 和 _TypeNamespace_。[ERROR]
 7. 使用 CLS 冲突标识符规则比较 _TypeName_ 和 _TypeNamespace_ 的字段时，不应出现重复的行。[CLS]

>---
### 20.37. TypeDef: 0x02
<a id="TypeDef_0x02"></a>

| *Token* | *Flags* | *TypeName* | *TypeNamespace* | *Extends* | _FieldList_ | _MethodList_ |
| :------ | :------ | :--------- | :-------------- | :-------- | :---------- | :----------- |

> _TypeDef_ 表有以下列：
 * **_Flags_**：或 *Attributes*，一个 4 字节的 _TypeAttributes_ 类型的位掩码，参见 [*TypeAttributes*](#TypeAttributes)。 
 - **_TypeName_**：或 *Name*，一个指向 ***String Heap*** 的索引。
 * **_TypeNamespace_**：或 *Namespace*，一个指向 ***String Heap*** 的索引。
 - **_Extends_**：或 *BaseType*，一个指向 _TypeDef_，**`TypeRef`** 或 _TypeSpec_ 表的索引；更准确地说，是一个 _TypeDefOrRef_ (参见 [_元数据物理流_](#physical-stream) ) 编码索引。 
 * **_FieldList_**：一个指向 _Field_ 表的索引，它标记了由此类型拥有的一连串字段的第一个。该连续运行继续到以下较小的一个：
   * _Field_ 表的最后一行。
   * 通过检查此 _TypeDef_ 表中下一行的 _FieldList_ 找到的下一组字段。

 - **_MethodList_**：一个指向 _MethodDef_ 表的索引，它标记了由此类型拥有的一连串方法的第一个。该连续运行继续到以下较小的一个：
   * _MethodDef_ 表的最后一行。
   * 通过检查此 _TypeDef_ 表中下一行的 _MethodList_ 找到的下一组方法。

   _TypeDef_ 表的第一行代表伪类，该伪类 ("`<Module>`") 作为在模块范围内定义的函数和变量的父类。

任何类型都应该是以下之一，并且只能是以下之一：
 * **Class**：_Flags_._Interface_ = 0，并最终派生自 `System.Object`。 
 * **Interface**：_Flags_._Interface_ = 1。 
 * **Value type**：最终派生自 `System.ValueType`。

对于任何给定的类型，都有两个独立且不同的指向其他类型的指针链 (这些指针实际上是作为元数据表索引实现的)。这两个链分别是：
 * Extension chain：扩展链，通过 _TypeDef_ 表的 _Extends_ 列定义。通常，一个派生类继承扩展一个基类 (始终是一个，有且只有一个基类)。 
 * Interface chains：接口链，通过 _InterfaceImpl_ 表定义。通常，一个类可以实现零个、一个或多个接口。

这两个链 (扩展和接口) 在元数据中始终保持分离。_Extends_ 链表示一对一关系，即一个类扩展 (或 “派生自”) 另一个类 (称为其直接基类)。_Interface_ 链可以表示一对多关系，即一个类可能实现两个或更多接口。

接口也可以实现一个或多个其他接口 —— 元数据通过 _InterfaceImpl_ 表存储这些链接 (这里的术语有些不适当，因为接口没有涉及 “Implementation”；也许更清晰的名称可能是 _Interface_ 表，或 _InterfaceInherit_ 表) 

另一种稍微专门化的类型是嵌套类型，它在 ILAsm 中被声明为在封闭类型声明中的词法嵌套。类型是否嵌套可以通过其 _Flags_._`Visibility`_ 子字段的值确定 —— 它应该是 {_NestedPublic_, _NestedPrivate_, _NestedFamily_, _NestedAssembly_, _NestedFamANDAssem_, _NestedFamORAssem_} 集合中的一个。

如果类型是泛型，其参数在 _GenericParam_ 表中定义 (参见 [[↗]](#GenericParam_0x2A))。_GenericParam_ 表中的条目引用 _TypeDef_ 表中的条目；_TypeDef_ 表没有引用 _GenericParam_ 表。

继承层次结构的根看起来像这样：

![继承层次结构的根](./.img/继承层次结构的根.png)

有一个系统定义的根，`System.Object`。所有的类和值类型最终都应该从 `System.Object` 派生；类可以从其他类派生 (通过一个单一的，非循环的链) 到任何需要的深度。这个 _Extends_ 继承链用实箭头表示。

接口不能相互继承；但是，它们可以有零个或多个必需的接口，这些接口应该被实现。_Interface_ 需求链显示为虚箭头。它包含了接口和类或值类型之间的链接 —— 后者被称为实现该接口或多个接口。

常规值类型 (即，排除枚举 — 见后文) 被定义为直接从 `System.ValueType` 派生。常规值类型不能派生到一个以上的深度。另一种表述方式是，用户定义的值类型应该是密封的。用户定义的枚举应该直接从 `System.Enum` 派生。枚举不能在 `System.Enum` 以下派生到一个以上的深度。另一种表述方式是，用户定义的枚举应该是密封的。`System.Enum` 直接从 `System.ValueType` 派生。

用户定义的委托从 `System.Delegate` 派生。委托不能派生到一个以上的深度。

关于声明类型的指令，请参见 [_泛型定义_](#generic-type) 和 [_类型定义_](#class)。

> 元数据验证规则

 1. _TypeDef_ 表可以包含一个或多个行。
 2. _Flags:_
     1. _Flags_ 只能设置那些指定的值。[ERROR]
     2. 可以设置 0 或 1 个 `SequentialLayout` 和 `ExplicitLayout`；如果没有设置，则默认为 `AutoLayout`。[ERROR]
     3. 可以设置 0 或 1 个 `UnicodeClass` 和 `AutoClass`；如果没有设置，则默认为 `AnsiClass`。[ERROR]
     4. 如果 _Flags_._`HasSecurity`_ = 1，那么以下条件中至少有一个应该为真：。[ERROR]
        * 这个类型在 _DeclSecurity_ 表中拥有至少一行。
        * 这个类型有一个名为 `SuppressUnmanagedCodeSecurityAttribute` 的自定义特性。
     5. 如果这个类型在 _DeclSecurity_ 表中拥有一行 (或多行)，那么 _Flags_.*`HasSecurity`* 应该是 1。[ERROR]
     6. 如果这个类型有一个名为 `SuppressUnmanagedCodeSecurityAttribute` 的自定义特性，那么 _Flags_.*`HasSecurity`* 应该是 1。[ERROR]
     7. 注意，接口设置 *`HasSecurity`* 是有效的。然而，安全系统忽略任何附加到该接口的权限请求。

 3. ***`Name`*** 应该在 ***String Heap*** 中索引一个 non-empty 字符串。[ERROR]
 4. _TypeName_ 字符串应该是一个有效的 CLS 标识符。[CLS]
 5. _TypeNamespace_ 可以为空或非空。
 6. 如果非空，那么 _TypeNamespace_ 应该在 ***String Heap*** 中索引一个 non-empty 字符串。[ERROR]
 7. 如果非空，_TypeNamespace_ 的字符串应该是一个有效的 CLS 标识符。[CLS]
 8. 每个类 (除了 `System.Object` 和特殊类 `<Module>`) 都应该继承一个 (有且只有一个) 其他类 —— 所以对于一个类，_Extends_ 应该是非空的。[ERROR]
 9. `System.Object` 应该有一个 _Extends_ 值为 null。[ERROR]
 10. `System.ValueType` 应该有一个 _Extends_ 值为 `System.Object`。[ERROR]
 11. 除了 `System.Object` 和特殊类 `<Module>`，对于任何类，_Extends_ 应该索引在 _TypeDef_，**`TypeRef`**，或 _TypeSpec_ 表中的一个有效行，其中有效意味着 1 ≤ row ≤ rowcount。此外，该行本身必须是一个类 (而不是接口或值类型)，该基类不应该被密封 (其 _Flags_.*`Sealed`* 应该是 0)。[ERROR]
 12. 一个类不能扩展自身或其子类 (即，它的派生类)，因为这将在层次树中引入循环。[ERROR] (对于泛型类型，参见 [_泛型定义_](#generic-define) 和 [_泛型和递归继承图_](#generic-inherit)) 
 13. 一个接口永远不会扩展另一个类型 —— 所以 _Extends_ 应该为空；接口确实实现了其他接口，但是这种关系是通过 _InterfaceImpl_ 表捕获的，而不是 _Extends_ 列。[ERROR]
 14. _FieldList_ 可以为空或非空。
 15. 一个类或接口可以拥有零个或多个字段。
 16. 一个值类型应该有一个非零的大小 —— 通过定义至少一个字段，或者提供一个非零的 _ClassSize_。[ERROR]
 17. 如果 _FieldList_ 是非空的，它应该索引 _Field_ 表中的一个有效行，其中有效意味着 1 ≤ row ≤ rowcount+1。[ERROR]
 18. _MethodList_ 可以为空或非空。
 19. 一个类型可以拥有零个或多个方法。
 20. 值类型的运行时大小不应超过 1 MByte (0x100000 字节)。[ERROR]
 21. 如果 _MethodList_ 是非空的，它应该索引 _MethodDef_ 表中的一个有效行，其中有效意味着 1 ≤ row ≤ rowcount+1。[ERROR]
 22. 一个类如果有一个或多个抽象方法不能被实例化，那么它必须具有 _Flags_.*`Abstract`* = 1。注意类拥有的方法包括从其基类继承以及它实现的接口的所有方法，以及通过其 _MethodList_ 定义的方法。CLI 将在运行时分析类定义；如果它发现一个类有一个或多个抽象方法，但是 _Flags_.*`Abstract`* = 0，它将抛出一个异常。[ERROR]
 23. 一个接口应该有 _Flags_.*`Abstract`* = 1。[ERROR]
 24. 对于一个抽象类型来说，有一个构造方法 (即，一个名为 `.ctor` 的方法) 是有效的。
 25. 任何非抽象类型 (即 _Flags_.*`Abstract`* = 0) 应该为其抽象类或接口协议要求的每个方法提供一个实现 (提供方法主体)。它的方法可以从其基类继承，从它实现的接口继承，或者由它自己定义。实现可以从其基类继承，或者由它自己定义。[ERROR]
 26. 一个接口 (_Flags_.*Interface* = 1) 可以拥有静态字段 (_Field_.*Static* = 1) 但不能拥有实例字段 (_Field_.*Static* = 0)。[ERROR]
 27. 一个接口不能被密封 (如果 _Flags_.*`Interface`* = 1，那么 _Flags_.*Sealed* 应该是 0)。[ERROR]
 28. 一个接口拥有的所有方法 (_Flags_.*`Interface`* = 1) 应该是抽象的 (_Flags_.*Abstract* = 1)。[ERROR]
 29. 在 _TypeDef_ 表中，基于 _TypeNamespace_+_TypeName_ 不应该有重复的行 (除非这是一个嵌套类型 — 见下文)。[ERROR]
 30. 如果这是一个嵌套类型，那么在 _TypeDef_ 表中，基于 _TypeNamespace_+_TypeName_+_OwnerRowInNestedClassTable_ 不应该有重复的行。[ERROR]
 31. 使用 CLS 冲突标识符规则进行比较时，基于 _TypeNamespace_+_TypeName_ 的字段，不应该有重复的行 (除非这是一个嵌套类型 — 见下文)。[CLS]
 32. 使用 CLS 冲突标识符规则进行比较时，如果是一个嵌套类型，基于 _TypeNamespace_+_TypeName_+_OwnerRowInNestedClassTable_ 和 _TypeNamespace_+_TypeName_ 的字段，不应该有重复的行。[CLS]
 33. 如果 _Extends_ = `System.Enum` (即，类型是用户定义的枚举) 那么：
      1. 应该是封闭的 (*`Sealed`* = 1)。[ERROR]
      2. 不应该有自己的任何方法 (_MethodList_ 链应该是零长度)。[ERROR]
      3. 不应该实现任何接口 (此类型在 _InterfaceImpl_ 表中没有条目)。[ERROR]
      4. 不应该有任何属性。[ERROR]
      5. 不应该有任何事件。[ERROR]
      6. 任何静态字段应该是字面值 (具有 _Flags_.*`Literal`* = 1)。[ERROR]
      7. 应该有一个或多个 **static literal** 字段，每个字段都具有枚举的类型。[CLS]
      8. 应该有一个实例字段，为内置整数类型。[ERROR]
      9. 实例字段的 ***`Name`*** 字符串应该是 "`value__`"，该字段应该被标记为 `RTSpecialName`，并且该字段应该具有 CLS 整数类型之一。[CLS]
      10. 除非它们是文字的，否则不应该有任何静态字段。[ERROR]
 34. 嵌套类型 (如上所定义) 应该只拥有 _NestedClass_ 表中的一行，其中 “拥有” 意味着在 _NestedClass_ 表中的一行，其 _NestedClass_ 列包含此类型定义的    _TypeDef_  **_token_**。[ERROR]
 35. 值类型应该是封闭的。[ERROR]


>---
### 20.15. Field: 0x04
<a id="Field_0x04"></a>

| *Token* | *Flags* | *Name* | *Signature* |
| :------ | :------ | :----- | :---------- |

> _Field_ 表有以下列：

 * **_Flags_**：或 *Attributes*，一个 2 字节的位掩码，类型为 _FieldAttributes_，参见 [_FieldAttributes_](#FieldAttributes)。
 - *****`Name`*****：一个索引，指向 ***String Heap***。
 * **_Signature_**：一个索引，指向 ***Blob Heap***。 

从概念上讲，_Field_ 表中的每一行都由 _TypeDef_ 表中的一行 (有且只有一行) 拥有。然而，_Field_ 表中任何一行的 *owner* 并不存储在 _Field_ 表本身中。在 _TypeDef_ 表的每一行中只有一个前向指针 “*forward-pointer*” (_FieldList_ 列)，如下图所示。

 ![](./.img/field-figure.png)

_TypeDef_ 表有 1 ~ 4 行。_TypeDef_ 表的第一行对应于 CLI 自动插入的伪类型。它用于表示 _Field_ 表中对应于全局变量的那些行。_Field_ 表有 1 ~ 6 行：
 - 类型 1 (`<module>` 的伪类型) 拥有 _Field_ 表中的 1 和 2 行。
 - 类型 2 在 _Field_ 表中没有任何行，尽管其 _FieldList_ 索引了 _Field_ 表中的第 3 行。
 - 类型 3 拥有 _Field_ 表中的 3 ~ 5 行。
 - 类型 4 拥有 _Field_ 表中的第 6 行。

因此，在 _Field_ 表中，行 1 和 2 属于类型 1 (全局变量) ；行 3 ~ 5 属于类型 3；行 6 属于类型 4。

_Field_ 表中的每一行都是由顶级 **.field** 指令 [[↗]](#il-top-impl) 或类型内的 **.field** 指令 [[↗]](#field) 产生的。

> 元数据验证规则

 1. _Field_ 表可以包含零行或多行。
 2. 在 _TypeDef_ 表中每一行应有一个 _FieldList_，且只有一个。[ERROR]
 3. _TypeDef_ 表中的 *owner* 行不能是接口。[CLS]
 4. _Flags_ 只应设置那些指定的值。[ERROR]
 5. _Flags_ 的 *`FieldAccessMask`* 子字段应精确地包含 *`CompilerControlled`*、*`Private`*、*`FamANDAssem`*、*`Assembly`*、*`Family`*、*`FamORAssem`* 或 *`Public`* 中的一个 [[↗]](#FieldAttributes)。[ERROR]
 6. _Flags_ 可以设置 *`Literal`* 或 *`InitOnly`* 中的一个或两者都不设置，但不能同时设置两者 [[↗]](#FieldAttributes)。[ERROR]
 7. 如果 _Flags_.*`Literal`* = 1，那么 _Flags_.*`Static`* 也应为 1 [[↗]](#FieldAttributes)。[ERROR]
 8. 如果 _Flags_.*`RTSpecialName`* = 1，那么 _Flags_.*`SpecialName`* 也应为 1 [[↗]](#FieldAttributes)。[ERROR]
 9. 如果 _Flags_.*`HasFieldMarshal`* = 1，那么此行应 “拥有” _FieldMarshal_ 表中的确切一行 [[↗]](#FieldAttributes)。[ERROR]
 10. 如果 _Flags_.*`HasDefault`* = 1，那么此行应 “拥有” _Constant_ 表中的确切一行 [[↗]](#FieldAttributes)。[ERROR]
 11. 如果 _Flags_.*`HasFieldRVA`* = 1，那么此行应 “拥有” _Field's RVA_ 表中的确切一行 [[↗]](#FieldAttributes)。[ERROR]
 12. ***`Name`*** 应索引 ***String Heap*** 中的 non-empty 字符串。[ERROR]
 13. ***`Name`*** 字符串应是一个有效的 CLS 标识符。[CLS]
 14. _Signature_ 应索引 ***Blob Heap*** 中的有效字段签名。[ERROR]
 15. 如果 _Flags_.*`CompilerControlled`* = 1 [[↗]](#FieldAttributes)，那么在重复检查中将完全忽略此行。
 16. 如果此字段的 *owner* 是内部生成的类型 `<Module>`，它表示此字段在模块范围内定义 (通常称为全局变量)。在这种情况下：
     * _Flags_.*`Static`* 应为 1。[ERROR] 
     * _Flags_.*`MemberAccessMask`* 子字段应为 *`Public`*、*`CompilerControlled`* 或 *`Private`* 中的一个 [[↗]](#FieldAttributes)。[ERROR]
     * 不允许使用模块范围 (_module scope_) 字段。[CLS]
 17. 基于 _owner_+***`Name`***+_Signature_，_Field_ 表中不应有重复的行，其中 _owner_ 是在 _TypeDef_ 表中的 _owner_ 行。但如果 _Flags_.*`CompilerControlled`* = 1，那么完全排除此行的重复检查。[ERROR]
 18. 基于 _owner_+***`Name`***，_Field_ 表中不应有重复的行，其中 ***`Name`*** 字段使用 CLS 冲突标识符规则进行比较。所以例如，"`int i`" 和 "`float i`" 将被视为 CLS 重复。但如果 _Flags_.*`CompilerControlled`* = 1，那么完全排除此行在重复检查。[CLS]
 19. 如果这是一个枚举的字段，那么： 
     * _TypeDef_ 表中的 *owner* 行应直接派生自 `System.Enum`。[ERROR]
     * _TypeDef_ 表中的 *owner* 行不应有其他实例字段。[CLS]
     * 其 _Signature_ 应为 *`ELEMENT_TYPE_U1`*、*`ELEMENT_TYPE_I2`*、*`ELEMENT_TYPE_I4`* 或 *`ELEMENT_TYPE_I8`* 中的一个，参见 [_ELEMENT_TYPE_](#ELEMENT_TYPE)。[CLS]
 20. 其 _Signature_ 应为整型。[ERROR]

>---
### 20.26. MethodDef: 0x06
<a id="MethodDef_0x06"></a>

| *Token* | *Flags* | _ImplFlags_ | _RVA_ | ***`Name`*** | *Signature* | _ParamList_ |
| :------ | :------ | :---------- | :---- | :----------- | :---------- | :---------- |

> _MethodDef_ 表有以下列：

 * **_RVA_**：一个 4 字节的常数。

 - **_ImplFlags_**：或 _ImplAttributes_，一个 2 字节的位掩码，类型为 _MethodImplAttributes_，参见 [[↗]](#MethodImplAttributes)。 

 * **_Flags_**：或 _Attributes_，一个 2 字节的位掩码，类型为 _MethodAttributes_，参见 [[↗]](#MethodAttributes)。

 - *****`Name`*****：一个指向 ***String Heap*** 的索引。

 * **_Signature_**：一个指向 ***Blob Heap*** 的索引。

 - **_ParamList_**：一个指向 Param 表的索引。它标记了由此方法拥有的一连串参数的第一个。该连续运行继续到以下较小的：
     * _Param_ 表的最后一行
     * 下一个参数运行，通过检查 _MethodDef_ 表中下一行的 _ParamList_ 找到。

从概念上讲，_MethodDef_ 表中的每一行都由 _TypeDef_ 表中的一行拥有 (有且只有一行)。

_MethodDef_ 表中的行是 **.method** 指令的结果 [[↗]](#method)。当发出 PE 文件的映像时，计算 RVA 列，并指向方法体的 `COR_ILMETHOD` 结构 [[↗]](#CIL-physical-layout)。 

如果 _Signature_ 是 *`GENERIC`* (0x10)，则在 _GenericParam_ 表 [[↗]](#GenericParam_0x2A) 中描述泛型参数。

> 元数据验证规则

 1. _MethodDef_ 表可以包含零行或多行
 2. 在 _TypeDef_ 表中的 *owner* 行，每一行应该有一个 _MethodList_，且只有一个。[ERROR]
 3. _ImplFlags_ 只应设置那些指定的值。[ERROR]
 4. _Flags_ 只应设置那些指定的值。[ERROR]
 5. 如果 ***`Name`*** 是 `.ctor` 并且方法被标记为 `SpecialName`，那么在 _GenericParam_ 表中不应该有一行将此 _MethodDef_ 作为其 *owner*。[ERROR]
 6. _Flags_ 的 *`MemberAccessMask`* 子字段 [[↗]](#MethodAttributes) 应该包含 *`CompilerControlled`*、*`Private`*、*`FamANDAssem`*、*`Assem`*、*`Family`*、*`FamORAssem`* 或 *`Public`* 中的一个。[ERROR]
 7. _Flags_ 中的以下组合位设置是无效的。[ERROR]
     1. *`Static`* | *`Final`*
     2. *`Static`* | *`Virtual`*，除了接口中的静态虚或抽象方法
     3. *`Static`* | *`NewSlot`*
     4. *`Final`* | *`Abstract`*
     5. *`Abstract`* | *`PinvokeImpl`*
     6. *`CompilerControlled`* | *`SpecialName`*
     7. *`CompilerControlled`* | *`RTSpecialName`*

 8. 抽象方法应该是虚方法。所以，如果 _Flags_.*`Abstract`* = 1 那么 _Flags_.*`Virtual`* 也应该是 1。[ERROR]
 9. 如果 _Flags_.*`RTSpecialName`* = 1 那么 _Flags_.*`SpecialName`* 也应该是 1。[ERROR]
 10. 如果 _Flags_.*`HasSecurity`* = 1，那么以下条件中至少有一个应该为真：[ERROR]
     * 此方法拥有 _DeclSecurity_ 表中的至少一行。
     * 此方法具有名为 `SuppressUnmanagedCodeSecurityAttribute` 的自定义特性。
 11. 如果此方法拥有 _DeclSecurity_ 表中的一行 (或多行) 那么 _Flags_.*`HasSecurity`* 应该是 1。[ERROR]
 12. 如果此方法具有名为 `SuppressUnmanagedCodeSecurityAttribute` 的自定义特性那么 _Flags_.*`HasSecurity`* 应该是 1。[ERROR]
 13. 可以具有名为 `DynamicSecurityMethodAttribute` 的自定义特性，但这对其 _Flags_.*`HasSecurity`* 的值没有任何影响。
 14. ***`Name`*** 应索引 ***String Heap*** 中的 non-empty 字符串。[ERROR]
 15. 接口不能有实例构造函数。所以，如果这个方法是由接口拥有的，那么它的 ***`Name`*** 不能是 `.ctor`。[ERROR]
 16. ***`Name`*** 字符串应是一个有效的 CLS 标识符 (除非设置了 _Flags_.*`RTSpecialName`* — 例如，`.cctor` 是有效的)。[CLS]
 17. _Signature_ 应索引 ***Blob Heap*** 中的有效方法签名。[ERROR]
 18. 如果 _Flags_.*`CompilerControlled`* = 1，那么在重复检查中完全忽略此行。
 19. 如果此方法的 *owner* 是内部生成的类型 `<Module>`，它表示此方法在模块范围内定义。在 C++ 中，该方法被称为全局方法，只能在其编译单元内从其声明点向前引用。在这种情况下：
     1. _Flags_.*`Static`* 应为 1。[ERROR]
     2. _Flags_.*`Abstract`* 应为 0。[ERROR]
     3. _Flags_.*`Virtual`* 应为 0。[ERROR]
     4. _Flags_.*`MemberAccessMask`* 子字段应为 *`CompilerControlled`*、*`Public`* 或 *`Private`* 中的一个。[ERROR]
     5. 不允许模块范围方法。[CLS]
 20. 对于没有身份 (_identity_) 的值类型，具有同步方法是没有意义的 (除非它们被装箱)。所以，如果此方法的 *owner* 是一个值类型，那么该方法不能是同步的，即 _ImplFlags_.*`Synchronized`* 应为 0。[ERROR]
 21. 在 _MethodDef_ 表中，基于 *owner*+***`Name`***+_Signature_，不应有重复的行 (其中 *owner* 是在 _TypeDef_ 表中的拥有行)。注意，_Signature_ 编码了方法是否是泛型，对于泛型方法，它编码了泛型参数的数量。如果 _Flags_.*`CompilerControlled`* = 1，那么在重复检查中完全忽略此行。[ERROR]
 22. 在 _MethodDef_ 表中，基于 *owner*+***`Name`***+_Signature_，不应有重复的行，其中 ***`Name`*** 字段使用 CLS 冲突标识符规则进行比较；此外，签名中定义的类型应该是不同的。所以例如，`int i` 和 `float i` 将被视为 CLS 重复；此外忽略了方法的返回类型。如果 _Flags_.*`CompilerControlled`* = 1，那么在重复检查中完全忽略此行。[CLS]
 23. 如果在 _Flags_ 中设置了 *`Final`*、*`NewSlot`* 或 *`Strict`*，那么也应设置 _Flags_.*`Virtual`*。[ERROR]
 24. 如果设置了 _Flags_.*`PInvokeImpl`*，那么 _Flags_.*`Virtual`* 应为 0。[ERROR]
 25. 如果 _Flags_.*`Abstract`* &ne; 1，那么以下条件中必须有一个也为真：[ERROR]
     * RVA &ne; 0
     * _Flags_.*`PInvokeImpl`* = 1
     * _ImplFlags_.*`Runtime`* = 1
 26. 如果方法是 *`CompilerControlled`*，那么 RVA 应为非零或标记为 *`PinvokeImpl`* = 1。[ERROR]
 27. _Signature_ 应具有以下托管调用约定中的一个。[ERROR]
     1. *`DEFAULT`* (0x0)
     2. *`VARARG`* (0x5)
     3. *`GENERIC`* (0x10)
 28. _Signature_ 应具有调用约定 *`DEFAULT`* (0x0) 或 *`GENERIC`* (0x10)。[CLS]
 29. _Signature_：当且仅当方法不是 *`Static`* 时，_Signature_ 中的调用约定字节的 *`HASTHIS`* (0x20) 位应被设置。[ERROR]
 30. _Signature_：如果方法是 *`Static`*，那么调用约定中的 *`HASTHIS`* (0x20) 位应为0 。[ERROR]
 31. 如果签名中的 *`EXPLICITTHIS`* (0x40) 被设置，那么 *`HASTHIS`* (0x20) 也应被设置 (注意，如果设置了 *`EXPLICITTHIS`*，那么代码是不可验证的)。[ERROR]
 32. *`EXPLICITTHIS`* (0x40) 位只能在函数指针的签名中设置，它的 *MethodDefSig* 前面有 *`FNPTR`* (0x1B) 的签名。[ERROR]
 33. 如果 _RVA_ = 0，那么以下条件之一必须为真：[ERROR]
     * _Flags_.*`Abstract`* = 1。
     * _ImplFlags_.*`Runtime`* = 1。
     * _Flags_.*`PinvokeImpl`* = 1。
 34. 如果 _RVA_ ≠ 0，那么：[ERROR]
     1. _Flags_.*`Abstract`* 应为 0，并且
     2. _ImplFlags_.*`CodeTypeMask`* 应具有以下值之一：*`Native`*，*`CIL`* 或 *`Runtime`*，并且
     3. _RVA_ 应指向此文件中的 CIL 代码流
 35. 如果 _Flags_.*`PinvokeImpl`* = 1 那么。[ERROR]
     * _RVA_ = 0 并且方法在 _ImplMap_ 表中拥有一行
 36. 如果 _Flags_.*`RTSpecialName`* = 1 那么 ***`Name`*** 应为以下之一：[ERROR]
     1. `.ctor` (一个对象构造器方法) 
     2. `.cctor` (一个类构造器方法) 
 37. 相反，如果 ***`Name`*** 是上述特殊名称中的任何一个，那么 _Flags_.*`RTSpecialName`* 应被设置。[ERROR]
 38. 如果 ***`Name`*** = `.ctor` (一个对象构造器方法) 那么：
     1. _Signature_ 中的返回类型应为 *`ELEMENT_TYPE_VOID`* (参见 [_ELEMENT_TYPE_](#ELEMENT_TYPE))。[ERROR]
     2. _Flags_.*`Static`* 应为 0。[ERROR]
     3. _Flags_.*`Abstract`* 应为 0。[ERROR]
     4. _Flags_.*`Virtual`* 应为 0。[ERROR]
     5. "*owner*" 类型应为 _TypeDef_ 表中的有效类或值类型 (不是 `<Module>` 且不是接口)。[ERROR]
     6. 对于任何给定的 "*owner*"，可以有零个或多个 `.ctor`。
 39. 如果 ***`Name`*** = `.cctor` (一个类构造器方法) 那么：
     1. _Signature_ 中的返回类型应为 *`ELEMENT_TYPE_VOID`* (参见 [_ELEMENT_TYPE_](#ELEMENT_TYPE))。[ERROR]
     2. _Signature_ 的调用约定应为 *`DEFAULT`* (0x0)。[ERROR]
     3. _Signature_ 中不应提供参数。[ERROR]
     4. _Flags_.*`Static`* 应被设置。[ERROR]
     5. _Flags_.*`Virtual`* 应被清除。[ERROR]
     6. _Flags_.*`Abstract`* 应被清除。[ERROR]
 40. 在 _TypeDef_ 表中的任何给定行拥有的方法集合中，只能有 0 或 1 个名为 `.cctor` 的方法。[ERROR]
 
>--- 
### 20.33. Param: 0x08
<a id="Param_0x08"></a>

| *Token* | *Flags* | _Sequence_ | ***`Name`*** |
| :------ | :------ | :--------- | :----------- |

> _Param_ 表有以下列：

 * **_Flags_**：或 _Attributes_，一个 2 字节的位掩码，类型为 _ParamAttributes_，参见 [[↗]](#ParamAttributes)。

 - **_Sequence_**：一个 2 字节的常数。

 * *****`Name`*****：一个指向 ***String Heap***的索引。
 
从概念上讲，_Param_ 表中的每一行都由 _MethodDef_ 表中的一行拥有，且只有一行。

_Param_ 表中的行是方法声明中的参数 [[↗]](#MethodHeader)，或者是附加到方法的 **.param** 特性 [[↗]](#param) 的结果。

> 元数据验证规则

 1. _Param_ 表可以包含零行或多行。
 2. 在 _MethodDef_ 表中的 *owner* 行，每一行应该有一个 _ParamList_，且只有一个。[ERROR]
 3. _Flags_ 只应设置那些指定的值 (所有组合有效)。[ERROR]
 4. _Sequence_ 应该有一个值 &ge; 0 并且 &le;  *owner* 方法中的参数数量。_Sequence_ 值为 0 指的是 *owner* 方法的返回类型；然后从 1 开始编号其参数。[ERROR]
 5. 由同一方法拥有的 _Param_ 表的连续行应该按照增加的 _Sequence_ 值排序 —— 尽管序列中允许有间隙。[WARNING]
 6. 如果 _Flags_.*`HasDefault`* = 1，那么此行应该在 _Constant_ 表中只拥有一行。[ERROR]
 7. 如果 _Flags_.*`HasDefault`* = 0，那么在 _Constant_ 表中没有属于这一行的行。[ERROR]
 8. 如果 _Flags_.*`FieldMarshal`* = 1，那么此行应该在 *FieldMarshal* 表中只拥有一行。[ERROR]
 9. ***`Name`*** 可以为 null 或 non-null。
 10. 如果 ***`Name`*** 是非空的，那么它应该索引 ***String Heap*** 中的 non-empty 字符串。[WARNING]

>---
### 20.23. InterfaceImpl: 0x09
<a id="InterfaceImpl_0x09"></a>

| *Token* | *Class* | Interface |
| :------ | :------ | :-------- |

> _InterfaceImpl_ 表有以下列：

 * **_Class_**：_TypeDef_ 表的索引。

 - **_Interface_**：_TypeDef_，**`TypeRef`** 或 _TypeSpec_ 表的索引；更准确地说，是 _TypeDefOrRef_ [[↗]](#physical-stream) 编码索引。
 
_InterfaceImpl_ 表记录类型显式实现的接口。从概念上讲，_InterfaceImpl_ 表中的每一行都表示 _Class_ 实现了 _Interface_。

> 元数据验证规则
 1. _InterfaceImpl_ 表可以包含零行或多行。
 2. _Class_ 应为非 null。[ERROR]
 3. 如果 _Class_ 为非 null，则：
     1. _Class_ 应索引 _TypeDef_ 表中的有效行。[ERROR]
     2. _Interface_ 应索引 _TypeDef_ 或 **`TypeRef`** 表中的有效行。[ERROR]
     3. _Interface_ 索引的 _TypeDef_，**`TypeRef`** 或 _TypeSpec_ 表中的行应为接口 (_Flags_.*`Interface`* = 1)，而不是类或值类型。[ERROR]
 4. 基于非 null 的 _Class_ 和 _Interface_ 值，在 _InterfaceImpl_ 表中不应有重复项。[WARNING]
 5. 可以有许多行具有相同的 _Class_ 值 (因为一个类可以实现许多接口)。
 6. 可以有许多行具有相同的 _Interface_ 值 (因为许多类可以实现相同的接口)。


### 20.25. MemberRef: 0x0A
<a id="MemberRef_0x0A"></a>

| *Token* | *Class* | ***`Name`*** | _Signature_ |
| :------ | :------ | :----------- | :---------- |

_MemberRef_ 表将对类的方法和字段的两种引用合并在一起，分别称为 "MethodRef" 和 "FieldRef"。

> _MemberRef_ 表有以下列：

 * **_Class_**：或 *Parent*，表示为 _MethodDef_，**`ModuleRef`**，_TypeDef_，**`TypeRef`** 或 _TypeSpec_ 表的索引；更准确地说，是 _MemberRefParent_ (参见 [[↗]](#physical-stream)) 编码索引。

 - *****`Name`*****：***String Heap*** 的索引。

 * **_Signature_**：***Blob Heap*** 的索引。 

每当在 CIL 代码中对在另一个模块或程序集中定义的方法或字段进行引用时，都会在 _MemberRef_ 表中创建一个条目。此外，即使在与调用点相同的模块中定义了具有 *`VARARG`* 签名的方法，也会为其创建一个条目。

> 元数据验证规则

 1. _Class_ 应为以下之一：[ERROR]
     1. 如果定义成员的类在另一个模块中定义，则为 **`TypeRef`**  **_token_**。当成员在此相同的模块中定义时，使用 **`TypeRef`**  **_token_** 是不寻常但有效的，在这种情况下，可以使用其 _TypeDef_  **_token_** 代替。
     2. 如果成员在同一程序集的另一个模块中定义为全局函数或变量，则为 **`ModuleRef`**  **_token_**。
     3. 当用于为在此模块中定义的 **vararg** 方法提供调用点签名时，为 _MethodDef_  **_token_**。***`Name`*** 应与相应 _MethodDef_ 行中的 ***`Name`*** 匹配。_Signature_ 应与目标方法定义中的 _Signature_ 匹配。[ERROR]
     4. 如果成员是泛型类型的成员，则为 _TypeSpec_  **_token_**。
 2. _Class_ 不应为 null (因为这将表示对全局函数或变量的未解析引用)。[ERROR]
 3. ***`Name`*** 应索引 ***String Heap*** 中的 non-empty 字符串。[ERROR]
 4. ***`Name`*** 字符串应为有效的 CLS 标识符。[CLS]
 5. _Signature_ 应索引 ***Blob Heap*** 中的有效字段或方法签名。特别是，它应嵌入以下调用约定中的一个：[ERROR]
      1. *`DEFAULT`* (0x0)
      2. *`VARARG`* (0x5)
      3. *`FIELD`* (0x6)
      4. *`GENERIC`* (0x10)
 6. _MemberRef_ 表应不包含重复项，其中重复行具有相同的 _Class_，***`Name`*** 和 _Signature_。[WARNING]
 7. _Signature_ 不应具有 *`VARARG`* (0x5) 调用约定。[CLS]
 8. 不应有重复行，其中 ***`Name`*** 字段使用 CLS 冲突标识符规则进行比较。特别是，CLS 中忽略了返回类型以及参数是否标记为 *`ELEMENT_TYPE_BYREF`* (参见 [_ELEMENT_TYPE_](#ELEMENT_TYPE))。例如，方法 `.method int32 M()` 和 `.method float64 M()` 根据根据 CLS 规则会产生重复行。同样，方法 `.method void N(int32 i)` 和 `.method void N(int32& i)` 也根据 CLS 规则会产生重复行。[CLS]
 9. 如果 _Class_ 和 ***`Name`*** 解析为字段，则该字段的 _Flags_.*`FieldAccessMask`* 子字段中不应有 *`CompilerControlled`* (参见 [_FieldAttributes_](#FieldAttributes)) 的值。[ERROR]
 10. 如果 _Class_ 和 ***`Name`*** 解析为方法，则该方法的 _Flags_.*`MemberAccessMask`* (参见 [_MethodAttributes_](#MethodAttributes)) 子字段中不应有 *`CompilerControlled`* 的值。[ERROR]
 11. 包含 _MemberRef_ 定义的类型应为表示实例化类型的 _TypeSpec_。

>---
### 20.9. Constant: 0x0B
<a id="Constant_0x0B"></a>

| *Token* | _Type_ | _Parent_ | _Value_ |
| :------ | :----- | :------- | :------ |

_Constant_ 表用于存储字段、参数和属性的编译时常量值。

> _Constant_ 表有以下列：

 * **_Type_**：一个 1 字节常数，后面跟着一个 1 字节的填充零，参见 [_ELEMENT_TYPE_](#ELEMENT_TYPE)。对于 _ilasm_ 中 _FieldInit_ 的 **nullref** 值 [[↗]](#field-init)，_Type_ 的编码是 *`ELEMENT_TYPE_CLASS`*，其 _Value_ 是一个 4 字节的零。与 *`ELEMENT_TYPE_CLASS`* 在签名中的用法不同，这个不是后跟类型 **_token_**。

 - **_Parent_**：一个索引，指向 _Param_、_Field_ 或 _Property_ 表；更准确地说，是一个 _HasConstant_ [[↗]](#physical-stream) 编码索引。 

 * **_Value_**：一个索引，指向 ***Blob Heap***。

请注意，_Constant_ 信息并不直接影响运行时行为，尽管它可以通过反射可见 (因此可以用来实现像 `System.Enum.ToString` 这样的功能)。编译器在导入元数据时，在编译时检查这些信息，但如果使用了常量本身的值，它将嵌入到编译器发出的 CIL 流中。在运行时，没有 CIL 指令可以访问 _Constant_ 表。

每当为父项指定编译时值时，都会在 _Constant_ 表中为父项创建一行。有关示例，请参见 [[↗]](#field-init)。

> 元数据验证规则

 1. _Type_ 应该正好是以下之一：*`ELEMENT_TYPE_BOOLEAN`*，*`ELEMENT_TYPE_CHAR`*，*`ELEMENT_TYPE_I1`*，*`ELEMENT_TYPE_U1`*，*`ELEMENT_TYPE_I2`*，*`ELEMENT_TYPE_U2`*，*`ELEMENT_TYPE_I4`*，*`ELEMENT_TYPE_U4`*，*`ELEMENT_TYPE_I8`*，*`ELEMENT_TYPE_U8`*，*`ELEMENT_TYPE_R4`*，*`ELEMENT_TYPE_R8`*，或 *`ELEMENT_TYPE_STRING`*；或者 *`ELEMENT_TYPE_CLASS`*，其 _Value_ 为零 (参见 [_ELEMENT_TYPE_](#ELEMENT_TYPE))。[ERROR]
 2. _Type_ 不应该是任何以下的：*`ELEMENT_TYPE_I1`*，*`ELEMENT_TYPE_U2`*，*`ELEMENT_TYPE_U4`*，或 *`ELEMENT_TYPE_U8`* (参见 [_ELEMENT_TYPE_](#ELEMENT_TYPE))。[CLS]
 3. _Parent_ 应该索引 _Field_、_Property_ 或 _Param_ 表中的有效行。[ERROR]
 4. 基于 _Parent_，不应有重复的行。[ERROR]
 5. _Type_ 应该完全匹配由 _Parent_ 标识的 _Param_、_Field_ 或 _Property_ 的声明类型 (在父项是枚举的情况下，它应该完全匹配该枚举的底层类型)。[CLS]


>---
### 20.10. CustomAttribute: 0x0C
<a id="CustomAttribute_0x0C"></a>

| *Token* | _Type_ | _Parent_ | _Value_ |
| :------ | :----- | :------- | :------ |

> _CustomAttribute_ 表有以下列：

 * **_Parent_**：一个索引，指向一个与 _HasCustomAttribute_ [[↗]](#physical-stream) 编码索引关联的元数据表。

 - **_Type_**：或 _Constructor_，一个索引，指向 _MethodDef_ 或 _MemberRef_ 表；更准确地说，是一个 _CustomAttributeType_ [[↗]](#physical-stream) 编码索引。

 * **_Value_**：一个指向 ***Blob Heap*** 的索引。

_CustomAttribute_ 表存储的数据可以在运行时用来实例化自定义特性 (更准确地说，是指定的自定义特性类的对象)。名为 _Type_ 的列实际上索引了一个构造方法 —— 该构造方法的 *owner* 是自定义特性的类型。
在 _CustomAttribute_ 表中为父项创建的行由 **.custom** 特性创建，它给出了 _Type_ 列的值，以及可选的 _Value_ 列的值。参见 [_自定义特性_](#custom)。 

> 元数据验证规则

所有二进制值都以小端格式存储 (除了 _PackedLen_ 项，它们仅用作后续 UTF8 字符串中字节数的计数)。

 1. 若不需要 _CustomAttribute_，那么 _Value_ 可以为空。
 2. _Parent_ 可以是任何元数据表的索引，除了 _CustomAttribute_ 表本身。[ERROR]
 3. _Type_ 应索引 _MethodDef_ 或 _MemberRef_ 表中的有效行。该行应该是一个构造方法 (对于这个信息形成实例的类)。[ERROR]
 4. _Value_ 可以为空或非空。
 5. 如果 _Value_ 是非空的，它应该索引 ***Blob Heap*** 中的一个 *blob*。[ERROR]
 6. 以下规则适用于 _Value_ *blob* 的整体结构 [[↗]](#custom-attr-value)：
     * _Prolog_ 应该是 0x0001。[ERROR]
     * _FixedArg_ 出现次数应该与 _Constructor_ 方法中声明的参数一样多。[ERROR]
     * _NumNamed_ 可以是零或更多。
     * 应该有恰好 _NumNamed_ 个 _NamedArg_ 出现。[ERROR]
     * 每个 _NamedArg_ 应该可以被调用方访问。[ERROR]
     * 如果 _NumNamed_ = 0，那么 _CustomAttrib_ 中不应该有更多的项。[ERROR]
 7. 以下规则适用于 _FixedArg_ 的结构 [[↗]](#custom-attr-value)：
    * 如果此项不是向量 (单维数组，下界为 0)，那么应该只有一个 _Elem_。[ERROR]
    * 如果此项是向量，那么：
    * _NumElem_ 应该是 1 或更多。[ERROR]
    * 这后面应该有 _NumElem_ 个 _Elem_ 出现。[ERROR]
 8. 以下规则适用于 _Elem_ 的结构 [[↗]](#custom-attr-value)：
    * 如果这是一个简单类型或枚举 (参见 [[↗]](#custom-attr-value) 了解如何定义)，那么 _Elem_ 只包含它的值。[ERROR]
    * 如果这是一个字符串或类型，那么 _Elem_ 包含一个 _SerString_ —— _PackedLen_ 字节计数，后跟 UTF8 字符。[ERROR]
    * 如果这是一个装箱的简单值类型 (`bool`，`char`，`float32`，`float64`，`int8`，`int16`，`int32`，`int64`，`unsigned int8`，`unsigned int16`，`unsigned int32` 或 `unsigned int64`)，那么 Elem 包含相应的类型表示符 (*`ELEMENT_TYPE_BOOLEAN`*，*`ELEMENT_TYPE_CHAR`*，*`ELEMENT_TYPE_I1`*，*`ELEMENT_TYPE_U1`*，*`ELEMENT_TYPE_I2`*，*`ELEMENT_TYPE_U2`*，*`ELEMENT_TYPE_I4`*，*`ELEMENT_TYPE_U4`*，*`ELEMENT_TYPE_I8`*，*`ELEMENT_TYPE_U8`*，*`ELEMENT_TYPE_R4`* 或 *`ELEMENT_TYPE_R8`*)，并后跟它的值。[ERROR]
 9. 以下规则适用于 _NamedArg_ 的结构 [[↗]](#custom-attr-value)：
    * _NamedArg_ 应该以单字节 *`FIELD`* (0x53) 或 *`PROPERTY`* (0x54) 开头，用于标识。[ERROR]
    * 如果参数种类是装箱的简单值类型，那么字段或属性的类型是 *`ELEMENT_TYPE_BOOLEAN`*，*`ELEMENT_TYPE_CHAR`*，*`ELEMENT_TYPE_I1`*，*`ELEMENT_TYPE_U1`*，*`ELEMENT_TYPE_I2`*，*`ELEMENT_TYPE_U2`*，*`ELEMENT_TYPE_I4`*，*`ELEMENT_TYPE_U4`*，*`ELEMENT_TYPE_I8`*，*`ELEMENT_TYPE_U8`*，*`ELEMENT_TYPE_R4`*，*`ELEMENT_TYPE_R8`*，*`ELEMENT_TYPE_STRING`*，或常数 0x50 (对应于类型为 `System.Type` 的参数) 中的一个。[ERROR]
    * 字段或属性的名称，分别与前一项，存储为 _SerString_ — _PackedLen_ 字节计数，后跟名称的 UTF8 字符。[ERROR]
    * _NamedArg_ 是一个 _FixedArg_ (见上文) 。[ERROR]


### 20.17. FieldMarshal : 0x0D
<a id="FieldMarshal_0x0D"></a>

| *Token* | _Parent_ | _NativeType_ |
| :------ | :------- | :----------- |

_FieldMarshal_ 表有两列。它将 _Field_ 或 _Param_ 表中的现有行 “链接” 到 ***Blob Heap*** 中的信息，该信息定义了该字段或参数 (通常情况下，作为参数编号 0 的方法返回) 在通过 PInvoke 调度调用到 (或从) 非托管代码时应如何进行封送。

请注意，_FieldMarshal_ 信息仅由与非托管代码进行操作的代码路径使用。为了执行这样的路径，在大多数平台上调用方被安装为具有更高的安全权限。一旦它调用了非托管代码，它就脱离了 CLI 可以检查的范围 —— 它只是被信任不会违反类型系统。

> _FieldMarshal_ 表有以下列：

 * **_Parent_**：一个索引，指向 _Field_ 或 _Param_ 表；更准确地说，是一个 _HasFieldMarshal_ [[↗]](#physical-stream) 编码索引。

 - **_NativeType_**：一个指向 ***Blob Heap*** 的索引。 

有关 '*blob*' 的详细格式，请参见 [_二进制数据对象_](#blob-description)。

如果父字段的 **.field** 指令指定了 **marshal** 特性 [[↗]](#field-marshal)，则会在 _FieldMarshal_ 表中创建一行。

> 元数据验证规则

 1. _FieldMarshal_ 表可以包含零行或多行。
 2. _Parent_ 应该索引 _Field_ 或 _Param_ 表中的有效行 (_Parent_ 值被编码为表示每个索引引用的是这两个表中的哪一个)。[ERROR]
 3. _NativeType_ 应该索引 ***Blob Heap*** 中的非空 '*blob*'。[ERROR]
 4. 任何两行都不能指向同一个父项。换句话说，在 _Parent_ 值已被解码为确定它们是引用 _Field_ 表还是 _Param_ 表之后，没有两行可以指向 _Field_ 表或 _Param_ 表中的同一行。[ERROR]
 5. 以下检查适用于 _MarshalSpec_ '*blob*'：
     * _NativeIntrinsic_ 应该是其生成的常数值之一 [[↗]](#blob-description)。[ERROR]
     * 如果是 *`ARRAY`*，那么 *ArrayElemType* 应该是其生成的常数值之一。[ERROR]
     * 如果是 *`ARRAY`*，那么 _ParamNum_ 可以为零。
     * 如果是 *`ARRAY`*，那么 _ParamNum_ 不能小于 0。[ERROR]
     * 如果是 *`ARRAY`*，并且 _ParamNum_ > 0，那么 _Parent_ 应该指向 _Param_ 表中的一行，而不是 _Field_ 表。[ERROR]
     * 如果是 *`ARRAY`*，并且 _ParamNum_ > 0，那么 _ParamNum_ 不能超过父 _Param_ 是其成员的 _MethodDef_ (或者如果是 *`VARARG`* 调用，则为 _MethodRef_) 提供的参数数量。[ERROR]
     * 如果是 *`ARRAY`*，那么 _ElemMult_ 应该大于等于 1。[ERROR]
     * 如果是 *`ARRAY`* 并且 _ElemMult_ 不等于 1，则发出警告，因为这可能是一个错误。[WARNING]
     * 如果是 *`ARRAY`* 并且 _ParamNum_ = 0，那么 _NumElem_ 应该大于等于 1。[ERROR]
     * 如果是 *`ARRAY`* 并且 _ParamNum_ 不等于 0 并且 _NumElem_ 不等于 0，则发出警告，因为这可能是一个错误。[WARNING]
     
>---
### 20.11. DeclSecurity: 0x0E
<a id="DeclSecurity_0x0E"></a>

| *Token* | _Parent_ | _Action_ | _PermissionSet_ |
| :------ | :------- | :------- | :-------------- |

可以将源自 `System.Security.Permissions.SecurityAttribute` (参见第四部分 【】) 的安全特性附加到 _TypeDef_、_Method_ 或 _Assembly_。此类的所有构造函数都应将 `System.Security.Permissions.SecurityAction` 值作为其第一个参数，描述应对附加到的类型、方法或程序集的权限进行何种操作。代码访问派生自 `System.Security.Permissions.CodeAccessSecurityAttribute` 的安全特性，可以具有任何安全操作。

这些不同的安全操作在 _DeclSecurity_ 表中被编码为 2 字节的枚举 (见下文)。对于方法、类型或程序集上给定安全操作的所有安全自定义特性应聚集在一起，并创建一个 `System.Security.PermissionSet` 实例，存储在 ***Blob Heap*** 中，并从 _DeclSecurity_ 表中引用。

从编译器的角度来看，一般流程如下：用户通过某种特定于语言的语法指定自定义特性，该语法编码了对特性的构造函数的调用。如果特性的类型直接或间接派生自 `System.Security.Permissions.SecurityAttribute`，那么它就是一个安全自定义特性，并需要特殊处理，如下所述 (其他自定义特性通过简单地在元数据中记录构造函数来处理，如 [*CustomAttribute*](#CustomAttribute_0x0C) 表所述)。构造特性对象，并提供一个方法 (`CreatePermission`) 将其转换为安全权限对象 (从 `System.Security.Permission` 派生的对象)。所有附加到具有相同安全操作的给定元数据项的权限对象都被组合在一起，形成一个 `System.Security.PermissionSet`。使用其 `ToXML` 方法将此权限集转换为准备存储在 XML 中的形式，以创建 `System.Security.SecurityElement`。最后，使用 `SecurityElement` 上的 `ToString` 方法创建元数据所需的 XML。

> _DeclSecurity_ 表有以下列：

 * **_Action_**：一个 2 字节值。 

 * **_Parent_**：一个索引，指向 _TypeDef_、_MethodDef_ 或 _Assembly_ 表；更准确地说，是一个 _HasDeclSecurity_ [[↗]](#physical-stream) 编码索引。

 * **_PermissionSet_**：一个索引，指向 ***Blob Heap***。

_Action_ 是安全操作 (参见 `System.Security.SecurityAction`，第四部分 【】) 的 2 字节表示。值 0 ~ 0xFF 保留供未来标准使用。值 0x20 ~ 0x7F 和 0x100 ~ 0x07FF 用于操作，如果操作不被识别或支持，可以忽略。值 0x80 ~ 0xFF 和 0x0800 ~ 0xFFFF 用于需要执行安全操作的操作；在操作不可用的实现中，不应允许访问程序集、类型或方法。

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

_Parent_ 是一个元数据 **_token_**，用于标识在其上定义 _PermissionSet_ 中编码的安全自定义特性的 _Method_，_Type_ 或 _Assembly_。

_PermissionSet_ 是一个 '*blob*'，其格式如下：
 * 包含一个句点 (.) 的字节。
 * 一个压缩的无符号整数，包含 *blob* 中编码的特性的数量。
 * 包含以下内容的特性数组：
    * 一个字符串，它是特性的完全限定类型名称。字符串被编码为一个压缩的无符号整数，以指示大小，后跟一个 UTF8 字符数组。
    * 一组特性，编码为自定义特性的命名参数 [[↗]](#custom-attr-value)，从 _NumNamed_ 开始。

权限集包含在特定的 _Method_，_Type_ 或 _Assembly_ (参见 _Parent_) 上请求的具有 _Action_ 的权限。换句话说，*blob* 将包含 _Parent_ 上具有该特定 _Action_ 的所有特性的编码。

此标准的第一版指定了权限集的 XML 编码。后续实现应继续支持此编码以向后兼容。

_DeclSecurity_ 表的行是通过附加一个指定 _Action_ 和 _PermissionSet_ 的 **.permission** 或 **.permissionset** 指令到父程序集 [[↗]](#assembly-inside-decl) 或父类型或方法 [[↗]](#class-type-member) 上填充的。

> 元数据验证规则

 1. _Action_ 应该只设置那些指定的值。[ERROR]
 2. _Parent_ 应该是 _TypeDef_，_MethodDef_ 或 _Assembly_ 中的一个。也就是说，它应该索引 _TypeDef_ 表，_MethodDef_ 表或 _Assembly_ 表中的有效行。[ERROR]
 3. 如果 _Parent_ 索引了 _TypeDef_ 表中的一行，那么该行不应定义接口。安全系统会忽略任何这样的父项；编译器不应发出这样的权限集。[WARNING]
 4. 如果 _Parent_ 索引了一个 _TypeDef_，那么它的 _TypeDef_._Flags_.*`HasSecurity`* 位应该被设置。[ERROR]
 5. 如果 _Parent_ 索引了一个 _MethodDef_，那么它的 _MethodDef_._Flags_.*`HasSecurity`* 位应该被设置。[ERROR]
 6. _PermissionSet_ 应该索引 ***Blob Heap*** 中的一个 '*blob*'。[ERROR]
 7. _PermissionSet_ 索引的 '*blob*' 的格式应该表示一个有效的、编码的 CLI 对象图。所有标准化权限的编码形式在 [Partition IV【】]() 中指定。[ERROR]

>---
### 20.8. ClassLayout: 0x0F
<a id="ClassLayout_0x0F"></a>

| *Token* | _Parent_ | _PackingSize_ | _ClassSize_ |
| :------ | :------- | :------------ | :---------- |

_ClassLayout_ 表用于定义 CLI 应如何布局类或值类型的字段。通常，CLI 可以自由地重新排序和 / 或在为类或值类型定义的字段之间插入间隙。

此功能用于以与非托管 C 结构体完全相同的方式布局托管值类型，从而允许将托管值类型交给非托管代码，然后访问字段，就像该内存块是由非托管代码布局的一样。

_ClassLayout_ 表中的信息取决于 *owner* 类或值类型中 {*`AutoLayout`*,*`SequentialLayout`*, *`ExplicitLayout`*} 的 _Flags_ 值。如果类型被标记为 *`SequentialLayout`* 或 *`ExplicitLayout`*，则该类型具有布局。如果继承链中的任何类型具有布局，则其所有基类也应具有布局，直到它的直接基类是从 `System.ValueType` 派生的那个基类 (如果它存在于类型的层次结构中)；否则，直接基类是 `System.Object`。

布局不能在链的中间开始。但是在链的任何点停止具有 “布局” 都是有效的。例如，在下面的图表中，类 A 从 `System.Object` 派生；类 B 从 A 派生；类 C 从 B 派生。`System.Object` 没有布局。但是 A，B 和 C 都定义了布局，这是有效的。

 ![有效的布局设置](./.img/有效的布局设置.png)

类 E，F 和 G 的情况类似。G 没有布局，这也是有效的。下图显示了两个无效的设置： 

 ![无效的布局设置](./.img/无效的布局设置.png)

在左边，“具有布局的链” 并未从 “最高” 的类开始。在右边，“具有布局的链” 中有一个 “洞”。

类或值类型的布局信息保存在两个表 (*ClassLayout* 和 *FieldLayout*) 中，如下图所示：

 ![ClassLayout和FieldLayout](./.img/ClassLayout和FieldLayout.png)

在此示例中，_ClassLayout_ 表的第 3 行指向 _TypeDef_ 表的第 2 行 (类的定义，称为 “MyClass”)。_FieldLayout_ 表的第 4 ~ 6 行指向 _Field_ 表中的相应行。这说明了 CLI 如何存储在 “MyClass” 中定义的三个字段的显式偏移 (对于拥有类或值类型的每个字段，_FieldLayout_ 表中总是有一行) 因此，_ClassLayout_ 表充当 _TypeDef_ 表中具有布局信息的那些行的扩展；由于许多类没有布局信息，总的来说，这种设计节省了空间。

> _ClassLayout_ 表有以下列：

 * _PackingSize_：2 字节常量。 
 - _ClassSize_：4 字节常量。
 * _Parent_：_TypeDef_ 表的索引。

通过在此类型声明的类型声明主体上放置 **.pack** 和 **.size** 指令来定义 _ClassLayout_ 表的行 [[↗]](#class-type-member)。当省略这些指令中的任何一个时，其对应的值为零。参见 [_控制实例布局_](#ctrl-layout)。 

_ClassSize_ 为零并不意味着类的大小为零。这意味着在定义时没有指定 **.size** 指令，在这种情况下，实际大小是从字段类型计算出来的，并考虑到打包大小 (默认或指定) 和目标运行时平台上的自然对齐。

> 元数据验证规则

 1. _ClassLayout_ 表可以包含零行或多行。
 2. _Parent_ 应索引 _TypeDef_ 表中的有效行，对应于类或值类型 (但不对应于接口)。[ERROR]
 3. _Parent_ 索引的类或值类型应为 *`SequentialLayout`* 或 *`ExplicitLayout`* [[↗]](#FieldAttributes)。也就是说，*`AutoLayout`* 类型不应拥有 _ClassLayout_ 表中的任何行。[ERROR]
 4. 如果 _Parent_ 索引了一个 *`SequentialLayout`* 类型，那么：
    * _PackingSize_ 应为 {0, 1, 2, 4, 8, 16, 32, 64, 128} 中的一个。0 表示使用应用程序运行的平台的默认打包大小。[ERROR]
    * 如果 _Parent_ 索引了一个 ValueType，那么 _ClassSize_ 应小于 1 MByte (0x100000 字节)。[ERROR]
 5. 如果 _Parent_ 索引了一个 *`ExplicitLayout`* 类型，那么
    * 如果 _Parent_ 索引了一个 ValueType，那么 _ClassSize_ 应小于 1 MByte (0x100000 字节)。[ERROR]
    * _PackingSize_ 应为 0。为每个字段提供显式偏移以及打包大小是没有意义的。[ERROR]
 6. 注意，如果布局没有创建字段重叠的类型，那么 *`ExplicitLayout`* 类型可能会产生可验证的类型。
 7. 沿着继承链方向的布局应遵循上述规则 (从 “最高” 类型开始，没有 “孔” 等)。[ERROR]

>---
### 20.16. FieldLayout: 0x10
<a id="FieldLayout_0x10"></a>

| *Token* | _Field_ | _Offset_ |
| :------ | :------ | :------- |

> _FieldLayout_ 表有以下列：

 * **_Offset_**：或 _FieldOffSet_，4 字节常量。 

 - **_Field_**：_Field_ 表的索引。

请注意，任何类型中的每个字段都由其签名定义。当 CLI 布局类型实例 (即，对象) 时，每个字段是四种类型之一：
 * **Scalar**：用于任何内置类型的成员，例如 `int32`。字段的大小由该内在类型的实际大小给出，其大小在 1 到 8 字节之间变化。
 * **ObjectRef**：用于 *`ELEMENT_TYPE_CLASS`*，*`ELEMENT_TYPE_STRING`*，*`ELEMENT_TYPE_OBJECT`*，*`ELEMENT_TYPE_ARRAY`*，*`ELEMENT_TYPE_SZARRAY`*
 * **Pointer**：用于 *`ELEMENT_TYPE_PTR`*，*`ELEMENT_TYPE_FNPTR`*
 * **ValueType**：用于 *`ELEMENT_TYPE_VALUETYPE`*。该 ValueType 的实例实际上是在此对象中布局的，因此字段的大小是该 ValueType 的大小。

请注意，指定显式结构布局的元数据可以在一个平台上有效地使用，但在另一个平台上可能无效，因为这里指定的一些规则取决于特定于平台的对齐规则。

如果父字段的 **.field** 指令已指定字段偏移，则将在 _FieldLayout_ 表中创建一行。参见 [*字段定义和字段引用*](#field)。

> 元数据验证规则

 1. _FieldLayout_ 表可以包含零行或多行。
 2. _FieldLayout_ 表中每行描述的字段的类型应设置 _Flags_.*`ExplicitLayout`* (参见 [_TypeAttributes_](#TypeAttributes))。[ERROR]
 3. _Offset_ 应为零值或比零大值。[ERROR]
 4. _Field_ 应索引 _Field_ 表中的有效行。[ERROR]
 5. _Field_ 索引的 _Field_ 表中的行的 _Flags_.*`Static`* 应为非静态 (值为 0)。[ERROR]
 6. 在给定类型拥有的行中，基于 _Field_ 不应有重复项。也就是说，类型的给定 _Field_ 不能被赋予两个偏移。[ERROR]
 7. 类型 **ObjectRef** 的每个字段应在类型内自然对齐。[ERROR]
 8. 在给定类型拥有的行中，完全有效的是有几行具有相同的 _Offset_ 值。但是 **ObjectRef** 和值类型不能具有相同的偏移。[ERROR]
 9. *`ExplicitLayout`* 类型的每个字段都应给出偏移；也就是说，它应在 _FieldLayout_ 表中有一行。[ERROR]

>---
### 20.36. StandAloneSig: 0x11
<a id="StandAloneSig_0x11"></a>

| *Token* | _Signature_ |
| :------ | :---------- |

签名存储在元数据 ***Blob Heap*** 中。在大多数情况下，它们由某个表的某个列索引 —— _Field_._Signature_、_Method_._Signature_、_MemberRef_._Signature_ 等。然而，有两种情况需要一个元数据 **_token_** 来表示一个不由任何元数据表索引的签名。_StandAloneSig_ 表满足了这个需求。它只有一列，该列指向 ***Blob Heap*** 中的一个 _Signature_。

签名应描述以下之一：
 * **一个方法** — 代码生成器为每次 `calli` 指令的调用，在 _StandAloneSig_ 表中创建一行。该行索引 `calli` 指令的函数指针操作数的调用点签名。
 * **局部变量** — 代码生成器为每个方法在 _StandAloneSig_ 表中创建一行，以描述其所有的局部变量。ILAsm 中的 **.locals** 指令 [[↗]](#locals) 在 _StandAloneSig_ 表中生成一行。

> _StandAloneSig_ 表有以下列：

 * **_Signature_**：一个指向 ***Blob Heap*** 的索引。 

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

 1. _StandAloneSig_ 表可以包含零行或多行。
 2. _Signature_ 应该索引 ***Blob Heap*** 中的有效签名。[ERROR]
 3. 由 _Signature_ 索引的签名 '*blob*' 应该是一个有效的 *`METHOD`* 或 *`LOCALS`* 签名。[ERROR]
 4. 允许重复的行。

>---
### 20.12. EventMap: 0x12
<a id="EventMap_0x12"></a>

| *Token* | _Parent_ | _EventList_ |
| :------ | :------- | :---------- |

_EventMap_ 表有以下列：

 * **_Parent_**：一个索引，指向 _TypeDef_ 表。 

 - **_EventList_**：一个索引，指向 _Event_ 表。它标记了由此类型拥有的一连串事件的第一个。该连续运行继续到以下较小者：
    * _Event_ 表的最后一行。
    * 通过检查 _EventMap_ 表中下一行的 _EventList_ 找到的下一连串事件。

请注意，_EventMap_ 信息并不直接影响运行时行为；重要的是每个事件包含的方法的信息。_EventMap_ 和 _Event_ 表是将 **.event** 指令用于类上的结果 [[↗]](#event)。


> 元数据验证规则

 1. _EventMap_ 表可以包含零行或多行。
 2. 基于 _Parent_，不应有重复的行 (给定的类只有一个指向其事件列表开始的 “指针”)。[ERROR]
 3. 基于 _EventList_，不应有重复的行 (不同的类不能在 _Event_ 表中共享行)。[ERROR]

>---
### 20.13. Event: 0x14
<a id="Event_0x14"></a>

| *Token* | _EventFlags_ | ***`Name`*** | _EventType_ |
| :------ | :----------- | :----------- | :---------- |

事件在元数据中的处理方式与属性非常相似；也就是说，它将定义关联到给定类上的一组方法。有两个必需的方法 (`add_` 和 `remove_`) 以及一个可选的方法 (`raise_`)；还允许其他名称的附加方法 [[↗]](#event)。作为事件聚集在一起的所有方法都应在包含事件的类上定义。

在 _TypeDef_ 表的一行与构成给定事件的方法集合之间的关联关系保存在三个单独的表中 (这与属性类似)，如下所示：

 ![事件表示例](./.img/事件表示例.png)

_EventMap_ 表的第 3 行索引了左边 _TypeDef_ 表的第 2 行 (`MyClass`)，同时索引了右边 _Event_ 表的第 4 行 (一个名为 `DocChanged` 的事件)。这个设置构建了 `MyClass` 有一个名为 `DocChanged` 的事件。但是 _MethodDef_ 表中的哪些方法被聚集在一起作为 “属于” 事件 `DocChanged`？该关联关系包含在 _MethodSemantics_ 表中 —— 它的第 2 行索引了右边的事件 `DocChanged`，以及左边 _MethodDef_ 表的第 2 行 (一个名为 `add_DocChanged` 的方法)。此外，_MethodSemantics_ 表的第 3 行索引了右边的 `DocChanged`，以及左边 _MethodDef_ 表的第 3 行 (一个名为 `remove_DocChanged` 的方法)。如图所示，`MyClass` 还有另一个事件，名为 `TimedOut`，有两个方法，`add_TimedOut` 和 `remove_TimedOut`。

_Event_ 表不仅仅是将其他表中的现有行聚集在一起。_Event_ 表有 _EventFlags_，***`Name`*** (例如，这里的示例中的 `DocChanged` 和 `TimedOut`) 和 _EventType_ 列。此外，_MethodSemantics_ 表有一列用于记录它索引的方法是 `add_`，`remove_`，`raise_` 还是其他函数。

> _Event_ 表有以下列：

 * **_EventFlags_**：或 *Attributes*，类型为 _EventAttributes_ 的 2 字节位掩码，参见 §[II.23.1.4](ii.23.1.4-flags-for-events-eventattributes.md)) 

 - *****`Name`*****：***String Heap*** 的索引。 

 * **_EventType_**：或 *Type*，_TypeDef_，**`TypeRef`** 或 _TypeSpec_ 表的索引；更准确地说，是 _TypeDefOrRef_ (参见 §[_FieldAttributes_](#FieldAttributes)) 编码索引。它对应于事件的类型，而不是对应拥有此事件的类型。 

请注意，_Event_ 信息并不直接影响运行时行为；重要的是事件包含的每个方法的信息。_EventMap_ 和 _Event_ 表是将 **.event** 指令放在类上的结果 (参见 [_事件定义_](#event))。

> 元数据验证规则

 1. _Event_ 表可以包含零行或多行。
 2.  _EventMap_ 表中的每一行在都应有一个 _EventList_，且只有一个。[ERROR]
 3. _EventFlags_ 只应设置指定的值 (所有组合有效)。[ERROR]
 4. ***`Name`*** 应索引 ***String Heap*** 中的 no-empty 字符串。[ERROR]
 5. ***`Name`*** 字符串应为有效的 CLS 标识符。[CLS]
 6. _EventType_ 可以为 null 或非 null。
 7. 如果 _EventType_ 为非 null，则它应索引 _TypeDef_ 或 **`TypeRef`** 表中的有效行。[ERROR]
 8. 如果 _EventType_ 为非 null，则它索引的 _TypeDef_，**`TypeRef`** 或 _TypeSpec_ 表中的行应为类 (不是接口或值类型)。[ERROR]
 9. 对于每一行，在 _MethodSemantics_ 表中应有一个 `add_` 和一个 `remove_` 行。[ERROR]
 10. 对于每一行，可以有零个或一个 `raise_` 行，以及 _MethodSemantics_ 表中的零个或多个 `other` 行。[ERROR]
 11. 在 _TypeDef_ 表中的给定行拥有的行中，基于 ***`Name`*** 不应有重复项。[ERROR]
 12. 基于 ***`Name`*** 不应有重复行，其中 ***`Name`*** 字段使用 CLS 冲突标识符规则进行比较。[CLS]

>---
### 20.35. PropertyMap: 0x15
<a id="PropertyMap_0x15"></a>

| *Token* | _Parent_ | _PropertyList_ |
| :------ | :------- | :------------- |

> _PropertyMap_ 表有以下列：

 * **_Parent_**：_TypeDef_ 表的索引。

 * **_PropertyList_**：_Property_ 表的索引。它标记了由 _Parent_ 拥有的属性的连续运行的第一个。运行继续到以下较小者：
     * _Property_ 表的最后一行。
     * 通过检查此 _PropertyMap_ 表中下一行的 _PropertyList_ 找到的下一组属性。

_PropertyMap_ 和 _Property_ 表是将 **.property** 指令放在类上的结果 (参见 [*属性定义*](#property))。

> 元数据验证规则

 1. _PropertyMap_ 表可以包含零行或多行。
 2. 基于 _Parent_ 不应有重复行 (给定类只有一个指向其属性列表开始的 “指针”)。[ERROR]
 3. 基于 _PropertyList_ 不应有重复行 (不同的类不能在 _Property_ 表中共享行)。[ERROR]

>---
### 20.34. Property: 0x17
<a id="Property_0x17"></a>

| *Token* | _Flags_ | ***`Name`*** | _Type_ |
| :------ | :------ | :----------- | :----- |

在元数据中，属性最好被视为一种手段，用于将定义在类上的方法集合聚在一起，并给它们一个名字，而不是其他。这些方法通常是已经在类上定义的 *get_* 和 *set_* 方法，并像其他方法一样插入到 _MethodDef_ 表中。这种关联是由三个独立的表维护在一起，如下图所示：

 ![属性表示例](./.img/属性表示例.png)

_PropertyMap_ 表的第 3 行索引了左边 _TypeDef_ 表的第 2 行 (`MyClass`)，同时索引了右边 _Property_ 表的第 4 行 —— 一个名为 Foo 的属性的行。这个设置构建了 `MyClass` 有一个名为 `Foo` 的属性。但是在 _MethodDef_ 表中，哪些方法被聚集在一起作为 “属于” 属性 `Foo`？这种关联包含在 _MethodSemantics_ 表中 —— 它的第 2 行索引了右边的属性 `Foo`，和左边 _MethodDef_ 表的第 2 行 (一个名为 `get_Foo` 的方法)。此外，_MethodSemantics_ 表的第 3 行索引了右边的 `Foo`，和左边 _MethodDef_ 表的第 3 行 (一个名为 `set_Foo` 的方法)。如图所示，`MyClass` 还有另一个属性叫 `Bar`，有两个方法，`get_Bar` 和 `set_Bar`。

属性表做的不仅仅是将其他表中已有的行聚集在一起。_Property_ 表有 _Flags_、***`Name`*** (例如这里的 `Foo` 和 `Bar`) 和 _Type_ 的列。此外，_MethodSemantics_ 表有一个列来记录它指向的方法是 *set_*、*get_* 还是 *other*。

CLS 引用了实例、虚拟和静态属性。属性的签名 (来自 _Type_ 列) 可以用来区分静态属性，因为实例和虚拟属性在签名中会设置 "*`HASTHIS`*" 位 [[↗]](#MethodDefSig)，而静态属性则不会。实例和虚拟属性之间的区别取决于 *getter* 和 *setter* 方法的签名，CLS 要求它们要么都是虚拟的，要么都是实例的。

> _Property_ (0x17) 表有以下列：

 * **_Flags_**：或 *Attributes*，一个 2 字节的位掩码，类型为 _PropertyAttributes_ [[↗]](#EventAttributes)。

 - *****`Name`*****：一个索引，指向 ***String Heap***。 
 
 * **_Type_**：或 *Signature*，一个索引，指向 ***Blob Heap***。这个列的名称是可能有一定的误导性。它不是索引 _TypeDef_ 或 **`TypeRef`** 表，而是索引了 ***Blob Heap*** 中的属性的签名。

> 元数据验证规则

 1. _Property_ 表可以包含零行或多行。
 2. 在 _PropertyMap_ 表中的 *owner* 行，每一行应有一个 _PropertyList_，且只有一个。[ERROR]
 3. _PropFlags_ 只应设置那些指定的值 (所有组合有效)。[ERROR]
 4. ***`Name`*** 应索引 ***String Heap*** 中的 no-empty 字符串。[ERROR]
 5. ***`Name`*** 字符串应是一个有效的 CLS 标识符。[CLS]
 6. _Type_ 应索引 ***Blob Heap*** 中的非空签名。[ERROR]
 7. 由 _Type_ 索引的签名应是一个有效的属性签名 (即，前导字节的低四位是 0x8)。除了这个前导字节，签名与属性的 *get_* 方法相同。[ERROR]
 8. 在由 _TypeDef_ 表中的给定行拥有的行中，基于 ***`Name`***+_Type_ 不应有重复的行。[ERROR]
 9. 基于 ***`Name`***，不应有重复的行，其中 ***`Name`*** 字段使用 CLS 冲突标识符规则进行比较。特别是，属性不能通过它们的类型进行重载 —— 例如，一个类不能有两个属性，"`int Foo`" 和 "`String Foo`"。[CLS]
>---
### 20.28. MethodSemantics: 0x18
<a id="MethodSemantics_0x18"></a>

| *Token* | _Semantics_ | _Method_ | _Association_ |
| :------ | :---------- | :------- | :------------ |

> _MethodSemantics_ 表有以下列：

 * **_Semantics_**：一个 2 字节的位掩码，类型为 _MethodSemanticsAttributes_ [[↗]](#MethodSemanticsAttributes)。

 - **_Method_**：一个索引，指向 _MethodDef_ 表。

 * **_Association_**：一个索引，指向 _Event_ 或 _Property_ 表；更准确地说，是一个 _HasSemantics_ [[↗]](#physical-stream) 编码索引。

_MethodSemantics_ 表的行由 **.property** [[↗]](#property) 和 **.event** 指令 [[↗]](#event) 填充。有关更多信息，请参见 [_Event_0x14_](#Event_0x14) 和 [*Property_0x17*](#Property_0x17)。

> 元数据验证规则
 1. _MethodSemantics_ 表可以包含零行或多行。
 2. _Semantics_ 只应设置那些指定的值。[ERROR]
 3. _Method_ 应索引 _MethodDef_ 表中的有效行，该行应为此行描述的属性或事件的同一类中定义的方法。[ERROR]
 4. 对于给定的属性或事件，所有方法应具有相同的可访问性 (即他们的 _Flags_ 行的 *`MemberAccessMask`* 子字段) 并且不能是 *`CompilerControlled`*。[CLS]
 5. _Semantics_：受以下限制：
     * 如果此行是用于属性的，那么 *`Setter`*、*`Getter`* 或 *`Other`* 中的一个被设置。[ERROR]
     * 如果此行是用于事件的，那么 *`AddOn`*、*`RemoveOn`*、*`Fire`* 或 *`Other`* 中的一个被设置。[ERROR]
 6. 如果此行是用于事件的，并且其 _Semantics_ 是 *`Addon`* 或 *`RemoveOn`*，那么由 _Method_ 索引的 _MethodDef_ 表中的行应接受一个委托作为参数，并返回 `void`。[ERROR]
 7. 如果此行是用于事件的，并且其 _Semantics_ 是 *`Fire`*，那么由 _Method_ 索引的 _MethodDef_ 表中的行可以返回任何类型。
 8. 对于每个属性，应有一个 *setter*，或一个 *getter*，或两者都有。[CLS]
 9. 其 ***`Name`*** 是 `xxx`的任何属性的 *getter* 方法，它应被称为 `get_xxx`。[CLS]
 10. 其 ***`Name`*** 是 `xxx` 的任何属性的 *setter* 方法，它应被称为 `set_xxx`。[CLS]
 11. 如果一个属性提供了 *getter* 和 *setter* 方法，那么这些方法应在 _Flags_.*`MemberAccessMask`* 子字段中具有相同的值。[CLS]
 12. 如果一个属性提供了 *getter* 和 *setter* 方法，那么这些方法应对于他们的 _Method_._Flags_.*`Virtual`* 具有相同的值。[CLS]
 13. 任何 *getter* 和 *setter* 方法应具有 _Method_._Flags_.*`SpecialName`* = 1。[CLS]
 14. 任何 *getter* 方法应具有与 _Property_._Type_ 字段索引的签名匹配的返回类型。[CLS]
 15. 任何 *setter* 方法的最后一个参数应具有与 _Property_._Type_ 字段索引的签名匹配的类型。[CLS]
 16. 任何 *setter* 方法应在 _Method_._Signature_ 中具有返回类型 *`ELEMENT_TYPE_VOID`* [[↗]](#ELEMENT_TYPE)。[CLS]
 17. 如果属性被索引，那么 *getter* 和 *setter* 的索引在数量和类型上必须一致。[CLS]
 18. 任何事件的 *AddOn* 方法，其 ***`Name`*** 是 `xxx`，应具有签名：``void add_xxx (<DelegateType> handler)``。[CLS]
 19. 任何事件的 *RemoveOn* 方法，其 ***`Name`*** 是 `xxx`，应具有签名：`void remove_xxx(<DelegateType> handler)`。[CLS]
 20. 任何事件的 *Fire* 方法，其 ***`Name`*** 是 `xxx`，应具有签名：`void raise_xxx(Event e)`。[CLS]

>---
### 20.27. MethodImpl: 0x19
<a id="MethodImpl_0x19"></a>

| *Token* | _MethodDeclaration_ | _MethodBody_ | _Class_ |
| :------ | :------------------ | :----------- | :------ |

_MethodImpl_ 表允许编译器覆盖 CLI 提供的默认继承规则。它们最初的用途是允许一个类 `C`，它从接口 `I` 和 `J` 都继承了方法 `M`，为这两个方法提供实现 (而不是在其 vtable 中只有 `M` 的一个插槽)。然而，_MethodImpls_ 也可以出于其他原因使用，只受限于编译器编写者在下面定义的验证规则的约束内的独创性。

在上面的例子中，_Class_ 指定 `C`，_MethodDeclaration_ 指定 `I::M`，_MethodBody_ 指定为 `I::M` 提供实现的方法 (要么是 `C` 内的一个方法体，要么是 `C` 的基类实现的一个方法体)。

> _MethodImpl_ 表有以下列：

 * **_Class_**：或 *Type*，一个指向 _TypeDef_ 表的索引。 

 - **_MethodBody_**：一个指向 _MethodDef_ 或 _MemberRef_ 表的索引；更准确地说，是一个 _MethodDefOrRef_ [[↗]](#physical-stream) 编码索引。 

 * **_MethodDeclaration_**：一个指向 _MethodDef_ 或 _MemberRef_ 表的索引；更准确地说，是一个 _MethodDefOrRef_ [[↗]](#physical-stream) 编码索引。

ILAsm 使用 **.override** 指令来指定 _MethodImpl_ 表的行，参考 [[↗]](#override) 和 [[↗]](#MethodBody)。

> 元数据验证规则

 1. _MethodImpl_ 表可以包含零行或多行。
 2. _Class_ 应该索引 _TypeDef_ 表中的有效行。[ERROR]
 3. _MethodBody_ 应该索引 _MethodDef_ 或 _MemberRef_ 表中的有效行。[ERROR]
 4. 由 _MethodDeclaration_ 索引的方法应该设置 _Flags_.*`Virtual`*。[ERROR]
 5. 由 _MethodDeclaration_ 索引的方法的 *owner* 类型不应该有 _Flags_.*`Sealed`* = 0。[ERROR]
 6. 由 _MethodBody_ 索引的方法应该是 _Class_ 或 _Class_ 的某个基类的成员 (*MethodImpl*s 不允许编译器 “*hook*” 任意方法体)。[ERROR]
 7. 由 _MethodBody_ 索引的方法应该是虚方法。[ERROR]
 8. 由 _MethodBody_ 索引的方法其 _Method_._RVA_ ≠ 0 (例如，不能是通过 PInvoke 到达的非托管方法)。[ERROR]
 9. _MethodDeclaration_ 应该索引 _Class_ 的祖先链中的一个方法 (通过其 _Extends_ 链到达) 或 _Class_ 的接口树中的一个方法 (通过其 _InterfaceImpl_ 条目到达)。[ERROR]
 10. 由 _MethodDeclaration_ 索引的方法不应该是 **final** (其 _Flags_.`Final` 应该是 0)。[ERROR]
 11. 如果 _MethodDeclaration_ 设置了 *`Strict`* 标志，那么由 _MethodDeclaration_ 索引的方法应该对 _Class_ 是可访问的。[ERROR]
 12. 由 _MethodBody_ 定义的方法签名应该与 _MethodDeclaration_ 定义的方法签名匹配。[ERROR]
 13. 基于 _Class_+_MethodDeclaration_ 不应该有重复的行。[ERROR]

>---
### 20.31. ModuleRef: 0x1A
<a id="ModuleRef_0x1A"></a>

| *Token* | ***`Name`*** |
| :------ | :----------- |

> **`ModuleRef`** 表有以下列：

 * *****`Name`*****：一个索引，指向 ***String Heap***。 

**`ModuleRef`** 表中的行是由 Assembly 中的 **.module extern** 指令 [[↗]](#module-extern) 产生的。

> 元数据验证规则

 1. ***`Name`*** 应索引 ***String Heap*** 中的 no-empty 字符串。这个字符串应使 CLI 能够定位目标模块。通常，它可能命名用于保存模块的文件。[ERROR]
 2. 不应有重复的行。[WARNING]
 3. ***`Name`*** 应与 _File_ 表的 ***`Name`*** 列中的一个条目匹配。此外，该条目应使 CLI 能够定位目标模块。通常它可能命名用于保存模块的文件。[ERROR]

>---
### 20.39. TypeSpec: 0x1B
<a id="TypeSpec_0x1B"></a>

| *Token* | _Signature_ |
| :------ | :---------- |

_TypeSpec_ 表只有一列，它索引了存储在 ***Blob Heap*** 中的一个类型的规范。这为该类型提供了一个元数据 **_token_**  (而不是简单地索引 ***Blob Heap***)。这通常是必需的，例如，对数组操作，如创建或调用数组类的方法。

> _TypeSpec_ 表有以下列：

 * **_Signature_**：索引到 ***Blob Heap***，其中 *blob* 的格式如 [[↗]](#type-spec-blob) 所指定。

注意，_TypeSpec_  **_token_** 可以与任何接受 _TypeDef_ 或 **`TypeRef`**  **_token_** 的 CIL 指令一起使用；具体来说，`castclass`，`cpobj`，`initobj`，`isinst`，`ldelema`，`ldobj`，`mkrefany`，`newarr`，`refanyval`，`sizeof`，`stobj`，`box`，和 `unbox`。

> 元数据验证规则

 1. _TypeSpec_ 表可以包含零行或多行。
 2. _Signature_ 应该索引 ***Blob Heap*** 中的一个有效的类型规范。[ERROR]
 3. 基于 _Signature_，不应该有重复的行。[ERROR]
    

>---
### 20.22. ImplMap: 0x1C
<a id="ImplMap_0x1C"></a>

| *Token* |      |      |      |
| :------ | :--- | :--- | :--- |

_ImplMap_ 表保存了关于可以从托管代码通过 PInvoke 调度访问的非托管方法的信息。_ImplMap_ 表的每一行将 _MemberForwarded_(索引 _MethodDef_ 表中的一行) 与 _ImportScope_ 索引的 (**`ModuleRef`** 的) 某个非托管 DLL  中的例程 (_ImportName_) 的名称关联起来。

典型的例子是：将存储在 _Method_ 表的第 N 行的托管方法 (所以 _MemberForwarded_ 将有值 N) 与 DLL "`kernel32`" 中名为 "`GetEnvironmentVariable`" 的例程 (由 _ImportName_ 索引的字符串) 关联起来 (_ImportScope_ 索引的 **`ModuleRef`** 表中的字符串)。CLI 拦截对托管方法编号 N 的调用，并将它们转发为对 "`kernel32.dll`" 中名为 "`GetEnvironmentVariable`" 的非托管例程的调用 (包括根据需要封送任何参数)。

CLI 不支持此机制来访问从 DLL 导出的字段，只支持方法。

> _ImplMap_ 表有以下列：

 * **_MappingFlags_**：一个 2 字节的位掩码，类型为 _PInvokeAttributes_，[[↗]](#PInvokeAttributes)。

 * **_MemberForwarded_**：一个索引，指向 _Field_ 或 _MethodDef_ 表；更准确地说，是一个 _MemberForwarded_ [[↗]](#physical-stream) 编码索引。然而，它只会索引 _MethodDef_ 表，因为不支持 _Field_ 导出。

 * **_ImportName_**：一个指向 ***String Heap*** 的索引。

 * **_ImportScope_**：一个指向 **`ModuleRef`** 表的索引。

对于每个定义了一个指定 _MappingFlags_、_ImportName_ 和 _ImportScope_ 的 **.pinvokeimpl** 互操作特性的父方法 [[↗]](#unmanaged-method)，都会在 _ImplMap_ 表中生成一行。

> 元数据验证规则

 1. _ImplMap_ 可以包含零行或多行。
 2. _MappingFlags_ 只应设置那些指定的值。[ERROR]
 3. _MemberForwarded_ 应该索引 _MethodDef_ 表中的有效行。[ERROR]
 4. 在 _MethodDef_ 表中由 _MemberForwarded_ 索引的行中的 _MappingFlags_.*`CharSetMask`* [[↗]](#PInvokeAttributes) 应该最多设置以下位之一： 
*`CharSetAnsi`*、*`CharSetUnicode`* 或 *`CharSetAuto`* (如果没有设置，默认为 *`CharSetNotSpec`*)。[ERROR]
 1. _ImportName_ 应该索引 ***String Heap*** 中的 no-empty 字符串。[ERROR]
 2. _ImportScope_ 应该索引 **`ModuleRef`** 表中的有效行。[ERROR]
 3. 由 _MemberForwarded_ 在 _MethodDef_ 表中索引的行应该有其 _Flags_.*`PinvokeImpl`* = 1，并且 _Flags_.*`Static`* = 1。[ERROR]


>---
### 20.18. FieldRVA: 0x1D
<a id="FieldRVA_0x1D"></a>

| *Token* | _Field_ | _RVA_ |
| :------ | :------ | :---- |

> _FieldRVA_ 表有以下列：

 * **_RVA_**：或 *FieldOffset*，4 字节常量。

 - **_Field_**：_Field_ 表的索引。 

从概念上讲，_FieldRVA_ 表中的每一行都是 _Field_ 表中的确切一行的扩展，并记录了此字段的初始值存储在图像文件中的 RVA (相对虚拟地址)。

对于每个指定了可选的 **data** 标签的静态父字段，都会创建 _FieldRVA_ 表中的一行 (参见 [_字段定义和字段引用_](#field))。RVA 列是 PE 文件中数据的相对虚拟地址 (参见 [_在 PE 文件中嵌入数据_](#data))。

> 元数据验证规则

 1. _RVA_ 应为非零。[ERROR]
 2. _RVA_ 应指向当前模块的数据区域 (而不是其元数据区域)。[ERROR]
 3. _Field_ 应索引 _Field_ 表中的有效行。[ERROR]
 4. 任何具有 RVA 的字段应为 ValueType (而不是类或接口)。此外，它不应有任何私有字段 (同样适用于其自身为 ValueType 的任何字段)。如果违反了这些条件，代码可以覆盖该全局静态并访问其私有字段。此外，该 ValueType 的任何字段都不能是对象引用 (进入 GC 堆)。[ERROR]
 5. 只要两个基于 RVA 的字段符合前面的条件，两个 ValueType 跨越的内存范围就可以重叠，没有进一步的约束。这实际上不是一个额外的规则；它只是澄清了关于重叠的基于 RVA 的字段的位置

>---
### 20.2. Assembly: 0x20
<a id="Assembly_0x20"></a>

| *Token* | _HashAlgId_ | _Flags_ | _Version_ | _PublicKey_ | ***`Name`*** | _Culture_ |
| :------ | :---------- | :------ | :-------- | :---------- | :----------- | :-------- |

> _Assembly_ 表有以下列：

 * **_HashAlgId_**：或 *HashAlgorithm*，类型为 _AssemblyHashAlgorithm_ 的 4 字节常量，参见 [_AssemblyHashAlgorithm_](#AssemblyHashAlgorithm)。 
 
 - **_MajorVersion_**，**_MinorVersion_**，**_BuildNumber_**，**_RevisionNumber_**：或 _Version_，每个都是 2 字节常量。

 * **_Flags_**：类型为 _AssemblyFlags_ 的 4 字节位掩码，参见 [_AssemblyFlags_](#AssemblyFlags)。

 - **_PublicKey_**：一个 ***Blob Heap*** 的索引。

 * *****`Name`*****：一个 ***String Heap*** 的索引。

 - **_Culture_**：一个 ***String Heap*** 的索引。 

_Assembly_ 表使用 **.assembly** 指令定义 (参见 [_程序集定义_](#assembly) )；其列从相应的 **.hash** 算法，**.ver**，**.publickey** 和 **.culture** 中获取 [[↗]](#AsmDecl)。

> 元数据验证规则

 1. _Assembly_ 表应包含零行或一行。[ERROR]
 2. _HashAlgId_ 应为指定的值之一。[ERROR]
 3. _MajorVersion_，_MinorVersion_，_BuildNumber_ 和 _RevisionNumber_ 可以有任何值。
 4. _Flags_ 只应设置指定的值。[ERROR]
 5. _PublicKey_ 可以为 null 或非 null。
 6. ***`Name`*** 应索引 ***String Heap*** 中的 non-empty 字符串。[ERROR]
 7. ***`Name`*** 索引的字符串可以是无限长度。
 8. _Culture_ 可以为 null 或非 null。
 9. 如果 _Culture_ 为非 null，它应索引指定列表中的单个字符串 [[↗]](#Culture-values)。[ERROR]

***`Name`*** 是一个简单的名称 (例如，“Foo”，没有驱动器字母，没有路径，没有文件扩展名)；在符合 POSIX 的系统上，Name 不包含冒号，不包含正斜杠，不包含反斜杠，也不包含句点。

>---
### 20.4. AssemblyProcessor: 0x21
<a id="AssemblyProcessor_0x21"></a>

| *Token* | _Processor_ |
| :------ | :---------- |

> _AssemblyProcessor_ 表有以下列：

 * **_Processor_**：一个 4 字节常数。

此记录不应被发出到任何 PE 文件中。然而，如果它出现在 PE 文件中，应该将其字段视为零。CLI 应该忽略它。


>---
### 20.3. AssemblyOS: 0x22
<a id="AssemblyOS_0x22"></a>

| *Token* | _OSPlatformID_ | _OSMajorVersion_ | _OSMinorVersion_ |
| :------ | :------------- | :--------------- | :--------------- |

> _AssemblyOS_ 表有以下列：

 * **_OSPlatformID_**：4 字节常数 

 - **_OSMajorVersion_**：4 字节常数 

 * **_OSMinorVersion_**：4 字节常数 

此记录不应被发出到任何 PE 文件中。然而，如果它出现在 PE 文件中，它应被视为所有字段都为零。CLI 将忽略它。

>---
### 20.5. AssemblyRef: 0x23
<a id="AssemblyRef_0x23"></a>


| *Token* | *Version* | _Flags_ | _PublicKeyOrToken_ | ***`Name`*** | _Culture_ | _HashValue_ |
| :------ | :-------- | :------ | :----------------- | :----------- | :-------- | :---------- |

> **`AssemblyRef`** 表有以下列：

 * **_MajorVersion_**，**_MinorVersion_**，**_BuildNumber_**，**_RevisionNumber_**：或 *Version*，每个都是 2 字节常量。 

 - **_Flags_**：类型为 _AssemblyFlags_ 的 4 字节位掩码，参见 [_AssemblyFlags_](#AssemblyFlags)。

 * **_PublicKeyOrToken_**：一个 ***Blob Heap*** 的索引，表示标识此 Assembly 的发起者的公钥或 **_token_**。 

 - *****`Name`*****：***String Heap*** 的索引。

 * **_Culture_**：一个 ***String Heap*** 的索引。 

 - **_HashValue_**：一个 ***Blob Heap*** 的索引。 

该表由 **.assembly extern** 指令定义 [[↗]](#assembly-extern)。其列使用与 _Assembly_ 表类似的指令填充，除了 _PublicKeyOrToken_ 列，该列使用 **.publickeytoken** 指令定义。

> 元数据验证规则

 1. _MajorVersion_，_MinorVersion_，_BuildNumber_ 和 _RevisionNumber_ 可以有任何值。
 2. _Flags_ 只应设置一个位，即 *`PublicKey`* 位 [[↗]](#AssemblyFlags)。所有其他位应为零。[ERROR]
 3. _PublicKeyOrToken_ 可以为 null 或非 null (注意 _Flags_.*`PublicKey`* 位指定 '*blob*' 是完整的公钥还是短哈希 **_token_**)。
 4. 如果非 null，则 _PublicKeyOrToken_ 应索引 ***Blob Heap*** 中的有效偏移。[ERROR]
 5. ***`Name`*** 应索引 ***String Heap*** 中的 non-empty 字符串 (其长度没有限制)。[ERROR]
 6. _Culture_ 可以为 null 或非 null。
 7. 如果非 null，它应索引指定列表中的单个字符串 [[↗]](#Culture-values)。[ERROR]
 8. _HashValue_ 可以为 null 或非 null。
 9. 如果非 null，则 _HashValue_ 应索引 ***Blob Heap*** 中的非空 '*blob*'。[ERROR]
 10. **`AssemblyRef`** 表不应包含重复项 (其中重复行被视为具有相同的 _MajorVersion_，_MinorVersion_，_BuildNumber_，_RevisionNumber_，_PublicKeyOrToken_，***`Name`*** 和 _Culture_ 的行)。[WARNING]

***`Name`*** 是一个简单的名称 (例如，“Foo”，没有驱动器字母，没有路径，没有文件扩展名) ；在符合 POSIX 的系统上，Name 不包含冒号，不包含正斜杠，不包含反斜杠，也不包含句点。

>---
### 20.7. AssemblyRefProcessor: 0x24
<a id="AssemblyRefProcessor_0x24"></a>

| *Token* | _Processor_ | **`AssemblyRef`** |
| :------ | :---------- | :---------------- |

> _AssemblyRefProcessor_ 表有以下列：

 * **_Processor_**：一个 4 字节常数。 

 - ****`AssemblyRef`****：一个索引，指向 **`AssemblyRef`** 表。

这些记录不应被发出到任何 PE 文件中。然而，如果它们出现在 PE 文件中，应该将其字段视为零。CLI 应该忽略它们。

>---
### 20.6. AssemblyRefOS: 0x25
<a id="AssemblyRefOS_0x25"></a>

| *Token* | _OSPlatformId_ | _OSMajorVersion_ | _OSMinorVersion_ | **`AssemblyRef`** |
| :------ | :------------- | :--------------- | :--------------- | :---------------- |

> _AssemblyRefOS_ 表有以下列：

 * **_OSPlatformId_**：4 字节常数。 

 - **_OSMajorVersion_**：4 字节常数。 

 * **_OSMinorVersion_**：4 字节常数。

 + ****`AssemblyRef`****：索引到 **`AssemblyRef`** 表。 

这些记录不应被发出到任何 PE 文件中。然而，如果它们出现在 PE 文件中，它们应被视为其字段都为零。CLI 应忽略它们。

>---
### 20.19. File: 0x26
<a id="File_0x26"></a>

| *Token* | _Flags_ | ***`Name`*** | _HashValue_ |
| :------ | :------ | :----------- | :---------- |

> _File_ 表有以下列：

 * **_Flags_**：一个 4 字节的位掩码，类型为 _FileAttributes_ [[↗]](#FileAttributes)。

 - *****`Name`*****：一个指向 ***String Heap*** 的索引。

 * **_HashValue_**：一个指向 ***Blob Heap*** 的索引。

_File_ 表的行是程序集中的 **.file** 指令的结果 (§[II.6.2.3](ii.6.2.3-associating-files-with-an-assembly.md)) 

> 元数据验证规则

 1. _Flags_ 只应设置那些指定的值 (所有组合有效)。[ERROR]
 2. ***`Name`*** 应该索引 ***String Heap*** 中的 non-empty 字符串。它应该是 `<filename>.<extension>` 的格式 (例如，"`foo.dll`"，但不是 "`c:\utils\foo.dll`")。[ERROR]
 3. _HashValue_ 应该索引 ***Blob Heap*** 中的非空 '*blob*'。[ERROR]
 4. 不应该有重复的具有相同 ***`Name`*** 值的行。[ERROR]
 5. 如果此模块包含 _Assembly_ 表中的一行 (也就是说，如果此模块 “持有清单”)，那么 _File_ 表中不应该有任何关于此模块的行；也就是说，没有自引用。[ERROR]
 6. 如果 _File_ 表为空，那么按定义，这是一个单文件程序集。在这种情况下，_ExportedType_ 表应该为空。[WARNING]

>---
### 20.14. ExportedType: 0x27
<a id="ExportedType_0x27"></a>

| *Token* | _Flags_ | _TypeDefId_ | _TypeName_ | _TypeNamespace_ | _Implementation_ |
| :------ | :------ | :---------- | :--------- | :-------------- | :--------------- |

_ExportedType_ 表为每种类型保存一行：

 1. 在此程序集的其他模块中定义；也就是说，从此程序集中导出。本质上，它存储了此程序集包含的其他模块中所有标记为公共的类型的 _TypeDef_ 行号。
    
    实际的目标行在 _TypeDef_ 表中由 _TypeDefId_ (实际上是行号) 和 _Implementation_ (实际上是持有目标 _TypeDef_ 表的模块) 的组合给出。注意，这是元数据中 *foreign*  **_token_** 的唯一出现；也就是说， **_token_** 值在另一个模块中有意义。常规 **_token_** 值是对 *current* 模块中的表的索引；或者

 2. 最初在此程序集中定义，但现在已移至另一个程序集。_Flags_ 必须设置 *`IsTypeForwarder`*，并且 _Implementation_ 是一个 **`AssemblyRef`**，表示现在可以在另一个程序集中找到该类型。

类型的全名不需要直接存储。相反，它可以在任何包含的 "." 处分成两部分 (尽管通常这是在全名中的最后一个 "." 处完成的)。"." 前面的部分存储为 _TypeNamespace_，"." 后面的部分存储为 _TypeName_。如果全名中没有 "."，那么 _TypeNamespace_ 应该是空字符串的索引。

> _ExportedType_ 表有以下列：

 * **_Flags_**：一个 4 字节的位掩码，类型为 _TypeAttributes_ [[↗]](#TypeAttributes)。

 * **_TypeDefId_**：一个 4 字节的索引，指向此程序集的另一个模块中的 _TypeDef_ 表。此列仅用作提示。如果目标 _TypeDef_ 表中的条目与此表中的 _TypeName_ 和 _TypeNamespace_ 条目匹配，则解析成功。但是，如果不匹配，CLI 将回退到目标 _TypeDef_ 表的搜索。如果 _Flags_ 设置了 *`IsTypeForwarder`*，则忽略并应为零。

 * **_TypeName_**：一个指向 ***String Heap*** 的索引。

 * **_TypeNamespace_**：一个指向 ***String Heap*** 的索引。

 * **_Implementation_**：这是一个指向 (更准确地说，是一个 _Implementation_ [[↗]](#physical-stream) 编码索引) 以下表中的任何一个的索引：
     * _File_ 表，该条目说明当前程序集中的哪个模块持有 _TypeDef_。
     * _ExportedType_ 表，该条目是当前嵌套类型的封闭类型。
     * **`AssemblyRef`** 表，该条目说明在哪个程序集中现在可以找到类型 (_Flags_ 必须设置 *`IsTypeForwarder`* 标志)。

_ExportedType_ 表中的行是 **.class extern** 指令的结果 [[↗]](#class-extern)。

> 元数据验证规则

术语 "_FullName_" 指的是以下方式创建的字符串：如果 _TypeNamespace_ 为空，则使用 _TypeName_，否则使用 _TypeNamespace_"."_TypeName_ 的连接。

1. _ExportedType_ 表可以包含零行或多行。
2. _ExportedType_ 表中不应该有在当前模块中定义的类型的条目 —— 只有在程序集中的其他模块中定义的类型。[ERROR]
3. _Flags_ 只应设置那些指定的值。[ERROR]
4. 如果 _Implementation_ 索引 _File_ 表，那么 _Flags_.*`VisibilityMask`* 应该是 *`Public`* [[↗]](#TypeAttributes)。[ERROR]
5. 如果 _Implementation_ 索引 _ExportedType_ 表，那么 _Flags_.*`VisibilityMask`* 应该是 *`NestedPublic`* [[↗]](#TypeAttributes)。[ERROR]
6. 如果非空，_TypeDefId_ 应该索引此程序集中的某个模块 (但不是此模块) 中的 _TypeDef_ 表中的有效行，且所索引的行应该有其 _Flags_.*`Public`* = 1 [[↗]](#TypeAttributes)。[WARNING]
7. _TypeName_ 应该索引 ***String Heap*** 中的 non-empty 字符串。[ERROR]
8. _TypeNamespace_ 可以为空，或非空。
9. 如果 _TypeNamespace_ 是非空的，那么它应该索引 ***String Heap*** 中的 non-empty 字符串。[ERROR]
10. _FullName_ 应该是一个有效的 CLS 标识符。[CLS]
11. 如果这是一个嵌套类型，那么 _TypeNamespace_ 应该是空的，_TypeName_ 应该表示嵌套类型的未混淆的简单名称。[ERROR]
12. _Implementation_ 应该是一个有效的索引，指向以下任何一个：[ERROR]
     * _File_ 表；该文件应该在其 _TypeDef_ 表中持有目标类型的定义。
     * 当前 _ExportedType_ 表中的不同行 —— 这标识了当前嵌套类型的封闭类型。
13. _FullName_ 应该与 _TypeDefId_ 索引的 _TypeDef_ 表中的行的相应 _FullName_ 完全匹配。[ERROR]
14. 忽略嵌套类型时，基于 _FullName_ 不应该有重复的行。[ERROR]
15. 对于嵌套类型，基于 _TypeName_ 和封闭类型不应该有重复的行。[ERROR]
16. 从当前程序集导出的类型的完整列表是 _ExportedType_ 表与当前 _TypeDef_ 表中所有公共类型的连接，其中 "public" 指的是 _Flags_.*`VisibilityMask`* 是 *`Public`* 或 *`NestedPublic`*。在这个连接表中，基于 _FullName_ (如果这是一个嵌套类型，将封闭类型添加到重复检查中) 不应该有重复的行。[ERROR]

>---
### 20.24. ManifestResource: 0x28
<a id="ManifestResource_0x28"></a>

| *Token* |      |      |      |
| :------ | :--- | :--- | :--- |

> _ManifestResource_ 表有以下列：

 * **_Offset_**：一个 4 字节的常数。 

 - **_Flags_**：一个 4 字节的位掩码，类型为 _ManifestResourceAttributes_ [[↗]](#ManifestResourceAttributes)。

 * *****`Name`*****：一个指向 ***String Heap*** 的索引。 

 - **_Implementation_**：一个指向 _File_ 表、**`AssemblyRef`** 表或 null 的索引；更准确地说，是一个 _Implementation_ [[↗]](#physical-stream) 编码索引。

_Offset_ 指定此资源记录开始的引用文件内的字节偏移量。_Implementation_ 指定哪个文件持有此资源。

表中的行是程序集上的 **.mresource** 指令的结果 [[↗]](#mresource)。

> 元数据验证规则

 1. _ManifestResource_ 表可以包含零行或多行。
 2. _Offset_ 应该是目标文件中的有效偏移量，从 CLI 头部的资源条目开始。[ERROR]
 3. _Flags_ 只应设置那些指定的值。[ERROR]
 4. _Flags_ 的 *`VisibilityMask`* 子字段 [[↗]](#ManifestResourceAttributes) 应该是 `Public` 或 `Private` 中的一个。[ERROR]
 5. ***`Name`*** 应该索引 ***String Heap*** 中的 non-empty 字符串。[ERROR]
 6. _Implementation_ 可以为空或非空 (如果为空，表示资源存储在当前文件中)。 
 7. 如果 _Implementation_ 为空，那么 _Offset_ 应该是当前文件中的有效偏移量，从 CLI 头部的资源条目开始。[ERROR]
 8. 如果 _Implementation_ 非空，那么它应该索引 _File_ 或 **`AssemblyRef`** 表中的有效行。[ERROR]
 9. 基于 ***`Name`*** 不应该有重复的行。[ERROR]
 10. 如果资源是 _File_ 表中的索引，_Offset_ 应该为零。[ERROR]

>---
### 20.32. NestedClass: 0x29
<a id="NestedClass_0x29"></a>

| *Token* | _NestedClass_ | _EnclosingClass_ |
| :------ | :------------ | :--------------- |

> _NestedClass_ 表有以下列：

 * **_NestedClass_**：_TypeDef_ 表的索引。

 - **_EnclosingClass_**：_TypeDef_ 表的索引。

_NestedClass_ 被定义为在其封闭类型的文本 “内部”。_NestedClass_ 表记录哪些类型定义嵌套在哪些其他类型定义中。在典型的高级语言中，嵌套类被定义为在其封闭类型的文本  “内部”。

> 元数据验证规则

 1. _NestedClass_ 表可以包含零行或多行。
 2. _NestedClass_ 应索引 _TypeDef_ 表中的有效行。[ERROR]
 3. _EnclosingClass_ 应索引 _TypeDef_ 表中的有效行 (特别注意，不允许索引 **`TypeRef`** 表)。[ERROR]
 4. 不应有重复行 (即 _NestedClass_ 和 _EnclosingClass_ 的值相同)。[WARNING]
 5. 给定类型只能由一个封闭器嵌套。因此，不能有两行具有相同的 _NestedClass_ 值，但它们的 _EnclosingClass_ 值不同。[ERROR]
 6. 给定类型可以 “拥有” 几种不同的嵌套类型，因此具有两行或多行具有相同的 _EnclosingClass_ 值，但 _NestedClass_ 值不同的情况是完全有效的。

>---
### 20.20. GenericParam: 0x2A
<a id="GenericParam_0x2A"></a>

| *Token* | _Number_ | _Flags_ | _Owner_ | ***`Name`*** |
| :------ | :------- | :------ | :------ | :----------- |

> _GenericParam_ 表有以下列：

 * **_Number_**：泛型参数的 2 字节索引，从左到右编号，从零开始。

 - **_Flags_**：或 *Attributes*，一个 2 字节的位掩码，类型为 _GenericParamAttributes_ [[↗]](#GenericParamAttributes)。

 * **_Owner_**：一个索引，指向 _TypeDef_ 或 _MethodDef_ 表，指定此泛型参数适用的类型或方法；更准确地说，是一个 _TypeOrMethodDef_ [[↗]](#physical-stream) 编码索引。

 - *****`Name`*****：一个非空索引，指向 ***String Heap***，给出泛型参数的名称。这完全是描述性的，只由源语言编译器和反射使用。

以下是其他的限制：

 * _Owner_ 不能是非嵌套的枚举类型；并且
 * 如果 _Owner_ 是嵌套的枚举类型，那么 _Number_ 必须小于或等于封闭类的泛型参数的数量。

泛型枚举类型的作用很小，通常只存在于满足 CLS Rule 42。这些额外的限制约束了枚举类型的通用性，同时允许满足 CLS Rule 42。_GenericParam_ 表存储了在泛型类型定义和泛型方法定义中使用的泛型参数。这些泛型参数可以被约束 (即，泛型参数应扩展某个类和 / 或实现某些接口) 或无约束。这样的约束存储在 _GenericParamConstraint_ 表中。 

从概念上讲，_GenericParam_ 表中的每一行都属于 _TypeDef_ 或 _MethodDef_ 表中的一行，且只有一行拥有。

 ```cil
 .class Dict`2<([mscorlib]System.IComparable) K, V>
 ```

类 `Dict` 的泛型参数 `K` 被约束为实现 `System.IComparable`。

 ```cil
 .method static void ReverseArray<T>(!!0[] 'array')
 ```

泛型方法 `ReverseArray` 的泛型参数 `T` 没有约束。 

> 元数据验证规则

 1. _GenericParam_ 表可以包含零行或多行。
 2. 每一行应有一个，且只有一个，在 _TypeDef_ 或 _MethodDef_ 表中的 *owner* 行 (即，没有行共享)。[ERROR]
 3. 每个泛型类型应在 _GenericParam_ 表中为其每个泛型参数拥有一行。[ERROR]
 4. 每个泛型方法应在 _GenericParam_ 表中为其每个泛型参数拥有一行。[ERROR]

_Flags_：

 5. 可以持有 *`Covariant`* 或 *`Contravariant`* 的值，但只有当 *owner* 行对应于泛型接口或泛型委托类时才能这样做。[ERROR]
 6. 否则，应持有 *`None`* 值，表示非变量 (即，参数是非变量或 *owner* 是非委托类、值类型或泛型方法)。[ERROR]
 7. 如果 _Flags_ == *`Covariant`*，那么相应的泛型参数只能作为以下内容出现在类型定义中：[ERROR]
     * 方法的结果类型
     * 继承接口的泛型参数
 8. 如果 _Flags_ == *`Contravariant`*，那么相应的泛型参数只能作为方法的参数出现在类型定义中。[ERROR]
 9. _Number_ 应有一个值 &ge; 0 且 <  *owner* 类型或方法的泛型参数的数量。[ERROR]
 10. 同一方法拥有的 _GenericParam_ 表中的连续行应按照 _Number_ 值的增加顺序排序；_Number_ 序列中不应有间隙。[ERROR]
 11. ***`Name`*** 是索引 ***String Heap*** 中的 no-empty 字符串[ERROR]
 12. 基于 _Owner_+***`Name`***，不应有重复的行 。[ERROR]
 13. 基于 _Owner_+_Number_，不应有重复的行。[ERROR]

>---
### 20.29. MethodSpec: 0x2B
<a id="MethodSpec0x2B"></a>

| *Token* | _Method_ |      |      |
| :------ | :------- | :--- | :--- |

> _MethodSpec_ 表有以下列：

 * **_Method_**：一个指向 _MethodDef_ 或 _MemberRef_ 表的索引，指定此行引用的泛型方法；也就是说，此行是哪个泛型方法的实例；更准确地说，是 _MethodDefOrRef_ [[↗]](#physical-stream) 编码索引。

 - **_Instantiation_**：或 _Signature_，一个指向 ***Blob Heap*** 的索引 [[↗]](#MethodSpec-blob)。
 
_MethodSpec_ 表记录实例化泛型方法的签名。泛型方法的每个唯一实例 (即，_Method_ 和 _Instantiation_ 的组合) 应由表中的单个行表示。

> 元数据验证规则

 1. _MethodSpec_ 表可以包含零行或多行。
 2. 一个或多个行可以引用 _MethodDef_ 或 _MemberRef_ 表中的相同行。可以有同一泛型方法的多个实例。 
 3. 存储在 _Instantiation_ 中的签名应为 _Method_ 存储的泛型方法的签名的有效实例。[ERROR]
 4. 基于 _Method_+_Instantiation_ 不应有重复行。[ERROR]

>---
### 20.21. GenericParamConstraint: 0x2C
<a id="GenericParamConstraint_0x2C"></a>

| *Token* |      |      |      |
| :------ | :--- | :--- | :--- |

> _GenericParamConstraint_ 表有以下列：

 * **_Owner_**：一个指向 _GenericParam_ 表的索引，指定此行引用的泛型参数。

 - **_Constraint_**：一个指向 _TypeDef_，**`TypeRef`** 或 _TypeSpec_ 表的索引，指定此泛型参数受限于应从哪个类派生；或此泛型参数受限于实现哪个接口；更准确地说，是 _TypeDefOrRef_ [[↗]](#physical-stream) 编码索引。

_GenericParamConstraint_ 表记录每个泛型参数的约束。每个泛型参数可以约束为从零个或一个类派生。每个泛型参数可以约束为实现零个或多个接口。

从概念上讲，_GenericParamConstraint_ 表中的每一行都由 _GenericParam_ 表中的一行 “拥有”。给定 _Owner_ 的 _GenericParamConstraint_ 表中的所有行应引用不同的约束。

请注意，如果 _Constraint_ 是对 `System.ValueType` 的 **`TypeRef`**，那么它意味着约束类型应为 `System.ValueType`，或其子类型之一。然而，由于 `System.ValueType` 本身是引用类型，这种特定机制并不能保证类型是非引用类型。


> 元数据验证规则

 1. _GenericParamConstraint_ 表可以包含零行或多行。
 2. 每一行在 _GenericParam_ 表中都应有一个且只有一个 *owner* 行 (即，没有行共享)  。[ERROR]
 3. _GenericParam_ 表中的每一行应 “拥有” _GenericParamConstraint_ 表中的一个单独行，对应于该泛型参数具有的每个约束。[ERROR]
 4. 在 _GenericParam_ 表中的给定行拥有的 _GenericParamConstraint_ 表中的所有行应形成一个连续的范围 (行)。[ERROR]
 5. 任何泛型参数 (对应于 _GenericParam_ 表中的一行) 应拥有 _GenericParamConstraint_ 表中的零行或一行，对应于类约束。[ERROR]
 6. 任何泛型参数 (对应于 _GenericParam_ 表中的一行) 应拥有 _GenericParamConstraint_ 表中的零行或多行，对应于接口约束。[ERROR]
 7. 基于 _Owner_+_Constraint_ 不应有重复行。[ERROR]
 8. 约束不应引用 `System.Void`。[ERROR]

---