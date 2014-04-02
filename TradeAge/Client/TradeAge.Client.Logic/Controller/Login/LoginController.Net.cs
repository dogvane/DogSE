

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Client.Core;
using DogSE.Client.Core.Net;
using DogSE.Client.Core.Task;

namespace TradeAge.Client.Logic.Controller.Login
{


    partial class LoginController
    {
        public LoginController(NetController net)
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
            public ControllerPacketHandler(NetController net, BaseLoginController logic)
            {
                PacketHandlerManager = net.PacketHandlers;

                module = logic;
PacketHandlerManager.Register(1001, OnLoginServerResult);
PacketHandlerManager.Register(1003, OnCreatePlayerResult);

            }

        public PacketHandlersBase PacketHandlerManager {get;set;}

        BaseLoginController module;

void OnLoginServerResult(NetState netstate, PacketReader reader){
var p1 = (TradeAge.Client.Entity.Login.LoginServerResult)reader.ReadByte();
var p2 = reader.ReadBoolean();
module.OnLoginServerResult(p1,p2);
}
void OnCreatePlayerResult(NetState netstate, PacketReader reader){
var p1 = (TradeAge.Client.Entity.Login.CraetePlayerResult)reader.ReadByte();
module.OnCreatePlayerResult(p1);
}



        }



    }


}


