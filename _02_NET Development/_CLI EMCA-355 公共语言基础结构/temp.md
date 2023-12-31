

## 21. 元数据逻辑格式：其他结构
<a id="metadata-format-others"></a>

### 21.1. 位掩码和标志

此子条款解释了元数据表中使用的标志和位掩码。当符合规范的实现遇到未在此标准中指定的元数据结构 (如标志) 时，实现的行为是未指定的。

#### 21.1.1. AssemblyHashAlgorithm的值

 | 算法               | 值     |
 | ------------------ | ------ |
 | `None`             | 0x0000 |
 | `Reserved` (`MD5`) | 0x8003 |
 | `SHA1`             | 0x8004 |

#### 21.1.2. AssemblyFlags 的值

 | 标志                         | 值     | 描述                                                                                                                   |
 | ---------------------------- | ------ | ---------------------------------------------------------------------------------------------------------------------- |
 | `PublicKey`                  | 0x0001 | 程序集引用包含完整的 (未哈希的) 公钥。                                                                                 |
 | `Retargetable`               | 0x0100 | 运行时使用的此程序集的实现不预期与编译时看到的版本匹配。 (参见此表后的文本。)                                          |
 | `DisableJITcompileOptimizer` | 0x4000 | 保留 (CLI的符合规范的实现可以在读取时忽略此设置；一些实现可能使用此位来指示CIL到本地代码编译器不应生成优化的代码)      |
 | `EnableJITcompileTracking`   | 0x8000 | 保留 (CLI的符合规范的实现可以在读取时忽略此设置；一些实现可能使用此位来指示CIL到本地代码编译器应生成CIL到本地代码映射) |

#### 21.1.3. Culture的值

 | &nbsp;     | &nbsp;     | &nbsp;     | &nbsp;     |
 | ---------- | ---------- | ---------- | ---------- |
 | `ar-SA`    | `ar-IQ`    | `ar-EG`    | `ar-LY`    |
 | `ar-DZ`    | `ar-MA`    | `ar-TN`    | `ar-OM`    |
 | `ar-YE`    | `ar-SY`    | `ar-JO`    | `ar-LB`    |
 | `ar-KW`    | `ar-AE`    | `ar-BH`    | `ar-QA`    |
 | `bg-BG`    | `ca-ES`    | `zh-TW`    | `zh-CN`    |
 | `zh-HK`    | `zh-SG`    | `zh-MO`    | `cs-CZ`    |
 | `da-DK`    | `de-DE`    | `de-CH`    | `de-AT`    |
 | `de-LU`    | `de-LI`    | `el-GR`    | `en-US`    |
 | `en-GB`    | `en-AU`    | `en-CA`    | `en-NZ`    |
 | `en-IE`    | `en-ZA`    | `en-JM`    | `en-CB`    |
 | `en-BZ`    | `en-TT`    | `en-ZW`    | `en-PH`    |
 | `es-ES-Ts` | `es-MX`    | `es-ES-Is` | `es-GT`    |
 | `es-CR`    | `es-PA`    | `es-DO`    | `es-VE`    |
 | `es-CO`    | `es-PE`    | `es-AR`    | `es-EC`    |
 | `es-CL`    | `es-UY`    | `es-PY`    | `es-BO`    |
 | `es-SV`    | `es-HN`    | `es-NI`    | `es-PR`    |
 | `fi-FI`    | `fr-FR`    | `fr-BE`    | `fr-CA`    |
 | `fr-CH`    | `fr-LU`    | `fr-MC`    | `he-IL`    |
 | `hu-HU`    | `is-IS`    | `it-IT`    | `it-CH`    |
 | `ja-JP`    | `ko-KR`    | `nl-NL`    | `nl-BE`    |
 | `nb-NO`    | `nn-NO`    | `pl-PL`    | `pt-BR`    |
 | `pt-PT`    | `ro-RO`    | `ru-RU`    | `hr-HR`    |
 | `lt-sr-SP` | `cy-sr-SP` | `sk-SK`    | `sq-AL`    |
 | `sv-SE`    | `sv-FI`    | `th-TH`    | `tr-TR`    |
 | `ur-PK`    | `id-ID`    | `uk-UA`    | `be-BY`    |
 | `sl-SI`    | `et-EE`    | `lv-LV`    | `lt-LT`    |
 | `fa-IR`    | `vi-VN`    | `hy-AM`    | `lt-az-AZ` |
 | `cy-az-AZ` | `eu-ES`    | `mk-MK`    | `af-ZA`    |
 | `ka-GE`    | `fo-FO`    | `hi-IN`    | `ms-MY`    |
 | `ms-BN`    | `kk-KZ`    | `ky-KZ`    | `sw-KE`    |
 | `lt-uz-UZ` | `cy-uz-UZ` | `tt-TA`    | `pa-IN`    |
 | `gu-IN`    | `ta-IN`    | `te-IN`    | `kn-IN`    |
 | `mr-IN`    | `sa-IN`    | `mn-MN`    | `gl-ES`    |
 | `kok-IN`   | `syr-SY`   | `div-MV`   |

**关于RFC 1766，区域名称的注释：**典型的字符串将是"``en-US``"。第一部分 (例子中的"`en`") 使用ISO 639字符 ("小写的`拉丁字母`字符。不使用带有变音符号的或修改过的字符")。第二部分 (例子中的"`US`") 使用ISO 3166字符 (类似于ISO 639，但是大写) ；也就是说，熟悉的ASCII字符`a`&mdash;`z`和`A`&mdash;`Z`。然而，虽然RFC 1766建议第一部分使用小写，第二部分使用大写，但它允许混合大小写。因此，验证规则只检查_Culture_是否是上面列表中的字符串之一&mdash;但是检查是完全`不区分大小写`的&mdash;其中`不区分大小写`是对小于U+0080的值的熟悉的折叠。

#### 21.1.4. 事件标志 [EventAttributes]

 | 标志            | 值     | 描述                                   |
 | --------------- | ------ | -------------------------------------- |
 | `SpecialName`   | 0x0200 | 事件是特殊的。                         |
 | `RTSpecialName` | 0x0400 | CLI 提供 '特殊' 行为，取决于事件的名称 |

#### 21.1.5. 字段的标志 [FieldAttributes]

 | 标志                       | 值     | 描述                                              |
 | -------------------------- | ------ | ------------------------------------------------- |
 | **`FieldAccessMask`**      | 0x0007 | 这3位包含以下值之一：                             |
 | &emsp;`CompilerControlled` | 0x0000 | 成员不可引用                                      |
 | &emsp;`Private`            | 0x0001 | 仅父类型可访问                                    |
 | &emsp;`FamANDAssem`        | 0x0002 | 仅此程序集中的子类型可访问                        |
 | &emsp;`Assembly`           | 0x0003 | 程序集中的任何人都可以访问                        |
 | &emsp;`Family`             | 0x0004 | 仅类型和子类型可访问                              |
 | &emsp;`FamORAssem`         | 0x0005 | 任何地方的子类型以及程序集中的任何人都可以访问    |
 | &emsp;`Public`             | 0x0006 | 对于具有此范围字段合同属性的任何人都是可访问的    |
 | `Static`                   | 0x0010 | 在类型上定义，否则每个实例                        |
 | `InitOnly`                 | 0x0020 | 字段只能初始化，初始化后不能写入                  |
 | `Literal`                  | 0x0040 | 值是编译时常量                                    |
 | `NotSerialized`            | 0x0080 | 保留 (用于指示当类型被远程化时，不应序列化此字段) |
 | `SpecialName`              | 0x0200 | 字段是特殊的                                      |
 | **Interop Attributes**     | &nbsp; | &nbsp;                                            |
 | `PInvokeImpl`              | 0x2000 | 实现通过 PInvoke 转发。                           |
 | **Additional flags**       | &nbsp; | &nbsp;                                            |
 | `RTSpecialName`            | 0x0400 | CLI 提供 '特殊' 行为，取决于字段的名称            |
 | `HasFieldMarshal`          | 0x1000 | 字段有封送信息                                    |
 | `HasDefault`               | 0x8000 | 字段有默认值                                      |
 | `HasFieldRVA`              | 0x0100 | 字段有 RVA                                        |


#### 21.1.6. 文件的标志 [FileAttributes]

 | 标志                 | 值     | 描述                                     |
 | -------------------- | ------ | ---------------------------------------- |
 | `ContainsMetaData`   | 0x0000 | 这不是一个资源文件                       |
 | `ContainsNoMetaData` | 0x0001 | 这是一个资源文件或其他不包含元数据的文件 |

#### 21.1.7. 泛型参数的标志 [GenericParamAttributes]

 | 标志                                   | 值     | 描述                               |
 | -------------------------------------- | ------ | ---------------------------------- |
 | **`VarianceMask`**                     | 0x0003 | 这两位包含以下值之一：             |
 | &emsp;`None`                           | 0x0000 | 泛型参数是非变量，并且没有特殊约束 |
 | &emsp;`Covariant`                      | 0x0001 | 泛型参数是协变的                   |
 | &emsp;`Contravariant`                  | 0x0002 | 泛型参数是逆变的                   |
 | **`SpecialConstraintMask`**            | 0x001C | 这三位包含以下值之一：             |
 | &emsp;`ReferenceTypeConstraint`        | 0x0004 | 泛型参数具有类特殊约束             |
 | &emsp;`NotNullableValueTypeConstraint` | 0x0008 | 泛型参数具有值类型特殊约束         |
 | &emsp;`DefaultConstructorConstraint`   | 0x0010 | 泛型参数具有 `.ctor` 特殊约束      |

#### 21.1.8. ImplMap的标志 [PInvokeAttributes]

 | 标志                        | 值     | 描述                                                             |
 | --------------------------- | ------ | ---------------------------------------------------------------- |
 | `NoMangle`                  | 0x0001 | PInvoke将使用指定的成员名称                                      |
 | **字符集**                  | &nbsp; | &nbsp;                                                           |
 | **`CharSetMask`**           | 0x0006 | 这是一个资源文件或其他不包含元数据的文件。这两位包含以下值之一： |
 | &emsp;`CharSetNotSpec`      | 0x0000 | &nbsp;                                                           |
 | &emsp;`CharSetAnsi`         | 0x0002 | &nbsp;                                                           |
 | &emsp;`CharSetUnicode`      | 0x0004 | &nbsp;                                                           |
 | &emsp;`CharSetAuto`         | 0x0006 | &nbsp;                                                           |
 | `SupportsLastError`         | 0x0040 | 关于目标函数的信息。对字段不相关                                 |
 | **调用约定**                | &nbsp; | &nbsp;                                                           |
 | `CallConvMask`              | 0x0700 | 这三位包含以下值之一：                                           |
 | &emsp;`CallConvPlatformapi` | 0x0100 | &nbsp;                                                           |
 | &emsp;`CallConvCdecl`       | 0x0200 | &nbsp;                                                           |
 | &emsp;`CallConvStdcall`     | 0x0300 | &nbsp;                                                           |
 | &emsp;`CallConvThiscall`    | 0x0400 | &nbsp;                                                           |
 | &emsp;`CallConvFastcall`    | 0x0500 | &nbsp;                                                           |

#### 21.1.9. ManifestResource 标志 [ManifestResourceAttributes]

 | 标志                 | 值     | 描述                   |
 | -------------------- | ------ | ---------------------- |
 | **`VisibilityMask`** | 0x0007 | 这三位包含以下值之一： |
 | &emsp;`Public`       | 0x0001 | 资源从程序集中导出     |
 | &emsp;`Private`      | 0x0002 | 资源对程序集是私有的   |

#### 21.1.10. 方法的标志 [MethodAttributes]
<a id="MethodAttributes"></a>

 | 标志                       | 值     | 描述                                             |
 | -------------------------- | ------ | ------------------------------------------------ |
 | **`MemberAccessMask`**     | 0x0007 | 这3位包含以下值之一：                            |
 | &emsp;`CompilerControlled` | 0x0000 | 成员不可引用                                     |
 | &emsp;`Private`            | 0x0001 | 仅父类型可访问                                   |
 | &emsp;`FamANDAssem`        | 0x0002 | 仅此程序集中的子类型可访问                       |
 | &emsp;`Assem`              | 0x0003 | 程序集中的任何人都可以访问                       |
 | &emsp;`Family`             | 0x0004 | 仅类型和子类型可访问                             |
 | &emsp;`FamORAssem`         | 0x0005 | 任何地方的子类型以及程序集中的任何人都可以访问   |
 | &emsp;`Public`             | 0x0006 | 对于具有此范围字段合同属性的任何人都是可访问的   |
 | `Static`                   | 0x0010 | 在类型上定义，否则每个实例                       |
 | `Final`                    | 0x0020 | 方法不能被重写                                   |
 | `Virtual`                  | 0x0040 | 方法是虚拟的                                     |
 | `HideBySig`                | 0x0080 | 方法通过名称+签名隐藏，否则只通过名称隐藏        |
 | **`VtableLayoutMask`**     | 0x0100 | 使用此掩码检索 vtable 属性。此位包含以下值之一： |
 | &emsp;`ReuseSlot`          | 0x0000 | 方法重用 vtable 中的现有槽                       |
 | &emsp;`NewSlot`            | 0x0100 | 方法总是在 vtable 中获取新槽                     |
 | `Strict`                   | 0x0200 | 方法只有在也可访问时才能被重写                   |
 | `Abstract`                 | 0x0400 | 方法不提供实现                                   |
 | `SpecialName`              | 0x0800 | 方法是特殊的                                     |
 | **Interop attributes**     | &nbsp; | &nbsp;                                           |
 | `PInvokeImpl`              | 0x2000 | 实现通过 PInvoke 转发                            |
 | `UnmanagedExport`          | 0x0008 | 保留：对于符合规范的实现，应为零                 |
 | **Additional flags**       | &nbsp; | &nbsp;                                           |
 | `RTSpecialName`            | 0x1000 | CLI 提供 '特殊' 行为，取决于方法的名称           |
 | `HasSecurity`              | 0x4000 | 方法与其关联的安全性                             |
 | `RequireSecObject`         | 0x8000 | 方法调用包含安全代码的另一种方法                 |

#### 21.1.11. 方法标志 [MethodImplAttributes]

 | 标志                 | 值     | 描述                                                     |
 | -------------------- | ------ | -------------------------------------------------------- |
 | **`CodeTypeMask`**   | 0x0003 | 这两位包含以下值之一：                                   |
 | &emsp;`IL`           | 0x0000 | 方法实现是 CIL                                           |
 | &emsp;`Native`       | 0x0001 | 方法实现是本地的                                         |
 | &emsp;`OPTIL`        | 0x0002 | 保留：在符合规范的实现中应为零                           |
 | &emsp;`Runtime`      | 0x0003 | 方法实现由运行时提供                                     |
 | **`ManagedMask`**    | 0x0004 | 指定代码是托管的还是非托管的标志。这一位包含以下值之一： |
 | &emsp;`Unmanaged`    | 0x0004 | 方法实现是非托管的，否则是托管的                         |
 | &emsp;`Managed`      | 0x0000 | 方法实现是托管的                                         |
 | **实现信息和互操作** | &nbsp; | &nbsp;                                                   |
 | `ForwardRef`         | 0x0010 | 表示方法已定义；主要用于合并场景                         |
 | `PreserveSig`        | 0x0080 | 保留：符合规范的实现可以忽略                             |
 | `InternalCall`       | 0x1000 | 保留：在符合规范的实现中应为零                           |
 | `Synchronized`       | 0x0020 | 方法在主体中是单线程的                                   |
 | `NoInlining`         | 0x0008 | 方法不能内联                                             |
 | `MaxMethodImplVal`   | 0xffff | 范围检查值                                               |
 | `NoOptimization`     | 0x0040 | 在生成本地代码时，方法不会被优化                         |

####  21.1.12. MethodSemantics的标志 [MethodSemanticsAttributes]

 | 标志       | 值     | 描述                                                               |
 | ---------- | ------ | ------------------------------------------------------------------ |
 | `Setter`   | 0x0001 | 属性的设置器                                                       |
 | `Getter`   | 0x0002 | 属性的获取器                                                       |
 | `Other`    | 0x0004 | 属性或事件的其他方法                                               |
 | `AddOn`    | 0x0008 | 事件的AddOn方法。这指的是事件所需的`add_`方法。 (§[22.13]())       |
 | `RemoveOn` | 0x0010 | 事件的RemoveOn方法。这指的是事件所需的`remove_`方法。 (§[22.13]()) |
 | `Fire`     | 0x0020 | 事件的Fire方法。这指的是事件的可选`raise_`方法。 (§[22.13]())      |

#### 21.1.13. 参数的标志 [ParamAttributes]

 | 标志              | 值     | 描述                           |
 | ----------------- | ------ | ------------------------------ |
 | `In`              | 0x0001 | 参数是 `[in]`                  |
 | `Out`             | 0x0002 | 参数是 `[out]`                 |
 | `Optional`        | 0x0010 | 参数是可选的                   |
 | `HasDefault`      | 0x1000 | 参数有默认值                   |
 | `HasFieldMarshal` | 0x2000 | 参数有 _FieldMarshal_          |
 | `Unused`          | 0xcfe0 | 保留：在符合规范的实现中应为零 |

#### 21.1.14. 属性标志 [PropertyAttributes]

 | 标志            | 值     | 描述                                   |
 | --------------- | ------ | -------------------------------------- |
 | `SpecialName`   | 0x0200 | 属性是特殊的                           |
 | `RTSpecialName` | 0x0400 | 运行时 (元数据内部 API) 应检查名称编码 |
 | `HasDefault`    | 0x1000 | 属性有默认值                           |
 | `Unused`        | 0xe9ff | 保留：在符合规范的实现中应为零         |

#### 21.1.15. 类型的标志 [TypeAttributes]
<a id="TypeAttributes"></a>

 | 标志                      | 值         | 描述                                                                      |
 | ------------------------- | ---------- | ------------------------------------------------------------------------- |
 | **可见性属性**            | &nbsp;     | &nbsp;                                                                    |
 | **`VisibilityMask`**      | 0x00000007 | 使用此掩码检索可见性信息。这3位包含以下值之一：                           |
 | &emsp;`NotPublic`         | 0x00000000 | 类没有公共范围                                                            |
 | &emsp;`Public`            | 0x00000001 | 类具有公共范围                                                            |
 | &emsp;`NestedPublic`      | 0x00000002 | 类是具有公共可见性的嵌套类                                                |
 | &emsp;`NestedPrivate`     | 0x00000003 | 类是具有私有可见性的嵌套类                                                |
 | &emsp;`NestedFamily`      | 0x00000004 | 类是具有家族可见性的嵌套类                                                |
 | &emsp;`NestedAssembly`    | 0x00000005 | 类是具有程序集可见性的嵌套类                                              |
 | &emsp;`NestedFamANDAssem` | 0x00000006 | 类是具有家族和程序集可见性的嵌套类                                        |
 | &emsp;`NestedFamORAssem`  | 0x00000007 | 类是具有家族或程序集可见性的嵌套类                                        |
 | **类布局属性**            | &nbsp;     | &nbsp;                                                                    |
 | **`LayoutMask`**          | 0x00000018 | 使用此掩码检索类布局信息。这2位包含以下值之一：                           |
 | &emsp;`AutoLayout`        | 0x00000000 | 类字段是自动布局的                                                        |
 | &emsp;`SequentialLayout`  | 0x00000008 | 类字段是顺序布局的                                                        |
 | &emsp;`ExplicitLayout`    | 0x00000010 | 布局是显式提供的                                                          |
 | **类语义属性**            | &nbsp;     | &nbsp;                                                                    |
 | **`ClassSemanticsMask`**  | 0x00000020 | 使用此掩码检索类语义信息。此位包含以下值之一：                            |
 | &emsp;`Class`             | 0x00000000 | 类型是类                                                                  |
 | &emsp;`Interface`         | 0x00000020 | 类型是接口                                                                |
 | **除类语义外的特殊语义**  | &nbsp;     | &nbsp;                                                                    |
 | `Abstract`                | 0x00000080 | 类是抽象的                                                                |
 | `Sealed`                  | 0x00000100 | 类不能被扩展                                                              |
 | `SpecialName`             | 0x00000400 | 类名是特殊的                                                              |
 | **实现属性**              | &nbsp;     | &nbsp;                                                                    |
 | `Import`                  | 0x00001000 | 类/接口是导入的                                                           |
 | `Serializable`            | 0x00002000 | 保留 (类是可序列化的)                                                     |
 | **字符串格式属性**        | &nbsp;     | &nbsp;                                                                    |
 | **`StringFormatMask`**    | 0x00030000 | 使用此掩码检索用于本地互操作的字符串信息。这2位包含以下值之一：           |
 | &emsp;`AnsiClass`         | 0x00000000 | `LPSTR` 被解释为 ANSI                                                     |
 | &emsp;`UnicodeClass`      | 0x00010000 | `LPSTR` 被解释为 Unicode                                                  |
 | &emsp;`AutoClass`         | 0x00020000 | `LPSTR` 自动解释                                                          |
 | &emsp;`CustomFormatClass` | 0x00030000 | 由 `CustomStringFormatMask` 指定的非标准编码                              |
 | `CustomStringFormatMask`  | 0x00C00000 | 使用此掩码检索用于本地互操作的非标准编码信息。这2位的值的含义是未指定的。 |
 | **类初始化属性**          | &nbsp;     | &nbsp;                                                                    |
 | `BeforeFieldInit`         | 0x00100000 | 在第一次静态字段访问之前初始化类                                          |
 | **附加标志**              | &nbsp;     | &nbsp;                                                                    |
 | `RTSpecialName`           | 0x00000800 | CLI 提供 '特殊' 行为，取决于类型的名称                                    |
 | `HasSecurity`             | 0x00040000 | 类型具有与其关联的安全性                                                  |
 | `IsTypeForwarder`         | 0x00200000 | 此 _ExportedType_ 条目是类型转发器                                        |

 #### 21.1.16. 签名中使用的元素类型

下表列出了`ELEMENT_TYPE`常量的值。这些在元数据签名Blobs中被广泛使用 - 参见§[II.23.2](ii.23.2-blobs-and-signatures.md)。

 | 名称                       | 值   | 备注                                                                                                                         |
 | -------------------------- | ---- | ---------------------------------------------------------------------------------------------------------------------------- |
 | `ELEMENT_TYPE_END`         | 0x00 | 标记列表的结束                                                                                                               |
 | `ELEMENT_TYPE_VOID`        | 0x01 | &nbsp;                                                                                                                       |
 | `ELEMENT_TYPE_BOOLEAN`     | 0x02 | &nbsp;                                                                                                                       |
 | `ELEMENT_TYPE_CHAR`        | 0x03 | &nbsp;                                                                                                                       |
 | `ELEMENT_TYPE_I1`          | 0x04 | &nbsp;                                                                                                                       |
 | `ELEMENT_TYPE_U1`          | 0x05 | &nbsp;                                                                                                                       |
 | `ELEMENT_TYPE_I2`          | 0x06 | &nbsp;                                                                                                                       |
 | `ELEMENT_TYPE_U2`          | 0x07 | &nbsp;                                                                                                                       |
 | `ELEMENT_TYPE_I4`          | 0x08 | &nbsp;                                                                                                                       |
 | `ELEMENT_TYPE_U4`          | 0x09 | &nbsp;                                                                                                                       |
 | `ELEMENT_TYPE_I8`          | 0x0a | &nbsp;                                                                                                                       |
 | `ELEMENT_TYPE_U8`          | 0x0b | &nbsp;                                                                                                                       |
 | `ELEMENT_TYPE_R4`          | 0x0c | &nbsp;                                                                                                                       |
 | `ELEMENT_TYPE_R8`          | 0x0d | &nbsp;                                                                                                                       |
 | `ELEMENT_TYPE_STRING`      | 0x0e | &nbsp;                                                                                                                       |
 | `ELEMENT_TYPE_PTR`         | 0x0f | 后跟 *type*                                                                                                                  |
 | `ELEMENT_TYPE_BYREF`       | 0x10 | 后跟 *type*                                                                                                                  |
 | `ELEMENT_TYPE_VALUETYPE`   | 0x11 | 后跟 _TypeDef_ 或 _TypeRef_  _token_                                                                                              |
 | `ELEMENT_TYPE_CLASS`       | 0x12 | 后跟 _TypeDef_ 或 _TypeRef_  _token_                                                                                              |
 | `ELEMENT_TYPE_VAR`         | 0x13 | 泛型类型定义中的泛型参数，表示为 _number_ (压缩的无符号整数)                                                                 |
 | `ELEMENT_TYPE_ARRAY`       | 0x14 | *type* *rank* *boundsCount* *bound1* &hellip; *loCount* *lo1* &hellip;                                                       |
 | `ELEMENT_TYPE_GENERICINST` | 0x15 | 泛型类型实例化。后跟 *type* *type-arg-count* *type-1* &hellip; *type-n*                                                      |
 | `ELEMENT_TYPE_TYPEDBYREF`  | 0x16 | &nbsp;                                                                                                                       |
 | `ELEMENT_TYPE_I`           | 0x18 | `System.IntPtr`                                                                                                              |
 | `ELEMENT_TYPE_U`           | 0x19 | `System.UIntPtr`                                                                                                             |
 | `ELEMENT_TYPE_FNPTR`       | 0x1b | 后跟完整的方法签名                                                                                                           |
 | `ELEMENT_TYPE_OBJECT`      | 0x1c | `System.Object`                                                                                                              |
 | `ELEMENT_TYPE_SZARRAY`     | 0x1d | 单维数组，下界为0                                                                                                            |
 | `ELEMENT_TYPE_MVAR`        | 0x1e | 泛型方法定义中的泛型参数，表示为 *number* (压缩的无符号整数)                                                                 |
 | `ELEMENT_TYPE_CMOD_REQD`   | 0x1f | 必需的修饰符：后跟 _TypeDef_ 或 _TypeRef_  _token_                                                                                |
 | `ELEMENT_TYPE_CMOD_OPT`    | 0x20 | 可选的修饰符：后跟 _TypeDef_ 或 _TypeRef_  _token_                                                                                |
 | `ELEMENT_TYPE_INTERNAL`    | 0x21 | 在CLI中实现                                                                                                                  |
 | `ELEMENT_TYPE_MODIFIER`    | 0x40 | 与后续元素类型进行或运算                                                                                                     |
 | `ELEMENT_TYPE_SENTINEL`    | 0x41 | vararg方法签名的哨兵                                                                                                         |
 | `ELEMENT_TYPE_PINNED`      | 0x45 | 表示指向固定对象的局部变量                                                                                                   |
 | &nbsp;                     | 0x50 | 表示类型为 `System.Type` 的参数。                                                                                            |
 | &nbsp;                     | 0x51 | 在自定义属性中用于指定装箱的对象 (§[II.23.3](ii.23.3-custom-attributes.md))。                                                |
 | &nbsp;                     | 0x52 | 保留                                                                                                                         |
 | &nbsp;                     | 0x53 | 在自定义属性中用于表示 `FIELD` (§[II.22.10](ii.22.10-customattribute-0x0c.md), [II.23.3](ii.23.3-custom-attributes.md))。    |
 | &nbsp;                     | 0x54 | 在自定义属性中用于表示 `PROPERTY` (§[II.22.10](ii.22.10-customattribute-0x0c.md), [II.23.3](ii.23.3-custom-attributes.md))。 |
 | &nbsp;                     | 0x55 | 在自定义属性中用于指定枚举 (§[II.23.3](ii.23.3-custom-attributes.md))。                                                      |

### 21.2. Blobs 和签名

*签名* 这个词通常用来描述函数或方法的类型信息；也就是，它的每个参数的类型，以及它的返回值的类型。在元数据中，签名这个词也用来描述字段、属性和局部变量的类型信息。每个签名都存储为 Blob 堆中的一个 (计数) 字节数组。有几种类型的签名，如下：

 * _MethodRefSig_  (只有在 `VARARG` 调用中才与 _MethodDefSig_ 不同) 

 * _MethodDefSig_

 * _FieldSig_

 * _PropertySig_

 * _LocalVarSig_

 * _TypeSpec_

 * _MethodSpec_

签名 'blob' 的第一个字节的值表示它是什么类型的签名。它的最低4位包含以下之一：`C`，`DEFAULT`，`FASTCALL`，`STDCALL`，`THISCALL`，或 `VARARG`  (其值在 §[II.23.2.3](ii.23.2.3-standalonemethodsig.md) 中定义)，这些都是方法签名的限定符；`FIELD`，表示字段签名 (其值在 §[II.23.2.4](ii.23.2.4-fieldsig.md) 中定义) ；或 `PROPERTY`，表示属性签名 (其值在 §[II.23.2.5](ii.23.2.5-propertysig.md) 中定义)。本小节定义了每种类型的签名的二进制 'blob' 格式。在伴随许多定义的语法图中，使用阴影将本来会是多个图的内容合并到一个图中；附带的文本描述了阴影的使用。

在将签名存储到 Blob 堆中之前，通过压缩签名中嵌入的整数来压缩签名 (如下所述)。可编码的无符号整数的最大长度为29位，0x1FFFFFFF。对于有符号整数，如在 ArrayShape (§[II.23.2.13](ii.23.2.13-arrayshape.md)) 中出现的，范围是 -2<sup>28</sup> (0xF0000000) 到 2<sup>28</sup>-1 (0x0FFFFFFF)。使用的压缩算法如下 (位0是最低有效位) ：

 * 对于无符号整数：

   * 如果值在0 (0x00) 和 127 (0x7F) 之间，包含两者，编码为一个字节的整数 (位7清零，值保存在位6到位0) 

   * 如果值在2<sup>8</sup> (0x80) 和 2<sup>14</sup>-1 (0x3FFF) 之间，包含两者，编码为一个2字节的整数，位15设为1，位14清零 (值保存在位13到位0) 

   * 否则，编码为一个4字节的整数，位31设为1，位30设为1，位29清零 (值保存在位28到位0) 

 * 对于有符号整数：

   * 如果值在 -2<sup>6</sup> 和 2<sup>6</sup>-1 之间，包含两者：

      * 将值表示为一个7位的2的补数，给出 0x40 (-2<sup>6</sup>) 到 0x3F (2<sup>6</sup>-1)；

      * 将这个值左移1位，给出 0x01 (-2<sup>6</sup>) 到 0x7E (2<sup>6</sup>-1)；

      * 编码为一个字节的整数，位7清零，旋转后的值在位6到位0，给出 0x01 (-2<sup>6</sup>) 到 0x7E (2<sup>6</sup>-1)。

   * 如果值在 -2<sup>13</sup> 和 2<sup>13</sup>-1 之间，包含两者：

      * 将值表示为一个14位的2的补数，给出 0x2000 (-2<sup>13</sup>) 到 0x1FFF (2<sup>13</sup>-1)；

      * 将这个值左移1位，给出 0x0001 (-2<sup>13</sup>) 到 0x3FFE (2<sup>13</sup>-1)；

      * 编码为一个两字节的整数：位15设为1，位14清零，旋转后的值在位13到位0，给出 0x8001 (-2<sup>13</sup>) 到 0xBFFE (2<sup>13</sup>-1)。

   * 如果值在 -2<sup>28</sup> 和 2<sup>28</sup>-1 之间，包含两者：

      * 将值表示为一个29位的2的补数，给出 0x10000000 (-2<sup>28</sup>) 到 0xFFFFFFF (2<sup>28</sup>-1)；

      * 将这个值左移1位，给出 0x00000001 (-2<sup>28</sup>) 到 0x1FFFFFFE (2<sup>28</sup>-1)；

      * 编码为一个四字节的整数：位31设为1，位30设为1，位29清零，旋转后的值在位28到位0，给出 0xC0000001 (-2<sup>28</sup>) 到 0xDFFFFFFE (2<sup>28</sup>-1)。

* 空字符串应该用保留的单字节0xFF表示，后面没有跟随的数据

_[注：_ 下面的表格显示了几个例子。第一列给出了一个值，以熟悉的 (类C) 十六进制表示法表示。第二列显示了相应的压缩结果，就像它会出现在PE文件中一样，结果的连续字节位于文件中逐渐增大的字节偏移处。 (这与在PE文件中布局常规二进制整数的顺序相反。) 

无符号示例：

 | 原始值      | 压缩表示     |
 | ----------- | ------------ |
 | 0x03        | 03           |
 | 0x7F        | 7F (7位设定) |
 | 0x80        | 8080         |
 | 0x2E57      | AE57         |
 | 0x3FFF      | BFFF         |
 | 0x4000      | C000 4000    |
 | 0x1FFF FFFF | DFFF FFFF    |

有符号示例：

 | 原始值     | 压缩表示  |
 | ---------- | --------- |
 | 3          | 06        |
 | -3         | 7B        |
 | 64         | 8080      |
 | -64        | 01        |
 | 8192       | C000 4000 |
 | -8192      | 8001      |
 | 268435455  | DFFF FFFE |
 | -268435456 | C000 0001 |

_结束注释]_

"压缩"字段的最高有效位 (在PE文件中首次遇到的位) 可以揭示它占用1、2还是4个字节，以及它的值。为了使这个工作，如上所述，"压缩"值以大端顺序存储；即，最高有效字节位于文件中的最小偏移处。

签名广泛使用名为`ELEMENT_TYPE_xxx`的常量值 - 参见§[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md)。特别地，签名包括两个修饰符，称为：

 * `ELEMENT_TYPE_BYREF` - 这个元素是一个托管指针 (参见 [Partition I]())。这个修饰符只能出现在_LocalVarSig_ (§[II.23.2.6](ii.23.2.6-localvarsig.md))，_Param_ (§[II.23.2.10](ii.23.2.10-param.md)) 或 _RetType_ (§[II.23.2.11](ii.23.2.11-rettype.md))的定义中。它不应该出现在_Field_ (§[II.23.2.4](ii.23.2.4-fieldsig.md))的定义中。

 * `ELEMENT_TYPE_PTR` - 这个元素是一个非托管指针 (参见 [Partition I]())。这个修饰符可以出现在_LocalVarSig_ (§[II.23.2.6](ii.23.2.6-localvarsig.md))，_Param_ (§[II.23.2.10](ii.23.2.10-param.md))，_RetType_ (§[II.23.2.11](ii.23.2.11-rettype.md)) 或 _Field_ (§[II.23.2.4](ii.23.2.4-fieldsig.md))的定义中。

#### 21.2.1. MethodDefSig

_MethodDefSig_ 由 _Method_._Signature_ 列索引。它捕获方法或全局函数的签名。_MethodDefSig_ 的语法图如下：

 ![](ii.23.2.1-methoddefsig-figure-1.png)

此图使用以下缩写：

 * `HASTHIS` = 0x20，用于在调用约定中编码关键字 **instance**，参见 §[II.15.3](ii.15.3-calling-convention.md)

 * `EXPLICITTHIS` = 0x40，用于在调用约定中编码关键字 **explicit**，参见 §[II.15.3](ii.15.3-calling-convention.md)

 * `DEFAULT` = 0x0，用于在调用约定中编码关键字 **default**，参见 §[II.15.3](ii.15.3-calling-convention.md)

 * `VARARG` = 0x5，用于在调用约定中编码关键字 **vararg**，参见 §[II.15.3](ii.15.3-calling-convention.md)

 * `GENERIC` = 0x10，用于表示方法有一个或多个泛型参数。

签名的第一个字节包含 `HASTHIS`，`EXPLICITTHIS` 和调用约定 (`DEFAULT`，`VARARG` 或 `GENERIC`) 的位。这些被一起 OR。

_GenParamCount_ 是方法的泛型参数的数量。这是一个压缩的无符号整数。

_[注意：对于泛型方法，_MethodDef_ 和 _MemberRef_ 都应包含 `GENERIC` 调用约定，以及 _GenParamCount_；这些对于绑定很重要——它们使 CLI 能够根据泛型方法包含的泛型参数的数量进行重载。结束注释]_

_ParamCount_ 是一个无符号整数，它保存参数的数量 (0个或更多)。它可以是0到0x1FFFFFFF之间的任何数字。编译器也会压缩它 (参见 §[15]()) ——在存储到 'blob' 之前 (ParamCount 只计算方法参数——它不包括方法的返回类型) 

_RetType_ 项描述方法的返回值的类型 (参见 §[II.23.2.11](ii.23.2.11-rettype.md)) 

_Param_ 项描述每个方法参数的类型。应该有 _ParamCount_ 个 _Param_ 项的实例 (参见 §[II.23.2.10](ii.23.2.10-param.md))。

#### 21.2.2. MethodRefSig

_MethodRefSig_ 是由 _MemberRef_._Signature_ 列索引的。这提供了方法的 *调用点* 签名。通常，这个调用点签名应该与目标方法定义中指定的签名完全匹配。例如，如果定义了一个方法 `Foo`，它接受两个 `unsigned int32` 并返回 `void`；那么任何调用点都应该索引一个签名，该签名接受恰好两个 `unsigned int32` 并返回 `void`。在这种情况下，_MethodRefSig_ 的语法图与 _MethodDefSig_ 的语法图相同 - 参见 §[II.23.2.1](ii.23.2.1-methoddefsig.md)

只有对于具有 `VARARG` 调用约定的方法，调用点的签名才会与其定义处的签名不同。在这种情况下，调用点签名被扩展以包含关于额外 `VARARG` 参数的信息 (例如，对应于 C 语法中的 "`...`")。这种情况的语法图如下：

 ![](ii.23.2.2-methodrefsig-figure-1.png)

此图使用以下缩写：

 * `HASTHIS` = 0x20，用于在调用约定中编码关键字 **instance**，参见 §[II.15.3](ii.15.3-calling-convention.md)

 * `EXPLICITTHIS` = 0x40，用于在调用约定中编码关键字 **explicit**，参见 §[II.15.3](ii.15.3-calling-convention.md)
 
 * `VARARG` = 0x5，用于在调用约定中编码关键字 **vararg**，参见 §[II.15.3](ii.15.3-calling-convention.md)
 
 * `SENTINEL` = 0x41 (§[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md))，用于在参数列表中编码 "`...`"，参见 §[II.15.3](ii.15.3-calling-convention.md)

签名的第一个字节保存了 `HASTHIS`，`EXPLICITTHIS` 和调用约定 `VARARG` 的位。这些位被 OR 在一起。

_ParamCount_ 是一个无符号整数，保存了参数的数量 (0 或更多)。它可以是 0 到 0x1FFFFFFF 之间的任何数字。在存储到 'blob' 中之前，编译器会压缩它 (参见 §15)  - (_ParamCount_ 只计算方法参数的数量 - 它不包括方法的返回类型)

_RetType_ 项描述了方法返回值的类型 (§[II.23.2.11](ii.23.2.11-rettype.md))

_Param_ 项描述了每个方法参数的类型。应该有 _ParamCount_ 个 _Param_ 项的实例 (§[II.23.2.10](ii.23.2.10-param.md))。

_Param_ 项描述了每个方法参数的类型。应该有 _ParamCount_ 个 _Param_ 项的实例。这开始就像一个 `VARARG` 方法的 _MethodDefSig_ (§[II.23.2.1](ii.23.2.1-methoddefsig.md))。但然后追加了一个 `SENTINEL`  _token_ ，后面跟着额外的 _Param_ 项来描述额外的 `VARARG` 参数。注意，_ParamCount_ 项应该指示签名中 _Param_ 项的总数 - 在 `SENTINEL` 字节 (0x41) 之前和之后。

在罕见的情况下，如果调用点没有提供额外的参数，签名不应包含 `SENTINEL` (这是由下箭头显示的路径，它绕过 `SENTINEL` 并到达 _MethodRefSig_ 定义的末尾)。


#### 21.2.3. StandAloneMethodSig

_StandAloneMethodSig_由_StandAloneSig_._Signature_列索引。它通常在执行`calli`指令之前创建。它类似于_MethodRefSig_，因为它表示一个调用点签名，但是它的调用约定可以指定一个非托管目标 (`calli`指令调用托管代码或非托管代码)。它的语法图如下：

 ![](ii.23.2.3-standalonemethodsig-figure-1.png)

此图使用以下缩写 (§[II.15.3](ii.15.3-calling-convention.md))：

 * `HASTHIS` 对应 0x20
 * `EXPLICITTHIS` 对应 0x40
 * `DEFAULT` 对应 0x0
 * `VARARG` 对应 0x5
 * `C` 对应 0x1
 * `STDCALL` 对应 0x2
 * `THISCALL` 对应 0x3
 * `FASTCALL` 对应 0x4
 * `SENTINEL` 对应 0x41 (§[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md) 和 §[II.15.3](ii.15.3-calling-convention.md))

签名的第一个字节包含`HASTHIS`，`EXPLICITTHIS`和调用约定的位 - `DEFAULT`，`VARARG`，`C`，`STDCALL`，`THISCALL`或`FASTCALL`。这些被一起进行或运算。

_ParamCount_是一个无符号整数，它包含了非变参和变参参数的数量，合并在一起。它可以是0到0x1FFFFFFF之间的任何数字。编译器会压缩它 (参见§[15]()) - 在存储到blob之前 (_ParamCount_只计算方法参数 - 它不包括方法的返回类型) 

_RetType_项描述了方法返回值的类型 (§[II.23.2.11](ii.23.2.11-rettype.md)) 

第一个_Param_项描述了每个方法的非变参参数的类型。第二个 (可选的) _Param_项描述了每个方法的变参参数的类型。应该有_ParamCount_个_Param_实例 (§[II.23.2.10](ii.23.2.10-param.md))。

这是各种方法签名中最复杂的一个。在这个图中，两个单独的图被合并成一个，使用阴影来区分它们。因此，对于以下的调用约定：`DEFAULT` (托管)，`STDCALL`，`THISCALL`和`FASTCALL` (非托管)，签名在`SENTINEL`项之前就结束了 (这些都是非变参签名)。然而，对于托管和非托管的变参调用约定：

`VARARG` (托管) 和`C` (非托管)，签名可以包含SENTINEL和最后的_Param_项 (然而，它们并不是必需的)。这些选项由语法图中的框的阴影表示。

在罕见的情况下，如果调用点没有提供额外的参数，签名不应该包含`SENTINEL` (这是由下箭头显示的路径，它绕过`SENTINEL`并到达_StandAloneMethodSig_定义的结束)。

#### 21.2.4. FieldSig

_FieldSig_ 由 _Field_._Signature_ 列索引，或由 _MemberRef_._Signature_ 列索引 (当然，这是在它指定对字段的引用，而不是方法的情况下)。签名捕获了字段的定义。字段可以是类中的静态或实例字段，也可以是全局变量。_FieldSig_ 的语法图如下：

 ![](ii.23.2.4-fieldsig-figure-1.png)

此图使用以下缩写：

 * `FIELD` 代表 0x6

_CustomMod_ 在 §[II.23.2.7](ii.23.2.7-custommod.md) 中定义。_Type_ 在 §[II.23.2.12](ii.23.2.12-type.md) 中定义。

#### 21.2.5. PropertySig

_PropertySig_ 是由 _Property_._Type_ 列索引的。它捕获了属性的类型信息 - 本质上，它是其 _getter_ 方法的签名：

 * 提供给其 *getter* 方法的参数数量

 * 属性的基类型 (_getter_ 方法返回的类型) 
 
 * *getter* 方法中每个参数的类型信息 (即，索引参数) 

注意，getter 和 setter 的签名之间的关系如下：

 * *getter* 的 _ParamCount_ 参数的类型与 setter 的前 _ParamCount_ 参数的类型完全相同

 * *getter* 的返回类型与提供给 *setter* 的最后一个参数的类型完全相同

_PropertySig_ 的语法图如下：

 ![](ii.23.2.5-propertysig-figure-1.png)

签名的第一个字节保存了 `HASTHIS` 和 `PROPERTY` 的位。这些位被 OR 在一起。

_Type_ 指定了此属性的 *getter* 方法返回的类型。_Type_ 在 §[II.23.2.12](ii.23.2.12-type.md) 中定义。

_Param_ 在 §[II.23.2.10](ii.23.2.10-param.md) 中定义。

_ParamCount_ 是一个压缩的无符号整数，保存了 getter 方法中的索引参数数量 (0 或更多)。(§[II.23.2.1](ii.23.2.1-methoddefsig.md)) (_ParamCount_ 只计算方法参数的数量 - 它不包括方法的属性的基类型)

_LocalVarSig_由_StandAloneSig_._Signature_列索引。它捕获了一个方法中所有局部变量的类型。它的语法图如下：

 ![](ii.23.2.6-localvarsig-figure-1.png)

此图使用以下缩写：

 * `LOCAL_SIG` 对应 0x7，用于 **.locals** 指令，参见 §[II.15.4.1.3](ii.15.4.1.3-the-locals-directive.md)

 * `BYREF` 对应 `ELEMENT_TYPE_BYREF` (§[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md))

_Constraint_ 在 §[II.23.2.9](ii.23.2.9-constraint.md) 中定义。

_Type_ 在 §[II.23.2.12](ii.23.2.12-type.md) 中定义。

_Count_ 是一个压缩的无符号整数，它保存了局部变量的数量。它可以是1到0xFFFE之间的任何数字。

_LocalVarSig_中应该有 _Count_ 个 _Type_ 实例。

#### 21.2.6. LocalVarSig

_LocalVarSig_ 由 _StandAloneSig_._Signature_ 列索引。它捕获了方法中所有局部变量的类型。它的语法图如下：

 ![](ii.23.2.6-localvarsig-figure-1.png)

此图使用以下缩写：

 * `LOCAL_SIG` 代表 0x7，用于 **.locals** 指令，参见 §[II.15.4.1.3](ii.15.4.1.3-the-locals-directive.md)

 * `BYREF` 代表 `ELEMENT_TYPE_BYREF` (参见 §[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md)) 

_Constraint_ 在 §[II.23.2.9](ii.23.2.9-constraint.md) 中定义。

_Type_ 在 §[II.23.2.12](ii.23.2.12-type.md) 中定义。

_Count_ 是一个压缩的无符号整数，它保存了局部变量的数量。它可以是1到0xFFFE之间的任何数字。

_LocalVarSig_ 中应该有 _Count_ 个 _Type_ 的实例。


#### 21.2.7. CustomMod

_Signatures_ 中的 _CustomMod_ (自定义修饰符) 项的语法图如下：

 ![](ii.23.2.7-custommod-figure-1.png)

此图使用以下缩写：

 * `CMOD_OPT` 代表 `ELEMENT_TYPE_CMOD_OPT` (参见 §[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md)) 

 * `CMOD_REQD` 代表 `ELEMENT_TYPE_CMOD_REQD` (参见 §[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md)) 

`CMOD_OPT` 或 `CMOD_REQD` 的值是压缩的，参见 §[II.23.2](ii.23.2-blobs-and-signatures.md)。

`CMOD_OPT` 或 `CMOD_REQD` 后面跟着一个元数据 _token_ ，该 _token_ 索引 _TypeDef_ 表或 _TypeRef_ 表中的一行。然而，这些 _token_ 是编码和压缩的 - 详情参见 §[II.23.2.8](ii.23.2.8-typedeforreforspecencoded.md)

如果 CustomModifier 标记为 `CMOD_OPT`，那么任何导入编译器都可以完全忽略它。相反，如果 CustomModifier 标记为 `CMOD_REQD`，任何导入编译器都应该“理解”此 CustomModifier 所暗示的语义，以便引用周围的 Signature。


#### 21.2.8. TypeDefOrRefOrSpecEncoded

这些项是在签名中存储 _TypeDef_，_TypeRef_ 或 _TypeSpec_  _token_ 的紧凑方式 (§[II.23.2.12](ii.23.2.12-type.md))。考虑一个常规的 _TypeRef_  _token_ ，例如 0x01000012。最高字节 0x01 表示这是一个 _TypeRef_  _token_  (参见 §[II.22](ii.22-metadata-logical-format-tables.md) 中支持的元数据 _token_ 类型列表)。较低的3字节 (0x000012) 索引 _TypeRef_ 表中的行号 0x12。

这个 _TypeRef_  _token_ 的编码版本如下构造：

 1. 将此 _token_ 索引的表编码为最低有效的2位。要使用的位值是0，1和2，分别指定目标表是 _TypeDef_，_TypeRef_ 或 _TypeSpec_ 表。

 2. 将3字节行索引 (在此示例中为 0x000012) 左移2位，并将其与步骤1中的2位编码进行 OR 操作。

 3. 压缩结果值 (§[II.23.2](ii.23.2-blobs-and-signatures.md))。

此示例产生以下编码值：

 ```
 a)  编码 = TypeRef 表的值 = 0x01 (来自上述 1.) 
 b)  编码 = ( 0x000012 << 2 ) |  0x01
             = 0x48 | 0x01
             = 0x49
 c)  编码 = Compress (0x49)
             = 0x49
 ```

所以，与原始的，常规的 _TypeRef_  _token_ 值 0x01000012 不同，需要在签名 'blob' 中占用4字节的空间，这个 _TypeRef_  _token_ 被编码为一个单字节。

#### 21.2.9. Constraint

在签名中，_Constraint_项目前只有一个可能的值，即`ELEMENT_TYPE_PINNED` (§[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md))，它指定目标类型在运行时堆中被固定，不会被垃圾收集的操作移动。

_Constraint_只能在_LocalVarSig_中应用 (不能在_FieldSig_中应用)。局部变量的类型要么是引用类型 (换句话说，它*指向*实际的变量 - 例如，一个对象或一个字符串) ；要么包含`BYREF`项。原因是局部变量分配在运行时栈上 - 它们从不从运行时堆中分配；所以除非局部变量*指向*在GC堆中分配的对象，否则固定没有意义。

#### 21.2.10. Param

签名中的 _Param_  (参数) 项有以下语法图：

 ![](ii.23.2.10-param-figure-1.png)

此图使用以下缩写：

 * `BYREF` 代表 0x10 (§[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md))

 * `TYPEDBYREF` 代表 0x16 (§[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md))

_CustomMod_ 在 §[II.23.2.7](ii.23.2.7-custommod.md) 中定义

_Type_ 在 §[II.23.2.12](ii.23.2.12-type.md) 中定义。

#### 21.2.11. RetType

_RetType_ (返回类型) 项在签名中的语法图如下：

 ![](ii.23.2.11-rettype-figure-1.png)

_RetType_与_Param_相同，除了它可以包含类型`VOID`的额外可能性。

此图使用以下缩写：

 * `BYREF` 对应 `ELEMENT_TYPE_BYREF` (§[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md))

 * `TYPEDBYREF` 对应 `ELEMENT_TYPE_TYPEDBYREF` (§[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md))

 * `VOID` 对应 `ELEMENT_TYPE_VOID` (§[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md))。

#### 21.2.12. 类型

_Type_ 在签名中的编码如下 (`I1` 是 `ELEMENT_TYPE_I1` 的缩写，`U1` 是 `ELEMENT_TYPE_U1` 的缩写，依此类推；参见 [II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md)) ：

 | _Type_ ::=
 | ----
 | `BOOLEAN` \| `CHAR` \| `I1` \| `U1` \| `I2` \| `U2` \| `I4` \| `U4` \| `I8` \| `U8` \| `R4` \| `R8` \| `I` \| `U`
 | \| `ARRAY` _Type_ _ArrayShape_ (通用数组，参见 §[II.23.2.13](ii.23.2.13-arrayshape.md))
 | \| `CLASS` _TypeDefOrRefOrSpecEncoded_
 | \| `FNPTR` _MethodDefSig_
 | \| `FNPTR` _MethodRefSig_
 | \| `GENERICINST` (`CLASS` \| `VALUETYPE`) _TypeDefOrRefOrSpecEncoded_ _GenArgCount_ _Type_* 
 | \| `MVAR` _number_
 | \| `OBJECT` 
 | \| `PTR` _CustomMod_* _Type_
 | \| `PTR` _CustomMod_* `VOID` 
 | \| `STRING`
 | \| `SZARRAY` _CustomMod_* _Type_ (单维，零基数组，即向量) 
 | \| `VALUETYPE` _TypeDefOrRefOrSpecEncoded_
 | \| `VAR` _number_

_GenArgCount_ 非终结符是一个无符号整数值 (压缩)，指定此签名中的泛型参数的 _数量_。在 `MVAR` 或 `VAR` 后面的 number 非终结符是一个无符号整数值 (压缩)。

Source: Conversation with Bing, 2023/12/25
(1) github.com. https://github.com/stakx/ecma-335/tree/68d5015b146d347b2d76bd67d150af5f3fa68178/docs%2Fii.23.2.12-type.md.

#### 21.2.13. ArrayShape

_ArrayShape_ 的语法图如下：

 ![](ii.23.2.13-arrayshape-figure-1.png)

_Rank_ 是一个无符号整数 (以压缩形式存储，参见 §[II.23.2](ii.23.2-blobs-and-signatures.md))，它指定数组的维数 (应为1个或更多)。

_NumSizes_ 是一个压缩的无符号整数，表示有多少维度有指定的大小 (应为0个或更多)。

_Size_ 是一个压缩的无符号整数，指定该维度的大小 - 序列从第一维开始，总共有 _NumSizes_ 个项目。

类似地，_NumLoBounds_ 是一个压缩的无符号整数，表示有多少维度有指定的下界 (应为0个或更多)。

_LoBound_ 是一个压缩的有符号整数，指定该维度的下界 - 序列从第一维开始，总共有 _NumLoBounds_ 个项目。

这两个序列中的任何维度都不能被跳过，但指定的维度数量可以小于 _Rank_。

以下是一些示例，所有示例的元素类型都是 `int32`：

 | &nbsp;               | 类型 | 秩  | NumSizes | 大小   | NumLoBounds | 下界   |
 | -------------------- | ---- | --- | -------- | ------ | ----------- | ------ |
 | `[0...2]`            | `I4` | `1` | `1`      | `3`    | `0`         | &nbsp; |
 | [,,,,,,]             | `I4` | `7` | `0`      | &nbsp; | `0`         | &nbsp; |
 | `[0...3, 0...2,,,,]` | `I4` | `6` | `2`      | `4 3`  | `2`         | `0 0`  |
 | `[1...2, 6...8]`     | `I4` | `2` | `2`      | `2 3`  | `2`         | `1 6`  |
 | `[5, 3...5, , ]`     | `I4` | `4` | `2`      | `5 3`  | `2`         | `0 3`  |
 
_[注意：定义可以嵌套，因为类型本身可以是数组。结束注释]_

#### 21.2.14. 类型规范(TypeSpec)

在Blob堆中，由_TypeSpec_ _token_ 索引的签名具有以下格式：

 | _TypeSpecBlob_ ::=
 | ----
 | `PTR` _CustomMod_* `VOID` 
 |  \| `PTR` _CustomMod_* _Type_
 | \| `FNPTR` _MethodDefSig_
 | \| `FNPTR` _MethodRefSig_
 | \| `ARRAY` _Type_ _ArrayShape_
 | \| `SZARRAY` _CustomMod_* _Type_
 | \| `GENERICINST` ( `CLASS` \| `VALUETYPE` ) _TypeDefOrRefOrSpecEncoded_ _GenArgCount_ _Type_ _Type_*

为了紧凑，此列表省略了`ELEMENT_TYPE_`前缀。所以，例如，`PTR`是`ELEMENT_TYPE_PTR`的简写。(§[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md)) 请注意，TypeSpecBlob不以调用约定字节开始，因此它与存储到元数据中的各种其他签名不同。

#### 21.2.15. MethodSpec

由 _MethodSpec_  _token_ 索引的 Blob 堆中的签名具有以下格式

 | _MethodSpecBlob_ ::=
 | ----
 | `GENERICINST` _GenArgCount_ _Type_ _Type_*

`GENERICINST` 的值为 0x0A。 _[注：_ 在 Microsoft CLR 实现中，这个值被称为 `IMAGE_CEE_CS_CALLCONV_GENERICINST`。 _结束注]_

_GenArgCount_ 是一个压缩的无符号整数，表示方法中的泛型参数的数量。然后，blob 指定了实例化的类型，重复 _GenArgCount_ 次。

#### 21.2.16. 短格式签名

签名的一般规范在如何编码某些项上留有一些余地。例如，将 String 编码为以下两种形式之一似乎是有效的：

 * 长格式：(`ELEMENT_TYPE_CLASS`, TypeRef-to-`System.String`)
 * 短格式：`ELEMENT_TYPE_STRING`

只有短格式是有效的。下表显示了应该使用哪些短格式来代替每个长格式项。 (通常，为了紧凑，这里省略了 `ELEMENT_TYPE_` 前缀 - 所以 `VALUETYPE` 是 `ELEMENT_TYPE_VALUETYPE` 的简写) 

 | 长格式      | &nbsp;                  | 短格式       |
 | ----------- | ----------------------- | ------------ |
 | **前缀**    | **TypeRef 到：**        |
 | `CLASS`     | `System.String`         | `STRING`     |
 | `CLASS`     | `System.Object`         | `OBJECT`     |
 | `VALUETYPE` | `System.Void`           | `VOID`       |
 | `VALUETYPE` | `System.Boolean`        | `BOOLEAN`    |
 | `VALUETYPE` | `System.Char`           | `CHAR`       |
 | `VALUETYPE` | `System.Byte`           | `U1`         |
 | `VALUETYPE` | `System.Sbyte`          | `I1`         |
 | `VALUETYPE` | `System.Int16`          | `I2`         |
 | `VALUETYPE` | `System.UInt16`         | `U2`         |
 | `VALUETYPE` | `System.Int32`          | `I4`         |
 | `VALUETYPE` | `System.UInt32`         | `U4`         |
 | `VALUETYPE` | `System.Int64`          | `I8`         |
 | `VALUETYPE` | `System.UInt64`         | `U8`         |
 | `VALUETYPE` | `System.IntPtr`         | `I`          |
 | `VALUETYPE` | `System.UIntPtr`        | `U`          |
 | `VALUETYPE` | `System.TypedReference` | `TYPEDBYREF` |

### 21.3. 自定义属性

自定义属性具有以下语法图：

 ![](ii.23.3-custom-attributes-figure-1.png)

所有二进制值都以小端格式存储 (除了_PackedLen_项目，它们仅用作后续UTF8字符串中字节数的计数)。如果没有指定字段、参数或属性，则整个属性表示为空blob。

_CustomAttrib_以Prolog开始 - 一个无符号的*int16*，值为0x0001。

接下来是构造方法的固定参数的描述。通过检查_MethodDef_表中该构造函数的行，可以找到它们的数量和类型；这些信息在_CustomAttrib_本身中并未重复。如语法图所示，可以有零个或多个_FixedArgs_。 (注意，`VARARG`构造方法在自定义属性的定义中是不允许的。) 

接下来是可选的"命名"字段和属性的描述。这开始于_NumNamed_ - 一个无符号的int16，给出后面跟随的"命名"属性或字段的数量。注意，_NumNamed_总是存在的。值为零表示没有要跟随的"命名"属性或字段 (当然，在这种情况下，_CustomAttrib_将在_NumNamed_之后立即结束)。在_NumNamed_为非零的情况下，它后面跟随_NumNamed_重复的_NamedArgs_。

 ![](ii.23.3-custom-attributes-figure-2.png)

每个_FixedArg_的格式取决于该参数是否为`SZARRAY` - 这在语法图的下方和上方路径中分别显示。因此，每个_FixedArg_要么是单个_Elem_，要么是_NumElem_重复的_Elem_。

(`SZARRAY`是单字节0x1D，表示向量 - 即下界为零的单维数组。)

_NumElem_是一个无符号的_int32_，指定`SZARRAY`中的元素数量，或者0xFFFFFFFF表示该值为null。

 ![](ii.23.3-custom-attributes-figure-3.png)

_Elem_采用此图中的一种形式，如下所示：

 * 如果参数种类是简单的 (上述图表的第一行)  (**bool**，**char**，**float32**，**float64**，**int8**，**int16**，**int32**，**int64**，**unsigned int8**，**unsigned int16**，**unsigned int32**或**unsigned int64**)，那么'blob'包含其二进制值 (_Val_)。(*bool*是一个字节，值为0 (假) 或1 (真) ；*char*是一个两字节的Unicode字符；其他的含义很明显。)如果参数种类是*enum*，也使用这种模式 - 只需存储枚举的底层整数类型的值。

 * 如果参数种类是_string_， (上述图表的中间行) 那么blob包含一个_SerString_ - 一个_PackedLen_字节计数，后面跟着UTF8字符。如果字符串为null，其_PackedLen_的值为0xFF (没有后续字符)。如果字符串为空 ("")，那么_PackedLen_的值为0x00 (没有后续字符)。

 * 如果参数种类是`System.Type`， (同样，上述图表的中间行) 其值以_SerString_的形式存储 (如上一段所定义)，表示其规范名称。规范名称是其完整类型名称，后面可选地跟着定义它的程序集，其版本，文化和公钥 _token_ 。如果省略了程序集名称，CLI首先在当前程序集中查找，然后在系统库 (`mscorlib`) 中查找；在这两种特殊情况下，允许省略程序集名称，版本，文化和公钥 _token_ 。

 * 如果参数种类是`System.Object`， (上述图表的第三行) 存储的值表示该值类型的"装箱"实例。在这种情况下，blob包含实际类型的_FieldOrPropType_ (见下文)，后面跟着参数的未装箱值。_[注意：_在这种情况下，不可能传递null值。_结束注释]_

* 如果类型是一个装箱的简单值类型 (**bool**，**char**，**float32**，**float64**，**int8**，**int16**，**int32**，**int64**，**unsigned int8**，**unsigned int16**，**unsigned int32** 或 **unsigned int64**)，那么 _FieldOrPropType_ 紧接着是一个字节，该字节包含值 0x51。_FieldOrPropType_ 必须恰好是以下之一：`ELEMENT_TYPE_BOOLEAN`，`ELEMENT_TYPE_CHAR`，`ELEMENT_TYPE_I1`，`ELEMENT_TYPE_U1`，`ELEMENT_TYPE_I2`，`ELEMENT_TYPE_U2`，`ELEMENT_TYPE_I4`，`ELEMENT_TYPE_U4`，`ELEMENT_TYPE_I8`，`ELEMENT_TYPE_U8`，`ELEMENT_TYPE_R4`，`ELEMENT_TYPE_R8`，`ELEMENT_TYPE_STRING`。单维，零基数组被指定为一个字节 0x1D，后面跟着元素类型的 _FieldOrPropTypeof_。 (参见 §[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md)) 枚举被指定为一个字节 0x55，后面跟着一个 _SerString_。

 ![](ii.23.3-custom-attributes-figure-4.png)

_NamedArg_ 只是一个 _FixedArg_ (上面讨论过)，前面有信息来标识它代表哪个字段或属性。_[注：_ 记住，CLI 允许字段和属性具有相同的名称；所以我们需要一种方法来消除这种情况的歧义。_结束注]_

 * `FIELD` 是单字节 0x53。

 * `PROPERTY` 是单字节 0x54。

_FieldOrPropName_ 是字段或属性的名称，存储为 _SerString_ (上面定义)。涉及自定义属性的一些示例包含在 [Partition VI 的附录 B]() 中。

### 21.4. 编组描述符

编组描述符类似于签名 - 它是二进制数据的 'blob'。它描述了当通过 PInvoke 调度调用到或从非托管代码时，应如何编组字段或参数 (通常情况下，作为参数编号 0 的方法返回也包括在内)。ILAsm 语法 **marshal** 可以用来创建编组描述符，伪自定义属性 `MarshalAsAttribute` 也可以用来创建编组描述符 - 参见 §[II.21.2.1](ii.21.2.1-pseudo-custom-attributes.md)。

注意，CLI 的符合规范的实现只需要支持编组前面指定的类型 - 参见 §[II.15.5.4](ii.15.5.4-data-type-marshaling.md)。

编组描述符使用名为 `NATIVE_TYPE_xxx` 的常量。它们的名称和值列在下表中：

 | 名称                  | 值   |
 | --------------------- | ---- |
 | `NATIVE_TYPE_BOOLEAN` | 0x02 |
 | `NATIVE_TYPE_I1`      | 0x03 |
 | `NATIVE_TYPE_U1`      | 0x04 |
 | `NATIVE_TYPE_I2`      | 0x05 |
 | `NATIVE_TYPE_U2`      | 0x06 |
 | `NATIVE_TYPE_I4`      | 0x07 |
 | `NATIVE_TYPE_U4`      | 0x08 |
 | `NATIVE_TYPE_I8`      | 0x09 |
 | `NATIVE_TYPE_U8`      | 0x0a |
 | `NATIVE_TYPE_R4`      | 0x0b |
 | `NATIVE_TYPE_R8`      | 0x0c |
 | `NATIVE_TYPE_LPSTR`   | 0x14 |
 | `NATIVE_TYPE_LPWSTR`  | 0x15 |
 | `NATIVE_TYPE_INT`     | 0x1f |
 | `NATIVE_TYPE_UINT`    | 0x20 |
 | `NATIVE_TYPE_FUNC`    | 0x26 |
 | `NATIVE_TYPE_ARRAY`   | 0x2a |

'blob' 的格式如下

 | _MarshalSpec_ ::=
 | ----
 | _NativeIntrinsic_
 | \| `ARRAY` _ArrayElemType_
 | \| `ARRAY` _ArrayElemType_ _ParamNum_
 | \| `ARRAY` _ArrayElemType_ _ParamNum_ _NumElem_

 | _NativeIntrinsic_ ::=
 | ----
 | `BOOLEAN` \| `I1` \| `U1` \| `I2` \| `U2` \| `I4` \| `U4` \| `I8` \| `U8` \| `R4` \| `R8` \| `LPSTR` \| `LPSTR` \| `INT` \| `UINT` \| `FUNC`

为了紧凑，上述列表中省略了 `NATIVE_TYPE_` 前缀；例如，`ARRAY` 是 `NATIVE_TYPE_ARRAY` 的简写。

 | _ArrayElemType_ ::=
 | ----
 | _NativeIntrinsic_

_ParamNum_ 是一个无符号整数 (以 §[II.23.2](ii.23.2-blobs-and-signatures.md) 描述的方式压缩)，指定在方法调用中提供数组中元素数量的参数 - 参见下文。

_NumElem_ 是一个无符号整数 (以 §[II.23.2](ii.23.2-blobs-and-signatures.md) 描述的方式压缩)，指定元素或附加元素的数量 - 参见下文。

_[注意：例如，在方法声明中：

 ```ilasm
 .method void M(int32[] ar1, int32 size1, unsigned int8[] ar2, int32 size2) { … }
 ```

`ar1` 参数可能拥有 _FieldMarshal_ 表中的一行，该行索引 Blob 堆中的 _MarshalSpec_，格式为：

 ```
 ARRAY  MAX  2  1
 ```

这表示参数被编组为 `NATIVE_TYPE_ARRAY`。关于每个元素的类型没有额外的信息 (由 `NATIVE_TYPE_MAX` 表示)。_ParamNum_ 的值为 2，这表示方法中的参数编号 2 (名为 `size1` 的参数) 将指定实际数组中的元素数量 - 假设在特定调用中其值为 42。_NumElem_ 的值为 0。数组的总大小 (以字节为单位) 由以下公式给出：

 ```
 if ParamNum = 0
    SizeInBytes = NumElem * sizeof (elem)
 else
    SizeInBytes = ( @ParamNum +  NumElem ) * sizeof (elem)
 endif
 ```

这里使用 `@ParamNum` 语法表示传入参数编号 _ParamNum_ 的值 - 在这个例子中，它将是 42。每个元素的大小是从 `Foo` 的签名中的 `ar1` 参数的元数据计算出来的 - 是大小为 4 字节的 `ELEMENT_TYPE_I4` (参见 §[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md))。结束注释]_

源: 与必应的对话， 2023/12/25
(1) github.com. https://github.com/stakx/ecma-335/tree/68d5015b146d347b2d76bd67d150af5f3fa68178/docs%2Fii.23.4-marshalling-descriptors.md.


## 22. 元数据物理布局
<a id="metadata-physical-layout"></a>

元数据的物理磁盘表示是对逻辑表示的直接反映，逻辑表示在 §[II.22](ii.22-metadata-logical-format-tables.md) 和 §[II.23](ii.23-metadata-logical-format-other-structures.md) 中描述。也就是说，数据存储在表示元数据表和堆的流中。主要的复杂性在于，逻辑表示从索引到表和列所需的字节数中抽象出来，而物理表示必须通过定义如何将逻辑元数据堆和表映射到它们的物理表示来明确处理这个问题。

除非另有说明，所有的二进制值都以小端格式存储。

### 22.1. 固定字段

完整的CLI组件 (元数据和CIL指令) 存储在当前可移植可执行 (PE) 文件格式的一个子集中 (§[II.25](ii.25-file-format-extensions-to-pe.md))。由于这种遗产，元数据的物理表示中的一些字段具有固定值。在写入这些字段时，最好将它们设置为指示的值，读取时应忽略它们。

### 22.2. File headers

#### 22.2.1. 元数据根

物理元数据的根开始于一个魔术签名，接着是几个字节的版本和其他杂项信息，然后是一个计数和一个流头数组，每个流都有一个。实际的编码表和堆存储在流中，这些流紧接着这个头数组。

 | 偏移     | 大小    | 字段              | 描述                                                                                                                                                             |
 | -------- | ------- | ----------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------- |
 | 0        | 4       | **Signature**     | 物理元数据的魔术签名：0x424A5342。                                                                                                                               |
 | 4        | 2       | **MajorVersion**  | 主版本，1 (读取时忽略)                                                                                                                                           |
 | 6        | 2       | **MinorVersion**  | 次版本，1 (读取时忽略)                                                                                                                                           |
 | 8        | 4       | **Reserved**      | 保留，始终为0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                                                                            |
 | 12       | 4       | **Length**        | 分配用来保存版本字符串 (包括空终止符) 的字节数，称之为 *x*。<p>称字符串 (包括终止符) 的长度为 *m* (我们要求 *m* &le; 255) ；长度 *x* 是 *m* 向上舍入到四的倍数。 |
 | 16       | *m*     | **Version**       | 长度为 *m* 的 UTF8 编码的空终止版本字符串 (见上文)                                                                                                               |
 | 16+*m*   | *x*-*m* | &nbsp;            | 填充到下一个4字节边界。                                                                                                                                          |
 | 16+*x*   | 2       | **Flags**         | 保留，始终为0 (§[II.24.1](ii.24.1-fixed-fields.md))。                                                                                                            |
 | 16+*x*+2 | 2       | **Streams**       | 流的数量，比如说 *n*。                                                                                                                                           |
 | 16+*x*+4 | &nbsp;  | **StreamHeaders** | *n* 个 `StreamHdr` 结构的数组。                                                                                                                                  |

对于任何打算在 CLI 的任何符合规范的实现上执行的文件，版本字符串应为 "`Standard CLI 2005`"，所有符合规范的 CLI 实现都应接受使用此版本字符串的文件。当文件受限于特定于供应商的 CLI 实现时，应使用其他字符串。此标准的未来版本将指定不同的字符串，但它们应以 "`Standard CLI`" 开始。指定附加功能的其他标准应指定以 "`Standard□`" 开始的特定版本字符串，其中 "`□`" 表示一个空格。提供实现特定扩展的供应商应提供一个不以 "`Standard□`" 开始的版本字符串。 (对于此标准的第一个版本，版本字符串是 "`Standard CLI 2002`"。) 

#### 22.2.2. 流头

流头提供了表或堆的名称，位置和长度。请注意，流头结构的长度不是固定的，而取决于其名称字段的长度 (一个可变长度的空终止字符串)。

 | 偏移 | 大小   | 字段       | 描述                                                                                             |
 | ---- | ------ | ---------- | ------------------------------------------------------------------------------------------------ |
 | 0    | 4      | **Offset** | 从元数据根的开始到此流的开始的内存偏移 (§[II.24.2.1](ii.24.2.1-metadata-root.md))                |
 | 4    | 4      | **Size**   | 此流的字节大小，应为4的倍数。                                                                    |
 | 8    | &nbsp; | **Name**   | 流的名称，为ASCII字符的空终止可变长度数组，用`\0`字符填充到下一个4字节边界。名称限制为32个字符。 |

逻辑表和堆都存储在流中。有五种可能的流。一个名为"`#Strings`"的流头，指向存储标识符字符串的字符串堆的物理表示；一个名为"`#US`"的流头，指向用户字符串堆的物理表示；一个名为"`#Blob`"的流头，指向blob堆的物理表示，一个名为"`#GUID`"的流头，指向GUID堆的物理表示；以及一个名为"`#~`"的流头，指向一组表的物理表示。

每种类型的流最多只能出现一次，也就是说，元数据文件不应包含两个"`#US`"流，或五个"`#Blob`"流。如果流是空的，则不需要存在。

下一小节将更详细地描述每种类型的流的结构。

#### 22.2.3. #Strings 堆

由 "`#Strings`" 标头指向的字节流是逻辑字符串堆的物理表示。物理堆可以包含垃圾，也就是说，它可以包含从任何表都无法访问的部分，但是从表可以访问的部分应该包含一个有效的空终止 UTF8 字符串。当 #Strings 堆存在时，第一个条目总是空字符串 (即，`\0`)。

#### 22.2.4. #US 和 #Blob 堆

由"`#US`"或"`#Blob`"头指向的字节流分别是逻辑Userstring和'blob'堆的物理表示。只要从任何表中可达的任何部分包含有效的'blob'，这两个堆都可以包含垃圾。单个blob的长度在前几个字节中编码：

 * 如果'blob'的第一个字节是_0bbbbbbb<sub>2</sub>_，那么'blob'的其余部分包含_bbbbbbb<sub>2</sub>_字节的实际数据。

 * 如果'blob'的前两个字节是_10bbbbbb<sub>2</sub>_和*x*，那么'blob'的其余部分包含(_bbbbbb<sub>2</sub>_ << 8 + *x*)字节的实际数据。

 * 如果'blob'的前四个字节是_110bbbbb<sub>2</sub>_, *x*, *y*, 和 *z*，那么'blob'的其余部分包含(_bbbbb<sub>2</sub>_ << 24 + *x* << 16 + *y* << 8 + *z*)字节的实际数据。

这两个堆中的第一个条目是空的'blob'，由单个字节0x00组成。

 `#US` (用户字符串) 堆中的字符串使用16位Unicode编码。每个字符串的计数是字符串中的字节数 (不是字符)。此外，还有一个额外的终止字节 (因此所有字节计数都是奇数，而不是偶数)。这个最后的字节只有在字符串中的任何UTF16字符的顶字节设置了任何位，或者其低字节是以下任何一个：0x01&ndash;0x08, 0x0E&ndash;0x1F, 0x27, 0x2D, 0x7F时，才持有值1。否则，它持有0。1表示需要超出通常为8位编码集提供的处理的Unicode字符。

#### 22.2.5. #GUID 堆
<a id="guid-heap"></a>

"`#GUID`" 标头指向一系列 128 位的 GUID。在流中可能存储了无法访问的 GUID。

#### 22.2.6. #~ 流
<a id="physical-stream"></a>

"`#~`" 流包含逻辑元数据表的实际物理表示 (参见 §[II.22](ii.22-metadata-logical-format-tables.md))。"`#~`" 流具有以下顶级结构：

 | 偏移量    | 大小   | 字段             | 描述                                                               |
 | --------- | ------ | ---------------- | ------------------------------------------------------------------ |
 | 0         | 4      | **Reserved**     | 保留，始终为0 (参见 §[II.24.1](ii.24.1-fixed-fields.md))。         |
 | 4         | 1      | **MajorVersion** | 表模式的主版本；应为2 (参见 §[II.24.1](ii.24.1-fixed-fields.md))。 |
 | 5         | 1      | **MinorVersion** | 表模式的次版本；应为0 (参见 §[II.24.1](ii.24.1-fixed-fields.md))。 |
 | 6         | 1      | **HeapSizes**    | 堆大小的位向量。                                                   |
 | 7         | 1      | **Reserved**     | 保留，始终为1 (参见 §[II.24.1](ii.24.1-fixed-fields.md))。         |
 | 8         | 8      | **Valid**        | 存在表的位向量，让 *n* 是1的位的数量。                             |
 | 16        | 8      | **Sorted**       | 已排序表的位向量。                                                 |
 | 24        | 4\**n* | **Rows**         | 指示每个存在表的行数的 *n* 个4字节无符号整数的数组。               |
 | 24+4\**n* | &nbsp; | **Tables**       | 物理表的序列。                                                     |

_HeapSizes_ 字段是一个位向量，它编码了各种堆的索引的宽度。如果位0被设置，"`#Strings`" 堆的索引宽度为4字节；如果位1被设置，"`#GUID`" 堆的索引宽度为4字节；如果位2被设置，"`#Blob`" 堆的索引宽度为4字节。相反，如果未设置特定堆的 _HeapSizes_ 位，那么该堆的索引宽度为2字节。

 | 堆大小标志 | 描述                                        |
 | ---------- | ------------------------------------------- |
 | 0x01       | "`#Strings`" 流的大小 &ge; 2<sup>16</sup>。 |
 | 0x02       | "`#GUID`" 流的大小 &ge; 2<sup>16</sup>。    |
 | 0x04       | "`#Blob`" 流的大小 &ge; 2<sup>16</sup>。    |

_Valid_ 字段是一个64位位向量，对于存储在流中的每个表，都有一个特定的位被设置；表到索引的映射在 §[II.22](ii.22-metadata-logical-format-tables.md) 的开始处给出。例如，当 _DeclSecurity_ 表在逻辑元数据中存在时，应在 Valid 向量中设置位 0x0e。在 _Valid_ 中包含不存在的表是无效的，因此所有位于 0x2c 以上的位都应为零。

_Rows_ 数组包含每个存在的表的行数。在将物理元数据解码为逻辑元数据时，_Valid_ 中1的数量表示 _Rows_ 数组中的元素数量。

在编码逻辑表的过程中，一个关键的方面是其模式。每个表的模式在 §[II.22](ii.22-metadata-logical-format-tables.md) 中给出。例如，分配索引 0x02 的表是一个 _TypeDef_ 表，根据其在 §[II.22.37](ii.22.37-typedef-0x02.md) 中的规范，它具有以下列：一个4字节宽的标志，一个指向 String 堆的索引，另一个指向 String 堆的索引，一个指向 _TypeDef_、_TypeRef_ 或 _TypeSpec_ 表的索引，一个指向 _Field_ 表的索引，以及一个指向 _MethodDef_ 表的索引。

具有 *n* 列和 *m* 行的表的物理表示，其模式为 (*C*<sub>0</sub>,&hellip;,*C*<sub>*n*-1</sub>)，由每个行的物理表示的连接组成。具有模式 (*C*<sub>0</sub>,&hellip;,*C*<sub>n-1</sub>) 的行的物理表示是每个元素的物理表示的连接。在类型为 *C* 的列中，行单元 *e* 的物理表示定义如下：

 * 如果 *e* 是一个常量，它使用其列类型 *C* 指定的字节数存储 (即，_PropertyAttributes_ 类型的2位掩码) 

 * 如果 *e* 是一个指向 GUID 堆、'blob' 或 String 堆的索引，它使用 _HeapSizes_ 字段定义的字节数存储。

 * 如果 *e* 是一个简单的指向索引为 *i* 的表的索引，如果表 *i* 的行数小于 216，则使用2字节存储，否则使用4字节存储。

* 如果 *e* 是一个编码索引，它指向 *n* 个可能的表 *t*<sub>0</sub>,&hellip;*t*<sub>*n*-1</sub> 中的表 *t*<sub>*i*</sub>，那么它被存储为 *e* << (log *n*) | tag{ *t*<sub>0</sub>,&hellip;*t*<sub>*n*-1</sub> }\[ *t*<sub>*i*</sub> \]，如果表 *t*<sub>0</sub>,&hellip;*t*<sub>*n*-1</sub> 的最大行数小于 2(16 – (log *n*))，则使用2字节存储，否则使用4字节存储。有限映射族 tag{ *t*<sub>0</sub>,&hellip;*t*<sub>*n*-1</sub> } 在下面定义。注意，解码物理行需要这个映射的逆。[例如，_Constant_ 表的 _Parent_ 列索引 _Field_、_Param_ 或 _Property_ 表中的一行。实际的表被编码到数字的低2位，使用值：0 => _Field_，1 => _Param_，2 => _Property_。剩余的位保存了被索引的实际行号。例如，值 0x321，索引 _Param_ 表中的行号 0xC8。]

 | TypeDefOrRef: 2 bits to encode tag | 标签 |
 | ---------------------------------- | ---- |
 | `TypeDef`                          | 0    |
 | `TypeRef`                          | 1    |
 | `TypeSpec`                         | 2    |

 | HasConstant: 2 bits to encode tag | 标签 |
 | --------------------------------- | ---- |
 | `Field`                           | 0    |
 | `Param`                           | 1    |
 | `Property`                        | 2    |

| HasCustomAttribute: 5 bits to encode tag | 标签 |
| ---------------------------------------- | ---- |
| `MethodDef`                              | 0    |
| `Field`                                  | 1    |
| `TypeRef`                                | 2    |
| `TypeDef`                                | 3    |
| `Param`                                  | 4    |
| `InterfaceImpl`                          | 5    |
| `MemberRef`                              | 6    |
| `Module`                                 | 7    |
| `Permission`                             | 8    |
| `Property`                               | 9    |
| `Event`                                  | 10   |
| `StandAloneSig`                          | 11   |
| `ModuleRef`                              | 12   |
| `TypeSpec`                               | 13   |
| `Assembly`                               | 14   |
| `AssemblyRef`                            | 15   |
| `File`                                   | 16   |
| `ExportedType`                           | 17   |
| `ManifestResource`                       | 18   |
| `GenericParam`                           | 19   |
| `GenericParamConstraint`                 | 20   |
| `MethodSpec`                             | 21   |

_[注意：_ _HasCustomAttributes_ 只有对应于用户源程序中的项目的表的值；例如，可以将属性附加到 _TypeDef_ 表和 _Field_ 表，但不能附加到 _ClassLayout_ 表。因此，上面的枚举中缺少一些表类型。结束注释]_

 HasFieldMarshall: 1 bit to encode tag | 标签
 `Field` | 0
 `Param` | 1

 | HasDeclSecurity: 2 bits to encode tag | 标签 |
 | ------------------------------------- | ---- |
 | `TypeDef`                             | 0    |
 | `MethodDef`                           | 1    |
 | `Assembly`                            | 2    |

 | MemberRefParent: 3 bits to encode tag | 标签 |
 | ------------------------------------- | ---- |
 | `TypeDef`                             | 0    |
 | `TypeRef`                             | 1    |
 | `ModuleRef`                           | 2    |
 | `MethodDef`                           | 3    |
 | `TypeSpec`                            | 4    |

 | HasSemantics: 1 bit to encode tag | 标签 |
 | --------------------------------- | ---- |
 | `Event`                           | 0    |
 | `Property`                        | 1    |

 | MethodDefOrRef: 1 bit to encode tag | 标签 |
 | ----------------------------------- | ---- |
 | `MethodDef`                         | 0    |
 | `MemberRef`                         | 1    |

 | MemberForwarded: 1 bit to encode tag | 标签 |
 | ------------------------------------ | ---- |
 | `Field`                              | 0    |
 | `MethodDef`                          | 1    |

 | Implementation: 2 bits to encode tag | 标签 |
 | ------------------------------------ | ---- |
 | `File`                               | 0    |
 | `AssemblyRef`                        | 1    |
 | `ExportedType`                       | 2    |

 | CustomAttributeType: 3 bits to encode tag | 标签 |
 | ----------------------------------------- | ---- |
 | Not used                                  | 0    |
 | Not used                                  | 1    |
 | `MethodDef`                               | 2    |
 | `MemberRef`                               | 3    |
 | Not used                                  | 4    |

 | ResolutionScope: 2 bits to encode tag | 标签 |
 | ------------------------------------- | ---- |
 | `Module`                              | 0    |
 | `ModuleRef`                           | 1    |
 | `AssemblyRef`                         | 2    |
 | `TypeRef`                             | 3    |

 | TypeOrMethodDef: 1 bit to encode tag | 标签 |
 | ------------------------------------ | ---- |
 | `TypeDef`                            | 0    |
 | `MethodDef`                          | 1    |

源: 与必应的对话， 2023/12/25
(1) github.com. https://github.com/stakx/ecma-335/tree/68d5015b146d347b2d76bd67d150af5f3fa68178/docs%2Fii.24.2.6-metadata-stream.md.




## 23. PE文件格式的扩展

> _这只包含信息性文本。_

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
 | 20   | 4    | EntryPointToken         | 图像入口点的 _MethodDef_ 或 _File_ 的 _token_                                  |
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

 | 常量                           | 值   | 描述                                            |
 | ------------------------------ | ---- | ----------------------------------------------- |
 | `COR_VTABLE_32BIT`             | 0x01 | Vtable 插槽是 32 位的。                         |
 | `COR_VTABLE_64BIT`             | 0x02 | Vtable 插槽是 64 位的。                         |
 | `COR_VTABLE_FROM_UNMANAGED`    | 0x04 | 从非托管代码转换到托管代码。                    |
 | `COR_VTABLE_CALL_MOST_DERIVED` | 0x10 | 调用由 _token_ 描述的最派生的方法 (仅对虚方法有效)。 |

##### 23.3.3.4. 强名称签名

此头部条目指向一个图像的强名称哈希，可以用来确定性地从引用点 (§[II.6.2.1.3](ii.6.2.1.3-originators-public-key.md)) 识别一个模块。

### 23.4. CIL 物理布局

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
 | 8       | 4       | **LocalVarSigTok** | 描述方法的局部变量布局的签名的元数据 _token_ 。0表示没有局部变量存在                                        |

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
 | 8      | 4    | **ClassToken**    | 基于类型的异常处理程序的元数据 _token_          |
 | 8      | 4    | **FilterOffset**  | 基于过滤器的异常处理程序在方法体中的偏移量 |

大型异常处理条款的布局如下：

 | 偏移量 | 大小 | 字段              | 描述                                       |
 | ------ | ---- | ----------------- | ------------------------------------------ |
 | 0      | 4    | **Flags**         | 标志，见下文。                             |
 | 4      | 4    | **TryOffset**     | 从方法体开始的try块的字节偏移量。          |
 | 8      | 4    | **TryLength**     | try块的字节长度                            |
 | 12     | 4    | **HandlerOffset** | 此try块的处理程序的位置                    |
 | 16     | 4    | **HandlerLength** | 处理程序代码的字节大小                     |
 | 20     | 4    | **ClassToken**    | 基于类型的异常处理程序的元数据 _token_          |
 | 20     | 4    | **FilterOffset**  | 基于过滤器的异常处理程序在方法体中的偏移量 |

每个异常处理条款使用以下标志值：

 | 标志                               | 值     | 描述                                |
 | ---------------------------------- | ------ | ----------------------------------- |
 | `COR_ILEXCEPTION_CLAUSE_EXCEPTION` | 0x0000 | 类型化的异常条款                    |
 | `COR_ILEXCEPTION_CLAUSE_FILTER`    | 0x0001 | 异常过滤器和处理程序条款            |
 | `COR_ILEXCEPTION_CLAUSE_FINALLY`   | 0x0002 | 最终条款                            |
 | `COR_ILEXCEPTION_CLAUSE_FAULT`     | 0x0004 | 错误条款 (只在异常时调用的最终条款) |


---

