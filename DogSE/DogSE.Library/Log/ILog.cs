using System;

namespace DogSE.Library.Log
{
    /// <summary>
    /// 日志组件的接口
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// 输出Debug信息
        /// </summary>
        /// <param name="message"></param>
        void Debug(string message);

        /// <summary>
        /// 输出Debug信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="param"></param>
        void Debug(string format, params object[] param);

        /// <summary>
        /// 输出Info信息
        /// </summary>
        /// <param name="message"></param>
        void Info(string message);

        /// <summary>
        /// 输出Info信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="param"></param>
        void Info(string format, params object[] param);

        /// <summary>
        /// 输出Warn信息
        /// </summary>
        /// <param name="message"></param>
        void Warn(string message);

        /// <summary>
        /// 输出Warn信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="param"></param>
        void Warn(string format, params object[] param);

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="message"></param>
        void Error(string message);

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="param"></param>
        void Error(string format, params object[] param);

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ex"></param>
        void Error(string str, Exception ex);

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ex"></param>
        /// <param name="param1"></param>
        void Error(string str, string param1, Exception ex);

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ex"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        void Error(string str, string param1, string param2, Exception ex);

        /// <summary>
        /// 输出Error信息
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ex"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        void Error(string str, string param1, string param2, string param3, Exception ex);


        /// <summary>
        /// 日志记录等级
        /// </summary>
        LogMessageType Level { get; set; }
    }

}
