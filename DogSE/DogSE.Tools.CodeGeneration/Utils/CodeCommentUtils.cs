using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using DogSE.Library.Log;

namespace DogSE.Tools.CodeGeneration.Utils
{
    /// <summary>
    /// 代码注解处理类
    /// </summary>
    static class CodeCommentUtils
    {
        /// <summary>
        /// 把vs生成的xml文件转换为必要的文档
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <returns></returns>
        public static List<FunItem> LoadXmlDocument(string xmlFile)
        {
            List<FunItem> ret = new List<FunItem>();

            if (!File.Exists(xmlFile))
            {
                Logs.Error("not find xml file", xmlFile);
                return ret;
            }

            var xmlData = File.ReadAllText(xmlFile);

            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xmlData);

            foreach (XmlNode node in dom.SelectNodes("//member"))
            {
                FunItem item = new FunItem();

                item.Name = node.Attributes["name"].Value;
                var nav = node.CreateNavigator();
                item.Summary = nav.SelectSingleNode("summary").Value.Trim();
                
                foreach (XPathNavigator pn in nav.Select("param"))
                {
                    
                    ParamItem pi = new ParamItem();
                    pi.Name = pn.GetAttribute("name", "");
                    pi.Value = pn.Value.Trim();

                    item.Params.Add(pi);
                }

                ret.Add(item);
            }

            return ret;
        }

        /// <summary>
        /// 获得某个参数的注解
        /// </summary>
        /// <param name="item"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetParamSummary(this FunItem item, string name)
        {
            if (item == null)
                return string.Empty;

            var p = item.Params.FirstOrDefault(o => o.Name == name);
            if (p != null)
                return p.Value;
            return string.Empty;
        }


        /// <summary>
        /// 获得枚举数据
        /// </summary>
        /// <param name="items"></param>
        /// <param name="type"></param>
        /// <param name="enumName"></param>
        /// <returns></returns>
        public static string GetEnumDescription(this FunItem[] items, Type type, string enumName)
        {
            var name = "F:" + type.FullName + "." + enumName;
            var item = items.FirstOrDefault(o => o.Name.IndexOf(name) == 0);
            if (item == null)
                return string.Empty;
            return item.Summary;
        }

        public static void test()
        {
            var ret = LoadXmlDocument(@"D:\workspace\wakuang\trunk\program\server\Adventure.Server.Interface\bin\Debug\Adventure.Server.Interface.XML");
            Console.WriteLine(ret.Count);
        }
    }

    public class FunItem
    {
        public FunItem()
        {
            Params = new List<ParamItem>();
        }

        /// <summary>
        /// 函数的名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 方法的注解
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ParamItem> Params { get; private set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ParamItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Value { get; set; }
    }
}
