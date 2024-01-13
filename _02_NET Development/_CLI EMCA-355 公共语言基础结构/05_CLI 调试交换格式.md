# CLI：调试交换格式

---
## 1. 可移植 CILDB 文件

可移植的 CILDB 文件提供了一种标准的方式，用于在 CLI 生产者和消费者之间交换调试信息。这个分区用于填补元数据未覆盖的空白，特别是局部变量的名称和源代码行的对应关系。

>---
### 1.1. 整数的编码

所有的整数都以小端格式存储，除了签名中的整数，它们的编码方式在第二部分中所述。

>---
### 1.2. CILDB 头

CILDB 文件以一个 72 字节的头开始，其布局如下：

 | Offset | Size | Field                     | Description                            |
 | :----- | :--- | :------------------------ | :------------------------------------- |
 | 0      | 16   | **Signature**             | CILDB 的魔术签名 "`_ildb_signature\0`" |
 | 16     | 16   | **GUID**                  | 版本GUID                               |
 | 32     | 4    | **UserEntryPoint**        | 入口点的 _MethodDef_ 令牌。            |
 | 36     | 4    | **CountOfMethods**        | _SymMethod_ 表中的行数。               |
 | 40     | 4    | **CountOfScopes**         | _SymScopes_ 表中的行数。               |
 | 44     | 4    | **CountOfVars**           | _SymVariable_ 表中的行数。             |
 | 48     | 4    | **CountOfUsing**          | _SymUsing_ 表中的行数。                |
 | 52     | 4    | **CountOfConstants**      | _SymConstant_ 表中的行数。             |
 | 56     | 4    | **CountOfDocuments**      | _SymDocument_ 表中的行数。             |
 | 60     | 4    | **CountOfSequencePoints** | _SymSequencePoint_ 表中的行数。        |
 | 64     | 4    | **CountOfMiscBytes**      | _SymMisc_ 堆中的字节数。               |
 | 68     | 4    | **CountOfStringBytes**    | _SymString_ 堆中的字节数。             |

#### 1.2.1. 版本 GUID

版本 GUID 是下面显示的 16 字节序列：

 ```
 0x7F  0x55  0xE7  0xF1  0x3C  0x42  0x17  0x41
 0x8D  0xA9  0xC7  0xA3  0xCD  0x98  0x8D  0xF1
 ```

>---
### 1.3. 表和堆

CILDB 头紧接着是各种表和堆，按以下顺序排列：
 1. _SymConstant_
 2. _SymMethod_
 3. _SymScopes_
 4. _SymVariable_
 5. _SymUsing_
 6. _SymSequencePoint_
 7. _SymDocument_
 8. _SymMisc_
 9. _SymString_

其中一些表包含 CIL 偏移量。这些偏移量以字节为单位，第一条指令的偏移量为零。偏移量不一定与 CIL 指令的开始位置匹配。例如，表示字节范围结束的偏移量通常指的是指令的最后一个字节。长度也以字节为单位。

在上述表的 3 - 7 中，属于同一方法的每一行在其父表中必须是连续的。

#### 1.3.1. SymConstant 表

_SymConstant_ 表的每一行描述一个常量，如下所示：

 | Offset | Size | Field             | Description                |
 | :----- | :--- | :---------------- | :------------------------- |
 | 0      | 4    | **Scope**         | 父作用域的索引             |
 | 4      | 4    | **Name**          | _SymString_ 堆中名称的索引 |
 | 8      | 4    | **Signature**     | _SymMisc_ 堆中签名的索引   |
 | 12     | 4    | **SignatureSize** | 签名的长度                 |
 | 16     | 4    | **Value**         | _SymMisc_ 堆中值的索引     |
 | 20     | 4    | **ValueSize**     | 值的长度。                 |

常量的值的编码方式与 [Partition II]() 中 _Constant_ 元数据表的 **Value** 列的 *Blob* 相同，只是没有长度前缀。 

#### 1.3.2. SymDocument 表

_SymDocument_ 表的每一行描述一个源文档，如下所示。文档可以间接引用（通过其 URL）或直接作为 _SymMisc_ 堆的一部分并入 CILDB 文件。本小节中提到的 GUID 值未由此标准定义；只是为它们预留了空间。

 | Offset | Size | Field              | Description                                                  |
 | :----- | :--- | :----------------- | :----------------------------------------------------------- |
 | 0      | 16   | **Language**       | 语言的 GUID。                                                |
 | 16     | 16   | **LanguageVendor** | 语言供应商的 GUID。                                          |
 | 32     | 16   | **DocumentType**   | 文档类型的 GUID。                                            |
 | 48     | 16   | **AlgorithmId**    | 校验和算法的 GUID；如果没有校验和，则为 0。                  |
 | 64     | 4    | **CheckSumSize**   | 校验和的大小；如果没有校验和，则为 0。                       |
 | 68     | 4    | **CheckSumEntry**  | _SymMisc_ 堆中校验和的索引；如果没有校验和，则为 0。         |
 | 72     | 4    | **SourceSize**     | _SymMisc_ 堆中源的大小；如果源文档没有直接并入文件，则为 0。 |
 | 76     | 4    | **SourceEntry**    | _SymMisc_ 堆中源的索引；如果源文档没有直接并入文件，则为 0。 |
 | 80     | 4    | **UrlEntry**       | 文档 URL 在 _SymString_ 堆中的索引。                         |

#### 1.3.3. SymMethod 表

_SymMethod_表的每一行格式如下：

 | Offset | Size | Field              | Description                                  |
 | :----- | :--- | :----------------- | :------------------------------------------- |
 | 0      | 4    | **MethodToken**    | _MethodDef_ 元数据令牌。                     |
 | 4      | 8    | **Scopes**         | _SymScope_ 表的 [Start, Stop) 范围。         |
 | 12     | 8    | **Vars**           | _SymVariable_ 表的 [Start, Stop) 范围。      |
 | 20     | 8    | **Using**          | _SymUsing_ 表的 [Start, Stop) 范围。         |
 | 28     | 8    | **Constant**       | _SymConstant_ 表的 [Start, Stop) 范围。      |
 | 36     | 8    | **Documents**      | _SymDocument_ 表的 [Start, Stop) 范围。      |
 | 44     | 8    | **SequencePoints** | _SymSequencePoint_ 表的 [Start, Stop) 范围。 |

每个 [Start, Stop) 范围表示为两个 4 字节的整数。第一个整数是第一个相关表行的索引；第二个整数是最后一个相关表行之后的索引。

_SymMethod_ 表的行按照 **MethodToken** 字段的升序排序。每个方法最多有一行。

#### 1.3.4. SymSequencePoint 表

_SymSequencePoint_ 表的每一行描述一个序列点，如下所示：

 | Offset | Size | Field           | Description                          |
 | :----- | :--- | :-------------- | :----------------------------------- |
 | 0      | 4    | **Offset**      | 序列点的 CIL 偏移量。                |
 | 4      | 4    | **StartLine**   | 源文档的起始行。                     |
 | 8      | 4    | **StartColumn** | 起始列，如果未指定，则为 0。         |
 | 12     | 4    | **EndLine**     | 源文档的结束行，如果未指定，则为 0。 |
 | 16     | 4    | **EndColumn**   | 结束列，如果未指定，则为 0。         |
 | 20     | 4    | **Doc**         | 源文档在 _SymString_ 堆中的索引。    |

**EndLine** 和 **EndColumn** 一起指定了与序列点关联的最后一个字节位置的 "过去一列"。换句话说，它们指定了一个半开区间 [start, end) 的结束。

属于同一方法的 _SymSequencePoint_ 的行必须是连续的，并按照 **Offset** 的升序排序。

#### 1.3.5. SymScope 表

_SymScope_表的每一行描述一个范围，如下所示：

 | Offset | Size | Field           | Description                                         |
 | :----- | :--- | :-------------- | :-------------------------------------------------- |
 | 0      | 4    | **Parent**      | 父 _SymScope_ 行的索引，如果范围没有父级，则为 -1。 |
 | 4      | 4    | **StartOffset** | 范围中第一个字节的 CIL 偏移量。                     |
 | 8      | 4    | **EndOffset**   | 范围中最后一个字节的 CIL 偏移量。                   |
 | 12     | 4    | **HasChildren** | 如果范围有子范围，则为 1；否则为 0                  |
 | 16     | 4    | **HasVars**     | 如果范围有变量，则为 1；否则为 0                    |

属于一个方法的范围必须形成一个树，有以下约束：
 * 父范围必须在其子范围之前。
 * 子范围的 **StartOffset** 和 **EndOffset** 必须在其父范围指定的偏移量范围（包含）内。


#### 1.3.6. SymVariable 表

_SymVariable_表的每一行描述一个局部变量。

 | Offset | Size | Field             | Description                                |
 | :----- | :--- | :---------------- | :----------------------------------------- |
 | 0      | 4    | **Scope**         | 父作用域的索引                             |
 | 4      | 4    | **Name**          | 变量名在 _SymString_ 堆中的索引。            |
 | 8      | 4    | **Attributes**    | 描述变量的标志（见下文）。                 |
 | 12     | 4    | **Signature**     | 签名在 *SymMisc* 堆中的索引。                  |
 | 16     | 4    | **SignatureSize** | 签名的长度。                               |
 | 20     | 4    | **AddressKind**   | 总是 1。                                    |
 | 24     | 4    | **Address1**      | 局部变量号。                               |
 | 28     | 4    | **Address2**      | 总是 0。                                    |
 | 32     | 4    | **Address3**      | 总是 0。                                    |
 | 36     | 4    | **StartOffset**   | 变量首次可见的 CIL 偏移量。                  |
 | 40     | 4    | **EndOffset**     | 变量最后可见的 CIL 偏移量。                  |
 | 44     | 4    | **Sequence**      | 总是 0。                                    |
 | 48     | 4    | **IsParam**       | 总是 0。                                    |
 | 52     | 4    | **IsHidden**      | 如果变量应该对调试器隐藏，则为 1；否则为 0。 |

**Attributes** 的最低有效位表示变量是用户生成的（0）还是编译器生成的（1）。其他位保留，应设为零。

因为参数已经由元数据完全描述，所以它们不会出现在 _SymVariable_ 表中。

#### 1.3.7. SymUsing 表

_SymUsing_ 表的每一行描述了一个命名空间的导入，如下所示：

 | Offset | Size | Field         | Description                     |
 | :----- | :--- | :------------ | :------------------------------ |
 | 0      | 4    | **Scope**     | 父作用域的索引                  |
 | 4      | 4    | **Namespace** | 命名空间在 _SymString_ 堆中的索引 |

#### 1.3.8. SymMisc 堆

_SymMisc_ 堆存储各种字节序列（例如，签名和校验和）。 

#### 1.3.9. SymString 堆

_SymString_ 堆中的字节流与 ***Strings Heap*** 的形式相同。

>---
### 1.4. 签名

变量和常量的签名被编码为 _SymMisc_ 堆中的一个索引，以及一个签名大小。字节的值与 _FieldSig_ 的类似，并包括前缀 `FIELD`（0x6），即使变量不是字段。因为签名的长度在表中编码，所以它不包含在 _SymMisc_ 堆中。例如，类型 `int32` 被编码为 "0x06 0x08"。 

---