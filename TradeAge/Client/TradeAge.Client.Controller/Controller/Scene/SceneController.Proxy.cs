
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Client.Core.Net;

namespace TradeAge.Client.Controller.Scene
{


    /// <summary>
    /// 
    /// </summary>
    partial class SceneController
    {
                /// <summary>
        /// 
        /// </summary>
/// <param name="time"></param>
/// <param name="postion"></param>
/// <param name="direction"></param>

public void Move(DateTime time,TradeAge.Client.Entity.Common.Vector2 postion,TradeAge.Client.Entity.Common.Vector2 direction)
{
var pw = PacketWriter.AcquireContent(1100);
pw.Write(time.Ticks);
Vector2WriteProxy.Write(postion, pw);
Vector2WriteProxy.Write(direction, pw);
NetState.Send(pw);PacketWriter.ReleaseContent(pw);
}





    /// <summary>
    /// 
    /// </summary>
    public class Vector2WriteProxy
    {
    /// <summary>
    /// 
    /// </summary>
        public static void Write(TradeAge.Client.Entity.Common.Vector2 obj, PacketWriter pw)
        {

pw.Write(obj.X);
pw.Write(obj.Y);

        }
    }

    }


}

