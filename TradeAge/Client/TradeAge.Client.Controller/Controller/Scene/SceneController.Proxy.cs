
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
/// <param name="postion"></param>
/// <param name="direction"></param>

public void Move(TradeAge.Client.Entity.Common.Vector3 postion,TradeAge.Client.Entity.Common.Vector3 direction)
{
var pw = PacketWriter.AcquireContent(1100);
Vector3WriteProxy.Write(postion, pw);
Vector3WriteProxy.Write(direction, pw);
NetState.Send(pw);PacketWriter.ReleaseContent(pw);
}





    /// <summary>
    /// 
    /// </summary>
    public class Vector3WriteProxy
    {
    /// <summary>
    /// 
    /// </summary>
        public static void Write(TradeAge.Client.Entity.Common.Vector3 obj, PacketWriter pw)
        {

pw.Write(obj.X);
pw.Write(obj.Y);
pw.Write(obj.Z);

        }
    }

    }


}

