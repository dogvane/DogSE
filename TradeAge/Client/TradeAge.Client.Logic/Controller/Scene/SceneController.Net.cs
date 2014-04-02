

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Client.Core;
using DogSE.Client.Core.Net;
using DogSE.Client.Core.Task;

namespace TradeAge.Client.Logic.Controller.Scene
{


    partial class SceneController
    {
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
var p1 = reader.ReadStruct <DogSE.Common.Vector3>();
var p2 = reader.ReadStruct <DogSE.Common.Vector3>();
module.OnEnterSceneInfo(p1,p2);
}
void OnSpriteEnter(NetState netstate, PacketReader reader){
 var obj = SimplePlayerReadProxy.Read(reader);
module.OnSpriteEnter(obj);}
void OnSpriteMove(NetState netstate, PacketReader reader){
var p1 = reader.ReadInt32();
var p2 = reader.ReadStruct <DogSE.Common.Vector3>();
var p3 = reader.ReadStruct <DogSE.Common.Vector3>();
module.OnSpriteMove(p1,p2,p3);
}
void OnSpriteLeave(NetState netstate, PacketReader reader){
var p1 = reader.ReadInt32();
module.OnSpriteLeave(p1);
}



    class SimplePlayerReadProxy
    {
        public static TradeAge.Client.Entity.Character.SimplePlayer Read(PacketReader reader)
        {
            TradeAge.Client.Entity.Character.SimplePlayer ret = new TradeAge.Client.Entity.Character.SimplePlayer();

ret.Name = reader.ReadUTF8String();
ret.Postion = reader.ReadStruct <DogSE.Common.Vector3>();
ret.Direction = reader.ReadStruct <DogSE.Common.Vector3>();
ret.Id = reader.ReadInt32();
ret.AccountId = reader.ReadInt32();
ret.Sex = (TradeAge.Client.Entity.Character.Sex)reader.ReadByte();


            return ret;
        }
    }

        }



    }


}


