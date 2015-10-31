using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using DogSE.Client.Core.Timer;
using DogSE.Library.Log;

namespace DogSE.Client.Core
{
    /// <summary>
    ///  服务器状态类型
    /// </summary>
    public enum ServerStateType
    {
        /// <summary>
        /// 启动中
        /// </summary>
        Starting = 0,

        /// <summary>
        /// 运行中
        /// </summary>
        Runing = 1,

        /// <summary>
        /// 关闭中
        /// </summary>
        Closing = 2,
    }

    /// <summary>
    ///     游戏服务器服务
    ///     这个是整个游戏的入口事件以及基本操作处理流程
    /// </summary>
    public static class GameServerService
    {

        /// <summary>
        /// 服务器是否处于关闭状态
        /// </summary>
        public static ServerStateType RunType { get; set; }

        /// <summary>
        /// 因为在u3d模式下，任务队列的处理线程是不会启动的
        /// 所以，如果是非u3d模式，则需要在这里启动任务线程
        /// </summary>
        public static void StartTaskThread()
        {
            NetController.TaskManager.StartThread();
        }
}
}