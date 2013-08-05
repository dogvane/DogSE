
namespace DogSE.Library.Log
{
    /// <summary>
    /// 日志的输出等级
    /// </summary>
    public enum LogMessageType
    {
        /// <summary>
        /// 空
        /// </summary>
        MSG_NONE,

        /// <summary>
        /// 状态
        /// </summary>
        MSG_STATUS,

        /// <summary>
        /// sql 日志
        /// </summary>
        MSG_SQL,

        /// <summary>
        /// 调试
        /// </summary>
        MSG_DEBUG,

        /// <summary>
        /// 信息
        /// </summary>
        MSG_INFO,

        /// <summary>
        /// 通知
        /// </summary>
        MSG_NOTICE,

        /// <summary>
        /// 警告
        /// </summary>
        MSG_WARNING,

        /// <summary>
        /// 错误
        /// </summary>
        MSG_ERROR,

        /// <summary>
        /// 致命错误
        /// </summary>
        MSG_FATALERROR,

        /// <summary>
        /// ??
        /// </summary>
        MSG_HACK,

        /// <summary>
        /// 初始化
        /// </summary>
        MSG_LOAD,

        /// <summary>
        /// 输入
        /// </summary>
        MSG_INPUT,

        /// <summary>
        /// Dos窗口输出
        /// </summary>
        MSG_DOS_PROMPT
    }
}