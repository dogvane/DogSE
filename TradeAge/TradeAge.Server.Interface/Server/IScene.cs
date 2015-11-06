using DogSE.Server.Core.LogicModule;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Protocol;
using TradeAge.Server.Entity.Common;
using TradeAge.Server.Entity.NetCode;

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
        /// <param name="netstate">网络客户端</param>
        /// <param name="postion">当前位置</param>
        /// <param name="direction">朝向（方向以及速度）</param>
        [NetMethod((ushort)OpCode.OnMove, NetMethodType.SimpleMethod)]
        void OnMove(NetState netstate, Vector3 postion, Vector3 direction);
    }
}
