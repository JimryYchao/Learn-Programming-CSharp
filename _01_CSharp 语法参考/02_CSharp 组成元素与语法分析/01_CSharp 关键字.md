## CSharp 关键字

---

### 类型关键字

```csharp
// 整数
sbyte       short       int         long        nint
byte        ushort      uint        ulong       nuint
// 浮点数
float       double      decimal
// 其他值类型
bool        // 布尔类型        
char        // 字符类型
enum        // 枚举类型
struct      // 结构类型
// 引用类型
object      // 全体基类
string      // 字符串
delegate    // 委托
dynamic     // 动态类型
record      // 记录类型
class       // 类类型
interface   // 接口类型
void        // 无返回类型
var         // 隐式类型
```

<br>

### 修饰符

```csharp
// 访问修饰符
public      protected   private     internal    file

abstract    // 抽象声明
async       // 异步声明
const       // 常量声明
event       // 事件声明
extern      // 外部引用声明
in          // 泛型逆变声明
new         // 成员隐藏声明
out         // 泛型协变声明
override    // 重写声明
readonly    // 只读声明
sealed      // 密封声明
static      // 静态声明
unsafe      // 不安全上下文
virtual     // 虚方法声明
volatile    // 异变类型声明
```

<br>

### 语句关键字

```csharp
// 选择语句
if-else        
switch case
// 迭代语句
do-while
for
foreach
while
// 跳转语句
break
continue
goto
return
// 异常处理
try
catch
finally
throw
// 溢出检查
checked
unchecked
// 不安全上下文
fixed
// 锁定
lock
// yield 语句
yield
```

<br>

### 方法参数

```csharp
params          // 可变参数
in              // 只读参数修饰
ref             // 引用参数修饰
out             // 导出参数修饰
```

<br>

### 命名空间关键字

```csharp
namespace       // 命名空间声明
using           // using 指令
extern          // 外部别名 
```

<br>

### 泛型类型约束关键字

```csharp
where           // 指定类型参数的约束条件
struct          // 不可为 null 值类型约束
class           // 不可为 null 的引用类型
class?          // 可为 null 的引用类型
notnull         // 不可为 null 的类型
default         // default 约束表示基方法
unmanaged       // 不可为 null 的非托管类型
new()           // 指定类型必须具有公共无参构造函数
// 类型参数必须是指定的基类或派生自指定的基类
<base class>    
<base class>?   
// 类型参数必须是指定的接口或实现指定的接口
<interface>     
<interface>?    
// 为 T 提供的类型参数必须是为 U 提供的参数或派生自为 U 提供的参数
where T : U
```

<br>

### 访问关键字

```csharp
base            // 基类对象访问
this            // 当前实例对象
```

<br>

### 文字关键字

```csharp
// 空引用
null            
// 布尔值
true        false
// 默认值
default
```

<br>

### 上下文关键字

```csharp
// 访问器
get         set         init        value
add         remove
// 分部声明
partial
// 条件筛选
when

```

<br>

### 查询关键字

```csharp
from	        // 指定数据源和范围变量（类似于迭代变量）。
where	        // 基于由逻辑 AND 和 OR 运算符（&& 或 ||）分隔的一个或多个布尔表达式筛选源元素。
select	        // 指定执行查询时，所返回序列中元素的类型和形状。
group	        // 根据指定的密钥值对查询结果分组。
into	        // 提供可作为对 join、group 或 select 子句结果引用的标识符。
orderby	        // 根据元素类型的默认比较器对查询结果进行升序或降序排序。
join	        // 基于两个指定匹配条件间的相等比较而联接两个数据源。
let	            // 引入范围变量，在查询表达式中存储子表达式结果。
in	            // join 子句中的上下文关键字。
on	            // join 子句中的上下文关键字。
equals	        // join 子句中的上下文关键字。
by	            // group 子句中的上下文关键字。
ascending	    // orderby 子句中的上下文关键字。
descending	    // orderby 子句中的上下文关键字。
```

<br>

### 运算符重载

```csharp
explicit        // 显示转换
implicit        // 隐式转换
operator        // 运算符重载
```

<br>

### 运算符与表达式

```csharp
is
as
typeof
nameof
sizeof

```



---
### 修饰符

- 类型访问修饰符：`public`、`internal`、`protected`、`private`、`file`。
- 类型修饰符：`abstract`、`async`、`const`、`event`、`extern`、`in`、`new`、`out`、`override`、`readonly`、`sealed`、`static`、`unsafe`、`virtual`、`volatile`。

<br>

#### 访问修饰符

访问修饰符关键字用于指定成员或类型已声明的可访问性，指定相应的访问性级别：
- `public`：访问不受限制。
- `internal`：访问限于当前程序集。
- `protected`：访问限于包含类或派生自包含类的类型。
- `protected internal`：访问限于当前程序集或派生自包含类的类别。
- `private`：访问限于包含类。
- `private protected`：访问限于包含类或当前程序集中派生自包含类的类型。
- `file`：（C#11）已声明的类型仅在当前源文件中可见。文件范围的类型通常用于源生成器。

> 可访问性级别

使用访问修饰符 `public`、`protected`、`internal`、`private` 为顶级类型和成员类型指定可访问性级别。如果未在成员声明中指定访问修饰符，则将使用默认可访问性。

命名空间没有任何访问限制。顶级类型只能具有 `internal` 或 `public` 可访问性，默认可访问性为 `internal`。它们的成员则默认具有：
- `enum` 的成员默认为 `public`，其成员不允许添加访问修饰符。
- `class` 的成员默认为 `private`，其成员可以声明 `public`、`internal`、`protected`、`protected internal`、`private`、`private protected`。
- `interface` 的成员默认为 `public`，其成员可以声明为 `public`、`internal`、`protected`、`protected internal`、`private`、`private protected`，其中声明为 `private` 的接口成员必须具有默认的实现。
- `struct` 的成员默认为 `private`，其成员可以声明为 `public`、`internal`、`private`。

嵌套类型的可访问性依赖于它的可访问域，该域是由已声明的成员可访问性和直接包含类型的可访问域这二者共同确定的。嵌套类型的可访问域不能超出包含类型的可访问域。成员的可访问域可指定成员可以引用哪些程序分区。顶级类型的可访问域至少是在其中进行声明的项目的程序文本，该域包含此项目的所有源文件。

> 文件本地类型（C#11）

`file` 修饰符将顶级类型的范围和可见性限制为其所包含的文件范围。`file` 修饰符通常应用于源生成器编写的类型。**文件本地类型** 为源生成器提供了一种方便的方法，能够避免在生成的类型之间发生名称冲突。

`file` 可用于修饰 `class`、`struct`、`enum`、`interface`、`record`、`delegate`、`record struct`、`Attribute class`。

<br>

#### abstract

- `abstract` 修饰符指示被修改内容的实现已丢失或不完整。`abstract` 修饰符可用于类、方法、属性、索引和事件，且只能在抽象类中使用。标记为抽象的成员必须由派生自抽象类的非抽象类来实现（`override`）。

```csharp
public abstract class Father{
    public abstract void Func();
}
public class Son : Father{
    public override void Func(){  /* implementation  */  }
}
```

> 抽象类

- 抽象类无法被实例化，它可能包含有抽象方法或访问器。抽象方法需要由其非抽象派生类进行实现，因此抽象类要求类被继承，所以抽象类的抽象成员不能是 `static`。
- 抽象方法是隐式的虚拟方法，且只能在抽象类中声明。

<br>

#### async

- `async` 可将方法、lambda 表达式或匿名方法指定为异步。异步方法同步运行，直至到达其第一个 `await` 表达式，此时会将方法挂起，直到等待的任务完成。若异步方法中不包含 `await` 表达式或语句，方法将以同步执行。

- 异步方法可返回 `Task`、`Task<TResult>`、`void`（调用方不能使用 `await`）、任何具有可访问的 `GetAwaiter` 方法的类型。
- 异步方法不能声明任何 `in`、`ref`、`out` 参数，也不能具有引用返回值。

<br>

#### const

- `const` 关键字来声明某个常量字段或局部变量。`readonly` 关键字与 `const` 关键字不同，`const` 字段只能在该字段的声明中初始化，`readonly` 字段可以在声明或构造函数中初始化。

```csharp
const string Language = "C#";
const string Platform = ".NET";
const string Version = "11.0";
```

<br>

#### event

- `event` 关键字用于声明发布服务器类中的事件。事件是一种特殊的多播委托，仅可以从声明事件的类（或派生类）或结构（发布服务器类）中对其进行调用。如果其他类或结构订阅该事件，则在发布服务器类引发该事件时，将调用其事件处理程序方法。

```csharp
public class Program{
    static void Main(string[] args){
        var e = EntityCreator.GetEntity();
    }

    class EntityCreator{
        static void print(Entity e) => Console.WriteLine(e.ToString() + " Created");
        static EntityCreator() => Entity.PrintEvent += print;  // 注册 create 事件
        public static Entity GetEntity() => new Entity();
    }
    class Entity{
        public static event Action<Entity> PrintEvent;
        public Entity() => PrintEvent?.Invoke(this);
    }
}
```

#### extern

- `extern` 修饰符用于声明在外部实现的方法。`extern` 修饰符的常见用法是在使用 `Interop` 服务调入非托管代码时与 `DllImport` 特性一起使用。在这种情况下，还必须将方法声明为 `static`。

```csharp
using System.Runtime.InteropServices;
class ExternTest
{
    [DllImport("User32.dll", CharSet=CharSet.Unicode)]
    public static extern int MessageBox(IntPtr h, string m, string c, int type);

    static int Main()
    {
        string myString;
        Console.Write("Enter your message: ");
        myString = Console.ReadLine();
        return MessageBox((IntPtr)0, myString, "My Message Box", 0);
    }
}
```

<br>
 
#### in （泛型修饰）

- 对于泛型类型参数，`in` 关键字可指定类型参数是逆变的。可以在泛型接口和委托中使用 `in` 关键字。引用类型支持泛型类型参数中的协变和逆变，但值类型不支持它们。逆变使用户使用的类型可以比泛型参数指定的类型派生程度更小，这样可以隐式转换实现协变接口的类以及隐式转换委托类型。

```csharp
// 逆变泛型接口
interface IContravariant<in A> { }
interface IExtContravariant<in A> : IContravariant<A> { }
class Sample<A> : IContravariant<A> { }
void TestInterface()
{
    IContravariant<object> iobj = new Sample<object>();
    IContravariant<string> istr = iobj;
    istr = iobj;        // 逆变
}
// 逆变泛型委托
delegate void DContravariant<in A>(A argument);
static void SampleFunc(IContravariant<object> obj) { }
static void SampleExtFunc(IExtContravariant<object> exObj) { }
void TestDelegate(){
    DContravariant<IContravariant<object>> Dobj = SampleFunc;
    DContravariant<IExtContravariant<object>> Dstr = SampleExtFunc;
    Dstr = Dobj;        // 逆变
}
```

<br>

#### new （成员修饰）

- `new` 在用作声明修饰符时，可以显式隐藏从基类继承的成员。

```csharp
public class BaseC
{
    public int x;
    public void Invoke() { }
}
public class DerivedC : BaseC
{
    new public void Invoke() { }
}
```

<br>

#### out （泛型修饰）

- 对于泛型类型参数，`out` 关键字可指定类型参数是协变的。可以在泛型接口和委托中使用 `out` 关键字。协变使用户使用的类型可以比泛型参数指定的类型派生程度更大，这样可以隐式转换实现协变接口的类以及隐式转换委托类型。引用类型支持协变和逆变，值类型不支持。

```csharp
interface ICovariant<out R> { }
class Sample<R> : ICovariant<R> { }
void Test()
{
    ICovariant<Object> iobj = new Sample<Object>();
    ICovariant<String> istr = new Sample<String>();
    iobj = istr;        // 协变
}
```

<br>

#### override

- 扩展或修改继承的方法、属性、索引器或事件的抽象或虚拟实现需要 `override` 修饰符，通过 `override` 声明重写的方法称为重写基方法。`override` 方法支持协变返回类型。

```csharp
interface IInstance
{
    IInstance GetInstance();
}
abstract class BaseClass : IInstance
{
    public abstract IInstance GetInstance();
}
class SampleClass : BaseClass
{
    // 重写方法并返回其协变类型 SampleClass
    static SampleClass s_instance = new SampleClass();
    public sealed override SampleClass GetInstance() => s_instance;
}
```

<br>

#### readonly

- `readonly` 可以在：
  - 在字段声明中，`readonly` 指示只能在声明期间或在同一个类的构造函数中向字段赋值，可以在字段声明和构造函数中多次分配和重新分配只读字段。值类型直接包含数据，因此属于 `readonly` 值类型的字段不可变；引用类型包含对其数据的引用，因此属于 `readonly` 引用类型的字段必须始终引用同一对象，但是该对象的数据是可变的。
  - 在 `readonly struct` 类型定义中，`readonly` 指示结构类型是不可变的。 
  - 在结构类型内的实例成员声明中，`readonly` 指示实例成员不修改结构的状态。
  - 在 `ref readonly` 方法返回中，`readonly` 修饰符指示该方法返回一个引用，且不允许向该引用写入内容。

```csharp
class SampleClass
{
    private int GUI;
    // 只读字段
    private static readonly SampleClass s_default = new SampleClass(0);
    // ref readonly 返回
    public static ref readonly SampleClass Origin => ref s_default;

    public Data data { get; }
    public SampleClass(int gui)
    {
        GUI = gui;
        data = new Data(GetHashCode());
    }
    // 只读结构
    public readonly struct Data
    {
        // 只读结构中的只读属性
        public readonly int HashCode { get; init; }
        public Data(int hashCode) { HashCode = hashCode; }
        // 使用 readonly 修饰符来声明实例成员不会修改结构的状态
        public readonly void RefreshGUI(SampleClass sample)
        {
            sample.GUI = unchecked((sample.GUI + this.GetHashCode()) * 32253);
        }
    }
}
```

<br>

#### sealed

- 应用于某个类时，`sealed` 修饰符可阻止其他类继承自该类，表示该类已密封。应用于重写方法、属性、索引器时，`sealed` 阻止其派生类重写该成员，表示该成员已密封。

```csharp
// 密封类
sealed class SealedClass{
    public int x;
    public int y;
}
// class MyDerivedC: SealedClass {} // Error
abstract class Father{
    public abstract void Func();
}
class Derived : Father{
    // 密封方法
    public sealed override void Func(){}
}
```

<br>

#### static

- 使用 `static` 修饰符可声明属于类型本身而不是属于特定对象的静态成员，每个静态成员有且只有一个副本。
- `static` 修饰符可用于声明 `static` 类。在类、接口和结构中，可以将 `static` 修饰符添加到字段、方法、属性、运算符、事件和构造函数。`static` 修饰符不能用于索引器或终结器。
- 从 C#9.0 开始，可将 `static` 修饰符添加到 Lambda 表达式或匿名方法。静态 Lambda 表达式或匿名方法无法捕获局部变量或实例状态。

```csharp
// 静态类，静态类中只能包含静态成员
static class StaticSample{
    public static int GetHashCode(object obj) => obj.GetHashCode();
}
// 静态初始化，使用尚未声明的 static 字段来初始化另一个 static 字段，在向 static 字段显式赋值之后才会定义结果。
class Test
{
    static int x = y;
    static int y = 5;
    static void Main()
    {
        Console.WriteLine(Test.x);  // 0
        Console.WriteLine(Test.y);  // 5
        Test.x = 99;
        Console.WriteLine(Test.x);  // 99
    }
}
```

<br>

#### unsafe

- `unsafe` 关键字表示不安全上下文，该上下文是任何涉及指针的操作所必需的。可在类型或成员的声明中使用 `unsafe` 修饰符，该类型或成员的整个正文范围均被视为不安全上下文。

```csharp
class Test
{
    static unsafe void Main()
    {
        char[] arr1 = "Hello World".ToCharArray();
        fixed (char* parr = &arr1[0])
        {
            string rt = FastCopy(parr, arr1.Length, 5);
            Console.WriteLine(rt);
        }
    }
    unsafe static string FastCopy(char* psrc, int len, int count)
    {
        if (count > len || count < 0)
            throw new ArgumentOutOfRangeException("count");

        char[] arr = new char[count];
        for (int i = 0; i < count; i++)
            arr[i] = psrc[i];
        return new string(arr);
    }
}
```

<br>

#### virtual

- `virtual` 关键字用于修改方法、属性、索引器或事件声明，并使它们可以在派生类中被重写。

```csharp
class Father{
    public virtual void Func() { }
}
class Derived:Father{
    public override void Func(){
        //base.Func();
    }
}
```

<br>

#### volatile

- `volatile` 关键字指示一个字段可以由多个同时执行的线程修改。可修饰：引用类型、指针类型、简单类型（`sbyte`、`byte`、`short`、`ushort`、`int`、`uint`、`char`、`float` 和 `bool`）、枚举类型、已知为引用类型的泛型类型参数、`IntPtr` 和 `UIntPtr`。其他类型（包括 `double` 和 `long`）无法标记为 `volatile`，因为对这些类型的字段的读取和写入不能保证是原子的，若要保护对这些类型字段的多线程访问，请使用 `Interlocked` 类成员或使用 `lock` 语句保护访问权限。

- `volatile` 关键字只能应用于 `class` 或 `struct` 的字段。 不能将局部变量声明为 `volatile`。

- 出于性能原因，编译器，运行时系统甚至硬件都可能重新排列对存储器位置的读取和写入。声明为 `volatile` 的字段将从某些类型的优化中排除。在多处理器系统上，易失性读取操作不保证获取由任何处理器写入该内存位置的最新值；同样，易失性写入操作不保证写入的值会立即对其他处理器可见。

```csharp
class VolatileTest<T> where T : class
{
    // volatile 字段
    public volatile int sharedStorage;
    // volatile 引用类型的泛型类型参数
    volatile T shared_data;
}
```

---
### 语句关键字

- 选择语句：`if`、`else`、`case`、`switch`。
- 迭代语句：`do`、`while`、`for`、`foreach`。
- 跳转语句：`break`、`continue`、`goto`、`return`。
- 异常处理：`throw`、`try`、`catch`、`finally`。
- 溢出检查：`checked`、`unchecked`。
- 不安全上下文：`fixed`。
- 锁定：`lock`。
- 迭代器：`yield`。

---
### 方法参数

#### params