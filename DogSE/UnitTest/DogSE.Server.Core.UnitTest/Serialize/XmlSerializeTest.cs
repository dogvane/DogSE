using Microsoft.VisualStudio.TestTools.UnitTesting;
using DogSE.Library.Serialize;

namespace DogSE.Server.Core.UnitTest.Serialize
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class XmlSerializeTest
    {

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void XmlSerialize()
        {
            Abc o = new Abc();
            var str = o.XmlSerialize();
            o = str.XmlDeserialize<Abc>();
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void XmlSerializeArray()
        {
            var o = new Abc[2];
            o[0] = new Abc();
            o[1] = new Abc();

            var str = o.XmlSerialize();
            o = str.XmlDeserialize<Abc[]>();
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void XmlDeserialize()
        {
            var xml = "<?xml version=\"1.0\"?><Abc><A>3434</A><B>1</B><C>true</C></Abc>";
            var o = xml.XmlDeserialize<Abc>();
            Assert.AreEqual("3434", o.A);
            Assert.AreEqual(1, o.B);
            Assert.AreEqual(true, o.C);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Abc
    {
        /// <summary>
        /// 
        /// </summary>
        public string A { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int B { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool C { get; set; }

    }
}
