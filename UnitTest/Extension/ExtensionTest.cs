using Accounting.Sqlserver;
using System;
using System.Xml;
using Xunit;
using System.IO;
using Extensions;

namespace UnitTest
{
    public class ExtensionTest
    {
        [Fact]
        public void calc_percent_null()
        {
            //Arrange
            Extensions.LoginStatus log = new LoginStatus();

            //Act 
            //Assert
            Assert.Null(log.Percent());
        }

        [Fact]
        public void calc_percent_value()
        {
            //Arrange
            Extensions.LoginStatus log = new LoginStatus();
            log.BasicAcct = new BasicAcct() { TotalIn = 10, TotalOut = 10 };
            log.MonthlyTraffic = 100;
            //Act 
            var per = log.Percent();
            var result = per == "20%";
            //Assert
            Assert.True(result);
        }

        [Fact]
        public void calc_percent_value_complex()
        {
            //Arrange
            Extensions.LoginStatus log = new LoginStatus();
            log.BasicAcct = new BasicAcct() { TotalIn = 1001210151405, TotalOut = 4654645 };
            log.MonthlyTraffic = 1201210151405;
            //Act 
            var per = log.Percent();
            var result = per == "83%";
            //Assert
            Assert.True(result);
        }
    }
}
