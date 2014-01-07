
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Task;
using DogSE.Server.Core.LogicModule;

namespace DogSE.Server.Core.Protocol.AutoCode
{
    /// <summary>
    /// 代理客户端注册
    /// </summary>
    public static class ClientProxyRegister
    {
        /// <summary>
        /// 注册代理类
        /// </summary>
        public static void Register()
        {
            TradeAge.Server.Interface.Client.ClientProxy.Login = new ILoginProxy1();

        }
    }

    class ILoginProxy1 : TradeAge.Server.Interface.Client.ILogin
    {
        public void LoginServerResult(NetState netstate, TradeAge.Server.Entity.Login.LoginServerResult result)
        {
            var pw = new PacketWriter(1001);
            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile(1001);
            if (packetProfile != null)
                packetProfile.RegConstruct();
            pw.Write((byte)result);
            netstate.Send(pw); pw.Dispose();
        }

        public void CreatePlayerResult(NetState netstate, TradeAge.Server.Entity.Login.CraetePlayerResult result)
        {
            var pw = new PacketWriter(1003);
            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile(1003);
            if (packetProfile != null)
                packetProfile.RegConstruct();
            pw.Write((byte)result);
            netstate.Send(pw); pw.Dispose();
        }


    }


}

