

## 21. 元数据逻辑格式：其他结构
<a id="metadata-format-others"></a>

>---
### 21.1. BitMasks & Flags

此小节解释了元数据表中使用的 Flags 和 BitMasks。当符合规范的实现遇到未在此标准中指定的元数据结构 (如标志) 时，实现的行为是未指定的。

#### 21.1.1. AssemblyHashAlgorithm
<a id="AssemblyHashAlgorithm-value"></a>

 | Algorithm            | Value  |
 | :------------------- | :----- |
 | *`None`*             | 0x0000 |
 | *`Reserved`* (`MD5`) | 0x8003 |
 | *`SHA1`*             | 0x8004 |

#### 21.1.2. AssemblyFlags
<a id="AssemblyFlags"></a>

 | Flag                         | Value     | Description                                                                                                                   |
 | ---------------------------- | ------ | ---------------------------------------------------------------------------------------------------------------------- |
 | `PublicKey`                  | 0x0001 | 程序集引用包含完整的 (未哈希的) 公钥。                                                                                 |
 | `Retargetable`               | 0x0100 | 运行时使用的此程序集的实现不预期与编译时看到的版本匹配。 (参见此表后的文本。)                                          |
 | `DisableJITcompileOptimizer` | 0x4000 | 保留 (CLI的符合规范的实现可以在读取时忽略此设置；一些实现可能使用此位来指示CIL到本地代码编译器不应生成优化的代码)      |
 | `EnableJITcompileTracking`   | 0x8000 | 保留 (CLI的符合规范的实现可以在读取时忽略此设置；一些实现可能使用此位来指示CIL到本地代码编译器应生成CIL到本地代码映射) |

#### 21.1.3. Culture 的Value
<a id="Culture-values"></a>

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

**关于RFC 1766，区域名称的注释：**典型的字符串将是"``en-US``"。第一部分 (例子中的"`en`") 使用ISO 639字符 ("小写的`拉丁字母`字符。不使用带有变音符号的或修改过的字符")。第二部分 (例子中的"`US`") 使用ISO 3166字符 (类似于ISO 639，但是大写) ；也就是说，熟悉的ASCII字符`a`&mdash;`z`和`A`&mdash;`Z`。然而，虽然RFC 1766建议第一部分使用小写，第二部分使用大写，但它允许混合大小写。因此，验证规则只检查_Culture_是否是上面列表中的字符串之一&mdash;但是检查是完全`不区分大小写`的&mdash;其中`不区分大小写`是对小于U+0080的Value的熟悉的折叠。

#### 21.1.4. 事件标志 [EventAttributes]

 | 标志            | Value     | Description                                   |
 | --------------- | ------ | -------------------------------------- |
 | `SpecialName`   | 0x0200 | 事件是特殊的。                         |
 | `RTSpecialName` | 0x0400 | CLI 提供 '特殊' 行为，取决于事件的名称 |

#### 21.1.5. 字段的标志 [FieldAttributes]

 | 标志                       | Value     | Description                                              |
 | -------------------------- | ------ | ------------------------------------------------- |
 | **`FieldAccessMask`**      | 0x0007 | 这 3 位包含以下Value之一：                           |
 | &emsp;`CompilerControlled` | 0x0000 | 成员不可引用                                      |
 | &emsp;`Private`            | 0x0001 | 仅父类型可访问                                    |
 | &emsp;`FamANDAssem`        | 0x0002 | 仅此程序集中的子类型可访问                        |
 | &emsp;`Assembly`           | 0x0003 | 程序集中的任何人都可以访问                        |
 | &emsp;`Family`             | 0x0004 | 仅类型和子类型可访问                              |
 | &emsp;`FamORAssem`         | 0x0005 | 任何地方的子类型以及程序集中的任何人都可以访问    |
 | &emsp;`Public`             | 0x0006 | 对于具有此范围字段合同属性的任何人都是可访问的    |
 | `Static`                   | 0x0010 | 在类型上定义，否则每个实例                        |
 | `InitOnly`                 | 0x0020 | 字段只能初始化，初始化后不能写入                  |
 | `Literal`                  | 0x0040 | Value是编译时常量                                    |
 | `NotSerialized`            | 0x0080 | 保留 (用于指示当类型被远程化时，不应序列化此字段) |
 | `SpecialName`              | 0x0200 | 字段是特殊的                                      |
 | **Interop Attributes**     | &nbsp; | &nbsp;                                            |
 | `PInvokeImpl`              | 0x2000 | 实现通过 PInvoke 转发。                           |
 | **Additional flags**       | &nbsp; | &nbsp;                                            |
 | `RTSpecialName`            | 0x0400 | CLI 提供 '特殊' 行为，取决于字段的名称            |
 | `HasFieldMarshal`          | 0x1000 | 字段有封送信息                                    |
 | `HasDefault`               | 0x8000 | 字段有默认Value                                      |
 | `HasFieldRVA`              | 0x0100 | 字段有 RVA                                        |


#### 21.1.6. 文件的标志 [FileAttributes]

 | 标志                 | Value     | Description                                     |
 | -------------------- | ------ | ---------------------------------------- |
 | `ContainsMetaData`   | 0x0000 | 这不是一个资源文件                       |
 | `ContainsNoMetaData` | 0x0001 | 这是一个资源文件或其他不包含元数据的文件 |

#### 21.1.7. 泛型参数的标志 [GenericParamAttributes]

 | 标志                                   | Value     | Description                               |
 | -------------------------------------- | ------ | ---------------------------------- |
 | **`VarianceMask`**                     | 0x0003 | 这两位包含以下Value之一：             |
 | &emsp;`None`                           | 0x0000 | 泛型参数是非变量，并且没有特殊约束 |
 | &emsp;`Covariant`                      | 0x0001 | 泛型参数是协变的                   |
 | &emsp;`Contravariant`                  | 0x0002 | 泛型参数是逆变的                   |
 | **`SpecialConstraintMask`**            | 0x001C | 这三位包含以下Value之一：             |
 | &emsp;`ReferenceTypeConstraint`        | 0x0004 | 泛型参数具有类特殊约束             |
 | &emsp;`NotNullableValueTypeConstraint` | 0x0008 | 泛型参数具有Value类型特殊约束         |
 | &emsp;`DefaultConstructorConstraint`   | 0x0010 | 泛型参数具有 `.ctor` 特殊约束      |

#### 21.1.8. ImplMap的标志 [PInvokeAttributes]

 | 标志                        | Value     | Description                                                             |
 | --------------------------- | ------ | ---------------------------------------------------------------- |
 | `NoMangle`                  | 0x0001 | PInvoke将使用指定的成员名称                                      |
 | **字符集**                  | &nbsp; | &nbsp;                                                           |
 | **`CharSetMask`**           | 0x0006 | 这是一个资源文件或其他不包含元数据的文件。这两位包含以下Value之一： |
 | &emsp;`CharSetNotSpec`      | 0x0000 | &nbsp;                                                           |
 | &emsp;`CharSetAnsi`         | 0x0002 | &nbsp;                                                           |
 | &emsp;`CharSetUnicode`      | 0x0004 | &nbsp;                                                           |
 | &emsp;`CharSetAuto`         | 0x0006 | &nbsp;                                                           |
 | `SupportsLastError`         | 0x0040 | 关于目标函数的信息。对字段不相关                                 |
 | **调用约定**                | &nbsp; | &nbsp;                                                           |
 | `CallConvMask`              | 0x0700 | 这三位包含以下Value之一：                                           |
 | &emsp;`CallConvPlatformapi` | 0x0100 | &nbsp;                                                           |
 | &emsp;`CallConvCdecl`       | 0x0200 | &nbsp;                                                           |
 | &emsp;`CallConvStdcall`     | 0x0300 | &nbsp;                                                           |
 | &emsp;`CallConvThiscall`    | 0x0400 | &nbsp;                                                           |
 | &emsp;`CallConvFastcall`    | 0x0500 | &nbsp;                                                           |

#### 21.1.9. ManifestResource 标志 [ManifestResourceAttributes]

 | 标志                 | Value     | Description                   |
 | -------------------- | ------ | ---------------------- |
 | **`VisibilityMask`** | 0x0007 | 这三位包含以下Value之一： |
 | &emsp;`Public`       | 0x0001 | 资源从程序集中导出     |
 | &emsp;`Private`      | 0x0002 | 资源对程序集是私有的   |

#### 21.1.10. 方法的标志 [MethodAttributes]
<a id="MethodAttributes"></a>

 | 标志                       | Value     | Description                                             |
 | -------------------------- | ------ | ------------------------------------------------ |
 | **`MemberAccessMask`**     | 0x0007 | 这3位包含以下Value之一：                            |
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
 | **`VtableLayoutMask`**     | 0x0100 | 使用此掩码检索 vtable 属性。此位包含以下Value之一： |
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

 | 标志                 | Value     | Description                                                     |
 | -------------------- | ------ | -------------------------------------------------------- |
 | **`CodeTypeMask`**   | 0x0003 | 这两位包含以下Value之一：                                   |
 | &emsp;`IL`           | 0x0000 | 方法实现是 CIL                                           |
 | &emsp;`Native`       | 0x0001 | 方法实现是本地的                                         |
 | &emsp;`OPTIL`        | 0x0002 | 保留：在符合规范的实现中应为零                           |
 | &emsp;`Runtime`      | 0x0003 | 方法实现由运行时提供                                     |
 | **`ManagedMask`**    | 0x0004 | 指定代码是托管的还是非托管的标志。这一位包含以下Value之一： |
 | &emsp;`Unmanaged`    | 0x0004 | 方法实现是非托管的，否则是托管的                         |
 | &emsp;`Managed`      | 0x0000 | 方法实现是托管的                                         |
 | **实现信息和互操作** | &nbsp; | &nbsp;                                                   |
 | `ForwardRef`         | 0x0010 | 表示方法已定义；主要用于合并场景                         |
 | `PreserveSig`        | 0x0080 | 保留：符合规范的实现可以忽略                             |
 | `InternalCall`       | 0x1000 | 保留：在符合规范的实现中应为零                           |
 | `Synchronized`       | 0x0020 | 方法在主体中是单线程的                                   |
 | `NoInlining`         | 0x0008 | 方法不能内联                                             |
 | `MaxMethodImplVal`   | 0xffff | 范围检查Value                                               |
 | `NoOptimization`     | 0x0040 | 在生成本地代码时，方法不会被优化                         |

####  21.1.12. MethodSemantics的标志 [MethodSemanticsAttributes]

 | 标志       | Value     | Description                                                               |
 | ---------- | ------ | ------------------------------------------------------------------ |
 | `Setter`   | 0x0001 | 属性的设置器                                                       |
 | `Getter`   | 0x0002 | 属性的获取器                                                       |
 | `Other`    | 0x0004 | 属性或事件的其他方法                                               |
 | `AddOn`    | 0x0008 | 事件的AddOn方法。这指的是事件所需的`add_`方法。 (§[22.13]())       |
 | `RemoveOn` | 0x0010 | 事件的RemoveOn方法。这指的是事件所需的`remove_`方法。 (§[22.13]()) |
 | `Fire`     | 0x0020 | 事件的Fire方法。这指的是事件的可选`raise_`方法。 (§[22.13]())      |

#### 21.1.13. 参数的标志 [ParamAttributes]

 | 标志              | Value     | Description                           |
 | ----------------- | ------ | ------------------------------ |
 | `In`              | 0x0001 | 参数是 `[in]`                  |
 | `Out`             | 0x0002 | 参数是 `[out]`                 |
 | `Optional`        | 0x0010 | 参数是可选的                   |
 | `HasDefault`      | 0x1000 | 参数有默认Value                   |
 | `HasFieldMarshal` | 0x2000 | 参数有 _FieldMarshal_          |
 | `Unused`          | 0xcfe0 | 保留：在符合规范的实现中应为零 |

#### 21.1.14. 属性标志 [PropertyAttributes]

 | 标志            | Value     | Description                                   |
 | --------------- | ------ | -------------------------------------- |
 | `SpecialName`   | 0x0200 | 属性是特殊的                           |
 | `RTSpecialName` | 0x0400 | 运行时 (元数据内部 API) 应检查名称编码 |
 | `HasDefault`    | 0x1000 | 属性有默认Value                           |
 | `Unused`        | 0xe9ff | 保留：在符合规范的实现中应为零         |

#### 21.1.15. 类型的标志 [TypeAttributes]
<a id="TypeAttributes"></a>

 | 标志                      | Value         | Description                                                                      |
 | ------------------------- | ---------- | ------------------------------------------------------------------------- |
 | **可见性属性**            | &nbsp;     | &nbsp;                                                                    |
 | **`VisibilityMask`**      | 0x00000007 | 使用此掩码检索可见性信息。这3位包含以下Value之一：                           |
 | &emsp;`NotPublic`         | 0x00000000 | 类没有公共范围                                                            |
 | &emsp;`Public`            | 0x00000001 | 类具有公共范围                                                            |
 | &emsp;`NestedPublic`      | 0x00000002 | 类是具有公共可见性的嵌套类                                                |
 | &emsp;`NestedPrivate`     | 0x00000003 | 类是具有私有可见性的嵌套类                                                |
 | &emsp;`NestedFamily`      | 0x00000004 | 类是具有家族可见性的嵌套类                                                |
 | &emsp;`NestedAssembly`    | 0x00000005 | 类是具有程序集可见性的嵌套类                                              |
 | &emsp;`NestedFamANDAssem` | 0x00000006 | 类是具有家族和程序集可见性的嵌套类                                        |
 | &emsp;`NestedFamORAssem`  | 0x00000007 | 类是具有家族或程序集可见性的嵌套类                                        |
 | **类布局属性**            | &nbsp;     | &nbsp;                                                                    |
 | **`LayoutMask`**          | 0x00000018 | 使用此掩码检索类布局信息。这2位包含以下Value之一：                           |
 | &emsp;`AutoLayout`        | 0x00000000 | 类字段是自动布局的                                                        |
 | &emsp;`SequentialLayout`  | 0x00000008 | 类字段是顺序布局的                                                        |
 | &emsp;`ExplicitLayout`    | 0x00000010 | 布局是显式提供的                                                          |
 | **类语义属性**            | &nbsp;     | &nbsp;                                                                    |
 | **`ClassSemanticsMask`**  | 0x00000020 | 使用此掩码检索类语义信息。此位包含以下Value之一：                            |
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
 | **`StringFormatMask`**    | 0x00030000 | 使用此掩码检索用于本地互操作的字符串信息。这2位包含以下Value之一：           |
 | &emsp;`AnsiClass`         | 0x00000000 | `LPSTR` 被解释为 ANSI                                                     |
 | &emsp;`UnicodeClass`      | 0x00010000 | `LPSTR` 被解释为 Unicode                                                  |
 | &emsp;`AutoClass`         | 0x00020000 | `LPSTR` 自动解释                                                          |
 | &emsp;`CustomFormatClass` | 0x00030000 | 由 `CustomStringFormatMask` 指定的非标准编码                              |
 | `CustomStringFormatMask`  | 0x00C00000 | 使用此掩码检索用于本地互操作的非标准编码信息。这2位的Value的含义是未指定的。 |
 | **类初始化属性**          | &nbsp;     | &nbsp;                                                                    |
 | `BeforeFieldInit`         | 0x00100000 | 在第一次静态字段访问之前初始化类                                          |
 | **附加标志**              | &nbsp;     | &nbsp;                                                                    |
 | `RTSpecialName`           | 0x00000800 | CLI 提供 '特殊' 行为，取决于类型的名称                                    |
 | `HasSecurity`             | 0x00040000 | 类型具有与其关联的安全性                                                  |
 | `IsTypeForwarder`         | 0x00200000 | 此 _ExportedType_ 条目是类型转发器                                        |

 #### 21.1.16. 签名中使用的元素类型
<a id="ELEMENT_TYPE"></a>

下表列出了`ELEMENT_TYPE`常量的Value。这些在元数据签名Blobs中被广泛使用 - 参见§[II.23.2](ii.23.2-blobs-and-signatures.md)。

 | 名称                       | Value   | 备注                                                                                                                         |
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
 | `ELEMENT_TYPE_VALUETYPE`   | 0x11 | 后跟 _TypeDef_ 或 _TypeRef_  _token_                                                                                         |
 | `ELEMENT_TYPE_CLASS`       | 0x12 | 后跟 _TypeDef_ 或 _TypeRef_  _token_                                                                                         |
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
 | `ELEMENT_TYPE_CMOD_REQD`   | 0x1f | 必需的修饰符：后跟 _TypeDef_ 或 _TypeRef_  _token_                                                                           |
 | `ELEMENT_TYPE_CMOD_OPT`    | 0x20 | 可选的修饰符：后跟 _TypeDef_ 或 _TypeRef_  _token_                                                                           |
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

*签名* 这个词通常用来Description函数或方法的类型信息；也就是，它的每个参数的类型，以及它的返回Value的类型。在元数据中，签名这个词也用来Description字段、属性和局部变量的类型信息。每个签名都存储为 Blob 堆中的一个 (计数) 字节数组。有几种类型的签名，如下：

 * _MethodRefSig_  (只有在 `VARARG` 调用中才与 _MethodDefSig_ 不同)

 * _MethodDefSig_

 * _FieldSig_

 * _PropertySig_

 * _LocalVarSig_

 * _TypeSpec_

 * _MethodSpec_

签名 'blob' 的第一个字节的Value表示它是什么类型的签名。它的最低4位包含以下之一：`C`，`DEFAULT`，`FASTCALL`，`STDCALL`，`THISCALL`，或 `VARARG`  (其Value在 §[II.23.2.3](ii.23.2.3-standalonemethodsig.md) 中定义)，这些都是方法签名的限定符；`FIELD`，表示字段签名 (其Value在 §[II.23.2.4](ii.23.2.4-fieldsig.md) 中定义) ；或 `PROPERTY`，表示属性签名 (其Value在 §[II.23.2.5](ii.23.2.5-propertysig.md) 中定义)。本小节定义了每种类型的签名的二进制 'blob' 格式。在伴随许多定义的语法图中，使用阴影将本来会是多个图的内容合并到一个图中；附带的文本Description了阴影的使用。

在将签名存储到 Blob 堆中之前，通过压缩签名中嵌入的整数来压缩签名 (如下所述)。可编码的无符号整数的最大长度为29位，0x1FFFFFFF。对于有符号整数，如在 ArrayShape (§[II.23.2.13](ii.23.2.13-arrayshape.md)) 中出现的，范围是 -2<sup>28</sup> (0xF0000000) 到 2<sup>28</sup>-1 (0x0FFFFFFF)。使用的压缩算法如下 (位0是最低有效位) ：

 * 对于无符号整数：

   * 如果Value在0 (0x00) 和 127 (0x7F) 之间，包含两者，编码为一个字节的整数 (位7清零，Value保存在位6到位0)

   * 如果Value在2<sup>8</sup> (0x80) 和 2<sup>14</sup>-1 (0x3FFF) 之间，包含两者，编码为一个2字节的整数，位15设为1，位14清零 (Value保存在位13到位0)

   * 否则，编码为一个4字节的整数，位31设为1，位30设为1，位29清零 (Value保存在位28到位0)

 * 对于有符号整数：

   * 如果Value在 -2<sup>6</sup> 和 2<sup>6</sup>-1 之间，包含两者：

      * 将Value表示为一个7位的2的补数，给出 0x40 (-2<sup>6</sup>) 到 0x3F (2<sup>6</sup>-1)；

      * 将这个Value左移1位，给出 0x01 (-2<sup>6</sup>) 到 0x7E (2<sup>6</sup>-1)；

      * 编码为一个字节的整数，位7清零，旋转后的Value在位6到位0，给出 0x01 (-2<sup>6</sup>) 到 0x7E (2<sup>6</sup>-1)。

   * 如果Value在 -2<sup>13</sup> 和 2<sup>13</sup>-1 之间，包含两者：

      * 将Value表示为一个14位的2的补数，给出 0x2000 (-2<sup>13</sup>) 到 0x1FFF (2<sup>13</sup>-1)；

      * 将这个Value左移1位，给出 0x0001 (-2<sup>13</sup>) 到 0x3FFE (2<sup>13</sup>-1)；

      * 编码为一个两字节的整数：位15设为1，位14清零，旋转后的Value在位13到位0，给出 0x8001 (-2<sup>13</sup>) 到 0xBFFE (2<sup>13</sup>-1)。

   * 如果Value在 -2<sup>28</sup> 和 2<sup>28</sup>-1 之间，包含两者：

      * 将Value表示为一个29位的2的补数，给出 0x10000000 (-2<sup>28</sup>) 到 0xFFFFFFF (2<sup>28</sup>-1)；

      * 将这个Value左移1位，给出 0x00000001 (-2<sup>28</sup>) 到 0x1FFFFFFE (2<sup>28</sup>-1)；

      * 编码为一个四字节的整数：位31设为1，位30设为1，位29清零，旋转后的Value在位28到位0，给出 0xC0000001 (-2<sup>28</sup>) 到 0xDFFFFFFE (2<sup>28</sup>-1)。

* 空字符串应该用保留的单字节0xFF表示，后面没有跟随的数据

_[注：_ 下面的表格显示了几个例子。第一列给出了一个Value，以熟悉的 (类C) 十六进制表示法表示。第二列显示了相应的压缩结果，就像它会出现在PE文件中一样，结果的连续字节位于文件中逐渐增大的字节偏移处。 (这与在PE文件中布局常规二进制整数的顺序相反。)

无符号示例：

 | 原始Value      | 压缩表示     |
 | ----------- | ------------ |
 | 0x03        | 03           |
 | 0x7F        | 7F (7位设定) |
 | 0x80        | 8080         |
 | 0x2E57      | AE57         |
 | 0x3FFF      | BFFF         |
 | 0x4000      | C000 4000    |
 | 0x1FFF FFFF | DFFF FFFF    |

有符号示例：

 | 原始Value     | 压缩表示  |
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

"压缩"字段的最高有效位 (在PE文件中首次遇到的位) 可以揭示它占用1、2还是4个字节，以及它的Value。为了使这个工作，如上所述，"压缩"Value以大端顺序存储；即，最高有效字节位于文件中的最小偏移处。

签名广泛使用名为`ELEMENT_TYPE_xxx`的常量Value - 参见§[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md)。特别地，签名包括两个修饰符，称为：

 * `ELEMENT_TYPE_BYREF` - 这个元素是一个托管指针 (参见 [Partition I]())。这个修饰符只能出现在_LocalVarSig_ (§[II.23.2.6](ii.23.2.6-localvarsig.md))，_Param_ (§[II.23.2.10](ii.23.2.10-param.md)) 或 _RetType_ (§[II.23.2.11](ii.23.2.11-rettype.md))的定义中。它不应该出现在_Field_ (§[II.23.2.4](ii.23.2.4-fieldsig.md))的定义中。

 * `ELEMENT_TYPE_PTR` - 这个元素是一个非托管指针 (参见 [Partition I]())。这个修饰符可以出现在_LocalVarSig_ (§[II.23.2.6](ii.23.2.6-localvarsig.md))，_Param_ (§[II.23.2.10](ii.23.2.10-param.md))，_RetType_ (§[II.23.2.11](ii.23.2.11-rettype.md)) 或 _Field_ (§[II.23.2.4](ii.23.2.4-fieldsig.md))的定义中。

#### 21.2.1. MethodDefSig

<a id="MethodDefSig"></a>

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

_RetType_ 项Description方法的返回Value的类型 (参见 §[II.23.2.11](ii.23.2.11-rettype.md))

_Param_ 项Description每个方法参数的类型。应该有 _ParamCount_ 个 _Param_ 项的实例 (参见 §[II.23.2.10](ii.23.2.10-param.md))。

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

_RetType_ 项Description了方法返回Value的类型 (§[II.23.2.11](ii.23.2.11-rettype.md))

_Param_ 项Description了每个方法参数的类型。应该有 _ParamCount_ 个 _Param_ 项的实例 (§[II.23.2.10](ii.23.2.10-param.md))。

_Param_ 项Description了每个方法参数的类型。应该有 _ParamCount_ 个 _Param_ 项的实例。这开始就像一个 `VARARG` 方法的 _MethodDefSig_ (§[II.23.2.1](ii.23.2.1-methoddefsig.md))。但然后追加了一个 `SENTINEL`  _token_ ，后面跟着额外的 _Param_ 项来Description额外的 `VARARG` 参数。注意，_ParamCount_ 项应该指示签名中 _Param_ 项的总数 - 在 `SENTINEL` 字节 (0x41) 之前和之后。

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

_RetType_项Description了方法返回Value的类型 (§[II.23.2.11](ii.23.2.11-rettype.md))

第一个_Param_项Description了每个方法的非变参参数的类型。第二个 (可选的) _Param_项Description了每个方法的变参参数的类型。应该有_ParamCount_个_Param_实例 (§[II.23.2.10](ii.23.2.10-param.md))。

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

`CMOD_OPT` 或 `CMOD_REQD` 的Value是压缩的，参见 §[II.23.2](ii.23.2-blobs-and-signatures.md)。

`CMOD_OPT` 或 `CMOD_REQD` 后面跟着一个元数据 _token_ ，该 _token_ 索引 _TypeDef_ 表或 _TypeRef_ 表中的一行。然而，这些 _token_ 是编码和压缩的 - 详情参见 §[II.23.2.8](ii.23.2.8-typedeforreforspecencoded.md)

如果 CustomModifier 标记为 `CMOD_OPT`，那么任何导入编译器都可以完全忽略它。相反，如果 CustomModifier 标记为 `CMOD_REQD`，任何导入编译器都应该“理解”此 CustomModifier 所暗示的语义，以便引用周围的 Signature。


#### 21.2.8. TypeDefOrRefOrSpecEncoded

这些项是在签名中存储 _TypeDef_，_TypeRef_ 或 _TypeSpec_  _token_ 的紧凑方式 (§[II.23.2.12](ii.23.2.12-type.md))。考虑一个常规的 _TypeRef_  _token_ ，例如 0x01000012。最高字节 0x01 表示这是一个 _TypeRef_  _token_  (参见 §[II.22](ii.22-metadata-logical-format-tables.md) 中支持的元数据 _token_ 类型列表)。较低的3字节 (0x000012) 索引 _TypeRef_ 表中的行号 0x12。

这个 _TypeRef_  _token_ 的编码版本如下构造：

 1. 将此 _token_ 索引的表编码为最低有效的2位。要使用的位Value是0，1和2，分别指定目标表是 _TypeDef_，_TypeRef_ 或 _TypeSpec_ 表。

 2. 将3字节行索引 (在此示例中为 0x000012) 左移2位，并将其与步骤1中的2位编码进行 OR 操作。

 3. 压缩结果Value (§[II.23.2](ii.23.2-blobs-and-signatures.md))。

此示例产生以下编码Value：

 ```
 a)  编码 = TypeRef 表的Value = 0x01 (来自上述 1.)
 b)  编码 = ( 0x000012 << 2 ) |  0x01
             = 0x48 | 0x01
             = 0x49
 c)  编码 = Compress (0x49)
             = 0x49
 ```

所以，与原始的，常规的 _TypeRef_  _token_ Value 0x01000012 不同，需要在签名 'blob' 中占用4字节的空间，这个 _TypeRef_  _token_ 被编码为一个单字节。

#### 21.2.9. Constraint

在签名中，_Constraint_项目前只有一个可能的Value，即`ELEMENT_TYPE_PINNED` (§[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md))，它指定目标类型在运行时堆中被固定，不会被垃圾收集的操作移动。

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

_GenArgCount_ 非终结符是一个无符号整数Value (压缩)，指定此签名中的泛型参数的 _数量_。在 `MVAR` 或 `VAR` 后面的 number 非终结符是一个无符号整数Value (压缩)。

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
<a id="type-spec-blob"></a>

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
<a id="MethodSpec-blob"></a>

由 _MethodSpec_  _token_ 索引的 Blob 堆中的签名具有以下格式

 | _MethodSpecBlob_ ::=
 | ----
 | `GENERICINST` _GenArgCount_ _Type_ _Type_*

`GENERICINST` 的Value为 0x0A。 _[注：_ 在 Microsoft CLR 实现中，这个Value被称为 `IMAGE_CEE_CS_CALLCONV_GENERICINST`。 _结束注]_

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

### 21.3. 自定义特性
<a id="custom-attr-value"></a>

自定义属性具有以下语法图：

 ![](ii.23.3-custom-attributes-figure-1.png)

所有二进制Value都以小端格式存储 (除了_PackedLen_项目，它们仅用作后续UTF8字符串中字节数的计数)。如果没有指定字段、参数或属性，则整个属性表示为空blob。

_CustomAttrib_以Prolog开始 - 一个无符号的*int16*，Value为0x0001。

接下来是构造方法的固定参数的Description。通过检查_MethodDef_表中该构造函数的行，可以找到它们的数量和类型；这些信息在_CustomAttrib_本身中并未重复。如语法图所示，可以有零个或多个_FixedArgs_。 (注意，`VARARG`构造方法在自定义属性的定义中是不允许的。)

接下来是可选的"命名"字段和属性的Description。这开始于_NumNamed_ - 一个无符号的int16，给出后面跟随的"命名"属性或字段的数量。注意，_NumNamed_总是存在的。Value为零表示没有要跟随的"命名"属性或字段 (当然，在这种情况下，_CustomAttrib_将在_NumNamed_之后立即结束)。在_NumNamed_为非零的情况下，它后面跟随_NumNamed_重复的_NamedArgs_。

 ![](ii.23.3-custom-attributes-figure-2.png)

每个_FixedArg_的格式取决于该参数是否为`SZARRAY` - 这在语法图的下方和上方路径中分别显示。因此，每个_FixedArg_要么是单个_Elem_，要么是_NumElem_重复的_Elem_。

(`SZARRAY`是单字节0x1D，表示向量 - 即下界为零的单维数组。)

_NumElem_是一个无符号的_int32_，指定`SZARRAY`中的元素数量，或者0xFFFFFFFF表示该Value为null。

 ![](ii.23.3-custom-attributes-figure-3.png)

_Elem_采用此图中的一种形式，如下所示：

 * 如果参数种类是简单的 (上述图表的第一行)  (**bool**，**char**，**float32**，**float64**，**int8**，**int16**，**int32**，**int64**，**unsigned int8**，**unsigned int16**，**unsigned int32**或**unsigned int64**)，那么'blob'包含其二进制Value (_Val_)。(*bool*是一个字节，Value为0 (假) 或1 (真) ；*char*是一个两字节的Unicode字符；其他的含义很明显。)如果参数种类是*enum*，也使用这种模式 - 只需存储枚举的底层整数类型的Value。

 * 如果参数种类是_string_， (上述图表的中间行) 那么blob包含一个_SerString_ - 一个_PackedLen_字节计数，后面跟着UTF8字符。如果字符串为null，其_PackedLen_的Value为0xFF (没有后续字符)。如果字符串为空 ("")，那么_PackedLen_的Value为0x00 (没有后续字符)。

 * 如果参数种类是`System.Type`， (同样，上述图表的中间行) 其Value以_SerString_的形式存储 (如上一段所定义)，表示其规范名称。规范名称是其完整类型名称，后面可选地跟着定义它的程序集，其版本，文化和公钥 _token_ 。如果省略了程序集名称，CLI首先在当前程序集中查找，然后在系统库 (`mscorlib`) 中查找；在这两种特殊情况下，允许省略程序集名称，版本，文化和公钥 _token_ 。

 * 如果参数种类是`System.Object`， (上述图表的第三行) 存储的Value表示该Value类型的"装箱"实例。在这种情况下，blob包含实际类型的_FieldOrPropType_ (见下文)，后面跟着参数的未装箱Value。_[注意：_在这种情况下，不可能传递nullValue。_结束注释]_

* 如果类型是一个装箱的简单Value类型 (**bool**，**char**，**float32**，**float64**，**int8**，**int16**，**int32**，**int64**，**unsigned int8**，**unsigned int16**，**unsigned int32** 或 **unsigned int64**)，那么 _FieldOrPropType_ 紧接着是一个字节，该字节包含Value 0x51。_FieldOrPropType_ 必须恰好是以下之一：`ELEMENT_TYPE_BOOLEAN`，`ELEMENT_TYPE_CHAR`，`ELEMENT_TYPE_I1`，`ELEMENT_TYPE_U1`，`ELEMENT_TYPE_I2`，`ELEMENT_TYPE_U2`，`ELEMENT_TYPE_I4`，`ELEMENT_TYPE_U4`，`ELEMENT_TYPE_I8`，`ELEMENT_TYPE_U8`，`ELEMENT_TYPE_R4`，`ELEMENT_TYPE_R8`，`ELEMENT_TYPE_STRING`。单维，零基数组被指定为一个字节 0x1D，后面跟着元素类型的 _FieldOrPropTypeof_。 (参见 §[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md)) 枚举被指定为一个字节 0x55，后面跟着一个 _SerString_。

 ![](ii.23.3-custom-attributes-figure-4.png)

_NamedArg_ 只是一个 _FixedArg_ (上面讨论过)，前面有信息来标识它代表哪个字段或属性。_[注：_ 记住，CLI 允许字段和属性具有相同的名称；所以我们需要一种方法来消除这种情况的歧义。_结束注]_

 * `FIELD` 是单字节 0x53。

 * `PROPERTY` 是单字节 0x54。

_FieldOrPropName_ 是字段或属性的名称，存储为 _SerString_ (上面定义)。涉及自定义属性的一些示例包含在 [Partition VI 的附录 B]() 中。

### 21.4. 编组Description符
<a id="blob-description"></a>

编组Description符类似于签名 - 它是二进制数据的 'blob'。它Description了当通过 PInvoke 调度调用到或从非托管代码时，应如何编组字段或参数 (通常情况下，作为参数编号 0 的方法返回也包括在内)。ILAsm 语法 **marshal** 可以用来创建编组Description符，伪自定义属性 `MarshalAsAttribute` 也可以用来创建编组Description符 - 参见 §[II.21.2.1](ii.21.2.1-pseudo-custom-attributes.md)。

注意，CLI 的符合规范的实现只需要支持编组前面指定的类型 - 参见 §[II.15.5.4](ii.15.5.4-data-type-marshaling.md)。

编组Description符使用名为 `NATIVE_TYPE_xxx` 的常量。它们的名称和Value列在下表中：

 | 名称                  | Value   |
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

_ParamNum_ 是一个无符号整数 (以 §[II.23.2](ii.23.2-blobs-and-signatures.md) Description的方式压缩)，指定在方法调用中提供数组中元素数量的参数 - 参见下文。

_NumElem_ 是一个无符号整数 (以 §[II.23.2](ii.23.2-blobs-and-signatures.md) Description的方式压缩)，指定元素或附加元素的数量 - 参见下文。

_[注意：例如，在方法声明中：

 ```ilasm
 .method void M(int32[] ar1, int32 size1, unsigned int8[] ar2, int32 size2) { … }
 ```

`ar1` 参数可能拥有 _FieldMarshal_ 表中的一行，该行索引 Blob 堆中的 _MarshalSpec_，格式为：

 ```
 ARRAY  MAX  2  1
 ```

这表示参数被编组为 `NATIVE_TYPE_ARRAY`。关于每个元素的类型没有额外的信息 (由 `NATIVE_TYPE_MAX` 表示)。_ParamNum_ 的Value为 2，这表示方法中的参数编号 2 (名为 `size1` 的参数) 将指定实际数组中的元素数量 - 假设在特定调用中其Value为 42。_NumElem_ 的Value为 0。数组的总大小 (以字节为单位) 由以下公式给出：

 ```
 if ParamNum = 0
    SizeInBytes = NumElem * sizeof (elem)
 else
    SizeInBytes = ( @ParamNum +  NumElem ) * sizeof (elem)
 endif
 ```

这里使用 `@ParamNum` 语法表示传入参数编号 _ParamNum_ 的Value - 在这个例子中，它将是 42。每个元素的大小是从 `Foo` 的签名中的 `ar1` 参数的元数据计算出来的 - 是大小为 4 字节的 `ELEMENT_TYPE_I4` (参见 §[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md))。结束注释]_

源: 与必应的对话， 2023/12/25
(1) github.com. https://github.com/stakx/ecma-335/tree/68d5015b146d347b2d76bd67d150af5f3fa68178/docs%2Fii.23.4-marshalling-descriptors.md.
