using DogSE.Server.Core.LogicModule;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Protocol;
using TradeAge.Server.Entity.Character;
using TradeAge.Server.Entity.NetCode;

namespace TradeAge.Server.Interface.Server
{
    /// <summary>
    /// 登陆服务器模块
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

        //void OnLoginServer(NetState netsatte, PacketReader reader);

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
