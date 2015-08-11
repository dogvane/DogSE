using System;
using System.Collections.Generic;
using System.Text;
using DogSE.Library.Log;

namespace DogSE.Server.Core.Config
{
    /// <summary>
    ///     键值对配置管理
    /// </summary>
    public static class KeyValueConfigManger
    {

        /// <summary>
        /// 将一个数组转为一个字典
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="values"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
        static Dictionary<TKey, TValue> ToMap<TKey, TValue>(this TValue[] values, Func<TValue, TKey> fun)
        {
            Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>(values.Length + 1);
            foreach (var v in values)
            {
                var key = fun(v);
                if (ret.ContainsKey(key))
                {
                    Logs.Error(string.Format("To map has same key {0}", key));
                }
                ret[key] = v;
            }
            return ret;
        }

        private static Dictionary<string, GlobalData> map;

        /// <summary>
        ///     根据Key 获得字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            if (map == null)
                map = DynamicConfigFileManager.GetConfigData<GlobalData>("GlobalData").ToMap(o => o.Key);

            GlobalData item;
            if (map.TryGetValue(key, out item))
            {
                return item.Value;
            }

            return string.Empty;
        }

        /// <summary>
        /// 判断key是否在配置里
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static bool ContainsKey(string key)
        {
            if (map == null)
                map = DynamicConfigFileManager.GetConfigData<GlobalData>("GlobalData").ToMap(o => o.Key);

            return map.ContainsKey(key);
        }

        /// <summary>
        /// 更新模板数据
        /// </summary>
        public static void UpdateTemplate()
        {
            s_items.ForEach(o => o.ClearAndUpdate());
        }

        private static readonly List<KeyValueItem> s_items = new List<KeyValueItem>();

        /// <summary>
        /// 添加一个更新项
        /// </summary>
        /// <param name="item"></param>
        static internal void AddUpdateItem(KeyValueItem item)
        {
            s_items.Add(item);
        }

        /// <summary>
        /// 获得key的列表
        /// </summary>
        /// <returns></returns>
        public static string GetKeyList()
        {
            StringBuilder sb = new StringBuilder();
            s_items.ForEach(o => sb.AppendLine(o.Key));
            return sb.ToString();
        }

    }

    /// <summary>
    ///     键值项
    /// </summary>
    public class KeyValueItem
    {
        private readonly KeyValueType type;

        /// <summary>
        /// 清理数据并更新数据
        /// </summary>
        public void ClearAndUpdate()
        {
            switch (type)
            {
                case KeyValueType.Int:
                    GetInt();
                    break;
                case KeyValueType.Double:
                    GetDouble();
                    break;
                case KeyValueType.String:
                    GetString();
                    break;
                case KeyValueType.IntArray:
                    GetIntArray();
                    break;
            }
        }

        #region Int

        private readonly int defaultIntValue;
        private int intValue;
        private bool isInitInt;

        /// <summary>
        ///     创建一个int类型的数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        public KeyValueItem(string key, int defaultValue)
        {
            KeyValueConfigManger.AddUpdateItem(this);
            defaultIntValue = defaultValue;
            Key = key;
            type = KeyValueType.Int;
        }

        /// <summary>
        ///     获得Int数据
        /// </summary>
        /// <returns></returns>
        public int GetInt()
        {
            if (type != KeyValueType.Int)
            {
                throw new Exception(string.Format("global {0} not int type", Key));
            }

            if (isInitInt)
                return intValue;

            string value = KeyValueConfigManger.GetValue(Key);
            if (string.IsNullOrEmpty(value))
            {
                Logs.Warn(string.Format("Global item {0} not default.", Key));
                intValue = defaultIntValue;
            }
            else if (!int.TryParse(value, out intValue))
            {
                Logs.Error(string.Format("Global item {0}:{1} parse to int fail.", Key, value));
                intValue = defaultIntValue;
            }
            isInitInt = true;

            return intValue;
        }

        #endregion

        #region Double

        /// <summary>
        ///     创建一个double类型的数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        public KeyValueItem(string key, double defaultValue)
        {
            KeyValueConfigManger.AddUpdateItem(this);

            defaultDoubleValue = defaultValue;
            Key = key;
            type = KeyValueType.Double;
        }

        private double doubleValue;
        private bool isInitDouble;
        private readonly double defaultDoubleValue;

        /// <summary>
        ///     获得Double数据
        /// </summary>
        /// <returns></returns>
        public double GetDouble()
        {
            if (type != KeyValueType.Double)
            {
                throw new Exception(string.Format("global {0} not double type", Key));
            }

            if (isInitDouble)
                return doubleValue;

            string value = KeyValueConfigManger.GetValue(Key);
            if (string.IsNullOrEmpty(value))
            {
                Logs.Warn(string.Format("Global item {0} not default.", Key));
                doubleValue = defaultDoubleValue;
            }
            else if (!double.TryParse(value, out doubleValue))
            {
                Logs.Error(string.Format("Global item {0}:{1} parse to double fail.", Key, value));
                doubleValue = defaultDoubleValue;
            }
            isInitDouble = true;

            return intValue;
        }

        #endregion

        #region String

        /// <summary>
        ///     创建一个string类型的数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        public KeyValueItem(string key, string defaultValue)
        {
            KeyValueConfigManger.AddUpdateItem(this);

            defaultStringValue = defaultValue;
            Key = key;
            type = KeyValueType.String;
        }


        private string stringValue;
        private bool isInitString;
        private string defaultStringValue = String.Empty;


        /// <summary>
        ///     获得Double数据
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            if (type != KeyValueType.String)
            {
                throw new Exception(string.Format("global {0} not string type", Key));
            }

            if (isInitString)
                return stringValue;

            if (!KeyValueConfigManger.ContainsKey(Key))
            {
                Logs.Warn(string.Format("Global item {0} not default.", Key));
                stringValue = defaultStringValue;
            }
            else
            {
                stringValue =  KeyValueConfigManger.GetValue(Key);
            }

            isInitString = true;

            return stringValue;
        }

        #endregion

        #region int[]

        /// <summary>
        ///     创建一个int[] 类型的数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        public KeyValueItem(string key, int[] defaultValue)
        {
            KeyValueConfigManger.AddUpdateItem(this);

            defaultIntArray = defaultValue;
            Key = key;
            type = KeyValueType.IntArray;
        }

        private readonly int[] defaultIntArray;
        private int[] intArray;
        private bool isInitArray;


        /// <summary>
        ///     获得Int数据
        /// </summary>
        /// <returns></returns>
        public int[] GetIntArray()
        {
            if (type != KeyValueType.IntArray)
            {
                throw new Exception(string.Format("global {0} not int[] type", Key));
            }

            if (isInitArray)
                return intArray;

            string value = KeyValueConfigManger.GetValue(Key);
            if (string.IsNullOrEmpty(value))
            {
                Logs.Warn(string.Format("Global item {0} not default.", Key));
                intArray = defaultIntArray;
            }
            else
            {
                List<int> arr = new List<int>();
                foreach (var str in value.Split(',', ';'))
                {
                    if (string.IsNullOrEmpty(str))
                        continue;

                    int i;
                    if (int.TryParse(str, out i))
                        arr.Add(i);
                    else
                        Logs.Error(string.Format("Global item {0}:{1}-{2} parse to int fail.", Key, value, str));
                }
                intArray = arr.ToArray();
            }

            isInitArray = true;

            return intArray;
        }

        #endregion

        #region double[]

        /// <summary>
        ///     创建一个Double[] 类型的数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        public KeyValueItem(string key, double[] defaultValue)
        {
            KeyValueConfigManger.AddUpdateItem(this);

            defaultDoubleArray = defaultValue;
            Key = key;
            type = KeyValueType.DoubleArray;
        }

        private readonly double[] defaultDoubleArray;
        private double[] doubleArray;
        private bool isInitDoubleArray;


        /// <summary>
        ///     获得Double数据
        /// </summary>
        /// <returns></returns>
        public double[] GetDoubleArray()
        {
            if (type != KeyValueType.IntArray)
            {
                throw new Exception(string.Format("global {0} not dboule[] type", Key));
            }

            if (isInitDoubleArray)
                return doubleArray;

            string value = KeyValueConfigManger.GetValue(Key);
            if (string.IsNullOrEmpty(value))
            {
                Logs.Warn(string.Format("Global item {0} not default.", Key));
                doubleArray = defaultDoubleArray;
            }
            else
            {
                List<double> arr = new List<double>();
                foreach (var str in value.Split(',', ';'))
                {
                    if (string.IsNullOrEmpty(str))
                        continue;

                    double i;
                    if (double.TryParse(str, out i))
                        arr.Add(i);
                    else
                        Logs.Error(string.Format("Global item {0}:{1}-{2} parse to dboule fail.", Key, value, str));
                }
                doubleArray = arr.ToArray();
            }

            isInitDoubleArray = true;

            return doubleArray;
        }

        #endregion

        #region string[]

        /// <summary>
        ///     创建一个int[] 类型的数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        public KeyValueItem(string key, string[] defaultValue)
        {
            KeyValueConfigManger.AddUpdateItem(this);

            defaultStringArray = defaultValue;
            Key = key;
            type = KeyValueType.StringArray;
        }

        private readonly string[] defaultStringArray;
        private string[] stringArray;
        private bool isInitStringArray;


        /// <summary>
        ///     获得Int数据
        /// </summary>
        /// <returns></returns>
        public string[] GetStringArray()
        {
            if (type != KeyValueType.StringArray)
            {
                throw new Exception(string.Format("global {0} not string[] type", Key));
            }

            if (isInitStringArray)
                return stringArray;

            string value = KeyValueConfigManger.GetValue(Key);
            if (string.IsNullOrEmpty(value))
            {
                Logs.Warn(string.Format("Global item {0} not default.", Key));
                stringArray = defaultStringArray;
            }
            else
            {
                List<string> arr = new List<string>();
                foreach (var str in value.Split(',', ';'))
                {
                    if (string.IsNullOrEmpty(str))
                        continue;

                    arr.Add(str);
                }
                stringArray = arr.ToArray();
            }

            isInitStringArray = true;

            return stringArray;
        }

        #endregion

        /// <summary>
        ///     键值项
        /// </summary>
        public string Key { get; private set; }



        private enum KeyValueType
        {
            Int = 0,
            Double = 1,
            String = 2,
            IntArray = 3,
            StringArray = 4,
            DoubleArray = 5,
        }
    }

    /// <summary>
    ///     全局数据
    /// </summary>
    [DynamicCSVConfigRoot(@"..\ConfigData\GlobalData.csv", "GlobalData")]
    public class GlobalData
    {
        /// <summary>
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// </summary>
        public string Value { get; set; }
    }
}