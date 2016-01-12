
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
TradeAge.Server.Interface.Client.ClientProxy.Game = new IGameProxy1();

        }
    }

    class IGameProxy1:TradeAge.Server.Interface.Client.IGame
    {
        public void SyncServerTime(NetState netstate,DateTime serverTime,int id)
{
var pw = PacketWriter.AcquireContent(2);
            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( 2 );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
                pw.Write(serverTime.Ticks);
pw.Write(id);
netstate.Send(pw);
 if ( packetProfile != null ) packetProfile.Record(pw.Length);
PacketWriter.ReleaseContent(pw);
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

public void SyncInitDataFinish(NetState netstate)
{
var pw = PacketWriter.AcquireContent(1004);
            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( 1004 );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
                netstate.Send(pw);
 if ( packetProfile != null ) packetProfile.Record(pw.Length);
PacketWriter.ReleaseContent(pw);
}




    }


    class ISceneProxy1:TradeAge.Server.Interface.Client.IScene
    {
        public void EnterSceneInfo(NetState netstate,TradeAge.Server.Entity.Character.SimplePlayer player)
{
var pw = PacketWriter.AcquireContent(1101);
            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( 1101 );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
                SimplePlayerWriteProxy.Write(player, pw);
netstate.Send(pw);
 if ( packetProfile != null ) packetProfile.Record(pw.Length);
PacketWriter.ReleaseContent(pw);
}

public void SpriteEnter(NetState netstate,TradeAge.Server.Entity.Character.SceneSprite[] sprite)
{
var pw = PacketWriter.AcquireContent(1102);
            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( 1102 );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
                int spritelen = sprite == null ? 0:sprite.Length;pw.Write(spritelen);
for(int i = 0;i < spritelen ;i++){
SceneSpriteWriteProxy.Write(sprite[i], pw);
}
netstate.Send(pw);
 if ( packetProfile != null ) packetProfile.Record(pw.Length);
PacketWriter.ReleaseContent(pw);
}

public void SpriteLeave(NetState netstate,System.Int32[] spriteId)
{
var pw = PacketWriter.AcquireContent(1104);
            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( 1104 );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
                int spriteIdlen = spriteId == null ? 0:spriteId.Length;pw.Write(spriteIdlen);
for(int i = 0;i < spriteIdlen ;i++){
pw.Write(spriteId[i]);
}
netstate.Send(pw);
 if ( packetProfile != null ) packetProfile.Record(pw.Length);
PacketWriter.ReleaseContent(pw);
}

public void SpriteMove(NetState netstate,int spriteId,DateTime time,DogSE.Library.Maths.Vector3 postion,DogSE.Library.Maths.Quaternion rotation,float speed,float rotationRate,TradeAge.Server.Entity.Ship.SpeedUpTypes speedUpType)
{
var pw = PacketWriter.AcquireContent(1103);
            PacketProfile packetProfile = PacketProfile.GetOutgoingProfile( 1103 );
            if ( packetProfile != null )
                packetProfile.RegConstruct();
                pw.Write(spriteId);
pw.Write(time.Ticks);
Vector3WriteProxy.Write(postion, pw);
QuaternionWriteProxy.Write(rotation, pw);
pw.Write(speed);
pw.Write(rotationRate);
pw.Write((byte)speedUpType);
netstate.Send(pw);
 if ( packetProfile != null ) packetProfile.Record(pw.Length);
PacketWriter.ReleaseContent(pw);
}




    public class Vector3WriteProxy
    {
        public static void Write(DogSE.Library.Maths.Vector3 obj, PacketWriter pw)
        {

pw.Write(obj.X);
pw.Write(obj.Y);
pw.Write(obj.Z);

        }
    }

    public class QuaternionWriteProxy
    {
        public static void Write(DogSE.Library.Maths.Quaternion obj, PacketWriter pw)
        {

pw.Write(obj.X);
pw.Write(obj.Y);
pw.Write(obj.Z);
pw.Write(obj.W);

        }
    }

    public class SimplePlayerWriteProxy
    {
        public static void Write(TradeAge.Server.Entity.Character.SimplePlayer obj, PacketWriter pw)
        {

pw.WriteUTF8Null(obj.Name);
Vector3WriteProxy.Write(obj.Postion, pw);
QuaternionWriteProxy.Write(obj.Rotation, pw);
pw.Write(obj.Speed);
pw.Write(obj.Id);
pw.Write(obj.AccountId);
pw.Write((byte)obj.Sex);

        }
    }

    public class SceneSpriteWriteProxy
    {
        public static void Write(TradeAge.Server.Entity.Character.SceneSprite obj, PacketWriter pw)
        {

pw.Write(obj.Id);
pw.Write((byte)obj.SpriteType);
pw.WriteUTF8Null(obj.Name);
Vector3WriteProxy.Write(obj.Postion, pw);
QuaternionWriteProxy.Write(obj.Rotation, pw);
pw.Write(obj.Speed);

        }
    }

    }


}

