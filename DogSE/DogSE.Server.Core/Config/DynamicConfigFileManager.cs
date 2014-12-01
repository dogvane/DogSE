using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DogSE.Library.Component;
using DogSE.Library.Log;
using DogSE.Library.Serialize;
using DogSE.Server.Core.Util;

namespace DogSE.Server.Core.Config
{
    /// <summary>
    /// 动态配置文件管理
    /// 主要是xml格式的配置业务逻辑数据的加载
    /// </summary>
    public static class DynamicConfigFileManager
    {
        private static readonly ComponentManager s_data = new ComponentManager();

        /// <summary>
        /// 获得一个配置数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="componentId"></param>
        /// <returns></returns>
        public static T[] GetConfigData<T>(string componentId) where T : class
        {
            if (string.IsNullOrEmpty(componentId))
                throw new ArgumentNullException("componentId");

            var data = s_data.GetComponent<T[]>(componentId);
            if (data == null)
            {
                Logs.Error("Not find Template {0}", componentId);
                return new T[0];
            }
            return data;
        }

        /// <summary>
        /// 获得一个配置数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] GetConfigData<T>() where T : class
        {
            var type = typeof (T);
            var att = type.GetAttribute<DynamicCSVConfigRootAttribute>();
            if (att == null)
                throw new Exception(string.Format("{0} not has DynamicCSVConfigRootAttribute", type.Name));

            var data = s_data.GetComponent<T[]>(att.ComponentName);
            if (data == null)
            {
                Logs.Error("Not find Template {0}", att.ComponentName);
                return new T[0];
            }
            return data;
        }

        /// <summary>
        /// 初始化配置信息
        /// </summary>
        /// <param name="folder">配置文件的目录，不填写时，采用项目的根目录</param>
        public static void Load(string folder = null)
        {
            LoadXmlConfig(folder);
            LoadCSVConfig(folder);
        }

        /// <summary>
        /// 加载xml的配置文件
        /// </summary>
        /// <param name="folder">配置文件的目录，不填写时，采用项目的根目录</param>
        private static void LoadXmlConfig(string folder = null)
        {
            var configTypes = AssemblyUtil.GetTypesByAttribute(typeof (DynamicXmlConfigRootAttribute));
            foreach (var type in configTypes)
            {
                var rootAttribute =
                    (DynamicXmlConfigRootAttribute) type.GetCustomAttributes(typeof (DynamicXmlConfigRootAttribute), true)[0];
                if (string.IsNullOrEmpty(rootAttribute.FileName))
                {
                    Logs.Error("Dynamic config class:'{0}' not define file", type.Name);
                    continue;
                }

                if (string.IsNullOrEmpty(rootAttribute.ComponentName))
                {
                    Logs.Error("Dynamic config type:{0} component name is empty.", type.Name);
                    continue;
                }

                if (folder == null)
                    folder = AppDomain.CurrentDomain.BaseDirectory;
                var fileName = Path.Combine(folder, rootAttribute.FileName);

                if (!File.Exists(fileName))
                {
                    Logs.Error("Not find dynamic config file:{0} fail.", fileName);
                    continue;
                }

                var xml = File.ReadAllText(fileName);

                try
                {
                    var data = xml.XmlDeserialize(type.MakeArrayType());
                    s_data.RegisterComponent(rootAttribute.ComponentName, data);
                }
                catch (Exception ex)
                {
                    Logs.Error("Deserialize dynamic file fail. type:{0} file:{1}", type.Name, fileName, ex);
                }
            }
        }

        /// <summary>
        /// 加载csv的配置文件
        /// </summary>
        /// <param name="folder">配置文件的目录，不填写时，采用项目的根目录</param>
        private static void LoadCSVConfig(string folder = null)
        {
            var configTypes = AssemblyUtil.GetTypesByAttribute(typeof(DynamicCSVConfigRootAttribute));
            foreach (var type in configTypes)
            {
                var rootAttribute =(DynamicCSVConfigRootAttribute)type.GetCustomAttributes(typeof(DynamicCSVConfigRootAttribute), true)[0];
                if (string.IsNullOrEmpty(rootAttribute.FileName))
                {
                    Logs.Error("Dynamic config class:'{0}' not define file", type.Name);
                    continue;
                }

                if (string.IsNullOrEmpty(rootAttribute.ComponentName))
                {
                    Logs.Error("Dynamic config type:{0} component name is empty.", type.Name);
                    continue;
                }

                if (folder == null)
                    folder = AppDomain.CurrentDomain.BaseDirectory;
                var fileName = Path.Combine(folder, rootAttribute.FileName);

                if (!File.Exists(fileName))
                {
                    Logs.Error("Not find dynamic config file:{0} .", fileName);
                    continue;
                }

                var csv = File.ReadAllText(fileName);

                try
                {
                    var data = csv.CSVDeserialize(type);

                    s_data.RegisterComponent(rootAttribute.ComponentName, data);
                }
                catch (Exception ex)
                {
                    Logs.Error("Deserialize dynamic file fail. type:{0} file:{1}", type.Name, fileName, ex);
                }
            }
        }
    }
}
