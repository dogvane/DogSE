using System;
using System.Collections.Generic;

namespace DogSE.Library.Log
{
    /// <summary>
    /// 默认的日志处理对象
    /// </summary>
    class DefultLog:ILog
    {
        public DefultLog()
        {
            Level = LogMessageType.MSG_DEBUG;
        }

        public void Debug(string str)
        {
            if (Level > LogMessageType.MSG_DEBUG)
            {
                Write("Debug", str);
            }
        }

        public void Debug(string format, params object[] param)
        {
            if (Level > LogMessageType.MSG_DEBUG)
            {
                Write("Debug", string.Format(format, param));
            }
        }

        public void Info(string str)
        {
            if (Level > LogMessageType.MSG_INFO)
            {
                Write("Info", str);
            }
        }

        public void Info(string format, params object[] param)
        {
            if (Level > LogMessageType.MSG_INFO)
            {
                Write("Info", string.Format(format, param));
            }
        }

        public void Warn(string str)
        {
            if (Level > LogMessageType.MSG_WARNING)
            {
                Write("Warn", str);
            }
        }

        public void Warn(string format, params object[] param)
        {
            if (Level > LogMessageType.MSG_WARNING)
            {
                Write("Warn", string.Format(format, param));
            }
        }

        public void Error(string str)
        {
            if (Level > LogMessageType.MSG_ERROR)
            {
                Write("Error", str);
            }
        }

        public void Error(string format, params object[] param)
        {
            if (Level > LogMessageType.MSG_ERROR)
            {
                Write("Error", string.Format(format, param));
            }
        }

        public void Error(string str, Exception ex)
        {
            if (Level > LogMessageType.MSG_ERROR)
            {
                if (Level > LogMessageType.MSG_ERROR)
                {
                    Write("Error", string.Format("{0}\r\n[Exception]{1}", str, ex));
                }
            }
        }

        public void Error(string str,string param1, Exception ex)
        {
            if (Level > LogMessageType.MSG_ERROR)
            {
                if (Level > LogMessageType.MSG_ERROR)
                {
                    Write("Error", string.Format("{0}\r\n[Exception]{1}", str, ex));
                }
            }
        }

        public void Error(string str, string param1, string param2, Exception ex)
        {
            if (Level > LogMessageType.MSG_ERROR)
            {
                if (Level > LogMessageType.MSG_ERROR)
                {
                    Write("Error", string.Format("{0}\r\n[Exception]{1}", str, ex));
                }
            }
        }

        public void Error(string str, string param1, string param2, string param3, Exception ex)
        {
            if (Level > LogMessageType.MSG_ERROR)
            {
                if (Level > LogMessageType.MSG_ERROR)
                {
                    Write("Error", string.Format("{0}\r\n[Exception]{1}", str, ex));
                }
            }
        }


        /// <summary>
        /// 日志输出等级
        /// </summary>
        public LogMessageType Level
        {
            get;
            set;
        }

        void Write(string type, string msg)
        {
            Write(string.Format("{0} [{1}]:{2}", DateTime.Now.ToString("YYYY-MM-dd HH:mm:ss.ttt"), type, msg));
        }

        void Write(string msg)
        {

        }

        private readonly List<ILogAppender> logAppender = new List<ILogAppender>();

        /// <summary>
        /// 添加一个日志输出的适配器
        /// </summary>
        /// <param name="appender"></param>
        internal void AddAppender(ILogAppender appender)
        {
            if (appender == null)
                throw new NullReferenceException("appender");

            logAppender.Add(appender);
        }
    }
}
