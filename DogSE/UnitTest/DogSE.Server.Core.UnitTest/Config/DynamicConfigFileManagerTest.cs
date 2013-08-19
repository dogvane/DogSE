using System;
using System.IO;
using DogSE.Library.Log;
using DogSE.Server.Core.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogSE.Library.Serialize;

namespace DogSE.Server.Core.UnitTest.Config
{
    /// <summary>
    /// 动态配置文件的测试
    /// </summary>
    [TestClass]
    public class DynamicConfigFileManagerTest
    {
        /// <summary>
        /// 
        /// </summary>
        [TestInitialize]
        public void WriteConfigFile()
        {
            string str = @"<?xml version='1.0'?>
<ArrayOfDynamicConfigFileManagerTest_c1>
  <DynamicConfigFileManagerTest_c1>
<StrData>abc123中国</StrData>
    <IntData>123</IntData>
    <DoubleData>123.123</DoubleData>
    <LongData>220000000000</LongData>
    <BoolData>true</BoolData>
  </DynamicConfigFileManagerTest_c1>

  <DynamicConfigFileManagerTest_c1>
<StrData>abc123中国2</StrData>
    <IntData>1232</IntData>
    <DoubleData>123.1232</DoubleData>
    <LongData>120000000000</LongData>
    <BoolData>false</BoolData>
  </DynamicConfigFileManagerTest_c1>

</ArrayOfDynamicConfigFileManagerTest_c1>
";

            Logs.AddAppender(FileAppender.GetAppender("debug.log"));
            Logs.AddAppender(new DebugAppender());
            Logs.AddAppender(new ConsoleAppender());

            Logs.SetMessageLevel<FileAppender>(LogMessageType.MSG_DEBUG);

            File.WriteAllText("d_1.config", str.Replace("'", "\""));

        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void BaseLoadTest()
        {
            DynamicConfigFileManager.Load();

            var arr = DynamicConfigFileManager.GetConfigData<DynamicConfigFileManagerTest_c1>("d1test");

            Assert.IsNotNull(arr);

            Assert.AreEqual(2, arr.Length, "数据数值长度应该是2");

            var c1 = arr[0];
            Assert.AreEqual("abc123中国", c1.StrData);
            Assert.AreEqual(123, c1.IntData);
            Assert.AreEqual(123.123, c1.DoubleData);
            Assert.AreEqual(220000000000, c1.LongData);
            Assert.AreEqual(true, c1.BoolData);

            var c2 = arr[1];

            Assert.AreEqual("abc123中国2", c2.StrData);
            Assert.AreEqual(1232, c2.IntData);
            Assert.AreEqual(123.1232, c2.DoubleData);
            Assert.AreEqual(120000000000, c2.LongData);
            Assert.AreEqual(false, c2.BoolData);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [DynamicXmlConfigRoot("d_1.config", "d1test")]
    public class DynamicConfigFileManagerTest_c1
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
