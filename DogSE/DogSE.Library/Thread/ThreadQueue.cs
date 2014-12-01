using System;
using System.Collections.Generic;
using System.Linq;
using DogSE.Library.Log;

namespace DogSE.Library.Thread
{
    /// <summary>
    /// 线程队列，用于按照顺序执行方法
    /// 只用一个线程来执行队列，多线程时自己使用系统提供的线程池
    /// </summary>
    public static class ThreadQueue
    {
        static private readonly ThreadQueueEntity normalThreadQueue = new ThreadQueueEntity("normalThreadQueue");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        public static void Append(Action method)
        {
            normalThreadQueue.Append(method);
        }

        static private readonly ThreadQueueEntity ioThreadQueue = new ThreadQueueEntity("ioThreadQueue");

        /// <summary>
        /// 加入一个IO操作队列
        /// </summary>
        /// <param name="method"></param>
        public static void AppendIO(Action method)
        {
            ioThreadQueue.Append(method);
        }

        /// <summary>
        /// 添加一个io缓存方法
        /// 在没有调用 flushIoCache 方法前会先缓存在内部
        /// 等合适的时间再flush到队列里
        /// </summary>
        /// <param name="hasdCode"></param>
        /// <param name="method"></param>
        /// <returns>
        /// true 表示缓存里没有类似的实体数据，缓存添加成功
        /// false 表示缓存里有相同的实体，不再进行添加
        /// </returns>
        public static bool AppendIOCache(int hasdCode, Action method)
        {
            lock (ioCache)
            {
                if (!ioCache.ContainsKey(hasdCode))
                {
                    ioCache.Add(hasdCode, method);
                    return true;
                }
                return false;
            }
        }

        private static readonly Dictionary<int, Action> ioCache = new Dictionary<int,Action>();
        
        /// <summary>
        /// 
        /// </summary>
        public static void FlushIOCache()
        {
            lock (ioCache)
            {
                var mehtods = ioCache.Values.ToArray();
                ioCache.Clear();
                if (mehtods.Length > 0)
                {
                    Logs.Debug("flush db entity {0}", mehtods.Length);
                    foreach (var m in mehtods)
                        ioThreadQueue.Append(m);
                }
            }
        }

        /// <summary>
        /// 当前队列里是否还有正在执行的任务
        /// </summary>
        public static bool HasQueues
        {
            get { return normalThreadQueue.HasQueues | ioThreadQueue.HasQueues; }
        }

        private static readonly List<ThreadQueueEntity> s_threadQueueList = new List<ThreadQueueEntity>();

        /// <summary>
        /// 获得一个指定名称的线程队列
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ThreadQueueEntity GetThreadQueue(string name)
        {
            var ret = s_threadQueueList.FirstOrDefault(o => o.QueueName == name);

            if (ret == null)
            {
                ret = new ThreadQueueEntity(name);
                s_threadQueueList.Add(ret);
            }

            return ret;
        }
    }
}
