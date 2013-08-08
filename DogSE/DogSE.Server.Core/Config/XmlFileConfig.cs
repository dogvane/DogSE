using System;


namespace DogSE.Server.Core.Config
{
    /// <summary>
    /// 配置文件根节点项，只能给类的根节点用
    /// </summary>
    /// <remarks>
    /// FileName：是配置文件的名字，配置文件可以由多个静态配置类共享
    /// RootName: 当前静态配置类属性在配置文件的根节点名称，如果未配置，则为对象名
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public class StaticXmlConfigRootAttribute : Attribute
    {
        /// <summary>
        /// 静态配置文件根节点项
        /// </summary>
        /// <param name="fileName">配置文件的文件名</param>
        public StaticXmlConfigRootAttribute(string fileName)
        {
            FileName = fileName;
        }

        /// <summary>
        /// 配置文件的文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// xml节点的根节点名称，如果未赋值，则根节点为对象的名字
        /// </summary>
        public string RootName { get; set; }
    }

    /// <summary>
    /// 动态数据的配置节点项
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DynamicXmlConfigRootAttribute : Attribute
    {
        /// <summary>
        /// 动态数据配置文件根节点项
        /// </summary>
        /// <param name="fileName">配置文件的文件名</param>
        /// <param name="componentName">配置名字</param>
        public DynamicXmlConfigRootAttribute(string fileName, string componentName)
        {
            FileName = fileName;
            ComponentName = componentName;
        }

        /// <summary>
        /// 配置文件的文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 在管理数据里读取的配置名字
        /// </summary>
        public string ComponentName { get; set; }
    }


    /// <summary>
    /// 配置文件节点项
    /// </summary>
    public class XmlConfigAttribute:Attribute
    {
        /// <summary>
        /// 节点名字
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 忽略的配置节点项
    /// </summary>
    public class IngortXmlConfigAttribute : Attribute
    {
    }
}
