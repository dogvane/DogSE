using DogSE.Library.Log;
using DogSE.Server.Core.Config;
using DogSE.Server.Core.LogicModule;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Timer;
using DogSE.Server.Core.Protocol;
using DogSE.Server.Core.Task;
using DogSE.Server.Net;
using System;

namespace DogSE.Server.Core
{
    /// <summary>
    /// 基本的游戏世界
    /// </summary>
    public class WorldBase
    {
        private bool m_isStartWorld;

        /// <summary>
        /// 游戏世界对应的服务器监听器
        /// </summary>
        public Listener<NetState>[] Listeners { get; set; }

        /// <summary>
        /// 开启游戏世界
        /// </summary>
        public void StartWorld()
        {
            if (m_isStartWorld)
                return;
            m_isStartWorld = true;

            StaticConfigFileManager.LoadData();

            InitLogicModule();

            StartServerSocket();

            TimerThread.MainTask = mainTask;
            TimerThread.StartTimerThread();

            mainTask.StartThread();

            if (UseManyTaskThread)
            {
                lowTask.StartThread();
                assistTask.StartThread();
            }
        }

        /// <summary>
        /// 游戏世界停止
        /// </summary>
        public void StopWorld()
        {
            //  先退出socket，关闭连接
            StopServerSocket();

            m_isStartWorld = false;
            mainTask.Runing = false;
            lowTask.Runing = false;
            assistTask.Runing = false;

            //  等待任务线程退出

            foreach (var module in logicModuleManager.GetModules())
            {
                module.Release();
            }
        }

        /// <summary>
        /// 初始化游戏逻辑模块
        /// </summary>
        private void InitLogicModule()
        {
            logicModuleManager.Initializationing();
            logicModuleManager.Initializationed();

            var modules = logicModuleManager.GetModules();

            //  注册网络消息码
            if (IsAutoRegisterMessage)
                new RegisterNetMethod(PacketHandlersManger).Register(modules);
        }

        private bool isAutoRegisterMessage = true;

        /// <summary>
        /// 是否自动对逻辑模块自动注册消息处理函数
        /// </summary>
        public bool IsAutoRegisterMessage
        {
            get { return isAutoRegisterMessage; }
            set { isAutoRegisterMessage = value; }
        }

        /// <summary>
        /// 开始服务器socket，可以开始接收客户端数据
        /// </summary>
        private void StartServerSocket()
        {
            var serverTcpConfig = ServerConfig.Tcp;
            if (serverTcpConfig == null || serverTcpConfig.Length == 0)
            {
                Logs.Error(@"请先配置..\Server.Config 文件下root\ServerConfig\Tcp 的服务器配置");
                return;
            }

            Listeners = new Listener<NetState>[serverTcpConfig.Length];
            var index = 0;
            foreach (var tcp in serverTcpConfig)
            {
                var linster = new Listener<NetState>();
                linster.StartServer(tcp.Host, tcp.Port);

                linster.SocketConnect += OnSocketConnect;
                linster.SocketDisconnect += OnSocketDisconnect;
                linster.SocketRecv += OnSocketRecv;

                Listeners[index++] = linster;
                Logs.Info("open socket {0}:{1}", tcp.Host, tcp.Port);
            }
        }

        private void StopServerSocket()
        {
            foreach (var linster in Listeners)
            {
                linster.Close();

                linster.SocketConnect -= OnSocketConnect;
                linster.SocketDisconnect -= OnSocketDisconnect;
                linster.SocketRecv -= OnSocketRecv;
            }
        }

    /// <summary>
        /// 收到网络消息包
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSocketRecv(object sender, SocketRecvEventArgs<NetState> e)
        {
            var netState = e.Session.Data;
            netState.ReceiveBuffer.Enqueue(e.Buffer.Bytes, 0, e.Buffer.Length);

            var len = netState.ReceiveBuffer.GetPacketLength();
            do
            {
                if (len <= netState.ReceiveBuffer.Length)
                {

                    DogBuffer readBuffer;
                    if (len < 1024*4)
                        readBuffer = DogBuffer.GetFromPool4K();
                    else
                        readBuffer = DogBuffer.GetFromPool32K();

                    var get = netState.ReceiveBuffer.Dequeue(readBuffer.Bytes, 0, len);
                    if (get == len)
                    {
                        readBuffer.Length = len;

                        var packageReader = new PacketReader(readBuffer);
                        var packageId = packageReader.GetPacketID();
                        var packetHandler = PacketHandlersManger.GetHandler(packageId);
                        if (packetHandler != null)
                        {
                            //  加入网络消息处理
                            if (_useManyTaskThread)
                            {
                                switch (packetHandler.TaskType)
                                {
                                    case TaskType.Low:
                                        lowTask.AppendTask(netState, packetHandler, packageReader);
                                        break;
                                    case TaskType.Assist:
                                        Logs.Debug("assist task.");
                                        assistTask.AppendTask(netState, packetHandler, packageReader);
                                        break;
                                    default:
                                        mainTask.AppendTask(netState, packetHandler, packageReader);
                                        break;
                                }
                            }
                            else
                            {
                                mainTask.AppendTask(netState, packetHandler, packageReader);
                            }
                        }
                        else
                        {
                            Logs.Error("unknow packetid. code={0}", packageId);
                        }
                    }
                }
                //  一次网络消息可能会对应多个消息包，因此这里用循环获得消息包
                len = netState.ReceiveBuffer.GetPacketLength();
            } while (len > 0);
        }



        private void OnSocketDisconnect(object sender, SocketDisconnectEventArgs<NetState> e)
        {
            NetState netState = e.Session.Data;
            if (m_netStateManager.GetNetState(netState.Serial) != null)
            {
                //  如果在管理器里有，则说明netstate已经被业务逻辑初始化过
                //  所以需要通知业务逻辑进行处理，否则直接清理数据后退出
                mainTask.AppentdTask(RunTaskNetStateDisconnect, netState);
            }
            else
            {
                netState.ExitWorld();
                netState.Dispose();
            }
        }

        private void RunTaskNetStateDisconnect(NetState netState)
        {
            m_netStateManager.InternalRemoveNetState(netState.Serial);

            //  通知业务逻辑有客户端连接上来可以做一些初始化
            //  或者判断是否允许本次连接
            var tempEV = NetStateDisconnect;
            if (tempEV != null)
            {
                var arg = new NetStateDisconnectEventArgs
                {
                    NetState = netState,
                };
                tempEV(this, arg);
            }

            netState.ExitWorld();
            netState.Dispose();
        }

    /// <summary>
        /// 触发Socket的连接事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSocketConnect(object sender, SocketConnectEventArgs<NetState> e)
        {
            var netState = new NetState(e.Session, this);
            e.Session.Data = netState;

            //  网络连接会涉及到一些业务逻辑操作，因此需要把它加到任务队列里进行处理
            //  如果不考虑业务逻辑的处理，则可以不放到任务队列，节约一下处理时间
            mainTask.AppentdTask(RunTaskNetStateConnect, netState);
        }

        /// <summary>
        /// NetState的id分配器
        /// </summary>
        private volatile int netStateId = 1;

        void RunTaskNetStateConnect(NetState netState)
        {
            //  通知业务逻辑有客户端连接上来可以做一些初始化
            //  或者判断是否允许本次连接
            var tempEV = NetStateConnect;
            if (tempEV != null)
            {
                var arg = new NetStateConnectEventArgs
                {
                    NetState = netState,
                    AllowConnect = true
                };
                tempEV(this, arg);
                if (arg.AllowConnect == false)
                {
                    netState.Dispose();
                    return;
                }
            }
            netState.Serial = netStateId++;

            m_netStateManager.InternalAddNetState(netState.Serial, netState);
            netState.Start();
        }

        /// <summary>
        /// 网络连接事件
        /// </summary>
        public event EventHandler<NetStateConnectEventArgs> NetStateConnect;

        /// <summary>
        /// 网络连接关闭事件
        /// </summary>
        public event EventHandler<NetStateDisconnectEventArgs> NetStateDisconnect;

        private readonly NetStateManager m_netStateManager = new NetStateManager();

        readonly PacketHandlersBase PacketHandlersManger = new PacketHandlersBase();

        /// <summary>
        /// 包句柄管理器
        /// </summary>
        public PacketHandlersBase PacketHandlers
        {
            get { return PacketHandlersManger; }
        }

        private readonly TaskManager mainTask = new TaskManager("mainLogicTask");

        /// <summary>
        /// 对外公开的任务管理器
        /// </summary>
        public TaskManager MainTask
        {
            get { return mainTask; }
        }

        private readonly TaskManager lowTask = new TaskManager("lowLogicTask");

        /// <summary>
        /// 低级别的任务
        /// </summary>
        public TaskManager LowTask
        {
            get { return lowTask; }
        }

        private readonly TaskManager assistTask = new TaskManager("assistLogicTask");

        /// <summary>
        /// 辅助的任务队列
        /// </summary>
        public TaskManager AssistTask
        {
            get { return assistTask; }
        }

        private bool _useManyTaskThread;


        /// <summary>
        /// 是否使用多线程（3个）来处理任务
        /// false 的话只会存在一个线程队列
        /// true 会有3个线程队列
        /// 默认是 false
        /// </summary>
        public bool UseManyTaskThread
        {
            get { return _useManyTaskThread; }
            set { _useManyTaskThread = value; }
        }

        private readonly LogicModuleManager logicModuleManager = new LogicModuleManager();

        /// <summary>
        /// 获得逻辑模块
        /// </summary>
        /// <returns></returns>
        public ILogicModule[] GetModules()
        {
            return logicModuleManager.GetModules();
        }
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
        /// 是否允许连接
        /// </summary>
        public bool AllowConnect{get;set;}
    }
}
