
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
var pw = PacketWriter.AcquireContent(1001);
            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( 1001 );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
                pw.Write((byte)result);
pw.Write(isCreatePlayer);
netstate.Send(pw);
 if ( packetProfile != null ) packetProfile.Record(pw.Length);
PacketWriter.ReleaseContent(pw);
}

public void CreatePlayerResult(NetState netstate,TradeAge.Server.Entity.Login.CraetePlayerResult result)
{
var pw = PacketWriter.AcquireContent(1003);
            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( 1003 );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
                pw.Write((byte)result);
netstate.Send(pw);
 if ( packetProfile != null ) packetProfile.Record(pw.Length);
PacketWriter.ReleaseContent(pw);
}




    }


    class ISceneProxy1:TradeAge.Server.Interface.Client.IScene
    {
        public void EnterSceneInfo(NetState netstate,TradeAge.Server.Entity.Common.Vector3 postion,TradeAge.Server.Entity.Common.Vector3 direction)
{
var pw = PacketWriter.AcquireContent(1101);
            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( 1101 );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
                Vector3WriteProxy.Write(postion, pw);
Vector3WriteProxy.Write(direction, pw);
netstate.Send(pw);
 if ( packetProfile != null ) packetProfile.Record(pw.Length);
PacketWriter.ReleaseContent(pw);
}

public void SpriteEnter(NetState netstate,TradeAge.Server.Entity.Character.SimplePlayer player)
{
var pw = PacketWriter.AcquireContent(1102);
            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( 1102 );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
                SimplePlayerWriteProxy.Write(player, pw);
netstate.Send(pw);
 if ( packetProfile != null ) packetProfile.Record(pw.Length);
PacketWriter.ReleaseContent(pw);
}

public void SpriteMove(NetState netstate,int playerId,TradeAge.Server.Entity.Common.Vector3 postion,TradeAge.Server.Entity.Common.Vector3 direction)
{
var pw = PacketWriter.AcquireContent(1103);
            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( 1103 );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
                pw.Write(playerId);
Vector3WriteProxy.Write(postion, pw);
Vector3WriteProxy.Write(direction, pw);
netstate.Send(pw);
 if ( packetProfile != null ) packetProfile.Record(pw.Length);
PacketWriter.ReleaseContent(pw);
}

public void SpriteLeave(NetState netstate,int playerId)
{
var pw = PacketWriter.AcquireContent(1104);
            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( 1104 );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
                pw.Write(playerId);
netstate.Send(pw);
 if ( packetProfile != null ) packetProfile.Record(pw.Length);
PacketWriter.ReleaseContent(pw);
}




    public class Vector3WriteProxy
    {
        public static void Write(TradeAge.Server.Entity.Common.Vector3 obj, PacketWriter pw)
        {

pw.Write(obj.X);
pw.Write(obj.Y);
pw.Write(obj.Z);

        }
    }

    public class SimplePlayerWriteProxy
    {
        public static void Write(TradeAge.Server.Entity.Character.SimplePlayer obj, PacketWriter pw)
        {

pw.WriteUTF8Null(obj.Name);
Vector3WriteProxy.Write(obj.Postion, pw);
Vector3WriteProxy.Write(obj.Direction, pw);
pw.Write(obj.Id);
pw.Write(obj.AccountId);
pw.Write((byte)obj.Sex);

        }
    }

    }


}

