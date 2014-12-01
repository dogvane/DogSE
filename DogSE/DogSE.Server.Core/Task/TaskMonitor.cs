using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DogSE.Server.Core.Task
{
    /// <summary>
    /// 任务监控
    /// </summary>
    public struct TaskMonitor
    {
        /// <summary>
        /// 网络任务执行的次数
        /// </summary>
        public int NetTaskCount;

        /// <summary>
        /// 网络任务执行的毫秒数
        /// </summary>
        public long NetTaskRunTicks;

        /// <summary>
        /// 网络任务延迟
        /// </summary>
        public long NetTaskDelayTicks;

        /// <summary>
        /// 网络任务的异常数据
        /// </summary>
        public int NetTaskErrorCount;

        /// <summary>
        /// 普通任务执行次数
        /// </summary>
        public int ActionTaskCount;

        /// <summary>
        /// 普通任务执行的毫秒数
        /// </summary>
        public long ActionTaskRunTicks;

        /// <summary>
        /// 普通任务执行延迟
        /// </summary>
        public long ActionTaskDelayTicks;

        /// <summary>
        /// 普通任务异常数据
        /// </summary>
        public int ActionTaskErrorCount;
    }
}
