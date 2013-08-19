using DogSE.Library.Log;
using DogSE.Library.Time;
using DogSE.Server.Core.Config;
using DogSE.Server.Core.LogicModule;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Protocol;
using DogSE.Server.Core.Task;
using DogSE.Server.Core.Timer;
using DogSE.Server.Net;

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
            OneServer.Closing = false;

            StaticConfigFileManager.LoadData();

            InitLogicModule();

            StartServerSocket();

            TimerThread.TaskManager = taskManager;
            TimerThread.StartTimerThread();

            taskManager.StartThread();
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
            new RegisterNetMethod(PacketHandlersManger).Register(modules);
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

                Listeners[++index] = linster;
                Logs.Info("open socket {0}:{1}", tcp.Host, tcp.Port);
            }
        }

        /// <summary>
        /// 收到网络消息包
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSocketRecv(object sender, SocketRecvEventArgs<NetState> e)
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
                    if (get != len)
                    {
                        var packageReader = new PacketReader(readBuffer);
                        var packetHandler = PacketHandlersManger.GetHandler(packageReader.GetPacketID());
                        if (packetHandler != null)
                        {
                            //  加入网络消息处理
                            taskManager.AppendTask(netState, packetHandler, packageReader);
                        }
                        else
                        {
                            Logs.Error("unknow packetid. code={0}", packageReader.GetPacketID().ToString());
                        }
                    }
                }
                //  一次网络消息可能会对应多个消息包，因此这里用循环获得消息包
                len = netState.ReceiveBuffer.GetPacketLength();
            } while (len > 0);  
        }

        void OnSocketDisconnect(object sender, SocketDisconnectEventArgs<NetState> e)
        {
            NetState netState = e.Session.Data;
            m_netStateManager.InternalRemoveNetState(netState.Serial);
        }

        void OnSocketConnect(object sender, SocketConnectEventArgs<NetState> e)
        {
            var netState = new NetState(e.Session, this);

            m_netStateManager.InternalAddNetState(0, netState);

        }

        private NetStateManager m_netStateManager = new NetStateManager();

        PacketHandlersBase PacketHandlersManger = new PacketHandlersBase();

        private TaskManager taskManager = new TaskManager();

        private LogicModuleManager logicModuleManager = new LogicModuleManager();
    }
}
