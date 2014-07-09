using System.Diagnostics;


namespace DogSE.Library.Log
{

    /// <summary>
    /// 日志输出的适配器，所有需要接管日志输入的都需要实现这个接口
    /// </summary>
    public interface ILogAppender
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
}