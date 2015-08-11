using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DogSE.Library.Log;
using System.Xml;


namespace DogSE.Library.Util
{
    /// <summary>
    /// 服务器端的翻译语言处理模块
    /// </summary>
    public static class Lang
    {
        const string LangXMLFileName = "Lang.xml";

        static Lang()
        {
            //  默认从当前目录下的Lang.xml获得翻译文件，如果找不到，就等外部代码进行更新
            string watchFolder = ".";
            if (File.Exists(LangXMLFileName))
            {
                InitLangFile(LangXMLFileName);
            }
            //else if (HttpContext.Current != null)
            //{
            //    //  表示这是一个web项目
            //    var filePath = HttpContext.Current.Server.MapPath(".");
            //    if (Directory.Exists(filePath))
            //    {
            //        var file = Path.Combine(filePath, LangXMLFileName);
            //        if (File.Exists(file))
            //            InitLangFile(file);
            //        watchFolder = filePath;
            //    }
            //}

            var dirInfo = new DirectoryInfo(watchFolder);
            if (dirInfo.Exists)
            {
                if (s_fileWatch != null)
                {
                    s_fileWatch = new FileSystemWatcher(dirInfo.FullName, "*.xml");
                    s_fileWatch.Changed += OnWatchFileChanged;
                    s_fileWatch.NotifyFilter = NotifyFilters.LastWrite;
                    s_fileWatch.EnableRaisingEvents = true;
                }
            }
        }

        private static readonly FileSystemWatcher s_fileWatch;

        /// <summary>
        /// 初始化制定的翻译文件
        /// </summary>
        /// <param name="xmlFile"></param>
        public static void InitLangFile(string xmlFile)
        {
            try
            {
                var dict = new Dictionary<string, string>();
                using (var stream = new StreamReader(xmlFile, Encoding.UTF8))
                {
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(stream.ReadToEnd());
                    stream.Close();

                    if (xml.DocumentElement == null)
                    {
                        Logs.Error("Lang message xml fail load is null.");
                        return;
                    }
                    var xmlNodeList = xml.DocumentElement.SelectNodes("//M");
                    if (xmlNodeList != null)
                        foreach (XmlNode element in xmlNodeList)
                        {
                            if (element.Attributes != null)
                            {
                                var attribute = element.Attributes["k"];
                                if (attribute != null)
                                {
                                    var key = attribute.Value;
                                    var value = element.InnerText;

                                    dict[key] = value;
                                }
                            }
                        }

                    s_dict = dict;
                    Logs.Info("Init lang success. count={0}", dict.Count);
                }
            }
            catch (Exception ex)
            {
                Logs.Error("init lang xml fail.", ex);
            }
        }

        static void OnWatchFileChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                lock (s_fileWatch)
                {
                    if (e.Name.ToLower() == "lang.xml")
                    {
                        //if (HttpContext.Current != null)
                        //{
                        //    //  表示这是一个web项目
                        //    var filePath = HttpContext.Current.Server.MapPath(".");
                        //    if (Directory.Exists(filePath))
                        //    {
                        //        var file = Path.Combine(filePath, e.Name);
                        //        if (File.Exists(file))
                        //            InitLangFile(file);
                        //    }
                        //}
                        //else
                        {
                            InitLangFile(e.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Logs.Error("xml file change relaod fail.", ex);
            }

        }


        private static Dictionary<string, string> s_dict = new Dictionary<string, string>();

        /// <summary>
        /// 字典里包含翻译的数量
        /// </summary>
        public static int DictCount
        {
            get { return s_dict.Count; }
        }

        /// <summary>
        /// 初始化（临时用）
        /// </summary>
        /// <param name="source"></param>
        public static void Init(Dictionary<string, string> source)
        {
            if (source == null)
                return;

            s_dict = source;
        }

        /// <summary>
        /// 重新加载文件
        /// </summary>
        public static void ReloadFile()
        {
            if (File.Exists(LangXMLFileName))
            {
                InitLangFile(LangXMLFileName);
            }
            else
            {
                Logs.Error("not find xml file. {0}", LangXMLFileName);
            }

            //else if (HttpContext.Current != null)
            //{
            //    //  表示这是一个web项目
            //    var filePath = HttpContext.Current.Server.MapPath(".");
            //    if (Directory.Exists(filePath))
            //    {
            //        var file = Path.Combine(filePath, LangXMLFileName);
            //        if (File.Exists(file))
            //            InitLangFile(file);
            //    }
            //}
        }

        /// <summary>
        /// 将一个语言翻译为目标语言
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Trans(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;

            string ret;
            if (s_dict.TryGetValue(source, out ret)) 
                return ret;

            return source;
        }

        /// <summary>
        /// 将一个语言翻译为目标语言（带格式化输出）
        /// </summary>
        /// <param name="source"></param>
        /// <param name="objParams"></param>
        /// <returns></returns>
        public static string TransFormat(string source, params object[] objParams)
        {
            return string.Format(Trans(source), objParams);
        }
    }
}
