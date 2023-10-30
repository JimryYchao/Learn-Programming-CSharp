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
volatile    // 异变类型声明
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

