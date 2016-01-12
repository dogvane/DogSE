using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Protocol;
using DogSE.Server.Core.Timer;
using DogSE.Common;
using DogSE.Library.Maths;
using TradeAge.Server.Entity.Character;
using TradeAge.Server.Entity.Common;
using TradeAge.Server.Entity.NetCode;
using TradeAge.Server.Entity.Ship;

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
        /// <param name="player">玩家的简单信息</param>
        [NetMethod((ushort)OpCode.EnterSceneInfo, NetMethodType.SimpleMethod)]
        void EnterSceneInfo(NetState netstate,SimplePlayer player);


        /// <summary>
        /// 场景里有其他精灵（玩家）进入
        /// </summary>
        /// <param name="netstate"></param>
        /// <param name="sprite"></param>
        [NetMethod((ushort)OpCode.SpriteEnter, NetMethodType.SimpleMethod)]
        void SpriteEnter(NetState netstate,params SceneSprite[] sprite);

        /// <summary>
        /// 场景里有其他精灵（玩家）进入
        /// </summary>
        /// <param name="netstate"></param>
        /// <param name="spriteId"></param>
        [NetMethod((ushort)OpCode.SpriteLeave, NetMethodType.SimpleMethod)]
        void SpriteLeave(NetState netstate, params int[] spriteId);



        /// <summary>
        /// 通知某个客户端，有精灵在移动
        /// </summary>
        /// <param name="netstate"></param>
        /// <param name="spriteId">精灵id</param>
        /// <param name="time">客户端移动的时间</param>
        /// <param name="postion"></param>
        /// <param name="rotation"></param>
        /// <param name="speed"></param>
        /// <param name="rotationRate"></param>
        /// <param name="speedUpType"></param>

        [NetMethod((ushort)OpCode.SpriteMove, NetMethodType.SimpleMethod)]
        void SpriteMove(NetState netstate, int spriteId, DateTime time, Vector3 postion, Quaternion rotation,
            float speed, float rotationRate, SpeedUpTypes speedUpType);

    }
}
