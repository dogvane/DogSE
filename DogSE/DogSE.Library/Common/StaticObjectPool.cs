using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DogSE.Library.Common
{
    /// <summary>
    /// 泛型的，静态唯一的对象池【线程安全，放心使用】
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class StaticObjectPool<T> where T : new()
    {
        private static readonly ObjectPool<T> pool = new ObjectPool<T>();

        /// <summary>
        /// 内存池请求数据
        /// </summary>
        /// <returns></returns>
        public static T AcquireContent()
        {
            return pool.AcquireContent();
        }

        /// <summary>
        /// 回收内存
        /// </summary>
        /// <param name="contentT"></param>
        public static void ReleaseContent(T contentT)
        {
            pool.ReleaseContent(contentT);
        }
    }
}
