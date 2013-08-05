
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Task;


namespace DogSE.Server.Core.Protocol.AutoCode
{
    class PacketReaderModuleAccess1:IProtoclAutoCode
    {
        public PacketHandlersBase PacketHandlerManager {get; set; }

        public DogSE.Server.Core.UnitTest.Protocol.PacketReaderModule module;

        public void Init()
        {
PacketHandlerManager.Register(1, module.OnReadTest);

        }

void OnReadTest(NetState netstate, PacketReader reader){
module.OnReadTest(netstate, reader);}

    }
}
