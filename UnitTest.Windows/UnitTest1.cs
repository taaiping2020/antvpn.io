using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using Accounting.RouterReporter;

namespace UnitTest.Windows
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //"\\\\bosxixihome\\network interface(microsoft hyper-v network adapter _3)\\bytes received/sec"
            var path = "\\\\bosxixihome\\network interface(microsoft hyper-v network adapter _3)\\bytes received/sec";
            var name = CounterName.GetNetworkInterfaceName(path);
            Assert.IsTrue(name == "(microsoft hyper-v network adapter _3)");
        }
    }
}
