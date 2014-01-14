using System;
using System.Collections.Generic;
using DogSE.Library.Thread;
using System.Linq;

namespace Org.JQueen.Sanguo.GameWorld.Server.Common
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
        /// 当前队列里是否还有正在执行的任务
        /// </summary>
        public static bool HasQueues
        {
            get { return normalThreadQueue.HasQueues | ioThreadQueue.HasQueues; }
        }

        private static List<ThreadQueueEntity> s_threadQueueList = new List<ThreadQueueEntity>();

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
