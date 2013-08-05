using System.IO;
using DogSE.Library.Log;
using DogSE.Server.Core.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogSE.Server.Core.UnitTest.Config
{
    /// <summary>
    /// 带数组的静态文件测试
    /// </summary>
    [TestClass]
    public class StaticConfigFileManagerArrayTest
    {
        /// <summary>
        /// 
        /// </summary>
        [TestInitialize]
        public void WriteConfigFile()
        {
            string str = @"<?xml version='1.0'?>
<root>
    <BaseArray>
        <StrData>abc123中国</StrData>
        <StrData>abc123中国2</StrData>
        <IntData>1</IntData>
        <IntData>2</IntData>
        <IntData>3</IntData>
        <LongData>220000000000</LongData>
        <BoolData>true</BoolData>
        <BoolData>false</BoolData>
    </BaseArray>

    <ClassArrayProperty>
        <Pt>
            <StrData>abc123中国</StrData>
            <IntData>123</IntData>
            <DoubleData>123.123</DoubleData>
            <LongData>220000000000</LongData>
            <BoolData>true</BoolData>
        </Pt>
        <Pt>
            <StrData>123中国</StrData>
            <IntData>321</IntData>
            <DoubleData>321.321</DoubleData>
            <LongData>110000000000</LongData>
        </Pt>
    </ClassArrayProperty>


</root>
";

            Logs.AddAppender(FileAppender.GetAppender("debug.log"));
            Logs.SetMessageLevel<FileAppender>(LogMessageType.MSG_DEBUG);

            File.WriteAllText("testArray.config", str.Replace("'", "\""));

        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void BaseLoadArrayTest()
        {
            StaticConfigFileManager.LoadData();

            Assert.AreEqual(2, BaseArray.StrData.Length);
            Assert.AreEqual("abc123中国", BaseArray.StrData[0]);
            Assert.AreEqual("abc123中国2", BaseArray.StrData[1]);

            Assert.AreEqual(3, BaseArray.IntData.Length);
            Assert.AreEqual(1, BaseArray.IntData[0]);
            Assert.AreEqual(2, BaseArray.IntData[1]);
            Assert.AreEqual(3, BaseArray.IntData[2]);

            Assert.AreEqual(0, BaseArray.DoubleData.Length);

            Assert.AreEqual(1, BaseArray.LongData.Length);
            Assert.AreEqual(220000000000, BaseArray.LongData[0]);

            Assert.AreEqual(2, BaseArray.BoolData.Length);
            Assert.AreEqual(true, BaseArray.BoolData[0]);
            Assert.AreEqual(false, BaseArray.BoolData[1]);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void TestArrayClassProperty()
        {
            StaticConfigFileManager.LoadData();

            Assert.IsNotNull(ClassArrayProperty.Pt);

            var len = ClassArrayProperty.Pt.Length;
            Assert.AreEqual(2, len);

            var pro = ClassArrayProperty.Pt[0];

            Assert.AreEqual("abc123中国", pro.StrData);
            Assert.AreEqual(123, pro.IntData);
            Assert.AreEqual(123.123, pro.DoubleData);
            Assert.AreEqual(220000000000, pro.LongData);
            Assert.AreEqual(true, pro.BoolData);

            pro = ClassArrayProperty.Pt[1];

            Assert.AreEqual("123中国", pro.StrData);
            Assert.AreEqual(321, pro.IntData);
            Assert.AreEqual(321.321, pro.DoubleData);
            Assert.AreEqual(110000000000, pro.LongData);
            Assert.AreEqual(false, pro.BoolData);

        }
    }

    [XmlConfigRoot("testArray.config")]
    static class BaseArray
    {
        public static string[] StrData { get; set; }

        public static int[] IntData { get; set; }

        public static double[] DoubleData { get; set; }

        public static long[] LongData { get; set; }

        public static bool[] BoolData { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 如果配置结点使用内部类作为配置节点
    /// 则父类需要为public类型
    /// </remarks>
    [XmlConfigRoot("testArray.config")]
    public static class ClassArrayProperty
    {
        /// <summary>
        /// 
        /// </summary>
        public static PropertyType[] Pt { get; set; }

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
