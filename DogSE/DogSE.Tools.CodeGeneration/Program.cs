using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DogSE.Library.Log;
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
            CreateAdventureProxy();
            return;

            System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
            client.Connect("210.245.121.245", 4530);
            Console.WriteLine("Ok");
        }

        static void test()
        {
            Logs.AddAppender(new ConsoleAppender());

            ServerProxyProtocolGeneration.CreateCode(
    @"..\..\..\..\Adventure.Server.Interface\bin\Debug\Adventure.Server.Interface.dll",
    @"..\..\..\..\..\client\Package\Adventure.Client.Logic\",
    "Adventure.Client.Logic");
        }

        static void CreateAdventureProxy()
        {

            var file =
                new FileInfo(
                    @"..\..\..\..\..\client\Package\Adventure.Client.Logic\Adventure.Client.Logic.csproj");

            Console.WriteLine(file.FullName);
            Console.WriteLine(file.Exists);

            ServerLogicProtocolGeneration.CreateCode(@"..\..\..\..\Adventure.Server.Interface\bin\Debug\Adventure.Server.Interface.dll",
           @"..\..\..\..\Adventure.Server.Protocol\ServerLogicProtocol.cs");

            ClientProxyProtocolGeneration.CreateCode(@"..\..\..\..\Adventure.Server.Interface\bin\Debug\Adventure.Server.Interface.dll",
           @"..\..\..\..\Adventure.Server.Protocol\ClientProxyProtocol.cs");


            ClientLogicProtocolGeneration.CreateCode(
    @"..\..\..\..\Adventure.Server.Interface\bin\Debug\Adventure.Server.Interface.dll",
    @"..\..\..\..\..\client\Package\Adventure.Client.Logic\",
    "Adventure.Client.Logic");


            ServerProxyProtocolGeneration.CreateCode(
    @"..\..\..\..\Adventure.Server.Interface\bin\Debug\Adventure.Server.Interface.dll",
    @"..\..\..\..\..\client\Package\Adventure.Client.Logic\",
    "Adventure.Client.Logic");



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
