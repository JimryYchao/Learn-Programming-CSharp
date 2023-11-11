## CSharp 语言集成查询 LINQ

---
### 语言集成查询

- 语言集成查询 LINQ 是一系列直接将查询功能集成到 C# 语言的技术统称。LINQ 最明显的 “语言集成” 部分就是查询表达式。
- 查询表达式采用声明性查询语法编写而成。使用查询语法，可以用最少的代码对数据源执行筛选、排序和分组操作。

```csharp
// Specify the data source.
int[] scores = { 97, 92, 81, 60 };

// Define the query expression.
IEnumerable<int> scoreQuery =
    from score in scores
    where score > 80
    select score;

// Execute the query.
foreach (int i in scoreQuery)
    Console.Write(i + " ");
// Output: 97 92 81
```

---
### 查询表达式概述

- 查询表达式可用于查询并转换所有启用了 LINQ 的数据源中的数据。例如，通过一个查询即可检索 SQL 数据库中的数据，并生成 XML 流作为输出。

- 查询表达式中的变量全都是强类型，可以使用匿名隐式变量声明。

- 在编译时，查询表达式根据 C# 规范规则转换成标准查询运算符方法调用。可使用查询语法表示的任何查询都可以使用方法语法进行表示。

- 通常，在编写 LINQ 查询时尽量使用查询语法，并在必要时尽可能使用方法语法。查询表达式通常比使用方法语法编写的等同表达式更具可读性。

- 一些查询操作（如 `Count` 或 `Max`）没有等效的查询表达式子句，因此必须表示为方法调用。

- 查询表达式可被编译成表达式树或委托，`IEnumerable<T>` 查询编译为委托。`IQueryable` 和 `IQueryable<T>` 查询编译为表达式树。

---
### 查询操作的三个部分

- 查询是一种从数据源检索数据的表达式，使用专门的查询语句来表示。LINQ 通过提供处理各种数据源和数据格式的数据的一致模型，简化了各种各样的查询语言（例如用于关系数据库的 SQL 和用于 XML 的 XQuery 等）。
- 在 LINQ 查询中，可以使用相同的基本编码模式来查询和转换 XML 文档、SQL 数据库、ADO\.NET 数据集、.NET 集合中的数据以及 LINQ 提供程序可用的任何其他格式的数据。
- 所有的 LINQ 查询操作都是由获取数据源、创建查询、执行查询三个部分的操作组成。

```csharp
// The Three Parts of a LINQ Query:
// 1. Data source.
int[] numbers = new int[7] { 0, 1, 2, 3, 4, 5, 6 };

// 2. Query creation.
// numQuery is an IEnumerable<int>
var numQuery =
    from num in numbers
    where (num % 2) == 0
    select num;

// 3. Query execution.
foreach (int num in numQuery)
    Console.Write("{0,1} ", num);
```

<br>

#### 获取数据源

- 查询在 `foreach` 语句中执行，因此支持 ` IEnumerable`、`IEnumerable<T>` 或派生接口（如泛型 `IQueryable<T>`）的类型称为可查询类型。可查询类型不需要进行修改或特殊处理就可以用作 LINQ 数据源。

```csharp
// Create a data source from an XML document.
// using System.Xml.Linq;
XElement contacts = XElement.Load(@"c:\myContactList.xml");
```

<br>

#### 创建查询

- 查询指定要从数据源中检索的信息，可以指定在返回这些信息之前如何对其进行排序、分组和结构化。查询存储在查询变量中，并用查询表达式进行初始化。
- 可以使用查询语法或方法语法来构造查询表达式。

```csharp
int[] numbers = { 5, 10, 8, 3, 6, 12 };
// 查询语法
var numQuery1 =
           from num in numbers
           where num % 2 == 0
           orderby num
           select num;

Console.WriteLine(string.Join(", ", numQuery1)); // 6, 8, 10, 12

// 方法语法
IEnumerable<int> numQuery2 = numbers
    .Where(num => num % 2 == 0)
    .OrderBy(n => n);

Console.WriteLine(string.Join(", ", numQuery2)); // 6, 8, 10, 12
```

<br>

#### 执行查询

- 查询延迟执行：查询的实际执行将推迟到在 `foreach` 语句中循环访问查询变量之后进行。

```csharp
int[] numbers = { 5, 10, 8, 3, 6, 12 };

var numQuery =
     from num in numbers
     where (num % 2) == 0
     select num;

//  Query execution.
foreach (int num in numQuery)
    Console.Write("{0,1} ", num);
```

- 强制立即执行：对一系列源元素执行聚合函数的查询必须首先循环访问这些元素。`Count`、`Max`、`Average` 和 `First` 就属于此类查询。这些方法在执行时不使用显式 `foreach` 语句，它们常返回单个值。
- 要强制立即执行任何查询并缓存其结果，可调用 `ToList` 或 `ToArray` 方法。

```csharp
int[] numbers = { 5, 10, 8, 3, 6, 12 };

List<int> numQuery2 =
    (from num in numbers
     where (num % 2) == 0
     select num).ToList();

int[] numQuery3 =
    (from num in numbers
     where (num % 2) == 0
     select num).ToArray();
```

---
### 查询表达式

- 查询表达式是以查询语法表示的查询：
  - 以 `from` 子句开头，必须以 `select` 或 `group` 子句结尾。
  - 在第一个 `from` 子句与最后一个 `select` 或 `group` 子句之间，可以包含以下可选子句中的一个或多个：`where`、`orderby`、`join`、`let`，或者是其他 `from` 子句。
  - 可以使用 `into` 关键字，使 `join` 或 `group` 子句的结果充当相同查询表达式中的其他查询子句的数据源。

> 查询表达式示意图

![](./.img/LINQ%20查询表达式.png)


<br>

#### from-in 子句：获取数据源

- 查询表达式以 `from` 子句开头，用以指定将在其上运行查询或子查询的数据源序列（source sequence），并表示源序列中每个元素的本地范围变量（local range variable）。

```csharp
from [type-name] <identifier> in <enumerable-expr>
```

- `from` 数据源必须是 `IEnumerable` 或 `IEnumerable<T>`、`IQueryable<T>` 类型之一。范围变量和数据源已强类型化。
- 数据源实现 `IEnumerable<T>` 时，编译器推断范围变量的类型。若源是 `IEnumerable` 则需要显式指定范围变量的类型。

```csharp
// A simple data source.
int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

// Create the query.
// lowNums is an IEnumerable<int>
var lowNums = from num in numbers // 依据数据源的类型推断范围变量 num 为 int
              where num < 5
              select num;

// Execute the query.
foreach (int i in lowNums)
    Console.Write(i + " ");
```

- 若源序列的每个元素本身是一个序列或包含一个序列时，可以使用复合 `from` 子句。可以向任一的 `from` 子句添加 `where` 或 `orderby` 子句筛选结果。

```csharp
// A simple data source.
List<Student> students = new List<Student>
{
    new Student("Omelchenko", new List<int> {97, 72, 81, 60}),
    new Student ("O'Donnell", new List<int> {75, 84, 91, 39}),
    new Student ("Mortensen", new List<int> {88, 94, 65, 85}),
    new Student ("Garcia", new List<int> {97, 89, 85, 82}),
    new Student ("Beebe", new List<int> {35, 72, 91, 70})
};

// Create the query.
var scoreQuery = from student in students
                 from score in student.Scores   // from 子句
                 where score > 90
                 orderby score ascending   // 正序排序
                 select new { student = student.Name, score };

// Execute the query.
foreach (var s in scoreQuery)
    Console.WriteLine("{0} Score: {1}", s.student, s.score);

public record Student(string Name, List<int> Scores);
```

> 两个 from 子句的完全交叉联接

```csharp
// A simple data source.
char[] upperCase = { 'A', 'B', 'C' };
char[] lowerCase = { 'x', 'y', 'z' };

// Create the query.
var joinQuery =
           from lower in lowerCase
           where lower != 'x'
           from upper in upperCase
           select new { lower, upper };

// Execute the query.
foreach (var pair in joinQuery)
    Console.WriteLine("{0} is matched to {1}", pair.upper, pair.lower);

// Output
/*
    A is matched to y
    B is matched to y
    C is matched to y
    A is matched to z
    B is matched to z
    C is matched to z
 */
```

<br>

#### where 子句：筛选

- `where` 子句用在查询表达式中，用于指定将在查询表达式中返回数据源中的哪些元素。它使用一个布尔条件（谓词，predicate）应用于每个源元素并返回满足条件的元素。

```csharp
where <boolean-expr> 
```

- `where` 子句是一种筛选极值，可以在查询表达式中的任何位置，除了不能是第一个或最后一个子句。

```csharp
var queryLowNums =
        from num in new int[] { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 }
        where num < 5
        where num % 2 == 0
        // 等效于
        // where num < 5 && num % 2 == 0
        select num;
```

<br>

#### select 子句：选择与投影

- 在查询表达式中，`select` 子句指定在执行查询时产生的值的类型。根据计算所有以前的子句以及根据 `select` 子句本身的所有表达式得出结果。查询表达式必须以 `select` 子句或 `group` 子句结尾。

```csharp
select <expr>
```

- `select` 子句常用于直接返回源数据，也可以用于将源数据转换（或投影）为新类型。

```csharp
var Numbers =
    from i in new int[] { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 }
    select new { x = i, sin = Math.Sin(i) };  // select 创建新类型

foreach (var i in Numbers)
    Console.WriteLine($"Sin({i.x}) = {i.sin:F6} ");
```

<br>

#### group 子句：分组

- `group` 子句返回一个 `IGrouping<TKey,TElement>` 对象序列，这些对象包含零个或更多与该组的键值匹配的项。`by` 用于指定应返回项的分组方式。

```csharp
group <TElem> by <TKey> [into <identifier>]
```

- 以 `group` 结尾的查询。

```csharp
// Query variable is an IEnumerable<IGrouping<char, Student>>
var studentQuery1 =
    from student in students
    group student by student.Last[0];

record Student(string First, string Last);
```

> `IGrouping<TKey, TElement>`

- 由于 `group` 查询产生的 `IGrouping<TKey,TElement>` 对象实质上是一个由列表组成的列表，因此必须使用嵌套的 `foreach` 循环来访问每一组中的各个项。
- 外部循环用于循环访问组键，内部循环用于循环访问组本身包含的每个项。组可能具有键，但没有元素。

```csharp
foreach (IGrouping<char, Student> studentGroup in studentQuery2)
{
     Console.WriteLine(studentGroup.Key);
     // Explicit type for student could also be used here.
     foreach (var student in studentGroup)
         Console.WriteLine("   {0}, {1}", student.Last, student.First);
 }
```

> 键 Key 类型和分组

- 组键可以是任何类型，如字符串、内置数值类型、用户定义的命名类型或匿名类型。

```csharp
// --------------- 源数据定义 -----------------
List<Student> students = new List<Student>
{
    new Student {First="Svetlana", Last="Omelchenko", ID=111, Scores= new List<int> {97, 72, 81, 60}},
    new Student {First="Claire", Last="O'Donnell", ID=112, Scores= new List<int> {75, 84, 91, 39}},
    new Student {First="Sven", Last="Mortensen", ID=113, Scores= new List<int> {99, 89, 91, 95}},
    new Student {First="Cesar", Last="Garcia", ID=114, Scores= new List<int> {72, 81, 65, 84}},
    new Student {First="Debra", Last="Garcia", ID=115, Scores= new List<int> {97, 89, 85, 82}}
};

// 字符串分组
var StringQuery =
    from student in students
    group student by student.Last;

// 布尔分组
var boolQuery = 
    from student in students
    group student by student.Scores.Average() > 80;  // 分为 true / false 两组

// 数值范围分组
var RangeQuery =
            from student in students
            let avg = (int)student.Scores.Average()
            group student by (avg / 10);  // 以 10 分为范围

record Student
{
    public required string First { get; init; }
    public required string Last { get; init; }
    public required int ID { get; init; }
    public required List<int> Scores { get; init; }
}  
```

<br>

#### into 子句：附加查询

- 可使用 `into` 创建一个临时的标识符，并将 `group`、`join`、`select` 子句的结果存储到新的组中，该标识符成为附加查询命令的生成器。

```csharp
{select|join|group} into <identifier> {...}
```

> `into` 附加 `group`

```csharp
Random r = new Random();
int[] arr = new int[100];
for (int i = 0; i < 100; i++)
    arr[i] = r.Next(-100, 100);

var query =
    from num in arr
    group num by num / 10 into numGroup
    where numGroup.Count() > 0
    orderby numGroup.Key
    select new { Range = numGroup.Key * 10, Numbers = numGroup.ToArray() };

foreach (var item in query)
    Console.WriteLine($"Numbers in " +
        $"[{(item.Range < 0 ? item.Range - 10 : item.Range)}," +
        $"{(item.Range < 0 ? item.Range : item.Range + 10)})] " +
        $"are {string.Join(",", item.Numbers)}");
```

> `into` 附加 `select`

```csharp
Random r = new Random();
int[] arr = new int[100];
for (int i = 0; i < 100; i++)
    arr[i] = r.Next(-100, 100);

var query =
    from num in arr
    where num > 0
    orderby num
    select num into G
    group G by G / 10;

foreach (var item in query)
{
    Console.Write($"Numbers in [{item.Key * 10}, {(item.Key + 1) * 10}] are ");
    Console.WriteLine(string.Join(",", item));
}
```

<br>

#### orderby 子句：中间件排序

- 在查询表达式中，`orderby` 子句可导致返回的序列或子序列（组）以升序或降序排序。排序操作基于一个或多个属性对序列的元素进行排序。第一个排序条件对元素执行主要排序。通过指定第二个排序条件，可以对每个主要排序组内的元素进行次要排序。
- 元素类型的默认比较器执行排序，默认排序顺序为升序（`ascending`），降序为 `descending`。还可以指定自定义比较器，但只适用于方法语法。

```csharp
orderby <Primary>[,<secondary>...] [ascending/descending: default= ascending]
```

> 对数据排序

- 主要升序排序。

```csharp
var query = from word in new string[] { "the", "quick", "brown", "fox", "jumps" }
            orderby word.Length
            select word;

Console.WriteLine(string.Join(", ", query));
// the, fox, quick, brown, jumps
```

- 主要降序排序。

```csharp
var query = from word in new string[] { "the", "quick", "brown", "fox", "jumps" }
            orderby word.Substring(0,1) descending
            select word;

Console.WriteLine(string.Join(", ", query));
// the, quick, jumps, fox, brown
```

- 次要升序排序。

```csharp
var query = from word in new string[] { "the", "quick", "brown", "fox", "jumps" }
            orderby word.Length, word.Substring(0, 1)
            select word;

Console.WriteLine(string.Join(", ", query));
// fox, the, brown, jumps, quick
```

- 次要降序排序。

```csharp
var query = from word in new string[] { "the", "quick", "brown", "fox", "jumps" }
            orderby word.Length ascending, word.Substring(0, 1) descending
            select word;

Console.WriteLine(string.Join(", ", query));
// the, fox, quick, jumps, brown
```

<br>

#### join-in-on-equals 子句：联接

- `join` 子句可用于将两个没有直接关系元素的源序列相关联（同等联接），要求每个序列中的元素具有能够与其他序列的相应属性进行比较的属性，或者包含一个这样的属性。`join` 子句使用 `equals` 关键字比较指定的键是否相等（值相等性）。

```csharp
join <inner-identifier> in <inner-sequence> on <outer-key> equals <inner-key> [into <identifier>]
```

- `join` 子句的输出形式取决于执行的联接的具体类型：内部联接、分组联接、左外部联接。

```csharp
var categories = new[]
{
    new { Name = "A", ID = 101 },
    new { Name = "B", ID = 102 },
    new { Name = "C", ID = 103 },
    new { Name = "D", ID = 104 },
};
var products = new[]
{
    new { Name = "Apple", CategoryID = 101 },
    new { Name = "Football", CategoryID = 102 },
    new { Name = "Train", CategoryID = 103 },
    new { Name = "Banana", CategoryID = 101 },
    new { Name = "Car", CategoryID = 103 },
    new { Name = "Basketball", CategoryID = 102 },
};
// 内联联接
Console.WriteLine("----- inner Join Query -----");
var innerJoinQuery =
    from category in categories
    join prod in products on category.ID equals prod.CategoryID
    select new { ProductName = prod.Name, Category = category.Name }; //produces flat sequence
foreach (var product in innerJoinQuery)
    Console.WriteLine("{0} : {1}", product.Category, product.ProductName);

// 分组联接
Console.WriteLine("----- inner Group Join Query -----");
var innerGroupJoinQuery =
    from category in categories
    join prod in products on category.ID equals prod.CategoryID into prodGroup
    select new { CategoryName = category.Name, Products = prodGroup };
foreach (var item in innerGroupJoinQuery)
{
    Console.WriteLine($"{item.CategoryName} : ");
    foreach (var productName in item.Products.Select(p => p.Name))
        Console.WriteLine("    " + productName);
}

// 左外部联接
Console.WriteLine("----- left Outer Join Query -----");
var leftOuterJoinQuery =
    from category in categories
    join prod in products on category.ID equals prod.CategoryID into prodGroup
    from item in prodGroup.DefaultIfEmpty(new { Name = "Empty", CategoryID = 0 })
    select new { CatName = category.Name, ProdName = item.Name };
foreach (var product in leftOuterJoinQuery)
    Console.WriteLine("{0} : {1}", product.CatName, product.ProdName);
```

<br>

#### let 子句：引入范围变量

- 在查询表达式中，可以通过 `let` 子句创建一个新的范围变量并通过提供的表达式结果初始化该变量。
  
```csharp
let <identifier> = <expr>
```
  
- 使用值进行初始化后，范围变量不能用于存储另一个值。但是，如果范围变量持有可查询类型（`IEnumerable`），则可以查询该变量。

```csharp
string[] strings =
{
    "A penny saved is a penny earned.",
    "The early bird catches the worm.",
    "The pen is mightier than the sword."
 };

// Split the sentence into an array of words
// and select those whose first letter is a vowel.
var earlyBirdQuery =
    from sentence in strings
    let words = sentence.Split(' ', '.', ',')  // words 是可查询类型
    from word in words
    where !string.IsNullOrEmpty(word)
    let w = word.ToLower()[0]  // word 首字母小写化
    where w == 'a' || w == 'e' || w == 'i' || w == 'o' || w == 'u'
    select word; // 若以韵母开头

// Execute the query.
Console.WriteLine("Words start with a vowel : " + string.Join(",", earlyBirdQuery));
```

---
### 标准查询运算符

- 标准查询运算符是组成 LINQ 模式的方法，这些方法中的大多数都作用于序列。其中序列指其类型实现 `IEnumerable<T>` 接口或 `IQueryable<T>` 接口的对象，`System.Linq.Enumerable` 的扩展方法作用于类型 `IEnumerable<T>` 的对象，`System.Linq.Queryable` 的扩展方法作用于类型 `IQueryable<T>` 的对象。
- 标准查询运算符提供包括筛选、投影、聚合、排序等在内的查询功能。各个标准查询运算符在执行时间上有所不同，返回单一实例值的方法（例如 `Average` 和 `Sum` 等）立即执行，返回序列的方法会延迟查询执行，并返回一个可枚举的对象。
- 对于在集合上运行的 `IEnumerable<T>` 的查询方法，返回可枚举对象，在枚举该对象时，将使用查询运算符的逻辑，并返回查询结果。而扩展 `IQueryable<T>` 的方法不会实现任何查询行为，它们生成一个表示要执行的查询的表达式树，源 `IQueryable<T>` 对象执行查询处理。

```csharp
string sentence = "the quick brown fox jumps over the lazy dog";
string[] words = sentence.Split(' ');

// Using query expression syntax.  
var query = from word in words
            group word.ToUpper() by word.Length into gr
            orderby gr.Key
            select new { Length = gr.Key, Words = gr };

// Using method-based query syntax.  
var query2 = words.
    GroupBy(word => word.Length, w => w.ToUpper()).
    OrderBy(gr => gr.Key).
    Select(gr => new { Length = gr.Key, Words = gr });

foreach (var obj in query)
{
    Console.WriteLine("Words of length {0}:", obj.Length);
    foreach (string word in obj.Words)
        Console.WriteLine(word);
}
```

> 查询表达式语法表

- `Cast`：使用显式类型化范围变量，相当于 `from <type> <variable> in <source-sequence>`。
- `GroupBy`：对应 `group ... by ... [into ...]`。
- `GroupJoin`：对应 `join ... in ... on ... equals ... into ...`。
- `Join`：对应 `join ... in ... on ... equals ...`。
- `OrderBy`：对应 `orderBy <... [ascending]>`。
- `OrderByDescending`：对应 `orderBy <... descending>`。
- `Select`：对应 `select <...>`。
- `SelectMany`：对应多个 `from` 子句。
- `ThenBy`：对应 `orderBy ..., <... [ascending]>`。
- `ThenByDescending`：对应 `orderBy ..., <... descending>`。
- `Where` 方法：对应 `where <...>`。

<br>

#### 筛选数据：OfType、Where

- 筛选是指将结果集限制为仅包含满足指定条件的元素的操作。
  - `OfType<T>`：根据指定类型筛选 `IEnumerable` 或 `IQueryable` 的元素。
  - `Where`：基于谓词筛选值序列。对应于查询表达式的 `where` 子句。

> `ofType`

```csharp
// OfType
object?[] datas = { 1, "Mary", "Hello", 5.2f, "World", 15, "Ychao", null, 'A' };
IEnumerable<string> strs = datas.OfType<string>();
// Mary,Hello,World,Ychao
```

> `Where`

```csharp
string[] words = { "the", "quick", "brown", "fox", "jumps" };
var query = from w in words
            where w.Length > 2 select w;
// where 子句等效于
var queryFun = words.Where(str => str.Length > 3);
// quick,brown,jumps
```

<br>

#### 投影运算：Select、SelectMany、Zip

- 投影是指将序列中的每个元素投影到新表单。可以构造从每个元素生成的新类型，或对其执行数学函数等：
  - `Select`：投影基于转换函数的值。对应于查询表达式的 `Select` 子句。
  - `SelectMany`：投影基于转换函数的值序列，然后将它们展平为一个序列。对应于查询表达式的多个 `from` 子句。
  - `Zip`：使用 2-3 个指定序列中的元素生成元组序列或用户定义序列，组合的序列长度不超过最短序列。

* `Select` 与 `SelectMany` 的区别在于：`Select` 返回一个与源集合具有相同元素数目的集合；`SelectMany` 将中间数组序列串联为一个最终结果值，其中包含每个中间数组中的每个值。

> `Select`

```csharp
List<string> words = new() { "an", "apple", "a", "day" };

var query = from word in words
            select word.Substring(0, 1);
// 等效于
var queryFun = words.Select(word => word.Substring(0, 1));
// a,a,a,d
```

> `SelectMany`

```csharp
List<string> phrases = new() { "an apple a day", "the quick brown fox" };

IEnumerable<string> query = from phrase in phrases
            from word in phrase.Split(' ')
            select word;
// 等效于
IEnumerable<string> queryFun = phrases.SelectMany(selector: source => source.Split(' '));
// an,apple,a,day,the,quick,brown,fox
```

> Zip

- `Zip` 投影运算符有多个重载，可以投影为元组类型，或是用户定义类型。所有 `Zip` 方法都处理两个或更多可能是异构类型的序列。

```csharp
// An int array with 7 elements.
IEnumerable<int> numbers = new[] { 1, 2, 3, 4, 5, 6, 7 };
// A char array with 6 elements.
IEnumerable<char> letters = new[] { 'A', 'B', 'C', 'D', 'E', 'F' };

// 投影用户定义序列
var query = numbers.Zip(letters, resultSelector: (first, second) => new { number = first, letter = second });
// 投影元组
IEnumerable<(int First, char Second)> query2 = numbers.Zip(letters);
```

<br>

#### 集合操作：Distinct、Except、Intersect、Union

- LINQ 中的集合操作指的是生成结果集的查询操作，该结果集基于相同或不同集合（或集）中是否存在等效元素：
  - `Distinct`、`DistinctBy`：返回序列中的非重复元素。可以指定 `IEqualityComparer<T>` 对值进行比较；`DistinctBy` 可以指定键选择器函数。
  - `Except`、`ExceptBy`：返回差集，差集指位于一个集合但不位于另一个集合的元素。
  - `Intersect`、`IntersectBy`：返回交集，交集指同时出现在两个集合中的元素。元素不重复出现。
  - `Union`、`UnionBy`：返回并集，并集指位于两个集合中任一集合的唯一的元素。

```csharp
int[] numbers1 = { 1, 3, 4, 5, 5, 7, 8, 10, 20, 23, 45 };
int[] numbers2 = { 1, 1, 3, 5, 6, 9, 10, 12, 20, 25, 45 };

var queryDistinct = numbers1.Distinct();
// 非重复序列 : 1,3,4,5,7,8,10,20,23,45 
var queryExcept = numbers1.Except(numbers2);
// 差集 : 4,7,8,23
var queryIntersect = numbers1.Intersect(numbers2);  
// 交集 : 1,3,5,10,20,45
var queryUnion = numbers1.Union(numbers2);
// 并集 : 1,3,4,5,7,8,10,20,23,45,6,9,12,25
```

<br>

#### 排序操作：OrderBy、ThenBy、Reverse

- 排序操作基于一个或多个属性对序列的元素进行排序。第一个排序条件对元素执行主要排序，可以通过指定第二个排序条件，对每个主要排序组内的元素进行次要排序：
  - `Order`：按升序对序列的元素进行排序，此方法使用默认比较器。可以显式定义比较器接口。
  - `OrderBy`：按升序对值主要排序。对应于查询表达式的 `orderby <...>`。
  - `OrderByDescending`：按降序对值主要排序。对应于查询表达式的	`orderby <... descending>`。
  - `ThenBy`：按升序对主要排序组内元素执行次要排序。对应于查询表达式的 `orderby ..., <...>`。
  - `ThenByDescending`：按降序对主要排序组内元素执行次要排序。对应于查询表达式的 `orderby ..., <... descending>`。
  - `Reverse`：反转集合中元素的顺序。

```csharp
string[] words = { "the", "quick", "brown", "fox", "jumps" };

var Order = words.Order();
// 默认排序 : brown, fox, jumps, quick, the

var OrderBy = from word in words
              orderby word.Length
              select word;
var OrderByFun = words.OrderBy(word => word.Length);
// 主要升序排序 : the,fox,quick,brown,jumps

var OrderByDescending = from word in words
                        orderby word.Length descending
                        select word;
var OrderByDescendingFun = words.OrderByDescending(word => word.Length);
// 主要降序排序 : quick,brown,jumps,the,fox

var ThenBy = from word in words
             orderby word.Length, word.Substring(0, 1)
             select word;
var ThenByFun = words.OrderBy(word => word.Length).ThenBy(word => word.Substring(0, 1));
// 主要升序次要升序 : fox,the,brown,jumps,quick

var ThenByDescending = from word in words
                       orderby word.Length, word.Substring(0,1) descending
                       select word;
var ThenByDescendingFun = words.OrderBy(word => word.Length).ThenByDescending(word => word.Substring(0, 1));
// 主要升序次要降序 : the,fox,quick,jumps,brown

var Reverse = words.Reverse();
// 反转集合 : jumps,fox,brown,quick,the
```

<br>

#### 限定符运算：All、Any、Contains

- 限定符运算返回一个 `bool` 值，该值指示序列中是否有一些元素满足条件或是否所有元素都满足条件：
  - `All`：确定是否序列中的所有元素都满足条件。
  - `Any`：确定序列中是否存在元素，或有元素满足条件。
  - `Contains`：确定序列是否包含指定的元素。

```csharp
List<Market> markets = new List<Market>
{
    new Market { Name = "Emily's", Items = new string[] { "kiwi", "cheery", "banana" } },
    new Market { Name = "Kim's", Items = new string[] { "melon", "mango", "olive" } },
    new Market { Name = "Adam's", Items = new string[] { "kiwi", "apple", "orange" } },
};

var All = markets.Where(market => market.Items.All(item => item.Length == 5))
        .Select(market => market.Name + " market");   // Kim's market

var Any = markets.Where(market => market.Items.Any(item => item.StartsWith('o')))
        .Select(market => market.Name + " market");   // Kim's market, Adam's market

var Contains = markets.Where(market => market.Items.Contains("kiwi"))
        .Select(market => market.Name + " market");   // Emily's market, Adam's market
```

<br>

#### 数据分区：Skip、Take、Chunk

- LINQ 中的分区是指将输入序列划分为两个部分的操作，无需重新排列元素，然后返回其中一个部分：
  - `Skip`：省略序列从开头起 `Count` 个元素。
  - `SkipLast`：省略序列从末尾起 `Count` 个元素。
  - `SkipWhile`：基于谓词函数跳过元素，直到元素不符合条件时，返回后续不满足条件的元素。
  - `Take`：获取序列从开头起前 `Count` 个元素。
  - `TakeLast`：获取序列从末尾起 `Count` 个元素。
  - `TakeWhile`：基于谓词函数获取元素，直到元素不符合条件时，返回前面满足条件的元素。
  - `Chunk`：将序列的元素拆分为指定大小的区块。

```csharp
int[] numbers = { 1, 3, 5, 7, 9, 2, 4, 6, 8, 0 };

IEnumerable<int> Skip = numbers.Skip(3);
// 跳过前三个 : 7,9,2,4,6,8,0

IEnumerable<int> SkipLast = numbers.SkipLast(3);
// 省略后三个 : 1,3,5,7,9,2,4

IEnumerable<int> SkipWhile = numbers.SkipWhile(num => num <= 7);
// 直至不满足 <= 7 : 9,2,4,6,8,0

IEnumerable<int> Take = numbers.Take(3);
// 获取前 3 个 : 1,3,5

IEnumerable<int> TakeLast = numbers.TakeLast(3);
// 获取后 3 个 : 6,8,0

IEnumerable<int> TakeWhile = numbers.TakeWhile(num => num <= 7);
// 一直满足 <= 7 的前序列 : 1,3,5,7

IEnumerable<int[]> Chunk = numbers.Chunk(3);
// 三三分组 : (1,3,5), (7,9,2), (4,6,8), (0)
```

<br>

#### 生成运算：DefaultIfEmpty、Empty、Range、Repeat

- 生成是指创建新的值序列：
  - `DefaultIfEmpty`：用默认值单一实例集合替换空集合。
  - `Enumerable.Empty`：返回一个空集合。
  - `Enumerable.Range`：生成包含数字序列的集合。
  - `Enumerable.Repeat`：生成包含一个重复值的集合。

```csharp
var Query = from length in Enumerable.Range(0, 40)
            let str = Enumerable.Repeat('口', length).ToArray()
            select (Len: length, str: str) into gr
            let str = gr.Len % 2 == 0 ? Enumerable.Empty<char>() : gr.str
            select str.DefaultIfEmpty('\n');

foreach (var chars in Query)
    foreach (var c in chars)
        Console.Write(c);
```

<br>

#### 相等运算：SequenceEqual

- 两个序列，其相应元素相等且具有被视为相等的相同数量的元素：
  - `SequenceEqual`：通过以成对方式比较元素确定两个序列是否相等。长度不等或某一对元素不等，返回 `false`。

```csharp
int[] arr = { 1, 2, 3, 4, 5 };
List<int> list = new List<int>{ 1, 2, 3, 4, 5 };
var equals = arr.SequenceEqual(list);  // true
```

<br>

#### 元素运算：ElementAt、First、Last、Single

- 元素运算从序列中返回唯一、特定的元素：
  - `ElementAt`：返回集合中指定索引处的元素。
  - `ElementAtOrDefault`：返回集合中指定索引处的元素；如果索引超出范围，则返回默认值。
  - `First`：返回集合的第一个元素或满足条件的第一个元素。
  - `FirstOrDefault`：返回集合的第一个元素或满足条件的第一个元素，不存在则返回默认值。
  - `Last`：返回集合的最后一个元素或满足条件的最后一个元素。
  - `LastOrDefault`：返回集合的最后一个元素或满足条件的最后一个元素，不存在时返回默认值。
  - `Single`：返回集合的唯一一个元素或满足条件的唯一一个元素。
  - `SingleOrDefault`：返回集合的唯一一个元素或满足条件的唯一一个元素，没有要返回的元素则返回默认值。

```csharp
int?[] numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9];

int? ElementAt = numbers.ElementAt(5);  // 6
int? ElememtAtOrDefault = numbers.ElementAtOrDefault(numbers.Count());  // null

int? First = numbers.First();   // 1
int? FirstPredicate = numbers.First(predicate: num => num > 3);  // 4
int FirstDefault = new int[0].FirstOrDefault();  // 0 

int? Last = numbers.Last();  // 9 
int? LastPredicate = numbers.Last(predicate: num => num < 5);  // 4
int? LastDefault = numbers.FirstOrDefault(predicate: num => num < 10);  // 1

// var Single = numbers.Single();  // error
int? Single = numbers.Single(predicate: num => num == 5);  // 5
int SingleDefault = new int[0].SingleOrDefault();  // 0
int? SingleDefault2 = numbers.SingleOrDefault(predicate: num => num > 100);  // null
```

<br>

#### 转换数据类型：AsEnumerable、AsQueryable、OfType、ToArray、ToList、ToHashSet、ToDictionary、ToLookUp

- 转换方法可更改输入对象的类型：
  - `AsEnumerable`：返回类型化为 `IEnumerable<T>` 的输入。
  - `AsQueryable`：将 `IEnumerable` 转换为 `IQueryable` 或 `IEnumerable<T>` 转换为 `IQueryable<T>`。
  - `Cast`：将集合中的元素转换为指定类型。
  - `OfType`：根据其转换为指定类型的能力筛选值。
  - `ToArray`：将集合转换为数组。此方法将强制执行查询。
  - `ToList`：将集合转换为 `List<T>`。此方法强制执行查询。
  - `ToHashSet`：从 `IEnumerable<T>` 或 `IQueryable` 创建一个 `HashSet<T>`。
  - `ToDictionary`：根据键选择器函数将元素放入 `Dictionary<TKey,TValue>`。此方法强制执行查询。
  - `ToLookup`：根据键选择器函数将元素放入 `Lookup<TKey,TElement>`（一对多字典）。此方法强制执行查询。

```csharp
List<object> phrases = new List<object> { "an apple a day", "the quick brown fox" };
List<object> objs = new List<object> { 1, 2, 3, "World", 4, 5, 6, "Hello" };

IEnumerable<string> Cast = phrases.Cast<string>();

var OfTypeInt = objs.OfType<int>();  // 1,2,3,4,5,6
var OfTypeString = objs.OfType<string>();  // World,Hello

var SplitQuery = phrases.OfType<string>().SelectMany(phrase => phrase.Split(' '));
string[] ToArray = SplitQuery.ToArray();
List<string> ToList = SplitQuery.ToList();
HashSet<string> ToHashSet = SplitQuery.ToHashSet();
// an,apple,a,day,the,quick,brown,fox

Dictionary<char, string> ToDictionary = (from string phrase in phrases
                                         group phrase by phrase[0])
                                        .ToDictionary(keySelector: group => group.Key, elementSelector: group => group.ToArray()[0]);

ILookup<char, List<string>> ToLookup = (from string phrase in phrases
                                        from word in phrase.Split(' ')
                                        group word by word[0])
                                                  .ToLookup(keySelector: group => group.Key, elementSelector: g => g.ToList());
```

<br>

#### 附加运算：Concat、Append、Prepend

- 串联是指将一个序列或元素附加到另一个序列的操作：
  - `Concat`：连接两个序列以组成一个序列。
  - `Append`：在序列尾端附加一个同类型元素。
  - `Prepend`：在序列的开头添加值。

```csharp
int[] arr = { 1, 2, 3, 4, 5 };

var Concat = arr.Concat(new int[] { 6, 7, 8, 9, 0 });
// 1,2,3,4,5,6,7,8,9,0

var Append = arr.Append(100);
// 1,2,3,4,5,100

var Prepend = arr.Prepend(-100);
// -100,1,2,3,4,5
```

<br>

#### 聚合运算：Aggregate、Average、Count、LongCount、Max、Min、Sum

- 聚合运算从值的集合中计算出单个值：
  - `Aggregate`：对集合的值执行自定义聚合运算。
  - `Average`：计算值集合的平均值。
  - `Count`：对集合中元素计数，可选择仅对满足谓词函数的元素计数。
  - `LongCount`：对大型集合中元素计数，可选择仅对满足谓词函数的元素计数。
  - `Max`、`MaxBy`：确定集合中的最大值。
  - `Min`、`MinBy`：确定集合中的最小值。
  - `Sum`：对集合中的值求和。

```csharp
int[] numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 30, 40, 50];

var Aggregate = numbers.Aggregate(seed: 0, func: (Seed, num2) => Seed + num2);  // 累计相加：195
var Average = numbers[9..].Average(); // 第 9 索引起的后续元素的平均值：30

int Count = numbers.Count(predicate: num => num > 9);  // 大于 9 的元素数：5
long LongCount = numbers.LongCount();   // 14

var Max = numbers.Max();  // 50
var MinBy = numbers.MinBy(num => 1f / num);  // 50

var Sum = numbers.Sum(selector: num => num < 10 ? num : 0); // 小于 10 的元素和：45
```

<br>

#### 联接运算：Join、GroupJoin

- 联接两个数据源就是将一个数据源中的对象与另一个数据源中具有相同公共属性的对象相关联：
  - `Join`：根据键选择器函数 `Join` 两个序列并提取值对。对应于查询表达式的 `join-in-on-equals` 子句。
  - `GroupJoin`：根据键选择器函数 `Join` 两个序列，并对每个元素的结果匹配项进行分组。对应于查询表达式的 `join-in-on-equals into ...` 子句。

```csharp
List<Product> products = new List<Product>
{
    new Product { Name = "Cola", CategoryId = 0 },
    new Product { Name = "Tea", CategoryId = 0 },
    new Product { Name = "Apple", CategoryId = 1 },
    new Product { Name = "Kiwi", CategoryId = 1 },
    new Product { Name = "Carrot", CategoryId = 2 },
};
List<Category> categories = new List<Category>
{
    new Category { Id = 0, CategoryName = "Beverage" },
    new Category { Id = 1, CategoryName = "Fruit" },
    new Category { Id = 2, CategoryName = "Vegetable" }
};

var Join = from product in products
           join category in categories on product.CategoryId equals category.Id
           select (product: product.Name, category: category.CategoryName);
var JoinFun = products.Join(categories,
            outer => outer.CategoryId, inner => inner.Id,
            (outer, inner) => (product: outer.Name, category: inner.CategoryName));

if (Join.SequenceEqual(JoinFun))
    Console.WriteLine("Join is equal to JoinFun ");  // True

var GroupJoin = from category in categories
                join product in products on category.Id equals product.CategoryId into Group
                select new { Category = category.CategoryName, Group = Group.ToList() };
var GroupJoinFun = categories.GroupJoin(products,
            outer => outer.Id, inner => inner.CategoryId,
            (category, inners) => new { Category = category.CategoryName, Group = inners.ToList() });
// -------------------------------------------------------
record struct Product(string Name, int CategoryId);
record struct Category(int Id, string CategoryName);
```

> 执行内部联接

- 在关系数据库术语中，内部联接会生成一个结果集，在该结果集中，第一个集合的每个元素对于第二个集合中的每个匹配元素都会出现一次。如果第一个集合中的元素没有匹配元素，则它不会出现在结果集中：
  - 简单键联接：基于简单键使两个数据源中的元素相关联的简单内部联接。
  - 复合键联接：基于复合键使两个数据源中的元素相关联的内部联接。复合键是由多个值组成的键，可以基于多个属性使元素相关联。
  - 多联接：可以将任意数量的联接操作相互追加，以执行多联接。
  - 使用分组联接实现的内部联接。

```csharp
// ===================== 简单键联接 =======================
using System;
using System.Drawing;

Person magnus = new("Magnus", "Hedlund");
Person terry = new("Terry", "Adams");
Person charlotte = new("Charlotte", "Weiss");
Person arlene = new("Arlene", "Huff");
Person rui = new("Rui", "Raposo");
List<Person> people = new() { magnus, terry, charlotte, arlene, rui };
List<Pet> pets = new List<Cat>()
{
    new("Barley", terry),
    new("Boots", terry),
    new("Whiskers", charlotte),
    new("Blue Moon", rui),
    new("Daisy", magnus),
}.Cast<Pet>().ToList();
var SampleKeyQuery = from person in people
                     join pet in pets on person equals pet.Owner
                     select new { Owner = person.FirstName, PetName = pet.Name };
var SampleKeyQueryFun = people.Join(pets,
     outer => outer, inner => inner.Owner,
     (outer, inner) => new { Owner = outer.FirstName, PetName = inner.Name });
if (SampleKeyQuery.SequenceEqual(SampleKeyQueryFun))
    foreach (var ownerAndPet in SampleKeyQuery)
        Console.WriteLine($"\"{ownerAndPet.PetName}\" is owned by {ownerAndPet.Owner}");
/** Output
    "Daisy" is owned by Magnus
    "Barley" is owned by Terry
    "Boots" is owned by Terry
    "Whiskers" is owned by Charlotte
    "Blue Moon" is owned by Rui
*/

// ===================== 复合键联接 =======================
List<Employee> employees = new()
{
    new(FirstName: "Terry", LastName: "Adams", EmployeeID: 522459),
    new("Charlotte", "Weiss", 204467),
    new("Magnus", "Hedland", 866200),
    new("Vernette", "Price", 437139)
};
List<Student> students = new()
{
    new(FirstName: "Vernette", LastName: "Price", StudentID: 9562),
    new("Terry", "Earls", 9870),
    new("Terry", "Adams", 9913)
};
var CompositeKeyQuery = from employee in employees
                        join student in students on new { employee.FirstName, employee.LastName } 
                        equals new { student.FirstName, student.LastName }
                        select employee.FirstName + " " + employee.LastName;
var CompositeKeyQueryFun = employees.Join(students,
    employee => new { employee.FirstName, employee.LastName },
    student => new { student.FirstName, student.LastName },
     (e, s) => e.FirstName + " " + e.LastName);
if (CompositeKeyQuery.SequenceEqual(CompositeKeyQueryFun))
    foreach (var person in CompositeKeyQuery)
        Console.WriteLine(person);
/** Output
    Terry Adams
    Vernette Price
 */

// ===================== 多联接 =======================
Person phyllis = new("Phyllis", "Harris");
people.Add(phyllis);
List<Cat> cats = pets.Cast<Cat>().ToList();
List<Dog> dogs = new()
{
    new(Name: "Four Wheel Drive", Owner: phyllis),
    new("Duke", magnus),
    new("Denim", terry),
    new("Wiley", charlotte),
    new("Snoopy", rui),
    new("Snickers", arlene),
};
var MultipleJoinQuery = from person in people
                        join cat in cats on person equals cat.Owner
                        join dog in dogs on new { Owner = person, Letter = cat.Name.Substring(0, 1) }
                        equals new { dog.Owner, Letter = dog.Name.Substring(0, 1) }
                        select new { CatName = cat.Name, DogName = dog.Name };
var MultipleJoinQueryFun = people
            .Join(cats, per => per, cat => cat.Owner, (person, cat) => new { person, cat })
            .Join(dogs, onwer => new { Owner = onwer.person, Letter = onwer.cat.Name.Substring(0, 1) },
                        dog => new { dog.Owner, Letter = dog.Name.Substring(0, 1) },
                        (onwer, dog) => new { CatName = onwer.cat.Name, DogName = dog.Name });
if (MultipleJoinQuery.SequenceEqual(MultipleJoinQueryFun))
    foreach (var Pet in MultipleJoinQuery)
        Console.WriteLine($"Cat: {Pet.CatName} & Dog: {Pet.DogName} have the same owners");
/** Output
    Cat: Daisy & Dog: Duke have the same owners
    Cat: Whiskers & Dog: Wiley  have the same owners
 */

// =====================  使用分组联接的内联 =======================
var GroupJoinQuery =
    from person in people
    join pet in pets on person equals pet.Owner into gj
    from subpet in gj
    select new
    {
        OwnerName = person.FirstName,
        PetName = subpet.Name
    };
var GroupJoinQueryFun = people.GroupJoin(pets,
        person => person, pet => pet.Owner, (person, pets) => new { person, gj=pets })
        .SelectMany(pet => pet.gj,
       (groupJoinPet, subpet) => new { OwnerName = groupJoinPet.person.FirstName, PetName = subpet.Name });
if (GroupJoinQuery.SequenceEqual(GroupJoinQueryFun))
    foreach (var v in GroupJoinQuery)
        Console.WriteLine($"{v.OwnerName} - {v.PetName}");
/** Output
    Magnus - Daisy
    Terry - Barley
    Terry - Boots
    Charlotte - Whiskers
    Rui - Blue Moon
 */

// -----------------------------------------------------------
; record Person(string FirstName, string LastName);
record Pet(string Name, Person Owner);
record Employee(string FirstName, string LastName, int EmployeeID);
record Student(string FirstName, string LastName, int StudentID);
record Cat(string Name, Person Owner) : Pet(Name, Owner);
record Dog(string Name, Person Owner) : Pet(Name, Owner);
```

> 执行分组联接

- 分组联接对于生成分层数据结构十分有用，它将第一个集合中的每个元素与第二个集合中的一组相关元素进行配对。
- 第一个集合的每个元素都会出现在分组联接的结果集中（无论是否在第二个集合中找到关联元素）。 在未找到任何相关元素的情况下，该元素的相关元素序列为空。 
  
```csharp
var categories = new[]
{
    new { Name = "A", ID = 101 },
    new { Name = "B", ID = 102 },
    new { Name = "C", ID = 103 },
    new { Name = "D", ID = 104 },
};
var products = new[]
{
    new { Name = "Apple", CategoryID = 101 },
    new { Name = "Football", CategoryID = 102 },
    new { Name = "Train", CategoryID = 103 },
    new { Name = "Banana", CategoryID = 101 },
    new { Name = "Car", CategoryID = 103 },
    new { Name = "Basketball", CategoryID = 102 },
};
var innerGroupJoinQuery =
    from category in categories
    join prod in products on category.ID equals prod.CategoryID into prodGroup
    select new { CategoryName = category.Name, Products = prodGroup.DefaultIfEmpty(new { Name = "Empty", CategoryID = 0 }) };
var innerGroupJoinQueryFun = categories.GroupJoin(products, cate => cate.ID, prod => prod.CategoryID,
            (cate, gr) => new { CategoryName = cate.Name, Products = gr.DefaultIfEmpty(new { Name = "Empty", CategoryID = 0 }) });

foreach (var item in innerGroupJoinQueryFun)
{
    Console.Write($"\n{item.CategoryName} : ");
    foreach (var productName in item.Products.Select(p => p.Name))
        Console.Write(productName + "  ");
}
/*
    A : Apple  Banana
    B : Football  Basketball
    C : Train  Car
    D : Empty
 */
```

> 执行左外部联接

- 左外部联接：返回第一个集合的每个元素，无论该元素在第二个集合中是否有任何相关元素。

```csharp
// 改写分组联接的查询变量
var leftOuterJoins =
    from category in categories
    join prod in products on category.ID equals prod.CategoryID into prodGroup
    from p in prodGroup.DefaultIfEmpty(new { Name = "", CategoryID = 0 })
    select new { CategoryName = category.Name, Product = p };

var leftOuterJoinsFun = categories
            .GroupJoin(products,
                       cate => cate.ID,
                       prod => prod.CategoryID,
                       (cate, gr) => new { cate, gr })
            .SelectMany(prods => prods.gr.DefaultIfEmpty(new { Name = "", CategoryID = 0 }),
                       (cate, prod) => new { CategoryName = cate.cate.Name, Product = prod });
foreach (var v in leftOuterJoinsFun)
    Console.WriteLine($"{v.CategoryName + ":",-15}{v.Product.Name}");
/*
    A:             Apple
    A:             Banana
    B:             Football
    B:             Basketball
    C:             Train
    C:             Car
    D:
 */
```

> 执行交叉联接

- 谨慎使用交叉联接，因为它们可能会生成非常大的结果集。

```csharp
int[] numbers = { 1, 2, 3, 4 };
string[] strings = { "World", "Hello" };

var CrossJoinQuery = from number in numbers
                     from str in strings
                     select (number, str);
Console.WriteLine(string.Join(", ", CrossJoinQuery));
// (1, World), (1, Hello), (2, World), (2, Hello), (3, World), (3, Hello), (4, World), (4, Hello)
```

> 执行非同等联接

```csharp
var categories = new[]
{
    new { Name = "A", ID = 101 },
    new { Name = "B", ID = 102 },
};
var products = new[]
{
    new { Name = "Apple", CategoryID = 101 },
    new { Name = "Football", CategoryID = 102 },
    new { Name = "Train", CategoryID = 103 },
    new { Name = "Banana", CategoryID = 101 },
    new { Name = "Car", CategoryID = 103 },
    new { Name = "Basketball", CategoryID = 102 },
};

var nonEquiJoinQuery = from p in products
                       let cates = from c in categories
                                   select c.ID
                       where cates.Contains(p.CategoryID) == true
                       orderby p.CategoryID
                       select p;
foreach (var v in nonEquiJoinQuery)
    Console.WriteLine($"{v.CategoryID}  {v.Name}");
/** Output
    101  Apple
    101  Banana
    102  Football
    102  Basketball
 */
```

<br>

#### 数据分组：GroupBy、ToLookUp

- 分组是指将数据分到不同的组，使每组中的元素拥有公共的属性：
  - `GroupBy`：对共享通用属性的元素进行分组。每组由一个 `IGrouping<TKey,TElement>` 对象表示。对应于查询表达式的 `group-by` 或 `group-by-into` 子句。
  - `ToLookUp`：将元素插入基于键选择器函数的 `Lookup<TKey,TElement>`（一种一对多字典）。

```csharp
List<int> numbers = new List<int>() { 35, 44, 200, 84, 3987, 4, 199, 329, 446, 208 };
// ========= GroupBy
var query = from number in numbers
            orderby number
            group number by number % 2 into gr
            select new { Key = gr.Key == 0 ? "Even Numbers" : "Odd NUmbers", Numbers = gr.ToList() };
var queryFun = numbers
            .OrderBy(num => num)
            .GroupBy(num => num % 2)
            .Select(gr => new { Key = gr.Key == 0 ? "Even Numbers" : "Odd NUmbers", Numbers = gr.ToList() });
foreach (var group in query)
    Console.WriteLine($"{group.Key} : {string.Join(",", group.Numbers)}");
// Even Numbers : 4,44,84,200,208,446
// Odd NUmbers : 35,199,329,398

// ========= LookUp
Lookup<string, List<int>> queryToLookUp = numbers
            .OrderBy(num => num)
            .GroupBy(num => num % 2)
            .ToLookup(gr => gr.Key == 0 ? "Even Numbers" : "Odd NUmbers", gr => gr.ToList())
            as Lookup<string, List<int>>;
foreach (var group in queryToLookUp)
    foreach (var list in queryToLookUp[group.Key])
        Console.WriteLine($"{group.Key} : {string.Join(",", list)}");
// Even Numbers : 4,44,84,200,208,446
// Odd NUmbers : 35,199,329,3987
```

---