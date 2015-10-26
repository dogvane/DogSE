
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Client.Core.Net;

namespace TradeAge.Client.Controller.Login
{


    /// <summary>
    /// 
    /// </summary>
    partial class LoginController
    {
                /// <summary>
        /// 
        /// </summary>
/// <param name="accountName"></param>
/// <param name="password"></param>
/// <param name="serverId"></param>

public void LoginServer(string accountName,string password,int serverId)
{
var pw = PacketWriter.AcquireContent(1000);
pw.WriteUTF8Null(accountName);
pw.WriteUTF8Null(password);
pw.Write(serverId);
NetState.Send(pw);PacketWriter.ReleaseContent(pw);
}

        /// <summary>
        /// 
        /// </summary>
/// <param name="playerName"></param>
/// <param name="sex"></param>

public void CreatePlayer(string playerName,TradeAge.Client.Entity.Character.Sex sex)
{
var pw = PacketWriter.AcquireContent(1003);
pw.WriteUTF8Null(playerName);
pw.Write((byte)sex);
NetState.Send(pw);PacketWriter.ReleaseContent(pw);
}




    }


}

