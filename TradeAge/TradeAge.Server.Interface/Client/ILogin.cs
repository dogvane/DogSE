using DogSE.Server.Core.Protocol;
using TradeAge.Common.Entity.NetCode;
using TradeAge.Server.Entity.Login;

namespace TradeAge.Server.Interface.Client
{
    /// <summary>
    /// 客户端的登陆接口
    /// </summary>
    public interface ILogin
    {
        /// <summary>
        /// 登陆返回
        /// </summary>
        /// <param name="result"></param>
        [NetMethod((ushort)OpCode.LoginServerResult, NetMethodType.SimpleMethod)]
        void LoginServerResult(LoginServerResult result);

        /// <summary>
        /// 创建玩家返回结果
        /// </summary>
        /// <param name="result"></param>
        [NetMethod((ushort)OpCode.CreatePlayerResult, NetMethodType.SimpleMethod)]
        void CreatePlayerResult(CraetePlayerResult result);
    }
}
