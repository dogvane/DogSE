

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
PacketHandlerManager.Register(1103, OnSpriteMove);
PacketHandlerManager.Register(1104, OnSpriteLeave);

            }

        public PacketHandlersBase PacketHandlerManager {get;set;}

        BaseSceneController module;

void OnEnterSceneInfo(NetState netstate, PacketReader reader){
 var p1 = Vector3ReadProxy.Read(reader);
 var p2 = Vector3ReadProxy.Read(reader);
module.OnEnterSceneInfo(p1,p2);
}
void OnSpriteEnter(NetState netstate, PacketReader reader){
 var p1 = SimplePlayerReadProxy.Read(reader);
module.OnSpriteEnter(p1);
}
void OnSpriteMove(NetState netstate, PacketReader reader){
var p1 = reader.ReadInt32();
 var p2 = Vector3ReadProxy.Read(reader);
 var p3 = Vector3ReadProxy.Read(reader);
module.OnSpriteMove(p1,p2,p3);
}
void OnSpriteLeave(NetState netstate, PacketReader reader){
var p1 = reader.ReadInt32();
module.OnSpriteLeave(p1);
}



    class Vector3ReadProxy
    {
        public static TradeAge.Client.Entity.Common.Vector3 Read(PacketReader reader)
        {
            TradeAge.Client.Entity.Common.Vector3 ret = new TradeAge.Client.Entity.Common.Vector3();

ret.X = reader.ReadFloat();
ret.Y = reader.ReadFloat();
ret.Z = reader.ReadFloat();


            return ret;
        }
    }

    class SimplePlayerReadProxy
    {
        public static TradeAge.Client.Entity.Character.SimplePlayer Read(PacketReader reader)
        {
            TradeAge.Client.Entity.Character.SimplePlayer ret = new TradeAge.Client.Entity.Character.SimplePlayer();

ret.Name = reader.ReadUTF8String();
 ret.Postion = Vector3ReadProxy.Read(reader);
 ret.Direction = Vector3ReadProxy.Read(reader);
ret.Id = reader.ReadInt32();
ret.AccountId = reader.ReadInt32();
ret.Sex = (TradeAge.Client.Entity.Character.Sex)reader.ReadByte();


            return ret;
        }
    }

        }



    }


}


