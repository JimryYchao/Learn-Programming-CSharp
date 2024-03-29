## 目录

### 第一部分：CSharp 语言介绍

- [1. C./01_CSharp%20语言介绍.md/# 语言简介](./01_CSharp%20语言介绍.md/#1-c-语言简介)
- [2. .NET 体系结构](./01_CSharp%20语言介绍.md/#2-net-体系结构)
- [3. 术语与定义](./01_CSharp%20语言介绍.md/#3-术语与定义)
- [4. C./01_CSharp%20语言介绍.md/# 词法结构](./01_CSharp%20语言介绍.md/#4-c-词法结构)
  - [4.1. C./01_CSharp%20语言介绍.md/# Programs](./01_CSharp%20语言介绍.md/#41-c-programs)
  - [4.2. Tokens](./01_CSharp%20语言介绍.md/#42-tokens)
    - [4.2.1. Unicode 字符转义序列](./01_CSharp%20语言介绍.md/#421-unicode-字符转义序列)
    - [4.2.2. 关键字](./01_CSharp%20语言介绍.md/#422-关键字)
    - [4.2.3. 文本](./01_CSharp%20语言介绍.md/#423-文本)
  - [4.3. 预处理指令](./01_CSharp%20语言介绍.md/#43-预处理指令)
    - [4.3.1. 条件编译指令](./01_CSharp%20语言介绍.md/#431-条件编译指令)
    - [4.3.2. 定义指令](./01_CSharp%20语言介绍.md/#432-定义指令)
    - [4.3.3. 诊断指令](./01_CSharp%20语言介绍.md/#433-诊断指令)
    - [4.3.4. 区域指令](./01_CSharp%20语言介绍.md/#434-区域指令)
    - [4.3.5. 行指令](./01_CSharp%20语言介绍.md/#435-行指令)
    - [4.3.6. 可为空的上下文](./01_CSharp%20语言介绍.md/#436-可为空的上下文)
    - [4.3.7. 编译指示指令](./01_CSharp%20语言介绍.md/#437-编译指示指令)
- [5. 面向对象编程](./01_CSharp%20语言介绍.md/#5-面向对象编程)

>---
### 第二部分：基本概念

- [1. 程序结构](./02_CSharp%20基本概念.md/#1-程序结构)
  - [1.1. 应用启动与终止](./02_CSharp%20基本概念.md/#11-应用启动与终止)
  - [1.2. Main 方法](./02_CSharp%20基本概念.md/#12-main-方法)
  - [1.3. 命令行自变量](./02_CSharp%20基本概念.md/#13-命令行自变量)
  - [1.4. 顶级语句](./02_CSharp%20基本概念.md/#14-顶级语句)
  - [1.5. 终止应用](./02_CSharp%20基本概念.md/#15-终止应用)
  - [1.6. 托管执行和 CIL](./02_CSharp%20基本概念.md/#16-托管执行和-cil)
- [2. 声明](./02_CSharp%20基本概念.md/#2-声明)
- [3. 类型与变量](./02_CSharp%20基本概念.md/#3-类型与变量)
  - [3.1. 在变量声明中指定类型](./02_CSharp%20基本概念.md/#31-在变量声明中指定类型)
  - [3.2. .NET 内置类型](./02_CSharp%20基本概念.md/#32-net-内置类型)
  - [3.3. 非托管类型](./02_CSharp%20基本概念.md/#33-非托管类型)
  - [3.4. .NET 通用类型系统](./02_CSharp%20基本概念.md/#34-net-通用类型系统)
  - [3.5. 泛型类型](./02_CSharp%20基本概念.md/#35-泛型类型)
  - [3.6. 隐式类型、匿名类型和可以为 null 的值类型](./02_CSharp%20基本概念.md/#36-隐式类型匿名类型和可以为-null-的值类型)
  - [3.7. 编译时类型和运行时类型](./02_CSharp%20基本概念.md/#37-编译时类型和运行时类型)
- [4. 成员与可访问性](./02_CSharp%20基本概念.md/#4-成员与可访问性)
  - [4.1. 类型构建成员](./02_CSharp%20基本概念.md/#41-类型构建成员)
    - [4.1.1. 命名空间成员](./02_CSharp%20基本概念.md/#411-命名空间成员)
    - [4.1.2. 结构成员](./02_CSharp%20基本概念.md/#412-结构成员)
    - [4.1.3. 枚举成员](./02_CSharp%20基本概念.md/#413-枚举成员)
    - [4.1.4. 类成员](./02_CSharp%20基本概念.md/#414-类成员)
    - [4.1.5. 接口成员](./02_CSharp%20基本概念.md/#415-接口成员)
    - [4.1.6. 数组成员](./02_CSharp%20基本概念.md/#416-数组成员)
    - [4.1.7. 委托成员](./02_CSharp%20基本概念.md/#417-委托成员)
  - [4.2. 成员访问](./02_CSharp%20基本概念.md/#42-成员访问)
    - [4.2.1. 可访问性声明](./02_CSharp%20基本概念.md/#421-可访问性声明)
    - [4.2.2. 可访问性约束](./02_CSharp%20基本概念.md/#422-可访问性约束)
- [5. 签名与重载](./02_CSharp%20基本概念.md/#5-签名与重载)
- [6. 自动内存管理](./02_CSharp%20基本概念.md/#6-自动内存管理)
- [7. 执行顺序](./02_CSharp%20基本概念.md/#7-执行顺序)

>---
### 第三部分：类型与变量

- [1. 程序结构](./03_CSharp%20类型与变量.md/#1-程序结构)
  - [1.1. 应用启动与终止](./03_CSharp%20类型与变量.md/#11-应用启动与终止)
  - [1.2. Main 方法](./03_CSharp%20类型与变量.md/#12-main-方法)
  - [1.3. 命令行自变量](./03_CSharp%20类型与变量.md/#13-命令行自变量)
  - [1.4. 顶级语句](./03_CSharp%20类型与变量.md/#14-顶级语句)
  - [1.5. 终止应用](./03_CSharp%20类型与变量.md/#15-终止应用)
  - [1.6. 托管执行和 CIL](./03_CSharp%20类型与变量.md/#16-托管执行和-cil)
- [2. 声明](./03_CSharp%20类型与变量.md/#2-声明)
- [3. 类型与变量](./03_CSharp%20类型与变量.md/#3-类型与变量)
  - [3.1. 在变量声明中指定类型](./03_CSharp%20类型与变量.md/#31-在变量声明中指定类型)
  - [3.2. .NET 内置类型](./03_CSharp%20类型与变量.md/#32-net-内置类型)
  - [3.3. 非托管类型](./03_CSharp%20类型与变量.md/#33-非托管类型)
  - [3.4. .NET 通用类型系统](./03_CSharp%20类型与变量.md/#34-net-通用类型系统)
  - [3.5. 泛型类型](./03_CSharp%20类型与变量.md/#35-泛型类型)
  - [3.6. 隐式类型、匿名类型和可以为 null 的值类型](./03_CSharp%20类型与变量.md/#36-隐式类型匿名类型和可以为-null-的值类型)
  - [3.7. 编译时类型和运行时类型](./03_CSharp%20类型与变量.md/#37-编译时类型和运行时类型)
- [4. 成员与可访问性](./03_CSharp%20类型与变量.md/#4-成员与可访问性)
  - [4.1. 类型构建成员](./03_CSharp%20类型与变量.md/#41-类型构建成员)
    - [4.1.1. 命名空间成员](./03_CSharp%20类型与变量.md/#411-命名空间成员)
    - [4.1.2. 结构成员](./03_CSharp%20类型与变量.md/#412-结构成员)
    - [4.1.3. 枚举成员](./03_CSharp%20类型与变量.md/#413-枚举成员)
    - [4.1.4. 类成员](./03_CSharp%20类型与变量.md/#414-类成员)
    - [4.1.5. 接口成员](./03_CSharp%20类型与变量.md/#415-接口成员)
    - [4.1.6. 数组成员](./03_CSharp%20类型与变量.md/#416-数组成员)
    - [4.1.7. 委托成员](./03_CSharp%20类型与变量.md/#417-委托成员)
  - [4.2. 成员访问](./03_CSharp%20类型与变量.md/#42-成员访问)
    - [4.2.1. 可访问性声明](./03_CSharp%20类型与变量.md/#421-可访问性声明)
    - [4.2.2. 可访问性约束](./03_CSharp%20类型与变量.md/#422-可访问性约束)
- [5. 签名与重载](./03_CSharp%20类型与变量.md/#5-签名与重载)
- [6. 自动内存管理](./03_CSharp%20类型与变量.md/#6-自动内存管理)
- [7. 执行顺序](./03_CSharp%20类型与变量.md/#7-执行顺序)

>---
### 第四部分：运算符与表达式

- [1. 静态绑定和动态绑定](./04_CSharp%20运算符与表达式.md/#1-静态绑定和动态绑定)
  - [1.1. 动态绑定](./04_CSharp%20运算符与表达式.md/#11-动态绑定)
  - [1.2. 子表达式的类型](./04_CSharp%20运算符与表达式.md/#12-子表达式的类型)
- [2. 运算符](./04_CSharp%20运算符与表达式.md/#2-运算符)
  - [2.1. 运算符优先级和结合性](./04_CSharp%20运算符与表达式.md/#21-运算符优先级和结合性)
  - [2.2. 运算符重载](./04_CSharp%20运算符与表达式.md/#22-运算符重载)
    - [2.2.1. 可重载的运算符](./04_CSharp%20运算符与表达式.md/#221-可重载的运算符)
- [3. 基本表达式](./04_CSharp%20运算符与表达式.md/#3-基本表达式)
  - [3.1. 文本](./04_CSharp%20运算符与表达式.md/#31-文本)
  - [3.2. 内插字符串](./04_CSharp%20运算符与表达式.md/#32-内插字符串)
  - [3.3. 成员访问](./04_CSharp%20运算符与表达式.md/#33-成员访问)
  - [3.4. this 访问](./04_CSharp%20运算符与表达式.md/#34-this-访问)
  - [3.5. base 访问](./04_CSharp%20运算符与表达式.md/#35-base-访问)
  - [3.6. new 运算符](./04_CSharp%20运算符与表达式.md/#36-new-运算符)
    - [3.6.1. 对象创建](./04_CSharp%20运算符与表达式.md/#361-对象创建)
    - [3.6.2. 对象初始值设定项](./04_CSharp%20运算符与表达式.md/#362-对象初始值设定项)
    - [3.6.3. 集合初始值设定项](./04_CSharp%20运算符与表达式.md/#363-集合初始值设定项)
    - [3.6.4. 委托创建](./04_CSharp%20运算符与表达式.md/#364-委托创建)
    - [3.6.5. 匿名类型创建](./04_CSharp%20运算符与表达式.md/#365-匿名类型创建)
    - [3.6.6. 数组的创建](./04_CSharp%20运算符与表达式.md/#366-数组的创建)
  - [3.7. typeof 运算符](./04_CSharp%20运算符与表达式.md/#37-typeof-运算符)
  - [3.8. sizeof 运算符](./04_CSharp%20运算符与表达式.md/#38-sizeof-运算符)
  - [3.9. nameof 表达式](./04_CSharp%20运算符与表达式.md/#39-nameof-表达式)
  - [3.10. 空合并运算符](./04_CSharp%20运算符与表达式.md/#310-空合并运算符)
  - [3.11. 弃元](./04_CSharp%20运算符与表达式.md/#311-弃元)
  - [3.12. 空包容运算符](./04_CSharp%20运算符与表达式.md/#312-空包容运算符)
  - [3.13. checked 和 unchecked 运算符](./04_CSharp%20运算符与表达式.md/#313-checked-和-unchecked-运算符)
  - [3.14. default 表达式](./04_CSharp%20运算符与表达式.md/#314-default-表达式)
  - [3.15. stackalloc 表达式](./04_CSharp%20运算符与表达式.md/#315-stackalloc-表达式)
  - [3.16. with 表达式](./04_CSharp%20运算符与表达式.md/#316-with-表达式)
  - [3.17. await 表达式](./04_CSharp%20运算符与表达式.md/#317-await-表达式)
    - [3.17.1. awaitable expression](./04_CSharp%20运算符与表达式.md/#3171-awaitable-expression)
    - [3.17.2. await 表达式的运行时求值顺序](./04_CSharp%20运算符与表达式.md/#3172-await-表达式的运行时求值顺序)
  - [3.18. 匿名函数](./04_CSharp%20运算符与表达式.md/#318-匿名函数)
    - [3.18.1. delegate 匿名方法表达式](./04_CSharp%20运算符与表达式.md/#3181-delegate-匿名方法表达式)
    - [3.18.2. Lambda 表达式](./04_CSharp%20运算符与表达式.md/#3182-lambda-表达式)
    - [3.18.3. 匿名函数签名](./04_CSharp%20运算符与表达式.md/#3183-匿名函数签名)
    - [3.18.4. 匿名函数主体](./04_CSharp%20运算符与表达式.md/#3184-匿名函数主体)
    - [3.18.5. 外部变量](./04_CSharp%20运算符与表达式.md/#3185-外部变量)
- [4. 算数运算符](./04_CSharp%20运算符与表达式.md/#4-算数运算符)
- [5. 关系运算符](./04_CSharp%20运算符与表达式.md/#5-关系运算符)
  - [5.1. 相等性比较](./04_CSharp%20运算符与表达式.md/#51-相等性比较)
    - [5.1.1. 值相等性](./04_CSharp%20运算符与表达式.md/#511-值相等性)
    - [5.1.2. 用户定义类型相等性](./04_CSharp%20运算符与表达式.md/#512-用户定义类型相等性)
    - [5.1.3. 浮点值的相等性](./04_CSharp%20运算符与表达式.md/#513-浮点值的相等性)
    - [5.1.4. 为 ref 变量创建引用相等性比较](./04_CSharp%20运算符与表达式.md/#514-为-ref-变量创建引用相等性比较)
- [6. 类型测试](./04_CSharp%20运算符与表达式.md/#6-类型测试)
  - [6.1. is 运算符](./04_CSharp%20运算符与表达式.md/#61-is-运算符)
    - [6.1.1. 检查 null 值](./04_CSharp%20运算符与表达式.md/#611-检查-null-值)
  - [6.2. as 运算符](./04_CSharp%20运算符与表达式.md/#62-as-运算符)
  - [6.3. typeof 类型测试](./04_CSharp%20运算符与表达式.md/#63-typeof-类型测试)
- [7. 逻辑运算与条件逻辑运算](./04_CSharp%20运算符与表达式.md/#7-逻辑运算与条件逻辑运算)
  - [7.1. 用户定义条件逻辑运算符](./04_CSharp%20运算符与表达式.md/#71-用户定义条件逻辑运算符)
- [8. 条件运算符](./04_CSharp%20运算符与表达式.md/#8-条件运算符)
  - [8.1. ref 条件表达式](./04_CSharp%20运算符与表达式.md/#81-ref-条件表达式)
- [9. 位运算和移位运算](./04_CSharp%20运算符与表达式.md/#9-位运算和移位运算)
- [10. 类型转换](./04_CSharp%20运算符与表达式.md/#10-类型转换)
  - [10.1. 隐式转换](./04_CSharp%20运算符与表达式.md/#101-隐式转换)
    - [10.1.1. 隐式恒等转换](./04_CSharp%20运算符与表达式.md/#1011-隐式恒等转换)
    - [10.1.2. 隐式数值转换](./04_CSharp%20运算符与表达式.md/#1012-隐式数值转换)
    - [10.1.3. 隐式枚举转换](./04_CSharp%20运算符与表达式.md/#1013-隐式枚举转换)
    - [10.1.4. 隐式内插字符串转换](./04_CSharp%20运算符与表达式.md/#1014-隐式内插字符串转换)
    - [10.1.5. 隐式可空转换](./04_CSharp%20运算符与表达式.md/#1015-隐式可空转换)
    - [10.1.6. null 的转换](./04_CSharp%20运算符与表达式.md/#1016-null-的转换)
    - [10.1.7. 隐式引用转换](./04_CSharp%20运算符与表达式.md/#1017-隐式引用转换)
    - [10.1.8. 装箱转换](./04_CSharp%20运算符与表达式.md/#1018-装箱转换)
    - [10.1.9. 隐式动态转换](./04_CSharp%20运算符与表达式.md/#1019-隐式动态转换)
    - [10.1.10. 隐式常量表达式转换](./04_CSharp%20运算符与表达式.md/#10110-隐式常量表达式转换)
    - [10.1.11. 涉及类型参数的隐式转换](./04_CSharp%20运算符与表达式.md/#10111-涉及类型参数的隐式转换)
    - [10.1.12. 隐式元组转换](./04_CSharp%20运算符与表达式.md/#10112-隐式元组转换)
    - [10.1.13. 用户定义的隐式转换](./04_CSharp%20运算符与表达式.md/#10113-用户定义的隐式转换)
    - [10.1.14. 匿名函数转换和方法组转换](./04_CSharp%20运算符与表达式.md/#10114-匿名函数转换和方法组转换)
    - [10.1.15. 默认值转换](./04_CSharp%20运算符与表达式.md/#10115-默认值转换)
    - [10.1.16. 隐式抛出转换](./04_CSharp%20运算符与表达式.md/#10116-隐式抛出转换)
  - [10.2. 显式转换](./04_CSharp%20运算符与表达式.md/#102-显式转换)
    - [10.2.1. 显式数字转换](./04_CSharp%20运算符与表达式.md/#1021-显式数字转换)
    - [10.2.2. 显式枚举转换](./04_CSharp%20运算符与表达式.md/#1022-显式枚举转换)
    - [10.2.3. 显式可空转换](./04_CSharp%20运算符与表达式.md/#1023-显式可空转换)
    - [10.2.4. 显式引用转换](./04_CSharp%20运算符与表达式.md/#1024-显式引用转换)
    - [10.2.5. 显式元组转换](./04_CSharp%20运算符与表达式.md/#1025-显式元组转换)
    - [10.2.6. 拆箱转换](./04_CSharp%20运算符与表达式.md/#1026-拆箱转换)
    - [10.2.7. 显式动态转换](./04_CSharp%20运算符与表达式.md/#1027-显式动态转换)
    - [10.2.8. 涉及类型参数的显式转换](./04_CSharp%20运算符与表达式.md/#1028-涉及类型参数的显式转换)
    - [10.2.9. 用户定义的显式转换](./04_CSharp%20运算符与表达式.md/#1029-用户定义的显式转换)
  - [10.3. 用户定义转换](./04_CSharp%20运算符与表达式.md/#103-用户定义转换)
  - [10.4. 匿名方法的转换](./04_CSharp%20运算符与表达式.md/#104-匿名方法的转换)
    - [10.4.1. 匿名方法转换到委托类型的求值](./04_CSharp%20运算符与表达式.md/#1041-匿名方法转换到委托类型的求值)
    - [10.4.2. Lambda 表达式转换到表达式树类型的求值](./04_CSharp%20运算符与表达式.md/#1042-lambda-表达式转换到表达式树类型的求值)
  - [10.5. 方法组的转换](./04_CSharp%20运算符与表达式.md/#105-方法组的转换)
  - [10.6. 帮助程序类转换](./04_CSharp%20运算符与表达式.md/#106-帮助程序类转换)
- [11. 模式匹配](./04_CSharp%20运算符与表达式.md/#11-模式匹配)
  - [11.1. 模式匹配表达式](./04_CSharp%20运算符与表达式.md/#111-模式匹配表达式)
    - [11.1.1. is 模式](./04_CSharp%20运算符与表达式.md/#1111-is-模式)
    - [11.1.2. switch 语句](./04_CSharp%20运算符与表达式.md/#1112-switch-语句)
    - [11.1.3. switch 表达式](./04_CSharp%20运算符与表达式.md/#1113-switch-表达式)
  - [11.2. 声明模式](./04_CSharp%20运算符与表达式.md/#112-声明模式)
  - [11.3. 类型模式](./04_CSharp%20运算符与表达式.md/#113-类型模式)
  - [11.4. 常量模式](./04_CSharp%20运算符与表达式.md/#114-常量模式)
  - [11.5. 关系模式](./04_CSharp%20运算符与表达式.md/#115-关系模式)
  - [11.6. 逻辑模式](./04_CSharp%20运算符与表达式.md/#116-逻辑模式)
  - [11.7. 属性模式](./04_CSharp%20运算符与表达式.md/#117-属性模式)
  - [11.8. 位置模式](./04_CSharp%20运算符与表达式.md/#118-位置模式)
  - [11.9. var 模式](./04_CSharp%20运算符与表达式.md/#119-var-模式)
  - [11.10. 弃元模式](./04_CSharp%20运算符与表达式.md/#1110-弃元模式)
  - [11.11. 列表模式](./04_CSharp%20运算符与表达式.md/#1111-列表模式)
- [12. LINQ 和查询表达式](./04_CSharp%20运算符与表达式.md/#12-linq-和查询表达式)
  - [12.1. 查询表达式概述](./04_CSharp%20运算符与表达式.md/#121-查询表达式概述)
  - [12.2. 查询操作的三个部分](./04_CSharp%20运算符与表达式.md/#122-查询操作的三个部分)
    - [12.2.1. 获取数据源](./04_CSharp%20运算符与表达式.md/#1221-获取数据源)
    - [12.2.2. 创建查询](./04_CSharp%20运算符与表达式.md/#1222-创建查询)
    - [12.2.3. 执行查询](./04_CSharp%20运算符与表达式.md/#1223-执行查询)
  - [12.3. 查询表达式](./04_CSharp%20运算符与表达式.md/#123-查询表达式)
    - [12.3.1. from-in 子句：获取数据源](./04_CSharp%20运算符与表达式.md/#1231-from-in-子句获取数据源)
    - [12.3.2. where 子句：筛选](./04_CSharp%20运算符与表达式.md/#1232-where-子句筛选)
    - [12.3.3. select 子句：选择与投影](./04_CSharp%20运算符与表达式.md/#1233-select-子句选择与投影)
    - [12.3.4. group 子句：分组](./04_CSharp%20运算符与表达式.md/#1234-group-子句分组)
    - [12.3.5. into 子句：附加查询](./04_CSharp%20运算符与表达式.md/#1235-into-子句附加查询)
    - [12.3.6. orderby 子句：中间件排序](./04_CSharp%20运算符与表达式.md/#1236-orderby-子句中间件排序)
    - [12.3.7. join-in-on-equals 子句：联接](./04_CSharp%20运算符与表达式.md/#1237-join-in-on-equals-子句联接)
    - [12.3.8. let 子句：引入范围变量](./04_CSharp%20运算符与表达式.md/#1238-let-子句引入范围变量)
  - [12.4. 标准查询运算符](./04_CSharp%20运算符与表达式.md/#124-标准查询运算符)
    - [12.4.1. 筛选数据：OfType、Where](./04_CSharp%20运算符与表达式.md/#1241-筛选数据oftypewhere)
    - [12.4.2. 投影运算：Select、SelectMany、Zip](./04_CSharp%20运算符与表达式.md/#1242-投影运算selectselectmanyzip)
    - [12.4.3. 集合操作：Distinct、Except、Intersect、Union](./04_CSharp%20运算符与表达式.md/#1243-集合操作distinctexceptintersectunion)
    - [12.4.4. 排序操作：OrderBy、ThenBy、Reverse](./04_CSharp%20运算符与表达式.md/#1244-排序操作orderbythenbyreverse)
    - [12.4.5. 限定符运算：All、Any、Contains](./04_CSharp%20运算符与表达式.md/#1245-限定符运算allanycontains)
    - [12.4.6. 数据分区：Skip、Take、Chunk](./04_CSharp%20运算符与表达式.md/#1246-数据分区skiptakechunk)
    - [12.4.7. 生成运算：DefaultIfEmpty、Empty、Range、Repeat](./04_CSharp%20运算符与表达式.md/#1247-生成运算defaultifemptyemptyrangerepeat)
    - [12.4.8. 相等运算：SequenceEqual](./04_CSharp%20运算符与表达式.md/#1248-相等运算sequenceequal)
    - [12.4.9. 元素运算：ElementAt、First、Last、Single](./04_CSharp%20运算符与表达式.md/#1249-元素运算elementatfirstlastsingle)
    - [12.4.10. 转换数据类型：AsEnumerable、AsQueryable、OfType、ToArray、ToList、ToHashSet、ToDictionary、ToLookUp](./04_CSharp%20运算符与表达式.md/#12410-转换数据类型asenumerableasqueryableoftypetoarraytolisttohashsettodictionarytolookup)
    - [12.4.11. 附加运算：Concat、Append、Prepend](./04_CSharp%20运算符与表达式.md/#12411-附加运算concatappendprepend)
    - [12.4.12. 聚合运算：Aggregate、Average、Count、LongCount、Max、Min、Sum](./04_CSharp%20运算符与表达式.md/#12412-聚合运算aggregateaveragecountlongcountmaxminsum)
    - [12.4.13. 联接运算：Join、GroupJoin](./04_CSharp%20运算符与表达式.md/#12413-联接运算joingroupjoin)
    - [12.4.14. 数据分组：GroupBy、ToLookUp](./04_CSharp%20运算符与表达式.md/#12414-数据分组groupbytolookup)
- [13. 集合表达式](./04_CSharp%20运算符与表达式.md/#13-集合表达式)
  - [13.1. 内联集合值](./04_CSharp%20运算符与表达式.md/#131-内联集合值)
  - [13.2. 集合表达式转换](./04_CSharp%20运算符与表达式.md/#132-集合表达式转换)
  - [13.3. 集合表达式实例的构造过程](./04_CSharp%20运算符与表达式.md/#133-集合表达式实例的构造过程)
  - [13.4. 为 IDictionary 类型扩展集合表达式构造语法](./04_CSharp%20运算符与表达式.md/#134-为-idictionary-类型扩展集合表达式构造语法)
  - [13.5. 集合生成器](./04_CSharp%20运算符与表达式.md/#135-集合生成器)
    - [13.5.1. 泛型集合生成器](./04_CSharp%20运算符与表达式.md/#1351-泛型集合生成器)
- [14. 索引与范围运算符](./04_CSharp%20运算符与表达式.md/#14-索引与范围运算符)
  - [14.1. Index 与 Range](./04_CSharp%20运算符与表达式.md/#141-index-与-range)
  - [14.2. 索引和范围的类型支持](./04_CSharp%20运算符与表达式.md/#142-索引和范围的类型支持)
  - [14.3. 索引和范围的隐式支持](./04_CSharp%20运算符与表达式.md/#143-索引和范围的隐式支持)
- [15. 指针相关的运算符](./04_CSharp%20运算符与表达式.md/#15-指针相关的运算符)

>---
### 第五部分：语句

- [1. 声明语句](./05_CSharp%20语句.md/#1-声明语句)
- [2. 空语句](./05_CSharp%20语句.md/#2-空语句)
- [3. 选择语句](./05_CSharp%20语句.md/#3-选择语句)
  - [3.1. if 语句](./05_CSharp%20语句.md/#31-if-语句)
  - [3.2. switch 语句](./05_CSharp%20语句.md/#32-switch-语句)
- [4. 迭代语句](./05_CSharp%20语句.md/#4-迭代语句)
  - [4.1. for 语句](./05_CSharp%20语句.md/#41-for-语句)
  - [4.2. foreach 语句](./05_CSharp%20语句.md/#42-foreach-语句)
    - [4.2.1. foreach 编译时处理](./05_CSharp%20语句.md/#421-foreach-编译时处理)
    - [4.2.2. await foreach 异步流](./05_CSharp%20语句.md/#422-await-foreach-异步流)
  - [4.3. do 语句](./05_CSharp%20语句.md/#43-do-语句)
  - [4.4. while 语句](./05_CSharp%20语句.md/#44-while-语句)
- [5. 跳转语句](./05_CSharp%20语句.md/#5-跳转语句)
  - [5.1. break 语句](./05_CSharp%20语句.md/#51-break-语句)
  - [5.2. continue 语句](./05_CSharp%20语句.md/#52-continue-语句)
  - [5.3. return 语句](./05_CSharp%20语句.md/#53-return-语句)
  - [5.4. goto 语句](./05_CSharp%20语句.md/#54-goto-语句)
  - [5.5. yield 语句](./05_CSharp%20语句.md/#55-yield-语句)
  - [5.6. 异常处理语句](./05_CSharp%20语句.md/#56-异常处理语句)
    - [5.6.1. throw 表达式或语句](./05_CSharp%20语句.md/#561-throw-表达式或语句)
    - [5.6.2. try-catch](./05_CSharp%20语句.md/#562-try-catch)
    - [5.6.3. try-finally](./05_CSharp%20语句.md/#563-try-finally)
- [6. checked 和 unchecked 语句](./05_CSharp%20语句.md/#6-checked-和-unchecked-语句)
- [7. lock 语句](./05_CSharp%20语句.md/#7-lock-语句)
- [8. using 语句](./05_CSharp%20语句.md/#8-using-语句)

>---
### 第六部分：程序构建基块

- [1. 命名空间](./06_CSharp%20程序构建基块.md/#1-命名空间)
  - [1.1. 全局命名空间](./06_CSharp%20程序构建基块.md/#11-全局命名空间)
  - [1.2. 文件范围的命名空间](./06_CSharp%20程序构建基块.md/#12-文件范围的命名空间)
  - [1.3. using 指令](./06_CSharp%20程序构建基块.md/#13-using-指令)
    - [1.3.1. using namespace 指令](./06_CSharp%20程序构建基块.md/#131-using-namespace-指令)
    - [1.3.2. using 别名指令](./06_CSharp%20程序构建基块.md/#132-using-别名指令)
    - [1.3.3. using static 指令](./06_CSharp%20程序构建基块.md/#133-using-static-指令)
  - [1.4. 程序集外部别名](./06_CSharp%20程序构建基块.md/#14-程序集外部别名)
  - [1.5. 全局 using 指令](./06_CSharp%20程序构建基块.md/#15-全局-using-指令)
  - [1.6. 任何类型的别名](./06_CSharp%20程序构建基块.md/#16-任何类型的别名)
- [2. 类型构建基块](./06_CSharp%20程序构建基块.md/#2-类型构建基块)
  - [2.1. 静态成员和实例成员](./06_CSharp%20程序构建基块.md/#21-静态成员和实例成员)
  - [2.2. 保留的成员名称](./06_CSharp%20程序构建基块.md/#22-保留的成员名称)
    - [2.2.1. 为属性保留的成员名](./06_CSharp%20程序构建基块.md/#221-为属性保留的成员名)
    - [2.2.2. 为事件保留的成员名](./06_CSharp%20程序构建基块.md/#222-为事件保留的成员名)
    - [2.2.3. 为索引保留的成员名](./06_CSharp%20程序构建基块.md/#223-为索引保留的成员名)
    - [2.2.4. 为终结器保留的成员名](./06_CSharp%20程序构建基块.md/#224-为终结器保留的成员名)
  - [2.3. 常量](./06_CSharp%20程序构建基块.md/#23-常量)
  - [2.4. 字段](./06_CSharp%20程序构建基块.md/#24-字段)
    - [2.4.1. volatile 字段](./06_CSharp%20程序构建基块.md/#241-volatile-字段)
    - [2.4.2. readonly 字段](./06_CSharp%20程序构建基块.md/#242-readonly-字段)
    - [2.4.3. required 字段](./06_CSharp%20程序构建基块.md/#243-required-字段)
    - [2.4.4. ref 结构：ref 字段](./06_CSharp%20程序构建基块.md/#244-ref-结构ref-字段)
  - [2.5. 方法](./06_CSharp%20程序构建基块.md/#25-方法)
    - [2.5.1. 静态和实例方法](./06_CSharp%20程序构建基块.md/#251-静态和实例方法)
    - [2.5.2. 方法参数](./06_CSharp%20程序构建基块.md/#252-方法参数)
    - [2.5.3. 方法参数修饰符](./06_CSharp%20程序构建基块.md/#253-方法参数修饰符)
    - [2.5.4. 命名参数](./06_CSharp%20程序构建基块.md/#254-命名参数)
    - [2.5.5. 可选参数](./06_CSharp%20程序构建基块.md/#255-可选参数)
    - [2.5.6. 方法主体和局部变量](./06_CSharp%20程序构建基块.md/#256-方法主体和局部变量)
    - [2.5.7. 方法重载](./06_CSharp%20程序构建基块.md/#257-方法重载)
    - [2.5.8. 虚方法、重写方法、抽象方法和密封方法](./06_CSharp%20程序构建基块.md/#258-虚方法重写方法抽象方法和密封方法)
    - [2.5.9. 外部方法](./06_CSharp%20程序构建基块.md/#259-外部方法)
    - [2.5.10. 局部函数](./06_CSharp%20程序构建基块.md/#2510-局部函数)
    - [2.5.11. 方法协变返回](./06_CSharp%20程序构建基块.md/#2511-方法协变返回)
    - [2.5.12. 静态类：扩展方法](./06_CSharp%20程序构建基块.md/#2512-静态类扩展方法)
    - [2.5.13. ref 方法返回](./06_CSharp%20程序构建基块.md/#2513-ref-方法返回)
    - [2.5.14. 结构：readonly 方法](./06_CSharp%20程序构建基块.md/#2514-结构readonly-方法)
  - [2.6. 属性](./06_CSharp%20程序构建基块.md/#26-属性)
    - [2.6.1. 属性访问器](./06_CSharp%20程序构建基块.md/#261-属性访问器)
    - [2.6.2. 如何实现不可变属性](./06_CSharp%20程序构建基块.md/#262-如何实现不可变属性)
    - [2.6.3. 自动实现的属性](./06_CSharp%20程序构建基块.md/#263-自动实现的属性)
    - [2.6.4. 虚属性、重写属性、抽象属性和密封属性（类专属）](./06_CSharp%20程序构建基块.md/#264-虚属性重写属性抽象属性和密封属性类专属)
    - [2.6.5. 接口属性](./06_CSharp%20程序构建基块.md/#265-接口属性)
    - [2.6.6. ref 属性](./06_CSharp%20程序构建基块.md/#266-ref-属性)
    - [2.6.7. readonly 属性（结构专属）](./06_CSharp%20程序构建基块.md/#267-readonly-属性结构专属)
    - [2.6.8. required 属性](./06_CSharp%20程序构建基块.md/#268-required-属性)
    - [2.6.9. 记录中的主构造函数](./06_CSharp%20程序构建基块.md/#269-记录中的主构造函数)
  - [2.7. 索引器](./06_CSharp%20程序构建基块.md/#27-索引器)
    - [2.7.1. ref 返回](./06_CSharp%20程序构建基块.md/#271-ref-返回)
    - [2.7.2. readonly 索引器](./06_CSharp%20程序构建基块.md/#272-readonly-索引器)
    - [2.7.3. 为索引器提供索引和范围运算](./06_CSharp%20程序构建基块.md/#273-为索引器提供索引和范围运算)
    - [2.7.4. 接口中的索引器](./06_CSharp%20程序构建基块.md/#274-接口中的索引器)
  - [2.8. 事件](./06_CSharp%20程序构建基块.md/#28-事件)
    - [2.8.1. 事件访问器](./06_CSharp%20程序构建基块.md/#281-事件访问器)
    - [2.8.2. 事件的订阅与取消](./06_CSharp%20程序构建基块.md/#282-事件的订阅与取消)
    - [2.8.3. readonly 事件](./06_CSharp%20程序构建基块.md/#283-readonly-事件)
    - [2.8.4. 标准 .NET 事件模式](./06_CSharp%20程序构建基块.md/#284-标准-net-事件模式)
    - [2.8.5. 异步事件订阅](./06_CSharp%20程序构建基块.md/#285-异步事件订阅)
    - [2.8.6. 委托与事件的区别](./06_CSharp%20程序构建基块.md/#286-委托与事件的区别)
  - [2.9. 运算符](./06_CSharp%20程序构建基块.md/#29-运算符)
    - [2.9.1. 可重载的运算符](./06_CSharp%20程序构建基块.md/#291-可重载的运算符)
    - [2.9.2. checked 用户定义算数运算符](./06_CSharp%20程序构建基块.md/#292-checked-用户定义算数运算符)
    - [2.9.3. 用户定义类型转换](./06_CSharp%20程序构建基块.md/#293-用户定义类型转换)
  - [2.10. 实例构造函数](./06_CSharp%20程序构建基块.md/#210-实例构造函数)
    - [2.10.1. 构造函数初始化器](./06_CSharp%20程序构建基块.md/#2101-构造函数初始化器)
    - [2.10.2. 主构造函数](./06_CSharp%20程序构建基块.md/#2102-主构造函数)
    - [2.10.3. 复制构造函数](./06_CSharp%20程序构建基块.md/#2103-复制构造函数)
  - [2.11. 静态构造函数](./06_CSharp%20程序构建基块.md/#211-静态构造函数)
    - [2.11.1. 结构中的静态构造函数](./06_CSharp%20程序构建基块.md/#2111-结构中的静态构造函数)
    - [2.11.2. 接口中的静态构造函数](./06_CSharp%20程序构建基块.md/#2112-接口中的静态构造函数)
  - [2.12. 终结器](./06_CSharp%20程序构建基块.md/#212-终结器)
- [3. 分部声明](./06_CSharp%20程序构建基块.md/#3-分部声明)
  - [3.1. 分部类型](./06_CSharp%20程序构建基块.md/#31-分部类型)
  - [3.2. 分部方法](./06_CSharp%20程序构建基块.md/#32-分部方法)
- [4. 迭代器](./06_CSharp%20程序构建基块.md/#4-迭代器)
  - [4.1. 枚举器接口和可枚举接口](./06_CSharp%20程序构建基块.md/#41-枚举器接口和可枚举接口)
  - [4.2. yield 类型](./06_CSharp%20程序构建基块.md/#42-yield-类型)
  - [4.3. 迭代器对象](./06_CSharp%20程序构建基块.md/#43-迭代器对象)
    - [4.3.1. MoveNext 方法](./06_CSharp%20程序构建基块.md/#431-movenext-方法)
    - [4.3.2. Current 属性](./06_CSharp%20程序构建基块.md/#432-current-属性)
    - [4.3.3. Dispose 方法](./06_CSharp%20程序构建基块.md/#433-dispose-方法)
  - [4.4. 可枚举对象](./06_CSharp%20程序构建基块.md/#44-可枚举对象)
    - [4.4.1. GetEnumerator](./06_CSharp%20程序构建基块.md/#441-getenumerator)
  - [4.5. 实现一个可枚举对象和对应的枚举器](./06_CSharp%20程序构建基块.md/#45-实现一个可枚举对象和对应的枚举器)
- [5. 异步方法](./06_CSharp%20程序构建基块.md/#5-异步方法)
  - [5.1. *Task Type* 构建器](./06_CSharp%20程序构建基块.md/#51-task-type-构建器)
  - [5.2. *Task-returning* 异步方法的求值](./06_CSharp%20程序构建基块.md/#52-task-returning-异步方法的求值)
  - [5.3. *Void-returning* 异步方法的求值](./06_CSharp%20程序构建基块.md/#53-void-returning-异步方法的求值)
- [6. 异常](./06_CSharp%20程序构建基块.md/#6-异常)
  - [6.1. System.Exception](./06_CSharp%20程序构建基块.md/#61-systemexception)
  - [6.2. 异常处理](./06_CSharp%20程序构建基块.md/#62-异常处理)
  - [6.3. 重新引发异常](./06_CSharp%20程序构建基块.md/#63-重新引发异常)
  - [6.4. 定义异常的类别](./06_CSharp%20程序构建基块.md/#64-定义异常的类别)
  - [6.5. 常见 .NET 异常类型](./06_CSharp%20程序构建基块.md/#65-常见-net-异常类型)
  - [6.6. 捕捉非 CLS 异常](./06_CSharp%20程序构建基块.md/#66-捕捉非-cls-异常)

>---
### 第七部分：特性

- [1. 特性简介](./07_CSharp%20特性.md/#1-特性简介)
  - [1.1. Attribute 类](./07_CSharp%20特性.md/#11-attribute-类)
  - [1.2. 位置参数与命名参数](./07_CSharp%20特性.md/#12-位置参数与命名参数)
    - [1.2.1. 特性参数类型](./07_CSharp%20特性.md/#121-特性参数类型)
  - [1.3. 特性规范](./07_CSharp%20特性.md/#13-特性规范)
    - [1.3.1. 命名规范与歧义](./07_CSharp%20特性.md/#131-命名规范与歧义)
  - [1.4. 特性实例](./07_CSharp%20特性.md/#14-特性实例)
    - [1.4.1. 特性的编译](./07_CSharp%20特性.md/#141-特性的编译)
    - [1.4.2. 运行时检索特性实例](./07_CSharp%20特性.md/#142-运行时检索特性实例)
- [2. 保留特性](./07_CSharp%20特性.md/#2-保留特性)
  - [2.1. AttributeUsage 特性使用范围](./07_CSharp%20特性.md/#21-attributeusage-特性使用范围)
  - [2.2. Conditional 条件编译](./07_CSharp%20特性.md/#22-conditional-条件编译)
  - [2.3. Obsolete 过时](./07_CSharp%20特性.md/#23-obsolete-过时)
  - [2.4. Caller-info](./07_CSharp%20特性.md/#24-caller-info)
- [3. 语言特性](./07_CSharp%20特性.md/#3-语言特性)
  - [3.1. Flags 枚举位标志](./07_CSharp%20特性.md/#31-flags-枚举位标志)
  - [3.2. SetsRequiredMembers 构造函数](./07_CSharp%20特性.md/#32-setsrequiredmembers-构造函数)
- [4. 互操作特性](./07_CSharp%20特性.md/#4-互操作特性)
  - [4.1. IndexerName 索引器名称](./07_CSharp%20特性.md/#41-indexername-索引器名称)
- [5. .NET 运行时的特性支持](./07_CSharp%20特性.md/#5-net-运行时的特性支持)
  - [5.1. ModuleInitializer 模块初始化器](./07_CSharp%20特性.md/#51-moduleinitializer-模块初始化器)
  - [5.2. SkipLocalsInit 抑制 `.locals init` 标志发出](./07_CSharp%20特性.md/#52-skiplocalsinit-抑制-locals-init-标志发出)
  - [5.3. CollectionBuilder 集合生成器](./07_CSharp%20特性.md/#53-collectionbuilder-集合生成器)
- [6. 代码分析特性](./07_CSharp%20特性.md/#6-代码分析特性)
  - [6.1. 可空特性](./07_CSharp%20特性.md/#61-可空特性)
  - [6.2. Experimental 实验性 API（C./07_CSharp%20特性.md/#12）](./07_CSharp%20特性.md/#62-experimental-实验性-apic12)
- [7. 程序集特性](./07_CSharp%20特性.md/#7-程序集特性)
- [8. 编辑器特性](./07_CSharp%20特性.md/#8-编辑器特性)

>---
### 第八部分：XML 文档注释

- [1. 创建 XML 文档输出](./08_CSharp%20XML%20文档注释.md/#1-创建-xml-文档输出)
  - [1.1. ID 字符串](./08_CSharp%20XML%20文档注释.md/#11-id-字符串)
- [2. XML 文档标记元素](./08_CSharp%20XML%20文档注释.md/#2-xml-文档标记元素)
  - [2.1. 常规标记](./08_CSharp%20XML%20文档注释.md/#21-常规标记)
    - [2.1.1. summary](./08_CSharp%20XML%20文档注释.md/#211-summary)
    - [2.1.2. remarks](./08_CSharp%20XML%20文档注释.md/#212-remarks)
  - [2.2. 用于成员的标记](./08_CSharp%20XML%20文档注释.md/#22-用于成员的标记)
    - [2.2.1. returns](./08_CSharp%20XML%20文档注释.md/#221-returns)
    - [2.2.2. param](./08_CSharp%20XML%20文档注释.md/#222-param)
    - [2.2.3. paramref](./08_CSharp%20XML%20文档注释.md/#223-paramref)
    - [2.2.4. exception](./08_CSharp%20XML%20文档注释.md/#224-exception)
    - [2.2.5. value](./08_CSharp%20XML%20文档注释.md/#225-value)
    - [2.2.6. permission](./08_CSharp%20XML%20文档注释.md/#226-permission)
  - [2.3. 设置文档输出格式](./08_CSharp%20XML%20文档注释.md/#23-设置文档输出格式)
    - [2.3.1. para](./08_CSharp%20XML%20文档注释.md/#231-para)
    - [2.3.2. list](./08_CSharp%20XML%20文档注释.md/#232-list)
    - [2.3.3. c](./08_CSharp%20XML%20文档注释.md/#233-c)
    - [2.3.4. code](./08_CSharp%20XML%20文档注释.md/#234-code)
    - [2.3.5. example](./08_CSharp%20XML%20文档注释.md/#235-example)
  - [2.4. 重用文档文本](./08_CSharp%20XML%20文档注释.md/#24-重用文档文本)
    - [2.4.1. inheritdoc](./08_CSharp%20XML%20文档注释.md/#241-inheritdoc)
    - [2.4.2. include](./08_CSharp%20XML%20文档注释.md/#242-include)
  - [2.5. 生成链接和引用](./08_CSharp%20XML%20文档注释.md/#25-生成链接和引用)
    - [2.5.1. see](./08_CSharp%20XML%20文档注释.md/#251-see)
    - [2.5.2. seealso](./08_CSharp%20XML%20文档注释.md/#252-seealso)
  - [2.6. 用于泛型类型和方法的标记](./08_CSharp%20XML%20文档注释.md/#26-用于泛型类型和方法的标记)
    - [2.6.1. typeparam](./08_CSharp%20XML%20文档注释.md/#261-typeparam)
    - [2.6.2. typeparamref](./08_CSharp%20XML%20文档注释.md/#262-typeparamref)
  - [2.7. 用户定义标记](./08_CSharp%20XML%20文档注释.md/#27-用户定义标记)
  - [2.8. XML 标记案例](./08_CSharp%20XML%20文档注释.md/#28-xml-标记案例)
    - [2.8.1. 记录类和接口的层次结构](./08_CSharp%20XML%20文档注释.md/#281-记录类和接口的层次结构)
    - [2.8.2. 泛型类型](./08_CSharp%20XML%20文档注释.md/#282-泛型类型)
- [3. XML 文档创建输出工具](./08_CSharp%20XML%20文档注释.md/#3-xml-文档创建输出工具)
  - [3.1. DocFX](./08_CSharp%20XML%20文档注释.md/#31-docfx)
  - [3.2. Sandcastle](./08_CSharp%20XML%20文档注释.md/#32-sandcastle)
  - [3.3. Doxygen](./08_CSharp%20XML%20文档注释.md/#33-doxygen)

---