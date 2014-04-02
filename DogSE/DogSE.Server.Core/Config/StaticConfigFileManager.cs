using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using DogSE.Library.Log;
using DogSE.Server.Core.Util;
using DogSE.Library.Serialize;

namespace DogSE.Server.Core.Config
{
    /// <summary>
    /// 静态文件配置文件管理
    /// </summary>
    public static class StaticConfigFileManager
    {
        /// <summary>
        /// 是否已经加载过文件
        /// </summary>
        private static bool isLoadData;


        /// <summary>
        /// 加载静态配置文件
        /// </summary>
        /// <param name="reLoad">
        /// 是否重新加载
        /// 如果reload = true，则不管之前是否加载过配置文件，都重新进行一次加载
        /// 否则会验证之前是否加载过，如果加载过则不再进行加载
        /// </param>
        public static void LoadData(bool reLoad = false)
        {
            if (!reLoad && isLoadData)
                return;

            var configTypes = AssemblyUtil.GetTypesByAttribute(typeof (StaticXmlConfigRootAttribute));
            foreach(var type in configTypes)
            {
                var rootAttribute = (StaticXmlConfigRootAttribute) type.GetCustomAttributes(typeof (StaticXmlConfigRootAttribute), true)[0];
                if (string.IsNullOrEmpty(rootAttribute.FileName))
                {
                    Logs.Error("Static config class:'{0}' not define file", type.Name);
                    continue;
                }
                var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rootAttribute.FileName);

                var xmlDoc = GetXml(fileName);
                if (xmlDoc == null)
                    continue;

                //  xml的根结点
                var rootName = string.IsNullOrEmpty(rootAttribute.RootName) ? type.Name : rootAttribute.RootName;
                Logs.Info("Load static config node:{0}", rootName);

                foreach(var pro in type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.SetProperty | BindingFlags.GetProperty ))
                {
                    //  获得读取xml结点名字
                    var attribute = pro.GetAttribute<XmlConfigAttribute>();
                    string nodeName = attribute == null ? pro.Name : attribute.Name;

                    if (pro.PropertyType == typeof(string))
                    {
                        var node = xmlDoc.SelectSingleNode(string.Format("root/{0}/{1}", rootName, nodeName));
                        if (node != null)
                        {
                            var value = node.InnerText.Trim();
                            pro.SetValue(null, value, null);
                        }
                    }
                    else if (pro.PropertyType == typeof(int))
                    {
                        var node = xmlDoc.SelectSingleNode(string.Format("root/{0}/{1}", rootName, nodeName));
                        if (node != null)
                        {
                            var str = node.InnerText.Trim();
                            int value;
                            if (int.TryParse(str, out value))
                                pro.SetValue(null, value, null);
                        }
                    }
                    else if (pro.PropertyType == typeof(long))
                    {
                        var node = xmlDoc.SelectSingleNode(string.Format("root/{0}/{1}", rootName, nodeName));
                        if (node != null)
                        {
                            var str = node.InnerText.Trim();
                            long value;
                            if (long.TryParse(str, out value))
                                pro.SetValue(null, value, null);
                        }
                    }
                    else if (pro.PropertyType == typeof(bool))
                    {
                        var node = xmlDoc.SelectSingleNode(string.Format("root/{0}/{1}", rootName, nodeName));
                        if (node != null)
                        {
                            var str = node.InnerText.Trim();
                            bool value;
                            if (bool.TryParse(str, out value))
                                pro.SetValue(null, value, null);
                        }
                    }
                    else if (pro.PropertyType == typeof(double))
                    {
                        var node = xmlDoc.SelectSingleNode(string.Format("root/{0}/{1}", rootName, nodeName));
                        if (node != null)
                        {
                            var str = node.InnerText.Trim();
                            double value;
                            if (double.TryParse(str, out value))
                                pro.SetValue(null, value, null);
                        }
                    }
                    else if (pro.PropertyType.IsArray)
                    {
                        //  获得数组对应的类型
                        var elementType = pro.PropertyType.GetElementType();

                        if (elementType == typeof(string))
                        {
                            var nodes = xmlDoc.SelectNodes(string.Format("root/{0}/{1}", rootName, nodeName));
                            if (nodes != null)
                            {
                                var values = new List<string>();
                                foreach (XmlNode node in nodes)
                                {
                                    values.Add(node.InnerText.Trim());
                                }

                                pro.SetValue(null, values.ToArray(), null);
                            }
                        }
                        else if (elementType == typeof(int))
                        {
                            var nodes = xmlDoc.SelectNodes(string.Format("root/{0}/{1}", rootName, nodeName));
                            if (nodes != null)
                            {
                                var values = new List<int>();

                                foreach (XmlNode node in nodes)
                                {
                                    var str = node.InnerText.Trim();
                                    int value;
                                    if (int.TryParse(str, out value))
                                        values.Add(value);
                                }

                                pro.SetValue(null, values.ToArray(), null);
                            }
                        }
                        else if (elementType == typeof(long))
                        {
                            var nodes = xmlDoc.SelectNodes(string.Format("root/{0}/{1}", rootName, nodeName));
                            if (nodes != null)
                            {
                                var values = new List<long>();

                                foreach (XmlNode node in nodes)
                                {
                                    var str = node.InnerText.Trim();
                                    long value;
                                    if (long.TryParse(str, out value))
                                        values.Add(value);
                                }

                                pro.SetValue(null, values.ToArray(), null);
                            }
                        }
                        else if (elementType == typeof(bool))
                        {
                            var nodes = xmlDoc.SelectNodes(string.Format("root/{0}/{1}", rootName, nodeName));
                            if (nodes != null)
                            {
                                var values = new List<bool>();

                                foreach (XmlNode node in nodes)
                                {
                                    var str = node.InnerText.Trim();
                                    bool value;
                                    if (bool.TryParse(str, out value))
                                        values.Add(value);
                                }

                                pro.SetValue(null, values.ToArray(), null);
                            }
                        }
                        else if (elementType == typeof(double))
                        {
                            var nodes = xmlDoc.SelectNodes(string.Format("root/{0}/{1}", rootName, nodeName));
                            if (nodes != null)
                            {
                                var values = new List<double>();

                                foreach (XmlNode node in nodes)
                                {
                                    var str = node.InnerText.Trim();
                                    double value;
                                    if (double.TryParse(str, out value))
                                        values.Add(value);
                                }

                                pro.SetValue(null, values.ToArray(), null);
                            }
                        }
                        else if (elementType.IsClass && !elementType.IsValueType)
                        {
                            var nodes = xmlDoc.SelectNodes(string.Format("root/{0}/{1}", rootName, nodeName));

                            if (nodes == null)
                                continue;

                            var xml = new StringBuilder("<?xml version=\"1.0\"?>");
                            xml.AppendFormat("<ArrayOf{0}>", elementType.Name);
                            foreach (XmlNode node in nodes)
                                xml.Append(node.OuterXml);

                            xml.Replace(string.Format("<{0}>", pro.Name), string.Format("<{0}>", elementType.Name));
                            xml.Replace(string.Format("</{0}>", pro.Name), string.Format("</{0}>", elementType.Name));
                            xml.AppendFormat("</ArrayOf{0}>", elementType.Name);

                            var obj = xml.ToString().XmlDeserialize(pro.PropertyType);
                            if (obj != null)
                                pro.SetValue(null, obj, null);
                        }
                    }
                    else
                    {
                        //  是对象，需要重新加载
                        if (pro.PropertyType.IsClass)
                        {
                            var node = xmlDoc.SelectSingleNode(string.Format("root/{0}/{1}", rootName, nodeName));

                            if (node == null)
                                continue;

                            var xml = new StringBuilder("<?xml version=\"1.0\"?>");
                            xml.Append(node.OuterXml);
                            xml.Replace(string.Format("<{0}>", pro.Name), string.Format("<{0}>", pro.PropertyType.Name));
                            xml.Replace(string.Format("</{0}>", pro.Name), string.Format("</{0}>", pro.PropertyType.Name));

                            var obj = xml.ToString().XmlDeserialize(pro.PropertyType);
                            if (obj != null)
                                pro.SetValue(null, obj, null);
                        }
                    }
                }
            }

            isLoadData = true;
        }

        private static Dictionary<string, XmlDocument> xmlDocumentMap = new Dictionary<string, XmlDocument>();
        static XmlDocument GetXml(string fileName)
        {
            if (!File.Exists(fileName))
            {
                Logs.Error("Config file \"{0}\" can't exists.", fileName);
                return null;
            }

            XmlDocument ret;
            if (xmlDocumentMap.TryGetValue(fileName, out ret))
                return ret;

            try
            {
                var xml = File.ReadAllText(fileName);
                ret = new XmlDocument();
                ret.LoadXml(xml);
                xmlDocumentMap[fileName] = ret;
            }
            catch (Exception ex)
            {
                Logs.Error("Statc xml config({0}) load fail.", fileName, ex);
                return null;
            }

            return ret;
        }
    }
}
