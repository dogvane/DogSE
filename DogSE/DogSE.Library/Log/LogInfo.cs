using System;

namespace DogSE.Library.Log
{
    /// <summary>
    /// 日志信息
    /// </summary>
    public struct LogInfo
    {
        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose


        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageFlag"></param>
        /// <param name="strFormat"></param>
        /// <param name="parameter"></param>
        public LogInfo(LogMessageType messageFlag, string strFormat,object[] parameter = null)
        {
            m_messageFlag = messageFlag;
            format = strFormat;
            _parameter = parameter;
            outStr = null;
            time = DateTime.Now;
        }

        #endregion

        #region zh-CHS 属性 | en Properties
        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private readonly LogMessageType m_messageFlag;
        #endregion
        /// <summary>
        /// 日志标记
        /// </summary>
        public LogMessageType MessageFlag
        {
            get { return m_messageFlag; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private readonly string format;
        #endregion
        /// <summary>
        /// 日志信息（格式）
        /// </summary>
        public string Format
        {
            get { return format; }
        }

        #region zh-CHS 私有成员变量 | en Private Member Variables
        /// <summary>
        /// 
        /// </summary>
        private readonly object[] _parameter;
        #endregion
        /// <summary>
        /// 如果 format带格式，这里为参数
        /// </summary>
        public object[] Parameter
        {
            get { return _parameter; }
        }

        private readonly DateTime time;

        private string outStr;

        /// <summary>
        /// 输出定义好的格式化字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (outStr == null)
            {
                if (_parameter == null)
                    outStr = string.Format("{0} [{1}] {2}", time.ToString("yyyy-MM-dd HH:mm:ss.fff"), m_messageFlag,
                                           format);
                else
                    outStr = string.Format("{0} [{1}] {2}", time.ToString("yyyy-MM-dd HH:mm:ss.fff"), m_messageFlag,
                                           string.Format(format, _parameter));
            }

            return outStr;
        }
        #endregion
    }
}