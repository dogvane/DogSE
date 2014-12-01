using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using DogSE.Library.Log;
using System.Linq;

namespace DogSE.Library.Thread
{
    /// <summary>
    /// 线程队列实例
    /// </summary>
    /// <remarks>
    /// 这里是已实例情况存在
    /// 目的时，如果有新的扩展时，可以在静态类里，自己创建一个静态对象。
    /// 方便后面的代码使用
    /// </remarks>
    public class ThreadQueueEntity
    {

        private static List<ThreadQueueEntity> s_queueList = new List<ThreadQueueEntity>();

        /// <summary>
        /// 参看所有线程队列里是否有任务
        /// </summary>
        public static bool HasActionInAllQueue
        {
            get
            {
                bool result = true;
                for (var i = 0; i < s_queueList.Count; i++) 
                {
                    if (!s_queueList[i].HasQueues) 
                    {
                        result = false;
                        break;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// 创建队列需要输入名字
        /// </summary>
        /// <param name="queueName"></param>
        internal ThreadQueueEntity(string queueName)
        {
            _queueName = queueName;
            s_queueList.Add(this);
        }

        private readonly string _queueName = string.Empty;

        /// <summary>
        /// 线程队列名称
        /// </summary>
        public string QueueName { get { return _queueName;} }

        readonly Queue<Action> Methods = new Queue<Action>();
        volatile bool _isLockQueue;

        /// <summary>
        /// 增加一个异步任务
        /// </summary>
        /// <param name="method"></param>
        public void Append(Action method)
        {
            if (method == null)
                return;

            bool isLock = false;
            Action[] writeArray = null;
            lock (Methods)
            {
                Methods.Enqueue(method);

                // 检测是否有其它的线程已在处理中，如在使用就退出,否则开始锁定
                if (_isLockQueue == false)
                {
                    isLock = _isLockQueue = true;
                    writeArray = Methods.ToArray();
                    Methods.Clear();
                }
            }

            // 如果有其它的线程在处理就退出
            if (isLock == false)
                return;

            ThreadPool.QueueUserWorkItem(o =>
            {
                while (writeArray.Length > 0)
                {
                    Stopwatch watch = Stopwatch.StartNew();
                    foreach (var m in writeArray)
                    {
                        try
                        {
                            watch.Reset();
                            watch.Start();

                            m();

                            watch.Stop();

                            //  TODO 这里要考虑如何记录时间
                        }
                        catch (Exception ex)
                        {
                            Logs.Error("Run thread queue({0}) method error.", _queueName, ex);
                        }

                    }

                    watch.Stop();

                    lock (Methods)
                    {
                        //  这里在执行完一次操作后，需要返回重新读一次队列，如果队列里有数据，则还需要进行后面的处理
                        writeArray = Methods.ToArray();
                        Methods.Clear();
                    }
                }

                _isLockQueue = false;
            });
        }



        /// <summary>
        /// 当前队列里是否还有正在执行的任务
        /// </summary>
        public bool HasQueues
        {
            get
            {
                lock (Methods)
                {
                    if (Methods.Count > 0)
                        return true;
                }

                return _isLockQueue;
            }
        }
    }
}
