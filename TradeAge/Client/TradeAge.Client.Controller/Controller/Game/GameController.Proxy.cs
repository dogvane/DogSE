
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Client.Core.Net;

namespace TradeAge.Client.Controller.Game
{


    /// <summary>
    /// 
    /// </summary>
    partial class GameController
    {
                /// <summary>
        /// 
        /// </summary>
/// <param name="id"></param>

public void Heartbeat(int id)
{
var pw = PacketWriter.AcquireContent(1);
pw.Write(id);
NetState.Send(pw);PacketWriter.ReleaseContent(pw);
}




    }


}

