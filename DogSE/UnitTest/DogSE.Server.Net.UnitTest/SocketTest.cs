using System.Net.Sockets;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogSE.Server.Net.UnitTest
{
    [TestClass]
    public class SocketTest
    {
        [TestMethod]
        public void ConnectTest()
        {
            var server = new Listener<object>();
            server.StartServer(4530);
            server.SocketConnect += OnSocketConnect;

            TcpClient client = new TcpClient();
            client.Connect("localhost", 4530);
            Assert.IsTrue(client.Connected);

            if (!UnitTestUtil.Wait(1000, () => isConnect))
                Assert.Fail("socket连接在超时后，未出发连接事件。t");

            server.Close();
        }

        private bool isConnect;
        void OnSocketConnect(object sender, SocketConnectEventArgs<object> e)
        {
            isConnect = true;
        }


        private bool isRecvConnect;
        void OnSocketRecvConnect(object sender, SocketConnectEventArgs<object> e)
        {
            isRecvConnect = true;
        }


        [TestMethod]
        public void SendRecvTest()
        {
            isRecv = false;

            var server = new Listener<object>();
            server.StartServer(4531);
            server.SocketConnect += OnSocketRecvConnect;
            server.SocketRecv += server_SocketRecv;
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect("localhost", 4531);
            Assert.IsTrue(client.Connected);

            if (!UnitTestUtil.Wait(1000, () => isRecvConnect))
                Assert.Fail("socket连接在超时后，未出发连接事件。");

            var buffer = new byte[4];
            buffer[0] = 1;
            buffer[3] = 255;

            client.Send(buffer, buffer.Length, SocketFlags.None);

            if (!UnitTestUtil.Wait(1000, () => isRecv))
                Assert.Fail("socket连接成功后发数据，服务器没收到数据。");

            var outbuffer = new byte[10];
            var len = client.Receive(outbuffer);

            Assert.AreEqual(len, 4);
            Assert.AreEqual(outbuffer[0], 1);
            Assert.AreEqual(outbuffer[3], 255);
            Assert.AreEqual(outbuffer[4], 0);

            //  再发送测试一次
            isRecv = false;

            client.Send(buffer, 2, SocketFlags.None);

            if (!UnitTestUtil.Wait(1000, () => isRecv))
                Assert.Fail("socket连接成功后发数据，服务器没收到数据。");

            outbuffer = new byte[10];
            len = client.Receive(outbuffer);
            Assert.AreEqual(len, 2);

            Assert.AreEqual(outbuffer[0], 1);

            server.Close();
        }

        private bool isRecv;
        void server_SocketRecv(object sender, SocketRecvEventArgs<object> e)
        {
            //  把数据原样送回
            e.Session.SendPackage(e.Buffer);
            isRecv = true;
        }

        [TestMethod]
        public void CloseTest()
        {
            isDisconnect = false;
            var server = new Listener<object>();
            server.StartServer(4532);
            server.SocketConnect += OnSocketConnect;
            server.SocketDisconnect += server_SocketDisconnect;
            TcpClient client = new TcpClient();
            client.Connect("localhost", 4532);
            Assert.IsTrue(client.Connected);

            if (!UnitTestUtil.Wait(1000, () => isConnect))
                Assert.Fail("socket连接在超时后，未出发连接事件。");

            client.Close();

            if (!UnitTestUtil.Wait(1000, () => isDisconnect))
                Assert.Fail("服务器没收到客户端断开连接事件。");

            server.Close();
        }

        private bool isDisconnect;
        void server_SocketDisconnect(object sender, SocketDisconnectEventArgs<object> e)
        {
            isDisconnect = true;
        }

        [TestMethod]
        public void CloseSelfTest()
        {
            isDisconnect = false;

            var server = new Listener<object>();
            server.StartServer(4533);
            server.SocketConnect += OnSocketConnect2;
            server.SocketDisconnect += server_SocketDisconnect;
            TcpClient client = new TcpClient();
            client.Connect("localhost", 4533);
            Assert.IsTrue(client.Connected);

            if (!UnitTestUtil.Wait(1000, () => isConnect2))
                Assert.Fail("socket连接在超时后，未出发连接事件。");

            //  这里服务器端会主动断开连接
                
            if (!UnitTestUtil.Wait(8000, () => isDisconnect))
                Assert.Fail("服务器没收到客户端断开连接事件。");

            server.Close();
        }


        private bool isConnect2;
        void OnSocketConnect2(object sender, SocketConnectEventArgs<object> e)
        {
            isConnect2 = true;
            ThreadPool.QueueUserWorkItem(o =>
            {
                //  先延迟500ms再关闭
                Thread.Sleep(500);
                e.Session.CloseSocket();
            });
        }
    }
}
