using System.Collections.Generic;
using System.Linq;

namespace DogSE.Library.Common
{

    /// <summary>
    /// 对象池信息
    /// </summary>
    public struct PoolInfo
    {
        #region zh-CHS 共有属性 | en Public Properties

        /// <summary>
        /// 空闲数量
        /// </summary>
        public long FreeCount { get; internal set; }

        /// <summary>
        /// 初始化池数量
        /// </summary>
        public long InitialCapacity { get; internal set; }

        /// <summary>
        /// 当前池数量
        /// </summary>
        public long CurrentCapacity { get; internal set; }

        /// <summary>
        /// 请求失败次数
        /// </summary>
        public long Misses { get; internal set; }

#if DEBUG
        /// <summary>
        /// 输出当前数据
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {

            return string.Format("FreeCount={0} CurrentCapacity={1} Misses={2}", FreeCount, CurrentCapacity, Misses);
        }
#endif
        #endregion
    }

    /// <summary>
    /// 对象池信息
    /// </summary>
    public interface IPoolInfo
    {
        /// <summary>
        /// 对象池的名字
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 获得对象池信息
        /// </summary>
        /// <returns></returns>
        PoolInfo GetPoolInfo();
    }

    /// <summary>
    /// 对象池信息
    /// </summary>
    /// <remarks>
    /// </remarks>
    public static class ObjectPoolStateInfo
    {
        private static List<IPoolInfo> pools = new List<IPoolInfo>(32);

        internal static void Add(IPoolInfo pool)
        {
            pools.Add(pool);
        }

        /// <summary>
        /// 获得所有内存池的信息
        /// </summary>
        /// <returns></returns>
        public static PoolInfo[] GetPoolInfos()
        {
            return pools.Select(o => o.GetPoolInfo()).ToArray();
        }

        /// <summary>
        /// 获得指定名称的内存池信息
        /// </summary>
        /// <returns></returns>
        public static PoolInfo[] GetPoolInfos(string name)
        {
            return pools.Where(o => o.Name == name)
                .Select(o => o.GetPoolInfo()).ToArray();
        }
    }
}
