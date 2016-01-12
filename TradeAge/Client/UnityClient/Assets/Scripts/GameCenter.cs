using DogSE.Client.Core;
using DogSE.Client.Core.Task;
using DogSE.Library.Log;
using TradeAge.Client.Controller;

namespace Assets.Scripts
{
    /// <summary>
    /// 游戏的逻辑处理中心
    /// </summary>
    public static class GameCenter
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            Controller.Net.StartWorld();
        }

        /// <summary>
        /// 退出时释放数据
        /// </summary>
        public static void Release()
        {
            Logs.Info("OnDisable");
            Controller.Net.StopWorld();
            NetController.CloseThread();
        }

        /// <summary>
        /// 是否连接服务器
        /// </summary>
        public static bool IsConnectServer = false;


        static readonly GameController s_controller = new GameController();
  
        /// <summary>
        /// 游戏总的控制器
        /// </summary>
        public static GameController Controller
        {
            get
            {
                return s_controller;
            }
        }

        /// <summary>
        /// 任务管理器，可以当做主线程的异步处理管理器
        /// </summary>
        public static Unity3DTaskManager TaskManager
        {
            get { return NetController.TaskManager; }
        }
    }
}