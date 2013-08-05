using System;
using DogSE.Library.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogSE.Server.Core.UnitTest.Util
{
    /// <summary>
    /// des加密字符串测试
    /// </summary>
    [TestClass]
    public class StringCryptoUnitTest
    {
        /// <summary>
        /// 基本测试
        /// </summary>
        [TestMethod]
        public void BaseTest()
        {
            var cy = new StringCrypto(Convert.ToBase64String("123".ToArrayInByte()), Convert.ToBase64String("321".ToArrayInByte()));
            var 加密后字符串 = cy.Encrypt("abc");
            Console.WriteLine(加密后字符串);

            var 解密字符串 = cy.Decrypt(加密后字符串);
            Assert.AreEqual("abc", 解密字符串);
        }
    }
}
