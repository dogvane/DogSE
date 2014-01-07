using DogSE.Server.Core.LogicModule;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Protocol;
using TradeAge.Common.Entity.NetCode;
using TradeAge.Server.Entity.Character;

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
        /// <param name="serverId">服务器id</param>
        [NetMethod((ushort) OpCode.LoginServer, NetMethodType.SimpleMethod, false)]
        void OnLoginServer(NetState netstate, string accountName, string password, int serverId = 0);

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="netstate"></param>
        /// <param name="playerName"></param>
        /// <param name="sex">性别</param>
        [NetMethod((ushort)OpCode.CreatePlayerResult, NetMethodType.SimpleMethod)]
        void OnCreatePlayer(NetState netstate, string playerName, Sex sex);


    }
}
