using System;
using DogSE.Library.Log;
using DogSE.Server.Core.LogicModule;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Task;
using DogSE.Server.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogSE.Server.Core.Protocol;

namespace DogSE.Server.Core.UnitTest.Protocol
{
    /// <summary>
    /// 创建数据包读取的单元测试
    /// </summary>
    [TestClass]
    public class CraetePacketReaderTest
    {
        /// <summary>
        /// 初始化
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            Logs.AddAppender(FileAppender.GetAppender("debug.log"));
            Logs.SetMessageLevel<FileAppender>(LogMessageType.MSG_DEBUG);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void BaseTest()
        {
            
            var manager = new PacketHandlersBase();
            var pr = new PacketReaderModule();
            new RegisterNetMethod(manager).Register(new ILogicModule[] {pr});

            Assert.IsNotNull(manager.Handlers[1], "方法没注册到消息管理器里");

            manager.Handlers[1].OnReceive(new NetState(), PacketReader.AcquireContent(DogBuffer.GetFromPool32K()));
            Assert.IsTrue(pr.IsTouchOnReadTest);

            Assert.IsNotNull(manager.Handlers[2], "包读取方法没注册到消息管理器");
            manager.Handlers[2].OnReceive(new NetState(), PacketReader.AcquireContent(DogBuffer.GetFromPool32K()));
            Assert.IsTrue(pr.IsTouchPackageReader);

            Assert.IsNotNull(manager.Handlers[3], "简单读取方法没注册到消息管理器");
            manager.Handlers[3].OnReceive(new NetState(), PacketReader.AcquireContent(DogBuffer.GetFromPool32K()));
            Assert.IsTrue(pr.IsTouchSimpleMethod);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class PacketReaderModule:ILogicModule
    {

        #region ILogicModule 成员

        /// <summary>
        /// 
        /// </summary>
        public string ModuleId
        {
            get { return "PacketReaderModule"; }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initializationing()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initializationed()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReLoadTemplate()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void Release()
        {
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public bool IsTouchOnReadTest;

        /// <summary>
        /// 测试读取方法
        /// </summary>
        /// <param name="netstate"></param>
        /// <param name="reader"></param>
        [NetMethod(1, NetMethodType.PacketReader, false)]
        public void OnReadTest(NetState netstate, PacketReader reader)
        {
            IsTouchOnReadTest = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsTouchPackageReader;

        /// <summary>
        /// 包数据自动读取
        /// </summary>
        /// <param name="netstate"></param>
        /// <param name="pakcage"></param>
        [NetMethod(2, NetMethodType.ProtocolStruct, false)]
        public void OnReadTest2(NetState netstate, TestPackageReader pakcage)
        {
            if (pakcage.IsTouchRead)
                IsTouchPackageReader = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsTouchSimpleMethod;

        /// <summary>
        /// 简单的读取方法
        /// </summary>
        /// <param name="netstate"></param>
        /// <param name="i"></param>
        /// <param name="s"></param>
        /// <param name="d"></param>
        /// <param name="b"></param>
        [NetMethod(3, NetMethodType.SimpleMethod, false)]
        public void OnReadTest3(NetState netstate, int i, string s, double d, bool b)
        {
            IsTouchSimpleMethod = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TestPackageReader:IPacketReader
    {
        /// <summary>
        /// 是否触发读取
        /// </summary>
        public bool IsTouchRead;

        #region IPacketReader 成员

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public void Read(PacketReader reader)
        {
            IsTouchRead = true;
        }

        #endregion
    }
}
