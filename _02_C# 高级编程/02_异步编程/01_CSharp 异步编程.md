## CSharp 异步编程模式

> .NET 提供了执行异步操作的三种模式：

- 基于任务的异步模式（TAP）：模式使用单一方法表示异步操作的开始和完成。C# 中的 `async` 和 `await` 关键字为 TAP 添加了语言支持。

```csharp
public class MyClass  
{  
    public Task<int> ReadAsync(byte [] buffer, int offset, int count);  
}
```

- 基于事件的异步模式（EAP）：EAP 是用于提供基于事件的异步操作旧模型。这种模式需要后缀为 `Async` 的方法，以及一个或多个事件、事件处理程序委托类型和 `EventArg` 派生类型。

```csharp
public class MyClass  
{  
    public void ReadAsync(byte [] buffer, int offset, int count);  
    public event ReadCompletedEventHandler ReadCompleted;  
}
```

- `IAsyncResult` 异步编程模式（APM）：这是使用 `IAsyncResult` 接口提供异步行为的旧模型。在这种模式下，同步操作需要提供 `Begin` 和 `End` 方法。

```csharp
public class MyClass  
{  
    public IAsyncResult BeginRead(byte [] buffer, int offset, int count, AsyncCallback callback, object state);  
    public int EndRead(IAsyncResult asyncResult);  
}
```

---
### 基于任务的异步模式：TAP

- TAP 基于 `System.Threading.Tasks` 中的 `Task` 和 `Task<TResult>` 类型，用于对异步操作建模。异步操作受 `async` 和 `await` 关键字的支持。常在 I/O 操作和 CPU 的代码绑定上利用异步编程：
  - I/O 绑定：例如网络请求数据、访问数据库、读写文件等，可以构造为异步操作，一般不使用 `Task.Run` 和任务并行库。
  - CPU 绑定：用于执行开销巨大的计算，可以在另外一个线程上使用 `Task.Run` 生成计算工作，可考虑使用任务并行库。

> I/O 绑定：从 Web 服务下载数据

```csharp
await DownloadFromHttp("https://learn.microsoft.com/zh-cn/dotnet/csharp/asynchronous-programming/async-scenarios");

static async Task DownloadFromHttp(string requestUri)
{
    HttpClient hc = new HttpClient();
    var stringData = await hc.GetStringAsync(requestUri);
    Console.WriteLine(stringData?.Length ?? -1);
}
```

> CPU 绑定：为游戏执行计算

```csharp
await CalculateDamage(new int[] { 10, 15, 12, 20, 50 });

static async Task<int> CalculateDamage(IEnumerable<int> damageData)
{
    var damageRt = await Task.Run<int>(damageData.Sum);
    Console.WriteLine($"Deal {damageRt} Damage...");
    return damageRt;
}
// Deal 107 Damage...
```

> .NET 运行时包含支持异步的类型

- Web 访问：`HttpClient`。
- 使用文件：`JsonSerializer`、`StreamReader`、`StreamWriter`、`XmlReader`、`XmlWriter`。
- WCF 编程中的同步异步操作。

> Windows 运行时包含支持异步的类型

- Web 访问：`Windows.Web.Http.HttpClient`、`SyndicationClient`。
- 使用文件：`StorageFile`。
- 使用图像：`MediaCapture`、`BitmapEncoder`、`BitmapDecoder`。

<br>

#### 异步方法的运行机制

```csharp
await CallingMethod();
async Task CallingMethod()
{
    // NO.1
    var task = GetUriContentLengthAsync();
    // NO.7
    Console.WriteLine("Back to CallingMethod");
    // NO.12     // NO.8 ---> 挂起等待 
    int length = await task;
    Console.WriteLine($"The Content Length is {length}");
} // NO.13 End

static async Task<int> GetUriContentLengthAsync()
{
    var client = new HttpClient();

    // NO.3 <-------------------- NO.2        // NO.9 ----> NO.6 ----> NO.10
    Task<string> getStringTask  = client.GetStringAsync("https://docs.microsoft.com/dotnet");

    // NO.4 ---> DoIndependentWork()                                                                
    DoIndependentWork();                                                      
    // NO.10   <------ NO.6 ------> 控制权返回 NO.7
    string contents = await getStringTask;
    return contents.Length;
} // NO.11 ---> Back to CallingMethod


static void DoIndependentWork()
{
    Console.WriteLine("Working ...'");
} // NO.5 ---> Back to GetUriContentLengthAsync

/** Output:
        Working ...'
        Back to CallingMethod
        The Content Length is 84146
 */
```

1. `CallingMethod` 同步调用 `GetUriContentLengthAsync` 异步方法。
2. `GetUriContentLengthAsync` 创建 `HttpClient` 实例并调用 `GetStringAsync` 异步方法。
3. `GetStringAsync` 方法发生某些情况，该情况挂起了它的进程（可能必须等待网站下载或其他的阻止进程的活动）。为避免阻止资源，`GetStringAsync` 返回控制权给调用方 `GetUriContentLengthAsync` 并传递一个 `Task<string>` 任务对象分配给 `getStringTask`。该任务表示关联 `GetStringAsync` 正在进行的进程，其中承诺当工作完成时产生实际的字符串值（发生异常则包含异常信息）。
4. `GetUriContentLengthAsync` 尚未等待（`await`）`getStringTask`，因此该方法可以继续执行不依赖于 `GetStringAsync` 得出的最终结果的其他工作。此时 `GetUriContentLengthAsync` 将调用同步方法 `DoIndependentWork`。
5. `DoIndependentWork` 执行完毕时返回调用方 `GetUriContentLengthAsync`。
6. `GetStringAsync` 是否运行完成，不受 `getStringTask` 的结果影响。`GetUriContentLengthAsync` 方法使用 `await` 运算符来挂起其进度，并将控制权返回到 `CallingMethod`，同时传递当前挂起的任务 `Task<int>` 到 `CallingMethod.task`，该任务表示对产生下载字符串长度的整数结果的一个承诺。（若 `GetStringAsync` 在 `GetUriContentLengthAsync` 等待前完成，则控制权保留并且 `GetUriContentLengthAsync` 不必等待结果，`await` 从 `getStringTask` 查询结果并将承诺的字符串赋值给 `contents`，直接跳到 NO.10 ）。
7. 假定在 `GetUriContentLengthAsync` 挂起时 `GetStringAsync` 未执行完成，控制权传递给 `CallingMethod` 并继续执行进度。
8. 直到 `await` 挂起等待 `GetUriContentLengthAsync` 工作完成，控制权返回 `CallingMethod` 的调用方，然后继续等待。
9. `GetStringAsync` 完成并产生一个字符串结果保存到 `getStringTask` 中，并标记该挂起的任务已完成。 
10. `GetUriContentLengthAsync` 的等待工作结束，`await` 查询字符串结果并赋值给 `contents` 并计算字符串的长度。它的工作也被标记完成。
11. `GetUriContentLengthAsync` 工作完成，标记任务为已完成并将结果存储在 `task` 中。
12. 调用方 `CallingMethod` 的等待事件处理程序可继续使用，`await` 检索 `task` 的长度值并赋值给 `length`，`CallingMethod` 控制返回等待它的调用方或程序结束。 

<br>

#### async 和 await

- 使用 `async` 修饰方法指定为异步方法，将启用：
  - 标记的异步方法使用 `await` 指定暂停点：在等待的异步过程完成后才能继续通过该点，同时将控制返回至异步方法的调用方。异步方法不会构成方法退出。
  - 标记的异步方法本身也可以通过调用它的方法等待。

* 异步方法通常包含一个或若干个 `await` 运算符，缺少 `await` 的 `async` 方法，编译器会为此类方法发布一个警告，该方法会作为同步方法执行。

<br>

#### 异步方法的线程

- 异步方法中的 `await` 表达式在等待的任务正在运行时不会阻止当前线程，表达式会将控制返回到异步方法的调用方，并继续该调用位置之后的其余部分。
- `async` 和 `await` 不会创建其他线程，异步方法不会在其自身线程上运行，不需要创建额外的线程。只有当方法处于活动状态时，该方法将在当前同步上下文中运行并使用线程上的时间。
- `Task.Run` 会将占用大量 CPU 的工作移到后台线程，但是后台线程不会帮助正在等待结果的进程变为可用状态。

<br>

#### 异步方法的返回类型和方法参数限制

- 异步方法通常返回 `Task` 或 `Task<TResult>`，`await` 用于通过调用其他异步方法返回的 `Task`。若方法指定返回 `TResult` 类型操作数，在构造异步方法时指定 `Task<TResult>` 用作返回类型；无返回值时，构造的异步方法指定 `Task` 用作返回类型。

```csharp
// 异步无返回调用
await FunAsync();
// 异步返回调用，返回 string
var len = await FunWithRtAsync();
Console.WriteLine(len);

static async Task FunAsync()
{
    var content = await new HttpClient().GetStringAsync("https://learn.microsoft.com/dotnet");
    Console.WriteLine(content.Length);
}
static async Task<int> FunWithRtAsync()
{
    var content = await new HttpClient().GetStringAsync("https://learn.microsoft.com/dotnet");
    return content.Length;
}
```

- 异步方法可以具有 void 返回类型，该返回类型主要用于定义需要 `void` 返回类型的事件处理程序。异步事件处理程序通常用作异步程序的起始点。

```csharp

```


- 可以返回具有 `GetAwaiter` 方法的任何其他类型。  