using DogSE.Server.Core.Net;
using DogSE.Server.Core.Protocol;
using TradeAge.Server.Entity.Login;
using TradeAge.Server.Entity.NetCode;

namespace TradeAge.Server.Interface.Client
{
    /// <summary>
    /// 客户端的登陆接口
    /// </summary>
    [ClientInterface]
    public interface ILogin
    {
        /// <summary>
        /// 登陆返回
        /// </summary>
        /// <param name="netstate"></param>
        /// <param name="result"></param>
        /// <param name="isCreatePlayer">玩家是否创建过角色，如果没有创建过，则客户端需要调用创建角色代码</param>
        [NetMethod((ushort)OpCode.LoginServerResult, NetMethodType.SimpleMethod)]
        void LoginServerResult(NetState netstate, LoginServerResult result, bool isCreatePlayer = false);

        /// <summary>
        /// 创建玩家返回结果
        /// </summary>
        /// <param name="netstate"></param>
        /// <param name="result"></param>
        [NetMethod((ushort)OpCode.CreatePlayerResult, NetMethodType.SimpleMethod)]
        void CreatePlayerResult(NetState netstate, CraetePlayerResult result);

        /// <summary>
        /// 登陆游戏时的基本数据已同步完成
        /// 客户端可以开始进入游戏了
        /// </summary>
        /// <param name="netstate"></param>
        [NetMethod((ushort)OpCode.SyncInitDataFinish, NetMethodType.SimpleMethod)]
        void SyncInitDataFinish(NetState netstate);
    }
}
