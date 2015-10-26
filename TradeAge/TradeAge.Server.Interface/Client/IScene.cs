using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Protocol;
using DogSE.Server.Core.Timer;
using DogSE.Common;
using TradeAge.Common.Entity.NetCode;
using TradeAge.Server.Entity.Character;
using TradeAge.Server.Entity.Common;

namespace TradeAge.Server.Interface.Client
{

    /// <summary>
    /// 通知客户端的场景信息
    /// </summary>
    [ClientInterface]
    public interface IScene
    {
        /// <summary>
        /// 玩家进入场景的基本信息
        /// </summary>
        /// <param name="netstate"></param>
        /// <param name="postion"></param>
        /// <param name="direction"></param>
        [NetMethod((ushort)OpCode.EnterSceneInfo, NetMethodType.SimpleMethod)]
        void EnterSceneInfo(NetState netstate, Vector3 postion, Vector3 direction);


        /// <summary>
        /// 场景里有其他精灵（玩家）进入
        /// </summary>
        /// <param name="netstate"></param>
        /// <param name="player"></param>
        [NetMethod((ushort)OpCode.SpriteEnter, NetMethodType.SimpleMethod)]
        void SpriteEnter(NetState netstate, SimplePlayer player);

        /// <summary>
        /// 通知某个客户端，有精灵在移动
        /// </summary>
        /// <param name="netstate"></param>
        /// <param name="playerId"></param>
        /// <param name="postion"></param>
        /// <param name="direction"></param>

        [NetMethod((ushort)OpCode.SpriteMove, NetMethodType.SimpleMethod)]
        void SpriteMove(NetState netstate, int playerId, Vector3 postion, Vector3 direction);

        /// <summary>
        /// 场景里有其他精灵（玩家）进入
        /// </summary>
        /// <param name="netstate"></param>
        /// <param name="playerId"></param>
        [NetMethod((ushort)OpCode.SpriteLeave, NetMethodType.SimpleMethod)]
        void SpriteLeave(NetState netstate, int playerId);

    }
}
