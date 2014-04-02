
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Client.Core.Net;

namespace TradeAge.Client.Logic.Controller.Scene
{


    partial class SceneController
    {
        public void Move(DogSE.Common.Vector3 postion,DogSE.Common.Vector3 direction)
{
var pw = new PacketWriter(1100);
pw.WriteStruct(postion);
pw.WriteStruct(direction);
NetState.Send(pw);
}




    }


}

