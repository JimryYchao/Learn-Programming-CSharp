# CSharp 类型与变量

<!-- 值类型和引用类型是 C# 类型的两个主要类别：
值类型的变量包含类型的实例。对于值类型变量，会复制相应的类型实例。
引用类型的变量存储对其数据（对象）的引用。对于引用类型，两种变量可引用同一对象，对一个变量执行的操作会影响另一个变量所引用的对象。 -->

C# 的类型主要分为引用类型和值类型两大类，它们可以是泛型类型，并接受一个或多个类型参数。值类型和引用类型的区别在于，值类型的变量直接包含其数据，引用类型的变量存储对其数据（对象）的引用。
使用引用类型，两个变量可能引用同一个对象，因此对一个变量的操作可能影响到另一个变量引用的对象。对于值类型，每个变量都有其自己的数据副本，对一个变量的操作不可能影响另一个变量。

变量表示存储位置，每个变量都表示一个类型，用于确定可以在变量中存储的值，可以通过赋值或运算符操作改变变量的值。在获取变量前必须明确赋值。

当变量是 `ref` 或 `out` 形参时，它没有自己的存储空间，而是引用另一个变量的存储空间。实际上，引用参数的变量实际上是另一个变量的别名，而不是一个独立的变量。

C# 的类型系统是统一的，即任何类型的值都可以被视为对象，每个类型都直接或间接地派生自 `object` 类型，`object` 所有类型的最终基类。引用类型的值可以直接转换为 `object`，而值类型的值需要装箱操作转换为 `object`，反之 `object` 转换为原值类型则需要拆箱操作。

---
## 引用类型

### Class 类类型

类类型定义了一个数据结构，其中包含了数据成员（常量和字段）、函数成员（方法、属性、事件、索引器、运算符、实例构造函数、静态构造函数和终结器）和嵌套成员。类类型支持继承，这是一种派生类可以扩展和专门化基类的机制。

预定义的类类型在 C# 中具有特殊含义：
- `System.Object`：所有其他类型的最终基类。
- `System.String`：C# 字符串类型。
- `System.ValueType`：所有值类型的基类。
- `System.Enum`：所有枚举类型的基类。
- `System.Array`：所有数组类型的基类。
- `System.Delegate`：所有委托类型的基类。
- `System.Exception`：所有异常类型的基类。
- `System.Attribute`：所有特性类型的基类。

#### 类声明

使用 `class` 关键字声明类。类的声明中可以包含一组可选的 `Attribute` 特性、一组可选的类修饰符（访问修饰，`abstract`、`static`、`sealed` 等）、分部修饰 `partial`、`class` 关键字、一个可选的类型参数列表和附加的约束、类的标识符、类的主体。

```csharp
public sealed class SampleClass<T> where T : notnull
{
    // ... Members
}
```

> 类修饰符

- `unsafe` 修饰符表示允许在类的范围内使用不安全代码。
- `new` 修饰嵌套类用以隐藏同名的继承成员。
- `public`、`protected`、`internal`、`private` 修饰符控制类的可访问性。在顶级类中只能使用 `public` 和 `internal`。
- `partial` 修饰符用以声明分部类。
- `abstract` 声明抽象类，`static` 声明静态类，`sealed` 声明密封类。 

#### Generic 泛型类

类的声明中可以包含若干的类型参数，和一组可选的类型参数约束。具有类型参数的构造类未指定约束时，默认约束为 `object`。声明泛型构造类的实例时，必须指定类型参数的具体类型。

```csharp
Sample<int> S = new();
S.Value = 99;

class Sample<T> where T : struct // T 约束为结构类型
{
    public T Value { get; set; }
}
```

#### Abstract 抽象类

`abstract` 修饰符用于指示类是不完整的，并打算作为基类。抽象类与非抽象类的区别在于：
- 抽象类不能直接实例化为对象，但可以包含派生自抽象类的非抽象类实例的引用。
- 抽象类中可以可选的声明抽象函数成员（属性、方法、索引器、运算符等）。
- 抽象类无法被密封。

在抽象类中定义抽象方法，目的是将抽象方法的具体实现（`override`）延迟到派生类。抽象方法是没有实现的特殊的虚方法。在基类中也可以声明 `virtual` 虚方法并提供默认的方法实现，派生类可以重写（`override`）虚方法以扩展或重新定义派生类的行为。

```csharp
interface ISample       // 接口
{
    void Debug(string message);
}
abstract class BaseSample : ISample     // 继承接口并定义抽象方法
{
    public abstract void Debug(string mess);
    public virtual void Error(string mess) => Debug(mess);
}
internal class Sample : BaseSample      // 为抽象类和接口提供方法实现
{
    public override void Debug(string mess) => Console.WriteLine(mess);
    public sealed override void Error(string mess) => base.Error("ERROR : " + mess);
}
```

#### Sealed 密封类

使用 `sealed` 定义密封类或密封成员，密封类防止被继承，密封成员防止被派生类重写。因此密封类不能是抽象类。这些密封类也支持运行时优化，因此可以将密封类对象上的虚函数成员的调用转换为非虚调用。

```csharp
using System.Text;

PointArray ps = new((0, 0), (1, 1), (2, 2));
Console.WriteLine(ps);
// Output : (0, 0), (1, 1), (2, 2)

// 定义抽象记录防止被继承
sealed record PointArray(params (int, int)[] points)
{
    // 重写 ToString 方法
    public override string ToString()
    {
        if (points.Length > 0)
        {
            StringBuilder sb = new StringBuilder(points[0].ToString());
            foreach (var p in points[1..])
                sb.Append(", " + p);
            return sb.ToString();
        }
        else return string.Empty;
    }
}
class Point3D: PointArray;  // err: 无法派生密封类
```

#### Static 静态类

`static` 修饰符用以声明静态类。静态类不能被实例化，它的成员必须显式地包含静态修饰符（常量和嵌套类型除外）。

静态类也不能被用作类型，因此无法用作是基类、成员的组成类型、泛型类型参数或类型约束，也不能用于数组类型、`new()`、强制转换、模式匹配、`sizeof`、`default` 表达式等。

静态类只能隐式继承 `object` 且无法从其他类或接口派生，无法声明任何实例成员，也不能被任何类继承。静态类中可以声明静态扩展方法。

加载引用静态类的程序时，.NET 运行时会加载该静态类的类型信息，并在程序中首次引用类之前初始化其字段并调用其静态构造函数。静态构造函数只调用一次，在程序所驻留的应用程序域的生存期内，静态类会保留在内存中。

```csharp
public static class EnumExt
{   
    public static T? ConvertEnum<T>(this int eVal) where T : Enum
        => (T)Enum.ToObject(typeof(T), eVal);  // int 转换为 enum
    public static bool IsDefinedByEnum<T>(this int eVal) where T : Enum
        => Enum.IsDefined(typeof(T), eVal);    // 检查 enum 是否关联整数值
}
```

#### 基类规范

类声明中可以包含一个 *class_base* 规范，该规范定义了类的直接基类和类直接实现的接口。若规范中只列出了接口类型，则直接基类是 `object`。C# 类仅支持单一线性继承（不能循环依赖），因此除了 `object` 每个类都仅有一个直接基类。

类从其直接基类继承成员。若直接基类是构造类型，则必须指定构造类型的类型参数（不能是直接基类类型），也可是类本身作为构造类型的类型参数。

```csharp
class Sample<T>;
class DerivedSample : Sample<int>;
class DerivedSample<U> : Sample<U>;
```

*class_base* 中可能包含接口类型的列表，类可以直接实现给定的接口类型。

```csharp
interface ISample
{
    void Fun();
}
class Sample : ISample
{
    public void Fun() => Console.WriteLine("Hello World");
}
```

#### 类成员

一个类可包含的成员有：常量、字段、方法、属性、索引器、运算符、事件、终结器、实例构造函数、静态构造函数和嵌套类型（类、接口、结构、枚举、委托）。

```csharp
class MyClass
{
    // 实例构造函数
    public MyClass() { S_OnDestroy = static delegate { Console.WriteLine("MyClass Destroyed"); }; }
    // 常量
    public const string Version = "0.0.1";
    // 枚举
    public enum Day { workDay, weekDay }
    // 字段
    private readonly static MyClass s_default = new MyClass();
    // 静态属性
    public static MyClass Default => s_default;
    // 实例方法
    public Data CreateData(byte[] data, int id) => new Data(data, id);
    // 索引器
    public int this[int index] => index;
    // 运算符
    public static bool operator ==(MyClass left, MyClass right) => left.Equals(right);
    public static bool operator !=(MyClass left, MyClass right) => !left.Equals(right);
    // 委托
    delegate void OnDestroy();
    // 事件
    event OnDestroy S_OnDestroy;
    // 终结器
    ~MyClass() => S_OnDestroy?.Invoke();
    // 嵌套接口
    public interface INested { }
    // 嵌套类
    sealed class NestedClass : INested { }
    // 嵌套结构
    struct NestedStruct : INested { }
    // 记录
    public readonly record struct Data(byte[] data, int gui);
}
class Sample
{
    static void Main(string[] args)
    {
        MyClass.Data d1 = MyClass.Default.CreateData("Hello"u8.ToArray(), "Hello".GetHashCode());
    }
}
```

#### 类继承

类继承其直接基类的成员。继承意味着类隐式地包含其直接基类的所有成员，但基类的实例构造函数、终结器和静态构造函数除外。

继承的一些重要方面是:
- 继承是可传递的。如果 c 从 x 派生，x 派生自 A，则 c 继承 B 中声明的成员和 A 中声明的成员。
- 派生类继承其直接基类。派生类可以向其继承的成员添加新成员，但不能删除继承成员的定义。
- 实例构造函数、终结器和静态构造函数不被继承，但所有其他成员都可以继承，不管它们声明的可访问性如何。
- 派生类可以通过声明具有相同名称或签名的新成员来隐藏继承的成员。但是，隐藏继承的成员并不会删除该成员，它只是使该成员无法通过派生类直接访问。
- 类的实例包含在类及其基类中声明的所有实例字段的集合，并且存在从派生类类型到其任何基类类型的隐式转换。因此，对某个派生类实例的引用可以被视为对其任何基类实例的引用。
- 类可以声明虚方法、属性、索引器和事件，派生类可以重写这些函数成员的实现。这使类能够显示多态行为，其中函数成员调用执行的操作取决于调用该函数成员的实例的运行时类型。

<br>

### Object 对象类型

`object` 类型是 `System.Object` 在 .NET 中的别名。在 C# 的统一类型系统中，所有类型（预定义类型、用户定义类型、引用类型和值类型）都是直接或间接从 `System.Object` 继承的。可以将任何类型的值赋给 `object` 类型的变量。

引用类型的值可以直接隐式转换成 `object`。值类型的值需要经过装箱操作后隐式转换为 `object`。将 `object` 类型的变量转换为值类型的过程称为拆箱。装箱是隐式的，拆箱是显式的。装箱和拆箱的概念是类型系统 C# 统一视图的基础。

```csharp
string str = "Hello World";
object obj = str;  // implicit
string s_str = (string)str;  

int num = 10010;
obj = num;      // implicit boxing
int s_num = (int)num;  // unboxing
```

<br>

### Dynamic 动态类型

`dynamic` 类型可以引用任何对象，当运算符应用于动态类型的表达式时，操作解析被延迟到程序运行时。非法的操作应用到动态类型时，编译期间不会给出任何错误，当操作在运行时失败，将引发异常。

编译器不会对包含类型 `dynamic` 的表达式的操作进行解析或类型检查，编译器将有关该操作信息打包在一起，之后这些信息会用于在运行时评估操作，因此 `dynamic` 类型只在编译时存在，在运行时则不存在。

在大多数情况下，`dynamic` 类型与 `object` 类型的行为类似。由于 `dynamic` 和 `object` 之间存在隐式转换，两者在函数上被认为是具有相同的签名。若两者都可作为类型推断的目标类型时，类型推断的结果更倾向于 `dynamic` 而不是 `object`。

```csharp
using System;
class Program
{
    static void Main(string[] args)
    {
        ExampleClass ec = new ExampleClass();
        Console.WriteLine(ec.ExampleMethod(10));
        Console.WriteLine(ec.ExampleMethod("value"));
        /* The following line causes a compiler error because ExampleMethod takes only one argument. */
        //Console.WriteLine(ec.ExampleMethod(10, 4));

        dynamic dynamic_ec = new ExampleClass();
        Console.WriteLine(dynamic_ec.ExampleMethod(10));
        /* Because dynamic_ec is dynamic, the following call to ExampleMethod
           with two arguments does not produce an error at compile time.
           However, it does cause a run-time error. */
        //Console.WriteLine(dynamic_ec.ExampleMethod(10, 4));
    }
}
class ExampleClass
{
    static dynamic _field;
    dynamic Prop { get; set; }
    public dynamic ExampleMethod(dynamic d)
    {
        dynamic local = "Local variable";
        int two = 2;
        if (d is int)
            return local;
        else
            return two;
    }
}
// Results:
// Local variable
// 2
// Local variable
```

<br>

### String 字符串类型

`string` 类型是直接从 `object` 继承的密封类类型，它的实例表示 Unicode 字符序列。可以将 `string` 的值写成字符串字面值。关键字 `string` 是预定义类 `System.String` 的别名。

<br>

### Interface 接口类型

接口定义了一个协议，实现接口的类型必须遵循它的协议。一个接口可以继承多个基接口，一个类或结构可以实现多个接口。

使用 `interface` 关键字定义接口类型。一般而言接口不提供其成员的实现，仅用来指定实现接口的类或结构应提供实现的成员。接口可为成员定义默认实现，还可以定义 `static` 成员，以便提供常见功能的单个实现，在类型中声明的同名静态成员不会覆盖接口中声明的静态成员。从 C#11 开始，可以声明 `static abstract` 的非字段成员。

接口可以包含方法、属性、事件、索引器，也可以包含静态构造函数、静态字段、常量或运算符。接口成员默认是公共的，可以显式指定可访问性修饰符，其中 `private` 成员必须有默认实现。

```csharp
interface IMyInterface<T> where T : IMyInterface<T>
{
    // 接口静态构造
    static IMyInterface() => Console.WriteLine("Static IMyInterface()");
    // 静态成员
    static void Func() => Console.WriteLine("Static IMyInterface.Func");
    const int Zero = 0;
    abstract static void AbsFunc();
    // 实例方法
    void Action();
    // 实例方法：默认实现
    virtual void S_Action() => Console.WriteLine("IMyInterface.S_Action");
    // 属性或索引器
    int GUI { get; }
    // 运算符 static abstract/virtual 运算符
    static virtual bool operator ==(T lhs, T rhs) => lhs.Equals(rhs);
    static virtual bool operator !=(T lhs, T rhs) => !lhs.Equals(rhs);
    static abstract T operator ++(T other);
    // 
}
class MyClass : IMyInterface<MyClass>
{
    public int GUI => GetHashCode();
    int counter = 0;
    public static void AbsFunc() { }    // 实现接口静态抽象成员

    public void Action() => Console.WriteLine("MyClass.Action");

    public static MyClass operator ++(MyClass other)
    {
        other.counter++;
        return other;
    }
}
```

类的属性和索引器可以为接口中定义的属性或索引器定义额外的访问器。若接口属性或索引器使用显式接口实现而不是派生类型实现时，访问器必须匹配。

```csharp
interface ISample
{
    int Value { get; }
    string Name { get; }
}
class Sample : ISample
{
    public int Value { get; set; }  // 额外的 set 访问器

    string ISample.Name { get; /* set; // err */ }
}
```

#### 接口成员默认实现

接口成员中提供的默认实现等效于派生类型中的显式接口实现。具有默认实现的接口方法不要求其派生实现类型显式重定义，未显式重定义过的接口成员只能使用接口实例访问默认实现的成员。

接口实现类型可以重定义具有默认实现的接口成员（显式接口实现或默认实现）。

```csharp
Sample s = new Sample();
s.Func1();           // Sample.Func1
((IFace)s).Func2();  // IFace.Func2

IFace sf = s;
sf.Func1();          // Sample.Func1
sf.Func2();          // IFace.Func2

interface IFace{
    void Func1() => Console.WriteLine("IFace.Func1");
    void Func2() => Console.WriteLine("IFace.Func2");
}
class Sample : IFace{
    public void Func1() => Console.WriteLine("Sample.Func1");  // 重定义
}
```

#### 接口实现与显式接口实现

类或结构应提供在类或结构的基类列表中列出的接口的所有成员的实现。在实现类或结构中定位接口成员的实现的过程称为接口映射。

```csharp
interface ISample
{
    void Fun();
}
class Sample : ISample
{
    public void Fun() { /* ... */ }
}
```

对于多继承接口，当两个或多个不相关的基接口声明具有相同名称或签名的成员时，可能会出现歧义。

```csharp
interface ICounter
{
    void Count(int c);
    int Value { get; set; }
    void Fun();
}
interface IList
{
    int Count { get; set; }
    int Value { get; set; }
    void Fun();
}
interface IListCounter : ICounter, IList;

class Sample
{
    public void Test(IListCounter x)
    {
        x.Value = 100;  // 歧义
        x.Fun();        // 歧义

        x.Count(1);
        x.Count = 1;   // 被隐藏，
        ((IList)x).Count = 1;
        ((ICounter)x).Count(1);
    }
}
```

为消除接口之间的歧义，类或接口可以声明显式接口成员实现，用以调用限定于接口的成员。显式接口实现没有访问修饰符，它不作为实现类型的成员，只能通过接口实例调用。

```csharp
Sample logger = new Sample();
string mess = "Hello World";
logger.Print(mess);
((IDebug)logger).Print(mess);
((IError)logger).Print(mess);
/* Output
 Hello World
 DEBUG : Hello World
 ERROR : Hello World
*/

interface IDebug{
    void Print(string mess);
}
interface IError{
    void Print(string mess);
}
class Sample : IDebug, IError
{
    // 显式接口实现
    void IDebug.Print(string mess) => Console.WriteLine("DEBUG : " + mess);
    void IError.Print(string mess) => Console.WriteLine("ERROR : " + mess);
    // 默认实现
    public void Print(string mess) => Console.WriteLine(mess);
}
```

若接口函数成员具有一个参数数组，末位排序的参数数组在默认实现时可以在类或结构中附加 `params` 修饰。显式实现接口时，关联的接口成员不允许使用 `params` 数组。

```csharp
interface ISample
{
    void Fun(int[] arr);
}
class Sample : ISample
{
    public void Fun(params int[] arr) { }  // 默认实现
    void ISample.Fun(int[] arr) { }     // 显式接口实现
}
```

> 显式接口实现的目的

- 由于显式接口成员不能通过类或结构实例访问，因此它们允许将接口实现排除在类或结构的公共接口之外。
- 显式接口成员实现允许消除具有相同签名成员的歧义。若没有显式接口成员实现，类和结构就不可能具有相同签名和返回类型的接口成员的不同实现。

#### 泛型方法的实现

当泛型方法隐式实现接口方法时，为每个方法类型参数给出的约束在两个声明中应该是等效的（在任何接口类型参数被适当的类型参数替换之后），其中方法类型参数由从左到右的顺序位置标识。隐式实现的方法必须显式指定约束，而显式接口实现的方法不必（也不能）进行约束。

```csharp
interface ISample<X, Y, Z>
{
    void FunA<T>(T t) where T : X;
    void FunB<T>(T t) where T : Y;
    void FunC<T>(T t) where T : Z;
}

class C : ISample<object, C, string>
{
    public void FunA<T>(T t) { }                  // Ok
    public void FunB<T>(T t) where T : C { }      // Ok
//  public void FunC<T>(T t) where T : string { } // Error，只能接口显式实现
    void ISample<object, C, string>.FunC<T>(T t) { }
}
```

- `FunA` 不需要指定 `where T:object` 约束，因为 `object` 是所有类型参数的隐式约束。
- `FunB` 指定的约束和接口中的约束匹配。
- `FunC` 默认实现中的约束是一错误，密封类不能作为约束。约束也不能省略，默认实现的接口方法实现的约束需要匹配。因此该方法只能使用显式接口实现。 

#### 接口实现继承

类继承其基类提供的接口实现。若不能显式地重新实现接口，派生类就不能以任何方式改变它从基类继承的接口映射。

```csharp
interface ISample
{
    void Fun();
}
class Sample  : ISample
{
    public void Fun() => Console.WriteLine("Sample Fun");
}
class Derived : Sample
{
    public new void Fun() => Console.WriteLine("Derived Fun");
    static void Main(string[] args)
    {
        Sample s = new Sample();
        Derived d = new Derived();
        ISample Is = s;
        ISample Id = d;
        s.Fun();    // Sample Fun
        d.Fun();    // Derived Fun
        Is.Fun();   // Sample Fun
        Id.Fun();   // Sample Fun
    }
}
```

若当接口方法映射到类中的虚方法（使用默认实现）时，派生类则可能重写虚方法并更改接口的实现。

```csharp
interface ISample
{
    void Fun();
}
class Sample  : ISample
{
    public virtual void Fun() => Console.WriteLine("Sample Fun");
}
class Derived : Sample
{
    public override void Fun() => Console.WriteLine("Derived Fun");
    static void Main(string[] args)
    {
        Sample s = new Sample();
        Derived d = new Derived();
        ISample Is = s;
        ISample Id = d;
        s.Fun();    // Sample Fun
        d.Fun();    // Derived Fun
        Is.Fun();   // Sample Fun
        Id.Fun();   // Derived Fun
    }
}
```

#### 接口重实现

显式实现的接口则无法提供方法重写，派生类也无法重写接口映射，除非在子类中添加接口到 *class_base* 以进行接口的重实现。派生类的公共成员声明和显式接口成员声明参与重新实现接口的接口映射过程。

```csharp
interface ISample
{
    void Fun();
}
class Sample : ISample
{
    void ISample.Fun() => Console.WriteLine("Sample Fun");
}
class Derived : Sample, ISample
{
    public void Fun() => Console.WriteLine("Derived Fun");  // 接口重新映射
    static void Main(string[] args)
    {
        Sample s = new Sample();
        Sample sd = new Derived();
        Derived d = new Derived();
        ISample Is = s;
        ISample Isd = sd;
        ISample Id = d;
        Is.Fun();   // Sample Fun
        Isd.Fun();  // Derived Fun
        Id.Fun();   // Derived Fun
    }
}
```

#### 抽象类与接口

与非抽象类一样，抽象类应提供在类的基类列表中列出的所有接口成员的实现。但是，允许抽象类将接口方法映射到抽象方法上。显式接口实现的成员不能是抽象的。

```csharp
interface ISample
{
    void Fun();
    int Value { get; }
}
abstract class AbSample : ISample
{
    public abstract int Value { get; }
    void ISample.Fun() => Console.WriteLine(Value);
}
```

#### 接口的静态抽象和虚拟成员

- 从 C#11 开始，接口可以声明除字段之外的所有成员类型的 `static abstract` 和 `static virtual` 成员。接口指定抽象静态成员，然后要求类和结构为接口抽象静态成员提供显式或隐式实现。接口静态抽象或虚拟成员只能从受接口约束的类型参数或派生实现类型中访问。
- 接口中声明的 `static virtual` 和 `static abstract` 方法没有类似于类中声明的 `virtual` 或 `abstract` 方法的运行时调度机制。相反，编译器使用编译时可用的类型信息，即调用基（编译时）类型的静态方法。`static virtual` 方法几乎完全是在泛型接口中声明的。

```csharp
interface IFace
{
    static abstract void Func();
    static abstract event Action E;
    static abstract object Proper { get; set; }
}
interface IFace<T> where T : IFace<T>
{
    static abstract void Func();
    static abstract event Action E;
    static abstract T P { get; set; }
    static abstract object Proper { get; set; }
    static abstract T operator +(T l, T r);
    static abstract bool operator ==(T l, T r);
    static abstract bool operator !=(T l, T r);
    static abstract implicit operator T(string s);
    static abstract explicit operator string(T t);
}
```

- 访问接口静态抽象成员

```csharp
Console.WriteLine(Sample.Instance);
Sample.Func();

interface IInstance<T> where T : IInstance<T>
{
    static abstract void Func();
    static abstract T Instance { get; }
}
class Sample : IInstance<Sample>
{
    public static Sample Instance { get; } = new Sample();
    public static void Func() => Console.WriteLine("Sample.Func");
}
```

<br>

### Array 数组类型

数组是一种包含零个到多个变量的数据结构，可以通过计算索引访问这些变量。数组中的变量亦称为元素，它们具有相同的类型，数组的元素可以是任何类型。类型 `System.Array` 是所有数组类型的抽象基类型。

数组具有确定与每个元素相关联的索引的秩，数组的秩也被称为数组的维数。秩为一的数组称为一维数组，秩大于一的数组称为多维数组，特定尺寸的多维数组通常被称为二维数组、三维数组等。

数组的每个维度都有一个相关联的长度，该长度是一个大于等于零的整数。维度长度不是数组类型的一部分，而是在运行时创建数组类型的实例时建立的。维度的长度决定了该维度索引的有效范围：对于长度为 N 的维度，索引的范围可以从 0 到 N-1。数组中元素的总数是数组中每个维度长度的乘积。如果数组的一个或多个维度的长度为零，则称该数组为空。

```csharp
// 几种声明数组的形式
int[] arr1 = new int[10];        // 元素均初始化为 0
int[] arr2 = { 0, 1, 2, 3 };     // 初始化构造
int[] arr3 = new int[] { 1, 2, 3 };
int[] arr4 = new int[3] { 1, 2, 3 };
int[] arr5 = new[] { 1, 2, 3 };
int[] arr6 = [10, 20, 30];       // 集合表达式

// 多维数组的声明
int[] a1 = new int[10];             // 一维数组
int[,] a2 = new int[10, 5];         // 二维数组
int[,,] a3 = new int[10, 5, 2];     // 三维数组
```

数组的元素类型本身也可以是数组类型，这类数组不同于多维数组，被称为交错数组。

```csharp
int[][] pascals = 
{
    new int[] {1},
    new int[] {1, 1},
    new int[] {1, 2, 1},
    new int[] {1, 3, 3, 1}
};
```

#### 数组的访问

```csharp
// 一维数组
int[] arr = [1, 2, 3, 4, 5, 6, 7, 8, 9];
for (int i = 0; i < arr.Length; i++)
    arr[i] *= arr[i];
Console.WriteLine(arr.Rank); // 数组的秩
Console.WriteLine(string.Join(",", arr));

// 多维数组
int[,] Matrix3_4 = new int[3, 4] {
    { 31, 32, 33, 34 },
    { 24, 25, 26, 27 },
    { 19, 18, 17, 16 }
};
Console.WriteLine(Matrix3_4.Rank);
for (int L = 0, r = 0, i = 0; L < Matrix3_4.Length; L++)
{
    (r, i) = Math.DivRem(L, Matrix3_4.GetLength(0) + 1);
    Console.WriteLine($"Matrix3_4[{r},{i}] = {Matrix3_4[r, i]}");
}
```

#### 数组和泛型集合接口

一个单维数组 `T[]` 实现了 `System.Collections.Generic.IList<T>` 接口，因此存在从 `T[]` 到 `IList<T>` 及其基接口的隐式转换。同样 `T[]` 也实现了 `System.Collections.Generic.IReadOnlyList<T>` 接口。

若 `S` 到 `T` 存在隐式转换，则 `S[]` 也可以隐式转换为 `IList<T>` 或 `IReadOnlyList<T>`。

```csharp
class Test
{
    static void Main()
    {
        string[] sa = new string[5];
        object[] oa1 = new object[5];
        object[] oa2 = sa;

        IList<string> lst1 = sa;  // Ok
        IList<string> lst2 = oa1; // Error, cast needed
        IList<object> lst3 = sa;  // Ok
        IList<object> lst4 = oa1; // Ok

        IList<string> lst5 = (IList<string>)oa1; // Exception
        IList<string> lst6 = (IList<string>)oa2; // Ok

        IReadOnlyList<string> lst7 = sa;        // Ok
        IReadOnlyList<string> lst8 = oa1;       // Error, cast needed
        IReadOnlyList<object> lst9 = sa;        // Ok
        IReadOnlyList<object> lst10 = oa1;      // Ok
        IReadOnlyList<string> lst11 = (IReadOnlyList<string>)oa1; // Exception
        IReadOnlyList<string> lst12 = (IReadOnlyList<string>)oa2; // Ok
    }
}
```

<br>

### Delegate 委托类型

委托是引用一个或多个方法的数据结构。委托的声明定义了一个从 `System.Delegate` 派生的类。委托实例封装了一个调用列表，该列表是一个或多个方法的列表，每个方法都是一个可调用实体。对于实例方法，可调用实体由实例和该实例上的方法组成。对于静态方法，可调用实体仅由一个方法组成。调用委托时，将导致调用这个列表上的每个可调用实体。

在 C/C++ 中，与委托最接近的等效项是函数指针，但是函数指针只能引用静态函数，而委托可以引用静态和实例方法，并且委托还存储了对方法入口点的引用和对调用方法的对象实例引用。

#### 委托的声明

委托类型的声明与方法签名相似，它有一个返回值和任意数目任意类型的参数。委托类型是一种可用于封装命名方法或匿名方法的引用类型，是面向对象的、类型安全的和可靠的。

使用 `delegate` 关键字声明委托类型，编译器将委托相关的操作代码的调用映射到 `System.Delegate` 和 `System.MulticastDelegate` 类成员的方法调用。必须使用具有兼容返回类型和输入参数的方法或 Lambda 表达式实例化委托。在实例化委托时，可以将委托的实例与任何兼容的方法相关联，并可以通过委托实例调用方法。

将方法作为参数进行引用的能力使委托成为定义回调方法的理想选择。委托类似于 C++ 函数指针，但委托面向对象，会同时封装对象实例和方法，因此委托允许将方法作为参数进行传递。

```csharp
MessageDelegate debug = Console.WriteLine;
if (debug is MulticastDelegate or Delegate)
    debug("Hello World >>> " + debug.Method.Name);

public delegate void MessageDelegate(string message);
public delegate int AnotherDelegate(int num1, int num2);
```

在 .NET 中，`System.Action`、`System.Func`、`System.Predicate` 类型为许多常见委托提供泛型定义。

```csharp
Action<string> MessagePrint = null;
// 方法
void Print(string mess) => Console.WriteLine("Function : " + mess);
MessagePrint += Print; 
// delegate 匿名方法
MessagePrint += 
delegate (string mess)
{
    Console.WriteLine("Delegate : " + mess);
};
// Lambda 表达式
MessagePrint += message => Console.WriteLine("Lambda : " + message);

// 委托调用
MessagePrint("Hello World!"); 
MessagePrint?.Invoke("Hello World!"); // 等效写法
```

> delegate 匿名方法

- `delegate` 运算符创建一个可以转换为委托类型的匿名方法。匿名方法可以转换为 `System.Action` 和 `System.Func<TResult>` 等类型，用作许多方法的参数。

```csharp
// delegate 匿名方法
Func<int, int, int> sum = delegate (int a, int b) { return a + b; };
// lambda 匿名方法
var _Debug = (string message) => MessagePrint?.Invoke(message);
```

#### 多播委托

可以通过使用 `+` 组合多个委托对象分配到一个委托实例中。多播委托中包含已分配委托列表，此多播委托被调用时会按照添加的先后顺序依次调用列表中的委托。`-` 用于从多播委托中删除组件委托。

一个委托可以多次出现在调用列表中，这样的委托被删除时，总是删除调用列表的最后一个。

`+=` 可以将方法或匿名方法构造为委托对象并分配到多播委托中，`-=` 则表示从多播委托中移除该方法的委托实例。`+`、`-` 运算符支持委托对象和方法组之间的运算。

删除委托对象时，若右操作数是 Lambda 表达式或匿名方法（非匿名类型）时，此操作无效。

```csharp
class Sample
{
    delegate void SampleDelegate();
    private static void Function_1() => Console.WriteLine("Function_1");
    private static void Function_2() => Console.WriteLine("Function_2");

    static SampleDelegate Action_1 = delegate { Console.WriteLine("Action_1"); };
    static SampleDelegate Action_2 = delegate { Console.WriteLine("Action_2"); };

    static SampleDelegate Lambda_1 = () => Console.WriteLine("Lambda_1");
    static SampleDelegate Lambda_2 = () => Console.WriteLine("Lambda_2");

    static void Main(string[] args)
    {
        SampleDelegate MultoDel_1 = Function_1;
        MultoDel_1 += Action_1;
        MultoDel_1 += Lambda_1;
        MultoDel_1?.Invoke();
        // Function_1   Action_1    Lambda_1
        var MultoDel_2 = Action_2 + Function_2 + Lambda_2;
        MultoDel_2?.Invoke();
        // Action_2     Function_2  Lambda_2
        MultoDel_1 = MultoDel_1 + MultoDel_2 - Function_1 - Lambda_2;
        MultoDel_1?.Invoke();
        // Action_1     Lambda_1    Action_2    Function_2
    }
}
```

<br>

### 引用类型空注释

由于在可为 null 的感知上下文选择加入了代码，可以使用可为 null 的引用类型、null 静态分析警告和空包容运算符（`!`）是可选的语言功能。在可为 null 的感知上下文中：
  - 引用类型 `T` 的变量必须用非 `null` 值进行初始化，并且不能为其分配可能为 `null` 的值。
  - 引用类型 `T?` 的变量可以用 `null` 进行初始化，也可以分配 `null`，但在取消引用之前必须对照 `null` 进行检查。
  - 类型为 `T?` 的变量 `m` 在应用空包容运算符时被认为是非空的，如 `m!` 中所示。

类型为 `T` 的变量和类型为 `T?` 的变量由相同的 .NET 类型表示。可为 null 的引用类型不是新的类类型，而是对现有引用类型的注释。编译器使用这些注释来帮助你查找代码中潜在的 null 引用错误。不可为 null 的引用类型和可为 null 的引用类型在运行时没有区别。

可以通过两种方式控制可为 null 的上下文。在项目级别，可以添加 `<Nullable>enable</Nullable>` 项目设置。在单个 C# 源文件中，可以添加 `#nullable enable` 来启用可为 null 的上下文。在 .NET 6 之前，新项目使用默认值 `<Nullable>disable</Nullable>`。从 .NET 6 开始，新项目将在项目文件中包含 `<Nullable>enable</Nullable>` 元素。

---
## 值类型

值类型包含结构类型和枚举类型。C# 提供了一组预定义的结构类型，被称为简单类型，简单类型可以通过关键字来识别。

引用类型可以包含 `null` 值，但是值类型只有是为可为空的值类型时，才能包含 `null` 值。对于每一个非空值类型，都有一个对应的可空值类型。

<br>

### System.ValueType 类型

所有的值类型都隐式继承类 `System.ValueType`，任何类型都不可能从值类型派生，所有值类型都是隐式密封的。

<br>

### 默认构造函数

所有值类型都隐式声明了一个公共无参非静态的 *默认构造函数*，该函数返回一个零初始化的实例作为值类型的 *默认值（`default`）* 。用户定义的结构类型可以显式声明一个公共无参的实例构造函数和其他参数化的实例构造函数。

值类型的默认值是由全零位模式产生的值：
  - 对于 `sbyte`、`byte`、`short`、`ushort`、`int`、`uint`、`long`、`ulong` 的默认值为 0.
  - 对于 `char` 的默认值为 `\x0000`。
  - 对于 `float` 的默认值为 `0.0F`。
  - 对于 `double` 的默认值为 `0.0d`。
  - 对于 `decimal` 的默认值为 `0.0m`。
  - 对于 `bool` 的默认值为 `false`。
  - 对于 `enum` 的类型 `E` 默认值为 `0` 并转换为类型 `E`。
  - 对于 `struct`，其默认值为所有的值类型字段设置为默认值和所有的引用类型字段设置为 `null` 值。
  - 对于可空值类型的默认值为其 `HasValue` 属性为 `false` 的实例。

<br>

### Simple 简单类型

C# 提供了一组预定义的结构类型（简单类型），它们可以通过关键字来标识，这些关键字是这些预定义类型的别名：
- `sbyte` 对应于 `System.Sbyte`。
- `byte` 对应于 `System.Byte`。
- `short` 对应于 `System.Int16`。
- `ushort` 对应于 `System.UInt16`。
- `int` 对应于 `System.Int32`。
- `uint` 对应于 `System.UInt`。
- `long` 对应于 `System.Int64`。
- `ulong` 对应于 `System.UInt64`。
- `char` 对应于 `System.Char`。
- `float` 对应于 `System.Single`。
- `double` 对应于 `System.Double`。
- `decimal` 对应于 `System.Decimal`。
- `bool` 对应于 `System.Boolean`。

简单类型与其他结构类型的不同之处在于：
- 大多数简单类型允许通过文本字面量来创建值。
- 当表达式的操作数都是简单类型常量时，编译器会在编译时对表达式求值。
- 可以通过 `const` 声明简单类型的常量，其他类型只能通过 `static readonly` 起到类似的效果。
- 涉及简单类型的转换可以参与其他结构类型定义的转换运算符的求值，但是用户定义的转换运算符不能参与其他用户定义的转换运算符的求值。


### Integer 整数类型

C# 支持 9 种整数类型：`sbyte`、`byte`、`short`、`ushort`、`int`、`uint`、`long`、`ulong`、`char`，所有的有符号整数都使用二进制补码格式表示：
- `sbyte`：有符号 8 位整数，介于 -128 ~ 127 之间。
- `byte`：无符号 8 位整数，介于 0 ~ 255 之间。
- `short`：有符号 16 位整数，介于 -32768 ~ 32767 之间。
- `ushort`：无符号 16 位整数，介于 0 ~ 65535 之间。
- `int`：有符号 32 位整数，介于 -2147483648 ~ 2147483647 之间。
- `uint`：无符号 32 位整数，介于 0 ~ 4294967295 之间。
- `long`：有符号 64 位整数，介于 -9223372036854775808 ~ 9223372036854775807 之间。
- `ulong`：无符号 64 位整数，介于 0 ~ 18446744073709551615 之间。
- `char`：表示值介于 0 ~ 65535 之间的 16 位无符号整数。`char` 类型的可能值集与 Unicode 字符集相对应。

整数类型的一元或二元运算符总是使用 `int`、`uint`、`long`、`ulong` 精度的整型进行运算。`char` 归类于整型，但是没有其他整数类型到 `char` 类型的隐式转换。`char` 的常量应写成字符形式或 `(char)integer` 的强制转换形式。

`checked` 和 `unchecked` 运算符和语句用于控制整型算数运算和转换的溢出检查。`checked` 上下文中整型运算溢出会引发 `System.OverflowException` 异常；`unchecked` 上下文中将忽略溢出，并且不会丢弃任何不适合目标类型的高序位。  

```csharp
// 有符号整数
sbyte   _int8 = -8;         // -2^7 ~ 2^7-1
short   _int16 = -16;       // -2^15 ~ 2^15-1
int     _int32 = -32;       // -2^31 ~ 2^31-1
long    _int64 = -64L;      // -2^63 ~ 2^63-1
// 无符号整数
byte    _uint8 = 8;         // 0 ~ 2^8-1
ushort  _uint16 = 16;       // 0 ~ 2^16
uint    _uint32 = 32;       // 0 ~ 2^32
ulong   _uint64 = 64l;      // 0 ~ 2^64
```

> 不同进制数表示

```csharp
var decimalLiteral = 42;            // 十进制
var hexLiteral = 0x2A;              // 十六进制 0x, 0X
var binaryLiteral = 0b_0010_1010;   // 二进制 0b, 0B
```

> 整数文本后缀

```csharp
long _long_64 = 0xffffL;            // long, ulong 长整数后缀 L, l
uint _uint_32 = 32u;                // 无符号整数后缀 U, u
ulong _ulong_64 = 0xffffuL;         // 无符号长整数后缀 ul, lu, Ul, lU, UL, LU 
```

#### char 字符类型

`char` 类型用来表示 Unicode UTF-16 字符，类型支持比较、相等、增量和减量运算符。算数和逻辑位运算得到 `int` 结果。

`char` 的值可以用字符文本、Unicode 转义序列（`\uUUUU`，四位）、十六进制转义序列（`\xXX`）表示。`char` 可以隐式转换为其它包含数值类型，或显式转换为未包含数值类型。其他类型则无法隐式转换为 `char`。

```csharp
char Ch = 'H';
char Ch_x = '\x65';
char Ch_u = '\u00ff';
```

#### 本机大小的整数

```csharp
// 本机大小的整数
nint    _IntPtr = new IntPtr();     // 有符号本机 32 位或 64 位整数
nuint   _UIntPtr = new UIntPtr();   // 无符号本机 32 位或 64 位整数
```

- 本机大小的整数类型具有特殊行为，因为存储是由目标计算机上的自然整数大小决定的。

```csharp
// sizeof, 获取本机整数的大小需要在不安全的上下文中
unsafe
{
    Console.WriteLine($"size of nint = {sizeof(nint)}");
    Console.WriteLine($"size of nuint = {sizeof(nuint)}");
}
// 本机整数的极值
Console.WriteLine($"nint.MinValue = {nint.MinValue}");
Console.WriteLine($"nint.MaxValue = {nint.MaxValue}");
Console.WriteLine($"nuint.MinValue = {nuint.MinValue}");
Console.WriteLine($"nuint.MaxValue = {nuint.MaxValue}");
// 数值转换：没有适用于本机大小整数文本的直接语法，可以改为使用其他整数值的隐式或显式强制转换
// 本机整数支持内置数值转换规则
nint a = (nint)otherInteger;
```

<br>

### Floating-point 浮点类型

C# 支持两种 IEEE-754 格式表示的 `float` （32 位单精度）和 `double`（64 位双精度）浮点数，它们的值域包含：
- 正零和负零，常规算数中和简单值 0 行为相同，在一些特殊的操作中区分零的正负。
- 正无穷（`1.0/0.0`）与负无穷（`-1.0/0.0`）。
- 非数字值，NaN 是由无效的浮点运算产生的，如 `0.0/0`。
- `float` 可以表示具有大约 1.5 × 10⁻⁴⁵ ~ 3.4 × 10³⁸ 的值，精度为 7 位。
- `double` 可以表示具有大约 5.0 × 10⁻³²⁴ ~ 1.7 × 10³⁰⁸ 的值，精度为 15 ~ 16 位。

```csharp
float F_32 = 3.1415f;          // f 或 F 后缀，6~9 位精度
double D_64 = 3.1415;          // 隐式 d 或 D 后缀，15~17 位精度
// 无穷定义
var n_inf = double.NegativeInfinity;    // 负无穷  
var p_inf = double.PositiveInfinity;    // 正无穷
// 非数值
var nan = 0.0/0;   // 非数值
```

如果二元运算符的任一操作数是浮点类型，则应用数值提升，并以 `float` 或 `double` 执行操作：
- 浮点运算永远不会产生异常，在特殊情况下，浮点运算产生零、无穷大或 NaN。
- 浮点运算的结果四舍五入到目标格式中最接近的可表示值。
- 若浮点运算结果的大小对于目标格式来说太小，则将运算结果变成正零或负零。
- 若浮点运算结果的大小对于目标格式来说太大，则将运算结果变为正无穷大或负无穷大。
- 若浮点操作无效，则该操作的结果为 NaN；若其中一个操作数为 NaN 时，最终运算结果也是 NaN。

<br>

### Decimal 十进制数值类型 

`decimal` 类型是一种 128 位的数据类型，适合于金融和货币计算。十进制类型可以表示的范围至少为 -7.9 × 10⁻²⁸ ~ 7.9 × 10²⁸, 至少有 28 位精度。

```csharp
decimal M_128 = 3.1415m;       // m 或 M 后缀，28~29 位精度
```

若二元运算符的任一操作数是 `decimal` 类型，则应用数值提升，并以 `double` 执行该操作：
- 对于 `decimal` 类型的值进行操作的结果是计算精确的结果，并舍入以适应表示。
- 结果四舍五入到最接近的可表示值。舍入可能会从非零值产生零值。
- 若十进制算数运算产生的结果对于 `decimal` 太大，则会产生 `System.OverflowException` 异常。
- 不同于 `float` 和 `double`，`decimal` 可以精确表示 0.1 的值。

#### decimal 和浮点类型 

`decimal` 比 `float` 和 `double` 具有更高的精度，所以可能比浮点类型具有更小的范围。从浮点类型转换为 `decimal` 类型可能产生溢出异常，反过来 `decimal` 转换为浮点类型可能会损失精度或溢出异常。因此它们之间不存在隐式转换，可以显式强制转换。

```csharp
decimal dm = 1.0m;
double d = (double)dm;

float f = 3.14f;
decimal df = (decimal)f;
```

<br>

### Bool 布尔类型

`bool` 类型表示布尔逻辑值，值可以为 `true` 或 `false`。布尔类型和其他类型之间不存在标准转换，不能用布尔值代替整型。

在 C/C++ 中，零的整型或浮点型值、空指针可以被转换为布尔值 `false`，非零整型或浮点型、非空指针可以被转换为布尔值 `true`。在 C# 中，这种转换是通过显式地将整数或浮点值与零进行比较，或者显式地将对象引用与 `null` 进行比较来完成的。
 
可以使用 `Convert` 类进行布尔转换：非零数值转换为 `true`，零转换为 `false`；字符串转换时忽略大小写。
 
```csharp
bool t = true;
bool f = false;
bool rt = Convert.ToBoolean("True");    // true
bool rt2 = Convert.ToBoolean(0);        // false
```

<br>

### Enumeration 枚举类型

枚举类型是声明一组命名常量的值类型。每个枚举类型都有一个相应的整型作为其基础类型，该整数类型可以是 `byte`、`sbyte`、`short`、`ushort`、`int`、`uint`、`long`、`ulong`。默认的基础类型是 `int`。

使用 `enum` 定义枚举类型，并声明其枚举成员的命名常量和关联常数值，多个枚举成员可以共享相同的关联值。

关联值可以隐式或显式赋值。若枚举成员是枚举类型中的第一个枚举成员，在为显式赋值时，其关联值为 0。其他未显式赋值的枚举成员的关联值是前一个成员的值加 1。

```csharp
enum Color : uint
{
    Red = 1,
    Green = 2,
   // Blue = -3, // err 
    Blue = 3,
    Max = Blue
}
```

可以使用枚举类型，通过一组互斥值或选项组合来表示选项。若要表示选项组合，请将枚举类型定义为位标志。

```csharp
public enum Days
{
    None      = 0b_0000_0000,  // 0
    Monday    = 0b_0000_0001,  // 1
    Tuesday   = 0b_0000_0010,  // 2
    Wednesday = 0b_0000_0100,  // 4
    Thursday  = 0b_0000_1000,  // 8
    Friday    = 0b_0001_0000,  // 16
    Saturday  = 0b_0010_0000,  // 32
    Sunday    = 0b_0100_0000,  // 64
    Weekend   = Saturday | Sunday
}
```

#### System.Enum

`System.Enum` 类型是所有枚举类型的抽象基类。可在基类约束中使用 `System.Enum`（称为枚举约束），以指定类型参数为枚举类型。对于任何枚举类型，都存在分别与 `System.Enum` 类型的装箱和取消装箱的相互转换。

对于任何枚举类型，枚举类型与其基础整型类型之间存在显式转换。使用 `Enum.IsDefined` 方法来确定枚举类型是否包含具有特定关联值的枚举成员。

```csharp
Console.WriteLine(1.ConvertEnum<Season>());     // Summer
Console.WriteLine((-1).IsDefinedByEnum<Season>());  // False

Season val = (Season)1;     // 强制转换

public enum Season
{
    Spring,
    Summer,
    Autumn,
    Winter
}
public static class EnumExt
{
    public static T ConvertEnum<T>(this int eVal) where T : Enum
        => (T)Enum.ToObject(typeof(T), eVal);
    public static bool IsDefinedByEnum<T>(this int eVal) where T : Enum 
        => Enum.IsDefined(typeof(T), eVal);
}
```

<br>

### ValueTuple 元组类型

元组类型表示具有可选名称和单独类型的有序、固定长度的值序列。元组类型写为 `(T1 I1, ... , Tn In)`，n ≥ 2，标识符 `I1, ..., In` 是可选的元组元素名。

`(T1, ..., Tn)` 这种构造语法是 `System.ValueTuple<...>` 的简写，它是一组泛型结构类型。能够直接表示 2 ~ 7 数目之间的任意元组类型。数量大于 7 的元组用泛型 `ValueTuple<T1, ..., T7, TRest>` 表示，其中 `TRest` 包含一个嵌套其余元素的值，由另一个 `ValueTuple<...>` 表示。

元组的值可以通过元组表达式创建，也可以是 `new ValueTuple<...>`。

```csharp
using Point = (int, int);

Point p1 = (1, 2);  // 元组表达式
Point p2 = new ValueTuple<int, int>(1, 2);
Point p3 = new(1, 2);
Point p4 = ValueTuple.Create(1, 2);
```

元组类型中的元素名称必须不同，未提供显式名称的元素根据其元素顺序命名为 `ItemX`，`X` 是从 1 开始的序列。可选元素名不在 `ValueTuple<...>` 类型中，并且不会存在元组值得运行时表示形式中。`ItemX` 的显式定义名称只能用在相应的 `X` 位置。

```csharp
using Point = (int x, int y);

Point p1 = (1, 2);
Console.WriteLine(p1.Item1);  // 使用默认的元素名称
Console.WriteLine(p1.y);    // 使用显式提供的元素名称

(int Item1, string Item2) pair1 = (5, "Five");  
(int Item2, string) pair2 = (5, "Five");  // err, Item2 只能用于 2 位置
```

元组类型支持相等运算符 `==` 和 `!=`。

```csharp
(double x, double y) Point1 = new(1, 1);
(double, double) Point2 = new();
Point2.Item1 = Point1.x;
if(Point2 != Point1)
    Console.WriteLine(Point2);  // (1,0)
```

元组最常见的用例之一是作为方法返回类型，可以将方法结果分组为元组返回类型，而不是定义 `out` 方法参数。可以使用赋值运算符 `=` 在单独的变量中析构元组实例，析构元组时可以使用弃元。

```csharp
var (X, Y, _) = GetRandomPoint();   // 析构元组
Console.WriteLine("The Point2D = ({0},{1})", X, Y);
static (int, int, int) GetRandomPoint()
{
    Random rand = new Random(DateTime.Now.Millisecond);
    return (rand.Next(-128, 128), rand.Next(-128, 128), rand.Next(-128, 128));
}
```

#### 解构函数

可以在 `struct`、`class`、`record`、`interface` 中声明名为 `Deconstruct` 的方法，该方法返回 `void`，且拥有至少两个 `out` 参数。该方法为这些类型提供解构为元组的功能支持，`Deconstruct` 方法支持重载。

在声明主构造函数的记录中，编译器会为其自动生成一个 `Deconstruct` 方法，其参数列表对照位置记录中的位置参数。

```csharp
 // 为 class 定义解构函数
var (fname, lname) = new Person("Hello", "World");
class Person(string firstName, string lastName)
{
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;
    public void Deconstruct(out string firstName, out string lastName)
        => (firstName, lastName) = (FirstName, LastName);
}

// 解构一个位置记录
var (fname, lname) = new Person("Hello", "World");
record Person(string firstName, string lastName);
```

解构函数也可以是扩展方法，可以为指定类型提供额外的 `Deconstruct` 扩展方法。

```csharp
public static void Deconstruct(this <Type> destTypeObj, out <Type1> val, out <Type2> val2[, out < Type3 > val3...]0) { }

var (<Type1> rt1, <Type2> rt2[, ...]) = destTypeObj;
```

> 系统类型的扩展方法

为了方便起见，某些系统类型提供 `Deconstruct` 方法。例如 `System.Collections.Generic.KeyValuePair<TKey,TValue>` 类型提供此功能，循环访问 `Dictionary` 时，每个元素都是 `KeyValuePair<TKey,TValue>`。

```csharp
Dictionary<string, int> snapshotCommitMap = new(StringComparer.OrdinalIgnoreCase)
{
    ["https://github.com/dotnet/docs"] = 16_465,
    ["https://github.com/dotnet/runtime"] = 114_223,
    ["https://github.com/dotnet/installer"] = 22_436,
    ["https://github.com/dotnet/roslyn"] = 79_484,
    ["https://github.com/dotnet/aspnetcore"] = 48_386
};
foreach (var (repo, commitCount) in snapshotCommitMap)
    Console.WriteLine($"The {repo} repository had {commitCount:N0} commits as of November 10th, 2021.");
```

<br>

### Nullable-value 可为空的值类型

可空值类型 `T?` 表示其基础值类型 `T` 的所有值及额外的 `null` 值，其默认值为 `null`。任何可为空的值类型都是泛型 `System.Nullable<T>` 结构的实例。每个可空值类型包含两个只读属性 `HasValue` 和 `Value`，当 `HasValue` 为 `false` 时，`Value` 为 `null` 值。

访问可空值类型变量的 `Value` 属性的过程称为展开（*unwrapping*）；为给定值类型创建可空值类型的过程称为包装（*wrapping*） 

```csharp
int num = 0;
int? n_num = num;  // wrapping

int? nullNumble = 10010;
// num = (int)nullNumble; // 可能出现 null 引用异常
num = nullNumble ?? default;  // unwrapping
Console.WriteLine(num);  // 10010
```

#### 可空值类型的 null 检查

可以将 `is` 运算符与类型模式结合使用，既检查 null 的可空值类型的实例，又检索基础类型的值。或使用 `Nullable<T>.HasValue` 指示可为空值类型的实例是否有基础类型的值，如果 `HasValue` 为 `true`，则 `Nullable<T>.Value` 获取基础类型的值。也可以使用空合并操作符将可空类型转换为其基础类型。

```csharp
void NullCheck<T>(T? n) where T : struct
{
    // 几种空判定s
    if (n != null)
        Console.WriteLine("The input is " + n.Value);
    if (n is int temp)
        Console.WriteLine("The input is " + temp);
    if (n.HasValue)
        Console.WriteLine("The input is " + n.Value);
    Console.WriteLine("The input is " + (n ?? default(T)));
}
```

> 确定可为空的值类型

```csharp
Console.WriteLine($"int? is {(IsNullable(typeof(int?)) ? "nullable" : "non nullable")} value type");
Console.WriteLine($"int is {(IsNullable(typeof(int)) ? "nullable" : "non-nullable")} value type");

bool IsNullable(Type type) => Nullable.GetUnderlyingType(type) != null;

// Output:
// int? is nullable value type
// int is non-nullable value type
```

#### 可空值类型的运算

可为空值类型拥有预定义的一元或二元运算符时，若至少存在一个 `null` 值时，运算结果也为 `null`。对于比较运算符 `<`、`>`、`<=` 和 `>=`，如果一个或全部两个操作数都为 `null`，则结果为 `false`。`null == null` 返回 `true`。

<br>

### Struct 结构类型

结构体类似于类，因为它们表示可以包含数据成员和函数成员的数据结构。与类不同的是，结构是值类型，不需要堆分配。结构类型的变量直接包含该结构的数据，而类类型的变量包含对数据的引用，被称为对象。

结构类型可以声明常量、字段、方法、属性、事件、索引器、运算符、实例构造函数、静态构造函数和嵌套类型。使用 `struct` 关键字定义结构类型。

```csharp
public struct Coords(double x, double y)
{
    public double X => x;
    public double Y => y;
    public override string ToString() => $"({X}, {Y})";
}
// 记录结构声明
public record struct Data<T> where T : class
{
    public T Bindings;
    public readonly int GUI { get; init; }
}
```

#### readonly 只读结构

可以使用 `readonly` 修饰符来声明结构类型为不可变，它所有的实例字段都应声明为 `readonly` 的。它的实例属性不能包含 `set` 访问器，也不能声明字段形式的事件。

当一个只读结构的实例被传递给一个方法时，它的 `this` 被视为一个 `in` 参数，它禁止对任何实例字段进行写访问（构造函数除外）。

```csharp
readonly struct Sample
{
    public readonly string FirstName;
    public readonly string LastName;
    public string Name => FirstName + " " + LastName;
}
```

#### ref 引用结构

`ref` 修饰声明的结构类型被称为引用结构（*ref struct*），它的实例在执行堆栈上分配，不能转义到托管堆。在引用结构中可以声明 `ref` 修饰的实例字段，且不能从其安全上下文中复制出去。

`ref struct` 有以下限制：
  - `ref struct` 不能是数组的元素类型、元组的元素类型、不能实现接口、不能是类型参数、不能在迭代器中使用。
  - `ref struct` 不能是类或非 `ref struct` 的字段的声明类型。
  - `ref struct` 不能被装箱为 `System.ValueType` 或 `System.Object`。
  - `ref struct` 变量不能由 Lambda 表达式或本地函数捕获。
  - `ref struct` 变量不能在 `async` 方法中使用，但可以在同步方法中使用 `ref struct` 变量。例如，在返回 `Task` 或 `Task<TResult>` 的同步方法中。
  - `ref struct` 中不能声明异步实例方法、迭代器实例方法。

```csharp
interface ISample { }
interface ISample<T> { }
public ref struct Point(double x, double y) //:ISample  // err: 不能实现接口
{
    public static Point Origin => default;
    public double X { get => x; set => x = value; }
    public double Y { get => y; set => y = value; }
}
class Sample
{
    //Point[] points;   // err: 不能构造数组
    //ISample<Point> sample;    // err: 不能是类型参数
    //Point Origin;   // err: 不能是类或非 ref 结构的字段
    //object origin = (object)Point.Origin;  // err: ref 结构无法装箱成 object
    async void AsyncTest()
    {
        // Point Origin = new(0, 0); // err: async 方法中无法使用 ref 结构
    }
}
```

从 C#11 开始，可以在 `ref struct` 中声明 `ref` 字段。`ref` 字段可能具有 null 值，使用 `Unsafe.IsNullRef<T>(ref T src)` 方法确定 `ref` 字段是否为 `null`。

当 `readonly` 修饰 `ref` 字段时：
  - `ref`：在任何时候，都可以使用 `=` 为此字段关联引用赋值，或使用 `= ref` 重新赋值引用。
  - `readonly ref`：只能在构造函数或 `init` 访问器中使用 `= ref` 重新赋值引用。可以在字段访问修饰符允许的任何时间点使用 `=` 为此字段关联引用赋值。 
  - `ref readonly`：在任何时候，都不能使用 `=` 为此类字段关联引用赋值，但是可以使用 `= ref` 重新赋值引用。
  - `readonly ref readonly`：只能在构造函数或 `init` 访问器中通过 `= ref` 重新赋值引用。

```csharp
class DATA
{
    public static int F_Data = 0;
    public static int RF_Data = 0;
    public static int FR_Data = 0;
    public static int RFR_Data = 0;
}
ref struct Ref_Data
{
    public ref int F_Data;              // 表示引用可修改，值可修改
    public readonly ref int RF_Data;    // 表示引用不可修改，值可修改
    public ref readonly int FR_Data;    // 表示引用可修改，值不可修改
    public readonly ref readonly int RFR_Data;  // 表示引用和值均不可修改
    public Ref_Data()
    {
        // 可以在构造函数或 init 属性访问器中重新赋值
        F_Data = ref DATA.F_Data;
        FR_Data = ref DATA.FR_Data;
        RF_Data = ref DATA.RF_Data;
        RFR_Data = ref DATA.RFR_Data;
    }
}
```

#### 类和结构的区别

> 结构体的值类型，类是引用类型。

- 结构体是值类型，具有值语义。类是引用类型，具有引用语义。类实例之间可以相互依赖。但结构体之间相互依赖会导致编译时错误。结构体所依赖的结构体的完整集合是直接依赖关系的传递闭包（值包含值）。

```csharp
// 自我依赖
struct Node
{
    int data;
    Node next;  // 导致闭包循环
}
// 相互依赖
struct A { B b; }
struct B { A a; }
// ===================================
class NodeClass
{
    int data;
    NodeClass next; 
}
class AClass { BClass b;  }
class BClass { AClass b; }
```

> 继承

所有结构类型都隐式继承类 `System.ValueType`。结构声明可以指定接口，但不能指定基类。

结构总是隐式密封的，因此结构中不允许使用 `abstract` 和 `sealed`。由于结构体无法被继承，因此它的成员不能是 `protected`、`internal protected`、`private protected`。

结构体中的函数不能是 `virtual` 或 `abstract`，但是可以重写从 `System.ValueType` 继承的虚方法。

```csharp
struct Person(string name) : ICloneable
{
    public object Clone() => new Person(name);
    public override string ToString() => name ?? "";
}
```

> 赋值

对结构类型变量的赋值将创建被赋值值的副本。这与对类类型变量的赋值不同，后者复制引用，但不复制引用所标识的对象。

与赋值操作类似，当将结构体作为值形参传递或作为函数成员的结果返回时，将创建该结构体的副本。结构体可以使用 `in`、`out` 或 `ref` 形参通过引用传递给函数成员。

当结构体的属性或索引器是赋值的目标时，与该属性或索引器访问相关联的实例表达式应归类为变量。如果实例表达式被归类为值，则会发生编译时错误。

> 默认值

对于类类型和其他引用类型的变量，默认值为空。但是，由于结构体是不能为空的值类型，因此结构体的默认值是将所有值类型字段设置为默认值并将所有引用类型字段设置为空所产生的值。

```csharp
class Sample(string name)
{
    public readonly string Name = name;
    struct Person(string name)
    {
        public readonly string Name = name;
    }
    static void Main(string[] args)
    {
        Person[] people = new Person[10];
        Sample[] samples = new Sample[10];

        Console.WriteLine(people[0]);   // Sample+Person
        Console.WriteLine(samples[0]);  // null
    }
}
```

> 装箱与拆箱

只需在编译时将引用视为另一种类型，就可以将类类型的值转换为类型对象或由类实现的接口类型。同样，对象类型的值或接口类型的值可以在不更改引用的情况下转换回类类型（在这种情况下需要进行运行时类型检查）。

```csharp
interface ISample
{
    string Name { get; }
}
class Sample(string name) : ISample
{
    public string Name { get; set; } = name;
    static void Main(string[] args)
    {
        Sample S = new Sample("Hello");
        ISample id = S;             // Sample -> ISample
        Console.WriteLine(id.Name); // Hello
        S.Name = "World";
        Console.WriteLine(id.Name); // World

        Sample S2 = id as Sample;   // ISample -> Sample
    }
}
```

由于结构体不是引用类型，当结构类型的值转换为某些引用类型时，将发生装箱操作。同样，当某个引用类型的值被转换回结构类型时，将发生拆箱操作。与类类型上的相同操作的一个关键区别是，装箱和拆箱将结构体值复制到或复制到已装箱的实例之外。

在装箱或拆箱操作之后，对未装箱结构体所做的更改不会反映在装箱结构体中。

```csharp
interface ISample
{
    string Name { get; set; }
}
struct Sample(string name) : ISample
{
    public string Name { get; set; } = name;
    static void Main(string[] args)
    {
        Sample S = new Sample("Hello");
        ISample id = S;             // Sample -> ISample
        Console.WriteLine(id.Name); // Hello
        S.Name = "World";
        Console.WriteLine(id.Name); // Hello
        S.Name = id.Name;
        Console.WriteLine(((Sample)id).Equals(S));  // True, 值相等性

        id.Name = "World";
        if (id is Sample S2)    // ISample -> Sample
            Console.WriteLine(S2.Name);  // World
    }
}
```

> this 的含义

`this` 在结构体中的含义不同于 `this` 在类中的含义。当结构类型覆盖从 `System.ValueType` 继承的虚方法时（如 `Equals`、`GetHashCode` 或 `ToString`），通过结构类型的实例调用虚拟方法不会导致装箱。即使将结构用作类型参数并且通过类型参数类型的实例调用也是如此。

```csharp
class Sample
{
    struct Counter
    {
        int value;
        public override string ToString() => value++.ToString();
    }
    static void Test<T>() where T : new()
    {
        T x = new T();
        Console.WriteLine(x.ToString());    // 0
        Console.WriteLine(x.ToString());    // 1
        Console.WriteLine(x.ToString());    // 2
    }
    static void Main() => Test<Counter>();
}
```

> 字段初始化项

结构体中的实例字段和默认实现的属性具有初始值设定项时，必须同时包含显式声明的构造函数。

```csharp
struct Sample(string first, string last)
{
    public string Name { get; set; } = first + " " + last;
    public string FirstName = first;
    public string LastName = last;
}
```

> 构造函数

没有声明构造函数的非静态类或结构，编译器为提供一个默认的公共无参实例构造函数。类中显式声明实例构造函数，则无需提供默认构造函数。结构体中始终包含一个公共无参实例构造函数，无论是否显式声明。

```csharp
//Sample sc = new Sample(); // err
Sample sc = new Sample(10010);
SampleStruct ss = new SampleStruct();  // 隐式提供

class Sample(int value);
struct SampleStruct(int value);
```

#### 内联数组

从 C#12 开始，可以声明结构类型的内联数组。内联数组是包含相同类型的 N 个元素的连续块的结构，它是一个安全代码，等效于仅在不安全代码中可用的固定缓冲区声明，编译器可以利用有关内联数组的已知信息。内联数组是包含单个字段、且未指定其他任何的显式布局的 `struct`。

使用 `System.Runtime.CompilerServices.InlineArrayAttribute` 特性修饰 `struct` 类型，并指定一个大于零的值。

```csharp
[System.Runtime.CompilerServices.InlineArray(10)]
public struct InlineArray<T>
{
    private T Elem;
}
```

内联数组是一种高级语言功能。它们适用于高性能方案，在这些方案中，内联的连续元素块比其他替代数据结构速度更快。可以像访问数组一样访问内联数组，以读取和写入值，还可以使用范围和索引运算符。

内联数组对单个字段的类型有最低限制：它不能是指针类型，但可以是任何引用类型或任何值类型。几乎可以将内联数组与任何 C# 数据结构一起使用。

运行时团队和其他库作者使用内联数组来提高应用的性能。内联数组使开发人员能够创建固定大小的 `struct` 类型数组。具有内联缓冲区的结构应提供类似于不安全的固定大小缓冲区的性能特征。

```csharp
var buffer = new Buffer();
for (int i = 0; i < 10; i++)
    buffer[i] = i;
foreach (var i in buffer)
    Console.WriteLine(i);

[System.Runtime.CompilerServices.InlineArray(10)]
public struct Buffer
{
    private int _element0;
}
```

<br>

### Boxing & unboxing 装箱和拆箱

装箱和拆箱的概念提供了值类型和引用类型之间的桥梁。允许值类型的任何值和 `object` 之间进行转换。拆箱和装箱支持对类型系统的统一视图，任何类型的值最终都可以被视为对象。

#### 装箱

装箱用于在垃圾回收堆中存储值类型。装箱是值类型到引用类型的隐式转换。对值类型装箱会在堆中分配一个对象实例，并将该值复制到新的对象中。对于可空值类型，若 `HasValue` 为 `false`，装箱时将产生空引用，否则将展开并装箱其基础类型的值。

装箱转换意味着对被装箱的值做一个副本。

> 存在以下装箱转换

- 从任意值类型到 `object`。
- 从任意值类型到 `System.ValueType`。
- 从任意枚举类型到 `System.Enum`。
- 从任意非空值类型到其实现的接口类型 `I`，包含协变接口。
- 从任意可空值类型到引用类型，其中存在 `T?` 的底层类型 `T` 到引用类型的装箱转换。

> 装箱模拟

- 假设每个值类型存在一个对应的装箱类来模拟装箱过程。

```csharp
interface ISample
{
    void Fun();
}
struct S : ISample
{
    public void Fun() { }
}
sealed class S_Boxing<T>(T value) : ISample where T : ISample
{
    T Value = value;
    public void Fun() => Value.Fun();
}
```

- 装箱一个值类型。

```csharp
S s = new S();
object box = s;

// 可以想象为
S s = new S();
object box = new S_Boxing<S>(s);
```

- 实际上假象的 `S_Boxing` 并不存在，类型 `S` 的装箱值具有运行时类型，可以使用类型测试左操作数是否为右操作数的装箱版本。

```csharp
int i = 123;
object box = i;
if(box is int)
    Console.WriteLine("Box contains an int");
```

#### 拆箱

取消装箱（拆箱）是从引用类型到值类型的显式转换。拆箱操作首先要检查对象实例的运行时类型，以确保它是给定目标值类型的装箱值，然后再将该值从实例复制到值类型变量中。

被取消装箱的项必须是对一个对象的引用，该对象是先前通过装箱该值类型的实例创建的。目标类型是非空值类型时，尝试取消装箱 `null` 会导致 `NullReferenceException`。若是可空值类型，包含 `null` 的引用类型可以拆箱到该目标类型，否则拆箱到其基础值类型。尝试取消装箱对不兼容值类型的引用会导致 `InvalidCastException`。

如果值类型必须被频繁装箱，那么在这些情况下最好避免使用值类型。可通过使用泛型集合（例如 `System.Collections.Generic.List<T>`）来避免装箱值类型。

装箱和取消装箱过程需要进行大量的计算。对值类型进行装箱时，必须创建一个全新的对象，这可能比简单的引用赋值用时最多长 20 倍。取消装箱的过程所需时间可达赋值操作的四倍。

```csharp
int i = 123;
object o = i;  // implicit boxing
try
{
    int j = (short)o;  // attempt to unbox
    System.Console.WriteLine("Unboxing OK.");
}
catch (System.InvalidCastException e)
{
    System.Console.WriteLine("Error: Incorrect unboxing. \n{0}", e.Message);
    int j = (int)o;
    System.Console.WriteLine("Correct unboxing.");
}
/**
    Error: Incorrect unboxing.
    Unable to cast object of type 'System.Int32' to type 'System.Int16'.
    Correct unboxing.
 */
```

---
## 泛型类型

泛型类型声明本身表示一个未绑定的构造类型，使用类型参数为其构造形成许多不同类型的 “蓝图”。使用泛型构造时，需要为类型参数绑定具体类型名称。

借助泛型，可以根据要处理的精确数据类型设计方法、委托、类、结构或接口，以提高代码的可重用性和类型安全性。泛型是为所存储或使用的一个或多个类型具有占位符（类型形参）的类、结构、接口、方法和委托。例如泛型集合类可以将类型形参用作其存储的对象类型的占位符，泛型方法可将其类型形参用作其返回值的类型或用作其形参之一的类型。

```csharp
abstract class GenericSample
{
    // 泛型方法
    public abstract T GetGenericValue<T>();
    // 泛型类
    class GenericClass<T>;
    // 泛型结构
    struct GenericStruct<T>;
    // 泛型接口
    interface IGenericInterface<T>;
    // 泛型委托
    delegate void GenericDelegate<T>();
    // 泛型记录
    record GenericRecordClass<T>;
    record struct GenericRecordStruct<T>;
}
```

未绑定泛型类型本身不是类型，仅作为创建绑定的构造类型提供模板而存在，因此不能作为变量、参数、返回类型、或其他类的基类型。未绑定的泛型类型只能在 `typeof` 中使用。

```csharp
Sample s = new(); // 非泛型
Sample<int> s2 = new();  // 泛型 

class Sample;
class Sample<T>;
```

在泛型类型中声明的嵌套类型即使不直接指定类型参数，嵌套类型也被认为是泛型构造类型。

```csharp
class Sample<T>
{
    public class Nested;
}
class Sample: Sample<int>.Nested;
```

<br>

### 封闭类型和开放类型

所有类型都可以分为开放式类型或封闭式类型。开放式类型是包含类型参数的类型，具体含义为：
- 类型参数定义开放式类型。
- 当前仅当数组的元素类型是开放式类型时，该数组为开放式类型。
- 当前仅当一个或多个类型参数是开放式类型时，构造类型才是开放式类型。

在运行时，泛型类型声明中的所有代码在通过将类型自变量应用于泛型声明而创建的封闭式构造类型的上下文中执行。泛型类型中的每个类型形参都绑定到特定的运行时类型。所有语句和表达式的运行时处理始终出现在封闭式类型中，并且开放式类型仅在编译时处理期间出现。

每个封闭式构造类型都有自己的静态变量集，它们不与任何其他封闭构造类型共享。由于开放式类型在运行时不存在，因此没有与开放式类型关联的静态变量。如果两个封闭式构造类型是从同一个未绑定的泛型类型构造的，则这两个封闭式构造类型都是相同的类型，并且其对应的类型参数是相同的类型。

作为一种类型，类型参数纯粹是编译时构造。在运行时，每个类型参数都绑定到泛型类型的类型参数来指定的运行时类型。因此，在运行时，用类型参数声明的变量的类型将是封闭构造类型。所有涉及类型参数的语句和表达式的运行时执行都使用作为该参数的类型实参提供的类型。

```csharp
Generic<string> g_string = new Generic<string>();
g_string.Field = "A string";
Console.WriteLine("Generic.Field           = \"{0}\"", g_string.Field);
Console.WriteLine("g_string.GetType() = {0}", g_string.GetType().FullName);
/*
Generic.Field           = "A string"
g_string.GetType() = Generic`1[[System.String, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]
*/

record struct Generic<T>(string Field);   // 泛型记录
```

<br>

### 类型约束

只要引用构造类型或泛型方法，就会根据泛型类型或方法上声明的类型参数约束检查所提供的类型实参。约束告知编译器类型参数必须具备的功能。在没有任何约束的情况下，类型参数可以是任何类型。约束指定类型参数的功能和预期，声明这些约束意味着可以使用约束类型的操作和方法调用。

可以在泛型的定义中使用 `where` 子句指定对类型参数的参数类型约束。对于每个 `where` 子句，根据每个约束检查类型形参对应的类型实参。如果给定的类型参数不满足一个或多个类型参数的约束，则会发生编译时错误。

```csharp
interface ISample<T> where T : class;
// 指定类型参数必须是引用类型
```

可以对类型参数指定的约束有：
  - `struct`、`class`、`unmanaged`、`notnull`、`default` 约束不能组合或重复，且必须先在约束列表中指定。
  - `new()` 不能和 `unmanaged` 和 `struct` 一起使用，且只能是约束列表中的最后一个。

```csharp
interface ISample1<T> where T : struct;     // 不可为 null 的值类型
interface ISample2<T> where T : class;      // 不可为 null 的引用类型
interface ISample3<T> where T : class?;     // 可为 null 的引用类型
interface ISample4<T> where T : notnull;    // 不可为 null 的类型
interface ISample5<T> where T : unmanaged;  // 不可为 null 的非托管类型
interface ISample6<T> where T : new();      // 具有公共无参构造函数的类型
interface ISample7<T> where T : Base;       // 指定的基类或其派生类型
interface ISample8<T> where T : Base?;      // 可为 null 的指定基类或其派生类型
interface ISample9<T> where T : IBase;      // 指定的接口或实现接口的类型
interface ISample10<T> where T : IBase?;    // 可为 null 的指定接口或实现接口的类型
interface ISample11<T, U> where T : U;      // 指定 T 是 U 或 U 的派生类型

class Base;
interface IBase;
```

#### 不受约束的类型参数批注 `?` 和 default 约束

在 C# 8 中，`?` 批注仅适用于显式约束为值类型或引用类型的类型参数。在 C#9 中，`?` 批注可应用于任何类型参数，而不考虑约束。

```csharp
static T? FirstOrDefault<T>(this IEnumerable<T> collection) { ... };   // 不受约束的类型参数批注
```

如果类型参数 `T` 替换为引用类型，则 `T?` 表示该引用类型的可以为 null 的实例。

```csharp
var s1 = new string[0].FirstOrDefault();  // string? s1
var s2 = new string?[0].FirstOrDefault(); // string? s2
```

如果 `T` 用值类型替换，则 `T?` 表示的实例 `T`。 

```csharp
var i1 = new int[0].FirstOrDefault();   // int i1
var i2 = new int?[0].FirstOrDefault();  // int? i2
```

如果 `T` 使用批注类型替换 `U?`，则 `T?` 表示批注的类型 `U?` 而不是 `U??`。如果 `T` 将替换为类型 `U`，则 `T?` 表示 `U?`，即使在上下文中也是如此 `#nullable disable`。 

```csharp
var u1 = new U[0].FirstOrDefault();  // U? u1
var u2 = new U?[0].FirstOrDefault(); // U? u2
#nullable disable
var u3 = new U[0].FirstOrDefault();  // U? u3
```

> default 约束

- 为了与现有代码兼容，重写和显式接口实现的泛型方法不能包含显式约束子句，而 `T?` 在重写或显式接口实现的方法中被视为 `Nullable<T>`，其中 `T` 是值类型。
  
```csharp
class Base
{
    public virtual void Func<T>(T? t) { }
}
interface ISample
{
    void Func<T>(T? t); // T? 被认为是 Nullable<T>
}
class Derived : Base, ISample
{
    // 找不到合适的方法重写
    public override void Func<T>(T? t) { } // CS0453
    void ISample.Func<T>(T? t) { }   // CS0453
}
```  
  
- 为了允许对约束为引用类型的类型参数进行注释 `?`，C#8 允许在泛型方法上显式地约束 `where T: class` 和 `where T: struct`。

```csharp
class Base
{
    public virtual void Func<T>(T? t) where T : struct { }
    public virtual void Func<T>(T? t) where T : class { }
}
interface ISample
{
    void IFunc<T>(T? t) where T : struct; // T? 被认为是 Nullable<T>
}
class Derived : Base, ISample
{
    public override void Func<T>(T? t) /* where T: struct */{ }  // 重写 struct 约束方法
    public override void Func<T>(T? t) where T : class { }
    void ISample.IFunc<T>(T? t) { }
}
```

- 为了允许对不受引用类型或值类型约束的类型参数进行注释，C#9 允许一个新的 `where T: default` 约束。重写或显式接口实现的方法使用 `default` 约束以外的约束是错误的。

```csharp
class Base
{
    public virtual void Func<T>(T? t) where T : struct { }
    public virtual void Func<T>(T? t) { }
}
interface ISample
{
    void IFunc<T>(T? t);
}
class Derived : Base, ISample
{
    public override void Func<T>(T? t) /* where T: struct */{ }
    public override void Func<T>(T? t) where T : default { }
    void ISample.IFunc<T>(T? t) where T : default { }
}
```

- 当重写方法或接口方法中的相应类型参数被约束为引用类型或值类型时，使用 `default` 约束是错误的。

```csharp
class Base
{
    public virtual void Func<T>(T? t) where T : struct { }
    public virtual void Func<T>(T? t) where T : class { }
}
class Derived : Base
{
    public override void Func<T>(T? t) /* where T: struct */{ }
    //public override void Func<T>(T? t) where T : class{ }
    public override void Func<T>(T? t) where T : default { } // CS8822
}
```

#### 泛型类型中的静态成员

使用泛型类型时指定类型参数时，运行时将创建该类型参数的封闭式构造类型。从同一泛型类型的构建的不同构造类型之间，各构造泛型类型的静态成员（包括静态构造函数、字段、方法、属性等）独立存在。在首次调用该类型时，会首先调用它的静态构造函数。对于泛型接口类型的不能构造类型之间，静态成员（非抽象）也是相互独立的。

```csharp
var I1 = ISample<int>.Default;
Console.WriteLine("-------------");
var I2 = ISample<string>.Default;
Console.WriteLine("-------------");
var s1 = Sample<float>.Default;
Console.WriteLine("-------------");
var s2 = Sample<object>.Default;

/*
Static ISample() >> Int32
-------------
Static ISample() >> String
-------------
Static Sample() >> Single
-------------
Static Sample() >> Object
*/

interface ISample<T> 
{
    static ISample() =>Console.WriteLine($"Static ISample() >> {typeof(T).Name}");
    public static T? Default { get; set; } = default;
}
class Sample<T>
{
    static Sample() => Console.WriteLine($"Static Sample() >> {typeof(T).Name}");
    public static T? Default { get; set; } = default;
}
```

#### 泛型接口的协变与逆变

借助泛型类型参数的协变和逆变，可以使用类型自变量的派生程度比目标构造类型更高（协变）或更低（逆变）的构造泛型类型。协变和逆变统称为 “变体”，未标记为协变或逆变的泛型类型参数称为 “固定参数” 。

协变和逆变类型参数仅限于泛型接口和泛型委托类型，变体仅适用于与引用类型。当类型参数指定为值类型时，该类型参数对于生成的构造类型是不可变的。

使用 `in` 关键字指定类型参数是逆变的，逆变的类型参数可以用作泛型接口的方法或泛型委托的参数类型。`out` 关键字指定类型参数是协变的，协变的类型参数可用作接口方法的返回类型。

```csharp
delegate TResult GenericDelegate<in T, out TResult>(T arg);
interface IGeneric<in T, out TResult>
{
    TResult GetResult(T arg);
}
```

协变和逆变能够实现委托类型、泛型接口类型和泛型类型参数的隐式引用转换。

```csharp
class VariantSample
{
    delegate A DCovariant<out A>();             // 协变泛型委托
    delegate void DContravariant<in A>(A a);    // 逆变泛型委托

    interface ICovariant<out A> { }         // 协变泛型接口
    interface IContravariant<in A> { }      // 逆变泛型接口

    class Base;
    class Derived : Base;
    class Sample<T> : ICovariant<T>, IContravariant<T>;
    static T SampleFunc<T>() => default;
    static void SampleFunc<T>(T t) { }

    static void Main(string[] args)
    {
        // 泛型接口中的协变
        ICovariant<Base> I_B = new Sample<Base>();
        ICovariant<Derived> I_D = new Sample<Derived>();
        I_B = I_D;  // 协变

        // 泛型接口中的逆变
        IContravariant<Base> I_B2 = new Sample<Base>();
        IContravariant<Derived> I_D2 = new Sample<Derived>();
        I_D2 = I_B2;  // 逆变

        // 泛型委托中的协变
        DCovariant<Base> D_B = SampleFunc<Base>;
        DCovariant<Derived> D_D = SampleFunc<Derived>;
        D_B = D_D;  // 协变

        // 泛型委托中的逆变
        DContravariant<Base> D_B2 = SampleFunc<Base>;
        DContravariant<Derived> D_D2 = SampleFunc<Derived>;
        D_D2 = D_B2;  // 逆变
    }
}
```

> 扩展变体泛型接口

- 扩展变体泛型接口时，必须使用 `in` 和 `out` 关键字来显式指定派生接口是否支持变体。编译器不会根据正在扩展的接口来推断变体。

```csharp
interface ICovariant<out T> { }
interface IInvariant<T> : ICovariant<T> { }
interface IExtCovariant<out T> : ICovariant<T> { }

```

- 如果泛型类型参数 `T` 在一个接口中声明为协变，则无法在扩展接口中将其声明为逆变。

```csharp
interface ICovariant<out T> { }
interface IContravariant<in T> { }
interface IInvariant<T> : ICovariant<T>, IContravariant<T> { }  // 无法声明逆变或协变
```

> 避免多义性 

- 实现变体泛型接口时，变体有时可能会导致多义性。应避免这样的多义性。如果在一个类中使用不同的泛型类型参数来显式实现同一变体泛型接口，便会产生多义性。在这种情况下，编译器不会产生错误，但未指定将在运行时选择哪个接口实现。这种多义性可能导致代码中出现小 bug。

```csharp
// Simple class hierarchy.
class Animal;
class Cat : Animal;
class Dog : Animal;

// This class introduces ambiguity
// because IEnumerable<out T> is covariant.
class Pets : IEnumerable<Cat>, IEnumerable<Dog>
{
    IEnumerator<Cat> IEnumerable<Cat>.GetEnumerator()
    {
        Console.WriteLine("Cat");
        // Some code.
        return null;
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        // Some code.
        return null;
    }
    IEnumerator<Dog> IEnumerable<Dog>.GetEnumerator()
    {
        Console.WriteLine("Dog");
        // Some code.
        return null;
    }
}
class Program
{
    public static void Test()
    {
        IEnumerable<Animal> pets = new Pets();
        pets.GetEnumerator();  // Cat ? Dog ? 
    }
}
```

#### 数组的协变

数组的协变使派生程度更大的类型的数组能够隐式转换为派生程度更小的类型的数组。

```csharp
IEnumerable<object> e = new List<string>();
IEnumerable<object> e2 = new List<int>();  // CS0266，值类型不支持协变
IEnumerable<object>[] enumerables = new List<string>[] { }; // 数组的协变
```

---
## 表达式树类型

表达式树允许将 Lambda 表达式表示为数据结构，而不是可执行代码。表达式树类型是 `System.Linq.Expressions.LambdaExpression` 或 `System.Linq.Expressions.Expression<TDelegate>`，其中 `TDelegate` 是任意委托类型。

若存在从 `Lambda` 表达式到委托类型 `D` 的转换，则也存在到表达式树类型 `Expression<TDelegate>` 的转换。将 Lambda 表达式转换为委托类型会生成一个引用 Lambda 表达式可执行代码的委托，而将 Lambda 表达式转换为表达式树类型，则构造为一个表达式树。

```csharp
using System.Linq.Expressions;

Func<int, int> Del = x => x + 1;  // Code
Expression<Func<int, int>> Exp = x => x + 1;  // Data
```

将 Lambda 表达式转换为表达式树类型会生成表达式树。更准确地说，Lambda 表达式转换的求值产生一个表示 Lambda 表达式本身结构的对象结构。

并非每个 Lambda 表达式都可以转换为表达式树类型。虽然 Lambda 表达式始终存在到兼容委托类型的转换，但由于特定于实现的原因，它转换表达式树时可能在编译时失败。

<br>

### 表达式树的限制

- 不能调用没有实现声明的分部方法、调用已移除的条件方法（`Conditional`）、调用本地函数、调用 `ref` 返回的方法属性或索引器、调用使用可选参数的方法、调用包含命名参数规范的方法、调用省略 `ref` 的 COM 方法、。
- 不能使用 Lambda 语句、异步 Lambda 表达式、引用 `ref` 返回的 Lambda、使用引用传递（`in`、`out`、`ref`）参数的 Lambda、具有特性的 Lambda、
- 不能使用 `base` 访问、赋值操作、`dynamic` 动态操作、模式匹配、元组字面值和元组操作（相等、不等、转换）、`??=` 空合并运算符、`?.` 空传播运算符、索引和范围运算符、不安全的指针操作、不能包含左侧为 `null` 或 `default` 字面量的 `??` 合并运算符。
- 不能使用 `throw` 表达式、`with` 表达式、`switch` 表达式、匿名方法表达式、多维数组和字典的初始值设定项、不支持扩展 `Add` 的集合初始值设定项。
- 不能使用 `ref struct` 类型、调用访问或声明内联数组、弃元 `_` 声明、
- 无法访问静态抽象或虚拟的接口成员、不能包含模式 `System.Index` 或 `System.Range` 索引器访问、不能包含内插字符串处理程序转换、不能在方法组上使用 `&` 地址运算、不能包含索引属性。
- 表达式树的类型参数不能是非委托类型。

<br>

### 执行表达式树

表达式树是表示一些代码的数据结构，它不是经过编译且可执行的代码。若要执行由表达式树表示的 .NET 代码，必须将其转换为可执行的 IL 指令。执行表达式树可能返回一个值，或仅仅是执行操作。

仅可以执行表示 Lambda 表达式的表达式树。若要执行表达式树，调用 `Expression<TDelegate>` 的 `Compile` 方法创建一个 `TDelegate` 类型的委托。`LambdaExpression` 可以表示未知类型的委托，调用 `Compile` 返回一个 `Delegate` 类型。`Delegate` 应使用 `DynamicInvoke` 调用而不是直接调用 `Invoke`。

```csharp
Expression<Action<string>> exp= (string mess) => Console.Write(mess);
Action<string> del = exp.Compile();
del.Invoke("Hello");

LambdaExpression exp2 = (string mess) => Console.Write(mess);
Delegate del2 = exp2.Compile();
del2.DynamicInvoke(" World");
```

如果表达式树不表示 Lambda 表达式，可以通过调用 `Lambda<TDelegate>(Expression, IEnumerable<ParameterExpression>)` 方法创建一个新的 Lambda 表达式。


<br>

## 变量

C# 定义了 8 种变量：静态变量、实例变量、数组元素、值参数、输入参数、引用参数、输出参数和局部变量。

```csharp
class Sample
{
    static int x;  // 静态变量 
    private int y; // 实例变量
    void Fun(
        int[] v,   // v[0] 数组元素
        int a,     // 值参数
        in int b,  // 输入参数
        ref int c, // 引用参数
        out int d) // 输出参数
    {
        int i = 10;  // 局部变量
        d = a + c++ + b;
    }
}
```

<br>

### 变量类别
#### 静态变量 

静态变量使用 `static` 声明，静态变量在其包含类型的静态构造函数执行之前存在，并且在关联的零一程序域不存在时停止存在。静态变量的初始值是变量类型的默认值。

由于明确赋值的目的，静态变量被视为初始赋值。

#### 实例字段

未声明静态 `static` 修饰符的字段是实例字段。

类的实例字段在类的新实例创建时存在，当没有对该实例的引用且实例的终结器已经执行时，实例变量停止存在。类的实例变量的初始值是变量类型的默认值。由于明确赋值的目的，也被认为是初始赋值。

结构体的实例变量与它所属的结构体变量具有完全相同的生存期，其实例变量的初始赋值状态与包含结构体变量的初始赋值状态相同。当结构体变量被认为是初始赋值时，它的实例变量也是；若是初始未赋值的，它的实例变量也是为赋值的。

#### 数组元素

数组的元素在创建数组实例时开始存在，在没有对数组实例的引用时停止存在。数组中每个元素的初始值是数组元素类型的默认值。
由于明确赋值的目的，数组元素被认为是初始赋值。

#### 值参数

没有 `ref`、`in`、`out` 修饰的形参是值参数。值参数在调用函数成员或匿名函数时产生，并使用调用
中给出的实参的值进行初始化。当函数体执行完成时，值参数通常不再存在。若值参数被一个匿名函数捕获，它的生命周期至少会延长到从该匿名函数创建的委托或表达式树符合垃圾回收的条件。

由于明确赋值的目的，一个值参数被视为初始赋值。

#### 引用参数

用 `ref` 修饰声明的形参是引用参数。引用参数是在调用函数成员、委托、匿名方法或本地函数时产生的引用变量。引用参数不会创建新的存储位置，它与给定调用中的参数变量表示相同的存储位置。当函数体执行完成时，引用参数也不存在，且引用参数不会被捕获。

变量在作为函数成员或委托调用的引用参数传递之前必须明确赋值。由于明确赋值的目的，引用参数被视为初始赋值。

对于结构类型的实例方法或实例访问器中，`this` 的行为与引用参数完全相同。

#### 输出参数

用 `out` 修饰声明的形参是输出参数。输出参数是在调用函数成员、委托、匿名方法或本地函数时产生的引用变量。当函数体执行完成时，输出参数不再存在，且输出参数不会被捕获。

输出参数的赋值规则：
- 在函数成员或委托调用中将变量作为输出参数传递之前，不需要明确赋值。
- 在函数成员或委托调用正常完成之后，作为输出参数传递的每个变量都被认为是在该执行路径中分配的。
- 在函数成员或匿名函数中，输出参数最初被认为是未分配的。
- 函数成员、匿名函数或本地函数的每个输出参数必须在函数正常返回之前明确赋值。

#### 输入参数

用 `in` 修饰声明的参数是输入参数。输入参数是在调用函数成员、委托、匿名函数或局部函数时产生的引用变量，其引用被初始化该调用中作为实参给出的变量引用。当函数体执行完成时，输入参数不再存在，且输入参数不会被捕获。

变量在作为函数成员或委托调用的输入参数传递之前必须明确赋值。出于明确赋值的目的，输入参数被视为初始赋值。

#### 局部变量

局部变量在函数体、语句块中声明出现。局部变量的生命周期是程序执行期间保证为其保存存储空间的部分。此生命周期从进入与其关联的作用域开始扩展，至少到该作用域的执行以某种方式结束为止。若局部变量被匿名函数捕获，那么它的生命周期至少延续到从匿名函数创建的委托或表达式树，以及引用捕获变量的任何其他对象符合垃圾回收条件为止。

每次进入局部变量的作用域时都会实例化它。对于 `foreach` 的迭代变量，每次迭代都会创建一个新的只读变量。局部变量在使用前必须明确赋值。

> 弃元

弃元 `_` 是一个没有名称的局部变量，由声明表达式引入。弃元也可以作为 `out` 参数传递。由于弃元没有被明确赋值，所以访问它的值始终是错误的。但有些声明中 `_` 是一个有效的标识符，此时 `_` 是一个明确赋值的变量存在，在其作用域范围内，弃元无法使用。

<br>

### 明确赋值

在函数成员或匿名函数的可执行代码中的给定位置，如果编译器可以通过特定的静态流分析，证明该变量已被自动初始化或已成为至少一次赋值的目标，则称该变量已被明确赋值：
- 初始赋值的变量总是被认为是明确赋值的。
- 初始未赋值的变量，如果在指定位置由所有可能的执行路径中包含以下行为之一时，被认为是在给定位置明确赋值：
  - 简单的赋值操作，其中变量是左操作数。
  - 将变量作为输出参数传递的调用表达式或对象创建表达式。
  - 对于局部变量，为变量做一个局部变量声明，并包含变量初始化式。

在以下上下文中，明确赋值是必需的：
- 变量必须在获取其值的每个位置明确赋值。
- 变量必须在作为引用参数、输入参数传递的每个位置进行明确赋值。
- 函数成员的所有输出参数都必须在函数成员通过语句返回前明确赋值。
- 结构类型的实例构造函数 `this` 变量必须在该实例构造函数返回的每个位置明确赋值，结构变量被认为是明确赋值的前提是该实例变量包含的所有结构类型变量都被视为是明确赋值。

> 初始分配的变量

静态变量、类实例变量、初始分配的结构实例变量、数组元素、值参数、引用参数、输入参数、`catch` 或 `foreach` 子句的变量等。

> 初始未分配的变量

初始未分配的结构实例变量、输出参数、局部变量。

#### 变量引用的原子性

`bool`、`char`、`byte`、`sbyte`、`short`、`ushort`、`int`、`uint`、`float` 和引用类型的读取和写入是原子的，具有前面列表中基础类型的枚举类型的读写也是原子的；`long`、`ulong`、`double`、`decimal` 和用户定义类型的读写不能保证为原子性。

<br>

### 引用变量和引用返回

引用变量是指对另一个变量的变量。引用变量是 `ref` 修饰的局部变量。引用变量存储对变量的引用，而不是变量的值。

当在需要值的地方使用引用变量时，将返回其引用的值。当引用变量是赋值的目标时，它就是被赋值的引用对象。若要更改引用变量的引用对象时，使用 `= ref` 进行更改关联引用。

```csharp
int num1 = 10;
int num2 = 20;

ref int pnum = ref num1;
pnum = 100;
Console.WriteLine(num1);  // 100
Console.WriteLine(num2);  // 20
pnum = ref num2;
pnum = 10010;
Console.WriteLine(num1);  // 100
Console.WriteLine(num2);  // 10010
```

引用返回是由 `return ref` 方法返回的引用变量。

```csharp
int[] arr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
ref int arr5 = ref Fun();

arr5 = 10086;
Console.WriteLine(arr[5]);  // 10086

ref int Fun()
{
    // element is a reference variable that refers to arr[5]
    ref int element = ref arr[5];
    // return reference to arr[5];
    return ref element; 
}
```

<br>

### Ref-Safe-Context

所有引用变量都遵循安全规则，以确保引用变量本身的 *Ref-Safe-Context* 不大于其变量引用的 `Ref-Safe-Context`。

对于任何变量，该变量的 *Ref-Safe-Context* 是对该变量引用的有效上下文。引用变量的指针应该有一个至少和引用变量本身的 *Ref-Safe-Context* 一样宽的 *Ref-Safe-Context*。

编译器通过对程序文本的静态分析来确定 *Ref-Safe-Context*，*Ref-Safe-Context* 反映了变量在运行时的生存期。

有三种 *Ref-Safe-Context*：
- *Declaration-Block* 声明块：对局部变量的变量引用的 *Ref-Safe-Context* 是该局部变量的作用域，并包括该作用域中的任何嵌套嵌入语句。只有当引用变量在该变量的 *Ref-Safe-Context* 中声明时，对局部变量的变量引用才是引用变量的有效引用。
* *Function-Member* 函数成员：在函数中，对下列任何类型的变量的变量引用都有一个 *Ref-Safe-Context*：
  - 函数成员声明的值形参，包括类成员函数的隐式 `this` 和结构成员函数的隐式引用（`ref`）形参 `this` 及其字段。
  - 带有 *Function-Member* 的 *Ref-Safe-Context* 的变量引用，只有在引用变量在同一函数成员中声明时才是有效的引用。
- *Caller-Context* 调用上下文：在函数中，对下列任何一种类型的变量都有一个名为 *Caller-Context* 的 *Ref-Safe-Context*：`ref` 引用形参（不是结构成员函数的隐式 `this`）、这些参数的成员字段和元素、类类型参数的成员字段、数组类型参数的元素。
  - 具有调用者上下文的 *Ref-Safe-Context* 的变量引用可以是 `ref return` 的返回引用。

这些变量形成了从最窄（声明块）到最宽（调用者上下文）的嵌套关系。每个嵌套块代表一个不同的上下文。

```csharp
public class C
{
    // ref safe context of arr is "caller-context". 
    // ref safe context of arr[i] is "caller-context".
    private int[] arr = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    // ref safe context is "caller-context"
    public ref int M1(ref int r1)
    {
        return ref r1; // r1 is safe to ref return
    }

    // ref safe context is "function-member"
    public ref int M2(int v1)
    {
        return ref v1; // error: v1 isn't safe to ref return
    }

    public ref int M3()
    {
        int v2 = 5;
        return ref arr[v2]; // arr[v2] is safe to ref return
    }

    public void M4(int p)
    {
        int v3 = 6;

        // context of r2 is declaration-block,
        // ref safe context of p is function-member
        ref int r2 = ref p;

        // context of r3 is declaration-block,
        // ref safe context of v3 is declaration-block
        ref int r3 = ref v3;

        // context of r4 is declaration-block,
        // ref safe context of arr[v3] is caller-context
        ref int r4 = ref arr[v3];
    }
}
```

对于结构类型，隐式 `this` 形参作为 `ref` 形参传递。作为函数成员的结构类型的字段的 *Ref-Safe-Context* 防止通过 `ref return` 返回这些字段。

```csharp
public struct S
{
    private int n;
    // Disallowed: returning ref of a field.
    public ref int GetN() => ref n;
}
class Test
{
    public ref int M()
    {
        S s = new S();
        ref int numRef = ref s.GetN();
        return ref numRef; // reference to local variable 'numRef' returned
    }
}
```

#### 局部变量

对于局部变量如果 `v` 是引用变量，它的 *Ref-Safe-Context* 与其初始化表达式的 *Ref-Safe-Context* 相同。否则它的 *Ref-Safe-Context* 就是 *Declaration-Block*。

#### 形式参数

对于形式参数 `p`：
- 如果 `p` 是 `ref` 或 `in` 形参，则其 *Ref-Safe-Context* 是 *Caller-Context*。
- 如果 `p` 是一个 `in` 参数，它就不能作为可写 `ref` 返回，但也可以作为 `ref readonly` 返回。
- 如果 `p` 是一个 `out` 参数，它的 *Ref-Safe-Context 就是 *Caller-Context*。
- 否则，如果 `p` 是结构类型的 `this` 形参，则其 *Ref-Safe-Context* 是 *Function-Member*。
- 否则，形参是一个值形参，其 *Ref-Safe-Context* 是 *Function-Member*。

#### 字段

对于指定对字段的引用的变量，例如 `e.Field`：
- 如果 `e` 是引用类型，则 `Field` 的 *Ref-Safe-Context* 就是 *Caller-Context*。
- 如果 `e` 是值类型，则 `Field` 变量引用的 *Ref-Safe-Context* 与 `e` 的*Ref-Safe-Context* 相同。

#### 运算符

条件运算符 `c ? ref e1 : ref e2` 和引用赋值 `= ref` 将引用变量作为操作数并产生一个引用变量。对于这些操作符，结果的 *Ref-Safe-Context* 是所有 `ref` 操作数的 *Ref-Safe-Context* 中最窄的上下文。

#### 函数或属性调用

对于由 `ref return` 函数调用产生的变量 `c`，其 *Ref-Safe-Context* 是以下上下文最窄的：
- *Caller-Context* 上下文。 
- 所有 `ref`、`out` 和 `in` 参数表达式的 *Ref-Safe-Context*（不包括接收方）。
- 对于每个 `in` 形参，如果对应的表达式是一个变量，并且变量类型和形参类型之间存在恒等转换，该变量的 *Ref-Safe-Context*；否则为最近的封闭上下文。
- 所有参数表达式（包括接收方）的 *Ref-Safe-Context*。

对于 `ref` 返回的属性或索引器的调用，被视为底层访问器方法的调用。

```csharp
class S
{
    ref int M(ref int p)
    {
        return ref p;
    }
    ref int M2()
    {
        int v = 5;
        // Not valid.
        // ref safe context of "v" is block.
        // Therefore, ref safe context of the return value of M() is block.
        return ref M(ref v);   //err
    }
    ref int p
    {
        get
        {
            int v = 0;
            return ref M(ref v);  //err
        }
    }
}
```

#### 变量

变量的 *Ref-Safe-Context* 是它最近的封闭上下文。

#### 构造函数调用

调用构造函数的 `new` 表达式遵循与方法调用相同的规则，构造函数的调用被视为返回正在构造的类型的方法的调用。

#### 对引用变量的限制

- Lambda 表达式或本地函数不能捕获引用形参、输出形参、输入形参、`ref 局部变量或 `ref struct` 类型的局部。
- 引用形参、输出形参、输入形参和 `ref struct` 结构类型的形参都不能作为迭代器方法或异步方法的实参。
- `ref` 局部变量和 `ref struct` 类型的局部变量，都不能出现在 `yield return` 语句或 `await` 表达式的上下文中。
- 对于 `ref` 重赋值 `e1 = ref e2`，`e2` 的 *Ref-Safe-Context* 至少与 `e1` 的 *Ref-Safe-Context* 一样宽。
- 对于一个 `ref return` 方法的语句 `return ref`，`ref` 的 *Ref-Safe-Context* 是 *Caller-Context*。

<br>

### Safe-Context 约束

在编译时，每个表达式都与一个上下文相关联，在这个上下文中，该实例及其所有字段都可以被安全访问，即它的安全上下文 *Safe-Context*。 *Safe-Context* 是一个包含表达式的上下文，将值转义到该表达式是安全的。

任何编译时类型不是 `ref struct` 的表达式都有一个调用上下文 *Caller-Context* 的 *Safe-Context*。对于任何类型的 `default` 表达式，都有一个*Caller-Context* 的 *Safe-Context*。

*Safe-Context* 记录了值可以复制到哪个上下文。给定从具有安全上下文 `S1` 的表达式 `E1` 到具有安全上下文 `S2`  的表达式 `E2` 的赋值，如果 `S2` 具有比 `S1` 更宽的上下文，则会发生错误。

有三种不同的 *Safe-Context* 含义，与引用变量的引用安全上下文 *Ref-Safe-Context* 含义相同：声明块 *Declaration-Block*，方法成员 *Function-Member*，和调用上下文 *Caller-Context*。表达式的 *Safe-Context* 对其使用的限制如下：
- 对于 `return` 语句 `return E1`，`E1` 的安全上下文应为 *Caller-Context*。
- 对于赋值操作 `E1 = E2`，`E2` 至少具有与 `E1` 相同宽度的安全上下文。

对于方法调用，如果存在 `ref`、`in` 或 `out` 参数为 `ref` 结构类型（包括接收者，除非该类型为 `readonly`），且具有安全上下文 `S1`，则任何参数（包括接收者）都不能具有比 `S1` 更窄的 *Safe-Context*。

```csharp
ref struct Sample
{
    ref int b;
    ref int Fun(ref int a)
    {
        b = ref a;  // a 具有更窄的语义（上下文），只能通过 return 返回对 a 进行转义 
        return ref a;
    }
    ref int Fun2(scoped ref int a)
    {
        b = ref a;  // a 具有更窄的语义（上下文）且仅作用于当前函数体，
        return ref a;  // 也不能通过 return 返回对 a 进行转义 
    }
}
```

#### ref struct 类型变量

`ref struct` 类型的形参（包括实例方法的 `this`）具有 *Caller-Context* 的 *Safe-Context*。

`ref struct` 类型的局部变量具有：
- 如果变量是 `foreach` 循环的迭代变量，则该变量的 *Safe-Context* 与 `foreach` 循环表达式的 *Safe-Context* 相同。
- 否则，如果变量声明有初始化项，则变量的 *Safe-Context* 与该初始化项的 *Safe-Context* 相同。
- 否则，该变量在声明点未初始化，并且具有一个 *Caller-Context* 的 *Safe-Context*。

`ref struct` 类型的字段 `e.F` 与它的包含类型 `e` 具有相同的 *Safe-Context*。

#### 运算符

用户定义的运算符的调用被视为方法调用。对于产生值的运算符（如 `e1+e2`、`c?e1:e2`），结果的 *Safe-Context* 则是运算符操作数中 *Safe-Context* 中最窄的上下文。

#### 方法和属性调用
 
`t.M(e1, ...)` 方法调用或 `t.P` 属性调用产生的值的 *Safe-Context*，是调用方、所有的参数表达式、接收方中 *Safe-Context* 最窄的上下文。属性调用被视为对底层方法的调用。

#### stackalloc

`stackalloc` 表达式的结果具有 *Function-Member* 的 *Safe-Context*。 

#### 构造函数调用

调用构造函数的 `new` 表达式遵循与方法调用相同的规则，构造函数的调用被视为返回正在构造的类型的方法的调用。如果存在任何的初始化项，则 *Safe-Context* 是所有对象初始化项表达式的最窄的。

<br>




### ==============

所有的引用变量都遵守安全规则。对于任何变量，









---
### 引用类型
<br>

#### string 字符串类型

- `string` 类型表示零个或多个 Unicode 字符的序列。使用相等运算符 `==` 和 `!=` 比较 `string` 对象的值，为不是比较对象的引用。

```csharp
string str1 = "hello";
string str2 = "h";
str2 += "ello";
Console.WriteLine(str1 == str2);  // true
Console.WriteLine(object.ReferenceEquals(str1, str2)); // false
```

> 字符串拼接

- `+` 用于拼接两个字符串片段。字符串是不可变的，每次赋值时，编译器实际上会创建一个新的字符串对象来保存新的字符序列，并将新对象赋值给目标，并将之前的内存用于垃圾回收。

```csharp
string str = "Hello " + "World!";
```

> 字符串索引

- `[]` 运算符可用于只读访问字符串的个别字符。

```csharp
string str = "test";
for (int i = 0; i < str.Length; i++)
  Console.Write(str[i] + " ");
// Output: t e s t
```

> 字符串内插

- `$` 字符将字符串字面量标识为内插字符串，内插字符串是可能包含内插表达式的字符串文本。将内插字符串解析为结果字符串时，带有内插表达式的项会替换为表达式结果的字符串表示形式。
- 内插字符串初始化常量时，所有的内插表达式也必须是常量字符串。C#11 起内插表达式支持使用换行符，以使表达式更具有可读性

```csharp
$"{<interpolationExpression>[,<alignment>][:<formatString>]}"
// - interpolationExpression     生成需要设置格式的结果的表达式
// - alignment                   常数表达式，定义对齐方式和最小字符宽度，负值表示左对齐，正值表示右对齐
// - formatString                受表达式结果类型支持的格式字符串，例如 DateTime 格式化输出

Console.WriteLine($"|{"Left",-7}|{"Right",7}|");
// |Left   |  Right|

const int FieldWidthRightAligned = 20;      // $"{{" 打印 {
Console.WriteLine($"{{{Math.PI,FieldWidthRightAligned}}} - default formatting of the pi number");
Console.WriteLine($"{{{Math.PI,FieldWidthRightAligned:F3}}} - display only three decimal digits of the pi number");
//{   3.141592653589793} - default formatting of the pi number
//{               3.142} - display only three decimal digits of the pi number

string message = $"The usage policy for {safetyScore} is {
    safetyScore switch
    {
        > 90 => "Unlimited usage",
        > 80 => "General usage, with daily safety check",
        > 70 => "Issues must be addressed within 1 week",
        > 50 => "Issues must be addressed within 1 day",
        _ => "Issues must be addressed before continued use",
    }}";
```

> 逐字字符串

- `@` 指示将原义解释字符串。简单转义序列（如代表反斜杠的 `"\\"`）、十六进制转义序列（如代表大写字母 A 的 `"\x0041"`）和 Unicode 转义序列（如代表大写字母 A 的 `"\u0041"`）都将按字面解释。引号转义 `""` 不会按字面解释。
- 逐字内插字符串中，大括号转义序列（`{{` 和 `}}`）不按字面解释。

```csharp
string filename1 = @"c:\documents\files\u0066.txt";
string filename2 = "c:\\documents\\files\\u0066.txt";
Console.WriteLine(filename1);
Console.WriteLine(filename2);
// The example displays the following output:
//     c:\documents\files\u0066.txt
//     c:\documents\files\u0066.txt

string str = $@"{{{Math.PI,20}}} >> ""default formatting of the pi number""";
Console.WriteLine(str);
//{   3.141592653589793} >> "default formatting of the pi number"
```

> 原始字符串

- 原始字符串字面量从 C#11 开始可用。字符串字面量可以包含任意文本，而无需转义序列，字符串字面量可以包括空格和新行、嵌入引号以及其他特殊字符。原始字符串字面量用至少三个双引号（`"""`） 括起来。

```csharp
var message = """
This is a multi-line
    string literal with the second line indented.
"""
// 原始字符串的起始、结束引导序列长度要超过字符串中最长的引号序列长度
"""""
This raw string literal has four """", count them: """" four!
embedded quote characters in a sequence. That's why it starts and ends
with five double quotes.

You could extend this example with as many embedded quotes as needed for your text.
"""""
```

> UTF-8 字符串字面量

- .NET 中的字符串是使用 UTF-16 编码存储的。UTF-8 是 Web 协议和其他重要库的标准。从 C#11 开始，可以将 `u8` 后缀添加到字符串字面量以指定 UTF-8 编码。UTF-8 字面量存储为 `ReadOnlySpan<byte>` 对象。 UTF-8 字符串字面量的自然类型是 `ReadOnlySpan<byte>`。UTF-8 字符串字面量不能与字符串内插结合使用。

```csharp
using System.Text;

ReadOnlySpan<byte> strU8 = "Hello world!"u8;
string strU16 = Encoding.UTF8.GetString(strU8);
Console.WriteLine(strU16);

string str = "Hello world!";
ReadOnlySpan<byte> bytes = Encoding.UTF8.GetBytes(str);
```

<br>

#### record 记录类型

- 从 C#9 开始，可以使用 `record` 修饰符定义一个引用类型，用来提供用于封装数据的内置功能。C#10 允许 `record class` 语法作为同义词来阐明引用类型，并允许 `record struct` 使用相同功能定义值类型。
- 位置记录：在记录上声明主构造函数时，编译器会为记录类型自动生成一个位置构造函数，同时根据位置参数自动生成一个解构函数 `Deconstruct` 以支持将位置记录解构为元组，并在该位置记录中为主构造函数的参数生成公共属性。

```csharp
public record Person(string FirstName, string LastName);
// 相当于
public record Person{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    // 结构函数在记录中自动生成，可以声明方法重载或显式声明默认的 Deconstruct
    public void Deconstruct(out string firstName, out string lastName) 
    => (firstName, lastName) = (FirstName, LastName)
}

public record struct Point(int x, int y);
// 相当于
public record struct Point{
    public int x {get; set;}
    public int y {get; set;}
}

public readonly record struct Score(int Math, int English);
// 相当于
public readonly record struct Score{
    public int Math { get; init; }
    public int English { get; init; }
}
```

> 属性定义的位置语法

- 在创建实例时，可以使用位置参数来声明记录的属性，并初始化属性：
- 在为记录的属性定义使用位置语法时，编译器将创建以下内容：
  - 为记录声明中提供的每个位置参数提供一个公共的自动实现的属性：对于 `record` 类型，为 `required` 只读属性；对于 `readonly record struct` 类型，为只读属性；对于 `record struct` 类型，为读写属性。
  - 在主构造函数上，它的参数与记录声明上的位置参数匹配。
  - 对于 `record struct` 类型，则是将每个字段设置为其默认值的无参数构造函数。

* 若要更改自动实现的属性的可访问性或可变性，或为访问器提供实现，可以在源中自行定义同名的属性，并从记录的位置参数初始化该属性。

```csharp
public record Person(string FirstName, string LastName, string Id)
{
    internal string Id { get; init; } = Id;
}
```

> 位置记录中的解构函数

- 为了支持将 `record` 对象能解构成元组，我们给 `record` 添加解构函数 `Deconstruct`。声明主构造函数的记录定义为位置记录，该位置记录会为主构造函数中的位置参数自动生成一个解构函数。

```csharp
record Person
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public Person(string firstName, string lastName) 
        => (FirstName, LastName) = (firstName, lastName);
    public void Deconstruct(out string firstName, out string lastName) 
        => (firstName, lastName) = (FirstName, LastName);
}
// 相当于
record Person(string FirstName, string LastName);
```

- 解构记录为元组。

```csharp
var (first, last) = new Person("Hello", "World");
record Person(string FirstName, string LastName);
```

- 重定义解构函数或重载解构函数 `Deconstruct`

```csharp
using System.Diagnostics;

var (first, last) = new Person("Hello", "World");
var (firstName, _, Number) = new Person("Hello", "World") { PhoneNumber = "5566-6655" };

record Person(string FirstName, string LastName)
{
    public string PhoneNumber { get; set; } = "";
    // 重定义
    public void Deconstruct(out string firstName, out string lastName)
    {
        Console.WriteLine("Use Deconstruct >> " + new StackFrame(0, true));
        (firstName, lastName) = (FirstName, LastName);
    }
    // 重载
    public void Deconstruct(out string firstName, out string lastName, out string PhoneNumber)
    {
        firstName = FirstName;
        lastName = LastName;
        PhoneNumber = this.PhoneNumber;
    }
}
```

> 值相等性

- 对于 `class` 类型，两个对象引用内存中的同一对象，则这两个对象相等。
- 对于 `struct` 类型，两个对象是相同的类型并且存储相同的值，则这两个对象相等。
- 对于 `record` 类型，如果两个对象是相同的类型且存储相同的值，则这两个对象相等。

```csharp
class Sample
{
    public record Person(string FirstName, string LastName, string[] PhoneNumbers);

    static void Main()
    {
        var phoneNumbers = new string[2];
        Person person1 = new("Nancy", "Davolio", phoneNumbers);
        Person person2 = new("Nancy", "Davolio", phoneNumbers);

        Console.WriteLine(person1 == person2); // output: True
        person1.PhoneNumbers[0] = "555-1234";
        Console.WriteLine(person1 == person2); // output: True
        Console.WriteLine(ReferenceEquals(person1, person2)); // output: False
    }
}
```

- 为实现值相等性，编译器合成了几种方法：
  - `Object.Equals(Object)` 的替代，无法显式声明此替代。
  - 运算符 `==` 和 `!=` 的替代，无法显式声明这些运算符。
  - `virtual` 或 `sealed` 的 `Equals(R? other)`，其中 `R` 是记录类型。此方法实现 `IEquatable<T>`，可以显式声明此方法，还应该提供 `GetHashCode` 的实现。
  - `Object.GetHashCode()` 的替代，可以显式声明此方法。
  - 提供返回 `Type` 的 `EqualityContract` 只读属性的实现，可以显式声明此属性。该属性在密封记录中是 `private` 的，在可继承的记录中是 `protected virtual` 的。由于在默认实现的 `GetHashCode` 方法中调用了 `EqualityContract`，因此不建议在此属性中调用 `GetHashCode` 方法。  

```csharp
using System.Diagnostics;

Person p1 = new("Hello", "World");
Person pClone = p1;
pClone.PhoneNumber = "6666-5555";
Console.WriteLine(p1);
Console.WriteLine(Object.ReferenceEquals(p1, pClone));   // true

var p2 = p1 with { PhoneNumber = "5566-6655" };         // with 调用复制构造函数
Console.WriteLine(p2);
Console.WriteLine(Object.ReferenceEquals(p1, p2));      // false

var p3 = p1 with { };               // with 调用复制构造函数
Console.WriteLine(p3 == p1);        // 调用 Person.Equals, true
Console.WriteLine(Object.ReferenceEquals(p1, p3));      // false

record Person(string FirstName, string LastName) : IEquatable<Person>
{
    protected virtual Type EqualityContract
    {
        get
        {
            Console.WriteLine("Use EqualityContract at " + new StackFrame(1).GetMethod().Name);
            return this.GetType();
        }
    }
    public override int GetHashCode()
    {
        Console.WriteLine("Use GetHashCode");
        return unchecked((EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295
               + EqualityComparer<string>.Default.GetHashCode(FirstName)) * -1521134295
               + EqualityComparer<string>.Default.GetHashCode(LastName));
    }
    public virtual bool Equals(Person? other)
    {
        Console.WriteLine("Use Equals");
        return (object)other != null
                && EqualityContract == other.EqualityContract
                && EqualityComparer<string>.Default.Equals(FirstName, other.FirstName)
                && EqualityComparer<string>.Default.Equals(LastName, other.LastName);
    }
    protected Person(Person origin)
    {
        Console.WriteLine("Use Clone");
        (FirstName, LastName) = origin;
        PhoneNumber = origin.PhoneNumber;
    }
    public string PhoneNumber { get; set; } = "";
}
/*
Person { FirstName = Hello, LastName = World, PhoneNumber = 6666-5555 }
True
Use Clone
Person { FirstName = Hello, LastName = World, PhoneNumber = 5566-6655 }
False
Use Clone
Use Equals
Use EqualityContract at Equals
Use EqualityContract at Equals
True
False
*/
```

> 非破坏性变化

- 若需要复制包含一些修改的实例，可以使用 `with` 表达式来实现非破坏性变化。`with` 表达式创建一个新的记录实例，该实例是现有记录实例的一个副本，并修改了指定的属性或字段。

```csharp
class Sample
{
    public record Person(string FirstName, string LastName)
    {
        public string[] PhoneNumbers { get; init; }
    }

    public static void Main()
    {
        Person person1 = new("Nancy", "Davolio") { PhoneNumbers = new string[1] };
        Console.WriteLine(person1);
        // output: Person { FirstName = Nancy, LastName = Davolio, PhoneNumbers = System.String[] }

        Person person2 = person1 with { FirstName = "John" };
        Console.WriteLine(person2);
        // output: Person { FirstName = John, LastName = Davolio, PhoneNumbers = System.String[] }
        Console.WriteLine(person1 == person2);
        // output: False

        person2 = person1 with { PhoneNumbers = new string[1] };
        Console.WriteLine(person2);
        // output: Person { FirstName = Nancy, LastName = Davolio, PhoneNumbers = System.String[] }
        Console.WriteLine(person1 == person2); 
        // output: False

        person2 = person1 with { };
        Console.WriteLine(person1 == person2); 
        // output: True
    }
}
```

- `with` 表达式可以设置位置属性或使用标准属性语法创建的属性。显式声明属性必须有一个 `init` 或 `set` 访问器才能在 `with` 表达式中进行更改。
- `with` 表达式的结果是一个浅的副本，这意味着对于引用属性，只复制对实例的引用。原始记录和副本最终都具有对同一实例的引用。
- 编译器合成了一个克隆方法 `Clone` 和一个复制构造函数 `recordType(recordType origin)`，虚拟克隆方法返回由复制构造函数初始化的新记录。用户不能替代克隆方法，也不能在任意记录类型中创建名为 `Clone` 的成员。克隆方法的实际名称是由编译器生成的，当使用 `with` 表达式时，编译器将创建调用克隆方法的代码，然后设置 `with` 表达式中指定的属性。复制构造函数可以被显式定义，在非密封记录中必须是 `public` 或 `protected`。
- `with` 表达式会调用类型的复制构造函数。

```csharp
Person p1 = new("Hello", "World");
Person pClone = p1;
pClone.PhoneNumber = "6666-5555";
Console.WriteLine(p1);
Console.WriteLine(Object.ReferenceEquals(p1, pClone));   // true

var p2 = p1 with { PhoneNumber = "5566-6655" };
Console.WriteLine(p2);
Console.WriteLine(Object.ReferenceEquals(p1, p2));      // false

var p3 = p1 with { };
Console.WriteLine(Object.ReferenceEquals(p1, p3));      // false, 值相等性

sealed record Person(string FirstName, string LastName)
{
    private Person(Person origin)
    {
        Console.WriteLine("Use Clone");
        (FirstName, LastName) = origin;
        PhoneNumber = origin.PhoneNumber;
    }
    public string PhoneNumber { get; set; } = "";
}
```

> 用于显示的内置格式设置

- 记录类型具有编译器生成的 `ToString` 方法，可显式公共属性和字段的名称和值。 `ToString` 方法返回一个格式如下的字符串：`<record type name> { <property name> = <value>, <property name> = <value>, ...}`，其中每个 `<value>` 打印的字符串是属性或字段对应类型的 `ToString()`。为了实现此功能，编译器在 `record class` 类型中合成了一个虚拟 `PrintMembers` 方法和一个 `ToString` 替代，此成员在 `record struct` 类型中为 `private`。

```csharp
using System.Text;

PointArray X = new((0, 0), (1, 1), (2, 2), (3, 3), (4, 4));
Console.WriteLine(X);   // Output: (0,0),(1,1),(2,2),(3,3),(4,4)

public record struct Point(int x, int y)
{
    public static implicit operator Point((int, int) p) => new Point(p.Item1, p.Item2);
    public override string ToString() => $"({this.x},{this.y})";
}
public readonly record struct PointArray(params Point[] points)
{
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        if (points.Length > 0)
        {
            sb = new StringBuilder(points[0].ToString());
            foreach (Point p in points[1..points.Length])
                sb.Append("," + p.ToString());
        }
        return sb.ToString();
    }
}
```

- 自定义 PrintMembers 方法

```csharp
using System.Text;

PointArray X = new((0, 0), (1, 1), (2, 2), (3, 3), (4, 4));
Console.WriteLine(X);   
// Output: PointArray { points = { (0,0), (1,1), (2,2), (3,3), (4,4) } }

public record struct Point(int x, int y)
{
    public static implicit operator Point((int, int) p) => new Point(p.Item1, p.Item2);
    public override string ToString() => $"({this.x},{this.y})";
}
public readonly record struct PointArray(params Point[] points)
{
    public readonly int Length => points.Length;
    private bool PrintMembers(StringBuilder sb)
    {
        if (points.Length == 0)
            return false;
        else
        {
            sb.Append($"points = {{ {points[0].ToString()}");
            foreach (Point p in points[1..points.Length])
                sb.Append(", " + p.ToString());
            sb.Append(" }");
            return true;
        }
    }
}
```

> 继承

- 一条记录可以从另一条记录继承。派生记录为基本记录主构造函数中的所有参数声明位置参数，基本记录声明并初始化这些属性；派生记录不会隐藏它们，而只会创建和初始化未在其基本记录中声明的参数的属性。
- 要使两个记录变量相等，运行时类型必须相等。包含变量的类型可能不同，但相等性测试依赖于实际对象的运行时类型，而不是声明的变量类型。
- `with` 表达式结果的运行时间类型与表达式操作数相同：运行时类型的所有属性都会被复制，但用户只能设置编译时类型的属性。
- 派生记录类型的合成 `PrintMembers` 方法并调用基实现 `base.PrintMembers()`。结果是派生类型和基类型的所有公共属性和字段都包含在 `ToString` 输出中。派生记录也会重新合成基记录的 `EqualityContract`、`GetHashCode`、`Deconstruct` 方法。 

```csharp
class Sample
{
    public abstract record Person(string FirstName, string LastName);
    public record Teacher(string FirstName, string LastName, int Grade)
        : Person(FirstName, LastName);
    public record Student(string FirstName, string LastName, int Grade)
        : Person(FirstName, LastName);

    public static void Main()
    {
        Person teacher = new Teacher("Nancy", "Davolio", 3);
        Console.WriteLine(teacher);
        // output: Teacher { FirstName = Nancy, LastName = Davolio, Grade = 3 }

        /* 相等性测试 */
        Person student = new Student("Nancy", "Davolio", 3);
        Console.WriteLine(teacher == student); // output: False
        Student student2 = new Student("Nancy", "Davolio", 3);
        Console.WriteLine(student2 == student); // output: True

        /* with 表达式 */
        Person clone_teacher = teacher with { FirstName = "Tom" }; // 无法定义 Grade，虽然在运行时类型包含此属性
        Teacher teacher2 = (Teacher)teacher with { Grade = 6 };
        Console.WriteLine(teacher2);
        // output: Teacher { FirstName = Nancy, LastName = Davolio, Grade = 6 }

        /* 解构函数 */
        var (first, second) = (Teacher)teacher;  // 支持基记录的解构函数
        var (first2, second2, grade) = (Teacher)teacher; // 在 Teacher 重新生成的解构函数
    }
}
```

<br>


---
### 匿名类型

- 匿名类型提供了一种方便的方法，可用来将一组只读属性封装到单个对象中，而无需首先显式定义一个类型，每个属性的类型由编译器推断。类型名由编译器生成，并且不能在源代码级使用，可结合使用 `new` 运算符和对象初始值设定项创建匿名类型。
- 匿名类型包含一个或多个公共只读属性。无法包含其他种类的类成员（如方法或事件）。用来初始化属性的表达式不能为 null、匿名函数或指针类型。

```csharp
var v = new { Amount = 108, Message = "Hello" };
Console.WriteLine(v.Amount + v.Message);
```

- 匿名类型是 `class` 类型，它们直接派生自 `object`，并且无法强制转换为除 `object` 外的任何类型。如果程序集中的两个或多个匿名对象初始值指定了属性序列，这些属性采用相同顺序且具有相同的名称和类型，则编译器将对象视为相同类型的实例，它们共享同一编译器生成的类型信息。
- 无法将字段、属性、时间或方法的返回类型声明为具有匿名类型。同样，也不能将方法、属性、构造函数或索引器的形参声明为具有匿名类型。要将匿名类型或包含匿名类型的集合作为参数传递给某一方法，可将参数作为类型 `object` 进行声明。

> 应用

- 匿名类型通常用在查询表达式的 `select` 子句中，以便返回源序列中每个对象的属性子集。

```csharp
var productQuery =
    from prod in products
    select new { prod.Color, prod.Price };

foreach (var v in productQuery)
    Console.WriteLine("Color={0}, Price={1}", v.Color, v.Price);
```

- 还可以按另一种类型（类、结构或另一个匿名类型）的对象定义字段。它通过使用保存此对象的变量来完成。

```csharp
var product = new Product();
var bonus = new { note = "You won!" };
var shipment = new { address = "Nowhere St.", product };
var shipmentWithBonus = new { address = "Somewhere St.", product, bonus };
```

- 可通过将隐式键入的本地变量与隐式键入的数组相结合创建匿名键入的元素的数组。

```csharp
var anonArray = new[] { new { name = "apple", diam = 4 }, new { name = "grape", diam = 1 }};
```

- 匿名类型支持采用 `with` 表达式形式的非破坏性修改。

```csharp
var apple = new { Item = "apples", Price = 1.35 };
var onSale = apple with { Price = 0.79 };
Console.WriteLine(apple);
Console.WriteLine(onSale);
```

---
### 隐式类型

- 声明局部变量时，可以让编译器从初始化表达式推断出变量的类型。使用 `var` 关键字声明隐式类型，隐式类型只能应用于本地方法范围内的变量。`var` 的常见用途是用于构造函数调用表达式，例如 `var xs = new List<int>();`。

```csharp
var Ps = new PointArray(PointArray.RandomPoints(50));
Ps.AddPoints(PointArray.RandomPoints(50));
var first_Ps = Ps.GetPointsInQuadrant(1);
var Second_Ps = Ps.GetPointsInQuadrant(2);
var Third_Ps = Ps.GetPointsInQuadrant(3);
var Forth_Ps = Ps.GetPointsInQuadrant(4);

Print(first_Ps);
Print(Second_Ps);
Print(Third_Ps);
Print(Forth_Ps);
// -----------------------------------------------
static void Print<T>(in IEnumerable<T> arr)
{
    foreach (var item in arr)
        Console.WriteLine(item.ToString());
}

record struct PointArray(params (int x, int y)[] points)
{
    public readonly int PointsCount => points.Length;
    private bool PrintMembers(System.Text.StringBuilder sb)
    {
        if (points.Length > 0)
        {
            sb.Append(points[0]);
            foreach (var p in points[1..])
                sb.Append(" ," + p);
            return true;
        }
        return false;
    }
    public static (int, int)[] RandomPoints(int count)
    {
        int seed = DateTime.Now.Microsecond;
        Random r = new Random(seed);
        var ps = new (int, int)[count];
        for (int i = 0; i < count; i++)
            ps[i] = (r.Next(-128, 128), r.Next(-128, 128));
        return ps;
    }

    public void AddPoints(params (int x, int y)[] points)
    {
        (int x, int y)[] newPoints = new (int x, int y)[points.Length + this.points.Length];
        Array.Copy(this.points, newPoints, this.points.Length);
        Array.Copy(points, 0, newPoints, this.points.Length, points.Length);
        this.points = newPoints;
    }
    public (int, int)[] GetPointsInQuadrant(uint order)
    {
        if (order < 0 || order > 4)
            return default;
        var state = static delegate (int x, int y, uint order)
        {
            return order switch
            {
                1 => x > 0 && y > 0,
                2 => x > 0 && y < 0,
                3 => x < 0 && y < 0,
                4 => x < 0 && y > 0,
            };
        };
        var ps = from (int x, int y) p in this.points
                 where state(p.x, p.y, order)
                 select p;
        return ps.ToArray();
    }
}
```

---
### 指针类型

#### 不安全上下文

- C# 支持 `unsafe` 上下文，用户可在其中编写不可验证的代码。在 `unsafe` 上下文中，代码可使用指针、分配和释放内存块，以及使用函数指针调用方法。可以将方法、类型和代码块定义为不安全。调用需要指针的本机函数时，需使用不安全代码，因此可能会引发安全风险和稳定性风险。在某些情况下，通过移除数组绑定检查，不安全代码可提高应用程序的性能。
- 指针不能指向引用（`ref`）或包含引用的结构，因为无法对对象引用进行垃圾回收，即使有指针指向它也是如此。垃圾回收器并不跟踪是否有任何类型的指针指向对象。

```csharp
int* p;         // p 是指向整数的指针。
int** p;        // p 是指向整数的指针的指针。
int*[] p;       // p 是指向整数的指针的一维数组。
char* p;        // p 是指向字符的指针。
void* p;        // p 是指向未知类型的指针。

int* p1, p2, p3;    // Ok
int *p1, *p2, *p3;  // Invalid in C#
```

- 无法对 `void*` 类型的指针应用间接寻址运算符，但是可以使用强制转换将 `void` 指针转换为任何其他指针类型，反之亦然。
- 指针可以为 null。将间接寻址运算符应用于 null 指针将导致空引用异常。
- 在方法之间传递指针可能会导致未定义的行为。

```csharp
unsafe
{
    fixed (void* PEmptyString = &string.Empty)
        Console.WriteLine(Convert.ToString((long)(nuint)PEmptyString, 16));  // 输出指针地址值

    int* p = null;
    int a = *p;   // ERROR: System.NullReferenceException: “Object reference not set to an instance of an object.”
}
```

> 获取对象的地址

```csharp
int[] arr = [10, 20, 30, 40, 50];

unsafe
{
    // 必须将对象固定在堆上，这样它在使用时，垃圾回收器不会移动它
    fixed (int* p = arr) // 或 &arr[0]. &arr[index]
    {
        // 固定指针无法移动, 无法赋值
        //  p++;  // CS1656
        // 所以创建另一个指针来显示它的递增。
        int* p2 = p;
        Console.WriteLine(*p2);  // 10
        // 由于指针的类型，增加 p2 会使指针增加其基础类型大小的字节：4
        p2 += 1;
        Console.WriteLine(*p2);  // 20
        p2 += 1;
        Console.WriteLine(*p2);  // 30

        Console.WriteLine("--------");
        // 对 p 解引用并递增会改变 arr[0] 的值
        Console.WriteLine(*p);   // 10
        *p += 1;
        Console.WriteLine(*p);   // 11
        *p += 1;
        Console.WriteLine(*p);   // 12
    }
    Console.WriteLine(arr[0]);  // 12
}
```

<br>

#### 指针相关的运算符和语句

- `*`：执行指针间接寻址。
- `->`：通过指针访问结构或类对象的成员。
- `[]`：为指针建立索引。
- `&`：获取变量的地址。
- `++` 和 `--`：递增和递减指针。
- `+` 和 `-`：执行指针算法。
- `==`、`!=`、`<`、`>`、`<=` 和 `>=`：比较指针。
- `stackalloc`：在堆栈上分配内存。
- `fixed` 语句：临时固定变量以便找到其地址。

<br>

#### 固定大小的缓冲区

- 可以使用 `fixed` 关键字来创建在数据结构中具有固定大小的数组的缓冲区。当编写与其他语言或平台的数据源进行互操作的方法时，固定大小的缓冲区很有用。固定大小的缓冲区可以采用允许用于常规结构成员的任何属性或修饰符。唯一的限制是数组类型必须为 `bool`、`byte`、`char`、`short`、`int`、`long`、`sbyte`、`ushort`、`uint`、`ulong`、`float` 或 `double`。
  
```csharp
internal unsafe struct Buffer
{
    public fixed char fixedBuffer[128];
}
```

- 在安全代码中，包含数组的 C# 结构不包含该数组的元素，而是包含对该数组的引用。当在不安全的代码块中使用数组时，可以在结构中嵌入固定大小的数组。使用 `fixed` 语句获取指向数组第一个元素的指针，通过此指针访问数组的元素。`fixed` 语句将 `fixedBuffer` 实例字段固定到内存中的特定位置。

```csharp
internal unsafe struct Buffer
{
    public fixed char fixedBuffer[128];
}
internal unsafe class Example
{
    public Buffer buffer = default;
}
private static void AccessEmbeddedArray()
{
    var example = new Example();
    unsafe
    {
        // Pin the buffer to a fixed location in memory.
        fixed (char* charPtr = example.buffer.fixedBuffer)
        {
            *charPtr = 'A';
        }
        // Access safely through the index:
        char c = example.buffer.fixedBuffer[0];
        Console.WriteLine(c);

        // Modify through the index:
        example.buffer.fixedBuffer[0] = 'B';
        Console.WriteLine(example.buffer.fixedBuffer[0]);
    }
}
```

- 固定大小的缓冲区使用 `System.Runtime.CompilerServices.UnsafeValueTypeAttribute` 进行编译，它指示公共语言运行时 CLR 某个类型包含可能溢出的非托管数组。

```csharp
internal unsafe struct Buffer
{
    public fixed char fixedBuffer[128];
}
// 为 Buffer 生成 C# 的编译器的特性如下
internal struct Buffer
{
    [StructLayout(LayoutKind.Sequential, Size = 256)]
    [CompilerGenerated]
    [UnsafeValueType]
    public struct <fixedBuffer>e__FixedBuffer
    {
        public char FixedElementField;
    }

    [FixedBuffer(typeof(char), 128)]
    public <fixedBuffer>e__FixedBuffer fixedBuffer;
}
```

- 使用 `stackalloc` 分配的内存还会在 CLR 中自动启用缓冲区溢出检测功能

```csharp
unsafe
{
    int* pSafe = stackalloc int[10];
    for (int i = 0; i < 100; i++)
        *(pSafe + i) = i;
    // 进行缓冲区溢出检查，溢出时引发异常 System.AccessViolationException

    Example ex = new Example();
    fixed (int* pUnsafe = ex.buffer.fixedBuffer)
    {
        for (int i = 0; i < 100; i++)
            *(pUnsafe + i) = i;   // 不进行缓冲区溢出检查
    }
}
internal unsafe struct Buffer
{
    public fixed int fixedBuffer[10];
}
internal unsafe class Example
{
    public Buffer buffer = default;
}
```

<br> 

#### 函数指针

- C# 提供 `delegate` 类型来定义安全函数指针对象。 调用委托时，需要实例化从 `System.Delegate` 派生的类型并对其 `Invoke` 方法进行虚拟方法调用，该虚拟调用使用 IL 指令 `callvirt`
- 可以使用 `delegate*` 语法定义函数指针。编译器将使用 IL 指令 `calli` 指令来调用函数，而不是实例化为委托对象并调用 `Invoke`。在性能关键的代码路径中，使用 IL 指令 `calli` 效率更高。

```csharp
// 委托定义参数
public static T Combine<T>(Func<T, T, T> combinator, T left, T right) => combinator(left, right);
// 函数指针定义参数
public static T UnsafeCombine<T>(delegate*<T, T, T> combinator, T left, T right) => combinator(left, right);
```

- 函数指针只能在 `unsafe` 上下文中声明，只能在静态成员方法或静态本地方法使用地址运算符 `&`。

```csharp
unsafe
{
    // 函数指针声明和调用
    delegate*<int, int> pAbs = &Abs;
    Console.WriteLine(pAbs(-999));  // 999
    // 本地静态方法
    static int Abs(int val) => Math.Abs(val);
}
```

> 函数指针声明语法

```csharp
delegate * calling_convention_specifier? <parameter_list, return_type> 

calling_convention_specifier? : 可选的调用约定说明符, 默认为 managed
    - managed : 默认调用约定
    - unmanaged : 非托管调用约定, 未显式指定调用约定类别, 则使用运行时平台默认语法
    - unmanaged [Calling_convertion|,Calling_convertion...] : 指定特定的非托管调用约定, 一到若干个
        - Calling_convertion : 调用约定
                - Cdecl : 调用方清理堆栈
                - stdcall : 被调用方清理堆栈, 这是从托管代码调用非托管函数的默认约定
                - Thiscall : 指定方法调用的第一个参数是 this 指针, 该指针存储在寄存器 ECX 中
                - Fastcall : 调用约定指定在寄存器中传递函数的参数 (如果可能), NET 可能不支持 
                - MemberFunction : 指示使用的调用约定是成员函数变体
                - SuppressGCTransition : 指示方法应禁止 GC 转换作为调用约定的一部分
```

- 可以对函数指针显式使用调用约定说明符 `unmanaged`、`managed`，默认使用 `managed` 调用约定（使用托管方法）。
- 使用 `unmanaged` 调用约定时，可以显式指定一个或多个 ECMA-335 调用约定（`Cdecl`、`Stdcall`、`Fastcall`、`Thiscall`）或 `MemberFunction`、`SuppressGCTransition`。未显式指定的 `unmanaged` 调用约定，则指示 CLR 选择平台的默认调用约定（在运行时基于平台选择调用约定）。
- 函数调用约定，是指当一个函数被调用时，函数的参数会被传递给被调用的函数，返回值会被返回给调用函数。函数的调用约定就是描述参数是怎么传递和由谁平衡堆栈的，当然还有返回值。

```csharp
unsafe class Sample
{
    // 委托
    public static T Combine<T>(Func<T, T, T> combinator, T left, T right) => combinator?.Invoke(left, right);
    // 函数指针
    public static T UnsafeCombine<T>(delegate*<T, T, T> combinator, T left, T right) => combinator(left, right);

    public static T ManagedCombine<T>(delegate* managed<T, T, T> combinator, T left, T right) => combinator(left, right);

    public static T CDeclCombine<T>(delegate* unmanaged[Cdecl]<T, T, T> combinator, T left, T right) => combinator(left, right);
    
    public static T StdcallCombine<T>(delegate* unmanaged[Stdcall]<T, T, T> combinator, T left, T right) => combinator(left, right);
    
    public static T FastcallCombine<T>(delegate* unmanaged[Fastcall]<T, T, T> combinator, T left, T right) => combinator(left, right);
    
    public static T ThiscallCombine<T>(delegate* unmanaged[Thiscall]<T, T, T> combinator, T left, T right) => combinator(left, right);
    
    public static T UnmanagedCombine<T>(delegate* unmanaged<T, T, T> combinator, T left, T right) => combinator(left, right);
}
```

---
### 类型默认值

- 任何引用类型：`null`。
- 任何内置数值类型：`0`。
- `bool`：`false`。
- `char`：`\0`。
- `enum`：`(E)0`。
- `struct`：成员各类型默认值。
- 可为 null 的值类型：`HasValue` 属性为 `false` 且 `Value` 属性未定义的实例，即 `null`。

> 默认值表达式

- 默认值表达式生成类型的默认值。有两种类型的表达式：`default` 运算符调用和 `default` 文本：
  - `default` 运算符的实参必须是类型或类型形参的名称。
  - `default` 文本用于生成类型的默认值，可用于变量赋值、可选方法参数的默认值、`return` 语句、方法参数传递。

```csharp
int num = default(int);         // default 运算符
string str = default;           // default 文本值
```

---


---