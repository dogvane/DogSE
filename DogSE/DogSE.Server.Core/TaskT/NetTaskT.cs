using System;
using System.Collections.Concurrent;
using DogSE.Library.Common;
using DogSE.Library.Time;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Task;

namespace DogSE.Server.Core.TaskT
{

    /// <summary>
    /// NetTask绑定的实例必须包含的接口参数
    /// </summary>
    public interface INetTaskEntity
    {
        /// <summary>
        /// 业务id
        /// </summary>
        int BizId { get; }
    }

    /// <summary>
    /// 一个用于执行的任务接口
    /// </summary>
    public interface ITaskT<T> where T : INetTaskEntity, new()
    {
        /// <summary>
        /// 执行一个任务
        /// </summary>
        void Execute();

        /// <summary>
        /// 和任务相关的线程性能对象
        /// </summary>
        ITaskProfile TaskProfile { get; }

        /// <summary>
        /// 释放和任务相关的资源
        /// </summary>
        void Release();

        /// <summary>
        /// 写操作日志
        /// </summary>
        /// <param name="runTick"></param>
        /// <param name="isError"></param>
        void WriteLog(long runTick, bool isError);

        /// <summary>
        /// 父节点
        /// </summary>
        TaskManagerT<T> Parent { set; }
    }

    /// <summary>
    /// 网络消息调用任务
    /// </summary>
    internal class NetTaskT<T> : ITaskT<T> where T :INetTaskEntity,new()
    {
        /// <summary>
        /// 
        /// </summary>
        public PacketHandlerT<T> PacketHandler { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public PacketReader PacketReader { get; internal set; }

        /// <summary>
        /// 执行的客户端
        /// </summary>
        public T NetState { get; internal set; }

        #region ITask 成员

        /// <summary>
        /// 执行网络消息包指令
        /// </summary>
        public void Execute()
        {
            PacketHandler.OnReceive(NetState, PacketReader);            
        }

        public ITaskProfile TaskProfile { get; internal set; }

        private bool isRelease;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Release()
        {
            if (!isRelease)
            {
                isRelease = true;
                NetState = default(T);
                PacketHandler = null;

                TaskPool.ReleaseContent(this);
                PacketReader.ReleaseContent(PacketReader);
            }
        }

        #endregion

        private TaskManagerT<T> parent;

        /// <summary>
        /// 任务管理器
        /// </summary>
        public TaskManagerT<T> Parent
        {
            set { parent = value; }
        }

        ushort _packetId;


        /// <summary>
        /// 任务的对象池
        /// </summary>
        private static readonly ObjectPool<NetTaskT<T>> TaskPool = new ObjectPool<NetTaskT<T>>(1000);

        /// <summary>
        /// 从缓冲池里获得一个对象
        /// </summary>
        /// <returns></returns>
        public static NetTaskT<T> AcquireContent(ushort packetId)
        {
            var ret = TaskPool.AcquireContent();
            ret.isRelease = false;
            ret.TaskProfile = NetTaskProfile.GetNetTaskProfile(packetId);
            ret.RecvTime = OneServer.NowTime;
            ret._packetId = packetId;
            return ret;
        }

        /// <summary>
        /// 消息包的接收时间
        /// </summary>
        public DateTime RecvTime { get; internal set; }


        #region ITask 成员

        /// <summary>
        /// 写操作日志
        /// </summary>
        /// <param name="runTicks"></param>
        /// <param name="isError"></param>
        public void WriteLog(long runTicks, bool isError)
        {
            var now = OneServer.NowTime;
            long delayTicks = now.Ticks - RecvTime.Ticks;

            parent.NetTaskLogWriter.Write(_packetId, NetState.BizId, runTicks, delayTicks, isError);

            parent.Monitor.NetTaskCount++;
            parent.Monitor.NetTaskRunTicks = parent.Monitor.NetTaskRunTicks + runTicks;
            parent.Monitor.NetTaskDelayTicks = parent.Monitor.NetTaskDelayTicks + delayTicks;
            if (isError)
                parent.Monitor.NetTaskErrorCount++;
        }

        #endregion

        /// <summary>
        /// 输出一些网络状态数据
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("package id:{0} bizId:{1}", _packetId, NetState.BizId);
        }
    }

}
