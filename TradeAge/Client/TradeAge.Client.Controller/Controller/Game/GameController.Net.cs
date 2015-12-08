

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Client.Core;
using DogSE.Client.Core.Net;
using DogSE.Client.Core.Task;

namespace TradeAge.Client.Controller.Game
{


    partial class GameController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="net"></param>
        public GameController(NetController net)
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
            public ControllerPacketHandler(NetController net, BaseGameController logic)
            {
                PacketHandlerManager = net.PacketHandlers;

                module = logic;
PacketHandlerManager.Register(2, OnSyncServerTime);

            }

        public PacketHandlersBase PacketHandlerManager {get;set;}

        BaseGameController module;

void OnSyncServerTime(NetState netstate, PacketReader reader){
var p1 = new DateTime(reader.ReadLong64());
var p2 = reader.ReadInt32();
module.OnSyncServerTime(p1,p2);
}



        }



    }


}


