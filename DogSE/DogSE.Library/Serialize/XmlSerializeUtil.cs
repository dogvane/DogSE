using System;
using System.IO;
using System.Xml.Serialization;
using DogSE.Library.Log;

namespace DogSE.Library.Serialize
{
    /// <summary>
    /// xml序列化辅助类
    /// </summary>
    public static class XmlSerializeUtil
    {
        /// <summary>
        /// Xml 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(this string xmlStr) where T:class
        {
            if (String.IsNullOrEmpty(xmlStr)) return null as T;
            var ser = new XmlSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                var sw = new StreamWriter(stream);
                sw.Write(xmlStr);
                sw.Flush();
                stream.Position = 0;
                var obj = ser.Deserialize(stream);
                return obj as T;
            }
        }

        /// <summary>
        /// Xml 反序列化
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object XmlDeserialize(this string xmlStr, Type type)
        {
            try
            {


            var ser = new XmlSerializer(type);
            using (var stream = new MemoryStream())
            {
                var sw = new StreamWriter(stream);
                sw.Write(xmlStr);
                sw.Flush();
                stream.Position = 0;
                var obj = ser.Deserialize(stream);
                return obj;
            }
            }
            catch (Exception ex)
            {
                Logs.Error(string.Format("Deserialize {0} fail.xml={1} ex={2}", type.Name, xmlStr, ex.ToString()));

                throw;
            }
        }

        /// <summary>
        /// 将一个对象序列化为一个xml对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string XmlSerialize<T>(this T obj)
        {
            var ser = new XmlSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                ser.Serialize(stream, obj);
                stream.Position = 0;
                var sr = new StreamReader(stream);
                return sr.ReadToEnd();
            }
        }
    }
}
