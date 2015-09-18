using System;
using System.Collections.Concurrent;
using DogSE.Library.Common;
using DogSE.Library.Time;
using DogSE.Server.Core.Task;

namespace DogSE.Server.Core.TaskT
{
    /// <summary>
    /// 非网络消息包的任务
    /// </summary>
    class ActionTaskT<T>:ITaskT<T> where T : INetTaskEntity, new()
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
        static readonly ObjectPool<ActionTaskT<T>> TaskPool = new ObjectPool<ActionTaskT<T>>(128);

        private bool isRelease;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Release()
        {
            if (!isRelease)
            {
                Action = null;
                TaskPool.ReleaseContent(this);
                isRelease = true;
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }


        private TaskManagerT<T> parent;

        /// <summary>
        /// 任务管理器
        /// </summary>
        public TaskManagerT<T> Parent
        {
            set { parent = value; }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="runTicks"></param>
        /// <param name="isError"></param>
        public void WriteLog(long runTicks, bool isError)
        {
            var now = OneServer.NowTime;
            long delayTicks = now.Ticks - CreateTime.Ticks;
            parent.ActionTaskLogWriter.Write(ActionName, runTicks, delayTicks, isError);

            parent.Monitor.ActionTaskCount++;
            parent.Monitor.ActionTaskRunTicks = parent.Monitor.ActionTaskRunTicks + runTicks;
            parent.Monitor.ActionTaskDelayTicks = parent.Monitor.ActionTaskDelayTicks + delayTicks;
            if (isError)
                parent.Monitor.ActionTaskErrorCount++;
        }

        /// <summary>
        /// 从缓冲池里获得一个对象
        /// </summary>
        /// <returns></returns>
        public static ActionTaskT<T> AcquireContent(string actionName)
        {
            var ret = TaskPool.AcquireContent();
            ret.isRelease = false;
            ret.TaskProfile = ActionTaskProfile.GetNetTaskProfile(actionName);
            ret.CreateTime = OneServer.NowTime;
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("task name:{0}", ActionName);
        }
    }

    /// <summary>
    /// 带参数的任务
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T"></typeparam>
    class ParamActionTaskT<T1, T> : ITaskT<T> where T : INetTaskEntity, new()
    {

        public Action<T1> Action { get; internal set; }

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
        public T1 Obj { get; internal set; }

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
        static readonly ObjectPool<ParamActionTaskT<T1,T>> TaskPool = new ObjectPool<ParamActionTaskT<T1,T>>(256);

        private bool isRelease;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Release()
        {
            if (!isRelease)
            {
                Obj = default(T1);
                Action = null;

                TaskPool.ReleaseContent(this);
                isRelease = true;
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="runTicks"></param>
        /// <param name="isError"></param>
        public void WriteLog(long runTicks, bool isError)
        {
            var now = OneServer.NowTime;
            long delayTicks = now.Ticks - CreateTime.Ticks;
            parent.ActionTaskLogWriter.Write(ActionName, runTicks, delayTicks, isError);

            parent.Monitor.ActionTaskCount++;
            parent.Monitor.ActionTaskRunTicks = parent.Monitor.ActionTaskRunTicks + runTicks;
            parent.Monitor.ActionTaskDelayTicks = parent.Monitor.ActionTaskDelayTicks + delayTicks;
            if (isError)
                parent.Monitor.ActionTaskErrorCount++;
        }

        private TaskManagerT<T> parent;

        /// <summary>
        /// 任务管理器
        /// </summary>
        public TaskManagerT<T> Parent
        {
            set { parent = value; }
        }

        /// <summary>
        /// 从缓冲池里获得一个对象
        /// </summary>
        /// <returns></returns>
        public static ParamActionTaskT<T1,T> AcquireContent(string actionName)
        {
            var ret = TaskPool.AcquireContent();
            ret.isRelease = false;
            ret.TaskProfile = ActionTaskProfile.GetNetTaskProfile(actionName);
            ret.CreateTime = OneServer.NowTime;
            return ret;
        }

        public override string ToString()
        {
            return string.Format("task name:{0} obj:{1}", _actionName, Obj);
        }
    }

}
