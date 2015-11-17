using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Protocol;
using TradeAge.Server.Entity.NetCode;

namespace TradeAge.Server.Interface.Client
{
    /// <summary>
    /// 和游戏主体相关的一些方法
    /// </summary>
    [ClientInterface]
    public interface IGame
    {
        /// <summary>
        /// 同步当前服务器时间到客户端
        /// </summary>
        /// <param name="netstate"></param>
        /// <param name="serverTime">服务器当前时间</param>
        /// <param name="id">客户度发过来的id，原样返回</param>
        [NetMethod((ushort)OpCode.ServerTime, NetMethodType.SimpleMethod)]
        void SyncServerTime(NetState netstate, DateTime serverTime, int id);
    }
}
