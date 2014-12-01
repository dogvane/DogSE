using System;
using System.Collections.Generic;
using System.IO;

namespace DogSE.Library.Log
{
    /// <summary>
    /// 文件输出的适配器
    /// </summary>
    public class FileAppender:ILogAppender
    {
        private FileAppender()
        {
            
        }

        #region 日志输出

        /// <summary>
        /// 当前需要处理的集合
        /// </summary>
        private readonly Queue<LogInfo> logFileInfoQueue = new Queue<LogInfo>();

        private static object s_lockLogFileInfoQueue = new object();

        private volatile bool isLockFile;

        /// <summary>
        /// 日志输出
        /// </summary>
        /// <param name="info"></param>
        public void Write(LogInfo info)
        {
            //  日志过滤
            if (info.MessageFlag < level || info.MessageFlag > LogMessageType.MSG_FATALERROR)
                return;

            bool isLock = false;

            lock (s_lockLogFileInfoQueue)
            {
                logFileInfoQueue.Enqueue(info);

                // 检测是否有其它的线程已在处理中，如在使用就退出,否则开始锁定
                if (isLockFile == false)
                    isLock = isLockFile = true;
            }

            // 如果有其它的线程在处理就退出
            if (isLock == false)
                return;

            LogInfo[] logInfoArray;
            do
            {
                logInfoArray = null;

                lock (s_lockLogFileInfoQueue)
                {
                    if (logFileInfoQueue.Count > 0)
                    {
                        logInfoArray = logFileInfoQueue.ToArray();
                        logFileInfoQueue.Clear();
                    }
                    else
                        isLockFile = false; // 没有数据需要处理,释放锁定让其它的程序来继续处理
                }

                if (logInfoArray == null)
                    break;

                using (StreamWriter writer = File.AppendText(fileName))
                {
                    for (int iIndex = 0; iIndex < logInfoArray.Length; iIndex++)
                    {
                        LogInfo logInfo = logInfoArray[iIndex];

                        writer.WriteLine(logInfo.ToString());
                    }
                }
            } while (true);
        } 

        #endregion

        #region 日志等级(默认等级为 Notifce)

        /// <summary>
        /// 日志等级
        /// </summary>
        private LogMessageType level = LogMessageType.MSG_NOTICE;

        /// <summary>
        /// 日志等级(默认等级为 Notifce)
        /// </summary>
        public LogMessageType Level
        {
            get { return level; }
            set
            {
                level = value;

                //  理论上来说None表示不对日志做输出
                if (level == LogMessageType.MSG_NONE)
                    level = LogMessageType.MSG_DOS_PROMPT;
            }
        } 

        #endregion

        #region 获得一个文件输出的Appender

        private string fileName;

        private static readonly Dictionary<string, FileAppender> s_appenderMap = new Dictionary<string, FileAppender>();


        /// <summary>
        /// 获得一个文件输出的Appender
        /// </summary>
        /// <remarks>
        /// 这里不允许直接创建的原因是为了防止多个Appender对同一个文件做访问
        /// 在初始化的时候，必须先提供一个日志文件，内部会检查这个日志文件的Appender是否已经创建过
        /// 如果创建过则返回已经创建过的对象重用
        /// </remarks>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static FileAppender GetAppender(string fileName)
        {
            if (s_appenderMap.ContainsKey(fileName))
                return s_appenderMap[fileName];

            var ret = new FileAppender();
            s_appenderMap[fileName] = ret;

            ret.fileName = fileName;

            var dir = new FileInfo(fileName).Directory;
            if (dir == null)
                return null;

            if (!dir.Exists)
            {
                try
                {
                    dir.Create();
                }
                catch (Exception ex)
                {
                    Logs.Error(ex.ToString());
                    return null;
                }
            }

            return ret;
        } 

        #endregion
    }
}