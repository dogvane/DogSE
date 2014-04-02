
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Client.Core.Net;

namespace TradeAge.Client.Logic.Controller.Login
{


    partial class LoginController
    {
        public void LoginServer(string accountName,string password,int serverId)
{
var pw = new PacketWriter(1000);
pw.WriteUTF8Null(accountName);
pw.WriteUTF8Null(password);
pw.Write(serverId);
NetState.Send(pw);
}

public void CreatePlayer(string playerName,TradeAge.Client.Entity.Character.Sex sex)
{
var pw = new PacketWriter(1003);
pw.WriteUTF8Null(playerName);
pw.Write((byte)sex);
NetState.Send(pw);
}




    }


}

