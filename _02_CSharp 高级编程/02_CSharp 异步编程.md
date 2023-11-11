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

- TAP 基于 `System.Threading.Tasks` 中的 `Task` 和 `Task<TResult>` 类型。



CPU 绑定：代码执行开销巨大的计算，async/ await 在另外一个线程上使用 Task.Run 生成工作，可考虑使用任务并行库
I/O 绑定：等待某些内容或数据，async/await 不使用 Task.Run 和任务并行库