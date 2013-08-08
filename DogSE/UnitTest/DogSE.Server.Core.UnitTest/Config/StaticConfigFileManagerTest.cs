using System.IO;
using DogSE.Library.Log;
using DogSE.Server.Core.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogSE.Server.Core.UnitTest.Config
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class StaticConfigFileManagerTest
    {
        /// <summary>
        /// 
        /// </summary>
        [TestInitialize]
        public void WriteConfigFile()
        {
            string str = @"<?xml version='1.0'?>
<root>
    <TestConfigClass1>
        <StrData>abc123中国</StrData>
        <IntData>123</IntData>
        <DoubleData>123.123</DoubleData>
        <LongData>220000000000</LongData>
        <BoolData>true</BoolData>
    </TestConfigClass1>

    <TestConfigClassByDefaultValue>
        <LongData>220000000000</LongData>
        <BoolData>true</BoolData>
    </TestConfigClassByDefaultValue>

    <OtherNameNode>
        <StrData2>abc中国</StrData2>
        <LongData>220000000000</LongData>
    </OtherNameNode>

    <ClassProperty>
        <Pt>
            <StrData>abc123中国</StrData>
            <IntData>123</IntData>
            <DoubleData>123.123</DoubleData>
            <LongData>220000000000</LongData>
            <BoolData>true</BoolData>
        </Pt>
    </ClassProperty>

</root>
";

            Logs.AddAppender(FileAppender.GetAppender("debug.log"));
            Logs.SetMessageLevel<FileAppender>(LogMessageType.MSG_DEBUG);

            File.WriteAllText("test.config", str.Replace("'", "\""));

        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void BaseLoadTest()
        {
            StaticConfigFileManager.LoadData();

            Assert.AreEqual("abc123中国", TestConfigClass1.StrData);
            Assert.AreEqual(123, TestConfigClass1.IntData);
            Assert.AreEqual(123.123, TestConfigClass1.DoubleData);
            Assert.AreEqual(220000000000, TestConfigClass1.LongData);
            Assert.AreEqual(true, TestConfigClass1.BoolData);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void DefaultValueTest()
        {
            StaticConfigFileManager.LoadData();

            Assert.AreEqual("abc", TestConfigClassByDefaultValue.StrData);
            Assert.AreEqual(10, TestConfigClassByDefaultValue.IntData);
            Assert.AreEqual(321.321, TestConfigClassByDefaultValue.DoubleData);
            Assert.AreEqual(220000000000, TestConfigClassByDefaultValue.LongData);
            Assert.AreEqual(true, TestConfigClassByDefaultValue.BoolData);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void OtherNameNode()
        {
            StaticConfigFileManager.LoadData();

            Assert.AreEqual("abc中国", OtherName.StrData);
            Assert.AreEqual(220000000000, OtherName.LongData);
            Assert.AreEqual(false, OtherName.BoolData);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void TestClassProperty()
        {
            StaticConfigFileManager.LoadData();

            Assert.IsNotNull(ClassProperty.Pt);

            Assert.AreEqual("abc123中国", ClassProperty.Pt.StrData);
            Assert.AreEqual(123, ClassProperty.Pt.IntData);
            Assert.AreEqual(123.123, ClassProperty.Pt.DoubleData);
            Assert.AreEqual(220000000000, ClassProperty.Pt.LongData);
            Assert.AreEqual(true, ClassProperty.Pt.BoolData);
        }
    }

    [StaticXmlConfigRoot("test.config")]
    static class TestConfigClass1
    {
        public static string StrData { get; set; }

        public static int IntData { get; set; }

        public static double DoubleData { get; set; }

        public static long LongData { get; set; }

        public static bool BoolData { get; set; }
    }


    [StaticXmlConfigRoot("test.config")]
    static class TestConfigClassByDefaultValue
    {
        private static int _intData = 10;
        private static double _doubleData = 321.321;
        private static string _strData = "abc";

        public static string StrData
        {
            get { return _strData; }
            set { _strData = value; }
        }

        public static int IntData
        {
            get { return _intData; }
            set { _intData = value; }
        }

        public static double DoubleData
        {
            get { return _doubleData; }
            set { _doubleData = value; }
        }

        public static long LongData { get; set; }

        public static bool BoolData { get; set; }
    }

    [StaticXmlConfigRoot("test.config", RootName="OtherNameNode")]
    static class OtherName
    {
        [XmlConfig(Name="StrData2")]
        public static string StrData { get; set; }

        public static int IntData { get; set; }

        public static double DoubleData { get; set; }

        public static long LongData { get; set; }

        public static bool BoolData { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 如果配置结点使用内部类作为配置节点
    /// 则父类需要为public类型
    /// </remarks>
    [StaticXmlConfigRoot("test.config")]
    public static class ClassProperty
    {
        /// <summary>
        /// 
        /// </summary>
        public static PropertyType Pt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public class PropertyType
        {
            /// <summary>
            /// 
            /// </summary>
            public string StrData { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public int IntData { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public double DoubleData { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public long LongData { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public bool BoolData { get; set; }
        }
    }


}
