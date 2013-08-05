using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using DogSE.Library.Log;

namespace DogSE.Library.Util
{
    /*
    /// 游戏内的消息码通常使用枚举进行定义
    /// 在游戏里如果想方便的获得消息码对应的名称和描述
    /// 可以继承本对象
    /// 例如：
    ///     public enum OpCode 
    ///     {
    ///// <summary>
    ///// 消息码的名字和描述获取辅助类
    ///// </summary>
    ///         Code = 1,
    ///     }
    /// 只需要做一个继承
    ///     public class OpCodeName:OpCodeNameUtil&lt;OpCode&gt;
    ///     {
    ///     }
    /// 就可以在继承类的静态方法里获得消息码对应的名称和描述
    /// 需要注意的地方，如果要想获得消息码的描述，需要消息码项目的xml输出文档
    /// 具体使用可以看单元测试用例
    */
        /// <summary>
        /// 消息码的名字和描述获取辅助类
        /// </summary>
        /// <remarks>
        /// </remarks>
        public class OpCodeNameUtil<T>
        {

            /// <summary>
            /// 
            /// </summary>
            private static readonly Dictionary<int, string> s_opCodeName = new Dictionary<int, string>();

            /// <summary>
            /// 获得消息码的名字
            /// </summary>
            /// <param name="iOpCode"></param>
            public static string GetOpCodeName(int iOpCode)
            {
                var str = Enum.GetName(typeof(T), iOpCode);
                if (string.IsNullOrEmpty(str))
                    return "Unknow";
                return str;
            }

            /// <summary>
            /// 获得消息码的描述
            /// </summary>
            /// <param name="iOpCode"></param>
            public static string GetOpCodeDescription(int iOpCode)
            {
                InitGameOpCodeName();

                if (s_opCodeName.Count > 0)
                {
                    string ret;
                    if (s_opCodeName.TryGetValue(iOpCode, out ret))
                        return ret;
                }

                return GetOpCodeName(iOpCode);
            }

            private static bool isInit;

            /// <summary>
            /// 
            /// </summary>
            static void InitGameOpCodeName()
            {
                if (isInit)
                    return;
                isInit = true;

                XmlDocument xmlDoc = null;

                string xmlFile = typeof(T).Assembly.FullName.Substring(0, typeof(T).Assembly.FullName.IndexOf(',')) + ".xml";
                if (File.Exists(xmlFile))
                {
                    try
                    {
                        xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(File.ReadAllText(xmlFile));
                        if (xmlDoc.DocumentElement == null)
                            xmlDoc = null;
                    }
                    catch (Exception ex)
                    {
                        Logs.Error("Load OpCode file fail.", ex);
                        xmlDoc = null;
                    }

                }
                
                foreach(var ev in Enum.GetValues(typeof (T)))
                {
                    var name = Enum.GetName(typeof (T), ev);
                    if (xmlDoc != null)
                    {
                        var key = string.Format("//member[@name='F:{0}.{1}']/summary", typeof(T).FullName, name);
                        var node = xmlDoc.DocumentElement.SelectSingleNode(key);
                        if (node != null)
                            name = node.InnerText.Trim();
                    }
                    s_opCodeName[(int) ev] = name;
                }

            }


        }   // class WorldOpCodeName


}
