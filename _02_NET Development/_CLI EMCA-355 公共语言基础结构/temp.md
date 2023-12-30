
## 15. 定义属性
<a id="property"></a>

属性是通过使用 **.property** 指令声明的。属性只能在类型内部声明 (即，不支持全局属性)。

 | _ClassMember_ ::=
 | ----
 | **.property** _PropHeader_ `'{'` _PropMember_* `'}'`

请参阅 §[II.22.34](ii.22.34-property-0x17.md) 和 §[II.22.35](ii.22.35-propertymap-0x15.md) 了解如何在元数据中存储属性信息。

 | _PropHeader_ ::=
 | ----
 | \[ `specialname` \] \[ `rtspecialname` \] _CallConv_ _Type_ _Id_ `'('` _Parameters_ `')'`

**.property** 指令指定了调用约定 (§[II.15.3](ii.15.3-calling-convention.md)) 、类型、名称和括号中的参数。`specialname` 将属性标记为对其他工具*特殊*，而 `rtspecialname` 将属性标记为对 CLI *特殊*。属性的签名 (即，_PropHeader_ 产生) 应与属性的 **.get** 方法的签名匹配 (见下文) 

_[原因：_ 目前没有需要用 `rtspecialname` 标记的属性名称。它是为了扩展，未来的标准化，以及增加属性和方法声明之间的一致性 (实例和类型初始化方法应该用这个属性标记)。_结束原因]_

虽然 CLI 对构成属性的方法没有限制，但 CLS (参见 [Partition I]()) 规定了一组一致性约束。一个属性可以在其主体中包含任意数量的方法。下表显示了如何识别这些方法，并提供了每种项目的简短描述：

 | _PropMember_ ::=                                                                          | 描述                                       | 条款                                         |
 | ----------------------------------------------------------------------------------------- | ------------------------------------------ | -------------------------------------------- |
 | \| `.custom` _CustomDecl_                                                                 | 自定义属性。                               | §[II.21](ii.21-custom-attributes.md)         |
 | \| `.get` _CallConv_ _Type_ [ _TypeSpec_ `'::'` ] _MethodName_ `'('` _Parameters_ `')'`   | 指定属性的 getter。                        |
 | \| `.other` _CallConv_ _Type_ [ _TypeSpec_ `'::'` ] _MethodName_ `'('` _Parameters_ `')'` | 指定属性的除 getter 或 setter 之外的方法。 |
 | \| `.set` _CallConv_ _Type_ [ _TypeSpec_ `'::'` ] _MethodName_ `'('` _Parameters_ `')'`   | 指定属性的 setter。                        |
 | \| _ExternSourceDecl_                                                                     | `.line` 或 `#line`                         | §[II.5.7](ii.5.7-source-line-information.md) |

**.get** 指定了此属性的 *getter*。_TypeSpec_ 默认为当前类型。一个属性只能指定一个 getter。为了符合 CLS，getter 的定义应被标记为 `specialname`。

**.set** 指定了此属性的 *setter*。_TypeSpec_ 默认为当前类型。一个属性只能指定一个 setter。为了符合 CLS，setter 的定义应被标记为 `specialname`。

**.other** 用于指定此属性包含的任何其他方法。

此外，还可以指定自定义属性 (§[II.21](ii.21-custom-attributes.md)) 或源行声明。

_[示例：_ 这显示了属性 `count` 的声明。

 ```ilasm
 .class public auto autochar MyCount extends [mscorlib]System.Object {
   .method virtual hidebysig public specialname instance int32 get_Count() {
   // getter 的主体
   }

   .method virtual hidebysig public specialname instance void set_Count(
       int32 newCount) {
   // setter 的主体
   } 

   .method virtual hidebysig public instance void reset_Count() {
   // refresh 方法的主体
   } 

   // 属性的声明
   .property int32 Count() {
     .get instance int32 MyCount::get_Count()
     .set instance void MyCount::set_Count(int32)
     .other instance void MyCount::reset_Count()
   }
 }
 ```

_结束示例]_

## 16. 定义事件
<a id="event"></a>

事件是在类型内部使用 **.event** 指令声明的；没有全局事件。

 | _ClassMember_ ::=                                   | 条款                      |
 | --------------------------------------------------- | ------------------------- |
 | **.event** _EventHeader_ `'{'` _EventMember_* `'}'` |
 | \| &hellip;                                         | §[II.9](ii.9-generics.md) |
 
参见 §[II.22.13](ii.22.13-event-0x14.md) 和 §[II.22.11](ii.22.11-declsecurity-0x0e.md)

 | _EventHeader_ ::=
 | ----
 | [ `specialname` ] [ `rtspecialname` ] [ _TypeSpec_ ] _Id_
 
在典型的使用中，_TypeSpec_ (如果存在) 标识了一个委托，其签名与传递给事件的 fire 方法的参数匹配。

事件头可以包含关键字 **specialname** 或 **rtspecialname**。**specialname** 为其他工具标记属性的名称，而 **rtspecialname** 为运行时标记事件的名称为特殊。

_[原理：_ 目前没有需要用 **rtspecialname** 标记的事件名称。它是为了扩展，未来的标准化，以及增加事件和方法 (实例和类型初始化方法应该用这个属性标记) 声明之间的一致性而提供的。_原理结束]_

 | _EventMember_ ::=                                                                            | 描述               | 条款                                         |
 | -------------------------------------------------------------------------------------------- | ------------------ | -------------------------------------------- |
 | `.addon` _CallConv_ _Type_ [ _TypeSpec_ `'::'` ] _MethodName_ `'('` _Parameters_ `')'`       | 事件的添加方法。   |
 | \| `.custom` _CustomDecl_                                                                    | 自定义属性。       | §[II.21](ii.21-custom-attributes.md)         |
 | \| `.fire` _CallConv_ _Type_ [ _TypeSpec_ `'::'` ] _MethodName_ `'('` _Parameters_ `')'`     | 事件的触发方法。   |
 | \| `.other` _CallConv_ _Type_ [ _TypeSpec_ `'::'` ] _MethodName_ `'('` _Parameters_ `')'`    | 其他方法。         |
 | \| `.removeon` _CallConv_ _Type_ [ _TypeSpec_ `'::'` ] _MethodName_ `'('` _Parameters_ `')'` | 事件的移除方法。   |
 | \| _ExternSourceDecl_                                                                        | `.line` 或 `#line` | §[II.5.7](ii.5.7-source-line-information.md) |

**.addon** 指令指定 *add* 方法，_TypeSpec_ 默认为与事件相同的类型。CLS 为事件指定命名约定和一致性约束，并要求 *add* 方法的定义被标记为 **specialname**。 (§[I.10.4](i.10.4-naming-patterns.md)) 

**.removeon** 指令指定 *remove* 方法，_TypeSpec_ 默认为与事件相同的类型。CLS 为事件指定命名约定和一致性约束，并要求 *remove* 方法的定义被标记为 **specialname**。 (§[I.10.4](i.10.4-naming-patterns.md)) 

**.fire** 指令指定 *fire* 方法，_TypeSpec_ 默认为与事件相同的类型。CLS 为事件指定命名约定和一致性约束，并要求 *fire* 方法的定义被标记为 **specialname**。 (§[I.10.4](i.10.4-naming-patterns.md)) 

事件可以包含任意数量的其他方法，这些方法使用 **.other** 指令指定。从 CLI 的角度来看，这些方法只通过事件彼此关联。如果它们有特殊的语义，这需要由实现者来记录。事件也可以有与它们关联的自定义属性 (§[II.21](ii.21-custom-attributes.md))，并且它们可以声明源行信息。

这是一个示例，展示了如何声明一个事件，其对应的委托，以及事件的 *add*，*remove* 和 *fire* 方法的典型实现。事件和方法在名为 `Counter` 的类中声明。

```ilasm
// 委托
.class private sealed auto autochar TimeUpEventHandler extends
     [mscorlib]System.Delegate {

  .method public hidebysig specialname rtspecialname instance void .ctor(object
      'object', native int 'method') runtime managed {}

 .method public hidebysig virtual instance void Invoke() runtime managed {}

 .method public hidebysig newslot virtual instance class
    [mscorlib]System.IAsyncResult BeginInvoke(class
    [mscorlib]System.AsyncCallback callback, object 'object') runtime managed {}

 .method public hidebysig newslot virtual instance void EndInvoke(class
     [mscorlib]System.IAsyncResult result) runtime managed {}
}

// 声明事件的类
.class public auto autochar Counter extends [mscorlib]System.Object {

  // 存储处理程序的字段，初始化为 null
  .field private class TimeUpEventHandler timeUpEventHandler

  // 事件声明
  .event TimeUpEventHandler startStopEvent {
    .addon instance void Counter::add_TimeUp(class TimeUpEventHandler 'handler')
    .removeon instance void Counter::remove_TimeUp(class TimeUpEventHandler 'handler')
    .fire instance void Counter::fire_TimeUpEvent()
  }

  // add 方法，将处理程序与现有的委托组合
  .method public hidebysig virtual specialname instance void add_TimeUp(class TimeUpEventHandler 'handler') {
    .maxstack 4
    ldarg.0
    dup
    ldfld class TimeUpEventHandler Counter::TimeUpEventHandler
    ldarg 'handler'
    call class[mscorlib]System.Delegate
       [mscorlib]System.Delegate::Combine(class [mscorlib]System.Delegate, class
       [mscorlib]System.Delegate)
    castclass TimeUpEventHandler
    stfld class TimeUpEventHandler Counter::timeUpEventHandler
    ret
  }

  // remove 方法，从委托中移除处理程序
  .method virtual public specialname void remove_TimeUp(class TimeUpEventHandler 'handler') {
    .maxstack 4
    ldarg.0
    dup
    ldfld class TimeUpEventHandler Counter::timeUpEventHandler
    ldarg 'handler'
    call class[mscorlib]System.Delegate
       [mscorlib]System.Delegate::Remove(class
       [mscorlib]System.Delegate, class [mscorlib]System.Delegate)
    castclass TimeUpEventHandler
    stfld class TimeUpEventHandler Counter::timeUpEventHandler
    ret
  }

  // fire 方法
  .method virtual family specialname void fire_TimeUpEvent() {
    .maxstack 3
    ldarg.0
    ldfld class TimeUpEventHandler Counter::timeUpEventHandler
    callvirt instance void TimeUpEventHandler::Invoke()
    ret
  }
} // Counter 类结束
```

## 17. 异常处理
<a id="SEHBlock"></a>

在CLI中，一个方法可以定义一系列被称为 _受保护_ 的CIL指令。这被称为 _try块_。然后，它可以将一个或多个 _处理程序_ 与该try块关联。如果在try块内的任何地方执行过程中发生异常，就会创建一个描述问题的异常对象。然后CLI接管，将控制权从抛出异常的点转移到愿意处理该异常的代码块。参见 [Partition I]()。

没有两个处理程序 (fault，filter，catch或finally) 可以有相同的起始地址。当发生异常时，需要将执行地址转换为发生异常的最词法嵌套的try块。

 | _SEHBlock_ ::=
 | ----
 | _TryBlock_ _SEHClause_ [ _SEHClause_* ]

接下来的几个子条款通过描述参与异常处理的五种代码块：**try**，**catch**，**filter**，**finally**和**fault**，来扩展这个简单的描述。 (注意，给定的 _TryBlock_ 可以有多少个，以及什么类型的 _SEHClause_ 有限制；详见 [Partition I]()。) 

以下是详细描述的剩余语法项；它们在此处收集以供参考。

 | _TryBlock_ ::=              | 描述                                   |
 | --------------------------- | -------------------------------------- |
 | `.try` _Label_ `to` _Label_ | 保护从第一个标签到第二个标签之前的区域 |
 | \| `.try` _ScopeBlock_      | _ScopeBlock_ 是受保护的                |

 | _SEHClause_ ::=                        | 描述                             |
 | -------------------------------------- | -------------------------------- |
 | `catch` _TypeReference_ _HandlerBlock_ | 捕获指定类型的所有对象           |
 | \| `fault` _HandlerBlock_              | 处理所有异常，但不处理正常退出   |
 | \| `filter` _Label_ _HandlerBlock_     | 只有在过滤器成功时才进入处理程序 |
 | \| `finally` _HandlerBlock_            | 处理所有异常和正常退出           |

 | _HandlerBlock_ ::=             | 描述                                     |
 | ------------------------------ | ---------------------------------------- |
 | `handler` _Label_ `to` _Label_ | 处理程序范围从第一个标签到第二个标签之前 |
 | \| _ScopeBlock_                | _ScopeBlock_ 是处理程序块                |

### 17.1. 受保护的块

*try*，或 *protected*，或 *guarded* 块是用 **.try** 指令声明的。

 | _TryBlock_ ::=              | 描述                                       |
 | --------------------------- | ------------------------------------------ |
 | `.try` _Label_ `to` _Label_ | 从第一个标签到第二个标签之前的区域受保护。 |
 | \| `.try` _ScopeBlock_      | _ScopeBlock_ 受保护                        |

在第一种情况下，受保护的块由两个标签分隔。第一个标签是要受保护的第一条指令，而第二个标签是要受保护的最后一条指令之后的指令。这两个标签都应在此点之前定义。

第二种情况在 **.try** 指令后使用范围块 (§[II.15.4.4](ii.15.4.4-scope-blocks.md)) ——该范围内的指令是要受保护的指令。

### 17.2. 处理程序块

 | _HandlerBlock_ ::=                | 描述                                     |
 | --------------------------------- | ---------------------------------------- |
 | \| `handler` _Label_ `to` _Label_ | 处理程序范围从第一个标签到第二个标签之前 |
 | \| _ScopeBlock_                   | _ScopeBlock_ 就是处理程序块              |

在第一种情况下，标签包围了处理程序块的指令，第一个标签是处理程序的第一条指令，而第二个标签是处理程序后面的指令。在第二种情况下，处理程序块只是一个作用域块。

### 17.3. Catch块

使用 **catch** 关键字声明一个catch块。这指定了该子句设计用来处理的异常对象的类型，以及处理程序代码本身。

 | _SEHClause_ ::=
 | ----
 | `catch` _TypeReference_ _HandlerBlock_

_[示例：_

 ```ilasm
 .try {
   …                            // 受保护的指令
   leave exitSEH                // 正常退出
 }
 catch [mscorlib]System.FormatException {
   …                            // 处理异常
   pop                          // 弹出异常对象
   leave exitSEH                // 离开catch处理程序
 }
 exitSEH:                       // 在此处继续
 ```

_结束示例]_

### 17.4. 过滤块

过滤块是使用 **filter** 关键字声明的。

 | _SEHClause_ ::= &hellip;
 | ----
 | \| `filter` _Label_ _HandlerBlock_
 | \| `filter` _Scope_ _HandlerBlock_

过滤代码从指定的标签开始，结束于处理程序块的第一条指令。 (注意，CLI 要求过滤块在 CIL 流中必须紧接其对应的处理程序块。) 

_[示例：_

 ```ilasm
 .method public static void m () {
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

_结束示例]_

### 17.5. Finally 块

使用 **finally** 关键字声明 finally 块。这指定了处理程序代码，语法如下：

 | _SEHClause_ ::= &hellip;
 | ----
 | \| `finally` _HandlerBlock_
 
在 **finally** 处理程序中可以执行的最后一个可能的 CIL 指令应该是 `endfinally`。

_[示例：_

 ```ilasm
 .try {
   …              // 受保护的指令
   leave exitTry  // 应使用 leave
 }
 finally {
   …              // finally 处理程序
   endfinally
 }
 exitTry:         // 回到正常
 ```

_示例结束]_

### 17.6. Fault处理程序

使用 **fault** 关键字声明一个fault块。这指定了处理程序代码，具有以下语法：

 | _SEHClause_ ::= &hellip;
 | ----
 | \| `fault` _HandlerBlock_

在 **fault** 处理程序中可以执行的最后一个可能的CIL指令应该是 `endfault`。

_[示例：_

 ```ilasm
 .method public static void m() {
   startTry:
       …                        // 受保护的指令
       leave exitSEH            // 应使用leave
   endTry:

   startFault:
       …                        // fault处理程序指令
       endfault

   endFault:
       .try startTry to endTry  // fault处理程序startFault到endFault

   exitSEH:                     // 回到正常
 }
 ```

_结束示例]_

## 18. SecurityDecl：声明式安全
<a id="SecurityDecl"></a>

许多针对 CLI 的语言使用属性语法将声明式安全属性附加到元数据中的项。这些信息实际上是由编译器转换为存储在元数据中的基于 XML 的表示形式，参见 §[II.22.11](ii.22.11-declsecurity-0x0e.md)。相比之下，*ilasm* 要求在其输入中表示转换信息。

 | _SecurityDecl_ ::=
 | ----
 | `.permissionset` _SecAction_ = `'('` _Bytes_ `')'`
 | \| `.permission` _SecAction_ _TypeReference_ `'('` _NameValPairs_ `')'`

 | _NameValPairs_ ::=
 | ----
 | _NameValPair_ [ `','` _NameValPair_ ]*

 | _NameValPair_ ::=
 | ----
 | _SQSTRING_ `'='` _SQSTRING_

在 **.permission** 中，_TypeReference_ 指定了权限类，*NameValPair*s 指定了设置。参见 §[II.22.11](ii.22.11-declsecurity-0x0e.md)

在 **.permissionset** 中，字节指定了安全设置的编码版本：

 | _SecAction_ ::=   | 描述                           |
 | ----------------- | ------------------------------ |
 | `assert`          | 断言权限，以便调用者不需要它。 |
 | \| `demand`       | 要求所有调用者的权限。         |
 | \| `deny`         | 拒绝权限，以便检查失败。       |
 | \| `inheritcheck` | 要求派生类的权限。             |
 | \| `linkcheck`    | 要求调用者的权限。             |
 | \| `permitonly`   | 减少权限，以便检查失败。       |
 | \| `reqopt`       | 请求可选的额外权限。           |
 | \| `reqrefuse`    | 拒绝被授予这些权限。           |

## 19. custom：自定义特性
<a id= "custom"></a>

自定义特性向元数据添加用户定义的注解。自定义特性允许将类型的实例与元数据的任何元素一起存储。这种机制可以用于在编译时存储特定于应用程序的信息，并在运行时或当其他工具读取元数据时访问它。虽然任何用户定义的类型都可以用作特性，但是 CLS 的兼容性要求特性是基类为 `System.Attribute` 的类型的实例。CLI 预定义了一些特性类型并使用它们来控制运行时行为。一些语言预定义特性类型来表示 CTS 中没有直接表示的语言特性。用户或其他工具可以自定义和使用额外的特性类型。

自定义特性是使用 **.custom** 指令声明的，后面跟着类型构造函数的方法声明，然后是可选的括号中的 _Bytes_：

<pre>
    <em>CustomDecl</em> ::= <em>Ctor</em> [ '=' '(' <em>Bytes</em> ')' ]
</pre>

_Ctor_ 项表示方法声明，特定于方法名为 `.ctor` 的情况。

 ```cil
 custom instance void myAttribute::.ctor(bool, bool) = ( 01 00 00 01 00 00 )
 ```

自定义特性可以附加到元数据中的任何项目，除了自定义特性本身。通常，自定义特性附加到程序集、模块、类、接口、值类型、方法、字段、属性、泛型参数和事件 (自定义特性附加到紧接着的声明) 

如果构造函数不带参数，则不需要 _Bytes_ 项。在这种情况下，重要的只是自定义特性的存在。

如果构造函数带有参数，它们的值应在 _Bytes_ 项中指定。

下面显示了一个类，它被标记有名为 `System.CLSCompliantAttribute` 的特性，以及一个被标记有名为 `System.ObsoleteAttribute` 的方法。

```cil
class public MyClass extends [mscorlib]System.Object
{ 
    .custom instance void [mscorlib]System.CLSCompliantAttribute::.ctor(bool) =
        ( 01 00 01 00 00 )
    .method public static void CalculateTotals() cil managed
{ 
    .custom instance void [mscorlib]System.ObsoleteAttribute::.ctor() =
        ( 01 00 00 00 )
    ret
}
```

### 19.1. CLS 约定：自定义特性的使用

CLS对自定义特性的使用施加了某些约定，以便改进跨语言操作。

### 19.2. CLI 使用的特性

有两种类型的自定义特性，称为 _真正的自定义特性_ 和 _伪自定义特性_。在定义时，自定义特性和伪自定义特性被不同地处理，如下所示：

 * 自定义特性直接存储到元数据中；保存其定义数据的 'blob' 按原样存储。稍后可以检索该 'blob'。
 * 伪自定义特性被识别是因为其名称是一个短列表中的一个。而不是直接将其 'blob' 存储在元数据中，而是解析该 'blob'，并使用其中包含的信息来设置元数据表中的位和 / 或字段。然后丢弃 'blob'；稍后无法检索。

因此，伪自定义特性用于捕获用户指令，使用编译器为真正的自定义特性提供的相同熟悉的语法，但这些用户指令然后被存储到元数据表的更高效的空间形式中。在运行时检查表也比检查真正的自定义特性快。

许多自定义特性是由更高层的软件发明的。它们被 CLI 存储并返回，而 CLI 不知道也不关心它们的 “含义”。但是所有伪自定义特性，加上一组真正的自定义特性，对编译器和 CLI 都特别感兴趣。这样的自定义特性的一个例子是 `System.Reflection.DefaultMemberAttribute`。这被存储在元数据中作为一个真正的自定义特性 'blob'，但是当被调用来调用类型的默认成员 (特性) 时，反射使用这个自定义特性。

以下的子条目列出了所有的伪自定义特性和 _显著的_ 自定义特性，其中 _显著的_ 意味着 CLI 和 / 或编译器直接关注它们，并以某种方式影响它们的行为。

为了防止将来的名称冲突，`System` 命名空间中的所有自定义特性都保留用于标准化。

#### 19.2.1. 伪自定义特性

下表列出了 CLI 的伪自定义特性。 并非所有这些特性都在此标准中指定，但所有的名称都是保留的，不得用于其他目的。它们在 `System.Reflection`，`System.Runtime.CompilerServices` 和 `System.Runtime.InteropServices` 命名空间中定义。

 | 特性                           | 描述                                                 |
 | ------------------------------ | ---------------------------------------------------- |
 | `AssemblyAlgorithmIDAttribute` | 记录使用的哈希算法的 ID (仅保留)                     |
 | `AssemblyFlagsAttribute`       | 记录此程序集的标志 (仅保留)                          |
 | `DllImportAttribute`           | 提供关于在非托管库中实现的代码的信息                 |
 | `FieldOffsetAttribute`         | 指定字段在其封闭类或值类型中的字节偏移量             |
 | `InAttribute`                  | 表示方法参数是 `[in]` 参数                           |
 | `MarshalAsAttribute`           | 指定数据项在托管代码和非托管代码之间如何进行编组     |
 | `MethodImplAttribute`          | 指定方法实现的详细信息                               |
 | `OutAttribute`                 | 表示方法参数是 `[out]` 参数                          |
 | `StructLayoutAttribute`        | 允许调用者控制类或值类型的字段在托管内存中的布局方式 |

这些特性影响元数据中的位和字段，如下所示：

 * `AssemblyAlgorithmIDAttribute`：设置 _Assembly_._HashAlgId_ 字段。
 * `AssemblyFlagsAttribute`：设置 _Assembly_._Flags_ 字段。
 * `DllImportAttribute`：为带有特性的方法设置 _Method_._Flags_.`PinvokeImpl` 位；还在 _ImplMap_ 表中添加新行 (设置 _MappingFlags_，_MemberForwarded_，_ImportName_ 和 _ImportScope_ 列)。
 * `FieldOffsetAttribute`：为带有特性的字段设置 _FieldLayout_._Offset_ 值。
 * `InAttribute`：为带有特性的参数设置 _Param_._Flags_.`In` 位。
 * `MarshalAsAttribute`：为带有特性的字段设置 _Field_._Flags_.`HasFieldMarshal` 位 (或为带有特性的参数设置 _Param_._Flags_.`HasFieldMarshal` 位) ；还在 _FieldMarshal_ 表中为 _Parent_ 和 _NativeType_ 列输入新行。
 * `MethodImplAttribute`：设置带有特性的方法的 _Method_._ImplFlags_ 字段。

 * `OutAttribute`：为带有特性的参数设置 _Param_._Flags_.`Out` 位。

 * `StructLayoutAttribute`：为带有特性的类型设置 _TypeDef_._Flags_.`LayoutMask` 子字段，以及可选的 _TypeDef_._Flags_.`StringFormatMask` 子字段，_ClassLayout_._PackingSize_ 和 _ClassLayout_._ClassSize_ 字段。

#### 19.2.2. CLS 定义的自定义特性

CLS 指定了某些自定义特性，并要求符合规范的语言支持它们。这些特性位于 `System` 下。

 | 特性                      | 描述                                                        |
 | ------------------------- | ----------------------------------------------------------- |
 | `AttributeUsageAttribute` | 用于指定一个特性类的预期用途。                              |
 | `ObsoleteAttribute`       | 表示一个元素不应被使用。                                    |
 | `CLSCompliantAttribute`   | 通过特性对象上的实例字段，指示一个元素是否被声明为符合CLS。 |

#### 19.2.3. 安全性的自定义属性

以下自定义特性在 `System.Net` 和 `System.Security.Permissions` 命名空间中定义。注意，这些都是基类；在程序集中找到的安全特性的实例将是这些类的子类。

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

请注意，任何其他与安全性相关的自定义特性 (即，任何从 `System.Security.Permissions.SecurityAttribute` 派生的自定义特性) 包含在程序集中，都可能导致 CLI 的符合实现在加载时拒绝此类程序集，或者如果尝试访问这些与安全性相关的自定义特性，则在运行时抛出异常。 (这个声明对于任何无法解析的自定义特性都是成立的；与安全性相关的自定义特性只是一个特殊的情况) 

#### 19.2.4. 用于 TLS 的自定义特性

在 `System` 命名空间中定义了一个表示 TLS (线程局部存储) 字段的自定义特性。

 | 特性                    | 描述                           |
 | ----------------------- | ------------------------------ |
 | `ThreadStaticAttribute` | 提供相对于线程的类型成员字段。 |

#### 19.2.5. 自定义特性，各种

以下自定义特性控制 CLI 的各个方面：

 | 特性                              | 命名空间                          | 描述                                                                         |
 | --------------------------------- | --------------------------------- | ---------------------------------------------------------------------------- |
 | `ConditionalAttribute`            | `System.Diagnostics`              | 用于标记方法为可调用，基于某些编译时条件。如果条件为假，方法将不会被调用     |
 | `DecimalConstantAttribute`        | `System.Runtime.CompilerServices` | 在元数据中存储十进制常数的值                                                 |
 | `DefaultMemberAttribute`          | `System.Reflection`               | 定义了一个类型的成员，该成员是反射的 `InvokeMember` 使用的默认成员。         |
 | `CompilationRelaxationsAttribute` | `System.Runtime.CompilerServices` | 指示来自指令检查的异常是严格的还是宽松的。                                   |
 | `FlagsAttribute`                  | `System`                          | 自定义特性，表示枚举应被视为位字段；也就是说，一组标志                       |
 | `IndexerNameAttribute`            | `System.Runtime.CompilerServices` | 指示具有一个或多个参数的特性将以何种名称在不直接支持此功能的编程语言中被知道 |
 | `ParamArrayAttribute`             | `System`                          | 表示该方法将允许在其调用中使用可变数量的参数                                 |

##  20. 元数据逻辑格式：表格

此节定义了描述元数据的结构，以及它们如何交叉索引。这对应于元数据如何在从 PE 文件读入内存后进行布局。

元数据存储在两种结构中：表格 (记录数组) 和堆。任何模块中都有四个堆：String，Blob，Userstring 和 Guid。前三个是字节数组 (因此，这些堆的有效索引可能是 0，23，25，39 等)。Guid 堆是 GUID 的数组，每个 GUID 宽 16 字节。它的第一个元素编号为 1，第二个为 2，依此类推。

每个表的每个列的每个条目要么是常数，要么是索引。

常数要么是文字值 (例如，ALG_SID_SHA1 = 4，存储在 *Assembly* 表的 *HashAlgId* 列中)，要么更常见的是位掩码。大多数位掩码 (它们几乎都被称为 *Flags*) 宽 2 字节 (例如，*Field* 表中的 *Flags* 列)，但有几个是 4 字节的 (例如，*TypeDef* 表中的 *Flags* 列)。

每个索引宽度为 2 或 4 字节。索引指向同一表或其他表，或者指向四个堆中的一个。只有当对于特定模块需要时，表中的每个索引列的大小才会变为 4 字节。因此，如果特定列索引的表或表的最高行号适合于 2 字节值，则索引器列只需要宽 2 字节。相反，对于包含 64K 或更多行的表，该表的索引器将宽 4 字节。

表格的索引从 1 开始，因此索引 1 表示任何给定元数据表的第一行。 (索引值为零表示它根本不索引行；也就是说，它的行为就像一个空引用。) 

索引元数据表的列有两种。  (有关这些表的物理表示的详细信息，请参见 §[II.24.2.6](ii.24.2.6-metadata-stream.md)) ：

 * 简单 &mdash; 这样的列索引一个且只有一个表。例如，*TypeDef* 表中的 *FieldList* 列始终索引 *Field* 表。因此，该列中的所有值都是简单的整数，给出目标表中的行号

 * 编码 &mdash; 这样的列可以索引几个表中的任何一个。例如，*TypeDef* 表中的 *Extends* 列可以索引到 *TypeDef* 或 *TypeRef* 表。该索引值的几位被保留用于定义它的目标表。在大多数情况下，此规范讨论的是索引值在目标表中解码为行号之后的值。然而，规范在描述元数据的物理布局的部分中包含了这些编码索引的描述 (§[II.24](ii.24-metadata-physical-layout.md))。

元数据保留编译器或代码生成器创建的名称字符串，不做任何更改。本质上，它将每个字符串视为不透明的 blob。特别是，它保留了大小写。CLI 对存储在元数据中并随后由 CLI 处理的名称的长度没有限制。

匹配 *AssemblyRef* 和 *ModuleRef* 到它们对应的 *Assembly* 和 *Module* 应该是不区分大小写的 (参见 [Partition I]())。然而，所有其他名称匹配 (类型，字段，方法，属性，事件) 应该是精确的&mdash;这样，所有平台上的这个级别的解析都是相同的，无论它们的操作系统是否区分大小写。

表格都有一个名称 (例如，“Assembly”) 和一个数字 (例如，0x20)。每个表的编号都立即列在以下子条款的标题中。表格的编号表示它们对应的表在 PE 文件中出现的顺序，还有一组位 (§II.24.2.6) 表示给定的表是否存在。表的数量是在该位集中的位置。

有几个表代表了常规 CLI 文件的扩展。具体来说，*ENCLog* 和 *ENCMap*，它们出现在“编辑和继续”过程中生成的临时图像中，

某些表需要按主键排序，如下所示：

 | 表                       | 主键列            |
 | ------------------------ | ----------------- |
 | *ClassLayout*            | *Parent*          |
 | *Constant*               | *Parent*          |
 | *CustomAttribute*        | *Parent*          |
 | *DeclSecurity*           | *Parent*          |
 | *FieldLayout*            | *Field*           |
 | *FieldMarshal*           | *Parent*          |
 | *FieldRVA*               | *Field*           |
 | *GenericParam*           | *Owner*           |
 | *GenericParamConstraint* | *Owner*           |
 | *ImplMap*                | *MemberForwarded* |
 | *InterfaceImpl*          | *Class*           |
 | *MethodImpl*             | *Class*           |
 | *MethodSemantics*        | *Association*     |
 | *NestedClass*            | *NestedClass*     |

此外，*InterfaceImpl* 表使用 *Interface* 列作为次要键进行排序，*GenericParam* 表使用 *Number* 列作为次要键进行排序。

最后，*TypeDef* 表有一个特殊的排序约束：封闭类的定义应在其封闭的所有类的定义之前。

元数据项 (元数据表中的记录) 由元数据 _token_ 寻址。未编码的元数据 _token_ 是 4 字节无符号整数，其中最高有效字节包含元数据表索引，三个最低有效字节包含基于 1 的记录索引。元数据表及其各自的索引在 §[II.22.2](ii.22.2-assembly-0x20.md) 和后续子条款中描述。

编码的元数据 _token_ 也包含表和记录索引，但格式不同。有关编码的详细信息，请参见 §[II.24.2.6](ii.24.2.6-metadata-stream.md)。

### 20.1. 元数据验证规则

此外，一些规则用于检查与CLS要求的符合性 (参见 [Partition I]())，即使这些规则与有效元数据无关。这些规则以 **\[CLS\]** 标签结尾。

有效元数据的规则引用单个模块。模块是任何元数据的集合，它 *可能* 通常被保存到磁盘文件中。这包括编译器和链接器的输出，或脚本编译器的输出 (其中元数据通常只在内存中保存，但实际上从未保存到磁盘文件中)。

规则仅处理模块内验证。因此，检查与此标准符合性的软件无需解析引用或遍历在其他模块中定义的类型层次结构。然而，即使两个模块A和B分别分析，只包含有效的元数据，当一起查看时，它们仍然可能出错 (例如，从模块A调用到模块B中定义的方法，可能指定一个调用点签名，该签名与B中为该方法定义的签名不匹配)。

所有检查都被分类为ERROR，WARNING或CLS。

 * ERROR检查报告可能导致CLI崩溃或挂起的事情，它可能运行但产生错误答案；或者它可能完全良性。符合CLI的实现可以存在，它们不会接受违反ERROR规则的元数据，因此这样的元数据是无效的，且不可移植。

 * WARNING检查报告一些事情，虽然不是真正的错误，但可能是编译器的失误。通常，它表示编译器可以以更紧凑的方式编码相同的信息，或者元数据表示在运行时实际上没有用途的构造。所有符合规范的实现都应支持仅违反WARNING规则的元数据；因此这样的元数据既有效又可移植。

 * CLS检查报告缺乏符合Common Language Specification (参见 [Partition I]()) 的情况。这样的元数据既有效又可移植，但可能存在无法处理它的编程语言，尽管所有符合CLI的实现都支持这些构造。

验证规则分为以下几个大类：

 * **行数：**少数表只允许一行 (例如，*Module* 表)。大多数表没有这样的限制。

 * **唯一行：**没有表应包含重复的行，其中“重复”是根据其键列或列组合来定义的。

 * **有效索引：**作为索引的列应指向某个有意义的地方，如下所示：

    * 每个指向String，Blob或Userstring堆的索引应指向该堆，既不在其开始 (偏移量0) 之前，也不在其结束之后。

    * 每个指向Guid堆的索引应位于此模块的最大元素编号之间，包括1和最大元素编号。

    * 每个指向另一个元数据表的索引 (行号) 应位于0和该表的行数+1之间 (对于某些表，索引可以指向目标表的末尾，意味着它没有索引任何东西)。

 * **有效位掩码：**作为位掩码的列应只设置有效的位排列。

 * **有效RVA：**对于分配了RVA (相对虚拟地址，这是从将相应PE文件加载到内存的地址开始表示的字节偏移量) 的字段和方法，存在一些限制。

请注意，下面列出的一些规则实际上并没有说什么——例如，有些规则声明某个表允许零行或多行——在这种情况下，检查不可能失败。这样做只是为了完整性，记录这些细节确实已经被处理，而不是被忽视。

> _结束信息性文本。_

CLI对存储在元数据中并随后由CLI实现处理的名称的长度没有限制。

### 20.2. Assembly: 0x20
<a id="Assembly_0x20"></a>

_Assembly_ 表有以下列：

 * _HashAlgId_ (类型为 _AssemblyHashAlgorithm_ 的 4 字节常量，参见 §[II.23.1.1](ii.23.1.1-values-for-assemblyhashalgorithm.md)) 

 * _MajorVersion_，_MinorVersion_，_BuildNumber_，_RevisionNumber_ (每个都是 2 字节常量) 

 * _Flags_ (类型为 _AssemblyFlags_ 的 4 字节位掩码，参见 §[II.23.1.2](ii.23.1.2-values-for-assemblyflags.md)) 

 * _PublicKey_ (Blob 堆的索引) 

 * _Name_ (String 堆的索引) 

 * _Culture_ (String 堆的索引) 

_Assembly_ 表使用 **.assembly** 指令定义 (参见 §[II.6.2](ii.6.2-defining-an-assembly.md)) ；其列从相应的 **.hash** 算法，**.ver**，**.publickey** 和 **.culture** 中获取 (参见 §[II.6.2.1](ii.6.2.1-information-about-the-assembly-asmdecl.md))。 (有关示例，请参见 §[II.6.2](ii.6.2-defining-an-assembly.md)。) 

> _这只包含信息性文本。_

 1. _Assembly_ 表应包含零行或一行 \[错误\]

 2. _HashAlgId_ 应为指定的值之一 \[错误\]

 3. _MajorVersion_，_MinorVersion_，_BuildNumber_ 和 _RevisionNumber_ 可以有任何值

 4. _Flags_ 只应设置指定的值 \[错误\]

 5. _PublicKey_ 可以为 null 或非 null

 6. _Name_ 应索引 String 堆中的非空字符串 \[错误\]

 7. _Name_ 索引的字符串可以是无限长度

 8. _Culture_ 可以为 null 或非 null

 9. 如果 _Culture_ 为非 null，它应索引指定列表中的单个字符串 (参见 §[II.23.1.3](ii.23.1.3-values-for-culture.md))  \[错误\]

_[注意：_ _Name_ 是一个简单的名称 (例如，“Foo”，没有驱动器字母，没有路径，没有文件扩展名) ；在符合 POSIX 的系统上，Name 不包含冒号，不包含正斜杠，不包含反斜杠，也不包含句点。_结束注意]_

> _结束信息性文本。_

### 20.3. AssemblyOS: 0x22
<a id="AssemblyOS_0x22"></a>


_AssemblyOS_ 表有以下列：

 * _OSPlatformID_ (4 字节常数) 

 * _OSMajorVersion_ (4 字节常数) 

 * _OSMinorVersion_ (4 字节常数) 

此记录不应被发出到任何 PE 文件中。然而，如果它出现在 PE 文件中，它应被视为所有字段都为零。CLI 将忽略它。

### 20.4. AssemblyProcessor: 0x21
<a id="AssemblyProcessor_0x21"></a>


_AssemblyProcessor_ 表有以下列：

 * _Processor_ (一个4字节常数) 

此记录不应被发出到任何PE文件中。然而，如果它出现在PE文件中，应该将其字段视为零。CLI应该忽略它。

### 20.5. AssemblyRef: 0x23
<a id="AssemblyRef_0x23"></a>

_AssemblyRef_ 表有以下列：

 * _MajorVersion_，_MinorVersion_，_BuildNumber_，_RevisionNumber_ (每个都是 2 字节常量) 

 * _Flags_ (类型为 _AssemblyFlags_ 的 4 字节位掩码，参见 §[II.23.1.2](ii.23.1.2-values-for-assemblyflags.md)) 

 * _PublicKeyOrToken_ (Blob 堆的索引，表示标识此 Assembly 的作者的公钥或 _token_ ) 

 * _Name_ (String 堆的索引) 

 * _Culture_ (String 堆的索引) 

 * _HashValue_ (Blob 堆的索引) 

该表由 **.assembly extern** 指令定义 (参见 §[II.6.3](ii.6.3-referencing-assemblies.md))。其列使用与 _Assembly_ 表类似的指令填充，除了 _PublicKeyOrToken_ 列，该列使用 **.publickeytoken** 指令定义。 (有关示例，请参见 §[II.6.3](ii.6.3-referencing-assemblies.md)。) 

> _这只包含信息性文本。_

 1. _MajorVersion_，_MinorVersion_，_BuildNumber_ 和 _RevisionNumber_ 可以有任何值

 2. _Flags_ 只应设置一个位，即 `PublicKey` 位 (参见 §[II.23.1.2](ii.23.1.2-values-for-assemblyflags.md))。所有其他位应为零。 \[错误\]

 3. _PublicKeyOrToken_ 可以为 null 或非 null (注意 _Flags_.`PublicKey` 位指定 'blob' 是完整的公钥还是短哈希 _token_ ) 

 4. 如果非 null，则 _PublicKeyOrToken_ 应索引 Blob 堆中的有效偏移 \[错误\]

 5. _Name_ 应索引 String 堆中的非空字符串 (其长度没有限制)  \[错误\]

 6. _Culture_ 可以为 null 或非 null。

 7. 如果非 null，它应索引指定列表中的单个字符串 (参见 §[II.23.1.3](ii.23.1.3-values-for-culture.md))  \[错误\]

 8. _HashValue_ 可以为 null 或非 null

 9. 如果非 null，则 _HashValue_ 应索引 Blob 堆中的非空 'blob' \[错误\]

 10. _AssemblyRef_ 表不应包含重复项 (其中重复行被视为具有相同的 _MajorVersion_，_MinorVersion_，_BuildNumber_，_RevisionNumber_，_PublicKeyOrToken_，_Name_ 和 _Culture_ 的行)  \[警告\]

_[注意：_ _Name_ 是一个简单的名称 (例如，“Foo”，没有驱动器字母，没有路径，没有文件扩展名) ；在符合 POSIX 的系统上，Name 不包含冒号，不包含正斜杠，不包含反斜杠，也不包含句点。_结束注意]_

> _结束信息性文本。_

### 20.6. AssemblyRefOS: 0x25
<a id="AssemblyRefOS_0x25"></a>

_AssemblyRefOS_ 表有以下列：

 * _OSPlatformId_ (4 字节常数) 

 * _OSMajorVersion_ (4 字节常数) 

 * _OSMinorVersion_ (4 字节常数) 

 * _AssemblyRef_ (索引到 _AssemblyRef_ 表) 

这些记录不应被发出到任何 PE 文件中。然而，如果它们出现在 PE 文件中，它们应被视为其字段都为零。CLI 应忽略它们。

### 20.7. AssemblyRefProcessor: 0x24
<a id="AssemblyRefProcessor_0x24"></a>

_AssemblyRefProcessor_ 表有以下列：

 * _Processor_ (一个4字节常数) 

 * _AssemblyRef_ (一个索引，指向 _AssemblyRef_ 表) 

这些记录不应被发出到任何PE文件中。然而，如果它们出现在PE文件中，应该将其字段视为零。CLI应该忽略它们。

### 20.8. ClassLayout: 0x0F
<a id="ClassLayout_0x0F"></a>

_ClassLayout_ 表用于定义 CLI 应如何布局类或值类型的字段。 (通常，CLI 可以自由地重新排序和/或在为类或值类型定义的字段之间插入间隙。) 

_[原因：_ 此功能用于以与非托管 C 结构体完全相同的方式布局托管值类型，从而允许将托管值类型交给非托管代码，然后访问字段，就像该内存块是由非托管代码布局的一样。_结束原因]_

_ClassLayout_ 表中的信息取决于所有者类或值类型中 {`AutoLayout`,`SequentialLayout`, `ExplicitLayout`} 的 _Flags_ 值。如果类型被标记为 `SequentialLayout` 或 `ExplicitLayout`，则该类型具有布局。如果继承链中的任何类型具有布局，则其所有基类也应具有布局，直到从 `System.ValueType` 立即派生的那个 (如果它存在于类型的层次结构中) ；否则，从 `System.Object`。

> _这只包含信息性文本。_

布局不能在链的中间开始。但是，在链的任何点停止“具有布局”都是有效的。例如，在下面的图表中，类 A 从 `System.Object` 派生；类 B 从 A 派生；类 C 从 B 派生。`System.Object` 没有布局。但是 A，B 和 C 都定义了布局，这是有效的。

 ![有效的布局设置](ii.22.8-classlayout-figure-1.png)

类 E，F 和 G 的情况类似。G 没有布局，这也是有效的。下图显示了两个*无效*的设置： 

 ![无效的布局设置](ii.22.8-classlayout-figure-2.png)

在左边，“具有布局的链”并未从“最高”的类开始。在右边，“具有布局的链”中有一个“孔”。

类或值类型的布局信息保存在两个表 (*ClassLayout* 和 *FieldLayout*) 中，如下图所示：

 ![ClassLayout 和 FieldLayout](ii.22.8-classlayout-figure-3.png)

在此示例中，_ClassLayout_ 表的第 3 行指向 _TypeDef_ 表的第 2 行 (类的定义，称为“MyClass”)。_FieldLayout_ 表的第 4-6 行指向 _Field_ 表中的相应行。这说明了 CLI 如何存储在“MyClass”中定义的三个字段的显式偏移 (对于拥有类或值类型的每个字段，_FieldLayout_ 表中总是有一行) 因此，_ClassLayout_ 表充当 _TypeDef_ 表中具有布局信息的那些行的扩展；由于许多类没有布局信息，总的来说，这种设计节省了空间。

> _结束信息性文本。_

_ClassLayout_ 表有以下列：

 * _PackingSize_ (2 字节常量) 

 * _ClassSize_ (4 字节常量) 

 * _Parent_ (_TypeDef_ 表的索引) 

通过在此类型声明的类型声明主体上放置 **.pack** 和 **.size** 指令来定义 _ClassLayout_ 表的行 (参见 §[II.10.2](ii.10.2-body-of-a-type-definition.md))。当省略这些指令中的任何一个时，其对应的值为零。 (参见 §[II.10.7](ii.10.7-controlling-instance-layout.md)。) 

_ClassSize_ 为零并不意味着类的大小为零。这意味着在定义时没有指定 **.size** 指令，在这种情况下，实际大小是从字段类型计算出来的，考虑到打包大小 (默认或指定) 和目标运行时平台上的自然对齐。

> _这只包含信息性文本。_

 1. _ClassLayout_ 表可以包含零行或多行

 2. _Parent_ 应索引 _TypeDef_ 表中的有效行，对应于类或值类型 (但不对应于接口) \[错误\]

 3. _Parent_ 索引的类或值类型应为 `SequentialLayout` 或 `ExplicitLayout` (参见 §[II.23.1.15](ii.23.1.15-flags-for-types-typeattributes.md))。 (也就是说，`AutoLayout` 类型不应拥有 _ClassLayout_ 表中的任何行。) \[错误\]

 4. 如果 _Parent_ 索引了一个 `SequentialLayout` 类型，那么：

    * _PackingSize_ 应为 {0, 1, 2, 4, 8, 16, 32, 64, 128} 中的一个。 (0 表示使用应用程序运行的平台的默认打包大小。) \[错误\]

    * 如果 _Parent_ 索引了一个 ValueType，那么 _ClassSize_ 应小于 1 MByte (0x100000 字节)。\[错误\]

 5. 如果 _Parent_ 索引了一个 `ExplicitLayout` 类型，那么

    * 如果 _Parent_ 索引了一个 ValueType，那么 _ClassSize_ 应小于 1 MByte (0x100000 字节) \[错误\]

    * _PackingSize_ 应为 0。 (为每个字段提供显式偏移以及打包大小是没有意义的。) \[错误\]

 6. 注意，如果布局没有创建字段重叠的类型，那么 `ExplicitLayout` 类型可能会产生可验证的类型。

 7. 沿着继承链的长度的布局应遵循上述规则 (从“最高”类型开始，没有“孔”等) \[错误\]

> _结束信息性文本。_

### 20.9. Constant: 0x0B
<a id="Constant_0x0B"></a>

_Constant_ 表用于存储字段、参数和属性的编译时常量值。

_Constant_ 表有以下列：

 * _Type_ (一个1字节常数，后面跟着一个1字节的填充零) ；参见 §[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md)。对于 _ilasm_ 中 _FieldInit_ 的 **nullref** 值 (§[II.16.2](ii.16.2-field-init-metadata.md))，_Type_ 的编码是 `ELEMENT_TYPE_CLASS`，其 _Value_ 是一个4字节的零。与 `ELEMENT_TYPE_CLASS` 在签名中的用法不同，这个不是后跟类型 _token_ 。

 * _Parent_ (一个索引，指向 _Param_、_Field_ 或 _Property_ 表；更准确地说，是一个 _HasConstant_ (§[II.24.2.6](ii.24.2.6-metadata-stream.md)) 编码索引) 

 * _Value_ (一个索引，指向Blob堆) 

请注意，_Constant_ 信息并不直接影响运行时行为，尽管它可以通过反射可见 (因此可以用来实现像 `System.Enum.ToString` 这样的功能)。编译器在导入元数据时，在编译时检查这些信息，但如果使用了常量本身的值，它将嵌入到编译器发出的CIL流中。在运行时，没有CIL指令可以访问 _Constant_ 表。

每当为父项指定编译时值时，都会在 _Constant_ 表中为父项创建一行。 (有关示例，请参见 §[II.16.2.]()) 

> _这只包含信息性文本。_

 1. _Type_ 应该恰好是以下之一：`ELEMENT_TYPE_BOOLEAN`，`ELEMENT_TYPE_CHAR`，`ELEMENT_TYPE_I1`，`ELEMENT_TYPE_U1`，`ELEMENT_TYPE_I2`，`ELEMENT_TYPE_U2`，`ELEMENT_TYPE_I4`，`ELEMENT_TYPE_U4`，`ELEMENT_TYPE_I8`，`ELEMENT_TYPE_U8`，`ELEMENT_TYPE_R4`，`ELEMENT_TYPE_R8`，或 `ELEMENT_TYPE_STRING`；或者 `ELEMENT_TYPE_CLASS`，其 _Value_ 为零 (§II.23.1.16) \[ERROR\]

 2. _Type_ 不应该是任何以下的：`ELEMENT_TYPE_I1`，`ELEMENT_TYPE_U2`，`ELEMENT_TYPE_U4`，或 `ELEMENT_TYPE_U8` (§[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md)) \[CLS\]

 3. _Parent_ 应该索引 _Field_、_Property_ 或 _Param_ 表中的有效行。\[ERROR\]

 4. 基于 _Parent_，不应有重复的行。\[ERROR\]

 5. _Type_ 应该完全匹配由 _Parent_ 标识的 _Param_、_Field_ 或 _Property_ 的声明类型 (在父项是枚举的情况下，它应该完全匹配该枚举的底层类型)。\[CLS\]

> _结束信息性文本。_

### 20.10. CustomAttribute: 0x0C
<a id="CustomAttribute_0x0C"></a>

_CustomAttribute_ 表有以下列：

 * _Parent_ (一个索引，指向一个与 _HasCustomAttribute_ (§[II.24.2.6](ii.24.2.6-metadata-stream.md)) 编码索引关联的元数据表)。

 * _Type_ (一个索引，指向 _MethodDef_ 或 _MemberRef_ 表；更准确地说，是一个 _CustomAttributeType_ (§[II.24.2.6](ii.24.2.6-metadata-stream.md)) 编码索引)。

 * _Value_ (一个指向 Blob 堆的索引)。

_CustomAttribute_ 表存储的数据可以在运行时用来实例化自定义属性 (更准确地说，是指定的自定义属性类的对象)。名为 _Type_ 的列有些误导人——它实际上索引了一个构造方法——该构造方法的所有者是自定义属性的类型。
在 _CustomAttribute_ 表中为父项创建的行由 **.custom** 属性创建，它给出了 _Type_ 列的值，以及可选的 _Value_ 列的值 (§[II.21](ii.21-custom-attributes.md))。 

> _这只包含信息性文本。_

所有二进制值都以小端格式存储 (除了 _PackedLen_ 项，它们仅用作后续 UTF8 字符串中字节数的计数)。

 1. 不需要 _CustomAttribute_；也就是说，_Value_ 可以为空。

 2. _Parent_ 可以是任何元数据表的索引，除了 _CustomAttribute_ 表本身 \[错误\]

 3. _Type_ 应索引 _Method_ 或 _MemberRef_ 表中的有效行。该行应该是一个构造方法 (对于这个信息形成实例的类)  \[错误\]

 4. _Value_ 可以为空或非空。

 5. 如果 _Value_ 是非空的，它应该索引 Blob 堆中的一个 'blob' \[错误\]

 6. 以下规则适用于 _Value_ 'blob' 的整体结构 (§[II.23.3](ii.23.3-custom-attributes.md)) ：

     * _Prolog_ 应该是 0x0001 \[错误\]

     * 应该有与 _Constructor_ 方法中声明的一样多的 _FixedArg_ 出现 \[错误\]

    * _NumNamed_ 可以是零或更多

    * 应该有恰好 _NumNamed_ 个 _NamedArg_ 出现 \[错误\]

    * 每个 _NamedArg_ 应该可以被调用者访问 \[错误\]

    * 如果 _NumNamed_ = 0，那么 _CustomAttrib_ 中不应该有更多的项 \[错误\]

 7. 以下规则适用于 _FixedArg_ 的结构 (§[II.23.3](ii.23.3-custom-attributes.md)) ：

    * 如果此项不是向量 (单维数组，下界为 0)，那么应该有恰好一个 _Elem_ \[错误\]

    * 如果此项是向量，那么：

    * _NumElem_ 应该是 1 或更多 \[错误\]

    * 这应该后跟 _NumElem_ 个 _Elem_ 出现 \[错误\]

 8. 以下规则适用于 _Elem_ 的结构 (§[II.23.3](ii.23.3-custom-attributes.md)) ：

    * 如果这是一个简单类型或枚举 (参见 §[II.23.3](ii.23.3-custom-attributes.md) 了解如何定义)，那么 _Elem_ 只包含它的值 \[错误\]

    * 如果这是一个字符串或类型，那么 _Elem_ 包含一个 _SerString_ —— _PackedLen_ 字节计数，后跟 UTF8 字符 \[错误\]

    * 如果这是一个装箱的简单值类型 (`bool`，`char`，`float32`，`float64`，`int8`，`int16`，`int32`，`int64`，`unsigned int8`，`unsigned int16`，`unsigned int32` 或 `unsigned int64`)，那么 Elem 包含相应的类型表示符 (`ELEMENT_TYPE_BOOLEAN`，`ELEMENT_TYPE_CHAR`，`ELEMENT_TYPE_I1`，`ELEMENT_TYPE_U1`，`ELEMENT_TYPE_I2`，`ELEMENT_TYPE_U2`，`ELEMENT_TYPE_I4`，`ELEMENT_TYPE_U4`，`ELEMENT_TYPE_I8`，`ELEMENT_TYPE_U8`，`ELEMENT_TYPE_R4` 或 `ELEMENT_TYPE_R8`)，后跟它的值。 \[错误\]
9. 以下规则适用于 _NamedArg_ 的结构 (§[II.23.3](ii.23.3-custom-attributes.md)) ：

    * _NamedArg_ 应该以单字节 `FIELD` (0x53) 或 `PROPERTY` (0x54) 开始，用于标识 \[错误\]

    * 如果参数种类是装箱的简单值类型，那么字段或属性的类型是 `ELEMENT_TYPE_BOOLEAN`，`ELEMENT_TYPE_CHAR`，`ELEMENT_TYPE_I1`，`ELEMENT_TYPE_U1`，`ELEMENT_TYPE_I2`，`ELEMENT_TYPE_U2`，`ELEMENT_TYPE_I4`，`ELEMENT_TYPE_U4`，`ELEMENT_TYPE_I8`，`ELEMENT_TYPE_U8`，`ELEMENT_TYPE_R4`，`ELEMENT_TYPE_R8`，`ELEMENT_TYPE_STRING`，或常数 0x50 (对于类型为 `System.Type` 的参数) 中的一个 \[错误\]

    * 字段或属性的名称，分别与前一项，存储为 _SerString_ —— _PackedLen_ 字节计数，后跟名称的 UTF8 字符 \[错误\]

    * _NamedArg_ 是一个 _FixedArg_ (见上文)  \[错误\]

> _结束信息性文本。_

### 20.11. DeclSecurity: 0x0E
<a id="DeclSecurity_0x0E"></a>

可以将源自 `System.Security.Permissions.SecurityAttribute` (参见 [Partition IV]()) 的安全属性附加到 _TypeDef_、_Method_ 或 _Assembly_。此类的所有构造函数都应将 `System.Security.Permissions.SecurityAction` 值作为其第一个参数，描述应对附加到的类型、方法或程序集的权限进行何种操作。源自 `System.Security.Permissions.CodeAccessSecurityAttribute` 的代码访问安全属性可以具有任何安全操作。

这些不同的安全操作在 _DeclSecurity_ 表中被编码为2字节的枚举 (见下文)。对于方法、类型或程序集上给定安全操作的所有安全自定义属性应聚集在一起，并创建一个 `System.Security.PermissionSet` 实例，存储在Blob堆中，并从 _DeclSecurity_ 表中引用。

_[注意：_ 从编译器的角度来看，一般流程如下。用户通过某种特定于语言的语法指定自定义属性，该语法编码了对属性的构造函数的调用。如果属性的类型直接或间接派生自 `System.Security.Permissions.SecurityAttribute`，那么它就是一个安全自定义属性，并需要特殊处理，如下所述 (其他自定义属性通过简单地在元数据中记录构造函数来处理，如 §[II.22.10](ii.22.10-customattribute-0x0c.md) 所述)。构造属性对象，并提供一个方法 (`CreatePermission`) 将其转换为安全权限对象 (从 `System.Security.Permission` 派生的对象)。所有附加到具有相同安全操作的给定元数据项的权限对象都被组合在一起，形成一个 `System.Security.PermissionSet`。使用其 `ToXML` 方法将此权限集转换为准备存储在XML中的形式，以创建 `System.Security.SecurityElement`。最后，使用安全元素上的 `ToString` 方法创建元数据所需的XML。_结束注释]_

_DeclSecurity_ 表有以下列：

 * _Action_ (一个2字节值) 

 * _Parent_ (一个索引，指向 _TypeDef_、_MethodDef_ 或 _Assembly_ 表；更准确地说，是一个 _HasDeclSecurity_ (§[II.24.2.6](ii.24.2.6-metadata-stream.md)) 编码索引) 

 * _PermissionSet_ (一个索引，指向Blob堆) 

_Action_ 是安全操作 (参见 `System.Security.SecurityAction` 在 [Partition IV]()) 的2字节表示。值0&ndash;0xFF保留供未来标准使用。值0x20&ndash;0x7F和0x100&ndash;0x07FF用于操作，如果操作不被理解或支持，可以忽略。值0x80–0xFF和0x0800&ndash;0xFFFF用于操作，操作应为安全操作实施；在操作不可用的实现中，不应允许访问程序集、类型或方法。

| 安全操作          | 注释   | 行为解释                                                                                                   | 有效范围   |
| ----------------- | ------ | ---------------------------------------------------------------------------------------------------------- | ---------- |
| Assert            | 1      | 在没有进一步检查的情况下，满足对指定权限的需求。                                                           | 方法，类型 |
| Demand            | 1      | 检查调用链中的所有调用者是否已被授予指定的权限，在失败时抛出 `SecurityException` (参见 [Partition IV]())。 | 方法，类型 |
| Deny              | 1      | 在没有进一步检查的情况下，拒绝对指定权限的需求。                                                           | 方法，类型 |
| InheritanceDemand | 1      | 为了从类继承或覆盖虚方法，必须授予指定的权限。                                                             | 方法，类型 |
| LinkDemand        | 1      | 检查直接调用者是否已被授予指定的权限；在失败时抛出 `SecurityException` (参见 [Partition IV]())。           | 方法，类型 |
| NonCasDemand      | 2      | 检查当前程序集是否已被授予指定的权限；否则抛出 `SecurityException` (参见 [Partition IV]())。               | 方法，类型 |
| NonCasLinkDemand  | 2      | 检查直接调用者是否已被授予指定的权限；否则抛出 `SecurityException` (参见 [Partition IV]())。               | 方法，类型 |
| PrejitGrant       | &nbsp; | 保留供实现特定使用。                                                                                       | 程序集     |
| PermitOnly        | 1      | 在没有进一步检查的情况下，拒绝对除指定之外的所有权限的需求。                                               | 方法，类型 |
| RequestMinimum    | &nbsp; | 指定运行所需的最小权限。                                                                                   | 程序集     |
| RequestOptional   | &nbsp; | 指定要授予的可选权限。                                                                                     | 程序集     |
| RequestRefuse     | &nbsp; | 指定不授予的权限。                                                                                         | 程序集     |

**注释 1：** 指定的属性应派生自 `System.Security.Permissions.CodeAccessSecurityAttribute`

**注释 2：** 属性应派生自 `System.Security.Permissions.SecurityAttribute`，但不应派生自 `System.Security.Permissions.CodeAccessSecurityAttribute`

_Parent_ 是一个元数据 _token_ ，它标识在 _PermissionSet_ 中编码的安全自定义属性定义的 _Method_，_Type_ 或 _Assembly_。

_PermissionSet_ 是一个 'blob'，其格式如下：

 * 包含一个句点 (.) 的字节。

 * 一个压缩的无符号整数，包含 blob 中编码的属性的数量。

 * 包含以下内容的属性数组：

    * 一个字符串，它是属性的完全限定类型名称。 (字符串被编码为一个压缩的无符号整数，以指示大小，后跟一个 UTF8 字符数组。) 

    * 一组属性，编码为自定义属性的命名参数 (如 §[II.23.3](ii.23.3-custom-attributes.md)，从 _NumNamed_ 开始)。

权限集包含在特定的 _Method_，_Type_ 或 _Assembly_ (参见 _Parent_) 上请求的具有 _Action_ 的权限。换句话说，blob 将包含 _Parent_ 上具有该特定 _Action_ 的所有属性的编码。

_[注意：_ 此标准的第一版指定了权限集的 XML 编码。实现应继续支持此编码以实现向后兼容。_结束注意]_

_DeclSecurity_ 表的行是通过附加一个指定 _Action_ 和 _PermissionSet_ 的 **.permission** 或 **.permissionset** 指令在父程序集 (§[II.6.6](ii.6.6-declarations-inside-a-module-or-assembly.md)) 或父类型或方法 (§[II.10.2](ii.10.2-body-of-a-type-definition.md)) 上填充的。

> _这只包含信息性文本。_

 1. _Action_ 应该只设置那些指定的值 \[错误\]

 2. _Parent_ 应该是 _TypeDef_，_MethodDef_ 或 _Assembly_ 中的一个。也就是说，它应该索引 _TypeDef_ 表，_MethodDef_ 表或 _Assembly_ 表中的有效行。 \[错误\]

 3. 如果 _Parent_ 索引了 _TypeDef_ 表中的一行，那么该行不应定义接口。安全系统会忽略任何这样的父项；编译器不应发出这样的权限集。 \[警告\]

 4. 如果 _Parent_ 索引了一个 _TypeDef_，那么它的 _TypeDef_._Flags_.`HasSecurity` 位应该被设置 \[错误\]

 5. 如果 _Parent_ 索引了一个 _MethodDef_，那么它的 _MethodDef_._Flags_.`HasSecurity` 位应该被设置 \[错误\]

 6. _PermissionSet_ 应该索引 Blob 堆中的一个 'blob' \[错误\]

 7. _PermissionSet_ 索引的 'blob' 的格式应该表示一个有效的，编码的 CLI 对象图。  (所有标准化权限的编码形式在 [Partition IV]() 中指定。)  \[错误\]

> _结束信息性文本。_

### 20.12. EventMap: 0x12
<a id="EventMap_0x12"></a>

_EventMap_ 表有以下列：

 * _Parent_ (一个索引，指向 _TypeDef_ 表) 

 * _EventList_ (一个索引，指向 _Event_ 表)。它标记了由此类型拥有的一连串事件的第一个。该连续运行继续到以下较小者：

    * _Event_ 表的最后一行

    * 通过检查 _EventMap_ 表中下一行的 _EventList_ 找到的下一连串事件

请注意，_EventMap_ 信息并不直接影响运行时行为；重要的是每个事件包含的方法的信息。_EventMap_ 和 _Event_ 表是将 **.event** 指令放在类上的结果 (§[II.18](ii.18-defining-events.md))。

> _这只包含信息性文本。_

 1. _EventMap_ 表可以包含零行或多行

 2. 基于 _Parent_，不应有重复的行 (给定的类只有一个指向其事件列表开始的“指针”) \[ERROR\]

 3. 基于 _EventList_，不应有重复的行 (不同的类不能在 _Event_ 表中共享行) \[ERROR\]

> _结束信息性文本。_

### 20.13. Event: 0x14
<a id="Event_0x14"></a>

事件在元数据中的处理方式与属性非常相似；也就是说，它是一种将定义在给定类上的一组方法关联起来的方式。有两个必需的方法 (`add_` 和 `remove_`) 以及一个可选的方法 (`raise_`) ；还允许其他名称的附加方法 (参见 §[18]())。作为事件聚集在一起的所有方法都应在类上定义 (参见 §[I.8.11.4](i.8.11.4-event-definitions.md)) 

在 _TypeDef_ 表的一行与构成给定事件的方法集合之间的关联关系保存在三个单独的表中 (这与用于属性的方法完全类似)，如下所示：

 ![事件表示例](ii.22.13-event-figure-1.png)

_EventMap_ 表的第 3 行索引了左边 _TypeDef_ 表的第 2 行 (`MyClass`)，同时索引了右边 _Event_ 表的第 4 行 (一个名为 `DocChanged` 的事件的行)。这个设置建立了 `MyClass` 有一个名为 `DocChanged` 的事件。但是 _MethodDef_ 表中的哪些方法被聚集在一起作为事件 `DocChanged` 的'属于'呢？该关联关系包含在 _MethodSemantics_ 表中——它的第 2 行索引了右边的事件 `DocChanged`，以及左边 _MethodDef_ 表的第 2 行 (一个名为 `add_DocChanged` 的方法)。此外，_MethodSemantics_ 表的第 3 行索引了 `DocChanged` 到右边，以及左边 _MethodDef_ 表的第 3 行 (一个名为 `remove_DocChanged` 的方法)。如阴影所示，`MyClass` 有另一个事件，名为 `TimedOut`，有两个方法，`add_TimedOut` 和 `remove_TimedOut`。

_Event_ 表不仅仅是将其他表中的现有行聚集在一起。_Event_ 表有 _EventFlags_，_Name_ (例如，这里的示例中的 `DocChanged` 和 `TimedOut`) 和 _EventType_ 列。此外，_MethodSemantics_ 表有一列用于记录它索引的方法是 `add_`，`remove_`，`raise_` 还是其他函数。

_Event_ 表有以下列：

 * _EventFlags_ (类型为 _EventAttributes_ 的 2 字节位掩码，参见 §[II.23.1.4](ii.23.1.4-flags-for-events-eventattributes.md)) 

 * _Name_ (String 堆的索引) 

 * _EventType_ (_TypeDef_，_TypeRef_ 或 _TypeSpec_ 表的索引；更准确地说，是 _TypeDefOrRef_ (参见 §[II.24.2.6](ii.24.2.6-metadata-stream.md)) 编码索引)  (这对应于事件的类型；它不是拥有此事件的类型。) 

请注意，_Event_ 信息并不直接影响运行时行为；重要的是事件包含的每个方法的信息。_EventMap_ 和 _Event_ 表是将 **.event** 指令放在类上的结果 (参见 §[II.18](ii.18-defining-events.md))。

> _这只包含信息性文本。_

 1. _Event_ 表可以包含零行或多行

 2. 每一行在 _EventMap_ 表中都应有一个且只有一个所有者行 \[错误\]

 3. _EventFlags_ 只应设置指定的值 (所有组合有效)  \[错误\]

 4. _Name_ 应索引 String 堆中的非空字符串 \[错误\]

 5. _Name_ 字符串应为有效的 CLS 标识符 \[CLS\]

 6. _EventType_ 可以为 null 或非 null

 7. 如果 _EventType_ 为非 null，则它应索引 _TypeDef_ 或 _TypeRef_ 表中的有效行 \[错误\]

 8. 如果 _EventType_ 为非 null，则它索引的 _TypeDef_，_TypeRef_ 或 _TypeSpec_ 表中的行应为类 (不是接口或值类型)  \[错误\]

 9. 对于每一行，在 _MethodSemantics_ 表中应有一个 `add_` 和一个 `remove_` 行 \[错误\]

 10. 对于每一行，可以有零个或一个 `raise_` 行，以及 _MethodSemantics_ 表中的零个或多个其他行 \[错误\]

 11. 在 _TypeDef_ 表中的给定行拥有的行中，基于 _Name_ 不应有重复项 \[错误\]

 12. 基于 _Name_ 不应有重复行，其中 _Name_ 字段使用 CLS 冲突标识符规则进行比较 \[CLS\]

### 20.14. ExportedType: 0x27
<a id="ExportedType_0x27"></a>

_ExportedType_ 表为每种类型保存一行：

 1. 在此程序集的其他模块中定义；也就是说，从此程序集中导出。本质上，它存储了此程序集包含的其他模块中所有标记为公共的类型的 _TypeDef_ 行号。
 
    实际的目标行在 _TypeDef_ 表中由 _TypeDefId_ (实际上是行号) 和 _Implementation_ (实际上是持有目标 _TypeDef_ 表的模块) 的组合给出。注意，这是元数据中 *foreign*  _token_ 的唯一出现；也就是说， _token_ 值在另一个模块中有意义。 (常规 _token_ 值是对 *current* 模块中的表的索引) ；或者

 2. 最初在此程序集中定义，但现在已移至另一个程序集。_Flags_ 必须设置 `IsTypeForwarder`，并且 _Implementation_ 是一个 _AssemblyRef_，表示现在可以在其中找到类型。

类型的全名不需要直接存储。相反，它可以在任何包含的 "." 处分成两部分 (尽管通常这是在全名中的最后一个 "." 处完成的)。"." 前面的部分存储为 _TypeNamespace_，"." 后面的部分存储为 _TypeName_。如果全名中没有 "."，那么 _TypeNamespace_ 应该是空字符串的索引。

_ExportedType_ 表有以下列：

 * _Flags_ (一个 4 字节的位掩码，类型为 _TypeAttributes_，§[II.23.1.15](ii.23.1.15-flags-for-types-typeattributes.md)) 

 * _TypeDefId_ (一个 4 字节的索引，指向此程序集的另一个模块中的 _TypeDef_ 表)。此列仅用作提示。如果目标 _TypeDef_ 表中的条目与此表中的 _TypeName_ 和 _TypeNamespace_ 条目匹配，则解析成功。但是，如果有不匹配，CLI 将回退到目标 _TypeDef_ 表的搜索。如果 _Flags_ 设置了 `IsTypeForwarder`，则忽略并应为零。

 * _TypeName_ (一个指向 String 堆的索引) 

 * _TypeNamespace_ (一个指向 String 堆的索引) 

 * _Implementation_。这是一个索引 (更准确地说，是一个 _Implementation_ (§[II.24.2.6](ii.24.2.6-metadata-stream.md)) 编码索引)，指向以下表格中的任何一个：

     * _File_ 表，该条目说明当前程序集中的哪个模块持有 _TypeDef_

     * _ExportedType_ 表，该条目是当前嵌套类型的封闭类型

     * _AssemblyRef_ 表，该条目说明在哪个程序集中现在可以找到类型 (_Flags_ 必须设置 `IsTypeForwarder` 标志)。

_ExportedType_ 表中的行是 **.class extern** 指令的结果 (§[II.6.7](ii.6.7-exported-type-definitions.md))。

> _这只包含信息性文本。_

术语 "_FullName_" 指的是以下方式创建的字符串：如果 _TypeNamespace_ 为空，则使用 _TypeName_，否则使用 _TypeNamespace_，"." 和 _TypeName_ 的连接。

 1. _ExportedType_ 表可以包含零行或多行

 2. _ExportedType_ 表中不应该有在当前模块中定义的类型的条目——只有在程序集中的其他模块中定义的类型 \[错误\]

 3. _Flags_ 只应设置那些指定的值 \[错误\]

 4. 如果 _Implementation_ 索引 _File_ 表，那么 _Flags_.`VisibilityMask` 应该是 `Public` (§[II.23.1.15](ii.23.1.15-flags-for-types-typeattributes.md))  \[错误\]

 5. 如果 _Implementation_ 索引 _ExportedType_ 表，那么 _Flags_.`VisibilityMask` 应该是 `NestedPublic` (§[II.23.1.15](ii.23.1.15-flags-for-types-typeattributes.md))  \[错误\]

 6. 如果非空，_TypeDefId_ 应该索引此程序集中的某个模块 (但不是此模块) 中的 _TypeDef_ 表中的有效行，且所索引的行应该有其 _Flags_.`Public` = 1 (§[II.23.1.15](ii.23.1.15-flags-for-types-typeattributes.md))  \[警告\]

 7. _TypeName_ 应该索引 String 堆中的非空字符串 \[错误\]

 8. _TypeNamespace_ 可以为空，或非空

 9. 如果 _TypeNamespace_ 是非空的，那么它应该索引 String 堆中的非空字符串 \[错误\]

10. _FullName_ 应该是一个有效的 CLS 标识符 \[CLS\]

11. 如果这是一个嵌套类型，那么 _TypeNamespace_ 应该是空的，_TypeName_ 应该表示嵌套类型的未混淆的简单名称 \[错误\]

12. _Implementation_ 应该是一个有效的索引，指向以下任何一个： \[错误\]

     * _File_ 表；该文件应该在其 _TypeDef_ 表中持有目标类型的定义

     * 当前 _ExportedType_ 表中的不同行——这标识了当前嵌套类型的封闭类型

13. _FullName_ 应该与 _TypeDefId_ 索引的 _TypeDef_ 表中的行的相应 _FullName_ 完全匹配 \[错误\]

14. 忽略嵌套类型，基于 _FullName_ 不应该有重复的行 \[错误\]

15. 对于嵌套类型，基于 _TypeName_ 和封闭类型不应该有重复的行 \[错误\]

16. 从当前程序集导出的类型的完整列表是 _ExportedType_ 表与当前 _TypeDef_ 表中所有公共类型的连接，其中 "public" 指的是 _Flags_.`VisibilityMask` 是 `Public` 或 `NestedPublic`。在这个连接表中，基于 _FullName_ (如果这是一个嵌套类型，将封闭类型添加到重复检查中) 不应该有重复的行 \[错误\]

> _结束信息性文本。_

### 20.15. Field: 0x04
<a id="Field_0x04"></a>

_Field_ 表有以下列：

 * _Flags_ (一个2字节的位掩码，类型为 _FieldAttributes_，参见 §[II.23.1.5](ii.23.1.5-flags-for-fields-fieldattributes.md)) 

 * _Name_ (一个索引，指向String堆) 

 * _Signature_ (一个索引，指向Blob堆) 

从概念上讲，_Field_ 表中的每一行都由 _TypeDef_ 表中的一行，且只有一行拥有。然而，_Field_ 表中任何一行的所有者都不存储在 _Field_ 表本身中。在 _TypeDef_ 表的每一行中只有一个“前向指针” (_FieldList_ 列)，如下图所示。

 ![](ii.22.15-field-figure-1.png)

_TypeDef_ 表有1&ndash;4行。_TypeDef_ 表的第一行对应于CLI自动插入的伪类型。它用于表示 _Field_ 表中对应于全局变量的那些行。_Field_ 表有1&mdash;6行。类型1 ('module'的伪类型) 拥有 _Field_ 表中的1和2行。类型2在 _Field_ 表中没有任何行，尽管其 _FieldList_ 索引了 _Field_ 表中的第3行。类型3拥有 _Field_ 表中的3&mdash;5行。类型4拥有 _Field_ 表中的第6行。因此，在 _Field_ 表中，行1和2属于类型1 (全局变量) ；行3&mdash;5属于类型3；行6属于类型4。

_Field_ 表中的每一行都是由顶级 **.field** 指令 (§[II.5.10](ii.5.10-ilasm-source-files.md)) 或类型内的 **.field** 指令 (§[II.10.2](ii.10.2-body-of-a-type-definition.md)) 产生的。 (有关示例，请参见 §[II.14.5](ii.14.5-method-pointers.md)。) 

> _这只包含信息性文本。_

 1. _Field_ 表可以包含零行或多行

 2. 每一行应有一个，且只有一个，在 _TypeDef_ 表中的所有者行 \[ERROR\]

 3. _TypeDef_ 表中的所有者行不应是接口 \[CLS\]

 4. _Flags_ 只应设置那些指定的值 \[ERROR\]

 5. _Flags_ 的 `FieldAccessMask` 子字段应精确地包含 `CompilerControlled`、`Private`、`FamANDAssem`、`Assembly`、`Family`、`FamORAssem` 或 `Public` 中的一个 (§[II.23.1.5](ii.23.1.5-flags-for-fields-fieldattributes.md))  \[ERROR\]

 6. _Flags_ 可以设置 `Literal` 或 `InitOnly` 中的一个或两者都不设置，但不能同时设置两者 (§[II.23.1.5](ii.23.1.5-flags-for-fields-fieldattributes.md))  \[ERROR\]

 7. 如果 _Flags_.`Literal` = 1，那么 _Flags_.`Static` 也应为1 (§[II.23.1.5](ii.23.1.5-flags-for-fields-fieldattributes.md))  \[ERROR\]

 8. 如果 _Flags_.`RTSpecialName` = 1，那么 _Flags_.`SpecialName` 也应为1 (§[II.23.1.5](ii.23.1.5-flags-for-fields-fieldattributes.md))  \[ERROR\]

 9. 如果 _Flags_.`HasFieldMarshal` = 1，那么此行应“拥有” _FieldMarshal_ 表中的恰好一行 (§[II.23.1.5](ii.23.1.5-flags-for-fields-fieldattributes.md))  \[ERROR\]

 10. 如果 _Flags_.`HasDefault` = 1，那么此行应“拥有” _Constant_ 表中的恰好一行 (§[II.23.1.5](ii.23.1.5-flags-for-fields-fieldattributes.md) \[ERROR\]

 11. 如果 _Flags_.`HasFieldRVA` = 1，那么此行应“拥有” _Field's RVA_ 表中的恰好一行 (§[II.23.1.5](ii.23.1.5-flags-for-fields-fieldattributes.md))  \[ERROR\]

 12. _Name_ 应索引String堆中的非空字符串 \[ERROR\]

 13. _Name_ 字符串应是一个有效的CLS标识符 \[CLS\]

 14. _Signature_ 应索引Blob堆中的有效字段签名 \[ERROR\]

 15. 如果 _Flags_.`CompilerControlled` = 1 (§[II.23.1.5](ii.23.1.5-flags-for-fields-fieldattributes.md))，那么在重复检查中完全忽略此行。

 16. 如果此字段的所有者是内部生成的类型 `<Module>`，它表示此字段在模块范围内定义 (通常称为全局变量)。在这种情况下：

     * _Flags_.`Static` 应为1 \[ERROR\] 

     * _Flags_.`MemberAccessMask` 子字段应为 `Public`、`CompilerControlled` 或 `Private` 中的一个 (§[II.23.1.5](ii.23.1.5-flags-for-fields-fieldattributes.md))  \[ERROR\]

     * 不允许模块范围字段 \[CLS\]

 17. 基于 _owner_+_Name_+_Signature_，_Field_ 表中不应有重复的行 (其中 _owner_ 是在 _TypeDef_ 表中的拥有行，如上所述)  (但请注意，如果 _Flags_.`CompilerControlled` = 1，那么完全排除此行的重复检查)  \[ERROR\]

 18. 基于 _owner_+_Name_，_Field_ 表中不应有重复的行，其中 _Name_ 字段使用CLS冲突标识符规则进行比较。所以，例如，"`int i`" 和 "`float i`" 将被视为CLS重复。  (但请注意，如果 _Flags_.`CompilerControlled` = 1，那么如上所述，此行完全排除在重复检查之外)  \[CLS\]

 19. 如果这是一个枚举的字段，那么： 
 
     * _TypeDef_ 表中的所有者行应直接派生自 `System.Enum` \[ERROR\]

     * _TypeDef_ 表中的所有者行不应有其他实例字段 \[CLS\]

     * 其 _Signature_ 应为 `ELEMENT_TYPE_U1`、`ELEMENT_TYPE_I2`、`ELEMENT_TYPE_I4` 或 `ELEMENT_TYPE_I8` 中的一个 (§[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md))  \[CLS\]

 20. 其 _Signature_ 应为整型。 \[ERROR\]

> _结束信息性文本。_


### 20.16. FieldLayout: 0x10
<a id="FieldLayout_0x10"></a>

_FieldLayout_ 表有以下列：

 * _Offset_ (4 字节常量) 

 * _Field_ (_Field_ 表的索引) 

请注意，任何类型中的每个字段都由其签名定义。当 CLI 布局类型实例 (即，对象) 时，每个字段是四种类型之一：

 * **Scalar：** 对于任何内置类型的成员，例如 `int32`。字段的大小由该内在类型的大小给出，其大小在 1 到 8 字节之间变化

 * **ObjectRef：** 对于 `ELEMENT_TYPE_CLASS`，`ELEMENT_TYPE_STRING`，`ELEMENT_TYPE_OBJECT`，`ELEMENT_TYPE_ARRAY`，`ELEMENT_TYPE_SZARRAY`

 * **Pointer：** 对于 `ELEMENT_TYPE_PTR`，`ELEMENT_TYPE_FNPTR`

 * **ValueType：** 对于 `ELEMENT_TYPE_VALUETYPE`。该 ValueType 的实例实际上是在此对象中布局的，因此字段的大小是该 ValueType 的大小

请注意，指定显式结构布局的元数据可以在一个平台上有效地使用，但在另一个平台上可能无效，因为这里指定的一些规则取决于特定于平台的对齐规则。

如果父字段的 **.field** 指令已指定字段偏移，则将创建 _FieldLayout_ 表中的一行 (参见 §[II.16](ii.16-defining-and-referencing-fields.md))。

> _这只包含信息性文本。_

 1. _FieldLayout_ 表可以包含零行或多行

 2. _FieldLayout_ 表中每行描述的字段的类型应设置 _Flags_.`ExplicitLayout` (参见 §[II.23.1.15](ii.23.1.15-flags-for-types-typeattributes.md))  \[错误\]

 3. _Offset_ 应为零或更多 \[错误\]

 4. _Field_ 应索引 _Field_ 表中的有效行 \[错误\]

 5. _Field_ 索引的 _Field_ 表中的行的 _Flags_.`Static` 应为非静态 (即，零 0)  \[错误\]

 6. 在给定类型拥有的行中，基于 _Field_ 不应有重复项。也就是说，类型的给定 _Field_ 不能被赋予两个偏移。 \[错误\]

 7. 类型 **ObjectRef** 的每个字段应在类型内自然对齐 \[错误\]

 8. 在给定类型拥有的行中，完全有效的是有几行具有相同的 _Offset_ 值。**ObjectRef** 和值类型不能具有相同的偏移 \[错误\]

 9. `ExplicitLayout` 类型的每个字段都应给出偏移；也就是说，它应在 _FieldLayout_ 表中有一行 \[错误\]

> _结束信息性文本。_

### 20.17. FieldMarshal : 0x0D
<a id="FieldMarshal_0x0D"></a>

_FieldMarshal_ 表有两列。它将 _Field_ 或 _Param_ 表中的现有行“链接”到 Blob 堆中的信息，该信息定义了该字段或参数 (通常情况下，作为参数编号 0 的方法返回) 在通过 PInvoke 调度调用到或从非托管代码时应如何进行封送。

请注意，_FieldMarshal_ 信息仅由与非托管代码进行操作的代码路径使用。为了执行这样的路径，调用者在大多数平台上将安装具有提升的安全权限。一旦它调用非托管代码，它就脱离了 CLI 可以检查的范围——它只是被信任不会违反类型系统。

_FieldMarshal_ 表有以下列：

 * _Parent_ (一个索引，指向 _Field_ 或 _Param_ 表；更准确地说，是一个 _HasFieldMarshal_ (§[II.24.2.6](ii.24.2.6-metadata-stream.md)) 编码索引) 

 * _NativeType_ (一个指向 Blob 堆的索引) 

有关 'blob' 的详细格式，请参见 §[II.23.4](ii.23.4-marshalling-descriptors.md)

如果父字段的 **.field** 指令指定了 **marshal** 属性 (§[II.16.1](ii.16.1-attributes-of-fields.md))，则会在 _FieldMarshal_ 表中创建一行。

> _这只包含信息性文本。_

 1. _FieldMarshal_ 表可以包含零行或多行

 2. _Parent_ 应该索引 _Field_ 或 _Param_ 表中的有效行 (_Parent_ 值被编码以表示每个引用的是这两个表中的哪一个)  \[错误\]

 3. _NativeType_ 应该索引 Blob 堆中的非空 'blob' \[错误\]

 4. 没有两行应指向同一个父项。换句话说，在 _Parent_ 值已被解码以确定它们是引用 _Field_ 表还是 _Param_ 表之后，没有两行可以指向 _Field_ 表或 _Param_ 表中的同一行 \[错误\]

 5. 以下检查适用于 _MarshalSpec_ 'blob' (§[II.23.4](ii.23.4-marshalling-descriptors.md)) ：

     * _NativeIntrinsic_ 应该是其生产中的常数值之一 (§[II.23.4](ii.23.4-marshalling-descriptors.md))  \[错误\]

     * 如果是 `ARRAY`，那么 ArrayElemType 应该是其生产中的常数值之一 \[错误\]

     * 如果是 `ARRAY`，那么 _ParamNum_ 可以为零

     * 如果是 `ARRAY`，那么 _ParamNum_ 不能小于 0 \[错误\]

     * 如果是 `ARRAY`，并且 _ParamNum_ > 0，那么 _Parent_ 应该指向 _Param_ 表中的一行，而不是 _Field_ 表 \[错误\]

     * 如果是 `ARRAY`，并且 _ParamNum_ > 0，那么 _ParamNum_ 不能超过父 _Param_ 是其成员的 _MethodDef_ (或者如果是 `VARARG` 调用，则为 _MethodRef_) 提供的参数数量 \[错误\]

     * 如果是 `ARRAY`，那么 _ElemMult_ 应该大于等于 1 \[错误\]

     * 如果是 `ARRAY` 并且 _ElemMult_ 不等于 1，则发出警告，因为这可能是一个错误 \[警告\]

     * 如果是 `ARRAY` 并且 _ParamNum_ = 0，那么 _NumElem_ 应该大于等于 1 \[错误\]

     * 如果是 `ARRAY` 并且 _ParamNum_ 不等于 0 并且 _NumElem_ 不等于 0，则发出警告，因为这可能是一个错误 \[警告\]

> _结束信息性文本。_

### 20.18. FieldRVA: 0x1D
<a id="FieldRVA_0x1D"></a>

_FieldRVA_ 表有以下列：

 * _RVA_ (4 字节常量) 

 * _Field_ (_Field_ 表的索引) 

从概念上讲，_FieldRVA_ 表中的每一行都是 _Field_ 表中的确切一行的扩展，并记录了此字段的初始值存储在图像文件中的 RVA (相对虚拟地址)。

对于每个指定了可选的 **data** 标签的静态父字段，都会创建 _FieldRVA_ 表中的一行 (参见 §[II.16](ii.16-defining-and-referencing-fields.md))。RVA 列是 PE 文件中数据的相对虚拟地址 (参见 §[II.16.3](ii.16.3-embedding-data-in-a-pe-file.md))。

> _这只包含信息性文本。_

 1. _RVA_ 应为非零 \[错误\]

 2. _RVA_ 应指向当前模块的数据区域 (而不是其元数据区域)  \[错误\]

 3. _Field_ 应索引 _Field_ 表中的有效行 \[错误\]

 4. 任何具有 RVA 的字段应为 ValueType (而不是类或接口)。此外，它不应有任何私有字段 (同样适用于其自身为 ValueType 的任何字段)。 (如果违反了这些条件，代码可以覆盖该全局静态并访问其私有字段。) 此外，该 ValueType 的任何字段都不能是对象引用 (进入 GC 堆)  \[错误\]

 5. 只要两个基于 RVA 的字段符合前面的条件，两个 ValueType 跨越的内存范围就可以重叠，没有进一步的约束。这实际上不是一个额外的规则；它只是澄清了关于重叠的基于 RVA 的字段的位置

> _结束信息性文本。_

### 20.19. File: 0x26
<a id="File_0x26"></a>

_File_ 表有以下列：

 * _Flags_ (一个 4 字节的位掩码，类型为 _FileAttributes_，§[II.23.1.6](ii.23.1.6-flags-for-files-fileattributes.md)) 

 * _Name_ (一个指向 String 堆的索引) 

 * _HashValue_ (一个指向 Blob 堆的索引) 

_File_ 表的行是程序集中的 **.file** 指令的结果 (§[II.6.2.3](ii.6.2.3-associating-files-with-an-assembly.md)) 

> _这只包含信息性文本。_

 1. _Flags_ 只应设置那些指定的值 (所有组合有效)  \[错误\]

 2. _Name_ 应该索引 String 堆中的非空字符串。它应该是 `<filename>.<extension>` 的格式 (例如，"`foo.dll`"，但*不是* "`c:\utils\foo.dll`")  \[错误\]

 3. _HashValue_ 应该索引 Blob 堆中的非空 'blob' \[错误\]

 4. 不应该有重复的行；也就是说，具有相同 _Name_ 值的行 \[错误\]

 5. 如果此模块包含 _Assembly_ 表中的一行 (也就是说，如果此模块“持有清单”)，那么 _File_ 表中不应该有任何关于此模块的行；也就是说，没有自引用 \[错误\]

 6. 如果 _File_ 表为空，那么按定义，这是一个单文件程序集。在这种情况下，_ExportedType_ 表应该为空 \[警告\]

> _结束信息性文本。_

### 20.20. GenericParam: 0x2A
<a id="GenericParam_0x2A"></a>

_GenericParam_ 表有以下列：

 * _Number_ (泛型参数的2字节索引，从左到右编号，从零开始) 

 * _Flags_ (一个2字节的位掩码，类型为 _GenericParamAttributes_，参见 §[II.23.1.7](ii.23.1.7-flags-for-generic-parameters-genericparamattributes.md)) 

 * _Owner_ (一个索引，指向 _TypeDef_ 或 _MethodDef_ 表，指定此泛型参数适用的类型或方法；更准确地说，是一个 _TypeOrMethodDef_ (§[II.24.2.6](ii.24.2.6-metadata-stream.md)) 编码索引) 

 * _Name_ (一个非空索引，指向String堆，给出泛型参数的名称。这纯粹是描述性的，只由源语言编译器和反射使用) 

以下是其他的限制：

 * _Owner_ 不能是非嵌套的枚举类型；并且

 * 如果 _Owner_ 是嵌套的枚举类型，那么 _Number_ 必须小于或等于封闭类的泛型参数的数量。

_[原因：_ 泛型枚举类型的作用很小，通常只存在于满足CLS规则42。这些额外的限制限制了枚举类型的通用性，同时允许满足CLS规则42。_GenericParam_ 表存储了在泛型类型定义和泛型方法定义中使用的泛型参数。这些泛型参数可以被约束 (即，泛型参数应扩展某个类和/或实现某些接口) 或无约束。 (这样的约束存储在 _GenericParamConstraint_ 表中。) 

从概念上讲，_GenericParam_ 表中的每一行都由 _TypeDef_ 或 _MethodDef_ 表中的一行，且只有一行拥有。

_[示例：_

 ```il
 .class Dict`2<([mscorlib]System.IComparable) K, V>
 ```

类 `Dict` 的泛型参数 `K` 被约束为实现 `System.IComparable`。

 ```il
 .method static void ReverseArray<T>(!!0[] 'array')
 ```

泛型方法 `ReverseArray` 的泛型参数 `T` 没有约束。 

 1. _GenericParam_ 表可以包含零行或多行

 2. 每一行应有一个，且只有一个，在 _TypeDef_ 或 _MethodDef_ 表中的所有者行 (即，没有行共享) \[ERROR\]

 3. 每个泛型类型应在 _GenericParam_ 表中为其每个泛型参数拥有一行 \[ERROR\]

 4. 每个泛型方法应在 _GenericParam_ 表中为其每个泛型参数拥有一行 \[ERROR\]

_Flags_：

 5. 可以持有 `Covariant` 或 `Contravariant` 的值，但只有当所有者行对应于泛型接口或泛型委托类时才能这样做。 \[ERROR\]

 6. 否则，应持有 `None` 值，表示非变量 (即，参数是非变量或所有者是非委托类、值类型或泛型方法)  \[ERROR\]

 7. 如果 _Flags_ == `Covariant`，那么相应的泛型参数只能作为以下内容出现在类型定义中： \[ERROR\]

     * 方法的结果类型

     * 继承接口的泛型参数

 8. 如果 _Flags_ == `Contravariant`，那么相应的泛型参数只能作为方法的参数出现在类型定义中 \[ERROR\]

 9. _Number_ 应有一个值 &ge; 0 且 < 所有者类型或方法的泛型参数的数量。 \[ERROR\]

 10. 同一方法拥有的 _GenericParam_ 表中的连续行应按照 _Number_ 值的增加顺序排序；_Number_ 序列中不应有间隙 \[ERROR\]

 11. _Name_ 应为非空并索引String堆中的字符串  \[ERROR\]

     _[原因：_ 否则，反射输出不完全可用。_结束原因]_

12. 基于 _Owner_+_Name_，不应有重复的行  \[ERROR\]

     _[原因：_ 否则，使用反射的代码无法消除不同的泛型参数。_结束原因]_

 13. 基于 _Owner_+_Number_，不应有重复的行 \[ERROR\]

### 20.21. GenericParamConstraint: 0x2C
<a id="GenericParamConstraint_0x2C"></a>

_GenericParamConstraint_ 表有以下列：

 * _Owner_ (_GenericParam_ 表的索引，指定此行引用的泛型参数) 

 * _Constraint_ (_TypeDef_，_TypeRef_ 或 _TypeSpec_ 表的索引，指定此泛型参数受限于从哪个类派生；或此泛型参数受限于实现哪个接口；更准确地说，是 _TypeDefOrRef_ (参见 §[II.24.2.6](ii.24.2.6-metadata-stream.md)) 编码索引) 

_GenericParamConstraint_ 表记录每个泛型参数的约束。每个泛型参数可以约束为从零个或一个类派生。每个泛型参数可以约束为实现零个或多个接口。

从概念上讲，_GenericParamConstraint_ 表中的每一行都由 _GenericParam_ 表中的一行“拥有”。给定 _Owner_ 的 _GenericParamConstraint_ 表中的所有行应引用不同的约束。

请注意，如果 _Constraint_ 是对 `System.ValueType` 的 _TypeRef_，那么它意味着约束类型应为 `System.ValueType`，或其子类型之一。然而，由于 `System.ValueType` 本身是引用类型，这种特定机制并不能保证类型是非引用类型。

> _这只包含信息性文本。_

 1. _GenericParamConstraint_ 表可以包含零行或多行

 2. 每一行在 _GenericParam_ 表中都应有一个且只有一个所有者行 (即，没有行共享)   \[错误\]

 3. _GenericParam_ 表中的每一行应“拥有” _GenericParamConstraint_ 表中的一个单独行，对应于该泛型参数具有的每个约束 \[错误\]

 4. 在 _GenericParam_ 表中的给定行拥有的 _GenericParamConstraint_ 表中的所有行应形成一个连续的范围 (行)  \[错误\]

 5. 任何泛型参数 (对应于 _GenericParam_ 表中的一行) 应拥有 _GenericParamConstraint_ 表中的零行或一行，对应于类约束。 \[错误\]

6. 任何泛型参数 (对应于 _GenericParam_ 表中的一行) 应拥有 _GenericParamConstraint_ 表中的零行或多行，对应于接口约束。 \[错误\]

7. 基于 _Owner_+_Constraint_ 不应有重复行 \[错误\]

8. 约束不应引用 `System.Void`。 \[错误\]

> _结束信息性文本。_

### 20.22. ImplMap: 0x1C
<a id="ImplMap_0x1C"></a>

_ImplMap_ 表保存了关于可以从托管代码通过 PInvoke 调度访问的非托管方法的信息。_ImplMap_ 表的每一行将 _MethodDef_ 表中的一行 (_MemberForwarded_) 与某个非托管 DLL (_ImportScope_) 中的例程 (_ImportName_) 的名称关联起来。

_[注意：_ 典型的例子是：将存储在 _Method_ 表的第 N 行的托管方法 (所以 _MemberForwarded_ 将有值 N) 与 DLL "`kernel32`" 中名为 "`GetEnvironmentVariable`" 的例程 (由 _ImportName_ 索引的字符串) 关联起来 (_ImportScope_ 索引的 _ModuleRef_ 表中的字符串)。CLI 拦截对托管方法编号 N 的调用，并将它们转发为对 "`kernel32.dll`" 中名为 "`GetEnvironmentVariable`" 的非托管例程的调用 (包括根据需要封送任何参数) 

CLI 不支持此机制来访问从 DLL 导出的字段，只支持方法。_结束注释]_

_ImplMap_ 表有以下列：

 * _MappingFlags_ (一个 2 字节的位掩码，类型为 _PInvokeAttributes_，§[23.1.8]()) 

 * _MemberForwarded_ (一个索引，指向 _Field_ 或 _MethodDef_ 表；更准确地说，是一个 _MemberForwarded_ (§[II.24.2.6](ii.24.2.6-metadata-stream.md)) 编码索引)。然而，它只会索引 _MethodDef_ 表，因为不支持 _Field_ 导出。

 * _ImportName_ (一个指向 String 堆的索引) 

 * _ImportScope_ (一个指向 _ModuleRef_ 表的索引) 

对于每个定义了一个指定 _MappingFlags_、_ImportName_ 和 _ImportScope_ 的 **.pinvokeimpl** 互操作属性的父方法 (§[II.15.5](ii.15.5-unmanaged-methods.md))，在 _ImplMap_ 表中都会输入一行。

> _这只包含信息性文本。_

 1. _ImplMap_ 可以包含零行或多行

 2. _MappingFlags_ 只应设置那些指定的值 \[错误\]

 3. _MemberForwarded_ 应该索引 _MethodDef_ 表中的有效行 \[错误\]

 4. 在 _MethodDef_ 表中由 _MemberForwarded_ 索引的行中的 _MappingFlags_.`CharSetMask` (§[II.23.1.7](ii.23.1.7-flags-for-generic-parameters-genericparamattributes.md)) 应该最多设置以下位之一： 
`CharSetAnsi`、`CharSetUnicode` 或 `CharSetAuto` (如果没有设置，默认为 `CharSetNotSpec`)  \[错误\]

 5. _ImportName_ 应该索引 String 堆中的非空字符串 \[错误\]

 6. _ImportScope_ 应该索引 _ModuleRef_ 表中的有效行 \[错误\]

 7. 由 _MemberForwarded_ 在 _MethodDef_ 表中索引的行应该有其 _Flags_.`PinvokeImpl` = 1，并且 _Flags_.`Static` = 1 \[错误\]

> _结束信息性文本。_

### 20.23. InterfaceImpl: 0x09
<a id="InterfaceImpl_0x09"></a>

_InterfaceImpl_ 表有以下列：

 * _Class_ (_TypeDef_ 表的索引) 

 * _Interface_ (_TypeDef_，_TypeRef_ 或 _TypeSpec_ 表的索引；更准确地说，是 _TypeDefOrRef_ (参见 §[II.24.2.6](ii.24.2.6-metadata-stream.md)) 编码索引) 
 
_InterfaceImpl_ 表记录类型显式实现的接口。从概念上讲，_InterfaceImpl_ 表中的每一行都表示 _Class_ 实现了 _Interface_。

> _这只包含信息性文本。_

 1. _InterfaceImpl_ 表可以包含零行或多行

 2. _Class_ 应为非 null \[错误\]

 3. 如果 _Class_ 为非 null，则：

     1. _Class_ 应索引 _TypeDef_ 表中的有效行 \[错误\]

     2. _Interface_ 应索引 _TypeDef_ 或 _TypeRef_ 表中的有效行 \[错误\]

     3. _Interface_ 索引的 _TypeDef_，_TypeRef_ 或 _TypeSpec_ 表中的行应为接口 (_Flags_.`Interface` = 1)，而不是类或值类型 \[错误\]

 4. 基于非 null 的 _Class_ 和 _Interface_ 值，在 _InterfaceImpl_ 表中不应有重复项 \[警告\]

 5. 可以有许多行具有相同的 _Class_ 值 (因为一个类可以实现许多接口) 

 6. 可以有许多行具有相同的 _Interface_ 值 (因为许多类可以实现相同的接口) 

> _结束信息性文本。_

### 20.24. ManifestResource: 0x28
<a id="ManifestResource_0x28"></a>

_ManifestResource_ 表有以下列：

 * _Offset_ (一个 4 字节的常数) 

 * _Flags_ (一个 4 字节的位掩码，类型为 _ManifestResourceAttributes_，§[II.23.1.9](ii.23.1.9-flags-for-manifestresource-manifestresourceattributes.md)) 

 * _Name_ (一个指向 String 堆的索引) 

 * _Implementation_ (一个指向 _File_ 表、_AssemblyRef_ 表或 null 的索引；更准确地说，是一个 _Implementation_ (§[II.24.2.6](ii.24.2.6-metadata-stream.md)) 编码索引) 

_Offset_ 指定此资源记录开始的引用文件内的字节偏移量。_Implementation_ 指定哪个文件持有此资源。

表中的行是程序集上的 **.mresource** 指令的结果 (§[II.6.2.2](ii.6.2.2-manifest-resources.md))。

> _这只包含信息性文本。_

 1. _ManifestResource_ 表可以包含零行或多行

 2. _Offset_ 应该是目标文件中的有效偏移量，从 CLI 头部的资源条目开始 \[错误\]

 3. _Flags_ 只应设置那些指定的值 \[错误\]

 4. _Flags_ 的 `VisibilityMask` 子字段 (§[II.23.1.9](ii.23.1.9-flags-for-manifestresource-manifestresourceattributes.md)) 应该是 `Public` 或 `Private` 中的一个 \[错误\]

 5. _Name_ 应该索引 String 堆中的非空字符串 \[错误\]

 6. _Implementation_ 可以为空或非空 (如果为空，表示资源存储在当前文件中) 

 7. 如果 _Implementation_ 为空，那么 _Offset_ 应该是当前文件中的有效偏移量，从 CLI 头部的资源条目开始 \[错误\]

 8. 如果 _Implementation_ 非空，那么它应该索引 _File_ 或 _AssemblyRef_ 表中的有效行 \[错误\]

 9. 基于 _Name_ 不应该有重复的行 \[错误\]

 10. 如果资源是 _File_ 表中的索引，_Offset_ 应该为零 \[错误\]

> _结束信息性文本。_

### 20.25. MemberRef: 0x0A
<a id="MemberRef_0x0A"></a>

_MemberRef_ 表将对类的方法和字段的两种引用合并在一起，分别称为 'MethodRef' 和 'FieldRef'。

_MemberRef_ 表有以下列：

 * _Class_ (_MethodDef_，_ModuleRef_，_TypeDef_，_TypeRef_ 或 _TypeSpec_ 表的索引；更准确地说，是 _MemberRefParent_ (参见 §[II.24.2.6](ii.24.2.6-metadata-stream.md)) 编码索引) 

 * _Name_ (String 堆的索引) 

 * _Signature_ (Blob 堆的索引) 

每当在 CIL 代码中对在另一个模块或程序集中定义的方法或字段进行引用时，都会在 _MemberRef_ 表中创建一个条目。 (此外，即使在与调用点相同的模块中定义了具有 `VARARG` 签名的方法，也会为其创建一个条目。) 

> _这只包含信息性文本。_

 1. _Class_ 应为以下之一：\[错误\]

     1. 如果定义成员的类在另一个模块中定义，则为 _TypeRef_  _token_ 。 (请注意，当成员在此相同的模块中定义时，使用 _TypeRef_  _token_ 是不寻常的，但有效的，在这种情况下，可以使用其 _TypeDef_  _token_ 代替。) 

     2. 如果成员在同一程序集的另一个模块中定义为全局函数或变量，则为 _ModuleRef_  _token_ 。

     3. 当用于为在此模块中定义的 vararg 方法提供调用点签名时，为 _MethodDef_  _token_ 。_Name_ 应与相应 _MethodDef_ 行中的 _Name_ 匹配。_Signature_ 应与目标方法定义中的 _Signature_ 匹配 \[错误\]

     4. 如果成员是泛型类型的成员，则为 _TypeSpec_  _token_ 

 2. _Class_ 不应为 null (因为这将表示对全局函数或变量的未解析引用)  \[错误\]

 3. _Name_ 应索引 String 堆中的非空字符串 \[错误\]

 4. _Name_ 字符串应为有效的 CLS 标识符 \[CLS\]

 5. _Signature_ 应索引 Blob 堆中的有效字段或方法签名。特别是，它应嵌入以下 '调用约定' 中的一个：\[错误\]
 
      1. `DEFAULT` (0x0)
      2. `VARARG` (0x5)
      3. `FIELD` (0x6)
      4. `GENERIC` (0x10)

 6. _MemberRef_ 表应不包含重复项，其中重复行具有相同的 _Class_，_Name_ 和 _Signature_ \[警告\]

 7. _Signature_ 不应具有 `VARARG` (0x5) 调用约定 \[CLS\]

 8. 不应有重复行，其中 _Name_ 字段使用 CLS 冲突标识符规则进行比较。 (特别是注意，CLS 中忽略了返回类型以及参数是否标记为 `ELEMENT_TYPE_BYREF` (参见 §[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md))。例如，`.method int32 M()`和 `.method float64 M()` 根据 CLS 规则产生重复行。同样，`.method void N(int32 i)` 和 `.method void N(int32& i)` 也根据 CLS 规则产生重复行。)  \[CLS\]

 9. 如果 _Class_ 和 _Name_ 解析为字段，则该字段的 _Flags_.`FieldAccessMask` 子字段中不应有 `CompilerControlled` (参见 §[II.23.1.5](ii.23.1.5-flags-for-fields-fieldattributes.md)) 的值 \[错误\]

 10. 如果 _Class_ 和 _Name_ 解析为方法，则该方法的 _Flags_.`MemberAccessMask` (参见 §[II.23.1.10](ii.23.1.10-flags-for-methods-methodattributes.md)) 子字段中不应有 `CompilerControlled` 的值 \[错误\]

 11. 包含 _MemberRef_ 定义的类型应为表示实例化类型的 _TypeSpec_。

> _结束信息性文本。_

### 20.26. MethodDef: 0x06
<a id="MethodDef_0x06"></a>

_MethodDef_ 表有以下列：

 * _RVA_ (一个 4 字节的常数) 

 * _ImplFlags_ (一个 2 字节的位掩码，类型为 _MethodImplAttributes_，§[II.23.1.11](ii.23.1.11-flags-for-methods-methodimplattributes.md)) 

 * _Flags_ (一个 2 字节的位掩码，类型为 _MethodAttributes_，§[II.23.1.10](ii.23.1.10-flags-for-methods-methodattributes.md)) 

 * _Name_ (一个指向 String 堆的索引) 

 * _Signature_ (一个指向 Blob 堆的索引) 

 * _ParamList_ (一个指向 Param 表的索引)。它标记了由此方法拥有的一连串参数的第一个。该连续运行继续到以下较小的：

     * _Param_ 表的最后一行

     * 下一个参数运行，通过检查 _MethodDef_ 表中下一行的 _ParamList_ 找到

从概念上讲，_MethodDef_ 表中的每一行都由 _TypeDef_ 表中的一行，且只有一行，拥有。

_MethodDef_ 表中的行是 **.method** 指令的结果 (§[II.15](ii.15-defining-referencing-and-calling-methods.md))。当发出 PE 文件的映像时，计算 RVA 列，并指向方法体的 `COR_ILMETHOD` 结构 (§[II.25.4](ii.25.4-common-intermediate-language-physical-layout.md)) 

_[注意：_ 如果 _Signature_ 是 `GENERIC` (0x10)，则在 _GenericParam_ 表 (§[II.22.20](ii.22.20-genericparam-0x2a.md)) 中描述泛型参数。_结束注释]_

> _这只包含信息性文本。_

 1. _MethodDef_ 表可以包含零行或多行

 2. 每一行应该有一个，且只有一个，在 _TypeDef_ 表中的所有者行 \[错误\]

 3. _ImplFlags_ 只应设置那些指定的值 \[错误\]

 4. _Flags_ 只应设置那些指定的值 \[错误\]

 5. 如果 _Name_ 是 `.ctor` 并且方法被标记为 `SpecialName`，那么在 _GenericParam_ 表中不应该有一行将此 _MethodDef_ 作为其所有者。 \[错误\]

 6. _Flags_ 的 `MemberAccessMask` 子字段 (§[II.23.1.10](ii.23.1.10-flags-for-methods-methodattributes.md)) 应该包含 `CompilerControlled`、`Private`、`FamANDAssem`、`Assem`、`Family`、`FamORAssem` 或 `Public` 中的一个 \[错误\]

 7. _Flags_ 中的以下组合位设置是无效的 \[错误\]

     1. `Static` | `Final`

     2. `Static` | `Virtual`

     3. `Static` | `NewSlot`

     4. `Final` | `Abstract`

     5. `Abstract` | `PinvokeImpl`

     6. `CompilerControlled` | `SpecialName`

     7. `CompilerControlled` | `RTSpecialName`

 8. 抽象方法应该是虚方法。所以，如果 _Flags_.`Abstract` = 1 那么 _Flags_.`Virtual` 也应该是 1 \[错误\]

 9. 如果 _Flags_.`RTSpecialName` = 1 那么 _Flags_.`SpecialName` 也应该是 1 \[错误\]

 10. 如果 _Flags_.`HasSecurity` = 1，那么以下条件中至少有一个应该为真： \[错误\]

     * 此方法拥有 _DeclSecurity_ 表中的至少一行

     * 此方法具有名为 `SuppressUnmanagedCodeSecurityAttribute` 的自定义属性

 11. 如果此方法拥有 _DeclSecurity_ 表中的一行 (或多行) 那么 _Flags_.`HasSecurity` 应该是 1 \[错误\]

 12. 如果此方法具有名为 `SuppressUnmanagedCodeSecurityAttribute` 的自定义属性那么 _Flags_.`HasSecurity` 应该是 1 \[错误\]

 13. 方法可以具有名为 `DynamicSecurityMethodAttribute` 的自定义属性，但这对其 _Flags_.`HasSecurity` 的值没有任何影响

14. _Name_ 应索引String堆中的非空字符串 \[ERROR\]

 15. 接口不能有实例构造函数。所以，如果这个方法是由接口拥有的，那么它的 _Name_ 不能是 `.ctor` \[ERROR\]

 16. _Name_ 字符串应是一个有效的CLS标识符 (除非设置了 _Flags_.`RTSpecialName` - 例如，`.cctor` 是有效的)  \[CLS\]

 17. _Signature_ 应索引Blob堆中的有效方法签名 \[ERROR\]

 18. 如果 _Flags_.`CompilerControlled` = 1，那么在重复检查中完全忽略此行

 19. 如果此方法的所有者是内部生成的类型 `<Module>`，它表示此方法在模块范围内定义。 _[注：_ 在C++中，该方法被称为全局方法，只能在其编译单元内从其声明点向前引用。 _结束注释]_ 在这种情况下：

     1. _Flags_.`Static` 应为1 \[ERROR\]

     2. _Flags_.`Abstract` 应为0 \[ERROR\]

     3. _Flags_.`Virtual` 应为0 \[ERROR\]

     4. _Flags_.`MemberAccessMask` 子字段应为 `CompilerControlled`、`Public` 或 `Private` 中的一个 \[ERROR\]

     5. 不允许模块范围方法 \[CLS\]

 20. 对于没有身份的值类型，具有同步方法是没有意义的 (除非它们被装箱)。所以，如果此方法的所有者是一个值类型，那么该方法不能被同步。也就是说，_ImplFlags_.`Synchronized` 应为0 \[ERROR\]

 21. 在 _MethodDef_ 表中，基于所有者 + _Name_ + _Signature_，不应有重复的行 (其中所有者是在 _TypeDef_ 表中的拥有行)。 (注意，_Signature_ 编码了方法是否是泛型，对于泛型方法，它编码了泛型参数的数量。)  (然而，请注意，如果 _Flags_.`CompilerControlled` = 1，那么此行被排除在重复检查之外)  \[ERROR\]

 22. 在 _MethodDef_ 表中，基于所有者 + _Name_ + _Signature_，不应有重复的行，其中 _Name_ 字段使用CLS冲突标识符规则进行比较；此外，签名中定义的类型应该是不同的。所以，例如，`int i` 和 `float i` 将被视为CLS重复；此外，忽略了方法的返回类型 (然而，请注意，如果 _Flags_.`CompilerControlled` = 1，如上所述，此行被排除在重复检查之外。)  \[CLS\]

 23. 如果在 _Flags_ 中设置了 `Final`、`NewSlot` 或 `Strict`，那么也应设置 _Flags_.`Virtual` \[ERROR\]

 24. 如果设置了 _Flags_.`PInvokeImpl`，那么 _Flags_.`Virtual` 应为0 \[ERROR\]

 25. 如果 _Flags_.`Abstract` &ne; 1，那么以下条件中必须有一个也为真： \[ERROR\]

     * RVA &ne; 0

     * _Flags_.`PInvokeImpl` = 1

     * _ImplFlags_.`Runtime` = 1

 26. 如果方法是 `CompilerControlled`，那么RVA应为非零或标记为 `PinvokeImpl` = 1 \[ERROR\]

 27. _Signature_ 应具有以下托管调用约定中的恰好一个 \[ERROR\]

     1. `DEFAULT` (0x0)

     2. `VARARG` (0x5)

     3. `GENERIC` (0x10)

 28. _Signature_ 应具有调用约定 `DEFAULT` (0x0) 或 `GENERIC` (0x10)。 \[CLS\]

 29. _Signature_：当且仅当方法不是 `Static` 时，_Signature_ 中的调用约定字节的 `HASTHIS` (0x20) 位应被设置 \[ERROR\]

 30. _Signature_：如果方法是 `static`，那么调用约定中的 `HASTHIS` (0x20) 位应为0  \[ERROR\]

 31. 如果签名中的 `EXPLICITTHIS` (0x40) 被设置，那么 `HASTHIS` (0x20) 也应被设置 (注意，如果设置了 `EXPLICITTHIS`，那么代码是不可验证的)  \[ERROR\]

 32. `EXPLICITTHIS` (0x40) 位只能在函数指针的签名中设置：MethodDefSig 前面有 `FNPTR` (0x1B) 的签名 \[ERROR\]

 33. 如果 _RVA_ = 0，那么以下条件之一必须为真： \[ERROR\]

     * _Flags_.`Abstract` = 1

     * _ImplFlags_.`Runtime` = 1

     * _Flags_.`PinvokeImpl` = 1

34. 如果 _RVA_ ≠ 0，那么：\[错误\]

     1. _Flags_.`Abstract` 应为 0，并且

     2. _ImplFlags_.`CodeTypeMask` 应具有以下值之一：`Native`，`CIL` 或 `Runtime`，并且

     3. _RVA_ 应指向此文件中的 CIL 代码流

 35. 如果 _Flags_.`PinvokeImpl` = 1 那么 \[错误\]

     * _RVA_ = 0 并且方法在 _ImplMap_ 表中拥有一行

 36. 如果 _Flags_.`RTSpecialName` = 1 那么 _Name_ 应为以下之一：\[错误\]

     1. `.ctor` (一个对象构造器方法) 

     2. `.cctor` (一个类构造器方法) 

 37. 相反，如果 _Name_ 是上述特殊名称中的任何一个，那么 _Flags_.`RTSpecialName` 应被设置 \[错误\]

 38. 如果 _Name_ = `.ctor` (一个对象构造器方法) 那么：

     1. _Signature_ 中的返回类型应为 `ELEMENT_TYPE_VOID` (参见 §[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md))  \[错误\]

     2. _Flags_.`Static` 应为 0  \[错误\]

     3. _Flags_.`Abstract` 应为 0  \[错误\]

     4. _Flags_.`Virtual` 应为 \[错误\]

     5. 'Owner' 类型应为 _TypeDef_ 表中的有效类或值类型 (不是 `<Module>` 且不是接口)  \[错误\]

     6. 对于任何给定的 'owner'，可以有零个或多个 `.ctor`

 39. 如果 _Name_ = `.cctor` (一个类构造器方法) 那么：

     1. _Signature_ 中的返回类型应为 `ELEMENT_TYPE_VOID` (参见 §[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md))  \[错误\]

     2. _Signature_ 的调用约定应为 `DEFAULT` (0x0) \[错误\]

     3. _Signature_ 中不应提供参数 \[错误\]

     4. _Flags_.`Static` 应被设置  \[错误\]

     5. _Flags_.`Virtual` 应被清除 \[错误\]

     6. _Flags_.`Abstract` 应被清除 \[错误\]

 40. 在 _TypeDef_ 表中的任何给定行拥有的方法集合中，只能有 0 或 1 个名为 `.cctor` 的方法 \[错误\]

> _结束信息性文本。_


### 20.27. MethodImpl: 0x19
<a id="MethodImpl_0x19"></a>

_MethodImpl_ 表允许编译器覆盖 CLI 提供的默认继承规则。它们最初的用途是允许一个类 `C`，它从接口 `I` 和 `J` 都继承了方法 `M`，为这两个方法提供实现 (而不是在其 vtable 中只有 `M` 的一个插槽)。然而，_MethodImpls_ 也可以出于其他原因使用，只受限于编译器编写者在下面定义的验证规则的约束内的独创性。

在上面的例子中，_Class_ 指定 `C`，_MethodDeclaration_ 指定 `I::M`，_MethodBody_ 指定为 `I::M` 提供实现的方法 (要么是 `C` 内的一个方法体，要么是 `C` 的基类实现的一个方法体)。

_MethodImpl_ 表有以下列：

 * _Class_ (一个指向 _TypeDef_ 表的索引) 

 * _MethodBody_ (一个指向 _MethodDef_ 或 _MemberRef_ 表的索引；更准确地说，是一个 _MethodDefOrRef_ (§[II.24.2.6](ii.24.2.6-metadata-stream.md)) 编码索引) 

 * _MethodDeclaration_ (一个指向 _MethodDef_ 或 _MemberRef_ 表的索引；更准确地说，是一个 _MethodDefOrRef_ (§[II.24.2.6](ii.24.2.6-metadata-stream.md)) 编码索引) 

ILAsm 使用 **.override** 指令来指定 _MethodImpl_ 表的行 (§[II.10.3.2](ii.10.3.2-the-override-directive.md) 和 §[II.15.4.1](ii.15.4.1-method-body.md))。

> _这只包含信息性文本。_

 1. _MethodImpl_ 表可以包含零行或多行

 2. _Class_ 应该索引 _TypeDef_ 表中的有效行 \[错误\]

 3. _MethodBody_ 应该索引 _MethodDef_ 或 _MemberRef_ 表中的有效行 \[错误\]

 4. 由 _MethodDeclaration_ 索引的方法应该设置 _Flags_.`Virtual` \[错误\]

 5. 由 _MethodDeclaration_ 索引的方法的所有者类型不应该有 _Flags_.`Sealed` = 0 \[错误\]

 6. 由 _MethodBody_ 索引的方法应该是 _Class_ 或 _Class_ 的某个基类的成员 (*MethodImpls* 不允许编译器“挂钩”任意方法体)  \[错误\]

 7. 由 _MethodBody_ 索引的方法应该是虚方法 \[错误\]

 8. 由 _MethodBody_ 索引的方法应该有其 _Method_._RVA_ ≠ 0 (例如，不能是通过 PInvoke 到达的非托管方法)  \[错误\]

 9. _MethodDeclaration_ 应该索引 _Class_ 的祖先链中的一个方法 (通过其 _Extends_ 链到达) 或 _Class_ 的接口树中的一个方法 (通过其 _InterfaceImpl_ 条目到达)  \[错误\]

 10. 由 _MethodDeclaration_ 索引的方法不应该是 final (其 _Flags_.`Final` 应该是 0)  \[错误\]

 11. 如果 _MethodDeclaration_ 设置了 `Strict` 标志，那么由 _MethodDeclaration_ 索引的方法应该对 _Class_ 是可访问的。 \[错误\]

 12. 由 _MethodBody_ 定义的方法签名应该与 _MethodDeclaration_ 定义的那些匹配 \[错误\]

 13. 基于 _Class_+_MethodDeclaration_ 不应该有重复的行 \[错误\]

> _结束信息性文本。_

### 20.28. MethodSemantics: 0x18
<a id="MethodSemantics_0x18"></a>

_MethodSemantics_ 表有以下列：

 * _Semantics_ (一个2字节的位掩码，类型为 _MethodSemanticsAttributes_，参见 §[II.23.1.12](ii.23.1.12-flags-for-methodsemantics-methodsemanticsattributes.md)) 

 * _Method_ (一个索引，指向 _MethodDef_ 表) 

 * _Association_ (一个索引，指向 _Event_ 或 _Property_ 表；更准确地说，是一个 _HasSemantics_ (§[II.24.2.6](ii.24.2.6-metadata-stream.md)) 编码索引) 

_MethodSemantics_ 表的行由 **.property** (§[II.17](ii.17-defining-properties.md)) 和 **.event** 指令 (§[II.18](ii.18-defining-events.md)) 填充。 (有关更多信息，请参见 §[II.22.13](ii.22.13-event-0x14.md)。) 

> _这只包含信息性文本。_

 1. _MethodSemantics_ 表可以包含零行或多行

 2. _Semantics_ 只应设置那些指定的值 \[ERROR\]

 3. _Method_ 应索引 _MethodDef_ 表中的有效行，该行应为此行描述的属性或事件的同一类中定义的方法 \[ERROR\]

 4. 对于给定的属性或事件，所有方法应具有相同的可访问性 (即他们的 _Flags_ 行的 `MemberAccessMask` 子字段) 并且不能是 `CompilerControlled` \[CLS\]

 5. _Semantics_：受以下限制：

     * 如果此行是用于属性的，那么 `Setter`、`Getter` 或 `Other` 中的恰好一个应被设置 \[ERROR\]

     * 如果此行是用于事件的，那么 `AddOn`、`RemoveOn`、`Fire` 或 `Other` 中的恰好一个应被设置 \[ERROR\]

 6. 如果此行是用于事件的，并且其 _Semantics_ 是 `Addon` 或 `RemoveOn`，那么由 _Method_ 索引的 _MethodDef_ 表中的行应接受一个委托作为参数，并返回 `void` \[ERROR\]

 7. 如果此行是用于事件的，并且其 _Semantics_ 是 `Fire`，那么由 _Method_ 索引的 _MethodDef_ 表中的行可以返回任何类型

 8. 对于每个属性，应有一个设置器，或一个获取器，或两者都有 \[CLS\]

 9. 任何属性的获取器方法，其 _Name_ 是 `xxx`，应被称为 `get_xxx` \[CLS\]

 10. 任何属性的设置器方法，其 _Name_ 是 `xxx`，应被称为 `set_xxx` \[CLS\]

 11. 如果一个属性提供了获取器和设置器方法，那么这些方法应在 _Flags_.`MemberAccessMask` 子字段中具有相同的值 \[CLS\]

 12. 如果一个属性提供了获取器和设置器方法，那么这些方法应对于他们的 _Method_._Flags_.`Virtual` 具有相同的值 \[CLS\]

 13. 任何获取器和设置器方法应具有 _Method_._Flags_.`SpecialName` = 1 \[CLS\]

 14. 任何获取器方法应具有与 _Property_._Type_ 字段索引的签名匹配的返回类型 \[CLS\]

 15. 任何设置器方法的最后一个参数应具有与 _Property_._Type_ 字段索引的签名匹配的类型 \[CLS\]

 16. 任何设置器方法应在 _Method_._Signature_ 中具有返回类型 `ELEMENT_TYPE_VOID` (§[II.23.1.16](ii.23.1.16-element-types-used-in-signatures.md))  \[CLS\]

 17. 如果属性是索引的，那么获取器和设置器的索引在数量和类型上应一致 \[CLS\]

 18. 任何事件的 *AddOn* 方法，其 _Name_ 是 `xxx`，应具有签名：`void add_xxx (`\<DelegateType\>`handler)` (§[I.10.4](i.10.4-naming-patterns.md))  \[CLS\]

 19. 任何事件的 *RemoveOn* 方法，其 _Name_ 是 `xxx`，应具有签名：`void remove_xxx(`\<DelegateType\>` handler)` (§[I.10.4](i.10.4-naming-patterns.md))  \[CLS\]

 20. 任何事件的 *Fire* 方法，其 _Name_ 是 `xxx`，应具有签名：`void raise_xxx(Event e)` (§[I.10.4](i.10.4-naming-patterns.md))  \[CLS\]

> _结束信息性文本。_

### 20.29. MethodSpec: 0x2B
<a id="MethodSpec0x2B"></a>

_MethodSpec_ 表有以下列：

 * _Method_ (_MethodDef_ 或 _MemberRef_ 表的索引，指定此行引用的泛型方法；也就是说，此行是哪个泛型方法的实例；更准确地说，是 _MethodDefOrRef_ (参见 §[II.24.2.6](ii.24.2.6-metadata-stream.md)) 编码索引) 

 * _Instantiation_ (Blob 堆的索引 (参见 §[II.23.2.15](ii.23.2.15-methodspec.md))，保存此实例的签名) 
 
_MethodSpec_ 表记录实例化泛型方法的签名。每个唯一的泛型方法实例 (即，_Method_ 和 _Instantiation_ 的组合) 应由表中的单个行表示。

> _这只包含信息性文本。_

 1. _MethodSpec_ 表可以包含零行或多行

 2. 一个或多个行可以引用 _MethodDef_ 或 _MemberRef_ 表中的相同行。 (可以有同一泛型方法的多个实例。) 

 3. 存储在 _Instantiation_ 中的签名应为 _Method_ 存储的泛型方法的签名的有效实例 \[错误\]

 4. 基于 _Method_+_Instantiation_ 不应有重复行 \[错误\]

> _结束信息性文本。_

### 20.30. Module: 0x00
<a id="Module_0x00"></a>

_Module_ 表有以下列：

 * _Generation_ (一个 2 字节的值，保留，应为零) 

 * _Name_ (一个指向 String 堆的索引) 

 * _Mvid_ (一个指向 Guid 堆的索引；简单地说，是一个用于区分同一模块的两个版本的 Guid) 

 * _EncId_ (一个指向 Guid 堆的索引；保留，应为零) 

 * _EncBaseId_ (一个指向 Guid 堆的索引；保留，应为零) 

Mvid 列应该索引 GUID 堆中的一个唯一 GUID (§[II.24.2.5](ii.24.2.5-guid-heap.md))，该 GUID 标识此模块的此实例。CLI 的符合实现可以在读取时忽略 _Mvid_。_Mvid_ 应该为每个模块新生成，使用 ISO/IEC 11578:1996 (附录 A) 或其他兼容算法指定的算法。

_[注意：_ 术语 GUID 表示全局唯一标识符，是一个通常使用其十六进制编码显示的 16 字节长的数字。可以通过几种众所周知的算法生成 GUID，包括在 RPC 和 CORBA 中用于 UUID (通用唯一标识符) 的算法，以及在 COM 中用于 CLSID、GUID 和 IID 的算法。_结束注释]_

_[理由：_ 虽然 VES 本身不使用 _Mvid_，但其他工具 (如调试器，这超出了本标准的范围) 依赖于 _Mvid_ 几乎总是与一个模块到另一个模块不同的事实。_结束理由]_

可以将 _Generation_、_EncId_ 和 _EncBaseId_ 列写为零，并且可以由 CLI 的符合实现忽略。

_Module_ 表中的行是程序集中的 **.module** 指令的结果 (§[II.6.4](ii.6.4-declaring-modules.md))。

> _这只包含信息性文本。_

 1. _Module_ 表应该包含一行且只有一行 \[错误\]

 2. _Name_ 应该索引 String 堆中的非空字符串。此字符串应该与解析到此模块的任何相应 _ModuleRef_._Name_ 字符串完全匹配。 \[错误\]

 3. _Mvid_ 应该索引 Guid 堆中的非空 GUID \[错误\]

> _结束信息性文本。_

### 20.31. ModuleRef: 0x1A
<a id="ModuleRef_0x1A"></a>

_ModuleRef_ 表有以下列：

 * _Name_ (一个索引，指向String堆) 

_ModuleRef_ 表中的行是由Assembly中的 **.module extern** 指令 (§[II.6.5](ii.6.5-referencing-modules.md)) 产生的。

> _这只包含信息性文本。_

 1. _Name_ 应索引String堆中的非空字符串。这个字符串应使CLI能够定位目标模块 (通常，它可能命名用于保存模块的文件)  \[ERROR\]

 2. 不应有重复的行  \[WARNING\]

 3. _Name_ 应与 _File_ 表的 _Name_ 列中的一个条目匹配。此外，该条目应使CLI能够定位目标模块 (通常它可能命名用于保存模块的文件)  \[ERROR\]

> _结束信息性文本。_

### 20.32. NestedClass: 0x29
<a id="NestedClass_0x29"></a>

_NestedClass_ 表有以下列：

 * _NestedClass_ (_TypeDef_ 表的索引) 

 * _EnclosingClass_ (_TypeDef_ 表的索引) 

_NestedClass_ 被定义为在其封闭类型的文本 '内部'。

> _这只包含信息性文本。_

_NestedClass_ 表记录哪些类型定义嵌套在哪些其他类型定义中。在典型的高级语言中，嵌套类被定义为在其封闭类型的文本 '内部'

 1. _NestedClass_ 表可以包含零行或多行

 2. _NestedClass_ 应索引 _TypeDef_ 表中的有效行 \[错误\]

 3. _EnclosingClass_ 应索引 _TypeDef_ 表中的有效行 (特别注意，不允许索引 _TypeRef_ 表)  \[错误\]

 4. 不应有重复行 (即 _NestedClass_ 和 _EnclosingClass_ 的值相同)  \[警告\]

 5. 给定类型只能由一个封闭器嵌套。因此，不能有两行具有相同的 _NestedClass_ 值，但 _EnclosingClass_ 值不同 \[错误\]

 6. 给定类型可以 '拥有' 几种不同的嵌套类型，因此完全有效地具有两行或多行具有相同的 _EnclosingClass_ 值，但 _NestedClass_ 值不同

> _结束信息性文本。_
### 20.33. Param: 0x08
<a id="Param_0x08"></a>

_Param_ 表有以下列：

 * _Flags_ (一个 2 字节的位掩码，类型为 _ParamAttributes_，§[II.23.1.13](ii.23.1.13-flags-for-params-paramattributes.md)) 

 * _Sequence_ (一个 2 字节的常数) 

 * _Name_ (一个指向 String 堆的索引) 
 
从概念上讲，_Param_ 表中的每一行都由 _MethodDef_ 表中的一行，且只有一行，拥有。

_Param_ 表中的行是方法声明中的参数 (§II.15.4)，或者是附加到方法的 **.param** 属性 (§[II.15.4.1](ii.15.4.1-method-body.md)) 的结果。

> _这只包含信息性文本。_

 1. _Param_ 表可以包含零行或多行

 2. 每一行应该有一个，且只有一个，在 _MethodDef_ 表中的所有者行 \[错误\]

 3. _Flags_ 只应设置那些指定的值 (所有组合有效)  \[错误\]

 4. _Sequence_ 应该有一个值 &ge; 0 并且 &le; 所有者方法中的参数数量。_Sequence_ 值为 0 指的是所有者方法的返回类型；然后从 1 开始编号其参数 \[错误\]

 5. 由同一方法拥有的 _Param_ 表的连续行应该按照增加的 _Sequence_ 值排序——尽管序列中允许有间隙 \[警告\]

 6. 如果 _Flags_.`HasDefault` = 1 那么此行应该在 _Constant_ 表中拥有恰好一行 \[错误\]

 7. 如果 _Flags_.`HasDefault` = 0，那么在 _Constant_ 表中不应该有任何由此行拥有的行 \[错误\]

 8. 如果 _Flags_.`FieldMarshal` = 1 那么此行应该在 `FieldMarshal` 表中拥有恰好一行 \[错误\]

 9. _Name_ 可以为空或非空

 10. 如果 _Name_ 是非空的，那么它应该索引 String 堆中的非空字符串 \[警告\]

> _结束信息性文本。_
### 20.34. Property: 0x17
<a id="Property_0x17"></a>

在元数据中，属性最好被视为一种手段，用于将定义在类上的方法集合聚在一起，给它们一个名字，而不是其他。这些方法通常是已经在类上定义的 *get_* 和 *set_* 方法，并像其他方法一样插入到 _MethodDef_ 表中。这种关联是由三个独立的表维护在一起，如下图所示：

 ![](ii.22.34-property-0x17-figure-1.png)

_PropertyMap_ 表的第3行索引了左边 _TypeDef_ 表的第2行 (`MyClass`)，同时索引了右边 _Property_ 表的第4行 - 一个名为 Foo 的属性的行。这个设置建立了 `MyClass` 有一个名为 `Foo` 的属性。但是在 _MethodDef_ 表中，哪些方法被聚集在一起作为 '属于' 属性 `Foo`？这种关联包含在 _MethodSemantics_ 表中 - 它的第2行索引了右边的属性 `Foo`，和左边 _MethodDef_ 表的第2行 (一个名为 `get_Foo` 的方法)。此外，_MethodSemantics_ 表的第3行索引了 `Foo` 到右边，和左边 _MethodDef_ 表的第3行 (一个名为 `set_Foo` 的方法)。如阴影所示，`MyClass` 还有另一个属性，叫做 `Bar`，有两个方法，`get_Bar` 和 `set_Bar`。

属性表做的不仅仅是将其他表中已有的行聚集在一起。_Property_ 表有 _Flags_、_Name_ (例如这里的 `Foo` 和 `Bar`) 和 _Type_ 的列。此外，_MethodSemantics_ 表有一个列来记录它指向的方法是 *set_*、*get_* 还是 *other*。

_[注意：_ CLS (参见 Partition I) 引用了实例、虚拟和静态属性。属性的签名 (来自 _Type_ 列) 可以用来区分静态属性，因为实例和虚拟属性在签名中会设置 "`HASTHIS`" 位 (§[II.23.2.1](ii.23.2.1-methoddefsig.md))，而静态属性则不会。实例和虚拟属性之间的区别取决于 getter 和 setter 方法的签名，CLS 要求它们要么都是虚拟的，要么都是实例的。_结束注释]_

_Property_ (0x17) 表有以下列：

 * _Flags_ (一个2字节的位掩码，类型为 _PropertyAttributes_，§II.23.1.14) 

 * _Name_ (一个索引，指向String堆) 
 
 * _Type_ (一个索引，指向Blob堆)  (这个列的名称是误导性的。它不是索引 _TypeDef_ 或 _TypeRef_ 表，而是索引了 Blob 堆中的属性的签名) 

> _这只包含信息性文本。_

 1. _Property_ 表可以包含零行或多行

 2. 每一行应有一个，且只有一个，在 _PropertyMap_ 表中的所有者行 (如上所述)  \[ERROR\]

 3. _PropFlags_ 只应设置那些指定的值 (所有组合有效)  \[ERROR\]

 4. _Name_ 应索引String堆中的非空字符串 \[ERROR\]

 5. _Name_ 字符串应是一个有效的CLS标识符 \[CLS\]

 6. _Type_ 应索引Blob堆中的非空签名 \[ERROR\]

 7. 由 _Type_ 索引的签名应是一个有效的属性签名 (即，领先字节的低四位是0x8)。除了这个领先字节，签名与属性的 *get_* 方法相同 \[ERROR\]

 8. 在由 _TypeDef_ 表中的给定行拥有的行中，基于 _Name_+_Type_ 不应有重复的行 \[ERROR\]

 9. 基于 _Name_，不应有重复的行，其中 _Name_ 字段使用CLS冲突标识符规则进行比较 (特别是，属性不能通过它们的类型进行重载 - 例如，一个类不能有两个属性，"`int Foo`" 和 "`String Foo`")  \[CLS\]

> _结束信息性文本。_
### 20.35. PropertyMap: 0x15
<a id="PropertyMap_0x15"></a>

_PropertyMap_ 表有以下列：

 * _Parent_ (_TypeDef_ 表的索引) 

 * _PropertyList_ (_Property_ 表的索引)。它标记了由 _Parent_ 拥有的属性的连续运行的第一个。运行继续到以下较小者：

     * _Property_ 表的最后一行

     * 通过检查此 _PropertyMap_ 表中下一行的 _PropertyList_ 找到的下一组属性

_PropertyMap_ 和 _Property_ 表是将 **.property** 指令放在类上的结果 (参见 §[II.17](ii.17-defining-properties.md))。

> _这只包含信息性文本。_

 1. _PropertyMap_ 表可以包含零行或多行

 2. 基于 _Parent_ 不应有重复行 (给定类只有一个指向其属性列表开始的 '指针')  \[错误\]

 3. 基于 _PropertyList_ 不应有重复行 (不同的类不能在 _Property_ 表中共享行)  \[错误\]

> _结束信息性文本。_

### 20.36. StandAloneSig: 0x11
<a id="StandAloneSig_0x11"></a>

签名存储在元数据 Blob 堆中。在大多数情况下，它们由某个表的某个列索引——_Field_._Signature_、_Method_._Signature_、_MemberRef_._Signature_ 等。然而，有两种情况需要一个元数据 _token_ 来表示一个不由任何元数据表索引的签名。_StandAloneSig_ 表满足了这个需求。它只有一列，该列指向 Blob 堆中的一个 _Signature_。

签名应描述以下之一：

 * **一个方法** - 代码生成器为每次出现 `calli` CIL 指令在 _StandAloneSig_ 表中创建一行。该行索引 `calli` 指令的函数指针操作数的调用点签名

 * **局部变量** - 代码生成器为每个方法在 _StandAloneSig_ 表中创建一行，以描述其所有的局部变量。ILAsm 中的 **.locals** 指令 (§[II.15.4.1](ii.15.4.1-method-body.md)) 生成 _StandAloneSig_ 表中的一行。

_StandAloneSig_ 表有以下列：

 * _Signature_ (一个指向 Blob 堆的索引) 

_[示例：_

 ```ilasm
 // 在遇到 calli 指令时，ilasm 在 blob 堆中生成一个签名
 //  (DEFAULT，ParamCount = 1，RetType = int32，Param1 = int32)，
 // 由 StandAloneSig 表索引：
 .assembly Test {}
 .method static int32 AddTen(int32)
 { ldarg.0
   ldc.i4  10
   add
   ret
 }
 .class Test
 { .method static void main()
   { .entrypoint
     ldc.i4.1
     ldftn int32 AddTen(int32)
     calli int32(int32)
     pop
     ret
   }
 }
 ```

_结束示例]_

> _这只包含信息性文本。_

 1. _StandAloneSig_ 表可以包含零行或多行

 2. _Signature_ 应该索引 Blob 堆中的有效签名 \[错误\]

 3. 由 _Signature_ 索引的签名 'blob' 应该是一个有效的 `METHOD` 或 `LOCALS` 签名 \[错误\]

 4. 允许重复的行

> _结束信息性文本。_

Source: Conversation with Bing, 2023/12/25
(1) github.com. https://github.com/stakx/ecma-335/tree/68d5015b146d347b2d76bd67d150af5f3fa68178/docs%2Fii.22.36-standalonesig-0x11.md.

### 20.37. TypeDef: 0x02
<a id="TypeDef_0x02"></a>

_TypeDef_ 表有以下列：

 * _Flags_ (一个4字节的 _TypeAttributes_ 类型的位掩码，参见 §[II.23.1.15](ii.23.1.15-flags-for-types-typeattributes.md)) 

 * _TypeName_ (一个指向字符串堆的索引) 
 
 * _TypeNamespace_ (一个指向字符串堆的索引) 
 
 * _Extends_ (一个指向 _TypeDef_，_TypeRef_ 或 _TypeSpec_ 表的索引；更准确地说，是一个 _TypeDefOrRef_ (参见 §[II.24.2.6](ii.24.2.6-metadata-stream.md)) 编码索引) 

 * _FieldList_ (一个指向 _Field_ 表的索引；它标记了由此类型拥有的一连串字段的第一个)。该连续运行继续到以下较小的一个：

   * _Field_ 表的最后一行

   * 通过检查此 _TypeDef_ 表中下一行的 _FieldList_ 找到的下一组字段

 * _MethodList_ (一个指向 _MethodDef_ 表的索引；它标记了由此类型拥有的一连串方法的第一个)。该连续运行继续到以下较小的一个：

   * _MethodDef_ 表的最后一行

   * 通过检查此 _TypeDef_ 表中下一行的 _MethodList_ 找到的下一组方法

   _TypeDef_ 表的第一行代表伪类，该伪类作为在模块范围内定义的函数和变量的父类。

注意，任何类型都应该是以下之一，并且只能是以下之一：

 * 类 (_Flags_._Interface_ = 0，并最终派生自 `System.Object`) 

 * 接口 (_Flags_._Interface_ = 1) 

 * 值类型，最终派生自 `System.ValueType`

对于任何给定的类型，都有两个独立且不同的指向其他类型的链 (这些指针实际上是作为元数据表索引实现的)。这两个链是：

 * 扩展链 - 通过 _TypeDef_ 表的 _Extends_ 列定义。通常，派生类扩展基类 (始终是一个，且只有一个，基类) 

 * 接口链 - 通过 _InterfaceImpl_ 表定义。通常，一个类实现零个、一个或多个接口

这两个链 (扩展和接口) 在元数据中始终保持分离。_Extends_ 链表示一对一关系，即一个类扩展 (或“派生自”) 另一个类 (称为其直接基类)。_Interface_ 链可以表示一对多关系，即一个类可能实现两个或更多接口。

接口也可以实现一个或多个其他接口 - 元数据通过 _InterfaceImpl_ 表存储这些链接 (这里的术语有些不适当 - 这里没有“实现”涉及；也许更清晰的名称可能是 _Interface_ 表，或 _InterfaceInherit_ 表) 

另一种稍微专门化的类型是*嵌套*类型，它在 ILAsm 中被声明为在封闭类型声明中词法嵌套。是否嵌套类型可以通过其 _Flags_._Visibility_ 子字段的值确定 - 它应该是 {_NestedPublic_, _NestedPrivate_, _NestedFamily_, _NestedAssembly_, _NestedFamANDAssem_, _NestedFamORAssem_} 集合中的一个。

如果类型是泛型，其参数在 _GenericParam_ 表中定义 (参见 §[II.22.20](ii.22.20-genericparam-0x2a.md))。_GenericParam_ 表中的条目引用 _TypeDef_ 表中的条目；_TypeDef_ 表没有引用 _GenericParam_ 表。

继承层次结构的根看起来像这样：

![继承层次结构的根](ii.22.37-typedef-0x02-figure-1.png)

有一个系统定义的根，`System.Object`。所有的类和值类型最终都应该从`System.Object`派生；类可以从其他类派生 (通过一个单一的，非循环的链) 到任何需要的深度。这个_扩展_继承链用重箭头表示。

 (关于`System.Delegate`类的详细信息请参阅下文) 

接口不从彼此继承；然而，它们可以有零个或多个必需的接口，这些接口应该被实现。_接口_需求链显示为轻的、虚线箭头。这包括接口和类/值类型之间的链接——后者被称为*实现*那个接口或接口。常规值类型 (即，排除枚举——见后文) 被定义为直接从`System.ValueType`派生。常规值类型不能派生到一个以上的深度。 (另一种表述方式是，用户定义的值类型应该是*密封的*。) 用户定义的枚举应该直接从`System.Enum`派生。枚举不能在`System.Enum`以下派生到一个以上的深度。 (另一种表述方式是，用户定义的枚举应该是*密封的*。) `System.Enum`直接从`System.ValueType`派生。

用户定义的委托从`System.Delegate`派生。委托不能派生到一个以上的深度。

关于声明类型的指令，请参见§[II.9](ii.9-generics.md)。

 1. _TypeDef_表可以包含一个或多个行。

 2. _Flags:_

     1. _Flags_只能设置那些指定的值 \[错误\]

     2. 可以设置0或1个`SequentialLayout`和`ExplicitLayout` (如果没有设置，则默认为`AutoLayout`)  \[错误\]

     3. 可以设置0或1个`UnicodeClass`和`AutoClass` (如果没有设置，则默认为`AnsiClass`)  \[错误\]

     4. 如果_Flags_.`HasSecurity` = 1，那么以下条件中至少有一个应该为真： \[错误\]

        * 这个类型在_DeclSecurity_表中拥有至少一行

        * 这个类型有一个名为`SuppressUnmanagedCodeSecurityAttribute`的自定义属性

     5. 如果这个类型在_DeclSecurity_表中拥有一行 (或多行)，那么_Flags_.`HasSecurity`应该是1 \[错误\]

     6. 如果这个类型有一个名为`SuppressUnmanagedCodeSecurityAttribute`的自定义属性，那么_Flags_.`HasSecurity`应该是1 \[错误\]

     7. 注意，接口设置`HasSecurity`是有效的。然而，安全系统忽略任何附加到该接口的权限请求

 3. _Name_应该在字符串堆中索引一个非空字符串 \[错误\]

 4. _TypeName_字符串应该是一个有效的CLS标识符 \[CLS\]

 5. _TypeNamespace_可以为空或非空

 6. 如果非空，那么_TypeNamespace_应该在字符串堆中索引一个非空字符串 \[错误\]

 7. 如果非空，_TypeNamespace_的字符串应该是一个有效的CLS标识符 \[CLS\]

 8. 每个类 (除了`System.Object`和特殊类`<Module>`) 都应该扩展一个，且只有一个，其他类——所以对于一个类，_Extends_应该是非空的 \[错误\]

 9. `System.Object`应该有一个_Extends_值为null [错误]

 10. `System.ValueType`应该有一个_Extends_值为`System.Object` \[错误\]

 11. 除了`System.Object`和特殊类`<Module>`，对于任何类，_Extends_应该索引在_TypeDef_，_TypeRef_，或_TypeSpec_表中的一个有效行，其中有效意味着1 ≤ 行 ≤ 行数。此外，该行本身应该是一个类 (而不是接口或值类型) 此外，该基类不应该被密封 (其_Flags_.`Sealed`应该是0)  \[错误\]

12. 一个类不能扩展自身或其子类 (即，它的派生类)，因为这将在层次树中引入循环 \[错误\]  (对于泛型类型，参见§[II.9.1](ii.9.1-generic-type-definitions.md) 和 §[II.9.2](ii.9.2-generics-and-recursive-inheritance-graphs.md)。) 

13. 一个接口永远不会扩展另一个类型 - 所以 _Extends_ 应该为空 (接口确实实现了其他接口，但请记住，这种关系是通过 _InterfaceImpl_ 表捕获的，而不是 _Extends_ 列)  \[错误\]

14. _FieldList_ 可以为空或非空

15. 一个类或接口可以 '拥有' 零个或多个字段

16. 一个值类型应该有一个非零的大小 - 通过定义至少一个字段，或者提供一个非零的 _ClassSize_ \[错误\]

17. 如果 _FieldList_ 是非空的，它应该索引 _Field_ 表中的一个有效行，其中有效意味着 1 ≤ 行 ≤ rowcount+1 \[错误\]

18. _MethodList_ 可以为空或非空

19. 一个类型可以 '拥有' 零个或多个方法

20. 值类型的运行时大小不应超过 1 MByte (0x100000 字节) \[错误\]

21. 如果 _MethodList_ 是非空的，它应该索引 _MethodDef_ 表中的一个有效行，其中有效意味着 1 ≤ 行 ≤ rowcount+1 \[错误\]

22. 一个类如果有一个或多个抽象方法不能被实例化，并且应该有 _Flags_.`Abstract` = 1。请注意，类拥有的方法包括从其基类和它实现的接口继承的所有方法，以及通过其 _MethodList_ 定义的方法。 (CLI 将在运行时分析类定义；如果它发现一个类有一个或多个抽象方法，但是 _Flags_.`Abstract` = 0，它将抛出一个异常)  \[错误\]

23. 一个接口应该有 _Flags_.`Abstract` = 1 \[错误\]

24. 对于一个抽象类型来说，有一个构造方法 (即，一个名为 `.ctor` 的方法) 是有效的

25. 任何非抽象类型 (即 _Flags_.`Abstract` = 0) 应该为其合同要求的每个方法提供一个实现 (主体)。它的方法可以从其基类继承，从它实现的接口继承，或者由它自己定义。实现可以从其基类继承，或者由它自己定义 \[错误\]

26. 一个接口 (_Flags_.`Interface` = 1) 可以拥有静态字段 (_Field_.`Static` = 1) 但不能拥有实例字段 (_Field_.`Static` = 0) \[错误\]

27. 一个接口不能被密封 (如果 _Flags_.`Interface` = 1，那么 _Flags_.`Sealed` 应该是 0)  \[错误\]

28. 一个接口拥有的所有方法 (_Flags_.`Interface` = 1) 应该是抽象的 (_Flags_.`Abstract` = 1) \[错误\]

29. 在 _TypeDef_ 表中，基于 _TypeNamespace_+_TypeName_ 不应该有重复的行 (除非这是一个嵌套类型 - 见下文)  \[错误\]

30. 如果这是一个嵌套类型，那么在 _TypeDef_ 表中，基于 _TypeNamespace_+_TypeName_+_OwnerRowInNestedClassTable_ 不应该有重复的行 \[错误\]

31. 不应该有重复的行，其中 _TypeNamespace_+_TypeName_ 字段使用 CLS 冲突标识符规则进行比较 (除非这是一个嵌套类型 - 见下文)  \[CLS\]

32. 如果这是一个嵌套类型，那么不应该有重复的行，基于 _TypeNamespace_+_TypeName_+_OwnerRowInNestedClassTable_ 并且 _TypeNamespace_+_TypeName_ 字段使用 CLS 冲突标识符规则进行比较 \[CLS\]

33. 如果 _Extends_ = `System.Enum` (即，类型是用户定义的枚举) 那么：

     1. 应该是封闭的 (`Sealed` = 1) \[错误\]

     2. 不应该有自己的任何方法 (_MethodList_ 链应该是零长度) \[错误\]

     3. 不应该实现任何接口 (此类型在 _InterfaceImpl_ 表中没有条目) \[错误\]

     4. 不应该有任何属性 \[错误\]

     5. 不应该有任何事件 \[错误\]

     6. 任何静态字段应该是文字的 (具有 _Flags_.`Literal` = 1) \[错误\]

     7. 应该有一个或多个静态，文字字段，每个字段都具有枚举的类型 \[CLS\]

     8. 应该有一个实例字段，为内置整数类型 \[错误\]

     9. 实例字段的 _Name_ 字符串应该是 "`value__`"，该字段应该被标记为 `RTSpecialName`，并且该字段应该具有 CLS 整数类型之一 \[CLS\]

     10. 除非它们是文字的，否则不应该有任何静态字段 \[错误\]

 34. 嵌套类型 (如上所定义) 应该在 _NestedClass_ 表中拥有恰好一行，其中“拥有”意味着在 _NestedClass_ 表中的一行，其 _NestedClass_ 列包含此类型定义的 _TypeDef_  _token_  \[错误\]

 35. ValueType 应该是封闭的 \[错误\]

> _结束信息性文本。_

### 20.38. TypeRef: 0x01
<a id="TypeRef_0x01"></a>

_TypeRef_表有以下列：

 * _ResolutionScope_ (一个索引，指向_Module_，_ModuleRef_，_AssemblyRef_或_TypeRef_表，或者为空；更准确地说，是一个_ResolutionScope_ (§[II.24.2.6](ii.24.2.6-metadata-stream.md)) 编码索引) 

 * _TypeName_ (一个指向字符串堆的索引) 

 * _TypeNamespace_ (一个指向字符串堆的索引) 

> _这只包含信息性文本。_

 1. _ResolutionScope_应该严格是以下之一：

    1. 空——在这种情况下，_ExportedType_表中应该有一行对应这个类型——它的_Implementation_字段应该包含一个_File_ _token_ 或一个_AssemblyRef_ _token_ ，说明类型在哪里定义 \[错误\]

    2. 一个_TypeRef_ _token_ ，如果这是一个嵌套类型 (例如，可以通过检查它的_TypeDef_表中的_Flags_列来确定——可访问性子字段是`tdNestedXXX`集合中的一个)  \[错误\]

    3. 一个_ModuleRef_ _token_ ，如果目标类型在与当前模块相同的程序集中的另一个模块中定义 \[错误\]

    4. 一个_Module_ _token_ ，如果目标类型在当前模块中定义——这在CLI ("压缩元数据") 模块中不应该出现 \[警告\]

    5. 一个_AssemblyRef_ _token_ ，如果目标类型在与当前模块不同的程序集中定义 \[错误\]

 2. _TypeName_应该在字符串堆中索引一个非空字符串 \[错误\]

 3. _TypeNamespace_可以为空，或非空

 4. 如果非空，_TypeNamespace_应该在字符串堆中索引一个非空字符串 \[错误\]

 5. _TypeName_字符串应该是一个有效的CLS标识符 \[CLS\]

 6. 不应该有重复的行，其中重复的行具有相同的_ResolutionScope_，_TypeName_和_TypeNamespace_ \[错误\]

 7. 不应该有重复的行，其中_TypeName_和_TypeNamespace_字段使用CLS冲突标识符规则进行比较 \[CLS\]

> _结束信息性文本。_

### 20.39. TypeSpec: 0x1B
<a id="TypeSpec_0x1B"></a>

_TypeSpec_ 表只有一列，它索引了存储在 Blob 堆中的一个类型的规范。这为该类型提供了一个元数据 _token_  (而不仅仅是一个指向 Blob 堆的索引)。这通常是必需的，例如，对数组操作，如创建或调用数组类的方法。

_TypeSpec_ 表有以下列：

 * _Signature_  (索引到 Blob 堆，其中 blob 的格式如 §[II.23.2.14](ii.23.2.14-typespec.md) 所指定) 

注意，_TypeSpec_  _token_ 可以与任何接受 _TypeDef_ 或 _TypeRef_  _token_ 的 CIL 指令一起使用；具体来说，`castclass`，`cpobj`，`initobj`，`isinst`，`ldelema`，`ldobj`，`mkrefany`，`newarr`，`refanyval`，`sizeof`，`stobj`，`box`，和 `unbox`。

> _这只包含信息性文本。_

 1. _TypeSpec_ 表可以包含零行或多行

 2. _Signature_ 应该索引 Blob 堆中的一个有效的类型规范 \[错误\]

 3. 基于 _Signature_，不应该有重复的行 \[错误\]

> _信息性文本结束。_

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


## 22. 元数据的物理布局

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

"`#GUID`" 标头指向一系列 128 位的 GUID。在流中可能存储了无法访问的 GUID。

#### 22.2.6. #~ 流

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

