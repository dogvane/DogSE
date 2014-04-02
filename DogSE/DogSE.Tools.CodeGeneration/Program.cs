using System;
using System.Collections.Generic;
using System.Text;
using DogSE.Tools.CodeGeneration.Client.Unity3d;
using DogSE.Tools.CodeGeneration.Server;

namespace DogSE.Tools.CodeGeneration
{
    /// <summary>
    /// 代码生成
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
            client.Connect("210.245.121.245", 4530);
            Console.WriteLine("Ok");
        }

        void CreateServerCode()
        {
            ServerLogicProtocolGeneration.CreateCode(@"..\..\..\..\TradeAge\TradeAge.Server.Interface\bin\Debug\TradeAge.Server.Interface.dll",
                       @"..\..\..\..\TradeAge\Server\TradeAge.Server.Protocol\ServerLogicProtocol.cs");

            ClientProxyProtocolGeneration.CreateCode(@"..\..\..\..\TradeAge\TradeAge.Server.Interface\bin\Debug\TradeAge.Server.Interface.dll",
           @"..\..\..\..\TradeAge\Server\TradeAge.Server.Protocol\ClientProxyProtocol.cs");
        }

        void CreateClientCode()
        {
            ClientLogicProtocolGeneration.CreateCode(
                @"..\..\..\..\TradeAge\TradeAge.Server.Interface\bin\Debug\TradeAge.Server.Interface.dll",
                @"..\..\..\..\TradeAge\Client\TradeAge.Client.Logic\",
                "TradeAge.Client.Logic");

            ServerProxyProtocolGeneration.CreateCode(
    @"..\..\..\..\TradeAge\TradeAge.Server.Interface\bin\Debug\TradeAge.Server.Interface.dll",
    @"..\..\..\..\TradeAge\Client\TradeAge.Client.Logic\",
    "TradeAge.Client.Logic");


        }
    }
}
