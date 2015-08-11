using System;
using System.Collections.Generic;
using System.Linq;
using DogSE.Common;

namespace DogSE.Server.Core.Entity
{
    /// <summary>
    /// 内存里的热点数据管理
    /// </summary>
    public class HotDataManager<T> where T:IDataEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        public HotDataManager(int capacity = 1024)
        {
            entityMap = new Dictionary<int, Entity>(capacity);
        }

        /// <summary>
        /// 当前缓存的数据
        /// </summary>
        public int Count
        {
            get { return entityMap.Count; }
        }

        private readonly Dictionary<int, Entity> entityMap;

        /// <summary>
        /// 获得一个实例数据
        /// 如果不存在，返回空
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetEntity(int key)
        {
            lock (opLock)
            {
                Entity ret;
                if (entityMap.TryGetValue(key, out ret))
                {
                    ret.IsLock = true;
                    ret.LastUseTime = DateTime.Now;
                    return ret.Data;
                }

                return default(T);
            }
        }

        /// <summary>
        /// 获得所有的实例数据
        /// </summary>
        /// <returns></returns>
        public T[] GetEntitys()
        {
            List<T> list = new List<T>();
            foreach(var entity in entityMap.Values)
            {
                list.Add(entity.Data);
            }
            return list.ToArray();
        }

        /// <summary>
        /// 输出当前的状态
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            lock (opLock)
            {
                var count = entityMap.Count;
                var lockCount = entityMap.Count(o => o.Value.IsLock);
                return string.Format("{0} {1}(lock)/{2}(count)", typeof (T).Name, lockCount, count);
            }
        }

        /// <summary>
        /// 添加或者替换某个实例数据
        /// </summary>
        /// <param name="entity"></param>
        public void AddOrReplace(T entity)
        {
            lock (opLock)
            {
                Entity ret;
                if (entityMap.TryGetValue(entity.Id, out ret))
                {
                    ret.IsLock = true;
                    ret.Data = entity;
                }
                else
                {
                    ret = new Entity
                    {
                        IsLock = true,
                        LastUseTime = DateTime.Now,
                        Data = entity,
                    };
                    entityMap[entity.Id] = ret;
                }
            }
        }

        /// <summary>
        /// 解除某个数据的锁定
        /// 加锁是在AddOrReplace里进行的
        /// </summary>
        /// <param name="entity"></param>
        public void Unlock(T entity)
        {
            lock (opLock)
            {
                Entity ret;
                if (entityMap.TryGetValue(entity.Id, out ret))
                {
                    ret.IsLock = false;
                    ret.LastUseTime = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// 操作锁
        /// </summary>
        private readonly object opLock = new object();

        /// <summary>
        /// 移除某个数据
        /// </summary>
        /// <param name="keys"></param>
        public void Remove(params int[]keys)
        {
            lock (opLock)
            {
                foreach (var remove in keys)
                    entityMap.Remove(remove);
            }
        }

        /// <summary>
        /// 清理缓存数据
        /// 清理 time 之前使用的数据
        /// 默认清除一天的之前的数据
        /// </summary>
        /// <param name="time">
        /// 单位秒
        /// </param>
        /// <param name="checkIsUnLock">检查某个被锁住的对象，是否还处于解锁状态</param>
        /// <returns>
        /// 返回被移除的id列表
        /// </returns>
        public int[] Clear(int time = 24 * 60 * 60, Func<T, DateTime, bool> checkIsUnLock = null)
        {
            var checkTime = DateTime.Now.AddSeconds(-time);
            List<int> retIds = new List<int>();
            lock (opLock)
            {
                foreach (var data in entityMap.ToArray())
                {
                    if (data.Value.IsLock)
                    {
                        if (checkIsUnLock != null)
                        {
                            var ret = checkIsUnLock(data.Value.Data, data.Value.LastUseTime);
                            if (ret)
                            {
                                entityMap.Remove(data.Key);
                                retIds.Add(data.Key);
                            }
                        }
                    }
                    else if (data.Value.LastUseTime < checkTime)
                    {
                        entityMap.Remove(data.Key);
                        retIds.Add(data.Key);
                    }
                }
            }

            return retIds.ToArray();
        }

        class Entity
        {
            /// <summary>
            /// 最后一次使用时间
            /// </summary>
            public DateTime LastUseTime;

            /// <summary>
            /// 是否锁定
            /// </summary>
            public bool IsLock;

            /// <summary>
            /// 数据
            /// </summary>
            public T Data;
        }
    }
}
