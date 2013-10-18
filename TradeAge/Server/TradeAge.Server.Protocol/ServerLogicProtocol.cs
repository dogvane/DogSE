
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Task;
using DogSE.Server.Core.LogicModule;

namespace DogSE.Server.Core.Protocol.AutoCode
{
    internal class ILoginAccess1 : IProtoclAutoCode
    {
        public PacketHandlersBase PacketHandlerManager { get; set; }

        private TradeAge.Server.Interface.ServerLogic.ILogin module;

        public void SetModule(ILogicModule m)
        {
            if (m == null)
                throw new ArgumentNullException("ILogicModule");
            module = (TradeAge.Server.Interface.ServerLogic.ILogin) m;
            if (module == null)
            {
                throw new NullReferenceException(string.Format("{0} not TradeAge.Server.Interface.ServerLogic.ILogin",
                                                               m.GetType().FullName));
            }
        }


        public void Init()
        {
            PacketHandlerManager.Register(1000, OnLoginServer);

        }

        private void OnLoginServer(NetState netstate, PacketReader reader)
        {
            var p1 = reader.ReadUTF8String();
            var p2 = reader.ReadUTF8String();
            module.OnLoginServer(netstate, p1, p2);
        }

    }

}

