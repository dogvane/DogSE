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

            return s_data.GetComponent<T[]>(componentId);
        }

        /// <summary>
        /// 初始化配置信息
        /// </summary>
        internal static void Load()
        {
            var configTypes = AssemblyUtil.GetTypesByAttribute(typeof(DynamicXmlConfigRootAttribute));
            foreach (var type in configTypes)
            {
                var rootAttribute = (DynamicXmlConfigRootAttribute)type.GetCustomAttributes(typeof(DynamicXmlConfigRootAttribute), true)[0];
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

                var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rootAttribute.FileName);

                if (!File.Exists(fileName))
                {
                    Logs.Error("Load dynamic config file:{0} fail.", fileName);
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


    }
}
