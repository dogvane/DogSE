using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using DogSE.Library.Log;
using DogSE.Server.Core.Net;

namespace DogSE.Server.Core.Task
{
    /// <summary>
    /// 任务管理
    /// </summary>
    /// <remarks>
    /// 整个游戏的业务逻辑在这里开辟的线程里执行
    /// </remarks>
    public class TaskManager
    {
        private string taskName_;

        /// <summary>
        /// 任务管理器
        /// </summary>
        /// <param name="taskName"></param>
        public TaskManager(string taskName)
        {
            taskName_ = taskName;
        }


        /// <summary>
        /// 添加一个任务
        /// </summary>
        /// <param name="task"></param>
        public void AppendTask(ITask task)
        {
            taskList.Enqueue(task);
        }

        /// <summary>
        /// 增加一个独立任务
        /// </summary>
        /// <param name="action"></param>
        public void AppendTask(Action action)
        {
            var task = ActionTask.AcquireContent(action.Method.Name);
            task.Action = action;

            AppendTask(task);
        }

        /// <summary>
        /// 添加一个网络消息任务
        /// </summary>
        /// <param name="netState"></param>
        /// <param name="handler"></param>
        /// <param name="packetreader"></param>
        public void AppendTask(NetState netState, PacketHandler handler, PacketReader packetreader)
        {
            var task = NetTask.AcquireContent(handler.PacketID);
            task.PacketReader = packetreader;
            task.PacketHandler = handler;
            task.NetState = netState;

            AppendTask(task);
        }

        /// <summary>
        /// 任务队列
        /// </summary>
        /// <remarks>
        /// 这里可以进行优化，用多队列的方式进行优先级划分
        /// </remarks>
        private readonly ConcurrentQueue<ITask> taskList = new ConcurrentQueue<ITask>();

        /// <summary>
        /// 开启任务线程
        /// </summary>
        public void StartThread()
        {
            if (workThread != null)
            {
                Logs.Warn("work thread {0} is start.", taskName_);
                return;
            }

            var thread = new Thread(WorkThread);
            thread.Priority = ThreadPriority.AboveNormal;
            thread.Name = "work thread " + taskName_;
            thread.Start();
            workThread = thread;
        }

        private bool isRuning;

        /// <summary>
        /// 当前的任务线程状态
        /// </summary>
        public bool Runing
        {
            get { return isRuning; }
            set { isRuning = value; }
        }

        void WorkThread()
        {
            isRuning = true;
            Logs.Info("Logic thread {0} start.", taskName_);

            var watch = Stopwatch.StartNew();
            while(isRuning || taskList.Count > 0)
            {
                ITask task;
                if(taskList.TryDequeue(out task))
                {
                    watch.Restart();
                    bool isError = false;
                    try
                    {
                        task.Execute();
                    }
                    catch (Exception ex)
                    {
                        Logs.Error("{0} run task fail.", taskName_, ex);
                        isError = true;
                    }
                    watch.Stop();

                    task.TaskProfile.Append(watch.ElapsedTicks, isError);
                    task.Release();
                }
                else
                {
                    //  队列里没任务，则让线程先休息一小会
                    Thread.Sleep(0);
                }
            }

        }

        private Thread workThread;

        /// <summary>
        /// 重启线程
        /// 在实际运营中，会碰上任务线程进入死循环，或者在等待某些操作的情况
        /// 这时游戏可能进入假死状态，只有中断当前的线程，并重启业务逻辑线程
        /// 才能让游戏继续下去
        /// </summary>
        public void RestartThread()
        {
            if (workThread != null)
            {
                try
                {
                    workThread.Abort();
                }
                catch (Exception ex)
                {
                    Logs.Error("Abort logic thread {0} fail.", taskName_, ex);
                }
                workThread = null;
            }

            StartThread();
        }
    }
}
