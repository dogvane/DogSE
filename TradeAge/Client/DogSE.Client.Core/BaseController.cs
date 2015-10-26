using DogSE.Client.Core.Net;
using DogSE.Client.Core.Task;
using DogSE.Client.Core.Timer;
using System;
using DogSE.Library.Log;

namespace DogSE.Client.Core
{
    /// <summary>
    /// 网络连接的控制器
    /// </summary>
    public class NetController
    {
        static NetController()
        {
            TimerThread.TaskManager = s_taskManager;
            TimerThread.StartTimerThread();

            //  在u3d环境下，任务需要丢到主线程里执行
            //  因为默认情况会在里面进行图形绘制
            //  模拟器或者测试用例，则需要自己手动启动处理线程
            //s_taskManager.StartThread();
        }


        /// <summary>
        /// 关闭所有的线程
        /// </summary>
        public static void CloseThread()
        {
            Logs.Info("net controller is close.");
            s_taskManager.Runing = false;
            GameServerService.RunType = ServerStateType.Closing;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 这里任务管理器做成静态的
        /// 除了因为时间管理器也是静态的原因外
        /// 客户端的Controller将来会变为测试工具（压力测试工具）用的Controller
        /// 控制器不会是唯一的，所以，用静态的比较好，可以少开一些线程
        /// </remarks>
        private readonly static Unity3DTaskManager s_taskManager = new Unity3DTaskManager("logic");

        /// <summary>
        /// 对外公开的任务管理器
        /// </summary>
        public static Unity3DTaskManager TaskManager
        {
            get { return s_taskManager; }
        }


        private bool m_isStartWorld;


        /// <summary>
        /// 默认构造函数
        /// </summary>
        public NetController()
        {
            NetState = new NetState(new ClientSession<NetState>());
            NetState.NetSocket.SocketConnect += NetSocket_SocketConnect;
            NetState.NetSocket.SocketDisconnect += NetSocket_SocketDisconnect;
            NetState.NetSocket.SocketRecv += NetSocket_SocketRecv;
        }

        /// <summary>
        /// 新建一个NetSatte 为了断线重练，重新初始化数据
        /// </summary>
        public void NewNetState()
        {
            if (NetState != null && NetState.NetSocket != null)
            {
                NetState.NetSocket.SocketConnect += NetSocket_SocketConnect;
                NetState.NetSocket.SocketDisconnect += NetSocket_SocketDisconnect;
                NetState.NetSocket.SocketRecv += NetSocket_SocketRecv;
            }

            NetState = new NetState(new ClientSession<NetState>());
            NetState.NetSocket.SocketConnect += NetSocket_SocketConnect;
            NetState.NetSocket.SocketDisconnect += NetSocket_SocketDisconnect;
            NetState.NetSocket.SocketRecv += NetSocket_SocketRecv;
        }

        private const int MaxPackageSize = ReceiveQueue.BUFFER_SIZE;

        /// <summary>
        /// 是否处于等待心跳包回执的状态
        /// </summary>
        public bool IsWaitCheckOnline { get; set; }

        void NetSocket_SocketRecv(object sender, SocketRecvEventArgs<NetState> e)
        {
            var netState = e.Session.Data;
            netState.ReceiveBuffer.Enqueue(e.Buffer.Bytes, 0, e.Buffer.Length);

            if (netState.ReceiveBuffer.Length > MaxPackageSize)
            {
                //  缓冲区过多，一定发送了某种异常情况
                Logs.Error("client recv buff is full.");
                netState.NetSocket.CloseSocket();
                return;
            }

            while(netState.ReceiveBuffer.Length > 2)
            {
                var len = netState.ReceiveBuffer.GetPacketLength();
                if (len == 0)
                {
                    Logs.Error("get package len is zero.");
                    netState.NetSocket.CloseSocket();
                    return;
                }

                if (len < 4)
                {
                    Logs.Error("package is min 4.");
                    netState.NetSocket.CloseSocket();
                    return;
                }


                if (len > MaxPackageSize)
                {
                    Logs.Error("get package len is error. size:{0}", len);
                    netState.NetSocket.CloseSocket();
                    return;
                }

                IsWaitCheckOnline = false;

                if (len <= netState.ReceiveBuffer.Length)
                {

                    DogBuffer readBuffer;
                    if (len < 1024*4)
                        readBuffer = DogBuffer.GetFromPool4K();
                    else
                        readBuffer = DogBuffer.GetFromPool32K();

                    if (len >= readBuffer.Bytes.Length)
                        readBuffer.UpdateCapacity(len);

                    var get = netState.ReceiveBuffer.Dequeue(readBuffer.Bytes, 0, len);
                    if (get == len)
                    {
                        readBuffer.Length = len;

                        var packageReader = PacketReader.AcquireContent(readBuffer);
                        ushort id = packageReader.GetPacketID();
                        Logs.Debug("msgid= {0}", id);

                        var packetHandler = PacketHandlersManger.GetHandler(id);
                        if (packetHandler != null)
                        {
                            //  加入网络消息处理
                            TaskManager.AppendTask(netState, packetHandler, packageReader);
                        }
                        else
                        {
                            Logs.Error("unknow packetid. code={0}", id);
                        }
                    }

                    continue;
                }

                break;
            } 
        }

        void NetSocket_SocketDisconnect(object sender, SocketDisconnectEventArgs<NetState> e)
        {
            //m_netStateManager.InternalRemoveNetState(netState.Serial);
            //netState.ExitWorld();

            //  通知业务逻辑有客户端连接上来可以做一些初始化
            //  或者判断是否允许本次连接
            var tempEV = NetStateDisconnect;
            if (tempEV != null)
            {
                var arg = new NetStateDisconnectEventArgs
                {
                    NetState = NetState,
                };
                tempEV(this, arg);
            }

            //NetState.Dispose();
        }

        void NetSocket_SocketConnect(object sender, SocketConnectEventArgs<NetState> e)
        {
            //  通知业务逻辑有客户端连接上来可以做一些初始化
            //  或者判断是否允许本次连接
            var tempEV = NetStateConnect;
            if (tempEV != null)
            {
                var arg = new NetStateConnectEventArgs
                {
                    NetState = NetState,
                    IsConnected = Connected,
                };
                tempEV(this, arg);
            }
        }

        /// <summary>
        /// 开启游戏世界
        /// </summary>
        public void StartWorld()
        {
            if (m_isStartWorld)
                return;
            m_isStartWorld = true;
        }

        /// <summary>
        /// 游戏世界停止
        /// </summary>
        public void StopWorld()
        {
            Logs.Info("net controller is stop");
            m_isStartWorld = false;

            NetState.NetSocket.SocketConnect -= NetSocket_SocketConnect;
            NetState.NetSocket.SocketRecv -= NetSocket_SocketRecv;

            NetState.NetSocket.CloseSocket();
            NetState.NetSocket.SocketDisconnect -= NetSocket_SocketDisconnect;
            NetState.Dispose();
        }

        /// <summary>
        /// 网络连接对象
        /// </summary>
        public NetState NetState { get; private set; }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="host">服务器地址(ip)</param>
        /// <param name="port">服务器端口号</param>
        public void ConnectServer(string host, int port)
        {
            NetState.NetSocket.BeginConnect(host, port);
            m_server = host;
            m_port = port;
        }

        private string m_server;

        /// <summary>
        /// 服务器地址
        /// </summary>
        public string Host { get { return m_server; } }

        private int m_port;

        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get { return m_port; } }

        /// <summary>
        /// 重新连服务器
        /// </summary>
        public void ReConnectServer()
        {
            NetState.NetSocket.BeginConnect(m_server, m_port);
        }

        /// <summary>
        /// 客户端对服务器的连接状态
        /// </summary>
        public bool Connected
        {
            get
            {
                if (NetState == null || NetState.NetSocket == null || NetState.NetSocket.Socket == null)
                    return false;

                return NetState.NetSocket.Socket.Connected;
            }
        }

        /// <summary>
        /// 网络连接事件
        /// </summary>
        public event EventHandler<NetStateConnectEventArgs> NetStateConnect;

        /// <summary>
        /// 网络连接关闭事件
        /// </summary>
        public event EventHandler<NetStateDisconnectEventArgs> NetStateDisconnect;


        readonly PacketHandlersBase PacketHandlersManger = new PacketHandlersBase();

        /// <summary>
        /// 包句柄管理器
        /// </summary>
        public PacketHandlersBase PacketHandlers
        {
            get { return PacketHandlersManger; }
        }

        /// <summary>
        /// 和netcontroller 关联的对象
        /// </summary>
        public object Tag { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class NetStateDisconnectEventArgs : EventArgs
    {
        /// <summary>
        /// 网络连接
        /// </summary>
        public NetState NetState { get; internal set; }
    }

    /// <summary>
    /// 网络连接事件
    /// </summary>
    public class NetStateConnectEventArgs : EventArgs
    {
        /// <summary>
        /// 网络连接
        /// </summary>
        public NetState NetState{get;internal set;}

        /// <summary>
        /// 是否连接成功
        /// </summary>
        public bool IsConnected{get;set;}
    }
}
