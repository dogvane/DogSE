

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Client.Core;
using DogSE.Client.Core.Net;
using DogSE.Client.Core.Task;

namespace TradeAge.Client.Controller.Scene
{


    partial class SceneController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="net"></param>
        public SceneController(NetController net)
        {
            nc = net;
            new ControllerPacketHandler(net, this);
        }

        private NetController nc;


        private NetState NetState
        {
            get
            {
                return nc.NetState;
            }
        }



        class ControllerPacketHandler
        {
            public ControllerPacketHandler(NetController net, BaseSceneController logic)
            {
                PacketHandlerManager = net.PacketHandlers;

                module = logic;
PacketHandlerManager.Register(1101, OnEnterSceneInfo);
PacketHandlerManager.Register(1102, OnSpriteEnter);
PacketHandlerManager.Register(1104, OnSpriteLeave);
PacketHandlerManager.Register(1103, OnSpriteMove);

            }

        public PacketHandlersBase PacketHandlerManager {get;set;}

        BaseSceneController module;

void OnEnterSceneInfo(NetState netstate, PacketReader reader){
 var p1 = SimplePlayerReadProxy.Read(reader);
module.OnEnterSceneInfo(p1);
}
void OnSpriteEnter(NetState netstate, PacketReader reader){
var len1 = reader.ReadInt32();
var p1 = new TradeAge.Client.Entity.Character.SceneSprite[len1];for(int i =0;i< len1;i++){
p1[i] = SceneSpriteReadProxy.Read(reader);
}
module.OnSpriteEnter(p1);
}
void OnSpriteLeave(NetState netstate, PacketReader reader){
var len1 = reader.ReadInt32();
var p1 = new System.Int32[len1];for(int i =0;i< len1;i++){
p1[i] = reader.ReadInt32();
}
module.OnSpriteLeave(p1);
}
void OnSpriteMove(NetState netstate, PacketReader reader){
var p1 = reader.ReadInt32();
var p2 = new DateTime(reader.ReadLong64());
 var p3 = Vector2ReadProxy.Read(reader);
 var p4 = Vector2ReadProxy.Read(reader);
module.OnSpriteMove(p1,p2,p3,p4);
}



    class Vector2ReadProxy
    {
        public static TradeAge.Client.Entity.Common.Vector2 Read(PacketReader reader)
        {
            TradeAge.Client.Entity.Common.Vector2 ret = new TradeAge.Client.Entity.Common.Vector2();

ret.X = reader.ReadFloat();
ret.Y = reader.ReadFloat();


            return ret;
        }
    }

    class SimplePlayerReadProxy
    {
        public static TradeAge.Client.Entity.Character.SimplePlayer Read(PacketReader reader)
        {
            TradeAge.Client.Entity.Character.SimplePlayer ret = new TradeAge.Client.Entity.Character.SimplePlayer();

ret.Name = reader.ReadUTF8String();
 ret.Postion = Vector2ReadProxy.Read(reader);
 ret.Direction = Vector2ReadProxy.Read(reader);
ret.Id = reader.ReadInt32();
ret.AccountId = reader.ReadInt32();
ret.Sex = (TradeAge.Client.Entity.Character.Sex)reader.ReadByte();


            return ret;
        }
    }

    class SceneSpriteReadProxy
    {
        public static TradeAge.Client.Entity.Character.SceneSprite Read(PacketReader reader)
        {
            TradeAge.Client.Entity.Character.SceneSprite ret = new TradeAge.Client.Entity.Character.SceneSprite();

ret.Id = reader.ReadInt32();
ret.SpriteType = (TradeAge.Client.Entity.Character.SpriteType)reader.ReadByte();
ret.Name = reader.ReadUTF8String();
 ret.Postion = Vector2ReadProxy.Read(reader);
 ret.Direction = Vector2ReadProxy.Read(reader);


            return ret;
        }
    }

        }



    }


}


