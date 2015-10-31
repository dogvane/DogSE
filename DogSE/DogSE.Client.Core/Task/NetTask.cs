using System;
//using System.Collections.Concurrent;
using DogSE.Client.Core.Net;
using DogSE.Library.Common;
using DogSE.Library.Time;
using TradeAge.Client.Core.Net;

namespace DogSE.Client.Core.Task
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
                TaskPool.ReleaseContent(this);
                isRelease = true;
                PacketReader.ReleaseContent(PacketReader);
            }
        }

        #endregion

        ushort _packetId;


        /// <summary>
        /// 任务的对象池
        /// </summary>
        private static readonly ObjectPool<NetTask> TaskPool = new ObjectPool<NetTask>(32);

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
        /// <param name="runTick"></param>
        /// <param name="isError"></param>
        public void WriteLog(long runTick, bool isError)
        {
            var now = OneServer.NowTime;
            //NetTaskCodeRuntimeWriter.Write(_packetId, NetState.BizId, runTick, now.Ticks - RecvTime.Ticks, isError);
        }

        #endregion
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

            return ret;
        }
    }

}
