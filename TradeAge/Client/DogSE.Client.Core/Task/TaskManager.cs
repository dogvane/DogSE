using System;
//using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using DogSE.Client.Core.Net;
using DogSE.Library.Common;
using DogSE.Library.Log;
using DogSE.Library.Time;
using DogSE.Client.Core.Timer;

namespace DogSE.Client.Core.Task
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
        /// 推荐使用 public void AppendTask(string name, Action action) 方法，方便记录任务的时间
        /// </summary>
        /// <param name="action"></param>
        public void AppendTask(Action action)
        {
            var name = action.Method.Name;
            AppendTask(name, action);
        }

        /// <summary>
        /// 添加一个任务
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public void AppendTask(string name, Action action)
        {
            var task = ActionTask.AcquireContent(name);
            task.Action = action;
            task.ActionName = name;

            AppendTask(task);
        }

        /// <summary>
        /// 添加一个带参数的任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="obj">参数</param>
        public void AppentdTask<T>(Action<T> action, T obj)
        {
            var name = action.Method.Name;

            AppentdTask<T>(name, action,  obj);
        }

        /// <summary>
        /// 添加一个带参数的任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <param name="obj"></param>
        public void AppentdTask<T>(string name, Action<T> action, T obj)
        {
            var task = ParamActionTask<T>.AcquireContent(name);
            task.Action = action;
            task.Obj = obj;
            task.ActionName = name;

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
            set
            {
                isRuning = value;
                if (value == false)
                {
                    //  如果设置为false，则表示进程需要进行退出了
                    //  这里会开始等待逻辑线程退出，等逻辑线程退出后，再结束本次操作
                    int count = 1000;  
                    while (isWorkThreadRun && count-- > 0)
                    {
                        Thread.Sleep(100);
                    }
                    if (isWorkThreadRun)
                    {
                        //  100s 后如果工作线程没有退出，则发送异常，这里不理会工作线程，直接走后面的流程
                        Logs.Error("Logic thread can't stop.");
                    }
                }
            }
        }

        private bool isWorkThreadRun = false;
        void WorkThread()
        {
            isRuning = true;
            isWorkThreadRun = true;
            Logs.Info("Logic thread {0} start.", taskName_);
            
            var start = OneServer.NowTime;

            var watch = Stopwatch.StartNew();
            while(isRuning || taskList.Count > 0)
            {
                ITask task;
                if(taskList.TryDequeue(out task))
                {
                    //watch.Restart();
                    watch.Reset();
                    watch.Start();

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
                    task.WriteLog(watch.ElapsedTicks, isError);
                    task.Release();

                    var now = OneServer.NowTime;
                    if (now.Ticks - start.Ticks > 10000 * 1000 * 10)
                    {
                        start = now;
                        NetTaskCodeRuntimeWriter.Flush();
                        ActionTaskCodeRuntimeWriter.Flush();
                    }
                }
                else
                {
                    //  队列里没任务，则让线程先休息一小会
                    Thread.Sleep(1);
                }
            }

            NetTaskCodeRuntimeWriter.Flush();
            ActionTaskCodeRuntimeWriter.Flush();

            isWorkThreadRun = false;
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
