using System;
using DogSE.Library.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogSE.Server.Net.UnitTest
{
    [TestClass]
    public class DogBufferTest
    {
        [TestInitialize]
        public void Init()
        {
            new DogBuffer();    //  创建的目的是让内存池能够初始化
        }

        [TestMethod]
        public void TestSize()
        {
            var _4Kbuf = DogBuffer.GetFromPool4K();
            Assert.IsNotNull(_4Kbuf);
            Assert.AreEqual(_4Kbuf.Bytes.Length, 1024*4);
            Assert.AreEqual(_4Kbuf.Length, 0);

            var _32kbuf = DogBuffer.GetFromPool32K();
            Assert.IsNotNull(_32kbuf);
            Assert.AreEqual(_32kbuf.Bytes.Length, 1024*32);
            Assert.AreEqual(_32kbuf.Length, 0);
        }

        [TestMethod]
        public void TestRelease4K()
        {
            var firstFree = Get4KbufferFree();

            var buf = DogBuffer.GetFromPool4K();
            var 分配后的数量 = Get4KbufferFree();

            Assert.AreEqual(firstFree - 1, 分配后的数量);

            buf.Release();
            var 释放缓冲区后的数量 = Get4KbufferFree();

            Assert.AreEqual(firstFree, 释放缓冲区后的数量);

            //  测试重复释放会不会有问题
            buf.Release();
            释放缓冲区后的数量 = Get4KbufferFree();
            Assert.AreEqual(firstFree, 释放缓冲区后的数量);
        }

        [TestMethod]
        public void TestRelease32K()
        {
            var firstFree = Get32KbufferFree();

            var buf = DogBuffer.GetFromPool32K();
            var 分配后的数量 = Get32KbufferFree();

            Assert.AreEqual(firstFree - 1, 分配后的数量);

            buf.Release();
            var 释放缓冲区后的数量 = Get32KbufferFree();

            Assert.AreEqual(firstFree, 释放缓冲区后的数量);

            //  测试重复释放会不会有问题
            buf.Release();
            释放缓冲区后的数量 = Get32KbufferFree();
            Assert.AreEqual(firstFree, 释放缓冲区后的数量);
        }
        private static long  Get4KbufferFree()
        {
            var poolInfos = ObjectPoolStateInfo.GetPoolInfos("DogBuffer");
            Assert.IsTrue(poolInfos.Length > 0);
            return  poolInfos[0].FreeCount;
        }

        private static long Get32KbufferFree()
        {
            var poolInfos = ObjectPoolStateInfo.GetPoolInfos("DogBuffer32K");
            Assert.IsTrue(poolInfos.Length > 0);
            return poolInfos[0].FreeCount;
        }

        /// <summary>
        /// 测试缓冲区的扩容
        /// </summary>
        [TestMethod]
        public void TestUpdateCapacity()
        {
            var buf = DogBuffer.GetFromPool4K();

            Assert.AreEqual(buf.Bytes.Length, 1024 * 4);
            Assert.AreEqual(buf.Length, 0);

            buf.Bytes[0] = 1;
            buf.Bytes[1] = 2;
            buf.Bytes[1024*4 - 1] = 255;

            var b = buf.Bytes;

            buf.UpdateCapacity();
            //  扩容后对象引用会不一样了
            Assert.AreNotSame(buf.Bytes, b);
            Assert.AreEqual(buf.Bytes.Length, 1024*8);

            //  验证数据扩容后是否正确
            Assert.AreEqual(buf.Bytes[0], 1);
            Assert.AreEqual(buf.Bytes[1], 2);
            Assert.AreEqual(buf.Bytes[1024 * 4 - 1], 255);
        }

        [TestMethod]
        public void Test重复引用对象()
        {
            var firstFree = Get4KbufferFree();

            var buf = DogBuffer.GetFromPool4K();
            var 分配后的数量 = Get4KbufferFree();

            Assert.AreEqual(firstFree - 1, 分配后的数量);
            buf.Use();

            buf.Release();  // 只释放一个引用
            var 释放缓冲区后的数量 = Get4KbufferFree();
            Assert.AreEqual(分配后的数量, 释放缓冲区后的数量);

            buf.Release();
            释放缓冲区后的数量 = Get4KbufferFree();
            Assert.AreEqual(firstFree, 释放缓冲区后的数量);

            buf.Release();
            释放缓冲区后的数量 = Get4KbufferFree();
            Assert.AreEqual(firstFree, 释放缓冲区后的数量);

        }
    }
}
