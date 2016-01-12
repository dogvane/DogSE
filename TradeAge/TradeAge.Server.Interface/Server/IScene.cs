using System;
using DogSE.Library.Maths;
using DogSE.Server.Core.LogicModule;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Protocol;
using TradeAge.Server.Entity.NetCode;
using TradeAge.Server.Entity.Ship;

namespace TradeAge.Server.Interface.Server
{
    /// <summary>
    /// 场景业务逻辑
    /// </summary>
    public interface IScene : ILogicModule
    {
        /// <summary>
        /// 玩家通报一次移动
        /// </summary>
        /// <remarks>
        /// 我也不想要这么多参数
        /// 但是unity3d的对象的参数，不是以属性的方式存在
        /// 不能被协议代码生成工具识别，只好把参数拆开
        /// </remarks>
        /// <param name="netstate">网络客户端</param>
        /// <param name="time">当前客户端时间</param>
        /// <param name="postion"></param>
        /// <param name="rotation"></param>
        /// <param name="speed"></param>
        /// <param name="rotationRate"></param>
        /// <param name="speedUpType"></param>
        [NetMethod((ushort)OpCode.OnMove, NetMethodType.SimpleMethod)]
        void OnMove(NetState netstate, DateTime time, Vector3 postion, Quaternion rotation,
            float speed, float rotationRate, SpeedUpTypes speedUpType);

    //    void OnMove(NetState netstate, DateTime time, float px, float py, float pz,
    //float rx, float ry, float rz, float rw,
    //float speed, float rotationRate, SpeedUpTypes speedUpType);
    }
}
