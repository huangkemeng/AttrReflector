# AttrReflector
用于获取指定特性信息

## 获取和安装
> Nuget上搜索AttrReflector
![img1](https://github.com/huangkemeng/Images/blob/master/1571313966(1).jpg)

## 使用方法
- 准备阶段
  - 假设类的定义如下：
  
  ```cs
            [My("一个类")]
            public class OneClass
            {

                [My("实例属性")]
                public string Prop1 { get; set; } = string.Empty;

                [My("实例字段")]
                public string field1 = string.Empty;

                [My("实例方法")]
                public void Method1(string param1)
                {

                }

                [My("静态属性")]
                public static string Prop2 { get; set; } = string.Empty;

                [My("静态字段")]
                public static string field2 = string.Empty;

                [My("静态方法")]
                public static void Method2(string param1)
                {

                }
            }
     ```
     
    - 枚举的定义如下：
    
    ```cs
      public enum Week
        {
            [My("周一")]
            One,
            [My("周二")]
            Two
        }
    ```


- 获取实例类上的特性
  - 创建一个对象为  OneClass one = new OneClass();
  
  ```cs
   // 获取类上的特性
   MyAttribute myAttribute = one.GetAttributeProvider(x => x).GetAttributeInfo<MyAttribute>();
   // or
   MyAttribute myAttribute = one.GetAttributeProvider().GetAttributeInfo<MyAttribute>();

   //获取属性上的特性
   MyAttribute myAttribute = one.GetAttributeProvider(x => x.Prop1).GetAttributeInfo<MyAttribute>();

   //获取字段上的特性
   MyAttribute myAttribute = one.GetAttributeProvider(x => x.field1).GetAttributeInfo<MyAttribute>();

   //获取方法上的特性
   MyAttribute myAttribute = one.GetAttributeProvider(x => nameof(x.Method1)).GetAttributeInfo<MyAttribute>();

  //获取枚举上的特性
   MyAttribute myAttribute = Week.One.GetAttributeProvider(x => x).GetAttributeInfo<MyAttribute>();
 
 
  ```
- 获取静态类上的特性

  ```cs
  
  //获取类上的特性
  MyAttribute myAttribute = KMReflector.GetAttributeProvider<OneClass>().GetAttributeInfo<MyAttribute>();
  
  //获取静态属性上的特性
  MyAttribute myAttribute = KMReflector.GetAttributeProvider<OneClass>(() => OneClass.Prop2).GetAttributeInfo<MyAttribute>();
  
  //获取静态字段上的特性
  MyAttribute myAttribute = KMReflector.GetAttributeProvider<OneClass>(() => OneClass.field2).GetAttributeInfo<MyAttribute>();
  
  //获取静态方法上的特性信息
  MyAttribute myAttribute6 = KMReflector.GetAttributeProvider<OneClass>(() => nameof(OneClass.Method2)).GetAttributeInfo<MyAttribute>();
  
   //当然你也直接传入一个类型，获取该类型对象上的特性，但不建议那么做,可读性不高
   MyAttribute myAttribute8 = KMReflector.GetAttributeProvider<OneClass>(() => typeof(OneClass)).GetAttributeInfo<MyAttribute>();
   
  ```
  
