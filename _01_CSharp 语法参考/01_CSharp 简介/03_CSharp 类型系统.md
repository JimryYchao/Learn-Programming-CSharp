## CSharp 类型系统

C# 是一种强类型语言，每个变量和常量、求值的表达式、方法形参与返回值等都有一个类型。.NET 类库定义了内置数值类型和表示各种构造的复杂类型，其中包括文件系统、网络连接、对象的集合和数组以及日期。
类型中可存储的信息有：类型变量所需的存储空间、可以表示的最大值和最小值、包含的成员（方法、字段、事件等）、继承自的基类型、它实现的接口、允许执行的运算种类等。
编译器将类型信息作为元数据嵌入可执行文件中。公共语言运行时（CLR）在运行时使用元数据，以在分配和回收内存时进一步保证类型安全性。

> 变量声明中指定类型

```csharp
// 声明
float score;
string studentName;
// 声明并实例化
char ch = 'C';
var limit = 3;  // 匿名类型
int[] scores = {100,99,89};
```

---
### 内置类型

C# 提供了一组标准的内置类型。这些类型表示整数、浮点值、布尔表达式、文本字符、十进制值和其他数据类型。还有内置的 `string` 和 `object` 类型。

```csharp
// 整数
byte b = 1;
int i = 0xffff;  // 十六进制
// 浮点值
double d = 1.0;
float f = 1.0f;
// 布尔
bool state = true;
// 文本字符
char ch = 'C';
// 十进制值
decimal digit = 1980;
// 字符串
string str = "Hello world";
// object
object obj = null;
```

---
### 自定义类型

可以使用 `struct`、`class`、`interface`、`enum` 和 `record` 构造来创建自己的自定义类型。.NET 类库本身是一组自定义类型。

```csharp
class MyClass{}
struct MyStruct{}
interface IMyInterface{}
enum MyEnum{}
record class MyClass_R{}
record struct MyStruct_R{}
```

---
### 通用类型系统

对于 .NET 中的类型系统，它支持继承原则。类型可以派生自其他类型（称为基类型）。派生类型继承（有一些限制）基类型的方法、属性和其他成员。所有类型派生自单个基类型，即 `System.Object (C# keyword: object)`。这样的统一类型层次结构称为通用类型系统（CTS）。

CTS 中的每种类型被定义为值类型或引用类型。这些类型包括 .NET 类库中的所有自定义类型以及用户定义类型。使用 `struct` 关键字定义的类型是值类型；所有内置数值类型都是 `struct`。使用 `class` 或 `record` 关键字定义的类型是引用类型。

> 类、结构或记录声明类似于一张蓝图，用于在运行时创建实例或对象：
- 类是引用类型，创建类型的对象后，向其分配对象的变量仅保留对相应内存的引用（将对象引用分配给新变量后，新变量会引用原始对象）。
- 结构是值类型，创建结构时，向其分配结构的变量保留结构的实际数据（将结构分配给新变量时，会复制结构）。
- 记录类型可以是引用类型（`record class`）或值类型（`record struct`）

<br>

#### 值类型

值类型派生自 `System.ValueType`（派生自 `System.Object`）。值类型变量直接包含其值，结构的内存在声明变量的任何上下文中进行内联分配。对于值类型变量，没有单独的堆分配或垃圾回收开销。

值类型分为两类：`struct` 和 `enum`
- 内置的数值类型是结构，使用 `struct` 关键字可以创建用户自定义值类型。
- `enum` 枚举定义的是一组已命名的整型常量。所有枚举从 `System.Enum`（继承自 `System.ValueType`）继承。

<br>

#### 引用类型

定义为 `class`、`record class`、`delegate`、数组或 `interface` 的类型是引用类型。引用类型在声明时包含 null 值，直到用户为其分配类型的实例或 `new` 创建一个。接口对象无法使用 `new` 运算符。


```csharp
MyClass myClass = new MyClass();
MyClass myClass2 = myClass;

IMyInterface myInterface = myClass;
IMyInterface myInterface = new MyClass();
```

创建对象时，会在托管堆上分配内存。变量只保留对对象位置的引用。对于托管堆上的类型，在分配内存和回收内存时都会产生开销。“垃圾回收” 是 CLR 的自动内存管理功能，用于执行回收。

所有数组都是引用类型，即使元素是值类型。数组隐式派生自 `System.Array` 类。

引用类型完全支持继承。创建类时，可以从其他任何未定义为密封的接口或类继承。

<br>

#### 文本值的类型

在 C# 中，文本值从编译器接收类型。不同的文本值可以添加后缀用于指定文本应采用的类型，编译器就会自动推断文本值的类型。由于文本已类型化，且所有类型最终都是从 `System.Object` 派生：

```csharp
string s = "The answer is " + 5.ToString();
Console.WriteLine(s);
// Outputs: "The answer is 5"

Type type = 12345.GetType();
Console.WriteLine(type);
// Outputs: "System.Int32"
```

<br>

#### 泛型类型

可使用一个或多个类型参数声明的类型，用作实际类型（具体类型）的占位符 。客户端代码在创建类型实例时提供具体类型，这种类型称为泛型类型。

通过使用类型参数，可重新使用相同类以保存任意类型的元素，且无需将每个元素转换为对象。泛型集合类称为强类型集合，因为编译器知道集合元素的具体类型，并能在编译时引发错误。

泛型类定义类型参数。类型参数是跟在类名后面的类型参数名称列表。类声明的主体中可以使用类型参数来定义类成员。
    
声明为需要使用类型参数的类类型被称为泛型类类型。结构、接口和委托类型也可以是泛型。

```csharp
public class Pair<TFirst, TSecond> // <,> 类型参数列表
{
    public TFirst First { get; }
    public TSecond Second { get; }
    public Pair(TFirst first, TSecond second) => 
        (First, Second) = (first, second);
}
```

使用泛型类时，必须为每个类型参数提供类型自变量

```c
{
    var pair = new Pair<int, string>(1, "two");
    int i = pair.First;     //TFirst int
    string s = pair.Second; //TSecond string
}
```

<br>

#### 隐式类型、匿名类型和可以为 null 的值类型

隐式类型：使用 `var` 关键字隐式键入一个局部变量，变量仍可在编译时获取类型，但类型是由编译器提供。
匿名类型：匿名类型提供了一种方便的方法，可用来将一组只读属性封装到单个对象中，而无需首先显式定义一个类型。
可为 null 值类型：普通值类型不能具有 null 值，可以在类型后面追加 `?`，创建可为空的值类型。`int?` 是还可以包含值 null 的 `int` 类型。可以为 null 的值类型是泛型结构类型 `System.Nullable<T>` 的实例。

```csharp
var num = 1.0;  // double 隐式类型声明
var v = {Amount = 100, Message = "hello world"};   // 定义匿名类型
int? n_num = default;   // 声明 int?, 并赋值 null 
```

<br>

#### 编译时类型和运行时类型

变量可以具有不同的编译时和运行时类型。编译时类型是源代码中变量的声明或推断类型。运行时类型是该变量所引用的实例的类型。

```csharp
// 运行时和编译时类型相同
string message = "This is a string of characters";

// 运行时和编译时类型不同
object anotherMessage = "This is another string of characters";
IEnumerable<char> someCharacters = "abcdefghijklmnopqrstuvwxyz";
    // 运行时为 string
```

---
### 命名空间

命名空间具有以下属性：
- 它们组织大型代码项目。
- 通过使用 `.` 运算符分隔它们。
- `using` 指令可免去为每个类指定命名空间的名称。
- `global` 命名空间是 “根” 命名空间：`global::System` 始终引用 `.NET System` 命名空间。

```csharp
namespace SampleNamespace{
     class SampleClass{
        public void SampleMethod(){
            System.Console.WriteLine("SampleMethod inside SampleNamespace");
        }
    }
}
```

> C#10 命名空间简写

从 C#10 开始，可以为该文件中定义的所有类型声明一个命名空间。

```csharp
namespace SampleNamespace;
class AnotherSampleClass{
    public void AnotherSampleMethod(){
        System.Console.WriteLine("SampleMethod inside SampleNamespace");
    }
}
```

---
### 类

- 使用 class 关键字声明类，使用 new 运算符显式创建类的实例。一个类中可包含：
  - 构造函数：创建类或结构的实例时，会调用其构造函数。构造函数可重载。
  - 常量：使用 `const` 定义静态访问的不可变值，且仅支持 C# 内置类型，不包括 `object`、用户定义类型等。常量在声明时初始化。
  - 字段：字段是在类或结构中直接声明的任意类型的变量，字段是其包含类型的成员。可以声明静态或实例字段。
  - 


---
### 记录


---
### 接口


---
### 泛型


---
### 匿名类型

