using System;
//using System.Collections.Concurrent;
using DogSE.Library.Common;
using DogSE.Library.Time;
using TradeAge.Client.Core.Net;

namespace DogSE.Client.Core.Task
{
    /// <summary>
    /// 非网络消息包的任务
    /// </summary>
    class ActionTask:ITask
    {

        /// <summary>
        /// 任务函数
        /// </summary>
        public Action Action { get;internal set; }


        private string _actionName;

        /// <summary>
        /// 任务名称
        /// </summary>
        public string ActionName
        {
            get
            {
                if (_actionName == null)
                    _actionName = Action.Method.Name;

                return _actionName;
            }
            set
            {
                _actionName = value;
            }
        }

        #region ITask 成员

        /// <summary>
        /// 执行任务
        /// </summary>
        public void Execute()
        {
            Action();
        }

        public ITaskProfile TaskProfile { get; internal set; }

        #endregion

        /// <summary>
        /// 任务的对象池
        /// </summary>
        static readonly ObjectPool<ActionTask> TaskPool = new ObjectPool<ActionTask>(128);

        private bool isRelease;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Release()
        {
            if (!isRelease)
            {
                TaskPool.ReleaseContent(this);
                isRelease = true;
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="runTick"></param>
        /// <param name="isError"></param>
        public void WriteLog(long runTick, bool isError)
        {
            var now = OneServer.NowTime;
            //ActionTaskCodeRuntimeWriter.Write(ActionName, runTick, now.Ticks - CreateTime.Ticks, isError);
        }

        /// <summary>
        /// 从缓冲池里获得一个对象
        /// </summary>
        /// <returns></returns>
        public static ActionTask AcquireContent(string actionName)
        {
            var ret = TaskPool.AcquireContent();
            ret.isRelease = false;
            ret.TaskProfile = ActionTaskProfile.GetNetTaskProfile(actionName);
            ret.CreateTime = OneServer.NowTime;
            return ret;
        }
    }

    /// <summary>
    /// 带参数的任务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class ParamActionTask<T> : ITask
    {

        public Action<T> Action { get; internal set; }

        private string _actionName;

        /// <summary>
        /// 任务名称
        /// </summary>
        public string ActionName {
            get
            {
                if (_actionName == null)
                    _actionName = Action.Method.Name;

                return _actionName;
            }
            set
            {
                _actionName = value;
            }
        }

        /// <summary>
        /// 关联对象
        /// </summary>
        public T Obj { get; internal set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime{get;set;}

        #region ITask 成员

        /// <summary>
        /// 执行任务
        /// </summary>
        public void Execute()
        {
            Action(Obj);
        }

        public ITaskProfile TaskProfile { get; internal set; }

        #endregion

        /// <summary>
        /// 任务的对象池
        /// </summary>
        static readonly ObjectPool<ParamActionTask<T>> TaskPool = new ObjectPool<ParamActionTask<T>>(256);

        private bool isRelease;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Release()
        {
            if (!isRelease)
            {
                TaskPool.ReleaseContent(this);
                isRelease = true;
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="runTick"></param>
        /// <param name="isError"></param>
        public void WriteLog(long runTick, bool isError)
        {
            var now = OneServer.NowTime;
            //ActionTaskCodeRuntimeWriter.Write(ActionName, runTick, now.Ticks - CreateTime.Ticks, isError);
        }

        /// <summary>
        /// 从缓冲池里获得一个对象
        /// </summary>
        /// <returns></returns>
        public static ParamActionTask<T> AcquireContent(string actionName)
        {
            var ret = TaskPool.AcquireContent();
            ret.isRelease = false;
            ret.TaskProfile = ActionTaskProfile.GetNetTaskProfile(actionName);
            ret.CreateTime = OneServer.NowTime;
            return ret;
        }
    }

    /// <summary>
    /// 非网络消息任务的消息执行
    /// </summary>
    public class ActionTaskProfile : ITaskProfile
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionName"></param>
        public ActionTaskProfile(string actionName)
        {
            ActionName = actionName;
        }

        /// <summary>
        /// 执行的方法名
        /// </summary>
        public string ActionName { get; private set; }


        /// <summary>
        /// 调用次数
        /// </summary>
        public long Count { get; private set; }

        /// <summary>
        /// 总调用时间
        /// </summary>
        public long ElapsedTicks { get; private set; }

        /// <summary>
        /// 最大执行时间
        /// </summary>
        public long MaxElapsedTicks { get; private set; }

        /// <summary>
        /// 调用次数
        /// </summary>
        public long Error { get; private set; }

        /// <summary>
        /// 平均的处理时间
        /// </summary>
        public TimeSpan AverageProcTime
        {
            get
            {
                if (Count == 0)
                    return TimeSpan.Zero;

                return TimeSpan.FromTicks(ElapsedTicks / Count);
            }
        }

        /// <summary>
        /// 添加一次调用记录
        /// </summary>
        /// <param name="ticks"></param>
        /// <param name="isError"></param>
        public void Append(long ticks, bool isError = false)
        {
            Count++;
            ElapsedTicks += ticks;
            if (isError)
                Error++;
            else if (MaxElapsedTicks < ticks)
                    MaxElapsedTicks = ticks;
        }

        static readonly ConcurrentDictionary<string, ActionTaskProfile> Map = new ConcurrentDictionary<string, ActionTaskProfile>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public static ActionTaskProfile GetNetTaskProfile(string actionName)
        {
            ActionTaskProfile ret;
            if (Map.TryGetValue(actionName, out ret))
            {
                return ret;
            }

            //  新建一个包处理，然后
            ret = new ActionTaskProfile(actionName);
            Map.TryAdd(actionName, ret);

            return ret;
        }
    }
}
