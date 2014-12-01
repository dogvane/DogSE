using System;
using System.Collections.Concurrent;
using DogSE.Library.Common;
using DogSE.Library.Time;
using DogSE.Server.Core.Net;

namespace DogSE.Server.Core.Task
{
    /// <summary>
    /// 网络消息调用任务
    /// </summary>
    internal class NetTask : ITask
    {
        /// <summary>
        /// 
        /// </summary>
        public PacketHandler PacketHandler { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public PacketReader PacketReader { get; internal set; }

        /// <summary>
        /// 执行的客户端
        /// </summary>
        public NetState NetState { get; internal set; }

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

                TaskPool.ReleaseContent(this);
                PacketReader.ReleaseContent(PacketReader);
            }
        }

        #endregion

        private TaskManager parent;

        /// <summary>
        /// 任务管理器
        /// </summary>
        public TaskManager Parent
        {
            set { parent = value; }
        }

        ushort _packetId;


        /// <summary>
        /// 任务的对象池
        /// </summary>
        private static readonly ObjectPool<NetTask> TaskPool = new ObjectPool<NetTask>(1000);

        /// <summary>
        /// 从缓冲池里获得一个对象
        /// </summary>
        /// <returns></returns>
        public static NetTask AcquireContent(ushort packetId)
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

    /// <summary>
    /// 网络消息报的任务性能计数器
    /// </summary>
    public class NetTaskProfile : ITaskProfile
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="packageId"></param>
        public NetTaskProfile(ushort packageId)
        {
            PackageId = packageId;
        }

        /// <summary>
        /// 消息包Id
        /// </summary>
        public ushort PackageId { get; private set; }


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
        /// 调用次数
        /// </summary>
        public long Error { get; private set; }

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

        static readonly ConcurrentDictionary<ushort, NetTaskProfile> Map = new ConcurrentDictionary<ushort,NetTaskProfile>();
        private static NetTaskProfile[] profileList;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public static NetTaskProfile GetNetTaskProfile(ushort packageId)
        {
            NetTaskProfile ret;
            if (Map.TryGetValue(packageId, out ret))
            {
                return ret;
            }

            //  新建一个包处理，然后
            ret = new NetTaskProfile(packageId);
            Map.TryAdd(packageId, ret);

            profileList = null;

            return ret;
        }

        /// <summary>
        /// 获得网络性能监控的对象数组
        /// </summary>
        /// <returns></returns>
        public static NetTaskProfile[] GetNetTaskProfile()
        {
            if (profileList != null)
                return profileList;

            profileList = new NetTaskProfile[Map.Count];
            int index = 0;

            foreach (var data in Map)
            {
                profileList[index] = data.Value;
                index++;
            }

            return profileList;
        }

    }

}
