
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
TradeAge.Server.Interface.Client.ClientProxy.Scene = new ISceneProxy1();

        }
    }

    class ILoginProxy1:TradeAge.Server.Interface.Client.ILogin
    {
        public void LoginServerResult(NetState netstate,TradeAge.Server.Entity.Login.LoginServerResult result,bool isCreatePlayer)
{
var pw = new PacketWriter(1001);
            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( 1001 );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
                pw.Write((byte)result);
pw.Write(isCreatePlayer);
netstate.Send(pw);pw.Dispose();
}

public void CreatePlayerResult(NetState netstate,TradeAge.Server.Entity.Login.CraetePlayerResult result)
{
var pw = new PacketWriter(1003);
            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( 1003 );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
                pw.Write((byte)result);
netstate.Send(pw);pw.Dispose();
}




    }


    class ISceneProxy1:TradeAge.Server.Interface.Client.IScene
    {
        public void EnterSceneInfo(NetState netstate,DogSE.Common.Vector3 postion,DogSE.Common.Vector3 direction)
{
var pw = new PacketWriter(1101);
            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( 1101 );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
                pw.WriteStruct(postion);
pw.WriteStruct(direction);
netstate.Send(pw);pw.Dispose();
}

public void SpriteEnter(NetState netstate, TradeAge.Server.Entity.Character.SimplePlayer obj)
{
var pw = new PacketWriter(1102);
            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( 1102 );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
                SimplePlayerWriteProxy.Write(obj, pw);netstate.Send(pw);pw.Dispose();
}

public void SpriteMove(NetState netstate,int playerId,DogSE.Common.Vector3 postion,DogSE.Common.Vector3 direction)
{
var pw = new PacketWriter(1103);
            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( 1103 );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
                pw.Write(playerId);
pw.WriteStruct(postion);
pw.WriteStruct(direction);
netstate.Send(pw);pw.Dispose();
}

public void SpriteLeave(NetState netstate,int playerId)
{
var pw = new PacketWriter(1104);
            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( 1104 );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
                pw.Write(playerId);
netstate.Send(pw);pw.Dispose();
}




    public class SimplePlayerWriteProxy
    {
        public static void Write(TradeAge.Server.Entity.Character.SimplePlayer obj, PacketWriter pw)
        {

pw.WriteUTF8Null(obj.Name);
pw.WriteStruct(obj.Postion);
pw.WriteStruct(obj.Direction);
pw.Write(obj.Id);
pw.Write(obj.AccountId);
pw.Write((byte)obj.Sex);

        }
    }

    }


}

