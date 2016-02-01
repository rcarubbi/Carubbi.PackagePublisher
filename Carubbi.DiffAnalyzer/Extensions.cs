using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Carubbi.DiffAnalyzer
{
    public static class Extensions
    {
        public static object CreateGenericTypeByReflection(Type genericType, Type type, Object[] constructorParams)
        {
            Type[] typeArgs = { type };
            Type typedType = genericType.MakeGenericType(typeArgs);
            object typedtypeInstance = Activator.CreateInstance(typedType, constructorParams);
            return typedtypeInstance;
        }

        public static IEnumerable<KeyValuePair<Type, MethodInfo>> GetExtensionMethodsDefinedInType(this Type t)
        {
            if (!t.IsSealed || t.IsGenericType || t.IsNested)
                return Enumerable.Empty<KeyValuePair<Type, MethodInfo>>();

            var methods = t.GetMethods(BindingFlags.Public | BindingFlags.Static)
                           .Where(m => m.IsDefined(typeof(ExtensionAttribute), false));

            List<KeyValuePair<Type, MethodInfo>> pairs = new List<KeyValuePair<Type, MethodInfo>>();
            foreach (var m in methods)
            {
                var parameters = m.GetParameters();
                if (parameters.Length > 0)
                {
                    if (parameters[0].ParameterType.IsGenericParameter)
                    {
                        if (m.ContainsGenericParameters)
                        {
                            var genericParameters = m.GetGenericArguments();
                            Type genericParam = genericParameters[parameters[0].ParameterType.GenericParameterPosition];
                            foreach (var constraint in genericParam.GetGenericParameterConstraints())
                                pairs.Add(new KeyValuePair<Type, MethodInfo>(parameters[0].ParameterType, m));
                        }
                    }
                    else
                        pairs.Add(new KeyValuePair<Type, MethodInfo>(parameters[0].ParameterType, m));
                }
            }

            return pairs;
        }

        public static bool IsGenericTypes(this Type source)
        {
            if (source.IsGenericType)
                return true;

            Type baseType = source.BaseType;
            while (baseType != null)
            {
                if (baseType.IsGenericType)
                    return true;

                baseType = baseType.BaseType;
            }

            return false;
        }

        public static bool IsTypeByName(this Type source, string targetTypeName)
        {
            if (source.Name == targetTypeName)
                return true;

            Type baseType = source.BaseType;
            while (baseType != null)
            {
                if (baseType.Name == targetTypeName)
                    return true;

                baseType = baseType.BaseType;
            }

            return false;
        }

        public static string PadLeft(this string source, int totalWidth, string padPattern)
        {
            StringBuilder stb = new StringBuilder();
            for (int i = 0; i < totalWidth; i++)
                stb.Append(padPattern);

            stb.Append(source);
            return stb.ToString();
        }

        public static bool Has<T>(this System.Enum type, T value)
        {
            try
            {
                return (((int)(object)type & (int)(object)value) == (int)(object)value);
            }
            catch
            {
                return false;
            }
        }

        public static bool Is<T>(this System.Enum type, T value)
        {
            try
            {
                return (int)(object)type == (int)(object)value;
            }
            catch
            {
                return false;
            }
        }

        public static T Add<T>(this System.Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type | (int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("Could not append value from enumerated type '{0}'.", typeof(T).Name), ex);
            }
        }

        public static T Remove<T>(this System.Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type & ~(int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("Could not remove value from enumerated type '{0}'.", typeof(T).Name), ex);
            }
        }
    }
}
