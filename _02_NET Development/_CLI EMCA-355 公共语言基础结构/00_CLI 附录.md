# CLI：附录

---
## 术语与定义

- **ANSI character**：ANSI 字符，来自实现定义的 8 位字符集的字符，其前 128 个码位与 ISO/IEC-10646 的码位完全对应。
- **ANSI string**：由 ANSI 字符组成的字符串，最后一个字符的值为 *all-bits-zero*。
- **argument**：实参，在调用方法时为形参提供的表达式。
- **assembly**：程序集，一组配置好的可加载代码模块和其他资源，它们共同实现一个功能单元。
- **attribute**：特性，类型与其成员包含描述性信息的特征。常见的特性是预定义的，并且在与它们关联的元数据中具有特定的编码。用户定义的特性也可以添加到元数据中。
- **behavior, implementation-specific**：特定于实现的行为，需要在语言规范中具体记录基于实现的定义行为。
- **behavior, unspecific**：未指定行为，对于格式良好的程序结构和正确的数据，取决于实现的行为。实现的定义行为不需要记录在它的语言规范中。
- **behavior, undefined**：未定义行为，在使用错误的程序结构或错误的数据时可能出现的行为。
- **boxing**：装箱，将具有某种值类型的值转换为引用类型 `System.object` 的新实例。
- **Common Intermediate Language (CIL)**：通用中间语言，VES 能够理解的指令集。
- **Common Language Infrastructure (CLI)**：公共语言基础，可执行代码格式的规范，以及可以执行该代码的运行环境。
- **Common Language Specification (CLS)**：公共语言规范，语言设计者和框架（类库）设计者之间的协议，它指定了 CTS 的一个子集和一组使用协定。
- **Common Type System (CTS)**：公共类型系统，由编译器、工具和 CLI 本身共享的统一类型系统。该模型定义了 CLI 在声明、使用和管理类型时遵循的规则。CTS 建立就了一个支持跨语言集成、类型安全和高性能代码执行的框架。
- **delegate**：委托是一种引用类型，它的实例可以在调用列表中封装一个或多个方法。给定一个委托实例和一组适当的参数，就可以用这组参数调用委托调用列表中的所有方法
- **event**: 事件，使对象或类能够提供通知的成员。
- **Execution Engine**：执行引擎，虚拟执行系统。
- **field**：字段，指定在程序中存储某些数据的类型化内存位置的成员。
- **garbage collection**：垃圾收集，为托管数据分配和释放内存的过程。
- **generic argument**：泛型实参，用于实例化特定泛型类型或泛型方法的实际类型。例如，在 `List<string>` 中，`string` 是泛型类型定义 `List<T>` 中的泛型形参对应的泛型实参。
- **generic parameter**：泛型参数，泛型类型或泛型方法定义中的参数，用作泛型实参的占位符。例如，在泛型类型定义 `List<T>` 中，`T` 是一个泛型参数。
- **generic**：泛型，允许定义类型和方法的特性，使它们可以用一个或多个泛型参数进行参数化。
- **library**：类库，一组类型的存储库，这些类型被分组到一个或多个程序集中。库还可以包含对其他库中定义的类型的修改。例如，库可以包含其他库中定义的类型的附加方法、接口和异常。
- **managed code**：托管代码，包含足够信息以允许 CLI 提供一组核心服务的代码。例如，给定代码中某个方法的地址，CLI 必须能够定位描述该方法的元数据。它还必须能够遍历堆栈、处理异常以及存储和检索安全信息。
- **managed data**：托管数据，由 CLI 通过称为垃圾收集的进程自动分配和释放的数据。
- **manifest**：信息清单，程序集的一部分，指定有关该程序集的版本、名称、区域性和安全要求等信息；指示哪些其他文件（如果有）属于该程序集，以及每个文件的加密散列；指示在该程序集的其他文件中定义的哪些类型将从该程序集导出；以及（可选的）清单本身的数字签名和用于计算它的公钥。
- **member**：成员，一个类型的任何字段、数组元素、方法、属性和事件。
- **metadata** 元数据，描述和引用 CTS 定义的类型的数据。元数据以一种独立于任何特定编程语言的方式存储。因此，元数据为操作程序的工具（如编译器和调试器）之间以及这些工具和 VES 之间提供了一种通用的交换机制。
- **method**：方法，描述可对精确类型的值执行的操作的成员。
- **method, generic**：泛型方法，在类型中定义的方法（静态、实例或虚拟），其签名包括一个或多个泛型参数，而不是在类型定义本身中出现。封闭类型本身可能是泛型的，也可能不是。例如，在泛型类型 `List<T>` 中，泛型方法 `ConvertTo<s>()` 是泛型的。
- **method, non-generic**：非泛型的方法。
- **module**：模块，包含可由 VES 执行的内容的单个文件。
- **object**：对象，引用类型的实例。对象不仅仅是一个值。对象是自类型化的；它的类型显式地存储在它的表示中。它有一个区别于所有其他对象的标识，并且它有存储其他实体（可以是对象或值）的槽。虽然可以更改其槽的内容，但对象的标识永远不会更改。
- **parameter**：形式参数，在方法的头部和主体中使用的名称，用于引用在调用点提供的实参值。
- **profile**：一组库，组合在一起形成一个一致的整体，提供一个固定的级别。
- **property**：属性，定义命名值和访问该值的方法的成员。属性定义了对该值的访问契约。因此，属性定义指定存在哪些访问方法以及它们各自的方法契约。
- **signature**：签名，可以被检查和自动执行的部分。签名是通过向类型和其他签名添加约束而形成的。约束是对值或位置的使用或允许的操作的限制。
- **type, generic**：泛型类型，其定义由一个或多个其他类型参数化的类型。例如，`List<T>`，其中 `T` 是泛型参数。CLI 支持创建和使用泛型类型的实例。例如，`List<int32>` 或 `List<string>`。
- **type, reference**：引用类型，它的实例包含对其数据的引用。
- **type, value**：值类型，它的实例直接包含它的所有数据。
- **unboxing**：拆箱，对具有 `system.object` 类型的值进行的转换。其运行时类型为值类型，转换为值类型实例。
- **unmanaged code**：未托管代码。
- **unmanaged data**：未托管数据。
- **value**：整数或浮点数的简单位模式。每个值都有一个类型，该类型既描述了它所占用的存储空间，也描述了它的表示形式中 bits 的含义，还描述了在该表示形式上可以执行的操作。值用于表示编程语言中的简单类型和非对象。
- **verification**：验证，检查 CIL 及其相关元数据，以确保 CIL 代码序列不允许访问程序逻辑地址空间以外的内存。与验证测试一起，验证可确保程序不能访问未授予其访问权限的内存或其他资源。
- **Virtual Execution System (VES)**：虚拟执行系统，该系统实现并执行 CTS 模型。VES 负责加载和运行 CLI 编写的程序。它提供了执行托管代码和数据所需的服务，使用元数据在运行时将单独生成的模块连接在一起。VES 也被称为执行引擎。

---
## CLS Rules

这里收集了完整的 CLS 规则集以供参考。这些规则只适用于在程序集 “外部可见” 的项目类型，以及那些具有 `public`、`family` 或 `family-or-assembly` 可访问性的类型的成员。此外，可以使用 `System.CLSCompliantAttribute` 显式地将项目标记为 CLS 兼容或不兼容。CLS 规则仅适用于标记为 CLS 兼容的项目。

>---
### English Version

[**CLS Rule 1:**](01_CLI%20基本概念和体系结构.md#R1) CLS rules apply only to those parts of a type that are accessible or visible outside of the defining assembly. 

[**CLS Rule 2:**](01_CLI%20基本概念和体系结构.md#R2) Members of non-CLS compliant types shall not be marked CLS-compliant.

[**CLS Rule 3:**](01_CLI%20基本概念和体系结构.md#R3) Boxed value types are not CLS-compliant.

[**CLS Rule 4:**](01_CLI%20基本概念和体系结构.md#R4) Assemblies shall follow Annex 7 of Technical Report 15 of the Unicode Standard 3.0 governing the set of characters permitted to start and be included in identifiers, available on-line at http://www.unicode.org/unicode/reports/tr15/tr15-18.html. Identifiers shall be in the canonical format defined by Unicode Normalization Form C. For CLS purposes, two identifiers are the same if their lowercase mappings (as specified by the Unicode locale-insensitive, one-to-one lowercase mappings) are the same. That is, for two identifiers to be considered different under the CLS they shall differ in more than simply their case. However, in order to override an inherited definition the CLI requires the precise encoding of the original declaration be used.

[**CLS Rule 5:**](01_CLI%20基本概念和体系结构.md#R5) All names introduced in a CLS-compliant scope shall be distinct independent of kind, except where the names are identical and resolved via overloading. That is, while the CTS allows a single type to use the same name for a method and a field, the CLS does not.

[**CLS Rule 6:**](01_CLI%20基本概念和体系结构.md#R6) Fields and nested types shall be distinct by identifier comparison alone, even though the CTS allows distinct signatures to be distinguished.  Methods, properties, and events that have the same name (by identifier comparison) shall differ by more than just the return type, except as specified in CLS Rule 39. 

[**CLS Rule 7:**](01_CLI%20基本概念和体系结构.md#R7) The underlying type of an enum shall be a built-in CLS integer type, the name of the field shall be "value__", and that field shall be marked RTSpecialName.

[**CLS Rule 8:**](01_CLI%20基本概念和体系结构.md#R8) There are two distinct kinds of enums, indicated by the presence or absence of the `System.FlagsAttribute` (see [Partition IV Library] ) custom attribute. One represents named integer values; the other represents named bit flags that can be combined to generate an unnamed value.  The value of an enum is not limited to the specified values.

[**CLS Rule 9:**](01_CLI%20基本概念和体系结构.md#R9) Literal static fields of an enum shall have the type of the enum itself. 

[**CLS Rule 10:**](01_CLI%20基本概念和体系结构.md#R10) Accessibility shall not be changed when overriding inherited methods, except when overriding a method inherited from a different assembly with accessibility family-or-assembly.  In this case, the override shall have accessibility family. 

[**CLS Rule 11:**](01_CLI%20基本概念和体系结构.md#R11) All types appearing in a signature shall be CLS-compliant. All types composing an instantiated generic type shall be CLS-compliant.

[**CLS Rule 12:**](01_CLI%20基本概念和体系结构.md#R12) The visibility and accessibility of types and members shall be such that types in the signature of any member shall be visible and accessible whenever the member itself is visible and accessible. For example, a public method that is visible outside its assembly shall not have an argument whose type is visible only within the assembly. The visibility and accessibility of types composing an instantiated generic type used in the signature of any member shall be visible and accessible whenever the member itself is visible and accessible. For example, an instantiated generic type present in the signature of a member that is visible outside its assembly shall not have a generic argument whose type is visible only within the assembly.

[**CLS Rule 13:**](01_CLI%20基本概念和体系结构.md#R13) The value of a literal static is specified through the use of field initialization metadata. A CLS-compliant literal must have a value specified in field initialization metadata that is of exactly the same type as the literal (or of the underlying type, if that literal is an enum). 

[**CLS Rule 14:**](01_CLI%20基本概念和体系结构.md#R14) Typed references are not CLS-compliant. 

[**CLS Rule 15:**](01_CLI%20基本概念和体系结构.md#R15) The vararg constraint is not part of the CLS, and the only calling convention supported by the CLS is the standard managed calling convention.

[**CLS Rule 16:**](01_CLI%20基本概念和体系结构.md#R16) Arrays shall have elements with a CLS-compliant type, and all dimensions of the array shall have lower bounds of zero. Only the fact that an item is an array and the element type of the array shall be required to distinguish between overloads.  When overloading is based on two or more array types the element types shall be named types.

[**CLS Rule 17:**](01_CLI%20基本概念和体系结构.md#R17) Unmanaged pointer types are not CLS-compliant.

[**CLS Rule 18:**](01_CLI%20基本概念和体系结构.md#R18) CLS-compliant interfaces shall not require the definition of non-CLS compliant methods in order to implement them.

[**CLS Rule 19:**](01_CLI%20基本概念和体系结构.md#R19) CLS-compliant interfaces shall not define static methods, nor shall they define fields. 

[**CLS Rule 20:**](01_CLI%20基本概念和体系结构.md#R20) CLS-compliant classes, value types, and interfaces shall not require the implementation of non-CLS-compliant members.

[**CLS Rule 21:**](01_CLI%20基本概念和体系结构.md#R21) An object constructor shall call some instance constructor of its base class before any access occurs to inherited instance data. (This does not apply to value types, which need not have constructors.) 

[**CLS Rule 22:**](01_CLI%20基本概念和体系结构.md#R22) An object constructor shall not be called except as part of the creation of an object, and an object shall not be initialized twice. 

[**CLS Rule 23:**](01_CLI%20基本概念和体系结构.md#R23) `System.Object` is CLS-compliant. Any other CLS-compliant class shall inherit from a CLS-compliant class. 

[**CLS Rule 24:**](01_CLI%20基本概念和体系结构.md#R24) The methods that implement the getter and setter methods of a property shall be marked SpecialName in the metadata.

[**CLS Rule 25:**](01_CLI%20基本概念和体系结构.md#R25) No longer used. In an earlier version of this standard, this rule stated "The accessibility of a property and of its accessors shall be identical." The removal of this rule allows, for example, public access to a getter while restricting access to the setter. 

[**CLS Rule 26:**](01_CLI%20基本概念和体系结构.md#R26) A property’s accessors shall all be static, all be virtual, or all be instance. 

[**CLS Rule 27:**](01_CLI%20基本概念和体系结构.md#R27) The type of a property shall be the return type of the getter and the type of the last argument of the setter.  The types of the parameters of the property shall be the types of the parameters to the getter and the types of all but the final parameter of the setter.  All of these types shall be CLS-compliant, and shall not be managed pointers (i.e., shall not be passed by reference).

[**CLS Rule 28:**](01_CLI%20基本概念和体系结构.md#R28) Properties shall adhere to a specific naming pattern. The SpecialName attribute referred to in CLS rule 24 shall be ignored in appropriate name comparisons and shall adhere to identifier rules. A property shall have a getter method, a setter method, or both.

**CLS Rule 29:**[**CLS Rule 29:**](01_CLI%20基本概念和体系结构.md#R29) The methods that implement an event shall be marked SpecialName in the metadata. 

[**CLS Rule 30:**](01_CLI%20基本概念和体系结构.md#R30) The accessibility of an event and of its accessors shall be identical. 

[**CLS Rule 31:**](01_CLI%20基本概念和体系结构.md#R31) The add and remove methods for an event shall both either be present or absent. 

[**CLS Rule 32:**](01_CLI%20基本概念和体系结构.md#R32) The add and remove methods for an event shall each take one parameter whose type defines the type of the event and that shall be derived from `System.Delegate`.

[**CLS Rule 33:**](01_CLI%20基本概念和体系结构.md#R33) Events shall adhere to a specific naming pattern. The SpecialName attribute referred to in CLS rule 29 shall be ignored in appropriate name comparisons and shall adhere to identifier rules. 

[**CLS Rule 34:**](01_CLI%20基本概念和体系结构.md#R34) The CLS only allows a subset of the encodings of custom attributes.  The only types that shall appear in these encodings are: `System.Type`, `System.String`, `System.Char`, `System.Boolean`, `System.Byte`, `System.Int16`, `System.Int32`, `System.Int64`, `System.Single`, `System.Double`, and any enumeration type based on a CLS-compliant base integer type. 

[**CLS Rule 35:**](01_CLI%20基本概念和体系结构.md#R35) The CLS does not allow publicly visible required modifiers (modreq), but does allow optional modifiers (modopt) it does not understand. 

[**CLS Rule 36:**](01_CLI%20基本概念和体系结构.md#R36) Global static fields and methods are not CLS-compliant.

[**CLS Rule 37:**](01_CLI%20基本概念和体系结构.md#R37) Only properties and methods can be overloaded. 

[**CLS Rule 38:**](01_CLI%20基本概念和体系结构.md#R38) Properties and methods can be overloaded based only on the number and types of their parameters, except the conversion operators named `op_Implicit` and `op_Explicit`, which can also be overloaded based on their return type. 

[**CLS Rule 39:**](01_CLI%20基本概念和体系结构.md#R39) If either `op_Implicit` or `op_Explicit` is provided, an alternate means of providing the coercion shall be provided.

[**CLS Rule 40:**](01_CLI%20基本概念和体系结构.md#R40) Objects that are thrown shall be of type `System.Exception` or a type inheriting from it. Nonetheless, CLS-compliant methods are not required to block the propagation of other types of exceptions. 

[**CLS Rule 41:**](01_CLI%20基本概念和体系结构.md#R41) Attributes shall be of type `System.Attribute`, or a type inheriting from it. 

[**CLS Rule 42:**](01_CLI%20基本概念和体系结构.md#R42) Nested types shall have at least as many generic parameters as the enclosing type. Generic parameters in a nested type correspond by position to the generic parameters in its enclosing type. 

[**CLS Rule 43:**](01_CLI%20基本概念和体系结构.md#R43) The name of a generic type shall encode the number of type parameters declared on the non-nested type, or newly introduced to the type if nested, according to the rules defined above. 

[**CLS Rule 44:**](01_CLI%20基本概念和体系结构.md#R44) A generic type shall redeclare sufficient constraints to guarantee that any constraints on the base type, or interfaces would be satisfied by the generic type constraints.

[**CLS Rule 45:**](01_CLI%20基本概念和体系结构.md#R45) Types used as constraints on generic parameters shall themselves be CLS-compliant. 

[**CLS Rule 46:**](01_CLI%20基本概念和体系结构.md#R46) The visibility and accessibility of members (including nested types) in an instantiated generic type shall be considered to be scoped to the specific instantiation rather than the generic type declaration as a whole. Assuming this, the visibility and accessibility rules of CLS rule 12 still apply.

[**CLS Rule 47:**](01_CLI%20基本概念和体系结构.md#R47) For each abstract or virtual generic method, there shall be a default concrete (non-abstract) implementation. 

[**CLS Rule 48:**](01_CLI%20基本概念和体系结构.md#R48) If two or more CLS-compliant methods declared in a type have the same name and, for a specific set of type instantiations, they have the same parameter and return types, then all these methods shall be semantically equivalent at those type instantiations.

>---
### Chinese Translation
 
[**CLS Rule 1:**](01_CLI%20基本概念和体系结构.md#R1) CLS 规则仅适用于那些定义在程序集之外的可访问或可见类型的那些内容。

[**CLS Rule 2:**](01_CLI%20基本概念和体系结构.md#R2) 不兼容 CLS 的类型的成员不应标记为 *CLS-compliant*。

[**CLS Rule 3:**](01_CLI%20基本概念和体系结构.md#R3) 装箱的值类型不是 *CLS-compliant* 的。适当的情况下，可以使用 `System.Object`，`System.ValueType` 或 `System.Enum` 来代替装箱类型。

[**CLS Rule 4:**](01_CLI%20基本概念和体系结构.md#R4) 程序集应遵循附录 **Annex 7 of Technical Report 15 of the Unicode Standard 3.0**，该 [附录](http://www.unicode.org/unicode/reports/tr15/tr15-18.html) 规定了允许开始和包含在标识符中的字符集。标识符应采用 **Unicode Normalization Form C** 定义的规范格式。出于对 CLS 的支持，如果两个标识符的小写映射（由 Unicode 不区分区域的一对一小写映射指定）相同，那么这两个标识符就是相同的。要想在 CLS 下被认为是不同的，两个标识符在大小写上必须有所不同。但是，为了覆盖隐藏继承的定义，CLI 要求使用原始声明的精确编码。

[**CLS Rule 5:**](01_CLI%20基本概念和体系结构.md#R5) 在遵循 CLS 的范围内引入的所有名称应该是独立的，且不依赖于类型，除非这些名称相同并通过重载进行解析。尽管 CTS 允许单个类型为方法和字段使用相同的名称，但 CLS 不允许这样做。

[**CLS Rule 6:**](01_CLI%20基本概念和体系结构.md#R6) 字段和嵌套类型应仅通过标识符比较来区分，尽管 CTS 允许区分不同的签名。具有相同名称（通过标识符比较）的方法、属性和事件应该不仅仅在返回类型上有所不同，除 CLS Rule 39 情况外。

[**CLS Rule 7:**](01_CLI%20基本概念和体系结构.md#R7) 枚举的底层类型应该是内置的 CLS 整数类型，字段的名称应该是 `"value__"`，并且该字段应该被标记为 `RTSpecialName`。

[**CLS Rule 8:**](01_CLI%20基本概念和体系结构.md#R8) 有两种不同的枚举，通过 `System.FlagsAttribute` 自定义特性的存在或缺失来表示。一种代表命名的整数值；另一种代表可以组合生成未命名值的命名位标志。枚举的值不限于指定的值。

[**CLS Rule 9:**](01_CLI%20基本概念和体系结构.md#R9) 枚举的文字静态字段应该具有枚举本身的类型。

[**CLS Rule 10:**](01_CLI%20基本概念和体系结构.md#R10) 在重写继承的方法时，可访问性不应改变，除非重写从不同程序集继承的具有 **family-or-assembly** 可访问性的方法。在这种情况下，重写应具有 **family** 可访问性。

[**CLS Rule 11:**](01_CLI%20基本概念和体系结构.md#R11) 出现在签名中的所有类型都应该是 *CLS-compliant* 的。组成实例化泛型类型的所有类型都应该是 *CLS-compliant* 的。

[**CLS Rule 12:**](01_CLI%20基本概念和体系结构.md#R12) 类型和成员的可见性和可访问性应是：任何成员签名中的类型在成员本身可见和可访问时应该是可见和可访问的。例如，一个在其程序集外部可见的公共方法不应该有一个只在程序集内部可见的类型的参数。在任何成员的签名中使用的实例化泛型类型的组成类型的可见性和可访问性应该在成员本身可见和可访问时是可见和可访问的。一个在其程序集外部可见的成员的签名中存在的实例化泛型类型不应该有一个只在程序集内部可见的类型的泛型参数。

[**CLS Rule 13:**](01_CLI%20基本概念和体系结构.md#R13) 字面量静态的值是通过使用字段初始化元数据来指定的。一个符合 CLS 的字面量必须在字段初始化元数据中指定一个与字面量完全相同类型的值（或者，如果该字面量是枚举，则为底层类型）。

[**CLS Rule 14:**](01_CLI%20基本概念和体系结构.md#R14) 类型引用不是 *CLS-compliant*。

[**CLS Rule 15:**](01_CLI%20基本概念和体系结构.md#R15) `vararg` 约束不是 CLS 的一部分，CLS 支持的唯一调用约定是标准的托管调用约定。

[**CLS Rule 16:**](01_CLI%20基本概念和体系结构.md#R16) 数组应具有符合 CLS 的元素类型，且数组的所有维度的下界应为零。仅要求项是数组和数组的元素类型这一事实来区分重载。当重载基于两个或更多数组类型时，元素类型应为命名类型。

[**CLS Rule 17:**](01_CLI%20基本概念和体系结构.md#R17) 非托管指针类型不兼容 CLS。

[**CLS Rule 18:**](01_CLI%20基本概念和体系结构.md#R18) 兼容 CLS 的接口不应要求为了实现它们而定义非 CLS 兼容的方法。

[**CLS Rule 19:**](01_CLI%20基本概念和体系结构.md#R19) 兼容 CLS 的接口不应定义静态方法，也不应定义字段。

[**CLS Rule 20:**](01_CLI%20基本概念和体系结构.md#R20) 兼容 CLS 的类、值类型和接口不应要求实现非 CLS 兼容的成员。

[**CLS Rule 21:**](01_CLI%20基本概念和体系结构.md#R21) 对象构造函数在访问继承的实例数据之前，必须调用其基类的某个实例构造函数（这不适用于值类型，构造它们时可以不需要调用构造函数）。

[**CLS Rule 22:**](01_CLI%20基本概念和体系结构.md#R22) 对象构造函数只能在创建对象的过程中被调用，一个对象不应被初始化两次。

[**CLS Rule 23:**](01_CLI%20基本概念和体系结构.md#R23) `System.Object` 兼容 CLS。任何其他兼容 CLS 的类应继承自兼容 CLS 的类。

[**CLS Rule 24:**](01_CLI%20基本概念和体系结构.md#R24) 实现属性的 *getter* 和 *setter* 方法的方法应在元数据中标记为 `SpecialName`。

[**CLS Rule 25:**](01_CLI%20基本概念和体系结构.md#R25) 不再使用。在这个标准的早期版本中，这个规则声明 “属性和它的访问器的可访问性应相同”。删除这条规则以允许例如 “对 *getter* 的公共访问，同时限制对 *setter* 的访问” 的情景。

[**CLS Rule 26:**](01_CLI%20基本概念和体系结构.md#R26) 属性的访问器应全部为静态的，全部为虚拟的，或全部为实例的。

[**CLS Rule 27:**](01_CLI%20基本概念和体系结构.md#R27) 属性的类型应为 *getter* 的返回类型和 *setter* 的最后一个参数的类型。属性的参数类型应为 *getter* 的参数类型和 *setter* 除最后一个参数外的所有参数的类型。所有这些类型都应兼容 CLS，且不应为托管指针（不应按引用传递）。

[**CLS Rule 28:**](01_CLI%20基本概念和体系结构.md#R28) 属性应遵循特定的命名模式。在 CLS Rule 24 中引用的 `SpecialName` 特性应在适当的名称比较中被忽略，并应遵循标识符规则。属性应有一个 *getter* 方法，或一个 *setter* 方法，或两者都有。

[**CLS Rule 29:**](01_CLI%20基本概念和体系结构.md#R29) 实现事件的方法应在元数据中标记为 `SpecialName`。

[**CLS Rule 30:**](01_CLI%20基本概念和体系结构.md#R30) 事件及其访问器的可访问性应相同。

[**CLS Rule 31:**](01_CLI%20基本概念和体系结构.md#R31) 事件的 `add` 和 `remove` 方法应同时存在或不存在。

[**CLS Rule 32:**](01_CLI%20基本概念和体系结构.md#R32) 事件的 `add` 和 `remove` 方法应各自接受一个参数，其类型定义了事件的类型，并且应派生自 `System.Delegate`。

[**CLS Rule 33:**](01_CLI%20基本概念和体系结构.md#R33) 事件应遵循特定的命名模式。在 CLS Rule 29 中引用的 `SpecialName` 特性应在适当的名称比较中被忽略，并应遵循标识符规则。

[**CLS Rule 34:**](01_CLI%20基本概念和体系结构.md#R34) CLS 只允许自定义特性的一部分编码。在这些编码中仅支持的类型是 `System.Type`、  `System.String`、`System.Char`、`System.Boolean`、`System.Byte`、`System.Int16`、`System.  Int32`、`System.Int64`、`System.Single`、`System.Double`，以及任何基于 *CLS-Compliant* 兼容的基  础整数类型的枚举类型。

[**CLS Rule 35:**](01_CLI%20基本概念和体系结构.md#R35) CLS 不允许公开可见的必需修饰符（**modreq**），但是允许它不理解的可选修饰符（**modopt**）。

[**CLS Rule 36:**](01_CLI%20基本概念和体系结构.md#R36) 全局静态字段和方法不兼容 CLS。

[**CLS Rule 37:**](01_CLI%20基本概念和体系结构.md#R37) 只有属性和方法可以被重载。

[**CLS Rule 38:**](01_CLI%20基本概念和体系结构.md#R38) 属性和方法的重载只能基于它们的参数的数量和类型，除了名为 `op_Implicit` 和 `op_Explicit` 的转换运算符，它们也可以根据它们的返回类型进行重载。

[**CLS Rule 39:**](01_CLI%20基本概念和体系结构.md#R39) 如果提供了 `op_Implicit` 或 `op_Explicit`，则应提供提供强制转换的替代方法。

[**CLS Rule 40:**](01_CLI%20基本概念和体系结构.md#R40) 抛出的对象应该是 `System.Exception` 类型或从它继承的类型。但是，兼容 CLS 的方法不需要阻止其他类型的异常的传播。

[**CLS Rule 41:**](01_CLI%20基本概念和体系结构.md#R41) 特性应该是 `System.Attribute` 类型，或者从它继承的类型。

[**CLS Rule 42:**](01_CLI%20基本概念和体系结构.md#R42) 嵌套类型应该至少有与封闭类型一样多的泛型参数。嵌套类型中的泛型参数按位置对应于其封闭类型中的泛型参数。

[**CLS Rule 43:**](01_CLI%20基本概念和体系结构.md#R43) 泛型类型的名称应根据上述规则，编码非嵌套类型上声明的类型参数的数量，或者如果嵌套，则编码新引入到类型的类型参数的数量。

[**CLS Rule 44:**](01_CLI%20基本概念和体系结构.md#R44) 泛型类型应重新声明足够的约束，以保证基类型或接口上的任何约束都能被泛型类型约束满足。

[**CLS Rule 45:**](01_CLI%20基本概念和体系结构.md#R45) 用作泛型参数约束的类型本身应兼容 CLS。

[**CLS Rule 46:**](01_CLI%20基本概念和体系结构.md#R46) 在实例化的泛型类型中，成员（包括嵌套类型）的可见性和可访问性应被视为限定在特定实例，而不是整个泛型类型声明。

[**CLS Rule 47:**](01_CLI%20基本概念和体系结构.md#R47) 对于每个抽象或虚泛型方法，应该有一个默认的具体（非抽象）实现。

[**CLS Rule 48:**](01_CLI%20基本概念和体系结构.md#R48) 如果一个类型中声明的两个或多个符合 CLS 的方法具有相同的名称，并且对于一组特性的类型实例化，它们具有相同的参数和返回类型，那么所有这些方法在这些类型实例化中应该在语义上等效。

---
## 
