using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Library.Time;
using DogSE.Server.Core.Net;
using TradeAge.Server.Entity.Character;
using TradeAge.Server.Interface.Client;
using IGame = TradeAge.Server.Interface.Server.IGame;

namespace TradeAge.Server.Logic.Game
{
    /// <summary>
    /// 游戏的一个主体模块
    /// </summary>
    public class GameModule:IGame
    {
        /// <summary>
        /// 模块的ID（名字）
        /// </summary>
        public string ModuleId {
            get { return "GameModule"; }
        }

        /// <summary>
        /// 模块初始化中
        /// 和模块相关的控制器 Controller 在这里初始化
        /// </summary>
        public void Initializationing()
        {
        }

        /// <summary>
        /// 模块初始化结束
        /// 和模块相关的事件在这里初始化
        /// </summary>
        public void Initializationed()
        {
        }

        /// <summary>
        /// 重新加载模板（内部重新初始化）
        /// </summary>
        public void ReLoadTemplate()
        {
        }

        /// <summary>
        /// 服务器停止时释放资源
        /// </summary>
        public void Release()
        {
        }

        /// <summary>
        /// 客户端过来的心跳包
        /// </summary>
        /// <param name="netstate"></param>
        /// <param name="id">心跳包id，服务器确认的时候，把这个返回给客户端</param>
        public void Heartbeat(NetState netstate, int id)
        {
            ClientProxy.Game.SyncServerTime(netstate, OneServer.NowTime, id);

            //  记录下心跳包的时间，服务器可以用这个值，判断客户端是否掉线
            var player = (Player)netstate.Player;
            player.LastHeartbeat = OneServer.NowTime;
        }
    }
}
