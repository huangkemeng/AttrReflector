using System;
using System.Linq.Expressions;
using System.Reflection;

namespace KeMengSoft.AttrReflector
{
    public static class KMReflector
    {
        private static ICustomAttributeProvider SwitchMemberType(this Type type, MemberExpression expression)
        {
            ICustomAttributeProvider customAttributeProvider = null;
            string memberName = expression.Member.Name;
            switch (expression.Member.MemberType)
            {
                case MemberTypes.Event:
                    customAttributeProvider = type.GetEvent(memberName);
                    break;
                case MemberTypes.Field:
                    customAttributeProvider = type.GetField(memberName);
                    break;
                case MemberTypes.Method:
                    customAttributeProvider = type.GetMethod(memberName);
                    break;
                case MemberTypes.NestedType:
                    customAttributeProvider = type.GetNestedType(memberName);
                    break;
                case MemberTypes.Property:
                    customAttributeProvider = type.GetProperty(memberName);
                    break;
                case MemberTypes.TypeInfo:
                    customAttributeProvider = type.GetTypeInfo();
                    break;
            }
            return customAttributeProvider;
        }

        /// <summary>
        /// 获取实例类、字段、方法、枚举上的特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static ICustomAttributeProvider GetAttributeProvider<T>(this T t, Expression<Func<T, object>> expression = null)
        {
            Type type = typeof(T);
            ICustomAttributeProvider customAttributeProvider = type;
            if (expression != null)
            {
                if (type.IsClass)
                {
                    if (expression.Body is MemberExpression memberExpression)
                    {
                        customAttributeProvider = type.SwitchMemberType(memberExpression);
                    }
                    else if (expression.Body is UnaryExpression unaryExpression)
                    {
                        if (unaryExpression.Operand is MemberExpression mExpression)
                        {
                            customAttributeProvider = type.SwitchMemberType(mExpression);
                        }
                    }
                    else if (expression.Body is ConstantExpression constantExpression)
                    {
                        customAttributeProvider = type.GetMethod(constantExpression.Value.ToString());
                    }
                    else if (expression.Body is ParameterExpression parameterExpression)
                    {
                        customAttributeProvider = parameterExpression.Type;
                    }
                    else
                    {
                        throw new System.Exception("该类型不受支持");
                    }
                }
                else if (type.IsEnum)
                {
                    if (expression.Body is UnaryExpression unaryExpression)
                    {
                        customAttributeProvider = type.GetField(System.Enum.GetName(type, t));
                    }
                    else
                    {
                        throw new System.Exception("该类型不受支持");
                    }
                }
                else
                {
                    throw new System.Exception("该类型不受支持,仅支持实例类和枚举");
                }
            }
            return customAttributeProvider;
        }

        /// <summary>
        /// 获取泛型指定的特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="attributeProvider"></param>
        /// <returns></returns>
        public static T GetAttributeInfo<T>(this ICustomAttributeProvider attributeProvider) where T : class
        {
            if (attributeProvider == null)
            {
                throw new ArgumentNullException(nameof(attributeProvider));
            }
            var objs = attributeProvider.GetCustomAttributes(typeof(T), true);
            T attr;
            foreach (var obj in objs)
            {
                attr = obj as T;
                if (attr != null)
                {
                    return attr;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取静态类，静态字段、静态方法上的特性
        /// </summary>
        /// <typeparam name="T">所在类</typeparam>
        /// <param name="expression">表达式</param>
        /// <returns></returns>
        public static ICustomAttributeProvider GetAttributeProvider<T>(Expression<Func<object>> expression = null)
        {
            var classType = typeof(T);
            ICustomAttributeProvider customAttributeProvider = classType;
            if (expression != null)
            {

                if (expression.Body is MemberExpression memberExpression)
                {
                    customAttributeProvider = classType.SwitchMemberType(memberExpression);
                }
                else if (expression.Body is UnaryExpression unaryExpression)
                {
                    if (unaryExpression.Operand is MemberExpression mExpression)
                    {
                        customAttributeProvider = classType.SwitchMemberType(mExpression);
                    }
                }
                else if (expression.Body is ConstantExpression constantExpression)
                {
                    if (constantExpression.Type == typeof(string))
                    {
                        customAttributeProvider = classType.GetMethod(constantExpression.Value.ToString());
                    }
                    else if (constantExpression.Type == typeof(Type))
                    {
                        customAttributeProvider = constantExpression.Value as Type;
                    }
                }
                else if (expression.Body is ParameterExpression parameterExpression)
                {
                    customAttributeProvider = parameterExpression.Type;
                }
                else
                {
                    throw new System.Exception("该类型不受支持");
                }
            }
            return customAttributeProvider;
        }

    }
}
