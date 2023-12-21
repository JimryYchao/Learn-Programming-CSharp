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

[**CLS Rule 1:**](01_CLI%20基本概念和体系结构.md/#R1) CLS rules apply only to those parts of a type that are accessible or visible outside of the defining assembly. []()

[**CLS Rule 2:**](01_CLI%20基本概念和体系结构.md#R2) Members of non-CLS compliant types shall not be marked CLS-compliant. (§[I.7.3.1](i.7.3.1-marking-items-as-cls-compliant.md))

**CLS Rule 3:** Boxed value types are not CLS-compliant. (§[I.8.2.4](i.8.2.4-boxing-and-unboxing-of-values.md).)

**CLS Rule 4:** Assemblies shall follow Annex 7 of Technical Report 15 of the Unicode Standard 3.0 governing the set of characters permitted to start and be included in identifiers, available on-line at http://www.unicode.org/unicode/reports/tr15/tr15-18.html. Identifiers shall be in the canonical format defined by Unicode Normalization Form C. For CLS purposes, two identifiers are the same if their lowercase mappings (as specified by the Unicode locale-insensitive, one-to-one lowercase mappings) are the same. That is, for two identifiers to be considered different under the CLS they shall differ in more than simply their case. However, in order to override an inherited definition the CLI requires the precise encoding of the original declaration be used. (§[I.8.5.1](i.8.5.1-valid-names.md))

**CLS Rule 5:** All names introduced in a CLS-compliant scope shall be distinct independent of kind, except where the names are identical and resolved via overloading. That is, while the CTS allows a single type to use the same name for a method and a field, the CLS does not. (§[I.8.5.2](i.8.5.2-assemblies-and-scoping.md))

**CLS Rule 6:** Fields and nested types shall be distinct by identifier comparison alone, even though the CTS allows distinct signatures to be distinguished.  Methods, properties, and events that have the same name (by identifier comparison) shall differ by more than just the return type, except as specified in CLS Rule 39. (§[I.8.5.2](i.8.5.2-assemblies-and-scoping.md))

**CLS Rule 7:** The underlying type of an enum shall be a built-in CLS integer type, the name of the field shall be "value__", and that field shall be marked RTSpecialName. (§[I.8.5.2](i.8.5.2-assemblies-and-scoping.md))

**CLS Rule 8:** There are two distinct kinds of enums, indicated by the presence or absence of the `System.FlagsAttribute` (see [Partition IV Library] ) custom attribute. One represents named integer values; the other represents named bit flags that can be combined to generate an unnamed value.  The value of an enum is not limited to the specified values. (§[I.8.5.2](i.8.5.2-assemblies-and-scoping.md))

**CLS Rule 9:** Literal static fields (see §[I.8.6.1](i.8.6.1-signatures.md)) of an enum shall have the type of the enum itself. (§[I.8.5.2](i.8.5.2-assemblies-and-scoping.md))

**CLS Rule 10:** Accessibility shall not be changed when overriding inherited methods, except when overriding a method inherited from a different assembly with accessibility family-or-assembly.  In this case, the override shall have accessibility family. (§[I.8.5.3.2](i.8.5.3.2-accessibility-of-members-and-nested-types.md))

**CLS Rule 11:** All types appearing in a signature shall be CLS-compliant. All types composing an instantiated generic type shall be CLS-compliant. (§[I.8.6.1](i.8.6.1-signatures.md))

**CLS Rule 12:** The visibility and accessibility of types and members shall be such that types in the signature of any member shall be visible and accessible whenever the member itself is visible and accessible. For example, a public method that is visible outside its assembly shall not have an argument whose type is visible only within the assembly. The visibility and accessibility of types composing an instantiated generic type used in the signature of any member shall be visible and accessible whenever the member itself is visible and accessible. For example, an instantiated generic type present in the signature of a member that is visible outside its assembly shall not have a generic argument whose type is visible only within the assembly. (§[I.8.6.1](i.8.6.1-signatures.md))

**CLS Rule 13:** The value of a literal static is specified through the use of field initialization metadata (see Partition II Metadata). A CLS-compliant literal must have a value specified in field initialization metadata that is of exactly the same type as the literal (or of the underlying type, if that literal is an enum). (§[I.8.6.1.2] )

**CLS Rule 14:** Typed references are not CLS-compliant. (§[I.8.6.1.3] )

**CLS Rule 15:** The vararg constraint is not part of the CLS, and the only calling convention supported by the CLS is the standard managed calling convention. (§[I.8.6.1.5] )

**CLS Rule 16:** Arrays shall have elements with a CLS-compliant type, and all dimensions of the array shall have lower bounds of zero. Only the fact that an item is an array and the element type of the array shall be required to distinguish between overloads.  When overloading is based on two or more array types the element types shall be named types. (§[I.8.9.1](i.8.9.1-array-types.md))

**CLS Rule 17:** Unmanaged pointer types are not CLS-compliant. (§[I.8.9.2](i.8.9.2-unmanaged-pointer-types.md))

**CLS Rule 18:** CLS-compliant interfaces shall not require the definition of non-CLS compliant methods in order to implement them. (§[I.8.9.4](i.8.9.4-interface-type-definition.md))

**CLS Rule 19:** CLS-compliant interfaces shall not define static methods, nor shall they define fields. (§[I.8.9.4](i.8.9.4-interface-type-definition.md))

**CLS Rule 20:** CLS-compliant classes, value types, and interfaces shall not require the implementation of non-CLS-compliant members. (§[I.8.9.6.4]] )

**CLS Rule 21:** An object constructor shall call some instance constructor of its base class before any access occurs to inherited instance data. (This does not apply to value types, which need not have constructors.) (§[I.8.9.6.6]] )

**CLS Rule 22:** An object constructor shall not be called except as part of the creation of an object, and an object shall not be initialized twice. (§[I.8.9.6.6]] )

**CLS Rule 23:** `System.Object` is CLS-compliant. Any other CLS-compliant class shall inherit from a CLS-compliant class. (§[I.8.9.9]] )

**CLS Rule 24:** The methods that implement the getter and setter methods of a property shall be marked SpecialName in the metadata. (§[I.8.11.3]] )

**CLS Rule 25:** No longer used. _[Note:_ In an earlier version of this standard, this rule stated "The accessibility of a property and of its accessors shall be identical." The removal of this rule allows, for example, public access to a getter while restricting access to the setter. _end note]_ (§[I.8.11.3](i.8.11.3-property-definitions.md))

**CLS Rule 26:** A property’s accessors shall all be static, all be virtual, or all be instance. (§[I.8.11.3](i.8.11.3-property-definitions.md))

**CLS Rule 27:** The type of a property shall be the return type of the getter and the type of the last argument of the setter.  The types of the parameters of the property shall be the types of the parameters to the getter and the types of all but the final parameter of the setter.  All of these types shall be CLS-compliant, and shall not be managed pointers (i.e., shall not be passed by reference). (§[I.8.11.3](i.8.11.3-property-definitions.md))

**CLS Rule 28:** Properties shall adhere to a specific naming pattern. See §[I.10.4](i.10.4-naming-patterns.md). The SpecialName attribute referred to in CLS rule 24 shall be ignored in appropriate name comparisons and shall adhere to identifier rules. A property shall have a getter method, a setter method, or both. (§[I.8.11.3](i.8.11.3-property-definitions.md))

**CLS Rule 29:** The methods that implement an event shall be marked SpecialName in the metadata. (§[I.8.11.4](i.8.11.4-event-definitions.md))

**CLS Rule 30:** The accessibility of an event and of its accessors shall be identical. (§[I.8.11.4](i.8.11.4-event-definitions.md))

**CLS Rule 31:** The add and remove methods for an event shall both either be present or absent. (§[I.8.11.4](i.8.11.4-event-definitions.md))

**CLS Rule 32:** The add and remove methods for an event shall each take one parameter whose type defines the type of the event and that shall be derived from `System.Delegate`. (§[I.8.11.4](i.8.11.4-event-definitions.md))

**CLS Rule 33:** Events shall adhere to a specific naming pattern. See §[I.10.4](i.10.4-naming-patterns.md). The SpecialName attribute referred to in CLS rule 29 shall be ignored in appropriate name comparisons and shall adhere to identifier rules. (§[I.8.11.4](i.8.11.4-event-definitions.md))

**CLS Rule 34:** The CLS only allows a subset of the encodings of custom attributes.  The only types that shall appear in these encodings are (see Partition IV): `System.Type`, `System.String`, `System.Char`, `System.Boolean`, `System.Byte`, `System.Int16`, `System.Int32`, `System.Int64`, `System.Single`, `System.Double`, and any enumeration type based on a CLS-compliant base integer type. (§[I.9.7](i.9.7-metadata-extensibility.md))

**CLS Rule 35:** The CLS does not allow publicly visible required modifiers (modreq, see [Partition II](i.9.7-metadata-extensibility.md#cls-rule-35)), but does allow optional modifiers (modopt, see [Partition II](i.9.7-metadata-extensibility.md#cls-rule-35)) it does not understand. (§[I.9.7](i.9.7-metadata-extensibility.md))

**CLS Rule 36:** Global static fields and methods are not CLS-compliant. (§[I.9.8](i.9.8-globals-imports-and-exports.md))

**CLS Rule 37:** Only properties and methods can be overloaded. (§[I.10.2](i.10.2-overloading.md))

**CLS Rule 38:** Properties and methods can be overloaded based only on the number and types of their parameters, except the conversion operators named `op_Implicit` and `op_Explicit`, which can also be overloaded based on their return type. (§[I.10.2](i.10.2-overloading.md))

**CLS Rule 39:** If either `op_Implicit` or `op_Explicit` is provided, an alternate means of providing the coercion shall be provided. (§[I.10.3.3](i.10.3.3-conversion-operators.md))

**CLS Rule 40:** Objects that are thrown shall be of type `System.Exception` or a type inheriting from it. Nonetheless, CLS-compliant methods are not required to block the propagation of other types of exceptions. (§[I.10.5](i.10.5-exceptions.md))

**CLS Rule 41:** Attributes shall be of type `System.Attribute`, or a type inheriting from it. (§[I.10.6](i.10.6-custom-attributes.md))

**CLS Rule 42:** Nested types shall have at least as many generic parameters as the enclosing type. Generic parameters in a nested type correspond by position to the generic parameters in its enclosing type. (§[I.10.7.1](i.10.7.1-nested-type-parameter-re-declaration.md))

**CLS Rule 43:** The name of a generic type shall encode the number of type parameters declared on the non-nested type, or newly introduced to the type if nested, according to the rules defined above. (§[I.10.7.2](i.10.7.2-type-names-and-arity-encoding.md))

**CLS Rule 44:** A generic type shall redeclare sufficient constraints to guarantee that any constraints on the base type, or interfaces would be satisfied by the generic type constraints. (§[I.10.7.3](i.10.7.3-type-constraint-re-declaration.md))

**CLS Rule 45:** Types used as constraints on generic parameters shall themselves be CLS-compliant. (§[I.10.7.4](i.10.7.4-constraint-type-restrictions.md))

**CLS Rule 46:** The visibility and accessibility of members (including nested types) in an instantiated generic type shall be considered to be scoped to the specific instantiation rather than the generic type declaration as a whole. Assuming this, the visibility and accessibility rules of CLS rule 12 still apply. (§[I.10.7.5](i.10.7.5-frameworks-and-accessibility-of-nested-types.md))

**CLS Rule 47:** For each abstract or virtual generic method, there shall be a default concrete (non-abstract) implementation. (§[I.10.7.6](i.10.7.6-frameworks-and-abstract-or-virtual-methods.md))

**CLS Rule 48:** If two or more CLS-compliant methods declared in a type have the same name and, for a specific set of type instantiations, they have the same parameter and return types, then all these methods shall be semantically equivalent at those type instantiations. (§[I.7.2.1](i.7.2.1-cls-framework.md))


**CLS Rule 1**：CLS rules apply only to those parts of a type that are accessible or visible outside of the defining assembly.

**CLS Rule 2**：

**CLS Rule 3**：

**CLS Rule 4**：

**CLS Rule 5**：

**CLS Rule 6**：

**CLS Rule 7**：

**CLS Rule 8**：

**CLS Rule 9**：
**CLS Rule 10**：
**CLS Rule 11**：
**CLS Rule 12**：
**CLS Rule 13**：
**CLS Rule 14**：
**CLS Rule 15**：
**CLS Rule 16**：
**CLS Rule 17**：
**CLS Rule 18**：
**CLS Rule 19**：
**CLS Rule 20**：
**CLS Rule 21**：
**CLS Rule 22**：
**CLS Rule 23**：
**CLS Rule 24**：
**CLS Rule 25**：
**CLS Rule 26**：
**CLS Rule 27**：
**CLS Rule 28**：
**CLS Rule 29**：
**CLS Rule 30**：
**CLS Rule 31**：
**CLS Rule 32**：
**CLS Rule 33**：
**CLS Rule 34**：
**CLS Rule 35**：
**CLS Rule 36**：
**CLS Rule 37**：
**CLS Rule 38**：
**CLS Rule 39**：
**CLS Rule 40**：
**CLS Rule 41**：
**CLS Rule 42**：
**CLS Rule 43**：
**CLS Rule 44**：
**CLS Rule 45**：
**CLS Rule 46**：
**CLS Rule 47**：
**CLS Rule 48**：