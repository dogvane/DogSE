using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Server.Core.LogicModule;
using DogSE.Server.Core.Util;

namespace TradeAge.Server.Logic
{
    public class LogicModule
    {
        public static void Prints()
        {
            var types = AssemblyUtil.GetTypesByInterface(typeof (ILogicModule));
            foreach (var t in types)
            {
                Console.WriteLine(t.Name);
            }
        }
    }
}
