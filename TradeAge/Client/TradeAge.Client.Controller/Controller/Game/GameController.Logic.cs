using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Library.Log;
using DogSE.Library.Time;

namespace TradeAge.Client.Controller.Game
{
    /// <summary>
    /// 游戏主要的控制器
    /// </summary>
    public partial class GameController: BaseGameController
    {


        /// <summary>
        /// 上一次发送心跳包的时间
        /// </summary>
        private DateTime lastSendTime;

        /// <summary>
        /// 上一次发送心跳包的序列id
        /// </summary>
        private int lastSendHeartbeatSeqId;

        private int lastRecvHeartbetaSeqId;

        /// <summary>
        /// 向服务器要求同步时间
        /// 这里会向服务器发送一个心跳包，然后顺便同步时间
        /// 建议每个1分钟可以向服务器发送一次心跳包
        /// </summary>
        public void SyncTime()
        {
            const int checkHeartbeatCount = 3;

            if (lastSendHeartbeatSeqId - lastRecvHeartbetaSeqId > checkHeartbeatCount)
            {
                //  客户端发了 3 次心跳请求都没收到反馈，说明网络已经断开了
                //  自己断开自己的socket，让客户端显示网络已断开的提示
                nc.NetState.NetSocket.CloseSocket();
                return;
            }

            lastSendTime = DateTime.Now;
            Heartbeat(++lastSendHeartbeatSeqId);
        }

        internal override void OnSyncServerTime(DateTime serverTime, int id)
        {
            if (id != lastSendHeartbeatSeqId)
            {
                Logs.Error("服务器同步时间有问题，id编号错误 {0} {1}", id.ToString(), lastSendHeartbeatSeqId.ToString());
                return;
            }

            lastRecvHeartbetaSeqId = id;

            //  服务器当前的时间可以视为服务器发过来的时间，再加上网络延迟
            //  网络延迟 = (当前时间 - 发送心跳包的时间) / 2
            var ts = DateTime.Now - lastSendTime;
            var nowServerTime = serverTime.AddMilliseconds(ts.TotalMilliseconds/2);
            OneServer.SetServerTime(nowServerTime);
        }
    }
}
