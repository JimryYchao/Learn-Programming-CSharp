## CSharp 语言介绍

```csharp
using System;
class Hello{
    static void Main(){
        Console.WriteLine("Hello World!");
    }
}
```

---
### C# 语言简介

C# 源于 C 语言系列，是一种面向对象的、面向组件的、类型安全的新式编程语言。开发人员利用 C# 能够生成在 .NET 中运行的多种安全可靠的应用程序。


---
### .NET 体系结构

C# 程序在 .NET 上运行，而 .NET 是名为公共语言运行时（CLR）的虚拟执行系统和一组类库。CLR 是 Microsoft 对公共语言基础结构（CLI）国际标准的实现。CLI 是创建执行和开发环境的基础，语言和库可以在其中无缝地协同工作。

C# 源代码被编译成符合 CLI 规范的中间语言（IL）。IL 代码和资源（如位图和字符串）存储在扩展名通常为 `.dll` 的程序集中。程序集包含一个介绍程序集的类型、版本和区域性的清单。

执行 C# 程序时，程序集将加载到 CLR。CLR 会直接执行实时（JIT）编译，将 IL 代码转换成本机指令。CLR 可提供自动垃圾回收、异常处理和资源管理相关的服务。CLR 执行的代码有时称为 “托管代码”。而 “非托管代码” 被编译成面向特定平台的本机语言。

语言互操作性是 .NET 的一项重要功能。C# 编译器生成的 IL 代码符合公共类型规范（CTS）。通过 C# 生成的 IL 代码可以与通过 .NET 版本的 F#、Visual Basic、C++ 生成的代码进行交互。还有 20 多种与 CTS 兼容的语言。单个程序集可包含多个用不同 .NET 语言编写的模块。这些类型可以相互引用，就像它们是用同一种语言编写的一样。

.NET 还包含大量库。这些功能包括文件输入输出、字符串控制、XML 分析、Web 应用程序框架和 Windows 窗体控件。典型的 C# 应用程序广泛使用 .NET 类库来处理常见的 “管道” 零碎工作。

---
### 类型与变量

类型定义 C# 中的任何数据的结构和行为。类型的声明可以包含其成员、基类型、它实现的接口和该类型允许的操作。变量是用于引用特定类型的实例的标签。标识符是变量名称。

C# 有两种类型：值类型和引用类型。值类型的变量直接包含它们的数据。引用类型的变量存储对数据（称为 “对象”）的引用。

C# 的值类型可分为：简单类型、枚举类型、结构类型、可以为 null 的值类型和元组值类型。C# 引用类型可分为类类型、接口类型、数组类型和委托类型。

C# 程序使用类型声明创建新类型。用户可定义以下六种 C# 类型：类类型、结构类型、接口类型、枚举类型、委托类型和元组值类型。还可以声明 record 类型（record struct 或 record class）。记录类型具有编译器合成成员，主要用于存储值，关联行为最少。

C# 采用统一的类型系统，因此任意类型的值都可视为 `object`。 每种 C# 类型都直接或间接地派生自 `object` 类类型，而 `object` 是所有类型的最终基类。

> 装箱与拆箱

- 装箱：将值类型的值分配给 `object` 对象引用时，会分配一个 “箱” 来保存此值。该箱是引用类型的实例，此值会被复制到该箱。
- 拆箱：当 `object` 引用被显式转换成值类型时，将检查引用的 `object` 是否是具有正确值类型的箱。 如果检查成功，则会将箱中的值复制到值类型。

---










---
### C# 类型和成员

#### 类和对象

类是最基本的 C# 类型。类是一种数据结构，可在一个单元中就将状态（字段）和操作（方法和其他函数成员）结合起来。 类为类实例（亦称为 “对象”）提供了定义。类支持继承和多形性，即派生类可以扩展和专门针对基类的机制。

```c
/* 类的定义 */
public class Point
{
    public int X { get; }
    public int Y { get; }
    public Point(int x, int y) => (X, Y) = (x, y);      // 构造函数
}
/* 类的实例 */
{ // 某个方法实现
    var p1 = new Point(0, 0);
    var p2 = new Point(10, 20);     
}
```

<br>

#### 类型参数



<br>

#### 基类

类声明可以指定基类，并继承其基类的成员。继承意味着一个类隐式包含其基类的几乎所有成员。类不继承实例、静态构造函数以及终结器。派生类可以在其继承的成员中添加新成员，但无法删除继承成员的定义。

```csharp
public class Point3D(int x,int y, int z) : Point(x, y)
{
    public int Z => z;
}
```

可以将类类型隐式转换成其任意基类类型。类类型的变量可以引用相应类的实例或任意派生类的实例。

```csharp
{
    Point p1 = new(10, 20);
    Point p2 = new Point3D(10, 20, 30);
}
```

<br>

#### 结构

结构类型是较为简单的类型，其主要目的是存储数据值。结构不能声明基类型，它们从 `System.ValueType` 隐式派生。因此不能从 struct 类型派生其他 struct 类型。这些类型已隐式密封。

```c
public struct Point(double x, double y)
{
    public double X { get => x; set => x = value; }
    public double Y { get => y; set => y = value; }
}
```

<br>

#### 接口

接口定义了可由类和结构实现的协定，定义接口来声明在不同类型之间共享的功能。接口可以包含方法、属性、事件和索引器。接口通常不提供所定义成员的实现，仅指定必须由实现接口的类或结构提供的成员。接口可以采用 “多重继承”。

```csharp
interface IControl{
    void Paint();
}
interface ITextBox : IControl{
    void SetText(string text);
}
interface IListBox : IControl{
    void SetItems(string[] items);
}
interface IComboBox : ITextBox, IListBox { }
```

类和结构可以实现多个接口。

```csharp
interface IDataBound{
    void Bind(Binder b);
}
class EditBox: IControl, IDataBound{
    public void Paint() { }
    public void Bind(Binder b) { }
}
```

当类或结构实现特定接口时，此类或结构的实例可以隐式转换成相应的接口类型。

```csharp
{
    EditBox editBox = new();
    IControl control = editBox;
    IDataBound dataBound = editBox;
}
```

<br>

#### 枚举

枚举类型定义了一组常数值。枚举支持定义一个使用标志组合的枚举项。

```csharp
public enum Season
{
    None = 0,
    Summer = 1,
    Autumn = 2,
    Winter = 4,
    Spring = 8,
    All = Summer | Autumn | Winter | Spring
}
```

<br>

#### 可以为 null 的类型

任何类型的变量都可以声明为 “不可为 null” 或 “可为 null” 类型。可为 null 的变量包含一个额外的 null 值，表示没有值。

可为 null 的值类型（结构或枚举）由 `System.Nullable<T>` 表示。不可为 null 和可为 null 的引用类型都由基础引用类型表示。

```csharp
// 可为 null 的值类型
int? optionalInt = default; 
optionalInt = 5;
// 可为 null 的引用类型
string? optionalText = default;
optionalText = "Hello World.";
```

<br>

#### 元组

C# 支持元组，通过声明 `(` 和 `)` 之间的成员的类型和名称来实例化元组。

```c
{
    (int num1, int num2) N = (1, 2);
    (int, double) N2 = (0, 4.5);
}
```

---