using System.Diagnostics;


namespace DogSE.Library.Log
{

    /// <summary>
    /// 日志输出的适配器，所有需要接管日志输入的都需要实现这个接口
    /// </summary>
    interface ILogAppender
    {
        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="info"></param>
        void Write(LogInfo info);

        /// <summary>
        /// 日志等级
        /// </summary>
        LogMessageType Level { get; set; }
    }

    /// <summary>
    /// 系统日志记录
    /// </summary>
    /// <remarks>
    /// 系统日志纪录通常是在调用日志模块发生错误以后调用
    /// 作为最后一道日志纪录的屏障
    /// </remarks>
    internal class SystemLogWirter
    {
        /// <summary>
        /// 事件源名称
        /// </summary>
        private string eventSourceName;

        public SystemLogWirter()
        {
            eventSourceName = "MMOSE";
        }

        /// <summary>
        /// 消息事件源名称
        /// </summary>
        public string EventSourceName
        {
            set { eventSourceName = value; }
        }

        /// <summary>
        /// 写入系统日志
        /// </summary>
        /// <param name="message">事件内容</param>
        public void LogEvent(string message)
        {
            if (!EventLog.SourceExists(eventSourceName))
            {
                EventLog.CreateEventSource(eventSourceName, "Application");
            }
            EventLog.WriteEntry(eventSourceName, message, EventLogEntryType.Error);
        }
    }

}