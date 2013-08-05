using System;
using System.Collections.Generic;
using System.Reflection;
using DogSE.Library.Log;

namespace DogSE.Server.Core.Util
{
    /// <summary>
    /// 程序集的辅助类
    /// </summary>
    public static class AssemblyUtil
    {
        /// <summary>
        /// 获得运行时所有的Assembly
        /// </summary>
        /// <returns></returns>
        public static List<Assembly> GetAssemblies()
        {
            List<Assembly> assemblies = new List<Assembly>();

            //  先获得当前进程的程序集
            var runAssembly = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in runAssembly)
                if (!asm.GlobalAssemblyCache)
                    assemblies.Add(asm);

            // 反射出当前程序集的入口程序集
            // 找到符合标记的          
            var entryAsm = Assembly.GetEntryAssembly();

            if (entryAsm != null)
            {
                // 再查找引用的程序集
                var refAsms = entryAsm.GetReferencedAssemblies();
                if (assemblies.IndexOf(entryAsm) == 0)
                    assemblies.Add(entryAsm);

                foreach (var name in refAsms)
                {
                    // XG: 为了测试而改的, 以后可以改回去
                    Assembly asm = null;
                    try
                    {
                        asm = AppDomain.CurrentDomain.Load(name); // XG改的代码
                    }
                    catch (Exception ex)
                    {
                        Logs.Error("Load assembly fail. assembly = {0}", name.FullName, ex);
                    }

                    if (asm != null)
                    {
                        if (!asm.GlobalAssemblyCache &&
                            assemblies.IndexOf(asm) == -1)
                                assemblies.Add(asm);
                    }
                }
            }
            return assemblies;
        }

        /// <summary>
        /// 获得运行时的所有类型
        /// </summary>
        /// <returns></returns>
        public static Type[] GetTypes()
        {
            var ret = new List<Type>();
            foreach (var ass in GetAssemblies())
            {
                ret.AddRange(ass.GetTypes());
            }

            return ret.ToArray();
        }

        /// <summary>
        /// 获得打过指定对象标签的类型
        /// </summary>
        /// <returns></returns>
        public static Type[] GetTypesByAttribute(Type attributeType)
        {
            var ret = new List<Type>();
            foreach (var type in GetTypes())
            {
                var customAttribute = type.GetCustomAttributes(attributeType, true);
                if (customAttribute.Length > 0)
                {
                    ret.Add(type);
                }
            }

            return ret.ToArray();
        }


        /// <summary>
        /// 获得思想指定接口的类型
        /// </summary>
        /// <returns></returns>
        public static Type[] GetTypesByInterface(Type interfaceType)
        {
            var name = interfaceType.Name;
            var ret = new List<Type>();
            foreach (var type in GetTypes())
            {
                var customAttribute = type.GetInterface(name, true);
                if (customAttribute != null)
                    ret.Add(type);
            }

            return ret.ToArray();
        }

        /// <summary>
        /// 获得一个类型下的是否包含有特殊的属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns>如果不存在返回null</returns>
        public static T GetAttribute<T>(this Type type) where T : Attribute
        {

            var ret = type.GetCustomAttributes(typeof (T), true);
            if (ret.Length > 0)
                return ret[0] as T;
            return default(T);
        }
        
        /// <summary>
        /// 获得一个类型下的是否包含有特殊的属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns>如果不存在返回null</returns>
        public static T GetAttribute<T>(this PropertyInfo type) where T : Attribute
        {

            var ret = type.GetCustomAttributes(typeof (T), true);
            if (ret.Length > 0)
                return ret[0] as T;
            return default(T);
        }
    }
}
