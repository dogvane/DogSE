using DogSE.Library.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogSE.Server.Core.UnitTest.Util
{
    /// <summary>
    /// OpCode的测试
    /// </summary>
    [TestClass]
    public class OpCodeNameUtilTest
    {
        /// <summary>
        /// 测试获得消息码对应的名字
        /// </summary>
        [TestMethod]
        public void TestGetName()
        {
            //OpCodeName.InitGameOpCodeName();

            Assert.AreEqual("Code1", OpCodeName.GetOpCodeName(1));
            Assert.AreEqual("Code2", OpCodeName.GetOpCodeName(2));
            Assert.AreEqual("Unknow", OpCodeName.GetOpCodeName(0));
        }

        /// <summary>
        /// 测试获得消息码对应的名字
        /// </summary>
        [TestMethod]
        public void TestGetDescription()
        {
            //OpCodeName.InitGameOpCodeName();

            Assert.AreEqual("编码1", OpCodeName.GetOpCodeDescription(1));
            Assert.AreEqual("编码2", OpCodeName.GetOpCodeDescription(2));
            Assert.AreEqual("Unknow", OpCodeName.GetOpCodeDescription(0));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OpCodeName:OpCodeNameUtil<OpCode>
    {
        
    }

    /// <summary>
    /// 测试用消息码
    /// </summary>
    public enum OpCode
    {
        /// <summary>
        /// 编码1
        /// </summary>
        Code1= 1,

        /// <summary>
        /// 编码2
        /// </summary>
        Code2 = 2
    }
}
