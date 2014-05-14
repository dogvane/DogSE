using System;
using System.Collections.Generic;

namespace DogSE.Library.Log
{
    /// <summary>
    /// 线程安全的日志
    /// </summary>
    public static class Logs
    {
        /// <summary>
        /// 初始化日志配置文件
        /// </summary>
        /// <param name="logFile"></param>
        /// <param name="msgType"></param>
        public static void ConfigLogFile(string logFile, LogMessageType msgType = LogMessageType.MSG_INFO)
        {
            var append = FileAppender.GetAppender(logFile);
            append.Level = msgType;
            AddAppender(append);
        }

        #region 新的操作接口

        /// <summary>
        /// 输出Debug信息
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message)
        {
            WriteLine(LogMessageType.MSG_DEBUG, message);
        }

        /// <summary>
        /// 输出Debug信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="param"></param>
        public static void Debug(string format, params object[] param)
        {
            WriteLine(LogMessageType.MSG_DEBUG, format, param);
        }

        /// <summary>
        /// 输出Info信息
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            WriteLine(LogMessageType.MSG_INFO, message);
        }

        /// <summary>
        /// 输出Info信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="param"></param>
        public static void Info(string format, params object[] param)
        {
            WriteLine(LogMessageType.MSG_INFO, format, param);
        }

        /// <summary>
        /// 输出Warn信息
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string message)
        {
            WriteLine(LogMessageType.MSG_WARNING, message);
        }

        /// <summary>
        /// 输出Warn信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="param"></param>
        public static void Warn(string format, params object[] param)
        {
            WriteLine(LogMessageType.MSG_WARNING, format, param);
        }

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            WriteLine(LogMessageType.MSG_ERROR, message);
        }

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="param"></param>
        public static void Error(string format, string param)
        {
            WriteLine(LogMessageType.MSG_ERROR, format, param);
        }

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="param"></param>
        public static void Error(string format, int param)
        {
            WriteLine(LogMessageType.MSG_ERROR, format, param);
        }

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Error(string message, Exception ex)
        {
            WriteLine(LogMessageType.MSG_ERROR, string.Format("{0}\r\n[Exception]:{1}", message, ex));
        }

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="param2"></param>
        /// <param name="param1"></param>
        public static void Error(string message, string param1, string param2)
        {
            WriteLine(LogMessageType.MSG_ERROR,
                      message, new object[] {param1, param2});
        }

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="param1"></param>
        /// <param name="ex"></param>
        public static void Error(string message, string param1, Exception ex)
        {
            WriteLine(LogMessageType.MSG_ERROR,
                      message + "\r\n[Exception]:{1}", new object[] { param1, ex });
        }

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        public static void Error(string message, string param1, string param2, Exception ex)
        {
            WriteLine(LogMessageType.MSG_ERROR,
                      message + "\r\n[Exception]:{2}", new object[] { param1, param2, ex });
        }

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        public static void Error(string message, string param1, string param2, string param3, Exception ex)
        {
            WriteLine(LogMessageType.MSG_ERROR,
                      message + "\r\n[Exception]:{3}", new object[] { param1, param2, param3, ex });
        }

        #endregion

        #region zh-CHS 公开的函数 | en Class Public Methods

        private static readonly List<ILogAppender> appenders = new List<ILogAppender>();


        /// <summary>
        /// 防止多线程的问题
        /// </summary>
        /// <param name="messageFlag"></param>
        /// <param name="strFormat"></param>
        private static void WriteLine(LogMessageType messageFlag, string strFormat)
        {
            WriteLine(messageFlag, strFormat, null);
        }

        /// <summary>
        /// 防止多线程的问题
        /// </summary>
        /// <param name="messageFlag"></param>
        /// <param name="strFormat"></param>
        /// <param name="arg"></param>
        public static void WriteLine(LogMessageType messageFlag, string strFormat, params object[] arg)
        {
            if (appenders.Count == 0)
                return;

            var logInfo = new LogInfo(messageFlag, strFormat, arg);
            for (int i = 0; i < appenders.Count; i++)
                appenders[i].Write(logInfo);
        }

        /// <summary>
        /// 添加日志的输出适配器
        /// </summary>
        /// <param name="appender"></param>
        public static void AddAppender(ILogAppender appender)
        {
            if (appender == null)
                throw new ArgumentNullException("appender");
            foreach (var a in appenders)
            {
                if (a.GetType() == appender.GetType())
                    return;
            }

            appenders.Add(appender);
        }

        /// <summary>
        /// 删除某个日志输出适配器
        /// </summary>
        /// <param name="appender"></param>
        public static void RemoveAppender(ILogAppender appender)
        {
            if (appender == null)
                throw new ArgumentNullException("appender");

            appenders.Remove(appender);
        }

        /// <summary>
        /// 获得某个适配器类型的日志输出等级
        /// </summary>
        /// <returns></returns>
        public static LogMessageType GetMessageType<TLogAppender>() where TLogAppender : ILogAppender
        {
            foreach (ILogAppender app in appenders)
                if (app is TLogAppender)
                    return app.Level;

            return LogMessageType.MSG_NONE;
        }

        /// <summary>
        /// 设置某个适配器的日志输出等级
        /// </summary>
        /// <typeparam name="TLogAppender"></typeparam>
        /// <param name="level"></param>
        public static void SetMessageLevel<TLogAppender>(LogMessageType level) where TLogAppender : ILogAppender
        {
            foreach (ILogAppender app in appenders)
                if (app is TLogAppender)
                    app.Level = level;
        }


        #endregion

        /// <summary>
        /// 将一个字符串抓换为日志的输出类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal static LogMessageType ParseLogMessageType(string str)
        {
            string level = str.ToLower();
            string[] names = Enum.GetNames(typeof (LogMessageType));
            foreach (string name in names)
            {
                if (name.ToLower().Contains(level))
                    return (LogMessageType) Enum.Parse(typeof (LogMessageType), name);
            }

            return LogMessageType.MSG_NONE;
        }

        /// <summary>
        /// 添加一个控制台的适配器
        /// </summary>
        public static void AddConsoleAppender()
        {
            AddAppender(new ConsoleAppender());
        }
    }
}
