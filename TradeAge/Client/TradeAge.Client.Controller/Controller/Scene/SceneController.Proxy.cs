
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
/// <param name="rotation"></param>
/// <param name="speed"></param>
/// <param name="rotationRate"></param>
/// <param name="speedUpType"></param>

public void Move(DateTime time,DogSE.Library.Maths.Vector3 postion,DogSE.Library.Maths.Quaternion rotation,float speed,float rotationRate,TradeAge.Client.Entity.Ship.SpeedUpTypes speedUpType)
{
var pw = PacketWriter.AcquireContent(1100);
pw.Write(time.Ticks);
Vector3WriteProxy.Write(postion, pw);
QuaternionWriteProxy.Write(rotation, pw);
pw.Write(speed);
pw.Write(rotationRate);
pw.Write((byte)speedUpType);
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
        public static void Write(DogSE.Library.Maths.Vector3 obj, PacketWriter pw)
        {

pw.Write(obj.X);
pw.Write(obj.Y);
pw.Write(obj.Z);

        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class QuaternionWriteProxy
    {
    /// <summary>
    /// 
    /// </summary>
        public static void Write(DogSE.Library.Maths.Quaternion obj, PacketWriter pw)
        {

pw.Write(obj.X);
pw.Write(obj.Y);
pw.Write(obj.Z);
pw.Write(obj.W);

        }
    }

    }


}

