# CSharp 类型转换与模式匹配

---
## 类型转换


转换使表达式被转换为或被视为属于特定类型。转换可能涉及表示的变化，转换可以是隐式转换或显式强制转换。一些转换是语言定义的，用户可以自定义类型转换。

```csharp
int a = 123;
long b = a;      // implicit
int c = (int)b;  // explicit
```

<br>

### 隐式转换

隐式转换可能会在多种情况下发生，包括函数成员调用、强制转换表达式、赋值运算等。预定义的隐式转换始终会成功，并且不会引发异常。

#### 隐式恒等转换

恒等转换将任意类型转换为相同类型，即类型 `T` 或类型 `T` 的表达式可转换为 `T` 本身：
- `dynamic` 和 `object` 之间存在恒等转换。
- 在相同泛型构造的类型之间，如果每个对应类型参数之间存在恒等转换，构造实例之间存在恒等转换。
- 具有相同密度的元组类型之间，如果每对对应元素类型之间存在恒等转换时，元组之间存在恒等转换。


#### 隐式数值转换

当某一数值类型 `T` 的值域在目标类型 `U` 的可表示值域范围内，则 `T` 可以隐式转换为 `U`，例如：
- 从 `byte`、`sbyte` 到 `short`、`int`、`long`、`float`、`double`、`decimal`。
- 从 `byte` 到 `ushort`、`uint`、`ulong`。
- 从 `short`、`ushort` 到 `int`、`long`、`float`、`double`、`decimal`。
- 从 `ushort` 到 `uint`、`ulong`。
- 从 `int`、`uint` 到 `long`、`float`、`double`、`decimal`。
- 从 `uint` 到 `ulong`。
- 从 `long`、`ulong` 到 `float`、`double`、`decimal`。
- 从 `char` 到 `ushort`、`short`、`uint`、`int`、`ulong`、`long`、`float`、`double`、`decimal`。
- 从 `float` 到 `double`。

从整型到浮点数的转换可能会导致精度损失，但是不会导致范围损失。

#### 隐式枚举转换

隐式枚举转换允许将任意整数类型的 `0` 值常量转换为任意枚举类型和任何可空枚举类型。其他的整数类型数值需要强制转换运算。

```csharp
TaskStatus status0 = 0;
TaskStatus status = (TaskStatus)5;
```

#### 隐式内插字符串转换

隐式内插字符串转换允许将内插字符串转换为 `System.IFormattable` 或 `System.FormattableString`。

#### 隐式可空转换

对于非空值类型的预定义隐式转换，也可以用于这些类型的可空值类型。非空值类型 `T` 到其可空类型 `T?` 之间存在隐式转换。

基于从 `S` 到 `T` 的底层转换的可空转换求值过程：
- 对于 `S?` 到 `T?` 的转换，若 `S?` 的值为空，则结果为 `T?` 的空值；否则转换将作为从 `S?` 展开为 `S` 后，再从 `S` 到 `T`，再从 `T` 包装为 `T?`。
- 对于 `S` 到 `T?` 的转换，先从 `S` 到 `T`，再从 `T` 包装到 `T?`。

```csharp
int num = 10;
long lnum = num;    // S to T
long? nlnum = num;  // S to T?

int? n_num = num;
long? n_lnum = n_num;  // S? to T?
```


#### null 的转换

存在从 `null` 字面值到任何引用类型或可空值类型的隐式转换。对于引用类型转换为空引用，对于可空值类型则生成空值。

#### 隐式引用转换

隐式引用转换是指那些可以证明总是成功的引用类型之间的转换，在运行时不需要检查。隐式引用转换有：
- 从任意引用类型到 `object` 和 `dynamic` 的转换。
- 从任意类类型 `S` 到其基类 `T`、基接口 `I` 的转换。
- 从任意接口类型 `I` 到其基接口的 `Ibase` 的转换。
- 元素类型为 `Se` 的数组 `S` 到元素类型为 `Te` 的数组 `T` 存在隐式转换需要满足：
  - 数组 `S` 和 `T` 具有相同的维度。
  - `Se` 和 `Te` 都是引用类型，且 `Se` 到 `Te` 存在隐式转换。  
- 从任何数组类型到 `System.Array` 以及它实现的接口类型的转换。
- 从任意一维数组类型 `S[]` 到 `System.Collections.Generic.IList<T>` 及其基接口的隐式转换，需满足 `S` 到 `T` 存在隐式恒等转换或隐式引用转换。 
- 从任意委托类型到 `Delegate` 和它实现的接口的转换。
- 从 null 值到任意引用类型。
- 从任意引用类型 `S` 到引用类型 `T` 的隐式转换，需满足从 `S` 到 `T` 存在隐式恒等转换或隐式引用转换。
- 从任意引用类型 `S` 到接口或委托类型 `T` 的隐式转换，需满足 `S` 到 `T` 存在隐式恒等转换或隐式引用转换，且 `S` 可以协变转换到 `T`。
- 涉及任何引用类型的类型参数的隐式转换。
 
引用转换永远不会改变被转换对象的引用标识。虽然引用转换可以更改引用的类型，但它不会更改引用对象的类型或值。

#### 装箱转换

装箱转换允许将值类型隐式转换为引用类型，装箱转换有：
- 从任意值类型到 `object`、`System.ValueType` 的转换。
- 从任意枚举类型到 `System.Enum` 的转换。
- 从任意非空值类型到其实现的接口的转换。
- 从任意非空值类型到任意接口 `I` 的隐式转换，需满足非空值类型可装箱转换的另一个接口 `I0` 到 `I` 之间存在恒等转换或协变转换。
- 从任意可空类型到任意引用类型的转换，其中存在可空类型的基础类型到该引用类型的装箱转换。

将非空值类型的值装箱包括分配一个对象实例并将值复制到该实例中。若可空值类型的值是空值，则其装箱为空引用，否则将展开底层值并生成该值装箱的引用。

#### 隐式动态转换

存在从动态类型表达式到任意类型 `T` 的隐式动态转换，该转换是动态绑定的，这意味着将在运行时寻求从表达式的运行时类型到 `T` 的隐式转换，若无法成功转换，则抛出运行时异常。

```csharp
object o  = "object"
dynamic d = "dynamic";

string s1 = o;  // Fails at compile-time -- no conversion exists
string s2 = d;  // Compiles and succeeds at run-time
int i     = d;  // Compiles but fails at run-time -- no conversion exists
```

#### 隐式常量表达式转换

隐式常量表达式的转换允许：
- `int` 类型的常量表达式可以转换为 `sbyte`、`byte`、`short`、`ushort`、`uint`、`ulong` 类型，前提是该常量值在目标类型的范围内。
- `long` 类型的常量表达式可以转换为 `ulong` 类型，前提是非负值。

#### 涉及类型参数的隐式转换

给定的类型参数 `T` 存在以下隐式转换： 

- 对于已知为引用类型的类型参数 `T`，允许从 `T` 到其任意基类 `C`、从由 `T` 到 `C` 实现的任何接口 `I` 和 `I` 的任意基接口的隐式引用转换。
  
- 若 `T` 不知是否为引用类型时，涉及 `T` 到任意基类或基接口的转换在编译时被认为是装箱转换。在运行时，如果 `T` 是值类型，则转换为装箱转换执行，否则转换作为隐式转换或恒等转换执行。

- 从 `T` 到类型参数 `U` 的转换，具体取决于 `U` 的类型参数约束。若 `U` 是值类型，则 `T` 到 `U` 的类型必须相同，且不执行任何转换；若 `T` 是值类型，则转换将作为装箱转换；否则将作为隐式引用转换或恒等转换。

- 从 `null` 到引用类型的类型参数 `T`。 

#### 隐式元组转换

如果元组表达式 `E` 与元组类型 `T` 具有相同的密度，且存在从 `E` 中的每个元素到 `T` 中相应元素类型的隐式转换，则存在 `E` 到 `T` 的隐式转换。转换通过创建 `System.ValueTuple<...>` 类型，并从左到右的顺序初始化它的每个字段。

如果元组表达式中的元素名与元组类型中相应的元素名不匹配，则发出警告，表达式的元素名称将被忽略。

```csharp
(int, string) t1 = (1, "One");
(byte, string) t2 = (2, null);
(int, string) t3 = (null, null);        // Error: No conversion
(int i, string s) t4 = (i: 4, "Four");
(int i, string) t5 = (x: 5, s: "Five"); // Warning: Names are ignored
```

#### 用户定义的隐式转换

用户定义的隐式转换包括由从一个可选的标准隐式转换，到执行用户定义的隐式转换运算符，再到执行另一个可选的标准隐式转换。

#### 匿名函数转换和方法组转换

匿名函数和方法组本身没有类型，它们可以隐式地转换为委托类型。一些 Lambda 表达式可以隐式转换为表达式树类型。

#### 默认值转换

存在从 `dafault` 到任何类型的隐式转换，此转换将生成推断类型的默认值。

#### 隐式抛出转换

`throw` 表达式没有类型，但是它们可以隐式转换为任何类型。

<br>

### 显式转换

显式转换可在强制转换表达式（`(type)value`）中发生。显式转换集包含所有的隐式转换，即隐式转换可以显式使用强制转换表达式。

```csharp
int num = 123;
object obj = (object)num;  // 隐式转换

int num2 = (int)obj;       // 显式强制转换
```

不是隐式转换的显式转换是指不能证明总是成功的转换、已知可能丢失信息的转换以及跨类型域的转换，这些转换差异很大，必须显式标记。显式转换可能会存在无效的强制转换。

#### 显式数字转换

- 从 `sbyte` 到 `byte`、`ushort`、`uint`、`ulong`、`char`。
- 从 `byte` 到 `sbyte`、`char`.
- 从 `short` 到 `sbyte`、`byte`、`ushort`、`uint`、`ulong`、`char`
- 从 `ushort` 到 `sbyte`、`byte`、`short`、`char`
- 从 `int` 到 `sbyte`、`byte`、`short`、`ushort`、`uint`、`ulong`、`char`。
- 从 `uint` 到 `sbyte`、`byte`、`short`、`ushort`、`int`、`char`。
- 从 `long` 到 `sbyte`、`byte`、`short`、`ushort`、`int`、`uint`、`ulong`、`char`。
- 从 `ulong` 到 `sbyte`、`byte`、`short`、`ushort`、`int`、`uint`、`long`、`char`。
- 从 `char` 到 `sbyte`、`byte`、`short`。
- 从 `float` 到 `sbyte`、`byte`、`short`、`ushort`、`int`、`uint`、`long`、`ulong`、`char`、`decimal`。
- 从 `double` 到 `sbyte`、`byte`、`short`、`ushort`、`int`、`uint`、`long`、`ulong`、`char`、`float`、`decimal`。
- 从 `decimal` 到 `sbyte`、`byte`、`short`、`ushort`、`int`、`uint`、`long`、`ulong`、`char`、`float`、`double`。

由于显式转换可能会丢失信息，或可能导致引发异常。对于显式数值转换：
- 在 `checked` 的上下文，源操作数在目标类型范围内则转换成功，否则将引发 `System.OverflowException` 异常。
- 在 `unchecked` 的上下文，始终转换成功。对于溢出的源类型将放弃额外的高位来截断源值，而小的源值则是符号扩展（有符号数）或零扩展（无符号数）高位。
- 对于从 `decimal` 到整数类型的转换，源值向零舍入到最接近的整数值。超出范围时引发 `Overflow` 异常。
- 对于 `float`、`double` 到整数类型的转换，在检查的上下文中，超出范围会引发溢出异常；未检查的上下文中向零舍入到最接近的整数值，不在范围的浮点值将是目标类型的未指定值。
- 对于 `double` 转换为 `float`，将值舍入到最接近的 `float` 值。若太小则结果是正零或负零，太大则是正无穷或负无穷。
- 对于 `float`、`double` 到 `decimal` 的转换，将源值转换为 `decimal` 格式，并将第 28 位小数后舍入到最接近的数。若太小时结果为零，若是 `nan`、无穷大或值太大而无法表示为 `decimal` 时，将引发溢出异常。
- 对于从 `decimal` 到浮点类型的转换，将源值舍入到最接近的目标类型值。这种转换可能会丢失精度，但不会引发异常。

#### 显式枚举转换

显式枚举转换包括从任意数值类型到枚举类型的显式转换，从任意枚举类型到任意数值类型的转换，或任意枚举类型之间的转换。

整数类型的 0 值可以隐式转换为枚举类型。

#### 显式可空转换

对于非空值类型的预定义显式转换，也可以用于这些类型的可空值类型。非空值类型 `T?` 到其可空类型 `T` 之间存在式转换。

基于从 `S` 到 `T` 的底层显式转换的可空显式转换求值过程：
- 对于 `S?` 到 `T?` 的转换，若 `S?` 的值为空，则结果为 `T?` 的空值；否则转换将作为从 `S?` 展开为 `S` 后，再从 `S` 到 `T`，再从 `T` 包装为 `T?`。
- 对于 `S` 到 `T?` 的转换，先从 `S` 到 `T`，再从 `T` 包装到 `T?`。
- 对于 `S?` 到 `T` 的转换，先是 `S?` 展开到 `S`，再从 `S` 到 `T` 的基本转换。

```csharp
long lnum = 10;    
int num = (int)lnum;  // (T)S to T
int? nnum = (int?)lnum;  // (T?)S to T?

long? l_num = lnum;   // T to T?
int? n_num = (int?)l_num;  // (T?)S? to T?
```

#### 显式引用转换

显式引用转换是需要运行时检查以确保它们正确的引用类型之间的转换。如果显式引用转换失败，将引发 `System.InvalidCastException` 异常。

若要在运行时成功进行显式引用转换，源操作数的值必须为 `null`，或者源操作数引用的对象的实际类型必须是可通过隐式引用转换转换为目标类型的类型或装箱转换。

```csharp
object obj = "Hello";
string str = (string)obj;

obj = 123;
str = (string)obj;  // err : 源类型的运行时类型 int 无法隐式转换为 string
```

#### 显式元组转换

如果元组表达式 `E` 与元组类型 `T` 具有相同的密度，且存在从 `E` 中的每个元素到 `T` 中相应元素类型的显式转换，则存在 `E` 到 `T` 的显式转换。转换通过创建 `System.ValueTuple<...>` 类型，并从左到右的顺序初始化它的每个字段，并对每一个元素应用显式转换。

如果元组表达式中的元素名与元组类型中相应的元素名不匹配，则发出警告，表达式的元素名称将被忽略。

```csharp
(int, string) t1 = (ValueTuple<int, string>)(1L, "One");
(byte, string) t2 = (ValueTuple<byte, string>)(2L, null);
(int, string) t3 = (ValueTuple<int, string>)(null, null);        // Error: No conversion
(int i, string s) t4 = (ValueTuple<int, string>)(d: 3.1415, "Four"); // Warning: Names are ignored
```

#### 拆箱转换

拆箱转换允许将引用类型显式转换为值类型。拆箱转换操作包括：首先检查对象实例是否是给定值类型的装箱值，然后将该值从实例中复制出来。拆箱到可空值类型时，空引用生成为可空值类型的 `null` 值。拆箱空引用将引发 `System.NullReferenceException`

前提是引用类型是包含目标类型的装箱类型或兼容类型，否则将引发 `System.InvalidCastException` 异常。

#### 显式动态转换

存在从 `dynamic` 到任何类型的 `T` 的显式动态转换，转换是动态绑定的，这意味着将在运行时检查表达式的运行时类型是否与目标类型存在显式转换。不存在任何转换时将产生异常。

#### 涉及类型参数的显式转换

给定的类型参数 `T` 存在以下显式转换： 

- 对于已知为引用类型的类型参数 `T`，允许从 `T` 的任意有效基类 `C` 到 `T`、或任意接口到 `T` 的显式引用转换。
  
- 若 `T` 不知是否为引用类型时，涉及任意基类或基接口到 `T` 的转换在编译时被认为是拆箱转换。在运行时，如果 `T` 是值类型，则转换为拆箱转换执行，否则转换作为显式引用转换或恒等转换执行。

- 从 `U` 到类型参数 `T` 的转换，前提是 `T` 依赖于 `U`。在运行时，若 `T`、`U` 都是值类型，则 `T` 和 `U` 的类型必须相同，且不执行任何转换；若 `T` 是值类型，`U` 是引用类型，则转换将作为拆箱转换；否则 `T`、`U` 都是引用类型且转换将作为显式引用转换或恒等转换。

- 不允许从无约束的类型参数直接显式转换为非接口类型。此目的是为了防止语义混淆。

```csharp
//var a = Sample<int>.Fun(15);  // err
var a2 = Sample<long>.Fun(15);  // ok

class Sample<T>
{
    public static long Fun(T t)
    {
        // return (long)t;  // err
        return (long)(object)t;  // t 必须是 long
    }
}
```

#### 用户定义的显式转换

用户定义的显式转换包括先可选的标准显式转换，然后执行用户定义的隐式或显式转换操作符，最后是另一个可选的标准显式转换。

<br>

### 用户定义转换

用户定义的转换将源表达式转换为另一种类型。用户定义转换的求值以查找源表达式或目标类型的最特定的用户定义转换运算符为核心。当确定了最特定的用户定义转换运算符，则用户定义的转换最多执行：
- （可选）首先，执行从源表达式到用户定义或提升的转换运算的操作数类型的标准转换。
- 接下来，调用用户定义或提升的转换运算符来执行转换。
- （可选）最后，执行从用户定义转换的结果类型到目标类型的标准转换。

标准转换指的是非用户定义的其他隐式或显式转换。用户定义的转换求值从不涉及多个用户定义或提升的转换运算符。也就是说，从类型 `S` 到 `T`，不会涉及先从 `S` 到 `X`，然后从 `X` 到 `T` 的转换。

使用关键字 `implicit`（隐式）和 `explicit`（显式）声明用户定义转换。定义转换的类型必须是该转换的源类型或目标类型。其中一个操作数必须是源类型。

```csharp
var p = (Point)(10, 1);
(double x, double y) P = new Point(1, 10);

class Point(double x, double y)
{
    public double X => x;
    public double Y => y;

    // 从 Point 到元组的隐式转换
    public static implicit operator (double x, double y)(Point p) => (p.X, p.Y);
    // 从 元组 到 Point 的显式转换
    public static explicit operator Point((double x, double y) p) => new Point(p.x, p.y);
}
```

<br>

### 匿名方法的转换

匿名方法表达式（`delegate`）和 Lambda 表达式或语句被归类为匿名方法。匿名方法没有类型，但可以隐式地转换为兼容的委托类型。一些 Lambda 表达式也可以转换为兼容的表达式树类型。

匿名方法 `F` 和委托类型 `D` 兼容，则：
- 若 `F` 包含一个匿名方法签名，则 `D` 和 `F` 具有相同数目的参数和返回类型。

```csharp
Action<int, string> dele = delegate (int x, string y) { };
Action<int, string> dele2 = delegate (long x, string y) { }; // err 签名不一致
```

- 若 `F` 不包含方法签名，则 `D` 可以有零到多个任意类型的参数，只要没有 `out` 修饰符。

```csharp
Action<int, string> dele = delegate { };
Action dele2 = delegate { };    // err : 委托参数中包含 out 参数

delegate void Action(out int val);
```

- 若 `F` 具有显式类型化的参数列表，则 `D` 的每个参数与 `F` 中相应的参数具有相同的类型和修饰符。

```csharp
Action dele2 = delegate (out int x, string str, ref float y) { x = 10; };   
delegate void Action(out int val, string str, ref float f);
```

- 若 `F` 具有隐式类型化的参数列表，则 `D` 中没有 `ref`、`in`、`out` 参数。

```csharp
Action dele = (x, str) => { x = str.Length; };
delegate void Action(int val, string str);
```

#### 匿名方法转换到委托类型的求值

将匿名函数转换为委托类型会生成一个委托实例，该实例引用匿名函数并可能在转换时捕获处于活动状态的外部变量集（可能为空）。当调用委托时，将执行匿名函数体。主体中的代码使用委托引用捕获的外部变量集执行。

从匿名函数生成的委托的调用列表包含单个条目，其中委托的确切目标对象和目标方法未指定。特别是，委托的目标对象是空、封闭函数成员的 `this` 值还是其他对象都没有指定。

将语义相同的匿名函数与捕获的相同（可能为空的）外部变量实例集转换为相同的委托类型，允许（但不是必需）返回相同的委托实例。相同是指在所有情况下，在给定相同参数的情况下，匿名函数的执行将产生相同的效果。

```csharp
delegate double Function(double x);

class Test
{
    static double[] Apply(double[] a, Function f)
    {
        double[] result = new double[a.Length];
        for (int i = 0; i < a.Length; i++)
            result[i] = f(a[i]);
        return result;
    }

    static void F(double[] a, double[] b)
    {
        a = Apply(a, (double x) => Math.Sin(x));
        b = Apply(b, (double y) => Math.Sin(y));
        // 由于两个匿名函数委托具有捕获的外部变量的相同集，并且由于匿名函数在语
        // 义上是相同的，因此编译器允许委托引用相同的目标方法。实际上，允许编译
        // 器从两个匿名函数表达式返回完全相同的委托实例。
    }
}
```
 
#### Lambda 表达式转换到表达式树类型的求值

将 Lambda 表达式转换为表达式树类型会生成表达式树类型，即 Lambda 表达式转换的结果产生一个表示 Lambda 表达式本身结构的对象结构。

并非每个 Lambda 表达式都可以转换为表达式树类型。始终存在到兼容委托类型的转换，但由于特定于实现的原因，它可能在编译时失败。常见的原因包括：
- Lambda 包含一个语句块。
- Lambda 是异步的。
- Lambda 包含 `in`、`out`、`ref` 参数。
- Lambda 包含赋值操作。
- Lambda 包含一个动态操作。

<br>

### 方法组的转换

存在从方法组 `E` 到兼容委托类型 `D` 的隐式转换。

```csharp
delegate string D1(object o);
delegate object D2(string s);
delegate object D3();
delegate string D4(object o, params object[] a);
delegate string D5(int i);

class Test
{
    static string Fun(object o) => "";
    static void Test()
    {
        D1 d1 = Fun;            // Ok
        D2 d2 = Fun;            // Ok
        D3 d3 = Fun;            // Error -- 不适用
        D4 d4 = Fun;            // Error -- 适用但不兼容
        D5 d5 = Fun;            // Error -- 不兼容
    }
}
```

---
## 模式匹配

模式是一种语法形式，可以使用 `is` 表达式、`switch` 语句和 `switch` 表达式将输入表达式与任意数量的特征匹配。C# 支持多种模式，包括声明、类型、常量、关系、属性、列表、var 和弃元。可以使用布尔逻辑关键字 `and`、`or` 和 `not` 组合模式。

模式匹配类型：
  - 声明模式：用于检查表达式的运行时类型，如果匹配成功，则将表达式结果分配给声明的变量。
  - 类型模式：用于检查表达式的运行时类型。 在 C# 9.0 中引入。
  - 常量模式：用于测试表达式结果是否等于指定常量。
  - 关系模式：用于将表达式结果与指定常量进行比较。 在 C# 9.0 中引入。
  - 逻辑模式：用于测试表达式是否与模式的逻辑组合匹配。 在 C# 9.0 中引入。
  - 属性模式：用于测试表达式的属性或字段是否与嵌套模式匹配。
  - 位置模式：用于解构表达式结果并测试结果值是否与嵌套模式匹配。
  - var 模式：用于匹配任何表达式并将其结果分配给声明的变量。
  - 弃元模式：用于匹配任何表达式。
  - 列表模式：测试序列元素是否与相应的嵌套模式匹配。 在 C# 11 中引入。

<br>

### is 表达式

```csharp
bool rt_1 = E is T;
bool rt_2 = E is T t;

// exam
int i = 34;
object iBoxed = i;
int? jNullable = 42;
if (jNullable is not null)  // 检查 null
    if (iBoxed is int a && jNullable is int b)
        Console.WriteLine(a + b);  // output 76
```

<br>

### switch 语句

```csharp
switch (<switch_on>)
{
    case <exp_1>:
    case <exp_2>: 
        // do...
        break; // return, goto
    default:
        // default do
        break;
}

// exam
DisplayMeasurement(-4);     // Output: Measured value is -4; too low.
DisplayMeasurement(5);      // Output: Measured value is 5.
DisplayMeasurement(30);     // Output: Measured value is 30; too high.
DisplayMeasurement(double.NaN);  // Output: Failed measurement.

void DisplayMeasurement(double measurement)
{
    switch (measurement)
    {
        case < 0.0: Console.WriteLine($"Measured value is {measurement}; too low."); break;
        case > 15.0: Console.WriteLine($"Measured value is {measurement}; too high."); break;
        case double.NaN: Console.WriteLine("Failed measurement."); break;
        default: Console.WriteLine($"Measured value is {measurement}."); break;
    }
}
```

<br>

### switch 表达式

```csharp
var rt = <switch_on> switch
{
    case_1 => case_1_return,
    case_2 => case_2_return,
    //....cases
    _ => default_return
};

// exam
public static class SwitchExample
{
    public enum Direction { Up, Down, Right, Left }
    public enum Orientation { North, South, East, West }
    public static Orientation ToOrientation(Direction direction) => direction switch
    {
        Direction.Up => Orientation.North,
        Direction.Right => Orientation.East,
        Direction.Down => Orientation.South,
        Direction.Left => Orientation.West,
        _ => throw new ArgumentOutOfRangeException(nameof(direction), $"Not expected direction value: {direction}"),
    };
    static void Main()
    {
        var direction = Direction.Right;
        Console.WriteLine($"Map view direction is {direction}." + $" Cardinal orientation is {ToOrientation(direction)}.");
        // Output: Map view direction is Right. Cardinal orientation is East.
    }
}
```

<br>

### 声明模式

- 使用声明模式检查表达式的运行时类型是否与给定类型兼容。声明模式的表达式结果 `E` 为非 null 且在运行时类型是 `T` 类型、或可隐式转换类型、或 `T` 的派生类型、或具有基础类型 `T` 的可为 null 的值类型、或存在从 `E` 的运行时类型到类型 `T` 的装箱或取消装箱转换，则模式匹配成功。  

> is 表达式

```csharp
object greeting = "Hello, World!";
if (greeting is string message)
    Console.WriteLine(message.ToLower());  // output: hello, world!
```

> switch 表达式和语句

```csharp
var numbers = new int[] { 10, 20, 30 };
Console.WriteLine(GetSourceLabel(numbers));  // output: 1

var letters = new List<char> { 'a', 'b', 'c', 'd' };
Console.WriteLine(GetSourceLabel(letters));  // output: 2
// switch 表达式
static int GetSourceLabel<T>(IEnumerable<T> source) => source switch
{
    Array array => 1,
    ICollection<T> collection => 2,
    _ => 3,
};
// switch 语句
static int GetSourceLabel<T>(IEnumerable<T> source)
{
    switch (source)
    {
        case Array array: /*do...*/ return 1;
        case ICollection<T> collection: return 2;
        default: return 3;
    }
}
```

- 可以使用弃元代替变量名。

```csharp
static int GetSourceLabel<T>(IEnumerable<T> source) => source switch
{
    Array _ => 1,     // 弃元替代
    ICollection<T> _ => 2,  
    _ => 3,
};
```

<br>

### 类型模式

- 使用类型模式检查表达式 `E` 的运行时类型是否与给定类型 `T` 兼容。类型模式的表达式结果 `E` 为非 null 且在运行时类型是 `T` 类型、或可隐式转换类型、或 `T` 的派生类型、或具有基础类型 `T` 的可为 null 的值类型、或存在从 `E` 的运行时类型到类型 `T` 的装箱或取消装箱转换，则模式匹配成功。

> is 表达式

```csharp
var input = Console.ReadLine();
if (input is string)
    Console.WriteLine("INPUT: " + input);
```

> switch 表达式和语句

```csharp
int[] d = null;
Console.WriteLine(GetSourceLabel(d));  // output: -1

var numbers = new int[] { 10, 20, 30 };
Console.WriteLine(GetSourceLabel(numbers));  // output: 1

var letters = new List<char> { 'a', 'b', 'c', 'd' };
Console.WriteLine(GetSourceLabel(letters));  // output: 2
// switch 表达式
static int GetSourceLabel<T>(IEnumerable<T> source) => source switch
{
    Array => 1, // 类型模式
    ICollection<T> => 2,
    null => -1,
    _ => 3,
};
// switch 语句
static int GetSourceLabel<T>(IEnumerable<T> source)
{
    switch (source)
    {
        case Array: return 1;
        case ICollection<T>: return 2;
        case null: return -1;
        default: return 3;
    }
};
```

<br>

### 常量模式

- 可使用常量模式来测试表达式结果是否等于指定的常量。

```csharp
// is 表达式
var input = Console.ReadLine();
if (input is not (null or "" or "\n"))
    Console.WriteLine("INPUT: "+input);

// switch 表达式
var rt = Console.ReadLine() switch
{
    "\n" => "\\n",
    "" => "Empty",  // 直接回车输入 ""
    string input => input,
    null => "null",     // ctrl+letter 输出 null
};
Console.WriteLine("INPUT: " + rt);

// switch 语句
switch (Console.ReadLine())
{
    case "\n": rt = "\\n"; break;
    case "": rt = "Empty"; break;
    case string input: rt = input; break;
    case null: rt = "null"; break;
}
Console.WriteLine("INPUT: " + rt);
```

<br>

### 关系模式

- 可使用关系模式将表达式结果与常量进行比较。在关系模式中，可使用关系运算符 `<`、`>`、`<=` 或 `>=` 中的任何一个。关系模式的右侧部分必须是常数表达式。

```csharp
Console.WriteLine(Classify(13));   // output: Too high
Console.WriteLine(Classify(double.NaN));  // output: Unknown
Console.WriteLine(Classify(2.4));  // output: Acceptable

static string Classify(double measurement) => measurement switch
{
    < -4.0 => "Too low",
    > 10.0 => "Too high",
    double.NaN => "Unknown",
    _ => "Acceptable",
};
```

<br>

### 逻辑模式

- 可使用 `not`、`and` 和 `or` 模式连结符来创建逻辑模式。其中优先级为 `not` > `and` > `or`。

```csharp
Console.WriteLine(Classify(13));    // output: High
Console.WriteLine(Classify(-100));  // output: Too low
Console.WriteLine(Classify(5.7));   // output: Acceptable
static string Classify(double measurement) => measurement switch
{
    < -40.0 => "Too low",
    >= -40.0 and < 0 => "Low",
    >= 0 and < 10.0 => "Acceptable",
    >= 10.0 and < 20.0 => "High",
    >= 20.0 => "Too high",
    double.NaN => "Unknown",
};
```

<br>

### 属性模式

- 可以使用属性模式将表达式的属性或字段与嵌套模式进行匹配。

```csharp
public record Order(int Items, decimal Cost);

public decimal CalculateDiscount(Order order) => order switch
{
    { Items: > 10, Cost: > 1000.00m } => 0.10m,
    { Items: > 5, Cost: > 500.00m } => 0.05m,
    { Cost: > 250.00m } => 0.02m,
    null => throw new ArgumentNullException(nameof(order), "Can't calculate discount on null order"),
    var someObject => 0m,
};
```

> 嵌套

```csharp
public record Point(int X, int Y);
public record Segment(Point Start, Point End);

static bool IsAnyEndOnXAxis(Segment segment) =>
    segment is { Start: { Y: 0 } } or { End: { Y: 0 } };
// 扩展属性模式：等价行为
static bool IsAnyEndOnXAxis(Segment segment) =>
    segment is { Start.Y: 0 } or { End.Y: 0 };
```

<br>

### 位置模式

- 可使用位置模式来解构表达式结果并将结果值与相应的嵌套模式匹配。

```csharp
static string Classify(Point point) => point switch
{
    (0, 0) => "Origin",
    ( > 0, 0) => "positive X",
    ( < 0, 0) => "negative X",
    (0, > 0) => "positive Y",
    (0, < 0) => "negative Y",
    _ => "Just a point",
};
public readonly struct Point(int x, int y)
{
    public int X { get; } = x;
    public int Y { get; } = y;
    // Deconstruct 方法用于解构表达式结果
    public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);
}
```

> 在位置模式中使用属性模式

```csharp
var random = new Random(DateTime.Now.Microsecond);
int index = 0;
while (index < 100)
{
    index++;
    var input = new Vector2D(random.NextDouble() * 20 - 10, random.NextDouble() * 20 - 10);
    if (input is Vector2D(> 0, > 0) p)
    {
        if (IsInDomain(p))
            Console.WriteLine(p + " is in Domain"); // 第一象限与半径为 10 的圆的相交区域
    }
}
static bool IsInDomain(Vector2D point) => point is ( > 0, > 0) { magnitude: < 10 };
public record Vector2D(double X, double Y)
{
    public double magnitude => Math.Abs(unchecked(Math.Sqrt(X * X + Y * Y)));
    public override string ToString() => ($"{X:F2}", $"{Y:F2}").ToString();
}
```

<br>

### var 模式

- 可使用 `var` 模式来匹配任何表达式（包括 `null`），并将其结果分配给新的局部变量。

```csharp
Console.WriteLine(Transform(new Point(1, 2)));  // output: Point { X = -1, Y = 2 }
Console.WriteLine(Transform(new Point(5, 2)));  // output: Point { X = 5, Y = -2 }

static Point Transform(Point point) => point switch
{
    var (x, y) when x < y => new Point(-x, y),
    var (x, y) when x > y => new Point(x, -y),
    (int x, int y) => new Point(x, y),
};
public record Point(int X, int Y);
```

<br>

### 弃元模式

- 可使用弃元模式 `_` 来匹配任何表达式，包括 `null`。

```csharp
static decimal GetDiscountInPercent(DayOfWeek? dayOfWeek) => dayOfWeek switch
{
    DayOfWeek.Monday => 0.5m,
    DayOfWeek.Tuesday => 12.5m,
    DayOfWeek.Wednesday => 7.5m,
    DayOfWeek.Thursday => 12.5m,
    DayOfWeek.Friday => 5.0m,
    DayOfWeek.Saturday => 2.5m,
    DayOfWeek.Sunday => 2.0m,
    _ => 0.0m,  // 弃元
};
```

<br>

### 列表模式

- 从 C#11 开始，可以将数组或列表与模式的序列进行匹配。当每个嵌套模式与输入序列的相应元素匹配时，列表模式就会匹配。若要匹配任何元素，可使用弃元模式；若想要捕获元素，可使用 `var` 模式；若要仅匹配输入序列开头或 / 和结尾的元素，可使用切片模式 `..`，切片模式匹配零个或多个元素，最多可在列表模式中使用一个切片模式。

```csharp
Console.WriteLine(new[] { 1, 2, 3, 4, 5 } is [> 0, > 0, ..]);  // True
Console.WriteLine(new[] { 1, 1 } is [_, _, ..]);               // True
Console.WriteLine(new[] { 0, 1, 2, 3, 4 } is [> 0, > 0, ..]);  // False
Console.WriteLine(new[] { 1 } is [1, 2, ..]);                  // False

Console.WriteLine(new[] { 1, 2, 3, 4 } is [.., > 0, > 0]);     // True
Console.WriteLine(new[] { 2, 4 } is [.., > 0, 2, 4]);          // False
Console.WriteLine(new[] { 2, 4 } is [.., 2, 4]);               // True

Console.WriteLine(new[] { 1, 2, 3, 4 } is [>= 0, .., 2 or 4]); // True
Console.WriteLine(new[] { 1, 0, 0, 1 } is [1, 0, .., 0, 1]);   // True
Console.WriteLine(new[] { 1, 0, 1 } is [1, 0, .., 0, 1]);      // False
```

- 可以在切片模式中嵌套子模式。

```csharp
void MatchMessage(string message)
{
    var result = message is ['a' or 'A', .. var s, 'a' or 'A']
        ? $"Message {message} matches; inner part is {s}."
        : $"Message {message} doesn't match.";
    Console.WriteLine(result);
}

MatchMessage("aBBA");  // output: Message aBBA matches; inner part is BB.
MatchMessage("apron");  // output: Message apron doesn't match.

void Validate(int[] numbers)
{
    var result = numbers is [< 0, .. { Length: >= 2 and <= 4 }, > 0] ? "valid" : "not valid";
    Console.WriteLine(result);
}

Validate(new[] { -1, 0, 1 });  // output: not valid
Validate(new[] { -1, 0, 0, 1 });  // output: valid
Validate(new[] { -1, 0, 2, 0, 1 });  // output: valid
```

---