using DogSE.Server.Core.LogicModule;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Protocol;
using TradeAge.Common.Entity.NetCode;

namespace TradeAge.Server.Interface.ServerLogic
{
    /// <summary>
    /// 登陆服务器
    /// </summary>
    public interface ILogin : ILogicModule
    {
        /// <summary>
        /// 登陆服务器
        /// </summary>
        /// <param name="netstate"></param>
        /// <param name="accountName"></param>
        /// <param name="password"></param>
        [NetMethod((ushort) OpCode.LoginServer, NetMethodType.SimpleMethod, false)]
        void OnLoginServer(NetState netstate, string accountName, string password);

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="netstate"></param>
        /// <param name="playerName"></param>
        [NetMethod((ushort)OpCode.LoginServer, NetMethodType.SimpleMethod)]
        void OnCreatePlayer(NetState netstate, string playerName);


    }
}
