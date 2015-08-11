using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DogSE.Library.Log;

namespace DogSE.Library.Serialize
{
    /// <summary>
    /// CSV 文件序列化
    /// </summary>
    public static class CSVSerializeUtil
    {

        /// <summary>
        /// 将csv文件反序列化到对象数组里
        /// </summary>
        /// <param name="csvStr"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object CSVDeserialize(this string csvStr, Type type)
        {
            MemoryStream stream = new MemoryStream();
            var bytes = Encoding.UTF8.GetBytes(csvStr);
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;

            return CSVDeserialize(type, stream);
        }

        /// <summary>
        /// 当前正在处理的列
        /// </summary>
        public static int CurrentLine = 0;

        static object CSVDeserialize(Type type, Stream stream)
        {
            var csvReader = new CsvFileReader(stream);
            List<string> head = new List<string>();
            csvReader.ReadRow(head); //  先读取头信息，用来做反射
            ArrayList ret = new ArrayList();

            //  找到csv字段对应的属性
            var pros = new PropertyInfo[head.Count];
            Dictionary<PropertyInfo, ArrayList> arrayMap = new Dictionary<PropertyInfo, ArrayList>();

            for (int i = 0; i < head.Count; i++)
            {
                var p = type.GetPropertyByName(head[i]);
                if (p == null)
                    Logs.Debug("Read csv {0} not find columns {1}", type.Name, head[i]);
                else
                {
                    if (p.PropertyType.IsGenericType)
                    {
                        if (
                            !p.PropertyType.InType(typeof(List<int>), typeof(List<float>), typeof(List<double>), typeof(List<long>),
                                typeof (List<bool>), typeof (List<string>)))
                        {
                            if (!p.PropertyType.IsListEnum())
                                continue;
                        }
                    }
                    if (p.PropertyType.IsArray)
                    {
                        arrayMap[p] = new ArrayList();
                        //Console.WriteLine("array has code {0}", p.GetHashCode());
                    }
                }
                pros[i] = p;
            }

            List<string> data = new List<string>();

            int row = 1;
            while (csvReader.ReadRow(data))
            {
                int count = 0;
                for (var i = 0; i < data.Count; i++)
                {
                    if (string.IsNullOrEmpty(data[i]))
                    {
                        count++;
                    }
                }

                if (count == data.Count)
                {
                    //  这里是忽略所有逗号的情况
                    continue;
                }

                row ++;
                object obj = Activator.CreateInstance(type);
                CurrentLine = row;
                for (int i = 0; i < head.Count; i++)
                {
                    if (i >= data.Count)
                    {
                        //  数据长度不够，就忽略后面的数据
                        Logs.Error(string.Format("Read csv {0} row {1} col {2} is empty", type.Name, row, i));
                        break;
                    }

                    var p = pros[i];
                    if (p == null)
                        continue;

                    if (p.PropertyType.IsGenericType)
                    {
                        if (p.PropertyType.InType(typeof(List<int>), typeof(List<float>), typeof(List<double>), typeof(List<long>),
                            typeof (List<bool>), typeof (List<string>))
                            || p.PropertyType.IsListEnum())
                        {
                            var list = Activator.CreateInstance(p.PropertyType) as IList;
                            var splits = data[i].Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
                            var genericType = p.PropertyType.GetGenericArguments()[0];

                            foreach (var d in splits)
                            {
#if DEBUG
                                list.Add(d.ConvertValue(genericType, head[i]));
#else
                                list.Add(d.ConvertValue(genericType));
#endif
                            }

                            p.SetValue(obj, list, null);
                        }
                    }
                    else if (p.PropertyType.IsArray)
                    {
                        //  p 是数组
#if DEBUG
                        arrayMap[p].Add(data[i].ConvertValue(p.PropertyType.GetElementType(), head[i]));
#else
                        arrayMap[p].Add(data[i].ConvertValue(p.PropertyType.GetElementType()));
#endif
                    }
                    else
                    {
#if DEBUG
                        p.SetValue(obj, data[i].ConvertValue(p.PropertyType, head[i]), null);
#else
                        p.SetValue(obj, data[i].ConvertValue(p.PropertyType), null);
#endif
                    }
                }

                foreach (var a in arrayMap)
                {
                    if (a.Value.Count > 0)
                    {
                        a.Key.SetValue(obj, a.Value.ToArray(a.Key.PropertyType.GetElementType()), null);
                        a.Value.Clear();
                    }
                    else
                    {
                    }
                }

                ret.Add(obj);
            }

            return ret.ToArray(type);
        }


        /// <summary>
        /// 通过别名或者列属性
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        static PropertyInfo GetPropertyByAliases(this Type type, string name)
        {
            foreach (var p in type.GetProperties())
            {
                
                foreach(CSVColumnAttribute c in p.GetCustomAttributes(typeof (CSVColumnAttribute), true) )
                {
                    if (c.Aliases == name)
                    {
                        return p;
                    }
                }
            }
            return null;
        }

        static bool InType(this Type type, params Type[] types)
        {
            foreach (var t in types)
            {
                if (type == t)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 释放时List&lt;枚举&gt;
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static bool IsListEnum(this Type type)
        {
            if (!type.IsGenericType)
                return false;
            if (type.Name.IndexOf("List") != 0)
                return false;

            return type.GetGenericArguments()[0].IsEnum;
        }

        static PropertyInfo GetPropertyByName(this Type type, string name)
        {
            var p = type.GetProperty(name);
            if (p == null)
                return type.GetPropertyByAliases(name);
            return p;
        }

        /// <summary>
        /// CSV 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="csvStr">csv的字符串</param>
        /// <returns></returns>
        public static T[] CSVDeserialize<T>(this string csvStr) where T : class, new()
        {
            return csvStr.CSVDeserialize(typeof (T)) as T[];
        }

        /// <summary>
        /// CSV 反序列一个文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="csvFileName">某个csv的文件</param>
        /// <returns></returns>
        public static T[] CSVDeserializeFile<T>(this string csvFileName) where T : class, new()
        {
            using (Stream fileStream = new FileStream(csvFileName, FileMode.Open, FileAccess.Read))
            {
                return CSVDeserialize(typeof(T), fileStream) as T[];
            }
        }

        static object ConvertValue(this string value, Type type, string paramName)
        {
            try
            {
                return value.ConvertValue(type);
            }
            catch (Exception ex)
            {
                var msg = string.Format("param {0} value fail. value {1}", paramName, value);
                Logs.Error(msg);
                throw new Exception(msg, ex);
            }
        }

        private static object ConvertValue(this string value, Type type)
        {
            if (type == typeof (int))
            {
                if (string.IsNullOrEmpty(value))
                    return 0;

                return Convert.ToInt32(value);
            }


            if (type == typeof (long))
            {
                if (string.IsNullOrEmpty(value))
                    return 0;

                return Convert.ToInt64(value);
            }

            if (type == typeof (string))
            {
                if (string.IsNullOrEmpty(value))
                    return "";
                return value;
            }

            if (type == typeof (float))
            {
                if (string.IsNullOrEmpty(value))
                    return 0.0f;
                return Convert.ToSingle(value);
            }

            if (type == typeof (double))
            {
                if (string.IsNullOrEmpty(value))
                    return 0.0;
                return Convert.ToDouble(value);
            }

            if (type == typeof(float))
            {
                if (string.IsNullOrEmpty(value))
                    return 0.0f;
                return Convert.ToSingle(value);
            }

            if (type == typeof (DateTime))
            {
                if (string.IsNullOrEmpty(value))
                    return new DateTime(0);

                return DateTime.Parse(value);
            }

            if (type == typeof (bool))
            {
                if (string.IsNullOrEmpty(value))
                    return false;

                if (value == "0")
                    return false;

                if (value == "1")
                    return true;

                return Boolean.Parse(value);
            }

            if (type.IsEnum)
            {
                if (string.IsNullOrEmpty(value))
                    return 0;
                return Convert.ToInt32(value);
            }

            Logs.Error("csv unknow read type {0}", type.Name);

            return 0;
        }

        /// <summary>
        /// 序列化到csv字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        public static string CSVSerialize<T>(this T[] values)
        {
            var stream = new MemoryStream();

            var csvWriter = new CsvFileWriter(stream);
            List<string> head = new List<string>();

            //  找到csv字段对应的属性
            var type = typeof(T);
            var types = type.GetProperties();
            List<string> data = new List<string>();
            foreach (var t in types)
            {
                var a = t.GetCustomAttributes(typeof (CSVColumnAttribute), true);
                if (a.Length > 0)
                    data.Add(((CSVColumnAttribute) a[0]).Aliases);
                else
                    data.Add(t.Name);
            }

            csvWriter.WriteRow(data);
            foreach (var v in values)
            {
                data.Clear();
                foreach (var t in types)
                {
                    var wv = t.GetValue(v, null);
                    data.Add(wv == null ? "" : wv.ToString());
                }
                csvWriter.WriteRow(data);
            }

            csvWriter.Flush();
            stream.Position = 0;
            return new StreamReader(stream).ReadToEnd();
        }
    }

    /// <summary>
    /// 给一个csv的列定义一个别名
    /// </summary>
    public class CSVColumnAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">别名</param>
        public CSVColumnAttribute(string name)
        {
            Aliases = name;
        }

        /// <summary>
        /// 别名
        /// </summary>
        public string Aliases { get; set; }
    }
}
