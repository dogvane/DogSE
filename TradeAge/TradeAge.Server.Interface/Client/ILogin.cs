using DogSE.Server.Core.Net;
using DogSE.Server.Core.Protocol;
using TradeAge.Common.Entity.NetCode;
using TradeAge.Server.Entity.Login;

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
        [NetMethod((ushort)OpCode.LoginServerResult, NetMethodType.SimpleMethod)]
        void LoginServerResult(NetState netstate, LoginServerResult result);

        /// <summary>
        /// 创建玩家返回结果
        /// </summary>
        /// <param name="netstate"></param>
        /// <param name="result"></param>
        [NetMethod((ushort)OpCode.CreatePlayerResult, NetMethodType.SimpleMethod)]
        void CreatePlayerResult(NetState netstate, CraetePlayerResult result);
    }
}
