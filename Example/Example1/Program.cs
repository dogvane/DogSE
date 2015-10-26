using DogSE.Library.Log;
using DogSE.Server.Core.Net;
using DogSE.Server.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Example2
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Logs.AddConsoleAppender();

            var servers = new Listener<Session>();
            servers.SocketConnect += OnSocketConnect;
            servers.SocketDisconnect += OnSocketDisconnect;
            servers.SocketRecv += OnSocketRecv;
            servers.StartServer(10086);

            Logs.Info("服务器启动，等待客户端连接。按Esc键退出");
            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                    break;

                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// socket连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnSocketConnect(object sender, SocketConnectEventArgs<Session> e)
        {
            //  互相绑定
            var s = new Session();
            s.Client = e.Session;
            e.Session.Data = s;

            nologinSessions.Add(s);

        }

        /// <summary>
        /// Socket关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnSocketDisconnect(object sender, SocketDisconnectEventArgs<Session> e)
        {
            var s = e.Session.Data;
            
            if (s.IsLogin)
                sessions.Remove(s);
            else
                nologinSessions.Remove(s);

            //  解除互相的引用关系
            s.Client = null;
            e.Session.Data = null;
        }

        /// <summary>
        /// 收到Socket数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnSocketRecv(object sender, SocketRecvEventArgs<Session> e)
        {
            var session = e.Session.Data;
            session.RQ.Enqueue(e.Buffer.Bytes, 0, e.Buffer.Length);

            var packetlen = session.RQ.GetPacketLength();
            if (packetlen > 1024*64)
            {
                session.Client.CloseSocket();
                return;
            }

            while (packetlen >= session.RQ.Length)
            {
                var dogBuffer = new DogBuffer();
                session.RQ.Dequeue(dogBuffer.Bytes, 0, packetlen);

                var reader = new PacketReader();
                reader.SetBuffer(dogBuffer);

                var pid = (OpCode)reader.GetPacketID();

                switch (pid)
                {
                    case OpCode.Login:
                    {
                        var userName = reader.ReadUTF8String();
                        var pwd = reader.ReadUTF8String();

                        if (string.IsNullOrEmpty(userName))
                        {
                            Logs.Error("连接的用户名是空");
                            session.Client.CloseSocket();
                        }

                        if (pwd != "123")
                        {
                            Logs.Error("用户名 {0} 速度的密码错误", userName);
                            var writer = new PacketWriter();
                            writer.SetNetCode((ushort) OpCode.LoginResult);
                            writer.Write(1); //  0表示登录成功 1表示密码错误
                            session.Client.SendPackage(writer.GetBuffer());
                            return;
                        }

                        //  如果玩家之前登录过，则把之前的客户端踢下线
                        var exists = sessions.FirstOrDefault(o => o.Name == userName);
                        if (exists != null)
                        {
                            exists.IsLogin = false;
                            sessions.Remove(exists);
                            exists.Client.CloseSocket();
                        }

                        //  登录完成
                        session.IsLogin = true;
                        nologinSessions.Remove(session);
                        sessions.Add(session);

                        session.Name = userName;
                        session.Pwd = pwd;
                        var writer2 = new PacketWriter();
                        writer2.SetNetCode((ushort) OpCode.LoginResult);
                        writer2.Write(0); //  0表示登录成功 1表示密码错误
                        session.Client.SendPackage(writer2.GetBuffer());
                    }
                        break;
                    case OpCode.SendMessage:
                    {
                        var message = reader.ReadUTF8String();
                        if (string.IsNullOrEmpty(message))
                        {
                            //  空消息
                            return;
                        }

                        //  广播给所有在线的用户
                        var writer = new PacketWriter();
                        writer.SetNetCode((ushort) OpCode.RecvMessage);
                        foreach (var ss in sessions)
                        {
                            ss.Client.SendPackage(writer.GetBuffer());
                        }
                    }
                        break;
                    case OpCode.SendPriviteMessage:
                    {
                        var userName = reader.ReadUTF8String();
                        var message = reader.ReadUTF8String();

                        if (message == null)
                            return;

                        var target = sessions.FirstOrDefault(o => o.Name == userName);
                        if (target == null)
                            return;

                        var writer = new PacketWriter();
                        writer.SetNetCode((ushort) OpCode.RecvPrivateMessage);
                        writer.WriteUTF8Null(session.Name);
                        writer.WriteUTF8Null(message);

                        target.Client.SendPackage(writer.GetBuffer());
                    }
                        break;
                    default:
                        Logs.Error("未知消息ID {0}", (int) pid);
                        break;
                }


                packetlen = session.RQ.GetPacketLength();
            }

        }

        /// <summary>
        /// 客户端连接列表
        /// </summary>
        private static readonly List<Session> sessions = new List<Session>();

        /// <summary>
        /// 未验证过的Session
        /// </summary>
        private static readonly List<Session> nologinSessions = new List<Session>();



    }

    public class Session
    {
        public Session()
        {
            RQ = new ReceiveQueue();
        }

        public string Name { get; set; }

        public string Pwd { get; set; }

        /// <summary>
        /// 是否已经登录验证过
        /// </summary>
        public bool IsLogin { get; set; }

        public ClientSession<Session> Client { get; set; }

        public ReceiveQueue RQ { get; set; }
    

    }
}
