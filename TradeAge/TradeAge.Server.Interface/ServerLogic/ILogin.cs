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
        [NetMethod((ushort) OpCode.LoginServer, NetMethodType.SimpleMethod)]
        void OnLoginServer(NetState netstate, string accountName, string password);
    }
}
