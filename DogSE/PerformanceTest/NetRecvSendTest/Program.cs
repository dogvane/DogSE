using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using DogSE.Library.Log;
using DogSE.Server.Net;

namespace NetRecvSendTest
{
    /// <summary>
    /// 服务器主要测试网络层的收发性能
    /// 基本是根据客户度过来数据，做1:1的返回
    /// </summary>
    class Program
    {

        private static string GetLocalIp()
        {
            string hostname = Dns.GetHostName(); //得到本机名   
            //IPHostEntry localhost = Dns.GetHostByName(hostname);//方法已过期，只得到IPv4的地址   
            IPHostEntry localhost = Dns.GetHostEntry(hostname);
            foreach (var item in localhost.AddressList)
            {
                if (item.IsIPv6LinkLocal)
                    continue;
                return item.ToString();
            }
            //IPAddress localaddr = localhost.AddressList[0];
            
            //return localaddr.ToString();
            return "localhost";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            Logs.AddConsoleAppender();
            var ip = GetLocalIp();
            int port = 10086;

            var servers = new Listener<Session>();
            servers.SocketConnect += OnSocketConnect;
            servers.SocketDisconnect += OnSocketDisconnect;
            servers.SocketRecv += OnSocketRecv;
            servers.StartServer(ip, port);

            Logs.Info("服务器启动 {0}:{1}，等待客户端连接。按Esc键退出",ip, port);
            Stopwatch time = Stopwatch.StartNew();
            int lastRecvCount = 0;
            int lastRecvLength = 0;

            int lastRecvCount10 = 0;
            int lastRecvLength10 = 0;

            int elaps2 = 0;

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                        break;
                }

                Thread.Sleep(1000);

                var count = recvPackageCount - lastRecvCount;
                var length = recvPackageLength - lastRecvLength;

                var elaps = time.Elapsed.Seconds;
                elaps2 += time.Elapsed.Seconds;
                time.Restart();

                lastRecvCount = recvPackageCount;
                lastRecvLength = recvPackageLength;

                Logs.Info("Time {0}s connect:{1}  package count:{2}   length:{3} ", elaps, sessions.Count, count, length);

                if (elaps2 >= 10)
                {
                    var count10 = recvPackageCount - lastRecvCount10;
                    var length10 = recvPackageLength - lastRecvLength10;

                    time.Restart();

                    lastRecvCount10 = recvPackageCount;
                    lastRecvLength10 = recvPackageLength;

                    Logs.Info("Time {0}s connect:{1}  package count:{2}   length:{3} ", 
                        elaps2, sessions.Count, count10 / 10, length10 / 10);

                    elaps2 = 0;
                }
            }
        }


        /// <summary>
        /// socket连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnSocketConnect(object sender, SocketConnectEventArgs<Session> e)
        {
            lock (sessions)
            {
                //  互相绑定
                var s = new Session();
                s.Client = e.Session;
                e.Session.Data = s;

                sessions.Add(s);
            }
        }

        /// <summary>
        /// Socket关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnSocketDisconnect(object sender, SocketDisconnectEventArgs<Session> e)
        {
            lock (sessions)
            {
                var s = e.Session.Data;

                sessions.Remove(s);

                //  解除互相的引用关系
                s.Client = null;
                e.Session.Data = null;
            }
        }

        /// <summary>
        /// 收到Socket数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnSocketRecv(object sender, SocketRecvEventArgs<Session> e)
        {
            lock (sessions)
            {
                var session = e.Session.Data;
                DogBuffer buf;
                if (e.Buffer.Length > 1024*4)
                {
                    buf = DogBuffer.GetFromPool32K();
                }
                else
                {
                    buf = DogBuffer.GetFromPool4K();
                }

                Array.Copy(e.Buffer.Bytes, buf.Bytes, e.Buffer.Length);
                buf.Length = e.Buffer.Length;
                session.Client.SendPackage(buf);
                buf.Release();

                recvPackageCount++;
                recvPackageLength += e.Buffer.Length;
            }
        }

        private static int recvPackageCount = 0;
        private static int recvPackageLength = 0;

        /// <summary>
        /// 客户端连接列表
        /// </summary>
        private static readonly List<Session> sessions = new List<Session>();

    }

    public class Session
    {
        public Session()
        {
        }

        public ClientSession<Session> Client { get; set; }
    }



}
