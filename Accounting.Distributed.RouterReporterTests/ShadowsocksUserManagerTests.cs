using Microsoft.VisualStudio.TestTools.UnitTesting;
using Accounting.RouterReporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.RouterReporter.Tests
{
    [TestClass()]
    public class ShadowsocksUserManagerTests
    {
        [TestMethod()]
        public void ShadowsocksTest()
        {
            var inmemorylogins = new List<Login>()
            {
                new Login(){ LoginName="bosxixi", Password="xboxone", Port=20000 },
                new Login(){ LoginName="cyahto", Password="xboxone", Port=20000 },
                new Login(){ LoginName="fifa", Password="xboxone", Port=20000 }
            };
            var lastlogins = new List<Login>()
            {
                new Login(){ LoginName="bosxixi", Password="xxxxxx", Port=20000 },
                new Login(){ LoginName="cyahto", Password="xboxone", Port=20000 },
                //new Login(){ LoginName="fifa", Password="xboxone", Port=20000 }
            };

            var usertoremove = inmemorylogins.Except(lastlogins, new LoginComparer());

            Assert.IsTrue(usertoremove.Any(c => c.LoginName == "bosxixi"));
        }

        [TestMethod()]
        public void ShadowsocksAddTest()
        {
            var inmemorylogins = new List<Login>()
            {
                new Login(){ LoginName="cyahto", Password="xboxone", Port=20000 }
            };
            var lastlogins = new List<Login>()
            {
                new Login(){ LoginName="bosxixi", Password="xxxxxx", Port=20000 },
                new Login(){ LoginName="cyahto", Password="xboxone", Port=20000 },
                new Login(){ LoginName="fifa", Password="xboxone", Port=20000 }
            };

            var usertoadd = lastlogins.Except(inmemorylogins, new LoginComparer());

            Assert.IsTrue(usertoadd.Any(c => c.LoginName == "bosxixi"));
        }
    }
}