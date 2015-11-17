using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Server.Core.LogicModule;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Protocol;
using TradeAge.Server.Entity.NetCode;

namespace TradeAge.Server.Interface.Server
{
    /// <summary>
    /// 和游戏主体有关的一些接口
    /// </summary>
    public interface IGame : ILogicModule
    {

        /// <summary>
        /// 客户端过来的心跳包
        /// </summary>
        /// <param name="netstate"></param>
        /// <param name="id">心跳包id，服务器确认的时候，把这个返回给客户端</param>
        [NetMethod((ushort)OpCode.Heart, NetMethodType.SimpleMethod, TaskType.Low)]
        void Heartbeat(NetState netstate, int id);
    }
}
