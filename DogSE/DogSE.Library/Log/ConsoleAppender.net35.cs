using System;
using System.Collections.Generic;
using System.Text;

namespace DogSE.Library.Log
{
    /// <summary>
    /// 控制台日志输出
    /// </summary>
    /// <remarks>
    /// 虽然这里实现的是实例的接口
    /// 但控制台只有一个，因此这边的数据都是写入一个静态的列表
    /// </remarks>
    public class ConsoleAppender : ILogAppender
    {
        /// <summary>
        /// 当前需要处理的集合
        /// </summary>
        private static readonly Queue<LogInfo> s_LogInfoQueue = new Queue<LogInfo>();

        /// <summary>
        /// 日志操作的锁
        /// </summary>
        private static object s_LockLogInfoQueue = new object();

        /// <summary>
        /// 
        /// </summary>
        private static volatile bool s_IsLock;

        #region IAppender 成员

        /// <summary>
        /// 写日志（可以多线程操作）
        /// </summary>
        /// <param name="info"></param>
        public void Write(LogInfo info)
        {
            //  日志过滤
            if (info.MessageFlag < level)
                return;

            bool bIsLock = false;

            lock (s_LockLogInfoQueue)
            {
                s_LogInfoQueue.Enqueue(info);

                // 检测是否有其它的线程已在处理中，如在使用就退出，否则开始锁定
                if (s_IsLock == false)
                    bIsLock = s_IsLock = true;
            }


            // 如果有其它的线程在处理就退出
            if (bIsLock == false)
                return;

            LogInfo[] logInfoArray;

            do
            {
                logInfoArray = null;

                    lock (s_LockLogInfoQueue)
                    {
                        if (s_LogInfoQueue.Count > 0)
                        {
                            logInfoArray = s_LogInfoQueue.ToArray();
                            s_LogInfoQueue.Clear();
                        }
                        else
                            s_IsLock = false; // 没有数据需要处理,释放锁定让其它的程序来继续处理
                    }

                if (logInfoArray == null)
                    break;

                for (int iIndex = 0; iIndex < logInfoArray.Length; iIndex++)
                {
                    LogInfo logInfo = logInfoArray[iIndex];

                    if (logInfo.Parameter == null)
                        InternalWriteLine(logInfo.MessageFlag, logInfo.Format);
                    else
                        InternalWriteLine(logInfo.MessageFlag, logInfo.Format, logInfo.Parameter);
                }

            } while (true);    
        }

        #endregion

        /// <summary>
        /// 日志等级
        /// </summary>
        private LogMessageType level = LogMessageType.MSG_INFO;

        /// <summary>
        /// 日志等级
        /// </summary>
        public LogMessageType Level
        {
            get { return level; }
            set { level = value; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static string s_strInput = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        private static string s_strDosPrompt = string.Empty;

        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageFlag"></param>
        /// <param name="strFormat"></param>
        static void InternalWriteLine(LogMessageType messageFlag, string strFormat)
        {
            Console.Write("\r");

            switch (messageFlag)
            {
                case LogMessageType.MSG_NONE: // direct printf replacement
                    Console.Write("[NONE]: ");
                    break;
                case LogMessageType.MSG_STATUS:
                    Console.Write("[STATUS]: ");
                    break;
                case LogMessageType.MSG_SQL:
                    Console.Write("[SQL]: ");
                    break;
                case LogMessageType.MSG_INFO:
                    Console.Write("[INFO]: ");

                    break;
                case LogMessageType.MSG_NOTICE:
                    Console.Write("[NOTICE]: ");

                    break;
                case LogMessageType.MSG_WARNING:
                    Console.Write("[WARNING]: ");

                    break;
                case LogMessageType.MSG_DEBUG:
                    Console.Write("[DEBUG]: ");

                    break;
                case LogMessageType.MSG_ERROR:
                    Console.Write("[ERROR]: ");

                    break;
                case LogMessageType.MSG_FATALERROR:
                    Console.Write("[FATAL ERROR]: ");

                    break;
                case LogMessageType.MSG_HACK:
                    Console.Write("[HACK]: ");

                    break;
                case LogMessageType.MSG_LOAD:
                    Console.Write("[LOAD]: ");

                    break;
                case LogMessageType.MSG_DOS_PROMPT:
                    s_strDosPrompt = strFormat;

                    break;
                case LogMessageType.MSG_INPUT:
                    if (s_strDosPrompt != string.Empty)
                    {
                        Console.Write(s_strDosPrompt);
                    }

                    s_strInput = strFormat;
                    break;
            }

            StringBuilder strStringBuilder = new StringBuilder("");

            if (messageFlag != LogMessageType.MSG_DOS_PROMPT)
            {
                int iBlankLength = (s_strDosPrompt.Length + s_strInput.Length) - strFormat.Length;
                for (int iIndex = 0; iIndex < iBlankLength; iIndex++)
                    strStringBuilder.Append(" ");
                for (int iIndex = 0; iIndex < iBlankLength; iIndex++)
                    strStringBuilder.Append("\b");
            }

            Console.Write(strFormat + strStringBuilder);

            if (messageFlag == LogMessageType.MSG_LOAD)
            {
                // none
            }
            else if (messageFlag == LogMessageType.MSG_INPUT)
            {
                // none
            }
            else if (messageFlag == LogMessageType.MSG_DOS_PROMPT)
            {
                if (s_strInput != string.Empty)
                {
                    Console.Write(s_strInput);
                }
            }
            else
            {
                Console.WriteLine(" ");

                if (s_strDosPrompt != string.Empty)
                {
                    Console.Write(s_strDosPrompt);
                }

                if (s_strInput != string.Empty)
                {
                    Console.Write(s_strInput);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageFlag"></param>
        /// <param name="strFormat"></param>
        /// <param name="arg"></param>
        static void InternalWriteLine(LogMessageType messageFlag, string strFormat, params object[] arg)
        {
            InternalWriteLine(messageFlag, string.Format(strFormat, arg));
        }

    }
}